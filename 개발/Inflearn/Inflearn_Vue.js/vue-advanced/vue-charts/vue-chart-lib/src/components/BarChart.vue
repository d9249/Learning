<template>
  <canvas ref="barChart" id="barChart" width="400" height="400"></canvas>
</template>

<script>
export default {
  porps: ["propsdata"],
  mounted() {
    // var ctx = document.getElementById("barChart");
    var barChart = new this.$_Chart(this.$refs.barChart, {
      type: "bar",
      data: {
        labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
        datasets: this.propsdata,
      },
      options: {
        scales: {
          yAxes: [
            {
              ticks: {
                beginAtZero: true,
              },
            },
          ],
        },
      },
    });
    console.log(barChart);
  },
};
</script>

<style></style>
