import { ADD_RECORD, ADD_RECORD_TIME } from "../_action/types"

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = {}, action) {
  switch (action.type) {
    case ADD_RECORD:
      return { ...state, addRecordSuccess: action.payload }

    case ADD_RECORD_TIME:
      return { ...state, addRecordTimeSuccess: action.payload}
    default:
      return state
  }
}
