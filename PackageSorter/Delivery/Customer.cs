namespace PackageSorter;

public class Customer
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public bool IsAtHome { get; set; }

    public Customer() {}

    public void SetHomeStatus(bool isAtHome)
    {
        IsAtHome = isAtHome;
    }
}