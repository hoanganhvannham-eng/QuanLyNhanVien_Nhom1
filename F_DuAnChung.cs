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
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using System.IO;
using iTextPdf = iTextSharp.text;

namespace QuanLyNhanVien3
{
    public partial class F_DuAnChung : Form
    {
        public F_DuAnChung()
        {
            InitializeComponent();
        }

        connectData cn = new connectData();
        string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName;

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

        private void LoadDataDuAn()
        {
            try
            {
                cn.connect();

                string sqlLoadDataDuAn = @"
                    SELECT 
                        MaDA_KienCD233824 as 'Mã dự án', 
                        TenDA_KienCD233824 as 'Tên dự án', 
                        MoTa_KienCD233824 as 'Mô tả', 
                        NgayBatDau_KienCD233824 as 'Ngày bắt đầu', 
                        NgayKetThuc_KienCD233824 as 'Ngày kết thúc', 
                        Ghichu_KienCD233824 as 'Ghi chú' 
                    FROM tblDuAn_KienCD233824 
                    WHERE DeletedAt_KienCD233824 = 0 
                    ORDER BY MaDA_KienCD233824";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataDuAn, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvDA.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Dự Án: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void LoadDataChiTietDuAn()
        {
            try
            {
                cn.connect();

                string sqlLoadDataChiTietDuAn = @"
                    SELECT 
                        ct.MaNV_TuanhCD233018 as 'Mã nhân viên',
                        nv.HoTen_TuanhCD233018 as 'Tên nhân viên',
                        ct.MaDA_KienCD233824 as 'Mã dự án', 
                        ct.VaiTro_KienCD233824 as 'Vai trò', 
                        ct.Ghichu_KienCD233824 as 'Ghi chú' 
                    FROM tblChiTietDuAn_KienCD233824 ct
                    LEFT JOIN tblNhanVien_TuanhCD233018 nv ON ct.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                    WHERE ct.DeletedAt_KienCD233824 = 0 
                    ORDER BY ct.MaNV_TuanhCD233018";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataChiTietDuAn, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvChiTietDA.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Chi Tiết Dự Án: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void LoadComboBoxMaNV()
        {
            try
            {
                cn.connect();
                string sql = @"
                    SELECT MaNV_TuanhCD233018, HoTen_TuanhCD233018
                    FROM tblNhanVien_TuanhCD233018 
                    WHERE DeletedAt_TuanhCD233018 = 0
                    ORDER BY MaNV_TuanhCD233018";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbMaNV.DataSource = dt;
                    cbMaNV.DisplayMember = "MaNV_TuanhCD233018";
                    cbMaNV.ValueMember = "MaNV_TuanhCD233018";
                    cbMaNV.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu mã NV: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void LoadComboBoxMaDA()
        {
            try
            {
                cn.connect();
                string sql = @"
                    SELECT MaDA_KienCD233824, TenDA_KienCD233824
                    FROM tblDuAn_KienCD233824 
                    WHERE DeletedAt_KienCD233824 = 0
                    ORDER BY MaDA_KienCD233824";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbMaDuAn.DataSource = dt;
                    cbMaDuAn.DisplayMember = "MaDA_KienCD233824";
                    cbMaDuAn.ValueMember = "MaDA_KienCD233824";
                    cbMaDuAn.SelectedIndex = -1;
                    cbTimDA.DataSource = dt;
                    cbTimDA.DisplayMember = "MaDA_KienCD233824";
                    cbTimDA.ValueMember = "MaDA_KienCD233824";
                    cbTimDA.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu mã DA: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }


        private void btnThemDA_Click(object sender, EventArgs e)
        {
            try
            {
                // KIỂM TRA - Lấy text từ ComboBox (có thể là text mới hoặc text đã chọn)
                string maDA = cbTimDA.Text.Trim();

                if (string.IsNullOrWhiteSpace(maDA))
                {
                    MessageBox.Show("Vui lòng nhập mã dự án!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbTenDA.Text) ||
                    string.IsNullOrWhiteSpace(tbMota.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (DatePickerNgayBatDau.Value > DatePickerNgayKetThuc.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();

                // Kiểm tra mã dự án đã tồn tại chưa
                string checkMaDA = @"
            SELECT COUNT(*) 
            FROM tblDuAn_KienCD233824  
            WHERE MaDA_KienCD233824 = @MaDA  
              AND DeletedAt_KienCD233824 = 0";

                using (SqlCommand cmd = new SqlCommand(checkMaDA, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaDA", maDA);
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Mã dự án đã tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Insert dự án mới
                string sqlInsert = @"
            INSERT INTO tblDuAn_KienCD233824 
                (MaDA_KienCD233824, TenDA_KienCD233824, MoTa_KienCD233824, 
                 NgayBatDau_KienCD233824, NgayKetThuc_KienCD233824, 
                 Ghichu_KienCD233824, DeletedAt_KienCD233824)
            VALUES 
                (@MaDA, @TenDA, @MoTa, @NgayBatDau, @NgayKetThuc, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(sqlInsert, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaDA", maDA);
                    cmd.Parameters.AddWithValue("@TenDA", tbTenDA.Text.Trim());
                    cmd.Parameters.AddWithValue("@MoTa", tbMota.Text.Trim());
                    cmd.Parameters.AddWithValue("@NgayBatDau", DatePickerNgayBatDau.Value);
                    cmd.Parameters.AddWithValue("@NgayKetThuc", DatePickerNgayKetThuc.Value);
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChuDA.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm dự án thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataDuAn();
                        LoadComboBoxMaDA();
                        ClearAllInputs(groupBox2);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnSuaDA_Click(object sender, EventArgs e)
        {
            try
            {
                // KIỂM TRA - Đúng cách
                if (cbMaDuAn.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn dự án cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbTenDA.Text) ||
                    string.IsNullOrWhiteSpace(tbMota.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (DatePickerNgayBatDau.Value > DatePickerNgayKetThuc.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa dự án này không?",
                    "Xác nhận sửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string sql = @"
                UPDATE tblDuAn_KienCD233824 
                SET TenDA_KienCD233824 = @TenDA, 
                    MoTa_KienCD233824 = @MoTa, 
                    NgayBatDau_KienCD233824 = @NgayBatDau, 
                    NgayKetThuc_KienCD233824 = @NgayKetThuc, 
                    Ghichu_KienCD233824 = @GhiChu
                WHERE MaDA_KienCD233824 = @MaDA
                  AND DeletedAt_KienCD233824 = 0";

                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        // LẤY GIÁ TRỊ - Sử dụng SelectedValue
                        cmd.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                        cmd.Parameters.AddWithValue("@TenDA", tbTenDA.Text.Trim());
                        cmd.Parameters.AddWithValue("@MoTa", tbMota.Text.Trim());
                        cmd.Parameters.AddWithValue("@NgayBatDau", DatePickerNgayBatDau.Value);
                        cmd.Parameters.AddWithValue("@NgayKetThuc", DatePickerNgayKetThuc.Value);
                        cmd.Parameters.AddWithValue("@GhiChu", tbGhiChuDA.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật dự án thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataDuAn();
                            LoadComboBoxMaDA();
                            ClearAllInputs(groupBox2);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy dự án để cập nhật!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnXoaDA_Click(object sender, EventArgs e)
        {
            try
            {
                // KIỂM TRA - Đúng cách
                if (cbMaDuAn.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn dự án cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa dự án này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = @"
                UPDATE tblDuAn_KienCD233824 
                SET DeletedAt_KienCD233824 = 1 
                WHERE MaDA_KienCD233824 = @MaDA";

                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        // LẤY GIÁ TRỊ - Sử dụng SelectedValue
                        cmd.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa dự án thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataDuAn();
                            LoadComboBoxMaDA();
                            ClearAllInputs(groupBox2);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy dự án để xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }
        private void dgvDA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                string maDA = dgvDA.Rows[i].Cells[0].Value.ToString();
                cbMaDuAn.SelectedValue = dgvDA.Rows[i].Cells[0].Value.ToString();
                tbTenDA.Text = dgvDA.Rows[i].Cells[1].Value.ToString();
                tbMota.Text = dgvDA.Rows[i].Cells[2].Value.ToString();
                DatePickerNgayBatDau.Value = Convert.ToDateTime(dgvDA.Rows[i].Cells[3].Value);
                DatePickerNgayKetThuc.Value = Convert.ToDateTime(dgvDA.Rows[i].Cells[4].Value);
                tbGhiChuDA.Text = dgvDA.Rows[i].Cells[5].Value?.ToString() ?? "";
                LoadDataChiTietDuAnTheoMaDA(maDA);
            }
        }

        private void LoadDataChiTietDuAnTheoMaDA(string maDA)
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT 
                        ct.MaNV_TuanhCD233018 AS 'Mã nhân viên',
                        nv.HoTen_TuanhCD233018 AS 'Tên nhân viên',
                        ct.MaDA_KienCD233824 AS 'Mã dự án',
                        ct.VaiTro_KienCD233824 AS 'Vai trò',
                        ct.Ghichu_KienCD233824 AS 'Ghi chú'
                    FROM tblChiTietDuAn_KienCD233824 ct
                    LEFT JOIN tblNhanVien_TuanhCD233018 nv 
                        ON ct.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                    WHERE ct.DeletedAt_KienCD233824 = 0
                      AND ct.MaDA_KienCD233824 = @MaDA";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MaDA", maDA);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvChiTietDA.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load chi tiết dự án: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnThemCTDA_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMaNV.SelectedIndex == -1 ||
                    cbMaDuAn.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbVaiTro.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();

                string checkExist = @"
                    SELECT COUNT(*) 
                    FROM tblChiTietDuAn_KienCD233824 
                    WHERE MaNV_TuanhCD233018 = @MaNV 
                      AND MaDA_KienCD233824 = @MaDA 
                      AND DeletedAt_KienCD233824 = 0";

                using (SqlCommand cmdCheck = new SqlCommand(checkExist, cn.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                    cmdCheck.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Nhân viên này đã tham gia dự án!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                string sqlInsert = @"
                    INSERT INTO tblChiTietDuAn_KienCD233824
                        (MaNV_TuanhCD233018, MaDA_KienCD233824, VaiTro_KienCD233824, 
                         Ghichu_KienCD233824, DeletedAt_KienCD233824)
                    VALUES 
                        (@MaNV, @MaDA, @VaiTro, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(sqlInsert, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                    cmd.Parameters.AddWithValue("@VaiTro", tbVaiTro.Text.Trim());
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChuCTDA.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm chi tiết dự án thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDataChiTietDuAn();
                        ClearAllInputs(groupBox1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnSuaCTDA_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMaNV.SelectedIndex == -1 ||
                    cbMaDuAn.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbVaiTro.Text))
                {
                    MessageBox.Show("Vui lòng chọn chi tiết dự án cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa chi tiết dự án này không?",
                    "Xác nhận sửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string sql = @"
                        UPDATE tblChiTietDuAn_KienCD233824 
                        SET VaiTro_KienCD233824 = @VaiTro,
                            GhiChu_KienCD233824 = @GhiChu
                        WHERE MaNV_TuanhCD233018 = @MaNV 
                          AND MaDA_KienCD233824 = @MaDA
                          AND DeletedAt_KienCD233824 = 0";

                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                        cmd.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                        cmd.Parameters.AddWithValue("@VaiTro", tbVaiTro.Text.Trim());
                        cmd.Parameters.AddWithValue("@GhiChu", tbGhiChuCTDA.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật chi tiết dự án thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataChiTietDuAn();
                            ClearAllInputs(groupBox1);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy chi tiết dự án để sửa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnXoaCTDA_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMaNV.SelectedIndex == -1 || cbMaDuAn.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn chi tiết dự án cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa chi tiết dự án này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = @"
                        UPDATE tblChiTietDuAn_KienCD233824 
                        SET DeletedAt_KienCD233824 = 1 
                        WHERE MaNV_TuanhCD233018 = @MaNV 
                          AND MaDA_KienCD233824 = @MaDA";

                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                        cmd.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa chi tiết dự án thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataChiTietDuAn();
                            ClearAllInputs(groupBox1);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy chi tiết dự án để xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDataDuAn();
            LoadDataChiTietDuAn();
            LoadComboBoxMaNV();
            LoadComboBoxMaDA();
            ClearAllInputs(this);
        }
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dgvDA.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                FileName = "DanhSachDuAn_" + DateTime.Now.ToString("ddMMyyyy")
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add("DuAn");

                            // ===== TIÊU ĐỀ CÔNG TY =====
                            int maxColumns = Math.Max(dgvDA.Columns.Count, dgvChiTietDA.Columns.Count);

                            ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                            ws.Range(1, 1, 1, maxColumns).Merge();
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Font.FontSize = 14;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== TIÊU ĐỀ CHÍNH =====
                            ws.Cell(2, 1).Value = "DANH SÁCH DỰ ÁN";
                            ws.Range(2, 1, 2, maxColumns).Merge();
                            ws.Cell(2, 1).Style.Font.Bold = true;
                            ws.Cell(2, 1).Style.Font.FontSize = 18;
                            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== NGÀY LẬP BÁO CÁO =====
                            ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy");
                            ws.Cell(3, 1).Style.Font.Italic = true;
                            ws.Cell(3, 1).Style.Font.FontSize = 11;

                            // ===== HEADER BẢNG DỰ ÁN =====
                            int headerRow = 5;
                            for (int i = 0; i < dgvDA.Columns.Count; i++)
                            {
                                ws.Cell(headerRow, i + 1).Value = dgvDA.Columns[i].HeaderText;
                                ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(headerRow, i + 1).Style.Font.FontSize = 12;
                                ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(headerRow, i + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(headerRow, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                            }

                            // ===== DỮ LIỆU DỰ ÁN =====
                            int dataStartRow = headerRow + 1;
                            for (int i = 0; i < dgvDA.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvDA.Columns.Count; j++)
                                {
                                    var cell = ws.Cell(dataStartRow + i, j + 1);
                                    var cellValue = dgvDA.Rows[i].Cells[j].Value;

                                    if (cellValue is DateTime)
                                    {
                                        cell.Value = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        cell.Value = cellValue?.ToString() ?? "";
                                    }

                                    // Định dạng ô
                                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                }
                            }

                            // ===== BORDER BẢNG DỰ ÁN =====
                            int lastDataRow = dataStartRow + dgvDA.Rows.Count - 1;
                            var tableRange = ws.Range(headerRow, 1, lastDataRow, dgvDA.Columns.Count);
                            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                            // ===== THỐNG KÊ DỰ ÁN =====
                            int statsRow = lastDataRow + 2;
                            ws.Cell(statsRow, 1).Value = "Tổng số dự án:";
                            ws.Cell(statsRow, 2).Value = dgvDA.Rows.Count;
                            ws.Cell(statsRow, 1).Style.Font.Bold = true;
                            ws.Cell(statsRow, 2).Style.Font.Bold = true;
                            ws.Cell(statsRow, 1).Style.Font.FontSize = 12;
                            ws.Cell(statsRow, 2).Style.Font.FontSize = 12;

                            // ===== BẢNG CHI TIẾT DỰ ÁN =====
                            int chiTietStartRow = statsRow + 3;

                            // Tìm số cột tối đa để merge tiêu đề (cộng thêm 1 vì cột Ghi chú chiếm 2 cột)
                            int chiTietTotalColumns = dgvChiTietDA.Columns.Count + 1;
                            maxColumns = Math.Max(dgvDA.Columns.Count, chiTietTotalColumns);

                            // Tiêu đề phụ
                            ws.Cell(chiTietStartRow, 1).Value = "DANH SÁCH NHÂN VIÊN HOẠT ĐỘNG TRONG DỰ ÁN";
                            ws.Range(chiTietStartRow, 1, chiTietStartRow, maxColumns).Merge();
                            ws.Cell(chiTietStartRow, 1).Style.Font.Bold = true;
                            ws.Cell(chiTietStartRow, 1).Style.Font.FontSize = 14;
                            ws.Cell(chiTietStartRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(chiTietStartRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // Header bảng chi tiết
                            int chiTietHeaderRow = chiTietStartRow + 2;
                            int currentCol = 1;

                            for (int i = 0; i < dgvChiTietDA.Columns.Count; i++)
                            {
                                string headerText = dgvChiTietDA.Columns[i].HeaderText;

                                // Kiểm tra nếu là cột "Ghi chú" (cột cuối cùng)
                                if (i == dgvChiTietDA.Columns.Count - 1 && (headerText.ToLower().Contains("ghi chú") || headerText.ToLower().Contains("ghichu")))
                                {
                                    // Gộp 2 cột cho cột Ghi chú (theo chiều ngang)
                                    ws.Cell(chiTietHeaderRow, currentCol).Value = headerText;
                                    var mergedRange = ws.Range(chiTietHeaderRow, currentCol, chiTietHeaderRow, currentCol + 1);
                                    mergedRange.Merge();

                                    // Định dạng cho header Ghi chú
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Font.Bold = true;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Font.FontSize = 12;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                    // Border cho toàn bộ vùng merge
                                    mergedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                    mergedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                    currentCol += 2; // Nhảy 2 cột
                                }
                                else
                                {
                                    // Header bình thường cho các cột khác
                                    ws.Cell(chiTietHeaderRow, currentCol).Value = headerText;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Font.Bold = true;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Font.FontSize = 12;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                    ws.Cell(chiTietHeaderRow, currentCol).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                    currentCol++;
                                }
                            }

                            // Dữ liệu chi tiết
                            int chiTietDataStartRow = chiTietHeaderRow + 1;
                            if (dgvChiTietDA.Rows.Count > 0)
                            {
                                for (int i = 0; i < dgvChiTietDA.Rows.Count; i++)
                                {
                                    currentCol = 1;

                                    for (int j = 0; j < dgvChiTietDA.Columns.Count; j++)
                                    {
                                        var cellValue = dgvChiTietDA.Rows[i].Cells[j].Value;
                                        string headerText = dgvChiTietDA.Columns[j].HeaderText;

                                        // Kiểm tra nếu là cột "Ghi chú"
                                        if (j == dgvChiTietDA.Columns.Count - 1 && (headerText.ToLower().Contains("ghi chú") || headerText.ToLower().Contains("ghichu")))
                                        {
                                            // Gộp 2 cột cho dữ liệu cột Ghi chú
                                            var cell = ws.Cell(chiTietDataStartRow + i, currentCol);
                                            cell.Value = cellValue?.ToString() ?? "";

                                            var mergedRange = ws.Range(chiTietDataStartRow + i, currentCol, chiTietDataStartRow + i, currentCol + 1);
                                            mergedRange.Merge();

                                            // Định dạng và border cho vùng merge
                                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                            mergedRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                            mergedRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                            currentCol += 2;
                                        }
                                        else
                                        {
                                            var cell = ws.Cell(chiTietDataStartRow + i, currentCol);
                                            cell.Value = cellValue?.ToString() ?? "";
                                            cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                            currentCol++;
                                        }
                                    }
                                }

                                // Border bảng chi tiết - bao quanh toàn bộ bảng
                                int lastChiTietRow = chiTietDataStartRow + dgvChiTietDA.Rows.Count - 1;
                                var chiTietTableRange = ws.Range(chiTietHeaderRow, 1, lastChiTietRow, chiTietTotalColumns);
                                chiTietTableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;

                                // Thống kê
                                int chiTietStatsRow = lastChiTietRow + 2;
                                ws.Cell(chiTietStatsRow, 1).Value = "Tổng số nhân viên tham gia:";
                                ws.Cell(chiTietStatsRow, 2).Value = dgvChiTietDA.Rows.Count;
                                ws.Cell(chiTietStatsRow, 1).Style.Font.Bold = true;
                                ws.Cell(chiTietStatsRow, 2).Style.Font.Bold = true;
                                ws.Cell(chiTietStatsRow, 1).Style.Font.FontSize = 12;
                                ws.Cell(chiTietStatsRow, 2).Style.Font.FontSize = 12;

                                // Chữ ký
                                FormatSignature(ws, chiTietStatsRow + 2);
                            }
                            else
                            {
                                // Không có dữ liệu
                                ws.Cell(chiTietDataStartRow, 1).Value = "Chưa có nhân viên tham gia dự án";
                                ws.Range(chiTietDataStartRow, 1, chiTietDataStartRow, maxColumns).Merge();
                                ws.Cell(chiTietDataStartRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(chiTietDataStartRow, 1).Style.Font.Italic = true;

                                // Chữ ký
                                FormatSignature(ws, chiTietDataStartRow + 3);
                            }

                            // ===== ĐIỀU CHỈNH CỘT VÀ DÒNG =====
                            ws.Columns().AdjustToContents();

                            // Điều chỉnh độ rộng cột cho tất cả các cột
                            for (int i = 1; i <= maxColumns; i++)
                            {
                                if (ws.Column(i).Width < 15)
                                    ws.Column(i).Width = 15;
                                if (ws.Column(i).Width > 50)
                                    ws.Column(i).Width = 50;
                            }

                            ws.Row(1).Height = 28;
                            ws.Row(2).Height = 35;
                            ws.Row(headerRow).Height = 25;
                            ws.Row(chiTietStartRow).Height = 28;
                            ws.Row(chiTietHeaderRow).Height = 25;

                            // Đóng băng tiêu đề
                            ws.SheetView.FreezeRows(headerRow);

                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DialogResult openFile = MessageBox.Show("Bạn có muốn mở file vừa xuất không?",
                            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (openFile == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Hàm phụ để định dạng phần chữ ký
        private void FormatSignature(IXLWorksheet ws, int signatureRow)
        {
            // Ngày tháng
            ws.Cell(signatureRow, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
            ws.Cell(signatureRow, 4).Style.Font.Italic = true;
            ws.Cell(signatureRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(signatureRow, 4, signatureRow, 5).Merge();

            // Chức danh
            ws.Cell(signatureRow + 1, 4).Value = "Người lập báo cáo";
            ws.Cell(signatureRow + 1, 4).Style.Font.Bold = true;
            ws.Cell(signatureRow + 1, 4).Style.Font.FontSize = 11;
            ws.Cell(signatureRow + 1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(signatureRow + 1, 4, signatureRow + 1, 5).Merge();

            // Tên người ký
            ws.Cell(signatureRow + 4, 4).Value = nguoiDangNhap;
            ws.Cell(signatureRow + 4, 4).Style.Font.Bold = true;
            ws.Cell(signatureRow + 4, 4).Style.Font.FontSize = 12;
            ws.Cell(signatureRow + 4, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(signatureRow + 4, 4, signatureRow + 4, 5).Merge();
        }
        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
             if (dgvDA.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Files|*.pdf",
                FileName = "DanhSachDuAn_" + DateTime.Now.ToString("ddMMyyyy") + ".pdf"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Tạo document PDF
                        iTextPdf.Document doc = new iTextPdf.Document(iTextPdf.PageSize.A4, 25, 25, 30, 30);
                        PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();

                        // Load font hỗ trợ tiếng Việt
                        string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                        BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                        iTextPdf.Font fontTitle = new iTextPdf.Font(bf, 14, iTextPdf.Font.BOLD);
                        iTextPdf.Font fontHeader = new iTextPdf.Font(bf, 16, iTextPdf.Font.BOLD);
                        iTextPdf.Font fontNormal = new iTextPdf.Font(bf, 11, iTextPdf.Font.NORMAL);
                        iTextPdf.Font fontBold = new iTextPdf.Font(bf, 11, iTextPdf.Font.BOLD);
                        iTextPdf.Font fontItalic = new iTextPdf.Font(bf, 11, iTextPdf.Font.ITALIC);
                        iTextPdf.Font fontTableHeader = new iTextPdf.Font(bf, 10, iTextPdf.Font.BOLD);
                        iTextPdf.Font fontTableData = new iTextPdf.Font(bf, 10, iTextPdf.Font.NORMAL);

                        // ===== TIÊU ĐỀ CÔNG TY =====
                        iTextPdf.Paragraph companyTitle = new iTextPdf.Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontTitle);
                        companyTitle.Alignment = iTextPdf.Element.ALIGN_CENTER;
                        companyTitle.SpacingAfter = 10f;
                        doc.Add(companyTitle);
                       


                        // ===== TIÊU ĐỀ CHÍNH =====
                        iTextPdf.Paragraph mainTitle = new iTextPdf.Paragraph("DANH SÁCH DỰ ÁN", fontHeader);
                        mainTitle.Alignment = iTextPdf.Element.ALIGN_CENTER;
                        mainTitle.SpacingAfter = 10f;
                        doc.Add(mainTitle);

                        // ===== NGÀY LẬP BÁO CÁO =====
                        iTextPdf.Paragraph dateReport = new iTextPdf.Paragraph("Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy"), fontItalic);
                        dateReport.SpacingAfter = 15f;
                        doc.Add(dateReport);

                        // ===== BẢNG DỰ ÁN =====
                        PdfPTable tableDuAn = new PdfPTable(dgvDA.Columns.Count);
                        tableDuAn.WidthPercentage = 100;
                        tableDuAn.SpacingBefore = 10f;
                        tableDuAn.SpacingAfter = 10f;

                        // Đặt độ rộng cột
                        float[] columnWidths = new float[] { 15f, 25f, 30f, 15f, 15f, 20f };
                        tableDuAn.SetWidths(columnWidths);

                        // Header bảng dự án
                        foreach (DataGridViewColumn column in dgvDA.Columns)
                        {
                            PdfPCell headerCell = new PdfPCell(new iTextPdf.Phrase(column.HeaderText, fontTableHeader));
                            headerCell.BackgroundColor = new iTextPdf.BaseColor(240, 240, 240);
                            headerCell.HorizontalAlignment = iTextPdf.Element.ALIGN_CENTER;
                            headerCell.VerticalAlignment = iTextPdf.Element.ALIGN_MIDDLE;
                            headerCell.Padding = 8;
                            headerCell.BorderWidth = 1.5f;
                            tableDuAn.AddCell(headerCell);
                        }

                        // Dữ liệu bảng dự án
                        foreach (DataGridViewRow row in dgvDA.Rows)
                        {
                            if (row.IsNewRow) continue;

                            for (int i = 0; i < dgvDA.Columns.Count; i++)
                            {
                                string cellValue = "";
                                if (row.Cells[i].Value != null)
                                {
                                    if (row.Cells[i].Value is DateTime)
                                    {
                                        cellValue = ((DateTime)row.Cells[i].Value).ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        cellValue = row.Cells[i].Value.ToString();
                                    }
                                }

                                PdfPCell dataCell = new PdfPCell(new iTextPdf.Phrase(cellValue, fontTableData));
                                dataCell.HorizontalAlignment = iTextPdf.Element.ALIGN_LEFT;
                                dataCell.VerticalAlignment = iTextPdf.Element.ALIGN_MIDDLE;
                                dataCell.Padding = 5;
                                tableDuAn.AddCell(dataCell);
                            }
                        }

                        // Dòng tổng số dự án
                        PdfPCell totalLabelCell = new PdfPCell(new iTextPdf.Phrase("Tổng số dự án:", fontBold));
                        totalLabelCell.Padding = 5;
                        totalLabelCell.BorderWidth = 1.5f;
                        tableDuAn.AddCell(totalLabelCell);

                        PdfPCell totalValueCell = new PdfPCell(new iTextPdf.Phrase(dgvDA.Rows.Count.ToString(), fontBold));
                        totalValueCell.Colspan = dgvDA.Columns.Count - 1;
                        totalValueCell.Padding = 5;
                        totalValueCell.BorderWidth = 1.5f;
                        tableDuAn.AddCell(totalValueCell);

                        doc.Add(tableDuAn);

                        // ===== TIÊU ĐỀ BẢNG CHI TIẾT =====
                        iTextPdf.Paragraph chiTietTitle = new iTextPdf.Paragraph("DANH SÁCH NHÂN VIÊN HOẠT ĐỘNG TRONG DỰ ÁN", fontTitle);
                        chiTietTitle.Alignment = iTextPdf.Element.ALIGN_CENTER;
                        chiTietTitle.SpacingBefore = 20f;
                        chiTietTitle.SpacingAfter = 15f;
                        doc.Add(chiTietTitle);

                        // ===== BẢNG CHI TIẾT NHÂN VIÊN =====
                        PdfPTable tableChiTiet = new PdfPTable(dgvChiTietDA.Columns.Count);
                        tableChiTiet.WidthPercentage = 100;
                        tableChiTiet.SpacingBefore = 10f;
                        tableChiTiet.SpacingAfter = 10f;

                        // Đặt độ rộng cột cho bảng chi tiết
                        float[] chiTietColumnWidths = new float[] { 18f, 25f, 15f, 20f, 30f };
                        tableChiTiet.SetWidths(chiTietColumnWidths);

                        // Header bảng chi tiết
                        foreach (DataGridViewColumn column in dgvChiTietDA.Columns)
                        {
                            PdfPCell headerCell = new PdfPCell(new iTextPdf.Phrase(column.HeaderText, fontTableHeader));
                            headerCell.BackgroundColor = new iTextPdf.BaseColor(240, 240, 240);
                            headerCell.HorizontalAlignment = iTextPdf.Element.ALIGN_CENTER;
                            headerCell.VerticalAlignment = iTextPdf.Element.ALIGN_MIDDLE;
                            headerCell.Padding = 8;
                            headerCell.BorderWidth = 1.5f;
                            tableChiTiet.AddCell(headerCell);
                        }

                        // Dữ liệu bảng chi tiết
                        if (dgvChiTietDA.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dgvChiTietDA.Rows)
                            {
                                if (row.IsNewRow) continue;

                                for (int i = 0; i < dgvChiTietDA.Columns.Count; i++)
                                {
                                    string cellValue = row.Cells[i].Value?.ToString() ?? "";
                                    PdfPCell dataCell = new PdfPCell(new iTextPdf.Phrase(cellValue, fontTableData));
                                    dataCell.HorizontalAlignment = iTextPdf.Element.ALIGN_LEFT;
                                    dataCell.VerticalAlignment = iTextPdf.Element.ALIGN_MIDDLE;
                                    dataCell.Padding = 5;
                                    tableChiTiet.AddCell(dataCell);
                                }
                            }

                            // Dòng tổng số nhân viên
                            PdfPCell nvTotalLabelCell = new PdfPCell(new iTextPdf.Phrase("Tổng số nhân viên tham gia:", fontBold));
                            nvTotalLabelCell.Padding = 5;
                            nvTotalLabelCell.BorderWidth = 1.5f;
                            tableChiTiet.AddCell(nvTotalLabelCell);

                            PdfPCell nvTotalValueCell = new PdfPCell(new iTextPdf.Phrase(dgvChiTietDA.Rows.Count.ToString(), fontBold));
                            nvTotalValueCell.Colspan = dgvChiTietDA.Columns.Count - 1;
                            nvTotalValueCell.Padding = 5;
                            nvTotalValueCell.BorderWidth = 1.5f;
                            tableChiTiet.AddCell(nvTotalValueCell);
                        }
                        else
                        {
                            PdfPCell noDataCell = new PdfPCell(new iTextPdf.Phrase("Chưa có nhân viên tham gia dự án", fontItalic));
                            noDataCell.Colspan = dgvChiTietDA.Columns.Count;
                            noDataCell.HorizontalAlignment = iTextPdf.Element.ALIGN_CENTER;
                            noDataCell.Padding = 10;
                            tableChiTiet.AddCell(noDataCell);
                        }

                        doc.Add(tableChiTiet);

                        // ===== CHỮ KÝ =====
                        iTextPdf.Paragraph signature = new iTextPdf.Paragraph();
                        signature.SpacingBefore = 30f;
                        signature.Alignment = iTextPdf.Element.ALIGN_RIGHT;

                        signature.Add(new iTextPdf.Chunk("Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year + "\n", fontItalic));
                        signature.Add(new iTextPdf.Chunk("Người lập báo cáo\n\n\n\n", fontBold));
                        signature.Add(new iTextPdf.Chunk(nguoiDangNhap, fontBold));

                        doc.Add(signature);

                        // Đóng document
                        doc.Close();

                        MessageBox.Show("Xuất PDF thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DialogResult openFile = MessageBox.Show("Bạn có muốn mở file vừa xuất không?",
                            "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (openFile == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xuất PDF: " + ex.Message + "\n\nChi tiết: " + ex.StackTrace,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvChiTietDA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                cbMaNV.SelectedValue = dgvChiTietDA.Rows[i].Cells[0].Value?.ToString();
                tbTenNhanVien.Text = dgvChiTietDA.Rows[i].Cells[1].Value?.ToString() ?? "";
                cbMaDuAn.SelectedValue = dgvChiTietDA.Rows[i].Cells[2].Value?.ToString();
                tbVaiTro.Text = dgvChiTietDA.Rows[i].Cells[3].Value?.ToString() ?? "";
                tbGhiChuCTDA.Text = dgvChiTietDA.Rows[i].Cells[4].Value?.ToString() ?? "";
            }
        }

        private void F_DuAnChung_Load(object sender, EventArgs e)
        {
            LoadDataDuAn();
            LoadDataChiTietDuAn();
            LoadComboBoxMaNV();
            LoadComboBoxMaDA();
        }

        private void cbMaNV_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbMaNV.SelectedValue == null) return;

            try
            {
                cn.connect();

                string sql = @"
                    SELECT HoTen_TuanhCD233018
                    FROM tblNhanVien_TuanhCD233018
                    WHERE MaNV_TuanhCD233018 = @MaNV
                      AND DeletedAt_TuanhCD233018 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue.ToString());

                    object result = cmd.ExecuteScalar();
                    tbTenNhanVien.Text = result != null ? result.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load tên NV: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void cbTimDA_SelectedValueChanged(object sender, EventArgs e)
        {
            // Kiểm tra xem có item nào được chọn không
            if (cbTimDA.SelectedIndex == -1 || cbTimDA.SelectedValue == null)
            {
                tbTenDA.Clear();
                return;
            }

            try
            {
                cn.connect();

                string sql = @"
            SELECT TenDA_KienCD233824
            FROM tblDuAn_KienCD233824
            WHERE MaDA_KienCD233824 = @MaDA
              AND DeletedAt_KienCD233824 = 0";

                // FIX: Missing SqlCommand creation!
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaDA", cbTimDA.SelectedValue.ToString());

                    object result = cmd.ExecuteScalar();
                    tbTenDA.Text = result != null ? result.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load tên dự án: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void cbTimDA_TextChanged(object sender, EventArgs e) // Đổi tên event
        {
            string maDA = cbTimDA.Text.Trim();

            if (string.IsNullOrWhiteSpace(maDA))
            {
                tbTenDA.Clear();
                return;
            }

            try
            {
                cn.connect();

                string sql = @"
            SELECT TenDA_KienCD233824
            FROM tblDuAn_KienCD233824
            WHERE MaDA_KienCD233824 = @MaDA
              AND DeletedAt_KienCD233824 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaDA", maDA);

                    object result = cmd.ExecuteScalar();

                    // Nếu tìm thấy dự án cũ, tự động fill tên
                    // Nếu không tìm thấy, để trống để user nhập mới
                    tbTenDA.Text = result != null ? result.ToString() : "";
                }
            }
            catch //(Exception ex)
            {
                // Không cần hiện lỗi khi đang gõ
                tbTenDA.Clear();
            }
            finally
            {
                cn.disconnect();
            }
        }
    }
}