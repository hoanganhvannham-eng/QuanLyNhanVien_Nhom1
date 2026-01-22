using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_TaiKhoan : Form
    {
        private readonly connectData cn = new connectData();

        public F_TaiKhoan()
        {
            InitializeComponent();
        }

        // ===================== HASH MD5 (đúng kiểu CSDL đang INSERT) =====================
        private string MD5HexUpper(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (byte b in bytes) sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }

        // ===================== UI HELPERS =====================
        private void ClearForm()
        {
            tbmaTK.Text = "";
            tbTenDangNhap.Text = "";
            tbMatKhau.Text = "";
            tbGhiChu.Text = "";
            tbMKkhoiphuc.Text = "";

            cbBoxMaNV.SelectedIndex = -1;
            cbBoxQuyen.SelectedIndex = -1;
        }

        private string GetSelectedQuyen()
        {
            // ưu tiên SelectedItem nếu có
            if (cbBoxQuyen.SelectedItem != null) return cbBoxQuyen.SelectedItem.ToString();
            return cbBoxQuyen.Text?.Trim();
        }

        private int GetRoleIdFromQuyen(string quyen)
        {
            // map theo dữ liệu mẫu bạn insert:
            // Admin = 1, Nhân sự = 2, Nhân viên = 3
            if (string.Equals(quyen, "Admin", StringComparison.OrdinalIgnoreCase)) return 1;
            if (quyen.Contains("Nhân sự")) return 2;
            return 3;
        }

        // ===================== LOAD DATA =====================
        private void LoadComboNhanVien()
        {
            cn.connect();

            string sql = @"
                SELECT MaNV_TuanhCD233018, HoTen_TuanhCD233018
                FROM tblNhanVien_TuanhCD233018
                WHERE DeletedAt_TuanhCD233018 = 0
                ORDER BY MaNV_TuanhCD233018";

            var da = new SqlDataAdapter(sql, cn.conn);
            var dt = new DataTable();
            da.Fill(dt);

            cbBoxMaNV.DataSource = dt;
            cbBoxMaNV.DisplayMember = "HoTen_TuanhCD233018";
            cbBoxMaNV.ValueMember = "MaNV_TuanhCD233018";

            cn.disconnect();
        }

        private void LoadComboQuyen()
        {
            // combo quyền đơn giản theo role mẫu
            cbBoxQuyen.Items.Clear();
            cbBoxQuyen.Items.Add("Admin");
            cbBoxQuyen.Items.Add("Nhân sự");
            cbBoxQuyen.Items.Add("Nhân viên");
            cbBoxQuyen.SelectedIndex = -1;
        }

        private void LoadDataTaiKhoan(bool showDeleted = false)
        {
            try
            {
                cn.connect();

                string sql = @"
                SELECT 
                    tk.MaTK_KhangCD233181        AS [Mã tài khoản],
                    tk.MaNV_TuanhCD233018        AS [Mã NV],
                    nv.HoTen_TuanhCD233018       AS [Tên nhân viên],
                    tk.SoDienThoai_KhangCD233181 AS [SĐT],
                    tk.MatKhau_KhangCD233181     AS [Mật khẩu],
                    tk.Quyen_KhangCD233181       AS [Quyền],
                    tk.Ghichu_KhangCD233181      AS [Ghi chú]
                    FROM tblTaiKhoan_KhangCD233181 tk
                    INNER JOIN tblNhanVien_TuanhCD233018 nv
                        ON tk.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                      AND tk.DeletedAt_KhangCD233181 = @del
                    ORDER BY tk.MaTK_KhangCD233181";

                var da = new SqlDataAdapter(sql, cn.conn);
                da.SelectCommand.Parameters.AddWithValue("@del", showDeleted ? 1 : 0);

                var dt = new DataTable();
                da.Fill(dt);

                dataGridViewTaiKhoan.DataSource = dt;

                cn.disconnect();

                // load combo + set trạng thái UI
                LoadComboNhanVien();
                LoadComboQuyen();
                tbMKkhoiphuc.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load tài khoản: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { cn.disconnect(); } catch { }
            }
        }

        // ===================== FORM LOAD =====================
        private void F_TaiKhoan_Load(object sender, EventArgs e)
        {
            LoadDataTaiKhoan(false);
            ClearForm();
        }

        // ===================== VALIDATE =====================
        private bool ValidateInputBasic(out string err)
        {
            err = "";

            if (string.IsNullOrWhiteSpace(tbmaTK.Text))
            {
                err = "Vui lòng nhập Mã tài khoản.";
                return false;
            }

            if (cbBoxMaNV.SelectedIndex == -1 || cbBoxMaNV.SelectedValue == null)
            {
                err = "Vui lòng chọn Nhân viên.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbTenDangNhap.Text))
            {
                err = "Vui lòng nhập SĐT / Tên đăng nhập.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbMatKhau.Text))
            {
                err = "Vui lòng nhập Mật khẩu.";
                return false;
            }

            string quyen = GetSelectedQuyen();
            if (string.IsNullOrWhiteSpace(quyen))
            {
                err = "Vui lòng chọn Quyền.";
                return false;
            }

            // check sdt 10 số (vì bạn đang dùng SoDienThoai làm đăng nhập)
            if (!long.TryParse(tbTenDangNhap.Text.Trim(), out _))
            {
                err = "SĐT phải là số.";
                return false;
            }
            if (tbTenDangNhap.Text.Trim().Length != 10)
            {
                err = "SĐT phải đúng 10 chữ số.";
                return false;
            }

            // mật khẩu >= 6 hoặc 8 tuỳ bạn, ở đây dùng 6 cho dễ test
            if (tbMatKhau.Text.Trim().Length < 6)
            {
                err = "Mật khẩu tối thiểu 6 ký tự.";
                return false;
            }

            return true;
        }

        // ===================== ADD =====================
        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputBasic(out string err))
                {
                    MessageBox.Show(err, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maTK = tbmaTK.Text.Trim();
                string maNV = cbBoxMaNV.SelectedValue.ToString();
                string sdt = tbTenDangNhap.Text.Trim();
                string mkHash = MD5HexUpper(tbMatKhau.Text.Trim());
                string quyen = GetSelectedQuyen();
                string ghiChu = tbGhiChu.Text.Trim();
                int roleId = GetRoleIdFromQuyen(quyen);

                cn.connect();

                // 1) check MaTK tồn tại (kể cả bị xóa mềm cũng coi là tồn tại để tránh trùng)
                string sqlCheckMaTK = "SELECT COUNT(*) FROM tblTaiKhoan_KhangCD233181 WHERE MaTK_KhangCD233181=@MaTK";
                using (var cmd = new SqlCommand(sqlCheckMaTK, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Mã tài khoản đã tồn tại.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // 2) check SĐT tồn tại (DeletedAt=0)
                string sqlCheckSDT = @"
                    SELECT COUNT(*) 
                    FROM tblTaiKhoan_KhangCD233181 
                    WHERE SoDienThoai_KhangCD233181=@SDT AND DeletedAt_KhangCD233181=0";
                using (var cmd = new SqlCommand(sqlCheckSDT, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("SĐT/Tên đăng nhập đã tồn tại.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // 3) check NV đã có tài khoản (DeletedAt=0)
                string sqlCheckNV = @"
                    SELECT COUNT(*)
                    FROM tblTaiKhoan_KhangCD233181
                    WHERE MaNV_TuanhCD233018=@MaNV AND DeletedAt_KhangCD233181=0";
                using (var cmd = new SqlCommand(sqlCheckNV, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Nhân viên này đã có tài khoản.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // insert
                string sqlInsert = @"
                    INSERT INTO tblTaiKhoan_KhangCD233181
                    (MaTK_KhangCD233181, MaNV_TuanhCD233018,
                     SoDienThoai_KhangCD233181, MatKhau_KhangCD233181,
                     Quyen_KhangCD233181, Ghichu_KhangCD233181,
                     DeletedAt_KhangCD233181, RoleId_ThuanCD233318)
                    VALUES
                    (@MaTK, @MaNV, @SDT, @MK, @Quyen, @GhiChu, 0, @RoleId)";

                using (var cmd = new SqlCommand(sqlInsert, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    cmd.Parameters.AddWithValue("@MK", mkHash);
                    cmd.Parameters.AddWithValue("@Quyen", quyen);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)ghiChu ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RoleId", roleId);

                    cmd.ExecuteNonQuery();
                }

                cn.disconnect();

                MessageBox.Show("Thêm tài khoản thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataTaiKhoan(false);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { cn.disconnect(); } catch { }
            }
        }

        // ===================== UPDATE =====================
        private void btnSua_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputBasic(out string err))
                {
                    MessageBox.Show(err, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maTK = tbmaTK.Text.Trim();
                string maNV = cbBoxMaNV.SelectedValue.ToString();
                string sdt = tbTenDangNhap.Text.Trim();
                string mkHash = MD5HexUpper(tbMatKhau.Text.Trim());
                string quyen = GetSelectedQuyen();
                string ghiChu = tbGhiChu.Text.Trim();
                int roleId = GetRoleIdFromQuyen(quyen);

                cn.connect();

                // check tồn tại MaTK (DeletedAt=0 hoặc 1 đều được sửa? thường chỉ sửa cái đang hoạt động)
                string sqlExist = "SELECT COUNT(*) FROM tblTaiKhoan_KhangCD233181 WHERE MaTK_KhangCD233181=@MaTK";
                using (var cmd = new SqlCommand(sqlExist, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    if ((int)cmd.ExecuteScalar() == 0)
                    {
                        MessageBox.Show("Không tìm thấy mã tài khoản.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // check SĐT trùng với tài khoản khác
                string sqlCheckSDT = @"
                    SELECT COUNT(*)
                    FROM tblTaiKhoan_KhangCD233181
                    WHERE SoDienThoai_KhangCD233181=@SDT
                      AND MaTK_KhangCD233181<>@MaTK
                      AND DeletedAt_KhangCD233181=0";
                using (var cmd = new SqlCommand(sqlCheckSDT, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("SĐT này đã thuộc tài khoản khác.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                DialogResult confirm = MessageBox.Show("Bạn chắc chắn muốn sửa tài khoản này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                {
                    cn.disconnect();
                    return;
                }

                string sqlUpdate = @"
                    UPDATE tblTaiKhoan_KhangCD233181
                    SET MaNV_TuanhCD233018=@MaNV,
                        SoDienThoai_KhangCD233181=@SDT,
                        MatKhau_KhangCD233181=@MK,
                        Quyen_KhangCD233181=@Quyen,
                        Ghichu_KhangCD233181=@GhiChu,
                        RoleId_ThuanCD233318=@RoleId
                    WHERE MaTK_KhangCD233181=@MaTK";

                using (var cmd = new SqlCommand(sqlUpdate, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@SDT", sdt);
                    cmd.Parameters.AddWithValue("@MK", mkHash);
                    cmd.Parameters.AddWithValue("@Quyen", quyen);
                    cmd.Parameters.AddWithValue("@GhiChu", (object)ghiChu ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@RoleId", roleId);

                    cmd.ExecuteNonQuery();
                }

                cn.disconnect();

                MessageBox.Show("Cập nhật thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataTaiKhoan(false);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { cn.disconnect(); } catch { }
            }
        }

        // ===================== DELETE (SOFT) =====================
        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbmaTK.Text))
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần xoá.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maTK = tbmaTK.Text.Trim();

                DialogResult confirm = MessageBox.Show("Bạn chắc chắn muốn xoá (ẩn) tài khoản này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                cn.connect();

                string sql = "UPDATE tblTaiKhoan_KhangCD233181 SET DeletedAt_KhangCD233181=1 WHERE MaTK_KhangCD233181=@MaTK";
                using (var cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    cmd.ExecuteNonQuery();
                }

                cn.disconnect();

                MessageBox.Show("Đã xoá (ẩn) tài khoản.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDataTaiKhoan(false);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xoá: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { cn.disconnect(); } catch { }
            }
        }

        // ===================== SEARCH =====================
        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                string key = tbTenDangNhap.Text.Trim(); // dùng chung làm ô tìm nhanh
                if (string.IsNullOrWhiteSpace(key) && cbBoxMaNV.SelectedIndex == -1 && string.IsNullOrWhiteSpace(tbmaTK.Text))
                {
                    MessageBox.Show("Nhập SĐT / Mã TK hoặc chọn NV để tìm.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maTK = tbmaTK.Text.Trim();
                string maNV = cbBoxMaNV.SelectedValue != null ? cbBoxMaNV.SelectedValue.ToString() : "";

                cn.connect();

                string sql = @"
                SELECT 
                    tk.MaTK_KhangCD233181        AS [Mã tài khoản],
                    tk.MaNV_TuanhCD233018        AS [Mã NV],
                    nv.HoTen_TuanhCD233018       AS [Tên nhân viên],
                    tk.SoDienThoai_KhangCD233181 AS [SĐT],
                    tk.MatKhau_KhangCD233181     AS [Mật khẩu],
                    tk.Quyen_KhangCD233181       AS [Quyền],
                    tk.Ghichu_KhangCD233181      AS [Ghi chú]
                    FROM tblTaiKhoan_KhangCD233181 tk
                    INNER JOIN tblNhanVien_TuanhCD233018 nv
                        ON tk.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                    WHERE tk.DeletedAt_KhangCD233181 = 0
                      AND nv.DeletedAt_TuanhCD233018 = 0
                      AND (
                            (@MaTK <> '' AND tk.MaTK_KhangCD233181 LIKE '%' + @MaTK + '%')
                         OR (@MaNV <> '' AND tk.MaNV_TuanhCD233018 = @MaNV)
                         OR (@Key <> '' AND tk.SoDienThoai_KhangCD233181 LIKE '%' + @Key + '%')
                      )
                    ORDER BY tk.MaTK_KhangCD233181";

                var da = new SqlDataAdapter(sql, cn.conn);
                da.SelectCommand.Parameters.AddWithValue("@MaTK", maTK);
                da.SelectCommand.Parameters.AddWithValue("@MaNV", maNV);
                da.SelectCommand.Parameters.AddWithValue("@Key", key);

                var dt = new DataTable();
                da.Fill(dt);

                dataGridViewTaiKhoan.DataSource = dt;
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { cn.disconnect(); } catch { }
            }
        }

        // ===================== RESET =====================
        private void btnrestar_Click_1(object sender, EventArgs e)
        {
            LoadDataTaiKhoan(false);
            ClearForm();
        }

        // ===================== SHOW DELETED =====================
        private void btnHienThiPhongBanCu_Click_1(object sender, EventArgs e)
        {
            LoadDataTaiKhoan(true);
        }

        // ===================== RESTORE WITH ADMIN PASSWORD =====================
        private void btnKhoiPhucPhongBan_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbmaTK.Text))
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần khôi phục.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbMKkhoiphuc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu Admin để khôi phục.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maTK = tbmaTK.Text.Trim();
                string mkAdminHash = MD5HexUpper(tbMKkhoiphuc.Text.Trim());

                cn.connect();

                // 1) Kiểm tra có admin nào đúng mật khẩu không
                string sqlCheckAdmin = @"
                    SELECT COUNT(*)
                    FROM tblTaiKhoan_KhangCD233181
                    WHERE DeletedAt_KhangCD233181 = 0
                      AND Quyen_KhangCD233181 = N'Admin'
                      AND MatKhau_KhangCD233181 = @MK";
                using (var cmd = new SqlCommand(sqlCheckAdmin, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MK", mkAdminHash);
                    if ((int)cmd.ExecuteScalar() == 0)
                    {
                        MessageBox.Show("Mật khẩu Admin không đúng.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        tbMKkhoiphuc.Clear();
                        return;
                    }
                }

                // 2) Kiểm tra tài khoản đang bị xoá mềm hay không
                string sqlCheckDeleted = @"
                    SELECT COUNT(*)
                    FROM tblTaiKhoan_KhangCD233181
                    WHERE MaTK_KhangCD233181=@MaTK
                      AND DeletedAt_KhangCD233181=1";
                using (var cmd = new SqlCommand(sqlCheckDeleted, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    if ((int)cmd.ExecuteScalar() == 0)
                    {
                        MessageBox.Show("Tài khoản này không nằm trong danh sách đã xoá.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                DialogResult confirm = MessageBox.Show("Bạn chắc chắn muốn khôi phục tài khoản này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                {
                    cn.disconnect();
                    return;
                }

                // 3) Restore
                string sqlRestore = @"
                    UPDATE tblTaiKhoan_KhangCD233181
                    SET DeletedAt_KhangCD233181 = 0
                    WHERE MaTK_KhangCD233181=@MaTK";
                using (var cmd = new SqlCommand(sqlRestore, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", maTK);
                    cmd.ExecuteNonQuery();
                }

                cn.disconnect();

                MessageBox.Show("Khôi phục thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                tbMKkhoiphuc.Clear();
                LoadDataTaiKhoan(false);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khôi phục: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { cn.disconnect(); } catch { }
            }
        }

        // ===================== SHOW/HIDE ADMIN PASSWORD =====================
        private void checkshowpassword_CheckedChanged_1(object sender, EventArgs e)
        {
            tbMKkhoiphuc.UseSystemPasswordChar = !checkshowpassword.Checked;
        }

        // ===================== GRID CLICK =====================
        private void dataGridViewTaiKhoan_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var r = dataGridViewTaiKhoan.Rows[e.RowIndex];
            if (r.Cells.Count < 6) return;

            tbmaTK.Text = r.Cells[0].Value?.ToString() ?? "";
            cbBoxMaNV.SelectedValue = r.Cells[1].Value?.ToString() ?? "";
            tbTenDangNhap.Text = r.Cells[3].Value?.ToString() ?? "";
            tbMatKhau.Text = ""; // không đổ hash
            cbBoxQuyen.Text = r.Cells[5].Value?.ToString() ?? "";
            tbGhiChu.Text = r.Cells[6].Value?.ToString() ?? "";

        }

        // ===================== EXPORT EXCEL =====================
        private void btnxuatExcel_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTaiKhoan.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel Workbook|*.xlsx";
                    sfd.FileName = "DanhSachTaiKhoan_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";

                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("TaiKhoan");

                        int colCount = dataGridViewTaiKhoan.Columns.Count;

                        // title
                        ws.Cell(1, 1).Value = "DANH SÁCH TÀI KHOẢN";
                        ws.Range(1, 1, 1, colCount).Merge();
                        ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
                        ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 16;
                        ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Cell(2, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        ws.Range(2, 1, 2, colCount).Merge();
                        ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                        // header
                        for (int i = 0; i < colCount; i++)
                            ws.Cell(4, i + 1).Value = dataGridViewTaiKhoan.Columns[i].HeaderText;

                        ws.Range(4, 1, 4, colCount).Style.Font.Bold = true;
                        ws.Range(4, 1, 4, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // data
                        int rowExcel = 5;
                        foreach (DataGridViewRow row in dataGridViewTaiKhoan.Rows)
                        {
                            if (row.IsNewRow) continue;

                            for (int c = 0; c < colCount; c++)
                            {
                                ws.Cell(rowExcel, c + 1).Value = row.Cells[c].Value?.ToString() ?? "";
                            }
                            rowExcel++;
                        }

                        ws.Columns().AdjustToContents();
                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
