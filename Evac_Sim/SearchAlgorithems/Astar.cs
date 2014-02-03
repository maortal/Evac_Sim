using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.AgentsLogic;
using Evac_Sim.AppGUI;
using Evac_Sim.DataStructures;
using Evac_Sim.WorldMap;

namespace Evac_Sim.SearchAlgorithems
{
    class Astar : SearchAlgo
    {
        protected BinaryHeap open;
        // protected LinkedList<State> closed;
        protected MainForm draw;
        protected ActionMoves[] aa = Constants.OctileMoves;
        protected IHeuristicFunc H;
        protected State goal;
        protected uint expanded;
        protected uint generated;

        public Astar(IHeuristicFunc h, MainForm draw = null)
        {
            open =new BinaryHeap();
            this.draw = draw;
            this.H = h;
        }
        public void init()
        {
             this.expanded = 0;
            this.generated = 0;
            open.Clear();
        }

        public string getName()
        {
            return "Astar Algorithm";
        }

        public bool FindPath(State start, State goal)
        {
            init();
            this.goal = goal;
            State s = null;
            start.gCost = 0;
            start.hCost = (start.hCost < H.ApplyH(start, goal)) ? H.ApplyH(start, goal) : start.hCost;
            start.PrevStep = null;
            start.Closed = true;
            open.Add(start);
            while (!Utils.cancel)
            {
                if (open.Count == 0)
                    return false;
                s = (State) open.Remove();
                if (s.Equals(goal))
                    break;
                Expand(s);
            }
            Utils.DrawExpandState(draw,s,s.Equals(goal));
            return true;
        }
        public bool FindPath(HashSet<State> exits,State agent)
        {
            init();
            goal = agent;
            State s = null;
            foreach (State st in exits)
            {
                st.gCost = 0;
                st.hCost = (st.hCost < H.ApplyH(st, goal)) ? H.ApplyH(st, goal) : st.hCost;
                st.PrevStep = null;
                st.Closed = true;
                open.Add(st);
            }

            while (!Utils.cancel)
            {
                if (open.Count == 0)
                    return false;
                s = (State)open.Remove();
                if (s.Equals(goal))
                {
                    goal = s;
                    break;
                }
                Expand(s);
            }
            Utils.DrawExpandState(draw, s, s.Equals(goal));
            return true;
        }


        protected virtual void Expand(State state)
        {
            State n;
            List<State> gDraw = new List<State>();
            foreach (ActionMoves a in aa)
            {
                n = (State) state.Neighbours[a.Index];
                if (n == null) continue;
                if (n.Closed)
                    if (n.gCost > state.gCost + a.Cost)
                    {
                        n.Closed = false;
                        open.Remove(n);
                    }
                if (n.Closed == false)
                {
                    n.Closed = true;
                    n.gCost = state.gCost + a.Cost;
                    n.hCost = H.ApplyH(n, goal);
                    n.PrevStep = (State) state;
                    open.Add(n);
                    gDraw.Add(n);
                    generated++;
                }
                Utils.DrawGenerateStates(draw, gDraw);
                expanded++;
                Utils.DrawExpandState(draw, state, state.Equals(goal));
            }
        }


        public void setHeuristics(IHeuristicFunc hFunc)
        {
            H = hFunc;
        }

        public double getSolutionCost()
        {
            return goal.gCost;
        }

        public SolPath getSolution()
        {
            return goal.GetPath();
        }

        public uint getExpanded()
        {
            return expanded;
        }

        public uint getGenerated()
        {
            return generated;
        }

        public double getGValOfGoal()
        {
            return goal.gCost;
        }
    }
}
