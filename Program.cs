class Program
{
    static void Main()
    {
        // List<Customer> customers = new List<Customer>
        // {
        //     new Customer("Kotok", "Dalbaebskaya"),
        //     new Customer("Hui", "Manasa"),
        //     new Customer("Charles", "DoBronx")
        // };

        // List<string> addresses1 = new List<string> { "Manasa", "DoBronx" };
        // List<string> addresses2 = new List<string> { "Dalbaebskaya" };

        // List<Courier> couriers = new List<Courier>
        // {
        //     new Courier("Dodik", addresses1, 25, PackageSize.Medium),
        //     new Courier("Eben", addresses2, 40, PackageSize.Big),
        // };

        // List<Package> packages1 = new List<Package>
        // {
        //     new Package(1, 10, "Dalbaebskaya", "Kotok", PackageSize.Small, PackageStatus.Pending, 10),
        //     new Package(2, 20, "Manasa", "Hui", PackageSize.Big, PackageStatus.Pending, 20),
        //     new Package(3, 4, "DoBronx", "Charles", PackageSize.Medium, PackageStatus.Pending, 11),
        // };

        // List<Package> packages2 = new List<Package>
        // {
        //     new Package(4, 15, "Dalbaebskaya", "Hui", PackageSize.Small, PackageStatus.Pending, 10),
        //     new Package(5, 3, "Manasa", "Hui", PackageSize.Medium, PackageStatus.Pending, 20),
        //     new Package(6, 1, "DoBronx", "Charles", PackageSize.Small, PackageStatus.Pending, 11),
        // };

        var customers = LoadData.LoadCustomers();
        var packages = LoadData.LoadPackages();
        var couriers = LoadData.LoadCouriers();
        Console.WriteLine($"Loaded C: {customers.Count}, P: {packages.Count}, R: {couriers.Count}");

        for (int i = 1; i < 5; i++)
        {
            Console.WriteLine($"****DAY {i}****");

            DeliveryController.DistributePackages(packages, couriers, customers);
            
            foreach(var customer in customers) 
                DaySimulator.NextDay(packages, customer);

            foreach (var c in couriers)
                c.ResetDay();
        }


        // ExportData.ExportDelivered(packages);

        // ExportData.ExportReturned(packages);

        // ExportData.ExportCourierInfo(couriers);
    }
}