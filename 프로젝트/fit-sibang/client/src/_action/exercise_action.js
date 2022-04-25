// axios about exercise function
// client와 server를 연결

import axios from "axios"
import { ADD_RECORD, ADD_RECORD_TIME} from "./types"

// exercise record about count : client -> server
export function addRecord(dataToSubmit) {
  const request = axios
    .post("/exercise/record", dataToSubmit)
    .then((response) => response.data)
  return {
    type: ADD_RECORD,
    payload: request,
  }
}

// exercise record about time : client -> server
export function addRecordTime(dataToSubmit){
  const request = axios.post("/exercise/recordtime", dataToSubmit).then(response => response.data)
  return{
    type: ADD_RECORD_TIME,
    payload: request,
  }
}
