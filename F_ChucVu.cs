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
    public partial class F_ChucVu: Form
    {

        connectData c = new connectData();
        //void loaddata()
        //{
        //    dgvHienThiChucVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        //    c.connect();
        //    DataSet data = new DataSet();
        //    string query = "select MaCV as N'Mã Chức Vụ', TenCV as N'Tên Chức Vụ', GhiChu as N'Ghi chú' " +
        //        "from tblChucVu WHERE DeletedAt IS NULL OR DeletedAt = 0";

        //    SqlDataAdapter sqlData = new SqlDataAdapter(query, c.conn);
        //    sqlData.Fill(data);
        //    dgvHienThiChucVu.DataSource = data.Tables[0];
        //    c.disconnect();
        //}
        public F_ChucVu()
        {
            InitializeComponent();
        }

        private void F_ChucVu_Load(object sender, EventArgs e)
        {
            LoadDataChucVu();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
        }


        //public void clear_form()
        //{
        //    cbMaChucVu.Text = "";
        //    txtTenChucVu.Text = "";
        //    txtGhiChu.Text = "";
        //    cbMaChucVu.Focus();
        //}

        private void btnSua_Click(object sender, EventArgs e)
        {
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
        }


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
        //private void LoadcbChucVu()
        //{
        //    // load chuc vu combobox
        //    try
        //    {
        //        c.connect();
        //        string sqlLoadcomboBoxttblChucVu = "SELECT MaCV  as 'Mã chức vụ', TenCV as 'Tên chức vụ', Ghichu as 'Ghi chú' FROM tblChucVu WHERE DeletedAt = 0";
        //        using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxttblChucVu, c.conn))
        //        {
        //            DataSet ds = new DataSet();
        //            da.Fill(ds);

        //            cbMaChucVu.DataSource = ds.Tables[0];
        //            cbMaChucVu.DisplayMember = "MaCV"; // cot hien thi
        //            cbMaChucVu.ValueMember = "MaCV"; // cot gia tri
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi tải dữ liệu mã Chức Vụ: " + ex.Message);
        //    }
        //}

        private void LoadDataChucVu()
        {
            try
            {
                c.connect();

                string sqlLoadDataChiTietDuAn = @"SELECT MaCV as N'Mã Chức Vụ' , TenCV as N'Tên Chức Vụ',Ghichu as N'Ghi chú' FROM tblChucVu WHERE DeletedAt = 0 ORDER BY MaCV;";
                //    string query = "select MaCV as N'Mã Chức Vụ', TenCV as N'Tên Chức Vụ', GhiChu as N'Ghi chú' " +
                //        "from tblChucVu WHERE DeletedAt IS NULL OR DeletedAt = 0";
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataChiTietDuAn, c.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvHienThiChucVu.DataSource = dt;
                }
                c.disconnect();
                ClearAllInputs(this);
                txtMKKhoiPhuc.UseSystemPasswordChar = true;
                //LoadcbChucVu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Chi Tiết Dự Án: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void btnThem_Click_1(object sender, EventArgs e)
        {
            if (tbMaChuVu.Text == "" || txtTenChucVu.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                c.connect();
                string query = "insert into tblChucVu(MaCV,TenCV,GhiChu) " +
                        "values ('" + tbMaChuVu.Text + "',N'" + txtTenChucVu.Text + "',N'" + txtGhiChu.Text + "')";
                bool kq = c.exeSQL(query);
                MessageBox.Show("Thêm thành công!!", "Thông báo", MessageBoxButtons.OK);
                LoadDataChucVu();
                ClearAllInputs(this);
                c.disconnect();
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (tbMaChuVu.Text == "" || txtTenChucVu.Text == "")
            {
                MessageBox.Show("Chưa nhập đủ thông tin", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                c.connect();

                string query = "update tblChucVu set " +
                               "TenCV = N'" + txtTenChucVu.Text + "', " +
                               "GhiChu = N'" + txtGhiChu.Text + "' " +
                               "where MaCV = '" + tbMaChuVu.Text + "'";

                bool kq = c.exeSQL(query);
                if (kq)
                    MessageBox.Show("Sửa thành công!!", "Thông báo", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Sửa thất bại!!", "Thông báo", MessageBoxButtons.OK);

                LoadDataChucVu();
                ClearAllInputs(this);

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
                    string query = "UPDATE tblChucVu SET DeletedAt = 1 WHERE MaCV = @MaCV";
                    using (SqlCommand cmd = new SqlCommand(query, c.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa Chức Vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            c.disconnect();
                            LoadDataChucVu();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Chức Vụ để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            c.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string MaNVtimkiem = tbMaChuVu.Text.Trim();
                string sql = @" SELECT MaCV as N'Mã Chức Vụ',TenCV as N'Tên Chức Vụ',Ghichu as N'Ghi chú'
                                FROM tblChucVu
                                WHERE DeletedAt = 0 AND MaCV LIKE @MaCV
                                ORDER BY MaCV";
                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    cmd.Parameters.AddWithValue("@MaCV", "%" + MaNVtimkiem + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvHienThiChucVu.DataSource = dt;
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
            LoadDataChucVu();
        }

        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dgvHienThiChucVu.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("ChucVu");

                                // Ghi header
                                for (int i = 0; i < dgvHienThiChucVu.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dgvHienThiChucVu.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dgvHienThiChucVu.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dgvHienThiChucVu.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dgvHienThiChucVu.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dgvHienThiChucVu.Rows.Count + 1, dgvHienThiChucVu.Columns.Count);
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

        private void btnHienThiNVNghiViec_Click_1(object sender, EventArgs e)
        {
            try
            {
                c.connect();
                string query = @" SELECT MaCV as N'Mã Chức Vụ',TenCV as N'Tên Chức Vụ', Ghichu as N'Ghi Chú' FROM tblChucVu WHERE DeletedAt = 1 ORDER BY MaCV;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, c.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvHienThiChucVu.DataSource = dt;
                }
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
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Chức Vụ để tìm Chức vụ  cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                c.connect();
                string query = "SELECT COUNT(*) FROM tblChucVu WHERE MaCV = @MaCV AND DeletedAt = 1";
                using (SqlCommand cmdcheckPB = new SqlCommand(query, c.conn))
                {
                    cmdcheckPB.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                    int emailCount = (int)cmdcheckPB.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        MessageBox.Show("Mã Chức Vụ này đã tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        c.disconnect();
                        return;
                    }
                }

                //
                if (txtMKKhoiPhuc.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu để khôi phục", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Question);
                    return;
                }

                string sqMKKhoiPhuc = "SELECT * FROM tblTaiKhoan WHERE Quyen = @Quyen AND MatKhau = @MatKhau";
                SqlCommand cmdkhoiphuc = new SqlCommand(sqMKKhoiPhuc, c.conn);
                cmdkhoiphuc.Parameters.AddWithValue("@Quyen", "Admin");
                cmdkhoiphuc.Parameters.AddWithValue("@MatKhau", txtMKKhoiPhuc.Text);
                SqlDataReader reader = cmdkhoiphuc.ExecuteReader();

                if (reader.Read() == false)
                {
                    MessageBox.Show("mật khẩu không đúng? Vui lòng nhập lại mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    txtMKKhoiPhuc.Text = "";
                    reader.Close();
                    c.disconnect();
                    return;
                }
                reader.Close();


                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn khôi phục chi tiết dữ liệu dự án này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


                if (confirm == DialogResult.Yes)
                {
                    txtMKKhoiPhuc.Text = "";
                    string querytblPhongBan = "UPDATE tblChucVu SET DeletedAt = 0 WHERE MaCV = @MaCV";
                    using (SqlCommand cmd = new SqlCommand(querytblPhongBan, c.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaCV", tbMaChuVu.Text.Trim());
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("khôi phục Chức Vụ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            c.disconnect();
                            ClearAllInputs(this);
                            LoadDataChucVu();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Chức Vụ để khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            c.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);

            }
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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvHienThiChucVu_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int i = dgvHienThiChucVu.CurrentRow.Index;
            tbMaChuVu.Text = dgvHienThiChucVu.Rows[i].Cells[0].Value.ToString();
            txtTenChucVu.Text = dgvHienThiChucVu.Rows[i].Cells[1].Value.ToString();
            txtGhiChu.Text = dgvHienThiChucVu.Rows[i].Cells[2].Value.ToString();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
