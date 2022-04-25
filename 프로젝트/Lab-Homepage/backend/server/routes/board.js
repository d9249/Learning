import express from 'express';
import { querySync } from '../lib/mysqlConnectionPool';
import { verifyAccessToken } from '../lib/token';
const router = express.Router();

// request: NOTHING
// response: 현재 진행중인 수업
// GET /board/ongoing
router.get("/ongoing", async (req, res, next) => {
  try {
    const qGetBoardsOngoing = "SELECT id, name FROM boards WHERE category='lecture' and ongoing=1; ";
    const results = await querySync(qGetBoardsOngoing);
    res.status(200).send(results);
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 게시판 아이디
// response: 게시판 정보
// GET /board
router.get("/", async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const results = await querySync('SELECT count(*) as count FROM users WHERE id=?; ', [decoded.id]);
      // 존재하지 않는 아이디
      if (results[0].count != 1) {
        res.status(401).send();
      }
      // 특정 게시판 조회
      else {
        const qGetBoard = "SELECT * FROM boards WHERE id = ?; ";
        const params = [req.query.lectureId];
        const results = await querySync(qGetBoard, params);

        res.status(200).send(results[0]);
      }

    }
  } catch(err) {
    next(err);
  }
});

// request: 게시판 아이디
// response: 게시글 개수
// GET /board/count
router.get("/count", async (req, res, next) => {
  try {
    const qGetPostsCount = "SELECT count(*) as count FROM posts WHERE board_id=? and user_id is not null; ";
    const params = [req.query.lectureId];
    const results = await querySync(qGetPostsCount, params);
    res.status(200).send(results[0]);

  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 게시판 이름, 카테고리
// response: insert 결과
// POST /board
router.post('/', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      // 관리자만 게시판 생성 가능
      if (decoded.role < 1) {
        const qInsertBoard = "INSERT INTO boards VALUES (null, ? ,?, ?); ";
        const params = [req.body.name, req.body.category, req.body.ongoing];
        const result = await querySync(qInsertBoard, params);
        res.status(200).send({ isOK: true });

      } else {
        res.status(401).send(); 
      }

    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: NOTHING
// response: delete 결과
// POST /board/delete
router.post('/delete', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      // 관리자만 게시판 삭제 가능
      if (decoded.role < 1) {
        const qDeleteBoard = "DELETE FROM boards WHERE id=? ";
        const params = [req.body.id];
        const result = await querySync(qDeleteBoard, params);
        res.status(200).send({ isOK: true });

      } else {
        res.status(401).send();
      }
      
    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: NOTHING
// response: update 결과
// POST /board/soft-delete
router.post('/soft-delete', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      // 관리자만 게시판 부드러운 삭제 가능
      if (decoded.role < 1) {
        const qUpdateOngoing = "UPDATE boards SET ongoing=0 WHERE id=? ";
        const params = [req.body.id];
        const result = await querySync(qUpdateOngoing, params);
        res.status(200).send({ isOK: true });

      } else {
        res.status(401).send();
      }
    }
  } catch(err) {
    next(err);
  }
});

export default router;
