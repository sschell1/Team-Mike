using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductApproval.DAL;
using ProductApproval.Password_and_Authentication_Helpers;
using static ProductApproval.Password_and_Authentication_Helpers.HashProvider;

namespace ProductApproval
{
    /// <summary>
    /// The asp.net api startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Creates a startup class instance.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// Configures all of the services used by the application.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Configures Swagger to look at the XmlComments above our methods
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Sample API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Add CORS policy allowing any origin
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Enables automatic authentication token.
            // The token is expected to be included as a bearer authentication token.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }) 
                .AddJwtBearer("JwtBearer", jwtOptions =>
                {
                    // The rules for token validation
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,                 // issuer not required
                        ValidateAudience = false,               // audience not required
                        ValidateLifetime = true,                // token must not have expired
                        ValidateIssuerSigningKey = true,   
                        ClockSkew = TimeSpan.FromMinutes(5),         // token signature must match so as not to be tampered with
                        NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier,   // allows us to use sub for username
                        RoleClaimType = "rol",                  // allows us to put the role in rol
                        IssuerSigningKey = new SymmetricSecurityKey(    // each token is signed with a private key so as to ensure its validity
                            Encoding.UTF8.GetBytes(Configuration["JwtSecret"]))
                    };
                });

            // Dependency Injection configuration
            services.AddSingleton<ITokenGenerator>(tk => new JwtGenerator(Configuration["JwtSecret"]));
            services.AddSingleton<IPasswordHasher>(ph => new HashProvider());

            services.AddTransient<IProductDAO>(m => new ProductSqlDAO(Configuration.GetConnectionString("Default")));
            services.AddTransient<IUsersDAO>(m => new UsersSqlDAO(Configuration.GetConnectionString("Default")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Configure automatic model state validation
            // This prevents us from having to manually check model state in each action.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    // Get all of the errors in a list
                    var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToList();
                    // Create a result object
                    var result = new
                    {
                        Message = "Validation errors",
                        Errors = errors
                    };
                    return new BadRequestObjectResult(result);
                };
            });

        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Includes middleware configuration for the HTTP Request Pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API v1");
            });

            app.UseCors("CorsPolicy");

            // Enables the middleware to check the incoming request headers.
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
