// Checks if the device is rooted by looking for common root paths and files.
public bool IsRooted()
{
    var rootPaths = new List<string>
    {
        "/sbin", "/system/bin", "/system/xbin", "/data/local/xbin", "/data/local/bin",
        "/system/sd/xbin", "/system/bin/failsafe", "/data/local", "/system/app/Superuser.apk",
        "/system/etc/init.d/99SuperSUDaemon", "/dev/com.koushikdutta.superuser.daemon/",
        "/system/xbin/daemonsu", "/system/xbin/busybox", "/sbin/su", "/system/bin/su",
        "/system/xbin/su", "/data/local/su", "/data/local/xbin/su", "/cache/", "/data/",
        "/dev/", "/su/bin/", "/system/bin/.ext/", "/system/usr/we-need-root/", "/system/xbin/"
    };

    // Check for the presence of 'su' binary in common paths.
    foreach (var path in rootPaths)
    {
        var fullPath = Path.Combine(path, "su");
        if (File.Exists(fullPath))
        {
            UserDialogs.Instance.Toast("Path present: " + path + "\n" + fullPath);
            return true;
        }
    }

    // Check if 'su' binary is in the system PATH.
    var paths = System.Environment.GetEnvironmentVariable("PATH");
    var pathsArray = paths.Split(':');
    foreach (var each in pathsArray)
    {
        var fullPath = Path.Combine(each, "su");
        if (File.Exists(fullPath))
        {
            UserDialogs.Instance.Toast("Split present: " + fullPath);
            return true;
        }
    }

    // Check for known root-related processes.
    var processList = ActivityManager.FromContext(Android.App.Application.Context).RunningAppProcesses;
    foreach (var eachProcess in processList)
    {
        if (eachProcess.ProcessName.Contains("supersu") || eachProcess.ProcessName.Contains("superuser"))
        {
            UserDialogs.Instance.Toast("Process present: " + eachProcess.ProcessName);
            return true;
        }
    }

    // Additional checks for security measures.
    return IsHooked() || IstestKeys() || IsDebuggerConnected() || IsMagisk() || IsMagiskRunning() || IsShamikoModuleActive() || IsShamikoModuleInstalled();
}
