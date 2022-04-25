import express from 'express';
import crypto from 'crypto';
import moment from 'moment';
import { querySync } from '../lib/mysqlConnectionPool';
import { getToken, verifyToken, 
        getAccessToken, verifyAccessToken,
        getAccessTokenHour, 
        getRefreshToken, verifyRefreshToken, 
        getForgottenToken, verifyForgottenToken  } from '../lib/token';
import { sendMail } from '../lib/mailSender'
import cryptoConfig from '../config/crypto.config';
import mailConfig from '../config/mail.config';
const router = express.Router();

// request: 아이디와 비밀번호,
// response: 검색 쿼리 결과
// POST /auth/signin
router.post('/signin', async (req, res, next) => {
  try {
    // 아이디 확인
    const qVerifyUserId = "SELECT id FROM users WHERE user_identification = ?;";
    const params = [req.body.userId];
    const results = await querySync(qVerifyUserId, params);
    
    if (results.length === 1) {
      const qGetUserInfo = "SELECT * FROM users where id = ?;";
      const params = [results[0].id];
      const results2 = await querySync(qGetUserInfo, params);
      
      // 해싱 후 비밀번호 확인
      const key = crypto.pbkdf2Sync(req.body.userPw, results2[0].salt, cryptoConfig.interation, cryptoConfig.length, cryptoConfig.hash).toString('base64');
      if (key == results2[0].user_password) {
        // Authorized.
        const user = {
          nickname: results2[0].nickname,
          role: results2[0].role,
        };

        // 자동 signin 
        if (req.body.autoSignin) {
          // access, refresh 토큰 발급
          const accessToken = await getAccessToken(results2[0].id, results2[0].role);
          const refreshToken = await getRefreshToken();
          // refresh 토큰 저장
          const qInsertRefresh = "UPDATE users SET refresh=? WHERE id=?; ";
          const params = [refreshToken, results2[0].id];
          const results3 = await querySync(qInsertRefresh, params);

          res.status(200).send({ user, accessToken, refreshToken });
        } 
        // non 자동 signin 
        else {
          // access 토큰 발급 (1 hour)
          const accessToken = await getAccessTokenHour(results2[0].id, results2[0].role);
          res.status(200).send({ user, accessToken });
        }
      } else {
        // Unauthorized. 비밀번호가 틀림.
        res.status(401).send();
      }
    } else {
      if (results.length == 0) {
        // Unauthorized. 아이디가 존재하지 않음.
        res.status(401).send();
      } else {
        // 일치하는 아이디가 2개 이상
        throw Error('[POST /auth/signin]: Too many rows');
      }
    }

  } catch (err) {
    next(err);
  }
});

// request: 이메일
// response: 이메일 전송 완료했는가
// POST /auth/verify-email
router.post('/verify-email', async (req, res, next) => {
  try {
    const userEmail = req.body.userId;
    // 인증 번호 6자리 생성
    const code = crypto.randomBytes(6).toString('hex').slice(0, 6);

    // 디비에 삽입
    const qInsertAuthCode = 'INSERT INTO auths VALUES (?, ?, ?, DEFAULT); ';
    const params = [null, userEmail, code];
    const result = await querySync(qInsertAuthCode, params);

    // email 전송
    const sentInfo = await sendMail(userEmail, '[Smart IoT Lab] 이메일 인증 코드 전송', `인증 코드 : ${code}`);
    
    res.status(200).send({
      isOK: true,
      emailAuthId: result.insertId,
    });
    
  } catch (err) {
    next(err);
  }
});

// request: 인증코드
// response: 인증코드가 올바른가
// GET /auth/verify-code
router.get('/verify-code', async (req, res, next) => {
  try {
    const qCheckCode = 'SELECT code, timestamp, user_email FROM auths WHERE id = ?; ';
    const params = [req.query.id];
    const result = await querySync(qCheckCode, params);

    if (result.length == 1) {
      const diff = Date.now() - moment(result[0].timestamp).valueOf();
      if (diff < mailConfig.expired && result[0].code == req.query.code) {

        if (req.query.type && req.query.type == 'forgotten') {
          const token = await getForgottenToken(result[0].user_email);
          res.status(200).send({
            isOK: true,
            token,
          });

        } else {
          res.status(200).send({
            isOK: true,
          });
        }
        
      } else {
        res.status(200).send({
          isOK: false,
        });
      }
    } else {
      res.status(200).send({
        isOK: false,
      });
    }
  } catch (err) {
    next(err);
  }
});

// request: NOTHING
// response: 토큰 유효 여부
// GET /auth/forgotten?t=[forgotten token]
router.get('/forgotten', async (req, res, next) => {
  try {    
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyForgottenToken(req.query.t);
      if (decoded.type == 'forgotten') {
        res.status(200).send({ isOK: true });
      } else {
        res.status(400).send();
      }
    }
  } catch (err) {
    next(err);
  }
});

// request: NOTHING
// response: 토큰 유효 여부
// GET /auth/check
router.get('/check', async (req, res, next) => {
  try {    
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      if (decoded.type == 'access') {
        res.status(200).send({ isOK: true });
      } else {
        res.status(400).send();
      }
    }
  } catch (err) {
    next(err);
  }
});

// request: NOTHING
// response: 토큰 재발행
// GET /auth/refresh
router.get('/refresh', async (req, res, next) => {
  try {
    const refreshToken = req.headers.authorization;
    if (refreshToken == 'null') {
      res.status(401).send();

    } else {
      // refresh 토큰 유효성 확인
      const decoded = await verifyRefreshToken(refreshToken);
      if (decoded.type == 'refresh') {
        // refresh 토큰 DB에서 확인
        const qSelectRefresh = "SELECT id, role FROM users WHERE refresh=?; ";
        const params = [refreshToken];
        const result = await querySync(qSelectRefresh, params);
        if (result.length == 1) {
          // access 토큰 발행
          const newAccessToken = await getAccessToken(result[0].id, result[0].role);
          res.status(200).send({ isOK: true, newAccessToken });
        } else {
          res.status(400).send();
        }
      } else {
        res.status(400).send();
      }
    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 비밀번호
// response: 본인확인 토큰
// POST /auth/check-identification
router.post('/check-identification', async (req, res, next) => {
  try { 
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const qGetSalt = "SELECT salt, user_identification, user_password FROM users WHERE id=?; ";
      const params = [decoded.id];
      const results = await querySync(qGetSalt, params);
      const key = crypto.pbkdf2Sync(req.body.userPw, results[0].salt, cryptoConfig.interation, cryptoConfig.length, cryptoConfig.hash).toString('base64');
      if (results[0].user_password == key) {
        const token = await getToken('check-identification' + decoded.id);
        res.status(200).send({ isOK: true, token });
      } else {
        res.status(401).send();
      }
    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 본인확인 토큰
// response: 유효성
// GET /auth/check-identification?t=[일반토큰]
router.get('/check-identification', async (req, res, next) => {
  try { 
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const decoded2 = await verifyToken(req.query.t);
      if (decoded2.default == ('check-identification' + decoded.id)) {
        res.status(200).send({ isOK: true });
      } else {
        res.status(401).send();
      }
    }
  } catch(err) {
    if (err.name == 'JsonWebTokenError' && err.message == 'jwt must be provided') {
      res.status(401).send();
    } else {
      next(err);
    }
  }
});

export default router;
