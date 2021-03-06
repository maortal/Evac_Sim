﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Evac_Sim.AgentsLogic;
using Evac_Sim.SearchAlgorithems.Heuristics;
using Evac_Sim.WorldMap;
using Evac_Sim.SearchAlgorithems;


namespace Evac_Sim.AppGUI
{
    public partial class MainForm : Form
    {
        private TextWriter _expFile;
        private Bitmap bm;
        private State start, goal;
        private Graph gr;
        public ActionMoves[] aa = Constants.OctileMoves;
        private bool moveMap, drawAgent, drawExit;
        private Point from;
        private int curx, cury;
        private string filenamekeeper;
        private Random rngn = new Random();
        private Color curragentCol;
        public Dictionary<State, Agent> AgentsList = new Dictionary<State, Agent>();
        public HashSet<State> Goals = new HashSet<State>();
        protected AgentsViewer agentsView;
        public static bool DMLearnMode = false;
        public static bool ExperimentMODE = false;
        public int numberofAgents { get; set; }
        public int nIteration { get; set; }
        public bool AstarTurn { get; set; }
        public bool DMTurn { get; set; }

        public int ExpnumAgents { get; set; }
        public int ExpnumIters { get; set; }


        public MainForm()
        {
            InitializeComponent();
            getNextRandCol();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (gr == null)
                return;
            if (e.Button == MouseButtons.Left)
            {
                moveMap = true;
                pictureBox1.Focus();
                curx = e.X;
                cury = e.Y;
                /*if (solution != null)
                {
                    Utils.drawSolution(solution);
                    Draw();
                    solution = null;
                }*/
            }
            if (e.Button == MouseButtons.Right)
            {
                State fillpoint = Utils.getState(e.Location);
                if (fillpoint != null)
                {
                    toolStripButton5.Enabled = false;
                    SetAgentorGoal(fillpoint);
                    if (Goals.Count > 0)
                    {
                        learnToolStripMenuItem.Enabled = true;
                        experStripButton3.Enabled = true;
                        if (AgentsList.Count > 0) selfishAstarToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        experStripButton3.Enabled = false;
                        learnToolStripMenuItem.Enabled = false;
                        selfishAstarToolStripMenuItem.Enabled = false;
                    }
                    Draw();
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            moveMap = false;
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Map File|*.map";
            ofd.Title = "Open map";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TextReader mapFile = new StreamReader(ofd.FileName);
                filenamekeeper = ofd.FileName;
                Graph g = new Graph(mapFile, aa); //CONSTANTS.aaOctile
                mapFile.Close();
                g.Height = Constants.height;
                g.Width = Constants.width;

                loadMap(g, ofd.SafeFileName);
                string tmp = filenamekeeper.Replace(Path.GetFileName(filenamekeeper),
                    "DM-" + Path.GetFileName(filenamekeeper));
                if (File.Exists(Path.ChangeExtension(tmp, ".txt"))) importFromFileToolStripMenuItem.Enabled = true;
                else importFromFileToolStripMenuItem.Enabled = false;
                learnToolStripMenuItem.Enabled = false;

            }
        }

        public void loadMap(Graph gr, string mapName)
        {

            this.gr = gr;

            Utils.indexing = false;
            start = null;
            goal = null;
            Utils.md = new MapDrawing(gr);
            pictureBox1.Size = new Size((int)gr.Width * Utils.md.size + 20, (int)gr.Height * Utils.md.size + 40);
            bm = new Bitmap((int)gr.Width * Utils.md.size + 20, (int)gr.Height * Utils.md.size + 40);
            Utils.paper = Graphics.FromImage(bm);
            Utils.md.DrawMap(Utils.paper, Utils.indexing);
            Draw();
            this.Text = mapName;
            ResetMap();
        }

        private void Draw()
        {
            pictureBox1.Image = bm;
            pictureBox1.Refresh();
            if (AgentsList.Count > 0)
            {
                if (agentsView == null || !agentsView.Visible)
                {
                    agentsView = new AgentsViewer(this);
                    agentsView.Show();
                }
                else
                    agentsView.Update();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moveMap)
            {
                int newx = pictureBox1.Location.X + (e.X - curx);
                int newy = pictureBox1.Location.Y + (e.Y - cury);
                pictureBox1.Location = new Point(newx, newy);
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            switch (e.Delta)
            {
                case 120:
                    Utils.zoom(true, ref bm);
                    pictureBox1.Size = bm.Size;
                    Draw();
                    break;

                case -120:
                    Utils.zoom(false, ref bm);
                    pictureBox1.Size = bm.Size;
                    Draw();
                    break;
            }
        }

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (gr == null)
                return;
            drawExit = true;
            drawAgent = false;
            pictureBox3.BackColor = Color.Yellow;
            pictureBox2.BackColor = Color.GhostWhite;
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (gr == null)
                return;
            drawExit = false;
            drawAgent = true;
            pictureBox3.BackColor = Color.GhostWhite;
            pictureBox2.BackColor = curragentCol;
        }

        private Color getNextRandCol()
        {
            Color prevColor = curragentCol;
            curragentCol = Color.FromArgb(rngn.Next(0, 255), rngn.Next(0, 255), rngn.Next(0, 255));
            if (drawAgent)
                pictureBox2.BackColor = curragentCol;
            return prevColor;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            backgroundWorker2.CancelAsync();
            if (!backgroundWorker2.IsBusy)
            {
                clearMap();
                backgroundWorker2.RunWorkerAsync();
            }
        }

        public void selectiveSolution()
        {
            Utils.ReDraw();
            foreach (Agent agen in AgentsList.Values.Where(agen => agen.visible))
                if (agen.Agentsolution != null) Utils.drawSolution(agen.Agentsolution, agen.GetAgColor());
                else Utils.fillState(agen.Index, agen.agenColor); //will still run agent search
            Draw();
        }

        private void ResetMap()
        {
            AgentsList = new Dictionary<State, Agent>();
            if (DMLearnMode == false && ExperimentMODE == false) Goals = new HashSet<State>();
            if (agentsView != null) agentsView.Close();
            if (gr != null)
            {
                gr.reset();
                clearMap();
                if (DMLearnMode)
                    foreach (State stgoal in Goals)
                        Utils.fillState(stgoal, Color.Yellow);
                Draw();
                selfishAstarToolStripMenuItem.Enabled = false;
                toolStripButton5.Enabled = false;
            }
        }

        private void clearMap()
        {
            Utils.paper.Clear(Color.Black);
            Utils.md.DrawMap(Utils.paper, Utils.indexing);
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!ExperimentMODE)
            {
                gr.reset();
                System.Threading.Thread.Sleep(100);
                foreach (State goal in Goals)
                    Utils.fillState(goal, Color.Yellow);
                foreach (KeyValuePair<State, Agent> agent in AgentsList)
                    Utils.fillState(agent.Key, agent.Value.agenColor);
                Draw();
            }
        }

