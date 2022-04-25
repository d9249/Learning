<template>
  <div class="wrapper">
    <p>
      <slot name="icon"></slot>
      <!-- EX) <b-icon icon="images"> </b-icon> -->
      <slot name="title" class="title"></slot>
      <!-- EX) 최근 활동 -->
    </p>
    <p class="smallTitle">
      <slot name="smallTitle"></slot>
      <!-- EX) Recent activies -->
    </p>
    <slot name="additional"></slot>
    <!-- <b-button class="buut" variant="link">
      <a class="buut" href="/gallery">
        > more
      </a>
    </b-button> -->
    <hr />
  </div>
</template>

<script>
export default {};
</script>

<style scoped>
.b-icon {
  margin-right: 10px;
}
i {
  margin-right: 10px;
}
.smallTitle {
  padding-left: 10px;
  color: #808080;
  font-size: 2.3vh;
}
.wrapper {
  padding-top: 20px;
}
.title {
  padding-left: 10px;
}
div > p {
  font-weight: bold;
  font-size: 2.6vh;
  display: inline;
}
@media (max-width: 768px) {
  .wrapper {
    padding-top: 5px;
  }
}
</style>
