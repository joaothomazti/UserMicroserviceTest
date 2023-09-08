using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Text;
using TechTalk.SpecFlow;
using UserMicroservice.Models;

namespace UserMicroservice.Test.StepDefinitions
{
    public class Hook
    {
        public HttpClient _client;
        public HttpResponseMessage _response = null!;
        public User _user = null!;
        public WebApplicationFactory<Startup> _factory;
        public string _newEmail = null!;
        public int _userId;
        public Hook()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5018")
            };
           
        }
    }

    [Binding]
    public class RegisterUserStepDefinitions : Hook
    {
        //Register User with name Only
        [Given(@"filling in the registration of a user with name as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithNameAsFollows(Table table)
        {
            var name = table.Rows[0]["name"];
            _user = new User
            {
                Name = name
            };
        }

        [When(@"i send the request POST")]
        public async Task WhenISendTheRequestPost()
        {
            var json = JsonConvert.SerializeObject(_user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _response = await _client.PostAsync("/users", content);
        }

        [Then(@"the user named must be registered")]
        public async Task Thentheusernamedmustberegistered()
        {
            Assert.True(_response.IsSuccessStatusCode);

            if (_response.IsSuccessStatusCode)
            {
                var responseContent = await _response.Content.ReadAsStringAsync();
                var createdUser = JsonConvert.DeserializeObject<User>(responseContent);

                Assert.AreEqual(_user.Name, createdUser?.Name);
            }
        }

        //Register user with name and email
        [Given(@"filling in the registration of a user with name and email as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithNameAndEmailAsFollows(Table table)
        {
            var name = table.Rows[0]["name"];  
            var email = table.Rows[0]["email"];
            _user = new User
            {
                Name = name,
                Email = email
            };
        }

        [Then(@"the user named with email must be registered")]
        public async Task ThenTheUserNamedWithEmailMustBeRegistered()
        {
            Assert.True(_response.IsSuccessStatusCode);

            if (_response.IsSuccessStatusCode)
            {
                var responseContent = await _response.Content.ReadAsStringAsync();
                var createdUser = JsonConvert.DeserializeObject<User>(responseContent);

                Assert.AreEqual(_user.Name, createdUser?.Name);
                Assert.AreEqual(_user.Email, createdUser?.Email);
            }

        }

        //User registration with email only should not be allowed
        [Given(@"filling in the registration of a user with email as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithEmailAsFollows(Table table)
        {
            var email = table.Rows[0]["email"];
            _user = new User
            {
                Email = email
            };
        }

        [Then(@"the system should respond with a BadRequest status code and message ""([^""]*)""")]
        public async Task ThenTheSystemShouldRespondWithABadRequestStatusCodeAndMessage(string BadRequestMessage)
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, _response.StatusCode);

            if (_response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await _response.Content.ReadAsStringAsync();
                Assert.IsTrue(responseContent.Contains(BadRequestMessage));
            }
        }

        //Update user email
        [Given(@"an existing user with the following details and I send a PUT request to update the user's email")]
        public async Task GivenAnExistingUserWithTheFollowingDetailsAndISendAPUTRequestToUpdateTheUsersEmail(Table table)
        {
            _userId = int.Parse(table.Rows[0]["id"]);
            _newEmail = table.Rows[0]["newEmail"];

            var updateUser = new
            {
                Email = _newEmail
            };
            

            var json = JsonConvert.SerializeObject(updateUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var apiUrl = $"/users/{_userId}";
            _response = await _client.PutAsync(apiUrl, content);
        }

        [Then(@"the system should respond with a status code (.*)")]
        public void ThenTheSystemShouldRespondWithAStatusCode(int StatusCode)
        {
            Assert.AreEqual((HttpStatusCode)StatusCode, _response.StatusCode);
        }

        [Then(@"the user's email should be updated to ""([^""]*)""")]
        public async Task ThenTheUsersEmailShouldBeUpdatedTo(string expectedEmail)
        {
            var apiUrl = $"/users/{_userId}";
            var getResponse = await _client.GetAsync(apiUrl);
            getResponse.EnsureSuccessStatusCode();

            var responseContent = await getResponse.Content.ReadAsStringAsync();
            var updatedUser = JsonConvert.DeserializeObject<User>(responseContent);

            Assert.AreEqual(expectedEmail, updatedUser?.Email);
        }

        //User delete
        [Given(@"an existing user with the following details")]
        public void GivenAnExistingUserWithTheFollowingDetails(Table table)
        {
            _userId = int.Parse(table.Rows[0]["id"]);
        }

        [When(@"I send a DELETE request to delete the user with ID (.*)")]
        public async Task WhenISendADELETERequestToDeleteTheUserWithID(int p0)
        {
            var apiUrl = $"/users/{_userId}";
            _response = await _client.DeleteAsync(apiUrl);
        }

        [Then(@"the system should respond with a successful deletion status code and message ""([^""]*)""")]
        public async Task ThenTheSystemShouldRespondWithASuccessfulDeletionStatusCodeAndMessage(string userDeleted)
        {
            Assert.AreEqual(HttpStatusCode.OK, _response.StatusCode);
            var responseContent = await _response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseContent.Contains(userDeleted));
        }


        [Then(@"the user with ID (.*) should no longer exist")]
        public async Task ThenTheUserWithIDShouldNoLongerExist(int _userId)
        {
            var apiUrl = $"/users/{_userId}";
            var getResponse = await _client.GetAsync(apiUrl);

            Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
   
}





















