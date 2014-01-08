using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evac_Sim.WorldMap
{
    public class Graph
    {
        public char[][] graphForMeir { get; set; }
        public State[] GraphMap { get; private set; }
        public State[] Blocked { get; private set; }
        public State[][] Grid { get; private set; }
        public uint Height; //{ get; private set; }
        public uint Width; //{ get; private set; }
        public ActionMoves[] PossiActions = {new ActionMoves("S",0,0,1,1,4), new ActionMoves("SE",1,1,1,Constants.DiagCost,5), new ActionMoves("E",2,1,0,1,6), 
                                 new ActionMoves("NE",3,1,-1,Constants.DiagCost,7), new ActionMoves("N",4,0,-1,1,0), new ActionMoves("NW",5,-1,-1,Constants.DiagCost,1),
                                 new ActionMoves("W",6,-1,0,1,2),  new ActionMoves("SW",7,-1,1,Constants.DiagCost,3)};


        public Graph(TextReader map, ActionMoves[] aa)
        {
            this.PossiActions = aa;
            string line = map.ReadLine();
            string[] lineParts = line.Split(' ');
            Height = 0;
            Width = 0;
            char cell;

            while (line.StartsWith("map") == false)
            {
                if (lineParts.Length > 0)
                {
                    if (lineParts[0].Equals("height"))
                        Height = uint.Parse(lineParts[1]);
                    else if (lineParts[0].Equals("width"))
                        Width = uint.Parse(lineParts[1]);
                }
                line = map.ReadLine();
                lineParts = line.Split(' ');
            }

            if (Height == 0 || Width == 0)
                throw new Exception("Can't read map file");
            Constants.height = Height;
            Constants.width = Width;
            uint i=0;
            List<State> green = new List<State>();
            Grid = new State[Height][];
            graphForMeir = new char[Height][];
            for (uint y = 0; y < Height; y++)
            {
                Grid[y] = new State[Width];
                graphForMeir[y] = new char[Width];
                line = map.ReadLine();
                for (uint x = 0; x < Width; x++)
                {
                    cell = line.ElementAt((int) x);
                    graphForMeir[y][x] = '@';
                    if (cell == '.'){
                        Grid[y][x] = new State(i++, (uint) aa.Length, x, y);
                        graphForMeir[y][x] = '.';
                    }
            else
                    {
                        Grid[y][x] = null;
                        if (cell == 'T')
                            green.Add(new State(0, (uint)aa.Length, x, y));
                    }
                }
            }
            this.Blocked = green.ToArray();
            GraphMap = new State[i];
            map.Close();

            for (uint y = 0; y < Height; y++)
            {
                for (uint x = 0; x < Width; x++)
                {
                    if (Grid[y][x] == null)
                        continue;
                    foreach (ActionMoves a in aa)
                    {
                        uint neighbourX = a.ApplyX(x);
                        uint neighbourY = a.ApplyY(y);
                        if (neighbourX < Width && neighbourY < Height)
                        {
                                if (Grid[y][neighbourX] != null && Grid[neighbourY][x] != null)
                                    Grid[y][x].SetNeighbour(a, Grid[neighbourY][neighbourX]);
                        }
                    }
                    GraphMap[(int)Grid[y][x].Index] = Grid[y][x];
                }
            }
        }

        public Graph(uint size, ActionMoves[] aa)
        {
            this.PossiActions = aa;

            Height = size;
            Width = size;


            Constants.height = Height;
            Constants.width = Width;

            uint i = 0;
            List<State> green = new List<State>();
            Grid = new State[Height][];
            for (uint y = 0; y < Height; y++)
            {
                Grid[y] = new State[Width];

                for (uint x = 0; x < Width; x++)
                {
                    Grid[y][x] = new State(i++, (uint)aa.Length, x, y);
                }
            }
            this.Blocked = green.ToArray();
            GraphMap = new State[i];

            for (uint y = 0; y < Height; y++)
            {
                for (uint x = 0; x < Width; x++)
                {
                    if (Grid[y][x] == null)
                        continue;
                    foreach (ActionMoves a in aa)
                    {
                        uint neighbourX = a.ApplyX(x);
                        uint neighbourY = a.ApplyY(y);
                        if (neighbourX < Width && neighbourY < Height)
                        {
                                if (Grid[y][neighbourX] != null && Grid[neighbourY][x] != null)
                                    Grid[y][x].SetNeighbour(a, Grid[neighbourY][neighbourX]);

                        }
                    }
                    GraphMap[(int)Grid[y][x].Index] = Grid[y][x];
                }
            }
        }
    }
}
