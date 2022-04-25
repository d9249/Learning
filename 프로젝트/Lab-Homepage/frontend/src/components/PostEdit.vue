<template>
  <div>
    <common-header>
      <b-icon icon="pencil-square" slot="icon"></b-icon>
      <span slot="title">글수정</span>
      <span slot="smallTitle">Edit post</span>
    </common-header>

    <div class="pt-1 pb-3">
      <label for="title" class="font-weight-bold">제목(Title)</label>
      <b-form-input
        v-model="title"
        :state="validationTitle"
        :class="{ 'shake-anime': isInvaildTitle }"
        ref="title"
        id="title"
      ></b-form-input>
      <b-form-invalid-feedback>
        제목(title)을 입력해주세요. (50자 이내)
      </b-form-invalid-feedback>
    </div>

    <div id="editor-container" class="border p-3 mb-3 shadow-sm rounded white-space-pre">
      <editor-tiptap
        :content="content"
        @changeContent="setContent"
      ></editor-tiptap>
    </div>

    <div class="mb-2">
      <b-form-file
        v-model="selectedFiles"
        :state="validationAttachedFile"
        :class="{ 'shake-anime': isInvaildFile }"
        multiple
        accept=".jpg, .jpeg, .png, .gif, .ppt, .pptx, .pdf, .doc, .docx, .hwp, .txt, .zip, .mp4, .mp3"
        placeholder="Select files or drop it here."
        drop-placeholder="Drop file here..."
        ref="attach"
      ></b-form-file>
      <b-form-invalid-feedback>
        업로드 가능한 최대 개수는 5개이며, 최대 크기는 50 MB 입니다.
      </b-form-invalid-feedback>

      <div class="w-100 h-1em">
        <div class="font-small-gray float-right">
          {{ formatNames() }}
        </div>
      </div>

      <div class="w-100">
        <span
          class="font-small-gray pr-2"
          v-for="(file, idx) in attachedFiles"
          :key="idx + file.name"
          @click="removeItem(idx)"
        >
          <b-icon icon="file-earmark-minus"></b-icon>
          {{ file.name }}
        </span>
        <span
          class="font-small-gray pr-2"
          v-for="(file, idx) in originFiles"
          :key="'origin' + idx + file.name"
          @click="removeOriginItem(idx)"
        >
          <b-icon icon="file-earmark-minus"></b-icon>
          {{ file.name }}
        </span>
      </div>
    </div>

    <div class="font-small-gray">
      <ul>
        <li>
          선택 가능한 파일의 확장자: jpg, jpeg, png, gif, ppt, pptx, pdf, doc,
          docx, hwp, txt, zip, mp4, mp3
        </li>
        <li>업로드 가능한 최대 개수는 5개이며, 최대 크기는 50 MB 입니다.</li>
        <li>선택한 파일 중 이름을 클릭하면 업로드에서 제외시킬 수 있습니다.</li>
      </ul>
    </div>

    <div class="text-center pt-3">
      <b-button variant="info" @click="onClickSubmit">
        저장
      </b-button>
    </div>
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";
import EditorTiptap from "./EditorTiptap";
import {
  getPost,
  updatePost,
  uploadAttachedFile,
  postAttachedFile,
  deleteAttachedFile,
} from "../../api/index";

