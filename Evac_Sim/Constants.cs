using Evac_Sim.WorldMap;

namespace Evac_Sim
{
    static class Constants
    {
        public static double DiagCost = 1.5;

        //public static double hexDiagCost = 2.25;

        public static ActionMoves[] OctileMoves = {new ActionMoves("S",0,0,1,1,4), new ActionMoves("SE",1,1,1,DiagCost,5), new ActionMoves("E",2,1,0,1,6), 
                                 new ActionMoves("NE",3,1,-1,DiagCost,7), new ActionMoves("N",4,0,-1,1,0), new ActionMoves("NW",5,-1,-1,DiagCost,1),
                                 new ActionMoves("W",6,-1,0,1,2),  new ActionMoves("SW",7,-1,1,DiagCost,3)};
        /*
         public static MoveAction[] aaHex = {new MoveAction("S",0,0,1,1,4), new MoveAction("SE",1,1,1,diagCost,5), new MoveAction("E",2,1,0,1,6), 
                                  new MoveAction("NE",3,1,-1,diagCost,7), new MoveAction("N",4,0,-1,1,0), new MoveAction("NW",5,-1,-1,diagCost,1),
                                  new MoveAction("W",6,-1,0,1,2),  new MoveAction("SW",7,-1,1,diagCost,3),
                                  new MoveAction("SSE", 8, 1,2,hexDiagCost,12), new MoveAction("SEE", 9, 2,1,hexDiagCost,13),new MoveAction("NEE", 10, 2,-1,hexDiagCost,14),
                                  new MoveAction("NNE", 11, 1,-2,hexDiagCost,15), new MoveAction("NNW", 12, -1,-2,hexDiagCost,8), new MoveAction("NWW", 13, -2,-1,hexDiagCost,9),
                                  new MoveAction("SWW", 14, -2,1,hexDiagCost,10), new MoveAction("SSW", 15, -1,2,hexDiagCost,11)};
       
         public static MoveAction[] aaFour = { new MoveAction("S", 0, 0, 1, 1,2), new MoveAction("E", 1, 1, 0, 1,3), new MoveAction("N", 2, 0, -1, 1,0), new MoveAction("W", 3, -1, 0, 1,1), };
        */
        public static uint GridWidth;

        public static ushort Radius = 1;

        public static uint NoSwamp = uint.MaxValue;

        public static uint height, width;


        public static int debugCounter = 0;

        public static uint expandStep = 1000;

        public static short expandState = 1;

        public static short generateStates = 2;
        /*
         public static string[] allSolvers = {"Depth First Search","Bredth First Search","Uniform Cost Search",
                                                 "Pure Heuristic Search","A*","LSS-LRTA*","EDA*","FLRTA*","IDA*","ATPHS", "RTA*"};
    
         */
    }
}
