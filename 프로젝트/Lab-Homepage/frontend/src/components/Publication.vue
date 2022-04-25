<template>
  <div>
    <common-header>
      <i class="fas fa-book" slot="icon"></i>
      <span slot="title">Publication</span>
      <span slot="additional" v-if="GET_ROLE < 2">
        <b-button
          class="float-right font-size-13 mt-1 px-2 ml-2"
          variant="danger"
          @click="onDeleteBtn = !onDeleteBtn"
        >
          <b-icon icon="trash" class="m-0"></b-icon>
        </b-button>
        <b-button
          v-b-modal.upload-research-modal
          class="float-right font-size-13 mt-1 px-2"
          variant="info"
        >
          <b-icon icon="file-earmark-plus" class="m-0"></b-icon>
        </b-button>
        <publication-upload-modal></publication-upload-modal>
      </span>
    </common-header>

    <div v-if="publications.length == 0" id="no-publication" class="rounded">
      준비중입니다...
    </div>
    <div
      v-for="publication in publications"
      v-bind:key="publication.id"
      class="d-flex justify-content-center text"
    >
      <b-list-group class="w-90 py-2">
        <b-list-group-item>
          <div>
            <h5 class="mb-1 text">
              <b-icon icon="file-earmark-text-fill" class="mr-1"> </b-icon>
              {{ publication.paper_title }}

              <a :href="publication.link" v-if="Boolean(publication.link)">
                <b-icon icon="link" class="mr-1"> </b-icon>
              </a>

              <a
                href="#"
                @click="onClickDownload(publication.file_path)"
                v-if="Boolean(publication.file_path)"
              >
                <b-icon icon="file-earmark-arrow-down"> </b-icon>
              </a>
            </h5>
          </div>

          <p class="mb-1 text2">
            <b-icon icon="person-fill"> </b-icon>
            {{ publication.authors ? publication.authors : "no author" }}
          </p>

          <small class="text-muted text2">
            <b-icon icon="calendar2-check-fill" class="mr-1"> </b-icon>
            {{ dateToFormat(publication.reg_date) }}
          </small>
          <br />

          <small class="text-muted text2">
            <i class="fas fa-book-reader mr-1"></i>
            {{ publication.publisher ? publication.publisher : "no publisher" }}
          </small>
        </b-list-group-item>
      </b-list-group>

      <span v-if="GET_ROLE < 2">
        <b-button
          v-if="GET_ROLE < 2 && onDeleteBtn"
          v-b-modal.delete-research-modal
          class="float-right publication-delete-btn p-0"
          variant="danger"
          @click="onClickDeleteBtn(publication.id)"
        >
          <b-icon icon="x" font-scale="2" class="m-0"></b-icon>
        </b-button>
      </span>
    </div>

    <b-modal
      id="delete-research-modal"
      title="논문 삭제"
      okVariant="danger"
      @ok="onClickDeleteOK()"
    >
      삭제하시겠습니까?
    </b-modal>
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";
import PublicationUploadModal from "./PublicationUploadModal";
import { getResearch, deleteResearch, downloadResearch } from "../../api/index";
import moment from "moment";
import { mapGetters } from "vuex";

export default {
  name: "Publication",
  components: {
    CommonHeader,
    PublicationUploadModal,
  },
  data() {
    return {
      publications: [],
      selectedItemId: null,
      onDeleteBtn: false,
    };
  },
  computed: {
    ...mapGetters(["GET_ROLE"]),
  },
  created() {
    getResearch()
      .then((response) => {
        this.publications = response.data;
      })
      .catch((err) => {
        if (err.response)   console.error(err.response.data.error);
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        this.$router.go("/");
      });
  },
  methods: {
    dateToFormat(date) {
      return moment(date).format("Do MMMM YYYY");
    },
    onClickDeleteBtn(id) {
      this.selectedItemId = id;
    },
    onClickDeleteOK() {
      deleteResearch({ id: this.selectedItemId })
        .then((response) => {
          if (response.data.isOK) {
            this.$router.go();
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
    onClickDownload(filepath) {
      downloadResearch({ filepath })
        .then((response) => {
          const url = window.URL.createObjectURL(new Blob([response.data]));
          const link = document.createElement("a");
          link.href = url;
          link.setAttribute(
            "download",
            filepath
              .split("_")
              .slice(1)
              .join("_")
          ); //or any other extension
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
  },
};
</script>

<style scoped>
.h4 {
  padding-right: 5px;
}
.w-90 {
  width: 90% !important;
}
.font-size-13 {
  font-size: 1.3vh;
}
.publication-delete-btn {
  transform: translateX(-50%);
  font-size: 1.5vh;
}
#no-publication {
  background-color: #eeeeee;
  font-size: 2rem;
  text-align: center;
  padding: 3em 0;
}
@media (max-width: 768px) {
  .text {
    font-size: 2vh;
  }
  .text2 {
    font-size: 1.4vh;
  }
}
</style>
