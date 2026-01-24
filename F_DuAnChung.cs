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

        private void cbMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaNV.SelectedIndex != -1)
            {
                try
                {
                    DataRowView drv = (DataRowView)cbMaNV.SelectedItem;
                    tbTenNhanVien.Text = drv["HoTen_TuanhCD233018"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            else
            {
                tbTenNhanVien.Clear();
            }
        }

        private void btnThemDA_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbmaDA.Text) ||
                    string.IsNullOrWhiteSpace(tbTenDA.Text) ||
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

                string checkMaDA = @"
                    SELECT COUNT(*) 
                    FROM tblDuAn_KienCD233824  
                    WHERE MaDA_KienCD233824 = @MaDA  
                      AND DeletedAt_KienCD233824 = 0";
                using (SqlCommand cmd = new SqlCommand(checkMaDA, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaDA", tbmaDA.Text.Trim());
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Mã dự án đã tồn tại trong hệ thống!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                string sqlInsert = @"
                    INSERT INTO tblDuAn_KienCD233824 
                        (MaDA_KienCD233824, TenDA_KienCD233824, MoTa_KienCD233824, 
                         NgayBatDau_KienCD233824, NgayKetThuc_KienCD233824, 
                         Ghichu_KienCD233824, DeletedAt_KienCD233824)
                    VALUES 
                        (@MaDA, @TenDA, @MoTa, @NgayBatDau, @NgayKetThuc, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(sqlInsert, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaDA", tbmaDA.Text.Trim());
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
                if (string.IsNullOrEmpty(tbmaDA.Text))
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
                            GhiChu_KienCD233824 = @GhiChu
                        WHERE MaDA_KienCD233824 = @MaDA
                          AND DeletedAt_KienCD233824 = 0";

                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaDA", tbmaDA.Text.Trim());
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
                if (string.IsNullOrEmpty(tbmaDA.Text))
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
                        cmd.Parameters.AddWithValue("@MaDA", tbmaDA.Text.Trim());
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
                string maDA = dgvDA.Rows[e.RowIndex].Cells[0].Value.ToString();
                tbmaDA.Text = dgvDA.Rows[i].Cells[0].Value.ToString();
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
                            ws.Cell(1, 1).Style.Font.FontColor = XLColor.DarkBlue;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== TIÊU ĐỀ CHÍNH =====
                            ws.Cell(2, 1).Value = "DANH SÁCH DỰ ÁN";
                            ws.Range(2, 1, 2, maxColumns).Merge();
                            ws.Cell(2, 1).Style.Font.Bold = true;
                            ws.Cell(2, 1).Style.Font.FontSize = 18;
                            ws.Cell(2, 1).Style.Font.FontColor = XLColor.White;
                            ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 112, 192); // Màu xanh dương đậm
                            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                            // ===== NGÀY LẬP BÁO CÁO =====
                            ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            ws.Cell(3, 1).Style.Font.Italic = true;
                            ws.Cell(3, 1).Style.Font.FontSize = 11;
                            ws.Cell(3, 1).Style.Font.FontColor = XLColor.Gray;

                            // ===== HEADER BẢNG DỰ ÁN =====
                            int headerRow = 5;
                            for (int i = 0; i < dgvDA.Columns.Count; i++)
                            {
                                ws.Cell(headerRow, i + 1).Value = dgvDA.Columns[i].HeaderText;
                                ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(headerRow, i + 1).Style.Font.FontSize = 12;
                                ws.Cell(headerRow, i + 1).Style.Font.FontColor = XLColor.White;
                                ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(headerRow, i + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(68, 114, 196); // Xanh dương vừa
                                ws.Cell(headerRow, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                ws.Cell(headerRow, i + 1).Style.Border.OutsideBorderColor = XLColor.White;
                            }

                            // ===== DỮ LIỆU DỰ ÁN =====
                            int dataStartRow = headerRow + 1;
                            for (int i = 0; i < dgvDA.Rows.Count; i++)
                            {
                                // Màu xen kẽ cho từng dòng
                                bool isEvenRow = (i % 2 == 0);

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
                                    cell.Style.Border.OutsideBorderColor = XLColor.LightGray;

                                    // Màu nền xen kẽ
                                    if (isEvenRow)
                                    {
                                        cell.Style.Fill.BackgroundColor = XLColor.FromArgb(217, 225, 242); // Xanh nhạt
                                    }
                                    else
                                    {
                                        cell.Style.Fill.BackgroundColor = XLColor.White;
                                    }
                                }
                            }

                            // ===== BORDER BẢNG DỰ ÁN =====
                            int lastDataRow = dataStartRow + dgvDA.Rows.Count - 1;
                            var tableRange = ws.Range(headerRow, 1, lastDataRow, dgvDA.Columns.Count);
                            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                            tableRange.Style.Border.OutsideBorderColor = XLColor.FromArgb(68, 114, 196);

                            // ===== THỐNG KÊ DỰ ÁN =====
                            int statsRow = lastDataRow + 2;
                            ws.Cell(statsRow, 1).Value = "Tổng số dự án:";
                            ws.Cell(statsRow, 2).Value = dgvDA.Rows.Count;
                            ws.Cell(statsRow, 1).Style.Font.Bold = true;
                            ws.Cell(statsRow, 2).Style.Font.Bold = true;
                            ws.Cell(statsRow, 1).Style.Font.FontSize = 12;
                            ws.Cell(statsRow, 2).Style.Font.FontSize = 12;
                            ws.Cell(statsRow, 2).Style.Font.FontColor = XLColor.FromArgb(0, 112, 192);

                            // ===== BẢNG CHI TIẾT DỰ ÁN =====
                            int chiTietStartRow = statsRow + 3;

                            // Tìm số cột tối đa để merge tiêu đề
                            maxColumns = Math.Max(dgvDA.Columns.Count, dgvChiTietDA.Columns.Count);

                            // Tiêu đề phụ
                            ws.Cell(chiTietStartRow, 1).Value = "DANH SÁCH NHÂN VIÊN HOẠT ĐỘNG TRONG DỰ ÁN";
                            ws.Range(chiTietStartRow, 1, chiTietStartRow, maxColumns).Merge();
                            ws.Cell(chiTietStartRow, 1).Style.Font.Bold = true;
                            ws.Cell(chiTietStartRow, 1).Style.Font.FontSize = 14;
                            ws.Cell(chiTietStartRow, 1).Style.Font.FontColor = XLColor.White;
                            ws.Cell(chiTietStartRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(chiTietStartRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            ws.Cell(chiTietStartRow, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(0, 176, 80); // Xanh lá

                            // Header bảng chi tiết
                            int chiTietHeaderRow = chiTietStartRow + 2;
                            for (int i = 0; i < dgvChiTietDA.Columns.Count; i++)
                            {
                                ws.Cell(chiTietHeaderRow, i + 1).Value = dgvChiTietDA.Columns[i].HeaderText;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Font.FontSize = 12;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Font.FontColor = XLColor.White;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Fill.BackgroundColor = XLColor.FromArgb(112, 173, 71); // Xanh lá vừa
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Border.OutsideBorderColor = XLColor.White;
                            }

                            // Dữ liệu chi tiết
                            int chiTietDataStartRow = chiTietHeaderRow + 1;
                            if (dgvChiTietDA.Rows.Count > 0)
                            {
                                for (int i = 0; i < dgvChiTietDA.Rows.Count; i++)
                                {
                                    bool isEvenRow = (i % 2 == 0);

                                    for (int j = 0; j < dgvChiTietDA.Columns.Count; j++)
                                    {
                                        var cell = ws.Cell(chiTietDataStartRow + i, j + 1);
                                        var cellValue = dgvChiTietDA.Rows[i].Cells[j].Value;
                                        cell.Value = cellValue?.ToString() ?? "";

                                        // Định dạng
                                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                        cell.Style.Border.OutsideBorderColor = XLColor.LightGray;

                                        // Màu xen kẽ
                                        if (isEvenRow)
                                        {
                                            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(226, 239, 218); // Xanh lá nhạt
                                        }
                                        else
                                        {
                                            cell.Style.Fill.BackgroundColor = XLColor.White;
                                        }
                                    }
                                }

                                // Border bảng chi tiết
                                int lastChiTietRow = chiTietDataStartRow + dgvChiTietDA.Rows.Count - 1;
                                var chiTietTableRange = ws.Range(chiTietHeaderRow, 1, lastChiTietRow, dgvChiTietDA.Columns.Count);
                                chiTietTableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                chiTietTableRange.Style.Border.OutsideBorderColor = XLColor.FromArgb(112, 173, 71);

                                // Thống kê
                                int chiTietStatsRow = lastChiTietRow + 2;
                                ws.Cell(chiTietStatsRow, 1).Value = "Tổng số nhân viên tham gia:";
                                ws.Cell(chiTietStatsRow, 2).Value = dgvChiTietDA.Rows.Count;
                                ws.Cell(chiTietStatsRow, 1).Style.Font.Bold = true;
                                ws.Cell(chiTietStatsRow, 2).Style.Font.Bold = true;
                                ws.Cell(chiTietStatsRow, 1).Style.Font.FontSize = 12;
                                ws.Cell(chiTietStatsRow, 2).Style.Font.FontSize = 12;
                                ws.Cell(chiTietStatsRow, 2).Style.Font.FontColor = XLColor.FromArgb(0, 176, 80);

                                // Chữ ký
                                FormatSignature(ws, chiTietStatsRow + 3);
                            }
                            else
                            {
                                // Không có dữ liệu
                                ws.Cell(chiTietDataStartRow, 1).Value = "Chưa có nhân viên tham gia dự án";
                                ws.Range(chiTietDataStartRow, 1, chiTietDataStartRow, maxColumns).Merge();
                                ws.Cell(chiTietDataStartRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(chiTietDataStartRow, 1).Style.Font.Italic = true;
                                ws.Cell(chiTietDataStartRow, 1).Style.Font.FontColor = XLColor.Gray;
                                ws.Cell(chiTietDataStartRow, 1).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);

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

                            // Tiêu đề công ty
                            ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                            ws.Range(1, 1, 1, 6).Merge();
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Font.FontSize = 14;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Tiêu đề chính
                            ws.Cell(2, 1).Value = "DANH SÁCH DỰ ÁN";
                            ws.Range(2, 1, 2, 6).Merge();
                            ws.Cell(2, 1).Style.Font.Bold = true;
                            ws.Cell(2, 1).Style.Font.FontSize = 16;
                            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Ngày lập báo cáo
                            ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy");
                            ws.Cell(3, 1).Style.Font.Italic = true;

                            // Header bảng dự án
                            int headerRow = 5;
                            for (int i = 0; i < dgvDA.Columns.Count; i++)
                            {
                                ws.Cell(headerRow, i + 1).Value = dgvDA.Columns[i].HeaderText;
                                ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            }

                            // Dữ liệu dự án
                            int dataStartRow = headerRow + 1;
                            for (int i = 0; i < dgvDA.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvDA.Columns.Count; j++)
                                {
                                    var cellValue = dgvDA.Rows[i].Cells[j].Value;
                                    if (cellValue is DateTime)
                                    {
                                        ws.Cell(dataStartRow + i, j + 1).Value = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                                    }
                                    else
                                    {
                                        ws.Cell(dataStartRow + i, j + 1).Value = cellValue?.ToString() ?? "";
                                    }
                                }
                            }

                            // Border bảng dự án
                            int lastDataRow = dataStartRow + dgvDA.Rows.Count - 1;
                            var tableRange = ws.Range(headerRow, 1, lastDataRow, dgvDA.Columns.Count);
                            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                            tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            // Thống kê dự án
                            int statsRow = lastDataRow + 2;
                            ws.Cell(statsRow, 1).Value = "Tổng số dự án:";
                            ws.Cell(statsRow, 2).Value = dgvDA.Rows.Count;
                            ws.Cell(statsRow, 1).Style.Font.Bold = true;
                            ws.Cell(statsRow, 2).Style.Font.Bold = true;

                            // ===== BẢNG CHI TIẾT DỰ ÁN =====
                            int chiTietStartRow = statsRow + 3;

                            // Tiêu đề phụ cho bảng chi tiết
                            ws.Cell(chiTietStartRow, 1).Value = "DANH SÁCH NHÂN VIÊN HOẠT ĐỘNG TRONG DỰ ÁN";
                            ws.Range(chiTietStartRow, 1, chiTietStartRow, 5).Merge();
                            ws.Cell(chiTietStartRow, 1).Style.Font.Bold = true;
                            ws.Cell(chiTietStartRow, 1).Style.Font.FontSize = 14;
                            ws.Cell(chiTietStartRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            ws.Cell(chiTietStartRow, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;

                            // Header bảng chi tiết
                            int chiTietHeaderRow = chiTietStartRow + 2;
                            for (int i = 0; i < dgvChiTietDA.Columns.Count; i++)
                            {
                                ws.Cell(chiTietHeaderRow, i + 1).Value = dgvChiTietDA.Columns[i].HeaderText;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(chiTietHeaderRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            }

                            // Dữ liệu chi tiết dự án
                            int chiTietDataStartRow = chiTietHeaderRow + 1;
                            if (dgvChiTietDA.Rows.Count > 0)
                            {
                                for (int i = 0; i < dgvChiTietDA.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dgvChiTietDA.Columns.Count; j++)
                                    {
                                        var cellValue = dgvChiTietDA.Rows[i].Cells[j].Value;
                                        ws.Cell(chiTietDataStartRow + i, j + 1).Value = cellValue?.ToString() ?? "";
                                    }
                                }

                                // Border bảng chi tiết
                                int lastChiTietRow = chiTietDataStartRow + dgvChiTietDA.Rows.Count - 1;
                                var chiTietTableRange = ws.Range(chiTietHeaderRow, 1, lastChiTietRow, dgvChiTietDA.Columns.Count);
                                chiTietTableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                                chiTietTableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // Thống kê chi tiết
                                int chiTietStatsRow = lastChiTietRow + 2;
                                ws.Cell(chiTietStatsRow, 1).Value = "Tổng số nhân viên tham gia:";
                                ws.Cell(chiTietStatsRow, 2).Value = dgvChiTietDA.Rows.Count;
                                ws.Cell(chiTietStatsRow, 1).Style.Font.Bold = true;
                                ws.Cell(chiTietStatsRow, 2).Style.Font.Bold = true;

                                // Chữ ký
                                int signatureRow = chiTietStatsRow + 3;
                                ws.Cell(signatureRow, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                                ws.Cell(signatureRow, 4).Style.Font.Italic = true;
                                ws.Cell(signatureRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow, 4, signatureRow, 5).Merge();

                                ws.Cell(signatureRow + 1, 4).Value = "Người lập báo cáo";
                                ws.Cell(signatureRow + 1, 4).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 1, 4, signatureRow + 1, 5).Merge();

                                ws.Cell(signatureRow + 3, 4).Value = nguoiDangNhap;
                                ws.Cell(signatureRow + 3, 4).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 3, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 3, 4, signatureRow + 3, 5).Merge();
                            }
                            else
                            {
                                // Nếu không có dữ liệu chi tiết
                                ws.Cell(chiTietDataStartRow, 1).Value = "Chưa có nhân viên tham gia dự án";
                                ws.Range(chiTietDataStartRow, 1, chiTietDataStartRow, 5).Merge();
                                ws.Cell(chiTietDataStartRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(chiTietDataStartRow, 1).Style.Font.Italic = true;

                                // Chữ ký
                                int signatureRow = chiTietDataStartRow + 3;
                                ws.Cell(signatureRow, 4).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                                ws.Cell(signatureRow, 4).Style.Font.Italic = true;
                                ws.Cell(signatureRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow, 4, signatureRow, 5).Merge();

                                ws.Cell(signatureRow + 1, 4).Value = "Người lập báo cáo";
                                ws.Cell(signatureRow + 1, 4).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 1, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 1, 4, signatureRow + 1, 5).Merge();

                                ws.Cell(signatureRow + 3, 4).Value = nguoiDangNhap;
                                ws.Cell(signatureRow + 3, 4).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 3, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 3, 4, signatureRow + 3, 5).Merge();
                            }

                            // Auto-fit columns
                            ws.Columns().AdjustToContents();
                            for (int i = 1; i <= Math.Max(dgvDA.Columns.Count, dgvChiTietDA.Columns.Count); i++)
                            {
                                if (ws.Column(i).Width < 15)
                                    ws.Column(i).Width = 15;
                            }

                            ws.Row(1).Height = 25;
                            ws.Row(2).Height = 30;
                            ws.Row(chiTietStartRow).Height = 25;

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
    }
}