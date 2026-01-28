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
        // CONSTRUCTOR
        // ===============================
        public F_BaoCaoChiTiet(string _loaiBaoCao, string _giaTri, DateTime _tuNgay, DateTime _denNgay)
        {
            InitializeComponent();

            loaiBaoCao = _loaiBaoCao;
            giaTri = _giaTri;
            tuNgay = _tuNgay;
            denNgay = _denNgay;

            dgvChiTiet.RowPrePaint += dgvChiTiet_RowPrePaint;
        }

        // ===============================
        // LOAD FORM
        // ===============================
        private void F_BaoCaoChiTiet_Load(object sender, EventArgs e)
        {
            switch (loaiBaoCao)
            {
                case "DUAN":
                    lblTitle.Text = "BÁO CÁO CHI TIẾT THEO DỰ ÁN";
                    lblContext.Text = $"Dự án: {giaTri}";
                    LoadTheoDuAn();
                    break;

                case "PHONGBAN":
                    lblTitle.Text = "BÁO CÁO CHI TIẾT THEO PHÒNG BAN";
                    lblContext.Text = $"Phòng ban: {giaTri}";
                    LoadTheoPhongBan();
                    break;

                case "CHUCVU":
                    lblTitle.Text = "BÁO CÁO CHI TIẾT THEO CHỨC VỤ";
                    lblContext.Text = $"Chức vụ: {giaTri}";
                    LoadTheoChucVu();
                    break;
            }
        }

        // ===============================
        // LOAD THEO DỰ ÁN
        // ===============================
        void LoadTheoDuAn()
        {
            string sql = @"
            SELECT 
                nv.MaNV_TuanhCD233018 AS MaNV,
                nv.HoTen_TuanhCD233018 AS HoTen,
                cv.TenCV_KhangCD233181 AS ChucVu,
                da.TenDA_KienCD233824 AS DuAn,
                ct.VaiTro_KienCD233824 AS VaiTroDuAn,
                hd.NgayBatDau_ChienCD232928 AS NgayBatDauHD,
                hd.NgayKetThuc_ChienCD232928 AS NgayKetThucHD
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
        // LOAD THEO PHÒNG BAN
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
        // LOAD THEO CHỨC VỤ
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
            PhanTichDuLieu(dt);

            cn.disconnect();
        }

        // ===============================
        // PHÂN TÍCH HỢP ĐỒNG
        // ===============================
        void PhanTichDuLieu(DataTable dt)
        {
            int tong = dt.Rows.Count;
            int conHan = 0, sapHet = 0, hetHan = 0;

            foreach (DataRow r in dt.Rows)
            {
                if (r["NgayKetThucHD"] == DBNull.Value)
                {
                    conHan++;
                    continue;
                }

                DateTime ngayKT = Convert.ToDateTime(r["NgayKetThucHD"]);
                double days = (ngayKT - denNgay).TotalDays;

                if (days < 0)
                    hetHan++;
                else if (days <= 30)
                    sapHet++;
                else
                    conHan++;
            }

            lblTongNV.Text = $"Tổng NV: {tong}";
            lblConHan.Text = $"Còn hạn: {conHan}";
            lblSapHetHan.Text = $"Sắp hết hạn: {sapHet}";
            lblHetHan.Text = $"Hết hạn: {hetHan}";
        }

        // ===============================
        // TÔ MÀU DÒNG
        // ===============================
        private void dgvChiTiet_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var row = dgvChiTiet.Rows[e.RowIndex];

            if (!dgvChiTiet.Columns.Contains("NgayKetThucHD")) return;
            if (row.Cells["NgayKetThucHD"].Value == DBNull.Value) return;

            DateTime ngayKT = Convert.ToDateTime(row.Cells["NgayKetThucHD"].Value);
            double days = (ngayKT - denNgay).TotalDays;

            if (days < 0)
                row.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;
            else if (days <= 30)
                row.DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon;
            else
                row.DefaultCellStyle.BackColor = System.Drawing.Color.Honeydew;
        }

        // ===============================
        // CHUẨN HÓA HEADER
        // ===============================
        void ChuanHoaHeader(DataGridView dgv)
        {
            if (dgv.Columns.Contains("MaNV")) dgv.Columns["MaNV"].HeaderText = "Mã nhân viên";
            if (dgv.Columns.Contains("HoTen")) dgv.Columns["HoTen"].HeaderText = "Họ tên";
            if (dgv.Columns.Contains("ChucVu")) dgv.Columns["ChucVu"].HeaderText = "Chức vụ";
            if (dgv.Columns.Contains("PhongBan")) dgv.Columns["PhongBan"].HeaderText = "Phòng ban";
            if (dgv.Columns.Contains("DuAn")) dgv.Columns["DuAn"].HeaderText = "Dự án";
            if (dgv.Columns.Contains("VaiTroDuAn")) dgv.Columns["VaiTroDuAn"].HeaderText = "Vai trò dự án";
            if (dgv.Columns.Contains("NgayBatDauHD")) dgv.Columns["NgayBatDauHD"].HeaderText = "Ngày bắt đầu HĐ";
            if (dgv.Columns.Contains("NgayKetThucHD")) dgv.Columns["NgayKetThucHD"].HeaderText = "Ngày kết thúc HĐ";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void dgvChiTiet_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy MaNV từ cột MaNV
            string maNV = dgvChiTiet.Rows[e.RowIndex]
                                        .Cells["MaNV"]
                                        .Value.ToString();

            // Mở form chi tiết nhân viên
            F_BaoCaoNhanVienChiTiet f =
                new F_BaoCaoNhanVienChiTiet(maNV);

            f.ShowDialog();
        }
    }
}
