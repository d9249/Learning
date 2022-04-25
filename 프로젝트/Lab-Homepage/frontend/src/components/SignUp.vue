<template>
  <div class="w-100 mt-5 bg-color-skyblue py-5">
    <div id="signup-wrapper" class="w-50 m-auto p-5">

      <div id="signup-title" class="text-center py-5">
        회원가입
      </div>

      <b-form @submit="onClickSubmit">

        <!-- 아이디 (이메일) form -->
        <label for="emailInput" class="pt-3">아이디 (이메일)</label>
        <b-form-group
          :invalid-feedback="invalidEmailFeedback"
        >
          <b-form-input 
            id="emailInput"
            placeholder="예) example@kyonggi.ac.kr"
            v-model="userId"
            :state="isValidEmail"
            :class="{ 'shake-anime': shakeEmail }"
            :disabled="isAuthEmail"
            ref="email"
          ></b-form-input>
        </b-form-group>

        <div class="d-flex">
          <b-form-input
            class="flex-8"
            placeholder="인증코드 6자리"
            v-model="emailAuthCode"
          >
          </b-form-input>

          <b-button v-if="isAuthEmail" variant="success" class="flex-1 font-size-10 ml-2 no-outline">완료</b-button>
          <b-button v-else-if="isEmailSent" variant="light" class="flex-1 font-size-10 ml-2" v-on:click="verifyEmailAuth">확인</b-button>
          <b-button v-else-if="isMailing" variant="light" class="flex-1 font-size-10 ml-2">
            <b-spinner small variant="success" label="Spinning"></b-spinner>
          </b-button>
          <b-button v-else variant="light" class="flex-1 font-size-10 ml-2" @click="onClickEmailAuth">인증</b-button>

        </div><!-- 아이디 (이메일) form -->
        
        <!-- 비밀번호 form -->
        <label for="passwordInput" class="pt-4">비밀번호</label>
        <b-form-group
          :invalid-feedback="invalidPwFeedback"
        >
          <b-form-input 
            id="passwordInput"
            placeholder="영문, 숫자, 특수문자 8~20자리"
            v-model="userPw"
            :state="isValidPw"
            type="password"
            :class="{ 'shake-anime': shakePw }"
            ref="pw"
          ></b-form-input>
        </b-form-group>
        <b-form-group
          :invalid-feedback="invalidPw2Feedback"
        >
          <b-form-input 
            placeholder="비밀번호 확인"
            v-model="userPw2"
            :state="isValidPw2"
            type="password"
            :class="{ 'shake-anime': shakePw2 }"
            ref="pw2"
          ></b-form-input>
        </b-form-group><!-- 비밀번호 form -->

        <!-- 닉네임 form -->
        <label for="nicknameInput" class="pt-3">닉네임</label>
        <b-form-group
          :invalid-feedback="invalidNicknameFeedback"
        >
          <b-form-input
            id="nicknameInput"
            placeholder="한글, 영문 2~10자리"
            class="flex-8"
            v-model="userNickname"
            :state="isValidNickname"
            :class="{ 'shake-anime': shakeNickname }"
            ref="nickname"
          ></b-form-input>
        </b-form-group><!-- 닉네임 form -->

        <!-- 이름 form -->
        <label for="nameInput" class="pt-3">이름</label>
        <b-form-group
          :invalid-feedback="invalidNameFeedback"
        >
          <b-form-input
            id="nameInput"
            placeholder="예) 홍길동"
            class="flex-8"
            v-model="userName"
            :state="isValidName"
            :class="{ 'shake-anime': shakeName }"
            ref="name"
          >
          </b-form-input>
        </b-form-group><!-- 이름 form -->

        <!-- 학번 form -->
        <label for="sIdInput" class="pt-3">학번</label>
        <b-form-input
          id="sIdInput"
          placeholder="예) 201511850"
          class="flex-8"
          v-model="userStudentId"
        >
        </b-form-input><!-- 학번 form -->

        <div class="text-center py-5">
          <b-button type="submit" variant="info" class="w-100">SIGN UP</b-button>
        </div>
      </b-form>

      <!-- 회원가입 제출 전 확인 form -->
      <b-modal ref="signup-check-modal" @ok="onClickSignUpOK" title="회원가입 정보 확인" centered>
        <b-table-simple class="m-auto" striped responsive>
          <b-tbody>
            <b-tr>
              <b-td class="font-weight-bold break-word-keep-all">아이디 (이메일)</b-td>
              <b-td class="vertical-middle">{{ userId }}</b-td>
            </b-tr>
            <b-tr>
              <b-td class="font-weight-bold">닉네임</b-td>
              <b-td class="vertical-middle">{{ userNickname }}</b-td>
            </b-tr>
            <b-tr>
              <b-td class="font-weight-bold">이름</b-td>
              <b-td class="vertical-middle">{{ userName.trim() }}</b-td>
            </b-tr>
            <b-tr>
              <b-td class="font-weight-bold">학번</b-td>
              <b-td class="vertical-middle">{{ getStudentId }}</b-td>
            </b-tr>
          </b-tbody>
        </b-table-simple>
      </b-modal>

    </div><!-- signup wrapper -->
  </div><!-- signup background -->
