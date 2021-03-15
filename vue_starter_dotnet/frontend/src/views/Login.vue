<template>
  <div id="login" class="text-center content">
    <h1>Login</h1>
    <form class="form-signin" @submit.prevent="login">
      <div
        class="alert alert-danger"
        role="alert"
        v-if="invalidCredentials"
      >Invalid username and password!</div>
      <div>
        <label for="username" >Username</label>
        <input
          type="text"
          id="username"
          class="form-control"
          placeholder="Username"
          v-model="user.username"
          required
          autofocus
        />
      </div>
      <div>
        <label for="password" >Password</label>
        <input
          type="password"
          id="password"
          class="form-control"
          placeholder="Password"
          v-model="user.password"
          required
        />
      </div>
      <button type="submit">Sign in</button>
    </form>
  </div>

  <!-- <div class="content">
      <h1>Login</h1>
      <form>
        <div class="form-element">
          <label for="username">Username:</label>
          <input id="username" type="text" />
        </div>

        <div class="form-element">
          <label for="password">Password:</label>
          <input id="password" type="text" />
        </div>
      </form>
      <router-link :to="{name: 'home'}">
        <button>Sign In</button>
      </router-link>
  </div>-->
</template>

<script>
import auth from "../auth";

export default {
  name: "login",
  title: "Login",
  data() {
    return {
      user: {
        username: "",
        password: ""
      },
      invalidCredentials: false
    };
  },
  methods: {
    login() {
      fetch(`${process.env.VUE_APP_REMOTE_API}/login`, {
        method: "POST",
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json"
        },
        body: JSON.stringify(this.user)
      })
        .then(response => {
          if (response.ok) {
            return response.text();
          } else {
            this.invalidCredentials = true;
          }
        })
        .then(token => {
          if (token != undefined) {
            if (token.includes('"')) {
              token = token.replace(/"/g, "");
            }
            auth.saveToken(token);
            this.$router.push("/home");
          }
        })
        .catch(err => console.error(err));
    }
  }
};
</script>

<style scoped>
label {
  padding-right: 10px;
}
</style>