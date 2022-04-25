<template>
  <div class="pb-5">
    <common-header>
      <b-icon icon="hand-thumbs-up" slot="icon"></b-icon>
      <span slot="title">현재 진행중인 수업</span>
      <span slot="smallTitle">Ongoing lectures</span>
      <span slot="additional" v-if="GET_ROLE < 1">
        <b-button
          @click="isDeleteBtnClick = !isDeleteBtnClick"
          class="float-right font-size-13 mt-1 px-2 ml-2"
          variant="danger"
        >
          <b-icon icon="trash" class="m-0"></b-icon>
        </b-button>
        <b-button
          v-b-modal.add-board-modal
          class="float-right font-size-13 mt-1 px-2"
          variant="info"
        >
          <b-icon icon="clipboard-plus" class="m-0"></b-icon>
        </b-button>
        <board-add-modal></board-add-modal>
      </span>
    </common-header>

    <div v-if="loading" class="loading">
      <b-spinner label="Spinning"></b-spinner>
    </div>
    <div v-else-if="lectures.length == 0" class="no-lecture">
      진행중인 수업이 없습니다.
    </div>

    <div
      class="d-flex"
      v-for="lecture in splitedLectureName"
      v-bind:key="lecture.id"
      v-else
    >
      <router-link :to="{ path: 'lecture/' + lecture.id }" class="div-lecture">
        <i v-for="(sliced, index) in lecture.name" :key="sliced + index">
          {{ sliced }}
        </i>
      </router-link>
      <span v-if="GET_ROLE < 1">
        <b-button
          v-if="isDeleteBtnClick"
          v-b-modal.delete-board-modal
          class="float-right board-delete-btn p-1 mt-3 ml-3"
          variant="danger"
          @click="onClickDeleteBtn(lecture.id)"
        >
          <b-icon icon="x" font-scale="1" class="m-0 p-0"></b-icon>
        </b-button>
      </span>
    </div>

    <b-modal
      id="delete-board-modal"
      title="강의 삭제"
      okVariant="danger"
      @ok="onClickDeleteOK()"
    >
      강의를 삭제하시겠습니까?
    </b-modal>
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";
import BoardAddModal from "./BoardAddModal";
import { getOngoingLecture, softDeleteBoard } from "../../api/index";
import { mapGetters } from "vuex";

export default {
  name: "OngoingLecture",
  components: {
    CommonHeader,
    BoardAddModal,
  },
  data() {
    return {
      lectures: [],
      loading: null,
      selectedItemId: null,
      isDeleteBtnClick: false,
    };
  },
  computed: {
    ...mapGetters(["GET_ROLE"]),
    splitedLectureName() {
      let newArr = [...this.lectures];
      newArr.map((el) => {
        return (el.name = el.name.split(""));
      });
      return newArr;
    },
  },
  created() {
    this.loading = true;
    getOngoingLecture()
      .then((response) => {
        this.lectures = response.data;
        this.loading = false;
      })
      .catch((err) => {
        if (err.response) console.error(err.response.data.error);
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        this.$router.push("/");
      });
  },
  methods: {
    onClickDeleteBtn(id) {
      this.selectedItemId = id;
    },
    onClickDeleteOK() {
      softDeleteBoard({ id: this.selectedItemId })
        .then((response) => {
          if (response.data.isOK) {
            this.$router.go();
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.push("/");
          }
        })
        .catch((err) => {
          if (err.response) console.error(err.response.data.error);
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        })
        .finally(() => {
          this.selectedItemId = null;
        });
    },
  },
};
</script>

<style scoped lang="scss">
.div-lecture {
  text-align: center;
  width: 100%;
  margin: 0 auto;
  font-family: "Oswald", sans-serif;
  font-size: 2.5vh;
  text-transform: uppercase;
  letter-spacing: 0.2rem;
  text-decoration: none;
  overflow: hidden;
  display: inline-block;
  padding: 0.3rem 5rem 0.3rem 5rem;
  color: black;
  border: 1px solid #fff;
  cursor: pointer;
  transform: translate3d(0, 0, 0);
  line-height: 32px;
  // float: left;
  top: -48px;
  &:hover {
    background-color: #808080;
    color: white;
  }
}
i {
  font-style: normal;
  line-height: 3.5rem;
  display: inline-block;
  height: 50%;
  padding-left: 1px;
  padding-right: 1px;
  transition: all 0.3s cubic-bezier(0.19, 1, 0.22, 1);
  &:hover {
    transform: translate3d(0, -6px, 0);
  }
}
.row {
  max-width: 50%;
  margin: 0 auto;
}
p:hover {
  color: white;
}
.col {
  text-align: center;
  cursor: pointer;
  &:hover {
    background-color: #808080;
    color: white;
  }
}
.no-lecture {
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
.font-size-13 {
  font-size: 1.3vh;
}
.board-delete-btn {
  font-size: 0.8rem;
}
@media (max-width: 768px) {
  .div-lecture {
    font-size: 2.3vh;
    padding: 0vh 3rem 0vh 3rem;
  }
  .px-2 {
    padding: 0.1rem !important;
  }
}
</style>
