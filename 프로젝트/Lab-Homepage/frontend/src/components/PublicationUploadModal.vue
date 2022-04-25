<template>
  <b-modal id="upload-research-modal" title="논문 업로드" @ok="uploadPrevent">
    <b-form-group
      label="제목"
      label-for="titleInput"
      :invalid-feedback="titleInvalidFeedback"
      :state="researchTitleState"
    >
      <b-form-input
        id="titleInput"
        v-model="uploadedResearchTitle"
        :state="researchTitleState"
        :class="{ 'shake-anime': isInvalidTitle }"
      ></b-form-input>
    </b-form-group> 

    <b-form-group
      label="저자"
      label-for="authorInput"
    >
      <b-form-input
        id="authorInput"
        placeholder="예) 홍길동, 이순신"
        v-model="uploadedResearchAuthor"
      ></b-form-input>
    </b-form-group> 

    <b-form-group
      label="날짜"
      label-for="whenInput"
    >
      <b-form-input
        id="whenInput"
        placeholder="예) 2020-03-11"
        v-model="uploadedResearchWhen"
      ></b-form-input>
    </b-form-group>

    <b-form-group
      label="발행기관"
      label-for="whereInput"
    >
      <b-form-input
        id="whereInput"
        placeholder="예) 한국정보기술학회"
        v-model="uploadedResearchWhere"
      ></b-form-input>
    </b-form-group>

    <b-form-group
      label="유형"
      label-for="typeInput"
      description="International journal / International conference / Domestic conference / Domestic journal 중 1개 선택"
    >
      <b-form-input
        id="typeInput"
        placeholder="예) Domestic conference"
        v-model="uploadedResearchType"
      ></b-form-input>
    </b-form-group>

    <b-form-group
      label="링크"
      label-for="linkInput"
    >
      <b-form-input
        id="linkInput"
        placeholder="논문이 확인 가능한 링크"
        v-model="uploadedResearchLink"
      ></b-form-input>
    </b-form-group> 

    <label for="fileInput">논문 파일</label>
    <b-form-file
      id="fileInput"
      v-model="uploadedResearchFile"
      placeholder="Choose a file or drop it here..."
      drop-placeholder="Drop file here..."
    ></b-form-file>

  </b-modal>
</template>

<script>
import { uploadResearch, insertResearch } from '../../api/index';

export default {
  name: 'PublicationUploadModal',
  data() {
    return {
      uploadedResearchTitle: '',
      uploadedResearchAuthor: null,
      uploadedResearchWhen: null,
      uploadedResearchWhere: null,
      uploadedResearchLink: null,
      uploadedResearchType: null,
      uploadedResearchFile: null,
      isInvalidTitle: false,
    }
  },
  computed: {
    titleInvalidFeedback() {
      if (this.researchTitleState) {
        return '';
      } else {
        return '제목은 필수사항 입니다.';
      }
    },
    researchTitleState() {
      return this.uploadedResearchTitle != '';
    }
  },
  methods: {
    uploadPrevent(e) {
      e.preventDefault();
      this.onClickUploadOK();
    },
    onClickUploadOK() {
      // validation
      if (! this.researchTitleState) {
        this.isInvalidTitle = true;
        setTimeout(() => {
          this.isInvalidTitle = false;
        }, 500);
        return;
      }
      // 논문 등록
      if (this.uploadedResearchFile != null) {
        // 파일 업로드, 저장된 경로와 함께 디비 삽입
        let formData = new FormData();
        formData.append("file", this.uploadedResearchFile);
        uploadResearch(formData)
          .then(response => {
            if (response.data.isOK) {
              const payload = { 
                title: this.uploadedResearchTitle,
                author: this.uploadedResearchAuthor,
                when: this.uploadedResearchWhen,
                where: this.uploadedResearchWhere,
                filepath: response.data.filepath,
                type: this.uploadedResearchType,
                link: this.uploadedResearchLink,
              };
              insertResearch(payload)
                .then(response => {
                  if (response.data.isOK) {
                    alert("업로드 되었습니다.");
                    this.$router.go();
                  } else {
                    alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
                    this.$router.go("/");
                  }
                })
                .catch(err => {
                  if (err.response)   console.error(err.response.data.error);
                  alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
                  this.$router.go("/");
                });
            } else {
              alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
              this.$router.go("/");
            }
          })
          .catch(err => {
            if (err.response)   console.error(err.response.data.error);
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go("/");
          });
      } else {
        const payload = { 
          title: this.uploadedResearchTitle,
          author: this.uploadedResearchAuthor,
          when: this.uploadedResearchWhen,
          where: this.uploadedResearchWhere,
          filepath: '',
          type: this.uploadedResearchType,
          link: this.uploadedResearchLink,
        };
        insertResearch(payload)
          .then(response => {
            if (response.data.isOK) {
              alert("업로드 되었습니다.");
              this.$router.go();
            } else {
              alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
              this.$router.go("/");
            }
          })
          .catch(err => {
            if (err.response)   console.error(err.response.data.error);
            alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
            this.$router.go("/");
          });
      }
    },
  }
}
</script>

<style scoped>
.shake-anime {
  animation: shake 0.5s;
}
@keyframes shake {
  10%, 90% {
    transform: translate3d(-1px, 0, 0);
  }
  20%, 80% {
    transform: translate3d(2px, 0, 0);
  }
  30%, 50%, 70% {
    transform: translate3d(-4px, 0, 0);
  }
  40%, 60% {
    transform: translate3d(4px, 0, 0);
  }
}
</style>