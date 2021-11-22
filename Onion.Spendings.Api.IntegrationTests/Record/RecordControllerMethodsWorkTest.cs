using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Onion.Spendings.Api.IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace Onion.Spendings.Api.Tests.Records
{
    public class RecordControllerMethodsWorkTest :
    IClassFixture<CustomWebApplicationFactory<onion_spendings.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<onion_spendings.Startup>
            _factory;

        public RecordControllerMethodsWorkTest(
            CustomWebApplicationFactory<onion_spendings.Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task PostAsync_IfRecordInserted_ReturnOk()
        {
            // Arrange
            int userId = 1;
            global::Spendings.Orchrestrators.Records.Record record = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = new DateTime(2005, 10, 9),
                CategoryId = 1,
                Amount = 1000
            };
            var request = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                       record),
                    Encoding.UTF8,
                    "application/json")
            };

            //Act
            var responce = await _client.SendAsync(request);

            // Assert
            responce.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, responce.StatusCode);

        }

        [Fact]
        public async Task RecordsListGetAsync_IfCorrectListReturned_ReturnOk()
        {
            // Arrange
            int categoryId = 1;
            int userId = 1;
            int amount = 1000;
            DateTime firstInsertedDate = new DateTime(2005, 10, 20);
            DateTime secondInsertedDate = new DateTime(2005, 10, 23);
            DateTime endDate = new DateTime(2005, 10, 26);
            global::Spendings.Orchrestrators.Records.Record firstRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date=firstInsertedDate,
                CategoryId = categoryId,
                Amount= amount
            };
            global::Spendings.Orchrestrators.Records.Record secondRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = secondInsertedDate,
                CategoryId = categoryId,
                Amount = amount
            };

            var firstPostRequest = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                       firstRecord),
                    Encoding.UTF8,
                    "application/json")
            };
            var secondPostRequest = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                       secondRecord),
                    Encoding.UTF8,
                    "application/json")
            };
            var GetRequest = new HttpRequestMessage(HttpMethod.Get, $"/User/{userId}/Record?from={firstInsertedDate.ToShortDateString()}&till={endDate.ToShortDateString()}");
            //Act
            var firstPostResponce = await _client.SendAsync(firstPostRequest);
            var secondPostResponce = await _client.SendAsync(secondPostRequest);

            var getResponce =  await _client.SendAsync(GetRequest);
            var records = await getModelListFromHttpResponce(getResponce);

            // Assert
            firstPostResponce.EnsureSuccessStatusCode();
            secondPostResponce.EnsureSuccessStatusCode();
            getResponce.EnsureSuccessStatusCode();
            Assert.Equal(2, records.Count);
            Assert.Equal(firstRecord.Date, records[0].Date);
            Assert.Equal(secondRecord.Date, records[1].Date);
        }
        [Fact]
        public async Task RecordsDeleteAsync_IfRecordDeleted_ReturnOk()
        {
            // Arrange
            int userId = 3;
            global::Spendings.Orchrestrators.Records.Record postRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = new DateTime(2005, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            
            var firstPostRequest = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                      postRecord),
                   Encoding.UTF8,
                   "application/json")
            };

            //Act
            var firstPostResponce = await _client.SendAsync(firstPostRequest);
            var record = await getModelFromHttpResponce(firstPostResponce);

            var deleteResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, $"/User/Record?recordId={record.Id}"));
            var exception = await Assert.ThrowsAsync<System.InvalidOperationException>
                (async () => await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/User/Record?recordId={record.Id}")));

            // Assert
            firstPostResponce.EnsureSuccessStatusCode();
            deleteResponce.EnsureSuccessStatusCode();
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task RecordUpdateAsync_IfRecordUpdated_ReturnOk()
        {
            // Arrange
            int userId = 1;
            global::Spendings.Orchrestrators.Records.Record postRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = new DateTime(2004, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            global::Spendings.Orchrestrators.Records.Record updatedRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = new DateTime(2004, 8, 20),
                CategoryId = 2,
                Amount = 10000
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                      postRecord),
                   Encoding.UTF8,
                   "application/json")
            };

            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"/User/{userId}/Record?from={postRecord.Date.ToShortDateString()}&till={postRecord.Date.ToShortDateString()}");

            //Act
            var postResponce = await _client.SendAsync(postRequest);
            var record = await getModelFromHttpResponce(postResponce);

            var updatedResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Put, $"/User/Record?recordId={record.Id}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                      updatedRecord),
                   Encoding.UTF8,
                   "application/json")
            });

            var getResponce = await _client.SendAsync(getRequest);
            var recordList = await getModelListFromHttpResponce(getResponce);

            // Assert
            postResponce.EnsureSuccessStatusCode();
            updatedResponce.EnsureSuccessStatusCode();
            Assert.Equal(updatedRecord.CategoryId, recordList[0].CategoryId);
            Assert.Equal(updatedRecord.Amount, recordList[0].Amount);
        }
       
        [Fact]
        public async Task RecordPatchAsync_IfRecordPatched_ReturnOk()
        {
            // Arrange
            int userId = 1;
            int addedAmount = 10000;

            global::Spendings.Orchrestrators.Records.Record postRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = new DateTime(2006, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                      postRecord),
                   Encoding.UTF8,
                   "application/json")
            };

            //Act
            var postResponce = await _client.SendAsync(postRequest);
            var record = await getModelFromHttpResponce(postResponce);

            var patchResponce = await _client.SendAsync(
                new HttpRequestMessage(HttpMethod.Patch, $"/User/Record?newAmount={addedAmount}&recordId={record.Id}"));

            var getResponce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"/User/Record?recordId={record.Id}"));
            record = await getModelFromHttpResponce(getResponce);

            // Assert
            patchResponce.EnsureSuccessStatusCode();
            postResponce.EnsureSuccessStatusCode();
            Assert.Equal(addedAmount + postRecord.Amount, record.Amount);
            Assert.Equal(postRecord.CategoryId, record.CategoryId);
        }
        [Fact]
        public async Task RecordPatchAsync_IfThrowsOverflowException_ReturnOk()
        {
            // Arrange
            int userId = 1;

            global::Spendings.Orchrestrators.Records.Record postRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = new DateTime(2005, 8, 20),
                CategoryId = 1,
                Amount = 1000
            };
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                      postRecord),
                   Encoding.UTF8,
                   "application/json")
            };

            int newAmount = int.MaxValue;
            int recordId = 1;

            var updateRequest = new HttpRequestMessage(HttpMethod.Patch, $"/User/Record?newAmount={newAmount}&recordId={recordId}");

            //Act
            var postResponce = await _client.SendAsync(postRequest);
            var exception = await Assert.ThrowsAsync<System.OverflowException>(async () => await _client.SendAsync(updateRequest));

            // Assert
            postResponce.EnsureSuccessStatusCode();
            Assert.NotNull(exception);
        }
        [Fact]
        public async Task RecordDeleteListAsync_IfWorks_ReturnOk()
        {
            // Arrange
            int categoryId = 1;
            int userId = 1;
            int amount = 1000;
            DateTime firstInsertedDate = new DateTime(2001, 10, 20);
            DateTime secondInsertedDate = new DateTime(2001, 10, 23);
            DateTime endDate = new DateTime(2001, 10, 26);
            global::Spendings.Orchrestrators.Records.Record firstRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = firstInsertedDate,
                CategoryId = categoryId,
                Amount = amount
            };
            global::Spendings.Orchrestrators.Records.Record secondRecord = new global::Spendings.Orchrestrators.Records.Record
            {
                Date = secondInsertedDate,
                CategoryId = categoryId,
                Amount = amount
            };

            var PostRequest = new HttpRequestMessage(HttpMethod.Post, $"/User/{userId}/Record")
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                       firstRecord),
                    Encoding.UTF8,
                    "application/json")
            };

            var DeleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/User/{userId}/Record?from={firstInsertedDate.ToShortDateString()}&till={endDate.ToShortDateString()}");
            var GetRequest = new HttpRequestMessage(HttpMethod.Get, $"/User/{userId}/Record?from={firstInsertedDate.ToShortDateString()}&till={endDate.ToShortDateString()}");

            //Act
            var PostResponce = await _client.SendAsync(PostRequest);
            var DeleteResponce = await _client.SendAsync(DeleteRequest);
            var GetResponce = await _client.SendAsync(GetRequest);

            var records = await getModelListFromHttpResponce(GetResponce);
            // Assert
            PostResponce.EnsureSuccessStatusCode();
            DeleteResponce.EnsureSuccessStatusCode();
            GetResponce.EnsureSuccessStatusCode();
            Assert.Empty(records);
        }

        async Task<global::Spendings.Core.Records.Record> getModelFromHttpResponce(HttpResponseMessage responce)
        {
            var byteResult = await responce.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var record = JsonConvert.DeserializeObject<global::Spendings.Core.Records.Record>(stringResult);
            return record;
        }

        async Task<List<global::Spendings.Core.Records.Record>> getModelListFromHttpResponce(HttpResponseMessage responce)
        {
            var byteResult = await responce.Content.ReadAsByteArrayAsync();
            var stringResult = Encoding.UTF8.GetString(byteResult);
            var records = JsonConvert.DeserializeObject<List<global::Spendings.Core.Records.Record>>(stringResult);
            return records;
        }
    }
}
