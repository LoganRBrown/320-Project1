const net = require("net");


const socket = net.connect({port:320, ip:"127.0.0.1"}, ()=>{
		console.log("connected to the server...");
		
		const username = "Logan";

		const buff = Buffer.alloc(9);

		buff.write("JOIN")
		buff.writeUInt8(4,4);
		buff.write("Logan", 5);

		socket.write(buff);

});