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
                sw.WriteLine($"Тип счёта{s}Номер счёта{s}Валюта{s}Оригинальная дата операции{s}Референс проводки{s}Описание операции{s}Приход{s}Расход{s}Дата 1{s}Дата совершения операции{s}Обратная дата 1{s}Обратная дата 2");

                _items.ToList().ForEach(i => i.AdjustTranscationDate(_minDate, _maxDate));

                foreach (AlfaItem item in _items.OrderBy(i => i.TransactionDate))
                {
                    WriteItem(sw, item);
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
                item.Date1Reversed.ToShortDateString(),
                item.TransactionDateReversed.ToShortDateString()
            };
            writer.WriteLine(string.Join(CommandLineArguments.Instance.OutputSeparator, parts));
        }
    }
}
