<template>
  <div class="content">
    <div class="confirm"  v-if="listType!=''">
      <SearchBar id="search-bar" v-on:filter-list="handleSearch" />
      <button v-on:click="confirmChanges()">
        <font-awesome-icon icon="clipboard-check" />
        <span v-if="listType=='Unapproved Products List'"> Approve Selected</span>
        <span v-else> Remove Selected</span>
        </button>
      <router-link :to="{name: 'edit', params: {selectedItems: this.selectedItems}}" ><button id="edit">
        <font-awesome-icon icon="edit" />
        Edit Selected Items</button></router-link>
      
    </div>
    <table v-if="listType!=''">
      <caption>
        <h2>{{listType}}</h2>
      </caption>
      <thead>
        <th>Select</th>
        <th>Number</th>
        <th>Description</th>
        <th>Default UOM</th>
        <th>Cross Reference</th>
        <th>Manufacturer ID</th>
        <th>Inventory Status</th>
        <th>Alternative Products</th>

      </thead>
      <hr />
      <tbody>
        <tr
          v-for="item in filteredList"
          v-bind:key="item.productNumber"
          :class="item.productNumber"
        >
          <td>
            <input
              type="checkbox"
              @click="markSelected($event)"
              v-model="selectedItems"
              :value="item.productNumber"
              :id="item.productNumber"
            />
          </td>
          <td>{{item.productNumber}}</td>
          <td>{{item.productDescription}}</td>  
          <td>{{item.defaultUOM}}</td>
          <td>{{item.crossReference}}</td>
          <td>{{item.manufacturerId}}</td>
          <td>{{item.inventoryStatus}}</td>
          <td>{{item.alternativeProducts}}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import {FontAwesomeIcon} from "@fortawesome/vue-fontawesome";
import SearchBar from "@/components/Searchbar";
export default {
  name: "products-list",
  components: {
    FontAwesomeIcon,
    SearchBar,
    
  },
  props: {
    listType: String,
    data: Array,
  },
  data() {
    return {
      selectedItems: [],
      search: "",
      API_URL: "http://localhost:64458/api"
    };
  },
   methods: {
    handleSearch(query) {
      console.log("filter-list", query);
      this.search = query;
    },
    resetStatus(productNumber) {
      const checkBox = document.getElementById(productNumber);
      checkBox.checked = false;
      checkBox.parentNode.classList.remove("selected");
      const arrIndex = this.data.findIndex(item => item.productNumber == productNumber);
      this.data.splice(arrIndex, 1);
    },
    markSelected(event) {
      event.stopPropagation();
      const { target } = event;
      const tableRow = target.closest("tr");
      if (!target.checked) {
        tableRow.classList.remove("selected");
      } else {
        tableRow.classList.add("selected");
      }
    },

    confirmChanges() {
      this.selectedItems.forEach(item => {
        this.resetStatus(item);
        fetch(`${this.API_URL}/item/${item}/Sellable`, {
          //localhost:#####/api/item/[Object object]
          method: "PUT",
          headers: {
            "Content-Type": "application/json; charset=utf-8"
          },
          body: JSON.stringify(item)
        }).then(response => {
          let rText = response.text().toString();
          if (rText == 0) {
            console.log(`FAIL: ${rText} failed to change isSellable value`);
          } else {
            console.log(`SUCCESS: ${rText} changed isSellable value`);
          }
        });
        this.selectedItems = [];
      });
    },

    editItems() {
      this.$emit('edit', this.selectedItems)
    }
  },
  computed: {
    filteredList() {
      const filter = new RegExp(this.search, "i");
      return this.data.filter(
        product => (
          product.productNumber.match(filter) ||
         product.productDescription.match(filter) ||
          product.manufacturerId.match(filter) ||
          product.crossReference.match(filter) ||
          product.alternativeProducts.match(filter) ||
          product.defaultUOM.match(filter) ||
          product.inventoryStatus.match(filter)
          ) 
          
        );
    }
  }
};
</script>

<style>
.selected {
  background-color: #f0ab00;
  color: black;
  padding: 0;
  margin: 0;
}
caption {
  background-color: white;
  color: black;
}
table {
  border-style: hidden;
  margin: 5px;
  border-collapse: collapse;
}
/* #search-bar {
      position: absolute;
    right: 20px;    
} */

</style>