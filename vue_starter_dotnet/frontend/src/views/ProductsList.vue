<template>
  <div class="content">
    <Navbar />
    <span>Select List Type:</span>
    <button v-on:click="getUnapprovedList()">Unapproved</button>
    <button v-on:click="getApprovedList()">Approved</button>
    <ProductsList :data="data" :listType="currentList" />
  </div>
  
</template>

<script>
import ProductsList from "@/components/ProductsList";
import Navbar from "@/components/Navbar";

export default {
  name: "products-list",
  components: {
    ProductsList,
    Navbar
  },
  data() {
    return {
      data: [],
      currentList: "",
      selectedItems: [],
      API_URL: "http://localhost:64458/api"
    };
  },
  methods: {
    edit() {
      this.$router.push({
        name: "edit",
        params: { selectedItems: this.selectedItems }
      });
    },
    

    getUnapprovedList() {
      fetch(`${this.API_URL}/products/0`)
        .then(response => {
          return response.json();
        })
        .then(products => {
          this.data = products;
        })
        .catch(err => console.log(err));
      this.currentList = "Unapproved Products List";
    },

    getApprovedList() {
      fetch(`${this.API_URL}/products/1`)
        .then(response => {
          return response.json();
        })
        .then(products => {
          this.data = products;
        })
        .catch(err => console.log(err));
      this.currentList = "Approved Products List";
    }
  }
};
</script>

<style>
button {
  margin: 1% 1% 1% 1%;
  border-radius: 25px;
  padding: 5px 10px;
  font-weight: bold;
}
button:hover {
  background-color: #f0ab00;
  color: black;
}
span {
  padding-top: 15px;
}
.options {
  display: flex;
  padding-top: 10px;
  background-color: #004165;
  color: #f0ab00
}
.products-list {
  padding-left: 10px;
 
}
.confirm {
  background-color: #004165;
}
caption {  
  color: black;
  width: 95vw; 
}


</style>