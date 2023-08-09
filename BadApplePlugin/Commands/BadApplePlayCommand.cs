using GameConsole;

namespace BadAppleSkulls.Commands
{
    public class BadApplePlayCommand: ICommand
    {
        public string Name => nameof(BadApplePlayCommand);

        public string Description => "Plays some wacky video from 2009 on blue skulls screen";

        public string Command => "bad_apple_play";
        
        public void Execute(Console console, string[] args)
        {
            var badApple = MonoSingleton<BadAppleScreenController>.Instance;
            if (!badApple.screenPresent)
            {
                console.PrintLine("You must place the skulls screen first! Place it using bad_apple_screen command.", ConsoleLogType.Error);
                return;
            }
            
            badApple.StartCoroutine(badApple.PlayBadApple());
        }
    }
}