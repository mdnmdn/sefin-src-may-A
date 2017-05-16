using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.ServiceTool.Common
{
    public static class Tools
    {
        public static string MoveFileToFolder(string sourceFullPath, string destinationFolder)
        {
            var fileName = Path.GetFileName(sourceFullPath);
            var destinationFullPath = Path.Combine(destinationFolder, fileName);

            File.Move(sourceFullPath, destinationFullPath);

            return destinationFullPath;
        }
    }
}
