using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoTongHop : Form
    {
        connectData cn = new connectData();

        // 0 = Phòng ban | 1 = Chức vụ | 2 = Dự án
        int currentMode = 0;

        public F_BaoCaoTongHop()
        {
            InitializeComponent();

            btnTheoPhongBan.Click += btnTheoPhongBan_Click;
            btnTheoChucVu.Click += btnTheoChucVu_Click;
            btnTheoDuAn.Click += btnTheoDuAn_Click;

            btnLamMoi.Click += btnLamMoi_Click;
            btnDong.Click += (s, e) => this.Close();

            dtGridViewBCTongHop.CellDoubleClick += dtGridViewBCTongHop_CellDoubleClick;
        }

        private void F_BaoCaoTongHop_Load(object sender, EventArgs e)
        {
            ResetDashboard();
            LoadBaoCaoPhongBan(); // mặc định
        }

        // =========================
        // RESET
        // =========================
        void ResetDashboard()
        {
            lblNVValue.Text = "0";
            lblPBValue.Text = "0";
            lblCVValue.Text = "0";
            lblDAValue.Text = "0";

            dtGridViewBCTongHop.DataSource = null;
            dtGridViewBCTongHop.Columns.Clear();
        }

        // =========================
        // BUTTON EVENTS
        // =========================
        private void btnTheoPhongBan_Click(object sender, EventArgs e)
        {
            currentMode = 0;
            LoadBaoCaoPhongBan();
        }

        private void btnTheoChucVu_Click(object sender, EventArgs e)
        {
            currentMode = 1;
            LoadBaoCaoChucVu();
        }

        private void btnTheoDuAn_Click(object sender, EventArgs e)
        {
            currentMode = 2;
            LoadBaoCaoDuAn();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ResetDashboard();
            LoadBaoCaoPhongBan();
        }

        // =========================
        // BÁO CÁO PHÒNG BAN
        // =========================
        void LoadBaoCaoPhongBan()
        {
            ResetDashboard();
            cn.connect();

            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    pb.TenPB_ThuanCD233318 AS PhongBan,
                    COUNT(nv.MaNV_TuanhCD233018) AS TongNhanVien
                FROM tblNhanVien_TuanhCD233018 nv
                JOIN tblChucVu_KhangCD233181 cv 
                    ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                JOIN tblPhongBan_ThuanCD233318 pb 
                    ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                WHERE nv.DeletedAt_TuanhCD233018 = 0
                GROUP BY pb.TenPB_ThuanCD233318";

            new SqlDataAdapter(sql, cn.conn).Fill(dt);

            dtGridViewBCTongHop.DataSource = dt;
            FormatGrid();

            lblNVValue.Text = TinhTong(dt, "TongNhanVien").ToString();
            lblPBValue.Text = dt.Rows.Count.ToString();

            lblGridTitle.Text = "Tổng hợp nhân sự theo phòng ban";

            cn.disconnect();
        }

        // =========================
        // BÁO CÁO CHỨC VỤ
        // =========================
        void LoadBaoCaoChucVu()
        {
            ResetDashboard();
            cn.connect();

            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    cv.TenCV_KhangCD233181 AS ChucVu,
                    COUNT(nv.MaNV_TuanhCD233018) AS SoLuong
                FROM tblNhanVien_TuanhCD233018 nv
                JOIN tblChucVu_KhangCD233181 cv 
                    ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                WHERE nv.DeletedAt_TuanhCD233018 = 0
                GROUP BY cv.TenCV_KhangCD233181";

            new SqlDataAdapter(sql, cn.conn).Fill(dt);

            dtGridViewBCTongHop.DataSource = dt;
            FormatGrid();

            lblNVValue.Text = TinhTong(dt, "SoLuong").ToString();
            lblCVValue.Text = dt.Rows.Count.ToString();

            lblGridTitle.Text = "Tổng hợp nhân sự theo chức vụ";

            cn.disconnect();
        }

        // =========================
        // BÁO CÁO DỰ ÁN
        // =========================
        void LoadBaoCaoDuAn()
        {
            ResetDashboard();
            cn.connect();

            DataTable dt = new DataTable();
            string sql = @"
                SELECT 
                    da.TenDA_KienCD233824 AS DuAn,
                    COUNT(ct.MaNV_TuanhCD233018) AS SoNhanVien
                FROM tblChiTietDuAn_KienCD233824 ct
                JOIN tblDuAn_KienCD233824 da 
                    ON ct.MaDA_KienCD233824 = da.MaDA_KienCD233824
                WHERE ct.DeletedAt_KienCD233824 = 0
                GROUP BY da.TenDA_KienCD233824";

            new SqlDataAdapter(sql, cn.conn).Fill(dt);

            dtGridViewBCTongHop.DataSource = dt;
            FormatGrid();

            lblNVValue.Text = TinhTong(dt, "SoNhanVien").ToString();
            lblDAValue.Text = dt.Rows.Count.ToString();

            lblGridTitle.Text = "Tổng hợp nhân sự theo dự án";

            cn.disconnect();
        }

        // =========================
        // FORMAT GRID + TIẾNG VIỆT
        // =========================
        void FormatGrid()
        {
            dtGridViewBCTongHop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtGridViewBCTongHop.ReadOnly = true;
            dtGridViewBCTongHop.RowHeadersVisible = false;

            if (dtGridViewBCTongHop.Columns.Contains("PhongBan"))
                dtGridViewBCTongHop.Columns["PhongBan"].HeaderText = "Phòng ban";

            if (dtGridViewBCTongHop.Columns.Contains("TongNhanVien"))
                dtGridViewBCTongHop.Columns["TongNhanVien"].HeaderText = "Tổng nhân viên";

            if (dtGridViewBCTongHop.Columns.Contains("ChucVu"))
                dtGridViewBCTongHop.Columns["ChucVu"].HeaderText = "Chức vụ";

            if (dtGridViewBCTongHop.Columns.Contains("SoLuong"))
                dtGridViewBCTongHop.Columns["SoLuong"].HeaderText = "Số lượng";

            if (dtGridViewBCTongHop.Columns.Contains("DuAn"))
                dtGridViewBCTongHop.Columns["DuAn"].HeaderText = "Dự án";

            if (dtGridViewBCTongHop.Columns.Contains("SoNhanVien"))
                dtGridViewBCTongHop.Columns["SoNhanVien"].HeaderText = "Số nhân viên";
        }

        // =========================
        // DOUBLE CLICK → CHI TIẾT
        // =========================
        private void dtGridViewBCTongHop_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string loai = currentMode == 0 ? "PHONGBAN"
                        : currentMode == 1 ? "CHUCVU"
                        : "DUAN";

            string giaTri = dtGridViewBCTongHop.Rows[e.RowIndex].Cells[0].Value.ToString();

            new F_BaoCaoChiTiet(
                loai,
                giaTri,
                DateTime.MinValue,
                DateTime.MaxValue
            ).ShowDialog();
        }

        // =========================
        int TinhTong(DataTable dt, string col)
        {
            int sum = 0;
            foreach (DataRow r in dt.Rows)
                sum += Convert.ToInt32(r[col]);
            return sum;
        }

        private void dtGridViewBCTongHop_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
