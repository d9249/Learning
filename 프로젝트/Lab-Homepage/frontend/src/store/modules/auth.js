/**
 * 인증 관련 store
 */

import Vue from 'vue';
import { singin, getUser } from '../../../api/index';

const ACCESS_EXPIRED_IN = 60 * 10;              // 10 min
const ACCESS_EXPIRED_IN_HOUR = 60 * 60;         // 1 hour
const REFRESH_EXPIRED_IN = 60 * 60 * 24 * 14;   // 2 weeks

const state = {
  isSignedIn: false,
  user: null,
};

const getters = {
  GET_NICKNAME(state) {
    if (!state.user) {
      return '';
    } else {
      return state.user.nickname
    }
  },
  GET_ROLE(state) {
    if (!state.user) {
      return 100;
    } else {
      return state.user.role;
    }
  }
};

const mutations = {
  SIGN_IN(state, payload) {
    state.user = payload;
    state.isSignedIn = true;
  },
  SIGN_OUT(state) {
    state.user = null;
    state.isSignedIn = false;
  },
  INIT_USER(state, payload) {
    state.user = payload.user;
    state.isSignedIn = payload.isSignedIn;
  }
}

const actions = {
  SIGN_IN(context, payload) {
    return new Promise((resolve, reject) => {
      singin(payload)
        .then(response => {
          if (payload.autoSignin) {
            Vue.$cookies.set('accessToken', response.data.accessToken, ACCESS_EXPIRED_IN);
            Vue.$cookies.set('refreshToken', response.data.refreshToken, REFRESH_EXPIRED_IN); 
          } else {  
            Vue.$cookies.set('accessToken', response.data.accessToken, ACCESS_EXPIRED_IN_HOUR);
          }
          
          context.commit('SIGN_IN', response.data.user);
          resolve({ isOK: true });
        })
        .catch(err => {
          if (err.response.status == 401) {
            resolve({ isOK: false });
          } else {
            reject(err);
          }          
        });
    });
  },
  SIGN_OUT(context) {
    Vue.$cookies.remove('accessToken');
    Vue.$cookies.remove('refreshToken');
    context.commit('SIGN_OUT');
  },
  INIT_USER(context) {
    // 토큰이 있어야 호출
    if (Vue.$cookies.isKey("accessToken") || Vue.$cookies.isKey("refreshToken")) {
      getUser()
      .then((response) => {
        if (response.data.isOK) {
          const payload = {
            user: { nickname: response.data.nickname, role: response.data.role },
            isSignedIn: true,
          }
          context.commit('INIT_USER', payload);
        }
      })
      .catch((err) => {
        if (err.response.status == 400) {
          Vue.$cookies.remove('accessToken');
          Vue.$cookies.remove('refreshToken');

        } else if (err.response.status != 401)
          console.error(err);
      });
    }
  },
  CLEAR(context) {
    Vue.$cookies.remove('accessToken');
    Vue.$cookies.remove('refreshToken');
    context.commit('SIGN_OUT');
  }
};

export default {
  state,
  getters,
  mutations,
  actions,
}