        private void selfishAstarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gr == null || Goals.Count == 0 || AgentsList.Count == 0) return;
            if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
            Draw();
        }

        private void SetAgentorGoal(State fillpoint, SearchAlgo searchalg = null)
        {
            if (drawAgent && !Goals.Contains(fillpoint))
            {
                if (AgentsList.ContainsKey(fillpoint))
                {
                    Agent tmp = AgentsList[fillpoint];
                    AgentsList.Remove(fillpoint);
                    agentsView.agentBindingSource.Remove(tmp);
                    Utils.fillState(fillpoint, Color.White);
                }
                else
                {
                    if (agentsView == null || !agentsView.Visible)
                    {
                        agentsView = new AgentsViewer(this);
                        Rectangle workingArea = Screen.GetWorkingArea(this);
                        agentsView.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
                        agentsView.Show();
                    }
                    if (searchalg == null) searchalg = new Astar(new OctileHeur(), this);
                    AgentsList[fillpoint] = new Agent(fillpoint, curragentCol, searchalg);
                    agentsView.agentBindingSource.Add(AgentsList[fillpoint]);
                    Utils.fillState(fillpoint, getNextRandCol());
                }

            }
            if (drawExit && !AgentsList.ContainsKey(fillpoint))
            {

                if (!Goals.Contains(fillpoint))
                {
                    Goals.Add(fillpoint);
                    Utils.fillState(fillpoint, Color.Yellow);
                }
                else
                {
                    Goals.Remove(fillpoint);
                    Utils.fillState(fillpoint, Color.White);
                }
            }
        }

        private void PlaceRandomAgents(int numberofAgents, SearchAlgo searchalgo)
        {
            pictureBox3.BackColor = Color.GhostWhite;
            drawExit = false;
            drawAgent = true;
            for (int i = 0; i < numberofAgents; i++)
            {
                int ridx;
                do
                {
                    ridx = rngn.Next(gr.GraphMap.Length);
                } while (AgentsList.ContainsKey(gr.GraphMap[ridx]) || Goals.Contains(gr.GraphMap[ridx]));
                SetAgentorGoal(gr.GraphMap[ridx], searchalgo);
            }
        }

        private void exportToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tmp = filenamekeeper.Replace(Path.GetFileName(filenamekeeper),
                "DM-" + Path.GetFileName(filenamekeeper));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.ChangeExtension(tmp, ".txt")))
            {
                foreach (State st in gr.GraphMap)
                {
                    file.WriteLine(st.Index + ", " + st.DVx.ToString() + ", " + st.DVy.ToString());
                }
                file.WriteLine();
                foreach (State state in Goals)
                {
                    file.WriteLine(state.Index.ToString());
                }
            }
        }

        private void importFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tmp = filenamekeeper.Replace(Path.GetFileName(filenamekeeper),
                "DM-" + Path.GetFileName(filenamekeeper));
            using (System.IO.StreamReader file = new System.IO.StreamReader(Path.ChangeExtension(tmp, ".txt")))
            {
                string line;
                while ((line = file.ReadLine()) != "")
                {
                    var numbers = line.Split(',').Select(double.Parse).ToList();
                    gr.GraphMap[(int)numbers[0]].DVx = numbers[1];
                    gr.GraphMap[(int)numbers[0]].DVy = numbers[2];
                }
                while ((line = file.ReadLine()) != null)
                {
                    int idx = int.Parse(line);
                    State tmpstate = gr.GraphMap[idx];
                    Goals.Add(tmpstate);
                    Utils.fillState(tmpstate, Color.Yellow);
                }
            }
            if (Goals.Count>0 ) experStripButton3.Enabled = true;
            Draw(); 
        }

        private void learnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            numberofAgents = 0;
            nIteration = 0;
            Form Learnprops = new LearnerProperties(this, gr.GraphMap.Length - Goals.Count);
            Learnprops.ShowDialog();
            if (nIteration != 0 && numberofAgents != 0)
            {
                DMLearnMode = true;
                executeIterations();
            }
        }

        private void executeIterations()
        {
            if (nIteration > 0)
            {
                PlaceRandomAgents(numberofAgents, new DM_Astar(new OctileHeur(), this));
                if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
                Draw();
            }
            else
            {
                exportToFileToolStripMenuItem.Enabled = true;
                DMLearnMode = false;
            }
        }

        private void experStripButton3_Click(object sender, EventArgs e)
        {
            numberofAgents = 0;
            nIteration = 0;
            Form Learnprops = new LearnerProperties(this, gr.GraphMap.Length - Goals.Count);
            Learnprops.Text = "Experiment Properties";
            Learnprops.ShowDialog();
            
            ExpnumAgents = numberofAgents;
            ExpnumIters = nIteration;
            numberofAgents = 0;
            nIteration = 0;
            if (ExpnumIters != 0 && ExpnumAgents != 0)
            {
                setNewExperiment();
            }
        }

        
        void setNewExperiment()
        {
            ExpnumIters--;
            ExperimentMODE = true;
            ResetMap();
            _expFile = new StreamWriter(Path.GetFileNameWithoutExtension(filenamekeeper) + "-experimentResults.txt", true);
            _expFile.WriteLine();
            _expFile.WriteLine(Path.GetFileName(filenamekeeper) + "\t" + ExpnumAgents);
            _expFile.WriteLine("-------------------");
            PlaceRandomAgents(ExpnumAgents, new Astar(new OctileHeur(), this));
            AstarTurn = true;
            if (!backgroundWorker3.IsBusy) backgroundWorker3.RunWorkerAsync();
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("States Expanded = " + solver.getExpanded() + "\nStates Generated = " + solver.getGenerated() + "\nTotal Distance Traveld = " + solver.getSolutionCost());
            //Utils.ReDraw();
            if (DMTurn == false)
            {
                clearMap();
                _expFile.WriteLine("A* Results!");
                SimulationHandler simulate = new SimulationHandler(AgentsList.Values,this);
                string result = simulate.BeginSimulation();
                Dictionary<State, Agent> dmAgentList = new Dictionary<State, Agent>();
                foreach (Agent agen in AgentsList.Values)
                {
                   // _expFile.WriteLine(agen.ToExcel() + "\t" + agen.Agentsolution.ToString());
                    Agent tmp = new Agent(agen.Index, agen.agenColor, new DM_Astar(new OctileHeur(), this));
                    tmp.GoalState = gr.GraphMap[agen.GoalState.Index];
                    dmAgentList.Add(agen.Index, tmp);
                }
                _expFile.WriteLine(result);
                _expFile.WriteLine();
                _expFile.WriteLine("DM Results");
              AgentsList = new Dictionary<State, Agent>(dmAgentList);
               // Draw();
                agentsView.dataGridView1.Refresh();

                AstarTurn = false;
                DMTurn = true;
                if (!backgroundWorker3.IsBusy) backgroundWorker3.RunWorkerAsync();
                return;
            }
            if (DMTurn)
            {
                //Utils.ReDraw();
                clearMap();
                foreach (Agent agen in AgentsList.Values)
                    agen.Agentsolution.Reverse();
                SimulationHandler simulate = new SimulationHandler(AgentsList.Values, this);
                string result = simulate.BeginSimulation();
                foreach (Agent agen in AgentsList.Values)
                {
                  //  _expFile.WriteLine(agen.ToExcel() + "\t" + agen.Agentsolution.ToString());
                    agen.Agentsolution.Reverse();
                    Utils.drawSolution(agen.Agentsolution, agen.GetAgColor());
                }
                _expFile.WriteLine(result);
                Draw();
                agentsView.dataGridView1.Refresh();
                DMTurn = false;
                toolStripButton5.Enabled = true;
                _expFile.Close();
            }
            if (ExpnumIters>0) setNewExperiment();
            else ExperimentMODE = false;
        }

        


        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            Utils.cancel = false;
            if (AstarTurn)
            {
                try
                {
                    foreach (Agent agen in AgentsList.Values)
                    {
                        gr.reset();
                        agen.Solve(Goals);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            if (DMTurn)
            {
                try
                {
                    foreach (Agent agen in AgentsList.Values)
                    {
                        gr.reset();
                        agen.Solve(agen.GoalState);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
