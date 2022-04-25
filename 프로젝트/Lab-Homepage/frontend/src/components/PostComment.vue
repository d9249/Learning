<template>
  <div>
    <!-- 댓글들 -->
    <div v-for="(comment, idx) in comments" :key="comment.id">
      <hr />

      <!-- 댓글 -->
      <div>
        <!-- 닉네임 & 메뉴 -->
        <div class="pt-1 pb-1">
          <span class="text font-weight-bold">{{ comment.nickname }}</span>
          <span class="font-size-18 color-gray ml-1">
            {{ timestampToDate(comment.reg_date) }}
            <span v-if="comment.edit_date"> (수정됨) </span>
          </span>

          <!-- 메뉴 - 수정 삭제 버튼 -->
          <b-dropdown
            id="comment-menu"
            class="float-right"
            v-if="GET_NICKNAME == comment.nickname || GET_ROLE < 1"
            no-caret
            variant="link"
            right
            menu-class="w-0"
          >
            <template #button-content>
              <b-icon icon="three-dots" class="color-gray"></b-icon>
            </template>

            <b-dropdown-item @click="onClickUpdateBtn(idx)"
              >수정</b-dropdown-item
            >
            <b-dropdown-item @click="onClickDeleteBtn(comment.id)"
              >삭제</b-dropdown-item
            > </b-dropdown
          ><!-- 메뉴 - 수정 삭제 버튼  -->
        </div>
        <!-- 닉네임 & 메뉴 -->

        <!-- 댓글 수정 -->
        <div v-if="comment.isEditting">
          <b-form-textarea
            v-model="editNewComment"
            rows="3"
            max-rows="6"
            class="mb-2"
          >
          </b-form-textarea>

          <div class="font-size-18 color-gray">
            {{ editNewComment.length }} / 3000
            <b-button
              variant="info"
              class="float-right btn-sm font-size-15"
              @click="onClickUpdateSubmit(comment.id)"
              >수정</b-button
            >
            <b-button
              variant="light"
              class="float-right btn-sm mr-2 font-size-15"
              @click="onClickUpdateCancel(idx)"
              >취소</b-button
            >
          </div>
        </div><!-- 댓글 수정 -->

        <!-- 댓글 본문 -->
        <div class="font-size-20 white-space-pre" v-else>{{ comment.content }}</div>

      </div>
      <!-- 댓글 -->
    </div>
    <!-- 댓글들 -->

    <b-modal
      id="delete-comment-modal"
      ref="deleteModal"
      title="댓글 삭제"
      okVariant="danger"
      @ok="onDeleteCommentOK"
      centered
    >
      댓글을 삭제하시겠습니까?
    </b-modal>

    <!-- 댓글 작성 -->
    <div class="mt-4 mb-5">
      <div class="text d-flex justify-content-between mb-1">
        <div class="font-size-10 font-weight-bold">
          {{ GET_NICKNAME }}
        </div>
      </div>

      <b-form-textarea v-model="newComment" rows="3" max-rows="6" class="mb-2">
      </b-form-textarea>

      <div class="font-size-18 color-gray">
        {{ newComment.length }} / 3000
        <b-button
          variant="info"
          class="float-right btn-sm"
          @click="onClickSubmit"
          >등록</b-button
        >
      </div>
    </div><!-- 댓글 작성 -->

  </div>
</template>

<script>
import {
  getComments,
  deleteComment,
  postComment,
  editComment,
} from "../../api/index";
import moment from "moment";
import { mapGetters } from "vuex";

export default {
  name: "PostComment",
  props: ["postId"],
  data() {
    return {
      comments: [],
      newComment: "",
      editNewComment: "",
      deleteSelected: null,
    };
  },
  computed: {
    ...mapGetters(["GET_NICKNAME", "GET_ROLE"]),
  },
  created() {
    getComments({ postId: this.postId })
      .then((response) => {
        this.comments = response.data;
        this.comments.map((item) => {
          item.isEditting = false;
        });
      })
      .catch((err) => {
        if (err.response)   console.error(err.response.data.error);
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        this.$router.push("/");
      });
  },
  methods: {
    timestampToDate(timestamp) {
      return moment(timestamp).format("YYYY-MM-DD HH:mm:ss");
    },
    onClickSubmit() {
      postComment({ postId: this.postId, content: this.newComment })
        .then((response) => {
          if (response.data.isOK) {
            this.$router.go();
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go();
          }
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go();
        });
    },
    onClickUpdateBtn(idx) {
      // 배열 자체가 바뀌는 것이 아니라면 re-rendering이 발생하지 않는다.
      // 따라서 새로 배열을 만들고 해당 요소의 isEditting을 변경하고 새로운 배열을 대입한다.
      let list = [...this.comments];
      list[idx] = { ...this.comments[idx], isEditting: true };
      this.editNewComment = this.comments[idx].content;
      this.comments = list;
    },
    onClickUpdateCancel(idx) {
      let list = [...this.comments];
      list[idx] = { ...this.comments[idx], isEditting: false };
      this.editNewComment = "";
      this.comments = list;
    },
    onClickUpdateSubmit(id) {
      editComment({ id, content: this.editNewComment })
        .then((response) => {
          if (response.data.isOK) {
            this.$router.go();
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go();
          }
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go();
        });

      this.editNewComment = "";
    },
    onClickDeleteBtn(id) {
      this.deleteSelected = id;
      this.$refs["deleteModal"].show();
    },
    onDeleteCommentOK() {
      deleteComment({ id: this.deleteSelected })
        .then((response) => {
          if (response.data.isOK) {
            this.$router.go();
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go();
          }
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go();
        });
      this.deleteSelected = null;
    },
  },
};
</script>

<style>
#comment-menu .dropdown-menu {
  min-width: unset !important;
}
#comment-menu .dropdown-item {
  font-weight: 350;
  font-size: 0.9rem;
}
#comment-menu button {
  padding: 0;
  margin-right: 0.5em;
  outline: none;
  box-shadow: none;
}
</style>

<style scoped>
.font-size-18 {
  font-size: 1.8vh;
}
.font-size-20 {
  font-size: 2vh
}
.font-size-15 {
  font-size: 1.5vh
}
.color-gray {
  color: #8a8a8a;
}
.white-space-pre {
  white-space: pre-wrap;
}
</style>
