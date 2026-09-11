////using PLC_Simulator.Models;

////namespace PLC_Simulator.Services
////{
////    public class PlcSimulationService
////    {
////        private readonly Random _random = new();
////        private decimal _totalProduction = 0;

////        public PlcMachineData GenerateData()
////        {
////            var status = GenerateStatus();  

////            decimal speed = 0;
////            decimal inputWeight = 0; 
////            decimal outputWeight = 0;

////            decimal grade1 = 0;
////            decimal grade2 = 0;
////            decimal grade3 = 0;
////            decimal grade4 = 0;

////            if(status == "Running")
////            {
////                speed = _random.Next(800, 1201);

////                inputWeight = Math.Round((decimal)(_random.NextDouble() * 15 + 20), 2);

////                outputWeight = Math.Round(inputWeight *0.97m, 2);

////                grade1 = Math.Round(outputWeight * 0.30m, 2);
////                grade2 = Math.Round(outputWeight * 0.25m, 2);
////                grade3 = Math.Round(outputWeight * 0.25m, 2);

////                grade4 = Math.Round(
////                    outputWeight - grade1 - grade2 - grade3,
////                    2);

////                _totalProduction += outputWeight;
////            }

////            return new PlcMachineData
////            {
////                MachineCode = "RG-001",
////                MachineStatus = status,
////                MachineSpeed = speed,
////                InputWeight = inputWeight,
////                OutputWeight = outputWeight,
////                Grade1Weight = grade1,
////                Grade2Weight = grade2,
////                Grade3Weight = grade3,
////                Grade4Weight = grade4,
////                TotalProduction = Math.Round(_totalProduction, 2),
////                RecordedAt = DateTime.UtcNow
////            };
////        }

////        private string GenerateStatus()
////        {
////            int value = _random.Next(1, 101);

////            if (value <= 85)
////            {
////                return "Running";
////            }

////            if(value <= 95)
////            {
////                return "Stopped";
////            }

////            return "Fault";
////        }
////    }
////}


//using PLC_Simulator.Models;

//namespace PLC_Simulator.Services
//{
//    public class PlcSimulationService
//    {
//        private readonly Random _random = new();

//        private decimal _totalProduction = 0;

//        private decimal _totalGoodProduction = 0;
//        private decimal _totalRejectProduction = 0;

//        private int _totalCycles = 0;
//        private int _runningCycles = 0;

//        private decimal _runningSpeedSum = 0;
//        private int _runningSpeedSamples = 0;

//        private int _downtimeSeconds = 0;

//        private decimal _processTemperature = 27m;

//        private DateTime _lastProductionTime = DateTime.UtcNow;

//        public PlcMachineData GenerateData()
//        {
//            var now = DateTime.UtcNow;

//            var status = GenerateStatus();

//            decimal speed = 0;
//            decimal inputWeight = 0;
//            decimal outputWeight = 0;

//            decimal grade1 = 0;
//            decimal grade2 = 0;
//            decimal grade3 = 0;
//            decimal grade4 = 0;

//            _totalCycles++;

//            // -----------------------------------------
//            // MACHINE RUNNING
//            // -----------------------------------------
//            if (status == "Running")
//            {
//                speed = _random.Next(800, 1201);

//                inputWeight = Math.Round(
//                    (decimal)(_random.NextDouble() * 15 + 20),
//                    2);

//                outputWeight = Math.Round(
//                    inputWeight * 0.97m,
//                    2);

//                GenerateGrades(
//                    outputWeight,
//                    out grade1,
//                    out grade2,
//                    out grade3,
//                    out grade4);

//                _totalProduction += outputWeight;

//                _totalGoodProduction +=
//                    grade1 + grade2 + grade3;

//                _totalRejectProduction += grade4;

//                _runningCycles++;

//                _runningSpeedSum += speed;
//                _runningSpeedSamples++;

