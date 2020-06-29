namespace Ex03.GarageLogic
{
    public class Generic : Vehicle
    {
        private eFuelType m_FuelType;
        
        public Generic(
            eVehicle i_TypeOfVehicle,
            string i_ModelName, 
            string i_LicenseNumber,
            string i_OwnerPhoneNumber, 
            string i_OwnerName,
            string i_WheelManufacturer,
            float i_CurrentAirPressure,
            float i_CurrentCapacity,
            float i_MaxAirPressure,
            int i_NumberOfWheels,
            float i_MaxCapacity,
            eFuelType i_FuelType)
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
            FuelType = i_FuelType;

            AddWheels(i_CurrentAirPressure, i_NumberOfWheels, i_MaxAirPressure);
            if (i_FuelType == eFuelType.Electric)
            {
                CurrEngine = new ElectricEngine(i_MaxCapacity);
            }
            else
            {
                CurrEngine = new FuelEngine(i_MaxCapacity, m_FuelType);
            }
            
            CurrEngine.CurrentCapacity = i_CurrentCapacity;
            CurrEngine.MaxCapacity = i_MaxCapacity;
        }

        public eFuelType FuelType
        {
            get { return m_FuelType; }
            set { m_FuelType = value; }
        }
    }
}