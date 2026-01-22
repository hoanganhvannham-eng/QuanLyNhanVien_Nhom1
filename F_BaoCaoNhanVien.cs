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
    public partial class F_BaoCaoNhanVien : Form
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

                string sqlLoadDataNhanVien = @"
                    SELECT 
                        nv.MaNV_TuanhCD233018 as N'Mã Nhân Viên', 
                        nv.HoTen_TuanhCD233018 as N'Họ Tên', 
                        pb.TenPB_ThuanCD233318 as N'Tên Phòng Ban', 
                        cv.TenCV_KhangCD233181 as N'Tên Chức Vụ', 
                        nv.Email_TuanhCD233018 as N'Email'
                    FROM tblNhanVien_TuanhCD233018 nv
                    JOIN tblChucVu_KhangCD233181 cv ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181
                    JOIN tblPhongBan_ThuanCD233318 pb ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                    ORDER BY pb.TenPB_ThuanCD233318, cv.TenCV_KhangCD233181;";

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

                string sqlLoadDataNhanVien = @"
                    SELECT 
                        GioiTinh_TuanhCD233018 as N'Giới Tính', 
                        COUNT(*) as N'Số Lượng'
                    FROM tblNhanVien_TuanhCD233018
                    WHERE DeletedAt_TuanhCD233018 = 0
                    GROUP BY GioiTinh_TuanhCD233018;";

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

        void timkiemtheomanhanvien()
        {
            try
            {
                cn.connect();
                string sql = @"
                    SELECT DISTINCT
                        nv.Id_TuanhCD233018       AS N'ID',
                        nv.MaNV_TuanhCD233018     AS N'Mã nhân viên',
                        nv.HoTen_TuanhCD233018    AS N'Họ tên',
                        nv.NgaySinh_TuanhCD233018 AS N'Ngày sinh',
                        nv.GioiTinh_TuanhCD233018 AS N'Giới tính',
                        nv.DiaChi_TuanhCD233018   AS N'Địa chỉ',
                        nv.SoDienThoai_TuanhCD233018 AS N'Số điện thoại',
                        nv.Email_TuanhCD233018    AS N'Email',
                        pb.TenPB_ThuanCD233318    AS N'Phòng ban',
                        cv.TenCV_KhangCD233181    AS N'Chức vụ',
                        hd.LuongCoBan_ChienCD232928 AS N'Lương cơ bản',
                        hd.LoaiHopDong_ChienCD232928 AS N'Loại hợp đồng'
                    FROM tblNhanVien_TuanhCD233018 nv
                    LEFT JOIN tblChucVu_KhangCD233181 cv 
                        ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181 
                        AND cv.DeletedAt_KhangCD233181 = 0
                    LEFT JOIN tblPhongBan_ThuanCD233318 pb 
                        ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318 
                        AND pb.DeletedAt_ThuanCD233318 = 0
                    LEFT JOIN tblHopDong_ChienCD232928 hd 
                        ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018
                        AND hd.DeletedAt_ChienCD232928 = 0
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                      AND nv.MaNV_TuanhCD233018 LIKE @TenTimKiem
                    ORDER BY pb.TenPB_ThuanCD233318, cv.TenCV_KhangCD233181";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenTimKiem",
                        "%" + textBoxmanhanvientimkiem.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dtGridViewBCNhanVien.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void timkiemtheoten()
        {
            try
            {
                cn.connect();
                string sql = @"
                    SELECT DISTINCT
                        nv.Id_TuanhCD233018       AS N'ID',
                        nv.MaNV_TuanhCD233018     AS N'Mã nhân viên',
                        nv.HoTen_TuanhCD233018    AS N'Họ tên',
                        nv.NgaySinh_TuanhCD233018 AS N'Ngày sinh',
                        nv.GioiTinh_TuanhCD233018 AS N'Giới tính',
                        nv.DiaChi_TuanhCD233018   AS N'Địa chỉ',
                        nv.SoDienThoai_TuanhCD233018 AS N'Số điện thoại',
                        nv.Email_TuanhCD233018    AS N'Email',
                        pb.TenPB_ThuanCD233318    AS N'Phòng ban',
                        cv.TenCV_KhangCD233181    AS N'Chức vụ',
                        hd.LuongCoBan_ChienCD232928 AS N'Lương cơ bản',
                        hd.LoaiHopDong_ChienCD232928 AS N'Loại hợp đồng'
                    FROM tblNhanVien_TuanhCD233018 nv
                    LEFT JOIN tblChucVu_KhangCD233181 cv 
                        ON nv.MaCV_KhangCD233181 = cv.MaCV_KhangCD233181 
                        AND cv.DeletedAt_KhangCD233181 = 0
                    LEFT JOIN tblPhongBan_ThuanCD233318 pb 
                        ON cv.MaPB_ThuanCD233318 = pb.MaPB_ThuanCD233318 
                        AND pb.DeletedAt_ThuanCD233318 = 0
                    LEFT JOIN tblHopDong_ChienCD232928 hd 
                        ON nv.MaNV_TuanhCD233018 = hd.MaNV_TuanhCD233018
                        AND hd.DeletedAt_ChienCD232928 = 0
                    WHERE nv.DeletedAt_TuanhCD233018 = 0
                      AND nv.HoTen_TuanhCD233018 LIKE @TenTimKiem
                    ORDER BY pb.TenPB_ThuanCD233318, cv.TenCV_KhangCD233181";

                using (SqlCommand cmd = new SqlCommand(sql, cn.conn))
                {
                    cmd.Parameters.AddWithValue("@TenTimKiem",
                        "%" + txttimkiemtheoten.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dtGridViewBCNhanVien.DataSource = dt;
                }

                cn.disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiemTheoTen_Click(object sender, EventArgs e)
        {
        }

        private void txttimkiemtheoten_TextChanged(object sender, EventArgs e)
        {
            timkiemtheoten();
        }

        private void textBoxmanhanvientimkiem_TextChanged(object sender, EventArgs e)
        {
            timkiemtheomanhanvien();
        }

        private void F_BaoCaoNhanVien_Load(object sender, EventArgs e)
        {

        }

        private void dtGridViewBCNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txttimkiemtheoten_TextChanged_1(object sender, EventArgs e)
        {
            timkiemtheoten();
        }

        private void textBoxmanhanvientimkiem_TextChanged_1(object sender, EventArgs e)
        {
            timkiemtheomanhanvien();
        }
    }
}