import React from "react"

import ExerciseSection from "./ExerciseSection"
import TopHeader from "./TopHeader"
import Footer from "./Footer"

function Exercise() {
  return (
    <div
      style={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <TopHeader />
      <ExerciseSection />
      <Footer />
    </div>
  )
}

export default Exercise
