namespace futr;

public interface ICommandline
{
    HttpContext HttpContext { get; set; }

    Commandline.HandlerMap GetHandlers();
    string Run(string script);
    bool HasRole(Commandline.Handler handler);
}
