using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace PackageSorter;

public class Courier
{
    public string Name { get; set; } = string.Empty;
    public List<string> Addresses { get; set; } = new();
    public int MaxWeight { get; set; }
    public int CollectedMoney { get; set; }
    public PackageSize MaxSize { get; set; }
    public List<Package> DailyPackages { get; set; } = new();
    public int DeliveredCount { get; set; }
    public int ReturnedCount { get; set; }

    [JsonIgnore] public int CurrentWeight = 0;

    public Courier() {}

    public bool CanTake(Package p)
    {
        bool canLift = (CurrentWeight + p.Weight) <= MaxWeight;
        bool canFit = p.Size <= MaxSize;
        // if Addresses list is empty, treat as wildcard (can deliver anywhere)
        bool correctAddress = Addresses.Count == 0 || Addresses.Contains(p.Address);

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