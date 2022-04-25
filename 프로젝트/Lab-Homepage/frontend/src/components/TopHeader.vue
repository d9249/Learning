<template>
  <b-navbar type="dark" class="header-blue py-1 px-3">
  
    <b-navbar-nav class="ml-auto" v-if="this.auth.isSignedIn">
      <!-- Navbar dropdowns -->
      <b-nav-item-dropdown right>
        <template #button-content>
          <span class="white"><b>{{ GET_NICKNAME + ' ' }}</b></span>
        </template>

        <b-dropdown-item href="/mypage-before">
          <b-icon icon="person" class="mr-2"></b-icon>
          Account
        </b-dropdown-item>
        <b-dropdown-divider></b-dropdown-divider>
        <b-dropdown-item @click="signout">
          <b-icon icon="power" class="mr-2"></b-icon>
          Sign out
        </b-dropdown-item>
      </b-nav-item-dropdown>
    </b-navbar-nav>

    <b-navbar-nav class="ml-auto" v-else>
      <b-nav-item class="white" href="/signin">
        <template>
          <span class="white">Sign in</span>
        </template>
      </b-nav-item>
      <b-nav-item href="/signup" class="aaa">
        <template>
          <span class="white">Sign up</span>
        </template>
      </b-nav-item>
    </b-navbar-nav>

  </b-navbar>

</template>

<script>
import { mapActions, mapGetters, mapState } from "vuex";

export default {
  name: "TopHeader",
  computed: {
    ...mapState(["auth"]),
    ...mapGetters(["GET_NICKNAME", "GET_ROLE"]),
  },
  created() {
    this.INIT_USER();
  },
  methods: {
    ...mapActions(["SIGN_OUT", "CHECK_TOKEN", "INIT_USER"]),
    signout() {
      this.SIGN_OUT();
      alert("로그아웃 되었습니다.");

      if (this.$router.currentRoute.path !== "/") {
        this.$router.push("/");
      } else {
        this.$router.go();
      }
    },
  },
};
</script>

<style scoped>
.header-blue {
  background-color: #3860e4;
}
.skyblue {
  color: skyblue;
}
.no-outline:focus {
  outline: 0;
}
.white {
  color: white;
}
.mr-10 {
  margin-right: 10%;
}
</style>
