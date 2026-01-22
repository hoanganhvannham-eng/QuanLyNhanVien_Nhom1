using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static QuanLyNhanVien3.F_FormMain;

namespace QuanLyNhanVien3
{
    public partial class F_Luong : Form
    {
        connectData cn = new connectData();
        private bool isLoadingComboBox = false;
        private string currentMaPB = "";
        private string currentMaCV = "";
        private string currentMaNV = "";

        public F_Luong()
        {
            InitializeComponent();
        }

        private void F_Luong_Load(object sender, EventArgs e)
        {
            InitThangNam();
            LoadComboBoxPhongBan();
            LoadComboBoxChucVu();
            LoadComboBoxNhanVien();
            LoadLuongTheoThangNam();

            if (LoginInfo.CurrentUserRole.ToLower() == "user")
            {
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
            }

            txtLuongCoBan.ReadOnly = true;
            txtLuongCoBan.BackColor = System.Drawing.SystemColors.Control;

            // Định dạng DataGridView
            dgvLuong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLuong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLuong.MultiSelect = false;
            dgvLuong.ReadOnly = true;
        }

        #region LOAD COMBOBOX PHÒNG BAN - CHỨC VỤ - NHÂN VIÊN

        // 1. Load ComboBox Phòng Ban
        private void LoadComboBoxPhongBan()
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT MaPB_ThuanCD233318, TenPB_ThuanCD233318 
                    FROM tblPhongBan_ThuanCD233318 
                    WHERE DeletedAt_ThuanCD233318 = 0 
                    ORDER BY TenPB_ThuanCD233318";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, cn.conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Thêm dòng "Tất cả"
                    DataRow row = dt.NewRow();
                    row["MaPB_ThuanCD233318"] = "";
                    row["TenPB_ThuanCD233318"] = "-- Chọn Phòng Ban --";
                    dt.Rows.InsertAt(row, 0);

                    isLoadingComboBox = true;
                    cbPB.DataSource = dt;
                    cbPB.DisplayMember = "TenPB_ThuanCD233318";
                    cbPB.ValueMember = "MaPB_ThuanCD233318";
                    cbPB.SelectedIndex = 0;
                    isLoadingComboBox = false;

                    currentMaPB = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Phòng Ban: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // 2. Load ComboBox Chức Vụ
        private void LoadComboBoxChucVu(string maPB = "")
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT CV.MaCV_KhangCD233181, CV.TenCV_KhangCD233181, PB.MaPB_ThuanCD233318, PB.TenPB_ThuanCD233318
                    FROM tblChucVu_KhangCD233181 CV
                    INNER JOIN tblPhongBan_ThuanCD233318 PB ON CV.MaPB_ThuanCD233318 = PB.MaPB_ThuanCD233318
                    WHERE CV.DeletedAt_KhangCD233181 = 0";

                if (!string.IsNullOrEmpty(maPB))
                {
                    sql += " AND CV.MaPB_ThuanCD233318 = @MaPB";
                }

