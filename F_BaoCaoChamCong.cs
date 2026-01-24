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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using PdfFont = iTextSharp.text.Font;
using PdfRectangle = iTextSharp.text.Rectangle;
using PdfDocument = iTextSharp.text.Document;
using PdfWriter = iTextSharp.text.pdf.PdfWriter;
using PdfPCell = iTextSharp.text.pdf.PdfPCell;
using PdfPTable = iTextSharp.text.pdf.PdfPTable;
using BaseFont = iTextSharp.text.pdf.BaseFont;
using BaseColor = iTextSharp.text.BaseColor;
using Phrase = iTextSharp.text.Phrase;
using Paragraph = iTextSharp.text.Paragraph;
using Element = iTextSharp.text.Element;
using PageSize = iTextSharp.text.PageSize;

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
    -- ⭐ THÊM STT
    ROW_NUMBER() OVER (ORDER BY nv.MaNV_TuanhCD233018) AS N'STT',
    nv.MaNV_TuanhCD233018      AS N'Mã NV',
    nv.HoTen_TuanhCD233018     AS N'Họ tên',
    @Thang                     AS N'Tháng',
    @Nam                       AS N'Năm',

    COUNT(DISTINCT cc.Ngay_TuanhCD233018) AS N'Ngày công',
    s.SoNgayCongChuan           AS N'Công chuẩn'

FROM tblNhanVien_TuanhCD233018 nv
JOIN tblChucVu_KhangCD233181 cv 
    ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
JOIN tblPhongBan_ThuanCD233318 pb 
    ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318

LEFT JOIN tblChamCong_TuanhCD233018 cc 
       ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
      AND cc.DeletedAt_TuanhCD233018 = 0
      AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
      AND YEAR(cc.Ngay_TuanhCD233018) = @Nam

CROSS JOIN SoNgayCongChuan s

WHERE nv.DeletedAt_TuanhCD233018 = 0
";

                SqlCommand cmd = new SqlCommand(sql, cn.conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                // ===== PHÒNG BAN =====
                if (cbBoxMaPB.SelectedValue != null &&
                    cbBoxMaPB.SelectedValue.ToString() != "")
                {
                    sql += " AND pb.MaPB_ThuanCD233318 = @MaPB ";
                    cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue.ToString());
                }

                // ===== CHỨC VỤ =====
                if (cbBoxChucVu.SelectedValue != null &&
                    cbBoxChucVu.SelectedValue.ToString() != "")
                {
                    sql += " AND cv.MaCV_KhangCD233181 = @MaCV ";
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue.ToString());
                }

                sql += @"
GROUP BY 
    nv.MaNV_TuanhCD233018,
    nv.HoTen_TuanhCD233018,
    s.SoNgayCongChuan
ORDER BY nv.MaNV_TuanhCD233018";

                cmd.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtGridViewBCChamCong.DataSource = dt;
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
JOIN tblChucVu_KhangCD233181 CV 
    ON NV.MaCV_KhangCD233181 = CV.MaCV_KhangCD233181
JOIN tblPhongBan_ThuanCD233318 PB
    ON CV.MaPB_ThuanCD233318 = PB.MaPB_ThuanCD233318
