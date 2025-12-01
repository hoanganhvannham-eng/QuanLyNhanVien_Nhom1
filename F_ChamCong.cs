using AForge.Video;
using AForge.Video.DirectShow;
using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

namespace QuanLyNhanVien3
{
    public partial class F_ChamCong : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        connectData cn = new connectData(); // Class kết nối SQL

        private bool isChamCongMode = false;  // true = Chấm công, false = Chỉ quét mã
        private string scannedMaNV = null;    // Lưu mã nhân viên quét được

        public F_ChamCong()
        {
            InitializeComponent();
        }

        // ===== FORM LOAD =====
        private void F_ChamCong_Load(object sender, EventArgs e)
        {
        }

        // ===== LOAD NHÂN VIÊN VÀO COMBOBOX =====

        // ===== HIỂN THỊ NHÂN VIÊN VỪA QUÉT =====
        private void HienThiNhanVienVuaQuet(string maNV)
        {
            if (string.IsNullOrEmpty(maNV)) return; // Kiểm tra null/rỗng

            try
            {
                cn.connect();

                string query = @"
                    SELECT 
                        cc.MaChamCong AS [Mã chấm công],
                        nv.MaNV AS [Mã nhân viên],
                        nv.HoTen AS [Họ tên],
                        cc.Ngay AS [Ngày],
                        CONVERT(VARCHAR(8), cc.GioVao, 108) AS [Giờ vào],
                        CONVERT(VARCHAR(8), cc.GioVe, 108) AS [Giờ về],
                        cc.Ghichu AS [Ghi chú]
                    FROM tblNhanVien nv
                    LEFT JOIN tblChamCong cc ON nv.MaNV = cc.MaNV AND cc.DeletedAt = 0
                    WHERE nv.MaNV = @MaNV
                    ORDER BY cc.Ngay DESC;
                ";

                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewChamCong.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị nhân viên vừa quét: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // ===== QUÉT QR TỪ FILE ẢNH =====
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn ảnh QR Code";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() != DialogResult.OK) return;

                pictureBoxChamCong.Image = Image.FromFile(ofd.FileName);

                BarcodeReader reader = new BarcodeReader();
                var result = reader.Decode((Bitmap)pictureBoxChamCong.Image);

                if (result == null)
                {
                    MessageBox.Show("Không nhận diện được mã QR!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maNV = result.Text.Trim();
                if (string.IsNullOrEmpty(maNV)) return;

                // Kiểm tra nhân viên trong CSDL
                try
                {
                    cn.connect();
                    string query = @"
                        SELECT nv.MaNV, nv.HoTen
                        FROM tblNhanVien nv
                        INNER JOIN tblHopDong hd ON nv.MaNV = hd.MaNV
                        WHERE nv.MaNV = @MaNV AND hd.DeletedAt = 0
                    ";

                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Mã nhân viên không tồn tại hoặc đã nghỉ việc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }


                        // Hiển thị dữ liệu chấm công
                        HienThiNhanVienVuaQuet(maNV);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi quét ảnh: " + ex.Message);
                }
                finally
                {
                    cn.disconnect();
                }
            }
        }

        // ===== BẮT ĐẦU CAMERA =====
        private void btnChamCong_Click(object sender, EventArgs e)
        {
            isChamCongMode = true;  // Chế độ chấm công
            StartCamera();
        }

        private void StartCamera()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (videoDevices.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy camera!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();

                timer1.Start(); // Timer quét QR
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bật camera: " + ex.Message);
            }
        }

