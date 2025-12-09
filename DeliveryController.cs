public class DeliveryController
{
    private static int attempts = 3;

    public static void Delivery(Courier courier, Package p, Customer customer)
    {
        Console.WriteLine($"===Package {p.ID}===");

        p.DeliveryAttempts++;
        p.LastAttemptDate = DaySimulator.currDay;
        Console.WriteLine($"Delivery attempt: {p.DeliveryAttempts}");

        if (customer.IsAtHome)
        {
            courier.CollectedMoney += p.COD;
            p.COD = 0;
            p.Status = PackageStatus.Delivered;

            Console.WriteLine($"(DELIVERED!) Package{p.ID} status: {p.Status}");
            return;
        }

        if (p.DeliveryAttempts >= attempts)
        {
            p.Status = PackageStatus.PickUpWaiting;
            Console.WriteLine($"(WASN'T DELIVERED) Package{p.ID} status: {p.Status}");
            return;
        }
        else
        {
            p.Status = PackageStatus.Pending;
            p.PickUpDeadline = DaySimulator.currDay.AddDays(7);
            Console.WriteLine($"Will try tommorow Package{p.ID} status: {p.Status}");
        }
    }

    public static void DistributePackages(List<Package> packages, List<Courier> couriers, List<Customer> customers)
    {
        foreach (var p in packages.Where(p => p.Status == PackageStatus.Pending))
        {
            var customer = customers.FirstOrDefault(c => c.Address == p.Address);

            if (customer == null)
            {
                Console.WriteLine($"There is no customer with address: {p.Address}. Package {p.ID} will be skipped");
                continue;
            }

            var courier = couriers.FirstOrDefault(c => c.CanTake(p));
            if (courier == null)
            {
                Console.WriteLine($"No courier who can take package {p.ID}");
                continue;
            }

            courier.AssignPackage(p);

            Delivery(courier, p, customer);
        }
    } 
}