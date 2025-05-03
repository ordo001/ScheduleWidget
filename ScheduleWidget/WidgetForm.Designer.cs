namespace ScheduleWidget
{
    partial class WidgetForm
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
            lbMain = new Label();
            SuspendLayout();
            // 
            // lbMain
            // 
            lbMain.AutoSize = true;
            lbMain.BackColor = Color.FromArgb(50, 0, 0, 0);
            lbMain.Dock = DockStyle.Fill;
            lbMain.Font = new Font("Lucida Console", 10F);
            lbMain.ForeColor = Color.White;
            lbMain.Location = new Point(0, 0);
            lbMain.Margin = new Padding(4, 0, 4, 0);
            lbMain.Name = "lbMain";
            lbMain.Size = new Size(0, 14);
            lbMain.TabIndex = 1;
            lbMain.TextAlign = ContentAlignment.MiddleCenter;
            lbMain.MouseMove += lbMain_MouseMove;
            // 
            // WidgetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.Black;
            ClientSize = new Size(419, 74);
            Controls.Add(lbMain);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "WidgetForm";
            ShowInTaskbar = false;
            Text = "MainForm";
            Load += WidgetForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label lbMain;
    }
}

