<template>
  <div>
    
    <b-modal
      id="delete-post-modal"
      title="삭제"
      okVariant="danger"
      @ok="onClickDeleteOK"
      centered
    >
      게시글을 삭제하시겠습니까?
    </b-modal>
    
  </div>
</template>

<script>
import { deletePost, deleteAttachedFile } from '../../api/index';

export default {
  name: 'PostDeleteModal',
  props: ['postId', 'attachedFiles'],
  methods: {
    async onClickDeleteOK() {
      try {
        // 게시글 삭제
        const response = await deletePost({ postId: this.postId });
        
        if (response.data.isOK) {
          // 첨부파일 있으면 삭제
          if (this.attachedFiles.length != 0) {

            let isOK = true;
            for (let i = 0; i < this.attachedFiles.length; i++) {
              const result = await deleteAttachedFile({ 
                path: this.attachedFiles[i].filepath
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

          this.$emit("onDeletePost");
        } else {
          alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
          this.$router.go("/");
        } 
      } catch(err) {
        if (err.response)   console.error(err.response.data.error);
        alert("알 수 없는 에러가 발생했습니다. 다시 시도해주세요.");
        this.$router.go("/");
      }
    },
  }
}
</script>

<style>
</style>