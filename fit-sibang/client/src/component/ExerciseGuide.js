import React from "react"
import "../css/ExerciseGuide.css"
import Youtube from "react-youtube"
// import { withRouter } from "react-router-dom"
import TopHeader from "./TopHeader"
import "../css/App.css"
import Footer from "./Footer"

export default function ExerciseGuide() {
  const opts = {
    width: "570",
    height: "400",
    playerVars: {
      autoplay: 0,
    },
  }

  //youtube 로드가 완료되고 API 호출을 받을 준비가 될 때마다 실행 플레이어가 준비되는 즉시 유튜브 영상 실행을 막음
  function _onReady(e) {
    e.target.pauseVideo()
  }

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
      <div className="ExerGuide">
        <Youtube
          className="youtube"
          videoId="PhMhgF3jKrg"
          opts={opts}
          onReady={_onReady}
        />

        <Youtube
          className="youtube"
          videoId="lb9bmRnA_t8"
          opts={opts}
          onReady={_onReady}
        />
        <Footer />
      </div>
    </div>
  )
}

// export default withRouter(ExerciseGuide)
