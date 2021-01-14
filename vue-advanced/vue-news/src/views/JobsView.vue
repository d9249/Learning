<template>
  <div>
    <div v-for="item in jobs">{{ item.title }}</div>
  </div>
</template>

<script>
import { fetchJobsList } from "../api/index.js";

export default {
  data() {
    return {
      jobs: [],
    };
  },
  created() {
    fetchJobsList()
      .then((response) => (this.jobs = response.data))
      .catch((error) => console.log(error));
  },
};
</script>

<style></style>
