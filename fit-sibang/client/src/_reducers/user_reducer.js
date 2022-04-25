import {
  LOGIN_USER,
  REGISTER_USER,
  AUTH_USER,
  MYPAGE_USER,
  ADD_WEIGHT,
  UPDATE_BADGE,
  GET_ALL_RECORD
} from "../_action/types"

// eslint-disable-next-line import/no-anonymous-default-export
export default function (state = {}, action) {
  switch (action.type) {
    case LOGIN_USER:
      return { ...state, loginSuccess: action.payload }
    case REGISTER_USER:
      return { ...state, registerSuccess: action.payload }
    case AUTH_USER:
      return { ...state, userData: action.payload }
    case MYPAGE_USER:
      return { ...state, userData: action.payload }
    case ADD_WEIGHT:
      return { ...state, addWeightSuccess: action.payload }
    case UPDATE_BADGE:
      return { ...state, updateBadgeSuccess: action.payload}
    case GET_ALL_RECORD:
      return { ...state, getAllRecordSuccess: action.payload}
    default:
      return state
  }
}
