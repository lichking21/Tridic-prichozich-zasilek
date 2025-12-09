public class DaySimulator
{
    public static DateTime currDay = DateTime.Today;

    public static void NextDay(List<Package> packages, Customer customer)
    {
        Random rnd = new Random();
        customer.SetHomeStatus(rnd.Next(2) == 0);
        
        currDay = currDay.AddDays(1);

        foreach(var p in packages.Where(p => p.Status == PackageStatus.PickUpWaiting))
        {
            if (currDay > p.PickUpDeadline)
            {
                p.Status = PackageStatus.Returned;
                Console.WriteLine($"(RETURNED) Package{p.ID} status: {p.Status}");
            }
        }
    } 
}