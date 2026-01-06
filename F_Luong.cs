using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static QuanLyNhanVien3.F_FormMain;

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

            InitThangNam();
            LoadcbNV();

            //cbThang.SelectedIndexChanged += FilterChanged;
            //numNam.ValueChanged += FilterChanged;
            //cbMaNV.SelectedIndexChanged += FilterChanged;

            LoadLuongTheoThangNamVaNV();
            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }
            txtLuongCoBan.ReadOnly = true;
        }

        private void LoadLuongTheoThangNamVaNV()
        {
            try
            {
                cn.connect();

                int thang = Convert.ToInt32(cbThang.SelectedItem);
                int nam = (int)numNam.Value;
                string maNV = cbMaNV.SelectedValue?.ToString();

                string sql = @"
        SELECT 
            MaLuong         AS N'Mã Lương',
            Thang           AS N'Tháng',
            Nam             AS N'Năm',
            LuongCoBan      AS N'Lương Cơ Bản',
            SoNgayCongChuan AS N'Số Ngày Công Chuẩn',
            PhuCap          AS N'Phụ Cấp',
            KhauTru         AS N'Khấu Trừ',
            Ghichu          AS N'Ghi Chú'
        FROM tblLuong
        WHERE DeletedAt = 0
          AND Thang = @Thang
          AND Nam = @Nam";

                // 👉 Nếu có chọn nhân viên → lọc theo nhân viên
                if (!string.IsNullOrEmpty(maNV))
                {
                    sql += " AND MaLuong LIKE '%' + @MaNV + '%'";
                }

                sql += " ORDER BY MaLuong";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    if (!string.IsNullOrEmpty(maNV))
                        cmd.Parameters.AddWithValue("@MaNV", maNV);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLuong.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load lương: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }
        }
        private void InitThangNam()
        {
            cbThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
                cbThang.Items.Add(i);

            cbThang.SelectedItem = DateTime.Now.Month;

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
            try
            {
                cn.connect();

                string sql = @"
        SELECT DISTINCT nv.MaNV
        FROM tblNhanVien nv
        INNER JOIN tblHopDong hd ON nv.MaNV = hd.MaNV
        WHERE nv.DeletedAt = 0
          AND hd.DeletedAt = 0
          AND (hd.NgayKetThuc IS NULL OR hd.NgayKetThuc >= GETDATE())
        ORDER BY nv.MaNV";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cbMaNV.DataSource = dt;
                    cbMaNV.DisplayMember = "MaNV";
                    cbMaNV.ValueMember = "MaNV";
                    cbMaNV.SelectedIndex = -1; // không chọn sẵn
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load nhân viên còn hợp đồng: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }




        private void LoadDataLuong()
        {
            try
            {
                cn.connect();

                string sql = @"SELECT MaLuong AS [Mã lương], Thang AS Tháng, Nam AS năm, LuongCoBan AS [Lương cơ bản],
                        SoNgayCongChuan AS [Ngày công], PhuCap AS [Phụ cấp], KhauTru AS [Khấu trừ], Ghichu AS [Ghi chú], ChamCongId
                        FROM     tblLuong
                        WHERE  (DeletedAt = 0)";

                SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvLuong.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load lương: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
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
            if (cbThang.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtLuongCoBan.Text) ||
                string.IsNullOrWhiteSpace(txtSoNgayCong.Text))
            {
                MessageBox.Show("Chưa nhập đủ dữ liệu!");
                return;
            }

            try
            {
                cn.connect();

                string sql = @"
        INSERT INTO tblLuong
        (MaLuong, Thang, Nam, LuongCoBan, SoNgayCongChuan, PhuCap, KhauTru, Ghichu, DeletedAt)
        VALUES
        (@MaLuong, @Thang, @Nam, @LuongCoBan, @SoNgayCong, @PhuCap, @KhauTru, @Ghichu, 0)";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", GenerateMaLuong());
                    cmd.Parameters.AddWithValue("@Thang", Convert.ToInt32(cbThang.SelectedItem));
                    cmd.Parameters.AddWithValue("@Nam", (int)numNam.Value);
                    cmd.Parameters.AddWithValue("@LuongCoBan", decimal.Parse(txtLuongCoBan.Text));
                    cmd.Parameters.AddWithValue("@SoNgayCong", int.Parse(txtSoNgayCong.Text));
                    cmd.Parameters.AddWithValue("@PhuCap", decimal.Parse(txtPhuCap.Text));
                    cmd.Parameters.AddWithValue("@KhauTru", decimal.Parse(txtKhauTru.Text));
                    cmd.Parameters.AddWithValue("@Ghichu", txtGhiChu.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm lương thành công!");
                LoadDataLuong();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm lương: " + ex.Message);
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
                MessageBox.Show("Chọn mã lương cần sửa!");
                return;
            }

            try
            {
                cn.connect();

                string sql = @"
        UPDATE tblLuong
        SET Thang = @Thang,
            Nam = @Nam,
            LuongCoBan = @LuongCoBan,
            SoNgayCongChuan = @SoNgayCong,
            PhuCap = @PhuCap,
            KhauTru = @KhauTru,
            Ghichu = @Ghichu
        WHERE MaLuong = @MaLuong AND DeletedAt = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text);
                    cmd.Parameters.AddWithValue("@Thang", Convert.ToInt32(cbThang.SelectedItem));
                    cmd.Parameters.AddWithValue("@Nam", (int)numNam.Value);
                    cmd.Parameters.AddWithValue("@LuongCoBan", decimal.Parse(txtLuongCoBan.Text));
                    cmd.Parameters.AddWithValue("@SoNgayCong", int.Parse(txtSoNgayCong.Text));
                    cmd.Parameters.AddWithValue("@PhuCap", decimal.Parse(txtPhuCap.Text));
                    cmd.Parameters.AddWithValue("@KhauTru", decimal.Parse(txtKhauTru.Text));
                    cmd.Parameters.AddWithValue("@Ghichu", txtGhiChu.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Cập nhật lương thành công!");
                LoadDataLuong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa lương: " + ex.Message);
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
                MessageBox.Show("Chọn mã lương để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.No) return;

            try
            {
                cn.connect();
                string sql = "UPDATE tblLuong SET DeletedAt = 1 WHERE MaLuong = @MaLuong";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Xóa thành công!");
                LoadDataLuong();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa lương: " + ex.Message);
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
                string fileName = $"BangLuong_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";

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
                                var ws = wb.Worksheets.Add("Luong");

                                int colCount = dgvLuong.Columns.Count;

                                /* ========== TIÊU ĐỀ ========= */
                                ws.Cell(1, 1).Value = "BẢNG LƯƠNG NHÂN VIÊN";
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
                                    ws.Cell(4, i + 1).Value = dgvLuong.Columns[i].HeaderText;
                                    ws.Cell(4, i + 1).Style.Font.Bold = true;
                                    ws.Cell(4, i + 1).Style.Alignment.Horizontal =
                                        XLAlignmentHorizontalValues.Center;
                                    ws.Cell(4, i + 1).Style.Fill.BackgroundColor =
                                        XLColor.LightGray;
                                }

                                /* ========== DỮ LIỆU ========= */
                                for (int i = 0; i < dgvLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dgvLuong.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 5, j + 1).Value =
                                            value != null ? value.ToString() : "";
                                    }
                                }

                                /* ========== BORDER ========= */
                                var range = ws.Range(
                                    4, 1,
                                    dgvLuong.Rows.Count + 4,
                                    colCount
                                );

                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                /* ========== AUTO SIZE ========= */
                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel bảng Lương thành công!",
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
            if (e.RowIndex < 0) return;

            var row = dgvLuong.Rows[e.RowIndex];

            cbMaLuong.Text = row.Cells["Mã Lương"].Value.ToString();
            cbThang.Text = row.Cells["Tháng"].Value.ToString();
            numNam.Value = Convert.ToInt32(row.Cells["Năm"].Value);
            txtLuongCoBan.Text = row.Cells["Lương Cơ Bản"].Value.ToString();
            txtSoNgayCong.Text = row.Cells["Số Ngày Công Chuẩn"].Value.ToString();
            txtPhuCap.Text = row.Cells["Phụ Cấp"].Value.ToString();
            txtKhauTru.Text = row.Cells["Khấu Trừ"].Value.ToString();
            txtGhiChu.Text = row.Cells["Ghi Chú"].Value.ToString();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbMaNV_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbMaNV.SelectedValue == null)
            {
                txtLuongCoBan.Text = "";
                return;
            }

            string maNV = cbMaNV.SelectedValue.ToString();

            if (string.IsNullOrWhiteSpace(maNV))
            {
                txtLuongCoBan.Text = "";
                return;
            }

            decimal luongCB = GetLuongCoBanByMaNV(maNV);

            if (luongCB > 0)
            {
                txtLuongCoBan.Text = luongCB.ToString("N0");
            }
            else
            {
                txtLuongCoBan.Text = "0";
                //MessageBox.Show("Nhân viên chưa có hợp đồng còn hiệu lực!",
                //    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private decimal GetLuongCoBanByMaNV(string maNV)
        {
            decimal luongCB = 0;

            try
            {
                cn.connect();

                string sql = @"
        SELECT TOP 1 LuongCoBan
        FROM tblHopDong
        WHERE MaNV = @MaNV
          AND DeletedAt = 0
          AND (NgayKetThuc IS NULL OR NgayKetThuc >= GETDATE())
        ORDER BY NgayBatDau DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        luongCB = Convert.ToDecimal(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy lương cơ bản: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }

            return luongCB;
        }

        private void txtLuongCoBan_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void numNam_ValueChanged(object sender, EventArgs e)
        {
            if (cbThang.SelectedItem == null) return;
            LoadLuongTheoThangNamVaNV();
        }

        private void btntimkiemtheothnag_Click(object sender, EventArgs e)
        {

            //try
            //{// Kiểm tra đã chọn tháng
            //    if (cbbThangtimkiem.SelectedIndex <= 0) // ví dụ 0 = "Tất cả"
            //    {
            //        MessageBox.Show("Vui lòng chọn tháng để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }

            //    // Lấy giá trị tháng (SelectedItem) và convert sang int
            //    int thang = Convert.ToInt32(cbbThangtimkiem.SelectedItem);

            //    string sql = @"SELECT MaLuong as N'Mã Lương', MaNV as N'Mã Nhân Viên',
            //          Thang as N'Tháng', Nam as N'Năm',
            //          LuongCoBan as N'Lương Cơ Bản', SoNgayCong as N'Số Ngày Công',
            //          PhuCap as N'Phụ cấp', KhauTru as N'Khấu Trừ', Ghichu as N'Ghi Chú',
            //          TongLuong as N'Tổng Lương'
            //   FROM tblLuong
            //   WHERE DeletedAt = 0 AND Thang = @Thang
            //   ORDER BY MaNV";

            //    using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
            //    {
            //        cmd.Parameters.AddWithValue("@Thang", thang);

            //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //        DataTable dt = new DataTable();
            //        adapter.Fill(dt);
            //        dgvLuong.DataSource = dt;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }

        private void cbbThangtimkiem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void LoadLuongTheoThang(int thang)
        {
            try
            {
                cn.connect();

                string sql = @"
        SELECT 
            MaLuong            AS N'Mã Lương',
            Thang              AS N'Tháng',
            Nam                AS N'Năm',
            LuongCoBan         AS N'Lương Cơ Bản',
            SoNgayCongChuan    AS N'Số Ngày Công Chuẩn',
            PhuCap             AS N'Phụ Cấp',
            KhauTru            AS N'Khấu Trừ',
            Ghichu             AS N'Ghi Chú'
        FROM tblLuong
        WHERE DeletedAt = 0 AND Thang = @Thang
        ORDER BY Nam DESC, MaLuong";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvLuong.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load lương theo tháng: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbThang.SelectedItem == null) return;
            LoadLuongTheoThangNamVaNV();
        }
    }

}
