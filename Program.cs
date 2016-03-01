using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            const string INPUT = "movementList.csv";
            const string OUTPUT = "alfa.csv";

            List<AlfaItem> results = new List<AlfaItem>();

            bool isFirstLine = true;
            foreach (string line in File.ReadLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, INPUT), Encoding.Default))
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue;
                }
                results.Add(new AlfaItem(line));
            }

            using (FileStream stream = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, OUTPUT)))
            {
                using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    sw.WriteLine("Оригинальная дата;Описание;Дата 1;Дата совершения операции;Приход;Расход;Обратная дата 1;Обратная дата 2");
                    foreach (AlfaItem item in results.OrderBy(i => i.Date2))
                    {
                        sw.WriteLine(item.ToString());
                    }
                }
            }
        }
    }
}
