<template>
  <div class="signin-bg">
    <div class="signin-wrap">
      <div class="signin-container">
        <span class="signin-header">Smart IoT Lab</span>
        <form v-on:submit.prevent="submitForm">
          <div>
            <input
              id="userId"
              type="text"
              placeholder="아이디(이메일)"
              v-model="userId"
              class="signin-input-box"
            />
          </div>
          <div>
            <input
              id="userPw"
              type="password"
              placeholder="비밀번호"
              v-model="userPw"
              class="signin-input-box"
            />
          </div>
          <button type="submit" class="signin-button">SIGN IN</button>
        </form>
        <div class="signin-footer">
          <b-form-checkbox
            v-model="autoSignin"
            value="accept"
            unchecked-value="not_accept"
            class="autosignin-checkbox"
          >
            자동 로그인
          </b-form-checkbox>

          <router-link to="/signup">회원가입</router-link>
          <br />
          <router-link to="/forgotten">비밀번호 변경</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";

export default {
  name: "Signin",
  data() {
    return {
      userId: "",
      userPw: "",
      autoSignin: "accept",
    };
  },
  computed: {
    ...mapGetters(["GET_USER"]),
  },
  methods: {
    ...mapActions(["SIGN_IN"]),
    submitForm() {
      const autoSignin = this.autoSignin == "accept" ? true : false;
      this.SIGN_IN({ userId: this.userId, userPw: this.userPw, autoSignin })
        .then((response) => {
          if (response.isOK) {
            this.$router.push("/");
          } else {
            alert("로그인 정보가 올바르지 않습니다.");
          }
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");
        });
    },
  },
};
</script>

<style scoped>
.signin-bg {
  background: #e2e8fd;
  margin-top: 30px;
  margin-bottom: 30px;
  padding: 3em;
  text-align: -webkit-center;
}
.signin-wrap {
  min-height: 70vh;
  display: flex;
  background: #ffffff;
  border-radius: 10px;
  padding: 3em 0.5em;
  align-items: center;
  justify-content: center;
  max-width: fit-content;
  box-shadow: 0.5em 0.5em 0.4em rgb(165, 165, 165);
}
.signin-container {
  padding: 0.5em 2em;
  width: 30em;
  min-width: -webkit-fill-available;
}
.signin-header {
  font-size: 2em;
  color: #303030;
  line-height: 1.2;
  padding-bottom: 2em;
  display: block;
}
.signin-input-box {
  margin: 0 0.2em 1em;
  font-size: 1.2em;
  color: #505050;
  line-height: 1.2;
  padding: 0.8em;
  width: -webkit-fill-available;
}
.signin-button {
  background: -webkit-gradient(
    linear,
    left top,
    right top,
    from(#21d4fd),
    to(#b721ff)
  );
  color: #ffffff;
  border-radius: 10px;
  border: 0;
  height: 2em;
  font-size: 1.3em;
  width: 100%;
  cursor: pointer;
  outline: none;
}
.signin-footer {
  width: 100%;
  padding-top: 1em;
  line-height: 1.5;
  text-align: right;
}
.signin-footer a {
  text-decoration: none;
  color: #5a5a5a;
}
.autosignin-checkbox {
  float: left;
  color: #5a5a5a;
}
@media (max-width: 768px) {
  .signin-bg {
    background: #e2e8fd;
    margin-top: 10px;
    margin-bottom: 1px;
    padding: 1em;
    text-align: -webkit-center;
  }
  .signin-header {
    font-size: 1.5rem;
    color: #303030;
    line-height: 1.2;
    padding-bottom: 2em;
    display: block;
  }
  .signin-input-box {
    margin: 0 0.2em 1em;
    font-size: 1rem;
    color: #505050;
    line-height: 1.2;
    padding: 0.8rem;
    width: -webkit-fill-available;
  }
  .signin-button {
    font-size: 1rem;
  }
  .signin-footer {
    width: 100%;
    padding-top: 1em;
    line-height: 1.5;
    text-align: right;
    font-size: 2vh;
  }
}
</style>
