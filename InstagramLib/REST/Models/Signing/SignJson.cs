using System.Text.Json.Serialization;

public class PersistentInfo
{
    public string device_id { get; set; }
    public string family_device_id { get; set; }
}

public class PhoneInfo
{
    public string androidId { get; set; }
    public int androidOsApi { get; set; }
    public string androidOsVersion { get; set; }
    public string phoneManufacture { get; set; }
    public string phoneModel { get; set; }
    public string HardwareModel { get; set; }
}

public class InstagramSign
{
    public PhoneInfo phone_info { get; set; }
    public PersistentInfo persistent_info { get; set; }
    public SessionInfo session_info { get; set; }
}

public class SessionInfo
{
    public string session_id { get; set; }
}

public class SignJson
{
    [JsonPropertyName("x-ig-app-locale")]
    public string xigapplocale { get; set; }

    [JsonPropertyName("x-ig-device-locale")]
    public string xigdevicelocale { get; set; }

    [JsonPropertyName("x-ig-mapped-locale")]
    public string xigmappedlocale { get; set; }

    [JsonPropertyName("x-pigeon-session-id")]
    public string xpigeonsessionid { get; set; }

    [JsonPropertyName("x-pigeon-rawclienttime")]
    public string xpigeonrawclienttime { get; set; }

    [JsonPropertyName("x-ig-bandwidth-speed-kbps")]
    public string xigbandwidthspeedkbps { get; set; }

    [JsonPropertyName("x-ig-bandwidth-totalbytes-b")]
    public string xigbandwidthtotalbytesb { get; set; }

    [JsonPropertyName("x-ig-bandwidth-totaltime-ms")]
    public string xigbandwidthtotaltimems { get; set; }

    [JsonPropertyName("x-bloks-version-id")]
    public string xbloksversionid { get; set; }

    [JsonPropertyName("x-ig-www-claim")]
    public string xigwwwclaim { get; set; }

    [JsonPropertyName("x-bloks-is-layout-rtl")]
    public string xbloksislayoutrtl { get; set; }

    [JsonPropertyName("x-ig-device-id")]
    public string xigdeviceid { get; set; }

    [JsonPropertyName("x-ig-family-device-id")]
    public string xigfamilydeviceid { get; set; }

    [JsonPropertyName("x-ig-android-id")]
    public string xigandroidid { get; set; }

    [JsonPropertyName("x-ig-timezone-offset")]
    public string xigtimezoneoffset { get; set; }

    [JsonPropertyName("x-fb-connection-type")]
    public string xfbconnectiontype { get; set; }

    [JsonPropertyName("x-ig-connection-type")]
    public string xigconnectiontype { get; set; }

    [JsonPropertyName("x-ig-capabilities")]
    public string xigcapabilities { get; set; }

    [JsonPropertyName("x-ig-app-id")]
    public string xigappid { get; set; }
    public string priority { get; set; }

    [JsonPropertyName("x-tigon-is-retry")]
    public string xtigonisretry { get; set; }

    [JsonPropertyName("user-agent")]
    public string useragent { get; set; }

    [JsonPropertyName("accept-language")]
    public string acceptlanguage { get; set; }

    [JsonPropertyName("ig-intended-user-id")]
    public string igintendeduserid { get; set; }

    [JsonPropertyName("x-fb-http-engine")]
    public string xfbhttpengine { get; set; }

    [JsonPropertyName("x-fb-client-ip")]
    public string xfbclientip { get; set; }

    [JsonPropertyName("x-fb-server-cluster")]
    public string xfbservercluster { get; set; }
}