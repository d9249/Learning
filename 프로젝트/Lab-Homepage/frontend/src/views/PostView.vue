<template>
  <div>
    <post-content :lectureId="lectureId" :postId="postId"></post-content>
    <post-comment :postId="postId"></post-comment>
  </div>
</template>

<script>
import PostContent from "../components/PostContent";
import PostComment from "../components/PostComment";

export default {
  name: "PostView",
  components: {
    PostContent,
    PostComment,
  },
  props: ["lectureId", "postId"],
};
</script>

<style scoped></style>
