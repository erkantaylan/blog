using System.Diagnostics.CodeAnalysis;
using Projects;

namespace Startup;

[SuppressMessage("ReSharper", "SuggestVarOrType_Elsewhere")]
public static class AspireProgram
{
    public static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        var blogDb = builder
                    .AddPostgres("db-blog")
                    .WithPgAdmin(null, "db-panel-postgres")
                    .PublishAsContainer()
                    .AddDatabase("cs-blog", "blog");

        builder.AddProject<Blog_Web>("web-blog")
               .WithReference(blogDb);

        builder.Build().Run();
    }
}
