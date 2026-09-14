using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuanLyTTBTv2.Services;

public class SrvComm
{
    public const string SoftwareName = "QuanLyTTBTv2";
    
    // Auth Server Address
    private string? _addr;
    public SrvDbBridge SrvDb { get; private set; } = new();

    public bool IsServerOk { get; private set; }
    public bool IsServerDbOk { get; private set; }
    // public bool IsLocalOk { get; private set; }
    
    /// <summary>
    /// Kết nối qua user, password để lấy connstr
    /// </summary>
    public async Task<string?> Connect(string addr, string username, string password)
    {
        _addr = addr;
        string url = addr.StartsWith("http") ? $"{addr}:10001/bttt/login" : $"http://{addr}:10001/bttt/login";
        HttpClient client = new();

        client.DefaultRequestHeaders.UserAgent.ParseAdd($"{SoftwareName}/1.0");
        var jsonStr = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";
        var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync(url, content);

            response.EnsureSuccessStatusCode();

            // Read response JSON
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var csdlparams = doc.RootElement
                .GetProperty("account")
                .GetProperty("csdl")
                .GetString();

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now:HH:mm:ss} Thông tin csdl: {csdlparams}");
            InitServerConnection(csdlparams, addr);

            return csdlparams;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        return null;
    }

    private void InitServerConnection(string? csdlparams, string? host = null)
    {
        IsServerOk = csdlparams != null;
        if (csdlparams == null) return;

        string[] ss = csdlparams.Split(';');
        if (host == null)
        {
            if (ss.Length > 3)
            {
                IsServerDbOk = SrvDb.Initialize(ss[3], ss[0], ss[1], ss[2]);
            }
        }
        else
        {
            if (ss.Length > 2)
            {
                IsServerDbOk = SrvDb.Initialize(host, ss[0], ss[1], ss[2]);
            }
        }
    }
    
    public void SyncDb()
    {
        if (IsServerOk)
        {
            SrvDb.SyncSchema();
        }
    }
}