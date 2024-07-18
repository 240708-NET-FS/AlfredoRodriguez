namespace Program.ApplicationController;

using Program.Utils;


// This would be the place where we define all the commands
public partial class Controller
{
    [Command(name:"PAGE", description:
    @"[C]>
    [E]Takes you to the next page of the content, if any.")]
    public int next(String[] args)
    {
        ConsoleScreen.PrintScreen("You just executed the method related to the TEST command !");
        return 1;
    }
}