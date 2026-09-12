using FolderSynchronization.Enums;
using FolderSynchronization.Models;
using System.Globalization;
using FolderSynchronization.Helpers;

namespace FolderSynchronization.Services
{
    internal class LogFileService
    {
        private LogFile _logFile;
        private IList<LogEvent> _logEvents; 

        public LogFileService() { 
            _logFile = new LogFile();
            _logEvents = new List<LogEvent>();
        }

        public LogFileService(string path)
        {

            _logFile = new LogFile() { Path = path, Name = GetDefaultFileName() };
            _logEvents = new List<LogEvent>();

        }

        public LogFileService(string path, string fileName) {

            _logFile = new LogFile() { Path = path, Name = fileName };
            _logEvents = new List<LogEvent>();
        }

        private string GetDefaultFileName()
        {
            string dateAndTime = System.DateTime.Now.ToString("d_M_yyyy_HH_mm_ss_fff", CultureInfo.InvariantCulture);
            return $"Logs_Sync_{dateAndTime}";
        }

        public void AddLog(EventType eventType, string filePath, string fileName)
        {
            filePath = Path.Combine(filePath, fileName);

            string message = GetLogMessage(eventType, filePath);

            _logEvents.Add(new LogEvent() { EventType = eventType, FilePath = filePath, Message = message });

            Console.WriteLine(message);
        }

        public void AddLog(EventType eventType, string filePath, List<string> fileNames)
        {
            foreach (string fileName in fileNames)
            {
                AddLog(eventType, filePath, fileName);
            }
        }

        private string GetLogMessage(EventType eventType, string filePath) {
            string message = string.Empty;
            
            switch (eventType)
            {
                case EventType.Create:
                    message = $"Created: {filePath}";
                    break;
                case EventType.Delete:
                    message = $"Deleted: {filePath}";
                    break;
                case EventType.Copy:
                    message = $"Copied: {filePath}";
                    break;
            }

            return message;
        }

        public IList<LogEvent> GetLogEvents() {
            return _logEvents;
        }

        public bool SaveLogsInTextFile(ApplicationParameters parameters) {

            if (!Validators.DirectoryExist(_logFile.Path))
            {
                Validators.CreateDirectory(_logFile.Path);
                AddLog(EventType.Create, _logFile.Path, string.Empty);
            }

            string filePath = Path.Combine(_logFile.Path, _logFile.Name);
            IList<LogEvent> logs = GetLogEvents();

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(Messages.SynchronizationStarting());

                foreach (LogEvent log in logs)
                {
                    writer.WriteLine($"{log.Message}");
                }

                writer.WriteLine(Messages.SynchronizationComplete(parameters, filePath));
            } 

            return true;
        }

        internal LogFile GetLogFile() {
            return _logFile;
        }
    }
}
