
namespace FolderSynchronization.Models
{
    internal class LogFile
    {
        public string Name { get; set; }
        public string Path { get; set; }         
        public IList<LogEvent> Events { get; set; }
    }
}
