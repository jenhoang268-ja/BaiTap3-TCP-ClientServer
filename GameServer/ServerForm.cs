using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer
{
    public partial class ServerForm : Form
    {
        // Đối tượng TCP Server
        private GameTcpServer? server;

        // Timer cập nhật giao diện
        private readonly System.Windows.Forms.Timer timer;

        // Theo dõi thời gian hoạt động
        private readonly Stopwatch stopwatch =
            new Stopwatch();

        // Trạng thái đóng và dừng
        private bool isStopping = false;
        private bool closeRequested = false;
        private bool allowClose = false;
        public ServerForm()
        {
            InitializeComponent();

            // Chỉ cài đặt DataGridView
            CaiDatDataGridView();

            // Trạng thái ban đầu
            lblStatus.Text = "Chưa chạy";
            lblStatus.ForeColor = Color.Red;

            lblPort.Text = "8006";
            lblClientCount.Text = "0";
            lblUpTime.Text = "00:00:00";

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            // Cấu hình Log
            txtLog.Multiline = true;
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;

            // Timer cập nhật mỗi giây
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;

            timer.Tick += Timer_Tick;

            // Gắn sự kiện nút
            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
            btnClearlog.Click += btnClearlog_Click;

            // Gắn sự kiện đóng cửa sổ
            this.FormClosing += ServerForm_FormClosing;

            GhiLogLocal("Khởi tạo GameServer.");
            GhiLogLocal(
                "Nhấn Bắt đầu Server để lắng nghe.");

        }
        private void CaiDatDataGridView()
        {
            dgvClients.ReadOnly = true;

            dgvClients.AllowUserToAddRows = false;
            dgvClients.AllowUserToDeleteRows = false;

            dgvClients.AllowUserToResizeRows = false;
            dgvClients.AllowUserToOrderColumns = false;

            dgvClients.RowHeadersVisible = false;

            dgvClients.MultiSelect = false;

            dgvClients.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvClients.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvClients.BackgroundColor = Color.White;

            dgvClients.BorderStyle =
                BorderStyle.FixedSingle;

            dgvClients.ColumnHeadersHeight = 40;

            dgvClients.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode
                    .DisableResizing;

            dgvClients.ColumnHeadersDefaultCellStyle
                .Font = new Font(
                    "Times New Roman",
                    11,
                    FontStyle.Bold);

            // Tạo các cột
            dgvClients.Columns.Clear();

            dgvClients.Columns.Add(
                "STT", "STT");

            dgvClients.Columns.Add(
                "ClientId", "ClientId");

            dgvClients.Columns.Add(
                "Username", "Username");

            dgvClients.Columns.Add(
                "Address", "Địa chỉ (IP:Port)");

            dgvClients.Columns.Add(
                "ConnectedAt",
                "Thời điểm kết nối");

            // Tỷ lệ kích thước cột
            dgvClients.Columns["STT"]!
                .FillWeight = 35;

            dgvClients.Columns["ClientId"]!
                .FillWeight = 90;

            dgvClients.Columns["Username"]!
                .FillWeight = 100;

            dgvClients.Columns["Address"]!
                .FillWeight = 130;

            dgvClients.Columns["ConnectedAt"]!
                .FillWeight = 140;

            // Căn giữa STT
            dgvClients.Columns["STT"]!
                .DefaultCellStyle.Alignment =
                    DataGridViewContentAlignment
                        .MiddleCenter;

            dgvClients.ClearSelection();
        }

        private void GhiLogLocal(string message)
        {
            GhiLog($"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        private void GhiLog(string message)
        {
            if (IsDisposed || Disposing)
                return;

            // Xử lý khi gọi từ luồng TCP
            if (InvokeRequired)
            {
                if (!IsHandleCreated)
                    return;

                try
                {
                    BeginInvoke(
                        new Action(() =>
                            GhiLog(message)));
                }
                catch (ObjectDisposedException)
                {
                    // Form đã đóng
                }
                catch (InvalidOperationException)
                {
                    // Form 
                }

                return;
            }

            txtLog.AppendText(message + Environment.NewLine);
        }


        private void btnStart_Click(object? sender, EventArgs e)
        {
            if (isStopping)
                return;

            if (server?.IsRunning == true)
                return;

            btnStart.Enabled = false;

            try
            {
                server = new GameTcpServer();

                // Nhận log từ GameTcpServer
                server.OnLog += GhiLog;

                // Nhận danh sách Client
                server.OnClientsChanged += CapNhatDanhSachClient;

                // Khởi động Server
                server.Start();

                // Bắt đầu đếm thời gian
                stopwatch.Restart();
                timer.Start();

                lblStatus.Text = "Đang chạy";
                lblStatus.ForeColor = Color.Green;

                lblPort.Text = "8006";
                lblClientCount.Text = "0";
                lblUpTime.Text = "00:00:00";

                dgvClients.Rows.Clear();

                btnStop.Enabled = true;

                GhiLogLocal(
                    "GameServer đã khởi động thành công.");
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khởi động";
                lblStatus.ForeColor = Color.Red;

                btnStart.Enabled = true;
                btnStop.Enabled = false;

                GhiLogLocal(
                    $"Lỗi khởi động: {ex.Message}");

                MessageBox.Show(
                    "Không thể khởi động Server. " +
                    "Hãy kiểm tra cổng 8006 và cấu hình hệ thống.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnStop_Click(object? sender, EventArgs e)
        {
            await DungServerAsync();
        }
        private async Task DungServerAsync()
        {
            if (isStopping)
                return;

            if (server == null ||
                !server.IsRunning)
                return;

            isStopping = true;

            btnStart.Enabled = false;
            btnStop.Enabled = false;

            lblStatus.Text = "Đang dừng...";
            lblStatus.ForeColor = Color.DarkOrange;

            try
            {
                // Dừng TCP Server
                await server.StopAsync();

                timer.Stop();
                stopwatch.Stop();
                stopwatch.Reset();

                lblStatus.Text = "Đã dừng";
                lblStatus.ForeColor = Color.Red;

                lblClientCount.Text = "0";
                lblUpTime.Text = "00:00:00";

                dgvClients.Rows.Clear();

                GhiLogLocal(
                    "Đã dừng Server và đóng các kết nối.");
            }
            catch (Exception ex)
            {
                GhiLogLocal(
                    $"Lỗi khi dừng Server: {ex.Message}");

                MessageBox.Show(
                    "Không thể dừng Server bình thường.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                isStopping = false;

                if (closeRequested && server?.IsRunning != true)
                {
                    allowClose = true;
                    this.Close();
                }
                else
                {
                    btnStart.Enabled = server?.IsRunning != true;

                    btnStop.Enabled = server?.IsRunning == true;
                }
            }
        }
        private void Timer_Tick(
            object? sender, EventArgs e)
        {
            if (server?.IsRunning != true)
                return;

            TimeSpan elapsed = stopwatch.Elapsed;

            lblUpTime.Text =
                $"{(int)elapsed.TotalHours:00}:" +
                $"{elapsed.Minutes:00}:" +
                $"{elapsed.Seconds:00}";

            lblClientCount.Text =
                server.ClientCount.ToString();
        }

        private void CapNhatDanhSachClient(List<ClientInfo> danhSach)
        {
            if (IsDisposed || Disposing)
                return;

            // Kiểm tra luồng gọi
            if (InvokeRequired)
            {
                if (!IsHandleCreated)
                    return;

                try
                {
                    BeginInvoke(new Action(() =>
                        CapNhatDanhSachClient(danhSach)));
                }
                catch (ObjectDisposedException)
                {
                    // Form đã bị Dispose
                }
                catch (InvalidOperationException)
                {
                    // Form không còn khả dụng
                }

                return;
            }

            // Xóa danh sách cũ
            dgvClients.Rows.Clear();

            int stt = 1;

            // Hiển thị Client đang kết nối
            foreach (ClientInfo client in danhSach)
            {
                dgvClients.Rows.Add(
                    stt++,
                    client.ClientId.ToString("N")[..8],
                    client.Username,
                    client.Address,
                    client.ConnectedAt.ToString("dd/MM/yyyy HH:mm:ss"));
            }

            // Cập nhật tổng số Client
            lblClientCount.Text =
                danhSach.Count.ToString();

            dgvClients.ClearSelection();
        }


        private void btnClearlog_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
        }
        private async void ServerForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Đã dừng Server, được phép đóng
            if (allowClose)
            {
                timer.Stop();
                return;
            }

            // Đang dừng thì chưa đóng ngay
            if (isStopping)
            {
                e.Cancel = true;
                closeRequested = true;
                return;
            }

            // Server đang chạy
            if (server?.IsRunning == true)
            {
                e.Cancel = true;
                closeRequested = true;

                await DungServerAsync();
                return;
            }

            timer.Stop();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvClients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grbLog_Enter(object sender, EventArgs e)
        {

        }

        private void grbDK_Enter(object sender, EventArgs e)
        {

        }
    }
}
