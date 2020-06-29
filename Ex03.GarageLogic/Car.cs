namespace Ex03.GarageLogic
{
    using System;
    using System.Collections.Generic;

    public class Car : Vehicle
    {
        protected eCarColor m_CarColor;
        protected eCarDoors m_NumberCarDoors;

        public Car(
            eVehicle i_TypeOfVehicle,
            string i_ModelName, 
            string i_LicenseNumber,
            string i_OwnerPhoneNumber, 
            string i_OwnerName,
            string i_WheelManufacturer,
            float i_CurrentAirPressure,
            float i_CurrentCapacity)
            : base(
                i_TypeOfVehicle, 
                i_ModelName, 
                i_LicenseNumber, 
                i_OwnerPhoneNumber, 
                i_OwnerName,
                i_WheelManufacturer,
                i_CurrentAirPressure,
                i_CurrentCapacity)
        {   
            AddWheels(i_CurrentAirPressure, 4, 32);
           
            if (i_TypeOfVehicle == eVehicle.ElectricCar)
            {
                CurrEngine = new ElectricEngine(2.1f);
                CurrEngine.CurrentCapacity = i_CurrentCapacity;
            }
            else
            {
                CurrEngine = new FuelEngine(60, eFuelType.Octane96);
                CurrEngine.CurrentCapacity = i_CurrentCapacity;
            }
        }
        
        public eCarColor CarColor
        {
            get { return m_CarColor; }
            set { m_CarColor = value; }
        }
       
        public eCarDoors NumberOfCarDoors
        {
            get { return m_NumberCarDoors; }
            set { m_NumberCarDoors = value; }
        }
    }
}