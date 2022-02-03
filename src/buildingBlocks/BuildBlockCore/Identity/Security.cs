using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Identity
{
    public static class Security
    {
        /*Emissores validos*/
        readonly static string Issuer = "MeuSistema";
        /*Receptores validos*/
        /*
         https://localhost:9093-> Enterprise.Catalog
        https://localhost:9095-> Enterprise.Shopping.Card
         
         */
        readonly static string ValidAt = "https://localhost";
        readonly static double ExpirationHours = 24;
        readonly static string Secret = "Ann@julia2010MacDonalds";

        public static string GenerateJwt(List<string> roles, List<Claim> addclaims, string userId, string email)
        {

            if (addclaims == null)
                addclaims = new List<Claim>();

            if (!string.IsNullOrEmpty(userId) && !addclaims.Any(x => x.Type == JwtRegisteredClaimNames.Sub))
                addclaims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId));
            if (!string.IsNullOrEmpty(email) && !addclaims.Any(x => x.Type == JwtRegisteredClaimNames.Email))
                addclaims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

            foreach (var role in roles)
                addclaims.Add(new Claim(ClaimTypes.Role, role));


            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(addclaims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Issuer = Issuer,
                Audience = ValidAt,
                Expires = DateTime.UtcNow.AddHours(ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }


        public static void AddJWT(this IServiceCollection services)
        {
            // JWT Setup
            var key = Encoding.ASCII.GetBytes(Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = ValidAt,
                    ValidIssuer = Issuer
                };
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });


        }
    }
}
