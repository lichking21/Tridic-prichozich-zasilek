using System.Text.Json;
using System.Text.Json.Serialization;

public static class LoadData
{
    private static string customersPath = "JSONData/customers.json";
    private static string packagesPath = "JSONData/packages.json";
    private static string couriersPath = "JSONData/couriers.json";

    public static List<Customer> LoadCustomers()
    {
        string json = File.ReadAllText(customersPath);
        
        var options = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true, 
            Converters = { new JsonStringEnumConverter() }    
        };

        List<Customer>? customers = JsonSerializer.Deserialize<List<Customer>>(json, options);

        if (customers == null)
            customers = new List<Customer>();

        return customers;
    }

    public static List<Package> LoadPackages()
    {
        string json = File.ReadAllText(packagesPath);
        var options = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true, 
            Converters = { new JsonStringEnumConverter() }    
        };

        List<Package>? packages = JsonSerializer.Deserialize<List<Package>>(json, options);

        if (packages == null)
            packages = new List<Package>();

        return packages;
    }

    public static List<Courier> LoadCouriers()
    {
        string json = File.ReadAllText(couriersPath);
        var options = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true, 
            Converters = { new JsonStringEnumConverter() }    
        };

        List<Courier>? couriers = JsonSerializer.Deserialize<List<Courier>>(json, options);

        if (couriers == null)
            couriers = new List<Courier>();

        return couriers;
    }
}