using Microsoft.Kinect;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Kinect
{
    public class ColorFrameSource : Source<TimeInterval<IplImage>>
    {
        public ColorFrameSource()
        {
            ColorConversion = ColorImageFormat.Bgra;
        }

        public ColorImageFormat ColorConversion { get; set; }

        public override IObservable<TimeInterval<IplImage>> Generate()
        {
            return KinectManager.DefaultSensor.SelectMany(kinect =>
            {
                return Observable.Using(
                    () => kinect.ColorFrameSource.OpenReader(),
                    reader => Observable.FromEventPattern<ColorFrameArrivedEventArgs>(
                        handler => reader.FrameArrived += handler,
                        handler => reader.FrameArrived -= handler)
                        .Select(evt =>
                        {
                            using (var frame = evt.EventArgs.FrameReference.AcquireFrame())
                            {
                                var description = frame.FrameDescription;
                                var depth = ImageHelper.GetImageDepth(ColorConversion);
                                var channels = ImageHelper.GetImageChannels(ColorConversion);
                                var output = new IplImage(new Size(description.Width, description.Height), depth, channels);
                                if (ColorConversion == ColorImageFormat.None)
                                {
                                    frame.CopyRawFrameDataToIntPtr(output.ImageData, (uint)(output.WidthStep * output.Height));
                                }
                                else
                                {
                                    frame.CopyConvertedFrameDataToIntPtr(
                                       output.ImageData,
                                       (uint)(output.WidthStep * output.Height),
                                       ColorConversion);
                                }
                                return new TimeInterval<IplImage>(output, frame.RelativeTime);
                            }
                        }));
            });
        }
    }
}
