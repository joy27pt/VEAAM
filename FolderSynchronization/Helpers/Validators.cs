using FolderSynchronization.Models;

namespace FolderSynchronization.Helpers
{
    internal class Validators
    {
        public static double ValidateDoubleValue(string value)
        {
            double number;
            
            return double.TryParse(value, out number) ? number : throw new Exception(Messages.SynchronizationIntervalIsNotValid(value));
        }

        public static string ValidatePath(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception(Messages.PathShouldNotBeEmpty());
            }
            else {

                return value;
            }
        }

        public static ApplicationParameters PopulateParameters(string[] args) {
            ApplicationParameters parameters = new ApplicationParameters();

            for (int i = 0; i < args.Length; i++)
            {
                string? value = args[i];

                switch (i)
                {
                    case 0: //source
                        parameters.SourceDirectoryPath = ValidatePath(value);
                        break;

                    case 1: //target
                        parameters.TargetDirectoryPath = ValidatePath(value);
                        break;

                    case 2: //synchronization interval
                        parameters.Interval = ValidateDoubleValue(value);
                        break;

                    case 3: //log file path
                        parameters.LogFilePath = ValidatePath(value);
                        break;

                    default:
                        break;
                }
            }

            return parameters;
        }

        /// <summary>
        /// Create the directory if it does not exist.
        /// Only create for Target path and Log file path.
        /// </summary> 
        internal static bool CreateDirectory(string path)
        {
            if (!DirectoryExist(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return DirectoryExist(path);
        }

        internal static bool DirectoryExist(string path) {
            var dirInfo = new DirectoryInfo(path);
            bool dirExist = dirInfo.Exists;

            return dirInfo.Exists;
        }
    }
}
