using CliFx.Infrastructure;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Adapters
{
    internal static class ConsoleAdapter
    {
        public static IAnsiConsole ConvertToAnsiConsole(ref IConsole console)
        {
            return AnsiConsole.Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.Detect,
                Out = console.Output
            });
        }
    }
}