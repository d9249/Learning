import Vue from 'vue'
import App from './App.vue'

// var App = {
//   template: '<div>app</div>'
// }

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
}).$mount('#app')

// new Vue ({
//   el: '#app',
//   render: h => h(App),
//   conponents: {
//     'app': App
//   }
//   같은 역활을 한다.
// })