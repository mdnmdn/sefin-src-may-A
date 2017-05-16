using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sefin.AnacenImporter
{
    public class CustomException : Exception
    {
        public CustomException(string message = null) : base(message)
        {
        }
    }
}
