import React from "react"
import TopHeader from "./TopHeader"
import Footer from "./Footer"
import "../css/Guide.css"
import "../css/Challenge.css"

function Guide() {
  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <TopHeader />
      <div className="">
        <div className="ChallText">먼저 저의 홈페이지 첫 화면입니다.</div>
        <img className="ChallImg" src="./img/GuideImg/mainpage.jpg" alt="err" />
        <div className="ChallText">
          Login을 눌러서 로그인을 진행해주시면 됩니다.
        </div>
        <img
          className="ChallImg"
          src="./img/GuideImg/Loginpage.jpg"
          alt="err"
        />
        <div className="ChallText">
          회원가입이 되어있지 않으시다면, 회원가입을 진행해주세요!
        </div>
        <img
          className="ChallImg"
          src="./img/GuideImg/SignUp-Page.jpg"
          alt="err"
        />
        <div className="ChallText">
          양식에 맞춰서 회원가입을 진행 후 로그인하신다면!
        </div>
        <div className="ChallText">
          아래와 같이 메인 페이지의 화면이 바뀌실 겁니다!
        </div>
        <img
          className="ChallImg"
          src="./img/GuideImg/mainpage to Login.jpg"
          alt="err"
        />
        <div className="ChallText">
          이제 저희 핏-시방의 모든 서비스를 이용할 준비가 되셨습니다!
        </div>
        <div className="ChallText">왼쪽 상단의 버튼을 눌러보세요!</div>
        <div className="ChallText">
          그럼 아래와 같은 기능들을 사용하실 수 있습니다!
        </div>
        <img className="ChallImg" src="./img/GuideImg/menubar.png" alt="err" />
        <div className="ChallText">자 이제 운동을 하러 가볼까요?</div>
        <div className="ChallText">
          메뉴에 Exercise를 누르시면 다음과 같은 화면을 보실 수 있습니다.
        </div>
        <img
          className="ChallImg"
          src="./img/GuideImg/exercisepage.jpg"
          alt="err"
        />
        <div className="ChallText">그리고 본인이 원하는 운동을 누르시면!</div>
        <div className="ChallText">
          약간의 로딩 후 다음과 같은 페이지가 뜨실겁니다!
        </div>
        <img
          className="ChallImg"
          src="./img/GuideImg/exercise page.jpg"
          alt="err"
        />
        <div className="ChallText">
          위의 페이지에서 열심히 운동을 진행하신 후
        </div>
        <div className="ChallText">
          운동 종료를 누르신다면 운동결과를 아래와 같이 확인하실 수 있습니다.
        </div>
        <img
          className="ChallImg"
          src="./img/GuideImg/result page.jpg"
          alt="err"
        />
        <div className="ChallText">자 이제 함께 운동을하러 가보실까요?</div>
        <div className="ChallText">
          저희와 함께 멎진 몸을 만들어보아요! 화이팅!
        </div>
      </div>
      <Footer />
    </div>
  )
}

export default Guide
