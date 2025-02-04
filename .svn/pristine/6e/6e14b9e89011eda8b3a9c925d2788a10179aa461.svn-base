using AutoMapper;
using Microsoft.Extensions.Configuration;
using MoreLinq;
using RestSharp;
using SERP.Application.Common;
using SERP.Application.Common.Constants;
using SERP.Application.Masters.Currencies.DTOs;
using SERP.Application.Masters.Currencies.Interfaces;
using SERP.Domain.Masters.Currencies;
using System.Data;
using System.Text.Json;
using SERP.Application.Common.Exceptions;

namespace SERP.Application.Masters.Currencies.Services
{
    internal class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(
            ICurrencyRepository currencyRepository,
            IMapper mapper,
            HttpClient httpClient,
            ICurrencyExchangeRepository currencyExchangeRepository,
            IUnitOfWork unitOfWork)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _httpClient = httpClient;
            _currencyExchangeRepository = currencyExchangeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CurrencyDto>> GetAllLimited(bool onlyEnabled)
        {
            return _mapper.Map<List<CurrencyDto>>(await _currencyRepository.GetAllLimited(onlyEnabled));
        }

        public async Task<CurrencyExchangeDto?> GetExchangeRate(int fromCurrencyId, int toCurrencyId)
        {
            var res = await _currencyRepository.GetExchangeRate(fromCurrencyId, toCurrencyId);
            if (res is null)
            {
                throw new NotFoundException(ErrorMessages.ExchangeRateNotFound);
            }

            return _mapper.Map<CurrencyExchangeDto>(res);
        }

        public async Task UpdateAllBasedCurrencyExchangeFixerIO()
        {
            try
            {
                IConfiguration config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .Build();

                var baseUrl = config["ExternalApi:BaseUrl"];
                var accessKey = config["ExternalApi:accesskey"];

                var getBasedCurrencyList = await _currencyRepository.Find(x => x.status_flag == ApplicationConstant.StatusFlag.Enabled);

                DataTable dtBasedCurrency = getBasedCurrencyList.ToDataTable();

                var result = dtBasedCurrency.Rows.Cast<DataRow>()
                    .Select(row => row["currency_code"].ToString())
                    .ToArray();

                string currencySymbolList = string.Join(",", result);

                Console.WriteLine(currencySymbolList);

                foreach (string currencyCode in result)
                {

                    var options = new RestClientOptions(baseUrl);

                    var baseCurrency = currencyCode;

                    var requestURL = baseUrl + "latest?" + "access_key=" + accessKey.ToString() + "&base=" + currencyCode.ToString() + "&symbols=" + currencySymbolList.ToString();
                    var client = new RestClient(options);
                    var request = new RestRequest(requestURL, Method.Get);
                    //RestResponse response = await client.ExecuteAsync(request);
                    var response = await _httpClient.GetAsync(requestURL);

                    var strResponse = await response.Content.ReadAsStringAsync();

                    // Parse the JSON string
                    JsonDocument jsonDoc = JsonDocument.Parse(strResponse);
                    JsonElement root = jsonDoc.RootElement;
                    if (root.TryGetProperty("success", out JsonElement successElement) && successElement.GetBoolean())
                    {

                        var getBasedCurrency = await _currencyRepository.GetByIdAsync(x => x.currency_code == baseCurrency);

                        if (root.TryGetProperty("rates", out JsonElement ratesElement))
                        {
                            foreach (JsonProperty rateProperty in ratesElement.EnumerateObject())
                            {
                                string curCode = rateProperty.Name;
                                var getCurrency = await _currencyRepository.GetByIdAsync(x => x.currency_code == curCode);

                                var getExistedCurrencyExchange = await _currencyExchangeRepository.GetByIdAsync(x => x.base_currency_id == getBasedCurrency.id && x.currency_id == getCurrency.id);


                                double exchangeRate = rateProperty.Value.GetDouble();

                                if (getExistedCurrencyExchange != null)
                                {
                                    var updateCurrencyExchangeRate = new CurrencyExchange
                                    {
                                        id = getExistedCurrencyExchange.id,
                                        base_currency_id = getExistedCurrencyExchange.base_currency_id,
                                        currency_id = getExistedCurrencyExchange.currency_id,
                                        exchange_rate = (decimal)exchangeRate,
                                        created_on = getExistedCurrencyExchange.created_on,
                                        created_by = getExistedCurrencyExchange.created_by,
                                        last_modified_on = DateTime.Now,
                                        last_modified_by = ApplicationConstant.createdByDefault
                                    };

                                    await _currencyExchangeRepository.UpdateAsync(updateCurrencyExchangeRate);
                                    await _unitOfWork.SaveChangesAsync();
                                }
                                else
                                {
                                    var createCurrencyExchangeRate = new CurrencyExchange
                                    {
                                        base_currency_id = getBasedCurrency.id,
                                        currency_id = getCurrency.id,
                                        exchange_rate = (decimal)exchangeRate,
                                        created_on = DateTime.Now,
                                        created_by = ApplicationConstant.createdByDefault,
                                    };
                                    await _currencyExchangeRepository.CreateAsync(createCurrencyExchangeRate);
                                    await _unitOfWork.SaveChangesAsync();
                                }

                            }
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