//                // Machine heats up gradually while running
//                _processTemperature +=
//                    (decimal)(_random.NextDouble() * 0.8 + 0.2);
//            }
//            else
//            {
//                // -----------------------------------------
//                // MACHINE STOPPED / FAULT
//                // -----------------------------------------

//                // Assuming each simulator cycle represents 1 second
//                _downtimeSeconds++;

//                // Machine cools down gradually
//                _processTemperature -=
//                    (decimal)(_random.NextDouble() * 0.5 + 0.1);
//            }

//            // -----------------------------------------
//            // KEEP TEMPERATURE IN REALISTIC RANGE
//            // -----------------------------------------

//            if (_processTemperature < 25m)
//                _processTemperature = 25m;

//            if (_processTemperature > 85m)
//                _processTemperature = 85m;

//            _processTemperature =
//                Math.Round(_processTemperature, 1);

//            // -----------------------------------------
//            // AVAILABILITY
//            // -----------------------------------------

//            decimal availability =
//                _totalCycles == 0
//                    ? 0
//                    : ((decimal)_runningCycles / _totalCycles) * 100;

//            // -----------------------------------------
//            // PERFORMANCE
//            // -----------------------------------------

//            decimal performance =
//                _runningSpeedSamples == 0
//                    ? 0
//                    : (_runningSpeedSum /
//                       (_runningSpeedSamples * 1200m)) * 100;

//            // -----------------------------------------
//            // QUALITY
//            // -----------------------------------------

//            decimal totalProduction =
//                _totalGoodProduction +
//                _totalRejectProduction;

//            decimal quality =
//                totalProduction == 0
//                    ? 0
//                    : (_totalGoodProduction /
//                       totalProduction) * 100;

//            // -----------------------------------------
//            // OEE
//            // -----------------------------------------

//            decimal oee =
//                (availability *
//                 performance *
//                 quality) / 10000;

//            // -----------------------------------------
//            // PRODUCTION RATE
//            // KG / HOUR
//            // -----------------------------------------

//            decimal productionRate = 0;

//            var elapsedHours =
//                (decimal)(now - _lastProductionTime).TotalHours;

//            if (elapsedHours > 0)
//            {
//                productionRate =
//                    outputWeight / elapsedHours;
//            }

//            // Better stable production rate based on
//            // cumulative production and running time
//            decimal runningHours =
//                _runningCycles / 3600m;

//            if (runningHours > 0)
//            {
//                productionRate =
//                    _totalProduction / runningHours;
//            }

//            // -----------------------------------------
//            // MACHINE HEALTH
//            // -----------------------------------------

//            string machineHealth =
//                GenerateMachineHealth(
//                    status,
//                    _processTemperature);

//            // -----------------------------------------
//            // UPDATE LAST TIME
//            // -----------------------------------------

//            _lastProductionTime = now;

//            // -----------------------------------------
//            // RETURN PLC DATA
//            // -----------------------------------------

//            return new PlcMachineData
//            {
//                MachineCode = "RG-001",

//                MachineStatus = status,

//                MachineSpeed = Math.Round(speed, 2),

//                InputWeight = inputWeight,

//                OutputWeight = outputWeight,

//                Grade1Weight = grade1,

//                Grade2Weight = grade2,

//                Grade3Weight = grade3,

//                Grade4Weight = grade4,

//                TotalProduction =
//                    Math.Round(
//                        _totalProduction,
//                        2),

//                Quality =
//                    Math.Round(
//                        quality,
//                        2),

//                OEE =
//                    Math.Round(
//                        oee,
//                        2),

//                ProcessTemperature =
//                    _processTemperature,

//                MachineHealth =
//                    machineHealth,

//                DowntimeSeconds =
//                    _downtimeSeconds,

//                ProductionRate =
//                    Math.Round(
//                        productionRate,
//                        2),

//                RecordedAt = now
//            };
//        }

//        // =====================================================
//        // GRADE DISTRIBUTION
//        // =====================================================

