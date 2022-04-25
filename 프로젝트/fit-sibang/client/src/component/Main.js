import React, { useState ,useEffect} from "react"
import axios from "axios"
import { Link } from "react-router-dom"
import ExerciseData from "./ExerciseData"
import TopHeader from "./TopHeader"
import { myPage } from "../_action/user_action"
import { useDispatch } from "react-redux"

import "../css/Main.css"

let randomExer = Math.floor(Math.random() * 5);

function Main() {
  const [auth_login, setAuth_login] = useState(false)
  const [userName, setUserName] = useState("")
  const [imgPath, setImgPath] = useState("/img/bmi/none.png")
  const [bmi, setBMI] = useState(0)
  const [height, setHeight] = useState(0)
  const [weight, setWeight] = useState(0)
  const [myState, setMyState] = useState("")
  const [randomTrue, setRandomTrue] = useState(false)
  const [pathName,setPathName] = useState('플랭크')
  const [path,setPath] = useState('/Plank')
  const dispatch = useDispatch()
  axios.get(`/user/auth`).then((response) => {
    if (response.data.isAuth) {
      // 로그인한 상태
      let copyAuth = auth_login
      copyAuth = true
      setAuth_login(copyAuth)
      setUserName(response.data.userName)
    }
    // 로그인하지 않은 상태
    else {
      let copyAuth = auth_login
      copyAuth = false
      setAuth_login(copyAuth)
    }
  })

  

  if (auth_login === true) {
    dispatch(myPage()).then((response) => {
      response.payload.weight.sort(function (a, b) {
        return a.date < b.date ? -1 : a.date > b.date ? 1 : 0
      })
      setHeight((response.payload.height / 100).toFixed(2))
      setWeight(
        response.payload.weight[response.payload.weight.length - 1].weight_,
      )
      if (response.payload.weight.length > 0) {
        setBMI(
          (
            response.payload.weight[response.payload.weight.length - 1]
              .weight_ /
            ((response.payload.height / 100) * (response.payload.height / 100))
          ).toFixed(2),
        )
        if (0 < bmi && bmi < 18.5) {
          setImgPath("/img/bmi/1.png")
          setMyState("저체중")
        } else if (18.49 < bmi && bmi < 25) {
          setImgPath("/img/bmi/2.png")
          setMyState("표준")
        }
        if (24.9 < bmi && bmi < 30) {
          setImgPath("/img/bmi/3.png")
          setMyState("과체중")
        }
        if (29.9 < bmi && bmi < 35) {
          setImgPath("/img/bmi/4.png")
          setMyState("비만")
        }
        if (34.9 < bmi) {
          setImgPath("/img/bmi/5.png")
        }
        if (bmi === -1) {
          setImgPath("/img/bmi/all.png")
          setMyState("고도비만")
        }
      } else {
        setBMI(-1)
      }
    })
  }

  
  useEffect(() => {
    if(randomTrue === false){
      setRandomTrue(true)
      if(randomExer === 0){
        setPathName('홈핏:플랭크')
        setPath('/Plank')
      }
      if(randomExer === 1){
        setPathName('요가:전사자세')
          setPath('/Warrior')
      }
      if(randomExer === 2){
        setPathName('홈핏:스쿼트')
        setPath('/Squat')
      }
      if(randomExer === 3){
        setPathName('홈핏:런지')
          setPath('/Lunge')
      }
      if(randomExer === 4){
        setPathName('홈핏:해머컬')
        setPath('/Hammercurl')
      }
    }
  }, [randomTrue])


  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        backgroundColor: "#fafafa",
      }}
    >
      <TopHeader />
      {/* 로그인한 유저 / 로그인 하지 않은 유저 구분 */}
      {auth_login === true ? (
        <div>
          <div className="eeeeFlex">
            <div>
              <span>나의 누적 운동량</span>
            </div>
            <span>
              <ExerciseData/>
            </span>
          </div>
          <div className="eeeeFlex">
            <p style={{ fontSize: "14px", marginBottom: "10px" }}>
              나의 Body Mass Index(BMI)
            </p>
            <p className="bmiWeight">
              {weight}(kg) ÷ {height}x{height}(m) = {bmi}(bmi){" "}
            </p>
            <p className="myState">현재 상태 : {myState} </p>
            <img src={imgPath} alt="badge" />
          </div>
        </div>
      ) : (
        <div className="eeeeFlex">
          <div>
            <span>로그인하기</span>
          </div>
          <span>
            <p>
              <Link to="login">Login</Link>
            </p>
          </span>
        </div>
      )}
      <div className="eeeeFlex">
        <span className="">핏시방's Pick</span>
        <span className="exercise"><a href={path}>{pathName}</a></span>
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
