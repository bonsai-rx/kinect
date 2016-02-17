using Microsoft.Kinect;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Kinect
{
    class ImageHelper
    {
        internal static IplDepth GetImageDepth(FrameDescription description)
        {
            switch (description.BytesPerPixel)
            {
                case 1: return IplDepth.U8;
                case 2: return IplDepth.U16;
                default: throw new InvalidOperationException("Invalid image depth.");
            }
        }
    }
}
