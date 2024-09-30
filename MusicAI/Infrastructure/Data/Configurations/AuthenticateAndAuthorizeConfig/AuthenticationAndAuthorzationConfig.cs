//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Newtonsoft.Json;
//using OnlineClassManagement.microServices.CommonServices.Application.Contracts.Dto.ServiceResult.Response;
//using OnlineClassManagement.microServices.UserService.Infrastructure.Data.Configs.Cookie;
//using OnlineClassManagement.microServices.UserService.Infrastructure.Data.Configs.JwtToken;

//namespace OnlineClassManagement.microServices.UserService.Infrastructure.Data.Configurations.AuthenticateAndAuthorizeConfig
//{
//    public static class AuthenticationAndAuthorzationConfig
//    {

//        public static void AddAuthentication(this IServiceCollection services, IConfiguration _Configuration, CookieTypesList cookieTypesList)
//        {
//            // calling appsetting to get jwt config
//            var jwtOptions = new JWTFixedOption();
//            _Configuration.GetSection("jwt").Bind(jwtOptions);
//            services.AddSingleton(jwtOptions);

//            services.AddAuthentication(Options =>
//            {
//                //Options.DefaultAuthenticateScheme = "Bearer";
//                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                Options.DefaultScheme = "Bearer";
//                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//            })
//           .AddJwtBearer(cfg =>
//           {
//               cfg.Events = new JwtBearerEvents();
//               cfg.RequireHttpsMetadata = false;
//               cfg.TokenValidationParameters = TokenValidationParams.GetInstance(jwtOptions);
//               //new TokenValidationParameters
//               //{
//               //    ValidateIssuerSigningKey = true,
//               //    ValidateIssuer = false,
//               //    ValidateAudience = false,
//               //    ValidateLifetime = true,
//               //    //ValidIssuer = jwtOptions.jwtIssuer,
//               //    //ValidAudience = jwtOptions.jwtIssuer,
//               //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.jwtKey)),
//               //    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.jwtPayloadEncKey)),
//               //};
//               cfg.Events.OnChallenge = context =>
//               {
//                   // Skip the default logic.
//                   context.HandleResponse();
//                   var resultDto = new ResultDto()
//                   {
//                       IsSuccess = false,
//                       Message = "شما مجوز این کار را ندارید :" + context.Error
//                   };
//                   context.Response.ContentType = "application/json";
//                   context.Response.StatusCode = 401;
//                   return context.Response.WriteAsync(JsonConvert.SerializeObject(resultDto));
//                   //return context.Response.WriteAsync(resultDto.ToString());
//               };
//               cfg.RequireHttpsMetadata = false;
//               cfg.Events.OnMessageReceived = context =>
//               {
//                   if (context.Request.Cookies.Any() && context.Request.Cookies != null)
//                   {
//                       if (context.Request.Path.Value.EndsWith("SetPass"))
//                       {
//                           context.Token = context.Request.Cookies[cookieTypesList.C_SetPass.Name];
//                       }
//                       else if (context.Request.Path.Value.EndsWith("VerifyCode"))
//                       {
//                           context.Token = context.Request.Cookies[cookieTypesList.C_VerifyPhone.Name];
//                           if (string.IsNullOrWhiteSpace(context.Token))
//                           {
//                               context.Token = context.Request.Cookies[cookieTypesList.C_VerifyEmail.Name];
//                           }
//                       }
//                       //else if (context.Request.Path.Value.EndsWith("VerifyCode"))
//                       //{
//                       //    context.Token = context.Request.Cookies[cookieTypesList.C_VerifyEmail.Name];
//                       //}
//                       else if (context.Request.Cookies.ContainsKey(cookieTypesList.C_EnterPhone.Name))
//                       {
//                           context.Token = context.Request.Cookies[cookieTypesList.C_EnterPhone.Name];
//                       }
//                       else if (context.Request.Cookies.ContainsKey(cookieTypesList.C_Access.Name))
//                       {
//                           context.Token = context.Request.Cookies[cookieTypesList.C_Access.Name];
//                       }
//                       else if (context.Request.Path.Value.EndsWith("GoogleAuthCode") || context.Request.Path.Value.EndsWith("GoogleAuthSetUpCode"))
//                       {
//                           context.Token = context.Request.Cookies[cookieTypesList.C_EnterGoogleAuth.Name];
//                       }
//                       else if (context.Request.Path.Value.EndsWith("EnterPass"))
//                       {
//                           context.Token = context.Request.Cookies[cookieTypesList.C_EnterPass.Name];
//                       }
//                       else if (context.Request.Path.Value.EndsWith("PreVerifyAccountUpdate"))
//                       {
//                           context.Token = context.Request.Cookies[cookieTypesList.C_preEmailVerification.Name];
//                       }
//                   }
//                   return Task.CompletedTask;
//               };
//           });
//        }

//        public static void AddAuthorization(this IServiceCollection services, int version)
//        {
//            services.AddAuthorization(options =>
//            {
//                options.AddPolicy(TokenType.AccessAndGoogleAuth.ToString(), policy =>
//                 policy.RequireClaim("Type", TokenType.access.ToString(), TokenType.EnterGoogleAuth.ToString()));

//                options.AddPolicy(TokenType.emailverification.ToString(), policy =>
//                  policy.RequireClaim("Type", TokenType.emailverification.ToString()));

//                options.AddPolicy(TokenType.access.ToString(), policy =>
//                  policy.RequireClaim("Type", TokenType.access.ToString()));

//                options.AddPolicy(TokenType.refresh.ToString(), policy =>
//                  policy.RequireClaim("Type", TokenType.refresh.ToString()));

//                options.AddPolicy(TokenType.passwordReset.ToString(), policy =>
//                  policy.RequireClaim("Type", TokenType.passwordReset.ToString()));

//                options.AddPolicy(TokenType.preAccountVerification.ToString(), policy =>
//                  policy.RequireClaim("Type", TokenType.preAccountVerification.ToString()));

//                options.AddPolicy(TokenType.verifyPhone.ToString(), policy =>
//                 policy.RequireClaim("Type", TokenType.verifyPhone.ToString(), TokenType.verifyEmail.ToString()));

//                options.AddPolicy(TokenType.verifyEmail.ToString(), policy =>
//                policy.RequireClaim("Type", TokenType.verifyEmail.ToString()));

//                options.AddPolicy(TokenType.EnterPass.ToString(), policy =>
//                 policy.RequireClaim("Type", TokenType.EnterPass.ToString()));

//                options.AddPolicy(TokenType.EnterGoogleAuth.ToString(), policy =>
//                policy.RequireClaim("Type", TokenType.EnterGoogleAuth.ToString()));

//                options.AddPolicy(TokenType.SetPass.ToString(), policy =>
//                policy.RequireClaim("Type", TokenType.SetPass.ToString()));

//                options.AddPolicy(TokenType.enterPhone.ToString(), policy =>
//                policy.RequireClaim("Type", TokenType.enterPhone.ToString()));

//            });
//        }
//    }
//}
