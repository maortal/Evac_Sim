using System;
using System.Collections;
using System.Collections.Generic;
using Evac_Sim.AgentsLogic;
using Evac_Sim.DataStructures;

namespace Evac_Sim.WorldMap
{
    public class State : IBinaryHeapItem
    {
        public uint Index { get; private set; }
        public uint xLoc { get; private set; }
        public uint yLoc { get; private set; }
        //public SortedList<uint,Agent> Timestamp { get; private set; }
        public State[] Neighbours { get; protected set; }
        public double gCost;
        public double hCost;
        public State PrevStep;
        public bool Closed;
        private int IdxInHeap;

        //Direction Map Variables
        public double DVx { get; set; }
        public double DVy { get; set; }
        private bool _visited = false;


        public State(){}

        public State(State copyof)
        {
            Index = copyof.Index;
            xLoc = copyof.xLoc;
            yLoc = copyof.yLoc;
            Neighbours = copyof.Neighbours;
            gCost = copyof.gCost;
            hCost = copyof.hCost;
            PrevStep = copyof.PrevStep;
            Closed = copyof.Closed;
            IdxInHeap = copyof.IdxInHeap;
            DVx = 0;
            DVy = 0;
        }

        public State(uint index, uint amountOfActions, uint xPos = 0, uint yPos = 0)
        {
            this.Index = index;
            this.xLoc = xPos;
            this.yLoc = yPos;
            this.Neighbours = new State[amountOfActions];
            this.Closed = false;
            this.hCost = -1;
            this.gCost = -1;
            DVx = 0;
            DVy = 0;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(DBNull))
                return xLoc == ((State) obj).xLoc && yLoc == ((State) obj).yLoc;
            return false;
        }

        public override int GetHashCode()
        {
            return (int) ((xLoc + yLoc*Constants.GridWidth) - int.MaxValue);
        }

        public int getIndexInHeap()
        {
            return IdxInHeap;
        }

        public void setIndexInHeap(int index)
        {
            IdxInHeap = index;
        }

        public int CompareTo(IBinaryHeapItem other)
        {
            State s = (State) other;
            if (this.hCost + this.gCost > s.hCost + s.gCost)
                return 1;
            else if (this.hCost + this.gCost < s.hCost + s.gCost)
                return -1;
            else if (this.hCost > s.hCost)
                return 1;
            else if (this.hCost < s.hCost)
                return -1;
            else
                return 0;
        }

        public State Apply(ActionMoves a)
        {
            return ((State) (Neighbours[a.Index]));
        }

        public void SetNeighbour(ActionMoves a, State v)
        {
            if (Neighbours[a.Index] != null)
                throw new Exception("Action leads to two different states");
            Neighbours[a.Index] = v;
        }

        public SolPath GetPath()
        {
            SolPath res = new SolPath();
            //res.Add(this);
            State curr = this;
            while (curr != null)
            {
                res.Add(curr);
                curr = curr.PrevStep;
            }
            return res;
        }

        public void reset()
        {
            this.hCost = -1;
            this.gCost = -1;
            this.PrevStep = null;
            resetRun();
        }

        public void resetRun()
        {
            this.Closed = false;
        }

        public override string ToString()
        {
            if (DVx != 0.0 || DVy != 0.0)
                return "(" + xLoc + "," + yLoc + ")" + "\tDV: [" + DVx + ", " + DVy + "]";
            return "(" + xLoc + "," + yLoc + ")";
        }


        private void Normalize()
        {
            double magnitude = Math.Sqrt(Math.Pow(DVx, 2) + Math.Pow(DVy, 2));
            DVx /= magnitude;
            DVy /= magnitude;
        }

        public void UpdateDirectionVector(double xDirection, double yDirection)
        {
            if (!_visited)
            {
                DVx = Constants.AlphaLearning * xDirection;
                DVy = Constants.AlphaLearning * yDirection;
            }
            else
            {
                DVx = (Constants.AlphaLearning * DVx) + ((1 - Constants.AlphaLearning) * xDirection);
                DVy = (Constants.AlphaLearning * DVy) + ((1 - Constants.AlphaLearning) * yDirection);
                Normalize();
            }
        }
        public SolPath GetPathUpdateDm()
        {
            SolPath res = new SolPath();
            //res.Add(this);
            State prev = this;
            State curr = this;
            while (curr != null)
            {
                res.Add(curr);
                if (!(prev == curr))
                {
                    uint bestact = ActionMoves.BestAction(prev, curr);
                    prev.UpdateDirectionVector(Constants.OctileMoves[bestact].xEffect, Constants.OctileMoves[bestact].yEffect);
                    prev._visited = true;
                    curr.UpdateDirectionVector(Constants.OctileMoves[bestact].xEffect, Constants.OctileMoves[bestact].yEffect);
                }
                prev = curr;
                curr = curr.PrevStep;  //we searched backward so its actually next
            }
            return res;
        }
    }

}