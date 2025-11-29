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

namespace QuanLyNhanVien3
{
    public partial class F_DangNhap: Form
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
            StartCamera();
        }

        private void btndangnhap_Click(object sender, EventArgs e)
        {
            cn.connect();
            string username = tbusename.Text.Trim();
            string password = tbpassword.Text.Trim();
            string query = "select * from tblTaiKhoan where DeletedAt = 3 AND TenDangNhap = '" + username + "' " + "and MatKhau = '" + password + "'";
            SqlCommand cmd = new SqlCommand(query, cn.conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (tbusename.Text == "" || tbpassword.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu để đăng nhâp", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Question);
            }
            else if (reader.Read() == true)
            {
                this.Hide();
                F_FormMain f_Main = new F_FormMain();
                //MessageBox.Show("Đăng nhập thành công!",
                //                            "Thông báo");
                StopCamera();
                f_Main.ShowDialog();
                f_Main = null;
                tbpassword.Text = "";
                this.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng? Vui lòng nhập lại tài khoản hoặc mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                tbpassword.Text = "";
            }
            cn.disconnect();
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn muốn thoat khong?", "tieu de thoat",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                this.Close(); 
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    videoSource = null;
                }
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
        //chekc cam 

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
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pictureBoxQR.Image = bitmap;
        }

        // ===== Quét QR mỗi giây =====
        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        // ===== Đăng nhập bằng mã QR =====
        private void DangNhapBangQR(string maNV)
        {
            try
            {
                cn.connect();
                string sql = @"SELECT TenDangNhap, Quyen 
                               FROM tblTaiKhoan 
                               WHERE MaNV = @MaNV AND DeletedAt = 3";
                SqlCommand cmd = new SqlCommand(sql, cn.conn);
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    // Mở form chính
                    this.Hide();
                    F_FormMain frm = new F_FormMain();
                    frm.Show();
                    if (timer1.Enabled)
                        timer1.Stop();

                    if (videoSource != null && videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();
                        videoSource.WaitForStop();
                        videoSource = null;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài khoản cho mã QR này!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
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

        //tat cam 
        private void F_DangNhapQR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer1.Enabled)
                timer1.Stop();

            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (pictureBoxQR.Image == null) return;

            try
            {
                BarcodeReader reader = new BarcodeReader
                {
                    Options = new ZXing.Common.DecodingOptions
                    {
                        CharacterSet = "UTF-8"
                    }
                };

                var result = reader.Decode((Bitmap)pictureBoxQR.Image);

                if (result != null)
                {
                    timer1.Stop();
                    videoSource.SignalToStop();

                    string maNV = result.Text.Trim();
                    //cb.Text = maNV;

                    // Đăng nhập luôn
                    DangNhapBangQR(maNV);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi quét QR: " + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
