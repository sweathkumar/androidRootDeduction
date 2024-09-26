// Checks if the build was signed with test keys.
public bool IstestKeys()
{
    string str = Build.Tags;
    return str.Contains("test-keys");
}
