namespace Ex03.GarageLogic
{
    public class Wheel
    {
        private readonly string r_ManufacturerName;
        private float m_CurrentAirPressure;
        private float m_RecommendedMaxAir;

        public Wheel(string i_ManufacturerName, float i_CurrentAirPressure, float i_RecommendedMaxAir)
        {
            r_ManufacturerName = i_ManufacturerName;
            m_CurrentAirPressure = i_CurrentAirPressure;
            m_RecommendedMaxAir = i_RecommendedMaxAir;
        }
        
        public float MaxAirPressure
        {
            get { return m_RecommendedMaxAir; }
            set { m_RecommendedMaxAir = value; }
        }

        public float CurrentAirPressure
        {
            get { return m_CurrentAirPressure; }
            set { m_CurrentAirPressure = value; }
        }

        public string ManufacturerName
        {
            get { return r_ManufacturerName; }
        }

        public void InflateWheel(float i_AmountToFillPressure)
        {
            CurrentAirPressure = (CurrentAirPressure + i_AmountToFillPressure <= MaxAirPressure) ? CurrentAirPressure + i_AmountToFillPressure : MaxAirPressure;
        }

        public float MissingAirPressure(Wheel w)
        {
            return w.m_RecommendedMaxAir - w.CurrentAirPressure;
        }
    }
}