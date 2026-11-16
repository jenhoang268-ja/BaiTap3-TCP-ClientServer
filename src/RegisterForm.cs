using System;
using System.Drawing;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GameShared;

namespace UserManagement
{
    public partial class RegisterForm : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        // Cho phép nhận trạng thái WindowState từ Form trước đó
        public RegisterForm(FormWindowState state = FormWindowState.Normal)
        {
            InitializeComponent();
            this.WindowState = state;

            if (panel1 != null)
            {
                panel1.MouseDown += Panel1_MouseDown;
                panel1.MouseMove += Panel1_MouseMove;
                panel1.MouseUp += Panel1_MouseUp;
            }
            CaiDatForm();
        }

        private void CaiDatForm()
        {
            CenterPanel();

            // Ẩn nội dung mật khẩu
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            // Nhấn Enter để đăng ký
            this.AcceptButton = btnRegister;

            // Không gọi Click thủ công khi nhấn Enter
            txtUsername.KeyDown -= InputBox_KeyDown;
            txtPassword.KeyDown -= InputBox_KeyDown;
            txtConfirmPassword.KeyDown -= InputBox_KeyDown;
            txtHovaTen.KeyDown -= InputBox_KeyDown;
            txtEmail.KeyDown -= InputBox_KeyDown;

            // Trạng thái nút
            btnRegister.Enabled = true;

            // Xóa thông báo lỗi
            XoaThongBaoLoi();

            // Thiết lập Tab
            SetupTabOrder();
        }

        private void XoaThongBaoLoi()
        {
            lblErrorUsername.Text = "";
            lblErrorPassword.Text = "";
            lblErrorConfirm.Text = "";
            lblErrorHoTen.Text = "";
            lblErrorEmail.Text = "";

            // Màu thông báo dễ nhìn trên Panel nâu
            lblErrorUsername.ForeColor = Color.Yellow;
            lblErrorPassword.ForeColor = Color.Yellow;
            lblErrorConfirm.ForeColor = Color.Yellow;
            lblErrorHoTen.ForeColor = Color.Yellow;
            lblErrorEmail.ForeColor = Color.Yellow;
        }


        private void RegisterForm_Load(object sender, EventArgs e)
        {
            CenterPanel();
        }

        private void SetupTabOrder()
        {
            txtUsername.TabStop = true;
            txtUsername.TabIndex = 0;

            txtPassword.TabStop = true;
            txtPassword.TabIndex = 1;

            txtConfirmPassword.TabStop = true;
            txtConfirmPassword.TabIndex = 2;

            txtHovaTen.TabStop = true;
            txtHovaTen.TabIndex = 3;

            txtEmail.TabStop = true;
            txtEmail.TabIndex = 4;

            btnRegister.TabStop = true;
            btnRegister.TabIndex = 5;

            btnCancel.TabStop = true;
            btnCancel.TabIndex = 6;

            btnMinimize.TabStop = false;
            btnMaximize.TabStop = false;
            btnExit.TabStop = false;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e) { }

        private void RegisterForm_Resize(object sender, EventArgs e)
        {
            CenterPanel();
        }

