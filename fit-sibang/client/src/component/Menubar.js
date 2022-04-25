import React, { useState } from "react"
import { Link, withRouter } from "react-router-dom"
import axios from "axios"
import "../css/App.css"
import "../css/Menubar.css"
import * as HiIcons from "react-icons/hi"
import * as BiIcons from "react-icons/bi"
import * as fiIcons from "react-icons/fi"
import { IconContext } from "react-icons"
import { SidebarData } from "./SidebarData"

function Menubar(props) {
  let auth_login
  const [text, setText] = useState("Login")
  axios.get(`/user/auth`).then((response) => {
    // 로그인한 상태
    if (response.data.isAuth) {
      auth_login = true
      let textCopy = [...text]
      textCopy = "Logout"
      setText(textCopy)
    }
    // 로그인하지 않은 상태
    else {
      auth_login = false
      let textCopy = [...text]
      textCopy = "Login"
      setText(textCopy)
    }
  })
  const onClickHandler = () => {
    if (auth_login) {
      axios.get(`/user/logout`).then((response) => {
        if (response.data.success) {
          props.history.push("/login")
        } else {
          alert("Login 안했음")
        }
      })
    } else {
      props.history.push("/login")
    }
  }
  // sidebar를 나타내기 위한 변수
  const [sidebar, setSidebar] = useState(false)
  const showSidebar = () => setSidebar(!sidebar)

  return (
    <>
      <IconContext.Provider value={{ color: "#0066cc" }}>
        <div className="navbar">
          <Link to="#" className="menu-bars">
            <HiIcons.HiOutlineMenuAlt2 onClick={showSidebar} />
          </Link>
        </div>
        {/* sidebar가 true라면 sidebar를 나타내고 false라면 나타내지 않음*/}
        <nav className={sidebar ? "nav-menu active" : "nav-menu"}>
          {/* 버거메뉴틀 클릭하면 sidebar 상태변경*/}
          <ul className="nav-menu-items" onClick={showSidebar}>
            <li className="navbar-toggle">
              <Link to="#" className="menu-bars">
                <BiIcons.BiLogInCircle />
              </Link>
            </li>
            {/* 반복문을 통해 sidebardata.js의 데이터들을 출력 */}
            {SidebarData.map((item, index) => {
              return (
                <li key={index} className={item.cName}>
                  <Link to={item.path}>
                    {item.icon}
                    <span>{item.title}</span>
                  </Link>
                </li>
              )
            })}
            <div className="text">
              <li className="text" onClick={onClickHandler}>
                <div className="">
                  <fiIcons.FiLogIn />
                  <span>{text}</span>
                </div>
              </li>
            </div>
          </ul>
        </nav>
      </IconContext.Provider>
    </>
  )
}

export default withRouter(Menubar)
