<template>
  <div>
    <div v-for="item in fetchedAsk">{{ item.title }}</div>
  </div>
</template>

<script>
import { mapState, mapGetters } from "vuex";
export default {
  computed: {
    ...mapGetters({
      fetchedAsk: "fetchedAsk",
    }),
    // ...mapState({
    //   ask: (state) => state.ask,
    // }),
    // ask() {
    //   return this.$store.state.askl
    // }
  },
  created() {
    this.$store.dispatch("FETCH_ASK");
  },
};
</script>

<style></style>
