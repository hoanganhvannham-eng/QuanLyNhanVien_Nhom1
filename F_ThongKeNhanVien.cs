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
    public partial class F_ThongKeNhanVien: Form
    {
        public F_ThongKeNhanVien()
        {
            InitializeComponent();
        }

        connectData c = new connectData();

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                if (!rdbTheoNgay.Checked && !rdbTheoThang.Checked)
                {
                    MessageBox.Show("Vui lòng chọn kiểu thống kê theo Ngày hoặc Tháng!",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = "";
                c.connect();

                // ============= TRƯỜNG HỢP THỐNG KÊ LƯƠNG =============
                if (rdbLuong.Checked)
                {
                    sql = @"SELECT 
                            nv.MaNV AS [Mã Nhân Viên],
                            nv.HoTen AS [Họ và Tên],
                            nv.MaPB AS [Mã Phòng Ban],
                            nv.MaCV AS [Mã Chức Vụ],
                            l.MaLuong AS [Mã Lương],
                            l.LuongCoBan AS [Lương Cơ Bản],
                            l.PhuCap AS [Phụ Cấp],
                            l.KhauTru AS [Khấu Trừ],
                            l.TongLuong AS [Tổng Lương]
                        FROM tblNhanVien AS nv
                        INNER JOIN tblLuong AS l ON nv.MaNV = l.MaNV
                        WHERE l.DeletedAt = 0";


                    // Nếu chọn theo ngày thì tự chuyển sang theo tháng
                    if (rdbTheoNgay.Checked)
                    {
                        MessageBox.Show("Thống kê Lương chỉ hỗ trợ theo Tháng, hệ thống sẽ tự động chuyển sang Thống kê theo Tháng!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // chuyển radio button
                        rdbTheoNgay.Checked = false;
                        rdbTheoThang.Checked = true;
                    }
                    // lọc theo tháng
                    if (rdbTheoThang.Checked)
                    {
                        if (numThang.Value == 0 || numNam.Value == 0)
                        {
                            MessageBox.Show("Vui lòng nhập Tháng và Năm hợp lệ!",
                                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        sql += " AND l.Thang = @Thang AND l.Nam = @Nam";
                    }

                    // lọc theo mã nhân viên
                    if (!string.IsNullOrEmpty(txtMaNV.Text.Trim()))
                    {
                        sql += " AND nv.MaNV = @MaNV";
                    }

                    // Thêm ORDER BY sau cùng
                    sql += " ORDER BY nv.MaNV";

                }

                // ============= TRƯỜNG HỢP THỐNG KÊ CHẤM CÔNG =============
                else if (rdbChamCong.Checked)
                {
                    sql = @"SELECT 
                                nv.MaNV AS [Mã Nhân Viên],
                                nv.HoTen AS [Họ và Tên],
                                nv.MaPB AS [Mã Phòng Ban],
                                nv.MaCV AS [Mã Chức Vụ],
                                cc.MaChamCong AS [Mã Chấm Công],
                                cc.Ngay AS [Ngày],
                                cc.GioVao AS [Giờ Vào],
                                cc.GioVe AS [Giờ Về],
                                CASE 
                                    WHEN DATEDIFF(HOUR, cc.GioVao, cc.GioVe) >= 8 
                                        THEN N'Đủ' 
                                    ELSE N'Không Đủ' 
                                END AS [Trạng Thái]
                            FROM tblNhanVien AS nv
                            INNER JOIN tblChamCong AS cc ON nv.MaNV = cc.MaNV
                            WHERE cc.DeletedAt = 0";


                    // Nếu chọn theo tháng thì tự chuyển sang theo ngày
                    if (rdbTheoThang.Checked)
                    {
                        MessageBox.Show("Thống kê Chấm công chỉ hỗ trợ theo Ngày, hệ thống sẽ tự động chuyển sang Thống kê theo Ngày!",
                                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // chuyển radio button
                        rdbTheoThang.Checked = false;
                        rdbTheoNgay.Checked = true;
                    }

                    // lọc theo ngày
                    if (rdbTheoNgay.Checked)
                    {
                        sql += " AND cc.Ngay BETWEEN @FromDate AND @ToDate";
                    }

                    // lọc theo mã nhân viên
                    if (!string.IsNullOrEmpty(txtMaNV.Text.Trim()))
                    {
                        sql += " AND nv.MaNV = @MaNV";
                    }

                    // Thêm ORDER BY cuối cùng
                    sql += " ORDER BY cc.Ngay, nv.MaNV";

                }
                else
                {
                    MessageBox.Show("Vui lòng chọn loại thống kê (Lương / Chấm công)",
                                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlCommand cmd = new SqlCommand(sql, c.conn))
                {
                    // Tham số ngày
                    if (rdbTheoNgay.Checked)
                    {
                        cmd.Parameters.AddWithValue("@FromDate", dtpFromDate.Value.Date);
                        cmd.Parameters.AddWithValue("@ToDate", dtpToDate.Value.Date);
                    }

                    // Tham số tháng/năm
                    if (rdbTheoThang.Checked)
                    {
                        cmd.Parameters.AddWithValue("@Thang", Convert.ToInt32(numThang.Value));
                        cmd.Parameters.AddWithValue("@Nam", Convert.ToInt32(numNam.Value));
                    }

                    // Tham số mã NV
                    if (!string.IsNullOrEmpty(txtMaNV.Text.Trim()))
                    {
                        cmd.Parameters.AddWithValue("@MaNV", txtMaNV.Text.Trim());
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dtGridViewThongKe.DataSource = dt;// Sau khi gán dữ liệu

                    // Định dạng cột "Tổng Lương" (tên phải khớp với tên cột trong DataTable)
                    if (dtGridViewThongKe.Columns.Contains("Tổng Lương"))
                    {
                        dtGridViewThongKe.Columns["Tổng Lương"].DefaultCellStyle.Format = "c0";
                        dtGridViewThongKe.Columns["Tổng Lương"].DefaultCellStyle.FormatProvider =
                            new System.Globalization.CultureInfo("vi-VN");
                    }
                    if (dtGridViewThongKe.Columns.Contains("Khấu Trừ"))
                    {
                        dtGridViewThongKe.Columns["Khấu Trừ"].DefaultCellStyle.Format = "c0";
                        dtGridViewThongKe.Columns["Khấu Trừ"].DefaultCellStyle.FormatProvider =
                            new System.Globalization.CultureInfo("vi-VN");
                    }
                    if (dtGridViewThongKe.Columns.Contains("Phụ Cấp"))
                    {
                    dtGridViewThongKe.Columns["Phụ Cấp"].DefaultCellStyle.Format = "c0";
                        dtGridViewThongKe.Columns["Phụ Cấp"].DefaultCellStyle.FormatProvider =
                            new System.Globalization.CultureInfo("vi-VN");
                    }
                    if (dtGridViewThongKe.Columns.Contains("Lương Cơ Bản"))
                    {
                        dtGridViewThongKe.Columns["Lương Cơ Bản"].DefaultCellStyle.Format = "c0";
                        dtGridViewThongKe.Columns["Lương Cơ Bản"].DefaultCellStyle.FormatProvider =
                            new System.Globalization.CultureInfo("vi-VN");
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                MessageBox.Show("Chi tiết lỗi: " + ex.ToString(), "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                c.disconnect();
            }
        }

        private void F_ThongKeNhanVien_Load(object sender, EventArgs e)
        {
            numThang.Value = DateTime.Now.Month;
            numNam.Value = DateTime.Now.Year;
            rdbTheoNgay.Checked = true; // mặc định theo ngày
                                        // Gắn sự kiện
            rdbLuong.CheckedChanged += rdbLuong_CheckedChanged;
            rdbChamCong.CheckedChanged += rdbChamCong_CheckedChanged;
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtGridViewThongKe.Rows.Count > 0)
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
                                for (int i = 0; i < dtGridViewThongKe.Columns.Count; i++)
                                {
                                    ws.Cell(1, i + 1).Value = dtGridViewThongKe.Columns[i].HeaderText;
                                }

                                // Ghi dữ liệu
                                for (int i = 0; i < dtGridViewThongKe.Rows.Count; i++)
                                {
                                    for (int j = 0; j < dtGridViewThongKe.Columns.Count; j++)
                                    {
                                        ws.Cell(i + 2, j + 1).Value = dtGridViewThongKe.Rows[i].Cells[j].Value?.ToString();
                                    }
                                }

                                // Thêm border
                                var range = ws.Range(1, 1, dtGridViewThongKe.Rows.Count + 1, dtGridViewThongKe.Columns.Count);
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

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdbLuong_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbLuong.Checked)
            {
                // Khi chọn Lương -> mặc định theo tháng
                rdbTheoThang.Checked = true;

                numThang.Value = DateTime.Now.Month;
                numNam.Value = DateTime.Now.Year;
            }
        }

        private void rdbChamCong_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbChamCong.Checked)
            {
                // Khi chọn Chấm công -> mặc định theo ngày
                rdbTheoNgay.Checked = true;

                dtpFromDate.Value = DateTime.Now.AddDays(-7); // mặc định từ 7 ngày trước
                dtpToDate.Value = DateTime.Now;               // đến hôm nay
            }
        }


    }
}
