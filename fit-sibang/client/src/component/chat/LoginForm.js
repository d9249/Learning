import { useState } from "react"
import axios from "axios"

const projectID = "5c13d0fd-49f8-4447-aa5c-7754ad15f60c"
const LoginForm = () => {
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [error, setError] = useState("")
  // 임시 아이디 발급을 위한 변수
  const tempid = [
    "ENFJ",
    "ENFP",
    "ENTJ",
    "ENTP",
    "ESFJ",
    "ESTJ",
    "ESTP",
    "INFJ",
    "INFP",
    "INTJ",
    "ISFJ",
    "ISFP",
    "ISTJ",
    "ISTP",
    "ESFP",
    "INTP",
  ]

  // tempid중 랜덤으로 임시아이디 발급
  const createId = () => {
    let rand = parseInt(Math.random() * 16 + 1)
    alert(
      "회원님의 임시 아이디는 : " +
        tempid[rand] +
        "\n" +
        "비밀번호는 " +
        tempid[rand].toLowerCase() +
        "at",
    )
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    // chatengine의 계정이 존재하는지 확인
    const authObject = {
      "Project-ID": projectID,
      "User-Name": username,
      "User-Secret": password,
    }

    try {
      // username과 password를 chatengine에 보냄
      await axios.get("https://api.chatengine.io/chats", {
        headers: authObject,
      })

      // username 과 password가 존재한다면 로그인
      localStorage.setItem("username", username)
      localStorage.setItem("password", password)

      window.location.reload()
    } catch (error) {}
    // 그렇지 않다면 에러메시지를 출력
    setError("아이디와 비밀번호를 다시 확인해주세요")
  }

  return (
    <div className="chat-wrapper">
      <div className="form">
        <h1 className="chat-title">트레이너 피드백</h1>
        <form onSubmit={handleSubmit}>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="input"
            placeholder="Username"
            required
          />
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="input"
            placeholder="Password"
            required
          />
          <div align="center">
            <button type="submit" className="button">
              <span>Start chatting</span>
            </button>
            <button onClick={createId} className="button">
              <span>아이디 발급</span>
            </button>
          </div>
          {error}
        </form>
      </div>
    </div>
  )
}

export default LoginForm
