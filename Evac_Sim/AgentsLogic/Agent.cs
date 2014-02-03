using System.Collections.Generic;
using System.Drawing;
using Evac_Sim.SearchAlgorithems;
using Evac_Sim.WorldMap;

namespace Evac_Sim.AgentsLogic
{
    public class Agent
    {
        public State Index { get; set; }
        public State GoalState { get; set; }
        public Color agenColor { get; set; }
        public SolPath Agentsolution { get; set; }
        private SearchAlgo solver;
        public uint totalExpand { get; set; }
        public uint totalGenerated { get; set; }
        public double solCost { get; set; }
        public bool visible { get; set; }

        public Agent(State location, Color nameColor,SearchAlgo algo)
        {
            Index = new State(location);
            agenColor = nameColor;
            solver = algo;
            solCost=totalGenerated=totalExpand = 0;
            visible = true;
        }

        public bool Solve(State goal)
        {
            bool ans= solver.FindPath(Index, goal);
            if (ans)
            {
                Agentsolution = solver.getSolution();
                totalExpand = solver.getExpanded();
                totalGenerated = solver.getGenerated();
                solCost = solver.getSolutionCost();
            }
            return ans;
        }

        public bool Solve(HashSet<State> exitStates)
        {
            bool ans = solver.FindPath(exitStates, this.Index);
            if (ans)
                Agentsolution = solver.getSolution();
            totalExpand = solver.getExpanded();
            totalGenerated = solver.getGenerated();
            GoalState = Agentsolution.Solgoal();
            solCost = solver.getSolutionCost();
            return ans;
        }

        public Color GetAgColor()
        {
            return agenColor;
        }

        public override string ToString()
        {
            return "Agent"+ agenColor + " at location" + Index.ToString() + "Expand: " + totalExpand + " Generated: " + totalGenerated + " Solution Cost: " + solCost;
        }
    }
}
