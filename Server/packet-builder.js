

exports.PacketBuilder = {
	join(responseID){
		const packet = Buffer.alloc(5);

		packet.write("JOIN", 0);
		packet.writeUInt8(responseID, 4);

		return packet;
	},
	start(tableSeat, playerCount){
		const packet = Buffer.alloc(6);

		packet.write("STRT", 0);
		packet.writeUInt8(playerCount, 4)
		packet.writeUInt8(tableSeat, 5);

		return packet;
	},
	chat(client, usernameLength, messageLength, messageToSend){
		const packet = Buffer.alloc(7+usernameLength+messageLength);

		packet.write("CHAT", 0);
		packet.writeUInt8(usernameLength, 4);
		packet.writeUInt16BE(messageLength, 5);
		packet.write(client.username, 7);
		packet.write(messageToSend, usernameLength);

		return packet;

	},
	pass(target, cardOne, cardTwo, cardThree){
		const packet = Buffer.alloc(8);

		packet.write("PASS", 0);
		packet.writeUInt8(target, 4)
		packet.writeUInt8(cardOne, 5);
		packet.writeUInt8(cardTwo, 6);
		packet.writeUInt8(cardThree, 7);

		return packet;
	},
	update(game){
		const packet = Buffer.alloc(125);
		packet.write("UPDT", 0);
		packet.writeUInt8(game.gameState, 4);
		packet.writeUInt8(game.whoseturn, 5);
		packet.writeUInt8(game.whoHasLost, 6);
		packet.writeUInt8(game.tablePot.length, 7);

		var offset = 8;

		/*game.server.clients.forEach(client => 

			client.hand.forEach(card => 

				packet.writeUInt8(card, offset++)	

			)
		); */

		game.tablePot.forEach(card =>

			packet.writeUInt8(card, offset++)
		);




		return packet;

	},
	hand(client){

		const packet = Buffer.alloc(19);
		packet.write("HAND", 0);
		packet.writeUInt8(client.seatAtTable, 4);
		packet.writeUInt8(client.hand.length, 5)
		var offset = 6;
		client.hand.forEach(card =>

			packet.writeUInt8(card, offset++)

		);
		packet.writeUInt8(client.score, offset);

		return packet;
	},
	tpot(target, pot = []){
		const packet = Buffer.alloc(5 + server.clients.length);
		packet.write("TPOT");
		packet.writeUInt8(target, 4);
		var offset = 5;
		pot.forEach(c => { packet.writeUInt8(c, offset++)});

		return packet;
	},
	rscr(){
		const packet = Buffer.alloc(4 + server.clients.length);
		packet.write("RSCR");
		var offset = 4;
		server.clients.forEach(c => { packet.writeUInt8(c.score, offset++) });

		return packet;
	},

};