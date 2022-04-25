const { User } = require("../models/modelSchema")

// auth확인을 위한 middleware
// 쿠키의 토큰과 db에 등록된 토큰을 비교하여 로그인 상태를 확인 후
// isAuth == false 이면 인증하지 않음
let auth = (req, res, next) => {
  let token = req.cookies.x_auth

  User.findByToken(token, (err, user) => {
    if (err) throw err
    if (!user) return res.json({ isAuth: false, error: true })
    req.token = token
    req.user = user
    next()
  })
}

module.exports = { auth }
