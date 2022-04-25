<template>
  <div>
    <common-header>
      <b-icon icon="person-circle" slot="icon"></b-icon>
      <span slot="title">내 정보 수정</span>
      <span slot="smallTitle">Information revision</span>
    </common-header>

    <div class="content-wrapper">

      <div class="pb-4">
        <div class="icon-wrapper"><i class="fas fa-key"></i></div>
        <span class="small-title">아이디 (이메일)</span>
        <div class="px-4 py-2 mt-2 font-size-12 border">{{ this.userEmail }}</div>
      </div>

      <b-form-group
        description="닉네임은 한글, 영문, 숫자로 이루어진 2~20자리 문자열입니다."
        class="mb-0 pb-4">
        
        <label for="input-user-nickname">
          <div class="icon-wrapper"><i class="fas fa-signature"></i></div>
          <span class="small-title">닉네임</span>
        </label>
        
        <div v-if="isAuthNickname">
          <div class="px-4 py-2 font-size-12 border">{{ this.userNickname }}</div>
        </div>
        <div v-else class="d-flex">
          <b-form-input
            id="input-user-nickname"
            placeholder="한글, 영문, 숫자 2~20자리"
            class="px-4 py-2 font-size-12 flex-8"
            :state="validationNickname"
            :class="{ 'shake-anime': this.isInvaildNickname }"
            v-model="userNickname">
          </b-form-input>

          <b-button @click="onClickDupCheck" variant="light" class="flex-1 ml-4 border dupcheck-btn">중복확인</b-button>
        </div>
        
      </b-form-group>

      <label for="input-user-name">
        <div class="icon-wrapper"><b-icon icon="person-badge"></b-icon></div>
        <span class="small-title">이름</span>
      </label>
      <b-form-group
        class="mb-0 pb-4">
        
        <b-form-input
          id="input-user-name"
          class="px-4 py-2 font-size-12"
          :state="validationName"
          :class="{ 'shake-anime': this.isInvaildName }"
          v-model="userName">
        </b-form-input>
      </b-form-group>

      <label for="input-user-studentId">
        <div class="icon-wrapper"><i class="fas fa-school"></i></div>
        <span class="small-title">학번</span>
      </label>
      <b-form-group
        class="mb-0 pb-4">
        
        <b-form-input
          id="input-user-studentId"
          class="px-4 py-2 font-size-12"
          :state="validationStudentId"
          :class="{ 'shake-anime': this.isInvaildStudentId }"
          v-model="userStudentId">
        </b-form-input>
      </b-form-group>
      
      <div class="text-right footer">
        <b-button class="m-3" @click="onCancelClick">취소</b-button>
        <b-button class="m-3" variant="info" @click="onUpdateClick">수정</b-button>
        <b-button class="m-3" variant="danger" v-b-modal.delete-post-modal>계정삭제</b-button>
      </div>
    </div>

    <b-modal id="same-nickname" title="닉네임 확인" centered ok-only>
      <div class="text-center">
        [ <b>{{ this.userNickname }}</b> ] 은(는) 현재 닉네임 입니다.
      </div>
    </b-modal>

    <b-modal id="change-nickname-good" title="닉네임 확인" okVariant="success" @ok="onChangeNicknameOK" centered>
      <div class="text-center">
        [ <b>{{ this.userNickname }}</b> ] 은(는) <span class="green">사용 가능</span> 합니다. <br>
        <span class="green">OK</span>을 누르면 <span class="red">변경할 수 없습니다.</span>
      </div>
    </b-modal>

    <b-modal id="change-nickname-bad" title="닉네임 확인" centered ok-only>
      <div class="text-center">
        [ <b>{{ this.userNickname }}</b> ] 은(는) <span class="red">사용 할 수 없습니다.</span>
      </div>
    </b-modal>

    <b-modal id="update-user-check" title="내 정보 수정" centered @ok="onUpdateUserOK">
      <div>
        내 정보를 수정하시겠습니까?
      </div>
    </b-modal>

    <b-modal id="delete-post-modal" title="계정 삭제" centered okVariant="danger" @ok="onDeleteUserOK" ok-only>
      <div>
        정말 계정을 삭제하시겠습니까?
      </div>
    </b-modal>
  </div>
</template>

<script>
import CommonHeader from "../components/common/CommonHeader";
import { deleteUser, updateUser, checkIdentification, getUserInfo, duplicationCheck } from '../../api/index';
import { mapActions } from 'vuex';

