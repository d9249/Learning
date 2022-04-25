import express from 'express';
import fs from 'fs';
import path from 'path';
import { querySync } from '../lib/mysqlConnectionPool';
import photoMulter from '../lib/photoMulter';
import { verifyAccessToken } from '../lib/token';
import storageConfig from '../config/storage.config';
const router = express.Router();

// request: NOTHING
// response: 사진들 전체
// GET /photo
router.get("/", async (req, res, next) => {
  try {
    const qGetPhotos = "SELECT posts.id, attached_files.filepath, posts.title as title FROM attached_files, posts, boards WHERE attached_files.post_id = posts.id and posts.board_id = boards.id and boards.category = 'gallery' ORDER BY posts.reg_date DESC; ";
    const results = await querySync(qGetPhotos);
    res.status(200).send(results);
  } catch(err) {
    next(err);
  }
});

// request: NOTHING
// response: 최근 사진 5장
// GET /photo/latest
router.get("/latest", async (req, res, next) => {
  try {
    const qGetPhotosLatest = "SELECT attached_files.filepath FROM attached_files, posts, boards WHERE attached_files.post_id = posts.id and posts.board_id = boards.id and boards.category = 'gallery' ORDER BY posts.reg_date DESC LIMIT 5; ";
    const results = await querySync(qGetPhotosLatest);
    res.status(200).send(results);
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 파일
// response: 업로드된 파일 이름
// POST /photo/upload
router.post("/upload", photoMulter.single("file"), async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      fs.unlinkSync(res.req.file.path);
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      if (decoded.role > 1) {
        fs.unlinkSync(res.req.file.path);
        res.status(401).send();

      } else {
        res.status(200).send({
          filepath: res.req.file.path.slice(7),
          originalname: res.req.file.originalname,
          isOK: true,
        });
      }
    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 사진 아이디
// response: delete 결과
// POST /photo/delete
router.post('/delete', async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      if (decoded.role > 1) {
        res.status(401).send();

      } else {
        const file = path.join(storageConfig.storagePath, req.body.path);
        if (fs.existsSync(file)) {
          fs.unlinkSync(file);
          res.status(200).send({ isOK: true });
        } else {
          res.status(404).send({ isOK: false });
        }
      }

    }
  } catch(err) {
    next(err);
  }
});

export default router;
