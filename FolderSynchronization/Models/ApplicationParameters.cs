
namespace FolderSynchronization.Models
{
    public class ApplicationParameters
    {
        private double _interval = 0;

        public string SourceDirectoryPath { get; set; }
        public string TargetDirectoryPath { get; set; }
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
