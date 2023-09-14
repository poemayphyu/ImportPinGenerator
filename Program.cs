namespace UniqueSerialNumberGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter Total Count:");
                var input = Console.ReadLine();
                int totalCount = int.TryParse(input, out totalCount) ? totalCount : 10;

                Console.WriteLine("Enter Start Sequence (eg. 2000000000211111):");
                input = Console.ReadLine();
                string startSequence = input ?? "2000000000211111";

                //unique serial number
                List<string> serialNumbers = GenerateUniqueSerialNumbers(totalCount);

                //sequence
                List<string> sequenceNumbers = GenerateSequenceNumbers(startSequence, totalCount);

                //export file
                Console.WriteLine("Start exporting file...");
                ExportToTextFile(sequenceNumbers, serialNumbers);
                Console.WriteLine("Finished.");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static List<string> GenerateUniqueSerialNumbers(int count)
        {
            List<string> serialNumbers = new List<string>();
            HashSet<string> uniqueNumbers = new HashSet<string>();
            Random random = new Random();

            while (serialNumbers.Count < count)
            {
                long randomNumber = GenerateRandomLong(random, 100000000000000000, 999999999999999999);
                string serialNumber = randomNumber.ToString();

                if (!uniqueNumbers.Contains(serialNumber))
                {
                    uniqueNumbers.Add(serialNumber);
                    serialNumbers.Add(serialNumber);
                }
            }

            return serialNumbers;
        }

        static long GenerateRandomLong(Random random, long minValue, long maxValue)
        {
            byte[] buf = new byte[8];
            random.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (maxValue - minValue)) + minValue);
        }

        static List<string> GenerateSequenceNumbers(string startNumber, int count)
        {
            List<string> numbers = new List<string>();

            long currentNumber = long.Parse(startNumber);

            for (int i = 0; i < count; i++)
            {
                numbers.Add(currentNumber.ToString());
                currentNumber++;
            }

            return numbers;
        }

        static void ExportToTextFile(List<string> sequenceNumbers, List<string> serialNumbers)
        {
            var combinedData = serialNumbers.Zip(sequenceNumbers, (serial, sequence) => new { Sequence = sequence, Serial = serial });

            using (StreamWriter writer = new StreamWriter("..\\..\\..\\EBA_Pins.txt"))
            {
                //header
                writer.WriteLine("SequenceNumbers  SerialNumbers" +
                               "\n-----------------------------------");

                foreach (var dataPair in combinedData)
                {
                    writer.WriteLine($"{dataPair.Sequence} {dataPair.Serial}");
                }
            }
        }

    }
}
