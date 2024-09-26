using System.Collections.Generic;
using System.IO;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MMFInvestorApp.Droid.Utils;
using MMFInvestorApp.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckHardware))]
namespace MMFInvestorApp.Droid.Utils
{
    public class CheckHardware : IHardwareSecurity
    {
        public CheckHardware() { }

        // Checks if the device is jailbroken by looking for common jailbreak paths and files.
        public bool IsJailBreaked()
        {
            var jailbreakPaths = new List<string>
            {
                "/sbin", "/system/bin", "/system/xbin", "/data/local/xbin", "/data/local/bin",
                "/system/sd/xbin", "/system/bin/failsafe", "/data/local", "/system/app/Superuser.apk",
                "/system/etc/init.d/99SuperSUDaemon", "/dev/com.koushikdutta.superuser.daemon/",
                "/system/xbin/daemonsu", "/system/xbin/busybox", "/sbin/su", "/system/bin/su",
                "/system/xbin/su", "/data/local/su", "/data/local/xbin/su", "/cache/", "/data/",
                "/dev/", "/su/bin/", "/system/bin/.ext/", "/system/usr/we-need-root/", "/system/xbin/"
            };

            // Check for the presence of 'su' binary in common paths.
            foreach (var path in jailbreakPaths)
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

            // Check for known jailbreak-related processes.
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

        // Checks if the app is running in an emulator.
        public bool IsInEmulator()
        {
            string str = Build.Fingerprint;
            return str.Contains("vbox") || str.Contains("generic");
        }

        // Checks if the build was signed with test keys.
        public bool IstestKeys()
        {
            string str = Build.Tags;
            return str.Contains("test-keys");
        }

        // Checks if the application is debuggable.
        public bool IsDebuggable()
        {
            return ((Android.App.Application.Context.ApplicationContext.ApplicationInfo.Flags & ApplicationInfoFlags.Debuggable) != 0);
        }

        // Checks if a debugger is currently connected.
        public bool IsDebuggerConnected()
        {
            return Debug.IsDebuggerConnected;
        }

        // Checks for common hooking applications installed on the device.
        public bool IsHooked()
        {
            var hookList = new List<string>
            {
                "com.topjohnwu.magisk", "com.chelpus.luckypatcher", "com.keramidas.TitaniumBackup",
                "com.devadvance.rootcloak2", "com.thirdparty.superuser", "eu.chainfire.supersu",
                "com.noshufou.android.su", "com.koushikdutta.superuser", "com.zachspong.temprootremovejb",
                "com.ramdroid.appquarantine", "de.robv.android.xposed.installer", "com.saurik.substrate",
                "com.amphoras.hidemyroot", "com.amphoras.hidemyrootadfree", "com.devadvance.rootcloak",
                "com.devadvance.rootcloakplus", "com.formyhm.hiderootPremium", "com.chelpus.lackypatch",
                "com.dimonvideo.luckypatcher", "com.android.vending.billing.InAppBillingService.COIN",
                "com.koushikdutta.rommanager", "com.koushikdutta.rommanager.license",
                "com.ramdroid.appquarantinepro", "com.noshufou.android.su.elite"
            };

            var packageManager = Android.App.Application.Context.PackageManager;
            var applicationInfoList = packageManager.GetInstalledApplications(PackageInfoFlags.MetaData);

            // Check for the presence of hooking packages.
            foreach (var applicationInfo in applicationInfoList)
            {
                foreach (var pckg in hookList)
                {
                    if (applicationInfo.PackageName == pckg)
                    {
                        UserDialogs.Instance.Toast("Package present: " + pckg);
                        return true;
                    }
                }
            }
            return false;
        }

        // Checks for the presence of Magisk on the device.
        public bool IsMagisk()
        {
            var packageManager = Android.App.Application.Context.PackageManager;
            var installedPackages = packageManager.GetInstalledPackages(PackageInfoFlags.MetaData);

            foreach (var package in installedPackages)
            {
                ApplicationInfo appInfo = package.ApplicationInfo;
                var fullPath = Path.Combine(appInfo.NativeLibraryDir, "libstub.so");
                if (File.Exists(fullPath))
                {
                    UserDialogs.Instance.Toast("Magisk present: " + appInfo.PackageName + "\n" + fullPath);
                    return true;
                }
            }
            return false;
        }

        // Gets the file path for storing files.
        public string GetFilePath(string filename)
        {
            var context = Android.App.Application.Context;
            return Path.Combine(context.GetExternalFilesDir(null).AbsolutePath, filename);
        }

        // Checks if Magisk is currently running.
        public bool IsMagiskRunning()
        {
            var magiskPaths = new List<string>
            {
                "/sbin/magisk", "/system/bin/magisk", "/system/xbin/magisk"
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

        // Checks if the Shamiko module is active.
        public bool IsShamikoModuleActive()
        {
            const string shamikoConfigPath = "/data/adb/modules/shamiko/module.prop";
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

        // Checks if the Shamiko module is installed.
        public bool IsShamikoModuleInstalled()
        {
            const string shamikoPath = "/data/adb/modules/shamiko";
            return Directory.Exists(shamikoPath); // Returns true if Shamiko module is installed
        }
    }
}
