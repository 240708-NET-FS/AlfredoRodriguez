namespace Program.ApplicationController;

using Program.Utils;


// This second part of the class defines.
// method handlers made for pagination commands.
public partial class Controller
{
    [Command(name:">", description:
    @"[C]>, <
    [E]> Takes you to the next page of the content, if any.
    [E]> Takes you to the previous page of the content, if any.")]
    public int next(String[] args)
    {
        ConsoleScreen.NextPage();
        return 1;
    }

    [Command(name:"<")]
    public int previous(String[] args)
    {
        return 1;
    }
}