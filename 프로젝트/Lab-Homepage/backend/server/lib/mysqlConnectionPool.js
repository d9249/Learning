import mysql from 'mysql';
import mysqlConfig from '../config/remote.mysql.config';

// Connection pool 생성
const pool = mysql.createPool(mysqlConfig);

// Get Connection in Pool
const getConnectionInPool = (cb) => {
  pool.getConnection((err, connection) => {
    if (err) throw err;
    
    cb(connection);
  });
}

const getConnectionInPoolSync = () => {
  return new Promise((resolve, reject) => {
    pool.getConnection((err, connection) => {
      if (err)  reject(err);
      resolve(connection);
    });
  });
}

const querySync = (q, params) => {
  if (params) {
    return new Promise(async (resolve, reject) => {
      const connection = await getConnectionInPoolSync();
      connection.query(q, params, (err, result) => {
        connection.release();
        if(err) reject(err);
        resolve(result);
      });
    });

  } else {
    return new Promise(async (resolve, reject) => {
      const connection = await getConnectionInPoolSync();
      connection.query(q, (err, result) => {
        connection.release();
        if(err) reject(err);
        resolve(result);
      });
    });
  }
}

export {
  getConnectionInPool,
  getConnectionInPoolSync,
  querySync
}