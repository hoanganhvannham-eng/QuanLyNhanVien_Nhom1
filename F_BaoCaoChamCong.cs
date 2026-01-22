using ClosedXML.Excel;
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

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoChamCong : Form
    {
        public F_BaoCaoChamCong()
        {
            InitializeComponent();
        }

        string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName;
        // Khởi tạo đối tượng kết nối
        connectData cn = new connectData();

        // Biến lưu trạng thái hiện tại để nút tìm kiếm biết đang ở bảng nào
        // 0: Mặc định (chưa chọn gì hoặc xem lịch sử chi tiết)
        // 1: Đang xem Báo cáo Số ngày làm việc
        // 2: Đang xem Báo cáo Đi trễ về sớm
        private int currentMode = 0;

        // Hàm xóa input (nếu cần dùng sau này)
        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).Clear();
                else if (ctl is ComboBox)
                    ((ComboBox)ctl).SelectedIndex = -1;
                else if (ctl.HasChildren)
                    ClearAllInputs(ctl);
            }
        }

        // =======================================================
        // 1. Nút: Số ngày làm việc mỗi nhân viên trong tháng
        // =======================================================
        private void btnSoNgayLamViec_Click(object sender, EventArgs e)
        {
            currentMode = 1; // Đặt chế độ là Số ngày làm việc
            LoadSoNgayLamViec();
        }
        private void LoadSoNgayLamViec()
        {
            try
            {
                dtGridViewBCChamCong.CellFormatting -= dtGridViewBCChamCong_CellFormatting;
                int thang = dtpThoiGian.Value.Month;
                int nam = dtpThoiGian.Value.Year;

                cn.connect();

                string sql = @"
SET DATEFIRST 7;

-- 🔹 Danh sách ngày trong tháng
WITH AllDays AS (
    SELECT 
        DATEADD(DAY, v.number, DATEFROMPARTS(@Nam, @Thang, 1)) AS Ngay
    FROM master.dbo.spt_values v
    WHERE v.type = 'P'
      AND v.number < DAY(EOMONTH(DATEFROMPARTS(@Nam, @Thang, 1)))
),

-- 🔹 Ngày công chuẩn (trừ Chủ nhật)
SoNgayCongChuan AS (
    SELECT COUNT(*) AS SoNgayCongChuan
    FROM AllDays
    WHERE DATEPART(WEEKDAY, Ngay) <> 1
)

SELECT 
    nv.MaNV_TuanhCD233018      AS N'Mã NV',
    nv.HoTen_TuanhCD233018     AS N'Họ tên',
    @Thang                      AS N'Tháng',
    @Nam                        AS N'Năm',

    -- 🔹 Số ngày làm việc thực tế
    COUNT(DISTINCT cc.Ngay_TuanhCD233018) AS N'Số ngày làm việc',

    -- 🔹 Số ngày công chuẩn
    s.SoNgayCongChuan AS N'Số ngày công chuẩn'

FROM tblNhanVien_TuanhCD233018 nv
LEFT JOIN tblChamCong_TuanhCD233018 cc 
       ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
      AND cc.DeletedAt_TuanhCD233018 = 0
      AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
      AND YEAR(cc.Ngay_TuanhCD233018) = @Nam

CROSS JOIN SoNgayCongChuan s

WHERE nv.DeletedAt_TuanhCD233018 = 0

GROUP BY nv.MaNV_TuanhCD233018, nv.HoTen_TuanhCD233018, s.SoNgayCongChuan
ORDER BY nv.MaNV_TuanhCD233018";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dtGridViewBCChamCong.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi báo cáo chấm công: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }
        // =======================================================
        // 2. Nút: Nhân viên đi trễ hoặc về sớm
        // =======================================================
        private void btnDiTreVeSom_Click(object sender, EventArgs e)
        {
            currentMode = 2; // Đặt chế độ là Đi trễ về sớm

            int thang = dtpThoiGian.Value.Month;
            int nam = dtpThoiGian.Value.Year;

            // GỠ EVENT CŨ TRƯỚC KHI GẮN LẠI
            dtGridViewBCChamCong.CellFormatting -= dtGridViewBCChamCong_CellFormatting;
            dtGridViewBCChamCong.CellFormatting += dtGridViewBCChamCong_CellFormatting;

            HienThiChamCong(thang, nam);
        }

        private void HienThiChamCong(int thang, int nam)
        {
            try
            {
                cn.connect();

                dtGridViewBCChamCong.DataSource = null;
                dtGridViewBCChamCong.Columns.Clear();

                DataTable dtNguon = new DataTable();

                string sql = @"
            SELECT 
                NV.Id_TuanhCD233018,
                NV.MaNV_TuanhCD233018,
                NV.HoTen_TuanhCD233018,
                CC.Ngay_TuanhCD233018,
                CC.GioVao_TuanhCD233018,
                CC.GioVe_TuanhCD233018
            FROM tblNhanVien_TuanhCD233018 NV
            LEFT JOIN tblChamCong_TuanhCD233018 CC 
                ON NV.Id_TuanhCD233018 = CC.NhanVienId_TuanhCD233018
                AND MONTH(CC.Ngay_TuanhCD233018) = @Thang
                AND YEAR(CC.Ngay_TuanhCD233018) = @Nam
                AND CC.DeletedAt_TuanhCD233018 = 0
            WHERE NV.DeletedAt_TuanhCD233018 = 0
            ORDER BY NV.MaNV_TuanhCD233018, CC.Ngay_TuanhCD233018";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    new SqlDataAdapter(cmd).Fill(dtNguon);
                }

                // ================== TẠO BẢNG HIỂN THỊ ==================
                DataTable table = new DataTable();
                table.Columns.Add("Mã NV");
                table.Columns.Add("Họ tên");

                int soNgay = DateTime.DaysInMonth(nam, thang);
                for (int i = 1; i <= soNgay; i++)
                    table.Columns.Add(i.ToString());

                DataTable dsNV = dtNguon.DefaultView.ToTable(true, "Id_TuanhCD233018", "MaNV_TuanhCD233018", "HoTen_TuanhCD233018");

                foreach (DataRow nv in dsNV.Rows)
                {
                    DataRow row = table.NewRow();
                    row["Mã NV"] = nv["MaNV_TuanhCD233018"];
                    row["Họ tên"] = nv["HoTen_TuanhCD233018"];

                    // Mặc định Vắng
                    for (int i = 1; i <= soNgay; i++)
                        row[i.ToString()] = "V";

                    DataRow[] chamCong = dtNguon.Select(
                        $"Id_TuanhCD233018 = {nv["Id_TuanhCD233018"]} AND Ngay_TuanhCD233018 IS NOT NULL");

                    foreach (DataRow cc in chamCong)
                    {
                        DateTime ngay = Convert.ToDateTime(cc["Ngay_TuanhCD233018"]);
                        TimeSpan gioVao = (TimeSpan)cc["GioVao_TuanhCD233018"];
                        TimeSpan gioVe = (TimeSpan)cc["GioVe_TuanhCD233018"];

                        double soGio = Math.Round((gioVe - gioVao).TotalHours, 2);

                        row[ngay.Day.ToString()] = soGio.ToString();
                    }

                    table.Rows.Add(row);
                }

                dtGridViewBCChamCong.DataSource = table;

                // ================== FONT ==================
                dtGridViewBCChamCong.Font = new Font("Segoe UI", 10);
                dtGridViewBCChamCong.ColumnHeadersDefaultCellStyle.Font =
                    new Font("Segoe UI", 10, FontStyle.Bold);

                // ================== HEADER NGÀY + THỨ ==================
                dtGridViewBCChamCong.EnableHeadersVisualStyles = false;
                dtGridViewBCChamCong.ColumnHeadersHeight = 45;

                for (int i = 1; i <= soNgay; i++)
                {
                    DateTime date = new DateTime(nam, thang, i);
                    string thu;
                    Color bg = Color.White;
                    Color fg = Color.Black;

                    switch (date.DayOfWeek)
                    {
                        case DayOfWeek.Monday: thu = "T2"; break;
                        case DayOfWeek.Tuesday: thu = "T3"; break;
                        case DayOfWeek.Wednesday: thu = "T4"; break;
                        case DayOfWeek.Thursday: thu = "T5"; break;
                        case DayOfWeek.Friday: thu = "T6"; break;
                        case DayOfWeek.Saturday: thu = "T7"; break;
                        default:
                            thu = "CN";
                            bg = Color.LightPink;
                            fg = Color.Red;
                            break;
                    }

                    DataGridViewColumn col = dtGridViewBCChamCong.Columns[i + 1];
                    col.HeaderText = i.ToString("00") + "\n" + thu;
                    col.HeaderCell.Style.BackColor = bg;
                    col.HeaderCell.Style.ForeColor = fg;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.Width = 40;
                }

                dtGridViewBCChamCong.ReadOnly = true;
                dtGridViewBCChamCong.AllowUserToAddRows = false;
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



        private void dtGridViewBCChamCong_CellFormatting(
    object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Bỏ qua 2 cột đầu (Mã NV, Họ tên)
            if (e.RowIndex < 0 || e.ColumnIndex < 2) return;

            if (e.Value == null) return;

            string value = e.Value.ToString().Trim();

            // 🟡 Nghỉ phép
            if (value == "P")
            {
                e.CellStyle.BackColor = Color.Khaki;
                e.CellStyle.ForeColor = Color.Black;
                return;
            }

            // ⚪ Vắng
            if (value == "V")
            {
                e.CellStyle.BackColor = Color.Gainsboro;
                e.CellStyle.ForeColor = Color.Black;
                return;
            }

            // 🔢 Số giờ làm
            if (double.TryParse(value, out double soGio))
            {
                if (soGio >= 8)
                {
                    // 🟢 Làm đủ / quá giờ
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    // 🔴 Làm thiếu giờ
                    e.CellStyle.BackColor = Color.LightCoral;
                    e.CellStyle.ForeColor = Color.White;
                }
            }
        }


        // =======================================================
        // 3. Nút: Tìm kiếm (Thông minh theo ngữ cảnh)
        // =======================================================
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTimkiem.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên hoặc mã nhân viên để tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                int thang = dtpThoiGian.Value.Month;
                int nam = dtpThoiGian.Value.Year;
                string tuKhoa = "%" + txtTimkiem.Text + "%";
                string sql = "";

                // Kiểm tra xem đang ở chế độ xem báo cáo nào để tìm kiếm tương ứng
                if (currentMode == 1)
                {
                    // --- TÌM KIẾM TRONG BẢNG SỐ NGÀY LÀM VIỆC ---
                    sql = @"SELECT nv.MaNV_TuanhCD233018 as 'Mã Nhân Viên', nv.HoTen_TuanhCD233018 as 'Họ Tên', 
                            cv.TenCV_KhangCD233181 as N'Tên Chức Vụ', COUNT(cc.Id_TuanhCD233018) AS N'Số Ngày Làm Việc'
                            FROM tblNhanVien_TuanhCD233018 nv
                            JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                            JOIN tblChamCong_TuanhCD233018 cc ON nv.Id_TuanhCD233018 = cc.NhanVienId_TuanhCD233018
                            WHERE nv.DeletedAt_TuanhCD233018 = 0 
                              AND cc.DeletedAt_TuanhCD233018 = 0
                              AND MONTH(cc.Ngay_TuanhCD233018) = @Thang 
                              AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                              AND (nv.HoTen_TuanhCD233018 LIKE @TuKhoa OR nv.MaNV_TuanhCD233018 LIKE @TuKhoa)
                            GROUP BY nv.MaNV_TuanhCD233018, nv.HoTen_TuanhCD233018, cv.TenCV_KhangCD233181
                            ORDER BY N'Số Ngày Làm Việc' DESC, nv.HoTen_TuanhCD233018;";
                }
                else if (currentMode == 2)
                {
                    // --- TÌM KIẾM TRONG BẢNG ĐI TRỄ VỀ SỚM ---
                    sql = @"SELECT nv.MaNV_TuanhCD233018 as N'Mã Nhân Viên', nv.HoTen_TuanhCD233018 as N'Họ Tên', cc.Ngay_TuanhCD233018 as N'Ngày', cc.GioVao_TuanhCD233018 as N'Giờ Vào', cc.GioVe_TuanhCD233018 as N'Giờ Về',
                            CASE 
                                WHEN cc.GioVao_TuanhCD233018 <= '08:00:00' AND cc.GioVe_TuanhCD233018 >= '17:00:00' THEN N'Đi làm đúng giờ'
                                WHEN cc.GioVao_TuanhCD233018 <= '08:00:00' AND cc.GioVe_TuanhCD233018 < '17:00:00' THEN N'Đi đúng giờ - Về sớm ' + CAST(DATEDIFF(MINUTE, cc.GioVe_TuanhCD233018, '17:00:00') AS NVARCHAR(20)) + N' phút'
                                WHEN cc.GioVao_TuanhCD233018 > '08:00:00' AND cc.GioVe_TuanhCD233018 >= '17:00:00' THEN N'Đi muộn ' + CAST(DATEDIFF(MINUTE, '08:00:00', cc.GioVao_TuanhCD233018) AS NVARCHAR(20)) + N' phút - Về đúng giờ'
                                ELSE N'Đi muộn ' + CAST(DATEDIFF(MINUTE, '08:00:00', cc.GioVao_TuanhCD233018) AS NVARCHAR(20)) + N' phút - Về sớm ' + CAST(DATEDIFF(MINUTE, cc.GioVe_TuanhCD233018, '17:00:00') AS NVARCHAR(20)) + N' phút'
                            END AS N'Trạng Thái'
                            FROM tblChamCong_TuanhCD233018 cc
                            JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                            WHERE cc.DeletedAt_TuanhCD233018 = 0
                              AND MONTH(cc.Ngay_TuanhCD233018) = @Thang 
                              AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                              AND (nv.HoTen_TuanhCD233018 LIKE @TuKhoa OR nv.MaNV_TuanhCD233018 LIKE @TuKhoa)
                            ORDER BY cc.Ngay_TuanhCD233018 DESC, nv.HoTen_TuanhCD233018;";
                }
                else
                {
                    // --- MẶC ĐỊNH (Nếu chưa chọn bảng nào): Tìm lịch sử chấm công gốc ---
                    sql = @"SELECT nv.MaNV_TuanhCD233018 as N'Mã Nhân Viên', nv.HoTen_TuanhCD233018 as 'Họ Tên', cc.Ngay_TuanhCD233018 as N'Ngày', cc.GioVao_TuanhCD233018 as N'Giờ Vào', cc.GioVe_TuanhCD233018 as N'Giờ Về'
                            FROM tblChamCong_TuanhCD233018 cc
                            JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                            WHERE cc.DeletedAt_TuanhCD233018 = 0
                              AND MONTH(cc.Ngay_TuanhCD233018) = @Thang 
                              AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                              AND (nv.HoTen_TuanhCD233018 LIKE @TuKhoa OR nv.MaNV_TuanhCD233018 LIKE @TuKhoa)
                            ORDER BY cc.Ngay_TuanhCD233018 DESC;";
                }

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    cmd.Parameters.AddWithValue("@TuKhoa", tuKhoa);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCChamCong.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        // =======================================================
        // 4. Nút: Xuất Excel (Sử dụng ClosedXML)
        // =======================================================
        private void btnXuat_Click(object sender, EventArgs e)
{
    if (dtGridViewBCChamCong.Rows.Count == 0)
    {
        MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    using (SaveFileDialog sfd = new SaveFileDialog())
    {
        sfd.Filter = "Excel Workbook|*.xlsx";
        sfd.FileName = $"BaoCaoChamCong_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

        if (sfd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Báo cáo chấm công");

                    // Đếm số cột hiển thị
                    int visibleColCount = 0;
                    foreach (DataGridViewColumn col in dtGridViewBCChamCong.Columns)
                    {
                        if (col.Visible)
                            visibleColCount++;
                    }

                    // ===== TÊN CÔNG TY =====
                    ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                    ws.Range(1, 1, 1, visibleColCount).Merge();
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Font.FontSize = 14;
                    ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    // ===== TIÊU ĐỀ CHÍNH =====
                    ws.Cell(2, 1).Value = "BÁO CÁO CHẤM CÔNG NHÂN VIÊN";
                    ws.Range(2, 1, 2, visibleColCount).Merge();
                    ws.Cell(2, 1).Style.Font.Bold = true;
                    ws.Cell(2, 1).Style.Font.FontSize = 16;
                    ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    // ===== NGÀY LẬP BÁO CÁO =====
                    ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy");
                    ws.Cell(3, 1).Style.Font.Italic = true;

                    // ===== THÔNG TIN THÁNG/NĂM =====
                    ws.Cell(5, 1).Value = "Tháng/Năm";
                    ws.Cell(5, 2).Value = dtpThoiGian.Value.ToString("MM/yyyy");
                    ws.Cell(5, 1).Style.Font.Bold = true;

                    // ===== HEADER BẢNG DỮ LIỆU =====
                    int startRow = 7;
                    ws.Cell(startRow, 1).Value = "BẢNG DỮ LIỆU CHẤM CÔNG";
                    ws.Range(startRow, 1, startRow, visibleColCount).Merge();
                    ws.Cell(startRow, 1).Style.Font.Bold = true;
                    ws.Cell(startRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(startRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                    // ===== TIÊU ĐỀ CỘT =====
                    int headerRow = startRow + 1;
                    int colIndex = 1;

                    foreach (DataGridViewColumn col in dtGridViewBCChamCong.Columns)
                    {
                        if (col.Visible)
                        {
                            ws.Cell(headerRow, colIndex).Value = col.HeaderText;
                            ws.Cell(headerRow, colIndex).Style.Font.Bold = true;
                            ws.Cell(headerRow, colIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(headerRow, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                            colIndex++;
                        }
                    }

                    // ===== GHI DỮ LIỆU =====
                    int dataStartRow = headerRow + 1;
                    for (int i = 0; i < dtGridViewBCChamCong.Rows.Count; i++)
                    {
                        colIndex = 1;
                        foreach (DataGridViewColumn col in dtGridViewBCChamCong.Columns)
                        {
                            if (col.Visible)
                            {
                                var cellValue = dtGridViewBCChamCong.Rows[i].Cells[col.Index].Value;

                                // Xử lý DateTime
                                if (cellValue is DateTime)
                                {
                                    ws.Cell(dataStartRow + i, colIndex).Value = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                                }
                                else if (cellValue is TimeSpan)
                                {
                                    ws.Cell(dataStartRow + i, colIndex).Value = ((TimeSpan)cellValue).ToString(@"hh\:mm\:ss");
                                }
                                else
                                {
                                    ws.Cell(dataStartRow + i, colIndex).Value = cellValue?.ToString() ?? "";
                                }

                                ws.Cell(dataStartRow + i, colIndex).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                colIndex++;
                            }
                        }
                    }

                    // ===== BORDER CHO BẢNG DỮ LIỆU =====
                    int lastDataRow = dataStartRow + dtGridViewBCChamCong.Rows.Count - 1;
                    var tableRange = ws.Range(startRow, 1, lastDataRow, visibleColCount);
                    tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // ===== PHẦN CHỮ KÝ =====
                    int signatureRow = lastDataRow + 2;
                    
                    // Tính vị trí cột cho chữ ký (3 cột cuối)
                    int signatureStartCol = visibleColCount - 2;
                    
                    ws.Cell(signatureRow, signatureStartCol).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                    ws.Cell(signatureRow, signatureStartCol).Style.Font.Italic = true;
                    ws.Cell(signatureRow, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(signatureRow, signatureStartCol, signatureRow, visibleColCount).Merge();

                    ws.Cell(signatureRow + 1, signatureStartCol).Value = "Người lập báo cáo";
                    ws.Cell(signatureRow + 1, signatureStartCol).Style.Font.Bold = true;
                    ws.Cell(signatureRow + 1, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(signatureRow + 1, signatureStartCol, signatureRow + 1, visibleColCount).Merge();

                    ws.Cell(signatureRow + 4, signatureStartCol).Value = nguoiDangNhap;
                    ws.Cell(signatureRow + 4, signatureStartCol).Style.Font.Bold = true;
                    ws.Cell(signatureRow + 4, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(signatureRow + 4, signatureStartCol, signatureRow + 4, visibleColCount).Merge();

                    // ===== TỰ ĐỘNG ĐIỀU CHỈNH CỘT =====
                    ws.Columns().AdjustToContents();

                    // Đặt chiều rộng tối thiểu cho các cột
                    for (int i = 1; i <= visibleColCount; i++)
                    {
                        if (ws.Column(i).Width < 12)
                            ws.Column(i).Width = 12;
                    }

                    // Đặt chiều cao dòng
                    ws.Row(1).Height = 20;
                    ws.Row(2).Height = 25;
                    ws.Row(startRow).Height = 20;
                    ws.Row(headerRow).Height = 20;

                    // Lưu file
                    wb.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Hỏi có muốn mở file không
                if (MessageBox.Show("Bạn có muốn mở file vừa xuất?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


        private void F_BaoCaoChamCong_Load(object sender, EventArgs e)
        {
            dtpThoiGian.Format = DateTimePickerFormat.Custom;
            dtpThoiGian.CustomFormat = "MM/yyyy";
            dtpThoiGian.ShowUpDown = true;
            LoadcomboBox();
        }


        private void LoadcomboBox()
        {
            try
            {
                cn.connect();
                // Load Phòng ban
                string sqlLoadcomboBoxtblPhongBan = "SELECT '' AS MaPB_ThuanCD233318, N'-- Tất cả phòng ban --' AS TenPB_ThuanCD233318 UNION ALL SELECT MaPB_ThuanCD233318, TenPB_ThuanCD233318 FROM tblPhongBan_ThuanCD233318 WHERE DeletedAt_ThuanCD233318 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxtblPhongBan, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxMaPB.DataSource = ds.Tables[0];
                    cbBoxMaPB.DisplayMember = "TenPB_ThuanCD233318";
                    cbBoxMaPB.ValueMember = "MaPB_ThuanCD233318";
                }

                // Load Chức vụ - ban đầu hiển thị tất cả
                LoadChucVuComboBox("");

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load combobox: " + ex.Message);
            }
        }

        // Load chức vụ theo phòng ban
        private void LoadChucVuComboBox(string maPB)
        {
            try
            {
                cn.connect();
                string sqlLoadChucVu = "";

                if (string.IsNullOrEmpty(maPB))
                {
                    // Load tất cả chức vụ
                    sqlLoadChucVu = "SELECT '' AS MaCV_KhangCD233181, N'-- Tất cả chức vụ --' AS TenCV_KhangCD233181 UNION ALL SELECT MaCV_KhangCD233181, TenCV_KhangCD233181 FROM tblChucVu_KhangCD233181 WHERE DeletedAt_KhangCD233181 = 0";
                }
                else
                {
                    // Load chức vụ theo phòng ban
                    sqlLoadChucVu = "SELECT '' AS MaCV_KhangCD233181, N'-- Tất cả chức vụ --' AS TenCV_KhangCD233181 UNION ALL SELECT MaCV_KhangCD233181, TenCV_KhangCD233181 FROM tblChucVu_KhangCD233181 WHERE DeletedAt_KhangCD233181 = 0 AND MaPB_ThuanCD233318 = @MaPB";
                }

                using (SqlCommand cmd = new SqlCommand(sqlLoadChucVu, cn.conn))
                {
                    if (!string.IsNullOrEmpty(maPB))
                    {
                        cmd.Parameters.AddWithValue("@MaPB", maPB);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxChucVu.DataSource = ds.Tables[0];
                    cbBoxChucVu.DisplayMember = "TenCV_KhangCD233181";
                    cbBoxChucVu.ValueMember = "MaCV_KhangCD233181";
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load chức vụ: " + ex.Message);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtpThoiGian_ValueChanged(object sender, EventArgs e)
        {
            int thang = dtpThoiGian.Value.Month;
            int nam = dtpThoiGian.Value.Year;

            // Kiểm tra chế độ hiện tại và load lại tương ứng
            if (currentMode == 1)
            {
                // Đang ở chế độ Số ngày làm việc
                LoadSoNgayLamViec();
            }
            else if (currentMode == 2)
            {
                // Đang ở chế độ Đi trễ về sớm
                HienThiChamCong(thang, nam);
            }
        }

        private void xuatpdf_Click(object sender, EventArgs e)
        {

        }
    }
}