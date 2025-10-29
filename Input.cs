using NAudio.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


enum KeyState {
    press,
    pressDown,
    pressUp,
}

namespace ConsoleRenderer {
    public class Input {

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState (int vKey);


        public static bool GetKey (int virtualkeyCode) {
            short s = GetAsyncKeyState(virtualkeyCode);
            return (s & 0x8000) > 0;
        }
        public static bool GetKeyDown (int virtualkeyCode) {
            int s = Convert.ToInt32(GetAsyncKeyState(virtualkeyCode));
            return (s == -32767);
        }

    }
}
