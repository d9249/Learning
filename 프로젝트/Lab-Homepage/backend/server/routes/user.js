import express from 'express';
import crypto from 'crypto';
import { querySync } from '../lib/mysqlConnectionPool';
import { verifyAccessToken, verifyForgottenToken } from '../lib/token';
import cryptoConfig from '../config/crypto.config';
const router = express.Router();

// !! REQUIRED AUTH
// request: NOTHING
// response: 사용자 정보
// GET /user
router.get('/', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const qGetUser = 'SELECT nickname, role FROM users WHERE id=?';
      const params = [decoded.id];

      const results = await querySync(qGetUser, params);
      if (results.length == 1) {
        res.status(200).send({
          isOK: true,
          nickname: results[0].nickname,
          role: results[0].role,
        });
      } else {
        res.status(200).send({ isOK: false });
      }
    }
  } catch (err) {
    next(err);
  }
});

// request: 사용자 아이디, 비밀번호, 이름, 닉네임, 학번
// response: 회원가입 데이터 삽입
// POST /user
router.post('/', async (req, res, next) => {
  try {
    const originPw = req.body.userPw;
    const salt = crypto.randomBytes(64).toString('base64');
    const hashedPw = crypto.pbkdf2Sync(originPw, salt, cryptoConfig.interation, cryptoConfig.length, cryptoConfig.hash).toString('base64');
  
    const qInsertUser = 'INSERT INTO users VALUES (?, ?, ?, ?, ?, ?, ?, ?, null, null)';
    const params = [null, req.body.userId, hashedPw, req.body.userName, req.body.userNickname, 2, salt, req.body.userStudentId];
    
    const result = await querySync(qInsertUser, params);
    res.status(200).send({ isOK: true });

  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: (헤더 속 토큰 정보 확인)
// response: 사용자 정보
// GET /user/info
router.get('/info', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const qGetUser = 'SELECT * FROM users WHERE id=?';
      const params = [decoded.id];

      const result = await querySync(qGetUser, params);
      res.status(200).send({
        isOK: true,
        userInfo: result[0],
      });
    }
  } catch (err) {
    next(err);
  }
});

// request: 중복 확인할 타겟(이메일, 닉네임), 값
// response: 확인 여부
// POST /user/duplication-check
router.post('/duplication-check', async (req, res, next) => {
  try {
    const qDupCheck = 'SELECT * FROM users WHERE ?? = ?;';
    const params = [req.body.target, req.body.data];
  
    const results = await querySync(qDupCheck, params);
    // 중복 없음
    if (results.length == 0) { 
      res.status(200).send({
        isOK: true,
      });
    // 중복 존재  
    } else {
      res.status(200).send({  
        isOK: false,
      });
    }
  } catch (err) {
    next(err);
  } 
});

// request: 토큰, 새로운 비밀번호
// response: update 결과
// POST /user/change-pw
router.post('/change-pw', async (req, res, next) => {
  try {
    const decoded = await verifyForgottenToken(req.body.token);
    const originPw = req.body.newPw;
    const salt = crypto.randomBytes(64).toString('base64');
    const hashedPw = crypto.pbkdf2Sync(originPw, salt, cryptoConfig.interation, cryptoConfig.length, cryptoConfig.hash).toString('base64');
  
    const qUpdatePw = "UPDATE users SET user_password=?, salt=? WHERE user_identification = ?; ";
    const params = [hashedPw, salt, decoded.email];
    const result = await querySync(qUpdatePw, params);
    res.status(200).send({ isOK: true });
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 닉네임, 이름, 학번
// response: update 결과
// POST /user/update
router.post('/update', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const qUpdateUser = "UPDATE users SET nickname=?, name=?, student_id=? WHERE id=?; ";
      const params = [req.body.nickname, req.body.name, req.body.studentId, decoded.id];
      const result = await querySync(qUpdateUser, params);
      res.status(200).send({ isOK: true });
    }

  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: NOTHING
// response: delete 결과
// GET /user/delete
router.get('/delete', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const result = await querySync('SELECT user_identification FROM users WHERE id=?; ', [decoded.id]);
      const qDeleteUser = "UPDATE users SET user_identification=null, user_password=null, name=null, nickname=null, role=null, salt=null, student_id=null, refresh=null, deleted_id=? WHERE id=?; ";
      const params = [result[0].user_identification, decoded.id];
      const result2 = await querySync(qDeleteUser, params);
      res.status(200).send({ isOK: true });
    }

  } catch(err) {
    next(err);
  }
});

export default router;
