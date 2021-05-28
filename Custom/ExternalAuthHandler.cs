using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.AspNetCore.DataProtection;
namespace ExampleApp.Custom
{
    public class ExternalAuthOptions
    {
        public string ClientId { get; set; } = "MyClientID";
        public string ClientSecret { get; set; } = "MyClientSecret";
        public virtual string RedirectRoot { get; set; } = "http://localhost:5000";
        public virtual string RedirectPath { get; set; } = "/signin-external";
        public virtual string Scope { get; set; } = "openid email profile";
        public virtual string StateHashSecret { get; set; } = "mysecret";
        public virtual string AuthenticationUrl { get; set; }
            = "http://localhost:5000/DemoExternalAuth/authenticate";
    }
    public class ExternalAuthHandler : IAuthenticationHandler
    {
        public ExternalAuthHandler(IOptions<ExternalAuthOptions> options,
                IDataProtectionProvider dp)
        {
            Options = options.Value;
            DataProtectionProvider = dp;
        }
        public AuthenticationScheme Scheme { get; set; }
        public HttpContext Context { get; set; }
        public ExternalAuthOptions Options { get; set; }
        public IDataProtectionProvider DataProtectionProvider { get; set; }
        public PropertiesDataFormat PropertiesFormatter { get; set; }
        public Task InitializeAsync(AuthenticationScheme scheme,
                 HttpContext context)
        {
            Scheme = scheme;
            Context = context;
            PropertiesFormatter = new PropertiesDataFormat(DataProtectionProvider
                .CreateProtector(typeof(ExternalAuthOptions).FullName));
            return Task.CompletedTask;
        }
        public Task<AuthenticateResult> AuthenticateAsync()
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        public async Task ChallengeAsync(AuthenticationProperties properties)
        {
            Context.Response.Redirect(await GetAuthenticationUrl(properties));
        }
        protected virtual Task<string>
                GetAuthenticationUrl(AuthenticationProperties properties)
        {
            Dictionary<string, string> qs = new Dictionary<string, string>();
            qs.Add("client_id", Options.ClientId);
            qs.Add("redirect_uri", Options.RedirectRoot + Options.RedirectPath);
            qs.Add("scope", Options.Scope);
            qs.Add("response_type", "code");
            qs.Add("state", PropertiesFormatter.Protect(properties));
            return Task.FromResult(Options.AuthenticationUrl
                + QueryString.Create(qs));
        }
        public Task ForbidAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }
    }
}
