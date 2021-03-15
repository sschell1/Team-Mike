<template>
  <div class="navbar">
    <router-link
      :to="{name: 'products-list'}"
      exact
      class="products"
      tag="button"
      v-if="$route.name!=='products-list'"
    >
      <font-awesome-icon icon="list" />Product List
    </router-link>

    <router-link :to="{name: 'edit'}" class="edit" tag="button" v-if="$route.name!=='edit'">
      <font-awesome-icon icon="edit" />Edit Products
    </router-link>

    <router-link
      :to="{name: 'user-management'}"
      class="user-management"
      tag="button"
      v-if="$route.name!=='user-management' && isAdmin"
    >
      <font-awesome-icon icon="user" />User Management
    </router-link>

    <button @click="logout" class="logout">Log Out</button>
    <hr />
  </div>
</template>

<script>
var jwt = require("jwt-simple");

import auth from "../auth";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
export default {
  name: "Navbar",
  components: {
    FontAwesomeIcon
  },
  data() {
    return {
      isAdmin: ""
    };
  },
  methods: {
    logout() {
      auth.destroyToken();
      this.$router.push("/");
    }
  },
  mounted() {
    let token = auth.getToken();
    if (token) {
      var decoded = jwt.decode(token, "TechElevatorCodingBootamp");
      this.isAdmin = decoded.rol == "Admin";
      console.log(decoded.rol);
    }
  }
};
</script>
<style scoped>
.logout {
  float: right;
}
</style>