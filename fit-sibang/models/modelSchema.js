const mongoose = require("mongoose")
const bcrypt = require("bcrypt") // 암호화
const saltRounds = 10
const jwt = require("jsonwebtoken")

//time
const moment = require("moment")
var date = moment().format("YYYY-MM-DD HH:mm:ss")

//user schema
const userSchema = mongoose.Schema({
  userName: {
    type: String,
    maxlength: 50,
    unique: true,
    required: true,
  },
  password: {
    type: String,
    minlength: 6,
    required: true,
  },
  address: {
    type: String,
    maxlength: 50,
    default: "",
  },
  age: {
    type: Number,
    default: 0,
  },
  height: {
    type: Number,
    default: 0.0,
  },
  weight: [
    {
      weight_: Number,
      date: String,
    },
  ],
  gender: {
    type: Boolean, // true: 남 false : 여
  },
  token: {
    type: String,
  },
  tokenExp: {
    type: Number,
  },
  badge: {
    babySqarter:{
      type:Boolean,
      default:false,
    },
    horseLeg: {
      type:Boolean,
      default:false,
    },
    gmBadge1: {
      type:Boolean,
      default:false,
    },
    gmBadge2: {
      type:Boolean,
      default:false,
    },
    hmBadge: {
      type:Boolean,
      default:false,
    },
    lgBadge1:{
      type:Boolean,
      default:false,
    },
    plBadge1:{
      type:Boolean,
      default:false,
    },
    plBadge2:{
      type:Boolean,
      default:false,
    },
  }
})

//exercise record
const recordSchema = mongoose.Schema({
  user: {
    type: mongoose.Schema.Types.ObjectId,
    ref: "User",
  },
  exercise: {
    type: String,
  },
  when: {
    type: String,
  },
  countOrTime: {
    //true: 카운트, false: 시간
    type: Boolean,
  },
  count: {
    type: Number,
  },
  time: {
    type: Number,
  },
  useKcal: {
    type: Number,
  },
})

// save함수의 전처리
userSchema.pre("save", function (next) {
  var user = this
  if (user.isModified("password")) {
    // 비밀번호 변경시의 암호화
    bcrypt.genSalt(saltRounds, function (err, salt) {
      if (err) {
        console.log(err)
        return next(err)
      }
      bcrypt.hash(user.password, salt, function (err, hash) {
        if (err) {
          console.log(err)
          return next(err)
        }
        user.password = hash
        next()
      })
    })
  } else {
    next()
  }
})

// login시 비밀번호 확인
userSchema.methods.comparePassword = function (plainPassword, cb) {
  bcrypt.compare(plainPassword, this.password, function (err, isMatch) {
    if (err) return cb(err)
    cb(null, isMatch)
  })
}

// login시 json webtoken 생성
userSchema.methods.generateToken = function (cb) {
  var user = this
  var token = jwt.sign(user._id.toHexString(), "LEESM")
  user.token = token
  user.save(function (err, user) {
    if (err) {
      console.log(err)
      return cb(err)
    }
    cb(null, user)
  })
}

// 서비스 이용시 auth 인증에 관한 함수
userSchema.statics.findByToken = function (token, cb) {
  var user = this

  jwt.verify(token, "LEESM", function (err, decoded) {
    user.findOne({ _id: decoded, token: token }, (err, user) => {
      if (err) return cb(err)
      cb(null, user)
    })
  })
}

// Add Daily Weight
userSchema.methods.updateWeight = function (info, err) {
  if (err) {
    return err
  }
  this.weight.push({ weight_: info.weight, date: info.date })
  this.save()
}

// update user infomation
userSchema.methods.updateUser = function (err, info) {
  if (err) {
    console.log(err)
  }
  this.address = info.address
  this.age = info.age
  this.height = info.height
  this.save()
}

const User = mongoose.model("User", userSchema)
const Record = mongoose.model("Record", recordSchema)

module.exports = { User, Record }
