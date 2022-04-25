import Vue from 'vue';
// Vue app
import App from './App.vue';
// Vue router
import router from './router/index';
// Vuex
import store from './store/store';
// Vue Bootstrap import
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';
// Cookie Usage
import VueCookies from 'vue-cookies';

Vue.use(BootstrapVue);
Vue.use(IconsPlugin);
Vue.use(VueCookies);

Vue.config.productionTip = true;

new Vue({
  render: h => h(App),
  store,
  router,
}).$mount('#app')