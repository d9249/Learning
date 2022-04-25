import express from 'express';
import { querySync } from '../lib/mysqlConnectionPool';
import { verifyAccessToken } from '../lib/token';
const router = express.Router();

// !! REQUIRED AUTH
// request: 게시글 아이디 
// response: 해당 게시글 댓글
// GET /comment?postId=[게시판아이디]
router.get('/', async (req, res, next) => {
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
        const qGetComments = 'SELECT comments.id, users.nickname, comments.content, comments.reg_date, comments.edit_date FROM comments, users, posts WHERE comments.user_id=users.id and comments.post_id=posts.id and posts.id=? ORDER BY reg_date ASC; ';
        const params = [req.query.postId];
        const results2 = await querySync(qGetComments, params);
        res.status(200).send(results2);
      }
    }

  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 게시글 아이디, 작성자 아이디, 댓글 내용
// response: INSERT 결과
// POST /comment
router.post('/', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const qInsertComment = 'INSERT INTO comments VALUES (null, ?, ?, ?, DEFAULT, DEFAULT); ';
      const params = [req.body.postId, decoded.id, req.body.content];
      const results2 = await querySync(qInsertComment, params);
      res.status(200).send({ isOK: true });
    }

  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 댓글 아이디, 댓글 내용
// response: UPDATE 결과
// POST /comment/:id/update
router.post('/:id/update', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      let qUpdateComment = '';
      let params = [];

      // admin은 모든 댓글을 수정할 수 있다.
      if (decoded.role < 1) {
        qUpdateComment = 'UPDATE comments SET content=?, edit_date=current_timestamp() WHERE id=?; ';
        params = [req.body.content, req.params.id];
      }
      // 본인의 댓글만 수정할 수 있다.
      else {
        qUpdateComment = 'UPDATE comments SET content=?, edit_date=current_timestamp() WHERE id=? and user_id=?; ';
        params = [req.body.content, req.params.id, decoded.id];
      }

      const results = await querySync(qUpdateComment, params);
      res.status(200).send({ isOK: true });
    }

  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 댓글 아이디
// response: DELETE 결과
// POST /comment/:id/delete
router.get('/:id/delete', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      let qDeleteComment = '';
      let params = [];

      // admin은 모든 댓글을 수정할 수 있다.
      if (decoded.role < 1) {
        qDeleteComment = 'DELETE FROM comments WHERE id=?; ';
        params = [req.params.id];
      }
      // 본인의 댓글만 수정할 수 있다.
      else {
        qDeleteComment = 'DELETE FROM comments WHERE id=? and user_id=?; ';
        params = [req.params.id, decoded.id];
      }

      const results = await querySync(qDeleteComment, params);
      res.status(200).send({ isOK: true });
    }

  } catch(err) {
    next(err);
  }
});

module.exports = router;