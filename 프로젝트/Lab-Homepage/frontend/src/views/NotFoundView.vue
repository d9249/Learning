<template>
  <div id="not_found">
    <section class="Notfound_xedition-error">
      <h1>404</h1>
      <p>
        Oops, Sorry.
        <br />Page is not found!
      </p>
      <img src="/assets/error.png" width="340" height="400" />
    </section>
  </div>
</template>

<script>
export default {
  name: "NotFoundView",
  data() {
    return {
      msg: "페이지를 찾을 수 없습니다"
    };
  }
};
</script>

<style scoped>
.Notfound_xedition-error h1 {
  font-size: 4.5vh;
  font-weight: normal;
  color: #e02b2b;
  line-height: 4.5vh;
}
.Notfound_xedition-error p {
  font-size: 2vh;
  color: #444;
  line-height: 2.2vh;
  display: block;
  margin-block-start: 1em;
  margin-block-end: 1em;
  margin-inline-start: 0;
  margin-inline-end: 0;
  font-family: "Open Sans", sans-serif;
  text-align: center;
}
.Notfound_xedition-error {
  min-height: 80vh;
  padding: 10vh 0;
  background-color: #efefef;
  font-family: "Open Sans", sans-serif;
  text-align: center;
}
</style>
