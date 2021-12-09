using System.IO;
using System.IO.Compression;

namespace ricaun.Nuke.Extensions
{
    public static class ZipExtension
    {
        /// <summary>
        /// Zip the file on the current folder
        /// </summary>
        /// <param name="file"></param>
        public static void ZipFileCompact(string file)
        {
            var fileName = Path.GetFileName(file);
            var fileZip = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".zip");
            using (var zip = ZipFile.Open(fileZip, ZipArchiveMode.Create))
                zip.CreateEntryFromFile(file, fileName);
        }

        /// <summary>
        /// Creates a zip archive that contains the files and directories from the specified directory.
        /// </summary>
        /// <param name="sourceDirectoryName"></param>
        /// <param name="destinationArchiveFileName"></param>
        /// <param name="includeBaseDirectory"></param>
        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, bool includeBaseDirectory = false)
        {
            destinationArchiveFileName = Path.ChangeExtension(destinationArchiveFileName, "zip");
            var folder = Path.GetDirectoryName(destinationArchiveFileName);
            if (Directory.Exists(folder) == false) Directory.CreateDirectory(folder);
            ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, CompressionLevel.Optimal, includeBaseDirectory);
        }
    }
}