export default {
  name: "MyPage",
  components: {
    CommonHeader,
  },
  props: ['token'],
  data() {
    return {
      userEmail: '',
      userNickname: '',
      userName: '',
      userStudentId: '',
      isAuthNickname: false,
      isInvaildNickname: false,
      isInvaildName: false,
      isInvaildStudentId: false,
      originNickname: '',
    };
  },
  computed: {
    validationNickname() {
      return /^[가-힣a-zA-Z0-9]{2,10}$/.test(this.userNickname) && this.originNickname == this.userNickname;
    },
    validationName() {
      return /^[가-힣a-zA-Z0-9]{1,10}$/.test(this.userName);
    },
    validationStudentId() {
      return /^[0-9]{4,12}$/.test(this.userStudentId);
    }
  },
  created() {
    // 먼저 접근 확인
    checkIdentification({ t: this.token })
      .then(response => {
        if (response.data.isOK) {
          // 사용자 정보 가져오기
          getUserInfo()
            .then(response => {
              if (response.data.isOK) {
                this.userEmail = response.data.userInfo.user_identification;
                this.originNickname = response.data.userInfo.nickname;
                this.userNickname = response.data.userInfo.nickname;
                this.userName = response.data.userInfo.name;
                this.userStudentId = response.data.userInfo.student_id;
              } else {
                alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
                this.$router.push("/");
              }
            })
            .catch(err => {
              if (err.response)   console.error(err.response.data.error);
              alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
              this.$router.push("/");
            })
        } else {
          alert("올바르지 않은 접근입니다.");
          this.$router.push("/");
        }
      })
      .catch(err => {
        console.error(err);
        alert("올바르지 않은 접근입니다.");
        this.$router.push("/");
      });
  },
  methods: {
    ...mapActions(['CLEAR']),
    onCancelClick() {
      this.$router.push("/");
    },
    onUpdateClick() {
      // Validation
      if (this.originNickname != this.userNickname && ! this.isAuthNickname) {
        this.isInvaildNickname = true;
        setTimeout(() => {
          this.isInvaildNickname = false;
        }, 500);
        return;
      } else if (! this.validationName) {
        this.isInvaildName = true;
        setTimeout(() => {
          this.isInvaildName = false;
        }, 500);
        return;
      } else if (! this.validationStudentId) {
        this.isInvaildStudentId = true;
        setTimeout(() => {
          this.isInvaildStudentId = false;
        }, 500);
        return;
      } else {
        this.$bvModal.show('update-user-check');
      }
    },
    onUpdateUserOK() {
      updateUser({ nickname: this.userNickname, name: this.userName, studentId: this.userStudentId })
        .then(response => {
          if (response.data.isOK) {
            this.$router.go();
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.push("/");
          }
        })
        .catch(err => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.push("/");
        });
    },
    onDeleteUserOK() {
      deleteUser()
        .then(response => {
          if (response.data.isOK) {
            this.CLEAR();
            alert("계정이 삭제되었습니다.");
            this.$router.push('/');
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.push("/");
          }
        })
        .catch(err => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.push("/");
        });
    },
    onClickDupCheck() {
      // validation
      if (this.originNickname == this.userNickname) {
        this.$bvModal.show('same-nickname');
        
      } else if (/^[가-힣a-zA-Z0-9]{2,10}$/.test(this.userNickname)) {
        duplicationCheck({ target: 'nickname', data: this.userNickname })
          .then(response => {
            if (response.data.isOK) {
              this.$bvModal.show('change-nickname-good');
            } else {
              this.$bvModal.show('change-nickname-bad');
            }
          })
          .catch(err => {
            if (err.response)   console.error(err.response.data.error);
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.push("/");
          });

      } else {
        this.isInvaildNickname = true;
        setTimeout(() => {
          this.isInvaildNickname = false;
        }, 500);
      }
    },
    onChangeNicknameOK() {
      this.isAuthNickname = true;
    }
  }
};
</script>

<style scoped>
.content-wrapper {
  width: 75%;
  margin: 5% auto 0;
}
.icon-wrapper {
  width: 30px;
  display: inline-block;
}
.small-title {
  font-size: 1.1rem;
  font-weight: 500;
}
.font-size-12 {
  font-size: 1.2rem;
}
.flex-8 {
  flex: 8;
}
.flex-1 {
  flex: 1;
}
.green {
  color: #5cb85c;
}
.red {
  color: #d9534f;
}
.dupcheck-btn {
  box-shadow: .1em .1em .3em rgb(165, 165, 165);
  font-size: 1rem;
}
@media (max-width: 992px) {
  .dupcheck-btn {
    font-size: .8rem;
  }
}
@media (max-width: 768px) {
  .content-wrapper {
    width: 100%;
    margin: 0;
  }
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
</style>
