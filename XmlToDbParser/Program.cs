using XmlToDbParser.Database;

namespace XmlToDbParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                using XmlToDbParserDatabase database = new("XmlToDbParserDatabase_1.db");

                Console.WriteLine();
                Console.WriteLine();

                try
                {
                    Console.WriteLine("Введите имя файла:");
                    string? filePath = Console.ReadLine();

                    var xmlOrders = XmlOrdersParser.Parse(filePath!);
                    if (xmlOrders == null)
                        return;

                    xmlOrders.AddOrUpdateTo(database);

                    Console.WriteLine("Изменения в базу внесены.");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Ошибка: не удалось найти указанный файл.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка:\n" + e.Message);
                }

                Console.WriteLine("Нажмите любую клавишу для завершения работы или C для смены имени файла.");
                if (Console.ReadKey(true).Key == ConsoleKey.C)
                    continue;
                break;
            }
        }
    }
}
