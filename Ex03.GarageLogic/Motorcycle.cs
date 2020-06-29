namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle 
    {
        private eLicenseType m_LicenseType;
        private int m_EngineVolume;
        
        public Motorcycle(
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
            AddWheels(i_CurrentAirPressure, 2, 30);
            
            if (i_TypeOfVehicle == eVehicle.ElectricMotorcycle)
            {
                CurrEngine = new ElectricEngine(1.2f);
                CurrEngine.CurrentCapacity = i_CurrentCapacity;
            }
            else
            {
                CurrEngine = new FuelEngine(7, eFuelType.Octane95);
                CurrEngine.CurrentCapacity = i_CurrentCapacity;
            }
        }
        
        public eLicenseType LicenseType
        {
            get { return m_LicenseType; }
            set { m_LicenseType = value; }
        }
        
        public int EngineVolume
        {
            get { return m_EngineVolume; }
            set { m_EngineVolume = value; }
        }
    }
}