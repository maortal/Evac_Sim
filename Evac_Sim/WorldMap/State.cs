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
        public SortedList<uint,Agent> Timestamp { get; private set; }
        public State[] Neighbours { get; protected set; }
        public double gCost;
        public double hCost;
        public State PrevStep;
        public bool Closed;
        public double LocalSearchGCostWith { get; private set; }
        public double LocalSearchGCostWithOut { get; private set; }
        private int IdxInHeap;
        private char chartowrite;

        public State()
        {
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
            this.LocalSearchGCostWith = double.MaxValue;
            this.LocalSearchGCostWithOut = double.MaxValue;
        }

        public override bool Equals(object obj)
        {
            return xLoc == ((State)obj).xLoc && yLoc == ((State)obj).yLoc;
        }

        public override int GetHashCode()
        {
            return (int)((xLoc + yLoc * Constants.GridWidth) - int.MaxValue);
        }

        public int getIndexInHeap()
        {
            return IdxInHeap;
        }

        public void setIndexInHeap(int index) { IdxInHeap = index; }

        public int CompareTo(IBinaryHeapItem other)
        {
            State s = (State)other;
            if ( this.hCost + this.gCost > s.hCost + s.gCost)
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
            return ((State)(Neighbours[a.Index]));
        }

        public void SetNeighbour(ActionMoves a, State v)
        {
            if (Neighbours[a.Index] != null)
                throw new Exception("Action leads to two different states");
            Neighbours[a.Index] = v;
        }
    }

}