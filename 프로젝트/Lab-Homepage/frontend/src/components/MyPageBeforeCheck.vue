<template>
  <div class="content-wrapper">

    <common-header>
      <b-icon icon="key" slot="icon"></b-icon>
      <span slot="title">비밀번호 확인</span>
      <span slot="smallTitle">Password check</span>
    </common-header>

    <div class="text-center pt-4">
      개인 정보 조회를 위해서는 인증이 필요합니다.<br>
      비밀번호 입력 후 확인 버튼을 클릭해 주세요.
    </div>

    <b-form-group
      label-cols-sm="4"
      label-cols-lg="3"
      label-size="lg"
      content-cols-sm
      content-cols-lg="7"
      label="비밀번호"
      label-for="input-pw"
      class="w-75 pt-5 mx-auto"
    >
      <b-form-input id="input-pw" type="password" v-model="inputPw" @keyup.enter="onClickSubmit"></b-form-input>
    </b-form-group>

    <div class="text-center">
      <b-button variant="primary" @click="onClickSubmit">확인</b-button>
    </div>

  </div>
</template>

<script>
import CommonHeader from './common/CommonHeader';
import { postIdentification } from '../../api/index';

export default {
  name: 'MyPageBeforeCheck',
  components: {
    CommonHeader,
  },
  data() {
    return {
      inputPw: '',
    }
  },
  methods: {
    onClickSubmit() {
      postIdentification({ userPw: this.inputPw })
        .then(response => {
          if (response.data.isOK) {
            this.$router.push({ path: '/mypage', query: { t: response.data.token } });
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.push("/");
          }
        })
        .catch(err => {
          if (err.response.status == 401) {
            alert("비밀번호가 올바르지 않습니다.");
          } else {
            if (err.response)   console.error(err.response.data.error);
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.push("/");
          }
        });
    }
  }
}
</script>

<style scoped>
.content-wrapper {
  min-height: 50vh;
}
</style>