import React from "react"
import Youtube from "react-youtube"
import Footer from "./Footer"

import Main from "./Main"
import "../css/App.css"
import { withRouter } from "react-router-dom"

function Home() {
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
    <div className="">
      <div>
        <Main />
        <Youtube
          className="youtube"
          videoId="qHapWanGDc8"
          opts={opts}
          onReady={_onReady}
        />
        <Footer />
      </div>
    </div>
  )
}

export default withRouter(Home)
