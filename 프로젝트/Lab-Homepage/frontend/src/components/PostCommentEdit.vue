<template>
  <div>
    <b-form-textarea
      id="textarea-state"
      v-model="newComment"
      :state="newComment.length <= 3000"
      placeholder="Enter 3000 characters or less."
      rows="3"
    >
    </b-form-textarea>

    <div class="pt-1 pb-1 text2">
      {{ newComment.length }} / 3000
      <b-button variant="primary" class="float-right btn-sm">등록</b-button>
    </div>
  </div>
</template>

<script>
export default {};
</script>

<style></style>
