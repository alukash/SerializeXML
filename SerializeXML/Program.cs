using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Epam.Upskill.SerializeXML
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Engine engine;
			Transmission transmission;

			List<Vehicle> vehicles = new List<Vehicle>();

			//vehicle#1
			engine = new Engine(150, 1500);
			transmission = new Transmission(Transmission.Types.Automatic, 0);
			vehicles.Add(new Vehicle(Vehicle.Types.Car, engine, transmission));

			//vehicle#2
			engine = new Engine(250, 2500);
			transmission = new Transmission(Transmission.Types.Manual, 5);
			vehicles.Add(new Vehicle(Vehicle.Types.Bus, engine, transmission));

			//vehicle#3
			engine = new Engine(280, 3000);
			transmission = new Transmission(Transmission.Types.Manual, 5);
			vehicles.Add(new Vehicle(Vehicle.Types.Lorry, engine, transmission));

			//serialize full list of vehicles to xml file
			SerializeXML("vehicles1.xml", vehicles);

			//serialize vehicles which volume > 1500 to xml file
			var vehicles2 = vehicles.Where(v => v.Engine.Volume > 1500);
			SerializeXML("vehicles2.xml", vehicles2.ToList());

			//serialize only engine info for bus/lorry
			CreateXml("vehicles3.xml", vehicles);

			//serialize vehicles grouped by transmission
			CreateXml2("vehicles4.xml", vehicles);

			Console.WriteLine();
			Console.Write("Press any key to close the app...");
			Console.ReadKey();

		}

		private static void SerializeXML(string file, List<Vehicle> vehicles)
		{
			var xmlOverrides = new XmlAttributeOverrides();
			var xmlAttribs = new XmlAttributes();
			xmlAttribs.XmlType = new XmlTypeAttribute("attr");
			xmlOverrides.Add(typeof(Transmission.Types), xmlAttribs);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vehicle>), xmlOverrides);
			FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
			xmlSerializer.Serialize(fs, vehicles);
			fs.Close();
		}
		private static void CreateXml(string file, List<Vehicle> vehicles)
		{
			var vehiclesTemp = from v in vehicles
							   where (v.Type.Equals(Vehicle.Types.Bus) || v.Type.Equals(Vehicle.Types.Lorry))
							   select new
							   {
								   type = v.Type,
								   power = v.Engine.Power,
								   volume = v.Engine.Volume
							   };

			FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
			XDocument xdoc = new XDocument();
			XElement vehiclesElem = new XElement("vehicles");
			vehiclesTemp.ToList().ForEach(v =>
			{
				XElement vehicleElem = new XElement("vehicle");
				XElement typeElem = new XElement("type", v.type);
				XElement powerElem = new XElement("power", v.power);
				XElement volumeElem = new XElement("volume", v.volume);
				vehicleElem.Add(typeElem);
				vehicleElem.Add(powerElem);
				vehicleElem.Add(volumeElem);
				vehiclesElem.Add(vehicleElem);
			});
			xdoc.Add(vehiclesElem);
			xdoc.Save(fs);
			fs.Close();
		}

		private static void CreateXml2(string file, List<Vehicle> vehicles)
		{
			var vehiclesTemp = vehicles.GroupBy(v => v.Transmission.Type);

			XElement transmissionElem = new XElement("transmission");
			vehiclesTemp.ToList().ForEach(v =>
			{
				XElement transmissionTypeElem = new XElement(v.Key.ToString());
				v.ToList().ForEach(t =>
				{
					XElement vehicleElem = new XElement("vehicle");
					XElement typeElem = new XElement("type", t.Type);
					XElement powerElem = new XElement("power", t.Engine.Power);
					XElement volumeElem = new XElement("volume", t.Engine.Volume);
					XElement gearsElem = new XElement("gears", t.Transmission.GearsNumber);
					vehicleElem.Add(typeElem);
					vehicleElem.Add(powerElem);
					vehicleElem.Add(volumeElem);
					vehicleElem.Add(gearsElem);
					transmissionTypeElem.Add(vehicleElem);
				});
				transmissionElem.Add(transmissionTypeElem);
			});
			XDocument xdoc = new XDocument();
			xdoc.Add(transmissionElem);
			FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
			xdoc.Save(fs);
			fs.Close();
		}
	}
}
