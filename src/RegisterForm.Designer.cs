namespace UserManagement
{
    partial class RegisterForm
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
            panel1 = new Panel();
            btnMaximize = new Button();
            btnMinimize = new Button();
            btnExit = new Button();
            label1 = new Label();
            panelCenter = new Panel();
            btnHienXacNhanMK = new Button();
            btnHienmatkhau = new Button();
            lblErrorHoTen = new Label();
            txtHovaTen = new TextBox();
            label6 = new Label();
            lblErrorEmail = new Label();
            lblErrorConfirm = new Label();
            lblErrorPassword = new Label();
            lblErrorUsername = new Label();
            btnCancel = new Button();
            btnRegister = new Button();
            txtEmail = new TextBox();
            txtConfirmPassword = new TextBox();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            panelCenter.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Peru;
            panel1.Controls.Add(btnMaximize);
            panel1.Controls.Add(btnMinimize);
            panel1.Controls.Add(btnExit);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(5, 4, 5, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(1216, 45);
            panel1.TabIndex = 0;
            // 
            // btnMaximize
            // 
            btnMaximize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMaximize.BackColor = Color.Peru;
            btnMaximize.ForeColor = Color.SaddleBrown;
            btnMaximize.Location = new Point(1090, 0);
            btnMaximize.Margin = new Padding(4);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(64, 45);
            btnMaximize.TabIndex = 4;
            btnMaximize.TabStop = false;
            btnMaximize.Text = "❐";
            btnMaximize.UseVisualStyleBackColor = false;
            btnMaximize.Click += btnMaximize_Click;
            // 
            // btnMinimize
            // 
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMinimize.BackColor = Color.Peru;
            btnMinimize.ForeColor = Color.SaddleBrown;
            btnMinimize.Location = new Point(1027, 0);
            btnMinimize.Margin = new Padding(4);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(64, 45);
            btnMinimize.TabIndex = 3;
            btnMinimize.TabStop = false;
            btnMinimize.Text = "-";
            btnMinimize.UseVisualStyleBackColor = false;
            btnMinimize.Click += btnMinimize_Click_1;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExit.BackColor = Color.Peru;
            btnExit.ForeColor = Color.SaddleBrown;
            btnExit.Location = new Point(1152, 0);
            btnExit.Margin = new Padding(4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(64, 45);
            btnExit.TabIndex = 2;
            btnExit.TabStop = false;
            btnExit.Text = "X";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(500, 9);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(199, 25);
            label1.TabIndex = 1;
            label1.Text = "Đăng ký tài khoản";
            // 
            // panelCenter
            // 
            panelCenter.Anchor = AnchorStyles.None;
            panelCenter.BackColor = Color.SaddleBrown;
            panelCenter.Controls.Add(btnHienXacNhanMK);
            panelCenter.Controls.Add(btnHienmatkhau);
            panelCenter.Controls.Add(lblErrorHoTen);
            panelCenter.Controls.Add(txtHovaTen);
            panelCenter.Controls.Add(label6);
            panelCenter.Controls.Add(lblErrorEmail);
            panelCenter.Controls.Add(lblErrorConfirm);
            panelCenter.Controls.Add(lblErrorPassword);
            panelCenter.Controls.Add(lblErrorUsername);
            panelCenter.Controls.Add(btnCancel);
            panelCenter.Controls.Add(btnRegister);
            panelCenter.Controls.Add(txtEmail);
            panelCenter.Controls.Add(txtConfirmPassword);
            panelCenter.Controls.Add(txtPassword);
            panelCenter.Controls.Add(txtUsername);
            panelCenter.Controls.Add(label5);
            panelCenter.Controls.Add(label4);
            panelCenter.Controls.Add(label3);
            panelCenter.Controls.Add(label2);
            panelCenter.Location = new Point(308, 83);
            panelCenter.Margin = new Padding(5, 4, 5, 4);
            panelCenter.Name = "panelCenter";
            panelCenter.Size = new Size(664, 513);
            panelCenter.TabIndex = 2;
            panelCenter.Paint += panelCenter_Paint;
            // 
            // btnHienXacNhanMK
            // 
            btnHienXacNhanMK.BackColor = Color.Transparent;
            btnHienXacNhanMK.BackgroundImage = Properties.Resources.view;
            btnHienXacNhanMK.BackgroundImageLayout = ImageLayout.Stretch;
            btnHienXacNhanMK.FlatAppearance.BorderSize = 0;
            btnHienXacNhanMK.FlatStyle = FlatStyle.Flat;
            btnHienXacNhanMK.Location = new Point(618, 185);
            btnHienXacNhanMK.Name = "btnHienXacNhanMK";
            btnHienXacNhanMK.Size = new Size(25, 27);
            btnHienXacNhanMK.TabIndex = 18;
            btnHienXacNhanMK.UseVisualStyleBackColor = false;
            btnHienXacNhanMK.Click += btnHienXacNhanMK_Click;
            // 
            // btnHienmatkhau
            // 
            btnHienmatkhau.BackColor = Color.Transparent;
            btnHienmatkhau.BackgroundImage = Properties.Resources.view;
            btnHienmatkhau.BackgroundImageLayout = ImageLayout.Stretch;
            btnHienmatkhau.FlatAppearance.BorderSize = 0;
            btnHienmatkhau.FlatStyle = FlatStyle.Flat;
            btnHienmatkhau.Location = new Point(618, 106);
            btnHienmatkhau.Name = "btnHienmatkhau";
            btnHienmatkhau.Size = new Size(25, 27);
            btnHienmatkhau.TabIndex = 17;
            btnHienmatkhau.UseVisualStyleBackColor = false;
            btnHienmatkhau.Click += btnHienmatkhau_Click;
            // 
            // lblErrorHoTen
            // 
            lblErrorHoTen.ForeColor = Color.Red;
            lblErrorHoTen.Location = new Point(51, 297);
            lblErrorHoTen.Margin = new Padding(4, 0, 4, 0);
            lblErrorHoTen.Name = "lblErrorHoTen";
            lblErrorHoTen.Size = new Size(560, 36);
            lblErrorHoTen.TabIndex = 16;
            // 
            // txtHovaTen
            // 
            txtHovaTen.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            txtHovaTen.ForeColor = Color.SaddleBrown;
            txtHovaTen.Location = new Point(289, 264);
            txtHovaTen.Margin = new Padding(4);
            txtHovaTen.Name = "txtHovaTen";
            txtHovaTen.Size = new Size(322, 29);
            txtHovaTen.TabIndex = 4;
            txtHovaTen.Click += txtHovaTen_Click;
            txtHovaTen.TextChanged += txtHovaTen_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label6.ForeColor = Color.NavajoWhite;
            label6.Location = new Point(51, 264);
            label6.Margin = new Padding(5, 0, 5, 0);
            label6.Name = "label6";
            label6.Size = new Size(111, 25);
            label6.TabIndex = 14;
            label6.Text = "Họ và tên";
            // 
            // lblErrorEmail
            // 
            lblErrorEmail.ForeColor = Color.Red;
            lblErrorEmail.Location = new Point(51, 373);
            lblErrorEmail.Margin = new Padding(4, 0, 4, 0);
            lblErrorEmail.Name = "lblErrorEmail";
            lblErrorEmail.Size = new Size(560, 35);
            lblErrorEmail.TabIndex = 13;
            lblErrorEmail.Click += lblErrorEmail_Click;
            // 
            // lblErrorConfirm
            // 
            lblErrorConfirm.ForeColor = Color.Red;
            lblErrorConfirm.Location = new Point(51, 218);
            lblErrorConfirm.Margin = new Padding(4, 0, 4, 0);
            lblErrorConfirm.Name = "lblErrorConfirm";
            lblErrorConfirm.Size = new Size(560, 35);
            lblErrorConfirm.TabIndex = 12;
            lblErrorConfirm.Click += lblErrorConfirm_Click;
            // 
            // lblErrorPassword
            // 
            lblErrorPassword.ForeColor = Color.Red;
            lblErrorPassword.Location = new Point(51, 139);
            lblErrorPassword.Margin = new Padding(4, 0, 4, 0);
            lblErrorPassword.Name = "lblErrorPassword";
            lblErrorPassword.Size = new Size(560, 39);
            lblErrorPassword.TabIndex = 11;
            // 
            // lblErrorUsername
            // 
            lblErrorUsername.ForeColor = Color.Red;
            lblErrorUsername.Location = new Point(51, 55);
            lblErrorUsername.Margin = new Padding(4, 0, 4, 0);
            lblErrorUsername.Name = "lblErrorUsername";
            lblErrorUsername.Size = new Size(560, 36);
            lblErrorUsername.TabIndex = 10;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            btnCancel.Location = new Point(361, 427);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(224, 52);
            btnCancel.TabIndex = 7;
            btnCancel.Text = "Huỷ";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnRegister
            // 
            btnRegister.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            btnRegister.Location = new Point(68, 427);
            btnRegister.Margin = new Padding(4);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(232, 52);
            btnRegister.TabIndex = 6;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click_1;
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            txtEmail.ForeColor = Color.SaddleBrown;
            txtEmail.Location = new Point(289, 340);
            txtEmail.Margin = new Padding(4);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(322, 29);
            txtEmail.TabIndex = 5;
            txtEmail.Click += txtEmail_Click;
            txtEmail.TextChanged += txtEmail_TextChanged;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            txtConfirmPassword.ForeColor = Color.SaddleBrown;
            txtConfirmPassword.Location = new Point(289, 185);
            txtConfirmPassword.Margin = new Padding(4);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(322, 29);
            txtConfirmPassword.TabIndex = 3;
            txtConfirmPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.Click += txtConfirmPassword_Click;
            txtConfirmPassword.TextChanged += txtConfirmPassword_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            txtPassword.ForeColor = Color.SaddleBrown;
            txtPassword.Location = new Point(289, 106);
            txtPassword.Margin = new Padding(4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(322, 29);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.Click += txtPassword_Click;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            txtUsername.ForeColor = Color.SaddleBrown;
            txtUsername.Location = new Point(289, 22);
            txtUsername.Margin = new Padding(4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(322, 29);
            txtUsername.TabIndex = 1;
            txtUsername.Click += txtUsername_Click;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label5.ForeColor = Color.NavajoWhite;
            label5.Location = new Point(51, 343);
            label5.Margin = new Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new Size(67, 25);
            label5.TabIndex = 3;
            label5.Text = "Email";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label4.ForeColor = Color.NavajoWhite;
            label4.Location = new Point(51, 188);
            label4.Margin = new Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new Size(199, 25);
            label4.TabIndex = 2;
            label4.Text = "Xác nhận mật khẩu";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label3.ForeColor = Color.NavajoWhite;
            label3.Location = new Point(51, 109);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(100, 25);
            label3.TabIndex = 1;
            label3.Text = "Mật khẩu";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Cascadia Code", 14F, FontStyle.Bold);
            label2.ForeColor = Color.NavajoWhite;
            label2.Location = new Point(51, 25);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(155, 25);
            label2.TabIndex = 0;
            label2.Text = "Tên đăng nhập";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Gemini_Generated_Image_w6rdwow6rdwow6rd;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1216, 648);
            Controls.Add(panelCenter);
            Controls.Add(panel1);
            DoubleBuffered = true;
            Font = new Font("Cascadia Code", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.SaddleBrown;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(5, 4, 5, 4);
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RegisterForm";
            Load += RegisterForm_Load_1;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelCenter.ResumeLayout(false);
            panelCenter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Panel panelCenter;
        private Label label2;
        private Label label4;
        private Label label3;
        private Button btnCancel;
        private Button btnRegister;
        private TextBox txtEmail;
        private TextBox txtConfirmPassword;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label label5;
        private Button btnExit;
        private Button btnMaximize;
        private Button btnMinimize;
        private Label lblErrorEmail;
        private Label lblErrorConfirm;
        private Label lblErrorPassword;
        private Label lblErrorUsername;
        private Label label6;
        private TextBox txtHovaTen;
        private Label lblErrorHoTen;
        private Button btnHienmatkhau;
        private Button btnHienXacNhanMK;
    }
}