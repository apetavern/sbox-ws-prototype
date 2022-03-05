using Sandbox;
using System.Text.Json;
using System.Threading.Tasks;

namespace Proto
{
	/// <summary>
	/// A basic WebSocketClient for sbox.
	/// </summary>
	public class WebSocketClient
	{
		/// Declare our WebSocket object.
		private readonly WebSocket ws;

		/// Declare our connection string.
		private readonly string connectionString;

		/// <summary>
		/// Constructor for our WebSocketClient.
		/// </summary>
		/// <param name="connectionString">
		/// The connection string this client will use to connect.
		/// </param>
		public WebSocketClient( string connectionString )
		{
			ws = new();
			this.connectionString = connectionString;

			ws.OnMessageReceived += OnMessageReceived;
			ws.OnDisconnected += OnDisconnected;
		}

		/// <summary>
		/// Connect to the WebSocket Server.
		/// </summary>
		/// <returns>Whether connection was successful.</returns>
		public async Task<bool> Connect()
		{
			await ws.Connect( connectionString );
			return ws.IsConnected;
		}

		/// <summary>
		/// Disconnect from the WebSocket Server.
		/// </summary>
		/// <returns>Whether we successfully disconnected from the server.</returns>
		public bool Disconnect()
		{
			ws.Dispose();
			return !ws.IsConnected;
		}

		/// <summary>
		/// Sends a message to the WebSocket Server.
		/// </summary>
		/// <param name="message">The message to be sent.</param>
		public async void Send( Message message )
		{
			string jsonMessage = JsonSerializer.Serialize( message );
			await ws.Send( jsonMessage );
		}

		/// <summary>
		/// Handler for received messages.
		/// </summary>
		/// <param name="message">
		/// The message received from the WebSocket Server.
		/// </param>
		private void OnMessageReceived( string message )
		{
			// TODO: Handle incoming messages.
			Log.Info( message );

		}

		/// <summary>
		/// Handler for when the client disconnects from the WebSocket Server.
		/// </summary>
		private void OnDisconnected( int status, string reason )
		{
			Log.Info( $"Disconnected from WebSocket Server with exit code {status} and reason {reason}." );
		}
	}
}
