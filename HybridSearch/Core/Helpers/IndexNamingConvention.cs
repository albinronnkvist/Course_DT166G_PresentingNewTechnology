namespace AlbinRonnkvist.HybridSearch.Core.Helpers;

public static class IndexNamingConvention
{
    private const string VersionSuffix = "-v";

    public static string GetTemplateName(string indexName) 
        => $"{indexName}-template";
    
    public static string GetTemplatePattern(string indexName) 
        => $"{indexName}*";
    
    public static string GetVersionedindexName(string indexName, int version) 
        => $"{indexName}{VersionSuffix}{version}";

    public static string GetVersionedAlias(string indexName, int version) 
        => $"{indexName}{VersionSuffix}{version}";

    public static string GetSearchAlias(string indexName) 
        => indexName;
}
