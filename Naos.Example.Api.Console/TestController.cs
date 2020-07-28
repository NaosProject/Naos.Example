// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestController.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Example.Api.Console
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.Owin.Security.Provider;
    using Naos.Bootstrapper;
    using Naos.Logging.Domain;
    using OBeautifulCode.Serialization.Json;
    using Owin;

    /// <summary>
    /// Entry point for Spritely OWIN adapter.
    /// </summary>
    public class TestController : ApiController
    {
        public static TestObject TestObject = new TestObject();

        public async Task<IHttpActionResult> Get()
        {
            return this.Ok(TestObject);
        }
    }

    public class TestObject
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}