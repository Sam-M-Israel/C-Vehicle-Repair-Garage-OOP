namespace Ex03.GarageLogic
{
    public class ElectricEngine : Engine
    {
        public ElectricEngine(float i_MaxCapacity)
        {
            m_MaxCapacity = i_MaxCapacity;
        }

        public float Recharge(float i_AmountToCharge, out float io_IdealToCharge)
        {
            float newCapacity;
            
            if (CurrentCapacity + i_AmountToCharge <= MaxCapacity)
            {
                CurrentCapacity += i_AmountToCharge;
                newCapacity = CurrentCapacity;
                io_IdealToCharge = 0;
            }
            else
            {
                io_IdealToCharge = MaxCapacity - CurrentCapacity;
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
    }
}