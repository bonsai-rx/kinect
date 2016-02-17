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

        internal static IplDepth GetImageDepth(ColorImageFormat format)
        {
            switch (format)
            {
                case ColorImageFormat.Bayer: return IplDepth.U8;
                case ColorImageFormat.Bgra: return IplDepth.U8;
                case ColorImageFormat.Rgba: return IplDepth.U8;
                case ColorImageFormat.Yuv: return IplDepth.U16;
                case ColorImageFormat.Yuy2: return IplDepth.U16;
                case ColorImageFormat.None:
                default:
                    return IplDepth.U16;
            }
        }

        internal static int GetImageChannels(ColorImageFormat format)
        {
            switch (format)
            {
                case ColorImageFormat.Bayer: return 1;
                case ColorImageFormat.Bgra: return 4;
                case ColorImageFormat.Rgba: return 4;
                case ColorImageFormat.Yuv: return 1;
                case ColorImageFormat.Yuy2: return 1;
                case ColorImageFormat.None:
                default:
                    return 1;
            }
        }
    }
}