                sql += " ORDER BY PB.TenPB_ThuanCD233318, CV.TenCV_KhangCD233181";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    if (!string.IsNullOrEmpty(maPB))
                    {
                        cmd.Parameters.AddWithValue("@MaPB", maPB);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Thêm dòng "Tất cả"
                        DataRow row = dt.NewRow();
                        row["MaCV_KhangCD233181"] = "";
                        row["TenCV_KhangCD233181"] = "-- Chọn Chức Vụ --";
                        row["MaPB_ThuanCD233318"] = "";
                        row["TenPB_ThuanCD233318"] = "";
                        dt.Rows.InsertAt(row, 0);

                        isLoadingComboBox = true;
                        cbCV.DataSource = dt;
                        cbCV.DisplayMember = "TenCV_KhangCD233181";
                        cbCV.ValueMember = "MaCV_KhangCD233181";
                        cbCV.SelectedIndex = 0;
                        isLoadingComboBox = false;

                        currentMaCV = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Chức Vụ: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // 3. Load ComboBox Nhân Viên
        private void LoadComboBoxNhanVien(string maCV = "", string maPB = "")
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT DISTINCT 
                        nv.MaNV_TuanhCD233018, 
                        nv.HoTen_TuanhCD233018, 
                        nv.MaCV_KhangCD233181,
                        cv.TenCV_KhangCD233181,
                        cv.MaPB_ThuanCD233318,
                        pb.TenPB_ThuanCD233318
                    FROM tblNhanVien_TuanhCD233018 nv
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                      AND cv.DeletedAt_KhangCD233181 = 0
                      AND pb.DeletedAt_ThuanCD233318 = 0";

                // Thêm điều kiện lọc
                if (!string.IsNullOrEmpty(maCV))
                {
                    sql += " AND nv.MaCV_KhangCD233181 = @MaCV";
                }
                else if (!string.IsNullOrEmpty(maPB))
                {
                    sql += " AND cv.MaPB_ThuanCD233318 = @MaPB";
                }

                sql += " ORDER BY pb.TenPB_ThuanCD233318, cv.TenCV_KhangCD233181, nv.HoTen_TuanhCD233018";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    if (!string.IsNullOrEmpty(maCV))
                    {
                        cmd.Parameters.AddWithValue("@MaCV", maCV);
                    }
                    else if (!string.IsNullOrEmpty(maPB))
                    {
                        cmd.Parameters.AddWithValue("@MaPB", maPB);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Thêm dòng "Tất cả"
                        DataRow row = dt.NewRow();
                        row["MaNV_TuanhCD233018"] = "";
                        row["HoTen_TuanhCD233018"] = "-- Chọn Nhân Viên --";
                        dt.Rows.InsertAt(row, 0);

                        isLoadingComboBox = true;
                        cbMaNV.DataSource = dt;
                        cbMaNV.DisplayMember = "HoTen_TuanhCD233018";
                        cbMaNV.ValueMember = "MaNV_TuanhCD233018";
                        cbMaNV.SelectedIndex = 0;
                        isLoadingComboBox = false;

                        currentMaNV = "";
                        txtLuongCoBan.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Nhân Viên: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // 4. Event khi chọn Phòng Ban
        private void cbPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingComboBox) return;

            if (cbPB.SelectedValue != null)
            {
                string maPB = cbPB.SelectedValue.ToString();
                currentMaPB = maPB;

                if (string.IsNullOrEmpty(maPB) || maPB == "-- Chọn Phòng Ban --")
                {
                    // Load tất cả chức vụ và nhân viên
                    LoadComboBoxChucVu();
                    LoadComboBoxNhanVien();
                    LoadLuongFilter(); // Load dữ liệu không lọc
                }
                else
                {
                    // Load chức vụ theo phòng ban
                    LoadComboBoxChucVu(maPB);

                    // Load nhân viên theo phòng ban
                    LoadComboBoxNhanVien("", maPB);

                    // Load dữ liệu lương theo phòng ban
                    LoadLuongFilter(maPB: maPB);
                }
            }
        }

        // 5. Event khi chọn Chức Vụ
        private void cbCV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingComboBox) return;