        private void CenterPanel()
        {
            if (panelCenter == null)
                return;

            int x = Math.Max(
                0,
                (ClientSize.Width - panelCenter.Width) / 2);

            int y = Math.Max(
                0,
                (ClientSize.Height - panelCenter.Height) / 2);

            panelCenter.Location = new Point(x, y);
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = Location;
            }
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && WindowState == FormWindowState.Normal)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));

                Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                dragging = false;
        }

        private void btnMinimize_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            WindowState =
                 WindowState == FormWindowState.Normal
                     ? FormWindowState.Maximized
                     : FormWindowState.Normal;

            CenterPanel();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            QuayVeLoginForm();
        }

        private async void btnRegister_Click_1(object sender, EventArgs e)
        {
            // Tránh gửi request trùng
            if (!btnRegister.Enabled)
                return;

            string username = txtUsername.Text.Trim();

            // Không Trim mật khẩu
            string password = txtPassword.Text;

            string confirmPassword =
                txtConfirmPassword.Text;

            string hoten = txtHovaTen.Text.Trim();

            string email = txtEmail.Text.Trim();

            XoaThongBaoLoi();


            if (string.IsNullOrWhiteSpace(username))
            {
                lblErrorUsername.Text =
                    "Không được để trống";

                txtUsername.Focus();
                return;
            }

            if (username.Length < 3 ||
                username.Length > 20)
            {
                lblErrorUsername.Text =
                    "Tên đăng nhập từ 3-20 ký tự";

                txtUsername.Focus();
                return;
            }

            if (!Regex.IsMatch(
                username, @"^[a-zA-Z0-9_]+$"))
            {
                lblErrorUsername.Text =
                    "Chỉ gồm chữ, số và gạch dưới";

                txtUsername.Focus();
                return;
            }


            if (string.IsNullOrEmpty(password))
            {
                lblErrorPassword.Text =
                    "Không được để trống";

                txtPassword.Focus();
                return;
            }

            if (password.Length < 8 ||
                password.Length > 128)
            {
                lblErrorPassword.Text =
                    "Mật khẩu phải từ 8-128 ký tự";

                txtPassword.Focus();
                return;
            }

            if (!Regex.IsMatch(password, @"[a-zA-Z]") ||
                !Regex.IsMatch(password, @"[0-9]"))
            {
                lblErrorPassword.Text =
                    "Mật khẩu phải có chữ và số";

                txtPassword.Focus();
                return;
            }


            if (password != confirmPassword)
            {
                lblErrorConfirm.Text =
                    "Mật khẩu xác nhận không khớp";

                txtConfirmPassword.Focus();
                return;
            }


            if (string.IsNullOrWhiteSpace(hoten))
            {
                lblErrorHoTen.Text =
                    "Không được để trống";

                txtHovaTen.Focus();
                return;
            }

            if (hoten.Length < 2 ||
                hoten.Length > 50)
            {
                lblErrorHoTen.Text =
                    "Họ tên phải từ 2-50 ký tự";

                txtHovaTen.Focus();
                return;
            }


            if (string.IsNullOrWhiteSpace(email) ||
                email.Length > 100)
            {
                lblErrorEmail.Text =
                    "Email không hợp lệ";

                txtEmail.Focus();
                return;
            }

            try
            {
                MailAddress address =
                    new MailAddress(email);

                if (address.Address != email)
                {
                    lblErrorEmail.Text =
                        "Email không đúng định dạng";

                    txtEmail.Focus();
                    return;
                }
            }
            catch (FormatException)
            {
                lblErrorEmail.Text =
                    "Email không đúng định dạng";

                txtEmail.Focus();
                return;
            }


            btnRegister.Enabled = false;
            btnCancel.Enabled = false;

            string originalText = btnRegister.Text;
            btnRegister.Text = "Đang xử lý...";

            bool registered = false;

            try
            {

                NetworkRequest request = new NetworkRequest
                {
                    Action = Commands.DANGKY,

                    Username = username,

                    Password = password,

                    FullName = hoten,

                    Email = email
                };


                NetworkResponse response = await NetworkService.SendAsync(request);


                if (response.Success &&
                    response.StatusCode == StatusCodes.CREATED)
                {
                    registered = true;

                    MessageBox.Show(
                        "Đăng ký tài khoản thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else if (response.StatusCode ==
                         StatusCodes.CONFLICT)
                {
                    lblErrorUsername.Text =
                        "Tên đăng nhập đã tồn tại!";

                    txtUsername.Focus();
                }
                else if (response.StatusCode ==
                         StatusCodes.VALIDATION_ERROR)
                {
                    MessageBox.Show(
                        response.Message,
                        "Dữ liệu không hợp lệ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else if (response.StatusCode ==
                         StatusCodes.BAD_REQUEST)
                {
                    MessageBox.Show(
                        response.Message,
                        "Yêu cầu không hợp lệ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else if (response.StatusCode ==
                         StatusCodes.SERVICE_UNAVAILABLE)
                {
                    MessageBox.Show(
                        response.Message,
                        "Lỗi kết nối Server",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(
                        response.Message,
                        "Đăng ký thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể đăng ký: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    btnRegister.Enabled = true;
                    btnCancel.Enabled = true;
                    btnRegister.Text = originalText;
                }
            }

            if (registered)
            {
                QuayVeLoginForm();
            }
        }

        private void QuayVeLoginForm()
        {
            LoginForm loginForm =
                new LoginForm(this.WindowState);

            this.Hide();

            loginForm.ShowDialog();

            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            QuayVeLoginForm();
        }

        private void panelCenter_Paint(object sender, PaintEventArgs e) { }
        private void RegisterForm_Load_1(object sender, EventArgs e)
        {
            CenterPanel();
        }
        private void txtUsername_TextChanged(object sender, EventArgs e) { if (lblErrorUsername != null) lblErrorUsername.Text = ""; }
        private void txtUsername_Click(object sender, EventArgs e) { if (lblErrorUsername != null) lblErrorUsername.Text = ""; }
        private void txtPassword_TextChanged(object sender, EventArgs e) { if (lblErrorPassword != null) lblErrorPassword.Text = ""; }
        private void txtPassword_Click(object sender, EventArgs e) { if (lblErrorPassword != null) lblErrorPassword.Text = ""; }
        private void txtConfirmPassword_TextChanged(object sender, EventArgs e) { if (lblErrorConfirm != null) lblErrorConfirm.Text = ""; }
        private void txtConfirmPassword_Click(object sender, EventArgs e) { if (lblErrorConfirm != null) lblErrorConfirm.Text = ""; }
        private void txtHovaTen_TextChanged(object sender, EventArgs e) { if (lblErrorHoTen != null) lblErrorHoTen.Text = ""; }
        private void txtHovaTen_Click(object sender, EventArgs e) { if (lblErrorHoTen != null) lblErrorHoTen.Text = ""; }
        private void lblErrorEmail_Click(object sender, EventArgs e) { if (lblErrorEmail != null) lblErrorEmail.Text = ""; }
        private void txtEmail_TextChanged(object sender, EventArgs e) { if (lblErrorEmail != null) lblErrorEmail.Text = ""; }
        private void txtEmail_Click(object sender, EventArgs e) { if (lblErrorEmail != null) lblErrorEmail.Text = ""; }

        private void lblErrorConfirm_Click(object sender, EventArgs e)
        {

        }

        private bool isPasswordVisible = false;

        private void btnHienmatkhau_Click(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible; // Đảo trạng thái

            // Cập nhật TextBox
            txtPassword.UseSystemPasswordChar = !isPasswordVisible;

            // Đổi ảnh nền của nút (sử dụng BackgroundImage)
            if (isPasswordVisible)
            {
                // Khi đang hiện chữ -> Hiện icon con mắt gạch chéo
                btnHienmatkhau.BackgroundImage = Properties.Resources.hide;
            }
            else
            {
                // Khi đang che chữ bằng dấu chấm -> Hiện icon con mắt mở
                btnHienmatkhau.BackgroundImage = Properties.Resources.view;
            }
        }

        // Khai báo thêm một biến cờ riêng
        private bool isConfirmPasswordVisible = false;

        private void btnHienXacNhanMK_Click(object sender, EventArgs e)
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;

            txtConfirmPassword.UseSystemPasswordChar = !isConfirmPasswordVisible;

            if (isConfirmPasswordVisible)
            {
                btnHienXacNhanMK.BackgroundImage = Properties.Resources.hide;
            }
            else
            {
                btnHienXacNhanMK.BackgroundImage = Properties.Resources.view;
            }
        }
    }
}