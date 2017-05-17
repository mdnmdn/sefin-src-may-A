using Sefin.ServiceTool.Common;
using System;
using System.IO;

namespace Sefin.AnacenImporter
{
    public class ImportFileInfo
    {
        public string Company { get; internal set; }
        public string FullPath { get; internal set; }

        public void MoveToFolder(string destinationFolder)
        {
            FullPath = Tools.MoveFileToFolder(FullPath, destinationFolder);
        }

        public override string ToString()
        {
            return String.Format("[{0}] - {1}", Company, Path.GetFileName(FullPath));
        }
    }
}