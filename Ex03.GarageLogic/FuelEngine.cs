namespace Ex03.GarageLogic
{
    public class FuelEngine : Engine
    {
        private eFuelType m_FuelType;
        
        public FuelEngine(float i_MaxCapacity, eFuelType i_FuelType)
        {
            m_FuelType = i_FuelType;
            MaxCapacity = i_MaxCapacity;
        }
        
        public float Refuel(float i_AmountToFill, out float io_IdealToRefuel)
        {
            float newCapacity;
            
            if (CurrentCapacity + i_AmountToFill <= MaxCapacity)
            {
                CurrentCapacity += i_AmountToFill;
                newCapacity = CurrentCapacity;
                io_IdealToRefuel = 0;
            }
            else
            {
                io_IdealToRefuel = MaxCapacity - CurrentCapacity;
                newCapacity = -1;
            }

            return newCapacity;
        }

        public override float CurrentCapacity
        {
            get { return m_CurrentCapacity; }
            set { m_CurrentCapacity = value; }
        }
        
        public override float MaxCapacity
        {
            get { return m_MaxCapacity; }
            set { m_MaxCapacity = value; }
        }

        public override object FuelType
        {
            get { return m_FuelType; }
        }
    }
}
