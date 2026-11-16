using System;
using System.Drawing;
using System.Windows.Forms;
using GameShared;
using System.Threading.Tasks;

namespace UserManagement
{
    public partial class LoginForm : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Cho phép nhận trạng thái WindowState từ Form trước đó
        public LoginForm(FormWindowState state = FormWindowState.Normal)
        {
            InitializeComponent();
            // 1. Tắt cơ chế tự bóp nhỏ Form
            this.AutoSize = false;

            // 2. Tắt cơ chế tự thu phóng theo màn hình Windows
            this.AutoScaleMode = AutoScaleMode.None;
            this.WindowState = state;

            if (panel1 != null)
            {
                panel1.MouseDown += Panel1_MouseDown;
                panel1.MouseMove += Panel1_MouseMove;
                panel1.MouseUp += Panel1_MouseUp;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '\0';
            // Căn giữa Panel
            CenterPanel();

            // Thông tin GameServer
            txtServerIP.Text = NetworkService.ServerIP;
            txtServerPort.Text =
                NetworkService.ServerPort.ToString();

            // Trạng thái kết nối
            lblConnectStatus.Text = "Chưa kết nối";
            lblConnectStatus.ForeColor = Color.White;

            // Xóa lỗi lúc mở Form
            lblError.Text = "";
            lblError.ForeColor = Color.Yellow;
            lblError.Visible = true;
            lblError.BringToFront();

            // Ẩn mật khẩu
            txtPassword.UseSystemPasswordChar = true;

            // Nhấn Enter để đăng nhập
            this.AcceptButton = btnDangNhap;

            // Bỏ xử lý Enter thủ công để không gửi trùng
            txtTenDangNhap.KeyDown -= InputBox_KeyDown;
            txtPassword.KeyDown -= InputBox_KeyDown;

            // Thứ tự Tab
            txtTenDangNhap.TabIndex = 0;
            txtPassword.TabIndex = 1;
            txtServerIP.TabIndex = 2;
            txtServerPort.TabIndex = 3;
            btnDangNhap.TabIndex = 4;
            btnThoat.TabIndex = 5;
            lnkRegister.TabIndex = 6;

            // Không chọn các nút trên thanh tiêu đề bằng Tab
            btnMinimize.TabStop = false;
            btnMaximize.TabStop = false;
            btnExit.TabStop = false;
        }

        private void SetupTabOrder()
        {
            if (txtTenDangNhap != null)
            {
                txtTenDangNhap.TabStop = true;
                txtTenDangNhap.TabIndex = 0;
            }

            if (txtPassword != null)
            {
                txtPassword.TabStop = true;
                txtPassword.TabIndex = 1;
            }

            if (btnDangNhap != null)
            {
                btnDangNhap.TabStop = true;
                btnDangNhap.TabIndex = 2;
            }

            if (btnThoat != null)
            {
                btnThoat.TabStop = true;
                btnThoat.TabIndex = 3;
            }

            if (lnkRegister != null)
            {
                lnkRegister.TabStop = true;
                lnkRegister.TabIndex = 4;
            }

            if (btnMinimize != null) btnMinimize.TabStop = false;
            if (btnMaximize != null) btnMaximize.TabStop = false;
            if (btnExit != null) btnExit.TabStop = false;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnDangNhap_Click_1(btnDangNhap, EventArgs.Empty);
            }
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            CenterPanel();
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && this.WindowState == FormWindowState.Normal)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = false;
            }
        }

        private void btnMinimize_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
            CenterPanel();
        }

        private void btnExit_Click_2(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void btnDangNhap_Click_1(object sender, EventArgs e)
        {

            lblError.Text = "";
            lblError.ForeColor = Color.Yellow;


            string username = txtTenDangNhap.Text.Trim();
            string password = txtPassword.Text;

            string ip = txtServerIP.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                lblError.Text = "Vui lòng nhập tên đăng nhập!";
                txtTenDangNhap.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                lblError.Text = "Vui lòng nhập mật khẩu!";
                txtPassword.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                lblError.Text = "Vui lòng nhập IP Server!";
                txtServerIP.Focus();
                return;
            }

            if (!int.TryParse(txtServerPort.Text,
                              out int port) ||
                port < 1 || port > 65535)
            {
                lblError.Text = "Port không hợp lệ!";
                txtServerPort.Focus();
                return;
            }


            btnDangNhap.Enabled = false;
            btnDangNhap.Text = "Đang xử lý...";

            lblConnectStatus.Text = "Đang kết nối...";
            lblConnectStatus.ForeColor = Color.Yellow;

            try
            {


                if (NetworkService.ServerIP != ip ||
                    NetworkService.ServerPort != port)
                {
                    NetworkService.Disconnect();
                }

                NetworkService.Configure(ip, port);


                NetworkRequest request = new NetworkRequest
                {
                    Action = Commands.DANGNHAP,
                    Username = username,
                    Password = password
                };

                NetworkResponse response =
                    await NetworkService.SendAsync(request);


                if (response.Success &&
                    response.StatusCode == StatusCodes.SUCCESS &&
                    response.Profile != null)
                {
                    lblConnectStatus.Text = "Đã kết nối Server";
                    lblConnectStatus.ForeColor = Color.LightGreen;

                    UserProfile profile = response.Profile;

                    MessageBox.Show(
                        "Đăng nhập thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    // Chuyển sang MainForm
                    MainForm main = new MainForm(
                        profile.Username,
                        profile.FullName,
                        profile.Email,
                        profile.LoginTime,
                        this.WindowState);

                    this.Hide();
                    main.ShowDialog();
                    this.Close();

                    return;
                }

                switch (response.StatusCode)
                {
                    case StatusCodes.UNAUTHORIZED:
                        lblError.Text =
                            "Sai tên đăng nhập hoặc mật khẩu!";
                        break;

                    case StatusCodes.BAD_REQUEST:
                        lblError.Text =
                            "Thông tin đăng nhập không hợp lệ!";
                        break;

                    case StatusCodes.SERVICE_UNAVAILABLE:
                        lblError.Text =
                            "Không thể kết nối hoặc sử dụng Server!";
                        break;

                    default:
                        lblError.Text = response.Message;
                        break;
                }

                lblError.ForeColor = Color.Yellow;
                lblError.Visible = true;
                lblError.BringToFront();

                // Trạng thái TCP
                if (NetworkService.IsConnected)
                {
                    lblConnectStatus.Text =
                        "Server đã phản hồi";

                    lblConnectStatus.ForeColor =
                        Color.LightGreen;
                }
                else
                {
                    lblConnectStatus.Text =
                        "Mất kết nối Server";

                    lblConnectStatus.ForeColor =
                        Color.Yellow;
                }

                txtPassword.Clear();
                txtPassword.Focus();
            }
            catch (Exception ex)
            {
                lblError.Text =
                    "Lỗi đăng nhập: " + ex.Message;

                lblError.ForeColor = Color.Yellow;

                lblConnectStatus.Text =
                    "Lỗi kết nối";

                lblConnectStatus.ForeColor = Color.Yellow;
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    btnDangNhap.Enabled = true;
                    btnDangNhap.Text = "Đăng Nhập";
                }
            }
        }

        private void lnkRegister_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Truyền WindowState hiện tại sang RegisterForm
            string ip = txtServerIP.Text.Trim();

            if (!int.TryParse(
                txtServerPort.Text, out int port) ||
                port < 1 || port > 65535)
            {
                lblError.Text = "Port không hợp lệ!";
                return;
            }

            try
            {
                if (NetworkService.ServerIP != ip ||
                    NetworkService.ServerPort != port)
                {
                    NetworkService.Disconnect();
                }

                // Lưu cấu hình cho RegisterForm sử dụng
                NetworkService.Configure(ip, port);

                RegisterForm regForm =
                    new RegisterForm(this.WindowState);

                this.Hide();
                regForm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.ForeColor = Color.Yellow;
            }
        }

        private void CenterPanel()
        {
            if (panelCenter != null)
            {
                panelCenter.Location = new Point(
                    (this.ClientSize.Width - panelCenter.Width) / 2,
                    (this.ClientSize.Height - panelCenter.Height) / 2
                );
            }
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panelCenter_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {
            if (lblError != null) lblError.Text = "";
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (lblError != null) lblError.Text = "";
        }

        private void txtTenDangNhap_Click(object sender, EventArgs e)
        {
            if (lblError != null) lblError.Text = "";
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            if (lblError != null) lblError.Text = "";
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void panelCenter_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
