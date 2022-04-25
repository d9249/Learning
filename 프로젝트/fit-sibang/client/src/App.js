import React from "react"
import { Route } from "react-router-dom"

import Chat from "./component/chat/Chat"
import Home from "./component/Home"
import Mypage from "./component/Mypage"
import Exercise from "./component/Exercise"
import Guide from "./component/Guide"
import Login from "./component/Login"
import Register from "./component/Register"
import Challenge from "./component/Challenge"
import Products from "./component/Products"
import ExerciseGuide from "./component/ExerciseGuide"
import Squat from "./component/exercise/Squat"
import Warrior from "./component/exercise/Warrior"
import Plank from "./component/exercise/Plank"
import Lunge from "./component/exercise/Lunge"
import Hammercurl from "./component/exercise/Hammercurl"
import GoodMorning from "./component/exercise/GoodMorning"

import "./css/App.css"

//hoc
import Auth from "./_hoc/auth"

// option null => any
// option true => only user
// option false => only visitor
// Auth(page, option)
function App() {
  return (
    <div>
      <Route path="/Chat" exact component={Chat} />
      <Route path="/" exact component={Home} />
      <Route path="/mypage" exact component={Auth(Mypage, true)} />
      <Route path="/challenge" exact component={Challenge} />
      <Route path="/exercise" exact component={Exercise} />
      <Route path="/products" exact component={Products} />
      <Route path="/Guide" exact component={Guide} />
      <Route path="/login" exact component={Auth(Login, false)} />
      <Route path="/register" exact component={Auth(Register, false)} />
      <Route path="/Squat" exact component={Auth(Squat, true)} />
      <Route path="/Warrior" exact component={Auth(Warrior, true)} />
      <Route path="/Plank" exact component={Auth(Plank, true)} />
      <Route path="/Lunge" exact component={Auth(Lunge, true)} />
      <Route path="/Hammercurl" exact component={Auth(Hammercurl, true)} />
      <Route path="/GoodMorning" exact component={Auth(GoodMorning, true)} />
      <Route path="/ExerciseGuide" exact component={ExerciseGuide} />
    </div>
  )
}

export default App
