<template>
  <div class="navbar">
    <router-link :to="{ path: '/' }" class="logo-link">
      <img class="logo-img" src="/assets/logo.jpg" />
    </router-link>

    <div class="navbar-list">
      <router-link to="/contact" class="navbar-a" exact-active-class="color-mint">Contact</router-link>
      <router-link to="/gallery" class="navbar-a" exact-active-class="color-mint">Gallery</router-link>
      <router-link to="/lecture" class="navbar-a" exact-active-class="color-mint">Lecture</router-link>
      <router-link to="/publication" class="navbar-a" exact-active-class="color-mint">Publication</router-link>
      <router-link to="/intro" class="navbar-a" exact-active-class="color-mint">Introduction</router-link>
    </div>

    <div id="navbar-mobile">
      <b-button
        variant="link"
        v-b-toggle.sidebar-right
        class="navbar-mobile-icon px-0"
        ><b-icon icon="list"></b-icon>
      </b-button>
      <b-sidebar id="sidebar-right" width="200px" right backdrop shadow>
        <div class="px-3 py-1">
          <div class="navbar-mobile-ele"><a href="/intro">Introduction</a></div>
          <div class="navbar-mobile-ele"><a href="/publication">Publication</a></div>
          <div class="navbar-mobile-ele"><a href="/lecture">Lecture</a></div>
          <div class="navbar-mobile-ele"><a href="/gallery">Gallery</a></div>
          <div class="navbar-mobile-ele"><a href="/contact">Contact</a></div>
        </div>
      </b-sidebar>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style scoped>
.navbar {
  font-weight: bold;
  /* font-size: large; */
  display: flex;
  padding: 0.5em 0;
}
.logo-link {
  width: fit-content;
  flex: 1;
  padding: 0.5em 0;
}
.navbar-list {
  flex: 4;
  padding: 0.5em 0;
}
.navbar-a {
  float: right;
  display: block;
  color: black;
  padding: 0 0.5em;
  text-decoration: none;
  font-size: 1.2em;
}
/* .navbar-a:hover:not(.active) {
  color: #808080;
} */
.color-mint {
  color: #17a2b8;
}
.logo-img {
  width: 90%;
  transform: translateY(10%);
}
/* 모바일 NavBar */
#navbar-mobile {
  display: none;
  flex: 4;
  padding: 0;
}
.navbar-mobile-btn {
  float: right;
  color: #2c2c2c;
}
.btn:focus {
  outline: none !important;
  box-shadow: none !important;
}
.navbar-mobile-icon {
  font-size: 2em;
  color: #808080;
  float: right;
}
.navbar-mobile-ele {
  padding: 0.5em 0;
  font-size: 1.2rem;
}
.navbar-mobile-ele a {
  text-decoration: none;
  color: #2c2c2c;
}
/* 반응형 */
@media (max-width: 992px) {
  .navbar-a {
    font-size: 1em;
  }
}
@media (max-width: 768px) {
  .navbar-list {
    display: none;
  }
  #navbar-mobile {
    display: block;
  }
  .logo-img {
    width: 130px;
    transform: translateY(10%);
  }
  .logo-link {
    padding: 0;
  }
  .navbar {
    padding: 0;
  }
}
</style>
