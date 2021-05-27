import React, { useState, useEffect } from "react"
import * as tmPose from "@teachablemachine/pose"
import "../../css/exer_css/TimeExercise.css"
import { withRouter } from "react-router-dom"
import Loader from "../Loader"
import WebcamCapture from "../WebcamCapture"
import ProgressBar from "../ProgressBar/TimeProgressbar"
import Modal from "react-modal"

import { myPage } from "../../_action/user_action"
import { useDispatch } from "react-redux"
import { addRecordTime } from "../../_action/exercise_action"
//timez
const moment = require("moment")
var today = moment().format("YYYY-MM-DD HH:mm:ss")

function Plank() {
  const [cam, setCam] = useState(false) // 캠 상태
  const [time, setTime] = useState(60) // 왼쪽 자세 시간
  const [timeModal, setTimeModal] = useState(false) // modal
  const [start, setStart] = useState(false)
  const [totalTime, setTotalTime] = useState('')
  // const [badgeModal, setbadgeModal] = useState(false) // 뱃지 획득
  // const [newRecordModal, setnewRecordModal] = useState(false) // 신기록
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

  useEffect(() => {
    let timer = setTimeout(() => {
      init()
    }, 1)
    return () => {
      clearTimeout(timer)
    }
  }, [])

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

  useEffect(() => {
    if (timeModal === true){
      dispatch(myPage()).then((Response) => {
        if (Response.payload.isAuth === false){
        
        } else {
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
            } else{
              alert("db 오류 발생 ...")
            }
          })
        }
      })
    }
  },[timeModal])
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
    // if (prediction[0].probability.toFixed(2) >= 1.0) {
    // }

    // for (let i = 0; i < maxPredictions; i++) {
    //   console.log(
    //     prediction[i].className + ":",
    //     prediction[i].probability.toFixed(2) * 100 + "%",
    //   )
    //   console.log("-------------------")
    drawPose(pose)
    // }
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
                <a className="Home" href="/">
                  홈
                </a>
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
