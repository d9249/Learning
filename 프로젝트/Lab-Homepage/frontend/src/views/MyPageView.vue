<template>
  <div>
    <my-page :token="this.$route.query.t ? this.$route.query.t : '' "></my-page>
  </div>
</template>

<script>
import MyPage from '../components/MyPage';

export default {
  name: 'MyPageView',
  components: {
    MyPage
  },
}
</script>

<style scoped>

</style>