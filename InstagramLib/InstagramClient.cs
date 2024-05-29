using SnapchatLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using InstagramLib.Extras;
using InstagramLib.Models;
using InstagramLib.REST;
using static InstagramLib.Models.LoginModel;

namespace InstagramLib;

public class InstagramClient : IDisposable
{
    internal InstagramClient() { }

    internal IClientLogger m_Logger;

    internal string sessionsId = Guid.NewGuid().ToString();

    internal InstagramClient(InstagramConfig config, IInstagramHttpClient httpClient, IClientLogger logger, IUtilities utilities, IRequestConfigurator configurator)
    {
        InstagramConfig = new InstagramLockedConfig(config);
        HttpClient = httpClient;
        m_Logger = logger;
        m_Utilities = utilities;
        m_RequestConfigurator = configurator;

    }

    public InstagramClient(InstagramConfig config)
    {
        InstagramConfig = new InstagramLockedConfig(config);
        m_Logger = new ClientLogger(InstagramConfig);
        m_Utilities = config.Utilities;

        m_RequestConfigurator = new RequestConfigurator(m_Logger, m_Utilities);

        HttpClient = new InstagramHttpClient(this, m_Logger, m_Utilities, m_RequestConfigurator);
    }



    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
    // Use C# finalizer syntax for finalization code.
    // This finalizer will run only if the Dispose method
    // does not get called.
    // It gives your base class the opportunity to finalize.
    // Do not provide finalizer in types derived from this class.
    ~InstagramClient()
    {
        // Do not re-create Dispose clean-up code here.
        // Calling Dispose(disposing: false) is optimal in terms of
        // readability and maintainability.
        Dispose();
    }
    static long LongRandom(long min, long max, Random rand)
    {
        byte[] buf = new byte[8];
        rand.NextBytes(buf);
        long longRand = BitConverter.ToInt64(buf, 0);
        return (Math.Abs(longRand % (max - min)) + min);
    }
    #region Fields
    internal bool HasLoggedIn { get; set; }
    internal virtual IInstagramHttpClient HttpClient { get; }
    public virtual InstagramLockedConfig InstagramConfig { get; }

    internal readonly IUtilities m_Utilities;
    private readonly IRequestConfigurator m_RequestConfigurator;

    #endregion

    #region Sign
    internal virtual async Task<string> SignRequest()
    {
        return await HttpClient.Sign.SignRequest();
    }
    #endregion

    #region Examples
    internal virtual async Task<string> ExampleMethod(string exampleparams)
    {
        return await HttpClient.Examples.ExampleMethod(exampleparams);
    }
    #endregion

    #region Login
    /*
    public virtual async Task<LoginResponse> Login(string password)
    {
        return await HttpClient.Login.Login(password);
    }
    */
    #endregion
}