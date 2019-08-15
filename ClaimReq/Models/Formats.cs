using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaimReq.Models
{
    public class Formats
    {

        public static bool isImageFormat(string format) {
            if(format.Equals("bmp") || format.Equals("gif") || format.Equals("jpeg") || 
               format.Equals("jpg") || format.Equals("png") || format.Equals("tiff"))
            {
                 return true;
            }
            return false;
        }
    }
}
