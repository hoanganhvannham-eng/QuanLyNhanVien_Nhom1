using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
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

        // Biến để tránh trigger event khi đang cập nhật programmatically
        private bool isUpdating = false;

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
                string sqlLoadcomboBox = "SELECT MaNV_TuanhCD233018, HoTen_TuanhCD233018 FROM tblNhanVien_TuanhCD233018 WHERE DeletedAt_TuanhCD233018 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBox, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbMaNV.DataSource = ds.Tables[0];
                    cbMaNV.DisplayMember = "MaNV_TuanhCD233018"; // cot hien thi
                    cbMaNV.ValueMember = "MaNV_TuanhCD233018"; // cot gia tri
                }
                cn.disconnect();
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

                // Chỉ hiển thị hợp đồng chưa bị xóa (DeletedAt = 0)
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
                                                WHERE hd.DeletedAt_ChienCD232928 = 0
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

            // Đăng ký event handlers
            cbMaNV.SelectedIndexChanged += cbMaNV_SelectedIndexChanged;
            tbTenNV.TextChanged += tbTenNV_TextChanged;
            tbTenNV.Leave += tbTenNV_Leave;

            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                btnKhoiPhucHDCu.Enabled = false;
                btnXemHDCu.Enabled = false;
            }
        }

        // ============= EVENT: KHI CHỌN MÃ NHÂN VIÊN TRONG COMBOBOX =============
        private void cbMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdating) return; // Tránh vòng lặp vô hạn

            if (cbMaNV.SelectedIndex == -1) return;

            try
            {
                isUpdating = true;

                string maNV = cbMaNV.SelectedValue.ToString();

                // Lấy tên nhân viên từ mã nhân viên
                cn.connect();
                string sqlGetName = "SELECT HoTen_TuanhCD233018 FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV AND DeletedAt_TuanhCD233018 = 0";

                using (SqlCommand cmd = new SqlCommand(sqlGetName, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        tbTenNV.Text = result.ToString();
                    }
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tên nhân viên: " + ex.Message);
            }
            finally
            {
                isUpdating = false;
            }
        }

        // ============= EVENT: KHI NHẬP TÊN NHÂN VIÊN (REAL-TIME SEARCH) =============
        private void tbTenNV_TextChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;

            // Tìm kiếm gợi ý khi người dùng đang nhập
            if (string.IsNullOrWhiteSpace(tbTenNV.Text)) return;

            try
            {
                // Chỉ tìm kiếm, không cập nhật combobox ngay
                // Để tránh làm gián đoạn việc nhập liệu
            }
            catch (Exception ex)
            {
                // Silent error
            }
        }

        // ============= EVENT: KHI RỜI KHỎI Ô TÊN NHÂN VIÊN =============
        private void tbTenNV_Leave(object sender, EventArgs e)
        {
            if (isUpdating) return;

            if (string.IsNullOrWhiteSpace(tbTenNV.Text)) return;

            try
            {
                isUpdating = true;

                string tenNV = tbTenNV.Text.Trim();

                // Tìm mã nhân viên từ tên (tìm chính xác hoặc gần đúng)
                cn.connect();

                // Tìm chính xác trước
                string sqlGetMa = @"
                    SELECT TOP 1 MaNV_TuanhCD233018 
                    FROM tblNhanVien_TuanhCD233018 
                    WHERE HoTen_TuanhCD233018 = @TenNV 
                    AND DeletedAt_TuanhCD233018 = 0";

                using (SqlCommand cmd = new SqlCommand(sqlGetMa, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenNV", tenNV);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        // Tìm thấy chính xác
                        string maNV = result.ToString();
                        cbMaNV.SelectedValue = maNV;
                    }
                    else
                    {
                        // Nếu không tìm thấy chính xác, tìm gần đúng
                        string sqlGetMaLike = @"
                            SELECT TOP 1 MaNV_TuanhCD233018, HoTen_TuanhCD233018
                            FROM tblNhanVien_TuanhCD233018 
                            WHERE HoTen_TuanhCD233018 LIKE @TenNV 
                            AND DeletedAt_TuanhCD233018 = 0";

                        using (SqlCommand cmdLike = new SqlCommand(sqlGetMaLike, cn.conn))
                        {
                            cmdLike.Parameters.AddWithValue("@TenNV", "%" + tenNV + "%");
                            using (SqlDataReader reader = cmdLike.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string maNV = reader["MaNV_TuanhCD233018"].ToString();
                                    string hoTen = reader["HoTen_TuanhCD233018"].ToString();

                                    cbMaNV.SelectedValue = maNV;
                                    tbTenNV.Text = hoTen; // Cập nhật lại tên đầy đủ
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy nhân viên có tên: " + tenNV,
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    tbTenNV.Clear();
                                    cbMaNV.SelectedIndex = -1;
                                }
                            }
                        }
                    }
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm mã nhân viên: " + ex.Message);
            }
            finally
            {
                isUpdating = false;
            }
        }

        private void dtGridViewHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                isUpdating = true; // Tắt trigger khi đang load từ grid

                tbMaHD.Text = dtGridViewHD.Rows[i].Cells[1].Value.ToString(); // Cell 1 là Mã Hợp Đồng

                string maNV = dtGridViewHD.Rows[i].Cells[2].Value.ToString(); // Cell 2 là Mã NV
                cbMaNV.SelectedValue = maNV;

                // Lấy tên nhân viên
                try
                {
                    cn.connect();
                    string sqlGetName = "SELECT HoTen_TuanhCD233018 FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV AND DeletedAt_TuanhCD233018 = 0";

                    using (SqlCommand cmd = new SqlCommand(sqlGetName, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            tbTenNV.Text = result.ToString();
                        }
                    }
                    cn.disconnect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy tên nhân viên: " + ex.Message);
                }

                DatePickerNgayBatDau.Value = Convert.ToDateTime(dtGridViewHD.Rows[i].Cells[3].Value);
                DatePickerNgayKetThuc.Value = Convert.ToDateTime(dtGridViewHD.Rows[i].Cells[4].Value);
                tbLoaiHD.Text = dtGridViewHD.Rows[i].Cells[5].Value.ToString();
                tbLuongCoBan.Text = dtGridViewHD.Rows[i].Cells[6].Value.ToString();
                tbGhiChu.Text = dtGridViewHD.Rows[i].Cells[7]?.Value?.ToString() ?? "";

                isUpdating = false; // Bật lại trigger
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

                // check ma Hop Dong (kiểm tra cả hợp đồng đã xóa)
                string checkMaDASql = "SELECT COUNT(*) FROM tblHopDong_ChienCD232928 WHERE MaHopDong_ChienCD232928 = @MaHopDong AND DeletedAt_ChienCD232928 = 0";
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
                           LuongCoBan_ChienCD232928, Ghichu_ChienCD232928, DeletedAt_ChienCD232928)
                           VALUES (@MaHopDong, @MaNV, @NgayBatDau, @NgayKetThuc, @LoaiHopDong, @LuongCoBan, @GhiChu, 0)";

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

                string checkMaHDSql = "SELECT COUNT(*) FROM tblHopDong_ChienCD232928 WHERE MaHopDong_ChienCD232928 = @MaHopDong AND DeletedAt_ChienCD232928 = 0";
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
                              WHERE MaHopDong_ChienCD232928 = @MaHopDong AND DeletedAt_ChienCD232928 = 0";
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

        // ============= XÓA MỀM =============
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

                    // Xóa mềm: Cập nhật DeletedAt = 1
                    string query = "UPDATE tblHopDong_ChienCD232928 SET DeletedAt_ChienCD232928 = 1 WHERE MaHopDong_ChienCD232928 = @MaHopDong AND DeletedAt_ChienCD232928 = 0";

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
                               WHERE MaNV_TuanhCD233018 LIKE @MaNV AND DeletedAt_ChienCD232928 = 0
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

        // ============= XEM HỢP ĐỒNG ĐÃ XÓA =============
        private void btnXemHDCu_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                // Lấy danh sách hợp đồng đã xóa (DeletedAt = 1)
                string sqlLoadDeletedHD = @"SELECT 
                                                hd.Id_ChienCD232928 AS [ID],
                                                hd.MaHopDong_ChienCD232928 AS [Mã Hợp Đồng], 
                                                hd.MaNV_TuanhCD233018 AS [Mã Nhân Viên], 
                                                hd.NgayBatDau_ChienCD232928 AS [Ngày Bắt Đầu], 
                                                hd.NgayKetThuc_ChienCD232928 AS [Ngày Kết Thúc], 
                                                hd.LoaiHopDong_ChienCD232928 AS [Loại Hợp Đồng], 
                                                hd.LuongCoBan_ChienCD232928 AS [Lương Cơ Bản], 
                                                hd.Ghichu_ChienCD232928 AS [Ghi Chú]
                                            FROM tblHopDong_ChienCD232928 AS hd
                                            WHERE hd.DeletedAt_ChienCD232928 = 1
                                            ORDER BY hd.MaHopDong_ChienCD232928;";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDeletedHD, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dtGridViewHD.DataSource = dt;
                        MessageBox.Show($"Đã tìm thấy {dt.Rows.Count} hợp đồng đã xóa!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không có hợp đồng nào đã bị xóa!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem hợp đồng đã xóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============= KHÔI PHỤC HỢP ĐỒNG =============
        private void btnKhoiPhucHDCu_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMaHD.Text))
                {
                    MessageBox.Show("Vui lòng chọn mã hợp đồng cần khôi phục!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Yêu cầu nhập mật khẩu xác nhận
                if (string.IsNullOrEmpty(tbMKKhoiPhuc.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu để xác nhận khôi phục!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string matKhauNhap = tbMKKhoiPhuc.Text.Trim();

                // Kiểm tra mật khẩu với RoleId = 1 (Admin)
                cn.connect();

                string sqlCheckPassword = @"
                    SELECT COUNT(*) 
                    FROM tblTaiKhoan_KhangCD233181 
                    WHERE MatKhau_KhangCD233181 = @MatKhau 
                    AND RoleId_ThuanCD233318 = 1 
                    AND DeletedAt_KhangCD233181 = 0";

                bool isValidAdmin = false;

                using (SqlCommand cmdCheck = new SqlCommand(sqlCheckPassword, cn.conn))
                {
                    cmdCheck.Parameters.AddWithValue("@MatKhau", matKhauNhap);
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        isValidAdmin = true;
                    }
                }

                if (!isValidAdmin)
                {
                    MessageBox.Show("Mật khẩu không đúng hoặc bạn không có quyền Admin để khôi phục!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cn.disconnect();
                    tbMKKhoiPhuc.Clear();
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn khôi phục Hợp Đồng này không?",
                    "Xác nhận khôi phục",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    // Kiểm tra xem hợp đồng có tồn tại trong danh sách đã xóa không
                    string checkDeletedSql = @"
                        SELECT COUNT(*) 
                        FROM tblHopDong_ChienCD232928 
                        WHERE MaHopDong_ChienCD232928 = @MaHopDong 
                        AND DeletedAt_ChienCD232928 = 1";

                    using (SqlCommand cmdCheckDeleted = new SqlCommand(checkDeletedSql, cn.conn))
                    {
                        cmdCheckDeleted.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text);
                        int deletedCount = (int)cmdCheckDeleted.ExecuteScalar();

                        if (deletedCount == 0)
                        {
                            MessageBox.Show("Hợp đồng này không tồn tại trong danh sách đã xóa!",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                            return;
                        }
                    }

                    // Khôi phục: Cập nhật DeletedAt = 0
                    string query = @"
                        UPDATE tblHopDong_ChienCD232928 
                        SET DeletedAt_ChienCD232928 = 0 
                        WHERE MaHopDong_ChienCD232928 = @MaHopDong 
                        AND DeletedAt_ChienCD232928 = 1";

                    using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHopDong", tbMaHD.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Khôi phục Hợp Đồng thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            cn.disconnect();
                            LoadDataHopDong(); // Load lại danh sách hợp đồng hiện tại
                            ClearAllInputs(this);
                            tbMKKhoiPhuc.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Không thể khôi phục Hợp Đồng!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cn.disconnect();
                        }
                    }
                }
                else
                {
                    cn.disconnect();
                }
            }
            catch (Exception ex)
            {
                try { cn.disconnect(); } catch { }
                MessageBox.Show("Lỗi khi khôi phục hợp đồng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // ============= HÀM LẤY THÔNG TIN NGƯỜI XUẤT =============
        private (string HoTen, string ChucVu, string PhongBan) LayThongTinNguoiXuat()
        {
            try
            {
                cn.connect();

                string maNVHienTai = "";

                string sqlFirstAccount = @"
                    SELECT TOP 1 MaNV_TuanhCD233018 
                    FROM tblTaiKhoan_KhangCD233181 
                    WHERE DeletedAt_KhangCD233181 = 0";

                using (SqlCommand cmdFirst = new SqlCommand(sqlFirstAccount, cn.conn))
                {
                    object firstResult = cmdFirst.ExecuteScalar();
                    if (firstResult != null)
                    {
                        maNVHienTai = firstResult.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(maNVHienTai))
                {
                    string sqlNguoiXuat = @"
                        SELECT 
                            nv.HoTen_TuanhCD233018 as HoTen,
                            ISNULL(cv.TenCV_KhangCD233181, 'Nhân viên') as ChucVu,
                            ISNULL(pb.TenPB_ThuanCD233318, 'Phòng Nhân sự') as PhongBan
                        FROM tblTaiKhoan_KhangCD233181 tk
                        INNER JOIN tblNhanVien_TuanhCD233018 nv 
                            ON tk.MaNV_TuanhCD233018 = nv.MaNV_TuanhCD233018
                        LEFT JOIN tblChucVu_KhangCD233181 cv 
                            ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                        LEFT JOIN tblPhongBan_ThuanCD233318 pb 
                            ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                        WHERE tk.MaNV_TuanhCD233018 = @MaNV
                        AND tk.DeletedAt_KhangCD233181 = 0
                        AND (nv.DeletedAt_TuanhCD233018 = 0 OR nv.DeletedAt_TuanhCD233018 IS NULL)";

                    using (SqlCommand cmdNguoiXuat = new SqlCommand(sqlNguoiXuat, cn.conn))
                    {
                        cmdNguoiXuat.Parameters.AddWithValue("@MaNV", maNVHienTai);
                        using (SqlDataReader reader = cmdNguoiXuat.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hoTen = reader["HoTen"].ToString();
                                string chucVu = reader["ChucVu"].ToString();
                                string phongBan = reader["PhongBan"].ToString();

                                cn.disconnect();
                                return (hoTen, chucVu, phongBan);
                            }
                        }
                    }
                }

                cn.disconnect();
                return ("Người xuất", "Trưởng phòng", "Phòng Nhân sự");
            }
            catch (Exception)
            {
                try { cn.disconnect(); } catch { }
                return ("Người xuất", "Trưởng phòng", "Phòng Nhân sự");
            }
        }

        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            try
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
                        cn.connect();
                        var nguoiXuatInfo = LayThongTinNguoiXuat();

                        string hoTenNguoiXuat = nguoiXuatInfo.HoTen;
                        string chucVuNguoiXuat = nguoiXuatInfo.ChucVu;
                        string phongBanNguoiXuat = nguoiXuatInfo.PhongBan;

                        string sqlHopDong = @"
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY hd.MaHopDong_ChienCD232928) AS [ID],
                                hd.MaHopDong_ChienCD232928 AS [Mã Hợp Đồng],
                                hd.MaNV_TuanhCD233018 AS [Mã Nhân Viên],
                                CONVERT(VARCHAR, hd.NgayBatDau_ChienCD232928, 103) + ' 12:00:00 AM' AS [Ngày Bắt Đầu],
                                CASE 
                                    WHEN hd.NgayKetThuc_ChienCD232928 IS NULL THEN 'Không xác định'
                                    ELSE CONVERT(VARCHAR, hd.NgayKetThuc_ChienCD232928, 103) + ' 12:00:00 AM'
                                END AS [Ngày Kết Thúc],
                                ISNULL(hd.LoaiHopDong_ChienCD232928, '') AS [Loại Hợp Đồng],
                                FORMAT(ISNULL(hd.LuongCoBan_ChienCD232928, 0), 'N2') AS [Lương Cơ Bản],
                                ISNULL(hd.Ghichu_ChienCD232928, '') AS [Ghi Chú]
                            FROM tblHopDong_ChienCD232928 hd
                            WHERE hd.DeletedAt_ChienCD232928 = 0
                            ORDER BY hd.MaHopDong_ChienCD232928";

                        DataTable dtHopDong = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(sqlHopDong, cn.conn))
                        {
                            da.Fill(dtHopDong);
                        }

                        cn.disconnect();

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add("HopDong");

                            ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                            ws.Range(1, 1, 1, 7).Merge();
                            ws.Range(1, 1, 1, 7).Style.Font.Bold = true;
                            ws.Range(1, 1, 1, 7).Style.Font.FontSize = 14;
                            ws.Range(1, 1, 1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Cell(2, 1).Value = "BÁO CÁO HỢP ĐỒNG";
                            ws.Range(2, 1, 2, 7).Merge();
                            ws.Range(2, 1, 2, 7).Style.Font.Bold = true;
                            ws.Range(2, 1, 2, 7).Style.Font.FontSize = 16;
                            ws.Range(2, 1, 2, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                            ws.Cell(3, 1).Value = "Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy");
                            ws.Range(3, 1, 3, 7).Merge();
                            ws.Range(3, 1, 3, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            ws.Range(3, 1, 3, 7).Style.Font.FontSize = 11;

                            ws.Cell(5, 1).Value = "Phòng " + phongBanNguoiXuat;
                            ws.Range(5, 1, 5, 7).Merge();
                            ws.Range(5, 1, 5, 7).Style.Font.Bold = true;
                            ws.Range(5, 1, 5, 7).Style.Font.FontSize = 12;
                            ws.Range(5, 1, 5, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            ws.Cell(6, 1).Value = "Chức vụ | " + chucVuNguoiXuat;
                            ws.Range(6, 1, 6, 7).Merge();
                            ws.Range(6, 1, 6, 7).Style.Font.Bold = true;
                            ws.Range(6, 1, 6, 7).Style.Font.FontSize = 12;
                            ws.Range(6, 1, 6, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            int startRow = 8;
                            string[] headers = { "ID", "Mã Hợp Đồng", "Ngày Bắt Đầu", "Ngày Kết Thúc", "Loại Hợp Đồng", "Lương Cơ Bản", "Ghi Chú" };

                            for (int i = 0; i < headers.Length; i++)
                            {
                                ws.Cell(startRow, i + 1).Value = headers[i];
                                ws.Cell(startRow, i + 1).Style.Font.Bold = true;
                                ws.Cell(startRow, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(startRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                ws.Cell(startRow, i + 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            }

                            if (dtHopDong.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtHopDong.Rows.Count; i++)
                                {
                                    DataRow row = dtHopDong.Rows[i];

                                    ws.Cell(startRow + i + 1, 1).Value = row["ID"].ToString();
                                    ws.Cell(startRow + i + 1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    ws.Cell(startRow + i + 1, 2).Value = row["Mã Hợp Đồng"].ToString();
                                    ws.Cell(startRow + i + 1, 3).Value = row["Ngày Bắt Đầu"].ToString();
                                    ws.Cell(startRow + i + 1, 4).Value = row["Ngày Kết Thúc"].ToString();
                                    ws.Cell(startRow + i + 1, 5).Value = row["Loại Hợp Đồng"].ToString();
                                    ws.Cell(startRow + i + 1, 6).Value = row["Lương Cơ Bản"].ToString();
                                    ws.Cell(startRow + i + 1, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    ws.Cell(startRow + i + 1, 7).Value = row["Ghi Chú"].ToString();

                                    for (int j = 1; j <= 7; j++)
                                    {
                                        ws.Cell(startRow + i + 1, j).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                    }
                                }
                            }
                            else
                            {
                                ws.Cell(startRow + 1, 1).Value = "Không có dữ liệu hợp đồng";
                                ws.Range(startRow + 1, 1, startRow + 1, 7).Merge();
                                ws.Range(startRow + 1, 1, startRow + 1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            }

                            ws.Columns().AdjustToContents();

                            if (dtHopDong.Rows.Count > 0)
                            {
                                ws.Column(6).Style.NumberFormat.Format = "#,##0.00";
                            }

                            int lastRow = startRow + Math.Max(dtHopDong.Rows.Count, 1) + 2;

                            ws.Range(lastRow, 1, lastRow, 7).Merge();
                            ws.Range(lastRow, 1, lastRow, 7).Style.Border.TopBorder = XLBorderStyleValues.Thin;

                            ws.Cell(lastRow + 1, 1).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                            ws.Range(lastRow + 1, 1, lastRow + 1, 7).Merge();
                            ws.Range(lastRow + 1, 1, lastRow + 1, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            ws.Range(lastRow + 1, 1, lastRow + 1, 7).Style.Font.Italic = true;

                            ws.Cell(lastRow + 2, 1).Value = "Người xuất";
                            ws.Range(lastRow + 2, 1, lastRow + 2, 7).Merge();
                            ws.Range(lastRow + 2, 1, lastRow + 2, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                            ws.Cell(lastRow + 3, 1).Value = hoTenNguoiXuat;
                            ws.Range(lastRow + 3, 1, lastRow + 3, 7).Merge();
                            ws.Range(lastRow + 3, 1, lastRow + 3, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            ws.Range(lastRow + 3, 1, lastRow + 3, 7).Style.Font.Bold = true;

                            wb.SaveAs(sfd.FileName);

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "BaoCaoHopDong_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".pdf";

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "PDF Document|*.pdf",
                    FileName = fileName
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        cn.connect();
                        var nguoiXuatInfo = LayThongTinNguoiXuat();

                        string hoTenNguoiXuat = nguoiXuatInfo.HoTen;
                        string chucVuNguoiXuat = nguoiXuatInfo.ChucVu;
                        string phongBanNguoiXuat = nguoiXuatInfo.PhongBan;

                        string sqlHopDong = @"
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY hd.MaHopDong_ChienCD232928) AS [ID],
                                hd.MaHopDong_ChienCD232928 AS [Mã Hợp Đồng],
                                hd.MaNV_TuanhCD233018 AS [Mã Nhân Viên],
                                CONVERT(VARCHAR, hd.NgayBatDau_ChienCD232928, 103) + ' 12:00:00 AM' AS [Ngày Bắt Đầu],
                                CASE 
                                    WHEN hd.NgayKetThuc_ChienCD232928 IS NULL THEN 'Không xác định'
                                    ELSE CONVERT(VARCHAR, hd.NgayKetThuc_ChienCD232928, 103) + ' 12:00:00 AM'
                                END AS [Ngày Kết Thúc],
                                ISNULL(hd.LoaiHopDong_ChienCD232928, '') AS [Loại Hợp Đồng],
                                FORMAT(ISNULL(hd.LuongCoBan_ChienCD232928, 0), 'N2') AS [Lương Cơ Bản],
                                ISNULL(hd.Ghichu_ChienCD232928, '') AS [Ghi Chú]
                            FROM tblHopDong_ChienCD232928 hd
                            WHERE hd.DeletedAt_ChienCD232928 = 0
                            ORDER BY hd.MaHopDong_ChienCD232928";

                        DataTable dtHopDong = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(sqlHopDong, cn.conn))
                        {
                            da.Fill(dtHopDong);
                        }

                        cn.disconnect();

                        if (dtHopDong.Rows.Count == 0)
                        {
                            MessageBox.Show("Không có dữ liệu hợp đồng để xuất!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                        PdfWriter.GetInstance(document, new FileStream(sfd.FileName, FileMode.Create));
                        document.Open();

                        string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.ttf");
                        BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL);
                        iTextSharp.text.Font fontSmall = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);
                        iTextSharp.text.Font fontItalic = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.ITALIC);
                        iTextSharp.text.Font fontTableHeader = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);
                        iTextSharp.text.Font fontTableData = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.NORMAL);

                        Paragraph company = new Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontHeader);
                        company.Alignment = Element.ALIGN_CENTER;
                        company.SpacingAfter = 5f;
                        document.Add(company);

                        Paragraph title = new Paragraph("BÁO CÁO HỢP ĐỒNG", fontTitle);
                        title.Alignment = Element.ALIGN_CENTER;
                        title.SpacingAfter = 10f;
                        document.Add(title);

                        Paragraph date = new Paragraph("Ngày lập báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy"), fontSmall);
                        date.Alignment = Element.ALIGN_LEFT;
                        date.SpacingAfter = 15f;
                        document.Add(date);

                        Paragraph phongBan = new Paragraph("Phòng " + phongBanNguoiXuat, fontNormal);
                        phongBan.SpacingAfter = 5f;
                        document.Add(phongBan);

                        Paragraph chucVu = new Paragraph("Chức vụ | " + chucVuNguoiXuat, fontNormal);
                        chucVu.SpacingAfter = 15f;
                        document.Add(chucVu);

                        int colCount = 7;
                        PdfPTable table = new PdfPTable(colCount);
                        table.WidthPercentage = 100;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 10f;

                        string[] pdfHeaders = { "ID", "Mã Hợp Đồng", "Ngày Bắt Đầu", "Ngày Kết Thúc", "Loại Hợp Đồng", "Lương Cơ Bản", "Ghi Chú" };

                        foreach (string header in pdfHeaders)
                        {
                            PdfPCell headerCell = new PdfPCell(new Phrase(header, fontTableHeader));
                            headerCell.BackgroundColor = new BaseColor(211, 211, 211);
                            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            headerCell.Padding = 5;
                            headerCell.BorderWidth = 1;
                            table.AddCell(headerCell);
                        }

                        for (int i = 0; i < dtHopDong.Rows.Count; i++)
                        {
                            DataRow row = dtHopDong.Rows[i];

                            PdfPCell idCell = new PdfPCell(new Phrase(row["ID"].ToString(), fontTableData));
                            idCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            idCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            idCell.Padding = 5;
                            idCell.BorderWidth = 1;
                            table.AddCell(idCell);

                            PdfPCell maHDCell = new PdfPCell(new Phrase(row["Mã Hợp Đồng"].ToString(), fontTableData));
                            maHDCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            maHDCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            maHDCell.Padding = 5;
                            maHDCell.BorderWidth = 1;
                            table.AddCell(maHDCell);

                            PdfPCell ngayBatDauCell = new PdfPCell(new Phrase(row["Ngày Bắt Đầu"].ToString(), fontTableData));
                            ngayBatDauCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            ngayBatDauCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            ngayBatDauCell.Padding = 5;
                            ngayBatDauCell.BorderWidth = 1;
                            table.AddCell(ngayBatDauCell);

                            PdfPCell ngayKetThucCell = new PdfPCell(new Phrase(row["Ngày Kết Thúc"].ToString(), fontTableData));
                            ngayKetThucCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            ngayKetThucCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            ngayKetThucCell.Padding = 5;
                            ngayKetThucCell.BorderWidth = 1;
                            table.AddCell(ngayKetThucCell);

                            PdfPCell loaiHDCell = new PdfPCell(new Phrase(row["Loại Hợp Đồng"].ToString(), fontTableData));
                            loaiHDCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            loaiHDCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            loaiHDCell.Padding = 5;
                            loaiHDCell.BorderWidth = 1;
                            table.AddCell(loaiHDCell);

                            string luong = row["Lương Cơ Bản"].ToString();
                            PdfPCell luongCell = new PdfPCell(new Phrase(luong, fontTableData));
                            luongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            luongCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            luongCell.Padding = 5;
                            luongCell.BorderWidth = 1;
                            table.AddCell(luongCell);

                            PdfPCell ghiChuCell = new PdfPCell(new Phrase(row["Ghi Chú"].ToString(), fontTableData));
                            ghiChuCell.HorizontalAlignment = Element.ALIGN_LEFT;
                            ghiChuCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            ghiChuCell.Padding = 5;
                            ghiChuCell.BorderWidth = 1;
                            table.AddCell(ghiChuCell);
                        }

                        document.Add(table);

                        Paragraph line = new Paragraph();
                        line.SpacingBefore = 20f;
                        document.Add(line);

                        PdfPTable lineTable = new PdfPTable(1);
                        lineTable.WidthPercentage = 100;
                        PdfPCell lineCell = new PdfPCell();
                        lineCell.BorderWidthTop = 1f;
                        lineCell.BorderWidthBottom = 0f;
                        lineCell.BorderWidthLeft = 0f;
                        lineCell.BorderWidthRight = 0f;
                        lineCell.FixedHeight = 1f;
                        lineTable.AddCell(lineCell);
                        document.Add(lineTable);

                        Paragraph dateFooter = new Paragraph("Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year, fontItalic);
                        dateFooter.Alignment = Element.ALIGN_RIGHT;
                        dateFooter.SpacingAfter = 5f;
                        document.Add(dateFooter);

                        Paragraph signerLabel = new Paragraph("Người xuất", fontNormal);
                        signerLabel.Alignment = Element.ALIGN_RIGHT;
                        signerLabel.SpacingAfter = 20f;
                        document.Add(signerLabel);

                        Paragraph signerName = new Paragraph(hoTenNguoiXuat, fontHeader);
                        signerName.Alignment = Element.ALIGN_RIGHT;
                        document.Add(signerName);

                        document.Close();

                        MessageBox.Show("Xuất PDF thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        System.Diagnostics.Process.Start(sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}