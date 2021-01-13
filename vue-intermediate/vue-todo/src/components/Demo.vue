<template>
  <div id="root">
    <p>{{ originalPrice }}</p>
    <p>{{ doublePrice }}</p>
    <p>{{ triplePrice }}</p>
  </div>
</template>

<script>
import { mapGetters } from "vuex";

export default {
  computed: {
    ...mapGetters({
      orignalPrice,
      doublePrice,
      triplePrice
    })
    // orignalPrice() {
    //   return this.$store.getters.originalPrice;
    // },
    // doublePrice() {
    //   return this.$store.getters.doublePrice;
    // },
    // triplePrice() {
    //   return this.$stroe.getters.triplePrice;
  }
};
</script>

<style></style>
