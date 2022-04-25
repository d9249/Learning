// 내가 보낸 첨부파일의 크기가 0이상이라면 출력
const MyMessage = ({ message }) => {
  if (message?.attachments?.length > 0) {
    return (
      <video
        source
        controls
        src={message.attachments[0].file}
        alt="message-attachment"
        className="message-image"
        style={{ float: "right" }}
      />
    )
  }

  return (
    <div
      className="message"
      style={{
        float: "right",
        marginRight: "18px",
        color: "white",
        backgroundColor: "#3B2A50",
      }}
    >
      {message.text}
    </div>
  )
}

export default MyMessage
