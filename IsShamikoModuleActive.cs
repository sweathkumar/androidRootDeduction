public bool IsShamikoModuleActive()
{
    string shamikoConfigPath = "/data/adb/modules/shamiko/module.prop";
    if (File.Exists(shamikoConfigPath))
    {
        var lines = File.ReadAllLines(shamikoConfigPath);
        foreach (var line in lines)
        {
            if (line.StartsWith("enabled=1"))
            {
                return true; // Shamiko module is active
            }
        }
    }
    return false; // Shamiko module is not active
}
