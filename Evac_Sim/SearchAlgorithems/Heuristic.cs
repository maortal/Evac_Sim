using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems
{
    public interface IHeuristicFunc
    {
        double ApplyH(State x1, State x2);
    }
}
