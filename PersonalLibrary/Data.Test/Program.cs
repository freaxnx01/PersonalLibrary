using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

using PersonalLibrary.Data;
using PersonalLibrary.Misc;

namespace Data.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> list = Process.GetProcesses().ToList();
            DataTable dt = CollectionHelper.ConvertToDataTable(list);
            string result = CommaSeparatedValues.ConvertDataTableToCsv(dt, ';', true);

            DataSet ds = Database.ExecuteQuery("System.Data.OleDb",
                @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\ima\Desktop\Ressourcenplanung\Ressourcenplanung.mdb",
                "SELECT * FROM tPlan WHERE Ausfuhrender = 'IMA' ORDER BY Status");
        }
    }
}
