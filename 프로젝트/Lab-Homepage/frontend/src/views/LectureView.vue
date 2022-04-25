<template>
  <div class="lectureview-wrapper">
    <ongoing-lecture></ongoing-lecture>
  </div>
</template>

<script>
import OngoingLecture from "../components/OngoingLecture";

export default {
  name: "LectureView",
  components: {
    OngoingLecture,
  },
};
</script>

<style scoped>
.lectureview-wrapper {
  min-height: 50vh;
}
</style>
