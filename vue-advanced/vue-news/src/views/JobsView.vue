<template>
  <div>
    <div v-for="item in this.$store.state.jobs">{{ item.title }}</div>
  </div>
</template>

<script>
export default {
  created() {
    this.$store.dispatch("FETCH_JOBS");
  },
};
</script>

<style></style>
