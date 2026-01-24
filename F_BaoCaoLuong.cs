using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoLuong : Form
    {
        public F_BaoCaoLuong()
        {
            InitializeComponent();
        }

        connectData cn = new connectData();

        // Thêm property để lưu mã tài khoản đang đăng nhập
        public string MaTaiKhoanDangNhap { get; set; }

        private void btnLuongHangThang_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                int thang = dateTimePicker1.Value.Month;
                int nam = dateTimePicker1.Value.Year;

                // Sửa query theo công thức mới
                string sql = @"SELECT 
                                      l.MaLuong_ChienCD232928 AS N'Mã Lương', 
                                      nv.MaNV_TuanhCD233018 AS N'Mã Nhân Viên', 
                                      nv.HoTen_TuanhCD233018 AS N'Họ Tên', 
                                      pb.TenPB_ThuanCD233318 AS N'Phòng Ban',
                                      cv.TenCV_KhangCD233181 AS N'Chức Vụ',
                                      l.Thang_ChienCD232928 AS N'Tháng', 
                                      l.Nam_ChienCD232928 AS N'Năm', 
                                      l.LuongCoBan_ChienCD232928 AS N'Lương Cơ Bản', 
                                      l.SoNgayCongChuan_ChienCD232928 AS N'Ngày Công Chuẩn',
                                      
                                      -- Tính số ngày đi làm thực tế: giờ vào từ 8h và giờ về từ 17h
                                      (
                                          SELECT COUNT(*)
                                          FROM tblChamCong_TuanhCD233018 cc
                                          WHERE cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                                            AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
                                            AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                                            AND cc.DeletedAt_TuanhCD233018 = 0
                                            AND CAST(cc.GioVao_TuanhCD233018 AS TIME) >= '08:00:00'
                                            AND CAST(cc.GioVe_TuanhCD233018 AS TIME) >= '17:00:00'
                                      ) AS N'Số ngày đi làm',
                                      
                                      l.PhuCap_ChienCD232928 AS N'Phụ Cấp',
                                      l.KhauTru_ChienCD232928 AS N'Khấu Trừ',
                                      
                                      -- Tính Tổng lương theo công thức mới: (Lương cơ bản / Ngày công chuẩn) * Số ngày đi làm + Phụ cấp - Khấu trừ
                                      ROUND(
                                          (l.LuongCoBan_ChienCD232928 / NULLIF(l.SoNgayCongChuan_ChienCD232928, 0)) * 
                                          (
                                              SELECT COUNT(*)
                                              FROM tblChamCong_TuanhCD233018 cc
                                              WHERE cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                                                AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
                                                AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                                                AND cc.DeletedAt_TuanhCD233018 = 0
                                                AND CAST(cc.GioVao_TuanhCD233018 AS TIME) >= '08:00:00'
                                                AND CAST(cc.GioVe_TuanhCD233018 AS TIME) >= '17:00:00'
                                          ) + 
                                          ISNULL(l.PhuCap_ChienCD232928, 0) - 
                                          ISNULL(l.KhauTru_ChienCD232928, 0), 
                                          2
                                      ) AS N'Tổng Lương',
                                      
                                      l.Ghichu_ChienCD232928 AS N'Ghi chú'
                               FROM tblLuong_ChienCD232928 l
                               JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                               JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                               JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                               JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                               WHERE l.DeletedAt_ChienCD232928 = 0 
                               AND l.Thang_ChienCD232928 = @Thang 
                               AND l.Nam_ChienCD232928 = @Nam
                               ORDER BY N'Tổng Lương' DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dtGridViewBCLuong.DataSource = dt;

                    // Định dạng các cột tiền
                    FormatCurrencyColumns();
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng lương: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNVTongLuongCaoNhat_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                int thang = dateTimePicker1.Value.Month;
                int nam = dateTimePicker1.Value.Year;

                string sql = @"SELECT TOP 5 
                                      nv.HoTen_TuanhCD233018 AS N'Họ Tên', 
                                      nv.MaNV_TuanhCD233018 AS N'Mã Nhân Viên',
                                      pb.TenPB_ThuanCD233318 AS N'Phòng Ban',
                                      cv.TenCV_KhangCD233181 AS N'Chức Vụ',
                                      
                                      -- Tính Tổng lương theo công thức mới
                                      ROUND(
                                          (l.LuongCoBan_ChienCD232928 / NULLIF(l.SoNgayCongChuan_ChienCD232928, 0)) * 
                                          (
                                              SELECT COUNT(*)
                                              FROM tblChamCong_TuanhCD233018 cc
                                              WHERE cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                                                AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
                                                AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                                                AND cc.DeletedAt_TuanhCD233018 = 0
                                                AND CAST(cc.GioVao_TuanhCD233018 AS TIME) >= '08:00:00'
                                                AND CAST(cc.GioVe_TuanhCD233018 AS TIME) >= '17:00:00'
                                          ) + 
                                          ISNULL(l.PhuCap_ChienCD232928, 0) - 
                                          ISNULL(l.KhauTru_ChienCD232928, 0), 
                                          2
                                      ) AS N'Tổng Lương Cao Nhất'
                               FROM tblLuong_ChienCD232928 l
                               JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                               JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                               JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                               JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                               WHERE l.DeletedAt_ChienCD232928 = 0 
                               AND l.Thang_ChienCD232928 = @Thang 
                               AND l.Nam_ChienCD232928 = @Nam
                               ORDER BY N'Tổng Lương Cao Nhất' DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCLuong.DataSource = dt;

                    // Định dạng cột tổng lương
                    if (dtGridViewBCLuong.Columns.Contains("Tổng Lương Cao Nhất"))
                    {
                        dtGridViewBCLuong.Columns["Tổng Lương Cao Nhất"].DefaultCellStyle.Format = "N0";
                        dtGridViewBCLuong.Columns["Tổng Lương Cao Nhất"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnTongLuongPhongBan_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                int thang = dateTimePicker1.Value.Month;
                int nam = dateTimePicker1.Value.Year;

                string sql = @"SELECT 
                                      pb.TenPB_ThuanCD233318 AS N'Phòng Ban', 
                                      COUNT(DISTINCT nv.Id_TuanhCD233018) AS N'Số Nhân Viên',
                                      SUM(
                                          ROUND(
                                              (l.LuongCoBan_ChienCD232928 / NULLIF(l.SoNgayCongChuan_ChienCD232928, 0)) * 
                                              (
                                                  SELECT COUNT(*)
                                                  FROM tblChamCong_TuanhCD233018 cc
                                                  WHERE cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                                                    AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
                                                    AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                                                    AND cc.DeletedAt_TuanhCD233018 = 0
                                                    AND CAST(cc.GioVao_TuanhCD233018 AS TIME) >= '08:00:00'
                                                    AND CAST(cc.GioVe_TuanhCD233018 AS TIME) >= '17:00:00'
                                              ) + 
                                              ISNULL(l.PhuCap_ChienCD232928, 0) - 
                                              ISNULL(l.KhauTru_ChienCD232928, 0), 
                                              2
                                          )
                                      ) AS N'Tổng Quỹ Lương Phòng'
                               FROM tblLuong_ChienCD232928 l
                               JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                               JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                               JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                               JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                               WHERE l.DeletedAt_ChienCD232928 = 0 
                               AND l.Thang_ChienCD232928 = @Thang 
                               AND l.Nam_ChienCD232928 = @Nam
                               GROUP BY pb.TenPB_ThuanCD233318
                               UNION ALL
                               SELECT 
                                      'TOÀN CÔNG TY' AS N'Phòng Ban', 
                                      COUNT(DISTINCT nv.Id_TuanhCD233018) AS N'Số Nhân Viên',
                                      SUM(
                                          ROUND(
                                              (l.LuongCoBan_ChienCD232928 / NULLIF(l.SoNgayCongChuan_ChienCD232928, 0)) * 
                                              (
                                                  SELECT COUNT(*)
                                                  FROM tblChamCong_TuanhCD233018 cc
                                                  WHERE cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                                                    AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
                                                    AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                                                    AND cc.DeletedAt_TuanhCD233018 = 0
                                                    AND CAST(cc.GioVao_TuanhCD233018 AS TIME) >= '08:00:00'
                                                    AND CAST(cc.GioVe_TuanhCD233018 AS TIME) >= '17:00:00'
                                              ) + 
                                              ISNULL(l.PhuCap_ChienCD232928, 0) - 
                                              ISNULL(l.KhauTru_ChienCD232928, 0), 
                                              2
                                          )
                                      ) AS N'Tổng Quỹ Lương Phòng'
                               FROM tblLuong_ChienCD232928 l
                               JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                               JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                               WHERE l.DeletedAt_ChienCD232928 = 0 
                               AND l.Thang_ChienCD232928 = @Thang 
                               AND l.Nam_ChienCD232928 = @Nam";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCLuong.DataSource = dt;

                    // Định dạng cột tổng quỹ lương
                    if (dtGridViewBCLuong.Columns.Contains("Tổng Quỹ Lương Phòng"))
                    {
                        dtGridViewBCLuong.Columns["Tổng Quỹ Lương Phòng"].DefaultCellStyle.Format = "N0";
                        dtGridViewBCLuong.Columns["Tổng Quỹ Lương Phòng"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // Thêm phương thức định dạng cột tiền
        private void FormatCurrencyColumns()
        {
            string[] currencyColumns = { "Lương Cơ Bản", "Phụ Cấp", "Khấu Trừ", "Tổng Lương", "Tổng Quỹ Lương Phòng" };

            foreach (string colName in currencyColumns)
            {
                if (dtGridViewBCLuong.Columns.Contains(colName))
                {
                    dtGridViewBCLuong.Columns[colName].DefaultCellStyle.Format = "N0";
                    dtGridViewBCLuong.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            // Căn giữa cho các cột số
            string[] centerColumns = { "Tháng", "Năm", "Ngày Công Chuẩn", "Số ngày đi làm", "Số Nhân Viên" };

            foreach (string colName in centerColumns)
            {
                if (dtGridViewBCLuong.Columns.Contains(colName))
                {
                    dtGridViewBCLuong.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        // THÊM HÀM MỚI: Lấy thông tin người xuất báo cáo
        private DataRow LayThongTinNguoiXuat()
        {
            try
            {
                cn.connect();

                string sql = "";
                SqlCommand cmd = null;

                if (!string.IsNullOrEmpty(MaTaiKhoanDangNhap))
                {
                    sql = @"SELECT 
                                nv.MaNV_TuanhCD233018,
                                nv.HoTen_TuanhCD233018,
                                cv.TenCV_KhangCD233181,
                                pb.TenPB_ThuanCD233318
                            FROM tblTaiKhoan_KhangCD233181 tk
                            INNER JOIN tblNhanVien_TuanhCD233018 nv 
                                ON tk.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                            INNER JOIN tblChucVu_KhangCD233181 cv 
                                ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                            INNER JOIN tblPhongBan_ThuanCD233318 pb 
                                ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                            WHERE tk.MaTK_KhangCD233181 = @MaTaiKhoan
                                AND tk.DeletedAt_KhangCD233181 = 0
                                AND nv.DeletedAt_TuanhCD233018 = 0";

                    cmd = new SqlCommand(sql, cn.conn);
                    cmd.Parameters.AddWithValue("@MaTaiKhoan", MaTaiKhoanDangNhap);
                }
                else
                {
                    sql = @"SELECT TOP 1
                                nv.MaNV_TuanhCD233018,
                                nv.HoTen_TuanhCD233018,
                                cv.TenCV_KhangCD233181,
                                pb.TenPB_ThuanCD233318
                            FROM tblTaiKhoan_KhangCD233181 tk
                            INNER JOIN tblNhanVien_TuanhCD233018 nv 
                                ON tk.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                            INNER JOIN tblChucVu_KhangCD233181 cv 
                                ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                            INNER JOIN tblPhongBan_ThuanCD233318 pb 
                                ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                            WHERE tk.DeletedAt_KhangCD233181 = 0
                                AND nv.DeletedAt_TuanhCD233018 = 0";

                    cmd = new SqlCommand(sql, cn.conn);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cn.disconnect();

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy thông tin người xuất: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }

            return null;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCLuong.Rows.Count > 0)
            {
                string fileName = $"BaoCaoLuong_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "Excel Workbook|*.xlsx",
                    FileName = fileName
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Lấy thông tin người xuất
                            DataRow nguoiXuat = LayThongTinNguoiXuat();

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("BaoCaoLuong");

                                int colCount = dtGridViewBCLuong.Columns.Count;
                                int currentRow = 1;

                                /* ================= TIÊU ĐỀ CÔNG TY ================= */
                                ws.Cell(currentRow, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                                ws.Range(currentRow, 1, currentRow, colCount).Merge();
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Font.Bold = true;
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Font.FontSize = 14;
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                currentRow++;

                                /* ================= TIÊU ĐỀ BÁO CÁO ================= */
                                ws.Cell(currentRow, 1).Value = "BÁO CÁO LƯƠNG NHÂN VIÊN";
                                ws.Range(currentRow, 1, currentRow, colCount).Merge();
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Font.Bold = true;
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Font.FontSize = 18;
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                currentRow++;

                                /* ================= NGÀY LẬP BÁO CÁO ================= */
                                ws.Cell(currentRow, 1).Value = $"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy}";
                                ws.Range(currentRow, 1, currentRow, colCount).Merge();
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                currentRow++;

                                /* ================= THÁNG/NĂM BÁO CÁO ================= */
                                int thang = dateTimePicker1.Value.Month;
                                int nam = dateTimePicker1.Value.Year;
                                ws.Cell(currentRow, 1).Value = $"Tháng/Năm: {thang}/{nam}";
                                ws.Range(currentRow, 1, currentRow, colCount).Merge();
                                ws.Range(currentRow, 1, currentRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                currentRow++;

                                /* ================= THÔNG TIN NGƯỜI XUẤT ================= */
                                if (nguoiXuat != null)
                                {
                                    ws.Cell(currentRow, 1).Value = "Phòng Ban";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = nguoiXuat["TenPB_ThuanCD233318"].ToString();
                                    currentRow++;

                                    ws.Cell(currentRow, 1).Value = "Chức vụ";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = nguoiXuat["TenCV_KhangCD233181"].ToString();
                                    currentRow++;
                                }

                                currentRow++; // Dòng trống

                                /* ================= HEADER ================= */
                                for (int i = 0; i < colCount; i++)
                                {
                                    ws.Cell(currentRow, i + 1).Value = dtGridViewBCLuong.Columns[i].HeaderText;
                                    ws.Cell(currentRow, i + 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Cell(currentRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }
                                int headerRow = currentRow;
                                currentRow++;

                                /* ================= DỮ LIỆU ================= */
                                for (int i = 0; i < dtGridViewBCLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dtGridViewBCLuong.Rows[i].Cells[j].Value;
                                        ws.Cell(currentRow, j + 1).Value = value != null ? value.ToString() : "";

                                        // Định dạng số cho các cột tiền
                                        if (dtGridViewBCLuong.Columns[j].HeaderText.Contains("Lương") ||
                                            dtGridViewBCLuong.Columns[j].HeaderText.Contains("Phụ Cấp") ||
                                            dtGridViewBCLuong.Columns[j].HeaderText.Contains("Khấu Trừ") ||
                                            dtGridViewBCLuong.Columns[j].HeaderText.Contains("Tổng"))
                                        {
                                            if (value != null && decimal.TryParse(value.ToString().Replace(",", ""), out decimal money))
                                            {
                                                ws.Cell(currentRow, j + 1).Value = money;
                                                ws.Cell(currentRow, j + 1).Style.NumberFormat.Format = "#,##0";
                                            }
                                        }
                                    }
                                    currentRow++;
                                }

                                int dataEndRow = currentRow - 1;

                                /* ================= DÒNG TỔNG CỘNG ================= */
                                ws.Cell(currentRow, 1).Value = "Tổng Cộng";
                                ws.Cell(currentRow, 1).Style.Font.Bold = true;

                                // Tính tổng cho từng cột số liệu
                                for (int j = 0; j < colCount; j++)
                                {
                                    string headerText = dtGridViewBCLuong.Columns[j].HeaderText;

                                    if (headerText.Contains("Lương Cơ Bản") ||
                                        headerText.Contains("Phụ Cấp") ||
                                        headerText.Contains("Khấu Trừ") ||
                                        headerText.Contains("Tổng Lương") ||
                                        headerText.Contains("Ngày Công Chuẩn") ||
                                        headerText.Contains("Số ngày đi làm") ||
                                        headerText.Contains("Số Nhân Viên") ||
                                        headerText.Contains("Tổng Quỹ Lương Phòng"))
                                    {
                                        // Tìm cột index trong DataGridView
                                        int colIndex = -1;
                                        for (int k = 0; k < dtGridViewBCLuong.Columns.Count; k++)
                                        {
                                            if (dtGridViewBCLuong.Columns[k].HeaderText == headerText)
                                            {
                                                colIndex = k;
                                                break;
                                            }
                                        }

                                        if (colIndex >= 0)
                                        {
                                            // Tính tổng cho cột này
                                            decimal tong = 0;
                                            for (int i = 0; i < dtGridViewBCLuong.Rows.Count; i++)
                                            {
                                                var value = dtGridViewBCLuong.Rows[i].Cells[colIndex].Value;
                                                if (value != null)
                                                {
                                                    string strValue = value.ToString().Replace(",", "");
                                                    if (decimal.TryParse(strValue, out decimal cellValue))
                                                    {
                                                        tong += cellValue;
                                                    }
                                                }
                                            }

                                            ws.Cell(currentRow, j + 1).Value = tong;
                                            ws.Cell(currentRow, j + 1).Style.Font.Bold = true;

                                            if (headerText.Contains("Lương") ||
                                                headerText.Contains("Phụ Cấp") ||
                                                headerText.Contains("Khấu Trừ") ||
                                                headerText.Contains("Tổng") ||
                                                headerText.Contains("Quỹ"))
                                            {
                                                ws.Cell(currentRow, j + 1).Style.NumberFormat.Format = "#,##0";
                                            }
                                        }
                                    }
                                }

                                currentRow++;

                                /* ================= THỐNG KÊ TỔNG HỢP ================= */
                                // Tìm các cột cần thống kê
                                int colLuongCoBan = -1, colNgayCongChuan = -1, colSoNgayDiLam = -1,
                                    colPhuCap = -1, colKhauTru = -1, colTongLuong = -1, colSoNhanVien = -1;

                                for (int j = 0; j < colCount; j++)
                                {
                                    string headerText = dtGridViewBCLuong.Columns[j].HeaderText;

                                    if (headerText.Contains("Lương Cơ Bản")) colLuongCoBan = j;
                                    else if (headerText.Contains("Ngày Công Chuẩn")) colNgayCongChuan = j;
                                    else if (headerText.Contains("Số ngày đi làm")) colSoNgayDiLam = j;
                                    else if (headerText.Contains("Phụ Cấp")) colPhuCap = j;
                                    else if (headerText.Contains("Khấu Trừ")) colKhauTru = j;
                                    else if (headerText.Contains("Tổng Lương")) colTongLuong = j;
                                    else if (headerText.Contains("Số Nhân Viên")) colSoNhanVien = j;
                                }

                                // Tính tổng từ DataGridView
                                decimal tongLuongCoBan = 0, tongNgayCongChuan = 0, tongSoNgayDiLam = 0,
                                        tongPhuCap = 0, tongKhauTru = 0, tongTongLuong = 0;
                                int tongSoNhanVien = 0;

                                foreach (DataGridViewRow row in dtGridViewBCLuong.Rows)
                                {
                                    if (colLuongCoBan >= 0 && row.Cells[colLuongCoBan].Value != null)
                                    {
                                        if (decimal.TryParse(row.Cells[colLuongCoBan].Value.ToString().Replace(",", ""), out decimal luongCB))
                                            tongLuongCoBan += luongCB;
                                    }
                                    if (colNgayCongChuan >= 0 && row.Cells[colNgayCongChuan].Value != null)
                                    {
                                        if (decimal.TryParse(row.Cells[colNgayCongChuan].Value.ToString().Replace(",", ""), out decimal ngayCong))
                                            tongNgayCongChuan += ngayCong;
                                    }
                                    if (colSoNgayDiLam >= 0 && row.Cells[colSoNgayDiLam].Value != null)
                                    {
                                        if (decimal.TryParse(row.Cells[colSoNgayDiLam].Value.ToString().Replace(",", ""), out decimal ngayDiLam))
                                            tongSoNgayDiLam += ngayDiLam;
                                    }
                                    if (colPhuCap >= 0 && row.Cells[colPhuCap].Value != null)
                                    {
                                        if (decimal.TryParse(row.Cells[colPhuCap].Value.ToString().Replace(",", ""), out decimal phuCap))
                                            tongPhuCap += phuCap;
                                    }
                                    if (colKhauTru >= 0 && row.Cells[colKhauTru].Value != null)
                                    {
                                        if (decimal.TryParse(row.Cells[colKhauTru].Value.ToString().Replace(",", ""), out decimal khauTru))
                                            tongKhauTru += khauTru;
                                    }
                                    if (colTongLuong >= 0 && row.Cells[colTongLuong].Value != null)
                                    {
                                        if (decimal.TryParse(row.Cells[colTongLuong].Value.ToString().Replace(",", ""), out decimal tong))
                                            tongTongLuong += tong;
                                    }
                                    if (colSoNhanVien >= 0 && row.Cells[colSoNhanVien].Value != null)
                                    {
                                        if (int.TryParse(row.Cells[colSoNhanVien].Value.ToString(), out int soNV))
                                            tongSoNhanVien += soNV;
                                    }
                                }

                                // Hiển thị thống kê
                                ws.Cell(currentRow, 1).Value = "THỐNG KÊ TỔNG HỢP";
                                ws.Range(currentRow, 1, currentRow, 4).Merge();
                                ws.Range(currentRow, 1, currentRow, 4).Style.Font.Bold = true;
                                ws.Range(currentRow, 1, currentRow, 4).Style.Font.FontSize = 12;
                                ws.Range(currentRow, 1, currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(currentRow, 1, currentRow, 4).Style.Fill.BackgroundColor = XLColor.LightBlue;
                                currentRow++;

                                // Tổng số nhân viên
                                if (colSoNhanVien >= 0)
                                {
                                    ws.Cell(currentRow, 1).Value = "Tổng số nhân viên:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = tongSoNhanVien > 0 ? tongSoNhanVien : dtGridViewBCLuong.Rows.Count;
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    currentRow++;
                                }

                                // Tổng lương cơ bản
                                if (colLuongCoBan >= 0)
                                {
                                    ws.Cell(currentRow, 1).Value = "Tổng lương cơ bản:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = tongLuongCoBan;
                                    ws.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    currentRow++;
                                }

                                // Tổng ngày công chuẩn
                                if (colNgayCongChuan >= 0)
                                {
                                    ws.Cell(currentRow, 1).Value = "Tổng ngày công chuẩn:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = tongNgayCongChuan;
                                    ws.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    currentRow++;
                                }

                                // Tổng số ngày đi làm thực tế
                                if (colSoNgayDiLam >= 0)
                                {
                                    ws.Cell(currentRow, 1).Value = "Tổng số ngày đi làm thực tế:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = tongSoNgayDiLam;
                                    ws.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    currentRow++;
                                }

                                // Tổng phụ cấp
                                if (colPhuCap >= 0)
                                {
                                    ws.Cell(currentRow, 1).Value = "Tổng phụ cấp:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = tongPhuCap;
                                    ws.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    currentRow++;
                                }

                                // Tổng khấu trừ
                                if (colKhauTru >= 0)
                                {
                                    ws.Cell(currentRow, 1).Value = "Tổng khấu trừ:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = tongKhauTru;
                                    ws.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    currentRow++;
                                }

                                // Tổng lương thực nhận
                                if (colTongLuong >= 0)
                                {
                                    ws.Cell(currentRow, 1).Value = "Tổng lương thực nhận:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = tongTongLuong;
                                    ws.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Style.Font.FontColor = XLColor.Red;
                                    currentRow++;
                                }

                                // Tính lương trung bình
                                if (colTongLuong >= 0 && dtGridViewBCLuong.Rows.Count > 0)
                                {
                                    decimal luongTrungBinh = tongTongLuong / dtGridViewBCLuong.Rows.Count;

                                    ws.Cell(currentRow, 1).Value = "Lương trung bình:";
                                    ws.Cell(currentRow, 1).Style.Font.Bold = true;
                                    ws.Cell(currentRow, 2).Value = luongTrungBinh;
                                    ws.Cell(currentRow, 2).Style.NumberFormat.Format = "#,##0";
                                    ws.Cell(currentRow, 2).Style.Font.Bold = true;
                                    currentRow++;
                                }

                                currentRow++; // Dòng trống

                                /* ================= CHỮ KÝ ================= */
                                if (nguoiXuat != null)
                                {
                                    ws.Cell(currentRow, colCount - 1).Value = $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}";
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Merge();
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Style.Font.Italic = true;
                                    currentRow++;

                                    ws.Cell(currentRow, colCount - 1).Value = "Người xuất";
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Merge();
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Style.Font.Bold = true;
                                    currentRow += 4; // Khoảng trống cho chữ ký

                                    ws.Cell(currentRow, colCount - 1).Value = nguoiXuat["HoTen_TuanhCD233018"].ToString();
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Merge();
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Range(currentRow, colCount - 1, currentRow, colCount).Style.Font.Bold = true;
                                }

                                /* ================= BORDER ================= */
                                var dataRange = ws.Range(headerRow, 1, dataEndRow, colCount);
                                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // Border cho dòng tổng cộng
                                var tongCongRow = dataEndRow + 1;
                                var tongCongRange = ws.Range(tongCongRow, 1, tongCongRow, colCount);
                                tongCongRange.Style.Border.TopBorder = XLBorderStyleValues.Medium;
                                tongCongRange.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                                /* ================= AUTO SIZE ================= */
                                ws.Columns().AdjustToContents();

                                /* ================= ĐỊNH DẠNG THÊM ================= */
                                // Căn phải cho các cột số tiền
                                for (int j = 0; j < colCount; j++)
                                {
                                    string headerText = dtGridViewBCLuong.Columns[j].HeaderText;
                                    if (headerText.Contains("Lương") ||
                                        headerText.Contains("Phụ Cấp") ||
                                        headerText.Contains("Khấu Trừ") ||
                                        headerText.Contains("Tổng") ||
                                        headerText.Contains("Quỹ"))
                                    {
                                        ws.Column(j + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    }
                                }

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCLuong.Rows.Count > 0)
            {
                string fileName = $"BaoCaoLuong_{DateTime.Now:ddMMyyyy_HHmmss}.pdf";

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "PDF Document|*.pdf",
                    FileName = fileName
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Lấy thông tin người xuất
                            DataRow nguoiXuat = LayThongTinNguoiXuat();

                            // Tạo document với khổ ngang
                            Document document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
                            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(sfd.FileName, FileMode.Create));

                            document.Open();

                            // Font Unicode cho tiếng Việt
                            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

                            BaseFont baseFont;
                            iTextSharp.text.Font fontNormal, fontBold, fontTitle, fontCompany, fontHeader, fontRed;

                            try
                            {
                                if (File.Exists(fontPath))
                                {
                                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                }
                                else
                                {
                                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                }

                                fontNormal = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                                fontBold = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontTitle = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontCompany = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontHeader = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontRed = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD, BaseColor.RED);
                            }
                            catch
                            {
                                baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                fontNormal = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                                fontBold = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontTitle = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontCompany = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontHeader = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                                fontRed = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD, BaseColor.RED);
                            }

                            /* ================= TIÊU ĐỀ CÔNG TY ================= */
                            Paragraph company = new Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontCompany);
                            company.Alignment = Element.ALIGN_CENTER;
                            document.Add(company);

                            /* ================= TIÊU ĐỀ BÁO CÁO ================= */
                            Paragraph title = new Paragraph("BÁO CÁO LƯƠNG NHÂN VIÊN", fontTitle);
                            title.Alignment = Element.ALIGN_CENTER;
                            title.SpacingBefore = 5;
                            document.Add(title);

                            /* ================= NGÀY LẬP BÁO CÁO ================= */
                            Paragraph date = new Paragraph($"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy}", fontNormal);
                            date.Alignment = Element.ALIGN_CENTER;
                            date.SpacingBefore = 10;
                            document.Add(date);

                            /* ================= THÁNG/NĂM BÁO CÁO ================= */
                            int thang = dateTimePicker1.Value.Month;
                            int nam = dateTimePicker1.Value.Year;
                            Paragraph monthYear = new Paragraph($"Tháng/Năm: {thang}/{nam}", fontNormal);
                            monthYear.Alignment = Element.ALIGN_CENTER;
                            monthYear.SpacingAfter = 10;
                            document.Add(monthYear);

                            /* ================= THÔNG TIN NGƯỜI XUẤT ================= */
                            if (nguoiXuat != null)
                            {
                                Paragraph phongBan = new Paragraph();
                                phongBan.Font = fontNormal;
                                phongBan.Add(new Chunk("Phòng Ban: ", fontBold));
                                phongBan.Add(new Chunk(nguoiXuat["TenPB_ThuanCD233318"].ToString(), fontNormal));
                                document.Add(phongBan);

                                Paragraph chucVu = new Paragraph();
                                chucVu.Font = fontNormal;
                                chucVu.Add(new Chunk("Chức vụ: ", fontBold));
                                chucVu.Add(new Chunk(nguoiXuat["TenCV_KhangCD233181"].ToString(), fontNormal));
                                chucVu.SpacingAfter = 15;
                                document.Add(chucVu);
                            }

                            /* ================= TẠO BẢNG DỮ LIỆU ================= */
                            int colCount = dtGridViewBCLuong.Columns.Count;
                            PdfPTable table = new PdfPTable(colCount);
                            table.WidthPercentage = 100;

                            // Tính chiều rộng cột
                            float[] columnWidths = new float[colCount];
                            float columnWidth = 100f / colCount;
                            for (int i = 0; i < colCount; i++)
                            {
                                columnWidths[i] = columnWidth;
                            }
                            table.SetWidths(columnWidths);

                            // Tìm các cột cần tính tổng
                            int colLuongCoBan = -1, colNgayCongChuan = -1, colSoNgayDiLam = -1,
                                colPhuCap = -1, colKhauTru = -1, colTongLuong = -1, colSoNhanVien = -1;

                            for (int j = 0; j < colCount; j++)
                            {
                                string headerText = dtGridViewBCLuong.Columns[j].HeaderText;

                                if (headerText.Contains("Lương Cơ Bản")) colLuongCoBan = j;
                                else if (headerText.Contains("Ngày Công Chuẩn")) colNgayCongChuan = j;
                                else if (headerText.Contains("Số ngày đi làm")) colSoNgayDiLam = j;
                                else if (headerText.Contains("Phụ Cấp")) colPhuCap = j;
                                else if (headerText.Contains("Khấu Trừ")) colKhauTru = j;
                                else if (headerText.Contains("Tổng Lương")) colTongLuong = j;
                                else if (headerText.Contains("Số Nhân Viên")) colSoNhanVien = j;
                            }

                            // Biến để tính tổng
                            decimal tongLuongCoBan = 0;
                            decimal tongNgayCongChuan = 0;
                            decimal tongSoNgayDiLam = 0;
                            decimal tongPhuCap = 0;
                            decimal tongKhauTru = 0;
                            decimal tongTongLuong = 0;
                            int tongSoNhanVien = 0;

                            // Header
                            for (int i = 0; i < colCount; i++)
                            {
                                PdfPCell headerCell = new PdfPCell(new Phrase(dtGridViewBCLuong.Columns[i].HeaderText, fontHeader));
                                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                headerCell.Padding = 5;
                                table.AddCell(headerCell);
                            }

                            // Dữ liệu
                            for (int i = 0; i < dtGridViewBCLuong.Rows.Count; i++)
                            {
                                for (int j = 0; j < colCount; j++)
                                {
                                    var value = dtGridViewBCLuong.Rows[i].Cells[j].Value;
                                    string cellValue = value != null ? value.ToString() : "";

                                    // Format số tiền và tính tổng
                                    if (dtGridViewBCLuong.Columns[j].HeaderText.Contains("Lương") ||
                                        dtGridViewBCLuong.Columns[j].HeaderText.Contains("Phụ Cấp") ||
                                        dtGridViewBCLuong.Columns[j].HeaderText.Contains("Khấu Trừ") ||
                                        dtGridViewBCLuong.Columns[j].HeaderText.Contains("Tổng"))
                                    {
                                        if (decimal.TryParse(cellValue.Replace(",", ""), out decimal money))
                                        {
                                            cellValue = money.ToString("#,##0");

                                            // Tính tổng
                                            if (j == colLuongCoBan) tongLuongCoBan += money;
                                            else if (j == colNgayCongChuan) tongNgayCongChuan += money;
                                            else if (j == colSoNgayDiLam) tongSoNgayDiLam += money;
                                            else if (j == colPhuCap) tongPhuCap += money;
                                            else if (j == colKhauTru) tongKhauTru += money;
                                            else if (j == colTongLuong) tongTongLuong += money;
                                        }
                                    }
                                    else if (j == colSoNhanVien)
                                    {
                                        if (int.TryParse(cellValue, out int soNV))
                                            tongSoNhanVien += soNV;
                                    }

                                    PdfPCell dataCell = new PdfPCell(new Phrase(cellValue, fontNormal));
                                    dataCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    dataCell.Padding = 3;

                                    // Căn phải cho các cột số tiền, căn giữa cho các cột số khác
                                    if (dtGridViewBCLuong.Columns[j].HeaderText.Contains("Lương") ||
                                        dtGridViewBCLuong.Columns[j].HeaderText.Contains("Phụ Cấp") ||
                                        dtGridViewBCLuong.Columns[j].HeaderText.Contains("Khấu Trừ") ||
                                        dtGridViewBCLuong.Columns[j].HeaderText.Contains("Tổng"))
                                    {
                                        dataCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    }
                                    else if (dtGridViewBCLuong.Columns[j].HeaderText.Contains("Tháng") ||
                                             dtGridViewBCLuong.Columns[j].HeaderText.Contains("Năm") ||
                                             dtGridViewBCLuong.Columns[j].HeaderText.Contains("Ngày Công Chuẩn") ||
                                             dtGridViewBCLuong.Columns[j].HeaderText.Contains("Số ngày đi làm") ||
                                             dtGridViewBCLuong.Columns[j].HeaderText.Contains("Số Nhân Viên"))
                                    {
                                        dataCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    }
                                    else
                                    {
                                        dataCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    }

                                    table.AddCell(dataCell);
                                }
                            }

                            /* ================= DÒNG TỔNG CỘNG ================= */
                            for (int j = 0; j < colCount; j++)
                            {
                                PdfPCell tongCell = new PdfPCell();
                                string headerText = dtGridViewBCLuong.Columns[j].HeaderText;

                                if (j == colLuongCoBan)
                                {
                                    tongCell = new PdfPCell(new Phrase(tongLuongCoBan.ToString("#,##0"), fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else if (j == colNgayCongChuan)
                                {
                                    tongCell = new PdfPCell(new Phrase(tongNgayCongChuan.ToString("#,##0"), fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else if (j == colSoNgayDiLam)
                                {
                                    tongCell = new PdfPCell(new Phrase(tongSoNgayDiLam.ToString("#,##0"), fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else if (j == colPhuCap)
                                {
                                    tongCell = new PdfPCell(new Phrase(tongPhuCap.ToString("#,##0"), fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else if (j == colKhauTru)
                                {
                                    tongCell = new PdfPCell(new Phrase(tongKhauTru.ToString("#,##0"), fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else if (j == colTongLuong)
                                {
                                    tongCell = new PdfPCell(new Phrase(tongTongLuong.ToString("#,##0"), fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                }
                                else if (j == colSoNhanVien)
                                {
                                    tongCell = new PdfPCell(new Phrase(tongSoNhanVien.ToString(), fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                }
                                else if (j == 0) // Cột đầu tiên hiển thị "Tổng Cộng"
                                {
                                    tongCell = new PdfPCell(new Phrase("Tổng Cộng", fontBold));
                                    tongCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                }
                                else
                                {
                                    tongCell = new PdfPCell(new Phrase("", fontNormal));
                                }

                                tongCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                tongCell.Padding = 3;
                                tongCell.BackgroundColor = new BaseColor(240, 240, 240);
                                table.AddCell(tongCell);
                            }

                            document.Add(table);

                            /* ================= THỐNG KÊ TỔNG HỢP ================= */
                            Paragraph thongKeTitle = new Paragraph("THỐNG KÊ TỔNG HỢP", fontBold);
                            thongKeTitle.Font.Size = 12;
                            thongKeTitle.Alignment = Element.ALIGN_CENTER;
                            thongKeTitle.SpacingBefore = 15;
                            thongKeTitle.SpacingAfter = 10;
                            document.Add(thongKeTitle);

                            // Bảng thống kê
                            PdfPTable tableThongKe = new PdfPTable(2);
                            tableThongKe.WidthPercentage = 60;
                            tableThongKe.HorizontalAlignment = Element.ALIGN_CENTER;
                            tableThongKe.SetWidths(new float[] { 60, 40 });

                            // Sử dụng hằng số 0 thay vì Rectangle.NO_BORDER
                            int NO_BORDER = 0;

                            // Tổng số nhân viên
                            PdfPCell cellLabel1 = new PdfPCell(new Phrase("Tổng số nhân viên:", fontBold));
                            cellLabel1.Border = NO_BORDER;
                            cellLabel1.Padding = 3;
                            tableThongKe.AddCell(cellLabel1);

                            int totalNV = tongSoNhanVien > 0 ? tongSoNhanVien : dtGridViewBCLuong.Rows.Count;
                            PdfPCell cellValue1 = new PdfPCell(new Phrase(totalNV.ToString(), fontBold));
                            cellValue1.Border = NO_BORDER;
                            cellValue1.Padding = 3;
                            cellValue1.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableThongKe.AddCell(cellValue1);

                            // Tổng lương cơ bản
                            PdfPCell cellLabel2 = new PdfPCell(new Phrase("Tổng lương cơ bản:", fontBold));
                            cellLabel2.Border = NO_BORDER;
                            cellLabel2.Padding = 3;
                            tableThongKe.AddCell(cellLabel2);

                            PdfPCell cellValue2 = new PdfPCell(new Phrase(tongLuongCoBan.ToString("#,##0"), fontBold));
                            cellValue2.Border = NO_BORDER;
                            cellValue2.Padding = 3;
                            cellValue2.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableThongKe.AddCell(cellValue2);

                            // Tổng ngày công chuẩn
                            PdfPCell cellLabel3 = new PdfPCell(new Phrase("Tổng ngày công chuẩn:", fontBold));
                            cellLabel3.Border = NO_BORDER;
                            cellLabel3.Padding = 3;
                            tableThongKe.AddCell(cellLabel3);

                            PdfPCell cellValue3 = new PdfPCell(new Phrase(tongNgayCongChuan.ToString("#,##0"), fontBold));
                            cellValue3.Border = NO_BORDER;
                            cellValue3.Padding = 3;
                            cellValue3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableThongKe.AddCell(cellValue3);

                            // Tổng số ngày đi làm thực tế
                            if (colSoNgayDiLam >= 0)
                            {
                                PdfPCell cellLabel4 = new PdfPCell(new Phrase("Tổng số ngày đi làm thực tế:", fontBold));
                                cellLabel4.Border = NO_BORDER;
                                cellLabel4.Padding = 3;
                                tableThongKe.AddCell(cellLabel4);

                                PdfPCell cellValue4 = new PdfPCell(new Phrase(tongSoNgayDiLam.ToString("#,##0"), fontBold));
                                cellValue4.Border = NO_BORDER;
                                cellValue4.Padding = 3;
                                cellValue4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                tableThongKe.AddCell(cellValue4);
                            }

                            // Tổng phụ cấp
                            PdfPCell cellLabel5 = new PdfPCell(new Phrase("Tổng phụ cấp:", fontBold));
                            cellLabel5.Border = NO_BORDER;
                            cellLabel5.Padding = 3;
                            tableThongKe.AddCell(cellLabel5);

                            PdfPCell cellValue5 = new PdfPCell(new Phrase(tongPhuCap.ToString("#,##0"), fontBold));
                            cellValue5.Border = NO_BORDER;
                            cellValue5.Padding = 3;
                            cellValue5.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableThongKe.AddCell(cellValue5);

                            // Tổng khấu trừ
                            PdfPCell cellLabel6 = new PdfPCell(new Phrase("Tổng khấu trừ:", fontBold));
                            cellLabel6.Border = NO_BORDER;
                            cellLabel6.Padding = 3;
                            tableThongKe.AddCell(cellLabel6);

                            PdfPCell cellValue6 = new PdfPCell(new Phrase(tongKhauTru.ToString("#,##0"), fontBold));
                            cellValue6.Border = NO_BORDER;
                            cellValue6.Padding = 3;
                            cellValue6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableThongKe.AddCell(cellValue6);

                            // Tổng lương thực nhận
                            PdfPCell cellLabel7 = new PdfPCell(new Phrase("Tổng lương thực nhận:", fontBold));
                            cellLabel7.Border = NO_BORDER;
                            cellLabel7.Padding = 3;
                            tableThongKe.AddCell(cellLabel7);

                            PdfPCell cellValue7 = new PdfPCell(new Phrase(tongTongLuong.ToString("#,##0"), fontRed));
                            cellValue7.Border = NO_BORDER;
                            cellValue7.Padding = 3;
                            cellValue7.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableThongKe.AddCell(cellValue7);

                            // Lương trung bình
                            PdfPCell cellLabel8 = new PdfPCell(new Phrase("Lương trung bình:", fontBold));
                            cellLabel8.Border = NO_BORDER;
                            cellLabel8.Padding = 3;
                            tableThongKe.AddCell(cellLabel8);

                            decimal luongTrungBinh = dtGridViewBCLuong.Rows.Count > 0 ? tongTongLuong / dtGridViewBCLuong.Rows.Count : 0;
                            PdfPCell cellValue8 = new PdfPCell(new Phrase(luongTrungBinh.ToString("#,##0"), fontBold));
                            cellValue8.Border = NO_BORDER;
                            cellValue8.Padding = 3;
                            cellValue8.HorizontalAlignment = Element.ALIGN_RIGHT;
                            tableThongKe.AddCell(cellValue8);

                            document.Add(tableThongKe);

                            /* ================= CHỮ KÝ ================= */
                            if (nguoiXuat != null)
                            {
                                Paragraph location = new Paragraph($"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}", fontNormal);
                                location.Alignment = Element.ALIGN_RIGHT;
                                location.SpacingBefore = 20;
                                document.Add(location);

                                Paragraph nguoiXuatLabel = new Paragraph("Người xuất", fontBold);
                                nguoiXuatLabel.Alignment = Element.ALIGN_RIGHT;
                                nguoiXuatLabel.SpacingBefore = 5;
                                document.Add(nguoiXuatLabel);

                                Paragraph nguoiXuatName = new Paragraph(nguoiXuat["HoTen_TuanhCD233018"].ToString(), fontBold);
                                nguoiXuatName.Alignment = Element.ALIGN_RIGHT;
                                nguoiXuatName.SpacingBefore = 60;
                                document.Add(nguoiXuatName);
                            }

                            // Đóng document
                            document.Close();

                            MessageBox.Show("Xuất PDF thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Mở file PDF sau khi xuất
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                            {
                                FileName = sfd.FileName,
                                UseShellExecute = true
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất PDF: " + ex.Message + "\n\n" + ex.StackTrace, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox) ((TextBox)ctl).Clear();
                else if (ctl is ComboBox) ((ComboBox)ctl).SelectedIndex = -1;
                else if (ctl.HasChildren) ClearAllInputs(ctl);
            }
        }

        private void F_BaoCaoLuong_Load(object sender, EventArgs e)
        {
            // Tự động load dữ liệu khi form load
            btnLuongHangThang_Click(sender, e);
        }
    }
}