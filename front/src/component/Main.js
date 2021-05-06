import React, { useState } from 'react'
import axios from 'axios'
import { Link } from 'react-router-dom'
import ExerciseData from './ExerciseData'
import TopHeader from './TopHeader'

import '../css/Main.css'

function Main() {
  const [auth_login, setAuth_login] = useState(false)

  axios.get(`/user/auth`).then((response) => {
    if (response.data.isAuth) {
      let copyAuth = auth_login
      copyAuth = true
      setAuth_login(copyAuth) // 현 상태 : login
    } else {
      let copyAuth = auth_login
      copyAuth = false
      setAuth_login(copyAuth) // 현 상태 : logout
    }
  })

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
      <div className="eeeeFlex">
        <div>
          <span>나의 하루 운동 양</span>
        </div>
        <span>
          {auth_login === true ? (
            <ExerciseData />
          ) : (
            <p>
              <Link to="login">Login</Link>
            </p>
          )}
        </span>
      </div>

      <div className="eeeeFlex">
        <span className="">오늘의 추천 운동</span>
        <span className="exercise">스쿼트</span>
      </div>

      <div className="eeeeFlex">
        <span>
          핏-시방이 처음인 당신을 위한 &emsp;
          <Link to="Guide">핏-시방 이용 가이드</Link>
          <br />
          <br />
        </span>
        <span>
          운동이 처음인 당신을 위한 &emsp;
          <Link to="ExerciseGuide">동기 부여 영상</Link>
        </span>
      </div>
    </div>
  )
}

export default Main
