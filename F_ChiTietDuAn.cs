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
using static QuanLyNhanVien3.F_FormMain;

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
                string sqlLoadcomboBoxttblChiTietDuAn = "SELECT * FROM tblNhanVien_TuanhCd233018 WHERE DeletedAt = 0";
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
                string sqlLoadcomboBoxttblChiTietDuAn = "SELECT * FROM tblDuAn_KienCD233824 WHERE DeletedAt = 0";
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

                string sqlLoadDataChiTietDuAn = @"SELECT MaNV as 'Mã nhân viên', MaDA as 'Mã dự án', VaiTro as 'Vai trò', Ghichu as 'Ghi chú' FROM tblChiTietDuAn_KienCD233824 WHERE DeletedAt = 0 ORDER BY MaNV;";

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
            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnKhoiPhucDACu.Enabled = false;
                btnXemDACu.Enabled = false;
            }
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


                

                string sqltblChiTietDuAn = @"INSERT INTO tblChiTietDuAn_KienCD233824
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
                    string query = "UPDATE tblChiTietDuAn_KienCD233824 SET DeletedAt = 1 WHERE MaNV = @MaNV";
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
                    string sql = @"UPDATE tblChiTietDuAn_KienCD233824 SET MaDA = @MaDA,Vaitro =@Vaitro ,GhiChu = @GhiChu, DeletedAt = 0 WHERE MaNV = @MaNV";
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
                                FROM tblChiTietDuAn_KienCD233824
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
            if (dtGridViewChiTietDuAn.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook|*.xlsx";
                sfd.FileName = "ChiTietDuAn_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add("Chi tiết dự án");
                            int colCount = dtGridViewChiTietDuAn.Columns.Count;

                            /* ================= TIÊU ĐỀ ================= */
                            ws.Range(1, 1, 1, colCount).Merge();
                            ws.Cell(1, 1).Value = "BÁO CÁO CHI TIẾT DỰ ÁN";
                            ws.Cell(1, 1).Style.Font.Bold = true;
                            ws.Cell(1, 1).Style.Font.FontSize = 18;
                            ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            /* ================= NGÀY XUẤT ================= */
                            ws.Range(2, 1, 2, colCount).Merge();
                            ws.Cell(2, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy");
                            ws.Cell(2, 1).Style.Font.Italic = true;
                            ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            /* ================= HEADER ================= */
                            for (int i = 0; i < colCount; i++)
                            {
                                ws.Cell(4, i + 1).Value = dtGridViewChiTietDuAn.Columns[i].HeaderText;
                                ws.Cell(4, i + 1).Style.Font.Bold = true;
                                ws.Cell(4, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(4, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                            }

                            /* ================= DỮ LIỆU ================= */
                            for (int i = 0; i < dtGridViewChiTietDuAn.Rows.Count; i++)
                            {
                                for (int j = 0; j < colCount; j++)
                                {
                                    var value = dtGridViewChiTietDuAn.Rows[i].Cells[j].Value;
                                    ws.Cell(i + 5, j + 1).Value = value != null ? value.ToString() : "";
                                }
                            }

                            /* ================= BORDER ================= */
                            var range = ws.Range(4, 1,
                                dtGridViewChiTietDuAn.Rows.Count + 4, colCount);

                            range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                            /* ================= AUTO WIDTH ================= */
                            ws.Columns().AdjustToContents();

                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("Xuất báo cáo chi tiết dự án thành công!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnXemDACu_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @" SELECT MaNV, MaDA, VaiTro, Ghichu FROM tblChiTietDuAn_KienCD233824 WHERE DeletedAt = 1 ORDER BY MaNV;";
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
                string query = "SELECT COUNT(*) FROM tblChiTietDuAn_KienCD233824 WHERE MaNV = @MaNV AND DeletedAt = 1";
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

                string sqMKKhoiPhuc = "SELECT * FROM tblTaiKhoan_KhangCD233181 WHERE Quyen = @Quyen AND MatKhau = @MatKhau";
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
                    string querytblPhongBan = "UPDATE tblChiTietDuAn_KienCD233824 SET DeletedAt = 0 WHERE MaNV = @MaNV";
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

        private void buttonduan_Click(object sender, EventArgs e)
        {
            F_DuAn f = new F_DuAn();
            f.MdiParent = this.MdiParent;
            f.Show();
        }
    }
}
