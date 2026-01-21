using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClosedXML.Excel;
using ZXing;
using static QuanLyNhanVien3.F_FormMain;

namespace QuanLyNhanVien3
{
    public partial class F_NhanVien : Form
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
        bool isLoadingNhanVien = false;
        bool isEditingNhanVien = false;

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
                string checkMaNVSql = "SELECT COUNT(*) FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV ";
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
                string checkEmailSql = "SELECT COUNT(*) FROM tblNhanVien_TuanhCD233018 WHERE Email_TuanhCD233018 = @Email ";
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
                string checkMaPBSql = "SELECT COUNT(*) FROM tblPhongBan_ThuanCD233318 WHERE MaPB_ThuanCD233318 = @MaPB ";
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
                string checkMaCVSql = "SELECT COUNT(*) FROM tblChucVu_KhangCD233181 WHERE MaCV_KhangCD233181 = @MaCV ";
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

        private void LoadcomboBox()
        {
            try
            {
                cn.connect();
                string sqlLoadcomboBoxtblPhongBan = "SELECT * FROM tblPhongBan_ThuanCD233318 WHERE DeletedAt_ThuanCD233318 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxtblPhongBan, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxMaPB.DataSource = ds.Tables[0];
                    cbBoxMaPB.DisplayMember = "TenPB_ThuanCD233318";// hien thi
                    cbBoxMaPB.ValueMember = "MaPB_ThuanCD233318"; // cot gia tri
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
                string sqsqlLoadcomboBoxttblChucVu = "SELECT * FROM tblChucVu_KhangCD233181 WHERE DeletedAt_KhangCD233181 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqsqlLoadcomboBoxttblChucVu, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxChucVu.DataSource = ds.Tables[0];
                    cbBoxChucVu.DisplayMember = "TenCV_KhangCD233181"; // cot hien thi
                    cbBoxChucVu.ValueMember = "MaCV_KhangCD233181"; // cot gia tri
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ma CV: " + ex.Message);
            }
        }

        private void NhanVien_Load(object sender, EventArgs e)
        {
            LoadcomboBox();
            loadcbbCV();
            LoadNhanVienTheoDieuKien();
            LoadTongSoNhanVien();
            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
        }
        //tong nhan vien 
        private void LoadTongSoNhanVien()
        {
            try
            {
                cn.connect();

                string sqlTongNV = @"
            SELECT COUNT(*) AS TongNV 
            FROM tblNhanVien_TuanhCD233018
            WHERE DeletedAt_TuanhCD233018 = 0";

                using (SqlDataAdapter da = new SqlDataAdapter(sqlTongNV, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        lblTongNhanVien.Text = "Tổng số nhân viên: " + dt.Rows[0]["TongNV"].ToString();
                    }
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load tổng nhân viên: " + ex.Message);
            }
        }

        //tong nhan vien and
        private void dtGridViewNhanVien_CellClick_2(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                isLoadingNhanVien = true; // 🔒 KHÓA EVENT
                isEditingNhanVien = true; // 🔴 ĐANG SỬA
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
                isLoadingNhanVien = false; // 🔓 MỞ KHÓA
            }
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {

                // Kiểm tra dữ liệu nhập vào
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

                if (!checknhanvien())
                {
                    cn.disconnect();
                    return;
                }
                // Câu lệnh SQL chèn dữ liệu vào bảng tblNhanVien
                string sqltblNhanVien = @"INSERT INTO tblNhanVien_TuanhCD233018 
                           (MaNV_TuanhCD233018, HoTen_TuanhCD233018, NgaySinh_TuanhCD233018, GioiTinh_TuanhCD233018, DiaChi_TuanhCD233018, SoDienThoai_TuanhCD233018, Email_TuanhCD233018, MaCV_KhangCD233181, Ghichu_TuanhCD233018, DeletedAt_TuanhCD233018)
                           VALUES ( @MaNV, @HoTen, @NgaySinh, @GioiTinh, @DiaChi, @SoDienThoai, @Email, @MaCV, @GhiChu, 0)";

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
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue);
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.disconnect();
                        LoadNhanVienTheoDieuKien();
                        ClearAllInputs(this);
                        isEditingNhanVien = false;
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
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã nhân viên cần xóa!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa nhân viên này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm != DialogResult.Yes) return;

                cn.connect();

                // 1️⃣ KIỂM TRA BẢNG LƯƠNG CHƯA HOÀN THÀNH
                string checkLuong = @"
            SELECT COUNT(*) 
            FROM tblLuong_ChienCD232928 
            WHERE ChamCongId_TuanhCD233018 IN (
                SELECT Id_TuanhCD233018 
                FROM tblChamCong_TuanhCD233018 
                WHERE NhanVienId_TuanhCD233018 = (
                    SELECT Id_TuanhCD233018 
                    FROM tblNhanVien_TuanhCD233018 
                    WHERE MaNV_TuanhCD233018 = @MaNV
                )
            ) AND DeletedAt_ChienCD232928 = 0";

