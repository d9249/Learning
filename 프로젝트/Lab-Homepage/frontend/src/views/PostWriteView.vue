<template>
  <div>
    <post-write :lectureId="this.$route.params.lectureId" @submitPost="onSubmit"></post-write>
  </div>
</template>

<script>
import PostWrite from "../components/PostWrite";

export default {
  name: "PostWriteView",
  components: {
    PostWrite,
  },
  data() {
    return {
      canLeavePage: false,
    }
  },
  beforeRouteLeave(to, from, next) {
    if (this.canLeavePage)  next();
    else if (confirm('이 사이트에서 나가시겠습니까?\n변경사항이 저장되지 않을 수 있습니다.')) {
      next();
    } else {
      next(false);
    }
  },
  methods: {
    onSubmit() {
      this.canLeavePage = true;
    }
  }
};
</script>

<style scoped>
</style>
