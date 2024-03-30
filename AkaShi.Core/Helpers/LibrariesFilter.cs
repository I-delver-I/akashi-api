namespace AkaShi.Core.Helpers;

public class LibrariesFilter
{
    public bool DotNet { get; set; }
    public bool DotNetCore { get; set; }
    public bool DotNetFramework { get; set; }
    public bool DotNetStandard { get; set; }
    
    public static LibrariesFilter ParseFromString(string frameworkProductNames)
    {
        var filter = new LibrariesFilter();
        if (string.IsNullOrEmpty(frameworkProductNames))
        {
            return filter;
        }

        var frameworkList = frameworkProductNames.Split(',');
        foreach (var framework in frameworkList)
        {
            switch (framework.Trim().ToLower())
            {
                case "net":
                    filter.DotNet = true;
                    break;
                case "netcoreapp":
                    filter.DotNetCore = true;
                    break;
                case "netframework":
                    filter.DotNetFramework = true;
                    break;
                case "netstandard":
                    filter.DotNetStandard = true;
                    break;
            }
        }

        return filter;
    }
}