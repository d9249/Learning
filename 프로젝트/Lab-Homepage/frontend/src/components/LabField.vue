<template>
  <div class="pb-3" v-if="fields.length != 0">
    <common-header>
      <b-icon icon="card-list" slot="icon"></b-icon>
      <span slot="title">연구실 분야</span>
      <span slot="smallTitle">Laboratory field</span>
    </common-header>

    <div class="card-container" v-for="(field, idx) in fields" :key="idx">
      <div class="card">
        <div class="card-content">
          <div class="card-display">
            <i class="material-icons">{{ field.mainTitle }}</i>
            <h2>{{ field.subTitle }}</h2>
          </div>
          <div class="card-hover">
            <h2>{{ field.subTitle }}</h2>
            <p>{{ field.body }}</p>
          </div>
        </div>
        <div class="card-border"></div>
      </div>
    </div>
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";
import LabFieldsJson from "../assets/lab-fields.json";

export default {
  name: "LabField",
  components: {
    CommonHeader,
  },
  data() {
    return {
      ...LabFieldsJson,
    };
  },
};
</script>

<style scoped lang="scss">
$highlight: #5bc0eb;
$darkhighlight: #fde74c;

.card-container {
  flex: 300px;
  margin: 30px;

  .card {
    font-weight: bold;
    position: relative;
    width: 100%;

    .card-content {
      padding: 30px;
      width: 100%;
      height: 200px;
      border: 2px solid black;
      background: white;
      text-decoration: none;
      color: black;
      display: block;
      transition: 0.25s ease;

      &:hover {
        transform: translate(-20px, -20px);
        border-color: $highlight;

        .card-display {
          display: none;
        }

        .card-hover {
          display: block;
        }
      }

      .card-display {
        i {
          font-size: 2rem;
          margin-top: 100px;
        }
        h2 {
          font-size: 1.7rem;
        }
      }

      .card-hover {
        display: none;

        p {
          font-weight: 450;
          font-size: 1.5vh;
        }
        h2 {
          font-weight: bold;
          font-size: 2.5vh;
        }
      }
    }

    .card-border {
      position: absolute;
      width: 100%;
      height: 100%;
      left: 0;
      top: 0;
      border: 2px dashed black;
      z-index: -1;
    }
  }
}
@media screen and (max-width: 600px) {
  .card-container {
    margin: 10px;
    .card {
      .card-content {
        padding: 15px;
        &:hover {
          transform: translate(-20px, -20px);
          border-color: $highlight;

          .card-display {
            display: none;
          }

          .card-hover {
            display: block;
          }
        }
        .card-hover {
          p {
            font-size: 2.3vw;
            font-weight: 500;
          }
          h2 {
            font-weight: bold;
            font-size: 1rem;
          }
        }
        .card-display {
          i {
            font-size: 5vw;
            margin-top: 100px;
          }
          h2 {
            font-size: 4vw;
          }
        }
      }
    }
  }
}
</style>
