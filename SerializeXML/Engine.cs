using System;

namespace Epam.Upskill.SerializeXML
{
    public class Engine
    {
        public int Power;
        public int Volume;

        public Engine()
        {
        }

        public Engine(int power, int volume)
        {
            Power = power;
            Volume = volume;
        }

        internal void PringInfo()
        {
            Console.WriteLine($"    Power: {Power} hp");
            Console.WriteLine($"    Volume: {Volume} cc");
        }
    }
}
