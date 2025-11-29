using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_Luong : Form
    {


        public F_Luong()
        {
            InitializeComponent();
        }

        private void F_Luong_Load(object sender, EventArgs e)
        {
            LoadDataLuong();
            cbMaNV.SelectedIndexChanged += cbMaNV_SelectedIndexChanged;
            InitThangNam();
        }
        private void InitThangNam()
        {
            // ComboBox Tháng
            cbThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                cbThang.Items.Add(i);
            }
            cbThang.SelectedIndex = DateTime.Now.Month - 1;

            // NumericUpDown Năm
            numNam.Minimum = 2000;
            numNam.Maximum = 2100;
            numNam.Value = DateTime.Now.Year;
        }

        connectData cn = new connectData();
        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).Clear();

                else if (ctl is ComboBox cb)
                {
                    // Nếu ComboBox đang bind DataSource thì không clear
                    if (cb.DataSource == null)
                        cb.SelectedIndex = -1;
                }
                else if (ctl is DateTimePicker dtp)
                    dtp.Value = DateTime.Now;

                else if (ctl is NumericUpDown nud)
                    nud.Value = nud.Minimum; // hoặc giữ nguyên tùy ý

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

        private void LoadcbLuong()
        {
            // load chuc vu combobox
            try
            {
                cn.connect();
                string sqlLoadcomboBoxttblLuong = "SELECT * FROM tblLuong WHERE DeletedAt = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxttblLuong, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbMaLuong.DataSource = ds.Tables[0];
                    cbMaLuong.DisplayMember = "MaLuong"; // cot hien thi
                    cbMaLuong.ValueMember = "MaLuong"; // cot gia tri
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu mã Lương: " + ex.Message);
            }
        }

        private void LoadDataLuong()
        {
            try
            {
                cn.connect();

                string sqlLoadDataLuong =
                    @"SELECT l.MaLuong as N'Mã Lương', l.MaNV as N'Mã Nhân Viên',
                                Thang as N'Tháng', Nam as N'Năm',
                                LuongCoBan as N'Lương Cơ Bản', SoNgayCong as N'Số Ngày Công',
                                PhuCap as N'Phụ cấp', KhauTru as N'Khấu Trừ', l.Ghichu as N'Ghi Chú',
                                TongLuong as N'Tổng Lương'
                         FROM tblLuong as l ,tblNhanVien as nv
                         WHERE nv.DeletedAt = 0 and l.MaNV = nv.MaNV";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataLuong, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvLuong.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
                LoadcbLuong();
                LoadcbNV();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Bảng Lương: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private string GenerateMaLuong()
        {
            string ma = "";
            try
            {
                cn.connect();

                string sql = @"SELECT TOP 1 MaLuong 
                       FROM tblLuong 
                       WHERE Thang = @Thang AND Nam = @Nam 
                       ORDER BY MaLuong DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", (int)cbThang.SelectedItem);
                    cmd.Parameters.AddWithValue("@Nam", (int)numNam.Value);

                    object result = cmd.ExecuteScalar();

                    int nextNumber = 1;

                    if (result != null && result != DBNull.Value)
                    {
                        string lastMa = result.ToString(); // vd: L202509003
                        string lastSeq = lastMa.Substring(lastMa.Length - 3); // 003
                        if (int.TryParse(lastSeq, out int seq))
                        {
                            nextNumber = seq + 1;
                        }
                    }

                    ma = "L" + numNam.Value.ToString("0000") +
                         ((int)cbThang.SelectedItem).ToString("00") +
                         nextNumber.ToString("000");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sinh mã lương: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }

            return ma;
        }
        private void dgvLuong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click vào header hoặc dòng không hợp lệ
            if (e.RowIndex < 0 || e.RowIndex >= dgvLuong.Rows.Count) return;

            DataGridViewRow row = dgvLuong.Rows[e.RowIndex];

            // Gán giá trị từ DataGridView sang các control
            cbMaLuong.Text = row.Cells["Mã Lương"].Value?.ToString();
            cbMaNV.Text = row.Cells["Mã Nhân Viên"].Value?.ToString();
            cbThang.Text = row.Cells["Tháng"].Value?.ToString();

            if (int.TryParse(row.Cells["Năm"].Value?.ToString(), out int nam))
                numNam.Value = nam;

            txtLuongCoBan.Text = row.Cells["Lương Cơ Bản"].Value?.ToString();
            txtSoNgayCong.Text = row.Cells["Số Ngày Công"].Value?.ToString();
            txtPhuCap.Text = row.Cells["Phụ cấp"].Value?.ToString();
            txtKhauTru.Text = row.Cells["Khấu Trừ"].Value?.ToString();
            txtGhiChu.Text = row.Cells["Ghi Chú"].Value?.ToString();
        }

        private void cbMaNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaNV.SelectedValue == null) return;

            try
            {
                cn.connect();

                string sql = "SELECT LuongCoBan FROM tblHopDong WHERE MaNV = @MaNV AND DeletedAt = 0";
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", cbMaNV.SelectedValue.ToString());

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        txtLuongCoBan.Text = result.ToString(); // gán vào textbox Lương cơ bản
                    }
                    else
                    {
                        txtLuongCoBan.Text = ""; // nếu NV chưa có hợp đồng thì để trống
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy lương cơ bản từ Hợp đồng: " + ex.Message,
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            string maLuong = GenerateMaLuong();

            // 1. Validate cơ bản
            if (string.IsNullOrWhiteSpace(cbMaLuong.Text) ||
                string.IsNullOrWhiteSpace(cbMaNV.Text) ||
                cbThang.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtSoNgayCong.Text) ||
                string.IsNullOrWhiteSpace(txtLuongCoBan.Text) ||
                string.IsNullOrWhiteSpace(txtPhuCap.Text) ||
                string.IsNullOrWhiteSpace(txtKhauTru.Text))
            {
                MessageBox.Show("Chưa nhập đủ thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Parse các giá trị số an toàn
            if (!int.TryParse(cbThang.SelectedItem.ToString(), out int thang))
            {
                MessageBox.Show("Tháng không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int nam = (int)numNam.Value;

            if (!int.TryParse(txtSoNgayCong.Text.Trim(), out int soNgayCong))
            {
                MessageBox.Show("Số ngày công không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtLuongCoBan.Text.Trim(), out decimal luongCoBan))
            {
                MessageBox.Show("Lương cơ bản không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtPhuCap.Text.Trim(), out decimal phuCap))
            {
                MessageBox.Show("Phụ cấp không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtKhauTru.Text.Trim(), out decimal khauTru))
            {
                MessageBox.Show("Khấu trừ không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Kiểm tra trùng MaLuong (nếu MaLuong là PK do user nhập)
            try
            {
                cn.connect();

                string checkSql = "SELECT COUNT(*) FROM tblLuong WHERE MaLuong = @MaLuong AND DeletedAt = 0";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, cn.conn))
                {
                    checkCmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text.Trim());
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Mã lương này đã tồn tại. Vui lòng chọn mã khác.", "Trùng khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // 4. Insert bằng parameterized query
                string insertSql = @"
            INSERT INTO tblLuong
                (MaLuong, MaNV, Thang, Nam, LuongCoBan, SoNgayCong, PhuCap, KhauTru, GhiChu, DeletedAt)
            VALUES
                (@MaLuong, @MaNV, @Thang, @Nam, @LuongCoBan, @SoNgayCong, @PhuCap, @KhauTru, @GhiChu, 0)";

                using (SqlCommand cmd = new SqlCommand(insertSql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaNV", cbMaNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    cmd.Parameters.AddWithValue("@LuongCoBan", luongCoBan);
                    cmd.Parameters.AddWithValue("@SoNgayCong", soNgayCong);
                    cmd.Parameters.AddWithValue("@PhuCap", phuCap);
                    cmd.Parameters.AddWithValue("@KhauTru", khauTru);
                    cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm thành công!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLuong();
                // Nếu bạn muốn reset input, gọi ClearAllInputs nhưng chú ý không clear cbThang/numNam nếu không muốn
                // hoặc gọi InitThangNam() sau khi clear
                ClearAllInputs(this);
                InitThangNam(); // (nếu bạn có hàm này để đặt Month/Year mặc định)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbMaLuong.Text))
            {
                MessageBox.Show("Vui lòng chọn Mã Lương để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                cn.connect();
                string sqlUpdate = @"
            UPDATE tblLuong 
            SET MaNV = @MaNV, Thang = @Thang, Nam = @Nam,
                LuongCoBan = @LuongCoBan, SoNgayCong = @SoNgayCong, 
                PhuCap = @PhuCap, KhauTru = @KhauTru, GhiChu = @GhiChu
            WHERE MaLuong = @MaLuong AND DeletedAt = 0";

                using (SqlCommand cmd = new SqlCommand(sqlUpdate, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaNV", cbMaNV.Text.Trim());
                    cmd.Parameters.AddWithValue("@Thang", int.Parse(cbThang.SelectedItem.ToString()));
                    cmd.Parameters.AddWithValue("@Nam", (int)numNam.Value);
                    cmd.Parameters.AddWithValue("@LuongCoBan", decimal.Parse(txtLuongCoBan.Text.Trim()));
                    cmd.Parameters.AddWithValue("@SoNgayCong", int.Parse(txtSoNgayCong.Text.Trim()));
                    cmd.Parameters.AddWithValue("@PhuCap", decimal.Parse(txtPhuCap.Text.Trim()));
                    cmd.Parameters.AddWithValue("@KhauTru", decimal.Parse(txtKhauTru.Text.Trim()));
                    cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLuong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa dữ liệu: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbMaLuong.Text))
            {
                MessageBox.Show("Vui lòng chọn Mã Lương để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa Mã Lương này?",
                                              "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            try
            {
                cn.connect();
                string sqlDelete = "UPDATE tblLuong SET DeletedAt = 1 WHERE MaLuong = @MaLuong";
                using (SqlCommand cmd = new SqlCommand(sqlDelete, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataLuong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            ClearAllInputs(this);
            InitThangNam();
            LoadDataLuong();
        }

        private void btnXuatExcel_Click_1(object sender, EventArgs e)
        {
            if (dgvLuong.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("Luong");

                                // Ghi header
                                for (int i = 0; i < dgvLuong.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dgvLuong.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dgvLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dgvLuong.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dgvLuong.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border
                                var range = ws.Range(1, 1, dgvLuong.Rows.Count + 1, dgvLuong.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // Tự động co giãn cột
                                ws.Columns().AdjustToContents();

                                // Lưu file
                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel bảng Lương thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlSearch = @"SELECT MaLuong as N'Mã Lương', MaNV as N'Mã Nhân Viên',
                                Thang as N'Tháng', Nam as N'Năm',
                                LuongCoBan as N'Lương Cơ Bản', SoNgayCong as N'Số Ngày Công',
                                PhuCap as N'Phụ cấp', KhauTru as N'Khấu Trừ', Ghichu as N'Ghi Chú',
                                TongLuong as N'Tổng Lương'
                         FROM tblLuong 
                         WHERE DeletedAt = 0";

                if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    sqlSearch += " AND (MaLuong LIKE @Search OR MaNV LIKE @Search)";
                }

                using (SqlCommand cmd = new SqlCommand(sqlSearch, cn.conn))
                {
                    if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + txtTimKiem.Text.Trim() + "%");
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvLuong.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void dgvLuong_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
