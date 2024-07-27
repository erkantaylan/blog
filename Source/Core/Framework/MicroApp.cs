using Microsoft.AspNetCore.Builder;

namespace Core.Framework;

public class MicroApp(string[] args)
{
    private readonly List<Action<WebApplication>> appActions = [];
    private readonly List<Action<WebApplicationBuilder>> webActions = [];
    private readonly WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

    public MicroApp RegisterBuilder(Action<WebApplicationBuilder> webAction)
    {
        webActions.Add(webAction);

        return this;
    }

    public MicroApp RegisterApp(Action<WebApplication> appAction)
    {
        appActions.Add(appAction);

        return this;
    }

    public MicroApp Register(Action<WebApplicationBuilder> builder, Action<WebApplication> app)
    {
        RegisterBuilder(builder);
        RegisterApp(app);

        return this;
    }

    public void Run()
    {
        foreach (Action<WebApplicationBuilder> action in webActions)
        {
            action.Invoke(webApplicationBuilder);
        }

        WebApplication app = webApplicationBuilder.Build();

        foreach (Action<WebApplication> action in appActions)
        {
            action.Invoke(app);
        }

        app.Run();
    }
}
