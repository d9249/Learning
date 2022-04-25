import { useState } from "react"
import { sendMessage, isTyping } from "react-chat-engine"
import { SendOutlined, PictureOutlined } from "@ant-design/icons"

const MessageForm = (props) => {
  const [value, setValue] = useState("")
  const { chatId, creds } = props

  // 공백을 제거하고 현재 입력한 메시지의 길이가 0이상이라면 sendMessage
  const handleSubmit = (event) => {
    event.preventDefault()

    const text = value.trim()

    if (text.length > 0) sendMessage(creds, chatId, { text })

    setValue("")
  }

  // 메시지 입력을 위한 함수
  const handleChange = (event) => {
    setValue(event.target.value)
    isTyping(props, chatId)
  }

  //file upload를 위한 함수
  const handleUpload = (event) => {
    sendMessage(creds, chatId, { files: event.target.files, text: "" })
  }
  return (
    <form className="message-form" onSubmit={handleSubmit}>
      <input
        className="message-input"
        placeholder="메시지를 입력하세요..."
        value={value}
        onChange={handleChange}
        onSubmit={handleSubmit}
      />
      <label htmlFor="upload-button">
        <span className="image-button">
          <PictureOutlined className="picture-icon" />
        </span>
      </label>
      <input
        type="file"
        accept="video/mp4,video/mkv, video/x-m4v,video/*"
        multiple={true}
        id="upload-button"
        style={{ display: "none" }}
        onChange={handleUpload}
      />
      <button type="submit" className="send-button">
        <SendOutlined className="send-icon" />
      </button>
    </form>
  )
}

export default MessageForm
