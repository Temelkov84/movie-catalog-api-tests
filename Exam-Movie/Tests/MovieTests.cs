using Exam_Movie.Clients;
using Exam_Movie.DTOs;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Text.Json;

namespace Exam_Movie.Tests
{
    [TestFixture]
    public class MovieTests
    {
        private MovieApiClient movieApiClient;
        private readonly List<string> createdMovieIds = new List<string>();

        [OneTimeSetUp]
        public void Setup()
        {
            movieApiClient = new MovieApiClient();
        }

        private string CreateTestMovie()
        {
            MovieDTO movie = new MovieDTO
            {
                Title = $"Test Movie {Guid.NewGuid()}",
                Description = "Test Description"
            };

            RestResponse response = movieApiClient.CreateMovie(movie);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ApiResponseDTO readyResponse =
                JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse.Movie, Is.Not.Null);
            Assert.That(readyResponse.Movie.Id, Is.Not.Null.And.Not.Empty);

            createdMovieIds.Add(readyResponse.Movie.Id);

            return readyResponse.Movie.Id;
        }

        [Test]
        public void CreateMovie_WithRequiredFields_ShouldSucceed()
        {
            MovieDTO movie = new MovieDTO
            {
                Title = $"Fight Club {Guid.NewGuid()}",
                Description = "This is a test movie."
            };

            RestResponse response = movieApiClient.CreateMovie(movie);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ApiResponseDTO readyResponse =
                JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse.Movie, Is.Not.Null);
            Assert.That(readyResponse.Movie.Id, Is.Not.Null.And.Not.Empty);
            Assert.That(readyResponse.Movie.Title, Is.EqualTo(movie.Title));
            Assert.That(readyResponse.Movie.Description, Is.EqualTo(movie.Description));
            Assert.That(readyResponse.Msg, Is.EqualTo("Movie created successfully!"));

            createdMovieIds.Add(readyResponse.Movie.Id);
        }

        [Test]
        public void EditMovie_WithValidData_ShouldReturnUpdatedMovie()
        {
            string movieId = CreateTestMovie();

            MovieDTO movie = new MovieDTO
            {
                Id = movieId,
                Title = $"Updated Title {Guid.NewGuid()}",
                Description = "Updated Description"
            };

            RestResponse response = movieApiClient.EditMovie(movieId, movie);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            ApiResponseDTO readyResponse =
                JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse.Movie, Is.Not.Null);
            Assert.That(readyResponse.Movie.Id, Is.EqualTo(movieId));
            Assert.That(readyResponse.Movie.Title, Is.EqualTo(movie.Title));
            Assert.That(readyResponse.Movie.Description, Is.EqualTo(movie.Description));
            Assert.That(readyResponse.Msg, Is.EqualTo("Movie edited successfully!"));
        }

        [Test]
        public void GetAllMovies_ShouldReturnListOfMovies()
        {
            RestResponse response = movieApiClient.GetAllMovies();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            List<MovieDTO> readyResponse =
                JsonSerializer.Deserialize<List<MovieDTO>>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse, Is.Not.Empty);
            Assert.That(readyResponse.Count, Is.GreaterThan(0));
        }

        [Test]
        public void DeleteExistingMovie_ShouldDeleteMovie()
        {
            string movieId = CreateTestMovie();

            RestResponse response = movieApiClient.DeleteMovie(movieId);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Null.And.Not.Empty);

            ApiResponseDTO readyResponse =
                JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(readyResponse.Msg, Is.EqualTo("Movie deleted successfully!"));

            createdMovieIds.Remove(movieId);
        }

        [Test]
        public void CreateMovie_WithoutRequiredFields_ShouldReturnBadRequest()
        {
            MovieDTO movie = new MovieDTO
            {
                Title = "",
                Description = ""
            };

            RestResponse response = movieApiClient.CreateMovie(movie);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void EditNonExistingMovie_ShouldReturnBadRequest()
        {
            MovieDTO movie = new MovieDTO
            {
                Id = "12345",
                Title = "Titanic",
                Description = "Some description"
            };

            RestResponse response = movieApiClient.EditMovie("12345", movie);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            ApiResponseDTO readyResponse =
                JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(
                readyResponse.Msg,
                Is.EqualTo("Unable to edit the movie! Check the movieId parameter or user verification!"));
        }

        [Test]
        public void DeleteNonExistingMovie_ShouldReturnBadRequest()
        {
            RestResponse response = movieApiClient.DeleteMovie("12345");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            ApiResponseDTO readyResponse =
                JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(readyResponse, Is.Not.Null);
            Assert.That(
                readyResponse.Msg,
                Is.EqualTo("Unable to delete the movie! Check the movieId parameter or user verification!"));

        }

        [TearDown]
        public void Cleanup()
        {
            foreach (string movieId in createdMovieIds)
            {
                movieApiClient.DeleteMovie(movieId);
            }

            createdMovieIds.Clear();
        }
    }
}