                using (SqlCommand cmdCheck = new SqlCommand(checkLuong, cn.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MaNV", tbmaNV.Text);
                    int countLuong = (int)cmdCheck.ExecuteScalar();

                    if (countLuong > 0)
                    {
                        MessageBox.Show(
                            "Chưa hoàn thành bảng lương cho nhân viên, không thể xóa!",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        cn.disconnect();
                        return;
                    }
                }

                // 2️⃣ BẮT ĐẦU XÓA THEO THỨ TỰ
                string[] deleteQueries =
                {
            "DELETE FROM tblChiTietDuAn_KienCD233824 WHERE MaNV_TuanhCD233018 = @MaNV",
            "DELETE FROM tblLuong_ChienCD232928 WHERE ChamCongId_TuanhCD233018 IN (SELECT Id_TuanhCD233018 FROM tblChamCong_TuanhCD233018 WHERE NhanVienId_TuanhCD233018 = (SELECT Id_TuanhCD233018 FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV))",
            "DELETE FROM tblChamCong_TuanhCD233018 WHERE NhanVienId_TuanhCD233018 = (SELECT Id_TuanhCD233018 FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV)",
            "DELETE FROM tblHopDong_ChienCD232928 WHERE MaNV_TuanhCD233018 = @MaNV",
            "DELETE FROM tblTaiKhoan_KhangCD233181 WHERE MaNV_TuanhCD233018 = @MaNV",
            "DELETE FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV"
        };

                foreach (string query in deleteQueries)
                {
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Xóa nhân viên thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cn.disconnect();
                ClearAllInputs(this);
                LoadNhanVienTheoDieuKien();
            }
            catch (Exception ex)
            {
                cn.disconnect();
                MessageBox.Show("Lỗi: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnrestar_Click_1(object sender, EventArgs e)
        {
            LoadNhanVienTheoDieuKien();
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
                string query = @"SELECT  MaNV_TuanhCD233018, HoTen_TuanhCD233018, NgaySinh_TuanhCD233018, GioiTinh_TuanhCD233018, DiaChi_TuanhCD233018, SoDienThoai_TuanhCD233018, Email_TuanhCD233018, MaCV_KhangCD233181, Ghichu_TuanhCD233018
                                FROM tblNhanVien_TuanhCD233018
                                WHERE DeletedAt_TuanhCD233018 = 1 ORDER BY MaNV_TuanhCD233018";
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
        }

        private void checkshowpassword_CheckedChanged_1(object sender, EventArgs e)
        {
        }

        private void btnSua_Click(object sender, EventArgs e)
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
                return;
            }
            else if (tbSoDienThoai.Text.Trim().Length != 10)
            {
                MessageBox.Show("Số điện thoại phải có đúng 10 chữ số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cn.disconnect();
                return;
            }
            if (!tbEmail.Text.Trim().ToLower().EndsWith("@gmail.com"))
            {
                MessageBox.Show("Email phải có đuôi @gmail.com!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cn.disconnect();
                return;
            }

            string checkMaNVSql = "SELECT COUNT(*) FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV AND DeletedAt_TuanhCD233018 = 0";
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
                string sql = @"UPDATE tblNhanVien_TuanhCD233018 SET  
                HoTen_TuanhCD233018 = @HoTen,
                NgaySinh_TuanhCD233018 = @NgaySinh,
                GioiTinh_TuanhCD233018 = @GioiTinh,
                DiaChi_TuanhCD233018 = @DiaChi,
                SoDienThoai_TuanhCD233018 = @SoDienThoai,
                Email_TuanhCD233018 = @Email,
                MaCV_KhangCD233181 = @MaCV,
                Ghichu_TuanhCD233018 = @GhiChu,
                DeletedAt_TuanhCD233018 = 0
            WHERE MaNV_TuanhCD233018 = @MaNV";
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", tbmaNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@HoTen", tbHoTen.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgaySinh", dateTimePickerNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@GioiTinh", cbBoxGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", tbDiaChi.Text.Trim());
                    cmd.Parameters.AddWithValue("@SoDienThoai", tbSoDienThoai.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", tbEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue);
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        isEditingNhanVien = false;
                        LoadNhanVienTheoDieuKien();
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
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
                MessageBox.Show(
                    "Lỗi: " + ex.Message +
                    "\nDòng: " + ex.StackTrace,
                    "Lỗi hệ thống",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

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
                string sql = @"SELECT MaNV_TuanhCD233018, HoTen_TuanhCD233018, NgaySinh_TuanhCD233018, GioiTinh_TuanhCD233018, DiaChi_TuanhCD233018, SoDienThoai_TuanhCD233018, Email_TuanhCD233018, MaCV_KhangCD233181, Ghichu_TuanhCD233018
                                FROM tblNhanVien_TuanhCD233018
                                WHERE DeletedAt_TuanhCD233018 = 0
                                  AND HoTen_TuanhCD233018 LIKE '%' + @TenTimKiem + '%'
                                ORDER BY MaNV_TuanhCD233018";

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
                    }
                };

                // Ghép nhiều thông tin vào chuỗi
                string maNV = tbmaNV.Text.Trim();

                // Sinh QR dựa trên MaNV
                string data = tbmaNV.Text.Trim(); // Dùng mã nhân viên
                Bitmap qrBitmap = writer.Write(data);

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

        // su ly load nhanh vien
        private void LoadNhanVienTheoDieuKien()
        {
            if (cbBoxMaPB.SelectedValue == null ||
                cbBoxMaPB.SelectedValue is DataRowView)
                return;

            cn.connect();
            string sql = @"
                            SELECT 
                                nv.MaNV_TuanhCD233018 AS [Mã nhân viên],
                                nv.HoTen_TuanhCD233018 AS [Họ tên],
                                nv.NgaySinh_TuanhCD233018 AS [Ngày sinh],
                                nv.GioiTinh_TuanhCD233018 AS [Giới tính],
                                nv.DiaChi_TuanhCD233018 AS [Địa chỉ],
                                nv.SoDienThoai_TuanhCD233018 AS [Điện thoại],
                                nv.Email_TuanhCD233018 AS [Email],
                                pb.MaPB_ThuanCD233318 AS [Mã PB],
                                nv.MaCV_KhangCD233181 AS [Mã CV],
                                nv.Ghichu_TuanhCD233018 AS [Ghi chú]
                            FROM tblNhanVien_TuanhCD233018 nv
                            INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                            INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                            WHERE pb.MaPB_ThuanCD233318 = @MaPB
                            AND nv.DeletedAt_TuanhCD233018 = 0
                            AND cv.DeletedAt_KhangCD233181 = 0
                            AND pb.DeletedAt_ThuanCD233318 = 0
                            ";

            // 🔹 LỌC CHỨC VỤ
            if (cbBoxChucVu.SelectedValue != null &&
                !(cbBoxChucVu.SelectedValue is DataRowView))
            {
                sql += " AND nv.MaCV_KhangCD233181 = @MaCV";
            }

            // 🔹 LỌC GIỚI TÍNH
            if (cbBoxGioiTinh.SelectedIndex != -1)
            {
                sql += " AND nv.GioiTinh_TuanhCD233018 = @GioiTinh";
            }

            SqlCommand cmd = new SqlCommand(sql, cn.conn);
            cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue);

            if (cbBoxChucVu.SelectedValue != null &&
                !(cbBoxChucVu.SelectedValue is DataRowView))
            {
                cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue);
            }

            if (cbBoxGioiTinh.SelectedIndex != -1)
            {
                cmd.Parameters.AddWithValue("@GioiTinh", cbBoxGioiTinh.Text);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dtGridViewNhanVien.DataSource = dt;

            cn.disconnect();
        }

        void loadcbbCV()
        {

            if (cbBoxMaPB.SelectedValue == null) return;
            if (cbBoxMaPB.SelectedValue is DataRowView) return;

            string maPB = cbBoxMaPB.SelectedValue.ToString();
            cn.connect();

            string sql = @"SELECT cv.MaCV_KhangCD233181, cv.TenCV_KhangCD233181
                            FROM     tblPhongBan_ThuanCD233318 pb INNER JOIN
                                     tblChucVu_KhangCD233181 cv ON pb.MaPB_ThuanCD233318 = cv.MaPB_ThuanCD233318
                            WHERE  (pb.MaPB_ThuanCD233318 = @MaPB) AND (cv.DeletedAt_KhangCD233181 = 0) AND (pb.DeletedAt_ThuanCD233318 = 0)";

            SqlCommand cmd = new SqlCommand(sql, cn.conn);
            cmd.Parameters.AddWithValue("@MaPB", maPB);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbBoxChucVu.DataSource = null;
            cbBoxChucVu.DataSource = dt;
            cbBoxChucVu.SelectedIndex = -1;
            cbBoxChucVu.ValueMember = "MaCV_KhangCD233181";
            cbBoxChucVu.DisplayMember = "TenCV_KhangCD233181";

            cn.disconnect();
        }
        private void cbBoxMaPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadcbbCV();
            if (isLoadingNhanVien) return; // ❗ CHỐNG LOAD NGƯỢC
            if (isEditingNhanVien) return; // 🔥 CHẶN LỌC KHI ĐANG SỬA
            if (cbBoxMaPB.SelectedValue == null) return;
            if (cbBoxMaPB.SelectedValue is DataRowView) return;
            // 🔥 LOAD NHÂN VIÊN THEO PHÒNG BAN
            LoadNhanVienTheoDieuKien();
        }

        private void cbBoxChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingNhanVien) return;
            if (isEditingNhanVien) return; // 🔥
            if (cbBoxChucVu.SelectedValue == null) return;
            if (cbBoxChucVu.SelectedValue is DataRowView) return;

            LoadNhanVienTheoDieuKien();
        }

        private void cbBoxGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingNhanVien) return;
            if (isEditingNhanVien) return;
            if (cbBoxGioiTinh.SelectedIndex == -1) return;

            LoadNhanVienTheoDieuKien();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnrestar_Click(object sender, EventArgs e)
        {
            LoadNhanVienTheoDieuKien();
            LoadTongSoNhanVien();
            isEditingNhanVien = false;
        }
    }
}