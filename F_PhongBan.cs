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
using static QuanLyNhanVien3.F_FormMain;

namespace QuanLyNhanVien3
{
    public partial class F_PhongBan: Form
    {
        public F_PhongBan()
        {
            InitializeComponent();
        }

        bool isViewingDeletedRecords = false;
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

        // ==================== SỰ KIỆN LOAD DỮ LIỆU BÌNH THƯỜNG ====================
        private void LoadDataPhongBan()
        {
            try
            {
                cn.connect();
                string sqlLoadDataNhanVien = @"SELECT 
                                            MaPB_ThuanCD233318 AS [Mã Phòng Ban],
                                            TenPB_ThuanCD233318 AS [Tên Phòng Ban],
                                            DiaChi_ThuanCD233318 AS [Địa Chỉ],
                                            SoDienThoai_ThuanCD233318 AS [Số Điện Thoại],
                                            GhiChu_ThuanCD233318 AS [Ghi Chú]
                                        FROM tblPhongBan_ThuanCD233318
                                        WHERE DeletedAt_ThuanCD233318 = 0
                                        ORDER BY MaPB_ThuanCD233318;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewPhongBan.DataSource = dt;
                }

                // ⭐ RESET FLAG = FALSE
                isViewingDeletedRecords = false;

                cn.disconnect();
                ClearAllInputs(this);
                tbMKkhoiphuc.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Phong Ban: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                if (
                    string.IsNullOrWhiteSpace(tbmaPB.Text) ||
                    string.IsNullOrWhiteSpace(tbTenPB.Text) ||
                    string.IsNullOrWhiteSpace(tbDiaChi.Text) ||
                    string.IsNullOrWhiteSpace(tbSoDienThoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double a;
                // cehck sdt
                if (!double.TryParse(tbSoDienThoai.Text.Trim(), out a))
                {
                    MessageBox.Show("Số điện thoại phải là số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return;
                }
                else if (tbSoDienThoai.Text.Trim().Length != 10)
                {
                    MessageBox.Show("Số điện thoại phải có đúng 10 chữ số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return;
                }


                // check ma phong ban
                string checkMaPBSql = "SELECT COUNT(*) FROM tblPhongBan_ThuanCD233318  WHERE MaPB_ThuanCD233318  = @MaPB_ThuanCD233318  AND DeletedAt_ThuanCD233318 = 0";
                using (SqlCommand cmdcheckMaPBSql = new SqlCommand(checkMaPBSql, cn.conn))
                {
                    cmdcheckMaPBSql.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text);
                    int MaPBCount = (int)cmdcheckMaPBSql.ExecuteScalar();

                    if (MaPBCount != 0)
                    {
                        MessageBox.Show("Mã phòng ban da tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                string checkdiachiTenPB = "SELECT COUNT(*) FROM tblPhongBan_ThuanCD233318  WHERE TenPB_ThuanCD233318  = @TenPB_ThuanCD233318 AND DiaChi_ThuanCD233318  = @DiaChi_ThuanCD233318";
                using (SqlCommand cmd = new SqlCommand(checkdiachiTenPB, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenPB_ThuanCD233318", tbTenPB.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiaChi_ThuanCD233318", tbDiaChi.Text.Trim());
                    int MaPBCount = (int)cmd.ExecuteScalar();

                    if (MaPBCount > 0)
                    {
                        MessageBox.Show("phòng ban nay da tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                string sqltblNhanVien = @"INSERT INTO tblPhongBan_ThuanCD233318 
                           (MaPB_ThuanCD233318, TenPB_ThuanCD233318,  DiaChi_ThuanCD233318, SoDienThoai_ThuanCD233318, Ghichu_ThuanCD233318, DeletedAt_ThuanCD233318)
                           VALUES ( @MaPB_ThuanCD233318, @TenPB_ThuanCD233318, @DiaChi_ThuanCD233318, @SoDienThoai_ThuanCD233318, @GhiChu_ThuanCD233318, 0)";

                using (SqlCommand cmd = new SqlCommand(sqltblNhanVien, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenPB_ThuanCD233318", tbTenPB.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiaChi_ThuanCD233318", tbDiaChi.Text.Trim());
                    cmd.Parameters.AddWithValue("@SoDienThoai_ThuanCD233318", tbSoDienThoai.Text.Trim());
                    cmd.Parameters.AddWithValue("@GhiChu_ThuanCD233318", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm phong ban thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.disconnect();
                        ClearAllInputs(this);
                        LoadDataPhongBan();
                    }
                    else
                    {
                        cn.disconnect();
                        MessageBox.Show("Thêm phòng ban thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //MessageBox.Show("Chi tiết lỗi: " + ex.ToString(), "Lỗi hệ thống",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã phòng ban cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa phòng ban này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = "UPDATE tblPhongBan_ThuanCD233318 SET DeletedAt_ThuanCD233318 = 1 WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa phòng ban thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataPhongBan();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phòng ban để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text))
                {
                    MessageBox.Show("Vui lòng chọn phòng ban cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (
                    string.IsNullOrWhiteSpace(tbTenPB.Text) ||
                    string.IsNullOrWhiteSpace(tbDiaChi.Text) ||
                    string.IsNullOrWhiteSpace(tbSoDienThoai.Text))
                //string.IsNullOrWhiteSpace(tbGhiChu.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa phòng ban này không?",
                    "Xác nhận sửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string sql = @"UPDATE tblPhongBan_ThuanCD233318 SET TenPB_ThuanCD233318 = @TenPB_ThuanCD233318, DiaChi_ThuanCD233318 = @DiaChi_ThuanCD233318, SoDienThoai_ThuanCD233318 = @SoDienThoai_ThuanCD233318, GhiChu_ThuanCD233318 = @GhiChu_ThuanCD233318, DeletedAt_ThuanCD233318 = 0 WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318";
                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenPB_ThuanCD233318", tbTenPB.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiaChi_ThuanCD233318", tbDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai_ThuanCD233318", tbSoDienThoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@GhiChu_ThuanCD233318", tbGhiChu.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataPhongBan();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Sửa phòng ban thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cn.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phòng ban để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); // hoặc mã
                    return;
                }
                cn.connect();
                string MaPBtimkiem = tbmaPB.Text.Trim();
                string sql = @" SELECT MaPB_ThuanCD233318, TenPB_ThuanCD233318, DiaChi_ThuanCD233318, SoDienThoai_ThuanCD233318, Ghichu_ThuanCD233318
                                FROM tblPhongBan_ThuanCD233318
                                WHERE DeletedAt_ThuanCD233318 = 0 AND MaPB_ThuanCD233318 LIKE @MaPB_ThuanCD233318
                                ORDER BY MaPB_ThuanCD233318";
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", "%" + MaPBtimkiem + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewPhongBan.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btnrestar_Click_1(object sender, EventArgs e)
        {
            LoadDataPhongBan();
        }

        // ==================== XUẤT EXCEL ====================
        private void btnxuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewPhongBan.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ XÁC ĐỊNH TIÊU ĐỀ DỰA VÀO FLAG
            string reportTitle = isViewingDeletedRecords ? "DANH SÁCH PHÒNG BAN ĐÃ XÓA" : "BÁO CÁO DANH SÁCH PHÒNG BAN";
            string fileName = isViewingDeletedRecords
                ? $"PhongBanDaXoa_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                : $"BaoCaoPhongBan_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName;

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
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add("PhongBan");
                            int colCount = dataGridViewPhongBan.Columns.Count;

                            ws.Style.Font.FontName = "Times New Roman";
                            ws.Style.Font.FontSize = 12;

                            // ===== TÊN CÔNG TY =====
                            ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                            ws.Range(1, 1, 1, colCount).Merge();
                            ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
                            ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 15;
                            ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Row(1).Height = 30;

                            // ===== TIÊU ĐỀ ĐỘNG =====
                            ws.Cell(2, 1).Value = reportTitle;
                            ws.Range(2, 1, 2, colCount).Merge();
                            ws.Range(2, 1, 2, colCount).Style.Font.Bold = true;
                            ws.Range(2, 1, 2, colCount).Style.Font.FontSize = 13;
                            ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Row(2).Height = 25;

                            // ===== NGÀY XUẤT =====
                            ws.Cell(3, 1).Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                            ws.Range(3, 1, 3, colCount).Merge();
                            ws.Range(3, 1, 3, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            ws.Range(3, 1, 3, colCount).Style.Font.Italic = true;
                            ws.Row(3).Height = 20;

                            // ===== HEADER BẢNG =====
                            int headerRow = 5;
                            for (int i = 0; i < colCount; i++)
                            {
                                ws.Cell(headerRow, i + 1).Value = dataGridViewPhongBan.Columns[i].HeaderText;
                            }

                            var headerRange = ws.Range(headerRow, 1, headerRow, colCount);
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            headerRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            ws.Row(headerRow).Height = 25;

                            // ===== DỮ LIỆU =====
                            int dataStartRow = headerRow + 1;
                            for (int i = 0; i < dataGridViewPhongBan.Rows.Count; i++)
                            {
                                for (int j = 0; j < colCount; j++)
                                {
                                    var value = dataGridViewPhongBan.Rows[i].Cells[j].Value;
                                    if (value is DateTime)
                                        ws.Cell(dataStartRow + i, j + 1).Value = ((DateTime)value).ToString("dd/MM/yyyy");
                                    else
                                        ws.Cell(dataStartRow + i, j + 1).Value = value != null ? value.ToString() : "";
                                }
                                ws.Row(dataStartRow + i).Height = 25;
                            }

                            // Kẻ khung & Canh giữa
                            var dataRange = ws.Range(headerRow, 1, dataStartRow + dataGridViewPhongBan.Rows.Count - 1, colCount);
                            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== CHỮ KÝ =====
                            int lastRow = dataStartRow + dataGridViewPhongBan.Rows.Count;
                            int signatureRow = lastRow + 2;
                            int signColStart = colCount > 3 ? colCount - 1 : (colCount > 1 ? colCount - 1 : 1);

                            ws.Cell(signatureRow, signColStart).Value = $"Hà Nội, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
                            ws.Range(signatureRow, signColStart, signatureRow, colCount).Merge();
                            ws.Range(signatureRow, signColStart, signatureRow, colCount).Style.Font.Italic = true;
                            ws.Range(signatureRow, signColStart, signatureRow, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Cell(signatureRow + 1, signColStart).Value = "Người lập báo cáo";
                            ws.Range(signatureRow + 1, signColStart, signatureRow + 1, colCount).Merge();
                            ws.Range(signatureRow + 1, signColStart, signatureRow + 1, colCount).Style.Font.Bold = true;
                            ws.Range(signatureRow + 1, signColStart, signatureRow + 1, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Cell(signatureRow + 4, signColStart).Value = nguoiDangNhap;
                            ws.Range(signatureRow + 4, signColStart, signatureRow + 4, colCount).Merge();
                            ws.Range(signatureRow + 4, signColStart, signatureRow + 4, colCount).Style.Font.Bold = true;
                            ws.Range(signatureRow + 4, signColStart, signatureRow + 4, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // ===== TỰ ĐỘNG GIÃN CỘT =====
                            ws.Columns().AdjustToContents();
                            for (int i = 1; i <= colCount; i++)
                            {
                                ws.Column(i).Width = ws.Column(i).Width + 8;
                            }

                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Xuất Excel phòng ban thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ==================== SỰ KIỆN NÚT HIỂN THỊ ĐÃ XÓA ====================
        private void btnHienThiPhongBanCu_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @" SELECT 
                            MaPB_ThuanCD233318 AS [Mã Phòng Ban],
                            TenPB_ThuanCD233318 AS [Tên Phòng Ban],
                            DiaChi_ThuanCD233318 AS [Địa Chỉ],
                            SoDienThoai_ThuanCD233318 AS [Số Điện Thoại],
                            GhiChu_ThuanCD233318 AS [Ghi Chú]
                          FROM tblPhongBan_ThuanCD233318 
                          WHERE DeletedAt_ThuanCD233318 = 1 
                          ORDER BY MaPB_ThuanCD233318";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewPhongBan.DataSource = dt;
                }

                // ⭐ ĐẶT FLAG = TRUE
                isViewingDeletedRecords = true;

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void btnKhoiPhucPhongBan_Click_1(object sender, EventArgs e)
		{
			try
			{
				// 1. Kiểm tra mã phòng ban đầu vào
				if (string.IsNullOrEmpty(tbmaPB.Text))
				{
					MessageBox.Show("Vui lòng chọn hoặc nhập mã Phòng Ban cần khôi phục!", "Thông báo",
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				// 2. Yêu cầu nhập mật khẩu xác nhận
				if (string.IsNullOrEmpty(tbMKkhoiphuc.Text))
				{
					MessageBox.Show("Vui lòng nhập mật khẩu để xác nhận khôi phục!", "Thông báo",
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				string matKhauNhap = tbMKkhoiphuc.Text.Trim();

				cn.connect();

				// 3. Kiểm tra mật khẩu và quyền Admin (RoleId = 1)
				string sqlCheckPassword = @"
            SELECT COUNT(*) 
            FROM tblTaiKhoan_KhangCD233181 
            WHERE RTRIM(MatKhau_KhangCD233181) = @MatKhau 
              AND RoleId_ThuanCD233318 = 1
              AND DeletedAt_KhangCD233181 = 0";

				bool isValidAdmin = false;

				using (SqlCommand cmdCheck = new SqlCommand(sqlCheckPassword, cn.conn))
				{
					cmdCheck.Parameters.AddWithValue("@MatKhau", matKhauNhap);
					int count = (int)cmdCheck.ExecuteScalar();

					if (count > 0)
					{
						isValidAdmin = true;
					}
				}

				if (!isValidAdmin)
				{
					MessageBox.Show("Mật khẩu không đúng hoặc bạn không có quyền Admin để khôi phục!",
						"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
					cn.disconnect();
					tbMKkhoiphuc.Clear(); // Xóa mật khẩu sai đi
					return;
				}

				// 4. Hộp thoại xác nhận
				DialogResult confirm = MessageBox.Show(
					"Bạn có chắc chắn muốn khôi phục Phòng Ban này không?",
					"Xác nhận khôi phục",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question
				);

				if (confirm == DialogResult.Yes)
				{
					// 5. Kiểm tra xem Phòng Ban có tồn tại trong danh sách đã xóa không (DeletedAt = 1)
					string checkDeletedSql = @"
                SELECT COUNT(*) 
                FROM tblPhongBan_ThuanCD233318 
                WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318 
                  AND DeletedAt_ThuanCD233318 = 1";

					using (SqlCommand cmdCheckDeleted = new SqlCommand(checkDeletedSql, cn.conn))
					{
						cmdCheckDeleted.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
						int deletedCount = (int)cmdCheckDeleted.ExecuteScalar();

						if (deletedCount == 0)
						{
							MessageBox.Show("Mã PB này không tìm thấy trong danh sách đã xóa (hoặc chưa bị xóa)!",
								"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							cn.disconnect();
							return;
						}
					}

					// 6. Thực hiện Khôi phục: Cập nhật DeletedAt = 0
					string query = @"
                UPDATE tblPhongBan_ThuanCD233318 
                SET DeletedAt_ThuanCD233318 = 0 
                WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318 
                  AND DeletedAt_ThuanCD233318 = 1";

					using (SqlCommand cmd = new SqlCommand(query, cn.conn))
					{
						cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
						int rowsAffected = cmd.ExecuteNonQuery();

						if (rowsAffected > 0)
						{
							MessageBox.Show("Khôi phục Phòng Ban thành công!", "Thông báo",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
							cn.disconnect();

							LoadDataPhongBan(); // Load lại danh sách phòng ban
							ClearAllInputs(this); // Xóa trắng các ô nhập
							tbMKkhoiphuc.Clear();
						}
						else
						{
							MessageBox.Show("Không thể khôi phục Phòng Ban!", "Thông báo",
								MessageBoxButtons.OK, MessageBoxIcon.Warning);
							cn.disconnect();
						}
					}
				}
				else
				{
					cn.disconnect(); // Ngắt kết nối nếu chọn No
				}
			}
			catch (Exception ex)
			{
				// Đảm bảo ngắt kết nối nếu có lỗi bất ngờ
				try { cn.disconnect(); } catch { }
				MessageBox.Show("Lỗi khi khôi phục phòng ban: " + ex.Message, "Lỗi",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void checkshowpassword_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkshowpassword.Checked)
            {
                tbMKkhoiphuc.UseSystemPasswordChar = false;
            }
            else
            {
                tbMKkhoiphuc.UseSystemPasswordChar = true;
            }
        }

        private void dataGridViewPhongBan_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbmaPB.Text = dataGridViewPhongBan.Rows[i].Cells[0].Value.ToString();
                tbTenPB.Text = dataGridViewPhongBan.Rows[i].Cells[1].Value.ToString();
                tbDiaChi.Text = dataGridViewPhongBan.Rows[i].Cells[2].Value.ToString();
                tbSoDienThoai.Text = dataGridViewPhongBan.Rows[i].Cells[3].Value.ToString();
                tbGhiChu.Text = dataGridViewPhongBan.Rows[i].Cells[4].Value.ToString();
            }
        }

        private void F_PhongBan_Load(object sender, EventArgs e)
        {
            LoadDataPhongBan();

            // Phân quyền dựa trên RoleId
            if (F_FormMain.LoginInfo.CurrentRoleId == 1) // Admin
            {
                // Admin có full quyền
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnHienThiPhongBanCu.Enabled = true;
                buttonlamsach.Enabled = true;
                btnKhoiPhucPhongBan.Enabled = true;

                // Hiển thị controls liên quan đến khôi phục
                // Giả sử bạn có textbox tên txtMKKhoiPhuc và checkbox tên checkshowpassword
                // txtMKKhoiPhuc.Visible = true;
                // checkshowpassword.Visible = true;
            }
            else if (F_FormMain.LoginInfo.CurrentRoleId == 2) // Manager
            {
                // Manager có một số quyền
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnHienThiPhongBanCu.Enabled = true;
                btnKhoiPhucPhongBan.Enabled = false; // Manager không được khôi phục
                buttonlamsach.Enabled = false;

                // txtMKKhoiPhuc.Visible = false;
                // checkshowpassword.Visible = false;
            }
            else // User hoặc role khác
            {
                // User không có quyền gì
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnHienThiPhongBanCu.Enabled = false;
                buttonlamsach.Enabled = false;
                btnKhoiPhucPhongBan.Enabled = false;

                // txtMKKhoiPhuc.Visible = false;
                // checkshowpassword.Visible = false;
            }

             tbMKkhoiphuc.UseSystemPasswordChar = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbGhiChu_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbSoDienThoai_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tbDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbTenPB_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbmaPB_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonlamsach_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn mã phòng ban cần xóa hẳng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yêu cầu nhập mật khẩu xác nhận
                if (string.IsNullOrEmpty(tbMKkhoiphuc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu Admin để xác nhận xóa hẳn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string matKhauNhap = tbMKkhoiphuc.Text.Trim();
                cn.connect();

                // KIỂM TRA MẬT KHẨU ADMIN
                string sqlCheckPassword = @"
            SELECT COUNT(*) 
            FROM tblTaiKhoan_KhangCD233181 
            WHERE RTRIM(MatKhau_KhangCD233181) = @MatKhau 
            AND RoleId_ThuanCD233318 = 1 
            AND DeletedAt_KhangCD233181 = 0";

                bool isValidAdmin = false;
                using (SqlCommand cmdCheck = new SqlCommand(sqlCheckPassword, cn.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MatKhau", matKhauNhap);
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        isValidAdmin = true;
                    }
                }

                if (!isValidAdmin)
                {
                    MessageBox.Show("Mật khẩu không đúng hoặc bạn không có quyền Admin!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cn.disconnect();
                    tbMKkhoiphuc.Clear();
                    return;
                }

                // KIỂM TRA XEM PHÒNG BAN CÓ TỒN TẠI TRONG DANH SÁCH ĐÃ XÓA KHÔNG
                string checkDeletedSql = @"
            SELECT COUNT(*) 
            FROM tblPhongBan_ThuanCD233318 
            WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318 
            AND DeletedAt_ThuanCD233318 = 1";

                using (SqlCommand cmdCheckDeleted = new SqlCommand(checkDeletedSql, cn.conn))
                {
                    cmdCheckDeleted.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                    int deletedCount = (int)cmdCheckDeleted.ExecuteScalar();

                    if (deletedCount == 0)
                    {
                        MessageBox.Show("Phòng ban này không tồn tại trong danh sách đã xóa!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // KIỂM TRA XEM CÓ CHỨC VỤ NÀO THUỘC PHÒNG BAN NÀY KHÔNG
                string checkChucVuSql = @"
            SELECT COUNT(*) 
            FROM tblChucVu_KhangCD233181 
            WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318";

                using (SqlCommand cmdCheckCV = new SqlCommand(checkChucVuSql, cn.conn))
                {
                    cmdCheckCV.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                    int cvCount = (int)cmdCheckCV.ExecuteScalar();

                    if (cvCount > 0)
                    {
                        MessageBox.Show(
                            $"Không thể xóa hẳn! Vẫn còn {cvCount} chức vụ thuộc phòng ban này.\n\n" +
                            "Vui lòng xóa hoặc chuyển các chức vụ sang phòng ban khác trước khi xóa hẳn phòng ban.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        cn.disconnect();
                        return;
                    }
                }

                // XÁC NHẬN XÓA HẲNG
                DialogResult confirm = MessageBox.Show(
                    "⚠️ CẢNH BÁO: Bạn có chắc chắn muốn XÓA HẲNG phòng ban này?\n\n" +
                    "Thao tác này sẽ xóa VĨNH VIỄN dữ liệu phòng ban khỏi hệ thống.\n" +
                    "Dữ liệu sẽ KHÔNG THỂ KHÔI PHỤC!\n\n" +
                    $"Mã phòng ban: {tbmaPB.Text}\n" +
                    $"Tên phòng ban: {tbTenPB.Text}",
                    "⚠️ XÁC NHẬN XÓA HẲNG",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm != DialogResult.Yes)
                {
                    cn.disconnect();
                    return;
                }

                // XÓA HẲNG PHÒNG BAN
                string deleteQuery = @"
            DELETE FROM tblPhongBan_ThuanCD233318 
            WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318 
            AND DeletedAt_ThuanCD233318 = 1";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đã xóa hẳn phòng ban thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        cn.disconnect();

                        // Clear inputs và reset
                        ClearAllInputs(this);
                        tbMKkhoiphuc.Clear();

                        // Load lại dữ liệu (hiển thị phòng ban đang hoạt động)
                        LoadDataPhongBan();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa hẳn phòng ban!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                try { cn.disconnect(); } catch { }
                MessageBox.Show("Lỗi khi xóa hẳn phòng ban: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonxuatpdf_Click(object sender, EventArgs e)
        {
            if (dataGridViewPhongBan.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ XÁC ĐỊNH TIÊU ĐỀ DỰA VÀO FLAG
            string reportTitle = isViewingDeletedRecords ? "DANH SÁCH PHÒNG BAN ĐÃ XÓA" : "BÁO CÁO DANH SÁCH PHÒNG BAN";
            string fileName = isViewingDeletedRecords
                ? $"PhongBanDaXoa_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                : $"BaoCaoPhongBan_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Files|*.pdf",
                FileName = fileName
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = sfd.FileName;

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 25, 25);
                            iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs);
                            document.Open();

                            // Font
                            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.ttf");
                            iTextSharp.text.pdf.BaseFont baseFont = iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);

                            var fontTitle = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);
                            var fontHeader = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                            var fontNormal = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.NORMAL);
                            var fontTableHeader = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
                            var fontTableContent = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.NORMAL);
                            var fontItalic = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.ITALIC);

                            // ===== TÊN CÔNG TY =====
                            var companyPara = new iTextSharp.text.Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontTitle);
                            companyPara.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            companyPara.SpacingAfter = 10;
                            document.Add(companyPara);

                            // ===== TIÊU ĐỀ ĐỘNG =====
                            var titlePara = new iTextSharp.text.Paragraph(reportTitle, fontHeader);
                            titlePara.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            titlePara.SpacingAfter = 10;
                            document.Add(titlePara);

                            // ===== NGÀY LẬP BÁO CÁO =====
                            var datePara = new iTextSharp.text.Paragraph("Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), fontItalic);
                            datePara.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                            datePara.SpacingAfter = 5;
                            document.Add(datePara);

                            // Thêm khoảng trắng
                            document.Add(new iTextSharp.text.Paragraph("\n"));

                            // ===== TIÊU ĐỀ BẢNG =====
                            var tableTitle = new iTextSharp.text.Paragraph(reportTitle, fontTableHeader);
                            tableTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            tableTitle.SpacingAfter = 5;
                            document.Add(tableTitle);

                            // ===== TẠO BẢNG =====
                            var table = new iTextSharp.text.pdf.PdfPTable(dataGridViewPhongBan.Columns.Count);
                            table.WidthPercentage = 100;

                            // Đặt độ rộng cột
                            float[] columnWidths = new float[dataGridViewPhongBan.Columns.Count];
                            for (int i = 0; i < dataGridViewPhongBan.Columns.Count; i++)
                            {
                                columnWidths[i] = 2f; // Độ rộng đều nhau
                            }
                            table.SetWidths(columnWidths);

                            // Header
                            foreach (DataGridViewColumn column in dataGridViewPhongBan.Columns)
                            {
                                var cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(column.HeaderText, fontTableHeader));
                                cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                                cell.Padding = 8;
                                table.AddCell(cell);
                            }

                            // Data
                            foreach (DataGridViewRow row in dataGridViewPhongBan.Rows)
                            {
                                foreach (DataGridViewCell dgvCell in row.Cells)
                                {
                                    var cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(dgvCell.Value?.ToString() ?? "", fontTableContent));
                                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                                    cell.Padding = 5;
                                    table.AddCell(cell);
                                }
                            }

                            table.SpacingAfter = 20;
                            document.Add(table);

                            // ===== CHỮ KÝ =====
                            var signatureTable = new iTextSharp.text.pdf.PdfPTable(2);
                            signatureTable.WidthPercentage = 100;
                            signatureTable.SetWidths(new float[] { 1f, 1f });

                            var emptyCell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(""));
                            emptyCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            signatureTable.AddCell(emptyCell);

                            var signatureCell = new iTextSharp.text.pdf.PdfPCell();
                            signatureCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            signatureCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

                            var dateLine = new iTextSharp.text.Paragraph(
                                $"Hà Nội, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}",
                                fontItalic
                            );
                            dateLine.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            signatureCell.AddElement(dateLine);

                            var titleLine = new iTextSharp.text.Paragraph("Người lập báo cáo", fontTableHeader);
                            titleLine.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            titleLine.SpacingBefore = 5;
                            signatureCell.AddElement(titleLine);

                            var spaceLine = new iTextSharp.text.Paragraph("\n\n\n");
                            signatureCell.AddElement(spaceLine);

                            string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName ?? "Administrator";
                            var nameLine = new iTextSharp.text.Paragraph(nguoiDangNhap, fontTableHeader);
                            nameLine.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            signatureCell.AddElement(nameLine);

                            signatureTable.AddCell(signatureCell);
                            document.Add(signatureTable);

                            document.Close();
                        }

                        MessageBox.Show("Xuất PDF thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất PDF:\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
