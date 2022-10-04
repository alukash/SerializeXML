using System;

namespace Epam.Upskill.SerializeXML
{
    public class Transmission
    {
        public enum Types
        {
            Automatic,
            Manual
        }

        public Types Type;
        public string GearsNumber;

        public Transmission()
        {
        }

        public Transmission(Types type, int gearsNumber)
        {
            this.Type = type;
            this.GearsNumber = type.Equals(Types.Automatic) ? "N/A" : gearsNumber.ToString(); ;
        }

        internal void PrintInfo()
        {
            Console.WriteLine($"    Transmission: {Type}");
            Console.WriteLine($"    Number Of Gears: {GearsNumber}");
        }
    }
}