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
    public class InfraredFrameSource : Source<TimeInterval<IplImage>>
    {
        public override IObservable<TimeInterval<IplImage>> Generate()
        {
            return KinectManager.DefaultSensor.SelectMany(kinect =>
            {
                return Observable.Using(
                    () => kinect.InfraredFrameSource.OpenReader(),
                    reader => Observable.FromEventPattern<InfraredFrameArrivedEventArgs>(
                        handler => reader.FrameArrived += handler,
                        handler => reader.FrameArrived -= handler)
                        .Select(evt =>
                        {
                            using (var frame = evt.EventArgs.FrameReference.AcquireFrame())
                            {
                                var description = frame.FrameDescription;
                                var depth = ImageHelper.GetImageDepth(description);
                                var output = new IplImage(new Size(description.Width, description.Height), depth, 1);
                                frame.CopyFrameDataToIntPtr(output.ImageData, (uint)(output.WidthStep * output.Height));
                                return new TimeInterval<IplImage>(output, frame.RelativeTime);
                            }
                        }));
            });
        }
    }
}
