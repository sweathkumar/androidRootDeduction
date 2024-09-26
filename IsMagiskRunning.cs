public bool IsMagiskRunning()
{
    List<string> magiskPaths = new List<string>
    {
        "/sbin/magisk",
        "/system/bin/magisk",
        "/system/xbin/magisk"
    };

    foreach (var path in magiskPaths)
    {
        if (File.Exists(path))
        {
            return true; // Magisk is running
        }
    }
    return false; // Magisk is not running
}
