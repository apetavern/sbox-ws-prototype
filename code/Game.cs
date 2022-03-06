using Sandbox;
using System;
using System.Linq;

namespace Proto
{
	public partial class Prototype : Game
	{
		private readonly WebSocketClient wsClient;

		public Prototype()
		{
			if ( IsClient )
			{
				wsClient = new( "ws://127.0.0.1:8080" );
				InitializeWSConnection();
			}
		}

		public async void InitializeWSConnection()
		{
			bool connected = await wsClient.Connect();
			if ( connected )
			{
				Log.Info( "Successfully connected to the WebSocket Server" );
			}
			else
			{
				Log.Error( "Could not connect to WS Server. Is it running?" );
			}

			Log.Info( "We are connected." );

			OutgoingMessage message = new();
			message.MessageType = 0;
			message.PlayerId = Local.PlayerId.ToString();
			message.PlayerName = Local.DisplayName;
			message.Token = TokenManager.GetToken();
			message.Text = "Client entry message.";

			wsClient.Send( message );
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			// Create a pawn for this client to play with
			var pawn = new Pawn();
			client.Pawn = pawn;

			// Get all of the spawnpoints
			var spawnpoints = Entity.All.OfType<SpawnPoint>();

			// chose a random one
			var randomSpawnPoint = spawnpoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

			// if it exists, place the pawn there
			if ( randomSpawnPoint != null )
			{
				var tx = randomSpawnPoint.Transform;
				tx.Position = tx.Position + Vector3.Up * 50.0f; // raise it up
				pawn.Transform = tx;
			}
		}
	}

}
