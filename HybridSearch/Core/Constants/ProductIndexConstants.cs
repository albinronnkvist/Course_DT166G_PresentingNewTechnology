namespace AlbinRonnkvist.HybridSearch.Core.Constants;

public class ProductIndexConstants
{
    public const string IndexName = "product";
    private const string VersionSuffix = "-v";

    public string GetTemplateName() 
        => $"{IndexName}-template";
    
    public string GetTemplatePattern() 
        => $"{IndexName}*";
    
    public string GetVersionedIndexName(int version) 
        => $"{IndexName}{VersionSuffix}{version}";

    public string GetVersionedAlias(int version) 
        => $"{IndexName}{VersionSuffix}{version}";

    public string GetSearchAlias() 
        => IndexName;
}
