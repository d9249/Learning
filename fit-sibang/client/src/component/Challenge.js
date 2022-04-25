import React from 'react'
import '../css/Challenge.css'
import TopHeader from './TopHeader'
import Footer from './Footer'

export default function Challenge() {
  return (
    <div
      style={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      <TopHeader />
      <div className="">
        <div className="ChallText">Fit-sibang과 함께하는 첫번째 Challenge</div>
        <img className="ChallImg" src="./Challengeimg/Flank.jpg" alt="err" />
        <div className="ChallText">Fit-sibang과 함께하는 두번째 Challenge</div>
        <img className="ChallImg" src="./Challengeimg/Pull-up.jpg" alt="err" />
        <div className="ChallText">Fit-sibang과 함께하는 세번째 Challenge</div>
        <img className="ChallImg" src="./Challengeimg/Push-up.jpg" alt="err" />
        <div className="ChallText">Fit-sibang과 함께하는 네번째 Challenge</div>
        <img className="ChallImg" src="./Challengeimg/Squat.jpg" alt="err" />
      </div>
      <Footer />
    </div>
  )
}
