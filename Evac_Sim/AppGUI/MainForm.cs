using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Evac_Sim.AgentsLogic;
using Evac_Sim.WorldMap;
using Evac_Sim.SearchAlgorithems;


namespace Evac_Sim.AppGUI
{
    public partial class MainForm : Form
    {
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
                                agentsView.Location = new Point(workingArea.Right - Size.Width,workingArea.Bottom - Size.Height);
                                agentsView.Show();
                            }
                            AgentsList[fillpoint] = new Agent(fillpoint, curragentCol,new Astar(new OctileHeur(), this));
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
                    if (Goals.Count > 0 && AgentsList.Count > 0) toolStripButton4.Enabled = true;
                    else toolStripButton4.Enabled = false;
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
            }
        }

        public void loadMap(Graph gr, string mapName)
        {

            this.gr = gr;

            Utils.indexing = false;
            start = null;
            goal = null;
            Utils.md = new MapDrawing(gr);
            pictureBox1.Size = new Size((int) gr.Width*Utils.md.size + 20, (int) gr.Height*Utils.md.size + 40);
            bm = new Bitmap((int) gr.Width*Utils.md.size + 20, (int) gr.Height*Utils.md.size + 40);
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
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (gr == null || Goals.Count == 0 || AgentsList.Count == 0) return;
            if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
            Draw();
        }

        public void selectiveSolution()
        {
            Utils.ReDraw();
            foreach (Agent agen in AgentsList.Values.Where(agen => agen.visible))
                Utils.drawSolution(agen.Agentsolution, agen.GetAgColor());
            Draw();
        }
        private void ResetMap()
        {
            AgentsList = new Dictionary<State, Agent>(); 
            Goals = new HashSet<State>();
            if (agentsView!=null) agentsView.Close();
            if (gr != null)
            {
                gr.reset();
                clearMap();
                Draw();
                toolStripButton4.Enabled = false;
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
            gr.reset();
            System.Threading.Thread.Sleep(100);
            foreach (State goal in Goals)
                Utils.fillState(goal, Color.Yellow);
            foreach (KeyValuePair<State, Agent> agent in AgentsList)
                Utils.fillState(agent.Key, agent.Value.agenColor);
            Draw();
        }
    }
}
