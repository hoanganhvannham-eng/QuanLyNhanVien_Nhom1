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

namespace QuanLyNhanVien3
{
    public partial class F_BaoCaoChamCong : Form
    {
        public F_BaoCaoChamCong()
        {
            InitializeComponent();
        }

        // Khởi tạo đối tượng kết nối
        connectData cn = new connectData();

        // Biến lưu trạng thái hiện tại để nút tìm kiếm biết đang ở bảng nào
        // 0: Mặc định (chưa chọn gì hoặc xem lịch sử chi tiết)
        // 1: Đang xem Báo cáo Số ngày làm việc
        // 2: Đang xem Báo cáo Đi trễ về sớm
        private int currentMode = 0;

        // Hàm xóa input (nếu cần dùng sau này)
        private void ClearAllInputs(Control parent)
        {
            foreach (Control ctl in parent.Controls)
            {
                if (ctl is TextBox)
                    ((TextBox)ctl).Clear();
                else if (ctl is ComboBox)
                    ((ComboBox)ctl).SelectedIndex = -1;
                else if (ctl.HasChildren)
                    ClearAllInputs(ctl);
            }
        }

        // =======================================================
        // 1. Nút: Số ngày làm việc mỗi nhân viên trong tháng
        // =======================================================
        private void btnSoNgayLamViec_Click(object sender, EventArgs e)
        {
            try
            {
                // Đánh dấu trạng thái đang xem là "Số ngày làm việc"
                currentMode = 1;
                cn.connect();

                // Lấy tháng và năm từ DateTimePicker trên giao diện
                int thang = dtpThoiGian.Value.Month;
                int nam = dtpThoiGian.Value.Year;

                string sql = @"SELECT nv.MaNV as N'Mã Nhân Viên', nv.HoTen as N'Họ Tên', pb.TenPB as N'Tên Phòng Ban',
                               COUNT(cc.Id) AS N'Số ngày làm việc'
                               FROM tblNhanVien nv
                               JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                               JOIN tblChamCong cc ON nv.MaNV = cc.MaNV 
                               WHERE nv.DeletedAt = 0 
                                 AND cc.DeletedAt = 0
                                 AND MONTH(cc.Ngay) = @Thang 
                                 AND YEAR(cc.Ngay) = @Nam
                               GROUP BY nv.MaNV, nv.HoTen, pb.TenPB
                               ORDER BY N'Số ngày làm việc' DESC, nv.HoTen;";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCChamCong.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =======================================================
        // 2. Nút: Nhân viên đi trễ hoặc về sớm
        // =======================================================
        private void btnDiTreVeSom_Click(object sender, EventArgs e)
        {
            try
            {
                // Đánh dấu trạng thái đang xem là "Đi trễ về sớm"
                currentMode = 2;
                cn.connect();

                int thang = dtpThoiGian.Value.Month;
                int nam = dtpThoiGian.Value.Year;

                // SQL: Tính toán chi tiết số phút đi muộn, về sớm
                string sql = @"SELECT nv.MaNV as N'Mã Nhân Viên', nv.HoTen as N'Họ Tên', cc.Ngay as N'Ngày', cc.GioVao as N'Giờ Vào', cc.GioVe as N'Giờ Về',
                               CASE 
                                   -- Trường hợp 1: Đi sớm/đúng giờ VÀ Về muộn/đúng giờ -> Tốt
                                   WHEN cc.GioVao <= '08:00:00' AND cc.GioVe >= '17:00:00' 
                                        THEN N'Đi làm đúng giờ'
                                   
                                   -- Trường hợp 2: Đi sớm/đúng giờ VÀ Về sớm -> Chỉ bị lỗi về sớm
                                   WHEN cc.GioVao <= '08:00:00' AND cc.GioVe < '17:00:00' 
                                        THEN N'Đi đúng giờ - Về sớm ' + CAST(DATEDIFF(MINUTE, cc.GioVe, '17:00:00') AS NVARCHAR(20)) + N' phút'
                                   
                                   -- Trường hợp 3: Đi muộn VÀ Về muộn/đúng giờ -> Chỉ bị lỗi đi muộn
                                   WHEN cc.GioVao > '08:00:00' AND cc.GioVe >= '17:00:00' 
                                        THEN N'Đi muộn ' + CAST(DATEDIFF(MINUTE, '08:00:00', cc.GioVao) AS NVARCHAR(20)) + N' phút - Về đúng giờ'
                                   
                                   -- Trường hợp 4: Đi muộn VÀ Về sớm -> Bị cả hai
                                   ELSE N'Đi muộn ' + CAST(DATEDIFF(MINUTE, '08:00:00', cc.GioVao) AS NVARCHAR(20)) + N' phút - Về sớm ' + CAST(DATEDIFF(MINUTE, cc.GioVe, '17:00:00') AS NVARCHAR(20)) + N' phút'
                               END AS N'Trạng Thái'
                               FROM tblChamCong cc
                               JOIN tblNhanVien nv ON cc.MaNV = nv.MaNV
                               WHERE cc.DeletedAt = 0
                                 AND MONTH(cc.Ngay) = @Thang 
                                 AND YEAR(cc.Ngay) = @Nam
                               ORDER BY cc.Ngay DESC, nv.HoTen;";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCChamCong.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu đi trễ/về sớm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =======================================================
        // 3. Nút: Tìm kiếm (Thông minh theo ngữ cảnh)
        // =======================================================
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTimkiem.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên hoặc mã nhân viên để tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();
                int thang = dtpThoiGian.Value.Month;
                int nam = dtpThoiGian.Value.Year;
                string tuKhoa = "%" + txtTimkiem.Text + "%";
                string sql = "";

                // Kiểm tra xem đang ở chế độ xem báo cáo nào để tìm kiếm tương ứng
                if (currentMode == 1)
                {
                    // --- TÌM KIẾM TRONG BẢNG SỐ NGÀY LÀM VIỆC ---
                    sql = @"SELECT nv.MaNV as 'Mã Nhân Viên', nv.HoTen as 'Họ Tên', 
                            pb.TenPB as N'Tên Phòng Ban', COUNT(cc.Id) AS N'Số Ngày Làm Việc'
                            FROM tblNhanVien nv
                            JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                            JOIN tblChamCong cc ON nv.MaNV = cc.MaNV 
                            WHERE nv.DeletedAt = 0 
                              AND cc.DeletedAt = 0
                              AND MONTH(cc.Ngay) = @Thang 
                              AND YEAR(cc.Ngay) = @Nam
                              AND (nv.HoTen LIKE @TuKhoa OR nv.MaNV LIKE @TuKhoa) -- Điều kiện tìm kiếm thêm vào đây
                            GROUP BY nv.MaNV, nv.HoTen, pb.TenPB
                            ORDER BY N'Số Ngày Làm Việc' DESC, nv.HoTen;";
                }
                else if (currentMode == 2)
                {
                    // --- TÌM KIẾM TRONG BẢNG ĐI TRỄ VỀ SỚM ---
                    sql = @"SELECT nv.MaNV as N'Mã Nhân Viên', nv.HoTen as N'Họ Tên', cc.Ngay as N'Ngày', cc.GioVao as N'Giờ Vào', cc.GioVe as N'Giờ Về',
                            CASE 
                                WHEN cc.GioVao <= '08:00:00' AND cc.GioVe >= '17:00:00' THEN N'Đi làm đúng giờ'
                                WHEN cc.GioVao <= '08:00:00' AND cc.GioVe < '17:00:00' THEN N'Đi đúng giờ - Về sớm ' + CAST(DATEDIFF(MINUTE, cc.GioVe, '17:00:00') AS NVARCHAR(20)) + N' phút'
                                WHEN cc.GioVao > '08:00:00' AND cc.GioVe >= '17:00:00' THEN N'Đi muộn ' + CAST(DATEDIFF(MINUTE, '08:00:00', cc.GioVao) AS NVARCHAR(20)) + N' phút - Về đúng giờ'
                                ELSE N'Đi muộn ' + CAST(DATEDIFF(MINUTE, '08:00:00', cc.GioVao) AS NVARCHAR(20)) + N' phút - Về sớm ' + CAST(DATEDIFF(MINUTE, cc.GioVe, '17:00:00') AS NVARCHAR(20)) + N' phút'
                            END AS N'Trạng Thái'
                            FROM tblChamCong cc
                            JOIN tblNhanVien nv ON cc.MaNV = nv.MaNV
                            WHERE cc.DeletedAt = 0
                              AND MONTH(cc.Ngay) = @Thang 
                              AND YEAR(cc.Ngay) = @Nam
                              AND (nv.HoTen LIKE @TuKhoa OR nv.MaNV LIKE @TuKhoa) -- Điều kiện tìm kiếm thêm vào đây
                            ORDER BY cc.Ngay DESC, nv.HoTen;";
                }
                else
                {
                    // --- MẶC ĐỊNH (Nếu chưa chọn bảng nào): Tìm lịch sử chấm công gốc ---
                    sql = @"SELECT nv.MaNV as N'Mã Nhân Viên', nv.HoTen as 'Họ Tên', cc.Ngay as N'Ngày', cc.GioVao as N'Giờ Vào', cc.GioVe as N'Giờ Về'
                            FROM tblChamCong cc
                            JOIN tblNhanVien nv ON cc.MaNV = nv.MaNV
                            WHERE cc.DeletedAt = 0
                              AND MONTH(cc.Ngay) = @Thang 
                              AND YEAR(cc.Ngay) = @Nam
                              AND (nv.HoTen LIKE @TuKhoa OR nv.MaNV LIKE @TuKhoa)
                            ORDER BY cc.Ngay DESC;";
                }

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@Thang", thang);
                    cmd.Parameters.AddWithValue("@Nam", nam);
                    cmd.Parameters.AddWithValue("@TuKhoa", tuKhoa);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCChamCong.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }

        // =======================================================
        // 4. Nút: Xuất Excel (Sử dụng ClosedXML)
        // =======================================================
        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCChamCong.Rows.Count > 0)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", FileName = "BaoCaoChamCong.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var ws = wb.Worksheets.Add("BaoCaoChamCong");

                                // 1. Ghi Header (Tiêu đề cột)
                                for (int i = 0; i < dtGridViewBCChamCong.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewBCChamCong.Columns[i].HeaderText;

                                    // Format Header cho đẹp
                                    ws.Cell(1, i + 1).Style.Font.Bold = true;
                                    ws.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                    ws.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                }

                                // 2. Ghi Dữ liệu
                                for (int i = 0; i < dtGridViewBCChamCong.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewBCChamCong.Columns.Count; j++)
                                    {
                                        // Kiểm tra null an toàn
                                        var cellValue = dtGridViewBCChamCong    .Rows[i].Cells[j].Value;
                                        ws.Cell(i + 2, j + 1).Value = cellValue != null ? cellValue.ToString() : "";
                                    }
                                }

                                // 3. Kẻ khung (Border) cho toàn bộ bảng dữ liệu
                                var range = ws.Range(1, 1, dtGridViewBCChamCong.Rows.Count + 1, dtGridViewBCChamCong.Columns.Count);
                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                // 4. Tự động co giãn cột (AutoFit)
                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}