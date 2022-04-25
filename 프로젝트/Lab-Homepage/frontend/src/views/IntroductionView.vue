<template>
  <div>
    <lab-introduction></lab-introduction>
    <lab-field></lab-field>
    <lab-members></lab-members>
  </div>
</template>

<script>
import LabField from "../components/LabField";
import LabIntroduction from "../components/LabIntroduction";
import LabMembers from "../components/LabMembers";

export default {
  name: "IntroductionView",
  components: {
    LabIntroduction,
    LabField,
    LabMembers,
  },
};
</script>

<style scoped>
</style>
