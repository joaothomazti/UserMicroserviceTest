using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Text;
using TechTalk.SpecFlow;

namespace UserMicroservice.Test.StepDefinitions
{
    [Binding]
    public class RegisterUserStepDefinitions
    {
        private readonly HttpClient _client = null!;
        private HttpResponseMessage _response = null!;
        private User _user = null!;
        private WebApplicationFactory<Startup> _factory;

        public RegisterUserStepDefinitions()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5018")
            };

        }

        [Given(@"filling in the registration of a user with name as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithNameAsFollows(Table table)
        {
            _user = new User
            {
                name = "Joao"
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
    }

    [Binding]
    public class RegisterUserWithEmailSteps
    {
        private readonly HttpClient _client = null!;
        private HttpResponseMessage _response = null!;
        private User _user = null!;
        private WebApplicationFactory<Startup> _factory;

        public RegisterUserWithEmailSteps()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5018")
            };

        }
        [Given(@"filling in the registration of a user with name and email as follows")]
        public void GivenFillingInTheRegistrationOfAUserWithNameAndEmailAsFollows(Table table)
        {
            _user = new User
            {
                name = "Joao Thomaz",
                email= "joao@teste.com"
            };
        }

        [When(@"I send the request post")]
        public async Task WhenISendTheRequestPost()
        {
            var json = JsonConvert.SerializeObject(_user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _response = await _client.PostAsync("/users", content);
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

    }
}
