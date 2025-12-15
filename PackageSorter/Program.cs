namespace PackageSorter{
    public class Program
    {
    private static string customersPath = "JSONData/customers.json";
    private static string packagesPath = "JSONData/packages.json";
    private static string couriersPath = "JSONData/couriers.json";
    static void Main()
    {

        LoadData simData = new LoadData(customersPath, packagesPath, couriersPath); 
        Console.WriteLine($"Loaded C: {simData.customers.Count}, P: {simData.packages.Count}, R: {simData.couriers.Count}");
        UserCLI.execute(simData);
    }

    // Test helpers: keep original test API compatibility
    public static void AddPackageFromConsole(List<Package> packages, string packagesPath)
    {
        UserCLI.AddPackageFromConsole(packages, packagesPath, skipConfirm: true);
    }

    public static void AddCustomerFromConsole(List<Customer> customers, string customersPath)
    {
        UserCLI.AddCustomerFromConsole(customers, customersPath, skipConfirm: true);
    }
    }
}