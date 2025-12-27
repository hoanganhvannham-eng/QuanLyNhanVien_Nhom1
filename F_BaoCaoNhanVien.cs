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
    public partial class F_BaoCaoNhanVien: Form
    {
        public F_BaoCaoNhanVien()
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

        private void btnThongKeNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT nv.MaNV as N'Mã Nhân Viên', nv.HoTen as N'Họ Tên', 
                                                    pb.TenPB as N'Tên Phòng Ban', cv.TenCV as N'Tên Chức Vụ', nv.Email
                                                    FROM tblNhanVien nv
                                                    JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                                                    JOIN tblChucVu cv ON nv.MaCV = cv.MaCV
                                                    WHERE nv.DeletedAt = 0
                                                    ORDER BY pb.TenPB, cv.TenCV;"; 

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsoluongtheogioitinh_Click(object sender, EventArgs e)
        {
            try
            {
                cn.connect();

                string sqlLoadDataNhanVien = @"SELECT GioiTinh as N'Giới Tính', COUNT(*) as N'Số Lượng'
                                                FROM tblNhanVien
                                                WHERE DeletedAt = 0
                                                GROUP BY GioiTinh;
                                                ";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlLoadDataNhanVien, cn.conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }
                cn.disconnect();
                ClearAllInputs(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu nhân viên: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXuatEXL_Click(object sender, EventArgs e)
        {
            if (dtGridViewBCNhanVien.Rows.Count > 0)
            {
                string fileName = $"BaoCaoNhanVien_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";

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
                                var ws = wb.Worksheets.Add("NhanVien");

                                int colCount = dtGridViewBCNhanVien.Columns.Count;

                                /* ========== TIÊU ĐỀ ========= */
                                ws.Cell(1, 1).Value = "BÁO CÁO DANH SÁCH NHÂN VIÊN";
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
                                    ws.Cell(4, i + 1).Value =
                                        dtGridViewBCNhanVien.Columns[i].HeaderText;

                                    ws.Cell(4, i + 1).Style.Font.Bold = true;
                                    ws.Cell(4, i + 1).Style.Alignment.Horizontal =
                                        XLAlignmentHorizontalValues.Center;
                                    ws.Cell(4, i + 1).Style.Fill.BackgroundColor =
                                        XLColor.LightGray;
                                }

                                /* ========== DỮ LIỆU ========= */
                                for (int i = 0; i < dtGridViewBCNhanVien.Rows.Count; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        var value = dtGridViewBCNhanVien.Rows[i].Cells[j].Value;
                                        ws.Cell(i + 5, j + 1).Value =
                                            value != null ? value.ToString() : "";
                                    }
                                }

                                /* ========== BORDER ========= */
                                var range = ws.Range(
                                    4, 1,
                                    dtGridViewBCNhanVien.Rows.Count + 4,
                                    colCount
                                );

                                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                                /* ========== AUTO SIZE ========= */
                                ws.Columns().AdjustToContents();

                                wb.SaveAs(sfd.FileName);
                            }

                            MessageBox.Show("Xuất Excel thành công!",
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

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txttimkiemtheoten.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nhân viên để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.connect();   // MỞ KẾT NỐI

                string sql = @"SELECT nv.MaNV as N'Mã Nhân Viên', nv.HoTen as N'Họ Tên', 
                       pb.TenPB as N'Phòng Ban', cv.TenCV as N'Tên Chức Vụ', nv.Email
                       FROM tblNhanVien nv
                       JOIN tblPhongBan pb ON nv.MaPB = pb.MaPB
                       JOIN tblChucVu cv ON nv.MaCV = cv.MaCV
                       WHERE nv.DeletedAt = 0 
                             AND nv.HoTen LIKE @TenTimKiem
                       ORDER BY pb.TenPB, cv.TenCV;";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    // chỉ thêm 1 lần % thôi
                    cmd.Parameters.AddWithValue("@TenTimKiem", "%" + txttimkiemtheoten.Text + "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dtGridViewBCNhanVien.DataSource = dt;
                }

                cn.disconnect(); // ĐÓNG KẾT NỐI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }
    }
}
