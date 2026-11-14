using System;
using System.Drawing;
using System.Windows.Forms;
using GameShared;

namespace UserManagement
{
    public partial class MainForm : Form
    {
        private string tenDangNhap = "";
        private string hoTen = "";
        private string email = "";
        private string loginTime = "";

        // Tránh xử lý chuyển Form nhiều lần
        private bool isNavigating = false;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public MainForm(FormWindowState state = FormWindowState.Normal)
        {
            InitializeComponent();
            this.WindowState = state;

            if (panel1 != null)
            {
                panel1.MouseDown += Panel1_MouseDown;
                panel1.MouseMove += Panel1_MouseMove;
                panel1.MouseUp += Panel1_MouseUp;
            }
        }

        public MainForm(string user, string name, string mail, string time, FormWindowState state = FormWindowState.Normal)
            : this(state)
        {
            tenDangNhap = user;
            hoTen = name;
            email = mail;
            loginTime = time;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            CenterPanel();

            // Cài đặt thứ tự Tab
            SetupTabOrder();

            // Hiển thị thông tin nhận từ LoginForm
            HienThiThongTin();

            // Khóa nút trong khi tải thông tin
            btnLogout.Enabled = false;

            try
            {
                // Gửi LAYTHONGTIN đến GameServer
                bool success = await TaiThongTinUserAsync();

                if (!success)
                {
                    MessageBox.Show(
                        "Không thể xác minh phiên đăng nhập. " +
                        "Vui lòng đăng nhập lại.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    // Ngắt kết nối cũ
                    NetworkService.Disconnect();

                    // Quay lại LoginForm
                    QuayVeLoginForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải thông tin: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                NetworkService.Disconnect();

                QuayVeLoginForm();
            }
            finally
            {
                if (!IsDisposed && !Disposing &&
                    !isNavigating)
                {
                    btnLogout.Enabled = true;
                }
            }
        }

        private void HienThiThongTin()
        {
            // Hiển thị lời chào
            if (!string.IsNullOrWhiteSpace(hoTen))
            {
                lblWelcome.Text =
                    $"Xin chào, {hoTen}!";
            }
            else
            {
                lblWelcome.Text =
                    "Xin chào, người chơi!";
            }

            // Hiển thị tên đăng nhập
            if (!string.IsNullOrWhiteSpace(tenDangNhap))
            {
                lblUsername.Text =
                    $"Tên đăng nhập: {tenDangNhap}";
            }
            else
            {
                lblUsername.Text =
                    "Tên đăng nhập: (Không có)";
            }

            // Hiển thị Email
            if (!string.IsNullOrWhiteSpace(email))
            {
                lblEmail.Text =
                    $"Email: {email}";
            }
            else
            {
                lblEmail.Text =
                    "Email: (Chưa cập nhật)";
            }

            // Hiển thị thời gian đăng nhập hiện tại
            if (!string.IsNullOrWhiteSpace(loginTime))
            {
                lblLoginTime.Text =
                    $"Thời gian đăng nhập: {loginTime}";
            }
            else
            {
                lblLoginTime.Text =
                    "Thời gian đăng nhập: Chưa có";
            }
        }

        private async Task<bool> TaiThongTinUserAsync()
        {
            // Tạo yêu cầu lấy thông tin
            NetworkRequest request =
                new NetworkRequest
                {
                    Action = Commands.LAYTHONGTIN
                };

            // Gửi yêu cầu qua TCP Socket
            NetworkResponse response =
                await NetworkService.SendAsync(request);

            // Kiểm tra trạng thái phản hồi
            if (!response.Success ||
                response.StatusCode != StatusCodes.SUCCESS ||
                response.Profile == null)
            {
                return false;
            }

            // Lấy thông tin từ Server
            UserProfile profile = response.Profile;

            tenDangNhap = profile.Username;
            hoTen = profile.FullName;
            email = profile.Email;
            loginTime = profile.LoginTime;

            // Cập nhật giao diện
            if (!IsDisposed && !Disposing)
            {
                HienThiThongTin();
            }

            return true;
        }


        private void SetupTabOrder()
        {
            if (btnLogout != null)
            {
                btnLogout.TabStop = true;
                btnLogout.TabIndex = 0;
            }

            if (btnMinimize != null)
                btnMinimize.TabStop = false;

            if (btnMaximize != null)
                btnMaximize.TabStop = false;

            if (btnExit != null)
                btnExit.TabStop = false;
        }

        private void MainForm_Resize(object sender, EventArgs e)
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
                dragFormPoint = this.Location;
            }
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && WindowState == FormWindowState.Normal)
            {
                Point diff = Point.Subtract(
                    Cursor.Position,
                    new Size(dragCursorPoint));

                this.Location = Point.Add(
                    dragFormPoint,
                    new Size(diff));
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

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            NetworkService.Disconnect();

            Application.Exit();
        }

        private async void btnLogout_Click_1(object sender, EventArgs e)
        {
            // Tránh nhấn nhiều lần
            if (!btnLogout.Enabled || isNavigating)
                return;

            // Xác nhận đăng xuất
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            // Khóa nút Đăng xuất
            btnLogout.Enabled = false;

            string originalText = btnLogout.Text;
            btnLogout.Text = "Đang đăng xuất...";

            try
            {

                NetworkRequest request =
                    new NetworkRequest
                    {
                        Action = Commands.DANGXUAT
                    };


                NetworkResponse response =
                    await NetworkService.SendAsync(request);

                if (response.Success &&
                    response.StatusCode ==
                        StatusCodes.NO_CONTENT)
                {
                    MessageBox.Show(
                        "Đăng xuất thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Không nhận được xác nhận đăng xuất " +
                        "từ Server. Phiên Client sẽ được đóng.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                NetworkService.Disconnect();


                QuayVeLoginForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi đăng xuất: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Dọn kết nối khi có lỗi
                NetworkService.Disconnect();

                QuayVeLoginForm();
            }
            finally
            {
                if (!IsDisposed && !Disposing &&
                    !isNavigating)
                {
                    btnLogout.Enabled = true;
                    btnLogout.Text = originalText;
                }
            }
        }
        private void QuayVeLoginForm()
        {
            // Không chuyển Form hai lần
            if (isNavigating)
                return;

            isNavigating = true;

            FormWindowState state = this.WindowState;

            LoginForm loginForm =
                new LoginForm(state);

            this.Hide();

            // Giữ cách chuyển Form của bài cũ
            loginForm.ShowDialog();

            this.Close();
        }


        private void panelCenter_Paint(object sender, PaintEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}