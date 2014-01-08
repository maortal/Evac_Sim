using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Evac_Sim.WorldMap;


namespace Evac_Sim.AppGUI
{
    public partial class MainForm : Form
    {
        private Bitmap bm;
        private State start, goal;
        private Graph gr;
        private List<State> solution;
        public ActionMoves[] aa = Constants.OctileMoves;
        private bool moveMap, setProb, drawAgent, drawExit;
        private Point from;
        private int curx, cury;
        private string filenamekeeper;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
                if (solution != null)
                {
                    Utils.drawSolution(solution);
                    Draw();
                    solution = null;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
               State fillpoint = Utils.getState(e.Location);
               if (fillpoint!=null)
                {
                    if (drawAgent)
                    {
                        Utils.fillState(fillpoint, Color.Red);
                    }
                    if (drawExit)
                    {
                        Utils.fillState(fillpoint,Color.Yellow);
                    }
                    Draw();
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            moveMap = false;
            setProb = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // GridHandler.PaintGrid(e);
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
            solution = null;
        }

        private void Draw()
        {
            pictureBox1.Image = bm;
            pictureBox1.Refresh();
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
            pictureBox2.BackColor = Color.Red; 
        }
    }
}
