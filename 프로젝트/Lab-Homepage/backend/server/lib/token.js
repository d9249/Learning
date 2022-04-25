import jwt from 'jsonwebtoken';
import tokenConfig from '../config/token.config';

const getAccessToken = (id, role) => {
  return new Promise((resolve, reject) => {
    jwt.sign(
      { type: 'access', id, role },
      tokenConfig.access.secret,
      { expiresIn: tokenConfig.access.expired, issuer: tokenConfig.issuer, subject: 'access' },
      (err, token) => {
        if (err)  reject(err);
        resolve(token);
      }
    );
  });
}

const getAccessTokenHour = (id, role) => {
  return new Promise((resolve, reject) => {
    jwt.sign(
      { type: 'access', id, role },
      tokenConfig.access.secret,
      { expiresIn: 60 * 60 /*1 hour*/, issuer: tokenConfig.issuer, subject: 'access' },
      (err, token) => {
        if (err)  reject(err);
        resolve(token);
      }
    );
  });
}

const getRefreshToken = () => {
  return new Promise((resolve, reject) => {
    jwt.sign(
      { type: 'refresh' },
      tokenConfig.refresh.secret,
      { expiresIn: tokenConfig.refresh.expired, issuer: tokenConfig.issuer, subject: 'refresh' },
      (err, token) => {
        if (err)  reject(err);
        resolve(token);
      }
    );
  });
}

const verifyAccessToken = (token) => {
  return new Promise((resolve, reject) => {
    jwt.verify(token, tokenConfig.access.secret, (err, decoded) => {
      if (err)  reject(err);
      resolve(decoded);
    })
  });
}

const verifyRefreshToken = (token) => {
  return new Promise((resolve, reject) => {
    jwt.verify(token, tokenConfig.refresh.secret, (err, decoded) => {
      if (err)  reject(err);
      resolve(decoded);
    })
  });
}

const getForgottenToken = (email) => {
  return new Promise((resolve, reject) => {
    jwt.sign(
      { type: 'forgotten', email },
      tokenConfig.forgotten.secret,
      { expiresIn: tokenConfig.forgotten.expired, issuer: tokenConfig.issuer, subject: 'forgotten' },
      (err, token) => {
        if (err)  reject(err);
        resolve(token);
      }
    );
  });
}

const verifyForgottenToken = (token) => {
  return new Promise((resolve, reject) => {
    jwt.verify(token, tokenConfig.forgotten.secret, (err, decoded) => {
      if (err)  reject(err);
      resolve(decoded);
    })
  })
}

const getToken = (anything) => {
  return new Promise((resolve, reject) => {
    jwt.sign(
      {type: 'default', default: anything},
      tokenConfig.default.secret,
      { expiresIn: tokenConfig.default.expired, issuer: tokenConfig.issuer, subject: 'default' },
      (err, token) => {
        if (err)  reject(err);
        resolve(token);
      }
    );
  });
}

const verifyToken = (token) => {
  return new Promise((resolve, reject) => {
    jwt.verify(token, tokenConfig.default.secret, (err, decoded) => {
      if (err)  reject(err);
      resolve(decoded);
    });
  });
}

export {
  getAccessToken,
  getAccessTokenHour,
  getRefreshToken,
  verifyAccessToken,
  verifyRefreshToken,
  getForgottenToken,
  verifyForgottenToken,
  getToken,
  verifyToken,
}
