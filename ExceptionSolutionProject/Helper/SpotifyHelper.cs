using SpotifyAPI.Web;

namespace ExceptionSolutionProject.Helper
{
    public class SpotifyHelper
    {
        public async Task<FullTrack> GetTrackInfoAsync(string trackId, string _clientId, string _clientSecret)
        {
            string accessToken = await GetAccessTokenAsync(_clientId,_clientSecret);

            var config = SpotifyClientConfig.CreateDefault().WithToken(accessToken);
            var spotify = new SpotifyClient(config);

            // Belirli bir şarkının bilgilerini almak
            var track = await spotify.Tracks.Get(trackId);
            return track;  // FullTrack türünde döndür
        }

        public async Task<string> GetAccessTokenAsync(string _clientId,string _clientSecret)
        {
            var config = SpotifyClientConfig.CreateDefault();

            // Client ID ve Client Secret ile access token almak
            var request = new ClientCredentialsRequest(_clientId, _clientSecret);
            var response = await new OAuthClient(config).RequestToken(request);

            return response.AccessToken; // Elde edilen access token'ı geri döndür
        }

    }
}
