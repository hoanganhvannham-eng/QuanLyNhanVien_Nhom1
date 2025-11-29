using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_ChiTietDuAn: Form
    {
        public F_ChiTietDuAn()
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
        private void LoadcbNV()
        {
            // load chuc vu combobox
            try
            {
                cn.connect();
                string sqlLoadcomboBoxttblChiTietDuAn = "SELECT * FROM tblNhanVien WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxttblChiTietDuAn, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbMaNV.DataSource = ds.Tables[0];
                    cbMaNV.DisplayMember = "MaNV"; // cot hien thi
                    cbMaNV.ValueMember = "MaNV"; // cot gia tri
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu mã NV: " + ex.Message);
            }
        }
        private void LoadcbDA()
        {
            // load chuc vu combobox
            try
            {
                cn.connect();
                string sqlLoadcomboBoxttblChiTietDuAn = "SELECT * FROM tblDuAn WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxttblChiTietDuAn, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbMaDuAn.DataSource = ds.Tables[0];
                    cbMaDuAn.DisplayMember = "MaDA"; // cot hien thi
                    cbMaDuAn.ValueMember = "MaDA"; // cot gia tri
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu mã DA: " + ex.Message);
            }
        }

        private void LoadDataChiTietDuAn()
        {
            try
            {
                cn.connect();

                string sqlLoadDataChiTietDuAn = @"SELECT MaNV as 'Mã nhân viên', MaDA as 'Mã dự án', VaiTro as 'Vai trò', Ghichu as 'Ghi chú' FROM tblChiTietDuAn WHERE DeletedAt = 0 ORDER BY MaNV;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataChiTietDuAn, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewChiTietDuAn.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
                tbMKKhoiPhuc.UseSystemPasswordChar = true;
                LoadcbDA();
                LoadcbNV();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Chi Tiết Dự Án: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void F_ChiTietDA_Load(object sender, EventArgs e)
        {
            LoadDataChiTietDuAn();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                if (
                    string.IsNullOrWhiteSpace(cbMaNV.Text)||
                    cbMaDuAn.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbVaiTro.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }




                // check ma Nhan vien
                //string checkMaDASql = "SELECT COUNT(*) FROM tblChiTietDuAn  WHERE MaNV  = @MaNV  AND DeletedAt = 0";
                //using (SqlCommand cmdcheckMaDASql = new SqlCommand(checkMaDASql, cn.conn))
                //{
                //    cmdcheckMaDASql.Parameters.AddWithValue("@MaNV", cbMaNV.Text);
                //    int MaHDCount = (int)cmdcheckMaDASql.ExecuteScalar();

                //    if (MaHDCount != 0)
                //    {
                //        MessageBox.Show("Mã Nhân Viên đã tồn tại trong hệ thống!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        cn.disconnect();
                //        return;
                //    }
                //}


                

                string sqltblChiTietDuAn = @"INSERT INTO tblChiTietDuAn
                           (MaNV, MaDA, VaiTro, Ghichu, DeletedAt)
                           VALUES ( @MaNV, @MaDA, @VaiTro, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(sqltblChiTietDuAn, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                    cmd.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                    cmd.Parameters.AddWithValue("@VaiTro", tbVaiTro.Text.Trim());
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm chi tiết dự án thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.disconnect();
                        ClearAllInputs(this);
                        LoadDataChiTietDuAn();
                    }
                    else
                    {
                        cn.disconnect();
                        MessageBox.Show("Thêm chi tiết dự án thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Nhân Viên cho chi tiết dự án cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa chi tiết dự án này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = "UPDATE tblChiTietDuAn SET DeletedAt = 1 WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", cbMaNV.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa Chi Tiết Dự Án thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataChiTietDuAn();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Chi Tiết Dự Án để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                if (
                    cbMaNV.SelectedIndex == -1 ||
                    cbMaDuAn.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbVaiTro.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }




                // check ma Nhan vien
                //string checkMaDASql = "SELECT COUNT(*) FROM tblChiTietDuAn  WHERE MaNV  = @MaNV  AND DeletedAt = 0";
                //using (SqlCommand cmdcheckMaDASql = new SqlCommand(checkMaDASql, cn.conn))
                //{
                //    cmdcheckMaDASql.Parameters.AddWithValue("@MaNV", cbMaNV.Text);
                //    int MaHDCount = (int)cmdcheckMaDASql.ExecuteScalar();

                //    if (MaHDCount != 0)
                //    {
                //        MessageBox.Show("Mã Nhân Viên đã tồn tại trong hệ thống!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        cn.disconnect();
                //        return;
                //    }
                //}




                DialogResult confirm = MessageBox.Show(
                   "Bạn có chắc chắn muốn sửa chi tiết dự án này không?",
                   "Xác nhận sửa",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question
               );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string sql = @"UPDATE tblChiTietDuAn SET MaDA = @MaDA,Vaitro =@Vaitro ,GhiChu = @GhiChu, DeletedAt = 0 WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                        cmd.Parameters.AddWithValue("@MaDA", cbMaDuAn.SelectedValue);
                        cmd.Parameters.AddWithValue("@Vaitro", tbVaiTro.Text.Trim());
                        cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataChiTietDuAn();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Sửa chi tiết dự án thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cn.disconnect();
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Lỗi" + ex.Message);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbMaNV.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã Nhân Viên để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); // hoặc mã
                    return;
                }
                cn.connect();
                string MaNVtimkiem = cbMaNV.Text.Trim();
                string sql = @" SELECT MaNV, MaDA, VaiTro, Ghichu
                                FROM tblChiTietDuAn
                                WHERE DeletedAt = 0 AND MaNV LIKE @MaNV
                                ORDER BY MaNV";
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", "%" + MaNVtimkiem + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewChiTietDuAn.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDataChiTietDuAn();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewChiTietDuAn.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("ChiTietDuAn");

                                // Ghi header
                                for (int i = 0; i < dtGridViewChiTietDuAn.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewChiTietDuAn.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewChiTietDuAn.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewChiTietDuAn.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dtGridViewChiTietDuAn.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dtGridViewChiTietDuAn.Rows.Count + 1, dtGridViewChiTietDuAn.Columns.Count);
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

        private void btnXemDACu_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @" SELECT MaNV, MaDA, VaiTro, Ghichu FROM tblChiTietDuAn WHERE DeletedAt = 1 ORDER BY MaNV;";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewChiTietDuAn.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnKhoiPhucDACu_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbMaNV.Text.Trim()))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Nhân Viên để tìm chi tiết dự án cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                string query = "SELECT COUNT(*) FROM tblChiTietDuAn WHERE MaNV = @MaNV AND DeletedAt = 1";
                using (SqlCommand cmdcheckPB = new SqlCommand(query, cn.conn))
                {
                    cmdcheckPB.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                    int emailCount = (int)cmdcheckPB.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        MessageBox.Show("Mã nhân viên quản lí chi tiết dự án này đã tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                //
                if (tbMKKhoiPhuc.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu để khôi phục", "Thông báo", MessageBoxButtons.OK,
                    MessageBoxIcon.Question);
                    return;
                }

                string sqMKKhoiPhuc = "SELECT * FROM tblTaiKhoan WHERE Quyen = @Quyen AND MatKhau = @MatKhau";
                SqlCommand cmdkhoiphuc = new SqlCommand(sqMKKhoiPhuc, cn.conn);
                cmdkhoiphuc.Parameters.AddWithValue("@Quyen", "Admin");
                cmdkhoiphuc.Parameters.AddWithValue("@MatKhau", tbMKKhoiPhuc.Text);
                SqlDataReader reader = cmdkhoiphuc.ExecuteReader();

                if (reader.Read() == false)
                {
                    MessageBox.Show("mật khẩu không đúng? Vui lòng nhập lại mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    tbMKKhoiPhuc.Text = "";
                    reader.Close();
                    cn.disconnect();
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
                    tbMKKhoiPhuc.Text = "";
                    string querytblPhongBan = "UPDATE tblChiTietDuAn SET DeletedAt = 0 WHERE MaNV = @MaNV";
                    using (SqlCommand cmd = new SqlCommand(querytblPhongBan, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("khôi phục chi tiết dữ liệu dự án thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            ClearAllInputs(this);
                            LoadDataChiTietDuAn();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy chi tiết dữ liệu dự án để khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
                
            }
        }
        

        private void CheckHienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckHienMK.Checked)
            {
                tbMKKhoiPhuc.UseSystemPasswordChar = false;
            }
            else
            {
                tbMKKhoiPhuc.UseSystemPasswordChar = true;
            }
        }

        private void dtGridViewChiTietDuAn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                cbMaNV.SelectedValue = dtGridViewChiTietDuAn.Rows[i].Cells[0].Value.ToString();
                cbMaDuAn.SelectedValue = dtGridViewChiTietDuAn.Rows[i].Cells[1].Value.ToString();
                tbVaiTro.Text = dtGridViewChiTietDuAn.Rows[i].Cells[2].Value.ToString();
                tbGhiChu.Text = dtGridViewChiTietDuAn.Rows[i].Cells[3].Value.ToString();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
