using System;
using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems.Heuristics
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
