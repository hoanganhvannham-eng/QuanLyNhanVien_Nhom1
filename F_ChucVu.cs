using ClosedXML.Excel;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using static QuanLyNhanVien3.F_FormMain;

namespace QuanLyNhanVien3
{
    public partial class F_ChucVu : Form
    {
        connectData c = new connectData();
        bool isLoadingChucVu = false;
        bool isEditingChucVu = false;

        string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName;
        public F_ChucVu()
        {
            InitializeComponent();
        }

        private void F_ChucVu_Load(object sender, EventArgs e)
        {
            loadcbbMaPB();
            LoadDataChucVu();
            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnHienThiNVNghiViec.Enabled = false;
                btnKhoiPhucNV.Enabled = false;
            }
        }

        void loadcbbMaPB()
        {
            try
            {
                c.connect();
                string sqlLoadcomboBoxtblPhongBan = "SELECT * FROM tblPhongBan_ThuanCD233318 WHERE DeletedAt_ThuanCD233318 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxtblPhongBan, c.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    cbbMaPB.DataSource = ds.Tables[0];
                    cbbMaPB.DisplayMember = "TenPB_ThuanCD233318";
                    cbbMaPB.ValueMember = "MaPB_ThuanCD233318";
                }
                c.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ma PB: " + ex.Message);
            }
        }

        private void LoadDataChucVu()
        {
            try
            {
                if (cbbMaPB.SelectedValue == null || cbbMaPB.SelectedValue is DataRowView)
                    return;

                c.connect();

                string sql = @"SELECT MaCV_KhangCD233181, TenCV_KhangCD233181, Ghichu_KhangCD233181, MaPB_ThuanCD233318
                       FROM tblChucVu_KhangCD233181
                       WHERE MaPB_ThuanCD233318 = @MaPB AND DeletedAt_KhangCD233181 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaPB", cbbMaPB.SelectedValue);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvHienThiChucVu.DataSource = dt;
                    }
                }

                txtMKKhoiPhuc.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khi tải dữ liệu Chức vụ:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                c.disconnect();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
        }

        private void btnHienThiNVNghiViec_Click(object sender, EventArgs e)
        {
        }

        private void btnKhoiPhucNV_Click(object sender, EventArgs e)
        {
        }

        private void checkshowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkshowpassword.Checked)
            {
                txtMKKhoiPhuc.UseSystemPasswordChar = false;
            }
            else
            {
                txtMKKhoiPhuc.UseSystemPasswordChar = true;
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
        }

        private void dgvHienThiChucVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbbMaPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingChucVu) return;
            if (isEditingChucVu) return;
            if (cbbMaPB.SelectedIndex == -1) return;

            LoadDataChucVu();
        }

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

        private void dgvHienThiChucVu_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            isEditingChucVu = true;
            isLoadingChucVu = true;
            int i = dgvHienThiChucVu.CurrentRow.Index;
            tbMaChuVu.Text = dgvHienThiChucVu.Rows[i].Cells[0].Value?.ToString() ?? "";
            txtTenChucVu.Text = dgvHienThiChucVu.Rows[i].Cells[1].Value?.ToString() ?? "";
            txtGhiChu.Text = dgvHienThiChucVu.Rows[i].Cells[2].Value?.ToString() ?? "";
            isLoadingChucVu = false;
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbMaChuVu.Text) ||
                    string.IsNullOrWhiteSpace(txtTenChucVu.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbbMaPB.SelectedValue == null || cbbMaPB.SelectedValue is DataRowView)
                {
                    MessageBox.Show("Vui lòng chọn phòng ban!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                c.connect();

                string checkSql = "SELECT COUNT(*) FROM tblChucVu_KhangCD233181 WHERE MaCV_KhangCD233181 = @MaCV AND DeletedAt_KhangCD233181 = 0";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, c.conn))
                {
                    checkCmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Mã chức vụ đã tồn tại!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                string insertSql = @"INSERT INTO tblChucVu_KhangCD233181 (MaCV_KhangCD233181, TenCV_KhangCD233181, Ghichu_KhangCD233181, MaPB_ThuanCD233318, DeletedAt_KhangCD233181)
                             VALUES (@MaCV, @TenCV, @GhiChu, @MaPB, 0)";

                using (SqlCommand cmd = new SqlCommand(insertSql, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenCV", txtTenChucVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaPB", cbbMaPB.SelectedValue);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm chức vụ thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataChucVu();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm chức vụ:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.disconnect();
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMaChuVu.Text) ||
                string.IsNullOrWhiteSpace(txtTenChucVu.Text))
            {
                MessageBox.Show("Chưa nhập đủ thông tin", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"UPDATE tblChucVu_KhangCD233181
                   SET TenCV_KhangCD233181 = @TenCV,
                       Ghichu_KhangCD233181 = @Ghichu,
                       DeletedAt_KhangCD233181 = 0,
                       MaPB_ThuanCD233318 = @MaPB
                   WHERE MaCV_KhangCD233181 = @MaCV";

            try
            {
                c.connect();

                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenCV", txtTenChucVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@Ghichu", txtGhiChu.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaPB", cbbMaPB.SelectedValue);

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật chức vụ thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDataChucVu();
                        ClearAllInputs(this);
                        isEditingChucVu = false;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy chức vụ để sửa!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa chức vụ:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.disconnect();
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaChuVu.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Chức Vụ cho chức vụ cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa chức vụ này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    c.connect();
                    string query = "UPDATE tblChucVu_KhangCD233181 SET DeletedAt_KhangCD233181 = 1 WHERE MaCV_KhangCD233181 = @MaCV";
                    using (SqlCommand cmd = new SqlCommand(query, c.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa Chức Vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            c.disconnect();
                            LoadDataChucVu();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Chức Vụ để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            c.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHienThiNVNghiViec_Click_1(object sender, EventArgs e)
        {
            try
            {
                c.connect();
                string query = @"SELECT MaCV_KhangCD233181 as N'Mã Chức Vụ', TenCV_KhangCD233181 as N'Tên Chức Vụ', Ghichu_KhangCD233181 as N'Ghi Chú', MaPB_ThuanCD233318 as N'Mã Phòng Ban' 
                                FROM tblChucVu_KhangCD233181 
                                WHERE DeletedAt_KhangCD233181 = 1 
                                ORDER BY MaCV_KhangCD233181;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, c.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvHienThiChucVu.DataSource = dt;
                }
                c.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnKhoiPhucNV_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaChuVu.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Chức Vụ để tìm Chức vụ cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                c.connect();
                string query = "SELECT COUNT(*) FROM tblChucVu_KhangCD233181 WHERE MaCV_KhangCD233181 = @MaCV AND DeletedAt_KhangCD233181 = 1";
                using (SqlCommand cmdcheckPB = new SqlCommand(query, c.conn))
                {
                    cmdcheckPB.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    int emailCount = (int)cmdcheckPB.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        MessageBox.Show("Không tìm thấy chức vụ đã xóa với mã này!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        c.disconnect();
                        return;
                    }
                }

                if (txtMKKhoiPhuc.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu để khôi phục", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Question);
                    return;
                }

                string sqMKKhoiPhuc = "SELECT * FROM tblTaiKhoan_KhangCD233181 WHERE Quyen_KhangCD233181 = @Quyen AND MatKhau_KhangCD233181 = @MatKhau";
                SqlCommand cmdkhoiphuc = new SqlCommand(sqMKKhoiPhuc, c.conn);
                cmdkhoiphuc.Parameters.AddWithValue("@Quyen", "Admin");
                cmdkhoiphuc.Parameters.AddWithValue("@MatKhau", txtMKKhoiPhuc.Text);
                SqlDataReader reader = cmdkhoiphuc.ExecuteReader();

                if (reader.Read() == false)
                {
                    MessageBox.Show("Mật khẩu không đúng! Vui lòng nhập lại mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    txtMKKhoiPhuc.Text = "";
                    reader.Close();
                    c.disconnect();
                    return;
                }
                reader.Close();

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn khôi phục chức vụ này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    txtMKKhoiPhuc.Text = "";
                    string querytblChucVu = "UPDATE tblChucVu_KhangCD233181 SET DeletedAt_KhangCD233181 = 0 WHERE MaCV_KhangCD233181 = @MaCV";
                    using (SqlCommand cmd = new SqlCommand(querytblChucVu, c.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Khôi phục Chức Vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            c.disconnect();
                            ClearAllInputs(this);
                            LoadDataChucVu();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Chức Vụ để khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            c.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaChuVu.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã Chức Vụ để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                c.connect();
                string MaCVtimkiem = tbMaChuVu.Text.Trim();
                string sql = @"SELECT MaCV_KhangCD233181 as N'Mã Chức Vụ', TenCV_KhangCD233181 as N'Tên Chức Vụ', Ghichu_KhangCD233181 as N'Ghi chú', MaPB_ThuanCD233318 as N'Mã Phòng Ban'
                                FROM tblChucVu_KhangCD233181
                                WHERE DeletedAt_KhangCD233181 = 0 AND MaCV_KhangCD233181 LIKE @MaCV
                                ORDER BY MaCV_KhangCD233181";
                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", "%" + MaCVtimkiem + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvHienThiChucVu.DataSource = dt;
                }
                c.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadDataChucVu();
        }
        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dgvHienThiChucVu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy thông tin phòng ban đang chọn
            string tenPhongBan = cbbMaPB.Text;
            string maPhongBan = cbbMaPB.SelectedValue?.ToString() ?? "";

            // Tạo tên file tự động
            string fileName = $"BaoCaoChucVu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

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
                            var ws = wb.Worksheets.Add("ChucVu");

                            // ===== TÊN CÔNG TY =====
                            ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                            ws.Range(1, 1, 1, 10).Merge();
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Font.FontSize = 14;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== TIÊU ĐỀ CHÍNH =====
                            ws.Cell(2, 1).Value = "DANH SÁCH CHỨC VỤ";
                            ws.Range(2, 1, 2, 10).Merge();
                            ws.Cell(2, 1).Style.Font.Bold = true;
                            ws.Cell(2, 1).Style.Font.FontSize = 16;
                            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== NGÀY XUẤT (THỜI GIAN HIỆN TẠI) =====
                            ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            ws.Cell(3, 1).Style.Font.Italic = true;

                            // ===== THÔNG TIN PHÒNG BAN =====
                            ws.Cell(5, 1).Value = "Phòng Ban";
                            ws.Cell(5, 2).Value = !string.IsNullOrEmpty(tenPhongBan) ? $"{tenPhongBan} ({maPhongBan})" : "Tất cả phòng ban";
                            ws.Cell(5, 1).Style.Font.Bold = true;

                            // ===== HEADER BẢNG DỮ LIỆU =====
                            int startRow = 7;
                            ws.Cell(startRow, 1).Value = "DANH SÁCH CHỨC VỤ";
                            ws.Range(startRow, 1, startRow, dgvHienThiChucVu.Columns.Count).Merge();
                            ws.Cell(startRow, 1).Style.Font.Bold = true;
                            ws.Cell(startRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(startRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                            // ===== TIÊU ĐỀ CỘT =====
                            int headerRow = startRow + 1;
                            for (int i = 0; i < dgvHienThiChucVu.Columns.Count; i++)
                            {
                                ws.Cell(headerRow, i + 1).Value = dgvHienThiChucVu.Columns[i].HeaderText;
                                ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            }

                            // ===== GHI DỮ LIỆU =====
                            int dataStartRow = headerRow + 1;
                            for (int i = 0; i < dgvHienThiChucVu.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvHienThiChucVu.Columns.Count; j++)
                                {
                                    var cellValue = dgvHienThiChucVu.Rows[i].Cells[j].Value;
                                    ws.Cell(dataStartRow + i, j + 1).Value = cellValue?.ToString();
                                    ws.Cell(dataStartRow + i, j + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                }
                            }

                            // ===== BORDER CHO BẢNG DỮ LIỆU =====
                            int lastDataRow = dataStartRow + dgvHienThiChucVu.Rows.Count - 1;
                            var tableRange = ws.Range(startRow, 1, lastDataRow, dgvHienThiChucVu.Columns.Count);
                            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            // ===== PHẦN CHỮ KÝ =====
                            int signatureRow = lastDataRow + 2;
                            ws.Cell(signatureRow, 8).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                            ws.Cell(signatureRow, 8).Style.Font.Italic = true;
                            ws.Cell(signatureRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Range(signatureRow, 8, signatureRow, 10).Merge();

                            ws.Cell(signatureRow + 1, 8).Value = "Người lập báo cáo";
                            ws.Cell(signatureRow + 1, 8).Style.Font.Bold = true;
                            ws.Cell(signatureRow + 1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Range(signatureRow + 1, 8, signatureRow + 1, 10).Merge();

                            // Giả sử biến lưu tên người dùng hiện tại là 'nguoiDangNhap'
                            // Nếu bạn có thông tin đăng nhập, hãy thay thế bằng biến thực tế
                            string nguoiDangNhap = LoginInfo.CurrentUserName ?? "Administrator";

                            ws.Cell(signatureRow + 4, 8).Value = nguoiDangNhap;
                            ws.Cell(signatureRow + 4, 8).Style.Font.Bold = true;
                            ws.Cell(signatureRow + 4, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Range(signatureRow + 4, 8, signatureRow + 4, 10).Merge();

                            // ===== TỰ ĐỘNG ĐIỀU CHỈNH CỘT =====
                            ws.Columns().AdjustToContents();

                            // Đặt chiều rộng tối thiểu cho các cột
                            for (int i = 1; i <= dgvHienThiChucVu.Columns.Count; i++)
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

                        MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Mở file sau khi xuất
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void PDF_Click(object sender, EventArgs e)
        {
            if (dgvHienThiChucVu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Files|*.pdf",
                FileName = $"BaoCaoChucVu_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string filePath = sfd.FileName;

                        // Tạo document PDF
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (var document = new iTextSharp.text.Document())
                            {
                                iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs);
                                document.Open();

                                // Thêm tiêu đề
                                document.Add(new iTextSharp.text.Paragraph("DANH SÁCH CHỨC VỤ",
                                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD)));

                                document.Add(new iTextSharp.text.Paragraph("Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                                document.Add(new iTextSharp.text.Paragraph("\n"));

                                // Tạo bảng
                                var table = new iTextSharp.text.pdf.PdfPTable(dgvHienThiChucVu.Columns.Count);
                                table.WidthPercentage = 100;

                                // Thêm header
                                foreach (DataGridViewColumn column in dgvHienThiChucVu.Columns)
                                {
                                    table.AddCell(new iTextSharp.text.pdf.PdfPCell(
                                        new iTextSharp.text.Phrase(column.HeaderText,
                                        new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD))));
                                }

                                // Thêm dữ liệu
                                foreach (DataGridViewRow row in dgvHienThiChucVu.Rows)
                                {
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        table.AddCell(new iTextSharp.text.pdf.PdfPCell(
                                            new iTextSharp.text.Phrase(cell.Value?.ToString() ?? "",
                                            new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9))));
                                    }
                                }

                                document.Add(table);
                                document.Close();
                            }
                        }

                        MessageBox.Show("Xuất PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất PDF:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}