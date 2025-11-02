namespace PKPZlab7._2
{
    public enum Plane
    {
       Cargo,
       Jet,
       Commercial,
       Propeller
    }
    public struct AEROFLOT
    {
        public string CITY { get; init; }
        public int NUM { get; init; }
        public Plane TYPE { get; init; }
        public override string ToString()
        {
            return $"Flight #{NUM} | Destination: {CITY} | Aircraft type: {TYPE}";
        }
        public AEROFLOT(string city, int num, Plane type)
        {
            CITY = city;
            NUM = num;
            TYPE = type;
        }
    }
}