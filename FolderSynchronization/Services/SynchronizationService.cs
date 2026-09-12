using FolderSynchronization.Enums;
using FolderSynchronization.Helpers;
using FolderSynchronization.Models;

namespace FolderSynchronization.Services
{
    public class SynchronizationService
    {
        private LogFileService _logFileService;
        private ApplicationParameters _parameters;

        public SynchronizationService(ApplicationParameters param) {
            _logFileService = new LogFileService(param.LogFilePath);
            _parameters = param;
        }

        public void Sync() {
            bool isSuccessful = true;
            string errorMessage = string.Empty;

            try
            {
                ConsoleWriteInColor(Messages.SynchronizationStarting(), ConsoleColor.Green);

                CopyDirectory(_parameters.SourceDirectoryPath, _parameters.TargetDirectoryPath, true);

                RemoveFilesInTargetNotFoundInSource(_parameters.SourceDirectoryPath, _parameters.TargetDirectoryPath, true);

                _logFileService.SaveLogsInTextFile(_parameters);

                ConsoleWriteInColor(Messages.SynchronizationComplete(_parameters, _logFileService.GetLogFile().Name), ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                isSuccessful = false;
                errorMessage = ex.Message;
            }

            if (!isSuccessful) {
                throw new Exception(errorMessage);
            }
        }

        protected void CopyDirectory(string sourcePath, string targetPath, bool recursive) {
            var sourceDirInfo = new DirectoryInfo(sourcePath);

            if (!sourceDirInfo.Exists)
            {
                throw new DirectoryNotFoundException(Messages.DirectoryNotFound(sourceDirInfo.FullName));
            }

            DirectoryInfo[] subDirectories = sourceDirInfo.GetDirectories();

            if (!Validators.DirectoryExist(targetPath))
            {
                Validators.CreateDirectory(targetPath);
                AddLog(EventType.Create, targetPath, string.Empty);
            }
            else {
                AddLog(EventType.Copy, targetPath, string.Empty);
            }

            var targetDirInfo = new DirectoryInfo(targetPath);

            List<string> targetDirectoryFiles = targetDirInfo.GetFiles().Select(x => x.Name).ToList();

            //Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in sourceDirInfo.GetFiles("*")) {
                string targetFilePath = Path.Combine(targetPath, file.Name);
                bool existInTargetDirectory = false;

                foreach (string fileName in targetDirectoryFiles)
                {
                    if (fileName == file.Name) existInTargetDirectory = true;
                }

                if (existInTargetDirectory) {
                    AddLog(EventType.Copy, targetPath, file.Name);
                }
                else
                {
                    AddLog(EventType.Create, targetPath, file.Name);
                }

                file.CopyTo(targetFilePath, true);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive) {
                foreach (DirectoryInfo subDir in subDirectories) {
                    string targetSubPath = Path.Combine(targetPath, subDir.Name);
                    CopyDirectory(subDir.FullName, targetSubPath, true);
                }
            }
        }

        /// <summary>
        /// Deletes files in Target folder that is not in the Source folder.
        /// Will also check the sub directories if recursive = true.
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        /// <param name="recursive"></param>
        protected void RemoveFilesInTargetNotFoundInSource(string sourceDirectory, string targetDirectory, bool recursive) {
            DirectoryInfo sourceDirInfo = new(sourceDirectory);
            DirectoryInfo targetDirInfo = new(targetDirectory);
            IEnumerable<FileInfo> targetDirFiles;

            if (!Directory.Exists(sourceDirectory) && !Directory.Exists(targetDirectory)) return;

            if (!Directory.Exists(sourceDirectory) && Directory.Exists(targetDirectory))
            {
                targetDirFiles = targetDirInfo.GetFiles("*");

                foreach (var file in targetDirFiles)
                {
                    DeleteFile(file);
                }

                DeleteDirectory(targetDirectory);
            }
            else {

                IEnumerable<FileInfo> sourceDirFiles = sourceDirInfo.GetFiles("*");

                targetDirFiles = targetDirInfo.GetFiles("*");

                FileCompare myFileCompare = new();

                //The files are in destination folder but not in source folder
                var filesFoundOnlyInDestination = (from fileInfo in targetDirFiles
                                                   select fileInfo)
                                                   .Except(sourceDirFiles, myFileCompare);


                if (filesFoundOnlyInDestination.Any())
                {
                    //delete these files in destination folder
                    foreach (var file in filesFoundOnlyInDestination)
                    {
                        DeleteFile(file);
                    }
                }

                if (recursive)
                {

                    foreach (DirectoryInfo targetSubDir in targetDirInfo.GetDirectories())
                    {
                        RemoveFilesInTargetNotFoundInSource(Path.Combine(sourceDirectory, targetSubDir.Name), targetSubDir.FullName, true);
                    }

                }
            }
        }

        private void DeleteFile(FileInfo file) {

            AddLog(EventType.Delete, file.DirectoryName, file.Name);

            file.Delete();
        }

        private void DeleteDirectory(string path)
        {
            AddLog(EventType.Delete, path, string.Empty);

            Directory.Delete(path);
        }

        private void AddLog(EventType eventType, string path, string fileName){
            _logFileService.AddLog(eventType, path, fileName);
        }

        public static ApplicationParameters PopulateParameters(string[] args) {
            return Validators.PopulateParameters(args);
        }

        public static string GetMessageNoArgumentsProvided() {
            return Messages.NoArgumentsProvided();
        }

        public static string GetMessageProvideArguments()
        {
            return Messages.ProvideParameters();
        }

        public static void ConsoleWriteInColor(string message, ConsoleColor fg = ConsoleColor.White, ConsoleColor bg = ConsoleColor.Black)
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;

            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
