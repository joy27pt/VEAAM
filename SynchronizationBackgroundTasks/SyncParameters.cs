using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynchronizationBackgroundTask
{
    internal class SyncParameters
    {
        private double _interval = 0;

        public string SourceDirectoryPath { get; set; }
        public string DestinationDirectoryPath { get; set; }
        public double Interval
        {
            get { return _interval; }

            set
            {
                _interval = value;
            }
        }
        public string LogFilePath { get; set; }
    }
}