//        private void GenerateGrades(
//            decimal outputWeight,
//            out decimal grade1,
//            out decimal grade2,
//            out decimal grade3,
//            out decimal grade4)
//        {
//            decimal p1 = _random.Next(20, 46);
//            decimal p2 = _random.Next(15, 36);
//            decimal p3 = _random.Next(10, 31);

//            decimal p4 =
//                100 - p1 - p2 - p3;

//            if (p4 < 5)
//            {
//                p3 -= 5;
//                p4 += 5;
//            }

//            grade1 =
//                Math.Round(
//                    outputWeight * p1 / 100,
//                    2);

//            grade2 =
//                Math.Round(
//                    outputWeight * p2 / 100,
//                    2);

//            grade3 =
//                Math.Round(
//                    outputWeight * p3 / 100,
//                    2);

//            grade4 =
//                Math.Round(
//                    outputWeight -
//                    grade1 -
//                    grade2 -
//                    grade3,
//                    2);

//            // Prevent equal grade values
//            if (grade1 == grade2)
//                grade2 += 0.01m;

//            if (grade2 == grade3)
//                grade3 += 0.01m;

//            if (grade3 == grade4)
//                grade4 += 0.01m;

//            // Ensure exact total
//            decimal difference =
//                outputWeight -
//                (grade1 +
//                 grade2 +
//                 grade3 +
//                 grade4);

//            grade4 += difference;

//            grade4 =
//                Math.Round(
//                    grade4,
//                    2);
//        }

//        // =====================================================
//        // MACHINE STATUS
//        // =====================================================

//        private string GenerateStatus()
//        {
//            int value =
//                _random.Next(1, 101);

//            if (value <= 85)
//            {
//                return "Running";
//            }

//            if (value <= 95)
//            {
//                return "Stopped";
//            }

//            return "Fault";
//        }

//        // =====================================================
//        // MACHINE HEALTH
//        // =====================================================

//        private string GenerateMachineHealth(
//            string status,
//            decimal temperature)
//        {
//            if (status == "Fault")
//            {
//                return "Fault";
//            }

//            if (temperature >= 75)
//            {
//                return "Critical";
//            }

//            if (temperature >= 65)
//            {
//                return "Warning";
//            }

//            if (status == "Stopped")
//            {
//                return "Warning";
//            }

//            return "Healthy";
//        }
//    }
//}

using MESDemo.Models;

namespace MESDemo.Services
{
    public class PlcSimulationService
    {
        private readonly Random _random = new();

        private decimal _totalProduction = 0;
        private decimal _totalGoodProduction = 0;
        private decimal _totalRejectProduction = 0;

        private int _totalCycles = 0;
        private int _runningCycles = 0;

        private decimal _runningSpeedSum = 0;
        private int _runningSpeedSamples = 0;

        private int _downtimeSeconds = 0;
        private decimal _processTemperature = 27m;

