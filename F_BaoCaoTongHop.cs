using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

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
        }

        private void F_BaoCaoTongHop_Load(object sender, EventArgs e)
        {
            dtGridViewBCTongHop.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtGridViewBCTongHop.MultiSelect = false;

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
            dtGridViewBCTongHop.ClearSelection();   // ← THÊM DÒNG NÀY

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
            dtGridViewBCTongHop.ClearSelection();   // ← THÊM

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
            dtGridViewBCTongHop.ClearSelection();   // ← THÊM

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

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCTongHop.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo");
                return;
            }

            bool exportSelectedOnly = dtGridViewBCTongHop.SelectedRows.Count == 1;
            string giaTriChon = exportSelectedOnly
                ? dtGridViewBCTongHop.SelectedRows[0].Cells[0].Value.ToString()
                : "";

            string loaiBaoCao =
                currentMode == 0 ? "PHÒNG BAN" :
                currentMode == 1 ? "CHỨC VỤ" :
                                   "DỰ ÁN";

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF file|*.pdf",
                FileName = exportSelectedOnly
                    ? $"BaoCao_{loaiBaoCao}_{giaTriChon}.pdf"
                    : $"BaoCaoTongHop_{DateTime.Now:ddMMyyyy}.pdf"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                // ===== FONT HỆ THỐNG (KHÔNG NHÚNG, KHÔNG COPY FILE) =====
                BaseFont bf = BaseFont.CreateFont(
                    @"C:\Windows\Fonts\arial.ttf",
                    BaseFont.IDENTITY_H,
                    BaseFont.NOT_EMBEDDED
                );
                iTextSharp.text.Font fontNormal =
                    new iTextSharp.text.Font(bf, 11);

                iTextSharp.text.Font fontBold =
                    new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD);

                iTextSharp.text.Font fontTitle =
                    new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD);

                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                doc.Open();

                // ===== HEADER =====
                Paragraph p;

                p = new Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontBold);
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                p = new Paragraph("PHÒNG NHÂN SỰ", fontNormal);
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Add(new Paragraph("\n"));

                p = new Paragraph(
                    exportSelectedOnly
                        ? $"BÁO CÁO NHÂN SỰ THEO {loaiBaoCao}: {giaTriChon}"
                        : $"BÁO CÁO TỔNG HỢP NHÂN SỰ THEO {loaiBaoCao}",
                    fontTitle
                );
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);

                doc.Add(new Paragraph($"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy}", fontNormal));
                doc.Add(new Paragraph("\n"));

                // ===== LẤY DỮ LIỆU =====
                DataTable dtTongHop = exportSelectedOnly
                    ? GetTongHopTheoGiaTri(giaTriChon)
                    : (DataTable)dtGridViewBCTongHop.DataSource;

                PdfPTable table = new PdfPTable(dtTongHop.Columns.Count);
                table.WidthPercentage = 100;

                // ===== HEADER BẢNG =====
                foreach (DataColumn col in dtTongHop.Columns)
                {
                    string header = col.ColumnName;

                    if (header.Contains("PhongBan")) header = "Phòng ban";
                    else if (header.Contains("ChucVu")) header = "Chức vụ";
                    else if (header.Contains("DuAn")) header = "Dự án";
                    else if (header.Contains("TongNhanVien")) header = "Tổng nhân viên";
                    else if (header.Contains("SoLuong")) header = "Số lượng";
                    else if (header.Contains("SoNhanVien")) header = "Số nhân viên";

                    PdfPCell cell = new PdfPCell(new Phrase(header, fontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }

                // ===== DỮ LIỆU =====
                foreach (DataRow r in dtTongHop.Rows)
                {
                    foreach (var item in r.ItemArray)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(item?.ToString(), fontNormal));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);
                    }
                }

                doc.Add(table);
                // ===== CHI TIẾT NHÂN VIÊN (GIỐNG EXCEL) =====
                if (exportSelectedOnly)
                {
                    doc.Add(new Paragraph("\nDANH SÁCH NHÂN VIÊN CHI TIẾT", fontBold));

                    DataTable dtChiTiet = GetChiTietNhanVien(giaTriChon);

                    PdfPTable ctTable = new PdfPTable(dtChiTiet.Columns.Count);
                    ctTable.WidthPercentage = 100;
                    ctTable.SpacingBefore = 10;

                    // Header
                    foreach (DataColumn col in dtChiTiet.Columns)
                    {
                        string header = col.ColumnName;

                        if (header.Contains("MaNV")) header = "Mã nhân viên";
                        else if (header.Contains("HoTen")) header = "Họ tên";
                        else if (header.Contains("TenCV")) header = "Chức vụ";
                        else if (header.Contains("TenPB")) header = "Phòng ban";
                        else if (header.Contains("TenDA")) header = "Dự án";

                        PdfPCell cell = new PdfPCell(new Phrase(header, fontBold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        ctTable.AddCell(cell);
                    }

                    // Data
                    foreach (DataRow r in dtChiTiet.Rows)
                    {
                        foreach (var item in r.ItemArray)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(item?.ToString(), fontNormal));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            ctTable.AddCell(cell);
                        }
                    }

                    doc.Add(ctTable);
                }


                // ===== FOOTER =====
                doc.Add(new Paragraph("\n\n"));
                p = new Paragraph(
                    $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}",
                    fontNormal
                );
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                p = new Paragraph("Người lập báo cáo", fontNormal);
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                p = new Paragraph("Vũ Minh Khang", fontBold);
                p.Alignment = Element.ALIGN_RIGHT;
                doc.Add(p);

                doc.Close();
            }

            MessageBox.Show("Xuất PDF thành công!", "Thông báo");
        }





        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCTongHop.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool exportSelectedOnly = dtGridViewBCTongHop.SelectedRows.Count == 1;
            string giaTriChon = exportSelectedOnly
                ? dtGridViewBCTongHop.SelectedRows[0].Cells[0].Value.ToString()
                : "";

            string loaiBaoCao =
                currentMode == 0 ? "PHÒNG BAN" :
                currentMode == 1 ? "CHỨC VỤ" :
                                   "DỰ ÁN";

            string fileName = exportSelectedOnly
                ? $"BaoCao_{loaiBaoCao}_{giaTriChon}_{DateTime.Now:ddMMyyyy}.xlsx"
                : $"BaoCaoTongHop_{DateTime.Now:ddMMyyyy}.xlsx";

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = fileName
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("BaoCao");
                    int row = 1;

                    // ========= LẤY DỮ LIỆU =========
                    DataTable dtTongHop = exportSelectedOnly
                        ? GetTongHopTheoGiaTri(giaTriChon)
                        : (DataTable)dtGridViewBCTongHop.DataSource;
                    int startCol = 2;   // cột B
                    int endCol = 8;     // CỐ ĐỊNH đến cột H (đủ rộng cho tiêu đề)
                    int colCount = dtTongHop.Columns.Count;


                    // ========= HEADER =========
                    ws.Cell(row, startCol).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                    ws.Range(row, startCol, row, endCol).Merge()
                        .Style.Font.SetBold().Font.SetFontSize(14)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    row++;

                    ws.Cell(row, startCol).Value = "PHÒNG NHÂN SỰ";
                    ws.Range(row, startCol, row, endCol).Merge()
                        .Style.Font.SetItalic()
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    row += 2;

                    ws.Cell(row, startCol).Value = exportSelectedOnly
                        ? $"BÁO CÁO NHÂN SỰ THEO {loaiBaoCao}: {giaTriChon}"
                        : $"BÁO CÁO TỔNG HỢP NHÂN SỰ THEO {loaiBaoCao}";

                    ws.Range(row, startCol, row, endCol).Merge()
                        .Style.Font.SetBold().Font.SetFontSize(16)
                        .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    row++;

                    ws.Cell(row, startCol).Value = $"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy}";
                    ws.Range(row, startCol, row, endCol).Merge()
                        .Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    row += 2;

                    // ========= BẢNG TỔNG HỢP =========
                    for (int i = 0; i < colCount; i++)
                    {
                        string header = dtTongHop.Columns[i].ColumnName;

                        if (header.Contains("PhongBan")) header = "Phòng ban";
                        else if (header.Contains("ChucVu")) header = "Chức vụ";
                        else if (header.Contains("DuAn")) header = "Dự án";
                        else if (header.Contains("TongNhanVien")) header = "Tổng nhân viên";
                        else if (header.Contains("SoLuong")) header = "Số lượng";
                        else if (header.Contains("SoNhanVien")) header = "Số nhân viên";

                        ws.Cell(row, startCol + i).Value = header;
                        ws.Cell(row, startCol + i).Style.Font.SetBold()
                            .Fill.SetBackgroundColor(XLColor.LightGray);
                    }
                    row++;

                    foreach (DataRow r in dtTongHop.Rows)
                    {
                        for (int i = 0; i < colCount; i++)
                        {
                            ws.Cell(row, startCol + i).Value = r[i]?.ToString();
                            ws.Cell(row, startCol + i).Style.Alignment.Horizontal =
                                i == 0 ? XLAlignmentHorizontalValues.Left
                                       : XLAlignmentHorizontalValues.Center;
                        }
                        row++;
                    }

                    var tongHopRange = ws.Range(row - dtTongHop.Rows.Count - 1, startCol, row - 1, endCol);
                    tongHopRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                    tongHopRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                    // ========= CHI TIẾT =========
                    if (exportSelectedOnly)
                    {
                        row += 2;
                        ws.Cell(row, startCol).Value = "DANH SÁCH NHÂN VIÊN CHI TIẾT";
                        ws.Range(row, startCol, row, endCol).Merge()
                            .Style.Font.SetBold();
                        row++;

                        DataTable dtChiTiet = GetChiTietNhanVien(giaTriChon);
                        int ctCols = dtChiTiet.Columns.Count;
                        int ctEndCol = startCol + ctCols - 1;

                        for (int i = 0; i < ctCols; i++)
                        {
                            string header = dtChiTiet.Columns[i].ColumnName;

                            if (header.Contains("MaNV")) header = "Mã nhân viên";
                            else if (header.Contains("HoTen")) header = "Họ tên";
                            else if (header.Contains("TenCV")) header = "Chức vụ";
                            else if (header.Contains("TenPB")) header = "Phòng ban";
                            else if (header.Contains("TenDA")) header = "Dự án";

                            ws.Cell(row, startCol + i).Value = header;
                            ws.Cell(row, startCol + i).Style.Font.SetBold()
                                .Fill.SetBackgroundColor(XLColor.LightGray);
                        }
                        row++;

                        foreach (DataRow r in dtChiTiet.Rows)
                        {
                            for (int i = 0; i < ctCols; i++)
                                ws.Cell(row, startCol + i).Value = r[i]?.ToString();
                            row++;
                        }

                        var ctRange = ws.Range(row - dtChiTiet.Rows.Count - 1, startCol, row - 1, ctEndCol);
                        ctRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                        ctRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    }

                    // ========= FOOTER =========
                    row += 2;
                    ws.Cell(row, endCol - 2).Value =
                        $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}";
                    row += 2;
                    ws.Cell(row, endCol - 2).Value = "Người lập báo cáo";
                    row += 2;
                    ws.Cell(row, endCol - 2).Value = "Vũ Minh Khang";
                    ws.Cell(row, endCol - 2).Style.Font.SetBold();

                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
                }

                MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        DataTable GetTongHopTheoGiaTri(string value)
        {
            DataTable dt = new DataTable();
            cn.connect();

            string sql =
                currentMode == 0 ? @"
            SELECT pb.TenPB_ThuanCD233318 AS PhongBan,
                   COUNT(nv.MaNV_TuanhCD233018) AS TongNhanVien
            FROM tblNhanVien_TuanhCD233018 nv
            JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
            WHERE pb.TenPB_ThuanCD233318 = @v
            GROUP BY pb.TenPB_ThuanCD233318"

                : currentMode == 1 ? @"
            SELECT cv.TenCV_KhangCD233181 AS ChucVu,
                   COUNT(nv.MaNV_TuanhCD233018) AS SoLuong
            FROM tblNhanVien_TuanhCD233018 nv
            JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            WHERE cv.TenCV_KhangCD233181 = @v
            GROUP BY cv.TenCV_KhangCD233181"

                : @"
            SELECT da.TenDA_KienCD233824 AS DuAn,
                   COUNT(ct.MaNV_TuanhCD233018) AS SoNhanVien
            FROM tblChiTietDuAn_KienCD233824 ct
            JOIN tblDuAn_KienCD233824 da ON ct.MaDA_KienCD233824 = da.MaDA_KienCD233824
            WHERE da.TenDA_KienCD233824 = @v
            GROUP BY da.TenDA_KienCD233824";

            using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
            {
                cmd.Parameters.AddWithValue("@v", value);
                new SqlDataAdapter(cmd).Fill(dt);
            }

            cn.disconnect();
            return dt;
        }
        DataTable GetChiTietNhanVien(string value)
        {
            DataTable dt = new DataTable();
            cn.connect();

            string sql =
                currentMode == 0 ? @"
            SELECT nv.MaNV_TuanhCD233018, nv.HoTen_TuanhCD233018,
                   cv.TenCV_KhangCD233181, pb.TenPB_ThuanCD233318
            FROM tblNhanVien_TuanhCD233018 nv
            JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
            WHERE pb.TenPB_ThuanCD233318 = @v"

                : currentMode == 1 ? @"
            SELECT nv.MaNV_TuanhCD233018, nv.HoTen_TuanhCD233018,
                   cv.TenCV_KhangCD233181
            FROM tblNhanVien_TuanhCD233018 nv
            JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            WHERE cv.TenCV_KhangCD233181 = @v"

                : @"
            SELECT nv.MaNV_TuanhCD233018, nv.HoTen_TuanhCD233018,
                   da.TenDA_KienCD233824
            FROM tblChiTietDuAn_KienCD233824 ct
            JOIN tblNhanVien_TuanhCD233018 nv ON ct.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
            JOIN tblDuAn_KienCD233824 da ON ct.MaDA_KienCD233824 = da.MaDA_KienCD233824
            WHERE da.TenDA_KienCD233824 = @v";

            using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
            {
                cmd.Parameters.AddWithValue("@v", value);
                new SqlDataAdapter(cmd).Fill(dt);
            }

            cn.disconnect();
            return dt;
        }


    }
}
