using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Kinect
{
    static class KinectManager
    {
        internal static readonly IObservable<KinectSensor> DefaultSensor = CreateSensor();

        static IObservable<KinectSensor> CreateSensor()
        {
            return Observable.Defer(() =>
            {
                var kinect = KinectSensor.GetDefault();
                kinect.Open();
                return Observable.Return(kinect)
                                 .Concat(Observable.Never(kinect))
                                 .Finally(kinect.Close);
            })
            .Multicast(() => new ReplaySubject<KinectSensor>(1))
            .RefCount();
        }
    }
}
