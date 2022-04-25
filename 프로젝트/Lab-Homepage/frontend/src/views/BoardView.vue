<template>
  <div>
    <board-header :lectureId="lectureId"></board-header>
    <board-content :lectureId="lectureId" :currentPage="currentPage"></board-content>
    <board-footer :lectureId="lectureId" v-on:clickPage="changePage"></board-footer>
  </div>
</template>

<script>
import BoardHeader from "../components/BoardHeader";
import BoardContent from "../components/BoardContent";
import BoardFooter from "../components/BoardFooter";

export default {
  name: 'BoardView',
  props: ['lectureId'],
  data() {
    return {
      currentPage: 1,
    }
  },
  components: {
    BoardHeader,
    BoardContent,
    BoardFooter,
  },
  methods: {
    changePage(clicked) {
      this.currentPage = clicked;
    },
  },
};
</script>

<style scoped></style>
