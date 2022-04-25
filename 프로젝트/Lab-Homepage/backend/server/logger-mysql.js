import { querySync } from './lib/mysqlConnectionPool';
import { verifyAccessToken } from './lib/token';
import moment from 'moment';

export default class LoggerMysql {
  write(message) { // message 구성 [:remote-addr :remote-user :method :url :status :res[content-length] :response-time :date :auth]
    // 마지막 줄바꿈 삭제
    message = message.slice(0, -1);
    // split
    const splitted = message.split(' ');

    // OPTIONS 메소드 제외
    if (splitted[2] == 'OPTIONS') {
      return;
    }
    // status 304 Not Modified 제외 (캐시된 자원의 암묵적인 리디렉션)
    if (splitted[4] == '304') {
      return;
    }

    // 시간
    splitted[7] = moment(splitted[7]).toDate();

    // 인증 헤더가 있으면 (로그인)
    if (splitted[8] != 'null' && splitted[8] != '-') {
      // 토큰 유효성 검사
      verifyAccessToken(splitted[8])
        .then(decoded => {
          // 토큰 속 id 삽입
          splitted[8] = decoded.id;
          const qInsertLog = "INSERT INTO logs VALUES (null, ?, ?, ?, ?, ?, ?, ?, ?, ?); ";
          const params = splitted;
          // 삽입 sql
          querySync(qInsertLog, params)
            .catch(err => {
              console.error(err);
            });
        })
        .catch(err => {
          // 토큰이 만료되어 에러 발생 (refresh)
          if (err.name == 'JsonWebTokenError' && err.message == 'invalid signature') {
            const qInsertLog = "INSERT INTO logs VALUES (null, ?, ?, ?, ?, ?, ?, ?, ?, null); ";
            const params = splitted.slice(0, 8);
            // 삽입 sql
            querySync(qInsertLog, params)
              .catch(err => {
                console.error(err);
              });

          } else {
            console.error(err);
          }
        });
    // 인증 헤더가 없으면 (비로그인)
    } else {
      const qInsertLog = "INSERT INTO logs VALUES (null, ?, ?, ?, ?, ?, ?, ?, ?, null); ";
      const params = splitted.slice(0, 8);
      // 삽입 sql
      querySync(qInsertLog, params)
        .catch(err => {
          console.error(err);
        });
    }
  }
}