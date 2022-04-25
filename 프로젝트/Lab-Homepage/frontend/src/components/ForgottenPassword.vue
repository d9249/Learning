<template>
  <div class='bg'>
    <div class='wrapper'>
      <div class='container'>

        <span class=header>비밀번호 변경</span>
        <form>

          <div class="input-box">
            <label for="userId" class="label">아이디 (이메일)</label>
            <div>
              <input name="userId" type="text" placeholder="예) SmartIoTLab@kyonggi.ac.kr" v-model="userId" ref="userId" class=input-halfmg>
            </div>
            <div class="flex-box">
              <input type="text" placeholder="이메일 인증번호를 입력하세요" v-model="emailAuthCode" ref="emailAuthCode" class=flex-input>
              <button v-if="isEmailSent" type="button" class="flex-button" v-on:click="verifyEmailAuth">확인</button>
              <button v-else-if="isMailing" type="button" class="flex-button">
                <b-spinner small variant="success" label="Spinning"></b-spinner>
              </button>
              <button v-else type="button" class="flex-button" v-on:click="requestEmailAuth">인증요청</button>
            </div>
          </div>

        </form>

        <div class="footer">
          <router-link to="/signup">회원가입</router-link>
        </div>

      </div>
    </div>
  </div>
</template>

<script>
import { duplicationCheck, verifyEmail, verifyCode } from '../../api/index'

export default {
  name: 'ForgottenPassword',
  data() {
		return {
      userId: '',
      newPw: '',
      newPw2: '',
      // check variables
      emailAuthCode: '',
      emailAuthId: 0,
      isEmailSent: false,
      isMailing: false,
			}
	},
	methods: {
		async requestEmailAuth() {      
      try {
        // 이메일 문자열 validation
        const emailRule = /^[a-z0-9_+.-]+@([a-z0-9-]+\.)+[a-z0-9]{2,4}$/;
        if (emailRule.test(this.userId) === false) {
          alert('이메일이 올바르지 않습니다');

        } else {
          this.isMailing = true;
          // 아이디(이메일) 중복 확인
          const dupCheck = await duplicationCheck({ target: 'user_identification', data: this.userId });
          if (!dupCheck.data.isOK) {
            // 아이디(이메일) 전송
            const emailSuccess = await verifyEmail({ userId: this.userId });
            if (emailSuccess.data.isOK) {
              alert(`${this.userId}로 인증번호가 전송되었습니다.`);
              this.emailAuthId = emailSuccess.data.emailAuthId;
              this.isEmailSent = true;
            }
          } else {
            alert("해당 이메일의 계정이 존재하지 않습니다.");
          }
        }

      } catch(err) {
        if (err.response)   console.error(err.response.data.error);
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        this.$router.push("/");

      } finally {
        this.isMailing = false;
      }
    },
		verifyEmailAuth() {
			verifyCode({ id: this.emailAuthId, code: this.emailAuthCode, type: 'forgotten' })
				.then(response => {
          if (response.data.isOK) {
            alert("이메일이 인증되었습니다.");
            this.$router.push({ path: '/forgotten/change', query: { e: this.userId, t: response.data.token } });
          } else {
            alert("인증번호가 올바르지 않습니다.");
          }
          
				})
				.catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
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
  min-height: 80vh;
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
.flex-box {
  display: flex;
  flex-direction: row;
}
.flex-input {
  margin: 0 0 1em;
  flex: 4;
}
.flex-button {
  color: #181818;
  border-radius: 10px;
  border: 0;
  font-size: 1em;
  width: fit-content;
  cursor: pointer;
  margin: 0 0 1em 1em;
  flex: 1;
  outline: none;
}
.label {
  font-weight: bold;
  padding-top: 1em;
}
.footer {
  width: 100%;
  padding-top: 1em;
  line-height: 1.5;
  text-align: right;
}
.footer a {
  text-decoration: none;
  color: #5a5a5a;
}
</style>