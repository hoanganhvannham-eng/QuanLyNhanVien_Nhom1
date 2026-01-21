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

namespace QuanLyNhanVien3
{
    public partial class F_ChamCongChiTiet : Form
    {
        connectData cn = new connectData();

        public F_ChamCongChiTiet()
        {
            InitializeComponent();
        }

        private void F_ChamCongChiTiet_Load(object sender, EventArgs e)
        {
            LoadAllChamCong();
            dateTimeNgayChamCong.Value = DateTime.Now;
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
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = tbmanhanvien.Text.Trim();

                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    MessageBox.Show("Vui lòng nhập Mã NV hoặc Tên nhân viên để tìm kiếm!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                      AND (nv.MaNV_TuanhCD233018 LIKE @TuKhoa OR nv.HoTen_TuanhCD233018 LIKE @TuKhoa)
                    ORDER BY cc.Ngay_TuanhCD233018 DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TuKhoa", "%" + tuKhoa + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dtGridViewChamCong.DataSource = dt;

                    // Ẩn cột ID
                    if (dtGridViewChamCong.Columns["ID"] != null)
                        dtGridViewChamCong.Columns["ID"].Visible = false;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Không tìm thấy kết quả nào!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
    }
}