using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Kinect
{
    public class BodyDataFrame
    {
        readonly IEnumerable<Body> bodies;
        readonly Vector4 floorClipPlane;
        readonly TimeSpan relativeTime;

        public BodyDataFrame(IEnumerable<Body> bodies, Vector4 floorClipPlane, TimeSpan relativeTime)
        {
            if (bodies == null)
            {
                throw new ArgumentNullException("bodies");
            }

            this.bodies = bodies;
            this.floorClipPlane = floorClipPlane;
            this.relativeTime = relativeTime;
        }

        public IEnumerable<Body> Bodies
        {
            get { return bodies; }
        }

        public Vector4 FloorClipPlane
        {
            get { return floorClipPlane; }
        }

        public TimeSpan RelativeTime
        {
            get { return relativeTime; }
        }
    }
}
