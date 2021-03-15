import Vue from "vue";
import Router from "vue-router";
import auth from "./auth";
import Login from "./views/Login.vue";
import ProductsList from "./views/ProductsList.vue";
import Home from "./views/Home.vue";
import Edit from "./views/Edit.vue";
import UserManagement from "./views/UserManagement.vue";


Vue.use(Router);

/**
 * The Vue Router is used to "direct" the browser to render a specific view component
 * inside of App.vue depending on the URL.
 *
 * It also is used to detect whether or not a route requires the user to have first authenticated.
 * If the user has not yet authenticated (and needs to) they are redirected to /login
 * If they have (or don't need to) they're allowed to go about their way.
 */

const router = new Router({
  mode: "history",
  base: process.env.BASE_URL,
  routes: [
    {
      path: "/home",
      name: "home",
      component: Home,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/products",
      name: "products-list",
      component: ProductsList,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/",
      name: "login",
      component: Login,
      meta: {
        requiresAuth: false
      }
    },
    {
      path: "/edit",
      name: "edit",
      component: Edit,
      meta: {
        requiresAuth: true
      }
    },
    {
      path: "/user-management",
      name: "user-management",
      API_URL: "http://localhost:64458/api",
      component: UserManagement,
      meta: {
        requiresAuth: true,
        requiresAdmin: true
      }
    }
  ]
});

router.beforeEach((to, from, next) => {
  // Determine if the route requires Authentication
  //const {authorize} = to.meta;
  const requiresAuth = to.matched.some(x => x.meta.requiresAuth);
  //const authorize = to.matched.some( x=> x.to.meta.requiresAdmin);
  const user = auth.getUser();

  // If it does and they are not logged in, send the user to "/"
  if (requiresAuth && !user) {
    next("/");
  } else if (to.meta.requiresAdmin) {
    console.log("checked for authorize");
    var jwt = require("jwt-simple");
    let token = auth.getToken();
    if (token) {
      var decoded = jwt.decode(token, "TechElevatorCodingBootamp");
      if (decoded.rol == "Admin") {
        next();
      } else {
        next("/home");
      }
    }
  } else {
    next();
    // Else let them go to their next destination   
  }
});

export default router;
