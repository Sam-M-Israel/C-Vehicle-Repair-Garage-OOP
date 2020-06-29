using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class CreateNewVehicleObject
    {
        /**
         * (1) Insert new vehicle
         */
        public Vehicle CreateVehicleObject(
            eVehicle i_TypeOfVehicle,
            string i_ModelName,
            string i_LicenseNumber,
            string i_OwnerPhoneNumber,
            string i_OwnerName,
            string i_WheelManufacturer,
            float i_CurrentAirPressure,
            float i_CurrentCapacity,
            List<object> i_ExtraProperties)
        {
            Vehicle newVehicle = null;

            try
            {
                if (i_TypeOfVehicle == eVehicle.FuelMotorcycle || i_TypeOfVehicle == eVehicle.ElectricMotorcycle)
                {
                    Motorcycle newMotorcycle = new Motorcycle(
                        i_TypeOfVehicle,
                        i_ModelName,
                        i_LicenseNumber,
                        i_OwnerPhoneNumber,
                        i_OwnerName,
                        i_WheelManufacturer,
                        i_CurrentAirPressure,
                        i_CurrentCapacity);
                    newMotorcycle.LicenseType = (eLicenseType) i_ExtraProperties[0];
                    newMotorcycle.EngineVolume = (int) i_ExtraProperties[1];
                    newVehicle = newMotorcycle;
                }

                if (i_TypeOfVehicle == eVehicle.FuelCar || i_TypeOfVehicle == eVehicle.ElectricCar)
                {
                    Car newCar = new Car(
                        i_TypeOfVehicle,
                        i_ModelName, 
                        i_LicenseNumber,
                        i_OwnerPhoneNumber,
                        i_OwnerName,
                        i_WheelManufacturer,
                        i_CurrentAirPressure,
                        i_CurrentCapacity);
                    newCar.CarColor = (eCarColor) i_ExtraProperties[0];
                    newCar.NumberOfCarDoors = (eCarDoors) i_ExtraProperties[1];   
                    newVehicle = newCar;
                }

                if (i_TypeOfVehicle == eVehicle.Truck)
                {
                    Truck newTruck = new Truck(
                        i_TypeOfVehicle,
                        i_ModelName,
                        i_LicenseNumber,
                        i_OwnerPhoneNumber,
                        i_OwnerName,
                        i_WheelManufacturer,
                        i_CurrentAirPressure,
                        i_CurrentCapacity);
                    newTruck.ContainsDangerousMaterials = (bool) i_ExtraProperties[0];
                    newTruck.VolumeOfCargo = (float) i_ExtraProperties[1];
                    
                    newVehicle = newTruck;
                }

                if (i_TypeOfVehicle == eVehicle.Generic)
                {
                    Generic newGeneric = new Generic(
                        i_TypeOfVehicle, 
                        i_ModelName, 
                        i_LicenseNumber, 
                        i_OwnerPhoneNumber, 
                        i_OwnerName, 
                        i_WheelManufacturer, 
                        i_CurrentAirPressure, 
                        i_CurrentCapacity,
                       (float)i_ExtraProperties[0],
                        (int)i_ExtraProperties[1],
                        (float)i_ExtraProperties[2],
                        (eFuelType)i_ExtraProperties[3]);
                    newVehicle = newGeneric;
                }
                
                // float i_CurrentCapacity,
                // float i_MaxAirPressure,
                // int i_NumberOfWheels,
                // float i_MaxCapacity,
                //     eFuelType i_FuelType)
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return newVehicle;
        }
    }
}