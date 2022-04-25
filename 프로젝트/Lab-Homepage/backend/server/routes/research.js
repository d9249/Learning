import express from 'express';
import path from 'path';
import fs from 'fs';
import mime from 'mime';
import { querySync } from '../lib/mysqlConnectionPool';
import publicationMulter from '../lib/publicationMulter';
import { verifyAccessToken } from '../lib/token';
import storageConfig from '../config/storage.config';
const router = express.Router();

// request: NOTHING
// response: 논문들
// GET /research
router.get('/', async (req, res, next) =>{
  const qGetResearch = 'SELECT * FROM research ORDER BY reg_date DESC; ';
  const results = await querySync(qGetResearch);
  res.status(200).send(results);
});

// !! REQUIRED AUTH, ROLE
// request: 논문 제목, 저자, 날짜, 위치, 파일 경로, 유형, 링크
// response: insert 결과
// POST /research
router.post('/', async (req, res, next) =>{
  try {
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else { 
      const decoded = await verifyAccessToken(req.headers.authorization);
      if (decoded.role > 1) {
        res.status(401).send()
      } else {
        const qInsertResearch = "INSERT INTO research VALUES (null, ?, ?, ?, ?, ?, ?, ?)";
        const params = [req.body.title, req.body.author, req.body.when, req.body.where, req.body.filepath, req.body.type, req.body.link];
        const result = await querySync(qInsertResearch, params);
        res.status(200).send({ isOK: true });
      }
    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 논문 파일
// response: 업로드된 파일 경로
// POST /research/upload
router.post("/upload", publicationMulter.single("file"), async (req, res, next) => {
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
          isOK: true,
          filepath: res.req.file.path.slice(7),
        });
      }
    }
  } catch(err) {
    next(err);
  }
});

// !! REQUIRED AUTH, ROLE
// request: 논문 아이디
// response: delete 결과
// POST /research/delete
router.post("/delete", async (req, res, next) => {
  try { 
    if (req.headers.authorization == 'null') {
      res.status(401).send();

    } else { 
      const decoded = await verifyAccessToken(req.headers.authorization);
      if (decoded.role > 1) {
        res.status(401).send();
      } else {
        const qGetPublicationInfo = "SELECT file_path FROM research WHERE id=?; ";
        const params = [req.body.id];
        const results = await querySync(qGetPublicationInfo, params);
        if (results[0].file_path && results[0].file_path != '') {
          // 파일 삭제  
          const file = path.join(storageConfig.storagePath, results[0].file_path);
          if (fs.existsSync(file)) {
            fs.unlinkSync(file);
          }
        } 
        // 디비 레코드 삭제
        const qDeletePublication = "DELETE FROM research WHERE id=?; ";
        const results2 = await querySync(qDeletePublication, params);
        res.status(200).send({ isOK: true });
      }
      
    }
  } catch(err) {
    next(err);
  }
});

// request: 첨부파일 경로
// response: 첨부파일
// GET /research/download?filepath=[첨부파일 경로]
router.get("/download", async (req, res, next) => {
  try {
    const file = path.join(storageConfig.storagePath, req.query.filepath);
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
  } catch (err) {
    next(err);
  }
});

module.exports = router;