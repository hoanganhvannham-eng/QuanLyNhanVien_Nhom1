using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanVien3
{
    public partial class F_ChamCongChiTiet : Form
    {
        connectData cn = new connectData();

        public F_ChamCongChiTiet()
        {
            InitializeComponent();
        }

        string nguoiDangNhap = F_FormMain.LoginInfo.CurrentUserName;
        private void F_ChamCongChiTiet_Load(object sender, EventArgs e)
        {
            LoadcomboBox();
            LoadAllChamCong(); // Load toàn bộ chấm công khi mở form
            dateTimeNgayChamCong.Value = DateTime.Now;
        }

        // load combo box 
        private void LoadcomboBox()
        {
            try
            {
                cn.connect();
                // Load Phòng ban
                string sqlLoadcomboBoxtblPhongBan = "SELECT '' AS MaPB_ThuanCD233318, N'-- Tất cả phòng ban --' AS TenPB_ThuanCD233318 UNION ALL SELECT MaPB_ThuanCD233318, TenPB_ThuanCD233318 FROM tblPhongBan_ThuanCD233318 WHERE DeletedAt_ThuanCD233318 = 0";
                using (SqlDataAdapter da = new SqlDataAdapter(sqlLoadcomboBoxtblPhongBan, cn.conn))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxMaPB.DataSource = ds.Tables[0];
                    cbBoxMaPB.DisplayMember = "TenPB_ThuanCD233318";
                    cbBoxMaPB.ValueMember = "MaPB_ThuanCD233318";
                }

                // Load Chức vụ - ban đầu hiển thị tất cả
                LoadChucVuComboBox("");

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load combobox: " + ex.Message);
            }
        }

        // Load chức vụ theo phòng ban
        private void LoadChucVuComboBox(string maPB)
        {
            try
            {
                cn.connect();
                string sqlLoadChucVu = "";

                if (string.IsNullOrEmpty(maPB))
                {
                    // Load tất cả chức vụ
                    sqlLoadChucVu = "SELECT '' AS MaCV_KhangCD233181, N'-- Tất cả chức vụ --' AS TenCV_KhangCD233181 UNION ALL SELECT MaCV_KhangCD233181, TenCV_KhangCD233181 FROM tblChucVu_KhangCD233181 WHERE DeletedAt_KhangCD233181 = 0";
                }
                else
                {
                    // Load chức vụ theo phòng ban
                    sqlLoadChucVu = "SELECT '' AS MaCV_KhangCD233181, N'-- Tất cả chức vụ --' AS TenCV_KhangCD233181 UNION ALL SELECT MaCV_KhangCD233181, TenCV_KhangCD233181 FROM tblChucVu_KhangCD233181 WHERE DeletedAt_KhangCD233181 = 0 AND MaPB_ThuanCD233318 = @MaPB";
                }

                using (SqlCommand cmd = new SqlCommand(sqlLoadChucVu, cn.conn))
                {
                    if (!string.IsNullOrEmpty(maPB))
                    {
                        cmd.Parameters.AddWithValue("@MaPB", maPB);
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    cbBoxChucVu.DataSource = ds.Tables[0];
                    cbBoxChucVu.DisplayMember = "TenCV_KhangCD233181";
                    cbBoxChucVu.ValueMember = "MaCV_KhangCD233181";
                }
                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load chức vụ: " + ex.Message);
            }
        }

        // Load danh sách chấm công theo điều kiện
        private void LoadChamCongTheoDieuKien()
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT 
                        cc.Id_TuanhCD233018 AS [ID],
                        cc.MaChamCong_TuanhCD233018 AS [Mã chấm công],
                        nv.MaNV_TuanhCD233018 AS [Mã NV],
                        nv.HoTen_TuanhCD233018 AS [Họ tên],
                        pb.TenPB_ThuanCD233318 AS [Phòng ban],
                        cv.TenCV_KhangCD233181 AS [Chức vụ],
                        cc.Ngay_TuanhCD233018 AS [Ngày],
                        CONVERT(VARCHAR(8), cc.GioVao_TuanhCD233018, 108) AS [Giờ vào],
                        CONVERT(VARCHAR(8), cc.GioVe_TuanhCD233018, 108) AS [Giờ về],
                        cc.Ghichu_TuanhCD233018 AS [Ghi chú]
                    FROM tblChamCong_TuanhCD233018 cc
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE cc.DeletedAt_TuanhCD233018 = 0
                      AND nv.DeletedAt_TuanhCD233018 = 0
                      AND cv.DeletedAt_KhangCD233181 = 0
                      AND pb.DeletedAt_ThuanCD233318 = 0";

                SqlCommand cmd = new SqlCommand(sql, cn.conn);

                // Lọc theo phòng ban
                if (cbBoxMaPB.SelectedValue != null && !string.IsNullOrEmpty(cbBoxMaPB.SelectedValue.ToString()))
                {
                    sql += " AND pb.MaPB_ThuanCD233318 = @MaPB";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@MaPB", cbBoxMaPB.SelectedValue.ToString());
                }

                // Lọc theo chức vụ
                if (cbBoxChucVu.SelectedValue != null && !string.IsNullOrEmpty(cbBoxChucVu.SelectedValue.ToString()))
                {
                    sql += " AND cv.MaCV_KhangCD233181 = @MaCV";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@MaCV", cbBoxChucVu.SelectedValue.ToString());
                }

                sql += " ORDER BY cc.Ngay_TuanhCD233018 DESC, nv.HoTen_TuanhCD233018";
                cmd.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtGridViewChamCong.DataSource = dt;

                // Ẩn cột ID
                if (dtGridViewChamCong.Columns["ID"] != null)
                    dtGridViewChamCong.Columns["ID"].Visible = false;

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                cn.disconnect();
            }
        }

        // ===== LOAD TOÀN BỘ CHẤM CÔNG =====
        private void LoadAllChamCong()
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT 
                        cc.Id_TuanhCD233018 AS [ID],
                        cc.MaChamCong_TuanhCD233018 AS [Mã chấm công],
                        nv.MaNV_TuanhCD233018 AS [Mã NV],
                        nv.HoTen_TuanhCD233018 AS [Họ tên],
                        pb.TenPB_ThuanCD233318 AS [Phòng ban],
                        cv.TenCV_KhangCD233181 AS [Chức vụ],
                        cc.Ngay_TuanhCD233018 AS [Ngày],
                        CONVERT(VARCHAR(8), cc.GioVao_TuanhCD233018, 108) AS [Giờ vào],
                        CONVERT(VARCHAR(8), cc.GioVe_TuanhCD233018, 108) AS [Giờ về],
                        cc.Ghichu_TuanhCD233018 AS [Ghi chú]
                    FROM tblChamCong_TuanhCD233018 cc
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE cc.DeletedAt_TuanhCD233018 = 0
                      AND nv.DeletedAt_TuanhCD233018 = 0
                    ORDER BY cc.Ngay_TuanhCD233018 DESC, nv.HoTen_TuanhCD233018";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewChamCong.DataSource = dt;

                    // Ẩn cột ID
                    if (dtGridViewChamCong.Columns["ID"] != null)
                        dtGridViewChamCong.Columns["ID"].Visible = false;
                }

                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // ===== LOAD CHẤM CÔNG THEO NGÀY =====
        private void LoadChamCongTheoNgay(DateTime ngay)
        {
            try
            {
                cn.connect();

                string sql = @"
                    SELECT 
                        cc.Id_TuanhCD233018 AS [ID],
                        cc.MaChamCong_TuanhCD233018 AS [Mã chấm công],
                        nv.MaNV_TuanhCD233018 AS [Mã NV],
                        nv.HoTen_TuanhCD233018 AS [Họ tên],
                        pb.TenPB_ThuanCD233318 AS [Phòng ban],
                        cv.TenCV_KhangCD233181 AS [Chức vụ],
                        cc.Ngay_TuanhCD233018 AS [Ngày],
                        CONVERT(VARCHAR(8), cc.GioVao_TuanhCD233018, 108) AS [Giờ vào],
                        CONVERT(VARCHAR(8), cc.GioVe_TuanhCD233018, 108) AS [Giờ về],
                        cc.Ghichu_TuanhCD233018 AS [Ghi chú]
                    FROM tblChamCong_TuanhCD233018 cc
                    INNER JOIN tblNhanVien_TuanhCD233018 nv ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
                    INNER JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    INNER JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE cc.DeletedAt_TuanhCD233018 = 0
                      AND nv.DeletedAt_TuanhCD233018 = 0
                      AND CAST(cc.Ngay_TuanhCD233018 AS DATE) = @Ngay
                    ORDER BY nv.HoTen_TuanhCD233018";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Ngay", ngay.Date);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewChamCong.DataSource = dt;

                    // Ẩn cột ID
                    if (dtGridViewChamCong.Columns["ID"] != null)
                        dtGridViewChamCong.Columns["ID"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load chấm công theo ngày: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // ===== SỰ KIỆN THAY ĐỔI NGÀY =====
        private void dateTimeNgayChamCong_ValueChanged(object sender, EventArgs e)
        {
            LoadChamCongTheoNgay(dateTimeNgayChamCong.Value);
        }

        // ===== SỰ KIỆN THAY ĐỔI PHÒNG BAN =====
        private void cbBoxMaPB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBoxMaPB.SelectedValue != null)
            {
                string maPB = cbBoxMaPB.SelectedValue.ToString();

                // Load chức vụ theo phòng ban
                LoadChucVuComboBox(maPB);

                // Load danh sách chấm công theo phòng ban
                LoadChamCongTheoDieuKien();
            }
        }

        // ===== SỰ KIỆN THAY ĐỔI CHỨC VỤ =====
        private void cbBoxChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadChamCongTheoDieuKien();
        }

        // ===== CLICK VÀO DATAGRIDVIEW =====
        private void dtGridViewChamCong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                DataGridViewRow row = dtGridViewChamCong.Rows[e.RowIndex];

                tbMaChamCong.Text = row.Cells["Mã chấm công"].Value?.ToString() ?? "";
                tbmanhanvien.Text = row.Cells["Mã NV"].Value?.ToString() ?? "";
                tbtennhanvien.Text = row.Cells["Họ tên"].Value?.ToString() ?? "";

                if (row.Cells["Ngày"].Value != null && row.Cells["Ngày"].Value != DBNull.Value)
                {
                    dateTimeNgayChamCong.Value = Convert.ToDateTime(row.Cells["Ngày"].Value);
                }

                tbGioVao.Text = row.Cells["Giờ vào"].Value?.ToString() ?? "";
                tbGioVe.Text = row.Cells["Giờ về"].Value?.ToString() ?? "";
                tbGhiChu.Text = row.Cells["Ghi chú"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== NÚT THÊM =====
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrWhiteSpace(tbmanhanvien.Text) ||
                    string.IsNullOrWhiteSpace(tbGioVao.Text) ||
                    string.IsNullOrWhiteSpace(tbGioVe.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin: Mã NV, Giờ vào, Giờ về!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();

                // Kiểm tra mã nhân viên có tồn tại không
                string checkNV = @"SELECT Id_TuanhCD233018 FROM tblNhanVien_TuanhCD233018 
                                  WHERE MaNV_TuanhCD233018 = @MaNV AND DeletedAt_TuanhCD233018 = 0";

                int nhanVienId = 0;
                using (SqlCommand cmd = new SqlCommand(checkNV, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", tbmanhanvien.Text.Trim());
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("Mã nhân viên không tồn tại trong hệ thống!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }

                    nhanVienId = Convert.ToInt32(result);
                }

                // Kiểm tra đã chấm công trong ngày chưa
                string checkExist = @"SELECT COUNT(*) FROM tblChamCong_TuanhCD233018 
                                     WHERE NhanVienId_TuanhCD233018 = @NhanVienId 
                                       AND CAST(Ngay_TuanhCD233018 AS DATE) = @Ngay
                                       AND DeletedAt_TuanhCD233018 = 0";

                using (SqlCommand cmd = new SqlCommand(checkExist, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@NhanVienId", nhanVienId);
                    cmd.Parameters.AddWithValue("@Ngay", dateTimeNgayChamCong.Value.Date);

                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Nhân viên này đã có chấm công trong ngày này!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // Tạo mã chấm công tự động
                string maChamCong = GenerateMaChamCong();

                // Thêm chấm công mới
                string insert = @"INSERT INTO tblChamCong_TuanhCD233018
                                 (MaChamCong_TuanhCD233018, Ngay_TuanhCD233018, GioVao_TuanhCD233018, 
                                  GioVe_TuanhCD233018, Ghichu_TuanhCD233018, DeletedAt_TuanhCD233018, 
                                  NhanVienId_TuanhCD233018)
                                 VALUES (@MaChamCong, @Ngay, @GioVao, @GioVe, @GhiChu, 0, @NhanVienId)";

                using (SqlCommand cmd = new SqlCommand(insert, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaChamCong", maChamCong);
                    cmd.Parameters.AddWithValue("@Ngay", dateTimeNgayChamCong.Value.Date);
                    cmd.Parameters.AddWithValue("@GioVao", TimeSpan.Parse(tbGioVao.Text.Trim()));
                    cmd.Parameters.AddWithValue("@GioVe", TimeSpan.Parse(tbGioVe.Text.Trim()));
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());
                    cmd.Parameters.AddWithValue("@NhanVienId", nhanVienId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm chấm công thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllChamCong();
                        ClearInputs();
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Giờ vào/về không đúng định dạng! Vui lòng nhập theo mẫu: HH:mm:ss (VD: 08:30:00)",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm chấm công: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // ===== NÚT SỬA =====
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbMaChamCong.Text))
                {
                    MessageBox.Show("Vui lòng chọn bản ghi cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbGioVao.Text) ||
                    string.IsNullOrWhiteSpace(tbGioVe.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Giờ vào và Giờ về!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn sửa bản ghi chấm công này không?",
                    "Xác nhận sửa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                cn.connect();

                string update = @"UPDATE tblChamCong_TuanhCD233018
                                 SET Ngay_TuanhCD233018 = @Ngay,
                                     GioVao_TuanhCD233018 = @GioVao,
                                     GioVe_TuanhCD233018 = @GioVe,
                                     Ghichu_TuanhCD233018 = @GhiChu
                                 WHERE MaChamCong_TuanhCD233018 = @MaChamCong
                                   AND DeletedAt_TuanhCD233018 = 0";

                using (SqlCommand cmd = new SqlCommand(update, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaChamCong", tbMaChamCong.Text.Trim());
                    cmd.Parameters.AddWithValue("@Ngay", dateTimeNgayChamCong.Value.Date);
                    cmd.Parameters.AddWithValue("@GioVao", TimeSpan.Parse(tbGioVao.Text.Trim()));
                    cmd.Parameters.AddWithValue("@GioVe", TimeSpan.Parse(tbGioVe.Text.Trim()));
                    cmd.Parameters.AddWithValue("@GhiChu", tbGhiChu.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật chấm công thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllChamCong();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy bản ghi để cập nhật!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Giờ vào/về không đúng định dạng! Vui lòng nhập theo mẫu: HH:mm:ss (VD: 08:30:00)",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // ===== NÚT XÓA =====
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbMaChamCong.Text))
                {
                    MessageBox.Show("Vui lòng chọn bản ghi cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa bản ghi chấm công này không?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                cn.connect();

                // Kiểm tra xem chấm công này đã được dùng trong bảng lương chưa
                string checkLuong = @"SELECT COUNT(*) FROM tblLuong_ChienCD232928 
                                     WHERE ChamCongId_TuanhCD233018 = (
                                         SELECT Id_TuanhCD233018 FROM tblChamCong_TuanhCD233018 
                                         WHERE MaChamCong_TuanhCD233018 = @MaChamCong
                                     ) AND DeletedAt_ChienCD232928 = 0";

                using (SqlCommand cmd = new SqlCommand(checkLuong, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaChamCong", tbMaChamCong.Text.Trim());
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Không thể xóa! Chấm công này đang được sử dụng trong bảng lương.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cn.disconnect();
                        return;
                    }
                }

                // Xóa mềm (soft delete)
                string delete = @"UPDATE tblChamCong_TuanhCD233018 
                                 SET DeletedAt_TuanhCD233018 = 1
                                 WHERE MaChamCong_TuanhCD233018 = @MaChamCong";

                using (SqlCommand cmd = new SqlCommand(delete, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@MaChamCong", tbMaChamCong.Text.Trim());

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa chấm công thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadAllChamCong();
                        ClearInputs();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }

        // ===== NÚT TÌM KIẾM =====
        // ===== NÚT TÌM KIẾM (ĐÃ SỬA) =====
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sql = @"
            SELECT 
                cc.Id_TuanhCD233018 AS [ID],
                cc.MaChamCong_TuanhCD233018 AS [Mã chấm công],
                nv.MaNV_TuanhCD233018 AS [Mã NV],
                nv.HoTen_TuanhCD233018 AS [Họ tên],
                pb.TenPB_ThuanCD233318 AS [Phòng ban],
                cv.TenCV_KhangCD233181 AS [Chức vụ],
                cc.Ngay_TuanhCD233018 AS [Ngày],
                CONVERT(VARCHAR(8), cc.GioVao_TuanhCD233018, 108) AS [Giờ vào],
                CONVERT(VARCHAR(8), cc.GioVe_TuanhCD233018, 108) AS [Giờ về],
                cc.Ghichu_TuanhCD233018 AS [Ghi chú]
            FROM tblChamCong_TuanhCD233018 cc
            INNER JOIN tblNhanVien_TuanhCD233018 nv 
                ON cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
            INNER JOIN tblChucVu_KhangCD233181 cv 
                ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
            INNER JOIN tblPhongBan_ThuanCD233318 pb 
                ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
            WHERE cc.DeletedAt_TuanhCD233018 = 0
              AND nv.DeletedAt_TuanhCD233018 = 0
              AND cv.DeletedAt_KhangCD233181 = 0
              AND pb.DeletedAt_ThuanCD233318 = 0";

                SqlCommand cmd = new SqlCommand(sql, cn.conn);

                // ===== LỌC THEO NGÀY (BẮT BUỘC) =====
                // Lấy ngày từ DateTimePicker
                DateTime ngayTimKiem = dateTimeNgayChamCong.Value.Date;
                sql += " AND CAST(cc.Ngay_TuanhCD233018 AS DATE) = @Ngay";
                cmd.Parameters.AddWithValue("@Ngay", ngayTimKiem);

                // ===== LỌC THEO MÃ NV HOẶC TÊN NV (NẾU CÓ NHẬP) =====\
                string maNV = tbmanhanvien.Text.Trim();
                if (!string.IsNullOrWhiteSpace(maNV))
                {
                    sql += " AND nv.MaNV_TuanhCD233018 LIKE @MaNV";
                    cmd.Parameters.AddWithValue("@MaNV", "%" + maNV + "%");
                }

                // ===== LỌC THEO TÊN NV (NẾU CÓ NHẬP) =====
                string tenNV = tbtennhanvien.Text.Trim();
                if (!string.IsNullOrWhiteSpace(tenNV))
                {
                    sql += " AND nv.HoTen_TuanhCD233018 LIKE @TenNV";
                    cmd.Parameters.AddWithValue("@TenNV", "%" + tenNV + "%");
                }

                // ===== LỌC THEO PHÒNG BAN (NẾU ĐÃ CHỌN) =====
                string maPB = cbBoxMaPB.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(maPB))
                {
                    sql += " AND pb.MaPB_ThuanCD233318 = @MaPB";
                    cmd.Parameters.AddWithValue("@MaPB", maPB);
                }

                // ===== LỌC THEO CHỨC VỤ (NẾU ĐÃ CHỌN) =====
                string maCV = cbBoxChucVu.SelectedValue?.ToString();
                if (!string.IsNullOrEmpty(maCV))
                {
                    sql += " AND cv.MaCV_KhangCD233181 = @MaCV";
                    cmd.Parameters.AddWithValue("@MaCV", maCV);
                }

                sql += " ORDER BY nv.HoTen_TuanhCD233018";
                cmd.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dtGridViewChamCong.DataSource = dt;

                // Ẩn cột ID
                if (dtGridViewChamCong.Columns["ID"] != null)
                    dtGridViewChamCong.Columns["ID"].Visible = false;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(
                        $"Không tìm thấy dữ liệu chấm công cho ngày {ngayTimKiem:dd/MM/yyyy}!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Tìm thấy {dt.Rows.Count} bản ghi chấm công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.disconnect();
            }
        }
        // ===== TẠO MÃ CHẤM CÔNG TỰ ĐỘNG =====
        private string GenerateMaChamCong()
        {
            string newMa = "CC001";

            try
            {
                string query = @"SELECT TOP 1 MaChamCong_TuanhCD233018
                                FROM tblChamCong_TuanhCD233018
                                WHERE MaChamCong_TuanhCD233018 LIKE 'CC%'
                                ORDER BY Id_TuanhCD233018 DESC";

                using (SqlCommand cmd = new SqlCommand(query, cn.conn))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string lastMa = result.ToString().Trim();

                        // Lấy phần số ở cuối
                        string so = "";
                        for (int i = lastMa.Length - 1; i >= 0; i--)
                        {
                            if (char.IsDigit(lastMa[i]))
                                so = lastMa[i] + so;
                            else
                                break;
                        }

                        if (int.TryParse(so, out int number))
                        {
                            number++;
                            newMa = "CC" + number.ToString("D3");
                        }
                    }
                }
            }
            catch { }

            return newMa;
        }

        // ===== XÓA CÁC Ô NHẬP LIỆU =====
        private void ClearInputs()
        {
            tbMaChamCong.Clear();
            tbmanhanvien.Clear();
            tbtennhanvien.Clear();
            tbGioVao.Clear();
            tbGioVe.Clear();
            tbGhiChu.Clear();
            dateTimeNgayChamCong.Value = DateTime.Now;
        }

        // ===== CÁC SỰ KIỆN KHÁC =====
        private void panel4_Paint(object sender, PaintEventArgs e)
        {
        }

        private void tbMaChamCong_TextChanged(object sender, EventArgs e)
        {
        }

        private void label8_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        // ===== XUẤT PDF =====
        private void xuatpdf_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra có dữ liệu không
                if (dtGridViewChamCong.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo tên file tự động
                string fileName = $"BaoCaoChamCong_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                // Chọn nơi lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF Files|*.pdf";
                saveFileDialog.Title = "Lưu báo cáo chấm công";
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                // Tạo PDF
                Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

                document.Open();

                // ===== FONT TIẾNG VIỆT =====
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font companyFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font smallFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL);

                // ===== TÊN CÔNG TY =====
                Paragraph company = new Paragraph("CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM", companyFont);
                company.Alignment = Element.ALIGN_CENTER;
                company.SpacingAfter = 5;
                document.Add(company);

                // ===== TIÊU ĐỀ =====
                Paragraph title = new Paragraph("BÁO CÁO CHẤM CÔNG NHÂN VIÊN", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 10;
                document.Add(title);

                // ===== NGÀY XUẤT (THỜI GIAN HIỆN TẠI) =====
                Paragraph date = new Paragraph($"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", normalFont);
                date.Alignment = Element.ALIGN_LEFT;
                date.SpacingAfter = 10;
                document.Add(date);

                // ===== THÔNG TIN PHÒNG BAN =====
                Paragraph pb = new Paragraph("Phòng Ban: " + cbBoxMaPB.Text, normalFont);
                date.Alignment = Element.ALIGN_LEFT;
                date.SpacingAfter = 10;
                document.Add(pb);


                // ===== THÔNG TIN CHỨC VỤ =====
                Paragraph cv = new Paragraph("Chức vụ: " + (cbBoxChucVu.Text != "" ? cbBoxChucVu.Text : "Tất cả"), normalFont);
                date.Alignment = Element.ALIGN_LEFT;
                date.SpacingAfter = 10;
                document.Add(cv);


                // ===== TẠO BẢNG =====
                int visibleColumns = dtGridViewChamCong.Columns.Cast<DataGridViewColumn>().Count(c => c.Visible);
                PdfPTable table = new PdfPTable(visibleColumns);
                table.WidthPercentage = 100;
                table.SpacingBefore = 20f;


                float[] columnWidths = new float[visibleColumns];
                for (int i = 0; i < visibleColumns; i++)
                {
                    columnWidths[i] = 100f / visibleColumns;
                }
                table.SetWidths(columnWidths);

                // ===== HEADER =====
                for (int i = 0; i < dtGridViewChamCong.Columns.Count; i++)
                {
                    if (dtGridViewChamCong.Columns[i].Visible)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dtGridViewChamCong.Columns[i].HeaderText, headerFont));
                        cell.BackgroundColor = new BaseColor(211, 211, 211);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 5;
                        table.AddCell(cell);
                    }
                }

                // ===== DATA =====
                foreach (DataGridViewRow dgvRow in dtGridViewChamCong.Rows)
                {
                    if (dgvRow.IsNewRow) continue;

                    for (int j = 0; j < dtGridViewChamCong.Columns.Count; j++)
                    {
                        if (dtGridViewChamCong.Columns[j].Visible)
                        {
                            var cellValue = dgvRow.Cells[j].Value;
                            string displayValue = "";

                            if (cellValue is DateTime)
                            {
                                displayValue = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                displayValue = cellValue?.ToString() ?? "";
                            }

                            PdfPCell cell = new PdfPCell(new Phrase(displayValue, smallFont));
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Padding = 3;
                            table.AddCell(cell);
                        }
                    }
                }

                document.Add(table);

                // ===== CHỮ KÝ =====
                Paragraph space = new Paragraph("\n", normalFont);
                document.Add(space);

                PdfPTable signatureTable = new PdfPTable(2);
                signatureTable.WidthPercentage = 100;
                signatureTable.SetWidths(new float[] { 50f, 50f });

                PdfPCell leftCell = new PdfPCell();
                leftCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                signatureTable.AddCell(leftCell);

                PdfPCell rightCell = new PdfPCell();
                rightCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                rightCell.HorizontalAlignment = Element.ALIGN_CENTER;

                Paragraph signatureDate = new Paragraph($"Hà Nội, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}", normalFont);
                signatureDate.Alignment = Element.ALIGN_CENTER;
                rightCell.AddElement(signatureDate);

                Paragraph signatureTitle = new Paragraph("Người lập báo cáo", headerFont);
                signatureTitle.Alignment = Element.ALIGN_CENTER;
                signatureTitle.SpacingBefore = 5;
                rightCell.AddElement(signatureTitle);

                Paragraph signatureName = new Paragraph($"\n\n\n{nguoiDangNhap}", headerFont);
                signatureName.Alignment = Element.ALIGN_CENTER;
                rightCell.AddElement(signatureName);

                signatureTable.AddCell(rightCell);
                document.Add(signatureTable);

                document.Close();

                MessageBox.Show("Xuất PDF thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Mở file tự động
                System.Diagnostics.Process.Start(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ===== XUẤT EXCEL =====
        private void xuatexcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewChamCong.Rows.Count > 0)
            {
                // Tạo tên file tự động
                string fileName = $"BaoCaoChamCong_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    sfd.FileName = fileName;

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("ChamCong");

                                // ===== TÊN CÔNG TY =====
                                ws.Cell(1, 1).Value = "CÔNG TY TNHH WISTRON INFOCOMM VIỆT NAM";
                                ws.Range(1, 1, 1, 10).Merge();
                                ws.Cell(1, 1).Style.Font.Bold = true;
                                ws.Cell(1, 1).Style.Font.FontSize = 14;
                                ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(1, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                // ===== TIÊU ĐỀ CHÍNH =====
                                ws.Cell(2, 1).Value = "BÁO CÁO CHẤM CÔNG NHÂN VIÊN";
                                ws.Range(2, 1, 2, 10).Merge();
                                ws.Cell(2, 1).Style.Font.Bold = true;
                                ws.Cell(2, 1).Style.Font.FontSize = 16;
                                ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(2, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                                // ===== NGÀY XUẤT (THỜI GIAN HIỆN TẠI) =====
                                ws.Cell(3, 1).Value = "Ngày xuất báo cáo: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                ws.Cell(3, 1).Style.Font.Italic = true;

                                // ===== THÔNG TIN PHÒNG BAN & CHỨC VỤ =====
                                ws.Cell(5, 1).Value = "Phòng Ban";
                                ws.Cell(5, 2).Value = cbBoxMaPB.Text;
                                ws.Cell(6, 1).Value = "Chức vụ";
                                ws.Cell(6, 2).Value = cbBoxChucVu.Text != "" ? cbBoxChucVu.Text : "Tất cả";

                                // ===== HEADER BẢNG DỮ LIỆU =====
                                int startRow = 7;
                                ws.Cell(startRow, 1).Value = "DANH SÁCH CHẤM CÔNG";
                                ws.Range(startRow, 1, startRow, 10).Merge();
                                ws.Cell(startRow, 1).Style.Font.Bold = true;
                                ws.Cell(startRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Cell(startRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;

                                // ===== TIÊU ĐỀ CỘT =====
                                int headerRow = startRow + 1;

                                int colIndex = 1;
                                for (int i = 0; i < dtGridViewChamCong.Columns.Count; i++)
                                {
                                    if (dtGridViewChamCong.Columns[i].Visible)
                                    {
                                        ws.Cell(headerRow, colIndex).Value = dtGridViewChamCong.Columns[i].HeaderText;
                                        ws.Cell(headerRow, colIndex).Style.Font.Bold = true;
                                        ws.Cell(headerRow, colIndex).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                        ws.Cell(headerRow, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                                        colIndex++;
                                    }
                                }

                                // ===== GHI DỮ LIỆU =====
                                int dataStartRow = headerRow + 1;
                                for (int i = 0; i < dtGridViewChamCong.Rows.Count; i++)
                                {
                                    colIndex = 1;
                                    for (int j = 0; j < dtGridViewChamCong.Columns.Count; j++)
                                    {
                                        if (dtGridViewChamCong.Columns[j].Visible)
                                        {
                                            var cellValue = dtGridViewChamCong.Rows[i].Cells[j].Value;

                                            if (cellValue is DateTime)
                                            {
                                                ws.Cell(dataStartRow + i, colIndex).Value = ((DateTime)cellValue).ToString("dd/MM/yyyy");
                                            }
                                            else
                                            {
                                                ws.Cell(dataStartRow + i, colIndex).Value = cellValue?.ToString();
                                            }

                                            ws.Cell(dataStartRow + i, colIndex).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                            colIndex++;
                                        }
                                    }
                                }

                                // ===== BORDER CHO BẢNG DỮ LIỆU =====
                                int lastDataRow = dataStartRow + dtGridViewChamCong.Rows.Count - 1;
                                int totalColumns = dtGridViewChamCong.Columns.Cast<DataGridViewColumn>().Count(c => c.Visible);
                                var tableRange = ws.Range(startRow, 1, lastDataRow, totalColumns);
                                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // ===== PHẦN CHỮ KÝ =====
                                int signatureRow = lastDataRow + 2;
                                ws.Cell(signatureRow, 8).Value = "Hà Nội, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                                ws.Cell(signatureRow, 8).Style.Font.Italic = true;
                                ws.Cell(signatureRow, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow, 8, signatureRow, 10).Merge();

                                ws.Cell(signatureRow + 1, 8).Value = "Người lập báo cáo";
                                ws.Cell(signatureRow + 1, 8).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 1, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 1, 8, signatureRow + 1, 10).Merge();

                                ws.Cell(signatureRow + 4, 8).Value = nguoiDangNhap;
                                ws.Cell(signatureRow + 4, 8).Style.Font.Bold = true;
                                ws.Cell(signatureRow + 4, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Range(signatureRow + 4, 8, signatureRow + 4, 10).Merge();

                                // ===== TỰ ĐỘNG ĐIỀU CHỈNH CỘT =====
                                ws.Columns().AdjustToContents();

                                for (int i = 1; i <= totalColumns; i++)
                                {
                                    if (ws.Column(i).Width < 12)
                                        ws.Column(i).Width = 12;
                                }

                                ws.Row(1).Height = 20;
                                ws.Row(2).Height = 25;
                                ws.Row(startRow).Height = 20;
                                ws.Row(headerRow).Height = 20;

                                // Lưu file
                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Mở file tự động
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}