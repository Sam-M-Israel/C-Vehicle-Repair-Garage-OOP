namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private float m_VolumeOfCargo;
        private bool m_ContainsDangerousMaterials;
        
        public Truck(
            eVehicle i_TypeOfVehicle,
            string i_Model_Name,
            string i_LicenseNumber,
            string i_OwnerPhoneNumber,
            string i_OwnerName,
            string i_WheelManufacturer,
            float i_CurrentAirPressure,
            float i_CurrentCapacity)
            : base(
                i_TypeOfVehicle,
                i_Model_Name,
                i_LicenseNumber,
                i_OwnerPhoneNumber,
                i_OwnerName,
                i_WheelManufacturer,
                i_CurrentAirPressure,
                i_CurrentCapacity)
        {
            AddWheels(i_CurrentAirPressure, 16, 28);
            CurrEngine = new FuelEngine(120, eFuelType.Solar);
            CurrEngine.CurrentCapacity = i_CurrentCapacity;
        }
        
        public float VolumeOfCargo
        {
            get { return m_VolumeOfCargo; }
            set { m_VolumeOfCargo = value; }
        }
        
        public bool ContainsDangerousMaterials
        {
            get { return m_ContainsDangerousMaterials; }
            set { m_ContainsDangerousMaterials = value; }
        }
    }
}