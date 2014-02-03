using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.AgentsLogic;
using Evac_Sim.SearchAlgorithems.Heuristics;
using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems
{
    public interface SearchAlgo
    {
        void init();
        string getName();
        bool FindPath(State start, State goal);
        bool FindPath(HashSet<State> exits, State agent);
        void setHeuristics(IHeuristicFunc hFunc);
        double getSolutionCost();
        SolPath getSolution();
        uint getExpanded();
        uint getGenerated();
        double getGValOfGoal();
    }
}
