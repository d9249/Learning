import axiosInstance from './axios.config';

////////////////////////////////////
//// 인증 관련 (회원가입, 본인확인 등) ////
////////////////////////////////////

function singin(payload) {
  return axiosInstance.post('/auth/signin', payload);
}

function verifyEmail(payload) {
  return axiosInstance.post('/auth/verify-email', payload);
}

function verifyCode(payload) {
  return axiosInstance.get('/auth/verify-code', { params: payload });
}

function postIdentification(payload) {
  return axiosInstance.post('/auth/check-identification', payload, { requireAuth: true });
}

function checkIdentification(payload) {
  return axiosInstance.get('/auth/check-identification', { params: payload, requireAuth: true });
}

function checkForgotten(payload) {
  return axiosInstance.get('/auth/forgotten', { params: payload });
}

////////////////////////////////////
//////////// 게시판(강의) 관련  ////////
////////////////////////////////////

function getOngoingLecture() {
  return axiosInstance.get('/board/ongoing');
}

function getBoard(payload) {
  return axiosInstance.get('/board', { params: payload, requireAuth: true});
}

function postBoard(payload) {
  return axiosInstance.post('/board', payload, { requireAuth: true });
}

function softDeleteBoard(payload) {
  return axiosInstance.post('/board/soft-delete', payload, { requireAuth: true });
}

function getCountOfPosts(payload) {
  return axiosInstance.get('/board/count', { params: payload });
}

////////////////////////////////////
//////////// 사진 관련  //////////////
////////////////////////////////////

function getPhotoLatest() {
  return axiosInstance.get('/photo/latest');
}

function getPhotos() {
  return axiosInstance.get('/photo');
}

function uploadPhoto(payload) {
  const headers = {
    'Content-Type': 'multipart/form-data'
  }
  return axiosInstance.post('/photo/upload', payload, { headers, requireAuth: true });
}

function deletePhoto(payload) {
  return axiosInstance.post('/photo/delete', payload, { requireAuth: true });
}

////////////////////////////////////
//////////// 게시글 관련  /////////////
////////////////////////////////////

function getPosts(payload) {
  return axiosInstance.get('/post', { params: payload, requireAuth: true });
}

function getPost(payload) {
  return axiosInstance.get(`/post/${payload.postId}`, { requireAuth: true });
}

function postPost(payload) {
  return axiosInstance.post('/post', payload, { requireAuth: true });
}

function updatePost(payload) {
  return axiosInstance.post(`/post/${payload.postId}/update`, payload, { requireAuth: true });
}

function deletePost(payload) {
  return axiosInstance.get(`/post/${payload.postId}/delete`, { requireAuth: true });
}

function postPostGallery(payload) {
  return axiosInstance.post('/post/gallery', payload, { requireAuth: true });
}

////////////////////////////////////
//////////// 첨부파일 관련  ///////////
////////////////////////////////////

function downloadAttachedFile(payload) {
  return axiosInstance.get('/attached-file/download', { responseType: 'blob', params: payload, requireAuth: true });
}

function postAttachedFile(payload) {
  return axiosInstance.post('/attached-file', payload, { requireAuth: true });
}

function uploadAttachedFile(payload) {
  const headers = {
    'Content-Type': 'multipart/form-data'
  }
  return axiosInstance.post('/attached-file/upload', payload, { headers, requireAuth: true });
}

function deleteAttachedFile(payload) {
  return axiosInstance.post('/attached-file/delete', payload, { requireAuth: true });
}

////////////////////////////////////
//////////// 사용자 관련  /////////////
////////////////////////////////////

function signup(payload) {
  return axiosInstance.post('/user', payload);
}

function getUser() {
  return axiosInstance.get('/user', { requireAuth: true });
}

function duplicationCheck(payload) {
  return axiosInstance.post('/user/duplication-check', payload);
}

function changePassword(payload) {
  return axiosInstance.post('/user/change-pw', payload);
}

function getUserInfo() {
  return axiosInstance.get('/user/info', { requireAuth: true });
}

function updateUser(payload) {
  return axiosInstance.post('/user/update', payload, { requireAuth: true });
}

function deleteUser() {
  return axiosInstance.get('/user/delete', { requireAuth: true });
}

////////////////////////////////////
//////////// 논문 관련  //////////////
////////////////////////////////////

function getResearch() {
  return axiosInstance.get('/research');
}

function uploadResearch(payload) {
  const headers = {
    'Content-Type': 'multipart/form-data',
  }
  return axiosInstance.post('/research/upload', payload, { headers, requireAuth: true, });
}

function insertResearch(payload) {
  return axiosInstance.post('/research', payload, { requireAuth: true });
}

function deleteResearch(payload) {
  return axiosInstance.post('/research/delete', payload, { requireAuth: true });
}

function downloadResearch(payload) {
  return axiosInstance.get('/research/download', { responseType: 'blob', params: payload });
}

////////////////////////////////////
//////////// 댓글 관련  //////////////
////////////////////////////////////

function getComments(payload) {
  return axiosInstance.get(`/comment?postId=${payload.postId}`, { requireAuth: true });
}

function postComment(payload) {
  return axiosInstance.post('/comment', payload, { requireAuth: true });
}

function editComment(payload) {
  return axiosInstance.post(`/comment/${payload.id}/update`, payload, { requireAuth: true });
}

function deleteComment(payload) {
  return axiosInstance.get(`/comment/${payload.id}/delete`, { requireAuth: true });
}

export {
  signup,
  singin,
  duplicationCheck,
  verifyEmail,
  verifyCode,
  getUser,
  getOngoingLecture,
  getPhotoLatest,
  getBoard,
  getPosts,
  getCountOfPosts,
  getPost,
  downloadAttachedFile,
  getPhotos,
  uploadPhoto,
  postPost,
  postAttachedFile,
  getResearch,
  changePassword,
  uploadAttachedFile,
  deleteAttachedFile,
  updatePost,
  deletePost,
  postIdentification,
  checkIdentification,
  getUserInfo,
  updateUser,
  deleteUser,
  uploadResearch,
  insertResearch,
  deleteResearch,
  postBoard,
  softDeleteBoard,
  downloadResearch,
  deletePhoto,
  postPostGallery,
  getComments,
  postComment,
  editComment,
  deleteComment,
  checkForgotten,
}