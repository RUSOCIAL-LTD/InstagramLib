using System.Collections.Generic;
using System.Threading.Tasks;
using InstagramLib.Extras;
using InstagramLib.REST;
using InstagramLib;

namespace SnapchatLib.REST.Endpoints;

public interface IExampleEndpoint
{
    Task<string> ExampleMethod(string exampleparam);
}

internal class ExampleEndpoint : EndpointAccessor, IExampleEndpoint
{
    internal static readonly EndpointInfo Example = new()
    {
        Url = "/Example",
        BaseEndpoint = RequestConfigurator.AttestationBaseEndpoint,
    };

    public ExampleEndpoint(InstagramClient client, IInstagramHttpClient httpClient, InstagramLockedConfig config, IClientLogger logger, IUtilities utilities, IRequestConfigurator configurator) : base(client, httpClient, config, logger, utilities, configurator)
    {
    }
    public async Task<string> ExampleMethod(string exampleparam)
    {
        var parameters = new Dictionary<string, string>
        {
            {"example_key", "example_value"},
        };
        var response = await Send(Example, parameters);
        return m_Utilities.JsonDeserializeObject<string>(await response.Content.ReadAsStringAsync());
    }
}