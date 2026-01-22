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
using static QuanLyNhanVien3.F_FormMain;

namespace QuanLyNhanVien3
{
    public partial class F_PhongBan: Form
    {
        public F_PhongBan()
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

        private void LoadDataPhongBan()
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT 
                                                    MaPB_ThuanCD233318 AS [Mã Phòng Ban],
                                                    TenPB_ThuanCD233318 AS [Tên Phòng Ban],
                                                    DiaChi_ThuanCD233318 AS [Địa Chỉ],
                                                    SoDienThoai_ThuanCD233318 AS [Số Điện Thoại],
                                                    GhiChu_ThuanCD233318 AS [Ghi Chú]
                                                FROM tblPhongBan_ThuanCD233318
                                                WHERE DeletedAt_ThuanCD233318 = 0
                                                ORDER BY MaPB_ThuanCD233318;
                                                ";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewPhongBan.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
                tbMKkhoiphuc.UseSystemPasswordChar = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Phong Ban: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                if (
                    string.IsNullOrWhiteSpace(tbmaPB.Text) ||
                    string.IsNullOrWhiteSpace(tbTenPB.Text) ||
                    string.IsNullOrWhiteSpace(tbDiaChi.Text) ||
                    string.IsNullOrWhiteSpace(tbSoDienThoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double a;
                // cehck sdt
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


                // check ma phong ban
                string checkMaPBSql = "SELECT COUNT(*) FROM tblPhongBan_ThuanCD233318  WHERE MaPB_ThuanCD233318  = @MaPB_ThuanCD233318  AND DeletedAt_ThuanCD233318 = 0";
                using (SqlCommand cmdcheckMaPBSql = new SqlCommand(checkMaPBSql, cn.conn))
                {
                    cmdcheckMaPBSql.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text);
                    int MaPBCount = (int)cmdcheckMaPBSql.ExecuteScalar();

                    if (MaPBCount != 0)
                    {
                        MessageBox.Show("Mã phòng ban da tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                string checkdiachiTenPB = "SELECT COUNT(*) FROM tblPhongBan_ThuanCD233318  WHERE TenPB_ThuanCD233318  = @TenPB_ThuanCD233318 AND DiaChi_ThuanCD233318  = @DiaChi_ThuanCD233318";
                using (SqlCommand cmd = new SqlCommand(checkdiachiTenPB, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenPB_ThuanCD233318", tbTenPB.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiaChi_ThuanCD233318", tbDiaChi.Text.Trim());
                    int MaPBCount = (int)cmd.ExecuteScalar();

                    if (MaPBCount > 0)
                    {
                        MessageBox.Show("phòng ban nay da tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                string sqltblNhanVien = @"INSERT INTO tblPhongBan_ThuanCD233318 
                           (MaPB_ThuanCD233318, TenPB_ThuanCD233318,  DiaChi_ThuanCD233318, SoDienThoai_ThuanCD233318, Ghichu_ThuanCD233318, DeletedAt_ThuanCD233318)
                           VALUES ( @MaPB_ThuanCD233318, @TenPB_ThuanCD233318, @DiaChi_ThuanCD233318, @SoDienThoai_ThuanCD233318, @GhiChu_ThuanCD233318, 0)";

                using (SqlCommand cmd = new SqlCommand(sqltblNhanVien, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenPB_ThuanCD233318", tbTenPB.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiaChi_ThuanCD233318", tbDiaChi.Text.Trim());
                    cmd.Parameters.AddWithValue("@SoDienThoai_ThuanCD233318", tbSoDienThoai.Text.Trim());
                    cmd.Parameters.AddWithValue("@GhiChu_ThuanCD233318", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm phong ban thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.disconnect();
                        ClearAllInputs(this);
                        LoadDataPhongBan();
                    }
                    else
                    {
                        cn.disconnect();
                        MessageBox.Show("Thêm phòng ban thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                //MessageBox.Show("Chi tiết lỗi: " + ex.ToString(), "Lỗi hệ thống",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã phòng ban cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa phòng ban này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = "UPDATE tblPhongBan_ThuanCD233318 SET DeletedAt_ThuanCD233318 = 1 WHERE MaPB_ThuanCD233318 = @MaPB_ThuanCD233318";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa phòng ban thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataPhongBan();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy phòng ban để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text))
                {
                    MessageBox.Show("Vui lòng chọn phòng ban cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (
                    string.IsNullOrWhiteSpace(tbTenPB.Text) ||
                    string.IsNullOrWhiteSpace(tbDiaChi.Text) ||
                    string.IsNullOrWhiteSpace(tbSoDienThoai.Text))
                //string.IsNullOrWhiteSpace(tbGhiChu.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa phòng ban này không?",
                    "Xác nhận sửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string sql = @"UPDATE tblPhongBan_ThuanCD233318 SET TenPB = @TenPB_ThuanCD233318, DiaChi = @DiaChi_ThuanCD233318, SoDienThoai = @SoDienThoai_ThuanCD233318, GhiChu= @GhiChu_ThuanCD233318, DeletedAt_ThuanCD233318 = 0 WHERE MaPB = @MaPB_ThuanCD233318";
                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenPB_ThuanCD233318", tbTenPB.Text.Trim());
                        cmd.Parameters.AddWithValue("@DiaChi_ThuanCD233318", tbDiaChi.Text.Trim());
                        cmd.Parameters.AddWithValue("@SoDienThoai_ThuanCD233318", tbSoDienThoai.Text.Trim());
                        cmd.Parameters.AddWithValue("@GhiChu_ThuanCD233318", tbGhiChu.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataPhongBan();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Sửa phòng ban thất bại!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cn.disconnect();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi" + ex.Message);
            }
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã phòng ban để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); // hoặc mã
                    return;
                }
                cn.connect();
                string MaPBtimkiem = tbmaPB.Text.Trim();
                string sql = @" SELECT MaPB_ThuanCD233318, TenPB_ThuanCD233318, DiaChi_ThuanCD233318, SoDienThoai_ThuanCD233318, Ghichu_ThuanCD233318
                                FROM tblPhongBan_ThuanCD233318
                                WHERE DeletedAt_ThuanCD233318 = 0 AND MaPB_ThuanCD233318 LIKE @MaPB_ThuanCD233318
                                ORDER BY MaPB_ThuanCD233318";
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", "%" + MaPBtimkiem + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewPhongBan.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
            }
        }

        private void btnrestar_Click_1(object sender, EventArgs e)
        {
            LoadDataPhongBan();
        }

        private void btnxuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewPhongBan.Rows.Count > 0)
            {
                string fileName = $"BaoCaoPhongBan_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "Excel Workbook|*.xlsx",
                    FileName = fileName
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("PhongBan");

                                int colCount = dataGridViewPhongBan.Columns.Count;

                                /* ========== TIÊU ĐỀ ========= */
                                ws.Cell(1, 1).Value = "BÁO CÁO DANH SÁCH PHÒNG BAN";
                                ws.Range(1, 1, 1, colCount).Merge();
                                ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
                                ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 18;
                                ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Center;

                                /* ========== NGÀY XUẤT ========= */
                                ws.Cell(2, 1).Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                                ws.Range(2, 1, 2, colCount).Merge();
                                ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Center;
                                ws.Range(2, 1, 2, colCount).Style.Font.Italic = true;

                                /* ========== HEADER ========= */
                                for (int i = 0; i < colCount; i++)
                                {
                                    ws.Cell(4, i + 1).Value =
                                        dataGridViewPhongBan.Columns[i].HeaderText;

                                    ws.Cell(4, i + 1).Style.Font.Bold = true;
                                    ws.Cell(4, i + 1).Style.Alignment.Horizontal =
                                        XLAlignmentHorizontalValues.Center;
                                    ws.Cell(4, i + 1).Style.Fill.BackgroundColor =
                                        XLColor.LightGray;
                                }

                                /* ========== DỮ LIỆU ========= */
                                for (int i = 0; i < dataGridViewPhongBan.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dataGridViewPhongBan.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 5, j + 1).Value =
                                            value != null ? value.ToString() : "";
                                    }
                                }

                                /* ========== BORDER ========= */
                                var range = ws.Range(
                                    4, 1,
                                    dataGridViewPhongBan.Rows.Count + 4,
                                    colCount
                                );

                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                /* ========== AUTO SIZE ========= */
                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel phòng ban thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất file: " + ex.Message,
                                "Lỗi",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnHienThiPhongBanCu_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @" SELECT MaPB_ThuanCD233318, TenPB_ThuanCD233318, DiaChi_ThuanCD233318, SoDienThoai_ThuanCD233318, Ghichu_ThuanCD233318 FROM tblPhongBan_ThuanCD233318 WHERE DeletedAt_ThuanCD233318 =1 ORDER BY MaPB_ThuanCD233318";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewPhongBan.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnKhoiPhucPhongBan_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbmaPB.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Phong Ban cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                string query = "SELECT COUNT(*) FROM tblPhongBan_ThuanCD233318 WHERE MaPB = @MaPB_ThuanCD233318 AND DeletedAt_ThuanCD233318 = 1";
                using (SqlCommand cmdcheckPB = new SqlCommand(query, cn.conn))
                {
                    cmdcheckPB.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text.Trim());
                    int emailCount = (int)cmdcheckPB.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        MessageBox.Show("Ma PB này đã tồn tại trong hệ thống!", "Thông báo",
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

                string sqMKkhoiphuc = "SELECT * FROM tblTaiKhoan_KhangCD233181 WHERE Quyen = @Quyen AND MatKhau = @MatKhau_KhangCD233181";
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
                    string querytblPhongBan = "UPDATE tblPhongBan_ThuanCD233318 SET DeletedAt_ThuanCD233318 = 0 WHERE MaPB = @MaPB_ThuanCD233318";
                    using (SqlCommand cmd = new SqlCommand(querytblPhongBan, cn.conn))
                    {
                        // DELETE FROM tblNhanVien WHERE MaNV = @MaNV / UPDATE tblNhanVien SET DeletedAt = 1 WHERE MaNV = @MaNV
                        cmd.Parameters.AddWithValue("@MaPB_ThuanCD233318", tbmaPB.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("khôi phục phong ban thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            ClearAllInputs(this);
                            LoadDataPhongBan();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy nhân viên để khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                    //cn.disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex.Message);
                //MessageBox.Show("Chi tiết lỗi: " + ex.ToString(), "Lỗi hệ thống",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridViewPhongBan_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbmaPB.Text = dataGridViewPhongBan.Rows[i].Cells[0].Value.ToString();
                tbTenPB.Text = dataGridViewPhongBan.Rows[i].Cells[1].Value.ToString();
                tbDiaChi.Text = dataGridViewPhongBan.Rows[i].Cells[2].Value.ToString();
                tbSoDienThoai.Text = dataGridViewPhongBan.Rows[i].Cells[3].Value.ToString();
                tbGhiChu.Text = dataGridViewPhongBan.Rows[i].Cells[4].Value.ToString();
            }
        }

        private void F_PhongBan_Load(object sender, EventArgs e)
        {
            LoadDataPhongBan();
            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnHienThiPhongBanCu.Enabled = false;
                btnKhoiPhucPhongBan.Enabled = false;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbGhiChu_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbSoDienThoai_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tbDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbTenPB_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbmaPB_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
