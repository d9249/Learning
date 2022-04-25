<template>
  <div>
    <main-hello></main-hello>
    <ongoing-lecture></ongoing-lecture>
    <photos-latest></photos-latest>
  </div>
</template>

<script>
import OngoingLecture from "../components/OngoingLecture";
import PhotosLatest from "../components/PhotosLatest.vue";
import MainHello from "../components/MainHello.vue";

export default {
  name: "MainView",
  components: {
    OngoingLecture,
    PhotosLatest,
    MainHello,
  },
};
</script>

<style></style>
