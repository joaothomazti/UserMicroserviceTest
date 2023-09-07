using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Text;
using TechTalk.SpecFlow;

namespace UserMicroservice.Test.StepDefinitions
{
    public class Hook
    {
        public HttpClient _client;
        public HttpResponseMessage _response = null!;
        public User _user = null!;
        public WebApplicationFactory<Startup> _factory;
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
        [Given(@"filling in the registration of a user with name as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithNameAsFollows(Table table)
        {
            var name = table.Rows[0]["name"];
            _user = new User
            {
                name = name
            };
        }

        [Given(@"filling in the registration of a user with name and email as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithNameAndEmailAsFollows(Table table)
        {
            var name = table.Rows[0]["name"];  
            var email = table.Rows[0]["email"];
            _user = new User
            {
                name = name,
                email = email
            };
        }

        [Given(@"filling in the registration of a user with email as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithEmailAsFollows(Table table)
        {
            var email = table.Rows[0]["email"];
            _user = new User
            {
                email = email
            };
        }

        [When(@"i send the request post")]
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

                Assert.AreEqual(_user.name, createdUser.name);
            }

        }
        [Then(@"the user named with email must be registered")]
        public async Task ThenTheUserNamedWithEmailMustBeRegistered()
        {
            Assert.True(_response.IsSuccessStatusCode);

            if (_response.IsSuccessStatusCode)
            {
                var responseContent = await _response.Content.ReadAsStringAsync();
                var createdUser = JsonConvert.DeserializeObject<User>(responseContent);

                Assert.AreEqual(_user.name, createdUser.name);
                Assert.AreEqual(_user.email, createdUser.email);
            }

        }
        [Then(@"the system should respond with a BadRequest status code and message ""([^""]*)""")]
        public async Task ThenTheSystemShouldRespondWithABadRequestStatusCodeAndMessage(string p0)
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, _response.StatusCode);

            if (_response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await _response.Content.ReadAsStringAsync();
                Assert.IsTrue(responseContent.Contains(p0));
            }
        }
    }
}




