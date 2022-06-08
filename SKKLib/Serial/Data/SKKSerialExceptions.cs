using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKKLib.Serial.Data
{
    public class SKKSerialException : Exception
    {
        public SKKSerialException() : this("Serial Port Exception") { }
        public SKKSerialException(string s) : base(s) { }
    }

    public class SKKSerialPortOpenException : SKKSerialException
    {
        public SKKSerialPortOpenException() : this("Serial Port is open.") { }
        public SKKSerialPortOpenException(string s) : base(s) { }
    }

    public class SKKSerialPortClosedException : SKKSerialException
    {
        public SKKSerialPortClosedException() : this("Serial Port is closed.") { }
        public SKKSerialPortClosedException(string s) : base(s) { }
    }

    public class SKKSerialTimeoutException : TimeoutException
    {
        public SKKSerialTimeoutException() : this("Serial Port timed out.") { }
        public SKKSerialTimeoutException(string s) : base(s) { }
    }

    public class SKKSerialPartialTimeoutException : TimeoutException
    {
        public SKKSerialPartialTimeoutException() : this("Serial Port partial time out.") { }
        public SKKSerialPartialTimeoutException(Object o) : this() { }
        public SKKSerialPartialTimeoutException(string s) : this(null, s) { }
        public SKKSerialPartialTimeoutException(Object o, string s) : base(s) { data = o; }

        public Object data;
    }
}
