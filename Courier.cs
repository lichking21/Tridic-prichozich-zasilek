using System.Text.Json.Serialization;

public class Courier
{
    public string Name { get; set; } = string.Empty;
    public List<string> Addresses { get; set; } = new();
    public int MaxWeight { get; set; }
    public int CollectedMoney { get; set; }
    public PackageSize MaxSize { get; set; }
    public List<Package> DailyPackages { get; set; } = new();

    [JsonIgnore] private int CurrentWeight = 0;

    // public Courier(string name, List<string> addresses, int maxWeight, PackageSize maxSize)
    // {
    //     Name = name;
    //     Addresses = addresses;
    //     MaxWeight = maxWeight;
    //     MaxSize = maxSize;
    // }

    // For json
    public Courier() {}

    public bool CanTake(Package p)
    {
        bool canLift = (CurrentWeight + p.Weight) <= MaxWeight;
        bool canFit = p.Size <= MaxSize;
        bool correctAddress = Addresses.Contains(p.Address);

        return correctAddress && canLift && canFit;
    }

    public void AssignPackage(Package p)
    {
        DailyPackages.Add(p);
        CurrentWeight += p.Weight;
    }

    public void ResetDay()
    {
        DailyPackages.Clear();
        CurrentWeight = 0;
    }
}