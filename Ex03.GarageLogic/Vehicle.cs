namespace Ex03.GarageLogic
{
    using System;
    using System.Collections.Generic;
    
    public class Vehicle
    {
        private readonly eVehicle r_TypeOfVehicle;
        private readonly string r_ModelName;
        private readonly string r_LicenseNumber;
        private readonly string r_OwnerName;
        private readonly string r_OwnerPhoneNumber;
        private readonly string r_WheelManufacturer;
        private readonly List<Wheel> r_WheelList = new List<Wheel>();
        private eVehicleStatus m_VehicleStatus;
        private Engine m_Engine;

        public Vehicle(
            eVehicle i_TypeOfVehicle,
            string i_ModelName,
            string i_LicenseNumber,
            string i_OwnerPhoneNumber,
            string i_OwnerName,
            string i_WheelManufacturer,
            float i_CurrentAirPressure,
            float i_CurrentCapacity)
        {
            r_TypeOfVehicle = i_TypeOfVehicle;
            r_ModelName = i_ModelName;
            r_OwnerPhoneNumber = i_OwnerPhoneNumber;
            r_LicenseNumber = i_LicenseNumber;
            r_OwnerName = i_OwnerName;
            m_VehicleStatus = eVehicleStatus.InRepair;
            r_WheelManufacturer = i_WheelManufacturer;
        }

        public List<Wheel> Wheels
        {
            get { return r_WheelList; }   
        }

        public Engine CurrEngine
        {
            get { return m_Engine; }
            set { m_Engine = value; }
        }

        public string LicenseNumber
        {
            get { return r_LicenseNumber; }
        }

        public eVehicleStatus VehicleStatus
        {
            get { return m_VehicleStatus; }
            set { m_VehicleStatus = value; }
        }

        public eVehicle VehicleType
        {
            get { return r_TypeOfVehicle; }
        }
         
        public string ModelName
        {
            get { return r_ModelName; }
        }

        public string OwnerName
        {
            get { return r_OwnerName; }
        }

        public string OwnerPhoneNumber
        {
            get { return r_OwnerPhoneNumber; }
        }

        protected void AddWheels(float i_CurrentAirPressure, int i_NumberOfWheels, float i_RecommendedAirPressure)
        {  
            try
            {
                for (int i = 0; i < i_NumberOfWheels; i++)
                {
                    Wheel newWheel = new Wheel(r_WheelManufacturer, i_CurrentAirPressure, i_RecommendedAirPressure);
                    r_WheelList.Add(newWheel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}