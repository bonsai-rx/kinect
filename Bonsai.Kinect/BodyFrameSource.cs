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
    public class BodyFrameSource : Source<BodyDataFrame>
    {
        public override IObservable<BodyDataFrame> Generate()
        {
            return KinectManager.DefaultSensor.SelectMany(kinect =>
            {
                return Observable.Using(
                    () => kinect.BodyFrameSource.OpenReader(),
                    reader => Observable.FromEventPattern<BodyFrameArrivedEventArgs>(
                        handler => reader.FrameArrived += handler,
                        handler => reader.FrameArrived -= handler)
                        .Select(evt =>
                        {
                            using (var frame = evt.EventArgs.FrameReference.AcquireFrame())
                            {
                                var bodies = new Body[frame.BodyCount];
                                frame.GetAndRefreshBodyData(bodies);
                                return new BodyDataFrame(bodies, frame.FloorClipPlane, frame.RelativeTime);
                            }
                        }));
            });
        }
    }
}
