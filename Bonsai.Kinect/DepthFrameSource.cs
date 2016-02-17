using Microsoft.Kinect;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Kinect
{
    public class DepthFrameSource : Source<IplImage>
    {
        public override IObservable<IplImage> Generate()
        {
            return Observable.Defer(() =>
            {
                var kinect = KinectSensor.GetDefault();
                kinect.Open();
                var reader = kinect.DepthFrameSource.OpenReader();
                return Observable.FromEventPattern<DepthFrameArrivedEventArgs>(
                    handler => reader.FrameArrived += handler,
                    handler => reader.FrameArrived -= handler)
                    .Select(evt =>
                    {
                        var frame = evt.EventArgs.FrameReference.AcquireFrame();
                        var description = frame.FrameDescription;
                        var depth = ImageHelper.GetImageDepth(description);
                        var output = new IplImage(new Size(description.Width, description.Height), depth, 1);
                        frame.CopyFrameDataToIntPtr(output.ImageData, (uint)(output.WidthStep * output.Height));
                        return output;
                    }).Finally(() =>
                    {
                        reader.Dispose();
                        kinect.Close();
                    });
            });
        }
    }
}
