using AkaShi.Infrastructure.Context;

namespace AkaShi.Infrastructure.Repositories.Abstract;

public class BaseRepository : IAsyncDisposable
{
    protected readonly ApplicationDbContext Context;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    public ValueTask DisposeAsync()
    {
        return Context?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}