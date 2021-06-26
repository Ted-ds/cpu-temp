using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cpu_Temp
{
    class App
    {
        static void Main()
        {
            Program a = new Program();
            a.Print_ThermalZone();
        }
    }

    class Program
    {
        const string CpuTempFilePath = @"/sys/devices/virtual/thermal/thermal_zone3/temp";
        const string ThermalZonePath = @"/sys/devices/virtual/thermal";

        double Average_temp;
        Dictionary<int, string> ThermalZoneDictionary;

        public void run()
        {
            try
            {
                string text = System.IO.File.ReadAllText(CpuTempFilePath);
                text = text.Trim();
                double temp = double.Parse(text)/1000;
                Console.WriteLine(temp+" degrees");
            }
            catch
            {
                Console.WriteLine("Couldn't read cpu temperature.");
            }
        }

        void Add_ThermalZone()
        {
            string[] zones;

            try
            {
                zones = Directory.GetDirectories(ThermalZonePath, @"thermal_zone*");
            }
            catch
            {
                Console.WriteLine("Can't find Thermalzone.");
                return;
            }            

            //thermalZoneList = new List<string>();
            ThermalZoneDictionary = new Dictionary<int, string>();

            foreach(string a in zones)
            {
                string name = Path.GetFileName(a);
                int number;
                if(Int32.TryParse(name.Substring("thermal_zone".Length), out number))
                {
                    //Console.WriteLine(number);
                    ThermalZoneDictionary.Add(number, a + @"/temp");
                }
            }

            //var sorted = from entry in ThermalZoneDictionary orderby entry.Key ascending select entry;
            //ThermalZoneDictionary = sorted.ToDictionary(x => x.Key, x => x.Value);
        }

        public void Print_ThermalZone()
        {
            int i =0;
            Average_temp = 0.0;
            Add_ThermalZone();

            if((ThermalZoneDictionary != null) && (ThermalZoneDictionary.Count != 0))
            {
                var sorted = from entry in ThermalZoneDictionary orderby entry.Key ascending select entry;

                foreach(var a in sorted)
                {
                    double temp = get_zone_thermal(a.Value);
                    Average_temp += temp;
                    Console.WriteLine($"Zone{a.Key}: {temp}'C");
                    i++;
                }

                Average_temp = Average_temp / i;
                Console.WriteLine("Average: {0:0.00}'C", Average_temp);
            }
        }

        string get_thermal_temp(string path)
        {
            try
            {
                string text = System.IO.File.ReadAllText(path);
                text = text.Trim();
                double temp = double.Parse(text)/1000;

                return temp+" degrees";
            }
            catch
            {
                return "Couldn't read temperature.";
            }
        }

        double get_zone_thermal(string path)
        {
            try
            {
                string text = System.IO.File.ReadAllText(path);
                text = text.Trim();
                double temp = double.Parse(text)/1000;
                
                return temp;
            }
            catch
            {
                return 0.0;
            }
        }
    }
}
