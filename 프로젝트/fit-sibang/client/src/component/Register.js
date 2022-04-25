import React, { useState } from 'react'
import Avatar from '@material-ui/core/Avatar'
import Button from '@material-ui/core/Button'
import CssBaseline from '@material-ui/core/CssBaseline'
import TextField from '@material-ui/core/TextField'

import Link from '@material-ui/core/Link'
import Grid from '@material-ui/core/Grid'
import Box from '@material-ui/core/Box'
import LockOutlinedIcon from '@material-ui/icons/LockOutlined'
import Typography from '@material-ui/core/Typography'
import { makeStyles } from '@material-ui/core/styles'
import Container from '@material-ui/core/Container'
import { useDispatch } from 'react-redux'
import { registerUser } from '../_action/user_action'
import { withRouter } from 'react-router-dom'
import Select from '@material-ui/core/Select'
import MenuItem from '@material-ui/core/MenuItem'
import FormControl from '@material-ui/core/FormControl'
import InputLabel from '@material-ui/core/InputLabel'
import clsx from 'clsx'
import Radio from '@material-ui/core/Radio'
import RadioGroup from '@material-ui/core/RadioGroup'
import FormControlLabel from '@material-ui/core/FormControlLabel'

//timezone
const moment = require('moment')
var today = moment().format('YYYY-MM-DD HH:mm:ss')

function Copyright() {
  // 로그인 Back-End 부분
  // const dispatch = useDispatch();

  return (
    <Typography variant="body2" color="textSecondary" align="center">
      {'Copyright © '}
      <Link color="inherit" href="https://material-ui.com/">
        Your Website
      </Link>{' '}
      {new Date().getFullYear()}
      {'.'}
    </Typography>
  )
}

const useStyles = makeStyles((theme) => ({
  paper: {
    marginTop: theme.spacing(8),
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
  },
  avatar: {
    margin: theme.spacing(1),
    backgroundColor: theme.palette.secondary.main,
  },
  form: {
    width: '100%', // Fix IE 11 issue.
    marginTop: theme.spacing(3),
  },
  submit: {
    margin: theme.spacing(3, 0, 2),
  },
  selectEmpty: {
    marginTop: theme.spacing(2),
  },
  formControl: {
    margin: theme.spacing(1),
    minWidth: 396,
  },
  Select: {
    fontSize: 10,
  },
  root: {
    '&:hover': {
      backgroundColor: 'transparent',
    },
  },
  icon: {
    borderRadius: '50%',
    width: 16,
    height: 16,
    boxShadow:
      'inset 0 0 0 1px rgba(16,22,26,.2), inset 0 -1px 0 rgba(16,22,26,.1)',
    backgroundColor: '#f5f8fa',
    backgroundImage:
      'linear-gradient(180deg,hsla(0,0%,100%,.8),hsla(0,0%,100%,0))',
    '$root.Mui-focusVisible &': {
      outline: '2px auto rgba(19,124,189,.6)',
      outlineOffset: 2,
    },
    'input:hover ~ &': {
      backgroundColor: '#ebf1f5',
    },
    'input:disabled ~ &': {
      boxShadow: 'none',
      background: 'rgba(206,217,224,.5)',
    },
  },
  checkedIcon: {
    backgroundColor: '#137cbd',
    backgroundImage:
      'linear-gradient(180deg,hsla(0,0%,100%,.1),hsla(0,0%,100%,0))',
    '&:before': {
      display: 'block',
      width: 16,
      height: 16,
      backgroundImage: 'radial-gradient(#fff,#fff 28%,transparent 32%)',
      content: '""',
    },
    'input:hover ~ &': {
      backgroundColor: '#106ba3',
    },
  },
}))

