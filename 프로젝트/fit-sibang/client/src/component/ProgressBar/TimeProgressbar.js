import React, { useState, useEffect } from "react"
import "../../css/ProgressBar.css"
import "../../css/exer_css/TimeExercise.css"

const ProgressBar = (props) => {
  // 시간카운트를 위한 progressbar 정의
  const [offset, setOffset] = useState(0)
  const { size, progress, strokeWidth, circleOneStroke, circleTwoStroke } =
    props
  const center = size / 2
  const radius = size / 2 - strokeWidth / 2
  const circumference = 2 * Math.PI * radius

  // 1초 간격으로 값을 1씩 감소시키며 progreesbar를 다시그림
  useEffect(() => {
    const progressOffset = ((60 - props.time) / 60) * circumference
    setOffset(progressOffset)
  }, [setOffset, props.time, circumference, offset])

  return (
    <div style={{ position: "absolute", top: "2%", left: "1%" }}>
      <svg className="circularChart" width={size} height={size}>
        <circle
          className="circularBg"
          stroke={circleOneStroke}
          cx={center}
          cy={center}
          r={radius}
          strokeWidth={strokeWidth}
        ></circle>
        <circle
          className="circle"
          stroke={circleTwoStroke}
          cx={center}
          cy={center}
          r={radius}
          strokeWidth={strokeWidth}
          strokeDasharray={circumference}
          strokeDashoffset={offset}
          transform={`rotate(-90, ${center}, ${center})`}
        ></circle>
        <text x={center} y={95} className="percentage">
          {props.time}
        </text>
      </svg>
    </div>
  )
}

export default ProgressBar
