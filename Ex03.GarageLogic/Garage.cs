namespace Ex03.GarageLogic
{
    using System;
    using System.Collections.Generic;
    
    public class Garage
    {
        private List<Vehicle> m_AllVehicles = new List<Vehicle>();

        /**
         * (1) Insert new vehicle
         */
        public void InsertVehicle(
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
            CreateNewVehicleObject newVehicleObject = new CreateNewVehicleObject();
            Vehicle newVehicle = newVehicleObject.CreateVehicleObject(
                i_TypeOfVehicle,
                i_ModelName,
                i_LicenseNumber,
                i_OwnerPhoneNumber,
                i_OwnerName,
                i_WheelManufacturer,
                i_CurrentAirPressure,
                i_CurrentCapacity,
                i_ExtraProperties);

            m_AllVehicles.Add(newVehicle);
        }

        /**
         * (2)
         * Runs through the list of all vehicles currently in garage, builds a list of all vehicles with the given
         * status (is given) and returns it.
         */
        public List<string> FilterVehicles(eVehicleStatus i_VehicleStatus, bool i_FilterIsOn)
        {
            List<string> licensePlateNumbers = new List<string>();
            
            foreach(var vehicle in m_AllVehicles)
            {
                if (vehicle.VehicleStatus == i_VehicleStatus && i_FilterIsOn)
                {
                    licensePlateNumbers.Add(vehicle.LicenseNumber);   
                }
                else if (!i_FilterIsOn)
                {
                    licensePlateNumbers.Add(vehicle.LicenseNumber);
                }
            }
            
            return licensePlateNumbers;
        }

        /**
         * (3) Change Vehicle Status
         */
        public void ChangeVehicleStatus(string i_LicenseNumber, eVehicleStatus i_VehicleStatus)
        {
            Vehicle currentVehicle = Find(i_LicenseNumber);
            
            if (currentVehicle != null)
            {
                currentVehicle.VehicleStatus = i_VehicleStatus;
            }
        }
        
        /**
         * (4) Inflate Tires
         */
        public void InflateTires(string i_LicenseNumber)
        {
            Vehicle currentVehicle = Find(i_LicenseNumber);
            
            if (currentVehicle == null)
            {
                return;
            }

            foreach (Wheel currWheel in currentVehicle.Wheels)
            {
                currWheel.InflateWheel(currWheel.MaxAirPressure - currWheel.CurrentAirPressure);
            }
        }
        
        /**
         * (5) Refuel a Fuel-Based Vehicle
         */
        public float RefuelVehicle(string i_LicenseNumber, eFuelType i_FuelType, float i_AmountToFill, out float io_NeededCapacity, out eFuelType io_ActualFuelType)
        {
            Vehicle currentVehicle = Find(i_LicenseNumber);
            float newCapacity;
            float io_IdealCapacity = 0;
            io_ActualFuelType = i_FuelType;
            
            if (currentVehicle.VehicleType == eVehicle.FuelMotorcycle || currentVehicle.VehicleType == eVehicle.FuelCar || currentVehicle.VehicleType == eVehicle.Truck)
            {
                if (currentVehicle.CurrEngine.FuelType.Equals(i_FuelType))
                {
                    newCapacity = (currentVehicle.CurrEngine as FuelEngine).Refuel(i_AmountToFill, out io_IdealCapacity);
                }
                else
                {
                    io_ActualFuelType = (eFuelType) currentVehicle.CurrEngine.FuelType;
                    newCapacity = -2;
                }
            }
            else if (currentVehicle.VehicleType == eVehicle.Generic)
            {
                Generic genericVehicle = (Generic) currentVehicle;
                if (genericVehicle.FuelType != eFuelType.Electric)
                {
                    if (genericVehicle.FuelType.Equals(i_FuelType))
                    {
                        newCapacity = (genericVehicle.CurrEngine as FuelEngine).Refuel(i_AmountToFill, out io_IdealCapacity);
                    }
                    else
                    {
                        io_ActualFuelType = genericVehicle.FuelType;
                        newCapacity = -2;
                    }
                }
                else
                {
                    newCapacity = -3;
                }
            }
            else
            {
                newCapacity = -3;
            }

            io_NeededCapacity = io_IdealCapacity;
            
            return newCapacity;
        }

        /**
         * (6) Charge an electric-based Car
         */
        public float ChargeVehicle(string i_LicenseNumber, float i_NumberOfMinToCharge, out float io_NeededCharge)
        {
            Vehicle newVehicle = Find(i_LicenseNumber);
            float newCapacity;
            float io_IdealToCharge = 0;
            
            if (newVehicle.VehicleType == eVehicle.ElectricCar || newVehicle.VehicleType == eVehicle.ElectricMotorcycle)
            {
                newCapacity = (newVehicle.CurrEngine as ElectricEngine).Recharge(i_NumberOfMinToCharge, out io_IdealToCharge);
            }
            else if (newVehicle.VehicleType == eVehicle.Generic)
            {
                Generic genericVehicle = (Generic) newVehicle;
                if (genericVehicle.FuelType == eFuelType.Electric)
                {
                    newCapacity = (genericVehicle.CurrEngine as ElectricEngine).Recharge(i_NumberOfMinToCharge, out io_IdealToCharge);
                }
                else
                {
                    newCapacity = -3;
                }
            }
            else
            {
                newCapacity = -3;
            }
            
            io_NeededCharge = io_IdealToCharge;

            return newCapacity;
        }
        
        /**
         * (7) Displays Vehicle information. Uses Find (from above)
         */
        public List<object> DisplayVehicleInfo(string i_LicenseNumber)
        {
            Vehicle newVehicle = Find(i_LicenseNumber);
            List<object> vehicleInfo = new List<object>();
            
            if (newVehicle != null)
            {
                vehicleInfo.Add(newVehicle.LicenseNumber);
                vehicleInfo.Add(newVehicle.OwnerName);
                vehicleInfo.Add(newVehicle.OwnerPhoneNumber);
                vehicleInfo.Add(newVehicle.VehicleType);
                vehicleInfo.Add(newVehicle.ModelName);
                vehicleInfo.Add(newVehicle.VehicleStatus);
                vehicleInfo.Add(newVehicle.Wheels[0].ManufacturerName);
                vehicleInfo.Add(newVehicle.Wheels[0].CurrentAirPressure);
                
                eVehicle newVehiclesType = newVehicle.VehicleType;

                switch (newVehiclesType)
                {
                    case eVehicle.ElectricMotorcycle:
                    case eVehicle.FuelMotorcycle:
                        Motorcycle motorcycle = (Motorcycle)newVehicle;
                        vehicleInfo.Add(motorcycle.LicenseType);
                        vehicleInfo.Add(motorcycle.EngineVolume);
                        vehicleInfo.Add(motorcycle.CurrEngine.CurrentCapacity);
                        if (newVehiclesType is eVehicle.FuelMotorcycle)
                        {
                            vehicleInfo.Add(motorcycle.CurrEngine.FuelType);
                        }

                        break;
                    case eVehicle.ElectricCar: 
                    case eVehicle.FuelCar:
                        Car car = (Car) newVehicle;
                        vehicleInfo.Add(car.CarColor);
                        vehicleInfo.Add(car.NumberOfCarDoors);
                        vehicleInfo.Add(car.CurrEngine.CurrentCapacity);

                        if (newVehicle.VehicleType is eVehicle.FuelCar)
                        {
                            vehicleInfo.Add(car.CurrEngine.FuelType);
                        }

                        break;
                    case eVehicle.Truck:
                        Truck truck = (Truck) newVehicle;
                        vehicleInfo.Add(truck.VolumeOfCargo);
                        vehicleInfo.Add(truck.ContainsDangerousMaterials);
                        vehicleInfo.Add(truck.CurrEngine.CurrentCapacity);
                        vehicleInfo.Add(truck.CurrEngine.FuelType);
                        break;
                    case eVehicle.Generic:
                        Generic genericVehicle = (Generic) newVehicle;
                        vehicleInfo.Add(genericVehicle.CurrEngine.CurrentCapacity);
                        vehicleInfo.Add(genericVehicle.FuelType);
                        break;
                }
            }

            return vehicleInfo;
        }
        
        /**
        * Returns True if the vehicle is already in the garage and False if not.
        */
        public bool IsInGarage(string i_LicenseNumber)
        {
            Vehicle newVehicle = Find(i_LicenseNumber);
            
            return newVehicle != null;
        }
        
        /**
         * Searches the garage for a vehicle by its license number. Returns that vehicle if it is found, else
         * it throws an exception.
         */
        private Vehicle Find(string i_LicenseNumber)
        {
            Vehicle vehicleToReturn = null;
            
            foreach (Vehicle vehicle in m_AllVehicles)
            {
                if (vehicle.LicenseNumber == i_LicenseNumber)
                {
                    vehicleToReturn = vehicle;
                    break;
                }
            }

            return vehicleToReturn;
        }
    }
}
