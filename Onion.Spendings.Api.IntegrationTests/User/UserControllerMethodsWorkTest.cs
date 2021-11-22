using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Onion.Spendings.Api.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Spendings.Core.Exeptions;
using Spendings.Orchrestrators.Users;
using Xunit;

namespace Onion.Spendings.Api.Tests.Users
{
    public class UserControllerMethodsWorkTest
    :
    IClassFixture<CustomWebApplicationFactory<onion_spendings.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<onion_spendings.Startup>
            _factory;

        public UserControllerMethodsWorkTest(
            CustomWebApplicationFactory<onion_spendings.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task GetAsync_IfServiceReturnsCorrectUser_ReturnOk()
        {
            // Arrange
            var postedUser = new User
            {
                Password = "sdasdasda",
                Login = "sdaDDSdad"
            };

            var addRequest = new HttpRequestMessage(HttpMethod.Post, "/User")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                    postedUser), 
                    Encoding.UTF8, "application/json")
            };

            //Act
            var addResponse = await _client.SendAsync(addRequest);
            var user = await getModelFromHttpResponce(addResponse);

            var getResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/User?userId={user.Id}"));
            user = await getModelFromHttpResponce(getResponse);

            // Assert
            addResponse.EnsureSuccessStatusCode();
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(postedUser.Login, user.Login);
            Assert.Equal(postedUser.Password, user.Password);
        }

        [Fact]
        public async Task GetAsync_IfServiceThrowsExceptionWhenIdUndefined_ReturnOk()
        {
            // Arrange
            int undefinedId = 99999;

            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/User?userId={undefinedId}");
            //Act

            var exception = await Assert.ThrowsAsync<System.InvalidOperationException>(async () => await _client.SendAsync(getRequest));

            // Assert
            Assert.NotNull(exception);

        }

        [Fact]
        public async Task PostAsync_IfServiceReturnsUser_ReturnOk()
        {
            // Arrange
            var postingUser = new User
            {
                Password = "sdasdasda",
                Login = "sdasdad"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/User")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                    postingUser),
                    Encoding.UTF8, "application/json")
            };

            //Act
            var response = await _client.SendAsync(request);

            var byteResult = await response.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var user = JsonConvert.DeserializeObject<global::Spendings.Core.Users.User>(stringResult);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(postingUser.Login, user.Login);
            Assert.Equal(postingUser.Password, user.Password);
        }

        [Fact]
        public async Task PatcAsync_IfLoginCorrect_ReturnOk()
        {
            // Arrange
            var addedUser = new User
            {
                Password = "password",
                Login = "login"
            };
            var updatedUpdate = new User
            {
                Password = "password",
                Login = "superlogin"
            };
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/User")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                        addedUser),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var postResponce = await _client.SendAsync(postRequest);
            var user = await getModelFromHttpResponce(postResponce);

            var patchResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Patch, $"/User?userId={user.Id}&newLogin={updatedUpdate.Login}"));

            var getResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/User?userId={user.Id}"));
            user = await getModelFromHttpResponce(getResponce);

            // Assert
            postResponce.EnsureSuccessStatusCode();
            patchResponce.EnsureSuccessStatusCode();
            getResponce.EnsureSuccessStatusCode();

            Assert.Equal(updatedUpdate.Login, user.Login);
        }

        [Fact]
        public async Task PostUserWithExistingLogin_IfProgramThrowException_ReturnOk()
        {
            // Arrange
            var user = new User
            {
                Password = "password",
                Login = "loGGin"
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "/User")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                        new User
                        {
                            Password = user.Password,
                            Login = user.Login
                        }),
                    Encoding.UTF8,
                    "application/json")
            };

            var duplicateRequest = new HttpRequestMessage(HttpMethod.Post, "/User")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                        new User
                        {
                            Password = user.Password,
                            Login = user.Login
                        }),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var exception = await Assert.ThrowsAsync<FailedInsertionException>(async () => await _client.SendAsync(duplicateRequest));

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task DeleteUser_IfWorksAndSecondDeleteThrowsException_ReturnOk()
        {
            // Arrange
            var postedUser = new User
            {
                Password = "password",
                Login = "loGGgin"
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/User")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                        postedUser),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var postResponse = await _client.SendAsync(postRequest);
            var user = await getModelFromHttpResponce(postResponse);

            var deleteResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"/User?userId={user.Id}"));
  
            var exception = await Assert.ThrowsAsync<AlreadyDeletedException>(async () => await _client.SendAsync
            (new HttpRequestMessage(HttpMethod.Delete, $"/User?userId={user.Id}")));

            // Assert
            postResponse.EnsureSuccessStatusCode();
            deleteResponce.EnsureSuccessStatusCode();

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task GetAndPatchDeletedUser_IfThrowsException_ReturnOk()
        {
            // Arrange
            var postedUser = new User
            {
                Password = "password",
                Login = "loGsGgin"
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/User")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                        postedUser),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var postResponse = await _client.SendAsync(postRequest);
            var user = await getModelFromHttpResponce(postResponse);

            var deleteResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"/User?userId={user.Id}"));

            var onPostException = await Assert.ThrowsAsync<AlreadyDeletedException>(async () => await _client.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"/User?userId={user.Id}")));
            var onPatchException = await Assert.ThrowsAsync<AlreadyDeletedException>(async () => await _client.SendAsync(
                new HttpRequestMessage(HttpMethod.Patch, $"/User?userId={user.Id}&newLogin=sadasdqwzczxca")));

            // Assert
            postResponse.EnsureSuccessStatusCode();
            deleteResponce.EnsureSuccessStatusCode();
            Assert.NotNull(onPostException);
            Assert.NotNull(onPatchException);
        }

        async Task<global::Spendings.Core.Users.User> getModelFromHttpResponce(HttpResponseMessage responce)
        {
            var byteResult = await responce.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var record = JsonConvert.DeserializeObject<global::Spendings.Core.Users.User>(stringResult);
            return record;
        }
    }
}