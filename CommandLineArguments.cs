using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace AlfaParser
{
    class CommandLineArguments
    {
        #region Singleton

        private CommandLineArguments()
        {
        }

        private static CommandLineArguments _instance;

        /// <summary>
        /// Return single instance of CommandLineArguments.
        /// </summary>
        public static CommandLineArguments Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                CommandLineArguments tmp = new CommandLineArguments();
                Interlocked.CompareExchange(ref _instance, tmp, null);

                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Get or set path of input statement file.
        /// </summary>
        [Option('i', "input", DefaultValue = "movementList.csv", HelpText = "Path of input bank statement file", Required = false)]
        public string InputFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set path to converted statement.
        /// </summary>
        [Option('o', "output", DefaultValue = "alfa-statement.csv", HelpText = "Path to converted bank statement file")]
        public string OutputFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set input file separator.
        /// </summary>
        [Option("input-separator", DefaultValue = ";", HelpText = "Columns separator in input file")]
        public string InputSeparator
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set output file separator.
        /// </summary>
        [Option("output-separator", DefaultValue = ";", HelpText = "Columns separator in output file")]
        public string OutputSeparator
        {
            get;
            set;
        }

        
    }
}