WHERE NV.DeletedAt_TuanhCD233018 = 0
";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn.conn;
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                // ================= TEXTBOX TÌM KIẾM =================
                //tìm kiếm thông minh theo khoảng trắng phân biẹt rõ mã và tên 
                //string keyword = txtTimkiem.Text.Trim();
                //if (!string.IsNullOrEmpty(keyword))
                //{
                //    if (!keyword.Contains(" "))
                //    {
                //        sql += " AND NV.MaNV_TuanhCD233018 LIKE @MaNV ";
                //        cmd.Parameters.AddWithValue("@MaNV", "%" + keyword + "%");
                //    }
                //    else
                //    {
                //        sql += " AND NV.HoTen_TuanhCD233018 LIKE @TenNV ";
                //        cmd.Parameters.AddWithValue("@TenNV", "%" + keyword + "%");
                //    }
                //}

                // ================= TEXTBOX TÌM KIẾM (TÌM CẢ MÃ VÀ TÊN) =================
                string keyword = txtTimkiem.Text.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    sql += " AND (NV.MaNV_TuanhCD233018 LIKE @Keyword OR NV.HoTen_TuanhCD233018 LIKE @Keyword) ";
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                }
                // ================= COMBOBOX PHÒNG BAN =================
                if (cbBoxMaPB.SelectedValue != null &&
                    cbBoxMaPB.SelectedValue.ToString() != "")
                {
                    sql += " AND PB.MaPB_ThuanCD233318 = @MaPB ";
                    cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue.ToString());
                }

                // ================= COMBOBOX CHỨC VỤ =================
                if (cbBoxChucVu.SelectedValue != null &&
                    cbBoxChucVu.SelectedValue.ToString() != "")
                {
                    sql += " AND CV.MaCV_KhangCD233181 = @MaCV ";
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue.ToString());
                }

                sql += " ORDER BY NV.MaNV_TuanhCD233018, CC.Ngay_TuanhCD233018";
                cmd.CommandText = sql;

                new SqlDataAdapter(cmd).Fill(dtNguon);

                // ================== TẠO BẢNG HIỂN THỊ ==================
                DataTable table = new DataTable();
                // ⭐ THÊM CỘT STT
                table.Columns.Add("STT");
                table.Columns.Add("Mã NV");
                table.Columns.Add("Họ tên");

                int soNgay = DateTime.DaysInMonth(nam, thang);
                for (int i = 1; i <= soNgay; i++)
                    table.Columns.Add(i.ToString());

                DataTable dsNV = dtNguon.DefaultView.ToTable(
                    true, "Id_TuanhCD233018", "MaNV_TuanhCD233018", "HoTen_TuanhCD233018");

                // ⭐ BIẾN ĐẾM STT
                int stt = 1;
                foreach (DataRow nv in dsNV.Rows)
                {
                    DataRow row = table.NewRow();
                    // ⭐ GÁN STT
                    row["STT"] = stt++;
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

                    // ⭐ +3 vì cột 0 = STT, cột 1 = Mã NV, cột 2 = Họ tên
                    DataGridViewColumn col = dtGridViewBCChamCong.Columns[i + 2];

                    col.HeaderText = i.ToString("00") + "\n" + thu;
                    col.HeaderCell.Style.BackColor = bg;
                    col.HeaderCell.Style.ForeColor = fg;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.Width = 40;
                }

                // ================== GIAO DIỆN ==================
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
            // ⭐ Bỏ qua 3 cột đầu (STT, Mã NV, Họ tên)
            if (e.RowIndex < 0 || e.ColumnIndex < 3) return;

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
                cn.connect();

                int thang = dtpThoiGian.Value.Month;
                int nam = dtpThoiGian.Value.Year;
                string keyword = txtTimkiem.Text.Trim();

                // ===== TÌM KIẾM THEO CHẾ ĐỘ =====
                if (currentMode == 1) // SỐ NGÀY LÀM VIỆC
                {
                    string sql = @"
SET DATEFIRST 7;

WITH AllDays AS (
    SELECT 
        DATEADD(DAY, v.number, DATEFROMPARTS(@Nam, @Thang, 1)) AS Ngay
    FROM master.dbo.spt_values v
    WHERE v.type = 'P'
      AND v.number < DAY(EOMONTH(DATEFROMPARTS(@Nam, @Thang, 1)))
),
SoNgayCongChuan AS (
    SELECT COUNT(*) AS SoNgayCongChuan
    FROM AllDays
    WHERE DATEPART(WEEKDAY, Ngay) <> 1
)

SELECT 
    ROW_NUMBER() OVER (ORDER BY nv.MaNV_TuanhCD233018) AS N'STT',
    nv.MaNV_TuanhCD233018      AS N'Mã NV',
    nv.HoTen_TuanhCD233018     AS N'Họ tên',
    @Thang                     AS N'Tháng',
    @Nam                       AS N'Năm',
    COUNT(DISTINCT cc.Ngay_TuanhCD233018) AS N'Ngày công',
    s.SoNgayCongChuan           AS N'Công chuẩn'

FROM tblNhanVien_TuanhCD233018 nv
JOIN tblChucVu_KhangCD233181 cv 
    ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
JOIN tblPhongBan_ThuanCD233318 pb 
    ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
LEFT JOIN tblChamCong_TuanhCD233018 cc 
    ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
   AND cc.DeletedAt_TuanhCD233018 = 0
   AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
   AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
CROSS JOIN SoNgayCongChuan s
WHERE nv.DeletedAt_TuanhCD233018 = 0";

                    SqlCommand cmd = new SqlCommand(sql, cn.conn);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    // TÌM KIẾM
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        sql += @" AND (nv.MaNV_TuanhCD233018 LIKE @Keyword 
                          OR nv.HoTen_TuanhCD233018 LIKE @Keyword)";
                        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    }

                    if (!string.IsNullOrEmpty(cbBoxMaPB.SelectedValue?.ToString()))
                    {
                        sql += " AND pb.MaPB_ThuanCD233318 = @MaPB";
                        cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue.ToString());
                    }

                    if (!string.IsNullOrEmpty(cbBoxChucVu.SelectedValue?.ToString()))
                    {
                        sql += " AND cv.MaCV_KhangCD233181 = @MaCV";
                        cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue.ToString());
                    }

                    sql += @"
