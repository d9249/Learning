<template>
  <b-modal id="add-board-modal" title="강의 추가" centered @ok="addPrevent">
    
    <b-form-group label="강의 이름" description="예시와 같이 '강의이름(요일)'과 같은 형식을 가져야 합니다.">
      <b-form-input v-model="name" placeholder="예) 네트워크프로그래밍(월123)"></b-form-input>
    </b-form-group> 

  </b-modal>
</template>

<script>
import { postBoard } from '../../api/index';

export default {
  name: 'BoardAddModal',
  data() {
    return {
      name: '',
    }
  },
  methods: {
    addPrevent(e) {
      e.preventDefault();
      this.onClickAddOK();
    },
    onClickAddOK() {
      postBoard({ name: this.name, category: 'lecture', ongoing: 1 })
        .then(response => {
          if (response.data.isOK) {
            alert("강의가 추가되었습니다.");
            this.$router.go();
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          }
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        })
    }
  }
}
</script>

<style>

</style>