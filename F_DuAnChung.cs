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
    public partial class F_DuAnChung : Form
    {
        public F_DuAnChung()
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
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Dự Án: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Chi Tiết Dự Án: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    WHERE DeletedAt_TuanhCD233018 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbMaNV.DataSource = dt;
                    cbMaNV.DisplayMember = "MaNV_TuanhCD233018";
                    cbMaNV.ValueMember = "MaNV_TuanhCD233018";
                    cbMaNV.SelectedIndex = -1;

                    // Load tên nhân viên
                    tbTenNhanVien.DataBindings.Clear();
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu mã NV: " + ex.Message);
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
                    WHERE DeletedAt_KienCD233824 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbMaDuAn.DataSource = dt;
                    cbMaDuAn.DisplayMember = "MaDA_KienCD233824";
                    cbMaDuAn.ValueMember = "MaDA_KienCD233824";
                    cbMaDuAn.SelectedIndex = -1;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu mã DA: " + ex.Message);
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

                // Kiểm tra ngày bắt đầu và ngày kết thúc
                if (DatePickerNgayBatDau.Value > DatePickerNgayKetThuc.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();

                // Kiểm tra mã dự án đã tồn tại
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
                        cn.disconnect();
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
                        cn.disconnect();
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
                        WHERE MaDA_KienCD233824 = @MaDA";

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
                            cn.disconnect();
                            LoadDataDuAn();
                            LoadComboBoxMaDA();
                            ClearAllInputs(groupBox2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            cn.disconnect();
                            LoadDataDuAn();
                            LoadComboBoxMaDA();
                            ClearAllInputs(groupBox2);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy dự án để xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbmaDA.Text = dgvDA.Rows[i].Cells[0].Value.ToString();
                tbTenDA.Text = dgvDA.Rows[i].Cells[1].Value.ToString();
                tbMota.Text = dgvDA.Rows[i].Cells[2].Value.ToString();
                DatePickerNgayBatDau.Value = Convert.ToDateTime(dgvDA.Rows[i].Cells[3].Value);
                DatePickerNgayKetThuc.Value = Convert.ToDateTime(dgvDA.Rows[i].Cells[4].Value);
                tbGhiChuDA.Text = dgvDA.Rows[i].Cells[5].Value.ToString();
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

                // Kiểm tra xem nhân viên đã tham gia dự án này chưa
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
                        cn.disconnect();
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
                        cn.disconnect();
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
                        SET MaDA_KienCD233824 = @MaDA,
                            VaiTro_KienCD233824 = @VaiTro,
                            GhiChu_KienCD233824 = @GhiChu
                        WHERE MaNV_TuanhCD233018 = @MaNV 
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
                            cn.disconnect();
                            LoadDataChiTietDuAn();
                            ClearAllInputs(groupBox1);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy chi tiết dự án để sửa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaCTDA_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbMaNV.SelectedIndex == -1)
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
                            cn.disconnect();
                            LoadDataChiTietDuAn();
                            ClearAllInputs(groupBox1);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy chi tiết dự án để xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {

        }

        private void dgvChiTietDA_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                cbMaNV.SelectedValue = dgvChiTietDA.Rows[i].Cells[0].Value.ToString();
                tbTenNhanVien.Text = dgvChiTietDA.Rows[i].Cells[1].Value.ToString();
                cbMaDuAn.SelectedValue = dgvChiTietDA.Rows[i].Cells[2].Value.ToString();
                tbVaiTro.Text = dgvChiTietDA.Rows[i].Cells[3].Value.ToString();
                tbGhiChuCTDA.Text = dgvChiTietDA.Rows[i].Cells[4].Value.ToString();
            }
        }

        

        private void F_DuAnChung_Load(object sender, EventArgs e)
        {
            LoadDataDuAn();
            LoadDataChiTietDuAn();
            LoadComboBoxMaNV();
            LoadComboBoxMaDA();
        }
    }
}
