using System.Collections.Generic;
using QuanLyTTBTv2.Models.Local;

namespace QuanLyTTBTv2.Services;

public class DbSettings
{
    public Dictionary<string, DbSettingDto> Data { get; set; } = new();

    public int GetIntValue(string key, int defaultValue = 0)
    {
        if (Data.TryGetValue(key, out DbSettingDto? v))
        {
            if (v.Kieu == SettingValueTypes.Number && v.Value != null) return int.Parse(v.Value);
        }

        return defaultValue;
    }

    public double GetDoubleValue(string key, double defaultValue = 0)
    {
        if (Data.TryGetValue(key, out DbSettingDto? v))
        {
            if (v is { Kieu: SettingValueTypes.Number, Value: not null })
                if (double.TryParse(v.Value, out double dv))
                    return dv;
        }

        return defaultValue;
    }

    public double GetDoubleValueFromString(string key, double defaultValue = 0)
    {
        if (Data.TryGetValue(key, out DbSettingDto? v))
        {
            if (v.Value != null)
                if (double.TryParse(v.Value, out double dv))
                    return dv;
        }

        return defaultValue;
    }

    public bool GetBoolValue(string key, bool defaultValue = false)
    {
        if (Data.TryGetValue(@key, out DbSettingDto? v))
        {
            return v.Value == "True";
        }

        return defaultValue;
    }

    public string? GetValue(string key, string? def = null)
    {
        if (Data.TryGetValue(key, out DbSettingDto? value)) return value.Value;
        return def;
    }

    public void LoadSetting(DbSettingDto s)
    {
        if (Data.ContainsKey(s.Name))
        {
            Data[s.Name] = s;
        }
        else
        {
            Data.Add(s.Name, s);
        }

        s.Changed = false;
    }

    public void Update(string name, string? val, SettingValueTypes t = SettingValueTypes.String)
    {
        if (!Data.ContainsKey(name))
        {
            Data.Add(name, new DbSettingDto(name) { Kieu = t, Value = val });
        }
        else
        {
            Data[name].Value = val;
        }
    }

    public void UpdateBool(string name, bool val)
    {
        Update(name, val.ToString(), SettingValueTypes.Boolean);
    }

    public void UpdateDouble(string name, double val)
    {
        Update(name, val.ToString(), SettingValueTypes.Number);
    }

    public void UpdateDouble(string name, string val)
    {
        Update(name, val, SettingValueTypes.Number);
    }
}