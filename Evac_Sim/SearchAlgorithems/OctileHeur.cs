using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems
{
    class OctileHeur : IHeuristicFunc
    {
        public double ApplyH(State a1, State a2)
        {
            int deltaX = Math.Abs((int)a1.xLoc - (int)a2.xLoc);
            int deltaY = Math.Abs((int)a1.yLoc - (int)a2.yLoc);
            return Math.Max(deltaX, deltaY) + (Constants.DiagCost - 1)*Math.Min(deltaX, deltaY);
        }
    }
}
