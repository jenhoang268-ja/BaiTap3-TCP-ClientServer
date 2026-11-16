namespace UserManagement
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            panel1 = new Panel();
            label1 = new Label();
            btnMaximize = new Button();
            btnMinimize = new Button();
            btnExit = new Button();
            panelCenter = new Panel();
            chkHienMatKhau = new CheckBox();
            lblPort = new Label();
            lblIP = new Label();
            lblConnectStatus = new Label();
            txtServerPort = new TextBox();
            txtServerIP = new TextBox();
            lblConnect = new Label();
            lblRegister = new LinkLabel();
            btnThoat = new Button();
            lblError = new Label();
            btnDangNhap = new Button();
            txtPassword = new TextBox();
            txtTenDangNhap = new TextBox();
            label3 = new Label();
            label2 = new Label();
            lnkRegister = new LinkLabel();
            panel1.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Peru;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnMaximize);
            panel1.Controls.Add(btnMinimize);
            panel1.Controls.Add(btnExit);
            panel1.Location = new Point(-104, -10);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1305, 51);
            panel1.TabIndex = 0;
            panel1.MouseDoubleClick += Panel1_MouseDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label1.ForeColor = Color.SaddleBrown;
            label1.Location = new Point(602, 15);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(111, 25);
            label1.TabIndex = 1;
            label1.Text = "Đăng Nhập";
            // 
            // btnMaximize
            // 
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximize.BackColor = Color.Peru;
            btnMaximize.Font = new Font("Segoe UI Black", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMaximize.Location = new Point(1196, 8);
            btnMaximize.Margin = new Padding(4);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(55, 43);
            btnMaximize.TabIndex = 2;
            btnMaximize.TabStop = false;
            btnMaximize.Text = "❐";
            btnMaximize.UseVisualStyleBackColor = false;
            btnMaximize.Click += btnMaximize_Click_1;
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.BackColor = Color.Peru;
            btnMinimize.Font = new Font("Segoe UI Black", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnMinimize.Location = new Point(1144, 8);
            btnMinimize.Margin = new Padding(4);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(55, 43);
            btnMinimize.TabIndex = 1;
            btnMinimize.TabStop = false;
            btnMinimize.Text = "-";
            btnMinimize.UseVisualStyleBackColor = false;
            btnMinimize.Click += btnMinimize_Click_1;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExit.BackColor = Color.Peru;
            btnExit.Location = new Point(1249, 8);
            btnExit.Margin = new Padding(4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(55, 43);
            btnExit.TabIndex = 0;
            btnExit.TabStop = false;
            btnExit.Text = "X";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click_2;
            // 
            // panelCenter
            // 
            panelCenter.Anchor = AnchorStyles.None;
            panelCenter.Controls.Add(chkHienMatKhau);
            panelCenter.Controls.Add(lblPort);
            panelCenter.Controls.Add(lblIP);
            panelCenter.Controls.Add(lblConnectStatus);
            panelCenter.Controls.Add(txtServerPort);
            panelCenter.Controls.Add(txtServerIP);
            panelCenter.Controls.Add(lblConnect);
            panelCenter.Controls.Add(lblRegister);
            panelCenter.Controls.Add(btnThoat);
            panelCenter.Controls.Add(lblError);
            panelCenter.Controls.Add(btnDangNhap);
            panelCenter.Controls.Add(txtPassword);
            panelCenter.Controls.Add(txtTenDangNhap);
            panelCenter.Controls.Add(label3);
            panelCenter.Controls.Add(label2);
            panelCenter.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            panelCenter.Location = new Point(184, 100);
            panelCenter.Margin = new Padding(4);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(866, 531);
            panelCenter.TabIndex = 5;
            panelCenter.Paint += panelCenter_Paint_1;
            // 
            // chkHienMatKhau
            // 
            chkHienMatKhau.AutoSize = true;
            chkHienMatKhau.ForeColor = Color.Snow;
            chkHienMatKhau.Location = new Point(279, 135);
            chkHienMatKhau.Name = "chkHienMatKhau";
            chkHienMatKhau.Size = new Size(174, 29);
            chkHienMatKhau.TabIndex = 17;
            chkHienMatKhau.Text = "Hiện mật khẩu";
            chkHienMatKhau.UseVisualStyleBackColor = true;
            chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Font = new Font("Cascadia Code", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblPort.ForeColor = Color.NavajoWhite;
            lblPort.Location = new Point(587, 220);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(80, 18);
            lblPort.TabIndex = 16;
            lblPort.Text = "Cổng Port";
            // 
            // lblIP
            // 
            lblIP.AutoSize = true;
            lblIP.Font = new Font("Cascadia Code", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblIP.ForeColor = Color.NavajoWhite;
            lblIP.Location = new Point(21, 220);
            lblIP.Name = "lblIP";
            lblIP.Size = new Size(88, 18);
            lblIP.TabIndex = 15;
            lblIP.Text = "Địa chỉ IP";
            // 
            // lblConnectStatus
            // 
            lblConnectStatus.Anchor = AnchorStyles.None;
            lblConnectStatus.AutoSize = true;
            lblConnectStatus.Font = new Font("Cascadia Code", 12F);
            lblConnectStatus.ForeColor = Color.NavajoWhite;
            lblConnectStatus.Location = new Point(439, 271);
            lblConnectStatus.Name = "lblConnectStatus";
            lblConnectStatus.Size = new Size(118, 21);
            lblConnectStatus.TabIndex = 14;
            lblConnectStatus.Text = "Chưa kết nối";
            // 
            // txtServerPort
            // 
            txtServerPort.Font = new Font("Cascadia Code", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtServerPort.Location = new Point(587, 254);
            txtServerPort.Name = "txtServerPort";
            txtServerPort.Size = new Size(261, 29);
            txtServerPort.TabIndex = 13;
            // 
            // txtServerIP
            // 
            txtServerIP.Font = new Font("Cascadia Code", 13.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtServerIP.Location = new Point(21, 254);
            txtServerIP.Name = "txtServerIP";
            txtServerIP.Size = new Size(548, 29);
            txtServerIP.TabIndex = 12;
            // 
            // lblConnect
            // 
            lblConnect.AutoSize = true;
            lblConnect.Font = new Font("Cascadia Code", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblConnect.ForeColor = Color.NavajoWhite;
            lblConnect.Location = new Point(21, 173);
            lblConnect.Name = "lblConnect";
            lblConnect.Size = new Size(152, 18);
            lblConnect.TabIndex = 11;
            lblConnect.Text = "Kết nối GameServer";
            // 
            // lblRegister
            // 
            lblRegister.AutoSize = true;
            lblRegister.Font = new Font("Cascadia Code", 10.875F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRegister.ForeColor = Color.Chocolate;
            lblRegister.LinkColor = Color.NavajoWhite;
            lblRegister.Location = new Point(314, 454);
            lblRegister.Margin = new Padding(4, 0, 4, 0);
            lblRegister.Name = "lblRegister";
            lblRegister.Size = new Size(243, 20);
            lblRegister.TabIndex = 10;
            lblRegister.TabStop = true;
            lblRegister.Text = "Chưa có tài khoản? Đăng kí";
            lblRegister.LinkClicked += lnkRegister_LinkClicked_1;
            // 
            // btnThoat
            // 
            btnThoat.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnThoat.BackColor = Color.Peru;
            btnThoat.Location = new Point(470, 364);
            btnThoat.Margin = new Padding(4);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(349, 64);
            btnThoat.TabIndex = 9;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            // 
            // lblError
            // 
            lblError.Anchor = AnchorStyles.None;
            lblError.Font = new Font("Cascadia Code", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblError.ForeColor = Color.Red;
            lblError.Location = new Point(21, 131);
            lblError.Margin = new Padding(4, 0, 4, 0);
            lblError.Name = "lblError";
            lblError.Size = new Size(215, 29);
            lblError.TabIndex = 6;
            // 
            // btnDangNhap
            // 
            btnDangNhap.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnDangNhap.BackColor = Color.Peru;
            btnDangNhap.Location = new Point(43, 364);
            btnDangNhap.Margin = new Padding(4);
            btnDangNhap.Name = "btnDangNhap";
            btnDangNhap.Size = new Size(366, 64);
            btnDangNhap.TabIndex = 2;
            btnDangNhap.Text = "Đăng Nhập";
            btnDangNhap.UseVisualStyleBackColor = false;
            btnDangNhap.Click += btnDangNhap_Click_1;
            // 
            // txtPassword
            // 
            txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtPassword.ForeColor = Color.SaddleBrown;
            txtPassword.Location = new Point(279, 90);
            txtPassword.Margin = new Padding(4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(569, 29);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Click += txtPassword_Click;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTenDangNhap.ForeColor = Color.SaddleBrown;
            txtTenDangNhap.Location = new Point(279, 37);
            txtTenDangNhap.Margin = new Padding(4);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.Size = new Size(569, 29);
            txtTenDangNhap.TabIndex = 0;
            txtTenDangNhap.Click += txtTenDangNhap_Click;
            txtTenDangNhap.TextChanged += txtTenDangNhap_TextChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label3.ForeColor = Color.NavajoWhite;
            label3.Location = new Point(34, 90);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 25);
            label3.TabIndex = 1;
            label3.Text = "Mật khẩu";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label2.ForeColor = Color.NavajoWhite;
            label2.Location = new Point(34, 37);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(155, 25);
            label2.TabIndex = 0;
            label2.Text = "Tên đăng nhập";
            // 
            // lnkRegister
            // 
            lnkRegister.Anchor = AnchorStyles.None;
            lnkRegister.ForeColor = Color.NavajoWhite;
            lnkRegister.LinkColor = Color.NavajoWhite;
            lnkRegister.Location = new Point(430, 417);
            lnkRegister.Margin = new Padding(4, 0, 4, 0);
            lnkRegister.Name = "lnkRegister";
            lnkRegister.Size = new Size(438, 31);
            lnkRegister.TabIndex = 4;
            lnkRegister.TextAlign = ContentAlignment.MiddleCenter;
            lnkRegister.LinkClicked += lnkRegister_LinkClicked_1;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SaddleBrown;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1200, 721);
            Controls.Add(panelCenter);
            Controls.Add(panel1);
            Controls.Add(lnkRegister);
            DoubleBuffered = true;
            Font = new Font("Cascadia Code", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.SaddleBrown;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginForm";
            Load += LoginForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelCenter.ResumeLayout(false);
            panelCenter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnExit;
        private Button btnMaximize;
        private Button btnMinimize;
        private Label label1;
        private Panel panelCenter;
        private Label label2;
        private Label label3;
        private Button btnDangNhap;
        private TextBox txtPassword;
        private TextBox txtTenDangNhap;
        private Label lblError;
        private LinkLabel lnkRegister;
        private Button btnThoat;
        private LinkLabel lblRegister;
        private Label lblConnect;
        private TextBox txtServerPort;
        private TextBox txtServerIP;
        private Label lblConnectStatus;
        private Label lblPort;
        private Label lblIP;
        private CheckBox chkHienMatKhau;
    }
}