using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ClosedXML.Excel;
using ZXing;

namespace QuanLyNhanVien3
{
    public partial class F_NhanVien: Form
    {
        public F_NhanVien()
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
        ////check
        private bool checknhanvien()
        {
            try
            {
                double a;

                // check sdt
                if (!double.TryParse(tbSoDienThoai.Text.Trim(), out a))
                {
                    MessageBox.Show("Số điện thoại phải là số!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else if (tbSoDienThoai.Text.Trim().Length != 10)
                {
                    MessageBox.Show("Số điện thoại phải có đúng 10 chữ số!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                 // check ma nv
                string checkMaNVSql = "SELECT COUNT(*) FROM tblNhanVien WHERE MaNV = @MaNV ";
                using (SqlCommand cmd = new SqlCommand(checkMaNVSql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text.Trim());
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Mã nhân viên này đã tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                // check mail
                string checkEmailSql = "SELECT COUNT(*) FROM tblNhanVien WHERE Email = @Email ";
                using (SqlCommand cmd = new SqlCommand(checkEmailSql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Email", tbEmail.Text.Trim());
                    int emailCount = (int)cmd.ExecuteScalar();

                    if (emailCount > 0)
                    {
                        MessageBox.Show("Email này đã tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else if (!tbEmail.Text.Trim().ToLower().EndsWith("@gmail.com"))
                    {
                        MessageBox.Show("Email phải có đuôi @gmail.com!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                // check ma pb
                string checkMaPBSql = "SELECT COUNT(*) FROM tblPhongBan WHERE MaPB = @MaPB ";
                using (SqlCommand cmd = new SqlCommand(checkMaPBSql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue);
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        MessageBox.Show("Mã phòng ban không tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                // chech macv
                string checkMaCVSql = "SELECT COUNT(*) FROM tblChucVu WHERE MaCV = @MaCV ";
                using (SqlCommand cmd = new SqlCommand(checkMaCVSql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue);
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        MessageBox.Show("Mã chức vụ không tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                return true; //ok
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kiểm tra dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //check

        private void LoadDataNhanVien()
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT 
                                            nv.MaNV AS [Mã Nhân Viên], 
                                            nv.HoTen AS [Họ và Tên], 
                                            nv.NgaySinh AS [Ngày Sinh], 
                                            nv.GioiTinh AS [Giới Tính], 
                                            nv.DiaChi AS [Địa Chỉ], 
                                            nv.SoDienThoai AS [Số Điện Thoại], 
                                            nv.Email AS [Email], 
                                            nv.MaPB AS [Mã Phòng Ban], 
                                            nv.MaCV AS [Mã Chức Vụ], 
                                            nv.GhiChu AS [Ghi Chú]
                                        FROM tblNhanVien AS nv
                                        INNER JOIN tblHopDong AS hd ON nv.MaNV = hd.MaNV
                                        WHERE nv.DeletedAt = 0 
                                          AND hd.DeletedAt = 0
                                        ORDER BY nv.MaNV;
                                        "; //  and cv.DeletedAt = 0  ,tblChucVu as cv   and cv.MaCV = nv.MaCV 

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewNhanVien.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
                LoadcomboBox();
                tbMKkhoiphuc.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoadcomboBox()
        {
            try
            {
                cn.connect();
                string sqlLoadcomboBoxtblPhongBan = "SELECT * FROM tblPhongBan WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxtblPhongBan, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxMaPB.DataSource = ds.Tables[0];
                    cbBoxMaPB.DisplayMember = "MaPB";// hien thi
                    cbBoxMaPB.ValueMember = "MaPB"; // cot gia tri
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ma PB: " + ex.Message);
            }
            // load chuc vu combobox
            try
            {
                cn.connect();
                string sqsqlLoadcomboBoxttblChucVu = "SELECT * FROM tblChucVu WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqsqlLoadcomboBoxttblChucVu, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxChucVu.DataSource = ds.Tables[0];
                    cbBoxChucVu.DisplayMember = "TenCV"; // cot hien thi
                    cbBoxChucVu.ValueMember = "MaCV"; // cot gia tri
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ma CV: " + ex.Message);
            }
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            LoadDataNhanVien();
        }

        private void dtGridViewNhanVien_CellClick_2(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbmaNV.Text = dtGridViewNhanVien.Rows[i].Cells[0].Value.ToString();
                tbHoTen.Text = dtGridViewNhanVien.Rows[i].Cells[1].Value.ToString();
                dateTimePickerNgaySinh.Value = Convert.ToDateTime(dtGridViewNhanVien.Rows[i].Cells[2].Value);
                cbBoxGioiTinh.Text = dtGridViewNhanVien.Rows[i].Cells[3].Value.ToString();
                tbDiaChi.Text = dtGridViewNhanVien.Rows[i].Cells[4].Value.ToString();
                tbSoDienThoai.Text = dtGridViewNhanVien.Rows[i].Cells[5].Value.ToString();
                tbEmail.Text = dtGridViewNhanVien.Rows[i].Cells[6].Value.ToString();
                cbBoxMaPB.SelectedValue = dtGridViewNhanVien.Rows[i].Cells[7].Value.ToString();
                cbBoxChucVu.SelectedValue = dtGridViewNhanVien.Rows[i].Cells[8].Value.ToString();
                tbGhiChu.Text = dtGridViewNhanVien.Rows[i].Cells[9].Value.ToString();
            }
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {

                // Kiểm tra dữ liệu nhập vào    string.IsNullOrWhiteSpace(tbmaNV.Text) ||
                if (
                    string.IsNullOrWhiteSpace(tbmaNV.Text) ||
                    string.IsNullOrWhiteSpace(tbHoTen.Text) ||
                    cbBoxGioiTinh.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbDiaChi.Text) ||
                    string.IsNullOrWhiteSpace(tbSoDienThoai.Text) ||
                    string.IsNullOrWhiteSpace(tbEmail.Text) ||
                    cbBoxChucVu.SelectedIndex == -1 ||
                    cbBoxMaPB.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                //double a;
                //// cehck sdt
                //if (!double.TryParse(tbSoDienThoai.Text.Trim(), out a))
                //{
                //    MessageBox.Show("Số điện thoại phải là số!", "Thông báo",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    cn.disconnect();
                //    return;
                //}
                //else if (tbSoDienThoai.Text.Trim().Length != 10)
                //{
                //    MessageBox.Show("Số điện thoại phải có đúng 10 chữ số!", "Thông báo",
                //    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    cn.disconnect();
                //    return;

                //}


                ////  check ma nv
                //string checkMaNVSql = "SELECT COUNT(*) FROM tblNhanVien WHERE MaNV = @MaNV AND DeletedAt = 0";
                //using (SqlCommand cmdcheckEmailSql = new SqlCommand(checkMaNVSql, cn.conn))
                //{
                //    cmdcheckEmailSql.Parameters.AddWithValue("@MaNV", tbmaNV.Text.Trim());
                //    int emailCount = (int)cmdcheckEmailSql.ExecuteScalar();

                //    if (emailCount > 0)
                //    {
                //        MessageBox.Show("Ma NV này đã tồn tại trong hệ thống!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        cn.disconnect();
                //        return; // Dừng lại, không thêm nhân viên
                //    }
                //}

                ////  check mail
                //string checkEmailSql = "SELECT COUNT(*) FROM tblNhanVien WHERE Email = @Email AND DeletedAt = 0";
                //using (SqlCommand cmdcheckEmailSql = new SqlCommand(checkEmailSql, cn.conn))
                //{
                //    cmdcheckEmailSql.Parameters.AddWithValue("@Email", tbEmail.Text.Trim());
                //    int emailCount = (int)cmdcheckEmailSql.ExecuteScalar();

                //    if (emailCount > 0)
                //    {
                //        MessageBox.Show("Email này đã tồn tại trong hệ thống!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        cn.disconnect();
                //        return; // Dừng lại, không thêm nhân viên
                //    }
                //    else if (!tbEmail.Text.Trim().ToLower().EndsWith("@gmail.com"))
                //    {
                //        MessageBox.Show("Email phải có đuôi @gmail.com!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        cn.disconnect();
                //        return;
                //    }
                //}
                //// check ma phong ban
                //string checkMaPBSql = "SELECT COUNT(*) FROM tblPhongBan  WHERE MaPB  = @MaPB  AND DeletedAt = 0";
                //using (SqlCommand cmdcheckMaPBSql = new SqlCommand(checkMaPBSql, cn.conn))
                //{
                //    cmdcheckMaPBSql.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue);
                //    int MaPBCount = (int)cmdcheckMaPBSql.ExecuteScalar();

                //    if (MaPBCount == 0)
                //    {
                //        MessageBox.Show("Mã phòng ban không tồn tại trong hệ thống!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        cn.disconnect();
                //        return; // Dừng lại, không thêm nhân viên
                //    }

                //}
                //// check ma chuc vu
                //string checkMaCVSql = "SELECT COUNT(*) FROM tblChucVu  WHERE MaCV  = @MaCV  AND DeletedAt = 0";
                //using (SqlCommand cmdcheckMaCVSql = new SqlCommand(checkMaCVSql, cn.conn))
                //{
                //    cmdcheckMaCVSql.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue);
                //    int MaCVCount = (int)cmdcheckMaCVSql.ExecuteScalar();

                //    if (MaCVCount == 0)
                //    {
                //        MessageBox.Show("Mã chức vụ không tồn tại trong hệ thống!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        cn.disconnect();
                //        return; // Dừng lại, không thêm nhân viên
                //    }
                //}

                if (!checknhanvien())
                {
                    cn.disconnect();
                    return;
                }
                // Câu lệnh SQL chèn dữ liệu vào bảng tblNhanVien
                string sqltblNhanVien = @"INSERT INTO tblNhanVien 
                           (MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaPB, MaCV, GhiChu, DeletedAt)
                           VALUES ( @MaNV, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email, @MaPB, @MaCV, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(sqltblNhanVien, cn.conn))
                {
                    // Gán giá trị từ các ô nhập liệu vào tham số SQL
                    cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@HoTen", tbHoTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgaySinh", dateTimePickerNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@GioiTinh", cbBoxGioiTinh.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@DiaChi", tbDiaChi.Text.Trim());
                    cmd.Parameters.AddWithValue("@SoDienThoai", tbSoDienThoai.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", tbEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue);
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.disconnect();
                        ClearAllInputs(this);
                        LoadDataNhanVien();
                    }
                    else
                    {
                        MessageBox.Show("Thêm nhân viên thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cn.disconnect();
                    }
                }
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
                if (string.IsNullOrEmpty(tbmaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã nhân viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa nhân viên này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = "UPDATE tblNhanVien SET DeletedAt = 1 WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            ClearAllInputs(this);
                            LoadDataNhanVien();
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

        private void btnrestar_Click_1(object sender, EventArgs e)
        {
            LoadDataNhanVien();
        }

        private void btnxuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dtGridViewNhanVien.Rows.Count > 0)
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
                                for (int i = 0; i < dtGridViewNhanVien.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewNhanVien.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewNhanVien.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewNhanVien.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dtGridViewNhanVien.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dtGridViewNhanVien.Rows.Count + 1, dtGridViewNhanVien.Columns.Count);
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

        private void btnNVDaNghiViec_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @"SELECT  MaNV ,HoTen, NgaySinh, GioiTinh, DiaChi,  SoDienThoai,  Email, MaPB, MaCV,  GhiChu
                                FROM tblNhanVien
                                WHERE DeletedAt = 1 ORDER BY MaNV";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewNhanVien.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnKhoiPhucNhanVien_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã nhân viên cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                string checkMaNVSql = "SELECT COUNT(*) FROM tblNhanVien WHERE MaNV = @MaNV AND DeletedAt = 1";
                using (SqlCommand cmdcheckcheckMaNVSql = new SqlCommand(checkMaNVSql, cn.conn))
                {
                    cmdcheckcheckMaNVSql.Parameters.AddWithValue("@MaNV", tbmaNV.Text.Trim());
                    int emailCount = (int)cmdcheckcheckMaNVSql.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        MessageBox.Show("Ma NV này đã tồn tại trong hệ thống!", "Thông báo",
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
                    "Bạn có chắc chắn muốn khôi phục nhân viên này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


                if (confirm == DialogResult.Yes)
                {
                    tbMKkhoiphuc.Text = "";
                    string query = "UPDATE tblNhanVien SET DeletedAt = 0 WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        // DELETE FROM tblNhanVien WHERE MaNV = @MaNV / UPDATE tblNhanVien SET DeletedAt = 1 WHERE MaNV = @MaNV
                        cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("khôi phục nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearAllInputs(this);
                            LoadDataNhanVien();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                double a;
                cn.connect();
                if (string.IsNullOrEmpty(tbmaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoac nhap ma nhân viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return;
                }

                // check sdt
                if (!double.TryParse(tbSoDienThoai.Text.Trim(), out a))
                {
                    MessageBox.Show("Số điện thoại phải là số!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return ;
                }
                else if (tbSoDienThoai.Text.Trim().Length != 10)
                {
                    MessageBox.Show("Số điện thoại phải có đúng 10 chữ số!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return ;
                }
                if (!tbEmail.Text.Trim().ToLower().EndsWith("@gmail.com"))
                {
                    MessageBox.Show("Email phải có đuôi @gmail.com!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return;
                }

                string checkMaNVSql = "SELECT COUNT(*) FROM tblNhanVien WHERE MaNV = @MaNV AND DeletedAt = 0";
                using (SqlCommand cmd = new SqlCommand(checkMaNVSql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text.Trim());
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        MessageBox.Show("Mã nhân viên này khong tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }
                if (
                    string.IsNullOrWhiteSpace(tbHoTen.Text) ||
                    cbBoxGioiTinh.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbDiaChi.Text) ||
                    string.IsNullOrWhiteSpace(tbSoDienThoai.Text) ||
                    string.IsNullOrWhiteSpace(tbEmail.Text) ||
                    cbBoxChucVu.SelectedIndex == -1 ||
                    cbBoxMaPB.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa nhân viên này không?",
                    "Xác nhận sửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    string sql = @"UPDATE tblNhanVien SET  HoTen = @HoTen, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, DiaChi = @DiaChi, SoDienThoai = @SoDienThoai, 
                             Email = @Email, MaPB = @MaPB, MaCV = @MaCV, GhiChu= @GhiChu, DeletedAt = 0 WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text.Trim());
                        cmd.Parameters.AddWithValue("@HoTen", tbHoTen.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgaySinh", dateTimePickerNgaySinh.Value);
                        cmd.Parameters.AddWithValue("@GioiTinh", cbBoxGioiTinh.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@DiaChi", tbDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai", tbSoDienThoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", tbEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue);
                        cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue);
                        cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadDataNhanVien();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Sửa nhân viên thất bại!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nhân viên để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sql = @"SELECT MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaPB, MaCV, GhiChu
                                FROM tblNhanVien
                                WHERE DeletedAt = 0
                                  AND HoTen LIKE '%' + @TenTimKiem + '%'
                                ORDER BY MaNV";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenTimKiem", "%" + tbHoTen.Text + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewNhanVien.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btnTaoQR_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem đã nhập Mã nhân viên chưa
                if (string.IsNullOrWhiteSpace(tbmaNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập Mã Nhân Viên trước khi tạo QR!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng BarcodeWriter
                BarcodeWriter writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE, // Chọn kiểu QR
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 200, // Chiều rộng QR
                        Height = 200, // Chiều cao QR
                        Margin = 1,
                        //CharacterSet = "UTF-8" // <-- BẮT BUỘC để hỗ trợ tiếng Việt
                    }
                };

                // Ghép nhiều thông tin vào chuỗi
                string maNV = tbmaNV.Text.Trim();
                //string hoTen = tbHoTen.Text.Trim();
                //string gioitinh = cbBoxGioiTinh.Text.Trim();
                //string diachi = tbDiaChi.Text.Trim();
                //string sodienthoai = tbSoDienThoai.Text.Trim();
                //string email = tbEmail.Text.Trim();
                //string ghichu = tbGhiChu.Text.Trim();
                //string phongBan = cbBoxMaPB.Text.Trim();
                //string chucVu = cbBoxChucVu.Text.Trim();
                //string ngaySinh = dateTimePickerNgaySinh.Value.ToString("yyyy-MM-dd");

                // Gộp các thông tin lại với dấu phân cách |
                //string data = $"{maNV}||{hoTen}||{gioitinh}||{diachi}||{sodienthoai}||{email}||{ghichu}||{phongBan}||{chucVu}||{ngaySinh}";


                // Sinh QR dựa trên MaNV

                string data = tbmaNV.Text.Trim(); // Dùng mã nhân viên
                Bitmap qrBitmap = writer.Write(data);

                // Hiển thị lên PictureBox
                pictureBoxQRNV.Image = qrBitmap;

                // Hỏi có muốn lưu QR không
                DialogResult result = MessageBox.Show("Bạn có muốn lưu QR Code này không?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SaveFileDialog saveFile = new SaveFileDialog())
                    {
                        saveFile.Filter = "PNG Image|*.png";
                        saveFile.FileName = $"QR_{data}.png";

                        if (saveFile.ShowDialog() == DialogResult.OK)
                        {
                            qrBitmap.Save(saveFile.FileName);
                            MessageBox.Show("Lưu QR Code thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo QR Code: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
