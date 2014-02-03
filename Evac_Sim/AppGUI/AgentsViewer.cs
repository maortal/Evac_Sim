using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Evac_Sim.AgentsLogic;
using Evac_Sim.WorldMap;

namespace Evac_Sim.AppGUI
{
    public partial class AgentsViewer : Form
    {
        private MainForm main;

        public AgentsViewer(MainForm main)
        {
            this.main = main;
            InitializeComponent();
        }


        private void AgentsViewer_Load(object sender, EventArgs e)
        {
            foreach (Agent agn in main.AgentsList.Values.Where(agn => !agentBindingSource.Contains(agn)))
            {
                agentBindingSource.Add(agn);
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            main.selectiveSolution();
        }
        private void agentBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            Agent[] tmpagent = main.AgentsList.Values.ToArray();
            for (int i = 0; i < tmpagent.Length; i++)
                if (agentBindingSource.Count > i)
                {
                    dataGridView1.Rows[i].HeaderCell.Value = (i+1).ToString();
                    dataGridView1[1, i].Style.BackColor = tmpagent[i].GetAgColor();
                }
            dataGridView1.AutoResizeRowHeadersWidth(tmpagent.Length-1, DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
       
    }
}
