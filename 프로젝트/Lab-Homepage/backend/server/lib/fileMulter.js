import multer from 'multer';
import path from 'path';

const filters = {
  // 이미지
  png: 'png',
  jpg: 'jpg',
  jpeg: 'jpeg',
  gif: 'gif',
  // 문서
  ppt: 'ppt',
  pptx: 'pptx',
  pdf: 'pdf',
  doc: 'doc',
  docx: 'docx',
  hwp: 'hwp',
  txt: 'txt',
  // 압축
  zip: 'zip',
  // 미디어
  mp4: 'mp4',
  mp3: 'mp3',
};

const storage = multer.diskStorage({
  destination: function(req, file, cb) {
    cb(null, path.join('public', 'attached_files'));
  },
  filename: function(req, file, cb) {
    cb(null, `${new Date().valueOf()}-${file.originalname}`);
  },
});

const fileFilter = (req, file, cb) => {
  const ext = path.extname(file.originalname).slice(1).toLowerCase();
  if (ext in filters) {
    cb(null, true);
  } else {
    cb(new Error('Invalid file.'), false);
  }
}

const limits = {
  fileSize: 50 * 1024 * 1024,     // 50MB 이하만 가능
  files: 5                        // 최대 5개의 파일 
}

module.exports = multer({
  storage,
  limits,
  fileFilter
});