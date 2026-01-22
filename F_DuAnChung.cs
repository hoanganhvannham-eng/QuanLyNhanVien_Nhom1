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
        private string nguoiDangNhap = "Admin"; // Có thể lấy từ session/login

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

                            // Header
                            int headerRow = 5;
                            for (int i = 0; i < dgvDA.Columns.Count; i++)
                            {
                                ws.Cell(headerRow, i + 1).Value = dgvDA.Columns[i].HeaderText;
                                ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(headerRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(headerRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            }

                            // Dữ liệu
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

                            // Border
                            int lastDataRow = dataStartRow + dgvDA.Rows.Count - 1;
                            var tableRange = ws.Range(headerRow, 1, lastDataRow, dgvDA.Columns.Count);
                            tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                            tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            // Thống kê
                            int statsRow = lastDataRow + 2;
                            ws.Cell(statsRow, 1).Value = "Tổng số dự án:";
                            ws.Cell(statsRow, 2).Value = dgvDA.Rows.Count;
                            ws.Cell(statsRow, 1).Style.Font.Bold = true;
                            ws.Cell(statsRow, 2).Style.Font.Bold = true;

                            // Chữ ký
                            int signatureRow = lastDataRow + 4;
                            ws.Cell(signatureRow, 5).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                            ws.Cell(signatureRow, 5).Style.Font.Italic = true;
                            ws.Cell(signatureRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Cell(signatureRow + 1, 5).Value = "Người lập báo cáo";
                            ws.Cell(signatureRow + 1, 5).Style.Font.Bold = true;
                            ws.Cell(signatureRow + 1, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Cell(signatureRow + 3, 5).Value = nguoiDangNhap;
                            ws.Cell(signatureRow + 3, 5).Style.Font.Bold = true;
                            ws.Cell(signatureRow + 3, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            // Auto-fit columns
                            ws.Columns().AdjustToContents();
                            for (int i = 1; i <= dgvDA.Columns.Count; i++)
                            {
                                if (ws.Column(i).Width < 15)
                                    ws.Column(i).Width = 15;
                            }

                            ws.Row(1).Height = 25;
                            ws.Row(2).Height = 30;

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
                Filter = "PDF files|*.pdf",
                FileName = "DanhSachDuAn_" + DateTime.Now.ToString("ddMMyyyy")
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Tạo font Unicode
                        string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                        BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextPdf.Font titleFont = new iTextPdf.Font(bf, 16, iTextPdf.Font.BOLD);
                        iTextPdf.Font headerFont = new iTextPdf.Font(bf, 12, iTextPdf.Font.BOLD);
                        iTextPdf.Font normalFont = new iTextPdf.Font(bf, 11);
                        iTextPdf.Font smallFont = new iTextPdf.Font(bf, 10);

                        iTextPdf.Document doc = new iTextPdf.Document(iTextPdf.PageSize.A4.Rotate(), 25, 25, 30, 30);
                        PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                        doc.Open();

                        // Tiêu đề công ty
                        iTextPdf.Paragraph companyName = new iTextPdf.Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", headerFont);
                        companyName.Alignment = iTextPdf.Element.ALIGN_CENTER;
                        doc.Add(companyName);

                        // Tiêu đề chính
                        iTextPdf.Paragraph title = new iTextPdf.Paragraph("DANH SÁCH DỰ ÁN", titleFont);
                        title.Alignment = iTextPdf.Element.ALIGN_CENTER;
                        title.SpacingBefore = 10f;
                        title.SpacingAfter = 10f;
                        doc.Add(title);

                        // Ngày lập
                        iTextPdf.Paragraph date = new iTextPdf.Paragraph("Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy"), smallFont);
                        date.Alignment = iTextPdf.Element.ALIGN_LEFT;
                        date.SpacingAfter = 20f;
                        doc.Add(date);

                        // Tạo bảng
                        PdfPTable table = new PdfPTable(dgvDA.Columns.Count);
                        table.WidthPercentage = 100;

                        // Thiết lập độ rộng cột
                        float[] columnWidths = new float[dgvDA.Columns.Count];
                        for (int i = 0; i < dgvDA.Columns.Count; i++)
                        {
                            columnWidths[i] = i == 0 ? 10f : (i == 1 ? 20f : 15f);
                        }
                        table.SetWidths(columnWidths);

                        // Header của bảng
                        for (int i = 0; i < dgvDA.Columns.Count; i++)
                        {
                            PdfPCell cell = new PdfPCell(new iTextPdf.Phrase(dgvDA.Columns[i].HeaderText, normalFont));
                            cell.BackgroundColor = iTextPdf.BaseColor.LIGHT_GRAY;
                            cell.HorizontalAlignment = iTextPdf.Element.ALIGN_CENTER;
                            cell.VerticalAlignment = iTextPdf.Element.ALIGN_MIDDLE;
                            cell.Padding = 8;
                            table.AddCell(cell);
                        }

                        // Dữ liệu
                        for (int i = 0; i < dgvDA.Rows.Count; i++)
                        {
                            for (int j = 0; j < dgvDA.Columns.Count; j++)
                            {
                                var cellValue = dgvDA.Rows[i].Cells[j].Value;
                                string displayValue = "";

                                if (cellValue is DateTime)
                                {
                                    displayValue = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    displayValue = cellValue?.ToString() ?? "";
                                }

                                PdfPCell cell = new PdfPCell(new iTextPdf.Phrase(displayValue, smallFont));
                                cell.HorizontalAlignment = j == 0 ? iTextPdf.Element.ALIGN_CENTER : iTextPdf.Element.ALIGN_LEFT;
                                cell.VerticalAlignment = iTextPdf.Element.ALIGN_MIDDLE;
                                cell.Padding = 5;
                                table.AddCell(cell);
                            }
                        }

                        doc.Add(table);

                        // Thống kê
                        iTextPdf.Paragraph stats = new iTextPdf.Paragraph("\nTổng số dự án: " + dgvDA.Rows.Count, normalFont);
                        stats.SpacingBefore = 15f;
                        stats.SpacingAfter = 20f;
                        doc.Add(stats);

                        // Chữ ký
                        PdfPTable signatureTable = new PdfPTable(2);
                        signatureTable.WidthPercentage = 100;
                        signatureTable.SetWidths(new float[] { 50f, 50f });

                        PdfPCell emptyCell = new PdfPCell(new iTextPdf.Phrase(""));
                        emptyCell.Border = iTextPdf.Rectangle.NO_BORDER;
                        signatureTable.AddCell(emptyCell);

                        PdfPCell signatureCell = new PdfPCell();
                        signatureCell.Border = iTextPdf.Rectangle.NO_BORDER;
                        signatureCell.HorizontalAlignment = iTextPdf.Element.ALIGN_CENTER;

                        iTextPdf.Paragraph signDate = new iTextPdf.Paragraph("Hà Nội, ngày " + DateTime.Now.Day + " tháng " +
                            DateTime.Now.Month + " năm " + DateTime.Now.Year, smallFont);
                        signDate.Alignment = iTextPdf.Element.ALIGN_CENTER;

                        iTextPdf.Paragraph signTitle = new iTextPdf.Paragraph("Người lập báo cáo", normalFont);
                        signTitle.Alignment = iTextPdf.Element.ALIGN_CENTER;
                        signTitle.SpacingBefore = 5f;
                        signTitle.SpacingAfter = 40f;

                        iTextPdf.Paragraph signName = new iTextPdf.Paragraph(nguoiDangNhap, normalFont);
                        signName.Alignment = iTextPdf.Element.ALIGN_CENTER;

                        signatureCell.AddElement(signDate);
                        signatureCell.AddElement(signTitle);
                        signatureCell.AddElement(signName);

                        signatureTable.AddCell(signatureCell);
                        doc.Add(signatureTable);

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