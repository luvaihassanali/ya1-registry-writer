#r ".\assets\newtonsoft.json\net45\Newtonsoft.json.dll"

using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public class RegistryData
{
    public string Type { get; set; }
    public string RegType { get; set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
}

public class Startup
{
    public async Task<object> Invoke(dynamic input)
    {
        string rawData = (string)input;
        List<RegistryData> rdList = JsonConvert.DeserializeObject<List<RegistryData>>(rawData);

        RegistryKey key = null;
        foreach (RegistryData rd in rdList)
        {
            key = Registry.LocalMachine.CreateSubKey(rd.Path.Replace("HKEY_LOCAL_MACHINE\\",""));
            string regTypeStr = rd.RegType.ToLower();
            switch (regTypeStr)
            {
                case "binary":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.Binary);
                    break;
                case "dword":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.DWord);
                    break;
                case "expandstring":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.ExpandString);
                    break;
                case "multistring":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.MultiString);
                    break;
                case "none":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.None);
                    break;
                case "qword":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.QWord);
                    break;
                case "string":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.String);
                    break;
                case "unknown":
                    key.SetValue(rd.Name, rd.Value, RegistryValueKind.Unknown);
                    break;
                default:
                    return "Failed: RegType not recognized for entry " + rd.Name;
            }
            key.Close();
        }
        try { key.Dispose(); } catch (Exception e) { return "Failed: " + e.Message; }
        return "Success!";
    }
}