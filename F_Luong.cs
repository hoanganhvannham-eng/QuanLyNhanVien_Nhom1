using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

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

        string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName;
        private void F_Luong_Load(object sender, EventArgs e)
        {
            InitThangNam();
            LoadComboBoxPhongBan();
            LoadComboBoxChucVu();
            LoadComboBoxNhanVien();
            LoadLuongTheoThangNam();

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
                        txtSoNgayCong.Text = ""; // Xóa số ngày công khi thay đổi nhân viên
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

                    // Tự động load số ngày công từ bảng lương (nếu có)
                    LoadSoNgayCongTuBangLuong(maNV);

                    // Load dữ liệu lương theo nhân viên
                    LoadLuongFilter(maNV: maNV, maCV: currentMaCV, maPB: currentMaPB);
                }
                else
                {
                    txtLuongCoBan.Text = "";
                    txtSoNgayCong.Text = "";
                    // Load dữ liệu theo chức vụ hoặc phòng ban
                    LoadLuongFilter(maCV: currentMaCV, maPB: currentMaPB);
                }
            }
        }

        #endregion

        #region LOAD LƯƠNG VÀ FILTER (ĐÃ SỬA)

        // Phương thức load lương với bộ lọc (ĐÃ SỬA)
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
                        l.SoNgayCongChuan_ChienCD232928 AS N'Số ngày công',
                        
                        -- Số ngày đi làm: MIN(số ngày chấm công, số ngày công chuẩn)
                        CASE
                            WHEN so_ngay_cham_cong.so_ngay > l.SoNgayCongChuan_ChienCD232928
                            THEN l.SoNgayCongChuan_ChienCD232928
                            ELSE so_ngay_cham_cong.so_ngay
                        END AS N'Số ngày đi làm',
                        
                        l.PhuCap_ChienCD232928      AS N'Phụ cấp',
                        l.KhauTru_ChienCD232928     AS N'Khấu trừ',
                        
                        -- Tính tổng lương
                        ROUND(
                            (l.LuongCoBan_ChienCD232928 / NULLIF(l.SoNgayCongChuan_ChienCD232928, 0)) * 
                            CASE
                                WHEN so_ngay_cham_cong.so_ngay > l.SoNgayCongChuan_ChienCD232928
                                THEN l.SoNgayCongChuan_ChienCD232928
                                ELSE so_ngay_cham_cong.so_ngay
                            END + 
                            ISNULL(l.PhuCap_ChienCD232928, 0) - 
                            ISNULL(l.KhauTru_ChienCD232928, 0), 
                            2
                        ) AS N'Tổng lương',
                        
                        -- Ghi chú hiển thị số ngày đi làm ĐÚNG
                        N'Số ngày công thực tế: ' + 
                        CAST(
                            CASE
                                WHEN so_ngay_cham_cong.so_ngay > l.SoNgayCongChuan_ChienCD232928
                                THEN l.SoNgayCongChuan_ChienCD232928
                                ELSE so_ngay_cham_cong.so_ngay
                            END AS NVARCHAR
                        ) + N'/' + 
                        CAST(l.SoNgayCongChuan_ChienCD232928 AS NVARCHAR) AS N'Ghi chú'
                    FROM tblLuong_ChienCD232928 l
                    INNER JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    CROSS APPLY (
                        SELECT COUNT(DISTINCT CAST(cc2.Ngay_TuanhCD233018 AS DATE)) as so_ngay
                        FROM tblChamCong_TuanhCD233018 cc2
                        WHERE cc2.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                          AND MONTH(cc2.Ngay_TuanhCD233018) = @Thang
                          AND YEAR(cc2.Ngay_TuanhCD233018) = @Nam
                          AND cc2.DeletedAt_TuanhCD233018 = 0
                    ) so_ngay_cham_cong
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
            // Gọi phương thức LoadLuongFilter không có bộ lọc
            LoadLuongFilter();
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

        // Lấy số ngày công từ bảng lương (nếu đã có lương cho tháng/năm này)
        private void LoadSoNgayCongTuBangLuong(string maNV)
        {
            try
            {
                cn.connect();

                int thang = Convert.ToInt32(cbThang.SelectedItem);
                int nam = (int)numNam.Value;

                // Kiểm tra xem đã có bảng lương cho nhân viên này trong tháng/năm chưa
                string sql = @"
                    SELECT TOP 1 l.SoNgayCongChuan_ChienCD232928
                    FROM tblLuong_ChienCD232928 l
                    INNER JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    WHERE nv.MaNV_TuanhCD233018 = @MaNV
                      AND l.Thang_ChienCD232928 = @Thang
                      AND l.Nam_ChienCD232928 = @Nam
                      AND l.DeletedAt_ChienCD232928 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        int soNgayCong = Convert.ToInt32(result);
                        txtSoNgayCong.Text = soNgayCong.ToString();
                    }
                    else
                    {
                        // Nếu chưa có lương, để trống cho người dùng nhập
                        txtSoNgayCong.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy số ngày công từ bảng lương: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoNgayCong.Text = "";
            }
            finally
            {
                cn.disconnect();
            }
        }

        // Định dạng cột tiền trong DataGridView
        private void FormatCurrencyColumns()
        {
            // Định dạng các cột tiền
            string[] currencyColumns = { "Lương cơ bản", "Phụ cấp", "Khấu trừ", "Tổng lương" };

            foreach (string colName in currencyColumns)
            {
                if (dgvLuong.Columns.Contains(colName))
                {
                    dgvLuong.Columns[colName].DefaultCellStyle.Format = "N0";
                    dgvLuong.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            // Căn giữa cho các cột số khác
            string[] centerColumns = { "Tháng", "Năm", "Số ngày công", "Số ngày đi làm" };

            foreach (string colName in centerColumns)
            {
                if (dgvLuong.Columns.Contains(colName))
                {
                    dgvLuong.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        #endregion

        #region THÊM - SỬA - XÓA - REFRESH

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu
            if (cbThang.SelectedItem == null ||
                string.IsNullOrWhiteSpace(cbMaLuong.Text) ||
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

            // Kiểm tra xem nhân viên đã có lương trong tháng/năm này chưa
            if (CheckLuongExistForNhanVien(currentMaNV, Convert.ToInt32(cbThang.SelectedItem), (int)numNam.Value))
            {
                MessageBox.Show("Nhân viên này đã có bảng lương trong tháng/năm này!",
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

        private bool CheckLuongExistForNhanVien(string maNV, int thang, int nam)
        {
            try
            {
                if (cn.conn.State != ConnectionState.Open)
                    cn.connect();

                string sql = @"
                    SELECT COUNT(*) 
                    FROM tblLuong_ChienCD232928 l
                    INNER JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    WHERE nv.MaNV_TuanhCD233018 = @MaNV
                      AND l.Thang_ChienCD232928 = @Thang
                      AND l.Nam_ChienCD232928 = @Nam
                      AND l.DeletedAt_ChienCD232928 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra lương nhân viên: " + ex.Message);
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

            if (row.Cells["Số ngày công"].Value != null)
                txtSoNgayCong.Text = row.Cells["Số ngày công"].Value.ToString();

            if (row.Cells["Số ngày đi làm"].Value != null)
                txtSoNgayCong.Text = row.Cells["Số ngày đi làm"].Value.ToString();

            if (row.Cells["Phụ cấp"].Value != null)
                txtPhuCap.Text = Convert.ToDecimal(row.Cells["Phụ cấp"].Value).ToString("N0");

            if (row.Cells["Khấu trừ"].Value != null)
                txtKhauTru.Text = Convert.ToDecimal(row.Cells["Khấu trừ"].Value).ToString("N0");

            // KHÔNG hiển thị tổng lương trong textbox nữa, chỉ hiển thị trong DataGridView
            if (row.Cells["Ghi chú"].Value != null)
                txtGhiChu.Text = row.Cells["Ghi chú"].Value.ToString();
        }

        #endregion

        #region TÌM KIẾM

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
                        l.SoNgayCongChuan_ChienCD232928 AS N'Số ngày công',
                        
                        -- Số ngày đi làm: MIN(số ngày chấm công, số ngày công chuẩn)
                        CASE
                            WHEN so_ngay_cham_cong.so_ngay > l.SoNgayCongChuan_ChienCD232928
                            THEN l.SoNgayCongChuan_ChienCD232928
                            ELSE so_ngay_cham_cong.so_ngay
                        END AS N'Số ngày đi làm',
                        
                        l.PhuCap_ChienCD232928      AS N'Phụ cấp',
                        l.KhauTru_ChienCD232928     AS N'Khấu trừ',
                        
                        -- Tính tổng lương
                        ROUND(
                            (l.LuongCoBan_ChienCD232928 / NULLIF(l.SoNgayCongChuan_ChienCD232928, 0)) * 
                            CASE
                                WHEN so_ngay_cham_cong.so_ngay > l.SoNgayCongChuan_ChienCD232928
                                THEN l.SoNgayCongChuan_ChienCD232928
                                ELSE so_ngay_cham_cong.so_ngay
                            END + 
                            ISNULL(l.PhuCap_ChienCD232928, 0) - 
                            ISNULL(l.KhauTru_ChienCD232928, 0), 
                            2
                        ) AS N'Tổng lương',
                        
                        -- Ghi chú hiển thị số ngày đi làm ĐÚNG
                        N'Số ngày công thực tế: ' + 
                        CAST(
                            CASE
                                WHEN so_ngay_cham_cong.so_ngay > l.SoNgayCongChuan_ChienCD232928
                                THEN l.SoNgayCongChuan_ChienCD232928
                                ELSE so_ngay_cham_cong.so_ngay
                            END AS NVARCHAR
                        ) + N'/' + 
                        CAST(l.SoNgayCongChuan_ChienCD232928 AS NVARCHAR) AS N'Ghi chú'
                    FROM tblLuong_ChienCD232928 l
                    INNER JOIN tblChamCong_TuanhCD233018 cc ON l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    CROSS APPLY (
                        SELECT COUNT(DISTINCT CAST(cc2.Ngay_TuanhCD233018 AS DATE)) as so_ngay
                        FROM tblChamCong_TuanhCD233018 cc2
                        WHERE cc2.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                          AND MONTH(cc2.Ngay_TuanhCD233018) = @Thang
                          AND YEAR(cc2.Ngay_TuanhCD233018) = @Nam
                          AND cc2.DeletedAt_TuanhCD233018 = 0
                    ) so_ngay_cham_cong
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

        #endregion

        #region XUẤT EXCEL

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
                                int rowCount = dgvLuong.Rows.Count;

                                /* ========== TIÊU ĐỀ CÔNG TY ========= */
                                ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                                ws.Range(1, 1, 1, colCount).Merge();
                                ws.Range(1, 1, 1, colCount).Style.Font.Bold = true;
                                ws.Range(1, 1, 1, colCount).Style.Font.FontSize = 14;
                                ws.Range(1, 1, 1, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Center;

                                /* ========== TIÊU ĐỀ BÁO CÁO ========= */
                                ws.Cell(2, 1).Value = "BÁO CÁO LƯƠNG NHÂN VIÊN";
                                ws.Range(2, 1, 2, colCount).Merge();
                                ws.Range(2, 1, 2, colCount).Style.Font.Bold = true;
                                ws.Range(2, 1, 2, colCount).Style.Font.FontSize = 12;
                                ws.Range(2, 1, 2, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Center;

                                /* ========== NGÀY LẬP BÁO CÁO ========= */
                                ws.Cell(3, 1).Value = $"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy}";
                                ws.Range(3, 1, 3, colCount).Merge();
                                ws.Range(3, 1, 3, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Center;

                                /* ========== THÔNG TIN NGƯỜI XUẤT ========= */
                                // Lấy thông tin người xuất từ bảng tài khoản
                                var userInfo = GetCurrentUserInfoFromDatabase();
                                if (userInfo != null)
                                {
                                    ws.Cell(4, 1).Value = "Phòng Ban";
                                    ws.Cell(4, 2).Value = userInfo["PhongBan"]?.ToString() ?? "";
                                    ws.Cell(5, 1).Value = "Chức vụ";
                                    ws.Cell(5, 2).Value = userInfo["ChucVu"]?.ToString() ?? "";
                                }

                                /* ========== HEADER TABLE ========= */
                                int startRow = 7;
                                for (int i = 0; i < colCount; i++)
                                {
                                    string headerText = dgvLuong.Columns[i].HeaderText;
                                    ws.Cell(startRow, i + 1).Value = headerText;
                                    ws.Cell(startRow, i + 1).Style.Font.Bold = true;
                                    ws.Cell(startRow, i + 1).Style.Alignment.Horizontal =
                                        XLAlignmentHorizontalValues.Center;
                                    ws.Cell(startRow, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                }

                                /* ========== DỮ LIỆU ========= */
                                for (int i = 0; i < dgvLuong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dgvLuong.Rows[i].Cells[j].Value;
                                        ws.Cell(i + startRow + 1, j + 1).Value =
                                            value != null ? value.ToString() : "";
                                    }
                                }

                                /* ========== TÍNH TỔNG CỘNG ========= */
                                decimal tongLuongCoBan = 0;
                                decimal tongPhuCap = 0;
                                decimal tongKhauTru = 0;
                                decimal tongLuong = 0;
                                int tongSoNgayCong = 0;
                                int tongSoNgayDiLam = 0;

                                foreach (DataGridViewRow row in dgvLuong.Rows)
                                {
                                    if (row.Cells["Lương cơ bản"].Value != null)
                                    {
                                        decimal luongCB;
                                        if (decimal.TryParse(row.Cells["Lương cơ bản"].Value.ToString().Replace(",", ""), out luongCB))
                                            tongLuongCoBan += luongCB;
                                    }

                                    if (row.Cells["Phụ cấp"].Value != null)
                                    {
                                        decimal phuCap;
                                        if (decimal.TryParse(row.Cells["Phụ cấp"].Value.ToString().Replace(",", ""), out phuCap))
                                            tongPhuCap += phuCap;
                                    }

                                    if (row.Cells["Khấu trừ"].Value != null)
                                    {
                                        decimal khauTru;
                                        if (decimal.TryParse(row.Cells["Khấu trừ"].Value.ToString().Replace(",", ""), out khauTru))
                                            tongKhauTru += khauTru;
                                    }

                                    if (row.Cells["Tổng lương"].Value != null)
                                    {
                                        decimal tongLuongNV;
                                        if (decimal.TryParse(row.Cells["Tổng lương"].Value.ToString().Replace(",", ""), out tongLuongNV))
                                            tongLuong += tongLuongNV;
                                    }

                                    if (row.Cells["Số ngày công"].Value != null)
                                    {
                                        int soNgayCong;
                                        if (int.TryParse(row.Cells["Số ngày công"].Value.ToString(), out soNgayCong))
                                            tongSoNgayCong += soNgayCong;
                                    }

                                    if (row.Cells["Số ngày đi làm"].Value != null)
                                    {
                                        int soNgayDiLam;
                                        if (int.TryParse(row.Cells["Số ngày đi làm"].Value.ToString(), out soNgayDiLam))
                                            tongSoNgayDiLam += soNgayDiLam;
                                    }
                                }

                                /* ========== HIỂN THỊ TỔNG CỘNG ========= */
                                int totalRow = startRow + rowCount + 1;

                                // Dòng tổng cộng
                                ws.Cell(totalRow, 1).Value = "Tổng Cộng";
                                ws.Range(totalRow, 1, totalRow, 4).Merge();

                                // Tổng lương cơ bản
                                int luongCol = GetColumnIndex(dgvLuong, "Lương cơ bản");
                                if (luongCol >= 0)
                                    ws.Cell(totalRow, luongCol + 1).Value = tongLuongCoBan;

                                // Tổng số ngày công
                                int ngayCongCol = GetColumnIndex(dgvLuong, "Số ngày công");
                                if (ngayCongCol >= 0)
                                    ws.Cell(totalRow, ngayCongCol + 1).Value = tongSoNgayCong;

                                // Tổng số ngày đi làm
                                int ngayDiLamCol = GetColumnIndex(dgvLuong, "Số ngày đi làm");
                                if (ngayDiLamCol >= 0)
                                    ws.Cell(totalRow, ngayDiLamCol + 1).Value = tongSoNgayDiLam;

                                // Tổng phụ cấp
                                int phuCapCol = GetColumnIndex(dgvLuong, "Phụ cấp");
                                if (phuCapCol >= 0)
                                    ws.Cell(totalRow, phuCapCol + 1).Value = tongPhuCap;

                                // Tổng khấu trừ
                                int khauTruCol = GetColumnIndex(dgvLuong, "Khấu trừ");
                                if (khauTruCol >= 0)
                                    ws.Cell(totalRow, khauTruCol + 1).Value = tongKhauTru;

                                // Tổng lương
                                int tongLuongCol = GetColumnIndex(dgvLuong, "Tổng lương");
                                if (tongLuongCol >= 0)
                                    ws.Cell(totalRow, tongLuongCol + 1).Value = tongLuong;

                                // Ghi chú (trống)
                                int ghiChuCol = GetColumnIndex(dgvLuong, "Ghi chú");
                                if (ghiChuCol >= 0)
                                    ws.Cell(totalRow, ghiChuCol + 1).Value = "";

                                // Định dạng dòng tổng cộng
                                ws.Range(totalRow, 1, totalRow, colCount).Style.Font.Bold = true;
                                ws.Range(totalRow, 1, totalRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                /* ========== CHỮ KÝ (CĂN PHẢI) - 3 DÒNG RIÊNG BIỆT ========= */
                                int signatureRow = totalRow + 2;

                                // Dòng 1: "Hà Nội, ngày..." (căn phải)
                                string dateStr = $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}";
                                ws.Cell(signatureRow, colCount - 1).Value = dateStr;
                                ws.Range(signatureRow, colCount - 1, signatureRow, colCount).Merge();
                                ws.Range(signatureRow, colCount - 1, signatureRow, colCount).Style.Font.Italic = true;
                                ws.Range(signatureRow, colCount - 1, signatureRow, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Right;

                                // Dòng 2: "Người xuất" (căn phải)
                                ws.Cell(signatureRow + 1, colCount - 1).Value = "Người xuất";
                                ws.Range(signatureRow + 1, colCount - 1, signatureRow + 1, colCount).Merge();
                                ws.Range(signatureRow + 1, colCount - 1, signatureRow + 1, colCount).Style.Font.Bold = true;
                                ws.Range(signatureRow + 1, colCount - 1, signatureRow + 1, colCount).Style.Alignment.Horizontal =
                                    XLAlignmentHorizontalValues.Right;

                                // Dòng 3: Tên người xuất (căn phải)
                                if (userInfo != null)
                                {
                                    ws.Cell(signatureRow + 2, colCount - 1).Value = nguoiDangNhap;
                                    ws.Range(signatureRow + 2, colCount - 1, signatureRow + 2, colCount).Merge();
                                    ws.Range(signatureRow + 2, colCount - 1, signatureRow + 2, colCount).Style.Font.Bold = true;
                                    ws.Range(signatureRow + 2, colCount - 1, signatureRow + 2, colCount).Style.Alignment.Horizontal =
                                        XLAlignmentHorizontalValues.Right;
                                }

                                /* ========== BORDER ========= */
                                var range = ws.Range(
                                    startRow, 1,
                                    totalRow,
                                    colCount
                                );

                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                /* ========== FORMAT SỐ TIỀN ========= */
                                if (luongCol >= 0)
                                    ws.Column(luongCol + 1).Style.NumberFormat.Format = "#,##0";
                                if (phuCapCol >= 0)
                                    ws.Column(phuCapCol + 1).Style.NumberFormat.Format = "#,##0";
                                if (khauTruCol >= 0)
                                    ws.Column(khauTruCol + 1).Style.NumberFormat.Format = "#,##0";
                                if (tongLuongCol >= 0)
                                    ws.Column(tongLuongCol + 1).Style.NumberFormat.Format = "#,##0";

                                // Định dạng tổng cộng
                                if (luongCol >= 0)
                                    ws.Cell(totalRow, luongCol + 1).Style.NumberFormat.Format = "#,##0";
                                if (phuCapCol >= 0)
                                    ws.Cell(totalRow, phuCapCol + 1).Style.NumberFormat.Format = "#,##0";
                                if (khauTruCol >= 0)
                                    ws.Cell(totalRow, khauTruCol + 1).Style.NumberFormat.Format = "#,##0";
                                if (tongLuongCol >= 0)
                                    ws.Cell(totalRow, tongLuongCol + 1).Style.NumberFormat.Format = "#,##0";

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
                            MessageBox.Show("Lỗi xuất file Excel: " + ex.Message,
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

        // Lấy chỉ số cột từ DataGridView
        private int GetColumnIndex(DataGridView dgv, string columnName)
        {
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i].HeaderText == columnName)
                    return i;
            }
            return -1;
        }

        // Lấy thông tin người dùng hiện tại từ database
        private DataRow GetCurrentUserInfoFromDatabase()
        {
            try
            {
                cn.connect();

                // Thay bằng cách lấy mã NV thực tế từ hệ thống của bạn
                string maNVHienTai = "NV001"; // Thay bằng cách lấy thực tế

                string sql = @"
                    SELECT 
                        nv.MaNV_TuanhCD233018 AS [MaNV],
                        nv.HoTen_TuanhCD233018 AS [HoTen],
                        cv.TenCV_KhangCD233181 AS [ChucVu],
                        pb.TenPB_ThuanCD233318 AS [PhongBan]
                    FROM tblNhanVien_TuanhCD233018 nv
                    INNER JOIN tblChucVu_KhangCD233181 cv 
                        ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb 
                        ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE nv.MaNV_TuanhCD233018 = @MaNV
                      AND nv.DeletedAt_TuanhCD233018 = 0
                      AND cv.DeletedAt_KhangCD233181 = 0
                      AND pb.DeletedAt_ThuanCD233318 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNVHienTai);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy thông tin người dùng: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
            return null;
        }

        #endregion

        #region XUẤT PDF

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            if (dgvLuong.Rows.Count > 0)
            {
                string fileName = $"BangLuong_{DateTime.Now:ddMMyyyy_HHmmss}.pdf";

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "PDF Files|*.pdf",
                    FileName = fileName
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Lấy thông tin người xuất
                            var userInfo = GetCurrentUserInfoFromDatabase();

                            // Tạo tài liệu PDF
                            Document doc = new Document(PageSize.A4.Rotate(), 20f, 20f, 30f, 30f);
                            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                            doc.Open();

                            // Font chữ
                            BaseFont bf = BaseFont.CreateFont("c:/windows/fonts/times.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD);
                            iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                            iTextSharp.text.Font fontSubTitle = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.BOLD);
                            iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 9, iTextSharp.text.Font.NORMAL);
                            iTextSharp.text.Font fontSmall = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.NORMAL);
                            iTextSharp.text.Font fontBold = new iTextSharp.text.Font(bf, 8, iTextSharp.text.Font.BOLD);

                            /* ========== TIÊU ĐỀ ========= */
                            Paragraph title1 = new Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", fontTitle);
                            title1.Alignment = Element.ALIGN_CENTER;
                            title1.SpacingAfter = 5f;
                            doc.Add(title1);

                            Paragraph title2 = new Paragraph("BÁO CÁO LƯƠNG NHÂN VIÊN", fontHeader);
                            title2.Alignment = Element.ALIGN_CENTER;
                            title2.SpacingAfter = 10f;
                            doc.Add(title2);

                            Paragraph reportDate = new Paragraph($"Ngày lập báo cáo: {DateTime.Now:dd/MM/yyyy}", fontNormal);
                            reportDate.Alignment = Element.ALIGN_CENTER;
                            reportDate.SpacingAfter = 15f;
                            doc.Add(reportDate);

                            /* ========== THÔNG TIN NGƯỜI XUẤT ========= */
                            if (userInfo != null)
                            {
                                Paragraph phongBan = new Paragraph($"Phòng Ban: {userInfo["PhongBan"]}", fontNormal);
                                phongBan.SpacingAfter = 5f;
                                doc.Add(phongBan);

                                Paragraph chucVu = new Paragraph($"Chức vụ: {userInfo["ChucVu"]}", fontNormal);
                                chucVu.SpacingAfter = 10f;
                                doc.Add(chucVu);
                            }

                            /* ========== TÍNH TỔNG CỘNG ========= */
                            decimal tongLuongCoBan = 0;
                            decimal tongPhuCap = 0;
                            decimal tongKhauTru = 0;
                            decimal tongLuong = 0;
                            int tongSoNgayCong = 0;
                            int tongSoNgayDiLam = 0;

                            foreach (DataGridViewRow row in dgvLuong.Rows)
                            {
                                if (row.Cells["Lương cơ bản"].Value != null)
                                {
                                    decimal luongCB;
                                    if (decimal.TryParse(row.Cells["Lương cơ bản"].Value.ToString().Replace(",", ""), out luongCB))
                                        tongLuongCoBan += luongCB;
                                }

                                if (row.Cells["Phụ cấp"].Value != null)
                                {
                                    decimal phuCap;
                                    if (decimal.TryParse(row.Cells["Phụ cấp"].Value.ToString().Replace(",", ""), out phuCap))
                                        tongPhuCap += phuCap;
                                }

                                if (row.Cells["Khấu trừ"].Value != null)
                                {
                                    decimal khauTru;
                                    if (decimal.TryParse(row.Cells["Khấu trừ"].Value.ToString().Replace(",", ""), out khauTru))
                                        tongKhauTru += khauTru;
                                }

                                if (row.Cells["Tổng lương"].Value != null)
                                {
                                    decimal tongLuongNV;
                                    if (decimal.TryParse(row.Cells["Tổng lương"].Value.ToString().Replace(",", ""), out tongLuongNV))
                                        tongLuong += tongLuongNV;
                                }

                                if (row.Cells["Số ngày công"].Value != null)
                                {
                                    int soNgayCong;
                                    if (int.TryParse(row.Cells["Số ngày công"].Value.ToString(), out soNgayCong))
                                        tongSoNgayCong += soNgayCong;
                                }

                                if (row.Cells["Số ngày đi làm"].Value != null)
                                {
                                    int soNgayDiLam;
                                    if (int.TryParse(row.Cells["Số ngày đi làm"].Value.ToString(), out soNgayDiLam))
                                        tongSoNgayDiLam += soNgayDiLam;
                                }
                            }

                            /* ========== TẠO BẢNG ========= */
                            int colCount = dgvLuong.Columns.Count;
                            PdfPTable table = new PdfPTable(colCount);
                            table.WidthPercentage = 100;
                            table.HorizontalAlignment = Element.ALIGN_CENTER;

                            // Header
                            foreach (DataGridViewColumn column in dgvLuong.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, fontSmall));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cell.BackgroundColor = new BaseColor(240, 240, 240);
                                cell.Padding = 5f;
                                table.AddCell(cell);
                            }

                            // Data
                            foreach (DataGridViewRow row in dgvLuong.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.Value != null)
                                    {
                                        string value = cell.Value.ToString();

                                        // Format số tiền
                                        if (cell.OwningColumn.HeaderText.Contains("Lương") ||
                                            cell.OwningColumn.HeaderText.Contains("Phụ cấp") ||
                                            cell.OwningColumn.HeaderText.Contains("Khấu trừ") ||
                                            cell.OwningColumn.HeaderText.Contains("Tổng lương"))
                                        {
                                            if (decimal.TryParse(value, out decimal number))
                                            {
                                                value = number.ToString("N0");
                                            }
                                        }

                                        PdfPCell pdfCell = new PdfPCell(new Phrase(value, fontSmall));

                                        // Căn phải cho cột số tiền
                                        if (cell.OwningColumn.HeaderText.Contains("Lương") ||
                                            cell.OwningColumn.HeaderText.Contains("Phụ cấp") ||
                                            cell.OwningColumn.HeaderText.Contains("Khấu trừ") ||
                                            cell.OwningColumn.HeaderText.Contains("Tổng lương"))
                                        {
                                            pdfCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        }
                                        else if (cell.OwningColumn.HeaderText.Contains("Tháng") ||
                                                 cell.OwningColumn.HeaderText.Contains("Năm") ||
                                                 cell.OwningColumn.HeaderText.Contains("Số ngày công") ||
                                                 cell.OwningColumn.HeaderText.Contains("Số ngày đi làm"))
                                        {
                                            pdfCell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        }
                                        else
                                        {
                                            pdfCell.HorizontalAlignment = Element.ALIGN_LEFT;
                                        }

                                        pdfCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        pdfCell.Padding = 5f;
                                        table.AddCell(pdfCell);
                                    }
                                    else
                                    {
                                        PdfPCell pdfCell = new PdfPCell(new Phrase("", fontSmall));
                                        pdfCell.Padding = 5f;
                                        table.AddCell(pdfCell);
                                    }
                                }
                            }

                            /* ========== DÒNG TỔNG CỘNG ========= */
                            // Tổng Cộng text
                            PdfPCell totalLabelCell = new PdfPCell(new Phrase("Tổng Cộng", fontBold));
                            totalLabelCell.Colspan = 4;
                            totalLabelCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            totalLabelCell.Padding = 5f;
                            table.AddCell(totalLabelCell);

                            // Tổng lương cơ bản
                            PdfPCell totalLuongCell = new PdfPCell(new Phrase(tongLuongCoBan.ToString("N0"), fontBold));
                            totalLuongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            totalLuongCell.Padding = 5f;
                            table.AddCell(totalLuongCell);

                            // Tổng số ngày công
                            PdfPCell totalNgayCongCell = new PdfPCell(new Phrase(tongSoNgayCong.ToString(), fontBold));
                            totalNgayCongCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            totalNgayCongCell.Padding = 5f;
                            table.AddCell(totalNgayCongCell);

                            // Tổng số ngày đi làm
                            PdfPCell totalNgayDiLamCell = new PdfPCell(new Phrase(tongSoNgayDiLam.ToString(), fontBold));
                            totalNgayDiLamCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            totalNgayDiLamCell.Padding = 5f;
                            table.AddCell(totalNgayDiLamCell);

                            // Tổng phụ cấp
                            PdfPCell totalPhuCapCell = new PdfPCell(new Phrase(tongPhuCap.ToString("N0"), fontBold));
                            totalPhuCapCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            totalPhuCapCell.Padding = 5f;
                            table.AddCell(totalPhuCapCell);

                            // Tổng khấu trừ
                            PdfPCell totalKhauTruCell = new PdfPCell(new Phrase(tongKhauTru.ToString("N0"), fontBold));
                            totalKhauTruCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            totalKhauTruCell.Padding = 5f;
                            table.AddCell(totalKhauTruCell);

                            // Tổng lương
                            PdfPCell totalTongLuongCell = new PdfPCell(new Phrase(tongLuong.ToString("N0"), fontBold));
                            totalTongLuongCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            totalTongLuongCell.Padding = 5f;
                            table.AddCell(totalTongLuongCell);

                            // Ghi chú (empty)
                            PdfPCell totalGhiChuCell = new PdfPCell(new Phrase("", fontBold));
                            totalGhiChuCell.Padding = 5f;
                            table.AddCell(totalGhiChuCell);

                            doc.Add(table);
                            doc.Add(new Paragraph("\n"));

                            /* ========== CHỮ KÝ (CĂN PHẢI) ========= */
                            doc.Add(new Paragraph("\n"));

                            // Tạo bảng 1 cột cho chữ ký (căn phải)
                            PdfPTable signatureTable = new PdfPTable(1);
                            signatureTable.WidthPercentage = 100;
                            signatureTable.HorizontalAlignment = Element.ALIGN_RIGHT;

                            // Ngày tháng năm
                            string dateStr = $"Hà Nội, ngày {DateTime.Now:dd} tháng {DateTime.Now:MM} năm {DateTime.Now:yyyy}";
                            PdfPCell dateCell = new PdfPCell(new Phrase(dateStr, fontNormal));
                            dateCell.Border = Rectangle.NO_BORDER;
                            dateCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            dateCell.Padding = 5f;
                            signatureTable.AddCell(dateCell);

                            // Người xuất
                            if (userInfo != null)
                            {
                                PdfPCell signatureCell = new PdfPCell(new Phrase("Người xuất\n\n" + nguoiDangNhap, fontSubTitle));
                                signatureCell.Border = Rectangle.NO_BORDER;
                                signatureCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                signatureCell.Padding = 5f;
                                signatureTable.AddCell(signatureCell);
                            }

                            doc.Add(signatureTable);

                            doc.Close();
                            writer.Close();

                            MessageBox.Show("Xuất PDF bảng Lương thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất file PDF: " + ex.Message,
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

        #region HELPER METHODS

        // Phương thức helper để tính lương theo công thức
        private decimal TinhTongLuong(decimal luongCoBan, int soNgayCongChuan, int soNgayDiLam, decimal phuCap, decimal khauTru)
        {
            if (soNgayCongChuan == 0) return 0;

            decimal luongTheoNgay = (luongCoBan / soNgayCongChuan) * soNgayDiLam;
            decimal tongLuong = luongTheoNgay + phuCap - khauTru;

            return Math.Round(tongLuong, 2);
        }

        // Phương thức để lấy số ngày đi làm thực tế của nhân viên
        private int GetSoNgayDiLamThucTe(string maNV, int thang, int nam)
        {
            int soNgayDiLam = 0;
            try
            {
                cn.connect();

                string sql = @"
                    SELECT COUNT(DISTINCT CAST(cc.Ngay_TuanhCD233018 AS DATE))
                    FROM tblChamCong_TuanhCD233018 cc
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    WHERE nv.MaNV_TuanhCD233018 = @MaNV
                      AND MONTH(cc.Ngay_TuanhCD233018) = @Thang
                      AND YEAR(cc.Ngay_TuanhCD233018) = @Nam
                      AND cc.DeletedAt_TuanhCD233018 = 0";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", maNV);
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        soNgayDiLam = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy số ngày đi làm: " + ex.Message);
            }
            finally
            {
                cn.disconnect();
            }

            return soNgayDiLam;
        }

        #endregion

        #region EVENTS KHÁC

        private void numNam_ValueChanged(object sender, EventArgs e)
        {
            if (cbThang.SelectedItem == null) return;

            // Nếu đã chọn nhân viên, tự động load lại số ngày công từ bảng lương
            if (!string.IsNullOrEmpty(currentMaNV) && currentMaNV != "-- Chọn Nhân Viên --")
            {
                LoadSoNgayCongTuBangLuong(currentMaNV);
            }

            LoadLuongFilter(maPB: currentMaPB, maCV: currentMaCV, maNV: currentMaNV);
        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbThang.SelectedItem == null) return;

            // Nếu đã chọn nhân viên, tự động load lại số ngày công từ bảng lương
            if (!string.IsNullOrEmpty(currentMaNV) && currentMaNV != "-- Chọn Nhân Viên --")
            {
                LoadSoNgayCongTuBangLuong(currentMaNV);
            }

            LoadLuongFilter(maPB: currentMaPB, maCV: currentMaCV, maNV: currentMaNV);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}