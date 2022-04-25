import axios from 'axios'
import Vue from 'vue';

const ACCESS_EXPIRED_IN = 60 * 10;              // 10 min

let baseURL;
if (process.env.NODE_ENV == 'development') {
  baseURL = 'http://localhost:8000';
} else if (process.env.NODE_ENV == 'production') {
  baseURL = 'https://netlab.kyonggi.ac.kr:8000';
} else {
  baseURL = 'https://netlab.kyonggi.ac.kr:8000';
}

const axiosInstance = axios.create({
  baseURL,
});

// 특정 API 요청은 Authorization 헤더(토큰)를 필요로 하며,
// 해당 토큰에 대한 유효성을 검증해야한다.
// axios interceptor를 사용하여 특정 API에 대해서 토큰의 유효성을 검증하고 헤더를 삽입한다.

axios.defaults.requiredAuth = false;
axiosInstance.interceptors.request.use(
  async (config) => {
    // 인증 필요한 API
    if (config.requireAuth) {
      return await callTokenCheck(config);
    } else {
      return config;
    }
  },
  (err) => {
    return Promise.reject(err);
  }
);

const callTokenCheck = (config) => {
  return new Promise((resolve, reject) => {
    // 쿠키에서 access 토큰을 가져와서
    const token = Vue.$cookies.get('accessToken');
    // access 토큰의 유효성을 검사하고
    axios.get(`${baseURL}/auth/check`, { headers: { Authorization: token, } })
      .then(response => {
        // authorization에 토큰을 삽입한다.
        if (response.data.isOK) {
          config.headers.Authorization = token;
          resolve(config);
        } else {
          resolve(config);
        }
      })
      .catch(err => { 
        // access 토큰 만료 > 재발급
        if (err.response.status === 401) {
          // 쿠키에서 refresh 토큰 가져와서
          const refresh = Vue.$cookies.get('refreshToken');
          // refresh 토큰의 유효성을 검사하고
          axios.get(`${baseURL}/auth/refresh`, { headers: { Authorization: refresh, } })
            .then(response => {
              if (response.data.isOK) {
                console.log("REFRESH");
                // access 토큰을 쿠키에 삽입하고
                Vue.$cookies.set('accessToken', response.data.newAccessToken, ACCESS_EXPIRED_IN);
                // authorization에 토큰을 삽입한다.
                config.headers.Authorization = response.data.newAccessToken;
                resolve(config);
              } else {
                resolve(config);
              }
            })
            .catch(err => { reject(err) });
        } else {
          reject(err);
        }
      });
  });
}

export default axiosInstance;
export {
  axiosInstance,
  baseURL,
}