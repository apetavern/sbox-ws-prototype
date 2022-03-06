﻿using System.Text.Json.Serialization;

namespace Proto
{
	public class IncomingMessage
	{
		[JsonPropertyName( "MessageType" )]
		public int MessageType { get; set; }

		[JsonPropertyName( "Message" )]
		public string Text { get; set; }
	}
}
