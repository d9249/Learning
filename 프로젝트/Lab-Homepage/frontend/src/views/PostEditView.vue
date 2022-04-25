<template>
  <div>
    <post-edit :lectureId="this.$route.params.lectureId" :postId="this.$route.params.postId" @submitPost="onSubmit"></post-edit>
  </div>
</template>

<script>
import PostEdit from "../components/PostEdit";

export default {
  name: 'PostEditView',
  components: {
    PostEdit,
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
