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
    public partial class F_HopDong : Form
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
            // load nhan vien combobox
            try
            {
                cn.connect();
                string sqlLoadcomboBox = "SELECT * FROM tblNhanVien_TuanhCD233018";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBox, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbMaNV.DataSource = ds.Tables[0];
                    cbMaNV.DisplayMember = "MaNV_TuanhCD233018"; // cot hien thi
                    cbMaNV.ValueMember = "MaNV_TuanhCD233018"; // cot gia tri
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

                // Lưu ý: Trong bảng tblHopDong_ChienCD232928 không có trường DeletedAt
                // Tôi sẽ giả sử bảng này có trường DeletedAt_ChienCD232928 để phù hợp với logic của bạn
                // Nếu thực tế không có, bạn cần bỏ điều kiện WHERE
                string sqlLoadDataNhanVien = @"SELECT 
                                                    hd.Id_ChienCD232928 AS [ID],
                                                    hd.MaHopDong_ChienCD232928 AS [Mã Hợp Đồng], 
                                                    hd.MaNV_TuanhCD233018 AS [Mã Nhân Viên], 
                                                    hd.NgayBatDau_ChienCD232928 AS [Ngày Bắt Đầu], 
                                                    hd.NgayKetThuc_ChienCD232928 AS [Ngày Kết Thúc], 
                                                    hd.LoaiHopDong_ChienCD232928 AS [Loại Hợp Đồng], 
                                                    hd.LuongCoBan_ChienCD232928 AS [Lương Cơ Bản], 
                                                    hd.Ghichu_ChienCD232928 AS [Ghi Chú]
                                                FROM tblHopDong_ChienCD232928 AS hd
                                                -- JOIN tblNhanVien_TuanhCD233018 AS nv ON hd.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                                                -- WHERE hd.DeletedAt_ChienCD232928 = 0  -- Nếu có trường DeletedAt
                                                ORDER BY hd.MaHopDong_ChienCD232928;
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
                MessageBox.Show("Lỗi khi tải dữ liệu Hợp Đồng: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void F_HopDong_Load(object sender, EventArgs e)
        {
            LoadDataHopDong();
            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnKhoiPhucHDCu.Enabled = false;
                btnXemHDCu.Enabled = false;
            }
        }

        private void dtGridViewHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                tbMaHD.Text = dtGridViewHD.Rows[i].Cells[1].Value.ToString(); // Cell 1 là Mã Hợp Đồng
                cbMaNV.SelectedValue = dtGridViewHD.Rows[i].Cells[2].Value.ToString(); // Cell 2 là Mã NV
                DatePickerNgayBatDau.Value = Convert.ToDateTime(dtGridViewHD.Rows[i].Cells[3].Value);
                DatePickerNgayKetThuc.Value = Convert.ToDateTime(dtGridViewHD.Rows[i].Cells[4].Value);
                tbLoaiHD.Text = dtGridViewHD.Rows[i].Cells[5].Value.ToString();
                tbLuongCoBan.Text = dtGridViewHD.Rows[i].Cells[6].Value.ToString();
                tbGhiChu.Text = dtGridViewHD.Rows[i].Cells[7]?.Value?.ToString() ?? "";
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
                    string.IsNullOrWhiteSpace(tbLuongCoBan.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // check ma Hop Dong
                string checkMaDASql = "SELECT COUNT(*) FROM tblHopDong_ChienCD232928 WHERE MaHopDong_ChienCD232928 = @MaHopDong";
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
                if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double luong))
                {
                    MessageBox.Show("Lương cơ bản phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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

                string sqltblHopDong = @"INSERT INTO tblHopDong_ChienCD232928 
                           (MaHopDong_ChienCD232928, MaNV_TuanhCD233018, NgayBatDau_ChienCD232928, 
                           NgayKetThuc_ChienCD232928, LoaiHopDong_ChienCD232928, 
                           LuongCoBan_ChienCD232928, Ghichu_ChienCD232928)
                           VALUES (@MaHopDong, @MaNV, @NgayBatDau, @NgayKetThuc, @LoaiHopDong, @LuongCoBan, @GhiChu)";

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
                if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double luong))
                {
                    MessageBox.Show("Lương cơ bản phải là số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
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

                string checkMaHDSql = "SELECT COUNT(*) FROM tblHopDong_ChienCD232928 WHERE MaHopDong_ChienCD232928 = @MaHopDong";
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
                    string.IsNullOrWhiteSpace(tbLuongCoBan.Text))
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

                if (confirm == DialogResult.Yes)
                {
                    string sql = @"UPDATE tblHopDong_ChienCD232928 SET 
                              MaNV_TuanhCD233018 = @MaNV, 
                              NgayBatDau_ChienCD232928 = @NgayBatDau, 
                              NgayKetThuc_ChienCD232928 = @NgayKetThuc,
                              LoaiHopDong_ChienCD232928 = @LoaiHopDong, 
                              LuongCoBan_ChienCD232928 = @LuongCoBan, 
                              Ghichu_ChienCD232928 = @GhiChu
                              WHERE MaHopDong_ChienCD232928 = @MaHopDong";
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
                            MessageBox.Show("Sửa hợp đồng thất bại!", "Lỗi",
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
                    string query = "DELETE FROM tblHopDong_ChienCD232928 WHERE MaHopDong_ChienCD232928 = @MaHopDong";
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
                string MaNVtimkiem = cbMaNV.Text.Trim();

                if (string.IsNullOrEmpty(MaNVtimkiem))
                {
                    MessageBox.Show("Vui lòng nhập hoặc chọn Mã Nhân Viên để tìm kiếm!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();

                string sql = @"SELECT Id_ChienCD232928 AS [ID],
                                      MaHopDong_ChienCD232928 AS [Mã Hợp Đồng], 
                                      MaNV_TuanhCD233018 AS [Mã Nhân Viên], 
                                      NgayBatDau_ChienCD232928 AS [Ngày Bắt Đầu], 
                                      NgayKetThuc_ChienCD232928 AS [Ngày Kết Thúc], 
                                      LoaiHopDong_ChienCD232928 AS [Loại Hợp Đồng], 
                                      LuongCoBan_ChienCD232928 AS [Lương Cơ Bản], 
                                      Ghichu_ChienCD232928 AS [Ghi Chú]
                               FROM tblHopDong_ChienCD232928
                               WHERE MaNV_TuanhCD233018 LIKE @MaNV
                               ORDER BY MaHopDong_ChienCD232928";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", "%" + MaNVtimkiem + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dtGridViewHD.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXemHDCu_Click_1(object sender, EventArgs e)
        {
            // Nếu bảng không có cột DeletedAt, chức năng này không khả dụng
            MessageBox.Show("Chức năng xem hợp đồng đã xóa không khả dụng vì bảng không hỗ trợ soft delete.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnKhoiPhucHDCu_Click_1(object sender, EventArgs e)
        {
            // Nếu bảng không có cột DeletedAt, chức năng này không khả dụng
            MessageBox.Show("Chức năng khôi phục hợp đồng không khả dụng vì bảng không hỗ trợ soft delete.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string fileName = "BaoCaoHopDong_" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";

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
                                var ws = wb.Worksheets.Add("HopDong");
                                int colCount = dtGridViewHD.Columns.Count;

                                /* ================= TIÊU ĐỀ ================= */
                                ws.Cell(1, 1).Value = "BÁO CÁO HỢP ĐỒNG";
                                ws.Range(1, 1, 1, colCount).Merge();
                                ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
                                ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 18;
                                ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                /* ================= NGÀY XUẤT ================= */
                                ws.Cell(2, 1).Value = "Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                                ws.Range(2, 1, 2, colCount).Merge();
                                ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                ws.Range(2, 1, 2, colCount).Style.Font.Italic = true;

                                /* ================= HEADER ================= */
                                for (int i = 0; i < colCount; i++)
                                {
                                    ws.Cell(4, i + 1).Value = dtGridViewHD.Columns[i].HeaderText;
                                }

                                var headerRange = ws.Range(4, 1, 4, colCount);
                                headerRange.Style.Font.Bold = true;
                                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                /* ================= DỮ LIỆU ================= */
                                for (int i = 0; i < dtGridViewHD.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        ws.Cell(i + 5, j + 1).Value =
                                            dtGridViewHD.Rows[i].Cells[j].Value?.ToString() ?? "";
                                    }
                                }

                                /* ================= BORDER + AUTOFIT ================= */
                                var dataRange = ws.Range(4, 1,
                                    dtGridViewHD.Rows.Count + 4,
                                    colCount);

                                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                ws.Columns().AdjustToContents();

                                /* ================= LƯU FILE ================= */
                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất Excel: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}