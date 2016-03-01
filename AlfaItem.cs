using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlfaParser
{
    class AlfaItem
    {
        private static Regex _re = new Regex(@"[\d]{2}\.[\d]{2}\.[\d]{2,4}", RegexOptions.Singleline | RegexOptions.Compiled);

        public AlfaItem(string input)
        {
            RawString = input;
            string[] parts = input.Split(';');

            AccountType = parts[0];
            AccountNumber = parts[1];
            Currency = parts[2];
            Reference = parts[4];

            OriginalDate = DateTime.Parse(parts[3]);
            Description = parts[5];
            Debit = decimal.Parse(parts[6]);
            Credit = decimal.Parse(parts[7]);

            var match = _re.Matches(Description);
            if (match.Count == 0)
            {
                Date1 = Date1Reversed = Date2 = Date2Reversed = OriginalDate;
            }
            else
            {
                if (match.Count >= 1)
                {
                    var result = Parse(match[0].Value);
                    Date1 = Date2 = result.Item1;
                    Date1Reversed = Date2Reversed = result.Item2;
                }
                if (match.Count == 2)
                {
                    var result = Parse(match[1].Value);
                    Date2 = result.Item1;
                    Date2Reversed = result.Item2;
                }
            }
        }

        private Tuple<DateTime, DateTime> Parse(string raw)
        {
            DateTime first, second;

            if (!DateTime.TryParse(raw, out first))
                first = OriginalDate;
            string[] parts = raw.Split('.');
            if (parts[2].Length == 2)
            {
                string buf = parts[0];
                parts[0] = parts[2];
                parts[2] = buf;
            }

            if (!DateTime.TryParse(string.Join(".", parts), out second))
                second = OriginalDate;

            return  new Tuple<DateTime, DateTime>(first, second);
        }

        public string RawString
        {
            get;
            private set;
        }

        #region Trash

        /// <summary>
        /// Возвращает или устанавливает тип счёта.
        /// </summary>
        public string AccountType
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает номер счёта.
        /// </summary>
        public string AccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает валюту.
        /// </summary>
        public string Currency
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает референс подводки.
        /// </summary>
        public string Reference
        {
            get;
            set;
        }

        

        #endregion


        /// <summary>
        /// Возвращает или устанавливает оригинальную дату.
        /// </summary>
        public DateTime OriginalDate
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает описание.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает первую дату.
        /// </summary>
        public DateTime Date1
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает первую дату в обратном порядке.
        /// </summary>
        public DateTime Date1Reversed
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает вторую дату.
        /// </summary>
        public DateTime Date2
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает вторую дату в обратном порядке.
        /// </summary>
        public DateTime Date2Reversed
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает приход.
        /// </summary>
        public decimal Debit
        {
            get;
            set;
        }

        /// <summary>
        /// Возвращает или устанавливает расход.
        /// </summary>
        public decimal Credit
        {
            get;
            set;
        }

        public override string ToString()
        {
            string[] parts = new[]
            {
                OriginalDate.ToShortDateString(),
                Description,
                Date1.ToShortDateString(),
                Date2.ToShortDateString(),
                Debit.ToString(),
                Credit.ToString(),
                Date1Reversed.ToShortDateString(),
                Date2Reversed.ToShortDateString()
            };
            return string.Join(";", parts);
        }
    }
}
