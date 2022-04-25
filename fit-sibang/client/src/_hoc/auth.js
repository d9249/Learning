
/* 로그인 여부에 따른 페이지 접근권한 설정 */

import { useEffect } from 'react'
import React from 'react'
import { useDispatch } from 'react-redux'
import { authUser } from '../_action/user_action'

// eslint-disable-next-line import/no-anonymous-default-export
export default function (SpecificComponent, option) {
  function AuthenticationCheck(props) {
    const dispatch = useDispatch()

    // option null => any
    // option true => only user
    // option false => only visitor
    useEffect(() => {
      dispatch(authUser()).then((response) => {
        //로그인 상태 아닐때
        if (!response.payload.isAuth) {
          //option이 true 이면 로그인 페이지로 (mypage)
          if (option) {
            props.history.push('/login')
          }
          //로그인 상태일떄
        } else {
          //option 이 false이면 메인페이지로 (login, register)
          if (!option) {
            props.history.push('/')
          }
        }
      })
    }, [])

    return <SpecificComponent />
  }
  return AuthenticationCheck
}
