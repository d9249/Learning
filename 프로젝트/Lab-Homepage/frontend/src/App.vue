<template>
  <div id="app">
    <top-header></top-header>
    <b-container id="content-wrapper-main" fluid="sm">
      <nav-bar></nav-bar>
      <router-view></router-view>
    </b-container>
    <main-footer></main-footer>
  </div>
</template>

<script>
import TopHeader from "./components/TopHeader";
import MainFooter from "./components/MainFooter";
import NavBar from "./components/NavBar";

export default {
  name: "App",
  components: {
    TopHeader,
    MainFooter,
    NavBar,
  },
};
</script>

<style>
html,
body {
  margin: 0;
  padding: 0;
}
#app {
  position: relative;
}
#content-wrapper-main {
  padding-bottom: 100px;
}
/* 반응형 */
@media (max-width: 768px) {
  #content-wrapper-main {
    max-width: 100%;
    padding-bottom: 0px;
  }
}
</style>
