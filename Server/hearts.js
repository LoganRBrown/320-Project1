const server = require("./server.js");
const PacketBuilder = require("./packet-builder.js").PacketBuilder;


const Game = {
	whoseTurn: 1,
	gameState: 0,
	whoHasLost: 0,
	passcounter: 0,
	scoreCounter: 0;
	firstCard: null;
	oneDeck: [
		[1,2,3,4,5,6,7,8,9,10,11,12,13], //Spades 
		[14,15,16,17,18,19,20,21,21,22,23,24,25,26], //clubs 
		[27,28,29,30,31,32,33,34,35,36,37,38,39], //hearts
		[40,41,42,43,44,45,46,47,48,49,50,51,52] //diamonds
	],
	tablePot: [],
	kitty: [],
	client1:null, //player 1
	client2:null, // player 2
	client3:null, //player 3
	client4:null, //player 4
	client5:null, //player 5
	client6:null, //player 6
	client7:null, //player 7
	client8:null, //player 8

	cardsPlayedThisRound: [],

	playMove(client, card){

		if(this.whoHasLost > 0) return;
		if(this.whoseTurn == 1 && client != this.client1) return;
		if(this.whoseTurn == 2 && client != this.client2) return;
		if(this.whoseTurn == 3 && client != this.client3) return;
		if(this.whoseTurn == 4 && client != this.client4) return;
		if(this.whoseTurn == 5 && client != this.client5) return;
		if(this.whoseTurn == 6 && client != this.client6) return;
		if(this.whoseTurn == 7 && client != this.client7) return;
		if(this.whoseTurn == 8 && client != this.client8) return;

		if(card < 0) return;

		if (checkForValidCard(card) == false) return;

		if(++whoseTurn > 8) whoseTurn = 1; // flip between players turns.

		tablePot.push(card);

		client.hand.pop(card);

		this.checkStateAndUpdate();
	},
	checkStateAndUpdate(){

		if(gameState == 0){
			server.clients.forEach(c => {
				if (c.readyToStart != true) return;
				const pack = PacketBuilder.start(c.seatAtTable, server.clients.length);
				c.sendPacket(pack);
			});

			gameState = 1;
		}

		if(gameState == 1){
			server.clients.forEach(c => {
				const packet = PacketBuilder.hand(c);
				c.sendPacket(packet);
			});

			gameState = 2;
		}

		if(gameState == 2){
			server.clients.forEach(c => {
				if (c.hasPlayerPassed) handleCardPass(c); 
			});

			gameState = 3;
		}

		if(gameState == 3){
			server.clients.forEach(client => {
				if(client.score >= 100) client.hasLost = true
				if (client.hasLost == true) whoHasLost = client.seatAtTable
				if (client.hand.length == 0) scoreCounter++;
			});
		}

		if(scoreCounter == 8) gameState = 4;

		const packet = PacketBuilder.update(this);
		Server.broadcastPacket(packet);

		handleTablePot();

		if(scoreCounter == 8 && gameState == 4){
			handleScoring();
			scoreCounter = 8;
		}

		firstCard = null;

	},
	handleCardPass(client){

		switch(passcounter){
			case 1:
				switch(client.seatAtTable){
					case 1
						const packet = PacketBuilder.pass(4, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 2
						const packet = PacketBuilder.pass(5, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 3
						const packet = PacketBuilder.pass(6, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 4
						const packet = PacketBuilder.pass(7, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 5
						const packet = PacketBuilder.pass(8, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 6
						const packet = PacketBuilder.pass(1, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 7
						const packet = PacketBuilder.pass(2, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 8
						const packet = PacketBuilder.pass(3, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
				}
				break;
			case 2:
				switch(client.seatAtTable){
					case 1
						const packet = PacketBuilder.pass(, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 2
						const packet = PacketBuilder.pass(7, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 3
						const packet = PacketBuilder.pass(8, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 4
						const packet = PacketBuilder.pass(1, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 5
						const packet = PacketBuilder.pass(2, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 6
						const packet = PacketBuilder.pass(3, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 7
						const packet = PacketBuilder.pass(4, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 8
						const packet = PacketBuilder.pass(5, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
				}
				break;
			case 3:
				switch(client.seatAtTable){
					case 1
						const packet = PacketBuilder.pass(5, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 2
						const packet = PacketBuilder.pass(8, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 3
						const packet = PacketBuilder.pass(7, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 4
						const packet = PacketBuilder.pass(6, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 5
						const packet = PacketBuilder.pass(1, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 6
						const packet = PacketBuilder.pass(4, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 7
						const packet = PacketBuilder.pass(3, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 8
						const packet = PacketBuilder.pass(2, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
				}
				break;
			case 4:switch(client.seatAtTable){
					case 1
						const packet = PacketBuilder.pass(1, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 2
						const packet = PacketBuilder.pass(2, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 3
						const packet = PacketBuilder.pass(3, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 4
						const packet = PacketBuilder.pass(4, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 5
						const packet = PacketBuilder.pass(5, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 6
						const packet = PacketBuilder.pass(6, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 7
						const packet = PacketBuilder.pass(7, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
					case 8
						const packet = PacketBuilder.pass(8, client.pass[0],client.pass[1],client.pass[2]);
						client.sendPacket(packet);
						client.hasPlayerPassed = false;
						client.pass = [];
						break;
				}
				break;
		}

	},
	handleTablePot(){
		var pCount = server.clients.length;

		if (tablePot.length == pCount) {
			var largestCard = Math.max(tablePot);
			server.clients.forEach(c => {
				if(c.cardPlayed == largestCard){
					const packet = PacketBuilder.tpot(c, tablePot);
					c.sendPacket(packet);
					c.cardPlayed = 0;
				}
				else c.cardPlayed = 0; 
			});
		}
	},
	handleScoring(){

		var tempScore;

		server.clients.forEach(client => {
			client.pot.forEach(card => { 

				switch(card){
					case 27 || 28 || 29 || 30 || 31 || 32 || 33 || 34 || 35 || 36 || 37 || 38 || 39:
						tempScore += 1;
						break;
					case 12:
						tempScore += 13;
						break;
				}

			});

			if(tempScore == 26){
			
			server.clients.forEach(c => { c.score += 50 });
			tempScore = 0;

			}
			else  {
				client.score += tempScore;
				tempScore = 0;
			}

		});

	},
	countValsInArray(array, thing){

		let count = 0;
		for(let i = 0; i < array.length; i++){
			if (array[i] == thing) {
				count ++;
			}
		}

		return count;

	},

	checkForValidCard(card){

		if (firstCard == null) firstCard = card;

		if(oneDeck.includes(card))
		{

			if (server.clients.length >= 5 && countValsInArray(cardsPlayedThisRound, card) >= 2) return false;

			if (server.clients.length <= 4 && cardsPlayedThisRound.includes(card)) return false;

			cardsPlayedThisRound.push(card);

			return true;

		}

	},

};

Server.start(Game);

