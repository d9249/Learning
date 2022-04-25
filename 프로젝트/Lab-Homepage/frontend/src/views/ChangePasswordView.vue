<template>
  <div>
    <change-password :token="this.$route.query.t" :userEmail="this.$route.query.e"></change-password>
  </div>
</template>

<script>
import ChangePassword from '../components/ChangePassword.vue'

export default {
  name: 'ChangePasswordView',
  components: {
    ChangePassword,
  },
}
</script>

<style scoped>

</style>