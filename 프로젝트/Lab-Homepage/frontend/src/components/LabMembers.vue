<template>
  <div>
    <common-header>
      <b-icon icon="person-square" slot="icon"></b-icon>
      <span slot="title">연구실 구성원</span>
      <span slot="smallTitle">Laboratory members</span>
    </common-header>

    <!-- 교수 container -->
    <div id="member-professor-container" v-if="members.professor.length != 0">
      <div class="role-title">
        Professor
      </div>
      <div
        class="card"
        v-for="(prof, idx) in members.professor"
        :key="idx + prof.name"
      >
        <div class="imgBx">
          <img :src="prof.img" @error="onErrorImg" />
        </div>
        <div class="details">
          <h2>
            {{ prof.name }}<br />
            <span>{{ prof.role }}</span>
          </h2>
        </div>
        <div class="content-wrapper">
          <div class="pb-2">
            <b-icon icon="person-fill" class="mr-2"></b-icon>
            {{ prof.name }}
          </div>
          <div>
            <b-icon icon="envelope" class="mr-2"></b-icon>
            {{ prof.email }}
          </div>
          <div class="pt-4" v-if="prof.additional.length != 0">
            <p>Research Interests</p>
            {{ prof.additional }}
          </div>
        </div>
      </div>
    </div>
    <!-- 교수 container -->

    <!-- 박사 연구생 container -->
    <div id="member-PhD-container" v-if="members.PhD.length != 0">
      <div class="role-title">
        PhD Students
      </div>

      <div
        class="card"
        v-for="(member, idx) in members.PhD"
        :key="idx + member.name"
      >
        <div class="imgBx">
          <img :src="member.img" @error="onErrorImg" />
        </div>
        <div class="details">
          <h2>
            {{ member.name }}<br />
            <span>{{ member.role }}</span>
          </h2>
        </div>
        <div class="content-wrapper">
          <div class="pb-2">
            <b-icon icon="person-fill" class="mr-2"></b-icon> {{ member.name }}
          </div>
          <b-icon icon="envelope" class="mr-2"></b-icon> {{ member.email }}
          <div class="pt-4" v-if="member.additional.length != 0">
            <p>Research Interests</p>
            {{ member.additional }}
          </div>
        </div>
      </div>
    </div>
    <!-- 박사 연구생 container -->

    <!-- 석사 연구생 container -->
    <div id="member-master-container" v-if="members.master.length != 0">
      <div class="role-title">
        Master Students
      </div>
      <div
        class="card"
        v-for="(member, idx) in members.master"
        :key="idx + member.name"
      >
        <div class="imgBx">
          <img :src="member.img" @error="onErrorImg" />
        </div>
        <div class="details">
          <h2>
            {{ member.name }}<br />
            <span>{{ member.role }}</span>
          </h2>
        </div>
        <div class="content-wrapper">
          <div class="pb-2">
            <b-icon icon="person-fill" class="mr-2"></b-icon> {{ member.name }}
          </div>
          <b-icon icon="envelope" class="mr-2"></b-icon> {{ member.email }}
          <div class="pt-4" v-if="member.additional.length != 0">
            <p>Research Interests</p>
            {{ member.additional }}
          </div>
        </div>
      </div>
    </div>
    <!-- 석사 연구생 container -->

    <!-- 학부 연구생 container -->
    <div
      id="member-undergraduate-container"
      v-if="members.undergraduate.length != 0"
    >
      <div class="role-title">
        Undergraduate Students
      </div>
      <div
        class="card"
        v-for="(member, idx) in members.undergraduate"
        :key="idx + member.name"
      >
        <div class="imgBx">
          <img :src="member.img" @error="onErrorImg" />
        </div>
        <div class="details">
          <h2>
            {{ member.name }}<br />
            <span>{{ member.role }}</span>
          </h2>
        </div>
        <div class="content-wrapper">
          <div class="pb-2">
            <b-icon icon="person-fill" class="mr-2"></b-icon> {{ member.name }}
          </div>
          <b-icon icon="envelope" class="mr-2"></b-icon> {{ member.email }}
          <div class="pt-4" v-if="member.additional.length != 0">
            <p>Research Interests</p>
            {{ member.additional }}
          </div>
        </div>
      </div>
    </div>
    <!-- 학부 연구생 container -->

    <hr class="my-5" />

    <!-- 박사 졸업 container -->
    <div id="graduate-PhD-container" v-if="graduates.PhD.length != 0">
      <div class="role-title">
        PhD
      </div>
      <div
        class="card d-inline-block"
        v-for="(member, idx) in graduates.PhD"
        :key="idx + member.name"
      >
        <div class="imgBx">
          <img :src="member.img" @error="onErrorImg" />
        </div>
        <div class="details graduate-card">
          <h2>
            {{ member.name }}<br />
            <!-- <span>{{ member.role }}</span> -->
          </h2>
        </div>
      </div>
    </div>
    <!-- 박사 졸업 container -->

    <!-- 석사 졸업 container -->
    <div id="graduate-master-container" v-if="graduates.master.length != 0">
      <div class="role-title">
        Master's Degree
      </div>
      <div
        class="card d-inline-block"
        v-for="(member, idx) in graduates.master"
        :key="idx + member.name"
      >
        <div class="imgBx">
          <img :src="member.img" @error="onErrorImg" />
        </div>
        <div class="details graduate-card">
          <h2>
            {{ member.name }}<br />
            <!-- <span>{{ member.additional }}</span> -->
          </h2>
        </div>
      </div>
    </div>
    <!-- 석사 졸업 container -->

    <!-- 학사 졸업 container -->
    <div id="graduate-bachelor-container" v-if="graduates.bachelor.length != 0">
      <div class="role-title">
        Bachelor's Degree
      </div>
      <div
        class="card d-inline-block"
        v-for="(member, idx) in graduates.bachelor"
        :key="idx + member.name"
      >
        <div class="imgBx">
          <img :src="member.img" @error="onErrorImg" />
        </div>
        <div class="details graduate-card">
          <h2>
            {{ member.name }}<br />
            <!-- <span>{{ member.additional }}</span> -->
          </h2>
        </div>
      </div>
    </div>
    <!-- 학사 졸업 container -->
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";
import MembersInfoJson from "../assets/member-info.json";

