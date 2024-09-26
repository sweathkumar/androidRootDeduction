public bool IsShamikoModuleInstalled()
{
    string shamikoPath = "/data/adb/modules/shamiko";
    return Directory.Exists(shamikoPath); // Returns true if Shamiko module is installed
}
