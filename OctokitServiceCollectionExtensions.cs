using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using Octokit.Internal;

namespace DependencyFlow
{
    public static class OctokitServiceCollectionExtensions
    {
        public static readonly string DefaultProductHeaderValue = "DependencyFlow";
        public static readonly string DefaultBaseAddress = "https://api.github.com";

        public static void AddOctokit(this IServiceCollection services, IConfiguration gitHubConfiguration)
        {
            services.Configure<GitHubOptions>(gitHubConfiguration);
            services.AddTransient<GitHubClientWrapper>();
            services.AddTransient((sp) =>
            {
                var wrapper = sp.GetRequiredService<GitHubClientWrapper>();
                return wrapper.Client;
            });
        }

        internal class GitHubClientWrapper
        {
            public GitHubClient Client { get; }
            public GitHubClientWrapper(IOptions<GitHubOptions> options, ILogger<GitHubClient> logger)
            {
                var credentials = Credentials.Anonymous;
                if (string.IsNullOrEmpty(options.Value.Token))
                {
                    logger.LogWarning("GitHub credentials were not specified in 'GitHub:Token'. Using anonymous credentials. API usage may be limited.");
                }
                else
                {
                    credentials = new Credentials(options.Value.Token);
                }

                Client = new GitHubClient(
                    new Connection(
                        new ProductHeaderValue(options.Value.ProductHeaderValue ?? DefaultProductHeaderValue),
                        new Uri(options.Value.BaseAddress ?? DefaultBaseAddress),
                        new InMemoryCredentialStore(credentials)));
            }
        }
    }
}
