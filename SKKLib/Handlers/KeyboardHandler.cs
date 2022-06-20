using System.Collections.Generic;

namespace SKKLib.Handlers
{
    public static class KeyboardHandler
    {
        #region KEYBOARD EVENT VIRTUAL CODES
        public const int KVC_EXTKEY = 0x1;
        public const int KVC_KEYUP  = 0x2;
        public const int KVC_DOWN   = KVC_EXTKEY;
        public const int KVC_UP     = KVC_EXTKEY | KVC_KEYUP;
        public const int KVC_SHIFT  = 0x10;
        public const int KVC_ALT    = 0x12;
        public const int KVC_KEYP8  = 0x68;
        public const int KVC_KEYL   = 0x4C;
        #endregion

        #region TRANSLATE KEY PRESSES TO VIRTUAL CODES
        // Translate exotic keys to virtual code
        public static Dictionary<byte, byte> ChatTrans1 { get; } = new Dictionary<byte, byte>()
        {
            { 0x3B, 0xBA }, // ';'
            { 0x2C, 0xBC }, // ','
            { 0x2E, 0xBE }, // '.'
            { 0x2F, 0xBF }, // '/'
            { 0x27, 0xDE }, // '\''
            { 0x5C, 0xDC }  // '\\'
        };

        // Translate 'shifted' keys to parent key
        public static Dictionary<byte, byte> ChatTrans2 { get; } = new Dictionary<byte, byte>()
        {
            { 0x3F, 0xBF }, // '?' to '/'
            { 0x3A, 0xBA }, // ':' to ';"
            { 0x21, 0x31 }, // '!' to '1'
            { 0x25, 0x35 }, // '%' to '5'
            { 0x5E, 0x36 }, // '^' to '6'
            { 0x26, 0x37 }, // '&' to '7'
            { 0x28, 0x39 }, // '(' to '9'
            { 0x29, 0x30 }  // ')' to '0'
        };
        #endregion

        public static void KeyDown(byte b) => Win32.User32.keybd_event(b, 0x45, KVC_DOWN, 0);
        public static void KeyUp(byte b) => Win32.User32.keybd_event(b, 0x45, KVC_UP, 0);
        public static void KeyPress(byte b)
        {
            KeyDown(b);
            KeyUp(b);
        }
        
        public static void PressKeyByte(byte b, string processName = "")
        {
            bool isCap = b >= 0x41 && b <= 0x5A;
            b = (byte)(b - (b >= 0x61 && b <= 0x7A ? 0x20 : 0x00));

            if (ChatTrans1.ContainsKey(b)) b = ChatTrans1[b];
            if (ChatTrans2.ContainsKey(b)) isCap = (b = ChatTrans2[b]) > 0x00;

            if (processName != "") WindowHandler.Focus(processName);
            if (isCap) KeyDown(KVC_SHIFT);
            KeyPress(b);
            if (isCap) KeyUp(KVC_SHIFT);
        }
    }
}
