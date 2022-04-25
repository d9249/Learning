<template>
  <div>
    <common-header>
      <b-icon icon="columns-gap" slot="icon"></b-icon>
      <span slot="title">Gallery</span>
      <span slot="additional" v-if="GET_ROLE < 2">
        <b-button class="float-right font-size-13 px-2 ml-2 mt-1 no-outline" variant="danger" @click="onClickDeleteBtn">
          <b-icon icon="trash" class="m-0"></b-icon>
        </b-button>
        <b-button v-b-modal.upload-photo-modal class="float-right font-size-13 mt-1 px-2 no-outline" variant="info">
          <b-icon icon="cloud-arrow-up-fill" class="m-0"></b-icon>
        </b-button>
        <gallery-upload-modal></gallery-upload-modal>
      </span>
    </common-header>
    
     <b-card-group columns>
      <b-card 
        v-for="(photo, idx) in photos" 
        :key="'vfor' + idx" 
        class="border-none p-2" 
        :class="{ 'shake-item' : onClickDelete }"
        no-body>

        <b-icon
          id="delete-btn"
          v-b-modal.delete-photo-modal 
          icon="x" 
          variant="light" 
          class="rounded-circle bg-danger float-right no-outline" 
          font-scale="1.5" 
          v-if="onClickDelete" 
          @click="selectedItem = photo">
        </b-icon>

        <b-card-img-lazy 
          :src="baseURL + '/' +photo.filepath"
          class="rounded shadow"
          @error.native="onErrorImg">
        </b-card-img-lazy>  

      </b-card>
    </b-card-group>

    <b-modal id="delete-photo-modal" title="사진 삭제" okVariant="danger" @ok="onDeletePhotoOK" centered>
      정말 사진을 삭제하시겠습니까?
    </b-modal>

  </div>
</template>

<script>
import CommonHeader from './common/CommonHeader';
import GalleryUploadModal from './GalleryUploadModal';
import { getPhotos, deletePhoto, deletePost } from '../../api/index';
import { baseURL } from '../../api/axios.config';
import { mapGetters } from 'vuex';

export default {
  name: 'Photos',
  components: {
    CommonHeader,
    GalleryUploadModal,
  },
  data() {
    return {
      photos: [],
      imgProps: {
        center: true,
        fluidGrow: true,
        blank: true,
        blankColor: '#bbb',
      },
      onClickDelete: false,
      selectedItem: null,
      baseURL,
    }
  },
  created() {
    getPhotos()
      .then(response => {
        this.photos = response.data;
      })
      .catch(err => {
        if (err.response)   console.error(err.response.data.error);
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
      });
  },
  computed: {
    ...mapGetters(['GET_ROLE']),
  },
  methods: {
    onClickDeleteBtn() {
      this.onClickDelete = !this.onClickDelete
      this.selectedItem = null;
    },
    async onDeletePhotoOK() {
      try {
        // 1. 게시글 삭제
        const response = await deletePost({ postId: this.selectedItem.id });
        
        if (response.data.isOK) {
          // 2. 첨부파일 삭제
          const response = await deletePhoto({ path: this.selectedItem.filepath });
          
          if (response.data.isOK) {
            this.$router.go();
          }
        } else {
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.push("/");
        } 

      } catch(err) {
        if (err.response.status == 404) {
          alert("해당 사진을 찾을 수 없습니다. 다시 시도해주세요.");
          this.$router.go(); 
        }
        else {
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.push("/"); 
        }
      }

      this.selectedItem = null;

    },
    onErrorImg(e) {
      e.target.src = "assets/not-found-image.jpg";
    },
  }
};
</script>

<style>
.no-outline {
  outline: none !important;
  box-shadow: none !important;
}
</style>

<style scoped>
.border-none {
  border: none;
}
#delete-btn {
  transform: translateY(1.3rem) translateX(.7rem);
}
.rounded {
  border-radius: 2rem !important;
}
@media (min-width: 576px) {
  .card-columns {
    -moz-column-count: 2;
    column-count: 2;
    -moz-column-gap: 1rem;
    column-gap: 1rem;
    orphans: 1;
    widows: 1;
  }
}
@media (min-width: 992px) {
  .card-columns {
    -moz-column-count: 3;
    column-count: 3;
    -moz-column-gap: 1.25rem;
    column-gap: 1.25rem;
    orphans: 1;
    widows: 1;
  }
}
.icon-size-9 {
  font-size: .9rem;
}
.font-size-13 {
  font-size: 1.3vh
}

.shake-item {
  animation: shake 0.3s infinite;
}
@keyframes shake {
  0% {
    transform: rotate(-1deg);
	}
	50% {
    transform: rotate(1deg);
	}
}
</style>
