using System.Runtime.InteropServices;


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
