
const getPostsValidator = (params) => {
  // 모든 인자가 있는지
  if (!params[0] || !params[1] || !params[2]) {
    return false;
  }
  // 인자를 Number로 변환
  params[0] = parseInt(params[0]);
  params[1] = parseInt(params[1]);
  params[2] = parseInt(params[2]);
  // 인자가 적절한 Number형 문자열이 아니라면 NaN이다.
  if (params[0] == NaN || params[1] == NaN || params[2] == NaN) {
    return false;
  }
  return true;
};

const getPostValidator = (params) => {
  // 모든 인자가 있는지
  if (!params[0]) {
    return false;
  }
  // 인자를 Number로 변환
  params[0] = parseInt(params[0]);
  // 인자가 적절한 Number형 문자열이 아니라면 NaN이다.
  if (params[0] == NaN) {
    return false;
  }
  return true;
};

export {
  getPostValidator,
  getPostsValidator,
}