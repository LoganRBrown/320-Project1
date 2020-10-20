const server = require("./server.js");
const PacketBuilder = require("./packet-builder.js").PacketBuilder;


const Game = {
	whoseTurn: 1,
	whoHasWon: 0,
	board: [
		[0,0,0], 
		[0,0,0], 
		[0,0,0]
	],
	clientX:null, //player 1
	clientO:null, // player 2
	playMove(client, x, y){
		if(this.whoHasWon > 0) return;
		if(this.whoseTurn == 1 && client != this.clientX) return;

		if(this.whoseTurn == 2 && client != this.clientO) return;

		if(x < 0) return;
		if(y < 0) return;

		if(y >= this.board.length) return;
		if(x >= this.board[y].length) return;

		if(this.board[y][x] > 0) return; //ignore moves on taken spaces.

		this.board[y][x] = this.whoseTurn;

		this.whoseTurn = (this.whoseTurn == 1) ? 2 : 1; // toggles the turn to the next player

		this.checkStateAndUpdate();
	},
	checkStateAndUpdate(){

		const packet = PacketBuilder.update(this);
		Server.broadcastPacket(packet);

	},

};


Server.start(Game);

