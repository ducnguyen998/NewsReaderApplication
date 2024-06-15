using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Services
{
    public class MouseService
    {
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        const uint INPUT_MOUSE = 0;
        const uint MOUSEEVENTF_MOVE = 0x0001;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);


        public static void ClickAtPosition(int x, int y)
        {
            // Set the cursor position
            SetCursorPos(x, y);

            // Create the INPUT structure for mouse down
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = INPUT_MOUSE;
            mouseDownInput.u.mi = new MOUSEINPUT();
            mouseDownInput.u.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;

            // Create the INPUT structure for mouse up
            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = INPUT_MOUSE;
            mouseUpInput.u.mi = new MOUSEINPUT();
            mouseUpInput.u.mi.dwFlags = MOUSEEVENTF_LEFTUP;

            // Send the inputs
            INPUT[] inputs = new INPUT[] { mouseDownInput, mouseUpInput };
            SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendMouseMove(int x, int y)
        {
            SetCursorPos((int)x, (int)y);
        }
    }
}
