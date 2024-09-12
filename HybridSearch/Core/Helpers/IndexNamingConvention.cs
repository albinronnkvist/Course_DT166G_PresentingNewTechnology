using CSharpFunctionalExtensions;

namespace AlbinRonnkvist.HybridSearch.Core.Helpers;

public static class IndexNamingConvention
{
    private const string VersionSuffix = "-v";

    public static string GetTemplateName(string indexName) 
        => $"{indexName}-template";
    
    public static string GetTemplatePattern(string indexName) 
        => $"{indexName}*";
    
    public static string GetVersionedIndexName(string indexName, int version) 
        => $"{indexName}{VersionSuffix}{version}";

    public static string GetVersionedAlias(string indexName, int version) 
        => $"va-{indexName}{VersionSuffix}{version}";

    public static string GetSearchAlias(string indexName) 
        => $"sa-{indexName}";

    public static Result<int, string> ExtractVersionFromIndexName(string indexName)
    {
        int suffixIndex = indexName.LastIndexOf(VersionSuffix);
        if (suffixIndex is -1)
        {
            return Result.Failure<int, string>($"Could not extract version due to nvalid index name: {indexName}");
        }

        string versionString = indexName[(suffixIndex + VersionSuffix.Length)..];

        if (int.TryParse(versionString, out int version))
        {
            return version;
        }

        return Result.Failure<int, string>($"Could not extract version due to nvalid index name: {indexName}");
    }
}
