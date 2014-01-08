using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.WorldMap;

namespace Evac_Sim.AppGUI
{
        static class Utils
        {
            private static bool pause = false;

            public static bool cancel = false;

            public static int speed = 210;

            public static uint fastForword = 1;

            public static void slower()
            {
                if (fastForword > 1)
                    fastForword -= 2;
                else
                    speed += 20;
            }

            public static void faster()
            {
                if (speed > 10)
                    speed -= 20;
                else
                    fastForword += 2;
            }

            private static List<State> expandedStates = new List<State>();

            private static List<State> generatedStates = new List<State>();

            public static Graphics paper;

            public static MapDrawing md;

            public static bool indexing;

            public static bool fastSwampDetection = true;

            private static uint drawCounter = 0;

            public static void DrawExpandState(MainForm fm, State s, bool goal)
            {
                if (fm == null || cancel)
                    return;
                if (pause)
                    System.Threading.Thread.Sleep(100);
                Utils.md.PaintRec(Utils.paper, s.Index, Color.Red, indexing);
                drawCounter++;
                if (drawCounter % fastForword == 0 || goal)
                {
                    fm.BackgroundWorker.ReportProgress(1);
                    System.Threading.Thread.Sleep(speed);
                    drawCounter = 0;
                }

            }

            public static void DrawGenerateStates(MainForm fm, List<State> ls)
            {
                if (fm == null || cancel)
                    return;
                if (pause)
                    System.Threading.Thread.Sleep(100);
                foreach (State s in ls)
                {
                    md.PaintRec(paper, s.Index, Color.Yellow, indexing);
                }
            }

            public static void DrawLine(Point from, Point to)
            {
                md.DrawLine(paper, from, to);
            }

            public static void ReDraw()
            {
                md.DrawMap(paper, indexing);
            }

            public static State setPoint(Point cursorPos)
            {
                return md.PaintRec(paper, cursorPos, Color.Blue, indexing);
            }
            public static State getState(Point cursorPos)
            {
                return md.getState(paper, cursorPos, Color.Blue, indexing);
            }

            public static void zoom(bool closer, ref Bitmap bm)
            {
                pause = true;
                if (closer)
                    paper = md.zoomIn(ref bm, indexing);
                else
                    paper = md.zoomOut(ref bm, indexing);
                pause = false;
            }

            public static void drawSolution(List<State> sol)
            {
                if (sol != null)
                {
                    if (pause)
                        System.Threading.Thread.Sleep(100);
                    ReDraw();
                    State prevV = null;
                    foreach (State v in sol)
                    {
                        md.PaintRec(paper, ((State)v).Index, Color.Blue, indexing);

                        if (prevV != null)
                        {
                            //if (Graph.HairDistance(v, prevV) > 2)
                           // {
                          //      md.DrawLine(paper, prevV.Index, v.Index);
                          //  }
                        }
                        prevV = v;
                    }
                }

            }

            public static void drawOpenList(State[] open)
            {
                if (cancel)
                    return;
                for (int i = 0; i < open.Length; i++)
                {
                    if (open[i] == null)
                        return;
                    if (pause)
                        System.Threading.Thread.Sleep(100);
                    md.PaintRec(paper, open[i].Index, Color.Chartreuse, indexing);
                }
            }

            public static void drawCurrentLRTA(MainForm fm, State s, bool goal)
            {
                if (cancel)
                    return;
                if (s == null)
                    return;
                if (fm == null)
                    return;
                if (pause)
                    System.Threading.Thread.Sleep(100);
                Utils.md.PaintRec(Utils.paper, s.Index, Color.LightSalmon, false);
                if (Utils.indexing)
                    Utils.md.printIndex(Utils.paper, s.Index);
                drawCounter++;
                if (drawCounter % fastForword == 0 || goal)
                {
                    fm.BackgroundWorker.ReportProgress(1);
                    System.Threading.Thread.Sleep(speed);
                    drawCounter = 0;
                }
            }

            public static void updateValLRTA(State s, double d)
            {
                if (cancel)
                    return;
                if (paper == null)
                    return;
                int hue = 255 - (int)d / 2;
                if (hue < 100 || hue > 255)
                    hue = 100;
                if (pause)
                    System.Threading.Thread.Sleep(100);
                md.PaintRec(Utils.paper, s.Index, Color.FromArgb(hue, 0, 0), false);
                md.PaintRec(Utils.paper, s.Index, Color.Red, false);
                if (indexing)
                    md.printIndex(paper, s.Index);
            }

            public static void paintDeadState_State(State s)
            {
                if (cancel)
                    return;
                if (md == null)
                    return;
                md.PaintRec(paper, s.Index, Color.Gray, indexing);
            }

            public static void fillState(State s, Color color)
            {
                if (cancel)
                    return;
                if (md == null)
                    return;
                md.PaintRec(paper, s.Index, color, indexing);
            }

            public static void paintDeadState_gDead(State s)
            {
                if (cancel)
                    return;
                if (md == null)
                    return;
                md.PaintRec(paper, s.Index, Color.LightSteelBlue, indexing);
            }

            public static void paintDeadState_Special(State s)
            {
                if (cancel)
                    return;
                if (md == null)
                    return;
                md.PaintRec(paper, s.Index, Color.DarkOrange, indexing);
            }

            public static void clearMap(MainForm fm)
            {
                if (fm == null)
                    return;
                paper.Clear(Color.Black);
                md.DrawMap(Utils.paper, Utils.indexing);
                fm.BackgroundWorker.ReportProgress(1);
            }

            public static void SetIndexing()
            {
                indexing = !indexing;
                md.reIndex(paper, indexing);
            }

            public static State GetBestLS(ref LinkedList<State> open)
            {
                LinkedListNode<State> best = null;
                double bestValue = double.MaxValue;
                LinkedListNode<State> llns = open.First;
                while (llns != null)
                {
                    if (llns.Value.LocalSearchGCostWith < bestValue)
                    {
                        bestValue = llns.Value.LocalSearchGCostWith;
                        best = llns;
                    }
                    else if (llns.Value.LocalSearchGCostWith == bestValue)
                    {
                        if (llns.Value.LocalSearchGCostWithOut == bestValue)
                            bestValue = llns.Value.LocalSearchGCostWith;
                    }
                    llns = llns.Next;
                }
                open.Remove(best);
                return best.Value;
            }

            public static double GetMaxEdgeCost(ActionMoves[] aMa)
            {
                double max = 0;
                for (int i = 0; i < aMa.Length; i++)
                {
                    if (aMa[i].Cost > max)
                        max = aMa[i].Cost;
                }
                return max;
            }
        }
    }
