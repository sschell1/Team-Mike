<template>
  <div>
    <label for="search">
      <font-awesome-icon icon="search" /> Search Filter:
    </label>
    <input
      type="text"
      id="search"
      v-model=searchString
      v-on:keyup="filterList"
      placeholder="Enter search filter here"
    />
  </div>
</template>

<script>
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
export default {
  data() {
    return {
      searchString: ""
    };
  },
  mounted(){
    this.searchString = "";
  },
  methods: {
    filterList() {
      this.$emit("filter-list", this.searchString);
    }
  },
  components: {
    FontAwesomeIcon
  }
};
</script>
<style scoped>
label {
  margin-right: 20px;
}
input {
  width: 30%;
}
</style>