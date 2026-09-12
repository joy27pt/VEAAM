
namespace FolderSynchronization.Models
{
    internal class LogEvent
    {
        public Enums.EventType EventType { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
    }
}
