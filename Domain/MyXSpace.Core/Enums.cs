using System.ComponentModel;

namespace Core.Enums
{
    public enum ClientType
    {
        None = 0,
        Finance = 1,
        Operations= 2,
    }
    /// <summary>
    /// Candidate type
    /// </summary>
    public enum WorkerStatus
    {
        None = 0,
        TW = 1,
        PU = 2,
        Contractor = 3
    }

    public static class BrandName
    {
        //public const string DefaultXBrand = "MyX";

        public const string MyAdsearch = "MyAdsearch";
        public const string MySigmar = "MySigmar";
        public const string Adequat = "Adequat";
        //public const string Connect = "Connect";
        //public const string Totem = "Totem";
    }
}
