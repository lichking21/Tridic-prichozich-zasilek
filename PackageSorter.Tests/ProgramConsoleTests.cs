using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PackageSorter;

namespace PackageSorter.Tests
{
    public class ProgramConsoleTests
    {
        [Fact]
        public void AddPackageFromConsole_ValidInput_AddsPackageToList()
        {

            var simulatedInput = "Street 123\nJohn Pork\n5\nSmall\n100\n";
            var stringReader = new StringReader(simulatedInput);
            
            Console.SetIn(stringReader);

            Console.SetOut(new StringWriter());

            var packages = new List<Package>();
            var tempFile = Path.GetTempFileName(); 

            try 
            {
                UserCLI.AddPackageFromConsole(packages, tempFile);
            }
            finally
            {
                if(File.Exists(tempFile)) File.Delete(tempFile);
            }

            Assert.Single(packages); 
            var p = packages.First();
            
            Assert.Equal(1, p.ID);
            Assert.Equal("Street 123", p.Address);
            Assert.Equal("John Pork", p.CustomerName);
            Assert.Equal(5, p.Weight);
            Assert.Equal(PackageSize.Small, p.Size);
            Assert.Equal(100, p.COD);
            Assert.Equal(PackageStatus.Pending, p.Status);
        }

        [Fact]
        public void AddCustomerFromConsole_ValidInput_AddsCustomerToList()
        {
            // Arrange
            var simulatedInput = "Alice Smith\nBrno, Main St 1\n";
            Console.SetIn(new StringReader(simulatedInput));
            Console.SetOut(new StringWriter());

            var customers = new List<Customer>();
            var tempFile = Path.GetTempFileName();

            // Act
            try
            {
                UserCLI.AddCustomerFromConsole(customers, tempFile);
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }

            // Assert
            Assert.Single(customers);
            Assert.Equal("Alice Smith", customers[0].Name);
            Assert.Equal("Brno, Main St 1", customers[0].Address);
        }
    }
}