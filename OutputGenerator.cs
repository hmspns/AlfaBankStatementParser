using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaParser
{
    /// <summary>
    /// Generate output file.
    /// </summary>
    class OutputGenerator
    {
        private List<AlfaItem> _items;
        private DateTime _minDate, _maxDate;

        internal OutputGenerator(List<AlfaItem> items)
        {
            _items = items;
            _minDate = items.Select(i => i.StatementDate).Min();
            _maxDate = items.Select(i => i.StatementDate).Max();
        }

        internal void Write(Stream output)
        {
            using (StreamWriter sw = new StreamWriter(output, Encoding.UTF8))
            {
                string s = CommandLineArguments.Instance.OutputSeparator;
                string header = $"Тип счёта{s}Номер счёта{s}Валюта{s}Оригинальная дата операции{s}Референс проводки{s}Описание операции{s}Приход{s}Расход{s}Дата 1{s}Дата совершения операции{s}Обратная дата 1{s}Обратная дата 2";
                sw.WriteLine(header);

                //_items.ForEach(i => i.AdjustTranscationDate(_minDate, _maxDate));

                List<AlfaItem> itemsInInterval = _items.Where(i => i.TransactionDate >= _minDate && i.TransactionDate <= _maxDate).ToList();
                List<AlfaItem> itemsOutsideInterval = _items.Except(itemsInInterval).ToList();

                //itemsInInterval.ForEach(i => i.AdjustTranscationDate(_minDate, _maxDate));
                foreach (AlfaItem item in itemsInInterval.OrderBy(i => i.TransactionDate))
                {
                    WriteItem(sw, item);
                }
                if (itemsOutsideInterval.Count > 0)
                {
                    //itemsOutsideInterval.ForEach(i => i.AdjustTranscationDate(_minDate, _maxDate));
                    sw.WriteLine();
                    sw.WriteLine("Данные, которые не попали в выборку");
                    sw.WriteLine();
                    sw.WriteLine(header);
                    foreach (AlfaItem item in itemsOutsideInterval.OrderBy(i => i.TransactionDate))
                    {
                        WriteItem(sw, item);
                    }
                }
            }
        }

        private void WriteItem(StreamWriter writer, AlfaItem item)
        {
            string[] parts = new[]
            {
                item.AccountType,
                item.AccountNumber,
                item.Currency,
                item.StatementDate.ToShortDateString(),
                item.Reference,
                item.Description,
                item.Debit.ToString(),
                item.Credit.ToString(),
                item.Date1.ToShortDateString(),
                item.TransactionDate.ToShortDateString(),
                //item.Date1Reversed.ToShortDateString(),
                //item.TransactionDateReversed.ToShortDateString()
            };
            writer.WriteLine(string.Join(CommandLineArguments.Instance.OutputSeparator, parts));
        }
    }
}
