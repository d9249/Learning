<template>
  <div>
    <common-header>
      <b-icon icon="people-fill" slot="icon"></b-icon>
      <span slot="title">Contact us</span>
    </common-header>

    <div class="background">
      <div class="container">
        <div class="screen">
          <div class="screen-header">
            <div class="screen-header-left">
              <div class="screen-header-button maximizee"></div>
              <div class="screen-header-button maximize"></div>
              <div class="screen-header-button minimize"></div>
            </div>
            <div class="screen-header-right">
              <div class="screen-header-ellipsis"></div>
              <div class="screen-header-ellipsis"></div>
              <div class="screen-header-ellipsis"></div>
            </div>
          </div>
          <div class="screen-body">
            <div class="screen-body-item left pr-1">
              <div class="app-title">
                <span>Smart I.O.T Lab Information</span>
                <!-- <span></span> -->
              </div>
              <div class="app-contact">
                <b-icon icon="telephone-fill" class="mr-1"></b-icon>
                031-249-9662
              </div>
            </div>
            <div class="screen-body-item pl-1">
              <div class="app-form">
                <div class="app-form-group">
                  <span class="app-form-control">
                    <b-icon icon="geo-alt-fill" class="mr-1"></b-icon>
                    16227, 경기 수원시 영통구 광교산로 154-42 (이의동 산94-6)
                  </span>
                </div>
                <div class="app-form-group">
                  <span class="app-form-control">
                    <b-icon icon="house-fill" class="mr-1"></b-icon>
                    8강의동(육영관) 8506, 네트워크 산학협력 연구실
                  </span>
                </div>
                <div class="app-form-group">
                  <span class="app-form-control">
                    <b-icon icon="envelope-fill" class="mr-1"></b-icon>
                    ngkim@kyonggi.ac.kr
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import CommonHeader from "./common/CommonHeader";

export default {
  name: "ContactUs",
  components: {
    CommonHeader,
  },
};
</script>

<style scoped>
*,
*:before,
*:after {
  box-sizing: border-box;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

body,
button,
input {
  font-family: "Montserrat", sans-serif;
  font-weight: 700;
  letter-spacing: 1.4px;
}

.background {
  display: flex;
  min-height: 50vh;
}

.container {
  flex: 0 1 700px;
  margin: auto;
  padding: 10px;
}

.screen {
  position: relative;
  background: #3e3e3e;
  border-radius: 15px;
}

.screen:after {
  content: "";
  display: block;
  position: absolute;
  top: 0;
  left: 20px;
  right: 20px;
  bottom: 0;
  border-radius: 15px;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4);
  z-index: -1;
}

.screen-header {
  display: flex;
  align-items: center;
  padding: 10px 20px;
  background: #4d4d4f;
  border-top-left-radius: 15px;
  border-top-right-radius: 15px;
}

.screen-header-left {
  margin-right: auto;
}

.screen-header-button {
  display: inline-block;
  width: 8px;
  height: 8px;
  margin-right: 3px;
  border-radius: 8px;
  background: white;
}

.screen-header-button.maximizee {
  background: #ed1c6f;
}

.screen-header-button.maximize {
  background: #e8e925;
}

.screen-header-button.minimize {
  background: #74c54f;
}

.screen-header-right {
  display: flex;
}

.screen-header-ellipsis {
  width: 3px;
  height: 3px;
  margin-left: 2px;
  border-radius: 8px;
  background: #999;
}

.screen-body {
  display: flex;
}

.screen-body-item {
  flex: 1;
  padding: 2rem;
}
.screen-bosy-item2 {
  flex: 1;
  padding: 20px;
}

.screen-body-item.left {
  display: flex;
  flex-direction: column;
}

.app-title {
  display: flex;
  flex-direction: column;
  position: relative;
  color: #ea1d6f;
  font-size: 30px;
}

.app-title:after {
  content: "";
  display: block;
  position: absolute;
  left: 0;
  bottom: -10px;
  width: 200px;
  height: 4px;
  background: #ea1d6f;
}

.app-contact {
  margin: auto 0;
  font-size: 15px;
  color: white;
}

.app-form-group {
  margin-bottom: 15px;
}

.app-form-group.message {
  margin-top: 40px;
}

.app-form-group.buttons {
  margin-bottom: 0;
  text-align: right;
}

.app-form-control {
  width: 100%;
  padding: 10px 0;
  background: none;
  border: none;
  /* border-bottom: 1px solid #666; */
  color: #ddd;
  /* font-size: 14px; */
  /* text-transform: uppercase; */
  outline: none;
  transition: border-color 0.2s;
}

.app-form-control::placeholder {
  color: #666;
}

.app-form-control:focus {
  border-bottom-color: #ddd;
}

.app-form-button {
  background: none;
  border: none;
  color: #ea1d6f;
  font-size: 14px;
  cursor: pointer;
  outline: none;
}

.app-form-button:hover {
  color: #b9134f;
}

.credits {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
  color: #ffa4bd;
  font-family: "Roboto Condensed", sans-serif;
  font-size: 16px;
  font-weight: normal;
}

.credits-link {
  display: flex;
  align-items: center;
  color: #fff;
  font-weight: bold;
  text-decoration: none;
}

.dribbble {
  width: 20px;
  height: 20px;
  margin: 0 5px;
}

@media (max-width: 768px) {
  .screen-body {
    flex-direction: column;
  }

  .screen-body-item.left {
    margin-bottom: 10px;
  }

  .app-title {
    flex-direction: row;
    padding-bottom: 10px;
    font-size: 2.7vh;
  }

  .app-title span {
    margin-right: 12px;
  }

  .app-title:after {
    display: none;
  }

  .screen-body {
    padding: 35px;
  }

  .screen-body-item {
    padding: 0;
  }
  .background {
    font-size: 2vh;
    font-weight: 500;
  }
  .app-contact {
    font-size: 2vh;
  }
  #content-wrapper-main {
    padding-bottom: 0px;
  }
}
</style>
