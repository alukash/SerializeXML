using System;

namespace Epam.Upskill.SerializeXML
{
	public class Vehicle
	{
		public enum Types
		{
			Car, Bus, Lorry
		}

		public Types Type;
		public Engine Engine;
		public Transmission Transmission;

		public Vehicle()
		{
		}

		public Vehicle(Types type, Engine engine, Transmission transmission)
		{
			this.Type = type;
			this.Engine = engine;
			this.Transmission = transmission;
		}
	}
}