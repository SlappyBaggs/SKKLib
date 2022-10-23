using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKKLib.DB.Elements
{
    public class Report
    {
        private static Report _instance;
        public static Report Instance { get => _instance??(_instance = new Report()); }

        private List<string> reportEntries = new List<string>();
        private Report()
        {
        }

        public static void AddReport(string s)
        {
            Instance.reportEntries.Add(s);
            ReportEvent(s);
        }

        public static List<string> ReportEntries => Instance.reportEntries;

        public delegate void ReportHandler(string s);
        public static event ReportHandler ReportEvent = delegate { };

        public static List<string> ReservedWords;
    }
}
