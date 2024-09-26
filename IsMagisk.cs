public bool IsMagisk()
{
    // Get the PackageManager instance to retrieve information about installed packages
    PackageManager packageManager = Android.App.Application.Context.PackageManager;
    
    // Retrieve the list of installed packages on the device
    var installedPackages = packageManager.GetInstalledPackages(PackageInfoFlags.MetaData);

    // Iterate through each installed package
    foreach (var package in installedPackages)
    {
        ApplicationInfo appInfo = package.ApplicationInfo;
        var packageName = appInfo.PackageName;
        
        // Construct the full path to the Magisk binary (libstub.so)
        var fullPath = Path.Combine(appInfo.NativeLibraryDir, "libstub.so");
        
        // Check if the Magisk binary exists at the specified path
        if (File.Exists(fullPath))
        {
            // Show a toast message indicating that Magisk is present
            UserDialogs.Instance.Toast("Magisk present: " + packageName + "\n" + fullPath);
            return true; // Magisk is installed
        }
    }
    
    return false; // Magisk is not installed
}
