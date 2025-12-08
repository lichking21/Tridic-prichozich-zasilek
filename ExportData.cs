public class ExportData
{
    private static string deliveredPath = "ExportedData/delivered.csv";
    private static string returnedPath = "ExportedData/returned.csv";
    private static string couriersInfoPath = "ExportedData/couriersInfo.csv";

    public static void ExportDelivered(List<Package> packages)
    {
        var sw = new StreamWriter(deliveredPath);

        sw.WriteLine("ID;Address;Customer;Status;COD;DeliveredTime");

        foreach (var p in packages.Where(p => p.Status == PackageStatus.Delivered))
        {
            sw.WriteLine($"{p.ID};{p.Address};{p.CustomerName};{p.Status};{p.COD};{DaySimulator.currDay}");
        }

        sw.Close();
    }

    public static void ExportReturned(List<Package> packages)
    {
        var sw = new StreamWriter(returnedPath);

        sw.WriteLine("ID;Address;Customer;Status;COD");

        foreach (var p in packages.Where(p => p.Status == PackageStatus.Returned))
        {
            sw.WriteLine($"{p.ID};{p.Address};{p.CustomerName};{p.Status};{p.COD}");
        }
        
        sw.Close();
    }
    
    public static void ExportCourierInfo(List<Courier> couriers)
    {
        var sw = new StreamWriter(couriersInfoPath);

        sw.WriteLine("Courier;Delivered;Returned;CollectedMoney");

        foreach (var c in couriers)
        {
            int delivered = c.DailyPackages.Count(p => p.Status == PackageStatus.Delivered);
            int returned = c.DailyPackages.Count(p => p.Status == PackageStatus.Returned);

            sw.WriteLine($"{c.Name};{delivered};{returned};{c.CollectedMoney}");
        }

        sw.Close();
    }
}