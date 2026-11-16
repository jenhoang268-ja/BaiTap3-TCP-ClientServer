namespace UserManagement
{
    partial class MainForm
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
            panel1 = new Panel();
            label1 = new Label();
            btnMinimize = new Button();
            btnMaximize = new Button();
            btnExit = new Button();
            panelCenter = new Panel();
            lblLoginTime = new Label();
            lblEmail = new Label();
            lblUsername = new Label();
            lblWelcome = new Label();
            btnLogout = new Button();
            panel1.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Peru;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnMinimize);
            panel1.Controls.Add(btnMaximize);
            panel1.Controls.Add(btnExit);
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1194, 43);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label1.Location = new Point(517, 7);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(133, 25);
            label1.TabIndex = 1;
            label1.Text = "Trang chính";
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.BackColor = Color.Peru;
            btnMinimize.Location = new Point(1017, 0);
            btnMinimize.Margin = new Padding(4);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(60, 43);
            btnMinimize.TabIndex = 3;
            btnMinimize.TabStop = false;
            btnMinimize.Text = "-";
            btnMinimize.UseVisualStyleBackColor = false;
            btnMinimize.Click += btnMinimize_Click_1;
            // 
            // btnMaximize
            // 
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximize.BackColor = Color.Peru;
            btnMaximize.Location = new Point(1076, 0);
            btnMaximize.Margin = new Padding(4);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(60, 43);
            btnMaximize.TabIndex = 2;
            btnMaximize.TabStop = false;
            btnMaximize.Text = "❐";
            btnMaximize.UseVisualStyleBackColor = false;
            btnMaximize.Click += btnMaximize_Click_1;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExit.BackColor = Color.Peru;
            btnExit.Location = new Point(1134, 0);
            btnExit.Margin = new Padding(4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(60, 43);
            btnExit.TabIndex = 1;
            btnExit.TabStop = false;
            btnExit.Text = "X";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click_1;
            // 
            // panelCenter
            // 
            panelCenter.Anchor = AnchorStyles.None;
            panelCenter.BackColor = Color.SaddleBrown;
            panelCenter.Controls.Add(lblLoginTime);
            panelCenter.Controls.Add(lblEmail);
            panelCenter.Controls.Add(lblUsername);
            panelCenter.Controls.Add(lblWelcome);
            panelCenter.Controls.Add(btnLogout);
            panelCenter.Location = new Point(217, 105);
            panelCenter.Margin = new Padding(4);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(760, 370);
            panelCenter.TabIndex = 1;
            // 
            // lblLoginTime
            // 
            lblLoginTime.Font = new Font("Cascadia Code", 13F, FontStyle.Bold);
            lblLoginTime.ForeColor = Color.White;
            lblLoginTime.Location = new Point(30, 210);
            lblLoginTime.Name = "lblLoginTime";
            lblLoginTime.Size = new Size(700, 55);
            lblLoginTime.TabIndex = 8;
            lblLoginTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEmail
            // 
            lblEmail.Font = new Font("Cascadia Code", 13F, FontStyle.Bold);
            lblEmail.ForeColor = Color.White;
            lblEmail.Location = new Point(30, 150);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(700, 45);
            lblEmail.TabIndex = 7;
            lblEmail.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblUsername
            // 
            lblUsername.Font = new Font("Cascadia Code", 13F, FontStyle.Bold);
            lblUsername.ForeColor = Color.White;
            lblUsername.Location = new Point(30, 95);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(700, 45);
            lblUsername.TabIndex = 6;
            lblUsername.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblWelcome
            // 
            lblWelcome.Font = new Font("Cascadia Code", 13F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.BlanchedAlmond;
            lblWelcome.Location = new Point(30, 25);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(700, 55);
            lblWelcome.TabIndex = 5;
            lblWelcome.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(243, 178, 97);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            btnLogout.ForeColor = Color.SaddleBrown;
            btnLogout.Location = new Point(490, 290);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(240, 55);
            btnLogout.TabIndex = 0;
            btnLogout.Text = "Đăng xuất";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click_1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Gemini_Generated_Image_w6rdwow6rdwow6rd1;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1194, 580);
            Controls.Add(panelCenter);
            Controls.Add(panel1);
            DoubleBuffered = true;
            Font = new Font("Cascadia Code", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.SaddleBrown;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MainForm";
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelCenter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnMinimize;
        private Button btnMaximize;
        private Button btnExit;
        private Label label1;
        private Panel panelCenter;
        private Button btnLogout;
        private Label lblWelcome;
        private Label lblLoginTime;
        private Label lblEmail;
        private Label lblUsername;
    }
}