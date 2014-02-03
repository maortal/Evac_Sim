using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.AgentsLogic;
using Evac_Sim.AppGUI;
using Evac_Sim.SearchAlgorithems.Heuristics;

namespace Evac_Sim.SearchAlgorithems
{
    class DM_Astar : Astar, SearchAlgo
    {
        public DM_Astar(IHeuristicFunc h, MainForm draw = null) : base(h, draw)
        {
        }

        public SolPath getSolution()
        {
            return goal.GetPathUpdateDm();
        }
    }
}
