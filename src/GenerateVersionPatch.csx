#load "VersionLogic.cs"

string GenerateVersionPatch(string patchType, string prerelease, bool canRelease) =>
    VersionLogic.GenerateVersionPatch(patchType, prerelease, canRelease);