        public PlcMachineData GenerateData()
        {
            var now = DateTime.UtcNow;
            var status = GenerateStatus();

            decimal speed = 0;
            decimal inputWeight = 0;
            decimal outputWeight = 0;

            decimal grade1 = 0;
            decimal grade2 = 0;
            decimal grade3 = 0;
            decimal grade4 = 0;

            _totalCycles++;

            // Machine running
            if (status == "Running")
            {
                speed = _random.Next(800, 1201);

                inputWeight = Math.Round(
                    (decimal)(_random.NextDouble() * 15 + 20),
                    2);

                outputWeight = Math.Round(
                    inputWeight * 0.97m,
                    2);

                GenerateGrades(
                    outputWeight,
                    out grade1,
                    out grade2,
                    out grade3,
                    out grade4);

                _totalProduction += outputWeight;

                _totalGoodProduction +=
                    grade1 + grade2 + grade3;

                _totalRejectProduction += grade4;

                _runningCycles++;

                _runningSpeedSum += speed;
                _runningSpeedSamples++;

                _processTemperature +=
                    (decimal)(_random.NextDouble() * 0.8 + 0.2);
            }
            else
            {
                // Machine stopped or fault
                _downtimeSeconds++;

                _processTemperature -=
                    (decimal)(_random.NextDouble() * 0.5 + 0.1);
            }

            // Keep temperature within range
            if (_processTemperature < 25m)
                _processTemperature = 25m;

            if (_processTemperature > 85m)
                _processTemperature = 85m;

            _processTemperature = Math.Round(
                _processTemperature,
                1);

            // Calculate availability
            decimal availability =
                _totalCycles == 0
                    ? 0
                    : ((decimal)_runningCycles / _totalCycles) * 100;

            // Calculate performance
            decimal performance =
                _runningSpeedSamples == 0
                    ? 0
                    : (_runningSpeedSum /
                       (_runningSpeedSamples * 1200m)) * 100;

            // Calculate quality
            decimal totalProduction =
                _totalGoodProduction +
                _totalRejectProduction;

            decimal quality =
                totalProduction == 0
                    ? 0
                    : (_totalGoodProduction /
                       totalProduction) * 100;

            // Calculate OEE
            decimal oee =
                (availability *
                 performance *
                 quality) / 10000;

            // Calculate production rate
            decimal productionRate =
                status == "Running"
                    ? Math.Round(
                        1500m +
                        (decimal)_random.NextDouble() * 300m,
                        2)
                    : 0;

            // Calculate machine health
            string machineHealth =
                GenerateMachineHealth(
                    status,
                    _processTemperature);

            return new PlcMachineData
            {
                MachineCode = "RG-001",

                MachineStatus = status,

                MachineSpeed = Math.Round(
                    speed,
                    2),

                InputWeight = inputWeight,

                OutputWeight = outputWeight,

                Grade1Weight = grade1,

                Grade2Weight = grade2,

                Grade3Weight = grade3,

                Grade4Weight = grade4,

                TotalProduction = Math.Round(
                    _totalProduction,
                    2),

                Quality = Math.Round(
                    quality,
                    2),

                OEE = Math.Round(
                    oee,
                    2),

                ProcessTemperature =
                    _processTemperature,

                MachineHealth =
                    machineHealth,

                DowntimeSeconds =
                    _downtimeSeconds,

                ProductionRate =
                    productionRate,

                RecordedAt = now
            };
        }

        // Generate grade distribution
        private void GenerateGrades(
            decimal outputWeight,
            out decimal grade1,
            out decimal grade2,
            out decimal grade3,
            out decimal grade4)
        {
            decimal p1 = _random.Next(20, 46);
            decimal p2 = _random.Next(15, 36);
            decimal p3 = _random.Next(10, 31);

            decimal p4 =
                100 - p1 - p2 - p3;

            if (p4 < 5)
            {
                p3 -= 5;
                p4 += 5;
            }

            grade1 = Math.Round(
                outputWeight * p1 / 100,
                2);

            grade2 = Math.Round(
                outputWeight * p2 / 100,
                2);

            grade3 = Math.Round(
                outputWeight * p3 / 100,
                2);

            grade4 = Math.Round(
                outputWeight -
                grade1 -
                grade2 -
                grade3,
                2);

            if (grade1 == grade2)
                grade2 += 0.01m;

            if (grade2 == grade3)
                grade3 += 0.01m;

            if (grade3 == grade4)
                grade4 += 0.01m;

            decimal difference =
                outputWeight -
                (grade1 +
                 grade2 +
                 grade3 +
                 grade4);

            grade4 += difference;

            grade4 = Math.Round(
                grade4,
                2);
        }

        // Generate machine status
        private string GenerateStatus()
        {
            int value = _random.Next(1, 101);

            if (value <= 85)
                return "Running";

            if (value <= 95)
                return "Stopped";

            return "Fault";
        }

        // Generate machine health
        private string GenerateMachineHealth(
            string status,
            decimal temperature)
        {            

            if (temperature >= 75)
                return "Critical";

            if (temperature >= 65)
                return "Warning";

            if (status == "Stopped")
                return "Warning";

            return "Healthy";
        }
    }
}