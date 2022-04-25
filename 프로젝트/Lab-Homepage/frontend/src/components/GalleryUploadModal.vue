<template>
  <div>
    
    <b-modal
      id="upload-photo-modal"
      title="사진 업로드"
      @show="resetModal"
      @hidden="resetModal"
      @ok="uploadPrevent"
    >

      <label for="fileInput">사진</label>
      <b-form-file
        id="fileInput"
        ref="fileInput"
        v-model="uploadedPhotoFile"        
        :state="Boolean(uploadedPhotoFile)"
        :class="{ 'shake-anime': isInvalidFile }"
        accept="image/jpeg, image/png, image/gif"
        placeholder="Choose a file or drop it here..."
        drop-placeholder="Drop file here..."
        @change="onFileChange"
      ></b-form-file>
      <div id="preview">
        <img v-if="previewUrl" :src="previewUrl" />
      </div>

      <hr>

      <b-form-group
        label="제목"
        label-for="titleInput"
        :invalid-feedback="titleInvalidFeedback"
        :state="uploadedPhotoTitleState"
      >
        <b-form-input
          id="titleInput"
          ref="titleInput"
          placeholder="Enter a title for the picture"
          v-model="uploadedPhotoTitle"
          :state="uploadedPhotoTitleState"
          :class="{ 'shake-anime': isInvalidTitle }"
        ></b-form-input>
      </b-form-group>
    </b-modal>
    
  </div>
</template>

<script>
import { postPostGallery, uploadPhoto, postAttachedFile } from '../../api/index';

export default {
  name: 'GalleryUploadModal',
  data() {
    return {
      uploadedPhotoFile: null,
      uploadedPhotoTitle: '',
      previewUrl: '',
      isInvalidTitle: false,
      isInvalidFile: false,
    }
  },
  computed: {
    uploadedPhotoTitleState() {
      return this.uploadedPhotoTitle != '';
    },
    titleInvalidFeedback() {
      if (this.uploadedPhotoTitle == '') {
        return 'Please enter something...';
      } else {
        return '';
      }
    }
  },
  methods: {
    resetModal() {
      this.uploadedPhotoFile = null;
      this.uploadedPhotoTitle = '';
      this.previewUrl = '';
    },
    uploadPrevent(e) {
      e.preventDefault();
      this.handleSubmit();
    },
    async handleSubmit() {
      // Validation
      if (this.uploadedPhotoTitle == '') {
        this.isInvalidTitle = true;
        setTimeout(() => {
          this.isInvalidTitle = false;
        }, 500);
        return;
      } else if (! this.uploadedPhotoFile) {
        this.isInvalidFile = true;
        setTimeout(() => {
          this.isInvalidFile = false;
        }, 500);
        return;
      }

      try {
        // 사진 업로드
        // 1. 게시글 등록 
        const insertedResult = await postPostGallery({ title: this.uploadedPhotoTitle, content: '' });
        if (insertedResult.data.isOK) {
          // 2. 파일 업로드
          let formData = new FormData();
          formData.append("file", this.uploadedPhotoFile);
          const fileinfo = await uploadPhoto(formData);
          if (fileinfo.data.isOK) {
            // 3. 게시글 번호로 첨부파일 등록
            const result = await postAttachedFile({ 
              postId: insertedResult.data.insertId, 
              filepath: fileinfo.data.filepath, 
              filename: fileinfo.data.originalname });
            if (result.data.isOK) {
              alert("업로드 되었습니다.")
            }
          }
        }
        this.$router.go();

      } catch(err) {
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        this.$router.push("/"); 
      }

      // Hide the modal manually
      this.$nextTick(() => {
        this.$bvModal.hide('upload-photo-modal');
      });
    },
    onFileChange(e) {
      const file = e.target.files[0];
      if (file) {
        this.previewUrl = URL.createObjectURL(file);
      } else {
        this.previewUrl = '';
      }
    },
  }
}
</script>

<style>
#preview {
  display: flex;
  justify-content: center;
  align-items: center;
  padding-top: 1rem;
}
#preview img {
  max-width: 100%;
  max-height: 30rem;
}
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