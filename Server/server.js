const Client = require("./client.js").Client;
const PacketBuilder = require("./packet-builder.js").PacketBuilder;


exports.Server = {

	port:320,
	clients: [],
	maxConnectedUsers:10,
	start(game){

		this.game = game;

		this.socket = require("net").createServer({}, c=>this.onClientConnect(c));
		this.socket.on("error", e=>this.onError(e));
		this.socket.listen({port:this.port},()=>this.onStartListen());
	},
	onClientConnect(socket){
		console.log("new connection from "+socket.localAddress);

		if(this.isServerFull()){

			const packet = PacketBuilder.join(15);

			socket.end(packet); // end connection w/ this client (REJECTED!)

		}
		else {

			const client = new Client(socket, this);
			this.clients.push(client);

		}

		
	},
	onClientDisconect(client){

		if(this.game.client1 == client) this.game.client1 = null;
		if(this.game.client2 == client) this.game.client2 = null;
		if(this.game.client3 == client) this.game.client3 = null;
		if(this.game.client4 == client) this.game.client4 = null;
		if(this.game.client5 == client) this.game.client5 = null;
		if(this.game.client6 == client) this.game.client6 = null;
		if(this.game.client7 == client) this.game.client7 = null;
		if(this.game.client8 == client) this.game.client8 = null;


		const index = this.client.indexOf(client); //find object in array
		if(index >= 0) this.clients.splice(index, 1); // remove the object from the array
	},
	onError(e){
		console.log("ERROR with listener: "+e);
	},
	onStartListen(){
		console.log("Server is now listening on port "+this.port);
	},
	isServerFull(){
		return (this.clients.length >= this.maxConnectedUsers);
	},
	// this function returns a response id
	generateResponseID(desiredUsername, client){

					if(desiredUsername.length <= 3) return 10; //username to short

					if(desiredUsername.length >= 12) return 11; // username to long

					const regex1 = /^[a-zA-Z0-9]+$/; // literal regex in javascript

					if(!regex1.test(desiredUsername)) return 12; // uses invalid characters

					let isUsernameTaken = false;
					this.server.clients.forEach(c=>{

						if(c == client) return;
						if(c.username == desiredUsername) isUsernameTaken = true;
					});

					if(isUsernameTaken) return 13;

					const regex2 = /(fuck|shit|damn|ass|cunt|faggot|)/i;
					if(regex2.test(desiredUsername)) return 14;

					//TODO: Finish with response ids: 1/2/3

					if(this.game.client1 == client) return 1; // you should be client 1
					
					if(this.game.client2 == client) return 2; // you should be client 2

					if(this.game.client3 == client) return 3; // you should be client 3
					if(this.game.client4 == client) return 4; // you should be client 4
					if(this.game.client5 == client) return 5; // you should be client 5
					if(this.game.client6 == client) return 6; // you should be client 6
					if(this.game.client7 == client) return 7; // you should be client 7
					if(this.game.client8 == client) return 8; // you should be client 8
					
					if(!this.game.client1) {
						this.game.client1 = client;
						return 1; // you should be client 1
					}
					if(!this.game.client2) {
						this.game.client2 = client;
						return 2; // you should be client 2
					}
					if(!this.game.client3) {
						this.game.client3 = client;
						return 3; // you should be client 3
					}
					if(!this.game.client4) {
						this.game.client4 = client;
						return 4; // you should be client 4
					}
					if(!this.game.client5) {
						this.game.client5 = client;
						return 5; // you should be client 5
					}
					if(!this.game.client6) {
						this.game.client6 = client;
						return 6; // you should be client 6
					}
					if(!this.game.client7) {
						this.game.client7 = client;
						return 7; // you should be client 7
					}
					if(!this.game.client8) {
						this.game.client8 = client;
						return 8; // you should be client 8
					}

					return 9; // you are a spectator


	},
	broadcastPacket(packet){
		this.clients.forEach(c=>{
			if(c.username){
				c.sendPacket(packet);
			}
		});
	},

};
