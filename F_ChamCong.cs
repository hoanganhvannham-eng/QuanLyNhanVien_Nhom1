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
            StartCamera();

            scannedMaNV = result.Text.Trim();
            if (string.IsNullOrEmpty(scannedMaNV)) return;

            if (CheckNhanVien(scannedMaNV))
            {
                if (isChamCongMode) ChamCong(scannedMaNV);
            }
            else
            {
                MessageBox.Show("Nhân viên không tồn tại hoặc đã nghỉ việc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GenerateMaChamCong(SqlConnection conn)
        {
            string newMa = "CC001";

            string query = @"
        SELECT TOP 1 MaChamCong_TuanhCD233018
        FROM tblChamCong_TuanhCD233018
        WHERE MaChamCong_TuanhCD233018 LIKE 'CC%'
        ORDER BY Id_TuanhCD233018 DESC";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    string lastMa = result.ToString().Trim();

                    // 👉 LẤY PHẦN SỐ Ở CUỐI CHUỖI
                    string so = "";
                    for (int i = lastMa.Length - 1; i >= 0; i--)
                    {
                        if (char.IsDigit(lastMa[i]))
                            so = lastMa[i] + so;
                        else
                            break;
                    }

                    if (int.TryParse(so, out int number))
                    {
                        number++;
                        newMa = "CC" + number.ToString("D3");
                    }
                }
            }

            return newMa;
        }


        // ===== CHẤM CÔNG =====
        private void ChamCong(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV)) return;

            try
            {
                cn.connect();

                // ===================== 1️⃣ LẤY ID NHÂN VIÊN TỪ MÃ NV =====================
                string sqlGetNhanVienId = @"
                    SELECT Id_TuanhCD233018
                    FROM tblNhanVien_TuanhCD233018
                    WHERE MaNV_TuanhCD233018 = @MaNV
                      AND DeletedAt_TuanhCD233018 = 0";

                int nhanVienId = 0;
                using (SqlCommand cmd = new SqlCommand(sqlGetNhanVienId, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("Nhân viên không tồn tại hoặc đã nghỉ việc!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    nhanVienId = Convert.ToInt32(result);
                }

                // ===================== 2️⃣ KIỂM TRA HỢP ĐỒNG =====================
                string sqlCheckHD = @"
                    SELECT COUNT(*)
                    FROM tblHopDong_ChienCD232928
                    WHERE MaNV_TuanhCD233018 = @MaNV
                      AND DeletedAt_ChienCD232928 = 0
                      AND (NgayKetThuc_ChienCD232928 IS NULL OR NgayKetThuc_ChienCD232928 >= GETDATE())";

                using (SqlCommand cmd = new SqlCommand(sqlCheckHD, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    int count = (int)cmd.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("Nhân viên không có hợp đồng hiệu lực!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // ===================== 3️⃣ LẤY CHẤM CÔNG CUỐI TRONG NGÀY =====================
                string sqlCheckCC = @"
                    SELECT TOP 1 Id_TuanhCD233018
                    FROM tblChamCong_TuanhCD233018
                    WHERE NhanVienId_TuanhCD233018 = @NhanVienId
                      AND Ngay_TuanhCD233018 = CAST(GETDATE() AS DATE)
                      AND DeletedAt_TuanhCD233018 = 0
                    ORDER BY Id_TuanhCD233018 DESC";

                object lastId;

                using (SqlCommand cmd = new SqlCommand(sqlCheckCC, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@NhanVienId", nhanVienId);
                    lastId = cmd.ExecuteScalar();
                }

                // ===================== 4️⃣ CHƯA CÓ → CHECK-IN =====================
                if (lastId == null)
                {
                    string insert = @"
                        INSERT INTO tblChamCong_TuanhCD233018
                        (MaChamCong_TuanhCD233018, Ngay_TuanhCD233018, GioVao_TuanhCD233018, GioVe_TuanhCD233018, Ghichu_TuanhCD233018, DeletedAt_TuanhCD233018, NhanVienId_TuanhCD233018)
                        VALUES
                        (@MaChamCong, CAST(GETDATE() AS DATE), CONVERT(TIME, GETDATE()), CONVERT(TIME, GETDATE()), N'Chấm công vào', 0, @NhanVienId)";

                    using (SqlCommand cmd = new SqlCommand(insert, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaChamCong", GenerateMaChamCong(cn.conn));
                        cmd.Parameters.AddWithValue("@NhanVienId", nhanVienId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(
                        $"✅ {maNV} đã CHECK-IN\nGiờ vào: {DateTime.Now:HH:mm:ss}",
                        "Chấm công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // ===================== 5️⃣ ĐÃ CÓ → UPDATE GIỜ RA =====================
                else
                {
                    string update = @"
                        UPDATE tblChamCong_TuanhCD233018
                        SET GioVe_TuanhCD233018 = CONVERT(TIME, GETDATE()),
                            Ghichu_TuanhCD233018 = N'Cập nhật giờ ra'
                        WHERE Id_TuanhCD233018 = @Id";

                    using (SqlCommand cmd = new SqlCommand(update, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", lastId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(
                        $"✅ {maNV} đã CẬP NHẬT GIỜ RA\nGiờ mới: {DateTime.Now:HH:mm:ss}",
                        "Chấm công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // ===================== 6️⃣ HIỂN THỊ NGAY LÊN GRID =====================
                HienThiChamCongHienTai(maNV);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chấm công: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void HienThiChamCongHienTai(string maNV)
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT 
                        cc.MaChamCong_TuanhCD233018   AS N'Mã chấm công',
                        nv.MaNV_TuanhCD233018         AS N'Mã NV',
                        nv.HoTen_TuanhCD233018        AS N'Họ tên',
                        cc.Ngay_TuanhCD233018         AS N'Ngày',
                        CONVERT(varchar(8), cc.GioVao_TuanhCD233018, 108) AS N'Giờ vào',
                        CONVERT(varchar(8), cc.GioVe_TuanhCD233018, 108)  AS N'Giờ về',
                        cc.Ghichu_TuanhCD233018       AS N'Ghi chú'
                    FROM tblChamCong_TuanhCD233018 cc
                    JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    WHERE nv.MaNV_TuanhCD233018 = @MaNV
                      AND cc.Ngay_TuanhCD233018 = CAST(GETDATE() AS DATE)
                      AND cc.DeletedAt_TuanhCD233018 = 0
                    ORDER BY cc.Id_TuanhCD233018 DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
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
                MessageBox.Show("Lỗi hiển thị chấm công: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private bool CheckNhanVien(string maNV)
        {
            try
            {
                cn.connect();
                string query = @"
                    SELECT nv.MaNV_TuanhCD233018
                    FROM tblNhanVien_TuanhCD233018 nv
                    LEFT JOIN tblHopDong_ChienCD232928 hd ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018 AND hd.DeletedAt_ChienCD232928 = 0
                    WHERE nv.MaNV_TuanhCD233018 = @MaNV AND nv.DeletedAt_TuanhCD233018 = 0
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