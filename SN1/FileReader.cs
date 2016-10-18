using CsvHelper;
using Microsoft.Win32;
using SN1.Items;
using System;
using System.Collections.Generic;
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
                var csv = new CsvReader(reader);
                while (csv.Read())
                {
                    var field1 = csv.GetField<double>(0);
                    var field2= csv.GetField<double>(1);
                    var field3 = csv.GetField<int>(2);
                    list.Add(new ThreeVariableItem() { x = field1, y = field2, cls = field3 });
                }
                return list;
                //                var records = csv.GetRecords<ThreeVariableItem>().ToList();
                //return records;
            }            
        }
    }
}
