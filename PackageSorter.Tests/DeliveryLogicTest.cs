using Xunit;
using System.Collections.Generic;
using PackageSorter;

namespace PackageSorter.Tests
{
    public class DeliveryLogicTests
    {
        [Fact]
        public void DistributePackages_AssignsPendingPackageToCourier()
        {
            var packages = new List<Package>
            {
                new Package { ID = 1, Address = "Test St", Status = PackageStatus.Pending, Weight = 5 }
            };
            
            var couriers = new List<Courier>
            {
                new Courier { Name = "Dave", MaxWeight = 50, CurrentWeight = 0 } 
            };
            
            var customers = new List<Customer>();

            DeliveryController.DistributePackages(packages, couriers, customers);

            
            Assert.NotEqual(PackageStatus.Pending, packages[0].Status); 
            Assert.Contains(packages[0], couriers[0].DailyPackages);
        }

        [Fact]
        public void DistributePackages_NoCapacity_PackageRemainsPending()
        {
            var packages = new List<Package>
            {
                new Package { ID = 1, Weight = 100, Status = PackageStatus.Pending }
            };
            
            var couriers = new List<Courier>
            {
                new Courier { Name = "Tiny Dave", MaxWeight = 10 } 
            };

            DeliveryController.DistributePackages(packages, couriers, new List<Customer>());

            Assert.Equal(PackageStatus.Pending, packages[0].Status);
        }
    }
}