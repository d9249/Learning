import multer from 'multer';
import path from 'path';

const storage = multer.diskStorage({
  destination: function(req, file, cb) {
    cb(null, path.join('public', 'publications'));
  },
  filename: function(req, file, cb) {
    cb(null, `${new Date().valueOf()}_${file.originalname}`);
  },
});

module.exports = multer({
  storage,
});