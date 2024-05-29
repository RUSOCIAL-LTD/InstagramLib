using System;
using System.Net;
using InstagramLib.Extras;
using static InstagramLib.Extras.Utilities;

namespace InstagramLib;
public class InstagramConfig
{
    private string _ApiKey;
    public WebProxy Proxy { get; set; }
    public static bool IsBase64String(string base64)
    {
        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
    }
    public string ApiKey
    {
        get => _ApiKey;
        set
        {
            _ApiKey = value;
            if (string.IsNullOrEmpty(_ApiKey)) throw new ArgumentNullException("ApiKey Cannot be empty");
        }
    }

    public bool Debug { get; set; } = false;
    public bool ProxySigner { get; set; } = false;
    public bool BandwithSaver { get; set; } = true;
    public int Timeout { get; set; }
    public string androidId { get; set; } = AndroidIDGenerator.GenerateAndroidID();
    public string Username { get; set; }
    public string ro_product_product_manufacturer { get; set; }
    public string phoneModel { get; set; }
    public string ro_build_version_release { get; set; }
    public string HardwareModel { get; set; }
    public int ro_build_version_sdk { get; set; }

    internal readonly IUtilities Utilities;
    internal InstagramConfig(IUtilities utilities)
    {
        Utilities = utilities;
    }

    public InstagramConfig()
    {
        Utilities = new Utilities();
    }
}

public class InstagramLockedConfig
{
    public InstagramLockedConfig(InstagramConfig config)
    {
        androidId = config.androidId;
        Username = config.Username;
        ApiKey = config.ApiKey;
        Proxy = config.Proxy;
        Debug = config.Debug;
        BandwithSaver = config.BandwithSaver;
        Timeout = config.Timeout;
        ProxySigner = config.ProxySigner;
        ro_product_product_manufacturer = config.ro_product_product_manufacturer;
        HardwareModel = config.HardwareModel;
        phoneModel = config.phoneModel;
        ro_build_version_release = config.ro_build_version_release;
        ro_build_version_sdk = config.ro_build_version_sdk;
    }
    public WebProxy Proxy { get; set; }
    public string ApiKey { get; set; }
    public string Username { get; set; }
    public string androidId { get; set; }
    public bool Debug { get; set; }
    public bool BandwithSaver { get; set; }
    public bool ProxySigner { get; set; }
    public int Timeout { get; set; }
    public string ro_product_product_manufacturer { get; set; }
    public string phoneModel { get; set; }
    public string ro_build_version_release { get; set; }
    public int ro_build_version_sdk { get; set; }
    public string HardwareModel { get; set; }
}