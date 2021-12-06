using System.IO;
using System.IO.Compression;

namespace ricaun.Nuke.Extensions
{
    public static class ZipExtension
    {
        public static void ZipFileCompact(string file)
        {
            var fileName = Path.GetFileName(file);
            var fileZip = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".zip");
            using (var zip = ZipFile.Open(fileZip, ZipArchiveMode.Create))
                zip.CreateEntryFromFile(file, fileName);
        }
    }
}
