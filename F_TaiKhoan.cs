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
    public partial class F_TaiKhoan: Form
    {
        public F_TaiKhoan()
        {
            InitializeComponent();
        }

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
        private void LoadDataTaiKhoan()
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT 
                                                tk.MaTK AS [Mã Tài Khoản], 
                                                tk.MaNV AS [Mã Nhân Viên], 
                                                tk.TenDangNhap AS [Tên Đăng Nhập], 
                                                tk.MatKhau AS [Mật Khẩu], 
                                                tk.Quyen AS [Quyền], 
                                                tk.GhiChu AS [Ghi Chú]
                                            FROM tblTaiKhoan AS tk
                                            INNER JOIN tblNhanVien AS nv ON tk.MaNV = nv.MaNV
                                            WHERE nv.DeletedAt = 0  
                                            ORDER BY tk.MaTK;
                                            ";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewTaiKhoan.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
                LoadcomboBox();
                tbMKkhoiphuc.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu tai khoan nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        
        private void LoadcomboBox()
        {
            try
            {
                cn.connect(); //  || SELECT nv.MaNV, nv.HoTen FROM tblNhanVien nv LEFT JOIN tblTaiKhoan tk ON nv.MaNV = tk.MaNV AND tk.DeletedAt = 0 WHERE nv.DeletedAt = 0 AND tk.MaNV IS NULL
                string sqlLoadcomboBoxtblnhanvien = "SELECT * FROM tblNhanVien WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxtblnhanvien, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxMaNV.DataSource = ds.Tables[0];
                    cbBoxMaNV.DisplayMember = "HoTen";//Xác định cột nào của bảng dữ liệu sẽ được hiển thị lên ComboBox
                    cbBoxMaNV.ValueMember = "MaNV"; // cot gia tri
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ma NV: " + ex.Message);
            }
        }

        private void F_TaiKhoan_Load(object sender, EventArgs e)
        {
            LoadDataTaiKhoan();
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (
                    string.IsNullOrWhiteSpace(tbmaTK.Text) ||
                    string.IsNullOrWhiteSpace(tbTenDangNhap.Text) ||
                    string.IsNullOrWhiteSpace(tbMatKhau.Text) ||
                    cbBoxMaNV.SelectedIndex == -1 ||
                    cbBoxQuyen.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // check mk
                if (tbMatKhau.Text.Trim().Length < 8)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();

                // chceck matk
                string checkMaTKNVSql = "SELECT COUNT(*) FROM tblTaiKhoan WHERE MaTK = @MaTK AND DeletedAt != 1";
                using (SqlCommand cmdCheckMaTK = new SqlCommand(checkMaTKNVSql, cn.conn))
                {
                    cmdCheckMaTK.Parameters.AddWithValue("@MaTK", tbmaTK.Text.Trim());
                    int maTKCount = (int)cmdCheckMaTK.ExecuteScalar();

                    if (maTKCount > 0)
                    {
                        MessageBox.Show("Mã tài khoản này đã tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                //check ten dang nhap
                string checkTenDNNVSql = "SELECT COUNT(*) FROM tblTaiKhoan WHERE TenDangNhap = @TenDangNhap AND DeletedAt != 1";
                using (SqlCommand cmdCheckTenDN = new SqlCommand(checkTenDNNVSql, cn.conn))
                {
                    cmdCheckTenDN.Parameters.AddWithValue("@TenDangNhap", tbTenDangNhap.Text.Trim());
                    int maTKCount = (int)cmdCheckTenDN.ExecuteScalar();

                    if (maTKCount > 0)
                    {
                        MessageBox.Show("Ten dang nhap này đã tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // check TK NV 3
                string checkNVSql = "SELECT COUNT(*) FROM tblTaiKhoan WHERE MaNV = @MaNV AND DeletedAt != 1";
                using (SqlCommand cmdCheckNV = new SqlCommand(checkNVSql, cn.conn))
                {
                    cmdCheckNV.Parameters.AddWithValue("@MaNV", cbBoxMaNV.SelectedValue);
                    int countNV = (int)cmdCheckNV.ExecuteScalar();

                    if (countNV > 0)
                    {
                        MessageBox.Show("Nhân viên này đã có tài khoản trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                string sqltblTaiKhoan = @"
                    INSERT INTO tblTaiKhoan 
                        (MaTK, MaNV, TenDangNhap, MatKhau, Quyen, Ghichu, DeletedAt)
                    VALUES 
                        (@MaTK, @MaNV, @TenDangNhap, @MatKhau, @Quyen, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(sqltblTaiKhoan, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", tbmaTK.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaNV", cbBoxMaNV.SelectedValue);
                    cmd.Parameters.AddWithValue("@TenDangNhap", tbTenDangNhap.Text.Trim());
                    cmd.Parameters.AddWithValue("@MatKhau", tbMatKhau.Text.Trim());
                    cmd.Parameters.AddWithValue("@Quyen", cbBoxQuyen.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm tài khoản thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.disconnect();
                        ClearAllInputs(this);
                        LoadDataTaiKhoan();
                    }
                    else
                    {
                        cn.disconnect();
                        MessageBox.Show("Thêm tài khoản thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                //cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaTK.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã tai khoan cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa tai khoan này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = "UPDATE tblTaiKhoan SET DeletedAt = 1 WHERE MaTK = @MaTK";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTK", tbmaTK.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataTaiKhoan();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                cn.connect();

                // 1. Kiểm tra rỗng
                if (string.IsNullOrEmpty(tbmaTK.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã tài khoản cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (
                    string.IsNullOrWhiteSpace(tbmaTK.Text) ||
                    string.IsNullOrWhiteSpace(tbTenDangNhap.Text) ||
                    string.IsNullOrWhiteSpace(tbMatKhau.Text) ||
                    cbBoxMaNV.SelectedIndex == -1 ||
                    cbBoxQuyen.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newMaNV = cbBoxMaNV.SelectedValue.ToString();
                string newTenDangNhap = tbTenDangNhap.Text.Trim();
                string maTK = tbmaTK.Text.Trim();

                // 2. Kiểm tra xem tên đăng nhập hiện tại có khớp với tên trong DB theo MaNV không
                string sqlCheckOld = @"SELECT COUNT(*) FROM tblTaiKhoan 
                               WHERE MaNV = @MaNV AND TenDangNhap = @TenDangNhap AND DeletedAt = 0";
                using (SqlCommand cmd = new SqlCommand(sqlCheckOld, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", newMaNV);
                    cmd.Parameters.AddWithValue("@TenDangNhap", newTenDangNhap);

                    int countOld = (int)cmd.ExecuteScalar();

                    if (countOld == 0)
                    {
                        // Nếu tên đăng nhập đã thay đổi -> Kiểm tra tên mới đã tồn tại chưa
                        string sqlCheckNew = @"SELECT COUNT(*) FROM tblTaiKhoan 
                                       WHERE TenDangNhap = @TenDangNhap AND DeletedAt = 0";
                        using (SqlCommand cmdCheck = new SqlCommand(sqlCheckNew, cn.conn))
                        {
                            cmdCheck.Parameters.AddWithValue("@TenDangNhap", newTenDangNhap);
                            int countNew = (int)cmdCheck.ExecuteScalar();

                            if (countNew > 0)
                            {
                                MessageBox.Show("Tên đăng nhập này đã tồn tại trong hệ thống!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                cn.disconnect();
                                return;
                            }
                        }
                    }
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa tài khoản này không?",
                    "Xác nhận sửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    string sqlUpdate = @"UPDATE tblTaiKhoan 
                                 SET TenDangNhap = @TenDangNhap, 
                                     MatKhau = @MatKhau, 
                                     Quyen = @Quyen, 
                                     GhiChu = @GhiChu, 
                                     DeletedAt = 0 
                                 WHERE MaTK = @MaTK";

                    using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, cn.conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@MaTK", maTK);
                        cmdUpdate.Parameters.AddWithValue("@TenDangNhap", newTenDangNhap);
                        cmdUpdate.Parameters.AddWithValue("@MatKhau", tbMatKhau.Text.Trim());
                        cmdUpdate.Parameters.AddWithValue("@Quyen", cbBoxQuyen.SelectedItem.ToString());
                        cmdUpdate.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                        int rows = cmdUpdate.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataTaiKhoan();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            cn.disconnect();
                            MessageBox.Show("Sửa tài khoản thất bại!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (confirm == DialogResult.No)
                {
                    cn.disconnect();
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

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cbBoxMaNV.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên để tìm kiếm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string selectedMaNV = cbBoxMaNV.SelectedValue.ToString();

                cn.connect();
                string sql = @" SELECT MaTK, MaNV, TenDangNhap, MatKhau, Quyen, GhiChu
                                FROM tblTaiKhoan
                                WHERE DeletedAt = 0 
                                  AND MaNV = @MaNV
                                ORDER BY MaTK";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", selectedMaNV);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridViewTaiKhoan.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrestar_Click_1(object sender, EventArgs e)
        {
            LoadDataTaiKhoan();
        }

        private void btnxuatExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewTaiKhoan.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("NhanVien");

                                // Ghi header
                                for (int i = 0; i < dataGridViewTaiKhoan.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dataGridViewTaiKhoan.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dataGridViewTaiKhoan.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dataGridViewTaiKhoan.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dataGridViewTaiKhoan.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dataGridViewTaiKhoan.Rows.Count + 1, dataGridViewTaiKhoan.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // Tự động co giãn cột
                                ws.Columns().AdjustToContents();

                                // Lưu file
                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnHienThiPhongBanCu_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @" SELECT MaTK, MaNV, TenDangNhap, MatKhau, Quyen, Ghichu
                                FROM tblTaiKhoan
                                WHERE DeletedAt = 1
                                ORDER BY MaTK";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewTaiKhoan.DataSource = dt;
                }
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
                if (string.IsNullOrEmpty(tbmaTK.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã tai khoan cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                string checkMaTKSql = "SELECT COUNT(*) FROM tblTaiKhoan WHERE MaTK = @MaTK AND DeletedAt = 1";
                using (SqlCommand cmdcheckcheckMaTKSql = new SqlCommand(checkMaTKSql, cn.conn))
                {
                    cmdcheckcheckMaTKSql.Parameters.AddWithValue("@MaTK", tbmaTK.Text.Trim());
                    int emailCount = (int)cmdcheckcheckMaTKSql.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        MessageBox.Show("Ma TK này đã tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                //
                if (tbMKkhoiphuc.Text == "")
                {
                    MessageBox.Show("Vui lòng mật khẩu để khoi phuc", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Question);
                    return;
                }

                string sqMKkhoiphuc = "SELECT * FROM tblTaiKhoan WHERE Quyen = @Quyen AND MatKhau = @MatKhau";
                SqlCommand cmdkhoiphuc = new SqlCommand(sqMKkhoiphuc, cn.conn);
                cmdkhoiphuc.Parameters.AddWithValue("@Quyen", "Admin");
                cmdkhoiphuc.Parameters.AddWithValue("@MatKhau", tbMKkhoiphuc.Text);
                SqlDataReader reader = cmdkhoiphuc.ExecuteReader();

                if (reader.Read() == false)
                {
                    MessageBox.Show("mật khẩu không đúng? Vui lòng nhập lại mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    tbMKkhoiphuc.Text = "";
                    reader.Close();
                    cn.disconnect();
                    return;
                }
                reader.Close();


                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn khôi phục tai khoan này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


                if (confirm == DialogResult.Yes)
                {
                    tbMKkhoiphuc.Text = "";
                    string query = "UPDATE tblTaiKhoan SET DeletedAt = 0 WHERE MaTK = @MaTK";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaTK", tbmaTK.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("khôi phục tai kkhoan thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            ClearAllInputs(this);
                            LoadDataTaiKhoan();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy tai khoan để khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                }
                else if (confirm == DialogResult.No)
                {
                    cn.disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
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

        //private string oldMaNV = "";
        //private string oldTenDangNhap = "";
        private void dataGridViewTaiKhoan_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbmaTK.Text = dataGridViewTaiKhoan.Rows[i].Cells[0].Value.ToString();
                cbBoxMaNV.SelectedValue = dataGridViewTaiKhoan.Rows[i].Cells[1].Value.ToString();
                tbTenDangNhap.Text = dataGridViewTaiKhoan.Rows[i].Cells[2].Value.ToString(); ;
                tbMatKhau.Text = dataGridViewTaiKhoan.Rows[i].Cells[3].Value.ToString();
                cbBoxQuyen.Text = dataGridViewTaiKhoan.Rows[i].Cells[4].Value.ToString();
                tbGhiChu.Text = dataGridViewTaiKhoan.Rows[i].Cells[5].Value.ToString();

                //DataGridViewRow row = dataGridViewTaiKhoan.Rows[e.RowIndex];
                //oldMaNV = row.Cells["MaNV"].Value.ToString();
                //oldTenDangNhap = row.Cells["TenDangNhap"].Value.ToString();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
