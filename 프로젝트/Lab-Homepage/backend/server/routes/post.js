import express from 'express';
import { querySync } from '../lib/mysqlConnectionPool';
import { verifyAccessToken } from '../lib/token';
import { getPostsValidator, getPostValidator } from '../lib/validator'
const router = express.Router();

// !! REQUIRED AUTH
// request: 게시판 아이디, 제목, 내용
// response: 삽입된 데이터 아이디
// POST /post
router.post('/', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const qInsertPost = 'INSERT INTO posts VALUES (null, ?, ?, ?, DEFAULT, DEFAULT, ?, null); ';
      // 게시글 내용 삽입 전 처리
      let content = replaceAll(req.body.content, ' ', '&nbsp;');
      content = replaceAll(content, '<p></p>', '<br>');
      const params = [req.body.lectureId, decoded.id, req.body.title, content];

      const result = await querySync(qInsertPost, params);
      res.status(200).send({
        isOK: true,
        insertId: result.insertId,
      });
    }
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 게시판 아이디, 제목, 내용
// response: 삽입된 데이터 아이디
// POST /post/gallery
router.post('/gallery', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);

      // 갤러리 게시판 아이디 가져오기
      const result = await querySync("SELECT id FROM boards WHERE category='gallery'; ");
      const galleryId = result[0].id;
      // 갤러리 게시판에 삽입
      const qInsertPost = 'INSERT INTO posts VALUES (null, ?, ?, ?, DEFAULT, DEFAULT, ?, null); ';
      const params = [galleryId, decoded.id, req.body.title, req.body.content];

      const result2 = await querySync(qInsertPost, params);
      res.status(200).send({
        isOK: true,
        insertId: result2.insertId,
      });
    }
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 게시판 아이디, 시작 인덱스, 개수
// response: '시작 인덱스'부터 '개수'만큼의 게시글
// GET /post?id=[게시판 아이디]&start=[시작 인덱스]&count=[개수]
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

      } else {
        const qGetPosts = "SELECT posts.id, posts.title, users.nickname as author, posts.reg_date FROM posts, users WHERE posts.board_id = ? and users.id = posts.user_id ORDER BY posts.reg_date DESC LIMIT ?, ?; ";
        const params = [req.query.id, req.query.start, req.query.count];
        // parameter validation
        if (!getPostsValidator(params)) {
          throw new Error('Parameters validation error');
        }
        
        const results = await querySync(qGetPosts, params);
        res.status(200).send(results);
      }
    }
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 게시글 아이디
// response: 게시글 정보(제목, 내용, 등록 시간 등)
// GET /post/:id
router.get("/:id", async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const results = await querySync('SELECT count(*) as count FROM users WHERE id=?; ', [decoded.id]);
      // 존재하지 않는 아이디
      if (results[0].count != 1) {
        res.status(401).send();

      } else {
        const qGetPost = "SELECT posts.id, posts.title, users.nickname, posts.reg_date, posts.content, boards.name as board_name FROM posts, boards, users WHERE posts.board_id=boards.id and posts.user_id=users.id and boards.ongoing=1 and posts.id=?; ";
        const params = [req.params.id];
        // parameter validation
        if (!getPostValidator(params)) {
          throw new Error('Parameters validation error');
        }
        
        const results = await querySync(qGetPost, params);
        if (results.length != 1 || results[0].ongoing == 0) {
          res.status(404).send();
        } else {      
          const qGetAttachedFilesId = "SELECT id, origin_filename, size, filepath FROM attached_files WHERE post_id = ?; ";
          const results2 = await querySync(qGetAttachedFilesId, params);
          results[0].attached_files = results2;
          res.status(200).send(results[0]);
        }

      }
    }
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 게시글 아이디, 게시글 제목, 내용
// response: UPDATE 결과
// POST /post/:id/update
router.post("/:id/update", async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      let qUpdatePost = '';
      let params = [];

      // 게시글 내용 삽입 전 처리
      let content = replaceAll(req.body.content, ' ', '&nbsp;');
      content = replaceAll(content, '<p></p>', '<br>');

      // admin은 모든 글을 수정할 수 있다.
      if (decoded.role < 1) {
        qUpdatePost = "UPDATE posts SET title=?, content=?, edit_date=current_timestamp() WHERE id=?; ";
        params = [req.body.title, content, req.params.id];
      }
      // 본인의 글만 수정할 수 있다.
      else {
        qUpdatePost = "UPDATE posts SET title=?, content=?, edit_date=current_timestamp() WHERE id=? and user_id=?; ";
        params = [req.body.title, content, req.params.id, decoded.id];
      }

      const result = await querySync(qUpdatePost, params);
      if (result.changedRows > 0) {
        res.status(200).send({ isOK: true });
      } else {
        res.status(200).send({ isOK: false });
      }
    }
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: NOTHING
// response: DELETE 결과
// GET /post/:id/delete
router.get("/:id/delete", async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      let qDeletePost = '';
      let params = [];

      // admin은 모든 글을 삭제할 수 있다.
      if (decoded.role < 1) {
        qDeletePost = "DELETE FROM posts WHERE id=?; ";
        params = [req.params.id];
      }
      // 본인의 글만 삭제할 수 있다.
      else {
        qDeletePost = "DELETE FROM posts WHERE id=? and user_id=?; ";
        params = [req.params.id, decoded.id];
      }

      const result = await querySync(qDeletePost, params);
      if (result.affectedRows > 0) {
        res.status(200).send({ isOK: true });
      } else {
        res.status(200).send({ isOK: false });
      }

    }
  } catch (err) {
    next(err);
  }
});

function replaceAll(str, searchStr, replaceStr) {
  return str.split(searchStr).join(replaceStr);
}

export default router;
