using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems.Heuristics
{
    public interface IHeuristicFunc
    {
        double ApplyH(State x1, State x2);
    }
}
