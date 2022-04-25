import React from "react"
import HashLoader from "react-spinners/HashLoader"

function Loader() {
  return (
    <div className="sweet-loading">
      <HashLoader color="#297ed4" size={250} />
    </div>
  )
}

export default Loader
