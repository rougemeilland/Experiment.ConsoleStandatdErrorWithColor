using System;

namespace Experiment.ConsoleStandatdErrorWithColor.Experiment01
{
    /// <summary>
    /// A test program to check the strange behavior of the Console class.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Back up the current foreground color before changing the foreground color.
            var defaultColor = Console.ForegroundColor;

            // Change the foreground color to blue.
            Console.ForegroundColor = ConsoleColor.Blue;

            // Display with "Console.WriteLine(string?)".
            Console.WriteLine("This text is expected to be displayed in blue. (to default (==stdout))");

            // Display with "Console.Out.WriteLine(string?)"
            Console.Out.WriteLine("This text is expected to be displayed in blue. (to stdout)");

            // Display with "Console.Error.WriteLine(string?)"
            Console.Error.WriteLine("This text is expected to be displayed in blue. (to stderr)");

            // Display the foreground color of the console.
            Console.WriteLine($"The current console foreground color is \"{Console.ForegroundColor}\" (to stdout).");
            Console.Error.WriteLine($"The current console foreground color is \"{Console.ForegroundColor}\" (to stderr).");

            // Restore the foreground color.
            Console.ForegroundColor = defaultColor;
        }
    }

    // [Result]
    // Only in case (2) below (that is, when standard output is redirected), the foreground color of characters displayed on the console is not changed.
    //
    // 1) If no redirect is done, the execution result will be displayed as follows:
    //
    //     This text is expected to be displayed in blue. (to default (==stdout))    <= displayed in blue. (OK)
    //     This text is expected to be displayed in blue. (to stdout)                <= displayed in blue. (OK)
    //     This text is expected to be displayed in blue. (to stderr)                <= displayed in blue. (OK)
    //     The current console foreground color is "Blue" (to stdout).               <= displayed in blue. (OK)
    //     The current console foreground color is "Blue" (to stderr).               <= displayed in blue. (OK)
    //      
    // 2) If the standard output is redirected to a file, the execution result will be displayed as follows.:
    //
    //     This text is expected to be displayed in blue. (to stderr)                <= Displayed in the default color. (NG)
    //     The current console foreground color is "Gray" (to stderr).               <= Displayed in the default color. (NG)
    //      
    // 3) If the standard error is redirected to a file, the execution result will be displayed as follows.:
    //
    //     This text is expected to be displayed in blue. (to default (==stdout))    <= displayed in blue. (OK)
    //     This text is expected to be displayed in blue. (to stdout)                <= displayed in blue. (OK)
    //     The current console foreground color is "Blue" (to stdout).               <= displayed in blue. (OK)
    //
    // [Additional Info] 
    // Below is the URL of an article on "stack overflow".
    // According to Mark Lakata's comments on this article, it appears that the issue already existed in December 2012.
    // https://stackoverflow.com/questions/10532796/setting-the-color-for-console-error-writes

}
