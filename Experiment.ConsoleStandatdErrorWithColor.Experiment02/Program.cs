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
                SetConsoleTextAttribute(stdoutHandle, Color.ForegroundBlue, out string? errorMessage);
                if (errorMessage is not null)
                {
                    Write(stdoutHandle, $"Failed to change standard output foreground color. : \"{errorMessage}\"\r\n");
                    Write(stderrHandle, $"Failed to change standard output foreground color. : \"{errorMessage}\"\r\n");
                }
                else
                    Write(stderrHandle, "The standard output foregroundcolor has been changed to blue.\r\n");

                // Write text to standard output.
                Write(stdoutHandle, "This text is printed to standard output and should appear in blue.\r\n");
                // Write text to standard error.
                Write(stderrHandle, "This text is printed to standard error and should appear in blue.\r\n");

                // Reset standard output foreground color to default.
                SetConsoleTextAttribute(stdoutHandle, (Color)0x07, out errorMessage);
                if (errorMessage is not null)
                {
                    Write(stdoutHandle, $"Failed to reset standard output foreground color. : \"{errorMessage}\"\r\n");
                    Write(stderrHandle, $"Failed to reset standard output foreground color. : \"{errorMessage}\"\r\n");
                }
                else
                    Write(stderrHandle, "The standard output foregroundcolor was reset.\r\n");
            }

            // Code that examines the effect on stdout and stderr of changing the stderr foreground color.
            {
                // Change the standard error foreground color to red.
                SetConsoleTextAttribute(stderrHandle, Color.ForegroundRed, out string? errorMessage);
                if (errorMessage is not null)
                {
                    Write(stdoutHandle, $"Failed to change standard error foreground color. : \"{errorMessage}\"\r\n");
                    Write(stderrHandle, $"Failed to change standard error foreground color. : \"{errorMessage}\"\r\n");
                }
                else
                    Write(stderrHandle, $"The standard error foregroundcolor has been changed to red.\r\n");

                // Write text to standard output.
                Write(stdoutHandle, "This text is printed to standard output and should appear in red.\r\n");
                // Write text to standard error.
                Write(stderrHandle, "This text is printed to standard error and should appear in red.\r\n");

                // Reset standard error foreground color to default.
                SetConsoleTextAttribute(stderrHandle, (Color)0x07, out errorMessage);
                if (errorMessage is not null)
                {
                    Write(stdoutHandle, $"Failed to reset standard error foreground color. : \"{errorMessage}\"\r\n");
                    Write(stderrHandle, $"Failed to reset standard error foreground color. : \"{errorMessage}\"\r\n");
                }
                else
                    Write(stderrHandle, $"The standard error foregroundcolor was reset.\r\n");
            }
        }

        // [Result]
        // 1) If the redirect was not done, the following execution result will be displayed on the console.:
        //     
        //     The standard output foregroundcolor has been changed to blue.        <= displayed in blue.
        //     This text is printed to standard output and should appear in blue.   <= displayed in blue.
        //     This text is printed to standard error and should appear in blue.    <= displayed in blue.
        //     The standard output foregroundcolor was reset.                       <= displayed in default color.
        //     The standard error foregroundcolor has been changed to red.          <= displayed in red.
        //     This text is printed to standard output and should appear in red.    <= displayed in red.
        //     This text is printed to standard error and should appear in red.     <= displayed in red.
        //     The standard error foregroundcolor was reset.                        <= displayed in default color.
        //
        // 2) If the standard output is redirected, the following execution results will be displayed on the console.:
        //
        //     Failed to change standard output foreground color. : "ハンドルが無効です。 (0x80070006 (E_HANDLE))"
        //     This text is printed to standard error and should appear in blue.    <= displayed in default color. (Because it fails to change the foreground color of the standard output)
        //     Failed to reset standard output foreground color. : "ハンドルが無効です。 (0x80070006 (E_HANDLE))"
        //     The standard error foregroundcolor has been changed to red.          <= displayed in red.
        //     This text is printed to standard error and should appear in red.     <= displayed in red.
        //     The standard error foregroundcolor was reset.                        <= displayed in default color.
        //
        // 3) If the standard error is redirected, the following execution results will be displayed on the console.:
        //
        //     This text is printed to standard output and should appear in blue.   <= displayed in blue.
        //     Failed to change standard error foreground color. : "ハンドルが無効です。 (0x80070006 (E_HANDLE))"
        //     This text is printed to standard output and should appear in red.    <= displayed in default color. (Because it fails to change the foreground color of the standard error)
        //     Failed to reset standard error foreground color. : "ハンドルが無効です。 (0x80070006 (E_HANDLE))"
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
            if (!GetConsoleScreenBufferInfo(handle, out CONSOLE_SCREEN_BUFFER_INFO consoleInfo))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
                throw new Exception("internal error");
            }
            return consoleInfo;
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
        /// <param name="errorMessage">
        /// the message if an error occurred, null otherwise
        /// </param>
        /// <remarks>
        /// If the <paramref name="handle"/> has been redirected, do nothing.
        /// </remarks>
        private static void SetConsoleTextAttribute(IntPtr handle, Color color, out string? errorMessage)
        {
            try
            {
                if (!SetConsoleTextAttribute(handle, (short)color))
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                errorMessage = null;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
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
