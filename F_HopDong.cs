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
    public partial class F_HopDong: Form
    {
        public F_HopDong()
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


        private void LoadcomboBox()
        {
            // load chuc vu combobox
            try
            {
                cn.connect();
                string sqsqlLoadcomboBoxttblChucVu = "SELECT * FROM tblNhanVien WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqsqlLoadcomboBoxttblChucVu, cn.conn))
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
                MessageBox.Show("Lỗi load ma NV: " + ex.Message);
            }
        }
        private void LoadDataHopDong()
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT 
                                                    MaHopDong AS [Mã Hợp Đồng], 
                                                    hd.MaNV AS [Mã Nhân Viên], 
                                                    NgayBatDau AS [Ngày Bắt Đầu], 
                                                    NgayKetThuc AS [Ngày Kết Thúc], 
                                                    LoaiHopDong AS [Loại Hợp Đồng], 
                                                    LuongCoBan AS [Lương Cơ Bản], 
                                                    hd.GhiChu AS [Ghi Chú]
                                                FROM tblHopDong AS hd
                                                JOIN tblNhanVien AS nv ON hd.MaNV = nv.MaNV
                                                WHERE nv.DeletedAt = 0
                                                ORDER BY MaHopDong;
                                                ";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewHD.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
                tbMKKhoiPhuc.UseSystemPasswordChar = true;
                LoadcomboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Dự Án: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void F_HopDong_Load(object sender, EventArgs e)
        {
            LoadDataHopDong();

        }

        private void dtGridViewHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbMaHD.Text = dtGridViewHD.Rows[i].Cells[0].Value.ToString();
                cbMaNV.SelectedValue = dtGridViewHD.Rows[i].Cells[1].Value.ToString();
                DatePickerNgayBatDau.Value = Convert.ToDateTime(dtGridViewHD.Rows[i].Cells[2].Value);
                DatePickerNgayKetThuc.Value = Convert.ToDateTime(dtGridViewHD.Rows[i].Cells[3].Value);
                tbLoaiHD.Text = dtGridViewHD.Rows[i].Cells[4].Value.ToString();
                tbLuongCoBan.Text = dtGridViewHD.Rows[i].Cells[5].Value.ToString();
                tbGhiChu.Text = dtGridViewHD.Rows[i].Cells[6].Value.ToString();
            }
        }
   private void btnThem_Click_2(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                if (
                    string.IsNullOrWhiteSpace(tbMaHD.Text) ||
                    cbMaNV.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbLoaiHD.Text) ||
                    string.IsNullOrWhiteSpace(tbLuongCoBan.Text) ||
                    string.IsNullOrWhiteSpace(tbLoaiHD.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }




                // check ma Hop Dong
                string checkMaDASql = "SELECT COUNT(*) FROM tblHopDong  WHERE MaHopDong  = @MaHopDong  AND DeletedAt = 0";
                using (SqlCommand cmdcheckMaDASql = new SqlCommand(checkMaDASql, cn.conn))
                {
                    cmdcheckMaDASql.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text);
                    int MaHDCount = (int)cmdcheckMaDASql.ExecuteScalar();

                    if (MaHDCount != 0)
                    {
                        MessageBox.Show("Mã Hợp Đồng đã tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }


                //Kiểm Tra trong lương cơ bản 
                string input = tbLuongCoBan.Text.Trim();
                // 2. Kiểm tra có phải số hay không (cho phép dấu . hoặc , theo hệ thống)
                if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double luong))
                {
                    MessageBox.Show("Lương cơ bản phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // 3. Kiểm tra phải > 0
                if (luong <= 0)
                {
                    MessageBox.Show("Lương cơ bản phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Kiểm tra ngày bắt đầu và ngày kết thúc
                if (DatePickerNgayBatDau.Value > DatePickerNgayKetThuc.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // end check

                string sqltblHopDong = @"INSERT INTO tblHopDong 
                           (MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu, DeletedAt)
                           VALUES ( @MaHopDong, @MaNV, @NgayBatDau, @NgayKetThuc, @LoaiHopDong, @LuongCoBan, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(sqltblHopDong, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                    cmd.Parameters.AddWithValue("@NgayBatDau", DatePickerNgayBatDau.Value);
                    cmd.Parameters.AddWithValue("@NgayKetThuc", DatePickerNgayKetThuc.Value);
                    cmd.Parameters.AddWithValue("@LoaiHopDong", tbLoaiHD.Text.Trim());
                    cmd.Parameters.AddWithValue("@LuongCoBan", tbLuongCoBan.Text.Trim());
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm hợp đồng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.disconnect();
                        ClearAllInputs(this);
                        LoadDataHopDong();
                    }
                    else
                    {
                        cn.disconnect();
                        MessageBox.Show("Thêm hợp đồng thất bại!", "Lỗi",
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

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                if (string.IsNullOrEmpty(tbMaHD.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã hợp đồng cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cn.disconnect();
                    return;
                }

                //Kiểm Tra trong lương cơ bản 
                string input = tbLuongCoBan.Text.Trim();
                // 2. Kiểm tra có phải số hay không (cho phép dấu . hoặc , theo hệ thống)
                if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double luong))
                {
                    MessageBox.Show("Lương cơ bản phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // 3. Kiểm tra phải > 0
                if (luong <= 0)
                {
                    MessageBox.Show("Lương cơ bản phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Kiểm tra ngày bắt đầu và ngày kết thúc
                if (DatePickerNgayBatDau.Value > DatePickerNgayKetThuc.Value)
                {
                    MessageBox.Show("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // end check

                string checkMaHDSql = "SELECT COUNT(*) FROM tblHopDong WHERE MaHopDong = @MaHopDong AND DeletedAt = 0";
                using (SqlCommand cmd = new SqlCommand(checkMaHDSql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text.Trim());
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        MessageBox.Show("Mã hợp đồng này không tồn tại trong hệ thống!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }
                if (
                    string.IsNullOrWhiteSpace(tbMaHD.Text) ||
                    cbMaNV.SelectedIndex == -1 ||
                    string.IsNullOrWhiteSpace(tbLoaiHD.Text) ||
                    string.IsNullOrWhiteSpace(tbLuongCoBan.Text) ||
                    string.IsNullOrWhiteSpace(tbLoaiHD.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa Hợp Đồng này không?",
                    "Xác nhận sửa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                // NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu, DeletedAt)
                // @NgayBatDau, @NgayKetThuc, @LoaiHopDong, @LuongCoBan, @GhiChu, 0)"

                if (confirm == DialogResult.Yes)
                {
                    string sql = @"UPDATE tblHopDong SET  MaHopDong = @MaHopDong, MaNV = @MaNV, NgayBatDau = @NgayBatDau, NgayKetThuc = @NgayKetThuc,
                              LoaiHopDong = @LoaiHopDong, LuongCoBan = @LuongCoBan, GhiChu= @GhiChu, DeletedAt = 0 WHERE MaHopDong = @MaHopDong";
                    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue);
                        cmd.Parameters.AddWithValue("@NgayBatDau", DatePickerNgayBatDau.Value);
                        cmd.Parameters.AddWithValue("@NgayKetThuc", DatePickerNgayKetThuc.Value);
                        cmd.Parameters.AddWithValue("@LoaiHopDong", tbLoaiHD.Text.Trim());
                        cmd.Parameters.AddWithValue("@LuongCoBan", tbLuongCoBan.Text.Trim());
                        cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadDataHopDong();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Sửa hợp đông thất bại!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("lỗi" + ex.Message);
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaHD.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Hợp Đồng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa Hợp Đồng này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    cn.connect();
                    string query = "UPDATE tblHopDong SET DeletedAt = 1 WHERE MaHopDong = @MaHopDong";
                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa Hợp Đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataHopDong();
                            ClearAllInputs(this);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Hợp Đồng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadDataHopDong();
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaHD.Text))
                {
                    MessageBox.Show("Vui lòng nhập mã Hợp Đồng để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); // hoặc mã
                    return;
                }
                cn.connect();
                string MaHDtimkiem = tbMaHD.Text.Trim();
                string sql = @" SELECT MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu
                                FROM tblHopDong
                                WHERE DeletedAt = 0 AND MaHopDong LIKE @MaHopDong
                                ORDER BY MaHopDong";
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaHopDong", "%" + MaHDtimkiem + "%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewHD.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
            }
        }

        private void btnXemHDCu_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();
                string query = @" SELECT MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu FROM tblHopDong WHERE DeletedAt =1 ORDER BY MaHopDong";
                using (SqlDataAdapter da = new SqlDataAdapter(query, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewHD.DataSource = dt;
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnKhoiPhucHDCu_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaHD.Text))
                {
                    MessageBox.Show("Vui lòng chọn hoặc nhập mã Hợp Đồng cần khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                string query = "SELECT COUNT(*) FROM tblHopDong WHERE MaHopDong = @MaHopDong AND DeletedAt = 1";
                using (SqlCommand cmdcheckPB = new SqlCommand(query, cn.conn))
                {
                    cmdcheckPB.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text.Trim());
                    int emailCount = (int)cmdcheckPB.ExecuteScalar();

                    if (emailCount == 0)
                    {
                        MessageBox.Show("Mã Hợp Đồng này đã tồn tại trong hệ thống!", "Thông báo",
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
                    "Bạn có chắc chắn muốn khôi phục Hợp Đồng này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );


                if (confirm == DialogResult.Yes)
                {
                    tbMKKhoiPhuc.Text = "";
                    string querytblPhongBan = "UPDATE tblHopDong SET DeletedAt = 0 WHERE MaHopDong = @MaHopDong";
                    using (SqlCommand cmd = new SqlCommand(querytblPhongBan, cn.conn))
                    {
                        // DELETE FROM tblNhanVien WHERE MaNV = @MaNV / UPDATE tblNhanVien SET DeletedAt = 1 WHERE MaNV = @MaNV
                        cmd.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("khôi phục Hợp Đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            ClearAllInputs(this);
                            LoadDataHopDong();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy Hợp Đồng để khôi phục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                    //cn.disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message);
                //MessageBox.Show("Chi tiết lỗi: " + ex.ToString(), "Lỗi hệ thống",
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkHienMK_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkHienMK.Checked)
            {
                tbMKKhoiPhuc.UseSystemPasswordChar = false;
            }
            else
            {
                tbMKKhoiPhuc.UseSystemPasswordChar = true;
            }
        }

        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dtGridViewHD.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("HopDong");

                                // Ghi header
                                for (int i = 0; i < dtGridViewHD.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewHD.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewHD.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewHD.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dtGridViewHD.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border cho toàn bảng
                                var range = ws.Range(1, 1, dtGridViewHD.Rows.Count + 1, dtGridViewHD.Columns.Count);
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

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
