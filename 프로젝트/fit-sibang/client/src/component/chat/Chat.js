import { ChatEngine } from "react-chat-engine"

import LoginForm from "./LoginForm"

import ChatFeed from "./ChatFeed"

import "../../css/Chat.css"

const Chat = () => {
  if (!localStorage.getItem("username")) return <LoginForm />

  return (
    <ChatEngine
      height="100vh"
      projectID="5c13d0fd-49f8-4447-aa5c-7754ad15f60c"
      userName={localStorage.getItem("username")}
      userSecret={localStorage.getItem("password")}
      renderChatFeed={(chatAppProps) => <ChatFeed {...chatAppProps} />}
    />
  )
}

export default Chat
