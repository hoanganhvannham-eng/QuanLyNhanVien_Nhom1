using AForge.Video.DirectShow;
using AForge.Video;
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
using DocumentFormat.OpenXml.Spreadsheet;
using static QuanLyNhanVien3.F_FormMain;

namespace QuanLyNhanVien3
{
    public partial class F_DangNhap : Form
    {
        public F_DangNhap()
        {
            InitializeComponent();
        }
        connectData cn = new connectData();
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        private void DangNhap_Load(object sender, EventArgs e)
        {
            tbpassword.UseSystemPasswordChar = true;
            //StartCamera();
        }

        private void btndangnhap_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string username = tbusename.Text.Trim();
                string password = tbpassword.Text.Trim();

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu để đăng nhập",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }

                // ✅ SỬ DỤNG PARAMETERIZED QUERY ĐỂ TRÁNH SQL INJECTION
                string query = @"SELECT tk.MaTK_KhangCD233181, tk.SoDienThoai_KhangCD233181, tk.Quyen_KhangCD233181, 
                                        nv.HoTen_TuanhCD233018, r.TenRole_ThuanCD233318
                                FROM tblTaiKhoan_KhangCD233181 tk
                                INNER JOIN tblNhanVien_TuanhCD233018 nv ON tk.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                                LEFT JOIN tblRole_ThuanCD233318 r ON tk.RoleId_ThuanCD233318 = r.Id_ThuanCD233318
                                WHERE tk.DeletedAt_KhangCD233181 = 0 
                                  AND tk.SoDienThoai_KhangCD233181 = @Username 
                                  AND tk.MatKhau_KhangCD233181 = @Password";

                SqlCommand cmd = new SqlCommand(query, cn.conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // ✅ LƯU THÔNG TIN ĐĂNG NHẬP
                    F_FormMain.LoginInfo.CurrentUserName = reader["HoTen_TuanhCD233018"].ToString();
                    F_FormMain.LoginInfo.CurrentUserRole = reader["Quyen_KhangCD233181"].ToString();

                    reader.Close();

                    // ✅ MỞ FORM CHÍNH
                    this.Hide();
                    F_FormMain f_Main = new F_FormMain();
                    StopCamera();
                    f_Main.ShowDialog();
                    f_Main = null;

                    // ✅ RESET VÀ HIỂN THỊ LẠI FORM ĐĂNG NHẬP
                    tbusename.Text = "";
                    tbpassword.Text = "";
                    this.Show();
                    this.Close();
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng! Vui lòng nhập lại",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbpassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn thoát không?",
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (rs == DialogResult.Yes)
            {
                StopCamera();
                this.Close();
            }
        }

        private void checkshowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkshowpassword.Checked)
            {
                // Hiển thị mật khẩu
                tbpassword.UseSystemPasswordChar = false;
            }
            else
            {
                // Ẩn mật khẩu
                tbpassword.UseSystemPasswordChar = true;
            }
        }

        // ===== QUẢN LÝ CAMERA =====
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
                if (pictureBoxQR.Image != null)
                {
                    pictureBoxQR.Image.Dispose();
                    pictureBoxQR.Image = null;
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

        private void StartCamera()
        {
            try
            {
                // Dừng camera cũ trước khi bật mới
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource = null;
                }

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

                timer1.Start(); // Bắt đầu quét QR
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bật camera: " + ex.Message);
            }
        }

        // ===== Hiển thị video từ camera lên PictureBox =====
        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

                if (pictureBoxQR.Image != null)
                {
                    var oldImage = pictureBoxQR.Image;
                    pictureBoxQR.Image = null;
                    oldImage.Dispose();
                }

                pictureBoxQR.Image = bitmap;
            }
            catch { }
        }

        // ===== Quét QR mỗi giây =====
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBoxQR.Image == null) return;

            Bitmap snapshot = null;

            try
            {
                lock (pictureBoxQR)
                {
                    snapshot = new Bitmap(pictureBoxQR.Image);
                }
            }
            catch
            {
                return;
            }

            if (snapshot == null) return;

            try
            {
                BarcodeReader reader = new BarcodeReader
                {
                    Options = new ZXing.Common.DecodingOptions
                    {
                        CharacterSet = "UTF-8"
                    }
                };

                var result = reader.Decode(snapshot);

                if (result != null)
                {
                    timer1.Stop();

                    string maNV = result.Text.Trim();

                    // Đăng nhập bằng QR
                    DangNhapBangQR(maNV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi quét QR: " + ex.Message);
            }
            finally
            {
                snapshot?.Dispose();
            }
        }

        // ===== Đăng nhập bằng mã QR =====
        private void DangNhapBangQR(string maNV)
        {
            try
            {
                cn.connect();

                string sql = @"SELECT tk.SoDienThoai_KhangCD233181, tk.Quyen_KhangCD233181, 
                                      nv.HoTen_TuanhCD233018, r.TenRole_ThuanCD233318
                               FROM tblTaiKhoan_KhangCD233181 tk
                               INNER JOIN tblNhanVien_TuanhCD233018 nv ON tk.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                               LEFT JOIN tblRole_ThuanCD233318 r ON tk.RoleId_ThuanCD233318 = r.Id_ThuanCD233318
                               WHERE tk.MaNV_TuanhCD233018 = @MaNV 
                                 AND tk.DeletedAt_KhangCD233181 = 0
                                 AND nv.DeletedAt_TuanhCD233018 = 0";

                SqlCommand cmd = new SqlCommand(sql, cn.conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // ✅ LƯU THÔNG TIN ĐĂNG NHẬP
                    F_FormMain.LoginInfo.CurrentUserName = reader["HoTen_TuanhCD233018"].ToString();
                    F_FormMain.LoginInfo.CurrentUserRole = reader["Quyen_KhangCD233181"].ToString();

                    reader.Close();

                    // ✅ DỪNG CAMERA VÀ MỞ FORM CHÍNH
                    StopCamera();
                    this.Hide();

                    F_FormMain frm = new F_FormMain();
                    frm.ShowDialog();
                    frm = null;

                    // ✅ RESET VÀ HIỂN THỊ LẠI
                    tbusename.Text = "";
                    tbpassword.Text = "";
                    this.Show();
                    this.Close();
                }
                else
                {
                    reader.Close();
                    MessageBox.Show("Không tìm thấy tài khoản cho mã QR này!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Khởi động lại camera để quét tiếp
                    StartCamera();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập QR: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnTaiAnhQR_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Dừng camera cũ nếu đang chạy
                if (videoSource != null && videoSource.IsRunning)
                {
                    timer1.Stop();
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource = null;
                }

                // 2. Xóa hình ảnh cũ để tránh hiển thị ảnh cũ
                if (pictureBoxQR.Image != null)
                {
                    pictureBoxQR.Image.Dispose();
                    pictureBoxQR.Image = null;
                }

                // 3. Khởi động lại camera
                StartCamera();

                MessageBox.Show("Camera đã được làm mới!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi refresh camera: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== Tắt camera khi đóng form =====
        private void F_DangNhapQR_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // Gọi hàm timer1_Tick chính
            timer1_Tick(sender, e);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Xử lý sự kiện click vào link label (nếu cần)
        }
    }
}