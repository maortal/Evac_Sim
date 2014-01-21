namespace Evac_Sim.AppGUI
{
    partial class AgentsViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.visible = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.agenColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.indexDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalExpandDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalGeneratedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.solCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.agentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agentBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.visible,
            this.agenColor,
            this.indexDataGridViewTextBoxColumn,
            this.totalExpandDataGridViewTextBoxColumn,
            this.totalGeneratedDataGridViewTextBoxColumn,
            this.solCostDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.agentBindingSource;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(544, 241);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // visible
            // 
            this.visible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.visible.DataPropertyName = "visible";
            this.visible.HeaderText = "Visible";
            this.visible.Name = "visible";
            this.visible.Width = 55;
            // 
            // agenColor
            // 
            this.agenColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.agenColor.DataPropertyName = "agenColor";
            this.agenColor.HeaderText = "Color";
            this.agenColor.Name = "agenColor";
            this.agenColor.ReadOnly = true;
            this.agenColor.Width = 66;
            // 
            // indexDataGridViewTextBoxColumn
            // 
            this.indexDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.indexDataGridViewTextBoxColumn.DataPropertyName = "Index";
            this.indexDataGridViewTextBoxColumn.HeaderText = "Index";
            this.indexDataGridViewTextBoxColumn.Name = "indexDataGridViewTextBoxColumn";
            this.indexDataGridViewTextBoxColumn.ReadOnly = true;
            this.indexDataGridViewTextBoxColumn.Width = 66;
            // 
            // totalExpandDataGridViewTextBoxColumn
            // 
            this.totalExpandDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.totalExpandDataGridViewTextBoxColumn.DataPropertyName = "totalExpand";
            this.totalExpandDataGridViewTextBoxColumn.HeaderText = "totalExpand";
            this.totalExpandDataGridViewTextBoxColumn.Name = "totalExpandDataGridViewTextBoxColumn";
            this.totalExpandDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalExpandDataGridViewTextBoxColumn.Width = 107;
            // 
            // totalGeneratedDataGridViewTextBoxColumn
            // 
            this.totalGeneratedDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.totalGeneratedDataGridViewTextBoxColumn.DataPropertyName = "totalGenerated";
            this.totalGeneratedDataGridViewTextBoxColumn.HeaderText = "totalGenerated";
            this.totalGeneratedDataGridViewTextBoxColumn.Name = "totalGeneratedDataGridViewTextBoxColumn";
            this.totalGeneratedDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalGeneratedDataGridViewTextBoxColumn.Width = 128;
            // 
            // solCostDataGridViewTextBoxColumn
            // 
            this.solCostDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.solCostDataGridViewTextBoxColumn.DataPropertyName = "solCost";
            this.solCostDataGridViewTextBoxColumn.HeaderText = "solCost";
            this.solCostDataGridViewTextBoxColumn.Name = "solCostDataGridViewTextBoxColumn";
            this.solCostDataGridViewTextBoxColumn.ReadOnly = true;
            this.solCostDataGridViewTextBoxColumn.Width = 79;
            // 
            // agentBindingSource
            // 
            this.agentBindingSource.DataSource = typeof(Evac_Sim.AgentsLogic.Agent);
            this.agentBindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.agentBindingSource_ListChanged);
            // 
            // AgentsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(568, 265);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AgentsViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Agents List";
            this.Load += new System.EventHandler(this.AgentsViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agentBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.BindingSource agentBindingSource;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn visible;
        private System.Windows.Forms.DataGridViewTextBoxColumn agenColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn indexDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalExpandDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalGeneratedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn solCostDataGridViewTextBoxColumn;
    }
}