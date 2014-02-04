using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Evac_Sim.AppGUI
{
    public partial class LearnerProperties : Form
    {
        private MainForm mainForm;
        private int numberofstates;
        public LearnerProperties(MainForm mfrm, int nstates)
        {
            this.mainForm = mfrm;
            this.numberofstates = nstates;
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int tmp = (int) (trackBar1.Value*(numberofstates/100.0));
            label3.Text = trackBar1.Value.ToString() + "%\t[" + tmp.ToString() + "]";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar2.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainForm.nIteration = trackBar2.Value;
            mainForm.numberofAgents = (int)(trackBar1.Value * (numberofstates / 100.0));
            this.Close();
        }


    }
}
