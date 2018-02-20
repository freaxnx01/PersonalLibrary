using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Diagnostics;

using NUnit.Framework;

using PersonalLibrary.Misc;
using PersonalLibrary.Data;

namespace Data.Test
{
    [TestFixture]
    public class CommaSeparatedValuesTest
    {
        [Test]
        public void ConvertDataTableToCsvTest()
        {
            // Arrange
            List<Process> list = Process.GetProcesses().ToList();
            DataTable dt = CollectionHelper.ConvertToDataTable(list);

            // Act
            string result = CommaSeparatedValues.ConvertDataTableToCsv(dt, ';', true);

            // Assert
        }
    }
}
