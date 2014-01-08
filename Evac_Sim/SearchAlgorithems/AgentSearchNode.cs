using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems
{
    class AgentSearchNode
    {
        public State Location;
        public int Fcost;
        public int Gcost;
        public State PrevLocation;
    }
}
