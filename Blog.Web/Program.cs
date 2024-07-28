using Core.Framework;

namespace Blog.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var microApp = new MicroApp(args);
        
        microApp.RegisterApiDefaults();
        
        microApp.Run();
    }
}
