using Sandbox;
using System.Text.Json.Serialization;

namespace Proto
{
	public static class TokenManager
	{
		private static readonly string NO_TOKEN = "NO_TOKEN";
		private static string TOKEN_FILENAME = "token.json";
		private static bool INITIALIZED = false;

		/// <summary>
		/// Initialize the static class.
		/// </summary>
		public static void Init()
		{
			if ( Host.IsServer )
			{
				TOKEN_FILENAME = "sv_token.json";
			}
			else
			{
				TOKEN_FILENAME = "cl_token.json";
			}

			INITIALIZED = true;
		}

		/// <summary>
		/// Gets the locally saved token, if it exists.
		/// </summary>
		/// <returns>The token, or NO_TOKEN if the token does not exist.</returns>
		public static string GetToken()
		{
			if ( !INITIALIZED ) Init();

			if ( !FileSystem.Data.FileExists( TOKEN_FILENAME ) )
			{
				return NO_TOKEN;
			}
			else
			{
				TokenWrapper tokenWrapper = FileSystem.Data.ReadJson<TokenWrapper>( TOKEN_FILENAME );
				return tokenWrapper.Token;
			}
		}

		/// <summary>
		/// Locally saves the new token.
		/// </summary>
		/// <param name="newToken">The token obtained from the Ape Tavern backend.</param>
		public static void SaveToken( TokenWrapper newToken )
		{
			if ( !INITIALIZED ) Init();

			Log.Info( "Saving your auth token to your local data folder." );
			Log.Warning( "Please never share this token with anyone!" );
			FileSystem.Data.WriteJson( TOKEN_FILENAME, newToken );
		}
	}

	public class TokenWrapper
	{
		private string token;

		[JsonPropertyName( "token" )]
		public string Token { get { return token; } set { token = value; } }
	}
}
