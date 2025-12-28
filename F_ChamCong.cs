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
        //private void HienThiNhanVienVuaQuet(string maNV)
        //{
        //    if (string.IsNullOrEmpty(maNV)) return; // Kiểm tra null/rỗng

        //    try
        //    {
        //        cn.connect();

        //        string query = @"
        //            SELECT 
        //                cc.MaChamCong AS [Mã chấm công],
        //                nv.MaNV AS [Mã nhân viên],
        //                nv.HoTen AS [Họ tên],
        //                cc.Ngay AS [Ngày],
        //                CONVERT(VARCHAR(8), cc.GioVao, 108) AS [Giờ vào],
        //                CONVERT(VARCHAR(8), cc.GioVe, 108) AS [Giờ về],
        //                cc.Ghichu AS [Ghi chú]
        //            FROM tblNhanVien nv
        //            LEFT JOIN tblChamCong cc ON nv.MaNV = cc.MaNV AND cc.DeletedAt = 0
        //            WHERE nv.MaNV = @MaNV
        //            ORDER BY cc.Ngay DESC;
        //        ";

        //        using (SqlCommand cmd = new SqlCommand(query, cn.conn))
        //        {
        //            cmd.Parameters.AddWithValue("@MaNV", maNV);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);
        //            dtGridViewChamCong.DataSource = dt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi hiển thị nhân viên vừa quét: " + ex.Message);
        //    }
        //    finally
        //    {
        //        cn.disconnect();
        //    }
        //}

        // ===== QUÉT QR TỪ FILE ẢNH =====

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
        SELECT TOP 1 MaChamCong
        FROM tblChamCong
        WHERE MaChamCong LIKE 'CC%'
        ORDER BY Id DESC";

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

            try
            {
                if (string.IsNullOrEmpty(maNV)) return;

                cn.connect();

                // 1️⃣ Kiểm tra nhân viên còn làm việc
                string checkNV = @"
            SELECT 1
            FROM tblNhanVien nv
            INNER JOIN tblHopDong hd ON nv.MaNV = hd.MaNV
            WHERE nv.MaNV = @MaNV
              AND nv.DeletedAt = 0
              AND hd.DeletedAt = 0";

                using (SqlCommand cmd = new SqlCommand(checkNV, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    if (cmd.ExecuteScalar() == null)
                    {
                        MessageBox.Show("Nhân viên không tồn tại hoặc đã nghỉ việc!");
                        return;
                    }
                }

                // 2️⃣ Kiểm tra hôm nay đã chấm công chưa
                string checkCC = @"
            SELECT TOP 1 Id, GioVao, GioVe
            FROM tblChamCong
            WHERE MaNV = @MaNV
              AND Ngay = CAST(GETDATE() AS DATE)
              AND DeletedAt = 0
            ORDER BY Id DESC";

                DataTable dt = new DataTable();
                using (SqlCommand cmd = new SqlCommand(checkCC, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    new SqlDataAdapter(cmd).Fill(dt);
                }

                if (dt.Rows.Count == 0)
                {
                    // 🔹 CHẤM CÔNG VÀO
                    string insert = @"
                        INSERT INTO tblChamCong
                        (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu)
                        VALUES
                        (@MaChamCong, @MaNV, CAST(GETDATE() AS DATE), @GioVao, @GioVao, N'Chấm công vào')";

                    using (SqlCommand cmd = new SqlCommand(insert, cn.conn))
                    {
                        DateTime now = DateTime.Now;

                        cmd.Parameters.AddWithValue("@MaChamCong", GenerateMaChamCong(cn.conn));
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@GioVao", now);

                        cmd.ExecuteNonQuery();
                    }


                    MessageBox.Show("✅ Đã chấm công vào");
                }
                else if (dt.Rows[0]["GioVe"] == DBNull.Value)
                {
                    // 🔹 CHẤM CÔNG RA
                    string update = @"
                UPDATE tblChamCong
                SET GioVe = @GioVe,
                    Ghichu = N'Đã chấm công ra'
                WHERE Id = @Id";

                    using (SqlCommand cmd = new SqlCommand(update, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@GioVe", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Id", dt.Rows[0]["Id"]);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("✅ Đã chấm công ra");
                }
                else
                {
                    MessageBox.Show("⚠️ Hôm nay đã chấm công đầy đủ!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chấm công: " + ex.Message);
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
            cc.MaChamCong   AS N'Mã chấm công',
            nv.MaNV         AS N'Mã NV',
            nv.HoTen        AS N'Họ tên',
            cc.Ngay         AS N'Ngày',
            CONVERT(varchar(8), cc.GioVao, 108) AS N'Giờ vào',
            CONVERT(varchar(8), cc.GioVe, 108)  AS N'Giờ về',
            cc.GhiChu       AS N'Ghi chú'
        FROM tblChamCong cc
        JOIN tblNhanVien nv ON cc.MaNV = nv.MaNV
        WHERE cc.MaNV = @MaNV
          AND cc.Ngay = CAST(GETDATE() AS DATE)
          AND cc.DeletedAt = 0
        ORDER BY cc.Id DESC";

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
