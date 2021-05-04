import React from "react"
import "../css/Exercise.css"
import { Route, Link, withRouter } from "react-router-dom"

function ExerciseSection() {
  return (
    <div className="mainLayout">
      <div className="rutin_page">
        <div className="rutin_pageFlex">
          <div className="rutin_pageInfo">
            <div className="rutin__img">
              <div className="rutin__wrap">
                <img src="/img/plank.gif" alt="health__image" />
                {/* <p>플랭크</p> */}
              </div>
              <div className="rutin__wrap">
                <Link to="/Squat">
                  <img src="/img/squat.gif" alt="health__image" />
                  {/* <p>25분 칼로리 버닝! 타바타 클래스</p> */}
                </Link>
              </div>
              <div className="rutin__wrap">
                <img src="/img/warrior.png" alt="health__image" />
              </div>
              <div className="rutin__wrap">
                <img src="/img/good-mornings.gif" alt="health__image" />
              </div>
              <div className="rutin__wrap">
                <img src="/img/lunges.gif" alt="health__image" />
              </div>
              <div className="rutin__wrap">
                <img src="/img/hammer-curls.gif" alt="health__image" />
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ExerciseSection
