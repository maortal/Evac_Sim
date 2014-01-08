using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evac_Sim.WorldMap
{
    public class ActionMoves
    {
        public string Name {  get; private set; }
        public byte Index {  get; private set; }
        public sbyte xEffect {  get; private set; }
        public sbyte yEffect {  get; private set; }
        public double Cost { get; private set; }
        public byte ReverseActionIndex {  get; private set; }

        public ActionMoves(string name, byte index, sbyte xEffect, sbyte yEffect, double cost, byte reverseActionIndex)
        {
            this.Name = name;
            this.Index = index;
            this.xEffect = xEffect;
            this.yEffect = yEffect;
            this.Cost = cost;
            this.ReverseActionIndex = reverseActionIndex;
        }

        public uint ApplyX(uint x)
        {
            return (uint)(x + xEffect);
        }

        public uint ApplyY(uint y)
        {
            return (uint)(y + yEffect);
        }

        public uint ApplyX(State s)
        {
            return (uint)(s.xLoc + xEffect);
        }

        public uint ApplyY(State s)
        {
            return (uint)(s.yLoc + yEffect);
        }

        public static uint BestAction(State from, State to)
        {
            if (from.xLoc > to.xLoc)
            {
                if(from.yLoc > to.yLoc)
                {
                    return 5;
                }
                else  if(from.yLoc < to.yLoc)
                {
                    return 7;
                }
                return 6;
            }

            if (from.xLoc < to.xLoc)
            {
                if (from.yLoc > to.yLoc)
                {
                    return 3;
                }
                else if (from.yLoc < to.yLoc)
                {
                    return 1;
                }
                return 2;
            }

            if (from.yLoc < to.yLoc)
            {
                return 0;
            }
            return 4;
        }

    }
}
