using Microsoft.EntityFrameworkCore;

namespace VideoGameAPI.Controllers.Data;

public class VideoGameDbContext(DbContextOptions<VideoGameDbContext> options) : DbContext(options)
{
    public DbSet<VideoGame> VideoGames => Set<VideoGame>();
}
