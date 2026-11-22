using Xunit;

public class VersionLogicTests
{
    [Theory]
    [InlineData("dev", "-alpha")]
    [InlineData("alpha", "-alpha")]
    [InlineData("beta", "-beta")]
    [InlineData("preview", "-preview")]
    [InlineData("rc", "-rc")]
    [InlineData("candidate", "-rc")]
    [InlineData("release", "-preview")]
    public void BranchPrefixSelectsDocumentedPrerelease(string branch, string expected)
    {
        Assert.Equal(expected, VersionLogic.GenerateVersionPatch(branch, "", false));
    }

    [Theory]
    [InlineData("alpha", "-alpha")]
    [InlineData("BETA", "-beta")]
    [InlineData("rc", "-rc")]
    public void ExplicitPrereleaseOverridesBranch(string prerelease, string expected)
    {
        Assert.Equal(expected, VersionLogic.GenerateVersionPatch("release", prerelease, false));
    }

    [Theory]
    [InlineData("release/1.2.3", "1.2.3", "1.2-preview.3")]
    [InlineData("beta/1.2.3", "1.2.3", "1.2-beta.3")]
    [InlineData("rc/1.2.3", "1.2.3", "1.2-rc.3")]
    [InlineData("candidate/2.4.0", "2.4.0", "2.4-rc.0")]
    public void ExistingReleaseProducesExpectedVersionOutputs(string input, string number, string pack)
    {
        var result = VersionLogic.GenerateVersion(input);

        Assert.Equal(number, result.numberVersion);
        Assert.Equal(pack, result.versionPack);
        Assert.Equal(input, result.versionRelease);
    }

    [Theory]
    [InlineData("1.2.3")]
    [InlineData("release/1")]
    public void InvalidReleaseIsRejected(string input)
    {
        Assert.Throws<ArgumentException>(() => VersionLogic.GenerateVersion(input));
    }
}
