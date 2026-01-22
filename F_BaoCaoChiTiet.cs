using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoChiTiet : Form
    {
        connectData cn = new connectData();

        string loaiBaoCao;   // "DUAN" | "PHONGBAN" | "CHUCVU"
        string giaTri;
        DateTime tuNgay;
        DateTime denNgay;

        // ===============================
        // CONSTRUCTOR DUY NHẤT
        // ===============================
        public F_BaoCaoChiTiet(
            string _loaiBaoCao,
            string _giaTri,
            DateTime _tuNgay,
            DateTime _denNgay)
        {
            InitializeComponent();

            loaiBaoCao = _loaiBaoCao;
            giaTri = _giaTri;
            tuNgay = _tuNgay;
            denNgay = _denNgay;
        }

        // ===============================
        // LOAD FORM
        // ===============================
        private void F_BaoCaoChiTiet_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "BÁO CÁO CHI TIẾT";
            lblContext.Text = $"{giaTri} | {tuNgay:dd/MM/yyyy} - {denNgay:dd/MM/yyyy}";

            switch (loaiBaoCao)
            {
                case "DUAN":
                    LoadTheoDuAn();
                    break;

                case "PHONGBAN":
                    LoadTheoPhongBan();
                    break;

                case "CHUCVU":
                    LoadTheoChucVu();
                    break;
            }
        }

        // ===============================
        // 1. CHI TIẾT THEO DỰ ÁN
        // ===============================
        void LoadTheoDuAn()
        {
            string sql = @"
            SELECT 
                nv.MaNV_TuanhCD233018 AS MaNV,
                nv.HoTen_TuanhCD233018 AS HoTen,
                cv.TenCV_KhangCD233181 AS ChucVu,
                ct.VaiTro_KienCD233824 AS VaiTroDuAn,
                hd.NgayBatDau_ChienCD232928 AS NgayBatDauHD,
                hd.NgayKetThuc_ChienCD232928 AS NgayKetThucHD,
                CASE 
                    WHEN hd.NgayKetThuc_ChienCD232928 IS NULL 
                         OR hd.NgayKetThuc_ChienCD232928 >= GETDATE()
                    THEN N'Còn hạn'
                    ELSE N'Hết hạn'
                END AS TrangThaiHopDong
            FROM tblChiTietDuAn_KienCD233824 ct
            JOIN tblNhanVien_TuanhCD233018 nv
                ON ct.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
            JOIN tblDuAn_KienCD233824 da
                ON ct.MaDA_KienCD233824 = da.MaDA_KienCD233824
            JOIN tblChucVu_KhangCD233181 cv
                ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            LEFT JOIN tblHopDong_ChienCD232928 hd
                ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018
            WHERE da.TenDA_KienCD233824 = @GiaTri
              AND ct.DeletedAt_KienCD233824 = 0
              AND nv.DeletedAt_TuanhCD233018 = 0";

            LoadData(sql);
        }

        // ===============================
        // 2. CHI TIẾT THEO PHÒNG BAN
        // ===============================
        void LoadTheoPhongBan()
        {
            string sql = @"
            SELECT 
                nv.MaNV_TuanhCD233018 AS MaNV,
                nv.HoTen_TuanhCD233018 AS HoTen,
                cv.TenCV_KhangCD233181 AS ChucVu,
                pb.TenPB_ThuanCD233318 AS PhongBan,
                hd.NgayBatDau_ChienCD232928 AS NgayBatDauHD,
                hd.NgayKetThuc_ChienCD232928 AS NgayKetThucHD
            FROM tblNhanVien_TuanhCD233018 nv
            JOIN tblChucVu_KhangCD233181 cv
                ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            JOIN tblPhongBan_ThuanCD233318 pb
                ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
            LEFT JOIN tblHopDong_ChienCD232928 hd
                ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018
            WHERE pb.TenPB_ThuanCD233318 = @GiaTri
              AND nv.DeletedAt_TuanhCD233018 = 0";

            LoadData(sql);
        }

        // ===============================
        // 3. CHI TIẾT THEO CHỨC VỤ
        // ===============================
        void LoadTheoChucVu()
        {
            string sql = @"
            SELECT 
                nv.MaNV_TuanhCD233018 AS MaNV,
                nv.HoTen_TuanhCD233018 AS HoTen,
                cv.TenCV_KhangCD233181 AS ChucVu,
                pb.TenPB_ThuanCD233318 AS PhongBan,
                hd.NgayBatDau_ChienCD232928 AS NgayBatDauHD,
                hd.NgayKetThuc_ChienCD232928 AS NgayKetThucHD
            FROM tblNhanVien_TuanhCD233018 nv
            JOIN tblChucVu_KhangCD233181 cv
                ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            JOIN tblPhongBan_ThuanCD233318 pb
                ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
            LEFT JOIN tblHopDong_ChienCD232928 hd
                ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018
            WHERE cv.TenCV_KhangCD233181 = @GiaTri
              AND nv.DeletedAt_TuanhCD233018 = 0";

            LoadData(sql);
        }

        // ===============================
        // LOAD DATA CHUNG
        // ===============================
        void LoadData(string sql)
        {
            cn.connect();

            SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn);
            da.SelectCommand.Parameters.AddWithValue("@GiaTri", giaTri);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvChiTiet.DataSource = dt;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ChuanHoaHeader(dgvChiTiet);

            cn.disconnect();
        }
        void ChuanHoaHeader(DataGridView dgv)
        {
            if (dgv.Columns.Contains("PhongBan"))
                dgv.Columns["PhongBan"].HeaderText = "Phòng ban";

            if (dgv.Columns.Contains("TongNhanVien"))
                dgv.Columns["TongNhanVien"].HeaderText = "Tổng nhân viên";

            if (dgv.Columns.Contains("ChucVu"))
                dgv.Columns["ChucVu"].HeaderText = "Chức vụ";

            if (dgv.Columns.Contains("SoLuong"))
                dgv.Columns["SoLuong"].HeaderText = "Số lượng";

            if (dgv.Columns.Contains("DuAn"))
                dgv.Columns["DuAn"].HeaderText = "Dự án";

            if (dgv.Columns.Contains("SoNhanVien"))
                dgv.Columns["SoNhanVien"].HeaderText = "Số nhân viên";

            if (dgv.Columns.Contains("MaNV"))
                dgv.Columns["MaNV"].HeaderText = "Mã nhân viên";

            if (dgv.Columns.Contains("HoTen"))
                dgv.Columns["HoTen"].HeaderText = "Họ tên";

            if (dgv.Columns.Contains("VaiTroDuAn"))
                dgv.Columns["VaiTroDuAn"].HeaderText = "Vai trò dự án";

            if (dgv.Columns.Contains("NgayBatDauHD"))
                dgv.Columns["NgayBatDauHD"].HeaderText = "Ngày bắt đầu HĐ";

            if (dgv.Columns.Contains("NgayKetThucHD"))
                dgv.Columns["NgayKetThucHD"].HeaderText = "Ngày kết thúc HĐ";

            if (dgv.Columns.Contains("TrangThaiHopDong"))
                dgv.Columns["TrangThaiHopDong"].HeaderText = "Trạng thái hợp đồng";
        }


        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}