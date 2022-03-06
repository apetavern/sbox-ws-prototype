using System.Text.Json.Serialization;

namespace Proto
{
	public class OutgoingMessage
	{
		[JsonPropertyName( "MessageType" )]
		public int MessageType { get; set; }

		[JsonPropertyName( "PlayerId" )]
		public string PlayerId { get; set; }

		[JsonPropertyName( "PlayerName" )]
		public string PlayerName { get; set; }

		[JsonPropertyName( "X-Auth-Token" )]
		public string Token { get; set; }

		[JsonPropertyName( "Message" )]
		public string Text { get; set; }
	}
}
