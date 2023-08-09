using GameConsole;
using Sandbox.Arm;

namespace BadAppleSkulls.Commands
{
    public class BadAppleScreenCommand: ICommand
    {
        public string Name => nameof(BadAppleScreenCommand);

        public string Description => "Places a skull screen for playing Bad Apple on it later";

        public string Command => "bad_apple_screen";
        
        public void Execute(Console console, string[] args)
        {
            if (MonoSingleton<SandboxArm>.Instance == null)
            {
                console.PrintLine("Please select spawner arm first", ConsoleLogType.Error);
                return;
            }
            MonoSingleton<BadAppleScreenController>.Instance.PlaceScreenAnchors();
        }
    }
}