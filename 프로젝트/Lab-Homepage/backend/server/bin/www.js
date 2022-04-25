// #!/usr/bin/env node



if (process.env.NODE_ENV == 'production') {
  /**
   * Module dependencies.
   */
  var app = require("../app");
  var https = require('https');
  var fs = require('fs');
  var serverConfig = require("../config/nodejs.config.js");

  /**
   * Get port from environment and store in Express.
   */
  var port = serverConfig.port;
  app.set('port', port);

  /**
   * Set options for HTTPS
   */
  var options = {
    key: fs.readFileSync(serverConfig.ssl_key),
    cert: fs.readFileSync(serverConfig.ssl_crt),
    ca: fs.readFileSync(serverConfig.ssl_ca_bundle)
  }

  /**
   * Create HTTPS server.
   */
  var server = https.createServer(options, app);
  server.listen(port);
  server.on("error", onError);
  server.on("listening", onListening);

  
} else {
  /**
   * Module dependencies.
   */
  var app = require("../app");
  var http = require("http");
  var serverConfig = require("../config/nodejs.config.js");

  /**
   * Get port from environment and store in Express.
   */
  var port = serverConfig.port;
  app.set('port', port);

  /**
   * Create HTTP server.
   */
  var server = http.createServer(options, app);

  /**
   * Listen on provided port, on all network interfaces.
   */
  server.listen(port);
  server.on('error', onError);
  server.on('listening', onListening);
}


/**
 * Event listener for HTTP server "error" event.
 */
function onError(error) {
  if (error.syscall !== "listen") {
    throw error;
  }

  var bind = typeof port === "string" ? "Pipe " + port : "Port " + port;

  // handle specific listen errors with friendly messages
  switch (error.code) {
    case "EACCES":
      console.error(bind + " requires elevated privileges");
      process.exit(1);
      break;
    case "EADDRINUSE":
      console.error(bind + " is already in use");
      process.exit(1);
      break;
    default:
      throw error;
  }
}

/**
 * Event listener for HTTP server "listening" event.
 */
function onListening() {
  var addr = server.address();
  var bind = typeof addr === "string" ? "pipe " + addr : "port " + addr.port;
  console.log("Listening on " + bind);
}
