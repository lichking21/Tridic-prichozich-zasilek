namespace PackageSorter
{
    public class Package
    {
        public int ID { get; set; }
        public int Weight { get; set; }
        public string Address { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public PackageSize Size { get; set; }
        public PackageStatus Status { get; set; }
        public int COD { get; set; }
        public int DeliveryAttempts { get; set; }
        public DateTime LastAttemptDate { get; set; }
        public DateTime PickUpDeadline { get; set; }

        public Package() {}
    }
}