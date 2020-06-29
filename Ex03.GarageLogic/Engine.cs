namespace Ex03.GarageLogic
{
    public class Engine
    {
        protected float m_CurrentCapacity;
        protected float m_MaxCapacity;
        
        public virtual float CurrentCapacity
        {
            get;
            set;
        }

        public virtual float MaxCapacity
        {
            get;
            set;
        }
        
        public virtual object FuelType
        {
            get;
            set;
        }
    }
}