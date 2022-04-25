if (process.env.NODE_ENV === "production") {
  module.exports = require("./prod") // 배포 환경에서의 db uri
} else {
  module.exports = require("./dev") // 개발 환경에서의 db uri
}
