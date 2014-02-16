using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Evac_Sim.AgentsLogic;
using Evac_Sim.WorldMap;

namespace Evac_Sim.AppGUI
{
        static class Utils
        {
            private static bool pause = false;

            public static bool cancel = false;

            public static int speed = 0;

            public static uint fastForword = 10;

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
                if (fm == null || cancel || MainForm.ExperimentMODE || MainForm.DMLearnMode)
                    return;
                if (pause)
                    System.Threading.Thread.Sleep(100);
                Utils.md.PaintRec(Utils.paper, s.Index, Color.Red, indexing);
                drawCounter++;
                if (drawCounter % fastForword == 0 || goal)
                {
                    fm.backgroundWorker1.ReportProgress(1);
                    System.Threading.Thread.Sleep(speed);
                    drawCounter = 0;
                }

            }

            public static void DrawGenerateStates(MainForm fm, List<State> ls)
            {
                    if (fm == null || cancel || MainForm.ExperimentMODE || MainForm.DMLearnMode)
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

            public static void drawSolution(SolPath sol, Color pathcolor)
            {
                if (sol != null)
                {
                    if (pause)
                        System.Threading.Thread.Sleep(100);
                    //ReDraw();
                    foreach (State v in sol)
                    {
                        md.PaintRec(paper, ((State) v).Index, pathcolor, indexing);
                        //md.DrawVector(paper,v.Index,v.DVx,v.DVy);
                    }
                    md.PaintEllipse(paper, ((State)sol.Solgoal()).Index, Color.Yellow);
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
            

            public static void paintDeadState_State(State s)
            {
                if (cancel)
                    return;
                if (md == null)
                    return;
                md.PaintRec(paper, s.Index, Color.Gray, indexing);
            }

            public static bool isStateFree(State state)
            {
                return md.IsStateFree(state.Index);
            }
            public static void fillState(State s, Color color)
            {
                if (cancel)
                    return;
                if (md == null)
                    return;
                md.PaintRec(paper, s.Index, color, indexing);
            }

            public static void drawvector(State s)
            {
                if (cancel)
                    return;
                if (md == null)
                    return;
                md.DrawVector(paper, s.Index, s.DVx,s.DVy);
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
                fm.backgroundWorker1.ReportProgress(1);
            }

            public static void SetIndexing()
            {
                indexing = !indexing;
                md.reIndex(paper, indexing);
            }
            
        }
    }
