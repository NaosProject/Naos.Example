// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCustomMediaTypeFormatter.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Example.Api.Test.Console
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IdentityModel.Tokens;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security.DataHandler.Encoder;
    using Microsoft.Owin.Testing;
    using Naos.Bootstrapper;
    using Naos.Example.Api.Console;
    using OBeautifulCode.Assertion.Recipes;
    using Xunit;
    using Container = SimpleInjector.Container;

    public class TestCustomMediaTypeFormatter
    {
        [Fact]
        public static async Task MakeTestCall()
        {
            var expectedResponse = Startup.ObcJsonMediaTypeFormatter.Serializer.SerializeToString(TestController.TestObject);

            void InitializeContainer(
                Container container)
            {
                /* no-op */
            }

            string serverResponseIfAny = null;

            using (var server = TestServerFactory.CreateWith(InitializeContainer))
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await server.GetResponse(
                        "testcontroller",
                        TestClaims.None);
                }
                catch
                {
                    if (response != null)
                    {
                        serverResponseIfAny = await response.Content.ReadAsStringAsync();
                    }

                    throw;
                }

                response.StatusCode.MustForTest().NotBeEqualTo(HttpStatusCode.Unauthorized);
            }

            serverResponseIfAny.MustForTest().BeEqualTo(expectedResponse);
        }
    }

    internal static class TestServerFactory
    {
        // Used for debugging Internal Server Error messages by adding additional error details.
        // To use, Change IncludeErrorDetailPolicy to Always and then error details can be
        // retrieved using await response.Content.ReadAsStringAsync();
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance",
            "CA1811:AvoidUncalledPrivateCode",
            Justification = "Not certain if this is used or useful.")]

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Usage",
            "CA1801:ReviewUnusedParameters",
            MessageId = "initializeContainer",
            Justification = "Not certain if this is used or useful.")]
        public static TestServer CreateWith(
            InitializeContainer initializeContainer)
        {
            BasicWebApiLogPolicy.Log = Console.WriteLine;
            Startup.TestInitializeContainer = initializeContainer;
            var server = TestServer.Create(Startup.TestConfiguration);
            return server;
        }
    }

    internal static class TestClaims
    {
        public static Claim[] None => new Claim[] {};
    }

    internal static class TestServerExtensions
    {
        public static async Task<HttpResponseMessage> Get(
            this TestServer server,
            string requestPath,
            params Claim[] claims)
        {
            var response = await server
                                .CreateRequest(requestPath)
                                .AddAuthorizationHeader(claims)
                                .GetAsync();

            return response;
        }

        public static async Task<HttpResponseMessage> Post(
            this TestServer server,
            string requestPath,
            HttpContent content,
            params Claim[] claims)
        {
            var jwt = CreateJwtAccessToken(claims);

            var request = new HttpRequestMessage(HttpMethod.Post, requestPath)
                          {
                              Content = content,
                          };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var response = await server.HttpClient.SendAsync(request);

            return response;
        }

        public static async Task<HttpResponseMessage> GetResponse(
            this TestServer server,
            string requestPath,
            params Claim[] claims)
        {
            var requestPathBase = server.CreateRequest(requestPath);
            var requestPathWithAuthorizationHeader = requestPathBase.AddAuthorizationHeader(claims);
            requestPathWithAuthorizationHeader.AddHeader("content-type", "application/json");
            requestPathWithAuthorizationHeader.AddHeader("accept", "application/json");
            var response = await requestPathWithAuthorizationHeader.GetAsync();

            return response;
        }

        public static RequestBuilder AddAuthorizationHeader(
            this RequestBuilder requestBuilder,
            params Claim[] claims)
        {
            var jwt = CreateJwtAccessToken(claims);
            requestBuilder.AddHeader("Authorization", "bearer " + jwt);

            return requestBuilder;
        }

        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1303:Do not pass literals as localized parameters",
            MessageId = "Microsoft.Owin.Security.DataHandler.Encoder.ITextEncoder.Decode(System.String)",
            Justification = "String literals for tests are totally acceptable.")]
        public static string CreateJwtAccessToken(
            this Claim[] claims)
        {
            // Data should match .config/Local/JwtBearerAuthenticationSettings
            var clientId = "test.local.naosproject.com";
            var issuer = "https://" + clientId;
            var issued = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(5);
            var securityKey = TextEncodings.Base64Url.Decode("<SECURITY KEY PAYLOAD HERE>");

            var signingCredentials = new SigningCredentials(
                new InMemorySymmetricSecurityKey(securityKey),
                "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512",
                "http://www.w3.org/2001/04/xmlenc#sha512");

            var token = new JwtSecurityToken(issuer, clientId, claims, issued, expires, signingCredentials);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);

            return jwt;
        }
    }
}