export default {
  name: "PostEdit",
  components: {
    CommonHeader,
    EditorTiptap,
  },
  props: ["lectureId", "postId"],
  data() {
    return {
      selectedFiles: [],
      attachedFiles: [],
      originFiles: [],
      deletedOriginFiles: [],
      title: "",
      content: "",
      isInvaildTitle: null,
      isInvaildFile: null,
    };
  },
  created() {
    getPost({ postId: this.postId })
      .then((response) => {
        this.title = response.data.title;
        this.content = response.data.content;
        response.data.attached_files.map((item) => {
          this.originFiles.push({
            id: item.id,
            name: item.origin_filename,
            size: item.size,
            path: item.filepath,
          });
        });
      })
      .catch((err) => {
        console.error(err);
      });
  },
  mounted() {
    // 컴포넌트가 없어질 때, 알림 리스너 등록
    window.addEventListener('beforeunload', this.unloadEvent);
  },
  beforeDestroy() {
    // 리스너 삭제
    window.removeEventListener('beforeunload', this.unloadEvent);
  },
  watch: {
    selectedFiles(newSelected) {
      newSelected.map((item) => {
        this.attachedFiles.push(item);
      });
    },
  },
  computed: {
    validationTitle() {
      return this.title.length > 1 && this.title.length <= 50;
    },
    validationAttachedFile() {
      return (
        this.getTotalCountOfFiles <= 5 &&
        this.getTotalSizeOfFiles <= 50 * 1024 * 1024
      );
    },
    getTotalSizeOfFiles() {
      let total = 0;
      for (let i = 0; i < this.originFiles.length; i++) {
        total += this.originFiles[i].size;
      }
      for (let i = 0; i < this.attachedFiles.length; i++) {
        total += this.attachedFiles[i].size;
      }
      return total;
    },
    getTotalCountOfFiles() {
      return this.originFiles.length + this.attachedFiles.length;
    },
  },
  methods: {
    formatNames() {
      return `${this.getTotalCountOfFiles} files selected (${this.formatBytes(
        this.getTotalSizeOfFiles
      )})`;
    },
    removeItem(idx) {
      this.attachedFiles.splice(idx, 1);
    },
    removeOriginItem(idx) {
      const deleted = this.originFiles.splice(idx, 1);
      this.deletedOriginFiles.push(deleted[0]);
    },
    formatBytes(bytes, decimals = 2) {
      if (bytes === 0) return "0 Bytes";

      const k = 1024;
      const dm = decimals < 0 ? 0 : decimals;
      const sizes = ["Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];

      const i = Math.floor(Math.log(bytes) / Math.log(k));

      return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + " " + sizes[i];
    },
    setContent(text) {
      this.content = text;
    },
    async onClickSubmit() {
      // Validation
      if (!this.validationTitle) {
        this.$refs["title"].focus();
        this.isInvaildTitle = true;
        setTimeout(() => {
          this.isInvaildTitle = false;
        }, 500);
        return;
      } else if (!this.validationAttachedFile) {
        this.$refs["attach"].$el.scrollIntoView();
        this.isInvaildFile = true;
        setTimeout(() => {
          this.isInvaildFile = false;
        }, 500);
        return;
      }

      try {
        // 기존 첨부파일이 삭제
        if (this.deletedOriginFiles.length > 0) {
          // 첨부파일 삭제
          let isOK = true;
          for (let i = 0; i < this.deletedOriginFiles.length; i++) {
            const result = await deleteAttachedFile({
              id: this.deletedOriginFiles[i].id,
              path: this.deletedOriginFiles[i].path,
            });
            if (!result.data.isOK) {
              isOK = false;
            }
          }
          if (!isOK) {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go("/");
          }
        }
        // 첨부파일이 없으면
        if (this.attachedFiles.length == 0) {
          const insertResult = await updatePost({
            postId: this.postId,
            title: this.title,
            content: this.content,
          });
          if (insertResult.data.isOK) {
            // 업로드 완료
            this.$emit('submitPost');
            alert("게시글이 수정되었습니다.");
            this.$router.push(`/lecture/${this.lectureId}/post/${this.postId}`);

          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go("/");
          }
        }
        // 첨부파일이 있으면
        else {
          // 1. 게시글 DB 수정
          const insertResult = await updatePost({
            postId: this.postId,
            title: this.title,
            content: this.content,
          });

          if (insertResult.data.isOK) {
            // 2. 첨부파일 저장
            let formData = new FormData();
            for (let i = 0; i < this.attachedFiles.length; i++) {
              formData.append("file", this.attachedFiles[i]);
            }
            const uploadResult = await uploadAttachedFile(formData);

            if (uploadResult.data.isOK) {
              // 3. 첨부파일 DB 삽입
              let isOK = true;
              for (let i = 0; i < this.attachedFiles.length; i++) {
                const result = await postAttachedFile({
                  postId: this.postId,
                  filepath: uploadResult.data.filepaths[i],
                  filename: uploadResult.data.originalNames[i],
                  filesize: uploadResult.data.filesizes[i],
                });
                if (!result.data.isOK) {
                  isOK = false;
                }
              }

              if (isOK) {
                this.$emit('submitPost');
                alert("게시글이 수정되었습니다.");
                this.$router.push(
                  `/lecture/${this.lectureId}/post/${this.postId}`
                );

              } else {
                alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
                this.$router.go("/");
              }
            } else {
              alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
              this.$router.go("/");
            }
          } else {
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go("/"); 
          }
        }
      } catch (err) {
        if (err.response) {
          if (err.response.data.error == 'Invalid file.') {
            alert("파일 형식을 확인해주세요.");
          } else {
            console.error(err.response.data.error);
            this.$router.go("/");
          }
        } else {
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");
        }
        
      }
    },
    unloadEvent(event) {
      // 사이트에서 나가시겠습니까?
      event.preventDefault();
      event.returnValue = '';
    },
  },
};
</script>

<style scoped>
.h-1em {
  height: 1.2em;
}
.font-small-gray {
  font-size: 0.8rem;
  color: #636363;
}
.font-small-gray ul {
  list-style-type: square;
}
.white-space-pre {
  white-space: pre-line;
}
.shake-anime {
  animation: shake 0.5s;
}
@keyframes shake {
  10%,
  90% {
    transform: translate3d(-1px, 0, 0);
  }
  20%,
  80% {
    transform: translate3d(2px, 0, 0);
  }
  30%,
  50%,
  70% {
    transform: translate3d(-4px, 0, 0);
  }
  40%,
  60% {
    transform: translate3d(4px, 0, 0);
  }
}
@media (max-width: 768px) {
  #editor-container {
    padding: 0 !important;
  }
  .font-small-gray ul {
    font-size: .9em;
    padding-left: 2em;
  }
  button {
    font-size: .9em;
  }
}
</style>
