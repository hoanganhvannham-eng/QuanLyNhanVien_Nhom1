using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ClosedXML.Excel;
using ZXing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using static QuanLyNhanVien3.F_FormMain;
using System.IO;
using System.Collections.Generic;


namespace QuanLyNhanVien3
{
    public partial class F_NhanVien : Form
    {
        public F_NhanVien()
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
            // ===== LOAD PHÒNG BAN =====
            try
            {
                cn.connect();

                string sqlPB = "SELECT MaPB_ThuanCD233318, TenPB_ThuanCD233318 FROM tblPhongBan_ThuanCD233318 WHERE DeletedAt_ThuanCD233318 = 0";

                DataTable dtPB = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(sqlPB, cn.conn))
                {
                    da.Fill(dtPB);
                }

                // ⭐ THÊM DÒNG "Tất cả"
                DataRow rowAll = dtPB.NewRow();
                rowAll["MaPB_ThuanCD233318"] = "";
                rowAll["TenPB_ThuanCD233318"] = "-- Tất cả phòng ban --";
                dtPB.Rows.InsertAt(rowAll, 0);

                cbBoxMaPB.DataSource = dtPB;
                cbBoxMaPB.DisplayMember = "TenPB_ThuanCD233318";
                cbBoxMaPB.ValueMember = "MaPB_ThuanCD233318";

