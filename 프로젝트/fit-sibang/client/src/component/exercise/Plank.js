import React, { useState, useEffect } from "react"
import * as tmPose from "@teachablemachine/pose"
import "../../css/exer_css/TimeExercise.css"
import { withRouter } from "react-router-dom"
import Loader from "../Loader"
import WebcamCapture from "../WebcamCapture"
import ProgressBar from "../ProgressBar/TimeProgressbar"
import Modal from "react-modal"
import { myPage ,updateBadge} from "../../_action/user_action"
import { useDispatch } from "react-redux"
import { addRecordTime } from "../../_action/exercise_action"

// make today string format
const moment = require("moment")
var today = moment().format("YYYY-MM-DD HH:mm:ss")

function Plank() {
  const [cam, setCam] = useState(false) // 캠 상태
  const [time, setTime] = useState(60) // 왼쪽 자세 시간
  const [timeModal, setTimeModal] = useState(false) // modal
  const [start, setStart] = useState(false)
  const [totalTime, setTotalTime] = useState('')
  // about badge 
  const [plBadge1, setPlBadge1] = useState(false)
  const [plBadge2, setPlBadge2] = useState(false)
  const [userName, setUserName] = useState('')

  const tts = [
    "사진과 같은 자세를 취해주세요",
    "운동을 시작합니다.",
    "30초 남았어요!",
  ]
  const scale = 0.5 // 스켈레톤 점 크기

  const state = {
    size: 150,
    progress: time,
    strokeWidth: 15,
    circleOneStroke: "#d9edfe",
    circleTwoStroke: "#7ea9e1",
  }

  const dispatch = useDispatch()

  // timer 설정
  useEffect(() => {
    let timer = setTimeout(() => {
      init()
    }, 1)
    return () => {
      clearTimeout(timer)
    }
  }, [])

  // 운동시작 안내 tts 송출
  useEffect(() => {
    let time = setTimeout(() => {
      setCam(!cam)
      let audioTune = new Audio("/TTS/plank.mp3")
      audioTune.play()
    }, 10000)
    return () => {
      clearTimeout(time)
    }
  }, [])

  // 타이머를 보고 tts 송출
  useEffect(() => {
    if (start === true) {
      if (time === 30) {
        let audioTune = new Audio("/TTS/30s.mp3")
        audioTune.play()
      }

      if (time === 6) {
        let audioTune = new Audio("/TTS/countdown.mp3")
        audioTune.play()
      }

      if (time <= 0) {
        setTimeModal(!timeModal)
        return
      }

      if (timeModal === true) {
        return () => clearTimeout(timeout)
      }

      const timeout = setTimeout(() => setTime(time - 1), 1000)
      return () => clearTimeout(timeout)
    }
  }, [time, start])

  // 운동이 끝나면 결과를 Modal에 띄우고 server에 보냄
  useEffect(() => {
    if (timeModal === true){
      dispatch(myPage()).then((Response) => {
        if (Response.payload.isAuth === false){
        } else {
          setUserName(Response.payload.userName)
          const body = {
            userName: Response.payload.userName,
            exercise: "plank",
            countOrTime: false,
            time_ : 60 - time,
            useKcal : (60 - time) * 0.3,
            when: today
          }
          dispatch(addRecordTime(body)).then(response =>{
            if (response.payload.success){
              setTotalTime(response.payload.totaltime)
              if (response.payload.totaltimeSec > 59 && Response.payload.badge.plBadge1 === false){
                setPlBadge1(true)
              }
              if (response.payload.totaltimeSec > 119 && Response.payload.badge.plBadge2 === false){
                setPlBadge2(true)
              }
            } else{
              alert("db 오류 발생 ...")
            }
          })    
        }
      })
    }
  },[timeModal])

  // 뱃지 획득 조건에 부합한다면 뱃지 획득
  useEffect(() => {
    if(plBadge1 === true){
      const badge = {
        userName : userName,
        badge : "plBadge1"
      }
      dispatch(updateBadge(badge)).then(response => {
        
        if(response.payload.success){
          alert("뱃지획득!")
        }else{
          alert("err")
        }
   
      })
    }
  },[plBadge1])

  // 뱃지 획득 조건에 부합한다면 뱃지 획득
  useEffect(() => {
    if(plBadge2 === true){
      const badge = {
        userName : userName,
        badge : "plBadge2"
      }
      dispatch(updateBadge(badge)).then(response => {
        if(response.payload.success){
          alert("뱃지획득!")
        }else{
          alert("err")
        }
      })
    }
  },[plBadge2])


  const URL = "https://teachablemachine.withgoogle.com/models/MBENJ9eel/"
  let model, webcam, ctx, maxPredictions

  async function init() {
    const modelURL = URL + "model.json"
    const metadataURL = URL + "metadata.json"

    // load the model and metadata
    // Refer to tmPose.loadFromFiles() in the API to support files from a file picker
    model = await tmPose.load(modelURL, metadataURL)
    maxPredictions = model.getTotalClasses()

    // Convenience function to setup a webcam
    const flip = true // whether to flip the webcam
    webcam = new tmPose.Webcam(200, 200, flip) // width, height, flip
    await webcam.setup() // request access to the webcam
    webcam.play()
    window.requestAnimationFrame(loop)

    // append/get elements to the DOM
    const canvas = document.getElementById("canvas")
    canvas.width = 200
    canvas.height = 200
    ctx = canvas.getContext("2d")
    // labelContainer = document.getElementById("label-container")
    // for (let i = 0; i < maxPredictions; i++) {
    //   // and class labels
    //   labelContainer.appendChild(document.createElement("div"))
    // }
  }

  async function loop(timestamp) {
    webcam.update() // update the webcam frame
    await predict()
    window.requestAnimationFrame(loop)
  }

  function drawPose(pose) {
    ctx.drawImage(webcam.canvas, 0, 0)
    // draw the keypoints and skeleton
    if (pose) {
      const minPartConfidence = 0.5
      tmPose.drawKeypoints(pose.keypoints, minPartConfidence, ctx, scale)
      tmPose.drawSkeleton(pose.keypoints, minPartConfidence, ctx)
    }
  }

  async function predict() {
    const { pose, posenetOutput } = await model.estimatePose(webcam.canvas)
    const prediction = await model.predict(posenetOutput)
    drawPose(pose)
  }

  return (
    <>
      <div className={cam ? "display" : "displayNone"}>
        <div className="exerImg">
          <img src="/img/plank1.jpg" alt="" />
          <div className="tts">
            <WebcamCapture />
            {time === 60 ? <span>{tts[0]}</span> : null}
            {time <= 59 && time >= 40 ? <span>{tts[1]}</span> : null}
            {time <= 30 && time >= 27 ? <span>{tts[2]}</span> : null}
            <Modal isOpen={timeModal} className="exModal" ariaHideApp={false}>
              <div className="exermodalResult">
                <div className="exerResult">
                  <h2>운동 결과</h2>
                </div>
                <div className="exerCount">
                  <img src="img/health_count.png" alt="health_count" />
                  <p>
                    시간 : <span>{60 - time}</span>초
                  </p>
                </div>
                <div className="exertotalCount">
                  <img
                    src="img/health_total_count.png"
                    alt="health_total_count"
                  />
                  <p>
                    누적 시간 : <span>{totalTime}</span>
                  </p>
                </div>
                <div className="exerKcal">
                  <img src="img/health_kcal.png" alt="kcal" />
                  <p>
                    {60-time} x 0.3 kcal = <span>{((60 - time) * 0.3).toFixed(1)}</span>
                    kcal
                  </p>
                </div>
                {plBadge1 === true ? <div className="badge">
                    <h4>새로운 뱃지 획득!</h4> <br/>
                    <p>* 플랭크입문자 *</p> <br/>
                    <img src="img/bedge/pl.png" alt="pl" /><br/>
                    <p>획득조건 : 플랭크 누적 60초 이상</p>
                </div>:<div></div>}
                {plBadge2 === true ? <div className="badge">
                    <h4>새로운 뱃지 획득!</h4> <br/>
                    <p>* 어디서든 플랭크 *</p> <br/>
                    <img src="img/bedge/pl1.png" alt="pl" /><br/>
                    <p>획득조건 : 플랭크 누적 120초 이상</p>
                </div>:<div></div>}
                <div style ={{textAlign:"center"}}> 
                  <a className="Home" href="/">
                    홈으로
                  </a>
                </div>
                
              </div>
            </Modal>
          </div>
        </div>
        <div className="canvasCenter">
          <canvas id="canvas" />
          <div className="counter">
            <ProgressBar {...state} time={time} />
          </div>
          {start === true ? (
            <button
              onClick={() => {
                setTimeModal(!timeModal)
                // clearTimeout(timeout)
              }}
            >
              운동 종료
            </button>
          ) : (
            <button
              onClick={() => {
                setStart(!start)
              }}
            >
              운동 시작
            </button>
          )}

          <div className="hiddenImg">
            <img src="/img/transparentsPlank.png" alt="" />
            <WebcamCapture />
          </div>
        </div>
      </div>
      <div className={cam ? "LoaderNone" : "Loader"}>
        <Loader />
      </div>
    </>
  )
}

export default withRouter(Plank)
