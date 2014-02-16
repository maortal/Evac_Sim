using System.Collections.Generic;
using System.Drawing;
using Evac_Sim.AgentsLogic;
using Evac_Sim.AppGUI;
using Evac_Sim.SearchAlgorithems.Heuristics;
using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems
{
    class DM_Astar : Astar, SearchAlgo
    {
        public DM_Astar(IHeuristicFunc h, MainForm draw = null) : base(h, draw)
        {
        }
        public new bool FindPath(State start, State goal)
        {
            init();
            this.start = start;
            this.goal = goal;
            State s = null;
            start.gCost = 0;
            start.hCost = 0;//(start.hCost < H.ApplyH(start, goal)) ? H.ApplyH(start, goal) : start.hCost;
            start.PrevStep = null;
            start.Closed = true;
            open.Add(start);
            while (!Utils.cancel)
            {
                if (open.Count == 0)
                    return false;
                s = (State)open.Remove();
                if (s.Equals(goal))
                    break;
                Expand(s);
            }
            Utils.DrawExpandState(draw, s, s.Equals(goal));
            return true;
        }
        protected void Expand(State state)
        {
            State n;
            List<State> gDraw = new List<State>();
            foreach (ActionMoves a in aa)
            {
                n = state.Neighbours[a.Index];
                if (n == null) continue;
                Point Vm = new Point(a.xEffect, a.yEffect);
                double MDVa = state.DVx * Vm.X + state.DVy * Vm.Y;
                double MDVb = n.DVx * Vm.X + n.DVy * Vm.Y;
                double tmpcost = (state.gCost + a.Cost) + 0.25 * Constants.Wdmax * (2 - MDVa - MDVb);
                if (n.Closed)
                    if (n.gCost > tmpcost)
                    {
                        n.Closed = false;
                        open.Remove(n);
                    }
                if (n.Closed == false)
                {
                    
                    n.Closed = true;
                    //wab + 0.25*wmax(2 - DVa*MVab-DVb*MVab)
                    n.gCost = tmpcost;
                    n.hCost = 0;//H.ApplyH(n, goal);
                    n.PrevStep = state;
                    open.Add(n);
                    gDraw.Add(n);
                    generated++;
                }
                Utils.DrawGenerateStates(draw, gDraw);
                expanded++;
                Utils.DrawExpandState(draw, state, state.Equals(goal));
            }
        }
        public new SolPath getSolution()
        {
            if (MainForm.DMLearnMode)
                return goal.GetPathUpdateDm();
            else
            {
                return goal.GetPath();
            }
        }
    }
}
