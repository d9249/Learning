<template>
  <div class=bg>
    <div class=wrapper>
      <div class=container>

        <span class=header>새로운 비밀번호</span>
        <form>

          <div class="input-box">
            <label class="label">아이디 (이메일)</label>
            <div>
              <span ref="userId" class=input-halfmg>{{ userEmail }}</span>
            </div>

            <label for="newPw" class="label">비밀번호</label>
            <input name="newPw" type="password" placeholder="영문, 숫자, 특수문자 8~20자리" v-model="newPw" ref="newPw" class=input-halfmg>
            <input name="newPw2" type="password" placeholder="비밀번호를 다시 입력하세요" v-model="newPw2" ref="newPw2" class=input>
            <button type="button" class="button" v-on:click="updatePassword">비밀번호 변경</button>
          </div>

        </form>

      </div>
    </div>
  </div>
</template>

<script>
import { changePassword, checkForgotten } from '../../api/index'

export default {
  name: 'ChangePassword',
  props: ['token', 'userEmail'],
  data() {
		return {
      userId: '',
      newPw: '',
      newPw2: '',
      // check variables
      emailAuthCode: '',
      emailAuthId: 0,
      isEmailSent: false,
      isAuthEmail: false,
      isMailing: false,
		}
	},
  created() {
    // 토큰 유효성 확인
    checkForgotten({ t: this.token })
      .then(response => {
        if (! response.data.isOK) {
          alert("잘못된 접근입니다.");
          this.$router.push("/");
        }
      })
      .catch(() => {
        alert("잘못된 접근입니다.");
        this.$router.push("/");
      });
  },
	methods: {
    updatePassword() {
      // 비밀번호 빈칸 확인
      if (this.newPw.length == 0) {
        alert("비밀번호를 입력해주세요");
        this.$refs.newPw.focus();
      }
      // 비밀번호가 일치하는지
      else if (this.newPw !== this.newPw2) {
        alert("비밀번호가 일치하지 않습니다.");
        this.$refs.newPw2.focus();
      }
      // 비밀번호 validation
      else if (/^[a-zA-Z0-9`~!@#$%^&+=]{8,20}$/.test(this.newPw) === false) {
        alert("비밀번호는 영문, 숫자, 특수문자로 구성된 8~20자 이여야 합니다.");
        this.$refs.newPw.focus();
      }

      else {
        this.checkPwOk();
      }
    },
    checkPwOk() {
      changePassword({ token: this.token, newPw: this.newPw })
        .then(response => {
          if (response.data.isOK) {
            alert("비밀번호가 변경되었습니다. 다시 로그인 해주세요.");
            this.$router.push("/");
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.push("/");
          }
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("세션이 만료되었습니다. 다시 시도해주세요.");
          this.$router.push("/");
        });
    },
  }
}
</script>

<style scoped>

input {
  border-style: solid;
  border-color: white;
  border-bottom-color: #181818;
  padding: 0.8em 0.8em 0.8em 0;
  font-size: 1em;
  color: #505050;
  width: -webkit-fill-available;
}
input:focus {
  outline: none;
  border-bottom-color: #3860e4;
}
.bg {
  background: #e2e8fd;
  margin-top: 30px;
  margin-bottom: 30px;
  padding: 3em;
  text-align: -webkit-center;
}
.wrapper {
  min-height: 100vh;
  display: flex;
  background: #ffffff; 
  border-radius: 10px;
  padding: 5em 0.5em;
  align-items: center;
  justify-content: center;
  max-width: fit-content;
  box-shadow: 0.5em 0.5em .4em rgb(165, 165, 165);
}
.container {
  padding: 0.5em 2em;
  width: 30em;
  min-width: -webkit-fill-available;
}
.header {
  font-size: 2em;
  color: #303030;
  line-height: 1.2;
  padding-bottom: 2em;
  display: block;
}
.input-box {
  text-align: left;
  padding-bottom: 1.6em;
}
.input {
  margin: 0 0 1.5em;
}
.input-halfmg {
  margin: 0 0 0.5em;
}
.label {
  font-weight: bold;
  padding-top: 1em;
}
.button {
  background: -webkit-gradient(linear,left top,right top,from(#21d4fd),to(#b721ff));
  color: #ffffff;
  border-radius: 10px;
  border: 0;
  height: 2em;
  font-size: 1.3em;
  width: 100%;
  cursor: pointer;
  margin-top: 1em;
  outline: none;
}
</style>