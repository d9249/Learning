<template>
  <div>
    <common-header>
      <b-icon icon="images" slot="icon"></b-icon>
      <span slot="title">최근 활동</span>
      <span slot="smallTitle">Recent activities</span>
      <b-button
        class="more-link"
        variant="link"
        slot="additional"
        to="/gallery"
      >
        more
        <b-icon icon="arrow-right-square" class="more-link-icon"></b-icon>
      </b-button>
    </common-header>

    <div v-if="loading" class="loading">
      <b-spinner label="Spinning"></b-spinner>
    </div>
    <div v-else-if="photos.length == 0" class="no-photo">
      사진이 없습니다.
    </div>
    <b-carousel
      v-else
      :interval="5000"
      controls
      indicators
      style="text-shadow: 1px 1px 2px #333;"
    >
      <b-carousel-slide
        v-for="photo in this.photos"
        v-bind:key="photo.filepath"
      >
        <template #img>
          <img
            class="m-auto d-block rounded"
            height="480px"
            :src="baseURL + '/' + photo.filepath"
            @error="onErrorImg"
          />
        </template>
      </b-carousel-slide>
    </b-carousel>
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";
import { getPhotoLatest } from "../../api/index";
import { baseURL } from "../../api/axios.config";

export default {
  name: "PhotosLatest",
  components: {
    CommonHeader,
  },
  data() {
    return {
      photos: [],
      loading: null,
      baseURL,
    };
  },
  created() {
    this.loading = true;
    getPhotoLatest()
      .then((response) => {
        this.photos = response.data;
        this.loading = false;
      })
      .catch((err) => {
        if (err.response) console.error(err.response.data.error);
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        this.$router.push("/");
      })
      .finally(() => {
        this.loading = false;
      });
  },
  methods: {
    onErrorImg(e) {
      e.target.src = "/assets/not-found-image.jpg";
    },
  },
};
</script>

<style scoped lang="scss">
.more-link {
  float: right;
  color: #181818;
  padding-right: 5px;
  display: inline-flex;
  align-items: center;
}
.more-link:hover:not(.active) {
  color: #808080;
  text-decoration: none;
}
.more-link-icon {
  padding-left: 5px;
  transform: translateY(5%);
}
.no-photo {
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
.rounded {
  border-radius: 2rem !important;
}
@media screen and (max-width: 600px) {
  .rounded {
    display: block;
    max-width: 100%;
    height: auto;
  }
}
</style>
