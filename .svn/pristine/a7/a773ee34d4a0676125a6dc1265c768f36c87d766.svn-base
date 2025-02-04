using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Common.Exceptions;
using SERP.Application.Masters.ApplicationTokens.Interfaces;
using SERP.Application.Transactions.ApplicationToken.Response;
using SERP.Domain.Masters.ApplicationTokens;
using System.Security.Cryptography;
using System.Text;

namespace SERP.Application.Masters.ApplicationTokens.Services
{
    public class ApplicationTokenService : IApplicationTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BoldReportsSettings _boldReportsSettings;
        private readonly IApplicationTokenRepository _applicationTokenRepository;

        private readonly string tokenUrl;
        private readonly string boldReportsUrl;
        private readonly string username;
        private readonly string secretCode;
        private readonly string nonce;
        private readonly string timeStamp;
        public ApplicationTokenService(IUnitOfWork unitOfWork, IOptions<BoldReportsSettings> boldReportsSettings, IApplicationTokenRepository applicationTokenRepository)
        {
            _unitOfWork = unitOfWork;
            _boldReportsSettings = boldReportsSettings.Value;
            _applicationTokenRepository = applicationTokenRepository;

            tokenUrl = _boldReportsSettings.TokenUrl;
            boldReportsUrl = _boldReportsSettings.BoldReportsUrl;
            username = _boldReportsSettings.Username;
            secretCode = _boldReportsSettings.SecretCode;
            nonce = Guid.NewGuid().ToString();
            timeStamp = DateTimeToUnixTimeStamp(DateTime.UtcNow).ToString();
        }

        public async Task<ApplicationTokenDto> GetByApplicationCode(string userId, string applicationCode)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var dBApplicationToken = await _applicationTokenRepository.GetToken(applicationCode);
                if (dBApplicationToken == null)
                    throw new BadRequestException(ErrorCodes.ApplicationTokenNotFound, ErrorMessages.ApplicationTokenNotFound);

                // If token has expired
                else if (DateTime.Now > dBApplicationToken.expiry_date)
                {
                    Token token = GetToken();

                    dBApplicationToken.application_code = ApplicationConstant.ApplicationName.Code;
                    dBApplicationToken.application_name = ApplicationConstant.ApplicationName.Name;
                    dBApplicationToken.token_type = token.token_type;
                    dBApplicationToken.token = token.access_token;
                    dBApplicationToken.issued_date = token.issued;
                    dBApplicationToken.expiry_date = token.expires;
                    dBApplicationToken.last_modified_on = DateTime.Now;
                    dBApplicationToken.last_modified_by = userId;

                    await _applicationTokenRepository.UpdateAsync(dBApplicationToken);

                    await _unitOfWork.SaveChangesAsync();
                    _unitOfWork.Commit();
                }


                return new ApplicationTokenDto()
                {
                    id = dBApplicationToken.id,
                    application_code = dBApplicationToken.application_code,
                    application_name = dBApplicationToken.application_name,
                    token_type = dBApplicationToken.token_type,
                    token = dBApplicationToken.token,
                    issued_date = dBApplicationToken.issued_date,
                    expiry_date = dBApplicationToken.expiry_date,
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }

        }

        public async Task CreateTokenAsync(string userId)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                await CreateToken(userId);

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();
        }

        private async Task<ApplicationToken> CreateToken(string userId)
        {
            try
            {
                Token t = GetToken();
                ApplicationToken applicationToken = new ApplicationToken()
                {
                    application_code = ApplicationConstant.ApplicationName.Code,
                    application_name = ApplicationConstant.ApplicationName.Name,
                    token_type = t.token_type,
                    token = t.access_token,
                    issued_date = t.issued,
                    expiry_date = t.expires,
                    created_by = userId,
                    created_on = DateTime.Now,
                };
                await _applicationTokenRepository.CreateAsync(applicationToken);

                return applicationToken;

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }        }

        private Token GetToken()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(boldReportsUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            string embedMessage = "embed_nonce=" + nonce + "&user_email=" + username + "&timestamp=" + timeStamp;
            string signature = SignURL(embedMessage.ToLower(), secretCode);

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "embed_secret"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("embed_nonce", nonce),
                    new KeyValuePair<string, string>("embed_signature", signature),
                    new KeyValuePair<string, string>("timestamp", timeStamp)
                });

            var result = client.PostAsync(tokenUrl, content).Result;

            string resultContent = result.Content.ReadAsStringAsync().Result;

            if (JsonConvert.DeserializeObject<Token>(resultContent)?.error == "authorization_failed")
            {
                Console.WriteLine("authorization_failed: " + JsonConvert.DeserializeObject<Token>(resultContent)?.error_description);
                Console.ReadLine();
                Environment.Exit(-1);
            }

            return JsonConvert.DeserializeObject<Token>(resultContent);
        }

        private string SignURL(string embedMessage, string secretcode)
        {
            var encoding = new UTF8Encoding();
            var keyBytes = encoding.GetBytes(secretcode);
            var messageBytes = encoding.GetBytes(embedMessage);
            using (var hmacsha1 = new HMACSHA256(keyBytes))
            {
                var hashMessage = hmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashMessage);
            }
        }

        private static double DateTimeToUnixTimeStamp(DateTime dateTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long unixTimeStampInTicks = (dateTime.ToUniversalTime() - unixStart).Ticks;
            return unixTimeStampInTicks / TimeSpan.TicksPerSecond;
        }
    }
}
