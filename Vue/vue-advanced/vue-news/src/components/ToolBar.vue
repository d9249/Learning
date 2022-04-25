<template>
  <div class="header">
    <router-link to="/news">News</router-link> |
    <router-link to="/ask">Asks</router-link> |
    <router-link to="/jobs">Jobs</router-link>
  </div>
</template>

<script>
export default {};
</script>

<style scopes>
.header {
  color: white;
  background-color: #43b883;
  display: flex;
  padding: 8px 8px 8px 8px;
}
.header a:active {
  color: #35949e;
}
.header a {
  color: white;
}
</style>
