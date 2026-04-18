using Exam_Movie.DTOs;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using System.Text.Json;





namespace Exam_Movie
{
    [TestFixture]
    public class Tests
    {
        private const string BaseUrl = "http://144.91.123.158:5000";

        private const string LoginEmail = "Stoyan@Test.com";
        private const string LoginPassword = "Test123";

        private RestClient client;
        private static string movieId;

        [OneTimeSetUp]
        public void Setup()
        {
            string jwtToken = GetJwtToken(LoginEmail, LoginPassword);
            RestClientOptions options = new RestClientOptions(BaseUrl)
            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };
            this.client = new RestClient(options);
        }
        private string GetJwtToken(string email, string password)
        {
            RestClient client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest("/api/User/Authentication", Method.Post);
            request.AddJsonBody(new { email, password });
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var token = content.GetProperty("accessToken").GetString();

                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException("Token is not found in the response.");
                }
                return token;
            }
            else
            {
                throw new InvalidOperationException($"Failed to retrieve token. Status code: {response.StatusCode}, Response: {response.Content}");
            }
        }

        [Order(1)]
        [Test]
        public void CreateMovie_WithRequiredFileds_ShouldSuccess()
        {
            //Arrange
            MovieDTO movie = new MovieDTO
            {
                Title = "Fight Club",
                Description = "This is a test movie."
            };

            RestRequest request = new RestRequest("/api/Movie/Create", Method.Post);
            request.AddJsonBody(movie);

            //Act
            RestResponse response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse.Movie.Id, Is.Not.Null);
            Assert.That(readyResponse.Movie.Id, Is.Not.Empty);
            Assert.That(readyResponse.Msg, Is.EqualTo("Movie created successfully!"));
            movieId = readyResponse.Movie.Id;

        }

        [Order(2)]
        [Test]
        public void EditMovieTitle_ShouldChangeTitle()
        {
            //Arrange

            Console.WriteLine($"movieId before edit: '{movieId}'");
            Assert.That(movieId, Is.Not.Null.And.Not.Empty);


            MovieDTO movie = new MovieDTO
            {
                Id = movieId,
                Title = "Home alone",
                Description = "This is a test movie."
            };
            RestRequest request = new RestRequest("/api/Movie/Edit", Method.Put);
            request.AddQueryParameter("movieId", movieId);
            request.AddJsonBody(movie);
            //Act
            RestResponse response = this.client.Execute(request);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            Assert.That(readyResponse.Msg, Is.EqualTo("Movie edited successfully!"));
        }

        [Order(3)]
        [Test]
        public void GetAllMovies_ShouldReturnListOfMovies()
        {
            //Arrange
            RestRequest request = new RestRequest("/api/Catalog/All", Method.Get);
            //Act
            RestResponse response = this.client.Execute(request);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            List<MovieDTO> readyResponse = JsonSerializer.Deserialize<List<MovieDTO>>(response.Content);
            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse, Is.Not.Empty);
            Assert.That(readyResponse.Count, Is.GreaterThan(0));

        }

        [Order(4)]
        [Test]
        public void DeleteExistingMovie_ShouldDeleteMovie()
        {
            //Arrange
            RestRequest request = new RestRequest("/api/Movie/Delete", Method.Delete);
            request.AddQueryParameter("movieId", movieId);
            //Act
            RestResponse response = this.client.Execute(request);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            Assert.That(readyResponse.Msg, Is.EqualTo("Movie deleted successfully!"));
        }

        [Order(5)]
        [Test]
        public void CreateMovie_WithoutRequiredFields_ShouldReturnBadRequest()
        {
            //Arrange
            MovieDTO movie = new MovieDTO
            {
                Title = "",
                Description = ""
            };
            RestRequest request = new RestRequest("/api/Movie/Create", Method.Post);
            request.AddJsonBody(movie);
            //Act
            RestResponse response = this.client.Execute(request);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Order(6)]
        [Test]
        public void EditNonExistingMovie_ShouldReturnBadRequest()
        {
            //Arrange
            MovieDTO movie = new MovieDTO
            {
                Id = "12345",
                Title = "Titanic",
                Description = "Some description"
            };
            RestRequest request = new RestRequest("/api/Movie/Edit", Method.Put);
            request.AddQueryParameter("movieId", "12345");
            request.AddJsonBody(movie);
            //Act
            RestResponse response = this.client.Execute(request);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            Assert.That(readyResponse.Msg, Is.EqualTo("Unable to edit the movie! Check the movieId parameter or user verification!"));

        }

        [Order(7)]
        [Test]
        public void DeleteNonExistingMovie_ShouldReturnBadRequest()
        {
            //Arrange
            RestRequest request = new RestRequest("/api/Movie/Delete", Method.Delete);
            request.AddQueryParameter("movieId", "12345");
            //Act
            RestResponse response = this.client.Execute(request);
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            ApiResponseDTO readyResponse = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            Assert.That(readyResponse.Msg, Is.EqualTo("Unable to delete the movie! Check the movieId parameter or user verification!"));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            this.client.Dispose();
        }
    }
}
