<template>
  <div>
    <common-header>
      <span slot="title">{{ this.post.title }}</span>
      <span class="text" slot="smallTitle">
        <br />
        {{ this.post.nickname }}
        <br />
        {{ timestampToDate(this.post.reg_date) }}
      </span>
      <span
        slot="additional"
        v-if="this.GET_NICKNAME == this.post.nickname || GET_ROLE < 1"
      >
        <b-button
          v-b-modal.delete-post-modal
          class="float-right font-size-13 ml-2 px-2"
          variant="danger"
        >
          <b-icon class="" icon="trash"></b-icon>
        </b-button>
        <b-button
          class="float-right font-size-13 px-2"
          variant="info"
          :to="{ path: `/lecture/${this.lectureId}/post/${this.postId}/edit` }"
        >
          <b-icon icon="pencil-square"></b-icon>
        </b-button>
      </span>
    </common-header>

    <post-delete-modal
      :postId="postId"
      :attachedFiles="this.post.attached_files"
      @onDeletePost="goback"
    ></post-delete-modal>

    <div class="post-content-wrapper">
      <p id="post-content" class="text" v-html="this.post.content"></p>

      <p class="file-p pt-5">
        <span
          class="file-span"
          v-for="file in this.post.attached_files"
          :key="file.id"
          v-on:click="download(file.id, file.origin_filename)"
        >
          <b-icon icon="file-earmark-arrow-down-fill"></b-icon>
          {{ file.origin_filename }}
          <br />
        </span>
      </p>
    </div>
    <hr />

    <div class="to-list-div">
      <b-button
        class="to-list-btn"
        variant="dark"
        :to="{ path: '/lecture/' + this.lectureId }"
      >
        목록
      </b-button>
    </div>

    <br />
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";
import PostDeleteModal from "./PostDeleteModal";
import { getPost, downloadAttachedFile } from "../../api/index";
import moment from "moment";
import { mapGetters } from "vuex";

export default {
  name: "PostContent",
  props: ["lectureId", "postId"],
  components: {
    CommonHeader,
    PostDeleteModal,
  },
  data() {
    return {
      post: {},
    };
  },
  computed: {
    ...mapGetters(["GET_NICKNAME", "GET_ROLE"]),
  },
  created() {
    getPost({ postId: this.postId })
      .then((response) => {
        this.post = response.data;
      })
      .catch((err) => {
        if (err.response.status == 404) {
          alert("존재하지 않는 게시글입니다.");
          this.$router.go(-1);
        } else {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");
        }
      });
  },
  methods: {
    timestampToDate(timestamp) {
      return moment(timestamp).format("YYYY-MM-DD ddd HH:mm:ss");
    },
    download(fileId, filename) {
      downloadAttachedFile({ f: fileId })
        .then((response) => {
          const url = window.URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute("download", filename); //or any other extension
          document.body.appendChild(link);
          link.click();
          link.remove();
        })
        .catch((err) => {
          if (err.response)   console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");
        });
    },
    goback() {
      this.$router.push(`/lecture/${this.lectureId}`);
    },
  },
};
</script>

<style>
#post-content p {
  word-break: break-word;
}
</style>

<style scoped>
#post-content {
  font-weight: initial;
  font-size: 1.3rem;
  white-space: pre-wrap;
  display: block;
  letter-spacing: 1px;
}
.to-list-div {
  text-align: center;
}
.to-list-btn {
  display: inline-flex;
  align-items: center;
}
.file-span {
  cursor: pointer;
}
.file-p {
  color: #808080;
  font-size: 1.1rem;
}
.post-content-wrapper {
  min-height: 50vh;
}
.font-size-13 {
  font-size: 1.3vh;
}
.b-icon {
  margin: 0;
}
@media (max-width: 768px) {
  .text {
    font-size: 0.9rem;
  }
  .to-list-btn {
    font-size: 1.4vh;
  }
}
</style>
