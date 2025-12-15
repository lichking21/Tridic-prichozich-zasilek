using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;

namespace PackageSorter;

public class LoadData
{
    public List<Customer> customers;
    public List<Package> packages;
    public List<Courier> couriers;

    public LoadData(string customersPath, string packagesPath, string couriersPath)
    {
        customers = LoadCustomers(customersPath);
        packages = LoadPackages(packagesPath);
        couriers = LoadCouriers(couriersPath);
    }
    public static List<Customer> LoadCustomers(string customersPath)
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

    public static List<Package> LoadPackages(string packagesPath)
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

    public static List<Courier> LoadCouriers(string couriersPath)
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