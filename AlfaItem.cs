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
            string[] parts = input.Split(CommandLineArguments.Instance.InputSeparator.ToCharArray());

            AccountType = parts[0];
            AccountNumber = parts[1];
            Currency = parts[2];
            Reference = parts[4];

            StatementDate = DateTime.Parse(parts[3]);
            Description = parts[5];
            Debit = decimal.Parse(parts[6]);
            Credit = decimal.Parse(parts[7]);

            var match = _re.Matches(Description);
            if (match.Count == 0)
            {
                Date1 = Date1Reversed = TransactionDate = TransactionDateReversed = StatementDate;
            }
            else
            {
                if (match.Count >= 1)
                {
                    var result = Parse(match[0].Value);
                    Date1 = TransactionDate = result.Item1;
                    Date1Reversed = TransactionDateReversed = result.Item2;
                }
                if (match.Count == 2)
                {
                    var result = Parse(match[1].Value);
                    TransactionDate = result.Item1;
                    TransactionDateReversed = result.Item2;
                }
            }
        }

        private Tuple<DateTime, DateTime> Parse(string raw)
        {
            DateTime first, second;

            if (!DateTime.TryParse(raw, out first))
                first = StatementDate;
            string[] parts = raw.Split('.');
            if (parts[2].Length == 2)
            {
                string buf = parts[0];
                parts[0] = parts[2];
                parts[2] = buf;
            }

            if (!DateTime.TryParse(string.Join(".", parts), out second))
                second = StatementDate;

            return  new Tuple<DateTime, DateTime>(first, second);
        }

        public string RawString
        {
            get;
            private set;
        }

        #region Trash

        /// <summary>
        /// Get or set account type.
        /// </summary>
        public string AccountType
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set account number.
        /// </summary>
        public string AccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set currency.
        /// </summary>
        public string Currency
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set reference.
        /// </summary>
        public string Reference
        {
            get;
            set;
        }

        

        #endregion


        /// <summary>
        /// Get or set original date.
        /// </summary>
        public DateTime StatementDate
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set description.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set first date.
        /// </summary>
        public DateTime Date1
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set first date in reversed order.
        /// </summary>
        public DateTime Date1Reversed
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set second date.
        /// </summary>
        public DateTime TransactionDate
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set second date in reversed order.
        /// </summary>
        public DateTime TransactionDateReversed
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set debit.
        /// </summary>
        public decimal Debit
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set credit.
        /// </summary>
        public decimal Credit
        {
            get;
            set;
        }

        public void AdjustTranscationDate(DateTime minDate, DateTime maxDate)
        {
            if (TransactionDate < minDate || TransactionDate > maxDate)
                TransactionDate = TransactionDateReversed;
        }

        public override string ToString()
        {
            string[] parts = new[]
            {
                AccountType,
                AccountNumber,
                Currency,
                StatementDate.ToShortDateString(),
                Reference,
                Description,
                Date1.ToShortDateString(),
                TransactionDate.ToShortDateString(),
                Debit.ToString(),
                Credit.ToString(),
                Date1Reversed.ToShortDateString(),
                TransactionDateReversed.ToShortDateString()
            };
            return string.Join(";", parts);
        }
    }
}
