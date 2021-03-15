<template>
  <div class="content">
    <span>Edit Items:</span>
    <button v-on:click="saveDrafts()">
      <span>
        <font-awesome-icon icon="save" />Save Drafts
      </span>
    </button>
    <button v-on:click="confirmDrafts()">
      <font-awesome-icon icon="file-export" />Commit Changes to Database
    </button>

    <div class="body-container">
      <div class="draft-box" v-for="(item, index) in drafts" :key="index">
        <span>
          <font-awesome-icon icon="minus-circle" @click="discardItem(index)" style="color: red" />
          Product Number: {{ item.productNumber }}
        </span>
        <label for="description">Description</label>
        <input type="text" name="description" v-model="item.productDescription" />
        <label for="sellable">Cross Reference</label>
        <input type="text" name="crossReference" v-model="item.crossReference" />
        <label for="manufacturerId">Manufacturer</label>
        <input type="text" name="manufacturerId" v-model="item.manufacturerId" />
        <label for="inventoryStatus">Inventory Status</label>
        <input type="text" name="inventoryStatus" v-model="item.inventoryStatus" />
        <label for="alternativeProducts">Alternative Products</label>
        <input type="text" name="alternativeProducts" v-model="item.alternativeProducts" />
        <div>
          <input type="checkbox" class="checkbox" name="sellable" v-model="item.isSellable" />
          <label for="sellable">Approved</label>
        </div>
        <div>
          <input
            type="checkbox"
            class="checkbox"
            name="drugControlled"
            v-model="item.isDrugControlled"
          />
          <label for="drugControlled">Drug Controlled</label>
        </div>
        <div>
          <input type="checkbox" class="checkbox" name="returnable" v-model="item.isNonReturnable" />
          <label for="returnable">Returnable</label>
        </div>
        <div>
          <input type="checkbox" class="checkbox" name="refrigerated" v-model="item.isRefrigerated" />
          <label for="refrigerated">Refrigerated</label>
        </div>
        <div>
          <input type="checkbox" class="checkbox" name="regulated" v-model="item.isRegulated" />
          <label for="regulated">Regulated</label>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
const checkboxOptions = [
  "isSellable",
  "isDrugControlled",
  "isNonReturnable",
  "isRefrigerated",
  "isRegulated"
];
export default {
  name: "drafts",
  components: { FontAwesomeIcon },
  data() {
    return {
      title: "Edit Drafts",
      storedDrafts: [],
      drafts: [],
      loadedProductNumbers: [],
      API_URL: "http://localhost:64458/api"
    };
  },
  created() {
    this.selectedItems = this.$route.params.selectedItems;
  },
  methods: {
    saveDrafts() {
      const draftString = JSON.stringify(this.drafts);
      window.localStorage.setItem("productNumber", draftString);
    },
    confirmDrafts() {
      if (this.drafts != null) {
        this.drafts
          .map(draftProduct => {
            checkboxOptions.forEach(option => {
              if (typeof draftProduct[option] === "boolean") {
                draftProduct[option] = Number(draftProduct[option]);
              }
            });
            return draftProduct;
          })
          .forEach(product => {
            fetch(`${this.API_URL}/item/${product.productNumber}`, {
              method: "PUT",
              headers: {
                "Content-Type": "application/json; charset=utf-8"
              },
              body: JSON.stringify(product)
            }).then(response => response.JSON());
          });
        this.drafts = [];
        window.localStorage.removeItem("productNumber");
      }
    },
    discardItem(index) {
      this.drafts.splice(index, 1);
    }
  },
  mounted() {
    try {
      JSON.parse(window.localStorage.getItem("productNumber")).forEach(item => {
        this.drafts.push(item);
        this.loadedProductNumbers.push(item.productNumber);
      });
    } catch {
      console.log("No existing saved entries for edit");
    }

    // This is not loading selectedItems if drafts is empty because of filter working wrong.
    if (this.selectedItems != null) {
      let checkedForDupes = this.selectedItems.filter(
        item => !this.loadedProductNumbers.includes(item)
      );
      checkedForDupes.forEach(id => {
        {
          fetch(`${this.API_URL}/item/${id}`)
            .then(response => {
              return response.json();
            })
            .then(item => {
              this.drafts.push(item);
            });
        }
      });
    }
  }
};
</script>

<style scoped>
.body-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-evenly;
}
.draft-box {
  display: flex;
  display: block;
  padding: 1rem;
  margin: 1rem;
  width: 350px;
  border: 2px solid #f0ab00;
  text-align: center;
}

@media only screen and (max-width: 400px) {
  .draft-box {
    min-width: 20vw;
  }
}

input {
  width: 100%;
}
input.checkbox {
  display: inline;
}
span {
  margin-bottom: 5px;
}
</style>
