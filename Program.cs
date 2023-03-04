using System;
using System.IO;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Запит шляху до файлу CSV
            Console.Write("Введіть шлях до файлу CSV: ");
            string filePath = Console.ReadLine();

            // Формат дати в файлі CSV
            string dateFormat = "dd.MM.yyyy";

            // Функції для отримання дати та суми транзакції
            Func<string, DateTime> getDate = (line) =>
            {
                string[] parts = line.Split(',');
                return DateTime.ParseExact(parts[0], dateFormat, null);
            };
            Func<string, double> getAmount = (line) =>
            {
                string[] parts = line.Split(',');
                return double.Parse(parts[1]);
            };

            // Делегат для відображення загальної суми за кожен день
            Action<DateTime, double> showTotal = (date, t) =>
            {
                Console.WriteLine("{0}: {1}", date.ToString(dateFormat), t);
            };

            // Зчитування файлу CSV та обчислення загальної суми за кожен день
            double totalSum = 0;
            DateTime currentDate = DateTime.MinValue;
            int count = 0;
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    DateTime date = getDate(line);
                    double amount = getAmount(line);

                    if (currentDate != date)
                    {
                        if (count > 0)
                        {
                            showTotal(currentDate, totalSum);
                            totalSum = 0;
                            count = 0;
                        }
                        currentDate = date;
                    }

                    totalSum += amount;
                    count++;

                    if (count == 10)
                    {
                        // Запис результатів у файл
                        using (StreamWriter writer = new StreamWriter(filePath, true))
                        {
                            writer.WriteLine("{0},{1}", currentDate.ToString(dateFormat), totalSum.ToString());
                        }

                        totalSum = 0;
                        count = 0;
                    }
                }

                if (count > 0)
                {
                    showTotal(currentDate, totalSum);

                    // Запис результатів у файл
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("{0},{1}", currentDate.ToString(dateFormat), totalSum.ToString());
                    }
                }
            }
        }
    }
}