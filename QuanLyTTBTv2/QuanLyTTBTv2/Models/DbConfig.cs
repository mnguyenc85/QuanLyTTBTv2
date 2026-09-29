namespace QuanLyTTBTv2.Models;

public class DbConfig
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string? Db { get; set; }
    public string User { get; set; }
    public string Pw { get; set; }

    public DbConfig(string srv, int port, string? db, string user, string pw)
    {
        Server = srv;
        Port = port;
        Db = db;
        User = user;
        Pw = pw;
    }
    
    public string CreateConnStr(bool createDb = false)
    {
        if (Port < 0)
        {
            if (Db != null && !createDb) return $"Server={Server};Database={Db};User={User};Password={Pw};";
            return $"Server={Server};User={User};Password={Pw};";
        }
        else
        {
            if (Db != null && !createDb) return $"Server={Server};Port={Port};Database={Db};User={User};Password={Pw};";
            return $"Server={Server};Port={Port};User={User};Password={Pw};";
        }
    }
}