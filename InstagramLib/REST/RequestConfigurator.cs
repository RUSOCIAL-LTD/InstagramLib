using SnapchatLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InstagramLib.Exceptions;
using InstagramLib.Extras;

namespace InstagramLib.REST;

internal struct RequestConfiguration
{
    public string Endpoint;
    public HttpMethod HttpMethod;
    public bool IsMulti;
}

internal struct EndpointInfo
{
    public string BaseEndpoint;
    public string Url;
    public string SignUrlOverride = null;

    public EndpointInfo()
    {
        BaseEndpoint = RequestConfigurator.BaseEndpoint;
        Url = "";
    }
}

internal interface IRequestConfigurator
{
    HttpRequestMessage Configure(EndpointInfo endpointInfo, HttpContent content, HttpMethod httpMethod, InstagramClient client, IInstagramHttpClient httpClient, bool isMulti = false);
    Task<HttpRequestMessage> Configure(EndpointInfo endpointInfo, Dictionary<string, string> parameters, HttpMethod httpMethod, InstagramClient client, IInstagramHttpClient httpClient, bool isMulti = false);
}

internal class RequestConfigurator : IRequestConfigurator
{
    internal static string AttestationBaseEndpoint => "https://b.i.instagram.com";
    internal static string BaseEndpoint => "https://b.i.instagram.com";

    private readonly IUtilities m_Utilities;
    private readonly IClientLogger m_Logger;

    public RequestConfigurator(IClientLogger logger, IUtilities utilities)
    {
        m_Logger = logger;
        m_Utilities = utilities;
    }

    public HttpRequestMessage Configure(EndpointInfo endpointInfo, HttpContent content, HttpMethod httpMethod, InstagramClient client, IInstagramHttpClient httpClient, bool isMulti = false)
    {
        var config = CreateConfig(endpointInfo, httpMethod, client, isMulti);
        return GenerateRequest(httpClient, config, endpointInfo, content);
    }

    public async Task<HttpRequestMessage> Configure(EndpointInfo endpointInfo, Dictionary<string, string> parameters, HttpMethod httpMethod, InstagramClient client, IInstagramHttpClient httpClient, bool isMulti = false)
    {
        var config = CreateConfig(endpointInfo, httpMethod, client, isMulti);
        return await GenerateRequest(httpClient, config, endpointInfo, parameters, isMulti);
    }

    private RequestConfiguration CreateConfig(EndpointInfo endpointInfo, HttpMethod httpMethod, InstagramClient client, bool isMulti = false)
    {
        var config = new RequestConfiguration
        {
            Endpoint = endpointInfo.Url,
            HttpMethod = httpMethod,
            IsMulti = isMulti,
        };

        return config;
    }

    private HttpRequestMessage CreateRequest(IInstagramHttpClient client, RequestConfiguration configuration, EndpointInfo endpointInfo)
    {
        var baseEndpoint = endpointInfo.BaseEndpoint;
        var url = baseEndpoint + configuration.Endpoint;
        var request = new HttpRequestMessage(configuration.HttpMethod, url);
        request.Version = configuration.HttpMethod == HttpMethod.Put ? HttpVersion.Version11 : HttpVersion.Version20;
        request.VersionPolicy = HttpVersionPolicy.RequestVersionExact;
        return request;
    }

    private HttpRequestMessage GenerateRequest(IInstagramHttpClient client, RequestConfiguration configuration, EndpointInfo endpointInfo, HttpContent content)
    {
        var request = CreateRequest(client, configuration, endpointInfo);

        request.Content = content;
        return request;
    }

    private async Task<HttpRequestMessage> GenerateRequest(IInstagramHttpClient client, RequestConfiguration configuration, EndpointInfo endpointInfo, Dictionary<string, string> parameters, bool ismulti)
    {
        var request = CreateRequest(client, configuration, endpointInfo);
        var signResult = m_Utilities.JsonDeserializeObject<SignJson>(await client.Sign.SignRequest());;

        m_Logger.Debug("Trying to add sign headers to request");
        request.Headers.UserAgent.Clear();
        if (signResult == null)
            throw new SignerException("Could not deserialize SignRequest response");


        request.Headers.TryAddWithoutValidation("x-ig-app-locale", signResult.xigapplocale);
        request.Headers.TryAddWithoutValidation("x-ig-device-locale", signResult.xigdevicelocale);
        request.Headers.TryAddWithoutValidation("x-ig-mapped-locale", signResult.xigmappedlocale);
        request.Headers.TryAddWithoutValidation("x-pigeon-session-id", signResult.xpigeonsessionid);
        request.Headers.TryAddWithoutValidation("x-pigeon-rawclienttime", signResult.xpigeonrawclienttime);
        request.Headers.TryAddWithoutValidation("x-ig-bandwidth-speed-kbps", signResult.xigbandwidthspeedkbps);
        request.Headers.TryAddWithoutValidation("x-ig-bandwidth-totalbytes-b", signResult.xigbandwidthtotalbytesb);
        request.Headers.TryAddWithoutValidation("x-ig-bandwidth-totaltime-ms", signResult.xigbandwidthtotaltimems);
        request.Headers.TryAddWithoutValidation("x-bloks-version-id", signResult.xbloksversionid);
        request.Headers.TryAddWithoutValidation("x-ig-www-claim", signResult.xigwwwclaim);
        request.Headers.TryAddWithoutValidation("x-bloks-is-layout-rtl", signResult.xbloksislayoutrtl);
        request.Headers.TryAddWithoutValidation("x-ig-device-id", signResult.xigdeviceid);
        request.Headers.TryAddWithoutValidation("x-ig-family-device-id", signResult.xigfamilydeviceid);
        request.Headers.TryAddWithoutValidation("x-ig-android-id", signResult.xigandroidid);
        request.Headers.TryAddWithoutValidation("x-ig-timezone-offset", signResult.xigtimezoneoffset);
        request.Headers.TryAddWithoutValidation("x-fb-connection-type", signResult.xfbconnectiontype);
        request.Headers.TryAddWithoutValidation("x-ig-connection-type", signResult.xigconnectiontype);
        request.Headers.TryAddWithoutValidation("x-ig-capabilities", signResult.xigcapabilities);
        request.Headers.TryAddWithoutValidation("x-ig-app-id", signResult.xigappid);
        request.Headers.TryAddWithoutValidation("priority", signResult.priority);
        request.Headers.TryAddWithoutValidation("x-tigon-is-retry", signResult.xtigonisretry);
        request.Headers.TryAddWithoutValidation("user-agent", signResult.useragent);
        request.Headers.TryAddWithoutValidation("accept-language", signResult.acceptlanguage);
        request.Headers.TryAddWithoutValidation("ig-intended-user-id", signResult.igintendeduserid);
        request.Headers.TryAddWithoutValidation("x-fb-http-engine", signResult.xfbhttpengine);
        request.Headers.TryAddWithoutValidation("x-fb-client-ip", signResult.xfbclientip);
        request.Headers.TryAddWithoutValidation("x-fb-server-cluster", signResult.xfbservercluster);
        if (ismulti)
        {
            var content = new MultipartFormDataContent();
            foreach (var parameter in parameters)
            {
                Console.WriteLine(parameter.Key);
                content.Add(new StringContent(parameter.Value), parameter.Key);
            }
            request.Content = content;
        }
        else
        {
            request.Content = new StringContent(m_Utilities.JsonSerializeObject(parameters), Encoding.UTF8, "application/json");
        }
        
        return request;
    }
}