const express = require("express")
const app = express()
const path = require('path');

// 배포 환경과 개발 환경에 따른 포트번호 결정
const port = process.env.PORT||5000

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

// 배포 환경일 경우 리액트 정적 파일을 제공
if (process.env.NODE_ENV === "production") {
  app.use(express.static(path.join(__dirname, "client/build")));
}


// route
// get method
app.get("/", (req, res) => res.send("Hello World!zz"))

// client 에서 온 인증 요청을 처리
app.get("/user/auth", auth, (req, res) => {
  res.status(200).json({
    _id: req.user._id,
    userName: req.user.userName,
    isAuth: true,
  })
})

// client 에서 온 로그아웃 요청을 처리
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

// 로그인 된 유저 정보를 clientd에 전송
app.get("/user/mypage", auth, (req, res) => {
  return res.status(200).json(req.user)
})


/* post method */

// client 에서 받은 회원가입 정보를 암호화 하여 db에 등록
app.post("/user/register", (req, res) => {
  const user = new User(req.body) // body-parser를 이용해 request를 json형식으로 받음
  user.save((err, userInfo) => {
    if (err) return res.json({ success: false, err })
    return res.status(200).json({
      success: true,
    })
  })
})

// client 에서 받은 로그인 정보를 암호화 하여 db정보와 비교 후 쿠키토큰을 발급하여 로그인 처리
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
        if (err) {
          return res.status(400).send(err)
        }
        res.cookie("x_auth", user.token).status(200).json({
          success: true,
          userId: user._id,
        })
      })
    })
  })
})

// 유저의 몸무게 정보 객체를 추가
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

// 유저정보 수정 요청을 처리
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

// 뱃지 획득 시 뱃지 관련 db 처리
app.post("/user/updateBadge", (req,res) => {
  if (Object.keys(req.body).length === 0) {
    return res.status(404).json({
      success: false,
      mesage: "don't have object",
      message: "success user badge update",
    })
  }
  
  let badgeName = req.body.badge
  
  switch(badgeName){
    case "babySqarter":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        if(!user){
          console.log("cant find user")
        }
        user.badge.babySqarter = true
        user.save()
      })
      return res.status(200).json({success: true})
    case "horseLeg":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        user.badge.horseLeg = true
        user.save()
      })
      return res.status(200).json({success: true})
    case "gmBadge1":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        user.badge.gmBadge1 = true
        user.save()
      })
      return res.status(200).json({success: true})
    case "gmBadge2":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        user.badge.gmBadge2 = true
        user.save()
      })
      return res.status(200).json({success: true})
    case "hmBadge":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        user.badge.hmBadge = true
        user.save()
      })
      return res.status(200).json({success: true})
    case "lgBadge1":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        user.badge.lgBadge1 = true
        user.save()
      })
      return res.status(200).json({success: true})
    case "plBadge1":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        if(!user){
          console.log("cant find user")
          return res.status(200).json({success: false})
        }
        user.badge.plBadge1 = true
        user.save()
      })
      return res.status(200).json({success: true})
    case "plBadge2":
      User.findOne( {userName: req.body.userName} , (err, user) => {
        user.badge.plBadge2 = true
        user.save()
      })
      return res.status(200).json({success: true})

    default :
      return res.status(200).json({success: false})
    
  }
})

// 운동 완료시 db에 운동 기록 저장 (count 관련 운동)
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

// 운동 완료시 db에 운동 기록 저장 (time 관련 운동)
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
        totaltimeSec: totaltimeSec,
      })
    })
  })
})


// User의 모든 운동 정보를 client에 전송
app.post("/user/getAllRecord", (req,res) => {
  User.findOne({userName : req.body.userName}, (err,user)=>{
    Record.find({user: user}, (err, info) => {
      if(err){
        return res.status(200).json({
          success : false
        })
      }
     
      return res.status(200).json({
        success : true,
        info : info,
      })
    })
  })
})

app.get('*', (req, res) => {
  res.sendFile(path.join(__dirname+'/client/build/index.html'));
});



app.listen(port, () => console.log(`Backend be hosting... http://localhost:${port}/ `))
