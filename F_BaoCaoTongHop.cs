using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoTongHop : Form
    {
        connectData cn = new connectData();

        public F_BaoCaoTongHop()
        {
            InitializeComponent();
        }

        // ===============================
        // FORM LOAD
        // ===============================
        private void F_BaoCaoTongHop_Load(object sender, EventArgs e)
        {
            LoadLoaiBaoCao();
            LoadPhongBan();
            LoadSummary();
        }

        // ===============================
        // LOAD COMBOBOX LOẠI BÁO CÁO
        // ===============================
        void LoadLoaiBaoCao()
        {
            cboLoaiBaoCao.Items.Clear();
            cboLoaiBaoCao.Items.Add("Danh sách nhân viên");
            cboLoaiBaoCao.Items.Add("Nhân viên theo phòng ban");
            cboLoaiBaoCao.Items.Add("Nhân viên tham gia dự án");
            cboLoaiBaoCao.Items.Add("Nhân viên nhiều dự án nhất");
            cboLoaiBaoCao.SelectedIndex = 0;
        }

        // ===============================
        // LOAD PHÒNG BAN
        // ===============================
        void LoadPhongBan()
        {
            cn.connect();
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT MaPB, TenPB FROM tblPhongBan WHERE DeletedAt = 0",
                cn.conn);

            DataTable dt = new DataTable();
            da.Fill(dt);

            cboPhongBan.DataSource = dt;
            cboPhongBan.DisplayMember = "TenPB";
            cboPhongBan.ValueMember = "MaPB";
            cboPhongBan.SelectedIndex = -1;

            cn.disconnect();
        }

        // ===============================
        // SUMMARY CARD
        // ===============================
        void LoadSummary()
        {
            cn.connect();

            lblNVValue.Text = new SqlCommand(
                "SELECT COUNT(*) FROM tblNhanVien WHERE DeletedAt = 0",
                cn.conn).ExecuteScalar().ToString();

            lblPBValue.Text = new SqlCommand(
                "SELECT COUNT(*) FROM tblPhongBan WHERE DeletedAt = 0",
                cn.conn).ExecuteScalar().ToString();

            lblDAValue.Text = new SqlCommand(
                "SELECT COUNT(*) FROM tblDuAn WHERE DeletedAt = 0",
                cn.conn).ExecuteScalar().ToString();

            cn.disconnect();
        }

        // ===============================
        // NÚT XEM BÁO CÁO (TRỌNG TÂM)
        // ===============================
        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string sql = "";

                switch (cboLoaiBaoCao.SelectedIndex)
                {
                    // 1. Danh sách nhân viên
                    case 0:
                        sql = @"
                            SELECT 
                                nv.MaNV, nv.HoTen, nv.GioiTinh, nv.NgaySinh,
                                pb.TenPB AS PhongBan,
                                cv.TenCV AS ChucVu,
                                nv.SoDienThoai, nv.Email
                            FROM tblNhanVien nv
                            JOIN tblChucVu cv ON nv.MaCV = cv.MaCV
                            JOIN tblPhongBan pb ON cv.MaPB = pb.MaPB
                            WHERE nv.DeletedAt = 0
                            ORDER BY pb.TenPB, nv.HoTen";
                        break;

                    // 2. Nhân viên theo phòng ban
                    case 1:
                        if (cboPhongBan.SelectedValue == null)
                        {
                            MessageBox.Show("Vui lòng chọn phòng ban!");
                            return;
                        }
                        sql = @"
                            SELECT 
                                nv.MaNV, nv.HoTen,
                                cv.TenCV AS ChucVu,
                                pb.TenPB AS PhongBan
                            FROM tblNhanVien nv
                            JOIN tblChucVu cv ON nv.MaCV = cv.MaCV
                            JOIN tblPhongBan pb ON cv.MaPB = pb.MaPB
                            WHERE nv.DeletedAt = 0
                              AND pb.MaPB = @MaPB
                            ORDER BY nv.HoTen";
                        break;

                    // 3. Nhân viên tham gia dự án
                    case 2:
                        sql = @"
                            SELECT 
                                nv.MaNV, nv.HoTen,
                                da.TenDA AS DuAn,
                                ctda.VaiTro
                            FROM tblChiTietDuAn ctda
                            JOIN tblNhanVien nv ON ctda.MaNV = nv.MaNV
                            JOIN tblDuAn da ON ctda.MaDA = da.MaDA
                            WHERE ctda.DeletedAt = 0
                            ORDER BY da.TenDA, nv.HoTen";
                        break;

                    // 4. Nhân viên nhiều dự án nhất
                    case 3:
                        sql = @"
                            SELECT TOP 10
                                nv.MaNV, nv.HoTen,
                                COUNT(ctda.MaDA) AS SoDuAn
                            FROM tblChiTietDuAn ctda
                            JOIN tblNhanVien nv ON ctda.MaNV = nv.MaNV
                            WHERE ctda.DeletedAt = 0
                            GROUP BY nv.MaNV, nv.HoTen
                            ORDER BY SoDuAn DESC";
                        break;
                }

                SqlDataAdapter adapter = new SqlDataAdapter(sql, cn.conn);

                if (sql.Contains("@MaPB"))
                    adapter.SelectCommand.Parameters.AddWithValue(
                        "@MaPB", cboPhongBan.SelectedValue);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dtGridViewBCTongHop.DataSource = dt;
                dtGridViewBCTongHop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message);
            }
        }

        // ===============================
        // XUẤT EXCEL (GIỮ NGUYÊN)
        // ===============================
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCTongHop.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Workbook|*.xlsx";
            sfd.FileName = "BaoCao_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("BaoCao");

                    for (int i = 0; i < dtGridViewBCTongHop.Columns.Count; i++)
                        ws.Cell(1, i + 1).Value =
                            dtGridViewBCTongHop.Columns[i].HeaderText;

                    for (int i = 0; i < dtGridViewBCTongHop.Rows.Count; i++)
                        for (int j = 0; j < dtGridViewBCTongHop.Columns.Count; j++)
                            ws.Cell(i + 2, j + 1).Value = dtGridViewBCTongHop.Rows[i].Cells[j].Value?.ToString() ?? "";

                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Xuất Excel thành công!");
            }
        }
    }
}