export default {
  name: "LabMembers",
  components: {
    CommonHeader,
  },
  data() {
    return {
      ...MembersInfoJson,
    };
  },
  methods: {
    onErrorImg(e) {
      e.target.src = "assets/not-found-user.png";
    },
  },
};
</script>

<style scoped>
.profile {
  font-weight: 500;
  font-size: 1.5rem;
}
.role-title {
  font-size: 2.5vh;
  font-weight: bold;
  padding-bottom: 15px;
}
.content-wrapper {
  display: inline-block;
  flex-direction: column;
  padding-left: 225px;
  white-space: nowrap;
  font-size: 1.3rem;
  font-weight: 450;
  padding-top: 2rem;
}
.card {
  position: relative;
  width: 200px;
  height: 250px;
  background: #fff;
  border-radius: 4px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
  /* display: inline-block; */
  flex-direction: row;
  margin-left: 20px;
  margin-bottom: 20px;
}
.card:before,
.card:after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  border-radius: 4px;
  background: #fff;
  transition: 0.5s;
  z-index: -1;
}
.card:hover:before {
  transform: rotate(20deg);
  box-shadow: 0 2px 20px rgba(0, 0, 0, 0.2);
}
.card:hover:after {
  transform: rotate(10deg);
  box-shadow: 0 2px 20px rgba(0, 0, 0, 0.2);
}
.card .imgBx {
  position: absolute;
  top: 10px;
  left: 10px;
  bottom: 10px;
  right: 10px;
  background: #222;
  transition: 0.5s;
  z-index: 1;
}

.card:hover .imgBx {
  bottom: 80px;
}

.card .imgBx img {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.card .details {
  position: absolute;
  left: 10px;
  right: 10px;
  bottom: 10px;
  height: 60px;
  text-align: center;
}

.card .details h2 {
  margin: 0;
  padding: 0;
  font-weight: 600;
  font-size: 20px;
  color: #777;
}

.card .details h2 span {
  font-weight: 500;
  font-size: 16px;
  color: #f38695;
  display: block;
  margin-top: 5px;
  text-transform: uppercase;
}

.graduate-card {
  height: 40px !important;
}
.graduate-card h2 {
  font-size: 22px !important;
  color: #4d638c !important;
}
@media (max-width: 768px) {
  .card {
    position: relative;
    width: 115px;
    height: 145px;
    background: #fff;
    border-radius: 4px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
    /* display: inline-block; */
    flex-direction: row;
    margin-left: 20px;
    margin-right: 30px;
    margin-bottom: 20px;
  }
  .card .details h2 {
    /* margin: 0; */
    /* padding: 0; */
    /* font-weight: 600; */
    font-size: 1.4vh;
    color: #777;
    /* text-transform: uppercase; */
  }
  .card .details h2 span {
    /* font-weight: 500; */
    font-size: 10px;
    /* color: #f38695; */
    /* display: block; */
    /* margin-top: 5px; */
  }
  .card .details {
    position: absolute;
    left: 10px;
    right: 10px;
    bottom: 0px;
    height: 40px;
    text-align: center;
  }
  .content-wrapper {
    display: inline-block;
    flex-direction: column;
    padding-left: 125px;
    white-space: nowrap;
    font-size: 2vh;
    font-weight: 450;
    padding-top: 0.8rem;
  }
  .card:hover .imgBx {
    bottom: 45px;
  }
  .graduate-card {
    height: 30px !important;
  }
  .graduate-card h2 {
    font-size: 16px !important;
  }
}
</style>
