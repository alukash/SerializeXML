using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Epam.Upskill.SerializeXML
{
	internal class Vehicles
	{
		internal Vehicles()
		{
			_vehiclesList = new List<Vehicle>();
		}

		internal List<Vehicle> _vehiclesList;

		//Add new vehicle to the list
		internal void AddVehicle(Vehicle vehicle)
		{
			_vehiclesList.Add(vehicle);
		}

		//Serialize full list of autos to xml file
		internal void SerializeXml(string file)
		{
			var xmlOverrides = new XmlAttributeOverrides();
			var xmlAttribs = new XmlAttributes();
			xmlAttribs.XmlType = new XmlTypeAttribute("attr");
			xmlOverrides.Add(typeof(Transmission.Types), xmlAttribs);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vehicle>), xmlOverrides);
			FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
			xmlSerializer.Serialize(fs, _vehiclesList);
			fs.Close();
		}

		//Serialize autos which volume > 1500 to xml file
		internal void SerializeXmlWrere(string file)
		{
			var vehicles2 = _vehiclesList.Where(v => v.Engine.Volume > 1500);
			_vehiclesList = vehicles2.ToList();
			SerializeXml(file);
		}

		//Serialize only engine info for Bus/lorry
		internal void CreateXmlWhere(string file)
		{
			var vehiclesTemp = from v in _vehiclesList
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

		//Serialize autos grouped by transmission
		internal void CreateXmlGroup(string file)
		{
			var vehiclesTemp = _vehiclesList.GroupBy(v => v.Transmission.Type);

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
