using Exam_Movie.Configuration;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace Exam_Movie.Helpers
{
    public static class AuthHelper
    {
        public static string GetJwtToken()
        {
            RestClient client = new RestClient(TestConfig.BaseUrl);

            RestRequest request = new RestRequest("/api/User/Authentication", Method.Post);
            request.AddJsonBody(new
            {
                email = TestConfig.LoginEmail,
                password = TestConfig.LoginPassword
            });

            RestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException(
                    $"Failed to retrieve token. Status code: {response.StatusCode}, Response: {response.Content}");
            }

            JsonElement content = JsonSerializer.Deserialize<JsonElement>(response.Content);
            string? token = content.GetProperty("accessToken").GetString();

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Token is not found in the response.");
            }

            return token;
        }
    }
}