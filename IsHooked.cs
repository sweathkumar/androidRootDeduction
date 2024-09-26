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
