import "@babel/polyfill";                     // ES6 트랜스파일링

import express from 'express';                // express 앱
import path from 'path';                      // 경로
import cookieParser from 'cookie-parser';     // 웹 쿠키 
import cors from 'cors';                      // CORS 에러
import helmet from 'helmet';                  // 웹 보안
import compression from 'compression';        // 가능하다면 응답을 압축하여 전송
import logger from 'morgan';                  // Logger
import LoggerMysql from './logger-mysql';     // Log save to mysql

import userRouter from './routes/user';
import boardRouter from './routes/board';
import photoRouter from './routes/photo'
import authRouter from './routes/auth';
import postRouter from './routes/post';
import attachedFileRouter from './routes/attachedFile';
import researchRouter from './routes/research';
import commentRouter from './routes/comment';

const app = express();

// 미들웨어
app.use(helmet());
app.use(compression());
if (process.env.NODE_ENV == 'production') {   // mysql에 로그 저장
  const logFormat = ':remote-addr :remote-user :method :url :status :res[content-length] :response-time :date[iso] :auth';
  // Logger token customize
  logger.token('auth', function (req, res) { return req.headers['authorization'] });
  app.use(logger(logFormat, { stream: new LoggerMysql() }));
} else {
  app.use(logger('dev'));  // 콘솔에 로그 출력
}

app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, '..', 'public')));
app.use(cors());

// 라우팅
app.use('/user', userRouter);
app.use('/auth', authRouter);
app.use('/board', boardRouter);
app.use('/photo', photoRouter);
app.use('/post', postRouter);
app.use('/attached-file', attachedFileRouter);
app.use('/research', researchRouter);
app.use('/comment', commentRouter);

// 에러 핸들링 미들웨어
app.use((err, req, res, next) => {
  // 이미 응답을 전송했다면, default error handler에게 권한을 위임.
  if (res.headersSent) {
    next(err);
  } else {
    console.log(err);
    res.status(500).send({
      error: err.message
    });
  }
});

if (process.env.NODE_ENV == 'production') {
  console.log("Server in Production Mode");
} else {
  console.log("Server in Develop Mode");
}

console.log(`
               ===============       
            =====================    
         =======             ======= 
        ======  Smart IoT Lab  ======
        ==  API server for Vue.js  ==
        ======  = = = = = = =  ======
         =======             ======= 
            =====================    
               ===============       
`);

module.exports = app;
