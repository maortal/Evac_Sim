using System;
using System.Windows.Forms;

namespace Evac_Sim.WorldMap
{
    public class DirectionMapState : State
    {
        public double DVx { get; set; }
        public double DVy { get; set; }

        public DirectionMapState(uint index, uint amountOfActions, uint xPos = 0, uint yPos = 0)
            : base(index, amountOfActions, xPos, yPos)
        {
            DVx = 0;
            DVy = 0;
        }

        public DirectionMapState(State extendthis)
            : base(extendthis)
        {
            DVx = 0;
            DVy = 0;
        }

        public void Reset()
        {
            DVx = 0;
            DVy = 0;
        }

        public override string ToString()
        {
            return base.ToString() + "\tDV: [" + DVx + ", " + DVy + "]";
        }

        public void Normalize()
        {
            double magnitude = Math.Sqrt(Math.Pow(DVx, 2) + Math.Pow(DVy, 2));
            DVx /= magnitude;
            DVy /= magnitude;
        }
}
}