        private void StopCamera()
        {
            try
            {
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource.NewFrame -= VideoSource_NewFrame;
                    videoSource = null;
                }

                if (timer1.Enabled) timer1.Stop();

                if (pictureBoxChamCong.Image != null)
                {
                    pictureBoxChamCong.Image.Dispose();
                    pictureBoxChamCong.Image = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tắt camera: " + ex.Message);
            }
        }

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
            catch { }
        }

        // ===== TIMER QUÉT QR =====
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBoxChamCong.Image == null) return;

            Bitmap snapshot = null;

            try
            {
                lock (pictureBoxChamCong)
                {
                    // Clone ảnh an toàn
                    snapshot = new Bitmap(pictureBoxChamCong.Image);
                }
            }
            catch
            {
                return; // Nếu clone thất bại, thoát
            }

            if (snapshot == null) return;

            BarcodeReader reader = new BarcodeReader
            {
                Options = new ZXing.Common.DecodingOptions { CharacterSet = "UTF-8" }
            };

            var result = reader.Decode(snapshot);
            snapshot.Dispose();

            if (result == null) return;

            timer1.Stop();
            StopCamera();

            scannedMaNV = result.Text.Trim();
            if (string.IsNullOrEmpty(scannedMaNV)) return;

            if (CheckNhanVien(scannedMaNV))
            {
                if (isChamCongMode) ChamCong(scannedMaNV);
                HienThiNhanVienVuaQuet(scannedMaNV);
            }
            else
            {
                MessageBox.Show("Nhân viên không tồn tại hoặc đã nghỉ việc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ===== LOAD NHÂN VIÊN VÀO COMBOBOX THEO MA =====
        private void LoadNhanVienToComboBox(string maNV)
        {
            if (string.IsNullOrEmpty(maNV)) return;

            try
            {
                cn.connect();
                string query = @"
                    SELECT nv.MaNV, nv.HoTen
                    FROM tblNhanVien nv
                    INNER JOIN tblHopDong hd ON nv.MaNV = hd.MaNV
                    WHERE nv.MaNV = @MaNV AND hd.DeletedAt = 0
                ";

                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên hoặc nhân viên đã nghỉ việc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu nhân viên: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }
        private string GenerateMaChamCong(SqlConnection conn)
        {
            string newMa = "CC001";
            string query = "SELECT TOP 1 MaChamCong FROM tblChamCong ORDER BY Id DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    string lastMa = result.ToString();
                    int number = int.Parse(lastMa.Substring(2));
                    number++;
                    newMa = "CC" + number.ToString("D3");
                }
            }

            return newMa;
        }

        // ===== CHẤM CÔNG =====
        private void ChamCong(string maNV)
        {
            if (string.IsNullOrEmpty(maNV)) return;

            try
            {
                // Mở connection 1 lần
                cn.connect();

                // 1️⃣ Kiểm tra nhân viên
                string checkNVQuery = @"
            SELECT nv.DeletedAt
            FROM tblNhanVien nv
            INNER JOIN tblHopDong hd ON nv.MaNV = hd.MaNV
            WHERE nv.MaNV = @MaNV AND hd.DeletedAt = 0
        ";
                using (SqlCommand cmd = new SqlCommand(checkNVQuery, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    object result = cmd.ExecuteScalar();
                    int deletedAt = 0;
                    if (result != null && result != DBNull.Value)
                        deletedAt = Convert.ToInt32(result);

                    if (deletedAt != 0)
                    {
                        MessageBox.Show("Nhân viên không tồn tại hoặc đã nghỉ việc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // 2️⃣ Kiểm tra bản ghi chấm công hôm nay
                string checkQuery = @"SELECT TOP 1 * FROM tblChamCong WHERE MaNV = @MaNV AND Ngay = CAST(GETDATE() AS DATE) ORDER BY Id DESC";
                using (SqlCommand cmdCheck = new SqlCommand(checkQuery, cn.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MaNV", maNV);
                    SqlDataAdapter da = new SqlDataAdapter(cmdCheck);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        // Insert mới
                        string maChamCong = GenerateMaChamCong(cn.conn);
                        string insertQuery = @"
                    INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu)
                    VALUES (@MaChamCong, @MaNV, CAST(GETDATE() AS DATE), @GioVao, @GioVe, N'Đi làm')
                ";
                        using (SqlCommand cmdInsert = new SqlCommand(insertQuery, cn.conn))
                        {
                            cmdInsert.Parameters.AddWithValue("@MaChamCong", maChamCong);
                            cmdInsert.Parameters.AddWithValue("@MaNV", maNV);
                            cmdInsert.Parameters.AddWithValue("@GioVao", DateTime.Now);
                            cmdInsert.Parameters.AddWithValue("@GioVe", DateTime.Now);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Update giờ về
                        string updateQuery = @"UPDATE tblChamCong SET GioVe = @GioVe, Ghichu = N'Đã cập nhật giờ ra cuối cùng' WHERE Id = @Id";
                        using (SqlCommand cmdUpdate = new SqlCommand(updateQuery, cn.conn))
                        {
                            cmdUpdate.Parameters.AddWithValue("@GioVe", DateTime.Now);
                            cmdUpdate.Parameters.AddWithValue("@Id", dt.Rows[0]["Id"]);
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }
                }

                // 3️⃣ Hiển thị nhân viên vừa chấm công
                HienThiNhanVienVuaQuet(maNV);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chấm công: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect(); // đóng connection sau cùng
            }
        }



        // ===== TẠO MÃ CHẤM CÔNG MỚI =====
        private string GenerateMaChamCong()
        {
            string newMa = "CC001";
            try
            {
                cn.connect();
                string query = "SELECT TOP 1 MaChamCong FROM tblChamCong ORDER BY Id DESC";
                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        string lastMa = result.ToString();
                        int number = int.Parse(lastMa.Substring(2));
                        number++;
                        newMa = "CC" + number.ToString("D3");
                    }
                }
            }
            finally
            {
                cn.disconnect();
            }
            return newMa;
        }

        private bool CheckNhanVien(string maNV)
        {
            try
            {
                cn.connect();
                string query = @"
                    SELECT nv.MaNV
                    FROM tblNhanVien nv
                    LEFT JOIN tblHopDong hd ON nv.MaNV = hd.MaNV AND hd.DeletedAt = 0
                    WHERE nv.MaNV = @MaNV AND nv.DeletedAt = 0
                ";
                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    object result = cmd.ExecuteScalar();
                    return result != null;
                }
            }
            finally
            {
                cn.disconnect();
            }
        }

        // ===== NÚT DỪNG CAMERA =====
        private void btnDungQuetCam_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        // ===== THOÁT FORM =====
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