GROUP BY nv.MaNV_TuanhCD233018, nv.HoTen_TuanhCD233018, s.SoNgayCongChuan
ORDER BY nv.MaNV_TuanhCD233018";

                    cmd.CommandText = sql;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewBCChamCong.DataSource = dt;
                }
                else if (currentMode == 2) // ĐI TRỄ VỀ SỚM
                {
                    HienThiChamCong(thang, nam);
                }
                else if (currentMode == 3) // GIỜ RA VÀO
                {
                    string sql = @"
SELECT 
    ROW_NUMBER() OVER (ORDER BY nv.HoTen_TuanhCD233018) AS [STT],
    nv.MaNV_TuanhCD233018 AS [Mã NV],
    nv.HoTen_TuanhCD233018 AS [Họ tên],
    pb.TenPB_ThuanCD233318 AS [Phòng ban],
    cv.TenCV_KhangCD233181 AS [Chức vụ],
    cc.Ngay_TuanhCD233018 AS [Ngày],
    CONVERT(VARCHAR(8), cc.GioVao_TuanhCD233018, 108) AS [Giờ vào],
    CONVERT(VARCHAR(8), cc.GioVe_TuanhCD233018, 108) AS [Giờ về]
FROM tblChamCong_TuanhCD233018 cc
JOIN tblNhanVien_TuanhCD233018 nv 
    ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
JOIN tblChucVu_KhangCD233181 cv 
    ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
JOIN tblPhongBan_ThuanCD233318 pb 
    ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
WHERE cc.DeletedAt_TuanhCD233018 = 0
  AND nv.DeletedAt_TuanhCD233018 = 0
  AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
  AND YEAR(cc.Ngay_TuanhCD233018) = @Nam";

                    SqlCommand cmd = new SqlCommand(sql, cn.conn);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    // TÌM KIẾM
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        sql += @" AND (nv.MaNV_TuanhCD233018 LIKE @Keyword 
                          OR nv.HoTen_TuanhCD233018 LIKE @Keyword)";
                        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    }

                    if (!string.IsNullOrEmpty(cbBoxMaPB.SelectedValue?.ToString()))
                    {
                        sql += " AND pb.MaPB_ThuanCD233318 = @MaPB";
                        cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue.ToString());
                    }

                    if (!string.IsNullOrEmpty(cbBoxChucVu.SelectedValue?.ToString()))
                    {
                        sql += " AND cv.MaCV_KhangCD233181 = @MaCV";
                        cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue.ToString());
                    }

                    sql += " ORDER BY nv.HoTen_TuanhCD233018, cc.Ngay_TuanhCD233018";
                    cmd.CommandText = sql;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewBCChamCong.DataSource = dt;
                }

                if (dtGridViewBCChamCong.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu phù hợp!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
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

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Báo cáo chấm công");

                        // ===== ĐẾM CỘT HIỂN THỊ =====
                        var visibleCols = dtGridViewBCChamCong.Columns
                            .Cast<DataGridViewColumn>()
                            .Where(c => c.Visible)
                            .ToList();

                        int visibleColCount = visibleCols.Count;

                        // ===== TÊN CÔNG TY =====
                        ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                        ws.Range(1, 1, 1, visibleColCount).Merge();
                        ws.Row(1).Style.Font.Bold = true;
                        ws.Row(1).Style.Font.FontSize = 14;
                        ws.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // ===== TIÊU ĐỀ =====
                        //ws.Cell(2, 1).Value = "BÁO CÁO CHẤM CÔNG NHÂN VIÊN";
                        ws.Cell(2, 1).Value = $"BÁO CÁO CHẤM CÔNG NHÂN VIÊN - THÁNG {dtpThoiGian.Value:MM/yyyy}";
                        ws.Range(2, 1, 2, visibleColCount).Merge();
                        ws.Row(2).Style.Font.Bold = true;
                        ws.Row(2).Style.Font.FontSize = 16;
                        ws.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // ===== THỜI GIAN XUẤT =====
                        ws.Cell(3, 1).Value = "Thời gian xuất: " +
                            DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        ws.Row(3).Style.Font.Italic = true;

                        // ===== THÁNG / NĂM =====
                        //ws.Cell(5, 1).Value = "Tháng/Năm:";
                        //ws.Cell(5, 2).Value = dtpThoiGian.Value.ToString("MM/yyyy");
                        //ws.Cell(5, 1).Style.Font.Bold = true;
                        // ===== PHÒNG BAN =====
                        ws.Cell(5, 1).Value = "Phòng ban:";
                        ws.Cell(5, 2).Value =
                            cbBoxMaPB.SelectedIndex > 0 ? cbBoxMaPB.Text : "Tất cả";
                        ws.Cell(5, 1).Style.Font.Bold = true;

                        // ===== CHỨC VỤ =====
                        ws.Cell(6, 1).Value = "Chức vụ:";
                        ws.Cell(6, 2).Value =
                            cbBoxChucVu.SelectedIndex > 0 ? cbBoxChucVu.Text : "Tất cả";
                        ws.Cell(6, 1).Style.Font.Bold = true;


                        // ===== HEADER BẢNG =====
                        int startRow = 8;
                        ws.Cell(startRow, 1).Value = "BẢNG DỮ LIỆU CHẤM CÔNG";
                        ws.Range(startRow, 1, startRow, visibleColCount).Merge();
                        ws.Row(startRow).Style.Font.Bold = true;
                        ws.Row(startRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Row(startRow).Style.Fill.BackgroundColor = XLColor.LightGray;

                        // ===== TIÊU ĐỀ CỘT =====
                        int headerRow = startRow + 1;
                        // ===== CĂN ĐỘ RỘNG CỘT (QUAN TRỌNG) =====
                        for (int i = 0; i < visibleCols.Count; i++)
                        {
                            var col = visibleCols[i];

                            // 🔹 GHI CHÚ: CHỈNH ĐỘ RỘNG CỘT STT CHO EXCEL
                            if (col.Name.Contains("STT") || col.HeaderText.Contains("STT"))
                                ws.Column(i + 1).Width = 8;       // 👈 CỘT STT
                            else if (col.Name.Contains("HoTen") || col.HeaderText.Contains("Họ"))
                                ws.Column(i + 1).Width = 30;      // 👈 HỌ TÊN
                            else if (col.Name.Contains("MaNV") || col.HeaderText.Contains("Mã"))
                                ws.Column(i + 1).Width = 18;      // 👈 MÃ NV
                            else
                                ws.Column(i + 1).Width = 12;      // 👈 CỘT NGÀY / GIỜ
                        }
                        // ===== GHI TÊN CỘT =====
                        for (int i = 0; i < visibleCols.Count; i++)
                        {
                            ws.Cell(headerRow, i + 1).Value = visibleCols[i].HeaderText;

                            ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                            ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(headerRow, i + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            ws.Cell(headerRow, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        // ===== GHI DỮ LIỆU =====
                        int dataStartRow = headerRow + 1;

                        for (int r = 0; r < dtGridViewBCChamCong.Rows.Count; r++)
                        {
                            for (int c = 0; c < visibleCols.Count; c++)
                            {
                                var col = visibleCols[c];
                                var val = dtGridViewBCChamCong.Rows[r].Cells[col.Index].Value;

                                var cell = ws.Cell(dataStartRow + r, c + 1);

                                if (val is DateTime dt)
                                    cell.Value = dt.ToString("dd/MM/yyyy");
                                else if (val is TimeSpan ts)
                                    cell.Value = ts.ToString(@"hh\:mm");
                                else
                                    cell.Value = val?.ToString() ?? "";

                                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                // Căn lề đẹp
                                if (col.Name.Contains("HoTen") || col.HeaderText.Contains("Họ"))
                                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                else
                                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }
                        }

                        int lastDataRow = dataStartRow + dtGridViewBCChamCong.Rows.Count - 1;

                        // ===== BORDER =====
                        ws.Range(startRow, 1, lastDataRow, visibleColCount)
                            .Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        ws.Range(startRow, 1, lastDataRow, visibleColCount)
                            .Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                        // ===== CĂN ĐỘ RỘNG CỘT (QUAN TRỌNG) =====
                        for (int i = 0; i < visibleCols.Count; i++)
                        {
                            var col = visibleCols[i];

                            if (col.Name.Contains("HoTen") || col.HeaderText.Contains("Họ"))
                                ws.Column(i + 1).Width = 30;      // 👈 HỌ TÊN
                            else if (col.Name.Contains("MaNV") || col.HeaderText.Contains("Mã"))
                                ws.Column(i + 1).Width = 18;      // 👈 MÃ NV
                            else
                                ws.Column(i + 1).Width = 12;      // 👈 CỘT NGÀY / GIỜ
                        }

                        // ===== CHỮ KÝ =====
                        int signRow = lastDataRow + 2;
                        int signCol = visibleColCount - 2;

                        ws.Cell(signRow, signCol).Value =
                            $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}";
                        ws.Range(signRow, signCol, signRow, visibleColCount).Merge();
                        ws.Row(signRow).Style.Font.Italic = true;
                        ws.Row(signRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Cell(signRow + 1, signCol).Value = "Người lập báo cáo";
                        ws.Range(signRow + 1, signCol, signRow + 1, visibleColCount).Merge();
                        ws.Row(signRow + 1).Style.Font.Bold = true;
                        ws.Row(signRow + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Cell(signRow + 4, signCol).Value = nguoiDangNhap;
                        ws.Range(signRow + 4, signCol, signRow + 4, visibleColCount).Merge();
                        ws.Row(signRow + 4).Style.Font.Bold = true;
                        ws.Row(signRow + 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Bạn có muốn mở file vừa xuất?",
                        "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất Excel: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                LoadSoNgayLamViec();
            }
            else if (currentMode == 2)
            {
                HienThiChamCong(thang, nam);
            }
            else if (currentMode == 3) // ⭐ THÊM
            {
                LoadGioRaVao();
            }
        }
        private void xuatpdf_Click(object sender, EventArgs e)
            {
                if (dtGridViewBCChamCong.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất PDF!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "PDF File (*.pdf)|*.pdf",
                    FileName = $"BaoCaoChamCong_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    Document doc = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
                    PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                    doc.Open();

                    // ===== FONT TIẾNG VIỆT =====
                    string fontPath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
                        "arial.ttf");

                    BaseFont bf = BaseFont.CreateFont(
                        fontPath,
                        BaseFont.IDENTITY_H,
                        BaseFont.EMBEDDED);

                    iTextSharp.text.Font fTitle = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fHeader = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font fCell = new iTextSharp.text.Font(bf, 10);
                    iTextSharp.text.Font fItalic = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.ITALIC);

                    // ===== TÊN CÔNG TY =====
                    Paragraph company = new Paragraph(
                        "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM\n\n", fHeader);
                    company.Alignment = Element.ALIGN_CENTER;
                    doc.Add(company);

                // ===== TIÊU ĐỀ =====
                Paragraph title = new Paragraph( $"BÁO CÁO CHẤM CÔNG NHÂN VIÊN\nTháng {dtpThoiGian.Value:MM/yyyy}",  fTitle);
                title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);

                    // ===== THỜI GIAN XUẤT =====
                    Paragraph exportTime = new Paragraph(
                        $"Thời gian xuất: {DateTime.Now:dd/MM/yyyy HH:mm}\n",
                        fCell);
                    doc.Add(exportTime);

                    // ===== THÁNG / NĂM =====
                    Paragraph time = new Paragraph(
                        $"Tháng/Năm: {dtpThoiGian.Value:MM/yyyy}\n" +
                        $"Phòng ban: {cbBoxMaPB.Text}\n" +
                        $"Chức vụ: {cbBoxChucVu.Text}\n\n",
                        fCell);
                    doc.Add(time);

                    // ===== TẠO BẢNG =====
                    int visibleColCount = dtGridViewBCChamCong.Columns
                        .Cast<DataGridViewColumn>()
                        .Count(c => c.Visible);

                    PdfPTable table = new PdfPTable(visibleColCount);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 10;

                    // ===== SET WIDTH CỘT (HỌ TÊN RỘNG) =====
                    List<float> widths = new List<float>();

                    foreach (DataGridViewColumn col in dtGridViewBCChamCong.Columns)
                    {
                        if (!col.Visible) continue;

                        // 🔹 GHI CHÚ: CHỈNH ĐỘ RỘNG CỘT STT CHO PDF
                        if (col.Name.Contains("STT") || col.HeaderText.Contains("STT"))
                        {
                            widths.Add(1.5f); // 👈 CỘT STT – HẸP
                        }
                        else if (col.Name.Contains("MaNV") || col.HeaderText.Contains("Mã"))
                        {
                            widths.Add(3f); // 👈 MÃ NV
                        }
                        else if (col.Name.Contains("HoTen") || col.HeaderText.Contains("Họ"))
                        {
                            widths.Add(5.5f); // 👈 HỌ TÊN – RỘNG NHẤT
                        }
                        else
                        {
                            widths.Add(2.5f); // các cột ngày / giờ
                        }
                    }

                    table.SetWidths(widths.ToArray());

                    // ===== HEADER =====
                    foreach (DataGridViewColumn col in dtGridViewBCChamCong.Columns)
                    {
                        if (!col.Visible) continue;

                        PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, fHeader));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }

                    // ===== DATA =====
                    foreach (DataGridViewRow row in dtGridViewBCChamCong.Rows)
                    {
                        foreach (DataGridViewColumn col in dtGridViewBCChamCong.Columns)
                        {
                            if (!col.Visible) continue;

                            string value = "";

                            if (row.Cells[col.Index].Value is DateTime dt)
                                value = dt.ToString("dd/MM/yyyy");
                            else if (row.Cells[col.Index].Value is TimeSpan ts)
                                value = ts.ToString(@"hh\:mm");
                            else
                                value = row.Cells[col.Index].Value?.ToString() ?? "";

                            PdfPCell cell = new PdfPCell(new Phrase(value, fCell));

                            // HỌ TÊN CĂN TRÁI + XUỐNG DÒNG
                            if (col.Name.Contains("HoTen") || col.HeaderText.Contains("Họ"))
                            {
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                cell.NoWrap = false;
                            }
                            else if (col.Name.Contains("MaNV") || col.HeaderText.Contains("Mã"))
                            {
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.NoWrap = true;
                            }
                            else
                            {
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }

                            cell.Padding = 4;
                            table.AddCell(cell);
                        }
                    }

                    doc.Add(table);

                    // ===== CHỮ KÝ =====
                    doc.Add(new Paragraph("\n\n"));

                    PdfPTable signTable = new PdfPTable(1);
                    signTable.WidthPercentage = 40;
                    signTable.HorizontalAlignment = Element.ALIGN_RIGHT;

                    PdfPCell signCell = new PdfPCell(new Phrase(
                        $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}\n\n" +
                        "Người lập báo cáo\n\n\n" +
                        nguoiDangNhap,
                        fItalic));

                    signCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    signCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    signTable.AddCell(signCell);

                    doc.Add(signTable);
                    doc.Close();

                    MessageBox.Show("Xuất PDF thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (MessageBox.Show("Bạn có muốn mở file PDF không?",
                        "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xuất PDF: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void cbBoxMaPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int thang = dtpThoiGian.Value.Month;
            int nam = dtpThoiGian.Value.Year;

            // Kiểm tra chế độ hiện tại và load lại tương ứng
            if (currentMode == 1)
            {
                LoadSoNgayLamViec();
            }
            else if (currentMode == 2)
            {
                HienThiChamCong(thang, nam);
            }
            else if (currentMode == 3) // ⭐ THÊM
            {
                LoadGioRaVao();
            }

            string maPB = cbBoxMaPB.SelectedValue.ToString();
            LoadChucVuComboBox(maPB);
        }

        private void cbBoxChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            int thang = dtpThoiGian.Value.Month;
            int nam = dtpThoiGian.Value.Year;

            // Kiểm tra chế độ hiện tại và load lại tương ứng
            if (currentMode == 1)
            {
                LoadSoNgayLamViec();
            }
            else if (currentMode == 2)
            {
                HienThiChamCong(thang, nam);
            }
            else if (currentMode == 3) // ⭐ THÊM
            {
                LoadGioRaVao();
            }
        }

        private void txtTimkiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGioravao_Click(object sender, EventArgs e)
        {
            currentMode = 3; // Đặt chế độ là Giờ ra vào
            LoadGioRaVao();
        }
        private void LoadGioRaVao()
        {
            try
            {
                // GỠ EVENT CŨ (nếu có)
                dtGridViewBCChamCong.CellFormatting -= dtGridViewBCChamCong_CellFormatting;

                int thang = dtpThoiGian.Value.Month;
                int nam = dtpThoiGian.Value.Year;

                cn.connect();

                string sql = @"  SELECT 
                                ROW_NUMBER() OVER (ORDER BY nv.HoTen_TuanhCD233018) AS [STT],
                                nv.MaNV_TuanhCD233018 AS [Mã NV],
                                nv.HoTen_TuanhCD233018 AS [Họ tên],
                                pb.TenPB_ThuanCD233318 AS [Phòng ban],
                                cv.TenCV_KhangCD233181 AS [Chức vụ],
                                cc.Ngay_TuanhCD233018 AS [Ngày],
                                CONVERT(VARCHAR(8), cc.GioVao_TuanhCD233018, 108) AS [Giờ vào],
                                CONVERT(VARCHAR(8), cc.GioVe_TuanhCD233018, 108) AS [Giờ về]
                            FROM tblChamCong_TuanhCD233018 cc
                            JOIN tblNhanVien_TuanhCD233018 nv 
                                ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                            JOIN tblChucVu_KhangCD233181 cv 
                                ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                            JOIN tblPhongBan_ThuanCD233318 pb 
                                ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                            WHERE cc.DeletedAt_TuanhCD233018 = 0
                              AND nv.DeletedAt_TuanhCD233018 = 0
                              AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
                              AND YEAR(cc.Ngay_TuanhCD233018) = @Nam";

                SqlCommand cmd = new SqlCommand(sql, cn.conn);
                cmd.Parameters.AddWithValue("@Thang", thang);
                cmd.Parameters.AddWithValue("@Nam", nam);

                // ===== PHÒNG BAN =====
                if (cbBoxMaPB.SelectedValue != null &&
                    cbBoxMaPB.SelectedValue.ToString() != "")
                {
                    sql += " AND pb.MaPB_ThuanCD233318 = @MaPB ";
                    cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue.ToString());
                }

                // ===== CHỨC VỤ =====
                if (cbBoxChucVu.SelectedValue != null &&
                    cbBoxChucVu.SelectedValue.ToString() != "")
                {
                    sql += " AND cv.MaCV_KhangCD233181 = @MaCV ";
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue.ToString());
                }

                sql += " ORDER BY nv.HoTen_TuanhCD233018, cc.Ngay_TuanhCD233018";
                cmd.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtGridViewBCChamCong.DataSource = dt;

                // ===== ĐỊNH DẠNG HIỂN THỊ =====
                dtGridViewBCChamCong.EnableHeadersVisualStyles = true;
                dtGridViewBCChamCong.ColumnHeadersHeight = 25;
                dtGridViewBCChamCong.ReadOnly = true;
                dtGridViewBCChamCong.AllowUserToAddRows = false;

                // ===== ĐẶT ĐỘ RỘNG CỘT =====
                if (dtGridViewBCChamCong.Columns["STT"] != null)
                    dtGridViewBCChamCong.Columns["STT"].Width = 50;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load giờ ra vào: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void pnlControl_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}