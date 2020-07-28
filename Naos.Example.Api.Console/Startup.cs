// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Example.Api.Console
{
    using System.Diagnostics;
    using System.Net.Http.Formatting;
    using Naos.Bootstrapper;
    using Naos.Logging.Domain;
    using OBeautifulCode.Serialization.Json;
    using Owin;

    /// <summary>
    /// Entry point for Spritely OWIN adapter.
    /// </summary>
    public class Startup
    {
        public static readonly ObcJsonMediaTypeFormatter ObcJsonMediaTypeFormatter = new ObcJsonMediaTypeFormatter(
            typeof(AttemptOnUnregisteredTypeJsonSerializationConfiguration<
                CompactFormatJsonSerializationConfiguration<NullJsonSerializationConfiguration>>).ToJsonSerializationConfigurationType());

        /// <summary>
        /// Gets the initialize container delegate.
        /// </summary>
        public static InitializeContainer LiveInitializeContainer => container =>
                                                                     {
                                                                         /*no-op*/
                                                                     };

        /// <summary>
        /// Gets or sets the initialize container delegate, this allows a test to specify and then run the test server on the new specification.
        /// </summary>
        public static InitializeContainer TestInitializeContainer { get; set; }

        /// <summary>
        /// The main entry point for OWIN Web API.
        /// </summary>
        /// <param name="app">The application.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Highly coupled by nature.")]
        public static void Configuration(IAppBuilder app)
        {
            var obcJsonMediaTypeFormatter = new ObcJsonMediaTypeFormatter(
                typeof(AttemptOnUnregisteredTypeJsonSerializationConfiguration<
                    CompactFormatJsonSerializationConfiguration<NullJsonSerializationConfiguration>>).ToJsonSerializationConfigurationType());

            BasicWebApiLogPolicy.Initialize();
            Log.Write(() => Messages.Application_Started);

            // Execution order is important here
            BasicWebApiLogPolicy.Log = message =>
            {
                Trace.WriteLine(message);
                System.Console.WriteLine(message);
            };

            app
               .UseWebApiWithHttpConfigurationInitializers(
                    (
                        config,
                        service) =>
                    {
                        config.Formatters.Remove(config.Formatters.JsonFormatter);
                        config.Formatters.Insert(0, obcJsonMediaTypeFormatter);
                    })
               .UseContainerInitializer(LiveInitializeContainer)
               .UseCors()
               .UseJwtAuthentication()
               .UseGzipDeflateCompression();
        }

        /// <summary>
        /// The main entry point for OWIN Web API.
        /// </summary>
        /// <param name="app">The application.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Highly coupled by nature.")]
        public static void TestConfiguration(IAppBuilder app)
        {
            BasicWebApiLogPolicy.Initialize();
            Log.Write(() => Messages.Application_Started);

            // Execution order is important here
            BasicWebApiLogPolicy.Log = message =>
            {
                Trace.WriteLine(message);
                System.Console.WriteLine(message);
            };

            app
               .UseContainerInitializer(TestInitializeContainer)
               .UseJwtAuthentication()
               .UseGzipDeflateCompression()
               .UseWebApiWithHttpConfigurationInitializers(
                    (
                        config,
                        service) =>
                    {
                        config.Formatters.Remove(config.Formatters.JsonFormatter);
                        config.Formatters.Add(ObcJsonMediaTypeFormatter);
                    });
        }
    }
}