using System;

namespace Experiment.ConsoleStandatdErrorWithColor.Experiment04
{
    public static class ConsoleColorExtensions
    {
        /// <summary>
        /// コンソールの前景色を初期状態に戻す ANSI エスケープコードです。
        /// </summary>
        private const string _ansiEscapeCodeToResetForegroundColor = "\x1b[39m";

        /// <summary>
        /// コンソールの背景色を初期状態に戻す ANSI エスケープコードです。
        /// </summary>
        private const string _ansiEscapeCodeToResetBackgroundColor = "\x1b[49m";

        /// <summary>
        /// コンソールの前景色を <see cref="ConsoleColor"/> 型で与えられた色に変更する ANSI エスケープコードを取得します。
        /// </summary>
        /// <param name="color">
        /// <see cref="ConsoleColor"/>型の値です。
        /// </param>
        /// <returns>
        /// コンソールの前景色を <see cref="ConsoleColor"/> 型で与えられた色に変更する ANSI エスケープコードです。
        /// </returns>
        public static string ToForeGroundColorAnsiEscapeCode(this ConsoleColor color)
        {
            return
                color switch
                {
                    ConsoleColor.Black => "\x1b[30m",
                    ConsoleColor.DarkBlue => "\x1b[34m",
                    ConsoleColor.DarkGreen => "\x1b[32m",
                    ConsoleColor.DarkCyan => "\x1b[36m",
                    ConsoleColor.DarkRed => "\x1b[31m",
                    ConsoleColor.DarkMagenta => "\x1b[35m",
                    ConsoleColor.DarkYellow => "\x1b[33m",
                    ConsoleColor.Gray => "\x1b[37m",
                    ConsoleColor.DarkGray => "\x1b[90m",
                    ConsoleColor.Blue => "\x1b[94m",
                    ConsoleColor.Green => "\x1b[92m",
                    ConsoleColor.Cyan => "\x1b[96m",
                    ConsoleColor.Red => "\x1b[91m",
                    ConsoleColor.Magenta => "\x1b[95m",
                    ConsoleColor.Yellow => "\x1b[93m",
                    ConsoleColor.White => "\x1b[97m",
                    _ => _ansiEscapeCodeToResetForegroundColor,
                };
        }

        /// <summary>
        /// コンソールの背景色を <see cref="ConsoleColor"/> 型で与えられた色に変更する ANSI エスケープコードを取得します。
        /// </summary>
        /// <param name="color">
        /// <see cref="ConsoleColor"/>型の値です。
        /// </param>
        /// <returns>
        /// コンソールの背景色を <see cref="ConsoleColor"/> 型で与えられた色に変更する ANSI エスケープコードです。
        /// </returns>
        public static string ToBackGroundColorAnsiEscapeCode(this ConsoleColor color)
        {
            return
                color switch
                {
                    ConsoleColor.Black => "\x1b[40m",
                    ConsoleColor.DarkBlue => "\x1b[44m",
                    ConsoleColor.DarkGreen => "\x1b[42m",
                    ConsoleColor.DarkCyan => "\x1b[46m",
                    ConsoleColor.DarkRed => "\x1b[41m",
                    ConsoleColor.DarkMagenta => "\x1b[45m",
                    ConsoleColor.DarkYellow => "\x1b[43m",
                    ConsoleColor.Gray => "\x1b[47m",
                    ConsoleColor.DarkGray => "\x1b[100m",
                    ConsoleColor.Blue => "\x1b[104m",
                    ConsoleColor.Green => "\x1b[102m",
                    ConsoleColor.Cyan => "\x1b[106m",
                    ConsoleColor.Red => "\x1b[101m",
                    ConsoleColor.Magenta => "\x1b[105m",
                    ConsoleColor.Yellow => "\x1b[103m",
                    ConsoleColor.White => "\x1b[107m",
                    _ => _ansiEscapeCodeToResetBackgroundColor,
                };
        }
    }
}
