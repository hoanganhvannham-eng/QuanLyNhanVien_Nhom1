using ClosedXML.Excel;
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
using static QuanLyNhanVien3.F_FormMain;

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoNhanVien : Form
    {
        public F_BaoCaoNhanVien()
        {
            InitializeComponent();
        }
        string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName;
        connectData cn = new connectData();
        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).Clear();
                else if (ctl is ComboBox)
                    ((ComboBox)ctl).SelectedIndex = -1;
                else if (ctl is DateTimePicker)
                    ((DateTimePicker)ctl).Value = DateTime.Now;
                else if (ctl.HasChildren)
                    ClearAllInputs(ctl);
            }
        }

        private void btnThongKeNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"
                    SELECT 
                        nv.MaNV_TuanhCD233018 as N'Mã Nhân Viên', 
                        nv.HoTen_TuanhCD233018 as N'Họ Tên', 
                        pb.TenPB_ThuanCD233318 as N'Tên Phòng Ban', 
                        cv.TenCV_KhangCD233181 as N'Tên Chức Vụ', 
                        nv.Email_TuanhCD233018 as N'Email'
                    FROM tblNhanVien_TuanhCD233018 nv
                    JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                    ORDER BY pb.TenPB_ThuanCD233318, cv.TenCV_KhangCD233181;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsoluongtheogioitinh_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"
                    SELECT 
                        GioiTinh_TuanhCD233018 as N'Giới Tính', 
                        COUNT(*) as N'Số Lượng'
                    FROM tblNhanVien_TuanhCD233018
                    WHERE DeletedAt_TuanhCD233018 = 0
                    GROUP BY GioiTinh_TuanhCD233018;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatEXL_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCNhanVien.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "Excel Workbook|*.xlsx",
                    FileName = "DsachNhanVien_" + DateTime.Now.ToString("ddMMyyyy") // Thêm tên file mặc định
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("NhanVien");

                                // ===== TÊN CÔNG TY =====
                                ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                                ws.Range(1, 1, 1, 5).Merge();
                                ws.Cell(1, 1).Style.Font.Bold = true;
                                ws.Cell(1, 1).Style.Font.FontSize = 14;
                                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                // ===== TIÊU ĐỀ CHÍNH =====
                                ws.Cell(2, 1).Value = "DANH SÁCH NHÂN VIÊN";
                                ws.Range(2, 1, 2, 5).Merge();
                                ws.Cell(2, 1).Style.Font.Bold = true;
                                ws.Cell(2, 1).Style.Font.FontSize = 16;
                                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                // ===== NGÀY LẬP BÁO CÁO =====
                                ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy");
                                ws.Cell(3, 1).Style.Font.Italic = true;

                                // ===== TIÊU ĐỀ CỘT =====
                                int headerRow = 5;
                                for (int i = 0; i < dtGridViewBCNhanVien.Columns.Count; i++)
                                {
                                    ws.Cell(headerRow, i + 1).Value = dtGridViewBCNhanVien.Columns[i].HeaderText;
                                    ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                                    ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Cell(headerRow, i + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    ws.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }

                                // ===== GHI DỮ LIỆU =====
                                int dataStartRow = headerRow + 1;
                                for (int i = 0; i < dtGridViewBCNhanVien.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewBCNhanVien.Columns.Count; j++)
                                    {
                                        var cellValue = dtGridViewBCNhanVien.Rows[i].Cells[j].Value;

                                        // Xử lý DateTime
                                        if (cellValue is DateTime)
                                        {
                                            ws.Cell(dataStartRow + i, j + 1).Value = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            ws.Cell(dataStartRow + i, j + 1).Value = cellValue?.ToString() ?? "";
                                        }

                                        ws.Cell(dataStartRow + i, j + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                        // Căn giữa cho cột đầu tiên (Mã nhân viên), căn trái cho các cột khác
                                        if (j == 0)
                                        {
                                            ws.Cell(dataStartRow + i, j + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                        }
                                        else
                                        {
                                            ws.Cell(dataStartRow + i, j + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                                        }
                                    }
                                }

                                // ===== BORDER CHO BẢNG DỮ LIỆU =====
                                int lastDataRow = dataStartRow + dtGridViewBCNhanVien.Rows.Count - 1;
                                var tableRange = ws.Range(headerRow, 1, lastDataRow, dtGridViewBCNhanVien.Columns.Count);
                                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // ===== THỐNG KÊ =====
                                int statsRow = lastDataRow + 2;
                                ws.Cell(statsRow, 1).Value = "Tổng số nhân viên:";
                                ws.Cell(statsRow, 2).Value = dtGridViewBCNhanVien.Rows.Count;
                                ws.Cell(statsRow, 1).Style.Font.Bold = true;
                                ws.Cell(statsRow, 2).Style.Font.Bold = true;

                                // ===== PHẦN CHỮ KÝ =====
                                int signatureRow = lastDataRow + 4;
                                ws.Cell(signatureRow, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                                ws.Cell(signatureRow, 4).Style.Font.Italic = true;
                                ws.Cell(signatureRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow, 4, signatureRow, 5).Merge();

                                ws.Cell(signatureRow + 1, 4).Value = "Người lập báo cáo";
                                ws.Cell(signatureRow + 1, 4).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 1, 4, signatureRow + 1, 5).Merge();

                                // ===== TÊN NGƯỜI LẬP BÁO CÁO =====
                                ws.Cell(signatureRow + 3, 4).Value = nguoiDangNhap;
                                ws.Cell(signatureRow + 3, 4).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 3, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 3, 4, signatureRow + 3, 5).Merge();

                                // ===== TỰ ĐỘNG ĐIỀU CHỈNH CỘT =====
                                ws.Columns().AdjustToContents();

                                // Đặt chiều rộng tối thiểu cho các cột
                                for (int i = 1; i <= dtGridViewBCNhanVien.Columns.Count; i++)
                                {
                                    if (ws.Column(i).Width < 15)
                                        ws.Column(i).Width = 15;
                                }

                                // Đặt chiều cao dòng
                                ws.Row(1).Height = 25;
                                ws.Row(2).Height = 30;
                                ws.Row(headerRow).Height = 25;

                                // Lưu file
                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Hỏi có muốn mở file không
                            DialogResult openFile = MessageBox.Show("Bạn có muốn mở file vừa xuất không?", "Thông báo",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (openFile == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(sfd.FileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void timkiemtheomanhanvien()
        {
            try
            {
                cn.connect();
                string sql = @"
                    SELECT DISTINCT
                        nv.Id_TuanhCD233018       AS N'ID',
                        nv.MaNV_TuanhCD233018     AS N'Mã nhân viên',
                        nv.HoTen_TuanhCD233018    AS N'Họ tên',
                        nv.NgaySinh_TuanhCD233018 AS N'Ngày sinh',
                        nv.GioiTinh_TuanhCD233018 AS N'Giới tính',
                        nv.DiaChi_TuanhCD233018   AS N'Địa chỉ',
                        nv.SoDienThoai_TuanhCD233018 AS N'Số điện thoại',
                        nv.Email_TuanhCD233018    AS N'Email',
                        pb.TenPB_ThuanCD233318    AS N'Phòng ban',
                        cv.TenCV_KhangCD233181    AS N'Chức vụ',
                        hd.LuongCoBan_ChienCD232928 AS N'Lương cơ bản',
                        hd.LoaiHopDong_ChienCD232928 AS N'Loại hợp đồng'
                    FROM tblNhanVien_TuanhCD233018 nv
                    LEFT JOIN tblChucVu_KhangCD233181 cv 
                        ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181 
                        AND cv.DeletedAt_KhangCD233181 = 0
                    LEFT JOIN tblPhongBan_ThuanCD233318 pb 
                        ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318 
                        AND pb.DeletedAt_ThuanCD233318 = 0
                    LEFT JOIN tblHopDong_ChienCD232928 hd 
                        ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018
                        AND hd.DeletedAt_ChienCD232928 = 0
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                      AND nv.MaNV_TuanhCD233018 LIKE @TenTimKiem
                    ORDER BY pb.TenPB_ThuanCD233318, cv.TenCV_KhangCD233181";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenTimKiem",
                        "%" + textBoxmanhanvientimkiem.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dtGridViewBCNhanVien.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void timkiemtheoten()
        {
            try
            {
                cn.connect();
                string sql = @"
                    SELECT DISTINCT
                        nv.Id_TuanhCD233018       AS N'ID',
                        nv.MaNV_TuanhCD233018     AS N'Mã nhân viên',
                        nv.HoTen_TuanhCD233018    AS N'Họ tên',
                        nv.NgaySinh_TuanhCD233018 AS N'Ngày sinh',
                        nv.GioiTinh_TuanhCD233018 AS N'Giới tính',
                        nv.DiaChi_TuanhCD233018   AS N'Địa chỉ',
                        nv.SoDienThoai_TuanhCD233018 AS N'Số điện thoại',
                        nv.Email_TuanhCD233018    AS N'Email',
                        pb.TenPB_ThuanCD233318    AS N'Phòng ban',
                        cv.TenCV_KhangCD233181    AS N'Chức vụ',
                        hd.LuongCoBan_ChienCD232928 AS N'Lương cơ bản',
                        hd.LoaiHopDong_ChienCD232928 AS N'Loại hợp đồng'
                    FROM tblNhanVien_TuanhCD233018 nv
                    LEFT JOIN tblChucVu_KhangCD233181 cv 
                        ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181 
                        AND cv.DeletedAt_KhangCD233181 = 0
                    LEFT JOIN tblPhongBan_ThuanCD233318 pb 
                        ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318 
                        AND pb.DeletedAt_ThuanCD233318 = 0
                    LEFT JOIN tblHopDong_ChienCD232928 hd 
                        ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018
                        AND hd.DeletedAt_ChienCD232928 = 0
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                      AND nv.HoTen_TuanhCD233018 LIKE @TenTimKiem
                    ORDER BY pb.TenPB_ThuanCD233318, cv.TenCV_KhangCD233181";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenTimKiem",
                        "%" + txttimkiemtheoten.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dtGridViewBCNhanVien.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
        }

        private void txttimkiemtheoten_TextChanged(object sender, EventArgs e)
        {
            timkiemtheoten();
        }

        private void textBoxmanhanvientimkiem_TextChanged(object sender, EventArgs e)
        {
            timkiemtheomanhanvien();
        }

        private void F_BaoCaoNhanVien_Load(object sender, EventArgs e)
        {

        }

        private void dtGridViewBCNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txttimkiemtheoten_TextChanged_1(object sender, EventArgs e)
        {
            timkiemtheoten();
        }

        private void textBoxmanhanvientimkiem_TextChanged_1(object sender, EventArgs e)
        {
            timkiemtheomanhanvien();
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCNhanVien.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Files|*.pdf",
                FileName = "DanhSachNhanVien_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Thêm using cho iTextSharp ở đầu file nếu chưa có
                        // using iTextSharp.text;
                        // using iTextSharp.text.pdf;
                        // using System.IO;

                        // Tạo document PDF với kích thước A4 ngang (Landscape) để chứa nhiều cột
                        iTextSharp.text.Document doc = new iTextSharp.text.Document(
                            iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 30, 30);
                        PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();

                        // Load font hỗ trợ tiếng Việt
                        string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                        BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                        iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);
                        iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font fontItalic = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.ITALIC);
                        iTextSharp.text.Font fontTableHeader = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font fontTableData = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.NORMAL);

                        // ===== TIÊU ĐỀ CÔNG TY (CÓ VIỀN) =====
                        PdfPTable borderTable = new PdfPTable(1);
                        borderTable.WidthPercentage = 100;
                        PdfPCell borderCell = new PdfPCell(new iTextSharp.text.Phrase("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontTitle));
                        borderCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        borderCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                        borderCell.Padding = 8;
                        borderCell.Border = 0; // Bỏ viền
                        borderTable.AddCell(borderCell);
                        borderTable.SpacingAfter = 15f;
                        doc.Add(borderTable);

                        // ===== TIÊU ĐỀ CHÍNH =====
                        iTextSharp.text.Paragraph mainTitle = new iTextSharp.text.Paragraph("DANH SÁCH NHÂN VIÊN", fontHeader);
                        mainTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                        mainTitle.SpacingAfter = 10f;
                        doc.Add(mainTitle);

                        // ===== NGÀY LẬP BÁO CÁO =====
                        iTextSharp.text.Paragraph dateReport = new iTextSharp.text.Paragraph(
                            "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy"), fontItalic);
                        dateReport.SpacingAfter = 15f;
                        doc.Add(dateReport);

                        // ===== BẢNG NHÂN VIÊN =====
                        PdfPTable tableNhanVien = new PdfPTable(dtGridViewBCNhanVien.Columns.Count);
                        tableNhanVien.WidthPercentage = 100;
                        tableNhanVien.SpacingBefore = 10f;
                        tableNhanVien.SpacingAfter = 10f;

                        // Đặt độ rộng cột tự động dựa trên số lượng cột
                        float[] columnWidths = new float[dtGridViewBCNhanVien.Columns.Count];
                        for (int i = 0; i < dtGridViewBCNhanVien.Columns.Count; i++)
                        {
                            // Cột đầu tiên (ID/Mã) hẹp hơn, các cột khác rộng hơn
                            if (i == 0)
                                columnWidths[i] = 8f;
                            else if (i == 1)
                                columnWidths[i] = 12f;
                            else
                                columnWidths[i] = 15f;
                        }
                        tableNhanVien.SetWidths(columnWidths);

                        // Header bảng nhân viên
                        foreach (DataGridViewColumn column in dtGridViewBCNhanVien.Columns)
                        {
                            PdfPCell headerCell = new PdfPCell(new iTextSharp.text.Phrase(column.HeaderText, fontTableHeader));
                            headerCell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                            headerCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            headerCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                            headerCell.Padding = 8;
                            headerCell.BorderWidth = 1.5f;
                            tableNhanVien.AddCell(headerCell);
                        }

                        // Dữ liệu bảng nhân viên
                        foreach (DataGridViewRow row in dtGridViewBCNhanVien.Rows)
                        {
                            if (row.IsNewRow) continue;

                            for (int i = 0; i < dtGridViewBCNhanVien.Columns.Count; i++)
                            {
                                string cellValue = "";
                                if (row.Cells[i].Value != null)
                                {
                                    if (row.Cells[i].Value is DateTime)
                                    {
                                        cellValue = ((DateTime)row.Cells[i].Value).ToString("dd/MM/yyyy");
                                    }
                                    else if (row.Cells[i].Value is decimal || row.Cells[i].Value is double || row.Cells[i].Value is float)
                                    {
                                        // Format số tiền nếu là cột lương
                                        if (dtGridViewBCNhanVien.Columns[i].HeaderText.ToLower().Contains("lương"))
                                        {
                                            cellValue = string.Format("{0:N0}", row.Cells[i].Value);
                                        }
                                        else
                                        {
                                            cellValue = row.Cells[i].Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        cellValue = row.Cells[i].Value.ToString();
                                    }
                                }

                                PdfPCell dataCell = new PdfPCell(new iTextSharp.text.Phrase(cellValue, fontTableData));

                                // Căn giữa cho cột đầu tiên (ID/Mã), căn trái cho các cột khác
                                if (i == 0 || i == 1)
                                    dataCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                                else
                                    dataCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;

                                dataCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                                dataCell.Padding = 5;
                                tableNhanVien.AddCell(dataCell);
                            }
                        }

                        // Dòng tổng số nhân viên
                        PdfPCell totalLabelCell = new PdfPCell(new iTextSharp.text.Phrase("Tổng số nhân viên:", fontBold));
                        totalLabelCell.Padding = 5;
                        totalLabelCell.BorderWidth = 1.5f;
                        totalLabelCell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                        tableNhanVien.AddCell(totalLabelCell);

                        PdfPCell totalValueCell = new PdfPCell(new iTextSharp.text.Phrase(dtGridViewBCNhanVien.Rows.Count.ToString(), fontBold));
                        totalValueCell.Colspan = dtGridViewBCNhanVien.Columns.Count - 1;
                        totalValueCell.Padding = 5;
                        totalValueCell.BorderWidth = 1.5f;
                        totalValueCell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                        tableNhanVien.AddCell(totalValueCell);

                        doc.Add(tableNhanVien);

                        // ===== CHỮ KÝ =====
                        iTextSharp.text.Paragraph signature = new iTextSharp.text.Paragraph();
                        signature.SpacingBefore = 30f;
                        signature.Alignment = iTextSharp.text.Element.ALIGN_RIGHT;

                        signature.Add(new iTextSharp.text.Chunk(
                            "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "\n",
                            fontItalic));
                        signature.Add(new iTextSharp.text.Chunk("Người lập báo cáo\n\n\n\n", fontBold));
                        signature.Add(new iTextSharp.text.Chunk(nguoiDangNhap, fontBold));

                        doc.Add(signature);

                        // Đóng document
                        doc.Close();

                        MessageBox.Show("Xuất PDF thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DialogResult openFile = MessageBox.Show("Bạn có muốn mở file vừa xuất không?",
                            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (openFile == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xuất PDF: " + ex.Message + "\n\nChi tiết: " + ex.StackTrace,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}