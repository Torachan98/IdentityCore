using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using IdentityCore.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace IdentityCore.Services
{
    public class FCMService : IFCMService
    {
        private readonly HttpClient _httpClient;

        public FCMService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task SendAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null)
        {
            string _projectId = "charismatic-age-483618-d4";
            GoogleCredential credential = GoogleCredential.GetApplicationDefault().CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

            var accessToken = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            var payload = new
            {
                message = new
                {
                    token = fcmToken,
                    notification = new
                    {
                        title,
                        body
                    },
                    data = data
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://fcm.googleapis.com/v1/projects/{_projectId}/messages:send");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"FCM Error: {responseBody}");
            }
        }
    }
}
