<template>
  <div>
    <list-item></list-item>
  </div>
</template>

<script>
import ListItem from "../components/ListItem.vue";
// import bus from "../utils/bus";
import ListMixin from "../mixins/ListMixin";
export default {
  components: {
    ListItem,
  },
  mixins: [ListMixin],
  // created() {
  //   bus.$emit("start:spinner");
  //   setTimeout(() => {
  //     this.$store
  //       .dispatch("FETCH_NEWS")
  //       .then(() => {
  //         console.log("fetched");
  //         bus.$emit("end:spinner");
  //       })
  //       .catch((error) => {
  //         console.log(error);
  //       });
  //     bus.$emit("end:spinner");
  //   }, 3000);
  // },
};
</script>
