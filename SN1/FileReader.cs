using CsvHelper;
using Microsoft.Win32;
using SN1.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SN1
{
    public class FileReader
    {
        public FileReader()
        {

        }

        public object DialogResult { get; private set; }

        public List<ThreeVariableItem> GetItems()
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "All Files (*.*)|*.*";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;
            choofdlog.ShowDialog();
            string fileName = choofdlog.FileName; //used when Multiselect = true           

            using (TextReader reader = File.OpenText(fileName))
            {
                var list = new List<ThreeVariableItem>();
                var stringList = new List<string>();
                var csv = new CsvReader(reader);

                csv.Configuration.CultureInfo = CultureInfo.CurrentCulture;

                try {
                    while (csv.Read())
                    {

                        var field1 = Double.Parse(csv.GetField<string>(0));
                        var field2 = Double.Parse(csv.GetField<string>(1));
                        var field3 = csv.GetField<int>(2);
                        list.Add(new ThreeVariableItem() { x = field1, y = field2, cls = field3 });
                    }
                }
                catch
                {
                 while(csv.Read())
                    {
                        stringList.Add(csv.GetField<string>(0));
                    }   
                }
                return list;
                //                var records = csv.GetRecords<ThreeVariableItem>().ToList();
                //return records;
            }            
        }
    }
}
