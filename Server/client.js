
const PacketBuilder = require("./packet-builder.js").PacketBuilder;


exports.Client = class Client {
		constructor(sock, server){

				this.socket = sock;
				this.server = server;
				this.username = "";
				this.hand = [];
				this.pot = [];
				this.pass = [];
				this.cardPlayed = 0;
				this.score = 0;
				this.hasLost = false;
				this.seatAtTable = 0;
				this.readyToStart = false;
				this.hasPlayerPassed = false;

				this.buffer = Buffer.alloc(0);

				this.socket.on("error",(e)=>this.onError(e));
				this.socket.on("close",()=>this.onClose());
				this.socket.on("data",(d)=>this.onData(d));

		}
		onError(errMsg){
			console.log("ERROR with Client: "+errMsg);
		}
		onClose(){
			this.server.onClientDisconnect(this);
		}
		onData(data){

			// add new data to buffer:
			this.buffer += data;

			//parse buffer for packets (process the packets)

			if(this.buffer.length < 4) return;

			const packetIndetifier = this.buffer.slice(0, 4).toString();

			switch(packetIndetifier){
				case "JOIN": 
					if(this.buffer.length < 5) return; // not enough data
					const lengthOfUsername = this.buffer.readUInt8(4);

					if(this.buffer.length < 5 + lengthOfUsername) return;
					const desiredUsername = this.buffer.slice(5, 5+lengthOfUsername).toString();

					// TODO: Check username...

					let responseType = this.server.generateResponseID(desiredUsername, this);
					
					//buld and send packet
					const packet = PacketBuilder.join(responseType);
					this.seatAtTable = responseType;
					this.sendPacket(packet);

					// consume data out of the buffer
					this.buffer = this.buffer.slice(5 + lengthOfUsername);

					const packet2 = PacketBuilder.update(this.server.game);
					this.sendPacket(packet2);

					break;
				case "CHAT":
					if (this.buffer.length < 5) return;
					const lengthOfMessage = this.buffer.readUInt16BE(4);

					const desiredMessage = this.buffer.slice(5, 5+lengthOfMessage).toString();

					const packet3 = PacketBuilder.chat(this,lengthOfUsername,lengthOfMessage, desiredMessage);
					this.server.broadcastPacket(packet3);



					break;
				case "PLAY": 
					if(this.buffer.length < 6) return; //not enough data

					const x = this.buffer.readUInt8(4);
					const y = this.buffer.readUInt8(5);

					console.log("user wants to play card: " +x+" "+y);

					this.buffer = this.buffer.slice(6);

					var serverCardVal = convertCardValues(x,y);

					cardPlayed = serverCardVal;

					this.server.game.playMove(this, serverCardVal);
					
					break;

				case "HAND":

					if(this.buffer.length < 6) return;

					this.buffer = this.buffer.slice(32);

					const playerNumber = this.buffer.readUInt8(4);

					if(playerNumber = seatAtTable){

						const suit =[];
						const val = [];

						suit.push(this.buffer.readUInt8(5));
						suit.push(this.buffer.readUInt8(7));
						suit.push(this.buffer.readUInt8(9));
						suit.push(this.buffer.readUInt8(11));
						suit.push(this.buffer.readUInt8(13));
						suit.push(this.buffer.readUInt8(15));
						suit.push(this.buffer.readUInt8(17));
						suit.push(this.buffer.readUInt8(19));
						suit.push(this.buffer.readUInt8(21));
						suit.push(this.buffer.readUInt8(23));
						suit.push(this.buffer.readUInt8(25));
						suit.push(this.buffer.readUInt8(27));
						suit.push(this.buffer.readUInt8(29));

						val.push(this.buffer.readUInt8(6));
						val.push(this.buffer.readUInt8(8));
						val.push(this.buffer.readUInt8(10));
						val.push(this.buffer.readUInt8(12));
						val.push(this.buffer.readUInt8(14));
						val.push(this.buffer.readUInt8(16));
						val.push(this.buffer.readUInt8(18));
						val.push(this.buffer.readUInt8(20));
						val.push(this.buffer.readUInt8(22));
						val.push(this.buffer.readUInt8(24));
						val.push(this.buffer.readUInt8(26));
						val.push(this.buffer.readUInt8(28));
						val.push(this.buffer.readUInt8(30));

						for (var i = 0; i <= 13; i++) {
							if(suit[i] && val[i] != null){
								this.hand.push(convertCardValues(suit[i], val[i]));
							}
						}

						const tempScore = this.buffer.readUInt8(30);

						this.score = tempScore;
					}

					break;

				case "PASS":

					const psuit = [];
					const pval = [];

					suit.push(this.buffer.readUInt8(5));
					val.push(this.buffer.readUInt8(6));
					suit.push(this.buffer.readUInt8(7));
					val.push(this.buffer.readUInt8(8));
					suit.push(this.buffer.readUInt8(9));
					val.push(this.buffer.readUInt8(10));

					for (var i = 0; i <= 3; i++) {
						if(suit[i] && val[i] != null){
								this.pass.push(convertCardValues(psuit[i], pval[i]));
							}
					}

					this.hasPlayerPassed = true;

					break;

					case "PPOT":

						if(this.buffer.length < 6) return;

						const buffSize = this.buffer.readUInt8(5);

						this.buffer = this.buffer.slice(6, 6 + buffSize);

						playerNumber = this.buffer.readUInt8(4);

					if(playerNumber = seatAtTable){

						var tempSuit;
						var tempValue;

						for (var i = 0; i <= buffSize; i++) {
							tempSuit = this.buffer.readUInt8(i);
							tempValue = this.buffer.readUInt8(i + 1);
							this.pot.push(convertCardValues(tempSuit, tempValue));
						}

					}

						break;

				case "STRT":

					if (this.buffer.length < 4 ) return;

					readyToStart = true;

					break;

				case "RSCR":
					const packet4 = PacketBuilder.rscr();
					sendPacket(packet4);
					break;

				default:
					// don't recognize the packet....
					console.log("ERROR: Packet identifier not recognized ("+packetIndetifier+")");
					this.buffer = buffer.alloc(0);
					break;
			}

			// process packets (and consume data from buffer)

		}

		sendPacket(packet){
			this.socket.write(packet);
		}
		convertCardValues(suit, value){

			switch(suit){
				case 0:

					switch(value){
						case 0:
							return 1;
							break;
						case 1:
							return 2;
							break;
						case 2:
							return 3;
							break;
						case 3:
							return 4;
							break;
						case 4:
							return 5;
							break;
						case 5:
							return 6;
							break;
						case 6:
							return 7;
							break;
						case 7:
							return 8;
							break;
						case 8:
							return 9;
							break;
						case 9:
							return 10;
							break;
						case 10:
							return 11;
							break;
						case 11:
							return 12;
							break;
						case 12:
							return 13;
							break;
					}

					break;
				case 1:

					switch(value){
						case 0:
							return 14;
							break;
						case 1:
							return 15;
							break;
						case 2:
							return 16;
							break;
						case 3:
							return 17;
							break;
						case 4:
							return 18;
							break;
						case 5:
							return 19;
							break;
						case 6:
							return 20;
							break;
						case 7:
							return 21;
							break;
						case 8:
							return 22;
							break;
						case 9:
							return 23;
							break;
						case 10:
							return 24;
							break;
						case 11:
							return 25;
							break;
						case 12:
							return 26;
							break;
					}

					break;
				case 2:

					switch(value){
						case 0:
							return 27;
							break;
						case 1:
							return 28;
							break;
						case 2:
							return 29;
							break;
						case 3:
							return 30;
							break;
						case 4:
							return 31;
							break;
						case 5:
							return 32;
							break;
						case 6:
							return 33;
							break;
						case 7:
							return 34;
							break;
						case 8:
							return 35;
							break;
						case 9:
							return 36;
							break;
						case 10:
							return 37;
							break;
						case 11:
							return 38;
							break;
						case 12:
							return 39;
							break;
					}

					break;
				case 3:

					switch(value){
						case 0:
							return 40;
							break;
						case 1:
							return 41;
							break;
						case 2:
							return 42;
							break;
						case 3:
							return 43;
							break;
						case 4:
							return 44;
							break;
						case 5:
							return 45;
							break;
						case 6:
							return 46;
							break;
						case 7:
							return 47;
							break;
						case 8:
							return 48;
							break;
						case 9:
							return 49;
							break;
						case 10:
							return 50;
							break;
						case 11:
							return 51;
							break;
						case 12:
							return 52;
							break;
					}

					break;
				default:
					return null;
					break;
			}

		}


};