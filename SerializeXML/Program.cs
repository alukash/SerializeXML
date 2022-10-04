using System;
using System.Collections.Generic;

namespace Epam.Upskill.SerializeXML
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Engine engine;
			Transmission transmission;
			Vehicles vehicles = new Vehicles();

			//add vehicle - car
			engine = new Engine(150, 1500);
			transmission = new Transmission(Transmission.Types.Automatic, 0);
			vehicles.AddVehicle(new Vehicle(Vehicle.Types.Car, engine, transmission));

			//add vehicle - bus
			engine = new Engine(250, 2500);
			transmission = new Transmission(Transmission.Types.Manual, 5);
			vehicles.AddVehicle(new Vehicle(Vehicle.Types.Bus, engine, transmission));

			//add vehicle lorry
			engine = new Engine(280, 3000);
			transmission = new Transmission(Transmission.Types.Manual, 5);
			vehicles.AddVehicle(new Vehicle(Vehicle.Types.Lorry, engine, transmission));

			List<Vehicle> vehiclesList = vehicles._vehiclesList;

			//serialize full list of vehicles to xml file
			vehicles.SerializeXml("vehicles1.xml");

			//serialize vehicles which volume > 1500 to xml file
			vehicles.SerializeXmlWrere("vehicles2.xml");

			//serialize only engine info for bus/lorry
			vehicles._vehiclesList = vehiclesList;
			vehicles.CreateXmlWhere("vehicles3.xml");

			//serialize vehicles grouped by transmission
			vehicles._vehiclesList = vehiclesList;
			vehicles.CreateXmlGroup("vehicles4.xml");

			Console.WriteLine();
			Console.Write("Press any key to close the app...");
			Console.ReadKey();
		}
	}
}
