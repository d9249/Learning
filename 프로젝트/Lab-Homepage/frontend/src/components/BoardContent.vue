<template>
  <div class="wrapper">
    <div v-if="loading" class="loading">
      <b-spinner label="Spinning"></b-spinner>
    </div>
    <div v-else-if="posts.length == 0" class="no-post">
      게시글이 없습니다.
    </div>

    <b-table
      class="cursor-pointer"
      v-else
      hover
      :items="posts"
      :fields="fields"
      :head-variant="head"
      @row-clicked="toPostView"
    >
      <template #cell(date)="data">
        {{ dateToFormat(data.item.reg_date) }}
      </template>
    </b-table>
  </div>
</template>

<script>
import { getPosts } from "../../api/index";
import moment from "moment";

export default {
  name: "BoardContent",
  props: ["lectureId", "currentPage"],
  data() {
    return {
      loading: null,
      posts: [],
      items: [],
      fields: [
        { key: "id", label: "#", class: "text-center" },
        {
          key: "title",
          label: "제목",
          class: "w-50 table2",
          thClass: "text-center",
        },
        { key: "author", label: "작성자", class: "text-center" },
        { key: "date", label: "날짜", class: "w-10 text-center" },
      ],
      head: "light",
    };
  },
  created() {
    this.callGetPostsAPI(this.currentPage);
  },
  watch: {
    currentPage(newPage) {
      this.callGetPostsAPI(newPage);
    },
  },
  methods: {
    callGetPostsAPI(page) {
      this.loading = true;
      getPosts({ id: this.lectureId, start: (page - 1) * 10, count: 10 })
        .then((response) => {
          this.posts = response.data;
          this.posts.map((item) => {
            this.items.push({
              ...item,
              reg_date: this.dateToFormat(item.reg_date),
            });
          });
          this.loading = false;
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
        });
    },
    dateToFormat(timestamp) {
      const diff = moment().diff(timestamp);
      if (diff < 60 * 1000) {
        // 1분
        return "in a few seconds";
      } else if (diff < 24 * 60 * 60 * 1000) {
        // 하루
        return moment(timestamp).format("HH : mm");
      } else {
        return moment(timestamp).format("YYYY-MM-DD");
      }
    },
    toPostView(record) {
      this.$router.push(`/lecture/${this.lectureId}/post/${record.id}`);
    },
  },
};
</script>

<style scoped>
.wrapper {
  min-height: 50vh;
}
.no-post {
  background-color: #eeeeee;
  font-size: 2rem;
  text-align: center;
  padding: 3em 0;
}
.loading {
  background-color: #eeeeee;
  text-align: center;
  padding: 3em 0;
}
.cursor-pointer {
  cursor: pointer;
}
@media (max-width: 768px) {
  .table {
    width: 100%;
    font-size: .7em;
  }
}
</style>
