using System;
using System.Collections.Generic;
using System.Linq;

namespace PackageSorter;

public class DaySimulator
{
        public static DateTime currDay = DateTime.Today;

        public static void PrepNextDay(List<Package> packages, List<Customer> customers, List<Courier> couriers)
        {
            // single Random instance for the day
            Random rnd = new Random();

            foreach(var customer in customers)
            {
                customer.SetHomeStatus(rnd.Next(2) == 0);
            }

            currDay = currDay.AddDays(1);

            foreach(var p in packages.Where(p => p.Status == PackageStatus.PickUpWaiting))
            {
                // try random self-pickup first
                var customer = customers.FirstOrDefault(c => c.Address == p.Address);
                bool customerCame = false;
                if (customer != null)
                {
                    customerCame = rnd.Next(2) == 0;
                }

                if (customerCame)
                {
                    p.Status = PackageStatus.Delivered;
                    p.LastAttemptDate = currDay;
                    // credit the courier who had this package
                    var courier = couriers.FirstOrDefault(c => c.DailyPackages.Contains(p));
                    if (courier != null)
                    {
                        courier.CollectedMoney += p.COD;
                        courier.DeliveredCount++;
                    }
                    Console.WriteLine($"(PICKUP) Package{p.ID} was picked up by customer. Status: {p.Status}");
                    continue;
                }

                // if nobody picked up, check deadline for return
                if (currDay.Date >= p.PickUpDeadline.Date)
                {
                    p.Status = PackageStatus.Returned;
                    // find the courier who had this package today and increment their returned count
                    var courier = couriers.FirstOrDefault(c => c.DailyPackages.Contains(p));
                    if (courier != null)
                    {
                        courier.ReturnedCount++;
                    }
                    Console.WriteLine($"(RETURNED) Package{p.ID} status: {p.Status} (currDay={currDay.Date}, deadline={p.PickUpDeadline.Date})");
                }
            }

        } 
    }