</template>

<script>
import { duplicationCheck, verifyEmail, verifyCode, signup } from '../../api/index';

export default {
  name: 'SignUp',
  data() {
    return {
      // 회원가입 정보
      userId: '',
      userPw: '',
      userPw2: '',
      userNickname: '',
      userName: '',
      userStudentId: '',
      emailAuthCode: '',
      emailAuthId: null,
      nicknameStatus: '',
      //
      isAuthEmail: false,
      isEmailSent: false,
      isMailing: false,
      //
      shakeEmail: false,
      shakePw: false,
      shakePw2: false,
      shakeNickname: false,
      shakeName: false,
    }
  },
  computed: {
    getStudentId() {
      return this.userStudentId.length==0? '없음' : this.userStudentId;
    },
    isValidEmail() {
      const emailRule = /^[a-z0-9_+.-]+@([a-z0-9-]+\.)+[a-z0-9]{2,4}$/;
      return this.userId.length != 0 && emailRule.test(this.userId);
    },
    invalidEmailFeedback() {
      const emailRule = /^[a-z0-9_+.-]+@([a-z0-9-]+\.)+[a-z0-9]{2,4}$/;
      if (this.userId.length == 0) {
        return "이메일을 입력해주세요";

      } else if (! emailRule.test(this.userId)) {
        return "형식이 올바르지 않습니다";

      } else {
        return "";
      }
    },
    isValidPw() {
      const passwordRule = /^[a-zA-Z0-9`~!@#$%^&+=]{8,20}$/;
      return this.userPw.length != 0 && passwordRule.test(this.userPw);
    },
    invalidPwFeedback() {
      if (this.userPw.length == 0) {
        return "비밀번호를 입력해주세요";

      } else if (! this.isValidPw) {
        return "올바르지 않은 형식입니다";

      } else {
        return "";
      }
    },
    isValidPw2() {
      return this.userPw2.length != 0 && this.userPw2 == this.userPw;
    },
    invalidPw2Feedback() {
      if (this.userPw2.length == 0) {
        return "비밀번호를 한 번 더 입력해주세요";

      } else if (! this.isValidPw2) {
        return "비밀번호가 일치하지 않습니다";

      } else {
        return "";
      }
    },
    isValidNickname() {
      if (this.userNickname.length == 0) {
        return false;
        
      } else if (this.nicknameStatus == 'Good') {
        return true;

      } else {
        return false;
      }
    },
    invalidNicknameFeedback() {
      if (this.userNickname.length == 0) {
        return '닉네임을 입력해주세요'
        
      } else if (this.nicknameStatus == 'Good') {
        return '';

      } else {
        return this.nicknameStatus;
      }
    },
    isValidName() {
      const nameRule = /^[가-힣a-zA-Z\s]{2,20}$/;
      const trimmed = this.userName.trim();
      return trimmed.length != 0 && nameRule.test(trimmed);
    },
    invalidNameFeedback() {
      const nameRule = /^[가-힣a-zA-Z]{2,20}$/;
      if (this.userName.length == 0) {
        return '이름을 입력해주세요';

      } else if (!nameRule.test(this.userName)) {
        return "올바르지 않은 형식입니다";

      } else {
        return '';
      }
    },

  },
  watch: {
    userNickname(newNickname) {
      const nicknameRule = /^[가-힣a-zA-Z0-9]{2,10}$/;
      if (nicknameRule.test(newNickname)) {
        console.error(newNickname);

        duplicationCheck({ target: 'nickname', data: newNickname })
          .then(response => {
            if (response.data.isOK) {
              this.nicknameStatus = 'Good';
            } else {
              this.nicknameStatus = '이미 존재하는 닉네임입니다.';
            }
          })
          .catch(err => {
            if (err.response)   console.error(err.response.data.error);
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go("/");
          })
        
      } else {
        this.nicknameStatus = '올바르지 않은 형식입니다.'
      }
    }
  },
  methods: {
    onClickSubmit(event) {
      event.preventDefault();

      // validation
      if (! this.isAuthEmail) {
        this.$refs['email'].focus();
        this.shakeEmail = true;
        setTimeout(() => {
          this.shakeEmail = false;
        }, 500);
      } else if (! this.isValidPw) {
        this.$refs['pw'].focus();
        this.shakePw = true;
        setTimeout(() => {
          this.shakePw = false;
        }, 500);
      } else if (! this.isValidPw2) {
        this.$refs['pw2'].focus();
        this.shakePw2 = true;
        setTimeout(() => {
          this.shakePw2 = false;
        }, 500);
      } else if (! this.nicknameStatus) {
        this.$refs['nickname'].focus();
        this.shakeNickname = true;
        setTimeout(() => {
          this.shakeNickname = false;
        }, 500);
      } else if (! this.isValidName) {
        this.$refs['name'].focus();
        this.shakeName = true;
        setTimeout(() => {
          this.shakeName = false;
        }, 500);
      } else {
        this.$refs['signup-check-modal'].show();
      }
    },
    onClickSignUpOK() {
      // DB에 저장
      const payload = {
        userId: this.userId,
        userPw: this.userPw,
        userNickname: this.userNickname,
        userName: this.userName.trim(),
        userStudentId: this.userStudentId
      };
      signup(payload)
        .then((response) => {
          if (response.data.isOK) {
             // 회원가입 완료 안내
            alert("회원가입이 완료되었습니다.\n" + "Welcome to Smart IoT Lab!");
            this.$router.push('/');
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go("/");
          }
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");
        });
    },
    async onClickEmailAuth() {
      // email validation
      if (this.isValidEmail) {

        try {
          // 아이디(이메일) 중복 확인
          const dupCheck = await duplicationCheck({ target: 'user_identification', data: this.userId });

          if (dupCheck.data.isOK) {
            this.isMailing = true;

            // 아이디(이메일) 전송
            const emailSuccess = await verifyEmail({ userId: this.userId });
            if (emailSuccess.data.isOK) {
              alert(`${this.userId}로 인증번호가 전송되었습니다.`);
              this.emailAuthId = emailSuccess.data.emailAuthId;
              this.isEmailSent = true;
            } else {
              alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
              this.$router.go("/");
            }
          } else {
            alert('해당 이메일로 가입된 계정이 존재합니다.');
          }

        } catch(err) {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");

        } finally {
          this.isMailing = false;
        }

      } else {  // email invalid
        this.shakeEmail = true;
        setTimeout(() => {
          this.shakeEmail = false;
        }, 500);
        return;
      }
    },
    verifyEmailAuth() {
      verifyCode({ id: this.emailAuthId, code: this.emailAuthCode })
        .then(response => {
          if (response.data.isOK) {
            alert("이메일이 인증되었습니다.")
            this.isAuthEmail = true;
          } else {
            alert("인증번호가 올바르지 않습니다.");
          }
        })
        .catch(err => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");
        });
    },
  }
}
</script>

<style>
.no-outline {
  outline: none !important;
  box-shadow: none !important;
}
</style>

<style scoped>
.flex-8 {
  flex: 8;
}
.flex-1 {
  flex: 1;
}
.font-size-10 {
  font-size: 1em;
}
.bg-color-skyblue {
  background-color: #e2e8fd;
}
.red {
  color: #d9534f;
}
.vertical-middle {
  vertical-align: middle;
}
.break-word-keep-all{
  word-break: keep-all;
}
.shake-anime {
  animation: shake 0.5s;
}
@keyframes shake {
  10%, 90% {
    transform: translate3d(-1px, 0, 0);
  }
  20%, 80% {
    transform: translate3d(2px, 0, 0);
  }
  30%, 50%, 70% {
    transform: translate3d(-4px, 0, 0);
  }
  40%, 60% {
    transform: translate3d(4px, 0, 0);
  }
}
#signup-title {
  font-size: 2em;
  letter-spacing: .3em;
  font-weight: bold;
}
#signup-wrapper {
  background-color: white;
  border-radius: 3em;
  box-shadow: 0.5em 0.5em .4em rgb(165, 165, 165);
}
@media (max-width: 992px) {
  #signup-wrapper {
    width: 80% !important;
  }
}
@media (max-width: 768px) {
  #signup-wrapper {
    width: 80% !important;
  }
}
@media (max-width: 576px) {
  #signup-wrapper {
    width: 100% !important;
    padding: 0 !important;
    border-radius: 0;
    box-shadow: none;
  }
}
</style>