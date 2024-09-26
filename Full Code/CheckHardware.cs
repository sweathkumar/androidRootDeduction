using System.Collections.Generic;
using System.IO;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MMFInvestorApp.Droid.Utils;
using MMFInvestorApp.Interfaces;
using Xamarin.Forms;

// Registering the CheckHardware class as a dependency for hardware security checks
[assembly: Dependency(typeof(CheckHardware))]
namespace MMFInvestorApp.Droid.Utils
{
    public class CheckHardware : IHardwareSecurity
    {
        public CheckHardware() { }

        // Method to check if the device is jailbroken/rooted
        public bool IsJailBreaked()
        {
            // List of common paths where root binaries can be found
            List<string> list = new List<string>
            {
                "/sbin",
                "/system/bin",
                "/system/xbin",
                "/data/local/xbin",
                "/data/local/bin",
                "/system/sd/xbin",
                "/system/bin/failsafe",
                "/data/local",
                "/system/app/Superuser.apk",
                "/system/etc/init.d/99SuperSUDaemon",
                "/dev/com.koushikdutta.superuser.daemon/",
                "/system/xbin/daemonsu",
                "/system/xbin/busybox",
                "/sbin/su",
                "/system/bin/su",
                "/system/xbin/su",
                "/data/local/su",
                "/data/local/xbin/su",
                "/cache/",
                "/data/",
                "/dev/",
                "/su/bin/",
                "/system/bin/.ext/",
                "/system/usr/we-need-root/",
                "/system/xbin/"
            };

            // Check each path for the existence of the 'su' binary
            foreach (var path in list)
            {
                var fullPath = Path.Combine(path, "su");
                if (File.Exists(fullPath))
                {
                    UserDialogs.Instance.Toast("Path present: " + path + "\n" + fullPath);
                    return true; // Root detected
                }
            }

            // Check the system's PATH environment variable for 'su' binary
            var paths = System.Environment.GetEnvironmentVariable("PATH");
            var pathsArray = paths.Split(':');
            foreach (var each in pathsArray)
            {
                string fullPath = Path.Combine(each, "su");
                if (File.Exists(fullPath))
                {
                    UserDialogs.Instance.Toast("Split present: " + fullPath);
                    return true; // Root detected
                }
            }

            // Check running processes for known root applications
            IList<ActivityManager.RunningAppProcessInfo> processList
                                 = ActivityManager.FromContext(Android.App.Application.Context).RunningAppProcesses;
            foreach (var eachProcess in processList)
            {
                if (eachProcess.ProcessName.Contains("supersu"))
                {
                    UserDialogs.Instance.Toast("Process present: supersu");
                    return true; // Root detected
                }
                if (eachProcess.ProcessName.Contains("superuser"))
                {
                    UserDialogs.Instance.Toast("Process present: superuser");
                    return true; // Root detected
                }
            }

            // Perform additional checks for root status
            if (IsHooked()) return true;
            if (IstestKeys()) return true;
            if (IsDebuggerConnected()) return true;

            return false; // No root detected
        }

        // Method to check if the application is running in an emulator
        public bool IsInEmulator()
        {
            string str = Build.Fingerprint;
            return str.Contains("vbox") || str.Contains("generic");
        }

        // Method to check if the device is using test keys
        public bool IstestKeys()
        {
            string str = Build.Tags;
            return str.Contains("test-keys");
        }

        // Method to check if the application is built in debug mode
        public bool IsDebuggable()
        {
            return ((Android.App.Application.Context.ApplicationContext.ApplicationInfo.Flags & ApplicationInfoFlags.Debuggable) != 0);
        }

        // Method to check if a debugger is currently connected to the application
        public bool IsDebuggerConnected()
        {
            return Debug.IsDebuggerConnected;
        }

        // Method to check for installed hooking applications
        public bool IsHooked()
        {
            // List of known hooking applications
            List<string> hookList = new List<string>
            {
                "com.topjohnwu.magisk",
                "com.chelpus.luckypatcher",
                "com.keramidas.TitaniumBackup",
                "com.devadvance.rootcloak2",
                "com.thirdparty.superuser",
                "eu.chainfire.supersu",
                "com.noshufou.android.su",
                "com.koushikdutta.superuser",
                "com.zachspong.temprootremovejb",
                "com.ramdroid.appquarantine",
                "de.robv.android.xposed.installer",
                "com.saurik.substrate",
                "com.amphoras.hidemyroot",
                "com.amphoras.hidemyrootadfree",
                "com.devadvance.rootcloak",
                "com.devadvance.rootcloakplus",
                "com.formyhm.hiderootPremium",
                "com.chelpus.lackypatch",
                "com.dimonvideo.luckypatcher",
                "com.android.vending.billing.InAppBillingService.COIN",
                "com.koushikdutta.rommanager",
                "com.koushikdutta.rommanager.license",
                "com.ramdroid.appquarantinepro",
                "com.noshufou.android.su.elite"
            };

            // Check for installed hooking applications
            PackageManager packageManager = Android.App.Application.Context.PackageManager;
            var applicationInfoList = packageManager.GetInstalledApplications(PackageInfoFlags.MetaData);

            foreach (var applicationInfo in applicationInfoList)
            {
                foreach (var pckg in hookList)
                {
                    if (applicationInfo.PackageName == pckg)
                    {
                        UserDialogs.Instance.Toast("Package present: " + pckg);
                        return true; // Hooking app detected
                    }
                }
            }
            return false; // No hooking apps detected
        }

        // Method to check for the presence of Magisk
        public bool IsMagisk()
        {
            PackageManager packageManager = Android.App.Application.Context.PackageManager;
            var installedPackages = packageManager.GetInstalledPackages(PackageInfoFlags.MetaData);

            // Check for Magisk-related library files
            foreach (var package in installedPackages)
            {
                ApplicationInfo appInfo = package.ApplicationInfo;
                var fullPath = Path.Combine(appInfo.NativeLibraryDir, "libstub.so");
                if (File.Exists(fullPath))
                {
                    UserDialogs.Instance.Toast("Magisk present: " + package.PackageName + "\n" + fullPath);
                    return true; // Magisk detected
                }
            }
            return false; // No Magisk detected
        }

        // Method to get the file path for saving files
        public string GetFilePath(string filename)
        {
            var context = Android.App.Application.Context;
            return Path.Combine(context.GetExternalFilesDir(null).AbsolutePath, filename);
        }
    }
}
