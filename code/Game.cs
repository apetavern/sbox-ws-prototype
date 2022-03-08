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
			wsClient = new( "ws://127.0.0.1:8080" );

			if ( IsServer )
			{
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

			Log.Info( $"{Host.Name}: We are connected." );

			// Attempt to authenticate to WS Server.
			OutgoingMessage message = new();
			message.MessageType = 0;

			if ( IsServer )
			{
				message.ServerId = "dev_server";
				message.IsServer = true;
			}
			else
			{
				message.PlayerId = Local.PlayerId.ToString();
				message.PlayerName = Local.DisplayName;
			}

			message.Token = TokenManager.GetToken();

			await wsClient.Send( message );
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			// Someone please tell me if this is stupid.
			ClientInitializeWS( To.Single( client ) );

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

		/// <summary>
		/// Someone please tell me if this is stupid.
		/// </summary>
		[ClientRpc]
		public void ClientInitializeWS()
		{
			InitializeWSConnection();
		}
	}

}
