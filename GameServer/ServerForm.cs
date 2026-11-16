using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer
{
    public partial class ServerForm : Form
    {
        private GameTcpServer? server;

        private readonly System.Windows.Forms.Timer timer;
        private readonly Stopwatch stopwatch = new Stopwatch();

        private bool isStopping = false;
        private bool closeRequested = false;
        private bool allowClose = false;

        // Lưu các lỗi JSON mà Server đã ghi nhận
        private readonly List<JsonErrorInfo> danhSachLoiJson = new();
        private readonly object lockLoiJson = new();

        public ServerForm()
        {
            InitializeComponent();

            CaiDatDataGridView();

            lblStatus.Text = "Chưa chạy";
            lblStatus.ForeColor = Color.Red;

            lblPort.Text = "8006";
            lblClientCount.Text = "0";
            lblUpTime.Text = "00:00:00";

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            txtLog.Multiline = true;
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
            btnClearlog.Click += btnClearlog_Click;
            // btnLoiJSON.Click đã được Designer gắn sẵn, không gắn lại

            this.FormClosing += ServerForm_FormClosing;

            GhiLogLocal("Khởi tạo GameServer.");
            GhiLogLocal("Nhấn Bắt đầu Server để lắng nghe.");
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
            dgvClients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClients.BackgroundColor = Color.White;
            dgvClients.BorderStyle = BorderStyle.FixedSingle;
            dgvClients.ColumnHeadersHeight = 40;
            dgvClients.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvClients.ColumnHeadersDefaultCellStyle.Font =
                new Font("Times New Roman", 11, FontStyle.Bold);

            dgvClients.Columns.Clear();
            dgvClients.Columns.Add("STT", "STT");
            dgvClients.Columns.Add("ClientId", "ClientId");
            dgvClients.Columns.Add("Username", "Username");
            dgvClients.Columns.Add("Address", "Địa chỉ (IP:Port)");
            dgvClients.Columns.Add("ConnectedAt", "Thời điểm kết nối");

            dgvClients.Columns["STT"]!.FillWeight = 35;
            dgvClients.Columns["ClientId"]!.FillWeight = 90;
            dgvClients.Columns["Username"]!.FillWeight = 100;
            dgvClients.Columns["Address"]!.FillWeight = 130;
            dgvClients.Columns["ConnectedAt"]!.FillWeight = 140;

            dgvClients.Columns["STT"]!.DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;

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

            if (InvokeRequired)
            {
                if (!IsHandleCreated)
                    return;

                try
                {
                    BeginInvoke(new Action(() => GhiLog(message)));
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }

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

                server.OnLog += GhiLog;
                server.OnClientsChanged += CapNhatDanhSachClient;
                server.OnJsonError += GhiNhanLoiJson;

                server.Start();

                stopwatch.Restart();
                timer.Start();

                lblStatus.Text = "Đang chạy";
                lblStatus.ForeColor = Color.Green;

                lblPort.Text = "8006";
                lblClientCount.Text = "0";
                lblUpTime.Text = "00:00:00";

                dgvClients.Rows.Clear();

                btnStop.Enabled = true;

                GhiLogLocal("GameServer đã khởi động thành công.");
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Lỗi khởi động";
                lblStatus.ForeColor = Color.Red;

                btnStart.Enabled = true;
                btnStop.Enabled = false;

                GhiLogLocal($"Lỗi khởi động: {ex.Message}");

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

            if (server == null || !server.IsRunning)
                return;

            isStopping = true;

            btnStart.Enabled = false;
            btnStop.Enabled = false;

            lblStatus.Text = "Đang dừng...";
            lblStatus.ForeColor = Color.DarkOrange;

            try
            {
                await server.StopAsync();

                timer.Stop();
                stopwatch.Stop();
                stopwatch.Reset();

                lblStatus.Text = "Đã dừng";
                lblStatus.ForeColor = Color.Red;

                lblClientCount.Text = "0";
                lblUpTime.Text = "00:00:00";

                dgvClients.Rows.Clear();

                GhiLogLocal("Đã dừng Server và đóng các kết nối.");
            }
            catch (Exception ex)
            {
                GhiLogLocal($"Lỗi khi dừng Server: {ex.Message}");

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

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (server?.IsRunning != true)
                return;

            TimeSpan elapsed = stopwatch.Elapsed;

            lblUpTime.Text =
                $"{(int)elapsed.TotalHours:00}:" +
                $"{elapsed.Minutes:00}:" +
                $"{elapsed.Seconds:00}";

            lblClientCount.Text = server.ClientCount.ToString();
        }

        private void CapNhatDanhSachClient(List<ClientInfo> danhSach)
        {
            if (IsDisposed || Disposing)
                return;

            if (InvokeRequired)
            {
                if (!IsHandleCreated)
                    return;

                try
                {
                    BeginInvoke(new Action(() => CapNhatDanhSachClient(danhSach)));
                }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }

                return;
            }

            dgvClients.Rows.Clear();

            int stt = 1;

            foreach (ClientInfo client in danhSach)
            {
                dgvClients.Rows.Add(
                    stt++,
                    client.ClientId.ToString("N")[..8],
                    client.Username,
                    client.Address,
                    client.ConnectedAt.ToString("dd/MM/yyyy HH:mm:ss"));
            }

            lblClientCount.Text = danhSach.Count.ToString();

            dgvClients.ClearSelection();
        }

        private void btnClearlog_Click(object? sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private async void ServerForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {
                timer.Stop();
                return;
            }

            if (isStopping)
            {
                e.Cancel = true;
                closeRequested = true;
                return;
            }

            if (server?.IsRunning == true)
            {
                e.Cancel = true;
                closeRequested = true;

                await DungServerAsync();
                return;
            }

            timer.Stop();
        }

        // ===================== LỖI JSON =====================

        // Chạy ở luồng TCP nên chỉ lưu vào list (có lock)
        private void GhiNhanLoiJson(JsonErrorInfo error)
        {
            lock (lockLoiJson)
            {
                danhSachLoiJson.Add(error);
            }
        }

        // Nút Lỗi JSON: Server tự gửi các trường hợp lỗi lên chính nó
       

        // Các hàm sự kiện rỗng do Designer tạo
        private void ServerForm_Load(object sender, EventArgs e) { }
        private void txtLog_TextChanged(object sender, EventArgs e) { }
        private void dgvClients_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void grbLog_Enter(object sender, EventArgs e) { }
        private void grbDK_Enter(object sender, EventArgs e) { }
    }
}