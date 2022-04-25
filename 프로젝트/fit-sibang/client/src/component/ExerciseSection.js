import React from "react"
import "../css/Exercise.css"

function ExerciseSection() {
  return (
    <div className="mainLayout">
      <div className="rutin_page">
        <div className="rutin_pageFlex">
          <div className="rutin_pageInfo">
            <div className="rutin__img">
              <div className="rutin__wrap">
                <a href="/Plank">
                  <img src="/img/plank.gif" alt="health__image" />
                </a>
              </div>
              <div className="rutin__wrap">
                <a href="/Squat">
                  <img src="/img/squat.gif" alt="health__image" />
                </a>
              </div>
              <div className="rutin__wrap">
                <a href="/Warrior">
                  <img src="/img/warrior.png" alt="health__image" />
                </a>
              </div>
              <div className="rutin__wrap">
                <a href="#">
                  <img src="/img/good-mornings.gif" alt="health__image" />
                </a>
              </div>
              <div className="rutin__wrap">
                <a href="/Lunge">
                  <img src="/img/lunges.gif" alt="health__image" />
                </a>
              </div>
              <div className="rutin__wrap">
                <a href="/Hammercurl">
                  <img src="/img/hammer-curls.gif" alt="health__image" />
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ExerciseSection