function SignUp(props) {
  const classes = useStyles()

  const dispatch = useDispatch()
  const [userName, setUserName] = useState('')
  const [password, setPassword] = useState('')
  const [re_password, setRePassword] = useState('')
  const [height, setHeight] = useState('')
  const [weight, setWeight] = useState('')
  const [address, setAddress] = useState('')
  const [gender, setGender] = useState('')

  const onGenderHandler = (event) => {
    setGender(event.target.value)
  }
  const onAddressHandler = (event) => {
    setAddress(event.target.value)
  }

  const onHeightHandler = (event) => {
    setHeight(event.currentTarget.value)
  }
  const onWeightHandler = (event) => {
    setWeight(event.currentTarget.value)
  }

  const onUserNameHandler = (event) => {
    setUserName(event.currentTarget.value)
  }
  const onPasswordHandler = (event) => {
    setPassword(event.currentTarget.value)
  }
  const onRePasswordHandler = (event) => {
    setRePassword(event.currentTarget.value)
  }
  function StyledRadio(props) {
    const classes = useStyles()

    return (
      <Radio
        className={classes.root}
        disableRipple
        color="default"
        checkedIcon={
          <span className={clsx(classes.icon, classes.checkedIcon)} />
        }
        icon={<span className={classes.icon} />}
        {...props}
      />
    )
  }
  const onSubmitHandler = (event) => {
    event.preventDefault()
    if (password !== re_password) {
      return alert('비밀번호를 다시 확인해주세요.')
    }
    if (password.length < 6) {
      return alert('비밀번호는 6자리 이상으로 정해주세요.')
    }
    if (height === '') {
      return alert('키를 입력해주세요')
    }
    if (weight === '') {
      return alert('몸무게를 입력해주세요')
    }
    if (isNaN(Number(height))) {
      return alert('잘못된 키값입니다.')
    } else if (height < 0) {
      return alert('잘못된 키값입니다.')
    }
    if (isNaN(Number(weight))) {
      return alert('잘못된 몸무게값입니다.')
    } else if (weight < 0) {
      return alert('잘못된 몸무게값입니다.')
    }
    if (address === '') {
      return alert('거주지를 선택해주세요.')
    }
    if (gender === '') {
      return alert('성별을 선택해주세요.')
    }
    var gd
    if (gender === '남성') {
      gd = true
    } else {
      gd = false
    }

    let body = {
      userName: userName,
      password: password,
      height: height,
      weight: { weight_: weight, date: today },
      address: address,
      gender: gd,
    }

    dispatch(registerUser(body)).then((response) => {
      if (response.payload.success) {
        props.history.push('/login')
      } else {
        alert('이미 존재하는 아이디입니다.')
      }
    })
  }

  return (
    <Container component="main" maxWidth="xs">
      <CssBaseline />
      <div className={classes.paper}>
        <Avatar className={classes.avatar}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          Sign up
        </Typography>
        <form className={classes.form} noValidate onSubmit={onSubmitHandler}>
          <Grid container spacing={2}>
            <Grid item xs={12}>
              <TextField
                variant="outlined"
                required
                fullWidth
                id="id"
                label="아이디"
                name="id"
                autoComplete="id"
                value={userName}
                onChange={onUserNameHandler}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                variant="outlined"
                required
                fullWidth
                name="password"
                label="비밀번호"
                type="password"
                id="password"
                autoComplete="current-password"
                value={password}
                onChange={onPasswordHandler}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                variant="outlined"
                required
                fullWidth
                name="password"
                label="비밀번호 확인"
                type="password"
                id="re-password"
                autoComplete="current-password"
                value={re_password}
                onChange={onRePasswordHandler}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                variant="outlined"
                required
                fullWidth
                name="height"
                label="키"
                id="height"
                value={height}
                onChange={onHeightHandler}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                variant="outlined"
                required
                fullWidth
                name="weight"
                label="몸무게"
                id="weight"
                value={weight}
                onChange={onWeightHandler}
              />
            </Grid>
            <FormControl variant="outlined" className={classes.formControl}>
              <InputLabel id="demo-simple-select-outlined-label">
                거주지
              </InputLabel>
              <Select
                labelId="demo-simple-select-outlined-label"
                id="demo-simple-select-outlined"
                value={address}
                onChange={onAddressHandler}
                label="Age"
              >
                <MenuItem disabled value="">
                  <em>시/도 선택</em>
                </MenuItem>
                <MenuItem value={'서울특별시'}>서울특별시</MenuItem>
                <MenuItem value={'인천광역시'}>인천광역시</MenuItem>
                <MenuItem value={'대전광역시'}>대전광역시</MenuItem>
                <MenuItem value={'광주광역시'}>광주광역시</MenuItem>
                <MenuItem value={'대구광역시'}>대구광역시</MenuItem>
                <MenuItem value={'울산광역시'}>울산광역시</MenuItem>
                <MenuItem value={'부산광역시'}>부산광역시</MenuItem>
                <MenuItem value={'경기도'}>경기도</MenuItem>
                <MenuItem value={'강원도'}>강원도</MenuItem>
                <MenuItem value={'충청북도'}>충청북도</MenuItem>
                <MenuItem value={'충청남도'}>충청남도</MenuItem>
                <MenuItem value={'전라북도'}>전라북도</MenuItem>
                <MenuItem value={'전라남도'}>전라남도</MenuItem>
                <MenuItem value={'경상북도'}>경상북도</MenuItem>
                <MenuItem value={'경상남도'}>경상남도</MenuItem>
                <MenuItem value={'경상남도'}>경상남도</MenuItem>
                <MenuItem value={'제주도'}>제주도</MenuItem>
              </Select>
            </FormControl>
            <FormControl component="fieldset">
              <RadioGroup
                value={gender}
                aria-label="gender"
                name="customized-radios"
                onChange={onGenderHandler}
              >
                <FormControlLabel
                  value="남성"
                  control={<StyledRadio />}
                  label="남성"
                />
                <FormControlLabel
                  value="여성"
                  control={<StyledRadio />}
                  label="여성"
                />
              </RadioGroup>
            </FormControl>
          </Grid>

          <Button
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            className={classes.submit}
          >
            회원가입
          </Button>

          <Grid container justify="flex-end">
            <Grid item xs>
              <Link href="Login" variant="body2">
                로그인
              </Link>
            </Grid>
            <Grid item>
              <Link href="./" variant="body2">
                홈으로
              </Link>
            </Grid>
          </Grid>
        </form>
      </div>
      <Box mt={5}>
        <Copyright />
      </Box>
    </Container>
  )
}

export default withRouter(SignUp)
