import Vuex from 'vuex'
import Vue from 'vue'

Vue.use(Vuex);
import mutations from './mutatons.js'
import actions from './actions.js'

export const store = new Vuex.Store({
    state: {
        news: [],
        jobs: [],
        ask: [],
        user: {},
        item: {}
    },
    getters: {
        fetchedAsk(state) {
            return state.ask
        },
        fetchedItem(state) {
            return state.item
        }
    },
    mutations,
    actions
});