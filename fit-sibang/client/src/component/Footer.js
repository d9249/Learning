import React from 'react'
import '../css/Footer.css'
import * as FaIcons from 'react-icons/fa'
import * as SiIcons from 'react-icons/si'
import * as GrIcons from 'react-icons/gr'

function footer() {
  return (
    <div className="footer">
      <div className="icons">
        <a href="https://github.com/KGU-Code-15/fit-sibang">
          <FaIcons.FaGithubSquare />
        </a>
        <a href="https://www.notion.so/Code-15-5cf2c2e66ce64994b1e4b06464d2c610">
          <SiIcons.SiNotion />
        </a>
        <a href="https://www.facebook.com/">
          <GrIcons.GrFacebook />
        </a>
        <a href="https://www.instagram.com/">
          <GrIcons.GrInstagram />
        </a>
      </div>
      <div className="copyright">
        <span>Copyright 2021. Code-15. all rights reserved.</span>
      </div>
    </div>
  )
}

export default footer
