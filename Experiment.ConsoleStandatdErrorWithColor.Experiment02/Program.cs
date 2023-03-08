using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Experiment.ConsoleStandatdErrorWithColor.Experiment02
{
    /// <summary>
    /// A test program to check whether the standard error output can be colored or not at the Win32 API level.
    /// </summary>
    internal class Program
    {
        #region Win32 API declaration

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct COORD
        {
            internal short X;
            internal short Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal partial struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }

        internal enum Color : short
        {
            Black = 0,
            ForegroundBlue = 0x1,
            ForegroundGreen = 0x2,
            ForegroundRed = 0x4,
            ForegroundYellow = 0x6,
            ForegroundIntensity = 0x8,
            BackgroundBlue = 0x10,
            BackgroundGreen = 0x20,
            BackgroundRed = 0x40,
            BackgroundYellow = 0x60,
            BackgroundIntensity = 0x80,

            ForegroundMask = 0xf,
            BackgroundMask = 0xf0,
            ColorMask = 0xff
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal short wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
        }

        [DllImport("kernel32.dll")]
        extern static IntPtr GetStdHandle(uint nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool GetConsoleScreenBufferInfo(IntPtr hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        extern static bool SetConsoleTextAttribute(IntPtr hConsoleOutput, short wAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        extern static bool WriteFile(IntPtr handle, byte[] bytes, int numBytesToWrite, out int numBytesWritten, IntPtr mustBeZero);

        #endregion


        #region Main code

        public static void Main(string[] args)
        {
            // get stdout handle
            var stdoutHandle = GetStdHandle(unchecked((uint)-11));

            // get stderr handle
            var stderrHandle = GetStdHandle(unchecked((uint)-12));

            // Code that examines the effect on stdout and stderr of changing the stdout foreground color.
            {
                // Change the standard output foreground color to blue.
                SetConsoleTextAttribute(stdoutHandle, Color.ForegroundBlue);
                Write(stderrHandle, $"The standard output foregroundcolor has been changed to \"Blue\".\r\n");

                // Write text to standard output.
                Write(stdoutHandle, $"This text is printed to standard output and is expected to be displayed in blue.\r\n");
                // Write text to standard error.
                Write(stderrHandle, $"This text is being printed to standard error and is expected to be displayed in blue.\r\n");

                // Reset standard output foreground color to default.
                SetConsoleTextAttribute(stdoutHandle, (Color)0x07);
                Write(stderrHandle, $"The standard output foregroundcolor was reset.\r\n");
            }

            // Code that examines the effect on stdout and stderr of changing the stderr foreground color.
            {
                // Change the standard error foreground color to red.
                SetConsoleTextAttribute(stderrHandle, Color.ForegroundRed);
                Write(stderrHandle, $"The standard output foregroundcolor has been changed to \"Red\".\r\n");

                // Write text to standard output.
                Write(stdoutHandle, $"This text is printed to standard output and is expected to be displayed in blue.\r\n");
                // Write text to standard error.
                Write(stderrHandle, $"This text is being printed to standard error and is expected to be displayed in blue.\r\n");

                // Reset standard error foreground color to default.
                SetConsoleTextAttribute(stderrHandle, (Color)0x07);
                Write(stderrHandle, $"The standard output foregroundcolor was reset.\r\n");
            }
        }

        // [Result]
        // 1) If the redirect was not done, the following execution result will be displayed on the console.:
        //     
        //     The standard output foregroundcolor has been changed to "Blue".                           <= displayed in blue. (OK)
        //     This text is printed to standard output and is expected to be displayed in blue.          <= displayed in blue. (OK)
        //     This text is being printed to standard error and is expected to be displayed in blue.     <= displayed in blue. (OK)
        //     The standard output foregroundcolor was reset.                                            <= displayed in default color. (OK)
        //     The standard output foregroundcolor has been changed to "Red".                            <= displayed in red. (OK)
        //     This text is printed to standard output and is expected to be displayed in blue.          <= displayed in red. (OK)
        //     This text is being printed to standard error and is expected to be displayed in blue.     <= displayed in red. (OK)
        //     The standard output foregroundcolor was reset.                                            <= displayed in default color. (OK)
        //
        // 2) If the standard output is redirected, the following execution results will be displayed on the console.:
        //     
        //     The standard output foregroundcolor has been changed to "Blue".                           <= displayed in default color. (OK)
        //     This text is being printed to standard error and is expected to be displayed in blue.     <= displayed in default color. (OK)
        //     The standard output foregroundcolor was reset.                                            <= displayed in default color. (OK)
        //     The standard output foregroundcolor has been changed to "Red".                            <= displayed in red. (OK)
        //     This text is being printed to standard error and is expected to be displayed in blue.     <= displayed in red. (OK)
        //     The standard output foregroundcolor was reset.                                            <= displayed in default color. (OK)
        //
        // 3) If the standard error is redirected, the following execution results will be displayed on the console.:
        //     
        //     This text is printed to standard output and is expected to be displayed in blue.          <= displayed in blue. (OK)
        //     This text is printed to standard output and is expected to be displayed in blue.          <= displayed in default color. (OK)
        //


        #endregion

        #region Win32API capsule method code

        /// <summary>
        /// Get CONSOLE_SCREEN_BUFFER_INFO structure.
        /// </summary>
        /// <param name="handle">
        /// A handle to stdout or stderr.
        /// This handle points to a console or file.
        /// </param>
        /// <returns>
        /// Returns the CONSOLE_SCREEN_BUFFER_INFO structure if successful, null otherwise.
        /// </returns>
        /// <remarks>
        /// Returns null if <paramref name="handle"/> is redirected to a file.
        /// </remarks>
        private static CONSOLE_SCREEN_BUFFER_INFO? GetConsoleScreenBufferInfo(IntPtr handle)
        {
            return
                GetConsoleScreenBufferInfo(handle, out CONSOLE_SCREEN_BUFFER_INFO consoleInfo)
                ? consoleInfo
                : null;
        }

        /// <summary>
        /// Sets the foreground color for the console indicated by the handle.
        /// </summary>
        /// <param name="handle">
        /// Console handle.
        /// </param>
        /// <param name="color">
        /// foreground color.
        /// </param>
        /// <remarks>
        /// If the <paramref name="handle"/> has been redirected, do nothing.
        /// </remarks>
        private static void SetConsoleTextAttribute(IntPtr handle, Color color)
        {
            if (GetConsoleScreenBufferInfo(handle) is not null)
            {
                if (!SetConsoleTextAttribute(handle, (short)color))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }

        /// <summary>
        /// Writes a string to the specified handle.
        /// </summary>
        /// <param name="handle">
        /// A handle to write to.
        /// </param>
        /// <param name="text">
        /// string to write.
        /// </param>
        /// <remarks>
        /// Strings are written in UTF-8 encoding.
        /// </remarks>
        private static void Write(IntPtr handle, string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var bytesLength = bytes.Length;
            while (bytesLength > 0)
            {
                if (!WriteFile(handle, bytes, bytesLength, out int written, IntPtr.Zero))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                Array.Copy(bytes, written, bytes, 0, bytesLength - written);
                bytesLength -= written;
            }
        }

        #endregion
    }
}
