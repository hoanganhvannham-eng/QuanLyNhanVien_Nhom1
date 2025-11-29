using AForge.Video;
using AForge.Video.DirectShow;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace QuanLyNhanVien3
{
    public partial class F_ChamCong : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        connectData cn = new connectData(); // Class kết nối SQL của bạn

        public F_ChamCong()
        {
            InitializeComponent();
        }

        // ===== XÓA INPUT =====
        //private void ClearAllInputs(Control parent)
        //{
        //    foreach (Control ctl in parent.Controls)
        //    {
        //        if (ctl is TextBox)
        //            ((TextBox)ctl).Clear();
        //        else if (ctl is ComboBox)
        //            ((ComboBox)ctl).SelectedIndex = -1;
        //        else if (ctl is DateTimePicker)
        //            ((DateTimePicker)ctl).Value = DateTime.Now;
        //        else if (ctl.HasChildren)
        //            ClearAllInputs(ctl);
        //    }
        //}

        // ===== LOAD DỮ LIỆU CHẤM CÔNG =====
        private void LoadDataChamCong()
        {
            try
            {
                cn.connect();

                string sql = @"SELECT MaChamCong as 'Mã chấm công' , MaNV as 'Mã nhân viên', Ngay as 'Ngày', CONVERT(VARCHAR(8), GioVao, 108) as 'Giờ vào',  CONVERT(VARCHAR(8), GioVe, 108) as ' Giờ về', Ghichu as 'Ghi chú'
                    FROM tblChamCong
                    WHERE DeletedAt = 0
                    ORDER BY Ngay DESC";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewChamCong.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu chấm công: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== LOAD NHÂN VIÊN VÀO COMBOBOX =====
        private void LoadcomboBox()
        {
            try
            {
                cn.connect();
                string sql = "SELECT MaNV, HoTen FROM tblNhanVien WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ccBoxMaNV.DataSource = dt;
                    ccBoxMaNV.DisplayMember = "HoTen";
                    ccBoxMaNV.ValueMember = "MaNV";
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load mã nhân viên: " + ex.Message);
            }
        }


        // ===== FORM LOAD =====
        private void F_ChamCong_Load(object sender, EventArgs e)
        {
            LoadcomboBox();
            LoadDataChamCong();
        }

        private void btnChamCong_Click(object sender, EventArgs e)
        {
            isChamCongMode = true;  // Chế độ chấm công
            StartCamera();          // Bật camera
        }

        // ======== Nút Chọn Ảnh QR từ file - Chỉ quét không chấm công ========
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh QR Code";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Hiển thị ảnh vừa chọn lên PictureBox
                        pictureBoxChamCong.Image = Image.FromFile(ofd.FileName);

                        // Giải mã QR
                        BarcodeReader reader = new BarcodeReader();
                        var result = reader.Decode((Bitmap)pictureBoxChamCong.Image);

                        if (result != null)
                        {
                            string maNV = result.Text.Trim();

                            // Kiểm tra mã NV có tồn tại trong CSDL không
                            cn.connect();
                            string query = "SELECT nv.MaNV, nv.HoTen FROM  tblNhanVien as nv , tblHopDong as hd WHERE nv.MaNV = @MaNV and nv.MaNV = hd.MaNV and hd.DeletedAt = 0";
                            using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                            {
                                cmd.Parameters.AddWithValue("@MaNV", maNV);

                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    // Load dữ liệu vào ComboBox
                                    ccBoxMaNV.DataSource = dt;
                                    ccBoxMaNV.DisplayMember = "HoTen";   // Hiển thị tên nhân viên
                                    ccBoxMaNV.ValueMember = "MaNV";      // Giá trị là mã nhân viên
                                    ccBoxMaNV.SelectedValue = maNV;

                                    MessageBox.Show("Đã quét thành công mã nhân viên: " + maNV,
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Mã nhân viên không tồn tại hoặc đã nghỉ việc!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không nhận diện được mã QR!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi chọn ảnh: " + ex.Message,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        cn.disconnect();
                    }
                }
            }
        }



        // tesst cam ==========================================
        private bool isChamCongMode = false;  // true = Chấm công, false = Chỉ quét mã
        private string scannedMaNV = null;    // Lưu mã nhân viên quét được
        private void LoadNhanVienToComboBox(string maNV)
        {
            try
            {
                cn.connect();
                string query = "SELECT nv.MaNV, nv.HoTen FROM  tblNhanVien as nv , tblHopDong as hd WHERE nv.MaNV = @MaNV and nv.MaNV = hd.MaNV and hd.DeletedAt = 0";

                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ccBoxMaNV.DataSource = dt;
                        ccBoxMaNV.DisplayMember = "HoTen";
                        ccBoxMaNV.ValueMember = "MaNV";
                        ccBoxMaNV.SelectedValue = maNV;
                        MessageBox.Show($"Đã quét thành công!\nMã nhân viên: {scannedMaNV}",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên hoặc nhân viên đã nghỉ việc!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void StartCamera()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy camera!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Chọn camera đầu tiên
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();

                timer1.Start(); // Timer quét QR liên tục
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bật camera: " + ex.Message);
            }
        }
        // sop camenra 
        private void StopCamera()
        {
            try
            {
                // 🔹 Dừng camera nếu đang chạy
                if (videoSource != null)
                {
                    if (videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();  // Yêu cầu camera dừng
                        videoSource.WaitForStop();   // Đợi camera dừng hẳn
                    }

                    videoSource.NewFrame -= VideoSource_NewFrame; // Gỡ sự kiện frame
                    videoSource = null; // Giải phóng đối tượng
                }

                // 🔹 Dừng Timer quét QR
                if (timer1.Enabled)
                    timer1.Stop();

                // 🔹 Giải phóng hình ảnh trong PictureBox
                if (pictureBoxChamCong.Image != null)
                {
                    pictureBoxChamCong.Image.Dispose();
                    pictureBoxChamCong.Image = null;
                }

                GC.Collect();       // Thu gom rác .NET
                GC.WaitForPendingFinalizers(); // Đảm bảo giải phóng xong
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tắt camera: " + ex.Message,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra camera có ảnh chưa
                if (pictureBoxChamCong.Image == null) return;

                Bitmap snapshot;

                // 🔹 Tạo bản sao an toàn từ ảnh hiện tại
                lock (pictureBoxChamCong.Image)
                {
                    snapshot = new Bitmap(pictureBoxChamCong.Image);
                }

                // Khởi tạo BarcodeReader
                BarcodeReader reader = new BarcodeReader
                {
                    Options = new ZXing.Common.DecodingOptions
                    {
                        CharacterSet = "UTF-8"
                    }
                };

                // Decode từ bản sao
                var result = reader.Decode(snapshot);

                snapshot.Dispose(); // Giải phóng bộ nhớ sau khi decode

                if (result != null)
                {
                    timer1.Stop(); // Dừng quét để xử lý dữ liệu
                    StopCamera();  // Dừng camera tạm thời

                    //string maNV = result.Text.Trim();

                    scannedMaNV = result.Text.Trim(); // Lưu mã nhân viên quét được
                    if (!string.IsNullOrEmpty(scannedMaNV))
                    {
                        if (isChamCongMode)
                        {
                            // Chế độ chấm công → Quét và chấm công luôn
                            ChamCong(scannedMaNV);
                        }
                        else
                        {
                            // Chế độ chỉ quét mã → Hiển thị thông tin nhân viên
                            LoadNhanVienToComboBox(scannedMaNV);
                        }
                    }
                    //MessageBox.Show("Quét thành công: " + maNV);

                    //ChamCong(maNV); // Hàm lưu dữ liệu chấm công

                    //StartCamera(); // Mở lại camera
                    //timer1.Start();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Lỗi quét QR: " + ex.Message);
            }
        }
        private string GenerateMaChamCong()
        {
            string newMa = "CC001"; // Mặc định nếu chưa có dữ liệu
            string query = "SELECT TOP 1 MaChamCong FROM tblChamCong ORDER BY Id DESC";

            using (SqlConnection conn = new SqlConnection(cn.conn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string lastMa = result.ToString();  // VD: "CC0005"
                    int number = int.Parse(lastMa.Substring(2)); // Lấy phần số: 0005 -> 5
                    number++;
                    newMa = "CC" + number.ToString("D3"); // Format về 4 chữ số: 0006
                }
            }
            return newMa;
        }


        // ===== Hiển thị video từ camera lên PictureBox =====
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

                if (pictureBoxChamCong.Image != null)
                {
                    var oldImage = pictureBoxChamCong.Image;
                    pictureBoxChamCong.Image = null;
                    oldImage.Dispose();
                }

                pictureBoxChamCong.Image = bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi camera: " + ex.Message);
            }
        }

        // ======== Hàm Chấm Công Check-in / Check-out ========
        public void ChamCong(string maNV)
        {
            try
            {
                cn.connect();

                // ========================== 🔹 BƯỚC 1: KIỂM TRA NHÂN VIÊN ==========================
                string checkNVQuery = @"
                                    SELECT nv.DeletedAt 
                                    FROM tblNhanVien AS nv 
                                    INNER JOIN tblHopDong AS hd ON nv.MaNV = hd.MaNV
                                    WHERE nv.MaNV = @MaNV AND hd.DeletedAt = 0";

                using (SqlCommand cmdNV = new SqlCommand(checkNVQuery, cn.conn))
                {
                    cmdNV.Parameters.AddWithValue("@MaNV", maNV);
                    object result = cmdNV.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show($"Không tìm thấy nhân viên hoặc hợp đồng không tồn tại!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // ⛔ Dừng luôn
                    }

                    int deletedAt = Convert.ToInt32(result);

                    if (deletedAt != 0)
                    {
                        MessageBox.Show($"Nhân viên này đã nghỉ việc!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return; // ⛔ Dừng luôn
                    }
                }

                // ========================== 🔹 BƯỚC 2: XỬ LÝ CHẤM CÔNG ==========================
                string checkQuery = @"
                                    SELECT TOP 1 * 
                                    FROM tblChamCong
                                    WHERE MaNV = @MaNV AND Ngay = CAST(GETDATE() AS DATE)
                                    ORDER BY Id DESC";

                using (SqlCommand cmdCheck = new SqlCommand(checkQuery, cn.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MaNV", maNV);
                    SqlDataAdapter da = new SqlDataAdapter(cmdCheck);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Nếu chưa có bản ghi nào hôm nay -> INSERT
                    if (dt.Rows.Count == 0)
                    {
                        string maChamCong = GenerateMaChamCong();

                        string insertQuery = @"
                                            INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu)
                                            VALUES (@MaChamCong, @MaNV, CAST(GETDATE() AS DATE), CONVERT(TIME, GETDATE()), CONVERT(TIME, GETDATE()), N'Đi làm')";

                        using (SqlCommand cmdInsert = new SqlCommand(insertQuery, cn.conn))
                        {
                            cmdInsert.Parameters.AddWithValue("@MaChamCong", maChamCong);
                            cmdInsert.Parameters.AddWithValue("@MaNV", maNV);

                            if (cmdInsert.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show(
                                    $"Nhân viên {maNV} đã **check-in** thành công!\nThời gian vào: {DateTime.Now:HH:mm:ss}",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information
                                );
                            }
                        }
                    }
                    else
                    {
                        // Nếu đã có bản ghi trong ngày -> Update giờ về để lấy giờ quét cuối cùng
                        DataRow row = dt.Rows[0];

                        string updateQuery = @"
                UPDATE tblChamCong
                SET GioVe = CONVERT(TIME, GETDATE()),
                    Ghichu = N'Đã cập nhật giờ ra cuối cùng'
                WHERE Id = @Id";

                        using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, cn.conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@Id", row["Id"]);

                            if (cmdUpdate.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show(
                                    $"Nhân viên {maNV} đã **cập nhật giờ ra** thành công!\nThời gian ra mới: {DateTime.Now:HH:mm:ss}",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information
                                );
                            }
                        }
                    }

                    LoadDataChamCong(); // Refresh danh sách chấm công
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chấm công: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }


        private void btnrestar_Click(object sender, EventArgs e)
        {
            LoadDataChamCong();
        }

        private void btnDungQuetCam_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private void btnQuetma_Click_1(object sender, EventArgs e)
        {
            isChamCongMode = false; // Chế độ chỉ quét mã
            StartCamera();          // Bật camera
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string maChamCong = GenerateMaChamCong();
                string query = @"INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu, DeletedAt)
                         VALUES (@MaChamCong, @MaNV, @Ngay, @GioVao, @GioVe, @Ghichu, 0)";

                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaChamCong", maChamCong);
                    cmd.Parameters.AddWithValue("@MaNV", ccBoxMaNV.SelectedValue);
                    cmd.Parameters.AddWithValue("@Ngay", dateTimeNgayChamCong.Value.Date);
                    cmd.Parameters.AddWithValue("@GioVao", TimeSpan.Parse(tbGioVao.Text));
                    cmd.Parameters.AddWithValue("@GioVe", TimeSpan.Parse(tbGioVe.Text));
                    cmd.Parameters.AddWithValue("@Ghichu", tbGhiChu.Text);

                    if (cmd.ExecuteNonQuery() > 0)
                        MessageBox.Show("Thêm dữ liệu thành công!");
                }

                LoadDataChamCong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtGridViewChamCong.CurrentRow == null) return;

                int id = Convert.ToInt32(dtGridViewChamCong.CurrentRow.Cells["Id"].Value);

                cn.connect();
                string query = @"UPDATE tblChamCong
                         SET MaNV = @MaNV, Ngay = @Ngay, GioVao = @GioVao, GioVe = @GioVe, Ghichu = @Ghichu
                         WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", ccBoxMaNV.SelectedValue);
                    cmd.Parameters.AddWithValue("@Ngay", dateTimeNgayChamCong.Value.Date);
                    cmd.Parameters.AddWithValue("@GioVao", TimeSpan.Parse(tbGioVao.Text));
                    cmd.Parameters.AddWithValue("@GioVe", TimeSpan.Parse(tbGioVe.Text));
                    cmd.Parameters.AddWithValue("@Ghichu", tbGhiChu.Text);
                    cmd.Parameters.AddWithValue("@Id", id);

                    if (cmd.ExecuteNonQuery() > 0)
                        MessageBox.Show("Cập nhật thành công!");
                }

                LoadDataChamCong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtGridViewChamCong.CurrentRow == null) return;

                int id = Convert.ToInt32(dtGridViewChamCong.CurrentRow.Cells["Id"].Value);

                cn.connect();
                string query = "UPDATE tblChamCong SET DeletedAt = 1 WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    if (cmd.ExecuteNonQuery() > 0)
                        MessageBox.Show("Xóa thành công!");
                }

                LoadDataChamCong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnxuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewChamCong.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("ChucVu");

                                // Ghi header
                                for (int i = 0; i < dtGridViewChamCong.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewChamCong.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewChamCong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewChamCong.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dtGridViewChamCong.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dtGridViewChamCong.Rows.Count + 1, dtGridViewChamCong.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // Tự động co giãn cột
                                ws.Columns().AdjustToContents();

                                // Lưu file
                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ccBoxMaNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã nhân viên để tìm kiếm!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
                cn.connect();
                string sql = @"SELECT Id, MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu
                   FROM tblChamCong
                   WHERE DeletedAt = 0
                     AND MaNV = @MaNV 
                   ORDER BY Id";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", ccBoxMaNV.SelectedValue);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewChamCong.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
                //MessageBox.Show("Chi tiết lỗi: " + ex.ToString(), "Lỗi hệ thống",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnNVDaNghiViec_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @"SELECT Id, MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu
                         FROM tblChamCong
                         WHERE DeletedAt = 1 ORDER BY Id";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewChamCong.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnKhoiPhucNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaChamCong.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã chấm công cần khôi phục!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                string checkSql = "SELECT COUNT(*) FROM tblChamCong WHERE MaChamCong = @MaChamCong AND DeletedAt = 1";
                using (SqlCommand cmdCheck = new SqlCommand(checkSql, cn.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MaChamCong", tbMaChamCong.Text.Trim());
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("Mã chấm công này không tồn tại trong danh sách đã xóa!",
                                        "Thông báo",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // Yêu cầu mật khẩu khôi phục
                if (string.IsNullOrEmpty(tbMKkhoiphuc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu để khôi phục!",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                string sqlCheckAdmin = "SELECT * FROM tblTaiKhoan WHERE Quyen = @Quyen AND MatKhau = @MatKhau";
                SqlCommand cmdAdmin = new SqlCommand(sqlCheckAdmin, cn.conn);
                cmdAdmin.Parameters.AddWithValue("@Quyen", "Admin");
                cmdAdmin.Parameters.AddWithValue("@MatKhau", tbMKkhoiphuc.Text);
                SqlDataReader reader = cmdAdmin.ExecuteReader();

                if (!reader.Read())
                {
                    MessageBox.Show("Mật khẩu không đúng! Vui lòng nhập lại.",
                                    "Thông báo",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    tbMKkhoiphuc.Text = "";
                    reader.Close();
                    cn.disconnect();
                    return;
                }
                reader.Close();

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn khôi phục bản chấm công này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    tbMKkhoiphuc.Text = "";
                    string query = "UPDATE tblChamCong SET DeletedAt = 0 WHERE MaChamCong = @MaChamCong";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChamCong", tbMaChamCong.Text.Trim());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Khôi phục bản chấm công thành công!",
                                            "Thông báo",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            LoadDataChamCong();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy bản chấm công để khôi phục!",
                                            "Thông báo",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                        }
                    }
                    cn.disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void checkshowpassword_CheckedChanged(object sender, EventArgs e)
        {
            tbMKkhoiphuc.UseSystemPasswordChar = !checkshowpassword.Checked;
        }

        private void dtGridViewChamCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbMaChamCong.Text = dtGridViewChamCong.Rows[i].Cells[0].Value.ToString();
                ccBoxMaNV.Text = dtGridViewChamCong.Rows[i].Cells[1].Value.ToString();
                dateTimeNgayChamCong.Value = Convert.ToDateTime(dtGridViewChamCong.Rows[i].Cells[2].Value);
                tbGioVao.Text = dtGridViewChamCong.Rows[i].Cells[3].Value.ToString();
                tbGioVe.Text = dtGridViewChamCong.Rows[i].Cells[4].Value.ToString();
                tbGhiChu.Text = dtGridViewChamCong.Rows[i].Cells[5].Value.ToString();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
