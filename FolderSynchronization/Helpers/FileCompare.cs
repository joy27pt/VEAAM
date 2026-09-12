using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderSynchronization.Helpers
{
    internal class FileCompare : IEqualityComparer<FileInfo>
    {
        // This implementation defines a very simple comparison
        // between two FileInfo objects. It only compares the name
        // of the files being compared and their length in bytes.
        public bool Equals(FileInfo? file1, FileInfo? file2)
        {
            return (file1?.Name == file2?.Name &&
                        file1?.Length == file2?.Length);
        }

        // Return a hash that reflects the comparison criteria. According to the
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must
        // also be equal. Because equality as defined here is a simple value equality, not
        // reference identity, it is possible that two or more objects will produce the same
        // hash code.
        public int GetHashCode([DisallowNull] FileInfo file)
        {
            string s = $"{file.Name}{file.Length}";
            return s.GetHashCode();
        }
    }

}
