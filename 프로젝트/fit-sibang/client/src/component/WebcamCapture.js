import { useRef, useState, useCallback } from "react"
import Webcam from "react-webcam"

const WebcamCapture = () => {
  // webcam과 녹화기능을 위한 변수
  const webcamRef = useRef(null)
  const mediaRecorderRef = useRef(null)
  const [capturing, setCapturing] = useState(false)
  const [recordedChunks, setRecordedChunks] = useState([])

  // capturing이 true 라면 녹화기능 실행
  // 이때 확장자는 webm
  const handleStartCaptureClick = useCallback(() => {
    setCapturing(true)
    mediaRecorderRef.current = new MediaRecorder(webcamRef.current.stream, {
      mimeType: "video/webm",
    })
    mediaRecorderRef.current.addEventListener(
      "dataavailable",
      handleDataAvailable,
    )
    mediaRecorderRef.current.start()
  }, [webcamRef, setCapturing, mediaRecorderRef])

  // webm의 사이즈가 0보다 크다면 계속해서 저장
  const handleDataAvailable = useCallback(
    ({ data }) => {
      if (data.size > 0) {
        setRecordedChunks((prev) => prev.concat(data))
      }
    },
    [setRecordedChunks],
  )

  // capturing이 false 라면 녹화중지
  const handleStopCaptureClick = useCallback(() => {
    mediaRecorderRef.current.stop()
    setCapturing(false)
  }, [mediaRecorderRef, webcamRef, setCapturing])

  // 녹화된 영상의 길이가 0보다 크다면 다운로드 창 생성
  const handleDownload = useCallback(() => {
    if (recordedChunks.length) {
      const blob = new Blob(recordedChunks, {
        type: "video/webm",
      })

      const url = URL.createObjectURL(blob)
      const a = document.createElement("a")
      document.body.appendChild(a)
      a.style = "display: none"
      a.href = url
      a.download = "react-webcam-stream-capture.webm"
      a.click()
      window.URL.revokeObjectURL(url)
      setRecordedChunks([])
    }
  }, [recordedChunks])

  return (
    <>
      <Webcam
        mirrored="true"
        audio={false}
        ref={webcamRef}
        style={{ width: "1px" }}
      />
      {/* 
        capturing이 true라면 녹화 중이므로 stop Caputure 
        capturing이 false라면 녹화중이 아니므로 start capture
        start버튼 클릭 시 녹화 시작 stop버튼 클릭 시 녹화 중지 후 download창 생성 
      */}
      {capturing ? (
        <button className="stopCapture" onClick={handleStopCaptureClick}>
          Stop Capture
        </button>
      ) : (
        <button className="startCapture" onClick={handleStartCaptureClick}>
          Start Capture
        </button>
      )}
      {/* 녹화된 데이터 크기가 0이상이면 download버튼 생성*/}
      {recordedChunks.length > 0 && (
        <button className="download" onClick={handleDownload}>
          Download
        </button>
      )}
    </>
  )
}

export default WebcamCapture
