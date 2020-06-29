namespace Ex03.ConsoleUI
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Ex03.GarageLogic;
    
    public class GarageManager
    {
        private readonly Garage m_Garage;
        private bool m_IsRunning = true;
        
        public GarageManager(Garage i_Garage)
        {
            m_Garage = i_Garage;
        }

        public void Service()
        {
            while (m_IsRunning)
            {
                showMenu();
                eMenu userChoice = (eMenu)getCustomerChoice();
                
                switch (userChoice)
                {
                    case eMenu.InsertVehicle:
                        //// Insert Vehicle:
                        InsertVehicle();
                        break;
                    case eMenu.GetLicensePlateNumbers:
                        //// Display all license plate numbers in Garage:
                        DisplayLicensePlateNumbers();
                        break;
                    case eMenu.ChangeVehicleStatus:
                        //// Change Vehicle's Status:
                        ChangeVehicleStatus();
                        break;
                    case eMenu.InflateTires:
                        //// Inflates tires to Maximum:
                        inflateWheels();
                        break;
                    case eMenu.RefuelVehicle:
                        //// Refuels a fuel-based vehicle:
                        refuelVehicle();
                        break;
                    case eMenu.ChargeVehicle:
                        //// Charges an electric vehicle:
                        chargeVehicle();
                        break;
                    case eMenu.GetVehicleInfo:
                        //// Displays all vehicle Info:
                        DisplayVehicleInfo();
                        break;
                    default:
                        ////Exit Garage:
                        exit();
                        break;
                }
            }
        }
        
        /**
         * Displays the main menu
         */
        private void showMenu()
        {
            string menu = string.Format(@"Welcome to Sam & Idos' Garage! What would you like do?
Please select from our available options by choosing the relevant number.
(1) Insert vehicle for repair.
(2) Display list of License plate numbers for all vehicle presently in the show. You have to option to filter the results.
(3) Change your Vehicle's status.
(4) Inflate your vehicle tires.
(5) Refuel your vehicle.
(6) Recharge your vehicle's battery.
(7) Display Vehicle info.
If none of these options are relevant to you, press 0 to exit our garage.");
            
            Console.WriteLine(menu);
        }
        
        /**
         * Gets the users choice from the menu options
         */
        private int getCustomerChoice()
        {
            string customerChoice = Console.ReadLine();
            bool isValidResult = int.TryParse(customerChoice, out int number);
            
            while (!isValidResult || number > 7 || number < 0)
            {
                Console.Clear();
                if (!isValidResult)
                {
                    Console.WriteLine(new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number > 7 || number < 1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(0, 7));
                }
                
                Console.WriteLine("Please choose again from our garage menu.");
                showMenu();
                customerChoice = Console.ReadLine();
                isValidResult = int.TryParse(customerChoice, out number);
            }

            return number;
        }
        
        /**
         * (1)
         * Function that handles the process of inserting a vehicle into the garage
         * with help from the backend function InsertVehicle() in Garage Logic
         */
        private void InsertVehicle()
        {
            Console.Clear();
            string licensePlateNumber = getLicensePlateNumber(true);
            bool vehicleExists = m_Garage.IsInGarage(licensePlateNumber);

            if (!vehicleExists)
            {
                string customerName = getCustomerName();
                string customerPhoneNumber = getCustomerPhoneNumber();
                eVehicle typeOfVehicle = getVehicleType();
                string modelName = getModelName();
                string wheelManufacturer = getWheelManufacturer();
                List<object> extraProperties = getProperties(typeOfVehicle);
                float maxAirPressure = typeOfVehicle == eVehicle.Generic ? (float) extraProperties[0] : 0f;
                float maxEngineCapacity = typeOfVehicle == eVehicle.Generic ? (float) extraProperties[2] : 0f;
                float currentAirPressure = getCurrentAirPressure(typeOfVehicle, maxEngineCapacity);
                float currentVehicleCapacity = getCurrentVehicleCapacity(typeOfVehicle, maxAirPressure);
                m_Garage.InsertVehicle(
                    typeOfVehicle,
                    modelName,
                    licensePlateNumber,
                    customerPhoneNumber,
                    customerName,
                    wheelManufacturer,
                    currentAirPressure,
                    currentVehicleCapacity,
                    extraProperties);
                Console.Clear();
                Console.WriteLine("Successfully inserted your {0}!", typeOfVehicle);
            }
            else
            {
                m_Garage.ChangeVehicleStatus(licensePlateNumber, eVehicleStatus.InRepair);
                Console.Clear();
                Console.WriteLine("Your vehicle's status has been successfully changed!");
            }
            
            Thread.Sleep(2000);
            exit();
        }
        
        /**
         * Asks for a license plate number from the user, checks to see if it is valid (8 character sequence) or if
         * a car with the same license plate number already exists in the garage and returns the license plate
         * number all goes well.
         */
        private string getLicensePlateNumber(bool i_IsDoingInsert)
        {
            Console.WriteLine("Please enter your license plate number.");
            string licensePlateNumber = Console.ReadLine();

            while (licensePlateNumber.Length == 0)
            {
                Console.Clear();
                
                if (licensePlateNumber.Length == 0)
                {
                    Console.WriteLine("Please enter license plate number comprised of 8 characters.");
                }

                if(licensePlateNumber.Contains(" "))
                {
                    Console.WriteLine("Please enter a license plate number without any spaces.");
                }

                licensePlateNumber = Console.ReadLine();
            }
  
            //// If the license plate number already exists in our system, we offer to either continue on, re-enter the
            //// license plate number, or to quit the system.
            bool vehicleAlreadyExists = m_Garage.IsInGarage(licensePlateNumber);       
            
            if (vehicleAlreadyExists && i_IsDoingInsert)
            {
                Console.WriteLine("This vehicle already exists in our system. Is this the correct vehicle? [Y/n/Q to exit].");
                string option = Console.ReadLine();

                while (!option.Equals("Y") && !option.Equals("n") && !option.Equals("Q") )
                {
                    Console.WriteLine("Please choose either 'Y' or 'n'.");
                    option = Console.ReadLine();
                }

                switch (option)
                {
                    case "Y":
                        //// Customer mistake, just wants to access vehicle. Continue as normal
                        break;
                    case "n":
                        //// Reenter license plate number
                         licensePlateNumber = getLicensePlateNumber(true);
                        break;
                    case "Q":
                        //// Exit the Garage
                        m_IsRunning = false;
                        exit();
                        break;
                }
            } 
            else if (!vehicleAlreadyExists && !i_IsDoingInsert)
            {
                Console.WriteLine(
                    "A Vehicle with this license plate number doesn't exist in the garage...");
                Thread.Sleep(2000);
                exit();
            }
            
            Console.Clear();
            
            return licensePlateNumber;
        }
        //// METHODS FOR GETTING CUSTOMER INFO

        /**
         * Gets the Customer's Name
         */
        private string getCustomerName()
        {
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Please enter your name:");
            string customerName = Console.ReadLine();

            while (customerName.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("Please input an actual name. Just pressing 'enter' doesn't count!");
                customerName = Console.ReadLine();
            }
            
            return customerName;
        }
        
        /**
         * Gets the Customer's Phone Number
         */
        private string getCustomerPhoneNumber()
        {
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Please enter your phone number:");
            string customerPhoneNumber = Console.ReadLine();
            
            while (customerPhoneNumber.Length == 0)
            {
                Console.Clear();
                Console.WriteLine(new FormatException("Invalid Phone number, not entering a phone number doesn't qualify as a phone number."));
                customerPhoneNumber = Console.ReadLine();
            }
            
            return customerPhoneNumber;
        }
        
        //// END OF METHODS FOR GETTING CUSTOMER INFO
        
        //// METHODS FOR STANDARD VEHICLE PROPERTIES:
        
        /**
         * Getting the model name of the vehicle
         */
        private string getModelName()
        {
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Please enter the model name:");
            string modelName = Console.ReadLine();
            
            while (modelName.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("Please input an actual Model name. Just pressing 'enter' doesn't count!");
                modelName = Console.ReadLine();
            }
            
            return modelName;
        }
        
        /**
         * Gets the name of the wheel manufacturer
         */
        private string getWheelManufacturer()
        {
            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine("Please enter the wheel manufacturer:");
            string wheelManufacturer = Console.ReadLine();
            
            while (wheelManufacturer.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("Please input an actual Model name. Just pressing 'enter' doesn't count!");
                wheelManufacturer = Console.ReadLine();
            }
            
            return wheelManufacturer;
        }
        
        /**
         * Gets the current air pressure of the wheels
         */
        private float getCurrentAirPressure(eVehicle i_TypeOfVehicle, float i_OptionalMaxAirPressure)
        {
            Console.Clear();
            
            string typeOfVehicle = string.Empty;
            float maxAirPressure = 0;
            
            switch (i_TypeOfVehicle)
            {
                case eVehicle.Truck:
                    typeOfVehicle = "truck";
                    maxAirPressure = 28;
                    break;
                case eVehicle.ElectricCar: 
                case eVehicle.FuelCar:
                    typeOfVehicle = "car";
                    maxAirPressure = 32;
                    break;
                case eVehicle.ElectricMotorcycle:
                case eVehicle.FuelMotorcycle:
                    typeOfVehicle = "motorcycle";
                    maxAirPressure = 30;
                    break;
                case eVehicle.Generic:
                    typeOfVehicle = "generic vehicle";
                    maxAirPressure = i_OptionalMaxAirPressure;
                    break;
                default:
                    break;
            }
            
            Console.WriteLine("Please enter the current air pressure of your {0}'s wheels:", typeOfVehicle);
            string customerAirPressureInput = Console.ReadLine();
            bool isValidAirPressure = float.TryParse(customerAirPressureInput, out float currentAirPressure);
            
            while (!isValidAirPressure || currentAirPressure > maxAirPressure || currentAirPressure < 0) 
            {    
                Console.Clear();
                
                if (!isValidAirPressure)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number."));
                }
                else if (currentAirPressure > maxAirPressure || currentAirPressure < 0)
                {
                    Console.WriteLine(new ValueOutOfRangeException(0, maxAirPressure) + " Please re-enter the current PSI.");
                }

                customerAirPressureInput = Console.ReadLine();
                isValidAirPressure = float.TryParse(customerAirPressureInput, out currentAirPressure);
            }

            return currentAirPressure;
        }
        
        /**
         * Gets the vehicle Type 
         */
        private eVehicle getVehicleType()
        {
            //// FuelMotorcycle = 1,
            //// ElectricMotorcycle = 2,
            //// FuelCar = 3,
            ////ElectricCar = 4,
            //// Truck = 5
            //// Generic = 6
            
            Console.Clear();
            string vehicleTypesMenu = string.Format(@"Please choose your vehicle type:
(1) Fuel-based Motorcycle.
(2) Electric-based Motorcycle.
(3) Fuel-based Car.
(4) Electric-based Car.
(5) Truck.
(6) Other
If your vehicle type doesn't appear here, we are unable to service your vehicle. Please try another garage.");
            Console.WriteLine(vehicleTypesMenu);
            string customerVehicleChoice = Console.ReadLine();
            bool isValidVehicleChoice = int.TryParse(customerVehicleChoice, out int number);
            
            while (!isValidVehicleChoice || number > 6 || number < 1)
            {
                Console.Clear();
            
                if (!isValidVehicleChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number > 6 || number < 1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(1, 6));
                }
                
                Console.WriteLine(vehicleTypesMenu);
                customerVehicleChoice = Console.ReadLine();
                isValidVehicleChoice = int.TryParse(customerVehicleChoice, out number);
            }
            
            eVehicle vehicleChoice = (eVehicle) number;

            return vehicleChoice;
        }
        
        /**
         * Gets the current vehicle's fuel/battery capacity
         */
        private float getCurrentVehicleCapacity(eVehicle i_TypeOfVehicle, float i_OptionalMaxCapacity)
        {
            Console.Clear();
            string typeOfVehicle = string.Empty, energyType = string.Empty;
            float maxCapacity = 0;
            
            switch (i_TypeOfVehicle)
            {
                case eVehicle.Truck:
                    typeOfVehicle = "truck";
                    energyType = "gas";
                    maxCapacity = 120;
                    break;
                case eVehicle.ElectricCar:
                    typeOfVehicle = "car";
                    energyType = "battery";
                    maxCapacity = 2.1f;
                    break;
                case eVehicle.ElectricMotorcycle:
                    typeOfVehicle = "motorcycle";
                    energyType = "battery";
                    maxCapacity = 1.2f;
                    break;
                case eVehicle.FuelCar:
                    typeOfVehicle = "car";
                    energyType = "gas";
                    maxCapacity = 60f;
                    break;
                case eVehicle.FuelMotorcycle:
                    typeOfVehicle = "motorcycle";
                    energyType = "gas";
                    maxCapacity = 7f;
                    break;
                case eVehicle.Generic:
                    typeOfVehicle = "generic vehicle";
                    energyType = "energy";
                    maxCapacity = i_OptionalMaxCapacity;
                    break;
                default:
                    
                    break;
            }
            
            Console.WriteLine("Please enter the current {0} level of your {1}'s engine:", energyType, typeOfVehicle);
            string currentVehicleCapacity = Console.ReadLine();
            bool isValidCapacity = float.TryParse(currentVehicleCapacity, out float vehicleCapacity);
            
            while (!isValidCapacity || vehicleCapacity > maxCapacity || vehicleCapacity < 0)
            {
                Console.Clear();
                
                if (!isValidCapacity)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number."));
                }
                else if (vehicleCapacity > maxCapacity || vehicleCapacity < 0)
                {
                    Console.WriteLine(new ValueOutOfRangeException(0, maxCapacity));
                }
                
                currentVehicleCapacity = Console.ReadLine();
                isValidCapacity = float.TryParse(currentVehicleCapacity, out vehicleCapacity);
            }

            return vehicleCapacity;
        }
        
        //// END OF METHODS FOR STANDARD VEHICLE PROPERTIES:
        
        //// METHODS FOR SPECIFIC VEHICLE TYPES
        
        /**
         * 
         */
        private void getEngineType(out bool o_IsFuelEngine)
        {
            Console.Clear();

            string engineType = string.Format(
                @"Please choose your engine type:
(1) Fuel
(2) Electric");
            Console.WriteLine(engineType);
            string customerEngineChoice = Console.ReadLine();
            bool isValidEngineType = int.TryParse(customerEngineChoice, out int number);

            while (!isValidEngineType || number > 2 || number < 1)
            {
                Console.Clear();

                if (!isValidEngineType)
                {
                    Console.WriteLine(
                        new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number > 2 || number < 1)
                {
                    Console.WriteLine(
                        new ValueOutOfRangeException(1, 2) + "Please re-choose a valid engine type from our menu!");
                }

                Console.WriteLine(engineType);
                customerEngineChoice = Console.ReadLine();
                isValidEngineType = int.TryParse(customerEngineChoice, out number);
            }

            ////If it is a Fuel engine
            if (number == 1)
            {
                o_IsFuelEngine = true;
            }
            else
            {
                o_IsFuelEngine = false;
            }
        }
        
        private float getMaxCapacity()
        {
            Console.Clear();
            Console.WriteLine("Please enter the maximum capacity for your engine:");
            string customerInput = Console.ReadLine();
            bool isValidNumber = float.TryParse(customerInput, out float number);

            while (!isValidNumber || number < 0)
            {
                Console.Clear();

                if (!isValidNumber)
                {
                    Console.WriteLine(
                        new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number < 0)
                {
                    Console.WriteLine("A negative capacity is not a valid max capacity. Please re-enter the maximum capacity for your engine!");
                }
                
                customerInput = Console.ReadLine();
                isValidNumber = float.TryParse(customerInput, out number);
            }

            return number;
        }
        
        private float getMaxAirPressure()
        {
            Console.Clear();
            Console.WriteLine("Please enter the maximum recommended air pressure for your wheels:");
            string customerInput = Console.ReadLine();
            bool isValidNumber = float.TryParse(customerInput, out float number);

            while (!isValidNumber || number < 0)
            {
                Console.Clear();

                if (!isValidNumber)
                {
                    Console.WriteLine(
                        new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number < 0)
                {
                    Console.WriteLine("A negative PSI is not a valid PSI. Please re-enter the maximum recommended air pressure for your wheels!");
                }
                
                customerInput = Console.ReadLine();
                isValidNumber = float.TryParse(customerInput, out number);
            }

            return number;
        }
        
        private int getNumberOfWheels()
        {
            Console.Clear();
            Console.WriteLine("Please enter the number of wheels on your vehicle:");
            string customerInput = Console.ReadLine();
            bool isValidNumber = int.TryParse(customerInput, out int number);

            while (!isValidNumber || number < 0)
            {
                Console.Clear();

                if (!isValidNumber)
                {
                    Console.WriteLine(
                        new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number < 0)
                {
                    Console.WriteLine("A vehicle cannot have a negative number of wheels, please re-enter the number of wheels on your vehicle!");
                }
                
                customerInput = Console.ReadLine();
                isValidNumber = int.TryParse(customerInput, out number);
            }

            return number;
        }

        /**
         * Getting the specific properties per vehicle type
         */
        private List<object> getProperties(eVehicle i_TypeOfVehicle)
        {
            List<object> extraProperties = new List<object>();
            
            switch (i_TypeOfVehicle)
            {
                case eVehicle.Truck:
                    extraProperties.Add(isCargoMaterialDangerous());
                    extraProperties.Add(getTruckVolumeOfCargo());
                    break;
                case eVehicle.ElectricCar:
                case eVehicle.FuelCar:
                    extraProperties.Add(getCarColor());
                    extraProperties.Add(getNumberOfCarDoors());
                    
                    break;
                case eVehicle.ElectricMotorcycle: 
                case eVehicle.FuelMotorcycle:
                    extraProperties.Add(getMotorCycleLicenseType());
                    extraProperties.Add(getMotorcycleEngineVolume());
                    break;
                case eVehicle.Generic:
                    extraProperties.Add(getMaxAirPressure());
                    extraProperties.Add(getNumberOfWheels());
                    extraProperties.Add(getMaxCapacity());
                    // extraProperties.Add(getMaxAirPressure());
                    // extraProperties.Add(getNumberOfWheels());
                    getEngineType(out bool isFuelEngine);
                    eFuelType genericFuelType;
                    genericFuelType = isFuelEngine ? getFuelType() : eFuelType.Electric;
                    extraProperties.Add(genericFuelType);
                    break;
                default:
                    break;
            }
            
            return extraProperties;
        }

        /**
         * Get Motorcycle license type
         */
        private eLicenseType getMotorCycleLicenseType()
        {
            //// A = 1, A1 = 2, AA = 3 , B = 4
            
            Console.Clear();
            string motorcycleLicenseTypesMenu = string.Format(@"Please choose your motorcycle license type:
(1) A
(2) A1
(3) AA
(4) B");
            Console.WriteLine(motorcycleLicenseTypesMenu);
            string customerLicenseChoice = Console.ReadLine();
            bool isValidLicenseChoice = int.TryParse(customerLicenseChoice, out int number);
            
            while (!isValidLicenseChoice || number > 4 || number < 1)
            {
                Console.Clear();
                
                if (!isValidLicenseChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number > 4 || number < 1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(1, 4) + "Please re-choose a valid license type from our menu!");
                }
                
                Console.WriteLine(motorcycleLicenseTypesMenu);
                customerLicenseChoice = Console.ReadLine();
                isValidLicenseChoice = int.TryParse(customerLicenseChoice, out number);
            }
            
            eLicenseType licenseInput = (eLicenseType) number;

            return licenseInput;
        }
        
        /**
         * Gets the current engine volume of the motorcycle the customer is putting into the shop
         */
        private int getMotorcycleEngineVolume()
        {
            Console.Clear();
            Console.WriteLine("Please enter your current engine volume:");
            string customerEngineVolume = Console.ReadLine();
            bool isValidEngineVolume = int.TryParse(customerEngineVolume, out int engineVolume);

            //// It wasn't clear in the instructions what are the parameters of Engine Volume. We assumed that it had to be
            //// above 0.
            while (!isValidEngineVolume || engineVolume <= 0)
            {
                Console.Clear();
                
                if (!isValidEngineVolume)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number as your choice."));
                }
                else if (engineVolume <= 0)
                {
                    Console.WriteLine(new FormatException("Please enter an engine volume above 0 cc's."));
                }
                
                customerEngineVolume = Console.ReadLine();
                isValidEngineVolume = int.TryParse(customerEngineVolume, out engineVolume);
            }

            return engineVolume;
        }

        /**
         * Gets the car's color from the customer
         */
        private eCarColor getCarColor()
        {
            // Red = 1,
            // Blue = 2,
            // Black = 3,
            // Grey = 4
            Console.Clear();
            string carColorsMenu = string.Format(@"Please choose your car's color:
(1) Red
(2) Blue
(3) Black
(4) Grey");
            Console.WriteLine(carColorsMenu);
            string customerColorChoice = Console.ReadLine();
            bool isValidColorChoice = int.TryParse(customerColorChoice, out int number);
            
            while (!isValidColorChoice || number < 1 || number > 4)
            {
                Console.Clear();
                
                if (!isValidColorChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input, please only use a number as your choice."));
                }
                else if (number > 4 || number < 1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(1, 4));
                }
                
                Console.WriteLine(carColorsMenu);
                customerColorChoice = Console.ReadLine();
                isValidColorChoice = int.TryParse(customerColorChoice, out number);
            }
            
            eCarColor carColor = (eCarColor)number;
            
            return carColor;
        }

        /**
         * Gets the number of car doors from the customer
         */
        private eCarDoors getNumberOfCarDoors()
        {
            //// Number of doors – 2, 3, 4, or 5
            //// Two = 2,
            //// Three = 3,
            //// Four = 4,
            //// Five = 5
            Console.Clear();
            string carDoorsMenu = string.Format(@"Please choose the number of doors your car has:
(2) Two
(3) Three
(4) Four
(5) Five");
            Console.WriteLine(carDoorsMenu);
            string customerCarDoorChoice = Console.ReadLine();
            bool isValidDoorChoice = int.TryParse(customerCarDoorChoice, out int number);
            
            while (!isValidDoorChoice || number < 2 || number > 5)
            {
                Console.Clear();
                
                if (!isValidDoorChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input of car doors, please re-choose the number of doors your cas has!"));
                }
                else if (number > 5 || number < 2)
                {
                    Console.WriteLine(new ValueOutOfRangeException(2, 5));
                }
                
                Console.WriteLine(carDoorsMenu);
                customerCarDoorChoice = Console.ReadLine();
                isValidDoorChoice = int.TryParse(customerCarDoorChoice, out number);
            }
            
            eCarDoors numberCarDoors = (eCarDoors)number;
            
            return numberCarDoors;
        }

        /**
         * Gets the current engine volume of the motorcycle the customer is putting into the shop
         */
        private float getTruckVolumeOfCargo()
        {
            Console.Clear();
            Console.WriteLine("Please enter your truck's cargo volume:");
            string customerCargoVolumeInput = Console.ReadLine();
            bool isValidCargoInput = float.TryParse(customerCargoVolumeInput, out float cargoVolume);

            //// It wasn't clear in the instructions what are the parameters of cargo volume. We assumed that it had to be
            //// above 0 as you cannot have a negative cargo volume in terms of weight.
            while (!isValidCargoInput || cargoVolume <= 0)
            {
                Console.Clear();
                
                if (!isValidCargoInput)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number."));
                }
                else if (cargoVolume < 0)
                {
                    Console.WriteLine("Invalid cargo volume, please re-enter your truck's cargo volume. It cannot be a negative volume! ");
                }
                
                customerCargoVolumeInput = Console.ReadLine();
                isValidCargoInput = float.TryParse(customerCargoVolumeInput, out cargoVolume);
            }

            return cargoVolume;
        }
        
        /**
         * Asks whether the truck cargo is dangerous
         */
        private bool isCargoMaterialDangerous()
        {
            Console.Clear();
            string dangerousMaterialsMenu = string.Format(@"Is your truck's cargo dangerous?
(1) Yes
(2) No");
            Console.WriteLine(dangerousMaterialsMenu);
            string customerDangerMatsChoice = Console.ReadLine();
            bool isValidDangerMatsChoice = int.TryParse(customerDangerMatsChoice, out int number);

            while (!isValidDangerMatsChoice || (number != 1 && number != 2))
            {
                Console.Clear();
                
                if (!isValidDangerMatsChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input for dangerous materials, please re-choose a number from the menu."));
                }
                else if (number != 1 && number != 2)
                {
                    Console.WriteLine(new ValueOutOfRangeException(1, 2));
                }
                
                Console.WriteLine(dangerousMaterialsMenu);
                customerDangerMatsChoice = Console.ReadLine();
                isValidDangerMatsChoice = int.TryParse(customerDangerMatsChoice, out number);
            }

            return number == 1;
        }
        
        /**
         * (2)
         * Displaying all license plate numbers with/without a filter according ot the users choice.
         */
        private void DisplayLicensePlateNumbers()
        {
            Console.Clear();
            string filterOptions = string.Format(@"Would you like to filter the license plate options by vehicle status?
(0) No thank you.
(1) By In repair.
(2) By Repaired.
(3) By Payed for.");
            Console.WriteLine(filterOptions);
            string customerFilterChoice = Console.ReadLine();
            bool isValidChoice = int.TryParse(customerFilterChoice, out int number);
            bool filterChosen = false;
            
            while (!isValidChoice || number > 3 || number < 0)
            {
                Console.Clear();
                
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input for display filters, please re-choose a number from the menu."));
                }
                else if (number > 3 || number < 0)
                {
                    Console.WriteLine(new ValueOutOfRangeException(0, 3));
                }

                Console.WriteLine(filterOptions);
                customerFilterChoice = Console.ReadLine();
                isValidChoice = int.TryParse(customerFilterChoice, out number);
            }

            eVehicleStatus newVehicleStatus = eVehicleStatus.InRepair;
            
            if (number != 0)
            {
                filterChosen = true;
                newVehicleStatus = (eVehicleStatus) number;
            }
            
            Console.Clear();
            List<string> allLicenseNumbers = m_Garage.FilterVehicles(newVehicleStatus, filterChosen);
            
            if (allLicenseNumbers != null)
            {
                StringBuilder stringLicenseNumbers = new StringBuilder();
                string toInsertInWrite = ".";

                if (filterChosen)
                {
                    toInsertInWrite = " with the status: " + newVehicleStatus;
                }
                
                stringLicenseNumbers.AppendFormat("There are a total of {0} vehicles in the garage{1}{2}", allLicenseNumbers.Count, toInsertInWrite, Environment.NewLine);
                
                foreach (string licenseNumber in allLicenseNumbers)
                {
                    stringLicenseNumbers.AppendLine(licenseNumber);
                }
                
                Console.WriteLine(stringLicenseNumbers);
            }
            else if (filterChosen)
            {
                Console.WriteLine("There are no vehicles with the selected status currently in the garage.");
            }
            else 
            {
                Console.WriteLine("The garage is currently empty.");
            }
            
            Console.Write("Press enter to continue...");
            Console.ReadLine();
            exit();
        }
        
        /*
         * (3)
         * Changes a vehicle's status
         */
        private void ChangeVehicleStatus()
        {
            Console.Clear();
            string licensePlateNumber = getLicensePlateNumber(false);
            string statusOptions = string.Format(@"Would you like change your vehicles status? Please choose your vehicles new status.
(1) To In repair.
(2) To Repaired.
(3) To Payed for.");
            Console.WriteLine(statusOptions);
            string customerVehicleStatusChoice = Console.ReadLine();
            bool isValidChoice = int.TryParse(customerVehicleStatusChoice, out int number);
            
            while (!isValidChoice || number > 3 || number < 1)
            {
                Console.Clear();
                
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input for vehicle status, please re-choose a number from the menu."));
                }
                else if (number > 3 || number < 1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(1, 3));
                }
                
                Console.WriteLine(statusOptions);
                customerVehicleStatusChoice = Console.ReadLine();
                isValidChoice = int.TryParse(customerVehicleStatusChoice, out number);
            }

            eVehicleStatus newVehicleStatus = (eVehicleStatus)number;
            m_Garage.ChangeVehicleStatus(licensePlateNumber, newVehicleStatus);
            Console.WriteLine("Your vehicles status has been successfully changed.");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
            Thread.Sleep(1500);
            exit();
        }

        /**
         * (4) Inflate wheels
         */
        private void inflateWheels()
        {
            Console.Clear();
            string licensePlateNumber = getLicensePlateNumber(false);
            m_Garage.InflateTires(licensePlateNumber);
            Console.WriteLine("Successfully inflated vehicle tires.");
            Thread.Sleep(2000);
            exit();
        }

        private eFuelType getFuelType()
        {
            Console.Clear();
            string fuelTypeOptions = string.Format(@"Please choose your fuel type:
(1) Octane95
(2) Octane96
(3) Octane98
(4) Solar");
            Console.WriteLine(fuelTypeOptions);
            string customerFuelTypeChoice = Console.ReadLine();
            bool isValidChoice = float.TryParse(customerFuelTypeChoice, out float number);
            
            while (!isValidChoice || number > 4 || number < 1)
            {
                Console.Clear();
                
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Invalid menu input for fuel type, please re-choose from the fuel type menu!."));
                }
                else if (number > 4 || number < 1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(1, 4));
                }
                
                Console.WriteLine(fuelTypeOptions);
                customerFuelTypeChoice = Console.ReadLine();
                isValidChoice = float.TryParse(customerFuelTypeChoice, out number);
            }
            
            return (eFuelType)number;
        }
        
        /**
         * (5) Refueling a fuel based vehicle
         */
        private void refuelVehicle()
        {
            Console.Clear();
            string licensePlateNumber = getLicensePlateNumber(false);
            eFuelType fuelType = getFuelType();
            Console.WriteLine("Please enter the amount to refuel your vehicle:");
            string customerRefuelAmount = Console.ReadLine();
            bool isValidChoice = float.TryParse(customerRefuelAmount, out float number);

            while (!isValidChoice || number < 0)
            {
                Console.Clear();
                
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number to refuel your vehicle:"));
                }
                else if (number < 0)
                {
                    Console.WriteLine("Refueling requires a positive number. Please re-enter the amount to refuel!");
                }

                customerRefuelAmount = Console.ReadLine();
                isValidChoice = float.TryParse(customerRefuelAmount, out number);
            }

            float idealEngineCapacity;
            eFuelType actualFuelType;
            float afterRefueling = m_Garage.RefuelVehicle(licensePlateNumber, fuelType, number, out idealEngineCapacity, out actualFuelType);

            while (!isValidChoice || afterRefueling == -1 || number < 0)
            {
                Console.Clear();
                
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number to refuel your vehicle:"));
                }
                else if(afterRefueling == -1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(0, idealEngineCapacity));
                }
                else if (number < 0)
                {
                    Console.WriteLine("Refueling requires a positive number. Please re-enter the amount to refuel!");
                }
                
                customerRefuelAmount = Console.ReadLine();
                isValidChoice = float.TryParse(customerRefuelAmount, out number);
                afterRefueling = number >= 0 ? m_Garage.RefuelVehicle(licensePlateNumber, fuelType, number, out idealEngineCapacity, out actualFuelType) : afterRefueling;
            }

            if (afterRefueling < -1)
            {
                energyErrors((eEnergyError)afterRefueling, idealEngineCapacity, actualFuelType);

                if (afterRefueling == -2)
                {
                    refuelVehicle();
                }
            }
            else
            {
                Console.WriteLine("Refueling success, your current fuel level of {0} is: {1}", actualFuelType, afterRefueling);
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            
            Thread.Sleep(2000);
            exit();
        }
        
        /**
         * (6) Recharging an electric based vehicle
         */
        private void chargeVehicle()
        {
            Console.Clear();
            string licensePlateNumber = getLicensePlateNumber(false);
            Console.WriteLine("Please enter amount to charge your vehicle's battery:");
            
            string line = Console.ReadLine();
            bool isValidChoice = float.TryParse(line, out float number);

            while (!isValidChoice || number < 0)
            {
                Console.Clear();
                
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Invalid input, please re-enter a desired recharge amount."));
                }
                else 
                {
                    Console.WriteLine(new ValueOutOfRangeException(0, 5)); // 5 for some free space 
                }
                
                line = Console.ReadLine();
                isValidChoice = float.TryParse(line, out number);
            }
            
            float idealEngineCapacity;
            float afterCharging = m_Garage.ChargeVehicle(licensePlateNumber, number, out idealEngineCapacity);
            
            while (!isValidChoice || afterCharging == -1 || number < 0)
            {
                Console.Clear();
                
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number"));
                }
                else if(afterCharging == -1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(0, idealEngineCapacity));
                }
                else if (number < 0)
                {
                    Console.WriteLine("Recharging requires a positive number. Please re-enter the amount to recharge!");
                }
                
                line = Console.ReadLine();
                isValidChoice = float.TryParse(line, out number);
                afterCharging = m_Garage.ChargeVehicle(licensePlateNumber, number, out idealEngineCapacity);
            }
            
            if (afterCharging < -1)
            {
                energyErrors((eEnergyError) afterCharging, 0, eFuelType.Electric);
            }
            else
            {
                Console.WriteLine("Recharging success, your current battery level is: {0}", afterCharging);
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            
            Thread.Sleep(1500);
            exit();
        }

        private void energyErrors(eEnergyError i_EnergyError, float i_EngineCapacity, eFuelType i_ActualFuelType)
        {
            if (i_EnergyError == eEnergyError.TooMuchEnergy)
            {
                Console.WriteLine(new ValueOutOfRangeException(0, i_EngineCapacity));
            }

            if (i_EnergyError == eEnergyError.FuelTypeError)
            {
                Console.WriteLine(new ArgumentException(string.Format("The type of fuel you chose doesn't match that of your vehicle's: {0}", i_ActualFuelType)));
            }

            if (i_EnergyError == eEnergyError.EngineTypeError)
            {
                Console.WriteLine(new ArgumentException("Your vehicle's engine type doesn't match the operation you wish to execute."));
            }
        }
        
        /**
         * (7) Displays Vehicle information
         */
        private void DisplayVehicleInfo()
        {
            Console.Clear();
            string licensePlateNumber = getLicensePlateNumber(false);
            List<object> vehicleInfo = m_Garage.DisplayVehicleInfo(licensePlateNumber);

            if (vehicleInfo != null)
            {
                StringBuilder stringVehicleInfo = new StringBuilder();
                stringVehicleInfo.AppendFormat("License Number: {0}" + Environment.NewLine, vehicleInfo[0]);
                stringVehicleInfo.AppendFormat("Owner's Name: {0}" + Environment.NewLine, vehicleInfo[1]);
                stringVehicleInfo.AppendFormat("Owner's Phone Number: {0}" + Environment.NewLine, vehicleInfo[2]);
                stringVehicleInfo.AppendFormat("Vehicle Type: {0}" + Environment.NewLine, vehicleInfo[3]);
                stringVehicleInfo.AppendFormat("Model Name: {0}" + Environment.NewLine, vehicleInfo[4]);
                stringVehicleInfo.AppendFormat("{0} status: {1}" + Environment.NewLine, vehicleInfo[3], vehicleInfo[5]);
                stringVehicleInfo.AppendFormat("Wheel Manufacturer: {0}" + Environment.NewLine, vehicleInfo[6]);
                stringVehicleInfo.AppendFormat("Current Tire Pressure: {0}" + Environment.NewLine, vehicleInfo[7]);
                eVehicle vehicleType = (eVehicle) vehicleInfo[3];

                switch (vehicleType)
                {
                    case eVehicle.ElectricMotorcycle:
                    case eVehicle.FuelMotorcycle:
                        stringVehicleInfo.AppendFormat("Motorcycle License Type: {0}" + Environment.NewLine, vehicleInfo[8]);
                        stringVehicleInfo.AppendFormat("Motorcycle Engine Volume: {0} cc" + Environment.NewLine, vehicleInfo[9]);
                        break;
                    case eVehicle.ElectricCar:
                    case eVehicle.FuelCar:
                        stringVehicleInfo.AppendFormat("Car Color: {0}" + Environment.NewLine, (eCarColor) vehicleInfo[8]);
                        stringVehicleInfo.AppendFormat("Number of Car doors: {0}" + Environment.NewLine, (eCarDoors) vehicleInfo[9]);
                        break;
                    case eVehicle.Truck:
                        stringVehicleInfo.AppendFormat("Cargo volume: {0}" + Environment.NewLine, vehicleInfo[8]);
                        stringVehicleInfo.AppendFormat("Is cargo dangerous? {0}" + Environment.NewLine, vehicleInfo[9]);
                        break;
                }
                
                switch (vehicleType)
                {
                    case eVehicle.FuelMotorcycle: 
                    case eVehicle.FuelCar:
                    case eVehicle.Truck:
                        stringVehicleInfo.AppendFormat("Current Fuel Levels: {0}" + Environment.NewLine, vehicleInfo[10]);
                        stringVehicleInfo.AppendFormat("Fuel Type: {0}" + Environment.NewLine, (eFuelType) vehicleInfo[11]);
                        break;
                    case eVehicle.ElectricMotorcycle: 
                    case eVehicle.ElectricCar:
                        stringVehicleInfo.AppendFormat("Current Battery Status: {0}" + Environment.NewLine, vehicleInfo[10]);
                        break;
                }

                if (vehicleType == eVehicle.Generic)
                {
                    eFuelType fuelType = (eFuelType) vehicleInfo[9];
                    
                    if (fuelType != eFuelType.Electric)
                    {
                        stringVehicleInfo.AppendFormat("Current Fuel Levels: {0}" + Environment.NewLine, vehicleInfo[8]);
                        stringVehicleInfo.AppendFormat("Fuel Type: {0}" + Environment.NewLine, fuelType);
                    }
                    else
                    {
                        stringVehicleInfo.AppendFormat("Current Battery Status: {0}" + Environment.NewLine, vehicleInfo[8]);
                    }
                }

                Console.WriteLine(stringVehicleInfo);
                Console.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("There is no vehicle with such license plate number in the garage.");
            }

            Thread.Sleep(1000);
            exit();
        }
        
        /**
         * Exiting the garage
         */
        private void exit()
        {
            Console.Clear();
            string exitString = string.Format(@"Would you like to perform another operation?
(1) Yes.
(2) No thanks!");
            Console.WriteLine(exitString);
            string userExitChoice = Console.ReadLine();
            bool isValidChoice = int.TryParse(userExitChoice, out int number);

            while (!isValidChoice || number != 1 & number != 2)
            {
                Console.Clear();
                if (!isValidChoice)
                {
                    Console.WriteLine(new FormatException("Please enter a valid number."));
                }
                else if (number > 2 || number < 1)
                {
                    Console.WriteLine(new ValueOutOfRangeException(1, 2));
                }

                Console.WriteLine(exitString);
                userExitChoice = Console.ReadLine();
                isValidChoice = int.TryParse(userExitChoice, out number);
            }
            
            Thread.Sleep(250);
            Console.Clear();
            
            if (number == 1)
            {
                Service();
            }
            else
            {
                m_IsRunning = false;
            }
        }
    }
}