<template>
  <div class="w-100 d-flex justify-content-center">
    <b-pagination
      v-model="currentPage"
      :total-rows="rows"
      :per-page="perPage"
      class="m-0"
    ></b-pagination>
  </div>
</template>

<script>
import { getCountOfPosts } from '../../api/index';

export default {
  name: 'BoardFooter',
  props: ['lectureId'],
  data() {
    return {
      currentPage: 1,
      perPage: 1,
      rows: 0,
    };
  },
  created() {
    getCountOfPosts({ lectureId: this.lectureId })
      .then(response => {
        const c = response.data.count;
        this.rows = c % 10 == 0 ? c / 10 : parseInt(c / 10) + 1;
      })
      .catch((err) => {
        if (err.response)   console.error(err.response.data.error);
      });
  },
  watch: {
    currentPage(newPage) {
      this.$emit('clickPage', newPage);
    }
  }
};
</script>

<style scoped>
.wrapper {
  display: block;
  text-align: center;
}
.post-write-div {
  text-align: center;
  float: right;
}
</style>
