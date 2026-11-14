namespace GameServer
{
    partial class ServerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblGS = new Label();
            grbDK = new GroupBox();
            btnStop = new Button();
            btnStart = new Button();
            btnClearlog = new Button();
            grbTT = new GroupBox();
            lblUpTime = new Label();
            lblClientCount = new Label();
            lblPort = new Label();
            lblStatus = new Label();
            lblThoigian = new Label();
            lblC = new Label();
            lblCong = new Label();
            lblTT = new Label();
            grbClients = new GroupBox();
            dgvClients = new DataGridView();
            grbLog = new GroupBox();
            txtLog = new TextBox();
            grbDK.SuspendLayout();
            grbTT.SuspendLayout();
            grbClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClients).BeginInit();
            grbLog.SuspendLayout();
            SuspendLayout();
            // 
            // lblGS
            // 
            lblGS.AutoSize = true;
            lblGS.BackColor = Color.FromArgb(255, 240, 214);
            lblGS.Font = new Font("Times New Roman", 19.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGS.ForeColor = Color.FromArgb(135, 60, 10);
            lblGS.Location = new Point(560, 27);
            lblGS.Margin = new Padding(4, 0, 4, 0);
            lblGS.Name = "lblGS";
            lblGS.Size = new Size(415, 61);
            lblGS.TabIndex = 0;
            lblGS.Text = "GAME SERVER";
            // 
            // grbDK
            // 
            grbDK.BackColor = Color.FromArgb(255, 240, 214);
            grbDK.Controls.Add(btnStop);
            grbDK.Controls.Add(btnStart);
            grbDK.Font = new Font("Times New Roman", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbDK.ForeColor = Color.FromArgb(135, 60, 10);
            grbDK.Location = new Point(39, 102);
            grbDK.Name = "grbDK";
            grbDK.Size = new Size(558, 168);
            grbDK.TabIndex = 1;
            grbDK.TabStop = false;
            grbDK.Text = "Điều khiển Server";
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(245, 210, 195);
            btnStop.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStop.ForeColor = Color.FromArgb(165, 47, 32);
            btnStop.Location = new Point(300, 66);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(242, 80);
            btnStop.TabIndex = 1;
            btnStop.Text = "Dừng Server";
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(243, 178, 97);
            btnStart.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnStart.Location = new Point(16, 66);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(242, 80);
            btnStart.TabIndex = 0;
            btnStart.Text = "Bắt đầu Server";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnClearlog
            // 
            btnClearlog.BackColor = Color.FromArgb(243, 178, 97);
            btnClearlog.Font = new Font("Times New Roman", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnClearlog.ForeColor = Color.FromArgb(135, 60, 10);
            btnClearlog.Location = new Point(1333, 939);
            btnClearlog.Name = "btnClearlog";
            btnClearlog.Size = new Size(216, 68);
            btnClearlog.TabIndex = 2;
            btnClearlog.Text = "Xoá Log";
            btnClearlog.UseVisualStyleBackColor = false;
            btnClearlog.Click += btnClearlog_Click;
            // 
            // grbTT
            // 
            grbTT.BackColor = Color.FromArgb(255, 240, 214);
            grbTT.Controls.Add(lblUpTime);
            grbTT.Controls.Add(lblClientCount);
            grbTT.Controls.Add(lblPort);
            grbTT.Controls.Add(lblStatus);
            grbTT.Controls.Add(lblThoigian);
            grbTT.Controls.Add(lblC);
            grbTT.Controls.Add(lblCong);
            grbTT.Controls.Add(lblTT);
            grbTT.Font = new Font("Times New Roman", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbTT.ForeColor = Color.FromArgb(135, 60, 10);
            grbTT.Location = new Point(35, 298);
            grbTT.Name = "grbTT";
            grbTT.Size = new Size(566, 322);
            grbTT.TabIndex = 3;
            grbTT.TabStop = false;
            grbTT.Text = "Thông tin Server";
            // 
            // lblUpTime
            // 
            lblUpTime.AutoSize = true;
            lblUpTime.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblUpTime.Location = new Point(304, 250);
            lblUpTime.Name = "lblUpTime";
            lblUpTime.Size = new Size(129, 36);
            lblUpTime.TabIndex = 7;
            lblUpTime.Text = "00:00:00";
            // 
            // lblClientCount
            // 
            lblClientCount.AutoSize = true;
            lblClientCount.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblClientCount.Location = new Point(304, 188);
            lblClientCount.Name = "lblClientCount";
            lblClientCount.Size = new Size(31, 36);
            lblClientCount.TabIndex = 6;
            lblClientCount.Text = "0";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPort.Location = new Point(304, 128);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(79, 36);
            lblPort.TabIndex = 5;
            lblPort.Text = "8006";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStatus.Location = new Point(304, 66);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(148, 36);
            lblStatus.TabIndex = 4;
            lblStatus.Text = "Chưa chạy";
            // 
            // lblThoigian
            // 
            lblThoigian.AutoSize = true;
            lblThoigian.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblThoigian.Location = new Point(20, 250);
            lblThoigian.Name = "lblThoigian";
            lblThoigian.Size = new Size(211, 36);
            lblThoigian.TabIndex = 3;
            lblThoigian.Text = "Thời gian chạy:";
            // 
            // lblC
            // 
            lblC.AutoSize = true;
            lblC.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblC.Location = new Point(20, 188);
            lblC.Name = "lblC";
            lblC.Size = new Size(143, 36);
            lblC.TabIndex = 2;
            lblC.Text = "Số Client:";
            // 
            // lblCong
            // 
            lblCong.AutoSize = true;
            lblCong.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCong.Location = new Point(20, 128);
            lblCong.Name = "lblCong";
            lblCong.Size = new Size(175, 36);
            lblCong.TabIndex = 1;
            lblCong.Text = "Cổng (Port):";
            // 
            // lblTT
            // 
            lblTT.AutoSize = true;
            lblTT.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTT.Location = new Point(20, 66);
            lblTT.Name = "lblTT";
            lblTT.Size = new Size(160, 36);
            lblTT.TabIndex = 0;
            lblTT.Text = "Trạng thái: ";
            // 
            // grbClients
            // 
            grbClients.BackColor = Color.FromArgb(255, 240, 214);
            grbClients.Controls.Add(dgvClients);
            grbClients.Font = new Font("Times New Roman", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbClients.ForeColor = Color.FromArgb(135, 60, 10);
            grbClients.Location = new Point(659, 102);
            grbClients.Name = "grbClients";
            grbClients.Size = new Size(890, 518);
            grbClients.TabIndex = 4;
            grbClients.TabStop = false;
            grbClients.Text = "Danh sách Client đang kết nối";
            // 
            // dgvClients
            // 
            dgvClients.AllowUserToAddRows = false;
            dgvClients.AllowUserToDeleteRows = false;
            dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClients.BackgroundColor = Color.FromArgb(255, 249, 238);
            dgvClients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvClients.Location = new Point(18, 49);
            dgvClients.Name = "dgvClients";
            dgvClients.ReadOnly = true;
            dgvClients.RowHeadersVisible = false;
            dgvClients.RowHeadersWidth = 82;
            dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClients.Size = new Size(854, 451);
            dgvClients.TabIndex = 0;
            // 
            // grbLog
            // 
            grbLog.BackColor = Color.FromArgb(255, 240, 214);
            grbLog.Controls.Add(txtLog);
            grbLog.Font = new Font("Times New Roman", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grbLog.ForeColor = Color.FromArgb(135, 60, 10);
            grbLog.Location = new Point(35, 626);
            grbLog.Name = "grbLog";
            grbLog.Size = new Size(1514, 307);
            grbLog.TabIndex = 5;
            grbLog.TabStop = false;
            grbLog.Text = "Nhật ký hoạt động (Log)";
            // 
            // txtLog
            // 
            txtLog.BackColor = Color.FromArgb(255, 249, 238);
            txtLog.ForeColor = Color.FromArgb(74, 43, 24);
            txtLog.Location = new Point(20, 49);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(1476, 252);
            txtLog.TabIndex = 0;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(18F, 36F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Phố_đêm_rực_rỡ_dưới_đèn_lồng;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1574, 1019);
            Controls.Add(grbLog);
            Controls.Add(grbClients);
            Controls.Add(grbTT);
            Controls.Add(btnClearlog);
            Controls.Add(grbDK);
            Controls.Add(lblGS);
            Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ServerForm";
            Text = "GAME SERVER";
            Load += ServerForm_Load;
            grbDK.ResumeLayout(false);
            grbTT.ResumeLayout(false);
            grbTT.PerformLayout();
            grbClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClients).EndInit();
            grbLog.ResumeLayout(false);
            grbLog.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblGS;
        private GroupBox grbDK;
        private Button btnStop;
        private Button btnStart;
        private Button btnClearlog;
        private GroupBox grbTT;
        private Label lblUpTime;
        private Label lblClientCount;
        private Label lblPort;
        private Label lblStatus;
        private Label lblThoigian;
        private Label lblC;
        private Label lblCong;
        private Label lblTT;
        private GroupBox grbClients;
        private DataGridView dgvClients;
        private GroupBox grbLog;
        private TextBox txtLog;
    }
}
