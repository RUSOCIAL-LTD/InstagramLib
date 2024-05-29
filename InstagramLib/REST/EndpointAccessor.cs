using SnapchatLib;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using InstagramLib.Extras;

namespace InstagramLib.REST;

internal abstract class EndpointAccessor
{
    protected IInstagramHttpClient HttpClient { get; }
    protected InstagramLockedConfig Config { get; }
    protected InstagramClient InstagramClient { get; }
    protected IClientLogger m_Logger;
    protected IUtilities m_Utilities;

    private readonly IRequestConfigurator m_Configurator;

    protected EndpointAccessor() { }

    protected EndpointAccessor(InstagramClient client, IInstagramHttpClient httpClient, InstagramLockedConfig config, IClientLogger logger, IUtilities utilities, IRequestConfigurator requestConfigurator)
    {
        HttpClient = httpClient;
        Config = config;
        InstagramClient = client;
        m_Logger = logger;
        m_Utilities = utilities;
        m_Configurator = requestConfigurator;
    }

    protected virtual async Task<HttpResponseMessage> Send(EndpointInfo endpointInfo, Dictionary<string, string> parameters, bool isMulti = false)
    {
        parameters ??= new Dictionary<string, string>();

        var request = await m_Configurator.Configure(endpointInfo, parameters, HttpMethod.Post, InstagramClient, HttpClient, isMulti);
        return await HttpClient.Send(endpointInfo.Url, request);
    }

    protected virtual async Task<HttpResponseMessage> Send(EndpointInfo endpointInfo, HttpContent streamContent, bool isMulti = false)
    {
        var request = m_Configurator.Configure(endpointInfo, streamContent, HttpMethod.Post, InstagramClient, HttpClient, isMulti);
        return await HttpClient.Send(endpointInfo.Url, request);
    }

    protected virtual async Task<HttpResponseMessage> SendPut(EndpointInfo endpointInfo, Stream stream)
    {
        using var fileStreamContent = new StreamContent(stream);

        var request = m_Configurator.Configure(endpointInfo, fileStreamContent, HttpMethod.Put, InstagramClient, HttpClient);

        m_Logger.Debug($"Calling SendPut to {endpointInfo.Url}. Request Version: {request.Version}. Request Url: {request.RequestUri}");
        return await HttpClient.SendPut(endpointInfo.Url, request, !Config.BandwithSaver);
    }
}