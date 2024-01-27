namespace AkaShi.Core.Helpers.RepositoryParams;

public class LibraryParams : PaginatedParams
{
    public FilterParams FilterParams { get; set; }
    public OrderByParams? OrderByParams { get; set; }
    public string SearchTerm { get; set; }
}