                cn.disconnect();
            }
            catch (Exception ex)
            {
                cn.disconnect();
                MessageBox.Show("Lỗi load phòng ban: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ===== LOAD CHỨC VỤ =====
            try
            {
                cn.connect();

                string sqlCV = "SELECT MaCV_KhangCD233181, TenCV_KhangCD233181 FROM tblChucVu_KhangCD233181 WHERE DeletedAt_KhangCD233181 = 0";

                DataTable dtCV = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(sqlCV, cn.conn))
                {
                    da.Fill(dtCV);
                }

                // ⭐ THÊM DÒNG "Tất cả"
                DataRow rowAll = dtCV.NewRow();
                rowAll["MaCV_KhangCD233181"] = "";
                rowAll["TenCV_KhangCD233181"] = "-- Tất cả chức vụ --";
                dtCV.Rows.InsertAt(rowAll, 0);

                cbBoxChucVu.DataSource = dtCV;
                cbBoxChucVu.DisplayMember = "TenCV_KhangCD233181";
                cbBoxChucVu.ValueMember = "MaCV_KhangCD233181";

                cn.disconnect();
            }
            catch (Exception ex)
            {
                cn.disconnect();
                MessageBox.Show("Lỗi load chức vụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void NhanVien_Load(object sender, EventArgs e)
        {
            // ⭐ TẮT SỰ KIỆN TẠM THỜI
            cbBoxMaPB.SelectedIndexChanged -= cbBoxMaPB_SelectedIndexChanged_1;
            cbBoxChucVu.SelectedIndexChanged -= cbBoxChucVu_SelectedIndexChanged;

            LoadcomboBox(); // Load phòng ban và chức vụ

            // ⭐ CHỌN MẶC ĐỊNH "Tất cả phòng ban"
            if (cbBoxMaPB.Items.Count > 0)
            {
                cbBoxMaPB.SelectedIndex = 0; // Chọn "-- Tất cả phòng ban --"
            }

            // ⭐ CHỌN MẶC ĐỊNH "Tất cả chức vụ"
            if (cbBoxChucVu.Items.Count > 0)
            {
                cbBoxChucVu.SelectedIndex = 0; // Chọn "-- Tất cả chức vụ --"
            }

            // ⭐ BẬT LẠI SỰ KIỆN
            cbBoxMaPB.SelectedIndexChanged += cbBoxMaPB_SelectedIndexChanged_1;
            cbBoxChucVu.SelectedIndexChanged += cbBoxChucVu_SelectedIndexChanged;

            dtGridViewNhanVien.RowPostPaint += dtGridViewNhanVien_RowPostPaint;

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
        // ===== 4. SỬA PHẦN dtGridViewNhanVien_CellClick_2 - CẬP NHẬT INDEX CỘT =====
        private void dtGridViewNhanVien_CellClick_2(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                isLoadingNhanVien = true;
                isEditingNhanVien = true;

                try
                {
                    // ✅ KIỂM TRA CỘT "Mã phòng ban" CÓ TỒN TẠI KHÔNG (tức là đang ở chế độ tìm kiếm)
                    bool isSearchMode = dtGridViewNhanVien.Columns.Contains("Mã phòng ban");

                    if (isSearchMode)
                    {
                        // ===== CHẾ ĐỘ TÌM KIẾM (có cột "Mã phòng ban", "Mã chức vụ") =====
                        tbmaNV.Text = dtGridViewNhanVien.Rows[i].Cells["Mã nhân viên"].Value.ToString();
                        tbHoTen.Text = dtGridViewNhanVien.Rows[i].Cells["Họ tên"].Value.ToString();
                        dateTimePickerNgaySinh.Value = Convert.ToDateTime(dtGridViewNhanVien.Rows[i].Cells["Ngày sinh"].Value);
                        cbBoxGioiTinh.Text = dtGridViewNhanVien.Rows[i].Cells["Giới tính"].Value.ToString();
                        tbDiaChi.Text = dtGridViewNhanVien.Rows[i].Cells["Địa chỉ"].Value.ToString();
                        tbSoDienThoai.Text = dtGridViewNhanVien.Rows[i].Cells["Điện thoại"].Value.ToString();
                        tbEmail.Text = dtGridViewNhanVien.Rows[i].Cells["Email"].Value.ToString();

                        // ✅ LẤY MÃ TỪ CỘT "Mã phòng ban"
                        string maPB = dtGridViewNhanVien.Rows[i].Cells["Mã phòng ban"].Value.ToString();
                        cbBoxMaPB.SelectedValue = maPB;

                        // ✅ Load lại chức vụ theo phòng ban
                        loadcbbCV();

                        string maCV = dtGridViewNhanVien.Rows[i].Cells["Mã chức vụ"].Value.ToString();
                        cbBoxChucVu.SelectedValue = maCV;

                        tbGhiChu.Text = dtGridViewNhanVien.Rows[i].Cells["Ghi chú"].Value.ToString();
                    }
                    else
                    {
                        // ===== CHẾ ĐỘ BÌNH THƯỜNG (cột 0 là STT, dữ liệu từ cột 1) =====
                        tbmaNV.Text = dtGridViewNhanVien.Rows[i].Cells[1].Value.ToString();
                        tbHoTen.Text = dtGridViewNhanVien.Rows[i].Cells[2].Value.ToString();
                        dateTimePickerNgaySinh.Value = Convert.ToDateTime(dtGridViewNhanVien.Rows[i].Cells[3].Value);
                        cbBoxGioiTinh.Text = dtGridViewNhanVien.Rows[i].Cells[4].Value.ToString();
                        tbDiaChi.Text = dtGridViewNhanVien.Rows[i].Cells[5].Value.ToString();
                        tbSoDienThoai.Text = dtGridViewNhanVien.Rows[i].Cells[6].Value.ToString();
                        tbEmail.Text = dtGridViewNhanVien.Rows[i].Cells[7].Value.ToString();

                        string maPB = dtGridViewNhanVien.Rows[i].Cells[8].Value.ToString();
                        cbBoxMaPB.SelectedValue = maPB;

                        loadcbbCV();

                        string maCV = dtGridViewNhanVien.Rows[i].Cells[9].Value.ToString();
                        cbBoxChucVu.SelectedValue = maCV;

                        tbGhiChu.Text = dtGridViewNhanVien.Rows[i].Cells[10].Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    isLoadingNhanVien = false;
                }
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

        // ==================== XUẤT EXCEL - ĐÃ GỌN GỌN ====================
        private void xuatexcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewNhanVien.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = $"DanhSachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("NhanVien");
                        int totalColumns = dtGridViewNhanVien.Columns.Count;

                        // ===== PHẦN HEADER =====
                        ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                        ws.Range(1, 1, 1, totalColumns).Merge().Style.Font.Bold = true;
                        ws.Cell(1, 1).Style.Font.FontSize = 14;
                        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Row(1).Height = 20;

                        ws.Cell(2, 1).Value = "DANH SÁCH NHÂN VIÊN";
                        ws.Range(2, 1, 2, totalColumns).Merge().Style.Font.Bold = true;
                        ws.Cell(2, 1).Style.Font.FontSize = 16;
                        ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Row(2).Height = 25;

                        ws.Cell(3, 1).Value = $"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                        ws.Cell(3, 1).Style.Font.Italic = true;

                        // ===== THÔNG TIN LỌC =====
                        ws.Cell(5, 1).Value = "Phòng Ban:";
                        ws.Cell(5, 2).Value = cbBoxMaPB.Text;
                        ws.Cell(5, 1).Style.Font.Bold = true;

                        ws.Cell(6, 1).Value = "Chức vụ:";
                        ws.Cell(6, 2).Value = string.IsNullOrEmpty(cbBoxChucVu.Text) ? "Tất cả" : cbBoxChucVu.Text;
                        ws.Cell(6, 1).Style.Font.Bold = true;

                        // ===== BẢNG DỮ LIỆU =====
                        int startRow = 8;
                        ws.Cell(startRow, 1).Value = "DANH SÁCH NHÂN VIÊN";
                        ws.Range(startRow, 1, startRow, totalColumns).Merge();
                        ws.Cell(startRow, 1).Style.Font.Bold = true;
                        ws.Cell(startRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Cell(startRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                        // Tiêu đề cột
                        int headerRow = startRow + 1;
                        for (int i = 0; i < totalColumns; i++)
                        {
                            var cell = ws.Cell(headerRow, i + 1);
                            cell.Value = dtGridViewNhanVien.Columns[i].HeaderText;
                            cell.Style.Font.Bold = true;
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                        }

                        // Dữ liệu
                        int dataStartRow = headerRow + 1;
                        for (int i = 0; i < dtGridViewNhanVien.Rows.Count; i++)
                        {
                            for (int j = 0; j < totalColumns; j++)
                            {
                                var cellValue = dtGridViewNhanVien.Rows[i].Cells[j].Value;
                                var cell = ws.Cell(dataStartRow + i, j + 1);
                                cell.Value = cellValue is DateTime dt ? dt.ToString("dd/MM/yyyy") : cellValue?.ToString();
                                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            }
                        }

                        // Border
                        int lastDataRow = dataStartRow + dtGridViewNhanVien.Rows.Count - 1;
                        ws.Range(startRow, 1, lastDataRow, totalColumns).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        ws.Range(startRow, 1, lastDataRow, totalColumns).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                        // ===== CHỮ KÝ =====
                        int signatureRow = lastDataRow + 2;
                        int signatureStartCol = Math.Max(1, totalColumns - 2);

                        ws.Cell(signatureRow, signatureStartCol).Value = $"Hà Nội, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
                        ws.Range(signatureRow, signatureStartCol, signatureRow, totalColumns).Merge();
                        ws.Cell(signatureRow, signatureStartCol).Style.Font.Italic = true;
                        ws.Cell(signatureRow, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Cell(signatureRow + 1, signatureStartCol).Value = "Người lập báo cáo";
                        ws.Range(signatureRow + 1, signatureStartCol, signatureRow + 1, totalColumns).Merge();
                        ws.Cell(signatureRow + 1, signatureStartCol).Style.Font.Bold = true;
                        ws.Cell(signatureRow + 1, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ws.Cell(signatureRow + 4, signatureStartCol).Value = nguoiDangNhap;
                        ws.Range(signatureRow + 4, signatureStartCol, signatureRow + 4, totalColumns).Merge();
                        ws.Cell(signatureRow + 4, signatureStartCol).Style.Font.Bold = true;
                        ws.Cell(signatureRow + 4, signatureStartCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        // ===== TỰ ĐỘNG ĐIỀU CHỈNH =====
                        // Cột STT: fix cứng vừa đủ
                        if (totalColumns > 0 && dtGridViewNhanVien.Columns[0].Name == "STT")
                        {
                            ws.Column(1).Width = 12; // Vừa đủ cho 2-3 chữ số
                        }

                        // Các cột khác: tự động nhưng có giới hạn
                        for (int i = 2; i <= totalColumns; i++)
                        {
                            ws.Column(i).AdjustToContents();

                            // Đặt độ rộng tối thiểu và tối đa
                            if (ws.Column(i).Width < 12)
                                ws.Column(i).Width = 12;
                            else if (ws.Column(i).Width > 40)
                                ws.Column(i).Width = 40; // Tránh quá rộng
                        }

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // ===== 6. SỬA PHẦN btnNVDaNghiViec_Click_1 - THÊM STT =====
        private void btnNVDaNghiViec_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @"SELECT MaNV_TuanhCD233018, HoTen_TuanhCD233018, NgaySinh_TuanhCD233018, 
                                GioiTinh_TuanhCD233018, DiaChi_TuanhCD233018, SoDienThoai_TuanhCD233018, 
                                Email_TuanhCD233018, MaCV_KhangCD233181, Ghichu_TuanhCD233018
                        FROM tblNhanVien_TuanhCD233018
                        WHERE DeletedAt_TuanhCD233018 = 1 
                        ORDER BY MaNV_TuanhCD233018";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
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

                    dtGridViewNhanVien.DataSource = dt;

                    // Tùy chỉnh cột STT
                    if (dtGridViewNhanVien.Columns["STT"] != null)
                    {
                        dtGridViewNhanVien.Columns["STT"].HeaderText = "STT";
                        dtGridViewNhanVien.Columns["STT"].Width = 50;
                        dtGridViewNhanVien.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
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

        // ===== 5. SỬA PHẦN btnTimKiem_Click - THÊM STT =====
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                // ⭐ LẤY KEYWORD
                string maNV = tbmaNV.Text.Trim();
                string hoTen = tbHoTen.Text.Trim();

                // ⭐ KIỂM TRA NHẬP ÍT NHẤT 1 TRƯỜNG
                if (string.IsNullOrWhiteSpace(maNV) && string.IsNullOrWhiteSpace(hoTen))
                {
                    MessageBox.Show("Vui lòng nhập Mã NV hoặc Tên nhân viên để tìm kiếm!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return;
                }

                // ⭐ CÂU TRUY VẤN - HIỂN THỊ CẢ MÃ VÀ TÊN
                string sql = @"
        SELECT 
            ROW_NUMBER() OVER (ORDER BY nv.MaNV_TuanhCD233018) AS [STT],
            nv.MaNV_TuanhCD233018 AS [Mã nhân viên],
            nv.HoTen_TuanhCD233018 AS [Họ tên],
            nv.NgaySinh_TuanhCD233018 AS [Ngày sinh],
            nv.GioiTinh_TuanhCD233018 AS [Giới tính],
            nv.DiaChi_TuanhCD233018 AS [Địa chỉ],
            nv.SoDienThoai_TuanhCD233018 AS [Điện thoại],
            nv.Email_TuanhCD233018 AS [Email],
            pb.MaPB_ThuanCD233318 AS [Mã phòng ban],        -- ✅ HIỂN THỊ MÃ PB
            pb.TenPB_ThuanCD233318 AS [Tên phòng ban],      -- ✅ HIỂN THỊ TÊN PB
            nv.MaCV_KhangCD233181 AS [Mã chức vụ],          -- ✅ HIỂN THỊ MÃ CV
            cv.TenCV_KhangCD233181 AS [Tên chức vụ],        -- ✅ HIỂN THỊ TÊN CV
            nv.Ghichu_TuanhCD233018 AS [Ghi chú]
        FROM tblNhanVien_TuanhCD233018 nv
        INNER JOIN tblChucVu_KhangCD233181 cv 
            ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
        INNER JOIN tblPhongBan_ThuanCD233318 pb 
            ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
        WHERE nv.DeletedAt_TuanhCD233018 = 0
            AND cv.DeletedAt_KhangCD233181 = 0
            AND pb.DeletedAt_ThuanCD233318 = 0
        ";

                SqlCommand cmd = new SqlCommand(sql, cn.conn);
                List<string> conditions = new List<string>();

                // ⭐ TÌM THEO MÃ NV / HỌ TÊN
                if (!string.IsNullOrWhiteSpace(maNV))
                {
                    conditions.Add("nv.MaNV_TuanhCD233018 LIKE @MaNV");
                    cmd.Parameters.AddWithValue("@MaNV", "%" + maNV + "%");
                }

                if (!string.IsNullOrWhiteSpace(hoTen))
                {
                    conditions.Add("nv.HoTen_TuanhCD233018 COLLATE Vietnamese_CI_AI LIKE @HoTen");
                    cmd.Parameters.AddWithValue("@HoTen", "%" + hoTen + "%");
                }

                if (conditions.Count > 0)
                {
                    sql += " AND (" + string.Join(" OR ", conditions) + ")";
                }

                // ⭐ LỌC PHÒNG BAN (NẾU KHÔNG PHẢI "Tất cả")
                if (cbBoxMaPB.SelectedValue != null &&
                    !(cbBoxMaPB.SelectedValue is DataRowView) &&
                    !string.IsNullOrEmpty(cbBoxMaPB.SelectedValue.ToString()))
                {
                    sql += " AND pb.MaPB_ThuanCD233318 = @MaPB";
                    cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue.ToString());
                }

                // ⭐ LỌC CHỨC VỤ (NẾU KHÔNG PHẢI "Tất cả")
                if (cbBoxChucVu.SelectedValue != null &&
                    !(cbBoxChucVu.SelectedValue is DataRowView) &&
                    !string.IsNullOrEmpty(cbBoxChucVu.SelectedValue.ToString()))
                {
                    sql += " AND cv.MaCV_KhangCD233181 = @MaCV";
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue.ToString());
                }

                // ⭐ LỌC GIỚI TÍNH
                if (cbBoxGioiTinh.SelectedIndex != -1)
                {
                    sql += " AND nv.GioiTinh_TuanhCD233018 = @GioiTinh";
                    cmd.Parameters.AddWithValue("@GioiTinh", cbBoxGioiTinh.Text);
                }

                sql += " ORDER BY nv.MaNV_TuanhCD233018";
                cmd.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtGridViewNhanVien.DataSource = dt;

                // ⭐ TÙY CHỈNH CỘT STT
                if (dtGridViewNhanVien.Columns["STT"] != null)
                {
                    dtGridViewNhanVien.Columns["STT"].HeaderText = "STT";
                    dtGridViewNhanVien.Columns["STT"].Width = 50;
                    dtGridViewNhanVien.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                // ⭐ TÙY CHỈNH ĐỘ RỘNG CỘT (TÙY CHỌN)
                if (dtGridViewNhanVien.Columns["Mã phòng ban"] != null)
                    dtGridViewNhanVien.Columns["Mã phòng ban"].Width = 80;

                if (dtGridViewNhanVien.Columns["Tên phòng ban"] != null)
                    dtGridViewNhanVien.Columns["Tên phòng ban"].Width = 150;

                if (dtGridViewNhanVien.Columns["Mã chức vụ"] != null)
                    dtGridViewNhanVien.Columns["Mã chức vụ"].Width = 80;

                if (dtGridViewNhanVien.Columns["Tên chức vụ"] != null)
                    dtGridViewNhanVien.Columns["Tên chức vụ"].Width = 150;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu phù hợp!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Tìm thấy {dt.Rows.Count} nhân viên!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
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
        // ===== 2. SỬA PHẦN LoadNhanVienTheoDieuKien - THÊM CỘT STT =====
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

            // 🔹 LỌC CHỨC VỤ (CHỈ KHI KHÔNG PHẢI "Tất cả")
            if (cbBoxChucVu.SelectedValue != null &&
                !(cbBoxChucVu.SelectedValue is DataRowView) &&
                !string.IsNullOrEmpty(cbBoxChucVu.SelectedValue.ToString()))
            {
                sql += " AND nv.MaCV_KhangCD233181 = @MaCV";
            }

            SqlCommand cmd = new SqlCommand(sql, cn.conn);
            cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue);

            if (cbBoxChucVu.SelectedValue != null &&
                !(cbBoxChucVu.SelectedValue is DataRowView) &&
                !string.IsNullOrEmpty(cbBoxChucVu.SelectedValue.ToString()))
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

            // ===== THÊM CỘT STT =====
            dt.Columns.Add("STT", typeof(int));
            dt.Columns["STT"].SetOrdinal(0); // Đặt cột STT ở vị trí đầu tiên

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["STT"] = i + 1;
            }

            dtGridViewNhanVien.DataSource = dt;

            // Tùy chỉnh cột STT
            if (dtGridViewNhanVien.Columns["STT"] != null)
            {
                dtGridViewNhanVien.Columns["STT"].HeaderText = "STT";
                dtGridViewNhanVien.Columns["STT"].Width = 50;
                dtGridViewNhanVien.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtGridViewNhanVien.Columns["STT"].ReadOnly = true;
            }

            cn.disconnect();
        }
        //

        // ===== 3. SỬA PHẦN LoadNhanVienTheoDieuKiengioitinh - THÊM CỘT STT =====
        private void LoadNhanVienTheoDieuKiengioitinh()
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

            // 🔹 LỌC CHỨC VỤ (CHỈ KHI KHÔNG PHẢI "Tất cả")
            if (cbBoxChucVu.SelectedValue != null &&
                !(cbBoxChucVu.SelectedValue is DataRowView) &&
                !string.IsNullOrEmpty(cbBoxChucVu.SelectedValue.ToString()))
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
                !(cbBoxChucVu.SelectedValue is DataRowView) &&
                !string.IsNullOrEmpty(cbBoxChucVu.SelectedValue.ToString()))
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

            // ===== THÊM CỘT STT =====
            dt.Columns.Add("STT", typeof(int));
            dt.Columns["STT"].SetOrdinal(0);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["STT"] = i + 1;
            }

            dtGridViewNhanVien.DataSource = dt;

            // Tùy chỉnh cột STT
            if (dtGridViewNhanVien.Columns["STT"] != null)
            {
                dtGridViewNhanVien.Columns["STT"].HeaderText = "STT";
                dtGridViewNhanVien.Columns["STT"].Width = 50;
                dtGridViewNhanVien.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dtGridViewNhanVien.Columns["STT"].ReadOnly = true;
            }

            cn.disconnect();
        }
        void loadcbbCV()
        {
            if (cbBoxMaPB.SelectedValue == null) return;
            if (cbBoxMaPB.SelectedValue is DataRowView) return;

            try
            {
                cn.connect();

                string maPB = cbBoxMaPB.SelectedValue.ToString();

                // ⭐ NẾU CHỌN "Tất cả phòng ban" (chuỗi rỗng)
                if (string.IsNullOrEmpty(maPB))
                {
                    string sqlAllCV = "SELECT MaCV_KhangCD233181, TenCV_KhangCD233181 FROM tblChucVu_KhangCD233181 WHERE DeletedAt_KhangCD233181 = 0";

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(sqlAllCV, cn.conn))
                    {
                        da.Fill(dt);
                    }

                    // Thêm "Tất cả"
                    DataRow newRow = dt.NewRow();
                    newRow["MaCV_KhangCD233181"] = "";  // ✅ QUAN TRỌNG: chuỗi rỗng, không phải DBNull
                    newRow["TenCV_KhangCD233181"] = "-- Tất cả chức vụ --";
                    dt.Rows.InsertAt(newRow, 0);

                    cbBoxChucVu.DataSource = null; // ⭐ QUAN TRỌNG: Reset trước
                    cbBoxChucVu.DataSource = dt;
                    cbBoxChucVu.ValueMember = "MaCV_KhangCD233181";
                    cbBoxChucVu.DisplayMember = "TenCV_KhangCD233181";
                    cbBoxChucVu.SelectedIndex = 0; // ⭐ Chọn "Tất cả"

                    cn.disconnect();
                    return;
                }

                // ⭐ NẾU CHỌN PHÒNG BAN CỤ THỂ
                string sql = @"SELECT cv.MaCV_KhangCD233181, cv.TenCV_KhangCD233181
               FROM tblPhongBan_ThuanCD233318 pb 
               INNER JOIN tblChucVu_KhangCD233181 cv 
                   ON pb.MaPB_ThuanCD233318 = cv.MaPB_ThuanCD233318
               WHERE pb.MaPB_ThuanCD233318 = @MaPB 
                   AND cv.DeletedAt_KhangCD233181 = 0 
                   AND pb.DeletedAt_ThuanCD233318 = 0";

                SqlCommand cmd = new SqlCommand(sql, cn.conn);
                cmd.Parameters.AddWithValue("@MaPB", maPB);

                DataTable dtCV = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtCV);
                }

                // Thêm "Tất cả"
                DataRow rowAll = dtCV.NewRow();
                rowAll["MaCV_KhangCD233181"] = "";  // ✅ QUAN TRỌNG: chuỗi rỗng
                rowAll["TenCV_KhangCD233181"] = "-- Tất cả chức vụ --";
                dtCV.Rows.InsertAt(rowAll, 0);

                cbBoxChucVu.DataSource = null; // ⭐ QUAN TRỌNG: Reset trước
                cbBoxChucVu.DataSource = dtCV;
                cbBoxChucVu.ValueMember = "MaCV_KhangCD233181";
                cbBoxChucVu.DisplayMember = "TenCV_KhangCD233181";
                cbBoxChucVu.SelectedIndex = 0; // ⭐ Chọn "Tất cả"

                cn.disconnect();
            }
            catch (Exception ex)
            {
                cn.disconnect();
                MessageBox.Show("Lỗi load chức vụ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            LoadNhanVienTheoDieuKiengioitinh();
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

        private void btnxuatexcel_Click(object sender, EventArgs e)
        {

            if (dtGridViewNhanVien.Rows.Count > 0)
            {
                // Tạo tên file tự động
                string fileName = $"DánhachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    sfd.FileName = fileName;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("NhanVien");

                                // ===== TÊN CÔNG TY =====
                                ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                                ws.Range(1, 1, 1, 10).Merge();
                                ws.Cell(1, 1).Style.Font.Bold = true;
                                ws.Cell(1, 1).Style.Font.FontSize = 14;
                                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                // ===== TIÊU ĐỀ CHÍNH =====
                                ws.Cell(2, 1).Value = "DANH SÁCH NHÂN VIÊN";
                                ws.Range(2, 1, 2, 10).Merge();
                                ws.Cell(2, 1).Style.Font.Bold = true;
                                ws.Cell(2, 1).Style.Font.FontSize = 16;
                                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                // ===== NGÀY XUẤT (THỜI GIAN HIỆN TẠI) =====
                                ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                ws.Cell(3, 1).Style.Font.Italic = true;

                                // ===== THÔNG TIN PHÒNG BAN & CHỨC VỤ =====
                                ws.Cell(5, 1).Value = "Phòng Ban";
                                ws.Cell(5, 2).Value = cbBoxMaPB.Text;
                                ws.Cell(6, 1).Value = "Chức vụ";
                                ws.Cell(6, 2).Value = cbBoxChucVu.Text != "" ? cbBoxChucVu.Text : "Tất cả";

                                ws.Cell(5, 1).Style.Font.Bold = true;
                                ws.Cell(6, 1).Style.Font.Bold = true;

                                // ===== HEADER BẢNG DỮ LIỆU =====
                                int startRow = 8;
                                ws.Cell(startRow, 1).Value = "DANH SÁCH NHÂN VIÊN";
                                ws.Range(startRow, 1, startRow, 10).Merge();
                                ws.Cell(startRow, 1).Style.Font.Bold = true;
                                ws.Cell(startRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(startRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                                // ===== TIÊU ĐỀ CỘT =====
                                int headerRow = startRow + 1;
                                for (int i = 0; i < dtGridViewNhanVien.Columns.Count; i++)
                                {
                                    ws.Cell(headerRow, i + 1).Value = dtGridViewNhanVien.Columns[i].HeaderText;
                                    ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                                    ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }

                                // ===== GHI DỮ LIỆU =====
                                int dataStartRow = headerRow + 1;
                                for (int i = 0; i < dtGridViewNhanVien.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewNhanVien.Columns.Count; j++)
                                    {
                                        var cellValue = dtGridViewNhanVien.Rows[i].Cells[j].Value;

                                        // Xử lý DateTime
                                        if (cellValue is DateTime)
                                        {
                                            ws.Cell(dataStartRow + i, j + 1).Value = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                                        }
                                        else
                                        {
                                            ws.Cell(dataStartRow + i, j + 1).Value = cellValue?.ToString();
                                        }

                                        ws.Cell(dataStartRow + i, j + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    }
                                }

                                // ===== BORDER CHO BẢNG DỮ LIỆU =====
                                int lastDataRow = dataStartRow + dtGridViewNhanVien.Rows.Count - 1;
                                var tableRange = ws.Range(startRow, 1, lastDataRow, dtGridViewNhanVien.Columns.Count);
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

                                ws.Cell(signatureRow + 4, 8).Value = nguoiDangNhap;
                                ws.Cell(signatureRow + 4, 8).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 4, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 4, 8, signatureRow + 4, 10).Merge();

                                // ===== TỰ ĐỘNG ĐIỀU CHỈNH CỘT =====
                                ws.Columns().AdjustToContents();

                                // Đặt chiều rộng tối thiểu cho các cột
                                for (int i = 1; i <= dtGridViewNhanVien.Columns.Count; i++)
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

                            System.Diagnostics.Process.Start(sfd.FileName);
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

        private void dtGridViewNhanVien_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Vẽ số thứ tự ở đầu mỗi row

            DataGridView grid = sender as DataGridView;
            if (grid == null) return;

            string rowNumber = (e.RowIndex + 1).ToString();

            using (StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                System.Drawing.Rectangle headerBounds =
                    new System.Drawing.Rectangle(
                        e.RowBounds.Left,
                        e.RowBounds.Top,
                        grid.RowHeadersWidth,
                        e.RowBounds.Height
                    );

                e.Graphics.DrawString(
                    rowNumber,
                    grid.Font,
                    SystemBrushes.ControlText,
                    headerBounds,
                    format
                );
            }
        }
        // HÀM XUẤT PDF - PHIÊN BẢN ĐƠN GIẢN, TRÁNH LỖI SMARTMODE
        // Thay thế hàm xuatpdf_Click hiện tại bằng code này

        // QUAN TRỌNG: Thêm using này vào đầu file
        // using iText.Kernel.Pdf.Canvas;
        // HÀM XUẤT PDF - iText7 KHÔNG BỊ LỖI
        // Thay thế hàm xuatpdf_Click bằng code này
        // BẠN VẪN DÙNG iText7 BÌNH THƯỜNG!

        private void xuatpdf_Click(object sender, EventArgs e)
        {


            if (dtGridViewNhanVien.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF File|*.pdf",
                FileName = $"DanhSachNhanVien_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
                    PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                    doc.Open();

                    // ===== LOAD FONT =====
                    BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    BaseFont baseFontBold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    BaseFont baseFontItalic = BaseFont.CreateFont(@"C:\Windows\Fonts\ariali.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                    iTextSharp.text.Font fontBold = new iTextSharp.text.Font(baseFontBold, 13);
                    iTextSharp.text.Font fontBold15 = new iTextSharp.text.Font(baseFontBold, 15);
                    iTextSharp.text.Font fontBold10 = new iTextSharp.text.Font(baseFontBold, 10);
                    iTextSharp.text.Font fontBold8 = new iTextSharp.text.Font(baseFontBold, 8);
                    iTextSharp.text.Font fontItalic = new iTextSharp.text.Font(baseFontItalic, 10);
                    iTextSharp.text.Font fontSmall = new iTextSharp.text.Font(baseFont, 7.5f);

                    // ===== HEADER =====
                    doc.Add(new Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontBold)
                    { Alignment = Element.ALIGN_CENTER });

                    doc.Add(new Paragraph("DANH SÁCH NHÂN VIÊN", fontBold15)
                    { Alignment = Element.ALIGN_CENTER, SpacingBefore = 5 });

                    doc.Add(new Paragraph($"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", fontItalic)
                    { SpacingAfter = 10 });

                    // ===== THÔNG TIN LỌC =====
                    doc.Add(new Paragraph($"Phòng Ban: {cbBoxMaPB.Text}", fontBold10));
                    doc.Add(new Paragraph($"Chức vụ: {(string.IsNullOrEmpty(cbBoxChucVu.Text) ? "Tất cả" : cbBoxChucVu.Text)}", fontBold10)
                    { SpacingAfter = 10 });

                    // ===== BẢNG DỮ LIỆU =====
                    int columnCount = dtGridViewNhanVien.Columns.Count;
                    PdfPTable table = new PdfPTable(columnCount) { WidthPercentage = 100 };

                    // Độ rộng cột
                    float[] columnWidths = new float[columnCount];
                    for (int i = 0; i < columnCount; i++)
                        columnWidths[i] = (i == 0 && dtGridViewNhanVien.Columns[i].Name == "STT") ? 1f : 2f;
                    table.SetWidths(columnWidths);

                    // Header bảng
                    for (int i = 0; i < columnCount; i++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dtGridViewNhanVien.Columns[i].HeaderText, fontBold8))
                        {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 3
                        };
                        table.AddCell(cell);
                    }

                    // Dữ liệu
                    for (int i = 0; i < dtGridViewNhanVien.Rows.Count; i++)
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            var cellValue = dtGridViewNhanVien.Rows[i].Cells[j].Value;
                            string displayValue = cellValue is DateTime dt ? dt.ToString("dd/MM/yyyy") : cellValue?.ToString() ?? "";

                            PdfPCell cell = new PdfPCell(new Phrase(displayValue, fontSmall))
                            {
                                HorizontalAlignment = (j == 0 && dtGridViewNhanVien.Columns[j].Name == "STT")
                                    ? Element.ALIGN_CENTER : Element.ALIGN_LEFT,
                                VerticalAlignment = Element.ALIGN_MIDDLE,
                                Padding = 2
                            };
                            table.AddCell(cell);
                        }
                    }

                    doc.Add(table);

                    // ===== CHỮ KÝ =====
                    doc.Add(new Paragraph("\n"));
                    PdfPTable signatureTable = new PdfPTable(2) { WidthPercentage = 100 };

                    signatureTable.AddCell(new PdfPCell(new Phrase("")) { Border = iTextSharp.text.Rectangle.NO_BORDER });

                    PdfPCell rightCell = new PdfPCell() { Border = iTextSharp.text.Rectangle.NO_BORDER };
                    rightCell.AddElement(new Paragraph(
                        $"Hà Nội, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}",
                        fontItalic)
                    { Alignment = Element.ALIGN_CENTER });
                    rightCell.AddElement(new Paragraph("Người lập báo cáo", fontBold10)
                    { Alignment = Element.ALIGN_CENTER, SpacingBefore = 5 });
                    rightCell.AddElement(new Paragraph(nguoiDangNhap, fontBold10)
                    { Alignment = Element.ALIGN_CENTER, SpacingBefore = 30 });
                    signatureTable.AddCell(rightCell);

                    doc.Add(signatureTable);
                    doc.Close();

                    MessageBox.Show("Xuất PDF thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbBoxMaPB_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (isLoadingNhanVien) return;
            if (isEditingNhanVien) return;
            if (cbBoxMaPB.SelectedValue == null) return;
            if (cbBoxMaPB.SelectedValue is DataRowView) return;

            // ⭐ TẮT SỰ KIỆN CHỨC VỤ TẠM THỜI
            cbBoxChucVu.SelectedIndexChanged -= cbBoxChucVu_SelectedIndexChanged;

            // ⭐ LOAD LẠI CHỨC VỤ THEO PHÒNG BAN
            loadcbbCV();

            // ⭐ BẬT LẠI SỰ KIỆN CHỨC VỤ
            cbBoxChucVu.SelectedIndexChanged += cbBoxChucVu_SelectedIndexChanged;

            // ⭐ LOAD LẠI NHÂN VIÊN
            LoadNhanVienTheoDieuKien();
        }
    }
}