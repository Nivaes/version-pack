#load "GenerateVersionPatch.csx"
#load "GetLastMinorVersion.csx"

(string versionNumber, string versionPack, string versionRelease) GenerateVersion(string versionRelease)
{
    return VersionLogic.GenerateVersion(versionRelease);
}
