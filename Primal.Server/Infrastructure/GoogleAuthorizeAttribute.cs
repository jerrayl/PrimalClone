using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Net.Http.Headers;

namespace Primal.Infrastructure
{
    public class GoogleJwtBearerHandler : JwtBearerHandler
    {
        public GoogleJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Cookies.TryGetValue(HeaderNames.Authorization, out var authToken))
            {
                return AuthenticateResult.Fail("Authorization token not found.");
            }

            try
            {
                var validationResult = await GoogleJsonWebSignature.ValidateAsync(authToken);
                var claims = new List<Claim>
                {
                    new (ClaimTypes.NameIdentifier, validationResult.Name),
                    new (ClaimTypes.Name, validationResult.Name),
                    new (JwtRegisteredClaimNames.FamilyName, validationResult.FamilyName),
                    new (JwtRegisteredClaimNames.GivenName, validationResult.GivenName),
                    new (JwtRegisteredClaimNames.Email, validationResult.Email),
                    new (JwtRegisteredClaimNames.Sub, validationResult.Subject),
                    new (JwtRegisteredClaimNames.Iss, validationResult.Issuer),
                };

                var principal = new ClaimsPrincipal();
                principal.AddIdentity(new ClaimsIdentity(claims, "Google"));
                return AuthenticateResult.Success(new AuthenticationTicket(principal, "GoogleJwtBearer"));
            }
            catch (InvalidJwtException)
            {
                return AuthenticateResult.Fail("Token is not valid.");
            }
        }
    }
}