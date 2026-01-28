using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;
using System.Drawing; // nếu bạn dùng Color ở chỗ khác
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoChiTiet : Form
    {
        connectData cn = new connectData();

        string loaiBaoCao;   // "DUAN" | "PHONGBAN" | "CHUCVU"
        string giaTri;
        DateTime tuNgay;
        DateTime denNgay;
        bool nguoiDungDaChonDong = false;

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
            dgvChiTiet.CellClick += dgvChiTiet_CellClick;
        }
        private void dgvChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                nguoiDungDaChonDong = true;
        }

        // ===============================
        // LOAD FORM
        // ===============================
        private void F_BaoCaoChiTiet_Load(object sender, EventArgs e)
        {
            nguoiDungDaChonDong = false;

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
            dgvChiTiet.ClearSelection();
            nguoiDungDaChonDong = false;

            cn.disconnect();
        }

        // ===============================
        // PHÂN TÍCH HỢP ĐỒNG
        // ===============================
        void PhanTichDuLieu(DataTable dt)
        {
            int tong = dt.Rows.Count;
            int conHan = 0, sapHet = 0, hetHan = 0;

            DateTime today = DateTime.Today;

            foreach (DataRow r in dt.Rows)
            {
                if (r["NgayKetThucHD"] == DBNull.Value)
                {
                    conHan++;
                    continue;
                }

                DateTime ngayKT = Convert.ToDateTime(r["NgayKetThucHD"]);
                double days = (ngayKT.Date - today).TotalDays;

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
            DateTime today = DateTime.Today;

            double days = (ngayKT.Date - today).TotalDays;

            if (days < 0)
                row.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;      // Hết hạn
            else if (days <= 30)
                row.DefaultCellStyle.BackColor = System.Drawing.Color.LemonChiffon;  // Sắp hết hạn
            else
                row.DefaultCellStyle.BackColor = System.Drawing.Color.Honeydew;      // Còn hạn
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

        // ===============================
        // XUẤT EXCEL (ClosedXML) - HOÀN CHỈNH
        // ===============================
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChiTiet == null || dgvChiTiet.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Nếu dgv có dòng "new row" cuối (AllowUserToAddRows = true) thì bỏ dòng đó
                int realRowCount = dgvChiTiet.AllowUserToAddRows ? dgvChiTiet.Rows.Count - 1 : dgvChiTiet.Rows.Count;
                if (realRowCount <= 0)
                {
                    MessageBox.Show("Không có dữ liệu hợp lệ để xuất!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                    sfd.Title = "Lưu báo cáo Excel";
                    sfd.FileName = $"BaoCao_{SanitizeFileName(lblTitle.Text)}_{DateTime.Now:ddMMyyyy}.xlsx";

                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    bool xuatMotNV = false;
                    string maNV = null;

                    // CHỈ xuất 1 NV nếu user ĐÃ CLICK CHỌN
                    if (nguoiDungDaChonDong && dgvChiTiet.CurrentRow != null)
                    {
                        if (dgvChiTiet.Columns.Contains("MaNV"))
                        {
                            maNV = SafeToString(dgvChiTiet.CurrentRow.Cells["MaNV"].Value);
                            if (!string.IsNullOrWhiteSpace(maNV))
                                xuatMotNV = true;
                        }
                    }


                    ExportExcel_BaoCaoChiTiet(sfd.FileName, xuatMotNV, maNV);

                    MessageBox.Show("Xuất Excel thành công!", "OK",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất Excel thất bại!\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ===============================
        // HÀM XUẤT EXCEL CHÍNH
        // ===============================
        private void ExportExcel_BaoCaoChiTiet(string filePath, bool xuatMotNV, string maNV)
        {
            // CẤU HÌNH "TÊN CÔNG TY / PHÒNG" (bạn đổi theo ý)
            string tenCongTy = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
            string tenPhong = "PHÒNG NHÂN SỰ";

            int colCount = dgvChiTiet.Columns.Count;
            int realRowCount = dgvChiTiet.AllowUserToAddRows ? dgvChiTiet.Rows.Count - 1 : dgvChiTiet.Rows.Count;

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("BaoCao");
                ws.Style.Font.FontName = "Times New Roman";
                ws.Style.Font.FontSize = 12;

                int r = 1;

                // ===== HEADER CHUẨN =====
                WriteHeader(ws, ref r, colCount, tenCongTy, tenPhong, lblTitle.Text, lblContext.Text);

                // ===== BẢNG DỮ LIỆU =====
                int tableHeaderRow = r;
                WriteTableHeader(ws, r, colCount);
                r++;

                // data rows
                // ===== DATA ROWS =====
                if (xuatMotNV && dgvChiTiet.CurrentRow != null)
                {
                    // ===== CHỈ XUẤT 1 DÒNG ĐƯỢC CHỌN =====
                    DataGridViewRow dgvr = dgvChiTiet.CurrentRow;

                    WriteTableRow(ws, r, colCount, dgvr);
                    ApplyContractColor(ws, r, dgvr);
                    r++;
                }
                else
                {
                    // ===== XUẤT TOÀN BỘ =====
                    for (int i = 0; i < realRowCount; i++)
                    {
                        DataGridViewRow dgvr = dgvChiTiet.Rows[i];

                        WriteTableRow(ws, r, colCount, dgvr);
                        ApplyContractColor(ws, r, dgvr);
                        r++;
                    }
                }

                // set border toàn bảng
                var tableRange = ws.Range(tableHeaderRow, 1, r - 1, colCount);
                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                r += 2;

                // ===== CHI TIẾT NHÂN VIÊN (NẾU CHỌN 1) =====
                if (xuatMotNV && !string.IsNullOrWhiteSpace(maNV))
                {
                    WriteNhanVienChiTiet(ws, ref r, colCount, maNV);
                    r += 1;
                }

                // ===== FOOTER CHUẨN =====
                WriteFooter(ws, ref r, colCount);

                // autosize + format ngày
                ws.Columns().AdjustToContents();

                // cố gắng format cột ngày nếu tồn tại
                FormatDateColumnIfExists(ws, "Ngày bắt đầu HĐ", colCount, tableHeaderRow, r);
                FormatDateColumnIfExists(ws, "Ngày kết thúc HĐ", colCount, tableHeaderRow, r);

                // lưu file an toàn
                EnsureDirectory(filePath);

                // nếu file đang mở -> báo lỗi rõ ràng
                try
                {
                    wb.SaveAs(filePath);
                }
                catch (IOException)
                {
                    throw new IOException("Không thể lưu file. Có thể file đang được mở. Hãy đóng file Excel rồi xuất lại.");
                }
            }
        }


        // ===============================
        // HEADER
        // ===============================
        private void WriteHeader(IXLWorksheet ws, ref int r, int colCount,
            string tenCongTy, string tenPhong, string tenBaoCao, string context)
        {
            // Dòng 1: tên công ty
            ws.Range(r, 1, r, colCount).Merge().SetValue(tenCongTy);
            ws.Row(r).Style.Font.Bold = true;
            ws.Row(r).Style.Font.FontSize = 16;
            ws.Row(r).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r++;

            // Dòng 2: phòng
            ws.Range(r, 1, r, colCount).Merge().SetValue(tenPhong);
            ws.Row(r).Style.Font.Bold = true;
            ws.Row(r).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r++;

            // Dòng 3: tên báo cáo
            ws.Range(r, 1, r, colCount).Merge().SetValue(SafeToString(tenBaoCao));
            ws.Row(r).Style.Font.Bold = true;
            ws.Row(r).Style.Font.FontSize = 14;
            ws.Row(r).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r++;

            // Dòng 4: context
            ws.Range(r, 1, r, colCount).Merge().SetValue(SafeToString(context));
            ws.Row(r).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r++;

            // Dòng 5: ngày xuất
            ws.Range(r, 1, r, colCount).Merge().SetValue($"Ngày xuất: {DateTime.Now:dd/MM/yyyy}");
            ws.Row(r).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r += 2;
        }


        // ===============================
        // TABLE HEADER
        // ===============================
        private void WriteTableHeader(IXLWorksheet ws, int row, int colCount)
        {
            for (int c = 0; c < colCount; c++)
            {
                string header = dgvChiTiet.Columns[c].HeaderText;
                var cell = ws.Cell(row, c + 1);
                cell.SetValue(SafeToString(header));
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
        }


        // ===============================
        // TABLE ROW
        // ===============================
        private void WriteTableRow(IXLWorksheet ws, int row, int colCount, DataGridViewRow dgvr)
        {
            for (int c = 0; c < colCount; c++)
            {
                object val = dgvr.Cells[c].Value;

                // SetValue string để tránh lỗi object->XLCellValue
                ws.Cell(row, c + 1).SetValue(SafeToString(val));

                // canh giữa cho cột ngày / mã (tùy bạn)
                string header = dgvChiTiet.Columns[c].HeaderText?.ToLower() ?? "";
                if (header.Contains("mã") || header.Contains("ngày"))
                    ws.Cell(row, c + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                else
                    ws.Cell(row, c + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            }
        }


        // ===============================
        // TÔ MÀU THEO HỢP ĐỒNG (nếu có cột NgayKetThucHD)
        // ===============================
        private void ApplyContractColor(IXLWorksheet ws, int excelRow, DataGridViewRow dgvr)
        {
            if (!dgvChiTiet.Columns.Contains("NgayKetThucHD")) return;

            object v = dgvr.Cells["NgayKetThucHD"].Value;
            if (v == null || v == DBNull.Value) return;

            if (!DateTime.TryParse(v.ToString(), out DateTime ngayKT)) return;

            double days = (ngayKT.Date - DateTime.Today).TotalDays;

            if (days < 0)
                ws.Row(excelRow).Style.Fill.BackgroundColor = XLColor.MistyRose;
            else if (days <= 30)
                ws.Row(excelRow).Style.Fill.BackgroundColor = XLColor.LemonChiffon;
            else
                ws.Row(excelRow).Style.Fill.BackgroundColor = XLColor.Honeydew;
        }


        // ===============================
        // CHI TIẾT NHÂN VIÊN (DÁN THÊM DƯỚI BẢNG)
        // (mình làm dạng key-value + có lịch sử HĐ)
        // ===============================
        private void WriteNhanVienChiTiet(IXLWorksheet ws, ref int r, int colCount, string maNV)
        {
            // title
            ws.Range(r, 1, r, colCount).Merge().SetValue("BÁO CÁO CHI TIẾT NHÂN VIÊN");
            ws.Row(r).Style.Font.Bold = true;
            ws.Row(r).Style.Font.FontSize = 14;
            ws.Row(r).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r += 2;

            // 1) THÔNG TIN CƠ BẢN + CÔNG VIỆC
            DataTable dtThongTin = GetNhanVienChiTiet_KeyValue(maNV);

            ws.Cell(r, 1).SetValue("Thông tin nhân viên");
            ws.Cell(r, 1).Style.Font.Bold = true;
            r++;

            int startRowKV = r;
            for (int i = 0; i < dtThongTin.Rows.Count; i++)
            {
                ws.Cell(r, 1).SetValue(SafeToString(dtThongTin.Rows[i]["Ten"]));
                ws.Cell(r, 2).SetValue(SafeToString(dtThongTin.Rows[i]["GiaTri"]));
                ws.Cell(r, 1).Style.Font.Bold = true;

                ws.Range(r, 1, r, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(r, 1, r, 2).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                r++;
            }
            ws.Columns(1, 2).AdjustToContents();
            r += 1;

            // 2) LỊCH SỬ HỢP ĐỒNG
            DataTable dtHD = GetLichSuHopDong(maNV);

            ws.Cell(r, 1).SetValue("Lịch sử hợp đồng");
            ws.Cell(r, 1).Style.Font.Bold = true;
            r++;

            // header lịch sử
            string[] hdHeaders = { "Mã HĐ", "Ngày bắt đầu", "Ngày kết thúc", "Loại hợp đồng", "Ghi chú" };
            for (int i = 0; i < hdHeaders.Length; i++)
            {
                var cell = ws.Cell(r, i + 1);
                cell.SetValue(hdHeaders[i]);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
            r++;

            int hdStart = r;
            foreach (DataRow dr in dtHD.Rows)
            {
                ws.Cell(r, 1).SetValue(SafeToString(dr["MaHD"]));
                ws.Cell(r, 2).SetValue(SafeToString(dr["NgayBatDau"]));
                ws.Cell(r, 3).SetValue(SafeToString(dr["NgayKetThuc"]));
                ws.Cell(r, 4).SetValue(SafeToString(dr["LoaiHD"]));
                ws.Cell(r, 5).SetValue(SafeToString(dr["GhiChu"]));

                ws.Range(r, 1, r, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(r, 1, r, 5).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Cell(r, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell(r, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                r++;
            }

            if (dtHD.Rows.Count == 0)
            {
                ws.Range(r, 1, r, 5).Merge().SetValue("Không có dữ liệu hợp đồng.");
                ws.Range(r, 1, r, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(r, 1, r, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                r++;
            }

            r += 1;
        }


        // ===============================
        // FOOTER
        // ===============================
        private void WriteFooter(IXLWorksheet ws, ref int r, int colCount)
        {
            int startCol = Math.Max(1, colCount - 2);

            ws.Range(r, startCol, r, colCount).Merge()
                .SetValue($"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}");
            ws.Range(r, startCol, r, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r++;

            ws.Range(r, startCol, r, colCount).Merge().SetValue("Người lập bảng");
            ws.Range(r, startCol, r, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r++;

            ws.Range(r, startCol, r, colCount).Merge().SetValue("Vũ Minh Khang");
            ws.Range(r, startCol, r, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            r++;
        }


        // ===============================
        // GET CHI TIẾT NHÂN VIÊN (KEY-VALUE) - AN TOÀN
        // (bạn chỉnh lại tên cột theo DB của bạn nếu khác)
        // ===============================
        private DataTable GetNhanVienChiTiet_KeyValue(string maNV)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Ten");
            dt.Columns.Add("GiaTri");

            cn.connect();

            string sql = @"
        SELECT 
            nv.MaNV_TuanhCD233018 AS MaNV,
            nv.HoTen_TuanhCD233018 AS HoTen,
            nv.NgaySinh_TuanhCD233018 AS NgaySinh,
            nv.GioiTinh_TuanhCD233018 AS GioiTinh,
            nv.SoDienThoai_TuanhCD233018 AS SoDienThoai,
            nv.Email_TuanhCD233018 AS Email,
            pb.TenPB_ThuanCD233318 AS PhongBan,
            cv.TenCV_KhangCD233181 AS ChucVu
        FROM tblNhanVien_TuanhCD233018 nv
        JOIN tblChucVu_KhangCD233181 cv 
            ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
        JOIN tblPhongBan_ThuanCD233318 pb 
            ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
        WHERE nv.MaNV_TuanhCD233018 = @MaNV
          AND nv.DeletedAt_TuanhCD233018 = 0";

            using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        dt.Rows.Add("Mã NV", dr["MaNV"]);
                        dt.Rows.Add("Họ tên", dr["HoTen"]);
                        dt.Rows.Add("Ngày sinh", dr["NgaySinh"]);
                        dt.Rows.Add("Giới tính", dr["GioiTinh"]);
                        dt.Rows.Add("Số điện thoại", dr["SoDienThoai"]);
                        dt.Rows.Add("Email", dr["Email"]);
                        dt.Rows.Add("Phòng ban", dr["PhongBan"]);
                        dt.Rows.Add("Chức vụ", dr["ChucVu"]);
                    }
                }
            }

            cn.disconnect();
            return dt;
        }



        // ===============================
        // GET LỊCH SỬ HỢP ĐỒNG - AN TOÀN
        // (tên cột theo code bạn gửi: tblHopDong_ChienCD232928)
        // ===============================
        private DataTable GetLichSuHopDong(string maNV)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHD");
            dt.Columns.Add("NgayBatDau");
            dt.Columns.Add("NgayKetThuc");
            dt.Columns.Add("LoaiHD");
            dt.Columns.Add("GhiChu");

            cn.connect();

            string sql = @"
        SELECT 
            MaHopDong_ChienCD232928 AS MaHD,
            NgayBatDau_ChienCD232928 AS NgayBatDau,
            NgayKetThuc_ChienCD232928 AS NgayKetThuc,
            LoaiHopDong_ChienCD232928 AS LoaiHD,
            Ghichu_ChienCD232928 AS GhiChu
        FROM tblHopDong_ChienCD232928
        WHERE MaNV_TuanhCD233018 = @MaNV
          AND DeletedAt_ChienCD232928 = 0
        ORDER BY NgayBatDau_ChienCD232928 DESC";

            using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
            {
                cmd.Parameters.AddWithValue("@MaNV", maNV);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        dt.Rows.Add(
                            dr["MaHD"],
                            dr["NgayBatDau"],
                            dr["NgayKetThuc"],
                            dr["LoaiHD"],
                            dr["GhiChu"]
                        );
                    }
                }
            }

            cn.disconnect();
            return dt;
        }



        // ===============================
        // TIỆN ÍCH CHỐNG LỖI
        // ===============================
        private string SafeToString(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            // nếu là DateTime -> format dd/MM/yyyy
            if (v is DateTime dt) return dt.ToString("dd/MM/yyyy");
            // nếu là kiểu ngày dạng SQL nhưng ra string
            if (DateTime.TryParse(v.ToString(), out DateTime d2))
                return d2.ToString("dd/MM/yyyy");
            return v.ToString();
        }

        private string SanitizeFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "BaoCao";
            foreach (char c in Path.GetInvalidFileNameChars())
                name = name.Replace(c.ToString(), "");
            name = name.Replace("  ", " ").Trim();
            return string.IsNullOrWhiteSpace(name) ? "BaoCao" : name;
        }

        private void EnsureDirectory(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        // Format cột ngày theo headertext (nếu muốn chuẩn dd/MM/yyyy)
        private void FormatDateColumnIfExists(IXLWorksheet ws, string headerText, int colCount, int headerRow, int lastRow)
        {
            int col = -1;
            for (int c = 1; c <= colCount; c++)
            {
                string h = ws.Cell(headerRow, c).GetString();
                if (string.Equals(h.Trim(), headerText.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    col = c;
                    break;
                }
            }
            if (col == -1) return;

            // áp format dd/MM/yyyy cho vùng dữ liệu (bỏ dòng header)
            var rng = ws.Range(headerRow + 1, col, lastRow, col);
            rng.Style.DateFormat.Format = "dd/MM/yyyy";
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvChiTiet == null || dgvChiTiet.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "PDF file (*.pdf)|*.pdf";
                    sfd.Title = "Lưu báo cáo PDF";
                    sfd.FileName = $"BaoCao_{SanitizeFileName(lblTitle.Text)}_{DateTime.Now:ddMMyyyy}.pdf";

                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    bool xuatMotNV = false;
                    string maNV = null;

                    if (nguoiDungDaChonDong && dgvChiTiet.CurrentRow != null)
                    {
                        maNV = SafeToString(dgvChiTiet.CurrentRow.Cells["MaNV"].Value);
                        if (!string.IsNullOrWhiteSpace(maNV))
                            xuatMotNV = true;
                    }

                    ExportPDF_BaoCaoChiTiet(sfd.FileName, xuatMotNV, maNV);

                    MessageBox.Show("Xuất PDF thành công!", "OK",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xuất PDF thất bại!\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportPDF_BaoCaoChiTiet(string filePath, bool xuatMotNV, string maNV)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            // ===== FONT CHUẨN (KHÔNG XUNG ĐỘT) =====
            BaseFont bf = BaseFont.CreateFont(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf"),
                BaseFont.IDENTITY_H,
                BaseFont.EMBEDDED
            );

            iTextSharp.text.Font fTitle = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fSub = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fHeader = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font fCell = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

            // ===== HEADER =====
            AddCenterText(doc, "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fTitle);
            AddCenterText(doc, "PHÒNG NHÂN SỰ", fSub);
            AddCenterText(doc, lblTitle.Text, fSub);
            AddCenterText(doc, lblContext.Text, fCell);
            AddCenterText(doc, $"Ngày xuất: {DateTime.Now:dd/MM/yyyy}", fCell);
            doc.Add(new Paragraph("\n"));

            int colCount = dgvChiTiet.Columns.Count;
            PdfPTable table = new PdfPTable(colCount);
            table.WidthPercentage = 100;

            // ===== HEADER TABLE =====
            foreach (DataGridViewColumn col in dgvChiTiet.Columns)
                table.AddCell(CreateCell(col.HeaderText, fHeader, BaseColor.LIGHT_GRAY));

            // ===== DATA =====
            if (xuatMotNV && dgvChiTiet.CurrentRow != null)
            {
                AddPdfRow(table, dgvChiTiet.CurrentRow, fCell);
            }
            else
            {
                foreach (DataGridViewRow row in dgvChiTiet.Rows)
                {
                    if (!row.IsNewRow)
                        AddPdfRow(table, row, fCell);
                }
            }

            doc.Add(table);

            // ===== CHI TIẾT NHÂN VIÊN (GIỐNG EXCEL) =====
            if (xuatMotNV && !string.IsNullOrWhiteSpace(maNV))
            {
                doc.NewPage();
                WriteNhanVienChiTietPDF(doc, bf, maNV);
            }

            // ===== FOOTER =====
            doc.Add(new Paragraph("\n\n"));
            Paragraph footer = new Paragraph(
                $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}\nNgười lập bảng\nVũ Minh Khang",
                new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.ITALIC)
            );
            footer.Alignment = Element.ALIGN_RIGHT;
            doc.Add(footer);

            doc.Close();

        }
        private void AddPdfRow(PdfPTable table, DataGridViewRow row, iTextSharp.text.Font font)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                table.AddCell(CreateCell(SafeToString(cell.Value), font));
            }
        }

        private PdfPCell CreateCell(string text, iTextSharp.text.Font font, BaseColor bg = null)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;

            if (bg != null)
                cell.BackgroundColor = bg;

            return cell;
        }

        private void AddCenterText(Document doc, string text, iTextSharp.text.Font font)
        {
            Paragraph p = new Paragraph(text, font);
            p.Alignment = Element.ALIGN_CENTER;
            doc.Add(p);
        }
        private void WriteNhanVienChiTietPDF(Document doc, BaseFont bf, string maNV)
        {
            var fTitle = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD);
            var fBold = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD);
            var fCell = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.NORMAL);

            // ===== TITLE =====
            Paragraph title = new Paragraph("BÁO CÁO CHI TIẾT NHÂN VIÊN", fTitle);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 15;
            doc.Add(title);

            // ===== THÔNG TIN NHÂN VIÊN (KEY - VALUE) =====
            DataTable dtInfo = GetNhanVienChiTiet_KeyValue(maNV);

            PdfPTable tblInfo = new PdfPTable(2);
            tblInfo.WidthPercentage = 70;
            tblInfo.SetWidths(new float[] { 30f, 70f });
            tblInfo.SpacingAfter = 20;

            foreach (DataRow r in dtInfo.Rows)
            {
                tblInfo.AddCell(CreateCell(r["Ten"].ToString(), fBold, BaseColor.LIGHT_GRAY));
                tblInfo.AddCell(CreateCell(SafeToString(r["GiaTri"]), fCell));
            }

            doc.Add(tblInfo);

            // ===== LỊCH SỬ HỢP ĐỒNG =====
            Paragraph hdTitle = new Paragraph("LỊCH SỬ HỢP ĐỒNG", fBold);
            hdTitle.SpacingAfter = 10;
            doc.Add(hdTitle);

            DataTable dtHD = GetLichSuHopDong(maNV);

            PdfPTable tblHD = new PdfPTable(5);
            tblHD.WidthPercentage = 100;
            tblHD.SetWidths(new float[] { 15f, 20f, 20f, 20f, 25f });

            string[] headers = { "Mã HĐ", "Ngày bắt đầu", "Ngày kết thúc", "Loại HĐ", "Ghi chú" };
            foreach (string h in headers)
                tblHD.AddCell(CreateCell(h, fBold, BaseColor.LIGHT_GRAY));

            if (dtHD.Rows.Count > 0)
            {
                foreach (DataRow r in dtHD.Rows)
                {
                    tblHD.AddCell(CreateCell(r["MaHD"].ToString(), fCell));
                    tblHD.AddCell(CreateCell(SafeToString(r["NgayBatDau"]), fCell));
                    tblHD.AddCell(CreateCell(SafeToString(r["NgayKetThuc"]), fCell));
                    tblHD.AddCell(CreateCell(r["LoaiHD"].ToString(), fCell));
                    tblHD.AddCell(CreateCell(r["GhiChu"].ToString(), fCell));
                }
            }
            else
            {
                PdfPCell empty = new PdfPCell(new Phrase("Không có dữ liệu hợp đồng", fCell));
                empty.Colspan = 5;
                empty.HorizontalAlignment = Element.ALIGN_CENTER;
                empty.Padding = 8;
                tblHD.AddCell(empty);
            }

            doc.Add(tblHD);
        }


    }
}
