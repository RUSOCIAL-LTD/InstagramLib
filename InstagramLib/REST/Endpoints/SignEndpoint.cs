using SnapchatLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InstagramLib.Exceptions;
using InstagramLib.Extras;

namespace InstagramLib.REST.Endpoints;
internal interface ISignEndpoint
{
    Task<string> SignRequest();
}

internal class SignEndpoint : EndpointAccessor, ISignEndpoint
{
    internal const string DefaultSignUrl = "https://sign.rusocial.io/instagram/sign";

    public SignEndpoint(InstagramClient client, IInstagramHttpClient httpClient, InstagramLockedConfig config, IClientLogger logger, IUtilities utilities, IRequestConfigurator configurator) : base(client, httpClient, config, logger, utilities, configurator)
    {
    }

    internal static HttpResponseMessage response { get; set; }
    private void RaiseForInvalidValues()
    {
        if (string.IsNullOrEmpty(Config.ApiKey))
            new Exception("ApiKey is required");

        if (string.IsNullOrEmpty(Config.Username))
            new Exception("Username is required");

        if (string.IsNullOrEmpty(Config.androidId) || string.IsNullOrEmpty(Config.ro_product_product_manufacturer) || string.IsNullOrEmpty(Config.ro_build_version_release) || string.IsNullOrEmpty(Config.HardwareModel) || string.IsNullOrEmpty(Config.phoneModel))
            throw new Exception("Phone Stuff is required");

        if (string.IsNullOrWhiteSpace(Config.androidId))
        {
            throw new AndroidIDNotSet();
        }
    }
    public async Task<string> SignRequest()
    {
        RaiseForInvalidValues();

        var request = new HttpRequestMessage(HttpMethod.Post, DefaultSignUrl);
        request.Version = HttpVersion.Version20;
        request.VersionPolicy = HttpVersionPolicy.RequestVersionExact;
        request.Headers.TryAddWithoutValidation("x-license", Config.ApiKey);
        request.Headers.TryAddWithoutValidation("User-Agent", "Instagram/Public");
        var sign_json = new InstagramSign { 
            phone_info = new PhoneInfo 
            { 
                androidId = Config.androidId,
                androidOsApi = Config.ro_build_version_sdk,
                androidOsVersion = Config.ro_build_version_release,
                phoneManufacture = Config.ro_product_product_manufacturer,
                HardwareModel = Config.HardwareModel,
                phoneModel = Config.phoneModel,
            },
            persistent_info = new PersistentInfo 
            {
                device_id = Guid.NewGuid().ToString(),
                family_device_id = Guid.NewGuid().ToString()
            },
            session_info = new SessionInfo 
            { 
                session_id = InstagramClient.sessionsId,
            }
        };
        request.Content = new StringContent(m_Utilities.JsonSerializeObject(sign_json), Encoding.UTF8, "application/json");

        if (Config.ProxySigner)
        {
            response = await HttpClient.Send(DefaultSignUrl, request, true);
        }
        else
        {
            response = await HttpClient.Send(DefaultSignUrl, request, false);
        }

        var responseData = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.OK)
            throw new SignerException(responseData);

        if (responseData != null)
            return responseData;

        throw new SignerException(responseData);
    }
}