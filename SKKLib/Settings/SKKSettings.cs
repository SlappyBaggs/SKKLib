using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace SKKLib.Settings
{
    public static class Settings
    {
        // Settings
        private static List<Type> extra_ = new List<Type>()
        {
            typeof(int?),
            typeof(double?),
            typeof(Point?),
            typeof(Size?),
            typeof(bool?)
        };

        private static List<SettingsOb> settings_ = null;
        private static bool loaded_ = false;
        private static bool hasChanges_ = false;

        private static string settingsFile_ = "SKKSettings.xml";
        public static string SettingsFile
        {
            set { settingsFile_ = value; }
            get { return settingsFile_; }
        }

        public static void AddSettingType(Type t)
        {
            if (!HasSettingType(t)) extra_.Add(t);
        }

        public static bool HasSettingType(Type t) => extra_.Contains(t);

        public static bool HasSetting(string s)
        {
            LoadSettings();
            return settings_.FindIndex(x => x.Key == s) != -1;
        }

        public static string GetSettingString(string s) => (string)GetSetting(s);
        public static string GetSettingString(string s1, string s2, bool save = false)
        {
            if (!HasSetting(s1)) SetSetting(s1, s2, save);
            return GetSettingString(s1);
        }

        public static int? GetSettingInt(string s) => (int?)GetSetting(s);
        public static int GetSettingInt(string s, int? i, bool save = false)
        {
            if (!HasSetting(s)) SetSetting(s, i, save);
            return GetSettingInt(s).Value;
        }

        public static double? GetSettingDouble(string s) => (double?)GetSetting(s);
        public static double GetSettingDouble(string s, double? d, bool save = false)
        {
            if (!HasSetting(s)) SetSetting(s, d, save);
            return GetSettingDouble(s).Value;
        }

        public static Point? GetSettingPoint(string s) => (Point?)GetSetting(s);
        public static Point GetSettingPoint(string s, Point? p, bool save = false)
        {
            if(!HasSetting(s)) SetSetting(s, p, save);
            return GetSettingPoint(s).Value;
        }

        public static Size? GetSettingSize(string s) => (Size?)GetSetting(s);
        public static Size GetSettingSize(string s, Size? sz, bool save = false)
        {
            if (!HasSetting(s)) SetSetting(s, sz, save);
            return GetSettingSize(s).Value;
        }

        public static bool? GetSettingBool(string s)
        {
            object o = GetSetting(s);
            if (o.GetType().Name == "Int32")
            {
                RemoveSetting(s);
                SetSetting(s, (bool)((int)o != 0), true);
                return (int)o != 0;
            }
            return (bool?)o;
        }
        public static bool GetSettingBool(string s, bool? b, bool save = false)
        {
            if (!HasSetting(s)) SetSetting(s, b, save);
            return GetSettingBool(s).Value;
        }

        public static T GetSetting<T>(string s)
        {
            AddSettingType(typeof(T));
            object o = GetSetting(s);
            return (T)o;
        }

        public static T GetSetting<T>(string s, T t, bool save = false)
        {
            AddSettingType(typeof(T));
            if (!HasSetting(s))
                SetSetting(s, t, save);
            return GetSetting<T>(s);
        }

        public static object GetSetting(string s)
        {
            LoadSettings();
            SettingsOb so = settings_.Find(x => x.Key == s);
            Type t = so.Value.GetType();
            return (so is null) ? null : so.Value;// /*|| so.Value is null*/) ? null : JsonConvert.DeserializeObject(so.Value.ToString());
        }

        public static void SetSetting(string k, object v, bool save = false)
        {
            AddSettingType(v.GetType());
            LoadSettings();

            SettingsOb so = settings_.Find(x => x.Key == k);// ?? new SettingsOb(k, null);
            if (so is null) settings_.Add(so = new SettingsOb(k, null));
            if (so.Value is null || !so.Value.Equals(v))
            {
                hasChanges_ = true;
                so.Value = v;
            }
            
            if(save && hasChanges_) SaveSettings();
        }

        public static void RemoveSetting(string s, bool save = false)
        {
            if (HasSetting(s))
            {
                settings_.Remove(settings_.Find(x => x.Key == s));
                hasChanges_ = true;
                SaveSettings();
            }
            
            if (save && hasChanges_) SaveSettings();
        }

        private static void LoadSettings()
        {
            if (loaded_) return;
            
            if (!File.Exists(settingsFile_)) { settings_ = new List<SettingsOb>(); }
            else
            {
                try
                {
                    using (FileStream stream = new FileStream(SettingsFile, FileMode.Open))
                    {
                        settings_ = (List<SettingsOb>)new XmlSerializer(typeof(List<SettingsOb>), extra_.ToArray()).Deserialize(stream);
                    }
                }
                catch { settings_ = null; }
            }

            if(settings_ is null) settings_ = new List<SettingsOb>();
            loaded_ = true;
        }

        public static void SaveSettings()
        {
            if (!hasChanges_ || !loaded_) return;
            try
            {
                using (StreamWriter sw = new StreamWriter(settingsFile_))
                {
                    XmlSerializer xmlwriter = new XmlSerializer(settings_.GetType(), extra_.ToArray());
                    xmlwriter.Serialize(sw, settings_);
                    sw.Close();
                    hasChanges_ = false;
                }
            }
            catch (Exception /*e*/)
            {
                System.Diagnostics.Debugger.Break();
            }
        }
    }

    [XmlInclude(typeof(Size))]
    [XmlInclude(typeof(List<string>))]
    public class SettingsOb
    {
        public SettingsOb()
        {
            Key = null;
            Value = null;
        }

        public SettingsOb(string k, object v)
        {
            Key = k;
            Value = v;
        }

        public string Key { get; set; }
        public object Value { get; set; }
        public override string ToString() => $"{Key}";
    }
}
