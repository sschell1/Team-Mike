<template>
  <div class="content">
    <Navbar />
    <Drafts :selectedItems="selectedItems" />
  </div>
</template>

<script>
import Drafts from "@/components/Drafts.vue";
import Navbar from "@/components/Navbar"
export default {
  name: "edit",
  components: {
    Drafts,
    Navbar
  },
  props: {
    selectedItems: []
  },
  data() {
    return {
      
    };
  }
};
</script>