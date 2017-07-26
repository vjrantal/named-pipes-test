var net = require('net');

var PIPE_NAME = "testpipe";
var PIPE_PATH = "\\\\.\\pipe\\" + PIPE_NAME;

var paused = false;

var server = net.createServer(function (stream) {
    console.log('Server: on connection');

    var pauseInterval = setInterval(function () {
        if (paused) {
            stream.resume();
            paused = false;
        } else {
            stream.pause();
            paused = true;
        }
    }, 10000);

    stream.on('data', function (c) {
        console.log(Date.now() + ' Server: on data');
    });

    stream.on('end', function () {
        console.log('Server: on end');
        server.close();
        clearInterval(pauseInterval);
    });
});

server.on('close', function () {
    console.log('Server: on close');
});

server.listen(PIPE_PATH, function () {
    console.log('Server: on listening');
});
