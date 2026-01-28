using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoNhanVienChiTiet : Form
    {
        private string _maNV;
        connectData cn = new connectData();

        public F_BaoCaoNhanVienChiTiet(string maNV)
        {
            InitializeComponent();
            _maNV = maNV;
        }

        // ================= FORM LOAD =================
        private void F_BaoCaoNhanVienChiTiet_Load(object sender, EventArgs e)
        {
            LoadThongTinNhanVien();
            LoadHopDongHienTai();
            LoadLichSuHopDong();
        }

        // ================= LOAD THÔNG TIN NHÂN VIÊN =================
        private void LoadThongTinNhanVien()
        {
            string sql = @"
        SELECT 
            nv.MaNV_TuanhCD233018 AS MaNV,
            nv.HoTen_TuanhCD233018 AS HoTen,
            nv.NgaySinh_TuanhCD233018 AS NgaySinh,
            nv.GioiTinh_TuanhCD233018 AS GioiTinh,
            nv.SoDienThoai_TuanhCD233018 AS DienThoai,
            nv.Email_TuanhCD233018 AS Email,
            cv.TenCV_KhangCD233181 AS ChucVu,
            pb.TenPB_ThuanCD233318 AS PhongBan
        FROM tblNhanVien_TuanhCD233018 nv
        JOIN tblChucVu_KhangCD233181 cv
            ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
        JOIN tblPhongBan_ThuanCD233318 pb
            ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
        WHERE nv.MaNV_TuanhCD233018 = @MaNV
          AND nv.DeletedAt_TuanhCD233018 = 0
          AND cv.DeletedAt_KhangCD233181 = 0
          AND pb.DeletedAt_ThuanCD233318 = 0";

            cn.connect();

            SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn);
            da.SelectCommand.Parameters.AddWithValue("@MaNV", _maNV);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cn.disconnect();

            if (dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];

            txtMaNV.Text = r["MaNV"].ToString();
            txtHoTen.Text = r["HoTen"].ToString();
            dtpNgaySinh.Value = Convert.ToDateTime(r["NgaySinh"]);
            txtGioiTinh.Text = r["GioiTinh"].ToString();
            txtSDT.Text = r["DienThoai"].ToString();
            txtEmail.Text = r["Email"].ToString();

            txtPhongBan.Text = r["PhongBan"].ToString();
            txtChucVu.Text = r["ChucVu"].ToString();

            // Vì CSDL không có:
            // txtTrangThaiLamViec.Text = "Đang làm";
        }


        // ================= LOAD HỢP ĐỒNG HIỆN TẠI =================
        private void LoadHopDongHienTai()
        {
            string sql = @"
                SELECT TOP 1 
                    NgayBatDau_ChienCD232928 AS NgayBatDau,
                    NgayKetThuc_ChienCD232928 AS NgayKetThuc
                FROM tblHopDong_ChienCD232928
                WHERE MaNV_TuanhCD233018 = @MaNV
                ORDER BY NgayKetThuc_ChienCD232928 DESC";

            cn.connect();

            SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn);
            da.SelectCommand.Parameters.AddWithValue("@MaNV", _maNV);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cn.disconnect();

            if (dt.Rows.Count == 0)
            {
                txtTrangThaiHD.Text = "Chưa có hợp đồng";
                txtTrangThaiHD.BackColor = Color.LightGray;
                return;
            }

            DateTime ngayBD = Convert.ToDateTime(dt.Rows[0]["NgayBatDau"]);
            DateTime ngayKT = Convert.ToDateTime(dt.Rows[0]["NgayKetThuc"]);

            dtpNgayBD.Value = ngayBD;
            dtpNgayKT.Value = ngayKT;

            if (ngayKT < DateTime.Now)
            {
                txtTrangThaiHD.Text = "Hết hạn";
                txtTrangThaiHD.BackColor = Color.MistyRose;
            }
            else if ((ngayKT - DateTime.Now).TotalDays <= 30)
            {
                txtTrangThaiHD.Text = "Sắp hết hạn";
                txtTrangThaiHD.BackColor = Color.LemonChiffon;
            }
            else
            {
                txtTrangThaiHD.Text = "Còn hiệu lực";
                txtTrangThaiHD.BackColor = Color.Honeydew;
            }
        }

        // ================= LOAD LỊCH SỬ HỢP ĐỒNG =================
        private void LoadLichSuHopDong()
        {
            string sql = @"
                SELECT  
                    MaHopDong_ChienCD232928 AS MaHopDong,
                    NgayBatDau_ChienCD232928 AS NgayBatDau,
                    NgayKetThuc_ChienCD232928 AS NgayKetThuc,
                    LoaiHopDong_ChienCD232928 AS LoaiHopDong,
                    GhiChu_ChienCD232928 AS GhiChu
                FROM tblHopDong_ChienCD232928
                WHERE MaNV_TuanhCD233018 = @MaNV
                ORDER BY NgayBatDau_ChienCD232928 DESC";

            cn.connect();

            SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn);
            da.SelectCommand.Parameters.AddWithValue("@MaNV", _maNV);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cn.disconnect();

            dgvHopDong.DataSource = dt;
            dgvHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dt.Rows.Count == 0) return;

            dgvHopDong.Columns["MaHopDong"].HeaderText = "Mã HĐ";
            dgvHopDong.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
            dgvHopDong.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
            dgvHopDong.Columns["LoaiHopDong"].HeaderText = "Loại hợp đồng";
            dgvHopDong.Columns["GhiChu"].HeaderText = "Ghi chú";
        }

        // ================= BUTTON ĐÓNG =================
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
