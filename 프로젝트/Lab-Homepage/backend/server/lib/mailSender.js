import nodemailer from 'nodemailer';
import mailConfig from '../config/mail.config';

const transporter = nodemailer.createTransport({
  service: mailConfig.service,
  auth: mailConfig.auth
});

const sendMail = (userEmail, subject, text) => {
  return new Promise((resolve, reject) => {
    const mailOptions = {
      from: {
        name: 'Smart IoT Lab',
        address: mailConfig.auth.user,
      },
      to: userEmail,
      subject,
      text,
    };

    transporter.sendMail(mailOptions, (err, info) => {
      if (err)  reject(err);
      resolve(info);
    });
  });
}

export {
  sendMail,
}