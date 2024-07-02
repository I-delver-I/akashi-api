namespace AkaShi.Core.Helpers.RepositoryParams;

public class LibraryParams : PaginatedParams
{
    public LibrariesFilter LibrariesFilter { get; set; }
    public SortBy SortBy { get; set; }
    public string SearchTerm { get; set; }
}