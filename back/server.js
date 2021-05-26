const express = require("express")
const app = express()
const port = 5000

// body-parser
const bodyParser = require("body-parser")
app.use(bodyParser.urlencoded({ extended: true }))
app.use(bodyParser.json())

// cookie-parser
const cookieParser = require("cookie-parser")
app.use(cookieParser())

// Mongo DB init
const mongoose = require("mongoose")
const config = require("./config/key")
const db = mongoose.connection
mongoose
  .connect(config.mongoURI, {
    useNewUrlParser: true,
    useUnifiedTopology: true,
    useCreateIndex: true,
    useFindAndModify: false,
  })
  .then(() => console.log("몽고DB 연결됨 ..."))
  .catch((err) => console.log(err))

// DB models
const { User, Record } = require("./models/modelSchema")

// auth
const { auth } = require("./middleware/auth")

// route
// get method
app.get("/", (req, res) => res.send("Hello World!zz"))

app.get("/user/auth", auth, (req, res) => {
  res.status(200).json({
    _id: req.user._id,
    userName: req.user.userName,
    isAuth: true,
  })
})

app.get("/user/logout", auth, (req, res) => {
  User.findOneAndUpdate(
    {
      _id: req.user._id,
    },
    {
      token: "",
    },
    (err, user) => {
      if (err) return res.json({ success: false, err })
      return res.status(200).send({
        success: true,
      })
    }
  )
})

app.get("/user/mypage", auth, (req, res) => {
  return res.status(200).json(req.user)
})

//post method
app.post("/user/register", (req, res) => {
  const user = new User(req.body) // body-parser를 이용해 request를 json형식으로 받음
  user.save((err, userInfo) => {
    if (err) return res.json({ success: false, err })
    return res.status(200).json({
      success: true,
    })
  })
})
app.post("/user/login", (req, res) => {
  User.findOne({ userName: req.body.userName }, (err, user) => {
    if (!user) {
      return res.json({ success: false, message: "아이디가 없습니다." })
    }

    user.comparePassword(req.body.password, (err, isMatch) => {
      if (!isMatch) {
        return res.json({
          loginSuccess: false,
          message: "비밀번호가 틀렸습니다.",
        })
      }
      user.generateToken((err, user) => {
        if (err) return res.status(400).send(err)
        res.cookie("x_auth", user.token).status(200).json({
          success: true,
          userId: user._id,
        })
      })
    })
  })
})

app.post("/user/addWeight", (req, res) => {
  if (Object.keys(req.body).length === 0) {
    return res.status(404).json({
      success: false,
      message: "don't have object",
    })
  }

  User.findOne({ userName: req.body.userName }, (err, user) => {
    if (err) {
      return res.status(400).json({
        success: false,
      })
    }

    user.updateWeight(req.body)

    return res.status(200).json({
      success: true,
      message: "success add weight",
    })
  })
})

app.post("/user/updateUser", (req, res) => {
  if (Object.keys(req.body).length === 0) {
    return res.status(404).json({
      success: false,
      mesage: "don't have object",
      message: "success user information update",
    })
  }
  user.findOne({ userName: req.body.userName }, (err, user) => {
    user.updateUser(req.body)

    return res.status(200).json({
      success: true,
    })
  })
})

app.post("/exercise/record", (req, res) => {
  User.findOne({ userName: req.body.userName }, (err, user) => {
    const record = new Record({
      user: user,
      exercise: req.body.exercise,
      when: req.body.when,
      countOrTime: true,
      count: req.body.count_,
      useKcal: req.body.useKcal,
    })
    record.save((err, userInfo) => {
      if (err) return res.json({ success: false, err })
    })
    let totalCount = 0
    Record.find({ user: user, exercise: req.body.exercise }, (err, info) => {
      for (let i = 0; i < info.length; i++) {
        totalCount += info[i].count
      }
      totalCount += req.body.count_
      return res.status(200).json({
        success: true,
        totalCount: totalCount,
      })
    })
  })
})

app.post("/exercise/recordtime", (req, res) => {
  User.findOne({ userName: req.body.userName }, (err, user) => {
    const record = new Record({
      user: user,
      exercise: req.body.exercise,
      when: req.body.when,
      countOrTime: false,
      time: req.body.time_,
      useKcal: req.body.useKcal,
    })
    record.save((err, userInfo) => {
      if (err) return res.json({ success: false, err })
    })
    let totaltimeSec = 0
    let temp1 = 0
    let temp2 = 0
    let totaltime
    Record.find({ user: user, exercise: req.body.exercise }, (err, info) => {
      for (let i = 0; i < info.length; i++) {
        totaltimeSec += info[i].time
      }
      totaltimeSec += req.body.time_ 
      temp1 = parseInt(totaltimeSec/60)
      temp2 = totaltimeSec % 60
      totaltime = String(temp1) + '분 ' + String(temp2) + '초'
      return res.status(200).json({
        success: true,
        totaltime: totaltime,
      })
    })
  })
})

app.listen(port, () => console.log(`로컬호스트 연결 http://localhost:${port}/`))
