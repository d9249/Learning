<template>
  <div>
    <common-header>
      <span slot="title">{{ this.lectureOnlyName }}</span>
      <span slot="smallTitle">{{ this.lectureOnlyDay }}</span>
      <span slot="additional" v-if="GET_ROLE < 3">
        <b-button 
          class="float-right font-size-6 mt-1 px-2" 
          variant="info" 
          :to="{ path: `/lecture/${this.lectureId}/post/write` }">
          <b-icon icon="file-plus"></b-icon>
        </b-button>
      </span>
    </common-header>
  </div>
</template>

<script>
import CommonHeader from './common/CommonHeader';
import { getBoard } from '../../api/index';
import { mapGetters } from 'vuex';

export default {
  name: 'BoardHeader',
  props: ['lectureId'],
  components: {
    CommonHeader,
  },
  data() {
    return {
      lectureName: '',
    }
  },
  created() {
    getBoard({ lectureId: this.lectureId })
      .then(response => {
        this.lectureName = response.data.name;
      })
      .catch(err => {
        if (err.response.status == 401) {
          alert("접근 권한이 없습니다.");
          this.$router.push("/");
        } else {  
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.push("/");
        }
      });
  },
  computed: {
    ...mapGetters(['GET_ROLE']),
    lectureOnlyName() {
      if (this.lectureName.indexOf('(') == -1) {
        return this.lectureName;
      } else {
        return this.lectureName.slice(0, this.lectureName.indexOf('('));
      }
    },
    lectureOnlyDay() {
      if (this.lectureName.indexOf('(') == -1) {
        return "";
      } else {
        return this.lectureName.slice(this.lectureName.indexOf('(') + 1, this.lectureName.indexOf(')'));
      }
    }
  },
};
</script>

<style scoped>
div > p {
  font-weight: bold;
  font-size: x-large;
  display: inline;
}
.text {
  padding-left: 10px;
  color: #808080;
  font-size: 1.1rem;
}
.font-size-6 {
  font-size: .6em;
}
</style>
