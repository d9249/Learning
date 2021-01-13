import Vue from 'vue'
import Vuex from 'vuex'
// import * as getters from './getters'
// import * as mutations from './mutations'
import todoApp from './modules/todoApp'

Vue.use(Vuex);

export const store = new Vuex.Store({
  modules: {
    todoApp
  }
});
