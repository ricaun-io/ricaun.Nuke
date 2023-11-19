using System;
using System.IO;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// PathTooLongUtils
    /// </summary>
    public class PathTooLongUtils
    {
        /// <summary>
        /// MAX_PATH
        /// </summary>
        public const int MAX_PATH = 260;
        /// <summary>
        /// FileMoveToTempUtils
        /// </summary>
        public class FileMoveToTemp : IDisposable
        {
            private readonly string filePath;
            private string tempFilePath;
            /// <summary>
            /// FileMoveToTempUtils
            /// </summary>
            /// <param name="filePath"></param>
            public FileMoveToTemp(string filePath)
            {
                this.filePath = filePath;
                Initialize();
            }

            private void Initialize()
            {
                if (IsPathTooLong())
                {
                    tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(this.filePath));
                    File.Move(this.filePath, tempFilePath, true);
                }
            }

            /// <summary>
            /// GetFilePathLong
            /// </summary>
            /// <returns></returns>
            public int GetFilePathLong()
            {
                return GetFilePath().Length;
            }

            /// <summary>
            /// Check if path is too long
            /// </summary>
            /// <returns></returns>
            public bool IsPathTooLong()
            {
                return GetFilePathLong() > MAX_PATH;
            }

            /// <summary>
            /// GetFilePath
            /// </summary>
            /// <returns></returns>
            public string GetFilePath()
            {
                return tempFilePath ?? filePath;
            }

            /// <summary>
            /// Dispose
            /// </summary>
            public void Dispose()
            {
                if (tempFilePath is null)
                    return;

                File.Move(tempFilePath, filePath, true);
            }
        }
    }
}
