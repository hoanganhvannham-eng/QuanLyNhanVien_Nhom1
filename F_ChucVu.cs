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

        bool isViewingDeletedRecords = false;
        private void F_ChucVu_Load(object sender, EventArgs e)
        {
            loadcbbMaPB();
            LoadDataChucVu();
            tbMaChuVu.ReadOnly = true;
            tbMaChuVu.Text = TaoMaChucVuTuDong();

            // Phân quyền dựa trên RoleId
            if (LoginInfo.CurrentRoleId == 1) // Admin
            {
                // Admin có full quyền
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                buttonlamsach.Enabled = true;
                btnHienThiNVNghiViec.Enabled = true;
                btnKhoiPhucNV.Enabled = true;

                // Hiển thị controls liên quan đến khôi phục
                txtMKKhoiPhuc.Visible = true;  // Thay đổi từ false -> true
                checkshowpassword.Visible = true;  // Thay đổi từ false -> true
            }
            else if (LoginInfo.CurrentRoleId == 2) // Manager
            {
                // Manager có một số quyền
                btnThem.Enabled = true;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnHienThiNVNghiViec.Enabled = true;
                btnKhoiPhucNV.Enabled = false; // Manager không được khôi phục
                buttonlamsach.Enabled = false;
                txtMKKhoiPhuc.Enabled = false;
                checkshowpassword.Enabled = false;
            }
            else // User hoặc role khác
            {
                // User không có quyền gì
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnHienThiNVNghiViec.Enabled = false;
                btnKhoiPhucNV.Enabled = false;
                buttonlamsach.Enabled = false;

                txtMKKhoiPhuc.Enabled = false;
                checkshowpassword.Enabled = false;
            }

            txtMKKhoiPhuc.UseSystemPasswordChar = true;
        }

        private string TaoMaChucVuTuDong()
        {
            string maCV = "CV001";
            try
            {
                c.connect();
                string sql = @"
                    SELECT MAX(CAST(SUBSTRING(MaCV_KhangCD233181, 3, LEN(MaCV_KhangCD233181)) AS INT))
                    FROM tblChucVu_KhangCD233181";

                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        int so = Convert.ToInt32(result) + 1;
                        maCV = "CV" + so.ToString("D3");
                    }
                }
            }
            catch { }
            finally
            {
                c.disconnect();
            }
            return maCV;
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

                    // Thêm dòng "Tất cả" vào đầu
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    row["MaPB_ThuanCD233318"] = DBNull.Value;
                    row["TenPB_ThuanCD233318"] = "-- Tất cả phòng ban --";
                    dt.Rows.InsertAt(row, 0);

                    cbbMaPB.DataSource = dt;
                    cbbMaPB.DisplayMember = "TenPB_ThuanCD233318";
                    cbbMaPB.ValueMember = "MaPB_ThuanCD233318";

                    // Mặc định chọn "Tất cả"
                    cbbMaPB.SelectedIndex = 0;
                }
                c.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load mã PB: " + ex.Message);
            }
        }


        // ==================== LOAD DỮ LIỆU BÌNH THƯỜNG ====================
        private void LoadDataChucVu()
        {
            try
            {
                if (isLoadingChucVu) return;

                c.connect();
                string sql = "";
                SqlCommand cmd;

                if (cbbMaPB.SelectedValue == null || cbbMaPB.SelectedValue == DBNull.Value || cbbMaPB.SelectedValue is DataRowView)
                {
                    sql = @"SELECT tblChucVu_KhangCD233181.MaCV_KhangCD233181 AS [Mã chức vụ], 
                          tblChucVu_KhangCD233181.TenCV_KhangCD233181 AS [Tên chức vụ], 
                          tblChucVu_KhangCD233181.Ghichu_KhangCD233181 AS [Ghi chú], 
                          tblChucVu_KhangCD233181.MaPB_ThuanCD233318 AS [Mã phòng ban], 
                          tblPhongBan_ThuanCD233318.TenPB_ThuanCD233318 AS [Tên phòng ban]
                    FROM tblChucVu_KhangCD233181 
                    INNER JOIN tblPhongBan_ThuanCD233318 ON tblChucVu_KhangCD233181.MaPB_ThuanCD233318 = tblPhongBan_ThuanCD233318.MaPB_ThuanCD233318
                    WHERE (tblChucVu_KhangCD233181.DeletedAt_KhangCD233181 = 0)
                    ORDER BY [Mã chức vụ]";
                    cmd = new SqlCommand(sql, c.conn);
                }
                else
                {
                    sql = @"SELECT tblChucVu_KhangCD233181.MaCV_KhangCD233181 AS [Mã chức vụ], 
                          tblChucVu_KhangCD233181.TenCV_KhangCD233181 AS [Tên chức vụ], 
                          tblChucVu_KhangCD233181.Ghichu_KhangCD233181 AS [Ghi chú], 
                          tblChucVu_KhangCD233181.MaPB_ThuanCD233318 AS [Mã phòng ban], 
                          tblPhongBan_ThuanCD233318.TenPB_ThuanCD233318 AS [Tên phòng ban]
                    FROM tblChucVu_KhangCD233181 
                    INNER JOIN tblPhongBan_ThuanCD233318 ON tblChucVu_KhangCD233181.MaPB_ThuanCD233318 = tblPhongBan_ThuanCD233318.MaPB_ThuanCD233318
                    WHERE (tblChucVu_KhangCD233181.MaPB_ThuanCD233318 = @MaPB) AND (tblChucVu_KhangCD233181.DeletedAt_KhangCD233181 = 0)
                    ORDER BY [Mã chức vụ]";
                    cmd = new SqlCommand(sql, c.conn);
                    cmd.Parameters.AddWithValue("@MaPB", cbbMaPB.SelectedValue);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns.Add("STT", typeof(int));
                    dt.Columns["STT"].SetOrdinal(0);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["STT"] = i + 1;
                    }

                    dgvHienThiChucVu.DataSource = dt;

                    if (dgvHienThiChucVu.Columns["STT"] != null)
                    {
                        dgvHienThiChucVu.Columns["STT"].HeaderText = "STT";
                        dgvHienThiChucVu.Columns["STT"].Width = 50;
                        dgvHienThiChucVu.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvHienThiChucVu.Columns["STT"].ReadOnly = true;
                    }
                }

                // ⭐ RESET FLAG = FALSE
                isViewingDeletedRecords = false;

                txtMKKhoiPhuc.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Chức vụ:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.disconnect();
            }
        }
        private void checkshowpassword_CheckedChanged(object sender, EventArgs e)
        {
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

            // Không reset mã chức vụ khi đang ở chế độ xem chức vụ đã xóa
            if (dgvHienThiChucVu.Columns.Count > 0 &&
                dgvHienThiChucVu.Columns[0].HeaderText != "Mã Chức Vụ")
            {
                // Đang ở chế độ xem đã xóa, không làm gì
            }
            else
            {
                tbMaChuVu.Text = TaoMaChucVuTuDong();
            }
        }

        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox)
                {
                    TextBox tb = (TextBox)ctl;
                    if (tb.Name != "tbMaChuVu") // Không xóa mã chức vụ
                        tb.Clear();
                }
                else if (ctl is ComboBox)
                {
                    // Không reset ComboBox phòng ban
                }
                else if (ctl is DateTimePicker)
                    ((DateTimePicker)ctl).Value = DateTime.Now;
                else if (ctl.HasChildren)
                    ClearAllInputs(ctl);
            }
        }

        private void dgvHienThiChucVu_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
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

                if (cbbMaPB.SelectedValue == null || cbbMaPB.SelectedValue == DBNull.Value || cbbMaPB.SelectedValue is DataRowView)
                {
                    MessageBox.Show("Vui lòng chọn phòng ban cụ thể (không phải 'Tất cả')!",
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

                // Clear inputs và reset
                ClearAllInputs(this);
                tbMaChuVu.Text = TaoMaChucVuTuDong();
                isEditingChucVu = false;

                // Load lại dữ liệu
                LoadDataChucVu();
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
                MessageBox.Show("Vui lòng chọn chức vụ cần sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbbMaPB.SelectedValue == null || cbbMaPB.SelectedValue == DBNull.Value || cbbMaPB.SelectedValue is DataRowView)
            {
                MessageBox.Show("Vui lòng chọn phòng ban cụ thể!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sql = @"
                UPDATE tblChucVu_KhangCD233181
                SET TenCV_KhangCD233181 = @TenCV,
                    Ghichu_KhangCD233181 = @Ghichu,
                    MaPB_ThuanCD233318 = @MaPB
                WHERE MaCV_KhangCD233181 = @MaCV
                  AND DeletedAt_KhangCD233181 = 0";

            try
            {
                c.connect();

                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenCV", txtTenChucVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@Ghichu", txtGhiChu.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaPB", cbbMaPB.SelectedValue.ToString());

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật chức vụ thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset form
                        ClearAllInputs(this);
                        tbMaChuVu.Text = TaoMaChucVuTuDong();
                        tbMaChuVu.ReadOnly = true;
                        isEditingChucVu = false;

                        // Load lại dữ liệu
                        LoadDataChucVu();
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

                            // Clear inputs và reset
                            ClearAllInputs(this);
                            tbMaChuVu.Text = TaoMaChucVuTuDong();
                            isEditingChucVu = false;

                            // Load lại dữ liệu
                            LoadDataChucVu();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Chức Vụ để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    c.disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== SỰ KIỆN NÚT HIỂN THỊ ĐÃ XÓA ====================
        private void btnHienThiNVNghiViec_Click_1(object sender, EventArgs e)
        {
            try
            {
                c.connect();
                string query = @"SELECT MaCV_KhangCD233181 as N'Mã Chức Vụ', 
                       TenCV_KhangCD233181 as N'Tên Chức Vụ', 
                       Ghichu_KhangCD233181 as N'Ghi Chú', 
                       MaPB_ThuanCD233318 as N'Mã Phòng Ban' 
                FROM tblChucVu_KhangCD233181 
                WHERE DeletedAt_KhangCD233181 = 1 
                ORDER BY MaCV_KhangCD233181;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, c.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Thêm cột STT
                    dt.Columns.Add("STT", typeof(int));
                    dt.Columns["STT"].SetOrdinal(0);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["STT"] = i + 1;
                    }

                    dgvHienThiChucVu.DataSource = dt;

                    if (dgvHienThiChucVu.Columns["STT"] != null)
                    {
                        dgvHienThiChucVu.Columns["STT"].HeaderText = "STT";
                        dgvHienThiChucVu.Columns["STT"].Width = 50;
                        dgvHienThiChucVu.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                // ⭐ ĐẶT FLAG = TRUE
                isViewingDeletedRecords = true;

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
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Chức Vụ để tìm Chức vụ cần khôi phục!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yêu cầu nhập mật khẩu xác nhận
                if (string.IsNullOrEmpty(txtMKKhoiPhuc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu để xác nhận khôi phục!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string matKhauNhap = txtMKKhoiPhuc.Text.Trim();

                // Kiểm tra mật khẩu với RoleId = 1 (Admin)
                c.connect();

                string sqlCheckPassword = @"
            SELECT COUNT(*) 
            FROM tblTaiKhoan_KhangCD233181 
            WHERE MatKhau_KhangCD233181 = @MatKhau 
            AND RoleId_ThuanCD233318 = 1 
            AND DeletedAt_KhangCD233181 = 0";

                bool isValidAdmin = false;

                using (SqlCommand cmdCheck = new SqlCommand(sqlCheckPassword, c.conn))
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
                    c.disconnect();
                    txtMKKhoiPhuc.Clear();
                    return;
                }

                // Kiểm tra xem chức vụ có tồn tại trong danh sách đã xóa không
                string checkDeletedSql = @"
            SELECT COUNT(*) 
            FROM tblChucVu_KhangCD233181 
            WHERE MaCV_KhangCD233181 = @MaCV 
            AND DeletedAt_KhangCD233181 = 1";

                using (SqlCommand cmdCheckDeleted = new SqlCommand(checkDeletedSql, c.conn))
                {
                    cmdCheckDeleted.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    int deletedCount = (int)cmdCheckDeleted.ExecuteScalar();

                    if (deletedCount == 0)
                    {
                        MessageBox.Show("Chức vụ này không tồn tại trong danh sách đã xóa!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        c.disconnect();
                        return;
                    }
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn khôi phục Chức Vụ này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    // Khôi phục: Cập nhật DeletedAt = 0
                    string query = @"
                UPDATE tblChucVu_KhangCD233181 
                SET DeletedAt_KhangCD233181 = 0 
                WHERE MaCV_KhangCD233181 = @MaCV 
                AND DeletedAt_KhangCD233181 = 1";

                    using (SqlCommand cmd = new SqlCommand(query, c.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Khôi phục Chức Vụ thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            c.disconnect();

                            // Clear inputs và reset
                            ClearAllInputs(this);
                            tbMaChuVu.Text = TaoMaChucVuTuDong();
                            txtMKKhoiPhuc.Clear();
                            isEditingChucVu = false;

                            // Load lại dữ liệu (hiển thị chức vụ đang hoạt động)
                            LoadDataChucVu();
                        }
                        else
                        {
                            MessageBox.Show("Không thể khôi phục Chức Vụ!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            c.disconnect();
                        }
                    }
                }
                else
                {
                    c.disconnect();
                }
            }
            catch (Exception ex)
            {
                try { c.disconnect(); } catch { }
                MessageBox.Show("Lỗi khi khôi phục chức vụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string sql = @"SELECT MaCV_KhangCD233181 as N'Mã Chức Vụ', 
                             TenCV_KhangCD233181 as N'Tên Chức Vụ', 
                             Ghichu_KhangCD233181 as N'Ghi chú', 
                             MaPB_ThuanCD233318 as N'Mã Phòng Ban'
                      FROM tblChucVu_KhangCD233181
                      WHERE DeletedAt_KhangCD233181 = 0 AND MaCV_KhangCD233181 LIKE @MaCV
                      ORDER BY MaCV_KhangCD233181";
                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", "%" + MaCVtimkiem + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Thêm cột STT
                    dt.Columns.Add("STT", typeof(int));
                    dt.Columns["STT"].SetOrdinal(0);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["STT"] = i + 1;
                    }

                    dgvHienThiChucVu.DataSource = dt;

                    // Tùy chỉnh cột STT
                    if (dgvHienThiChucVu.Columns["STT"] != null)
                    {
                        dgvHienThiChucVu.Columns["STT"].HeaderText = "STT";
                        dgvHienThiChucVu.Columns["STT"].Width = 50;
                        dgvHienThiChucVu.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
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
            // Clear inputs
            ClearAllInputs(this);
            isLoadingChucVu = true;
            cbbMaPB.SelectedIndex = 0;
            isLoadingChucVu = false;
            isEditingChucVu = false;

            LoadDataChucVu(); // Tự động reset flag trong này
            tbMaChuVu.Text = TaoMaChucVuTuDong();
        }
        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dgvHienThiChucVu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ XÁC ĐỊNH TIÊU ĐỀ DỰA VÀO FLAG
            string reportTitle = isViewingDeletedRecords ? "DANH SÁCH CHỨC VỤ ĐÃ XÓA" : "DANH SÁCH CHỨC VỤ";
            string fileName = isViewingDeletedRecords
                ? $"ChucVuDaXoa_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
                : $"BaoCaoChucVu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            // Lấy thông tin phòng ban đang chọn
            string tenPhongBan = cbbMaPB.Text;
            string maPhongBan = cbbMaPB.SelectedValue?.ToString() ?? "";

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
                            ws.Range(1, 1, 1, dgvHienThiChucVu.Columns.Count).Merge();
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Font.FontSize = 14;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== TIÊU ĐỀ ĐỘNG =====
                            ws.Cell(2, 1).Value = reportTitle;
                            ws.Range(2, 1, 2, dgvHienThiChucVu.Columns.Count).Merge();
                            ws.Cell(2, 1).Style.Font.Bold = true;
                            ws.Cell(2, 1).Style.Font.FontSize = 16;
                            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== NGÀY LẬP BÁO CÁO =====
                            ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            ws.Cell(3, 1).Style.Font.Italic = true;

                            int startRow;

                            // ⭐ CHỈ HIỂN THỊ THÔNG TIN LỌC KHI KHÔNG PHẢI DANH SÁCH ĐÃ XÓA
                            if (!isViewingDeletedRecords)
                            {
                                // ===== THÔNG TIN PHÒNG BAN =====
                                ws.Cell(5, 1).Value = "Phòng Ban:";
                                ws.Cell(5, 2).Value = !string.IsNullOrEmpty(tenPhongBan) ? $"{tenPhongBan} ({maPhongBan})" : "Tất cả phòng ban";
                                ws.Cell(5, 1).Style.Font.Bold = true;

                                startRow = 7;
                            }
                            else
                            {
                                startRow = 5;
                            }

                            // ===== HEADER BẢNG DỮ LIỆU =====
                            ws.Cell(startRow, 1).Value = reportTitle;
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

                            // ===== TỰ ĐỘNG ĐIỀU CHỈNH CỘT =====
                            ws.Columns().AdjustToContents();

                            if (dgvHienThiChucVu.Columns.Count > 0 && dgvHienThiChucVu.Columns[0].Name == "STT")
                            {
                                ws.Column(1).Width = 12;
                            }

                            for (int i = 2; i <= dgvHienThiChucVu.Columns.Count; i++)
                            {
                                if (ws.Column(i).Width < 12)
                                    ws.Column(i).Width = 12;
                            }

                            // ===== CHỮ KÝ =====
                            int signatureRow = lastDataRow + 2;
                            int totalColumns = dgvHienThiChucVu.Columns.Count;
                            int signatureStartCol = Math.Max(1, totalColumns - 2);
                            int signatureEndCol = totalColumns;

                            ws.Cell(signatureRow, signatureStartCol).Value = $"Hà Nội, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
                            ws.Range(signatureRow, signatureStartCol, signatureRow, signatureEndCol).Merge();
                            ws.Cell(signatureRow, signatureStartCol).Style.Font.Italic = true;
                            ws.Cell(signatureRow, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Cell(signatureRow + 1, signatureStartCol).Value = "Người lập báo cáo";
                            ws.Range(signatureRow + 1, signatureStartCol, signatureRow + 1, signatureEndCol).Merge();
                            ws.Cell(signatureRow + 1, signatureStartCol).Style.Font.Bold = true;
                            ws.Cell(signatureRow + 1, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            string nguoiDangNhap = LoginInfo.CurrentUserName ?? "Administrator";
                            ws.Cell(signatureRow + 4, signatureStartCol).Value = nguoiDangNhap;
                            ws.Range(signatureRow + 4, signatureStartCol, signatureRow + 4, signatureEndCol).Merge();
                            ws.Cell(signatureRow + 4, signatureStartCol).Style.Font.Bold = true;
                            ws.Cell(signatureRow + 4, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Row(1).Height = 20;
                            ws.Row(2).Height = 25;
                            ws.Row(startRow).Height = 20;
                            ws.Row(headerRow).Height = 20;

                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ==================== XUẤT PDF ====================
        private void PDF_Click(object sender, EventArgs e)
        {
            if (dgvHienThiChucVu.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ⭐ XÁC ĐỊNH TIÊU ĐỀ DỰA VÀO FLAG
            string reportTitle = isViewingDeletedRecords ? "DANH SÁCH CHỨC VỤ ĐÃ XÓA" : "DANH SÁCH CHỨC VỤ";
            string fileName = isViewingDeletedRecords
                ? $"ChucVuDaXoa_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                : $"BaoCaoChucVu_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

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

                        // Lấy thông tin phòng ban đang chọn
                        string tenPhongBan = cbbMaPB.Text;
                        string maPhongBan = cbbMaPB.SelectedValue?.ToString() ?? "";

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

                            // ⭐ CHỈ HIỂN THỊ THÔNG TIN PHÒNG BAN KHI KHÔNG PHẢI DANH SÁCH ĐÃ XÓA
                            if (!isViewingDeletedRecords)
                            {
                                var pbInfo = new iTextSharp.text.Paragraph("Phòng Ban: " + (!string.IsNullOrEmpty(tenPhongBan) ? $"{tenPhongBan} ({maPhongBan})" : "Tất cả phòng ban"), fontNormal);
                                pbInfo.Alignment = iTextSharp.text.Element.ALIGN_LEFT;
                                pbInfo.SpacingAfter = 15;
                                document.Add(pbInfo);
                            }
                            else
                            {
                                // Thêm khoảng trắng
                                document.Add(new iTextSharp.text.Paragraph("\n"));
                            }

                            // ===== TIÊU ĐỀ BẢNG =====
                            var tableTitle = new iTextSharp.text.Paragraph(reportTitle, fontTableHeader);
                            tableTitle.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                            tableTitle.SpacingAfter = 5;
                            document.Add(tableTitle);

                            // ===== TẠO BẢNG =====
                            var table = new iTextSharp.text.pdf.PdfPTable(dgvHienThiChucVu.Columns.Count);
                            table.WidthPercentage = 100;

                            float[] columnWidths = new float[dgvHienThiChucVu.Columns.Count];
                            for (int i = 0; i < dgvHienThiChucVu.Columns.Count; i++)
                            {
                                if (i == 0 && dgvHienThiChucVu.Columns[i].Name == "STT")
                                    columnWidths[i] = 0.5f;
                                else
                                    columnWidths[i] = 2f;
                            }
                            table.SetWidths(columnWidths);

                            // Header
                            foreach (DataGridViewColumn column in dgvHienThiChucVu.Columns)
                            {
                                var cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(column.HeaderText, fontTableHeader));
                                cell.BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY;
                                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                                cell.Padding = 8;
                                table.AddCell(cell);
                            }

                            // Data
                            foreach (DataGridViewRow row in dgvHienThiChucVu.Rows)
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

                            string nguoiDangNhap = LoginInfo.CurrentUserName ?? "Administrator";
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

        private void dgvHienThiChucVu_CellClick_2(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            isEditingChucVu = true;
            isLoadingChucVu = true;

            int i = dgvHienThiChucVu.CurrentRow.Index;
            tbMaChuVu.Text = dgvHienThiChucVu.Rows[i].Cells[1].Value?.ToString() ?? "";
            txtTenChucVu.Text = dgvHienThiChucVu.Rows[i].Cells[2].Value?.ToString() ?? "";
            txtGhiChu.Text = dgvHienThiChucVu.Rows[i].Cells[3].Value?.ToString() ?? "";

            // Tự động chọn phòng ban tương ứng
            string maPB = dgvHienThiChucVu.Rows[i].Cells[3].Value?.ToString() ?? "";
            if (!string.IsNullOrEmpty(maPB))
            {
                cbbMaPB.SelectedValue = maPB;
            }

            isLoadingChucVu = false;
            tbMaChuVu.ReadOnly = true;
        }

        private void checkshowpassword_CheckedChanged_1(object sender, EventArgs e)
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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonlamsach_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaChuVu.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn mã chức vụ cần xóa hẳng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(txtMKKhoiPhuc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu Admin để xác nhận xóa hẳn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string matKhauNhap = txtMKKhoiPhuc.Text.Trim();
                c.connect();

                // KIỂM TRA MẬT KHẨU ADMIN
                string sqlCheckPassword = @"
            SELECT COUNT(*) 
            FROM tblTaiKhoan_KhangCD233181 
            WHERE MatKhau_KhangCD233181 = @MatKhau 
            AND RoleId_ThuanCD233318 = 1 
            AND DeletedAt_KhangCD233181 = 0";

                bool isValidAdmin = false;
                using (SqlCommand cmdCheck = new SqlCommand(sqlCheckPassword, c.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MatKhau", matKhauNhap);
                    int count = (int)cmdCheck.ExecuteScalar();
                    if (count > 0)
                        isValidAdmin = true;
                }

                if (!isValidAdmin)
                {
                    MessageBox.Show("Mật khẩu không đúng hoặc bạn không có quyền Admin!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    c.disconnect();
                    txtMKKhoiPhuc.Clear();
                    return;
                }

                // KIỂM TRA XEM CHỨC VỤ CÓ TỒN TẠI TRONG DANH SÁCH ĐÃ XÓA KHÔNG
                string checkDeletedSql = @"
            SELECT COUNT(*) 
            FROM tblChucVu_KhangCD233181 
            WHERE MaCV_KhangCD233181 = @MaCV 
            AND DeletedAt_KhangCD233181 = 1";

                using (SqlCommand cmdCheckDeleted = new SqlCommand(checkDeletedSql, c.conn))
                {
                    cmdCheckDeleted.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    int deletedCount = (int)cmdCheckDeleted.ExecuteScalar();

                    if (deletedCount == 0)
                    {
                        MessageBox.Show("Chức vụ này không tồn tại trong danh sách đã xóa!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        c.disconnect();
                        return;
                    }
                }

                // KIỂM TRA XEM CÓ NHÂN VIÊN NÀO ĐANG SỬ DỤNG CHỨC VỤ NÀY KHÔNG
                string checkNhanVienSql = @"
            SELECT COUNT(*) 
            FROM tblNhanVien_TuanhCD233018 
            WHERE MaCV_KhangCD233181 = @MaCV";

                using (SqlCommand cmdCheckNV = new SqlCommand(checkNhanVienSql, c.conn))
                {
                    cmdCheckNV.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    int nvCount = (int)cmdCheckNV.ExecuteScalar();

                    if (nvCount > 0)
                    {
                        MessageBox.Show(
                            $"Không thể xóa hẳng! Vẫn còn {nvCount} nhân viên đang có chức vụ này.\n\n" +
                            "Vui lòng xóa hoặc chuyển chức vụ cho các nhân viên trước khi xóa hẳn chức vụ.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        c.disconnect();
                        return;
                    }
                }

                // XÁC NHẬN XÓA HẲNG
                DialogResult confirm = MessageBox.Show(
                    "⚠️ CẢNH BÁO: Bạn có chắc chắn muốn XÓA HẲNG chức vụ này?\n\n" +
                    "Thao tác này sẽ xóa VĨNH VIỄN dữ liệu chức vụ khỏi hệ thống.\n" +
                    "Dữ liệu sẽ KHÔNG THỂ KHÔI PHỤC!\n\n" +
                    $"Mã chức vụ: {tbMaChuVu.Text}\n" +
                    $"Tên chức vụ: {txtTenChucVu.Text}",
                    "⚠️ XÁC NHẬN XÓA HẲNG",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm != DialogResult.Yes)
                {
                    c.disconnect();
                    return;
                }

                // XÓA HẲNG CHỨC VỤ
                string deleteQuery = @"
            DELETE FROM tblChucVu_KhangCD233181 
            WHERE MaCV_KhangCD233181 = @MaCV 
            AND DeletedAt_KhangCD233181 = 1";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Đã xóa hẳn chức vụ thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        c.disconnect();

                        // Clear inputs và reset
                        ClearAllInputs(this);
                        tbMaChuVu.Text = TaoMaChucVuTuDong();
                        txtMKKhoiPhuc.Clear();
                        isEditingChucVu = false;

                        // Load lại dữ liệu (hiển thị chức vụ đang hoạt động)
                        LoadDataChucVu();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa hẳn chức vụ!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        c.disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                try { c.disconnect(); } catch { }
                MessageBox.Show("Lỗi khi xóa hẳn chức vụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMKKhoiPhuc_TextChanged(object sender, EventArgs e)
        {

        }
        //private void dgvHienThiChucVu_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        //{
        //    // Vẽ số thứ tự ở đầu mỗi row
        //    var grid = sender as DataGridView;
        //    var rowIdx = (e.RowIndex + 1).ToString();

        //    var centerFormat = new StringFormat()
        //    {
        //        Alignment = StringAlignment.Center,
        //        LineAlignment = StringAlignment.Center
        //    };

        //    var headerBounds = new System.Drawing.Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

        //    e.Graphics.DrawString(rowIdx, grid.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        //}
    }
}