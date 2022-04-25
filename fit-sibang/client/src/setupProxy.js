const { createProxyMiddleware } = require("http-proxy-middleware")

// server 와 client의 연결 생성
module.exports = function (app) {
  app.use(
    "/user",
    createProxyMiddleware({
      target: "http://localhost:5000/",
      changeOrigin: true,
    })
  )

  app.use(
    "/exercise",
    createProxyMiddleware({
      target: "http://localhost:5000/",
      changeOrigin: true,
    })
  )
}

//`http://localhost:${process.env.PORT}/`