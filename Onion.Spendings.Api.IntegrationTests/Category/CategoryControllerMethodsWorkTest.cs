using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Newtonsoft.Json;
using System.Text;

namespace Onion.Spendings.Api.IntegrationTests.Category
{
   public class CategoryControllerMethodsWorkTest :
    IClassFixture<CustomWebApplicationFactory<onion_spendings.Startup>>
    {

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<onion_spendings.Startup>
            _factory;

        public CategoryControllerMethodsWorkTest(
            CustomWebApplicationFactory<onion_spendings.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task CategoryPostAsync_IfMethodWorks_ReturnOk()
        {
            // Arrange
            global::Spendings.Orchrestrators.Categories.Category category = new global::Spendings.Orchrestrators.Categories.Category
            {
                Name = "food"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/Category")
            {
                Content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
            };
            //Act
            var responce = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);
        }

        [Fact]
        public async Task CategoryGetAsync_IfReturnsCorrectModel_ReturnOk()
        {
            // Arrange

            global::Spendings.Orchrestrators.Categories.Category added = new global::Spendings.Orchrestrators.Categories.Category
            {
                Name = "games"
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/Category")
            {
                Content = new StringContent(JsonConvert.SerializeObject(added), Encoding.UTF8, "application/json")
            };

            //Act
            var postResponce = await _client.SendAsync(postRequest);
            var byteResult = await postResponce.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var category = JsonConvert.DeserializeObject<global::Spendings.Core.Categories.Category>(stringResult);

            var getResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/Category?categoryId={category.Id}"));

            byteResult = await getResponse.Content.ReadAsByteArrayAsync();
            stringResult = Encoding.UTF8.GetString(byteResult);
            category = JsonConvert.DeserializeObject<global::Spendings.Core.Categories.Category>(stringResult);

            // Assert
            postResponce.EnsureSuccessStatusCode();
            getResponse.EnsureSuccessStatusCode();
            Assert.Equal(added.Name, category.Name);

        }

        [Fact]
        public async Task CategoryPostAsync_IfThrowsExceptionWhenAddingExistingCategory_ReturnOk()
        {
            // Arrange
            global::Spendings.Orchrestrators.Categories.Category category = new global::Spendings.Orchrestrators.Categories.Category
            {
                Name = "books"
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/Category")
            {
                Content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
            };
            var copyOfPostRequest = new HttpRequestMessage(HttpMethod.Post, $"/Category")
            {
                Content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
            };

            //Act
            var postResponce = await _client.SendAsync(postRequest);
            var exception = await Assert.ThrowsAsync<global::Spendings.Core.Exeptions.FailedInsertionException>(async () => await _client.SendAsync(copyOfPostRequest));

            // Assert
            postResponce.EnsureSuccessStatusCode();
            Assert.NotNull(exception);

        }
    }
}
