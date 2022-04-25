import express from 'express';
import fs from 'fs';
import path from 'path';
import mime from 'mime';
import { querySync } from '../lib/mysqlConnectionPool';
import fileMulter from '../lib/fileMulter';
import storageConfig from '../config/storage.config';
import { verifyAccessToken } from '../lib/token';
const router = express.Router();

// !! REQUIRED AUTH
// request: 게시글 아이디, 파일경로, 파일명
// response: NOTHING
// POST /attached-file
router.post("/", async (req, res, next) => {
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
        const qInsertAttachedFileInfo = "INSERT INTO attached_files VALUES (null, ?, ?, ?, ?); ";
        const params = [req.body.postId, req.body.filepath, req.body.filename, req.body.filesize];

        const results2 = await querySync(qInsertAttachedFileInfo, params);
        res.status(200).send({ isOK: true });
      }
    }
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 첨부파일 아이디
// response: 삭제 결과
// POST /attached-file/delete
router.post("/delete", async (req, res, next) => {
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
        const file = path.join(storageConfig.storagePath, req.body.path);
        if (fs.existsSync(file)) {
          // 파일 삭제
          fs.unlinkSync(file);

          if (req.body.id) {
            const qDeleteAttachedFile = "DELETE FROM attached_files WHERE id=?; ";
            const params = [req.body.id];
            await querySync(qDeleteAttachedFile, params);
          }
          
          res.status(200).send({ isOK: true });

        } else {
          res.status(404).send();
        }
      }
      
    }
  } catch (err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 파일(들)
// response: 업로드된 파일 이름(들)
// POST /attached-file/upload
router.post("/upload", fileMulter.array("file"), async (req, res, next) => {
  try {
    if (req.headers.authorization == 'null') {
      res.req.files.map((file) => { 
        fs.unlinkSync(file.path); 
      });
      res.status(401).send();

    } else {
      const decoded = await verifyAccessToken(req.headers.authorization);
      const results = await querySync('SELECT count(*) as count FROM users WHERE id=?; ', [decoded.id]);
      // 존재하지 않는 아이디
      if (results[0].count != 1) {
        res.req.files.map((file) => { 
          fs.unlinkSync(file.path); 
        });
        res.status(401).send();
        
      } else {
        const filepaths = res.req.files.map((file) => { return file.path.slice(7); });
        const originalNames = res.req.files.map((file) => { return file.originalname; });
        const filesizes = res.req.files.map((file) => { return file.size; });
        res.status(200).send({
          isOK: true,
          filepaths,
          originalNames,
          filesizes,
        });
      }
    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH
// request: 첨부파일 경로
// response: 첨부파일
// GET /attached-file/download?f=[첨부파일 아이디]
router.get("/download", async (req, res, next) => {
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
        const qGetAttachedFilePath = "SELECT filepath FROM attached_files WHERE id=?; ";
        const params = [req.query.f];
        const results = await querySync(qGetAttachedFilePath, params);
        if (results.length != 1) {
          throw new Error('File download error');
        }

        const file = path.join(storageConfig.storagePath, results[0].filepath);
        if (fs.existsSync(file)) { 
          const filename = path.basename(file); // 파일 경로에서 파일명(확장자포함)만 추출
          const mimetype = mime.getType(file);  // 파일의 타입(형식)을 가져옴
        
          res.setHeader('Content-Disposition', 'attachment; filename=' + encodeURI(filename)); // 다운받아질 파일명 설정
          res.setHeader('Content-type', mimetype); // 파일 형식 지정
        
          const filestream = fs.createReadStream(file);
          filestream.pipe(res);
        } else {
          res.status(404).send();
        }
          
      }
    }
  } catch (err) {
    next(err);
  }
});


export default router;
