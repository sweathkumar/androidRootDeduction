// Checks if the app is running in an emulator.
public bool IsInEmulator()
{
    string str = Build.Fingerprint;
    return str.Contains("vbox") || str.Contains("generic");
}
