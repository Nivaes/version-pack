using System;

public static class VersionLogic
{
    public static string GenerateVersionPatch(string patchType, string prerelease, bool canRelease)
    {
        patchType = (patchType ?? string.Empty).ToLowerInvariant();
        prerelease = (prerelease ?? string.Empty).ToLowerInvariant();

        if (prerelease == "alpha") return "-alpha";
        if (prerelease == "beta") return "-beta";
        if (prerelease == "rc") return "-rc";

        if (patchType.StartsWith("dev", StringComparison.Ordinal)) return "-alpha";
        if (patchType.StartsWith("a", StringComparison.Ordinal)) return "-alpha";
        if (patchType.StartsWith("b", StringComparison.Ordinal)) return "-beta";
        if (patchType.StartsWith("pre", StringComparison.Ordinal)) return "-preview";
        if (patchType == "rc" || patchType.Contains("can", StringComparison.Ordinal)) return "-rc";

        return canRelease ? string.Empty : "-preview";
    }

    public static (string numberVersion, string versionPack, string versionRelease) GenerateVersion(string versionRelease)
    {
        var parts = versionRelease.Split('/');
        if (parts.Length < 2)
            throw new ArgumentException($"Version release name '{versionRelease}' does not contain a valid version number.", nameof(versionRelease));

        var versionParts = parts[1].Split('.');
        if (versionParts.Length < 2)
            throw new ArgumentException($"Version release name '{versionRelease}' does not contain a valid version number.", nameof(versionRelease));

        var versionMajor = string.Join(".", versionParts[..^1]);
        var versionMinor = versionParts[^1];
        var versionPrerelease = GenerateVersionPatch(parts[0], string.Empty, true);
        var numberVersion = $"{versionMajor}.{versionMinor}";
        var versionPack = $"{versionMajor}{versionPrerelease}.{versionMinor}";
        return (numberVersion, versionPack, versionRelease);
    }
}
