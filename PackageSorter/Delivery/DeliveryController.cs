using System;
using System.Collections.Generic;
using System.Linq;

namespace PackageSorter;

public static class DeliveryController
{
    private static int attempts = 3;

    public static void Delivery(Courier courier, Package p, Customer? customer)
    {
        Console.WriteLine($"===Package {p.ID}===");

        p.DeliveryAttempts++;
        p.LastAttemptDate = DaySimulator.currDay;
        Console.WriteLine($"Delivery attempt: {p.DeliveryAttempts}");

        bool isAtHome = customer?.IsAtHome ?? true;
        if (isAtHome)
        {
            courier.CollectedMoney += p.COD;
            courier.DeliveredCount++;
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
        p.Status = PackageStatus.Pending;
        p.PickUpDeadline = DaySimulator.currDay.AddDays(1);
        Console.WriteLine($"Will try tommorow Package{p.ID} status: {p.Status}");
    }

    public static void DistributePackages(List<Package> packages, List<Courier> couriers, List<Customer> customers)
    {
        foreach (var p in packages.Where(p => p.Status == PackageStatus.Pending))
        {
            var customer = customers.FirstOrDefault(c => c.Address == p.Address);

            // don't return package just because customer record is missing; allow courier assignment

            var courier = couriers.FirstOrDefault(c => c.CanTake(p));
            if (courier == null)
            {
                Console.WriteLine($"No courier who can take package {p.ID}. Keeping package pending");
                // leave package as Pending (tests expect it to remain Pending if no courier capacity/address)
                continue;
            }

            courier.AssignPackage(p);

            Delivery(courier, p, customer);
        }
    } 
}