            if (cbCV.SelectedValue != null)
            {
                string maCV = cbCV.SelectedValue.ToString();
                currentMaCV = maCV;

                if (string.IsNullOrEmpty(maCV) || maCV == "-- Chọn Chức Vụ --")
                {
                    // Load nhân viên theo phòng ban (nếu có) hoặc tất cả
                    LoadComboBoxNhanVien("", currentMaPB);

                    // Load dữ liệu lương theo phòng ban (nếu có) hoặc tất cả
                    LoadLuongFilter(maPB: currentMaPB);
                }
                else
                {
                    // Load nhân viên theo chức vụ
                    LoadComboBoxNhanVien(maCV, currentMaPB);

                    // Load dữ liệu lương theo chức vụ
                    LoadLuongFilter(maCV: maCV, maPB: currentMaPB);
                }
            }
        }

        // 6. Event khi chọn Nhân Viên
        private void cbMaNV_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (isLoadingComboBox) return;

            if (cbMaNV.SelectedValue != null)
            {
                string maNV = cbMaNV.SelectedValue.ToString();
                currentMaNV = maNV;

                // Lấy và hiển thị lương cơ bản từ hợp đồng
                if (!string.IsNullOrEmpty(maNV) && maNV != "-- Chọn Nhân Viên --")
                {
                    LoadLuongCoBanTuHopDong(maNV);

                    // Load dữ liệu lương theo nhân viên
                    LoadLuongFilter(maNV: maNV, maCV: currentMaCV, maPB: currentMaPB);
                }
                else
                {
                    txtLuongCoBan.Text = "";
                    // Load dữ liệu theo chức vụ hoặc phòng ban
                    LoadLuongFilter(maCV: currentMaCV, maPB: currentMaPB);
                }
            }
        }

        #endregion

        #region LOAD LƯƠNG VÀ FILTER

        // Phương thức load lương với bộ lọc
        private void LoadLuongFilter(string maPB = "", string maCV = "", string maNV = "")
        {
            try
            {
                cn.connect();

                int thang = Convert.ToInt32(cbThang.SelectedItem);
                int nam = (int)numNam.Value;

                string sql = @"
                    SELECT 
                        l.Maluong_ChienCD232928     AS N'Mã Lương',
                        nv.MaNV_TuanhCD233018       AS N'Mã NV',
                        nv.HoTen_TuanhCD233018      AS N'Tên nhân viên',
                        cv.TenCV_KhangCD233181      AS N'Chức Vụ',
                        pb.TenPB_ThuanCD233318      AS N'Phòng Ban',
                        l.Thang_ChienCD232928       AS N'Tháng',
                        l.Nam_ChienCD232928         AS N'Năm',
                        l.LuongCoBan_ChienCD232928  AS N'Lương cơ bản',
                        l.SoNgayCongChuan_ChienCD232928 AS N'Số Ngày công chuẩn',
                        l.PhuCap_ChienCD232928      AS N'Phụ cấp',
                        l.KhauTru_ChienCD232928     AS N'Khấu trừ',
                        l.Ghichu_ChienCD232928      AS N'Ghi chú'
                    FROM tblLuong_ChienCD232928 l
                    INNER JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE l.DeletedAt_ChienCD232928 = 0
                      AND l.Thang_ChienCD232928 = @Thang
                      AND l.Nam_ChienCD232928 = @Nam";

                // Thêm điều kiện lọc
                if (!string.IsNullOrEmpty(maNV))
                {
                    sql += " AND nv.MaNV_TuanhCD233018 = @MaNV";
                }
                else if (!string.IsNullOrEmpty(maCV))
                {
                    sql += " AND cv.MaCV_KhangCD233181 = @MaCV";
                }
                else if (!string.IsNullOrEmpty(maPB))
                {
                    sql += " AND pb.MaPB_ThuanCD233318 = @MaPB";
                }

                sql += " ORDER BY nv.HoTen_TuanhCD233018, l.Maluong_ChienCD232928";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    if (!string.IsNullOrEmpty(maNV))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                    }
                    else if (!string.IsNullOrEmpty(maCV))
                    {
                        cmd.Parameters.AddWithValue("@MaCV", maCV);
                    }
                    else if (!string.IsNullOrEmpty(maPB))
                    {
                        cmd.Parameters.AddWithValue("@MaPB", maPB);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLuong.DataSource = dt;

                    // Định dạng số cho các cột tiền
                    FormatCurrencyColumns();

                    // Điều chỉnh độ rộng cột tự động
                    dgvLuong.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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

        private void LoadLuongTheoThangNam()
        {
            try
            {
                cn.connect();

                int thang = Convert.ToInt32(cbThang.SelectedItem);
                int nam = (int)numNam.Value;

                string sql = @"
                    SELECT 
                        l.Maluong_ChienCD232928     AS N'Mã Lương',
                        nv.MaNV_TuanhCD233018       AS N'Mã NV',
                        nv.HoTen_TuanhCD233018      AS N'Tên nhân viên',
                        cv.TenCV_KhangCD233181      AS N'Chức Vụ',
                        pb.TenPB_ThuanCD233318      AS N'Phòng Ban',
                        l.Thang_ChienCD232928       AS N'Tháng',
                        l.Nam_ChienCD232928         AS N'Năm',
                        l.LuongCoBan_ChienCD232928  AS N'Lương cơ bản',
                        l.SoNgayCongChuan_ChienCD232928 AS N'Số Ngày công chuẩn',
                        l.PhuCap_ChienCD232928      AS N'Phụ cấp',
                        l.KhauTru_ChienCD232928     AS N'Khấu trừ',
                        l.Ghichu_ChienCD232928      AS N'Ghi chú'
                    FROM tblLuong_ChienCD232928 l
                    INNER JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE l.DeletedAt_ChienCD232928 = 0
                      AND l.Thang_ChienCD232928 = @Thang
                      AND l.Nam_ChienCD232928 = @Nam
                    ORDER BY nv.HoTen_TuanhCD233018, l.Maluong_ChienCD232928";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLuong.DataSource = dt;

                    // Định dạng số cho các cột tiền
                    FormatCurrencyColumns();

                    // Điều chỉnh độ rộng cột tự động
                    dgvLuong.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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

        // Lấy lương cơ bản từ hợp đồng (bỏ qua kiểm tra hết hạn)
        private void LoadLuongCoBanTuHopDong(string maNV)
        {
            try
            {
                cn.connect();

                // Chỉ kiểm tra hợp đồng không bị xóa, không kiểm tra ngày hết hạn
                string sql = @"
    SELECT TOP 1 LuongCoBan_ChienCD232928
    FROM tblHopDong_ChienCD232928
    WHERE MaNV_TuanhCD233018 = @MaNV
    ORDER BY NgayBatDau_ChienCD232928 DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        decimal luongCB = Convert.ToDecimal(result);
                        txtLuongCoBan.Text = luongCB.ToString("N0");
                    }
                    else
                    {
                        // Nếu không có hợp đồng, hiển thị 0
                        txtLuongCoBan.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy lương cơ bản: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLuongCoBan.Text = "0";
            }
            finally
            {
                cn.disconnect();
            }
        }

        // Định dạng cột tiền trong DataGridView
        private void FormatCurrencyColumns()
        {
            if (dgvLuong.Columns.Contains("Lương cơ bản"))
            {
                dgvLuong.Columns["Lương cơ bản"].DefaultCellStyle.Format = "N0";
                dgvLuong.Columns["Lương cơ bản"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvLuong.Columns.Contains("Phụ cấp"))
            {
                dgvLuong.Columns["Phụ cấp"].DefaultCellStyle.Format = "N0";
                dgvLuong.Columns["Phụ cấp"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            if (dgvLuong.Columns.Contains("Khấu trừ"))
            {
                dgvLuong.Columns["Khấu trừ"].DefaultCellStyle.Format = "N0";
                dgvLuong.Columns["Khấu trừ"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            // Căn giữa cho các cột số
            if (dgvLuong.Columns.Contains("Tháng"))
                dgvLuong.Columns["Tháng"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (dgvLuong.Columns.Contains("Năm"))
                dgvLuong.Columns["Năm"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (dgvLuong.Columns.Contains("Số Ngày công chuẩn"))
                dgvLuong.Columns["Số Ngày công chuẩn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        #endregion

        #region CLEAR, GENERATE MÃ

        private void ClearAllInputs()
        {
            // Clear textboxes
            txtPhuCap.Text = "";
            txtSoNgayCong.Text = "";
            txtKhauTru.Text = "";
            txtGhiChu.Text = "";
            txtTimKiem.Text = "";
            txtLuongCoBan.Text = "";
            cbMaLuong.Text = "";

            // Reset comboboxes
            isLoadingComboBox = true;

            cbPB.SelectedIndex = 0;
            LoadComboBoxChucVu();
            LoadComboBoxNhanVien();

            cbThang.SelectedItem = DateTime.Now.Month;
            numNam.Value = DateTime.Now.Year;

            isLoadingComboBox = false;

            // Reset variables
            currentMaPB = "";
            currentMaCV = "";
            currentMaNV = "";

            // Load lại dữ liệu
            LoadLuongTheoThangNam();
        }

        #endregion

        #region THÊM - SỬA - XÓA - REFRESH

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu
            if (cbThang.SelectedItem == null ||
                string.IsNullOrWhiteSpace(cbMaLuong.Text) || // Thêm kiểm tra mã lương không được trống
                string.IsNullOrWhiteSpace(txtLuongCoBan.Text) ||
                string.IsNullOrWhiteSpace(txtSoNgayCong.Text) ||
                string.IsNullOrEmpty(currentMaNV) ||
                currentMaNV == "-- Chọn Nhân Viên --")
            {
                MessageBox.Show("Vui lòng chọn nhân viên, nhập mã lương và nhập đủ dữ liệu!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra mã lương đã tồn tại chưa
            if (CheckMaLuongExist(cbMaLuong.Text))
            {
                MessageBox.Show("Mã lương đã tồn tại! Vui lòng nhập mã khác.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Đảm bảo kết nối mở
                if (cn.conn.State != ConnectionState.Open)
                    cn.connect();

                // Lấy MaNV từ biến currentMaNV
                string maNV = currentMaNV;

                // Lấy NhanVienId từ MaNV
                int nhanVienId = GetNhanVienIdByMaNV(maNV);
                if (nhanVienId == 0)
                {
                    MessageBox.Show("Không tìm thấy ID nhân viên!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tìm ChamCongId cho nhân viên trong tháng/năm
                int? chamCongId = GetChamCongIdForNhanVien(nhanVienId,
                    Convert.ToInt32(cbThang.SelectedItem),
                    (int)numNam.Value);

                // Sử dụng mã lương từ combobox (người dùng nhập)
                string maLuong = cbMaLuong.Text.Trim();

                string sql = @"
                    INSERT INTO tblLuong_ChienCD232928
                    (Maluong_ChienCD232928, Thang_ChienCD232928, Nam_ChienCD232928, LuongCoBan_ChienCD232928, SoNgayCongChuan_ChienCD232928, PhuCap_ChienCD232928, KhauTru_ChienCD232928, Ghichu_ChienCD232928, DeletedAt_ChienCD232928, ChamCongId_TuanhCD233018)
                    VALUES
                    (@MaLuong, @Thang, @Nam, @LuongCoBan, @SoNgayCong, @PhuCap, @KhauTru, @Ghichu, 0, @ChamCongId)";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", maLuong);
                    cmd.Parameters.AddWithValue("@Thang", Convert.ToInt32(cbThang.SelectedItem));
                    cmd.Parameters.AddWithValue("@Nam", (int)numNam.Value);
                    cmd.Parameters.AddWithValue("@LuongCoBan", decimal.Parse(txtLuongCoBan.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@SoNgayCong", int.Parse(txtSoNgayCong.Text));
                    cmd.Parameters.AddWithValue("@PhuCap", decimal.Parse(string.IsNullOrEmpty(txtPhuCap.Text) ? "0" : txtPhuCap.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@KhauTru", decimal.Parse(string.IsNullOrEmpty(txtKhauTru.Text) ? "0" : txtKhauTru.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@Ghichu", txtGhiChu.Text.Trim());

                    if (chamCongId.HasValue)
                        cmd.Parameters.AddWithValue("@ChamCongId", chamCongId.Value);
                    else
                        cmd.Parameters.AddWithValue("@ChamCongId", DBNull.Value);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm lương thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadLuongTheoThangNam();
                ClearAllInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm lương: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Chỉ đóng kết nối nếu cần
                if (cn.conn.State == ConnectionState.Open)
                    cn.disconnect();
            }
        }

        private bool CheckMaLuongExist(string maLuong)
        {
            try
            {
                if (cn.conn.State != ConnectionState.Open)
                    cn.connect();

                string sql = "SELECT COUNT(*) FROM tblLuong_ChienCD232928 WHERE Maluong_ChienCD232928 = @MaLuong AND DeletedAt_ChienCD232928 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", maLuong);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra mã lương: " + ex.Message);
                return true; // Trả về true để ngăn thêm nếu có lỗi
            }
        }

        private int GetNhanVienIdByMaNV(string maNV)
        {
            int id = 0;
            try
            {
                // Kiểm tra và mở kết nối nếu cần
                if (cn.conn.State != ConnectionState.Open)
                    cn.connect();

                string sql = "SELECT Id_TuanhCD233018 FROM tblNhanVien_TuanhCD233018 WHERE MaNV_TuanhCD233018 = @MaNV AND DeletedAt_TuanhCD233018 = 0";
                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        id = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy ID nhân viên: " + ex.Message);
            }
            return id;
        }

        private int? GetChamCongIdForNhanVien(int nhanVienId, int thang, int nam)
        {
            int? chamCongId = null;
            try
            {
                // Kiểm tra và mở kết nối nếu cần
                if (cn.conn.State != ConnectionState.Open)
                    cn.connect();

                string sql = @"
                    SELECT TOP 1 Id_TuanhCD233018 
                    FROM tblChamCong_TuanhCD233018 
                    WHERE NhanVienId_TuanhCD233018 = @NhanVienId 
                      AND MONTH(Ngay_TuanhCD233018) = @Thang 
                      AND YEAR(Ngay_TuanhCD233018) = @Nam
                      AND DeletedAt_TuanhCD233018 = 0
                    ORDER BY Ngay_TuanhCD233018";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@NhanVienId", nhanVienId);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        chamCongId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy ChamCongId: " + ex.Message);
            }
            return chamCongId;
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
                    UPDATE tblLuong_ChienCD232928
                    SET Thang_ChienCD232928 = @Thang,
                        Nam_ChienCD232928 = @Nam,
                        LuongCoBan_ChienCD232928 = @LuongCoBan,
                        SoNgayCongChuan_ChienCD232928 = @SoNgayCong,
                        PhuCap_ChienCD232928 = @PhuCap,
                        KhauTru_ChienCD232928 = @KhauTru,
                        Ghichu_ChienCD232928 = @Ghichu
                    WHERE Maluong_ChienCD232928 = @MaLuong AND DeletedAt_ChienCD232928 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text);
                    cmd.Parameters.AddWithValue("@Thang", Convert.ToInt32(cbThang.SelectedItem));
                    cmd.Parameters.AddWithValue("@Nam", (int)numNam.Value);
                    cmd.Parameters.AddWithValue("@LuongCoBan", decimal.Parse(txtLuongCoBan.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@SoNgayCong", int.Parse(txtSoNgayCong.Text));
                    cmd.Parameters.AddWithValue("@PhuCap", decimal.Parse(string.IsNullOrEmpty(txtPhuCap.Text) ? "0" : txtPhuCap.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@KhauTru", decimal.Parse(string.IsNullOrEmpty(txtKhauTru.Text) ? "0" : txtKhauTru.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@Ghichu", txtGhiChu.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Cập nhật lương thành công!");
                LoadLuongTheoThangNam();
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
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                cn.connect();
                string sql = "UPDATE tblLuong_ChienCD232928 SET DeletedAt_ChienCD232928 = 1 WHERE Maluong_ChienCD232928 = @MaLuong";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaLuong", cbMaLuong.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Xóa thành công!");
                LoadLuongTheoThangNam();
                ClearAllInputs();
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
            ClearAllInputs();
        }

        #endregion

        #region DATAGRIDVIEW EVENTS

        private void dgvLuong_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvLuong.Rows[e.RowIndex];

            // Lấy thông tin từ DataGridView theo đúng tên cột
            cbMaLuong.Text = row.Cells["Mã Lương"].Value?.ToString();

            // Lấy mã nhân viên và tự động chọn combobox
            if (row.Cells["Mã NV"].Value != null)
            {
                string maNV = row.Cells["Mã NV"].Value.ToString();
                currentMaNV = maNV;

                // Load lại lương cơ bản
                LoadLuongCoBanTuHopDong(maNV);

                // Tự động chọn nhân viên trong combobox
                isLoadingComboBox = true;
                cbMaNV.SelectedValue = maNV;
                isLoadingComboBox = false;
            }

            // Cập nhật thông tin khác
            if (row.Cells["Tháng"].Value != null)
                cbThang.SelectedItem = Convert.ToInt32(row.Cells["Tháng"].Value);

            if (row.Cells["Năm"].Value != null)
                numNam.Value = Convert.ToInt32(row.Cells["Năm"].Value);

            if (row.Cells["Lương cơ bản"].Value != null)
                txtLuongCoBan.Text = Convert.ToDecimal(row.Cells["Lương cơ bản"].Value).ToString("N0");

            if (row.Cells["Số Ngày công chuẩn"].Value != null)
                txtSoNgayCong.Text = row.Cells["Số Ngày công chuẩn"].Value.ToString();

            if (row.Cells["Phụ cấp"].Value != null)
                txtPhuCap.Text = Convert.ToDecimal(row.Cells["Phụ cấp"].Value).ToString("N0");

            if (row.Cells["Khấu trừ"].Value != null)
                txtKhauTru.Text = Convert.ToDecimal(row.Cells["Khấu trừ"].Value).ToString("N0");

            if (row.Cells["Ghi chú"].Value != null)
                txtGhiChu.Text = row.Cells["Ghi chú"].Value.ToString();
        }

        #endregion

        #region TÌM KIẾM - XUẤT EXCEL

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlSearch = @"
                    SELECT 
                        l.Maluong_ChienCD232928     AS N'Mã Lương',
                        nv.MaNV_TuanhCD233018       AS N'Mã NV',
                        nv.HoTen_TuanhCD233018      AS N'Tên nhân viên',
                        cv.TenCV_KhangCD233181      AS N'Chức Vụ',
                        pb.TenPB_ThuanCD233318      AS N'Phòng Ban',
                        l.Thang_ChienCD232928       AS N'Tháng',
                        l.Nam_ChienCD232928         AS N'Năm',
                        l.LuongCoBan_ChienCD232928  AS N'Lương cơ bản',
                        l.SoNgayCongChuan_ChienCD232928 AS N'Số Ngày công chuẩn',
                        l.PhuCap_ChienCD232928      AS N'Phụ cấp',
                        l.KhauTru_ChienCD232928     AS N'Khấu trừ',
                        l.Ghichu_ChienCD232928      AS N'Ghi chú'
                    FROM tblLuong_ChienCD232928 l
                    INNER JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE l.DeletedAt_ChienCD232928 = 0
                      AND l.Thang_ChienCD232928 = @Thang
                      AND l.Nam_ChienCD232928 = @Nam";

                // Lọc theo tìm kiếm hoặc theo phòng ban/chức vụ/nhân viên đã chọn
                if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    sqlSearch += " AND (l.Maluong_ChienCD232928 LIKE @Search OR nv.HoTen_TuanhCD233018 LIKE @Search OR nv.MaNV_TuanhCD233018 LIKE @Search)";
                }
                else
                {
                    // Nếu không có tìm kiếm, lọc theo combobox đã chọn
                    if (!string.IsNullOrEmpty(currentMaNV))
                    {
                        sqlSearch += " AND nv.MaNV_TuanhCD233018 = @MaNV";
                    }
                    else if (!string.IsNullOrEmpty(currentMaCV))
                    {
                        sqlSearch += " AND cv.MaCV_KhangCD233181 = @MaCV";
                    }
                    else if (!string.IsNullOrEmpty(currentMaPB))
                    {
                        sqlSearch += " AND pb.MaPB_ThuanCD233318 = @MaPB";
                    }
                }

                sqlSearch += " ORDER BY nv.HoTen_TuanhCD233018, l.Maluong_ChienCD232928";

                using (SqlCommand cmd = new SqlCommand(sqlSearch, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", Convert.ToInt32(cbThang.SelectedItem));
                    cmd.Parameters.AddWithValue("@Nam", (int)numNam.Value);

                    if (!string.IsNullOrWhiteSpace(txtTimKiem.Text))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + txtTimKiem.Text.Trim() + "%");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(currentMaNV))
                        {
                            cmd.Parameters.AddWithValue("@MaNV", currentMaNV);
                        }
                        else if (!string.IsNullOrEmpty(currentMaCV))
                        {
                            cmd.Parameters.AddWithValue("@MaCV", currentMaCV);
                        }
                        else if (!string.IsNullOrEmpty(currentMaPB))
                        {
                            cmd.Parameters.AddWithValue("@MaPB", currentMaPB);
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvLuong.DataSource = dt;

                        // Định dạng số cho các cột tiền
                        FormatCurrencyColumns();

                        // Điều chỉnh độ rộng cột tự động
                        dgvLuong.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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

                                /* ========== THÁNG/NĂM ========= */
                                ws.Cell(3, 1).Value = $"Tháng: {cbThang.SelectedItem}, Năm: {numNam.Value}";
                                ws.Range(3, 1, 3, colCount).Merge();
                                ws.Range(3, 1, 3, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Center;

                                /* ========== HEADER ========= */
                                for (int i = 0; i < colCount; i++)
                                {
                                    string headerText = dgvLuong.Columns[i].HeaderText;
                                    ws.Cell(5, i + 1).Value = headerText;
                                    ws.Cell(5, i + 1).Style.Font.Bold = true;
                                    ws.Cell(5, i + 1).Style.Alignment.Horizontal =
                                        XLAlignmentHorizontalValues.Center;
                                    ws.Cell(5, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }

                                /* ========== DỮ LIỆU ========= */
                                for (int i = 0; i < dgvLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dgvLuong.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 6, j + 1).Value =
                                            value != null ? value.ToString() : "";
                                    }
                                }

                                /* ========== BORDER ========= */
                                var range = ws.Range(
                                    5, 1,
                                    dgvLuong.Rows.Count + 5,
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

        #endregion

        #region EVENTS KHÁC

        private void numNam_ValueChanged(object sender, EventArgs e)
        {
            if (cbThang.SelectedItem == null) return;
            LoadLuongFilter(maPB: currentMaPB, maCV: currentMaCV, maNV: currentMaNV);
        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbThang.SelectedItem == null) return;
            LoadLuongFilter(maPB: currentMaPB, maCV: currentMaCV, maNV: currentMaNV);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}