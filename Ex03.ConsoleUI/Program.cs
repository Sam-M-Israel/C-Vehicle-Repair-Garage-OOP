using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Garage newGarage = new Garage();
            GarageManager newGarageManager = new GarageManager(newGarage);
            newGarageManager.Service();
        }
    }
}