using Exam_Movie.Configuration;
using Exam_Movie.DTOs;
using Exam_Movie.Helpers;
using RestSharp;
using RestSharp.Authenticators;

namespace Exam_Movie.Clients
{
    public class MovieApiClient
    {
        private readonly RestClient client;

        public MovieApiClient()
        {
            string jwtToken = AuthHelper.GetJwtToken();

            RestClientOptions options = new RestClientOptions(TestConfig.BaseUrl)
            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };

            client = new RestClient(options);
        }

        public RestResponse CreateMovie(MovieDTO movie)
        {
            RestRequest request = new RestRequest("/api/Movie/Create", Method.Post);
            request.AddJsonBody(movie);

            return client.Execute(request);
        }

        public RestResponse EditMovie(string movieId, MovieDTO movie)
        {
            RestRequest request = new RestRequest("/api/Movie/Edit", Method.Put);
            request.AddQueryParameter("movieId", movieId);
            request.AddJsonBody(movie);

            return client.Execute(request);
        }

        public RestResponse GetAllMovies()
        {
            RestRequest request = new RestRequest("/api/Catalog/All", Method.Get);

            return client.Execute(request);
        }

        public RestResponse DeleteMovie(string movieId)
        {
            RestRequest request = new RestRequest("/api/Movie/Delete", Method.Delete);
            request.AddQueryParameter("movieId", movieId);

            return client.Execute(request);
        }
    }
}