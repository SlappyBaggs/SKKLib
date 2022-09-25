using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Megahard.Tester
{
    public class TestParser
    {
        private Regex regEx_;
        public TestParser()
        {
            regEx_ = new Regex("[a-zA-Z0-9]([a-zA-Z0-9])");
        }

        public Test ParseTest(string fileName)
        {
            Test ret = new Test();

            FileStream ReadFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader streamRead = new StreamReader(ReadFileStream);
            string s = streamRead.ReadLine();
            int i;
            while (s != null)
            {
                s = s.Replace("%FLOWRANGE", "438.0");
                if (!regEx_.IsMatch(s))
                {
                    // Bad line, bail!
                    break;
                }
                i = s.IndexOf('(');
                ret.AddCommand(s.Substring(0, i), s.Substring(i + 1, s.Length - 2 - i));
                s = streamRead.ReadLine();
            }

            streamRead.Close();
            ReadFileStream.Close();
            return ret;
        }
    }

    public class Test
    {
        public Test()
        {
            cmds_ = new List<Command>();
        }

        private List<Command> cmds_;
        public List<Command> Commands { get { return cmds_; } }
        
        public void AddCommand(string c, string a) { AddCommand(new Command(c, a)); }
        public void AddCommand(Command c) { cmds_.Add(c); }
    }

    public class Command
    {
        public Command(string c, string a)
        {
            cmd_ = c;
            args_ = new TestValue(a);
            subCmds_ = new List<Command>();
        }

        private string cmd_;
        public string Cmd { get { return cmd_; } }

        private TestValue args_;
        public string Args { get { return args_.ToString(); } }

        private List<Command> subCmds_;
        public List<Command> SubCmds { get { return subCmds_; } }
        public void AddCommand(string c, string a) { AddSubCommand(new Command(c, a)); }
        public void AddSubCommand(Command c) { subCmds_.Add(c); }
    }

    internal enum TestFunction
    {
        None = 0,
        Multiplication = 1,
        Division = 2,
        Addition = 3,
        Subtraction = 4
    }

    internal class TestValue
    {
        internal TestValue(string s)
        {
            myVal_ = 0.0;
            s.Replace(" ", "");
            parenthesesRegex_ = new Regex(@"[^\(]*\((.*)\).*");
            ParseVal(s);
        }

        private double myVal_;
        private Regex parenthesesRegex_;
        
        private void ParseVal(string s)
        {
            ExpPack pack = GetExp(s);
            while (pack.start != -1)
            {
                TestValue tv = new TestValue(pack.what);
                s = s.Remove(pack.start, pack.length);
                s = s.Insert(pack.start, tv.ToString());
                pack = GetExp(s);
            }
            
            int i;
            TestFunction tf = TestFunction.None;

             if ((i = s.IndexOf('+')) != -1)
                tf = TestFunction.Addition;
            else if ((i = s.IndexOf('-')) != -1)
                tf = TestFunction.Subtraction;
            else if ((i = s.IndexOf('*')) != -1)
                tf = TestFunction.Multiplication;
            else if ((i = s.IndexOf('/')) != -1)
                tf = TestFunction.Division;

            
            if (tf == TestFunction.None)
            {
                myVal_ = Double.Parse(s);
            }
            else
            {
                TestValue v1 = new TestValue(s.Substring(0, i));
                TestValue v2 = new TestValue(s.Substring(i + 1));

                if (tf == TestFunction.Multiplication)   myVal_ = v1.Value * v2.Value;
                else if (tf == TestFunction.Division)    myVal_ = v1.Value / v2.Value;
                else if (tf == TestFunction.Addition)    myVal_ = v1.Value + v2.Value;
                else if (tf == TestFunction.Subtraction) myVal_ = v1.Value - v2.Value;
            }
        }

        internal ExpPack GetExp(string s)
        {
            ExpPack ret = new ExpPack();

            int fp = s.IndexOf("(");
            ret.start = fp;
            int c = (fp == -1)? 0 : 1;
            char x;
            while (c > 0)
            {
                fp++;
                x = s[fp];
                if (x == '(')
                    c++;
                else if (x == ')')
                    c--;
            }
            if (ret.start > -1)
            {
                ret.length = fp - ret.start + 1;
                ret.what = s.Substring(ret.start + 1, ret.length - 2);
            }
            return ret;
        }
        
        internal struct ExpPack
        {
            internal int start;
            internal int length;
            internal string what;
        }

        internal double Value
        {
            get { return myVal_; }
        }

        public override string ToString()
        {
            return myVal_.ToString();
        }
    }
}
