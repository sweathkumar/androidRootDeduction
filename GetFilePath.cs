// Gets the file path for storing files.
public string GetFilePath(string filename)
{
    var context = Android.App.Application.Context;
    return Path.Combine(context.GetExternalFilesDir(null).AbsolutePath, filename);
}
