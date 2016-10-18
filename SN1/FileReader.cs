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
                var csv = new CsvReader(reader);
                var records = csv.GetRecords<ThreeVariableItem>().ToList();
                return records;
            }            
        }
    }
}
