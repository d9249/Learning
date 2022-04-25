import React, { useState, useEffect } from "react"
import "../../css/ProgressBar.css"
import "../../css/exer_css/CountExercise.css"

const ProgressBar = (props) => {
  // 카운트를 위한 progressbar 정의
  const [offset, setOffset] = useState(0)

  const { size, progress, strokeWidth, circleOneStroke, circleTwoStroke } =
    props

  const center = size / 2
  const radius = size / 2 - strokeWidth / 2
  const circumference = 2 * Math.PI * radius

  // 운동갯수가 1개씩 증가 할때마다 progressbar를 새로 그림
  useEffect(() => {
    const progressOffset = ((20 - props.count) / 20) * circumference
    setOffset(progressOffset)
  }, [setOffset, props.count, circumference, offset])

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
          {props.count}
        </text>
      </svg>
    </div>
  )
}

export default ProgressBar
