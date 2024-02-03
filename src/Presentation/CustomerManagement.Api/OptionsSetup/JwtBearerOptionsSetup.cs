using System.Text;
using CustomerManagement.Persistance.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CustomerManagement.Api.OptionsSetup
{
	public class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
	{
        private readonly JwtOptions _jwtOptions;

		public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
		{
            _jwtOptions = jwtOptions.Value;
		}

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new()
            {
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
            };
        }
    }
}

