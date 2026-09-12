using FolderSynchronization.Enums;
using FolderSynchronization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchronization.Helpers
{
    internal class  Messages
    {
        internal static string DirectoryNotFound(string directoryPath) {

            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine("The directory was not found.");
            sb.AppendLine($"Path: {directoryPath}");

            return sb.ToString();
        }

        internal static string SynchronizationStarting(){

            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine("Synchronization starting...");

            return sb.ToString();
        }

        internal static string CopyingFilesTo(string sourcePath, string targetPath) {
            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine($"Copying files from {sourcePath} to {targetPath}...");

            return sb.ToString();
        }

        internal static string SynchronizationComplete(ApplicationParameters parameters, string logFileName = "") {
            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine("Synchronization is complete.");
            sb.AppendLine($"Source folder: {parameters.SourceDirectoryPath}");
            sb.AppendLine($"Target folder: {parameters.TargetDirectoryPath}");
            sb.AppendLine($"Log file: {Path.Combine(parameters.LogFilePath, logFileName)}");

            return sb.ToString();
        }

        internal static string NoArgumentsProvided() {
            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine("No arguments provided.");
            sb.AppendLine(ProvideParameters());

            return sb.ToString();
        }

        internal static string SynchronizationIntervalIsNotValid(string value) {
            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine($"Synchronization Interval value \"{value}\" is invalid.");

            return sb.ToString();
        }

        internal static string PathShouldNotBeEmpty()
        {
            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine($"Path should not be empty.");
            
            return sb.ToString();
        }

        internal static string ProvideParameters() {
            StringBuilder sb = new StringBuilder().AppendLine();

            sb.AppendLine("Please provide the values for \"Source path\", \"Target path\", \"Synchronization interval\", and \"Log file path\".");
            sb.AppendLine("SynchronizationBackgroundTask.exe \"Source path\" \"Target path\" \"Synchronization interval\" \"Log file path\"");

            return sb.ToString();
        }
    }
}
