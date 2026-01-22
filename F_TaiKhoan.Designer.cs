namespace QuanLyNhanVien3
{
    partial class F_TaiKhoan
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.gbMiddle = new System.Windows.Forms.GroupBox();
            this.labelMK = new System.Windows.Forms.Label();
            this.tbMatKhau = new System.Windows.Forms.TextBox();
            this.labelQuyen = new System.Windows.Forms.Label();
            this.cbBoxQuyen = new System.Windows.Forms.ComboBox();
            this.labelGhiChu = new System.Windows.Forms.Label();
            this.tbGhiChu = new System.Windows.Forms.TextBox();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.gbRight = new System.Windows.Forms.GroupBox();
            this.labelAdminMK = new System.Windows.Forms.Label();
            this.tbMKkhoiphuc = new System.Windows.Forms.TextBox();
            this.checkshowpassword = new System.Windows.Forms.CheckBox();
            this.btnHienThiPhongBanCu = new System.Windows.Forms.Button();
            this.btnKhoiPhucPhongBan = new System.Windows.Forms.Button();
            this.btnrestar = new System.Windows.Forms.Button();
            this.btnxuatExcel = new System.Windows.Forms.Button();
            this.gbLeft = new System.Windows.Forms.GroupBox();
            this.labelMaTK = new System.Windows.Forms.Label();
            this.tbmaTK = new System.Windows.Forms.TextBox();
            this.labelNV = new System.Windows.Forms.Label();
            this.cbBoxMaNV = new System.Windows.Forms.ComboBox();
            this.labelSDT = new System.Windows.Forms.Label();
            this.tbTenDangNhap = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.gbGrid = new System.Windows.Forms.GroupBox();
            this.dataGridViewTaiKhoan = new System.Windows.Forms.DataGridView();
            this.panelHeader.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.gbMiddle.SuspendLayout();
            this.gbRight.SuspendLayout();
            this.gbLeft.SuspendLayout();
            this.panelGrid.SuspendLayout();
            this.gbGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTaiKhoan)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblSubTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1280, 80);
            this.panelHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(288, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ TÀI KHOẢN";
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubTitle.Location = new System.Drawing.Point(22, 45);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(316, 23);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Tạo, phân quyền và khôi phục tài khoản";
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelTop.Controls.Add(this.gbMiddle);
            this.panelTop.Controls.Add(this.gbRight);
            this.panelTop.Controls.Add(this.gbLeft);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 80);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12);
            this.panelTop.Size = new System.Drawing.Size(1280, 320);
            this.panelTop.TabIndex = 1;
            // 
            // gbMiddle
            // 
            this.gbMiddle.Controls.Add(this.labelMK);
            this.gbMiddle.Controls.Add(this.tbMatKhau);
            this.gbMiddle.Controls.Add(this.labelQuyen);
            this.gbMiddle.Controls.Add(this.cbBoxQuyen);
            this.gbMiddle.Controls.Add(this.labelGhiChu);
            this.gbMiddle.Controls.Add(this.tbGhiChu);
            this.gbMiddle.Controls.Add(this.btnSua);
            this.gbMiddle.Controls.Add(this.btnTimKiem);
            this.gbMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMiddle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbMiddle.Location = new System.Drawing.Point(412, 12);
            this.gbMiddle.Name = "gbMiddle";
            this.gbMiddle.Size = new System.Drawing.Size(436, 296);
            this.gbMiddle.TabIndex = 0;
            this.gbMiddle.TabStop = false;
            this.gbMiddle.Text = "Bảo mật và phân quyền";
            // 
            // labelMK
            // 
            this.labelMK.Location = new System.Drawing.Point(20, 40);
            this.labelMK.Name = "labelMK";
            this.labelMK.Size = new System.Drawing.Size(100, 23);
            this.labelMK.TabIndex = 0;
            this.labelMK.Text = "Mật khẩu";
            // 
            // tbMatKhau
            // 
            this.tbMatKhau.Location = new System.Drawing.Point(160, 38);
            this.tbMatKhau.Name = "tbMatKhau";
            this.tbMatKhau.Size = new System.Drawing.Size(200, 30);
            this.tbMatKhau.TabIndex = 1;
            // 
            // labelQuyen
            // 
            this.labelQuyen.Location = new System.Drawing.Point(20, 80);
            this.labelQuyen.Name = "labelQuyen";
            this.labelQuyen.Size = new System.Drawing.Size(100, 23);
            this.labelQuyen.TabIndex = 2;
            this.labelQuyen.Text = "Quyền";
            // 
            // cbBoxQuyen
            // 
            this.cbBoxQuyen.Location = new System.Drawing.Point(160, 78);
            this.cbBoxQuyen.Name = "cbBoxQuyen";
            this.cbBoxQuyen.Size = new System.Drawing.Size(200, 31);
            this.cbBoxQuyen.TabIndex = 3;
            // 
            // labelGhiChu
            // 
            this.labelGhiChu.Location = new System.Drawing.Point(20, 120);
            this.labelGhiChu.Name = "labelGhiChu";
            this.labelGhiChu.Size = new System.Drawing.Size(100, 23);
            this.labelGhiChu.TabIndex = 4;
            this.labelGhiChu.Text = "Ghi chú";
            // 
            // tbGhiChu
            // 
            this.tbGhiChu.Location = new System.Drawing.Point(160, 118);
            this.tbGhiChu.Name = "tbGhiChu";
            this.tbGhiChu.Size = new System.Drawing.Size(200, 30);
            this.tbGhiChu.TabIndex = 5;
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(156, 205);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(75, 32);
            this.btnSua.TabIndex = 6;
            this.btnSua.Text = "Sửa";
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click_1);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(256, 205);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(75, 32);
            this.btnTimKiem.TabIndex = 7;
            this.btnTimKiem.Text = "Tìm";
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click_1);
            // 
            // gbRight
            // 
            this.gbRight.Controls.Add(this.labelAdminMK);
            this.gbRight.Controls.Add(this.tbMKkhoiphuc);
            this.gbRight.Controls.Add(this.checkshowpassword);
            this.gbRight.Controls.Add(this.btnHienThiPhongBanCu);
            this.gbRight.Controls.Add(this.btnKhoiPhucPhongBan);
            this.gbRight.Controls.Add(this.btnrestar);
            this.gbRight.Controls.Add(this.btnxuatExcel);
            this.gbRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.gbRight.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbRight.Location = new System.Drawing.Point(848, 12);
            this.gbRight.Name = "gbRight";
            this.gbRight.Size = new System.Drawing.Size(420, 296);
            this.gbRight.TabIndex = 1;
            this.gbRight.TabStop = false;
            this.gbRight.Text = "Khôi phục tài khoản (Admin)";
            // 
            // labelAdminMK
            // 
            this.labelAdminMK.Location = new System.Drawing.Point(20, 40);
            this.labelAdminMK.Name = "labelAdminMK";
            this.labelAdminMK.Size = new System.Drawing.Size(100, 23);
            this.labelAdminMK.TabIndex = 0;
            this.labelAdminMK.Text = "Mật khẩu Admin";
            // 
            // tbMKkhoiphuc
            // 
            this.tbMKkhoiphuc.Location = new System.Drawing.Point(20, 65);
            this.tbMKkhoiphuc.Name = "tbMKkhoiphuc";
            this.tbMKkhoiphuc.Size = new System.Drawing.Size(260, 30);
            this.tbMKkhoiphuc.TabIndex = 1;
            this.tbMKkhoiphuc.UseSystemPasswordChar = true;
            // 
            // checkshowpassword
            // 
            this.checkshowpassword.Location = new System.Drawing.Point(20, 95);
            this.checkshowpassword.Name = "checkshowpassword";
            this.checkshowpassword.Size = new System.Drawing.Size(104, 24);
            this.checkshowpassword.TabIndex = 2;
            this.checkshowpassword.Text = "Hiển thị mật khẩu";
            // 
            // btnHienThiPhongBanCu
            // 
            this.btnHienThiPhongBanCu.Location = new System.Drawing.Point(20, 130);
            this.btnHienThiPhongBanCu.Name = "btnHienThiPhongBanCu";
            this.btnHienThiPhongBanCu.Size = new System.Drawing.Size(260, 32);
            this.btnHienThiPhongBanCu.TabIndex = 3;
            this.btnHienThiPhongBanCu.Text = "Hiển thị tài khoản cũ";
            this.btnHienThiPhongBanCu.Click += new System.EventHandler(this.btnHienThiPhongBanCu_Click_1);
            // 
            // btnKhoiPhucPhongBan
            // 
            this.btnKhoiPhucPhongBan.Location = new System.Drawing.Point(20, 165);
            this.btnKhoiPhucPhongBan.Name = "btnKhoiPhucPhongBan";
            this.btnKhoiPhucPhongBan.Size = new System.Drawing.Size(260, 32);
            this.btnKhoiPhucPhongBan.TabIndex = 4;
            this.btnKhoiPhucPhongBan.Text = "Khôi phục tài khoản";
            this.btnKhoiPhucPhongBan.Click += new System.EventHandler(this.btnKhoiPhucPhongBan_Click_1);
            // 
            // btnrestar
            // 
            this.btnrestar.Location = new System.Drawing.Point(20, 205);
            this.btnrestar.Name = "btnrestar";
            this.btnrestar.Size = new System.Drawing.Size(104, 32);
            this.btnrestar.TabIndex = 5;
            this.btnrestar.Text = "Làm mới";
            this.btnrestar.Click += new System.EventHandler(this.btnrestar_Click_1);
            // 
            // btnxuatExcel
            // 
            this.btnxuatExcel.Location = new System.Drawing.Point(158, 205);
            this.btnxuatExcel.Name = "btnxuatExcel";
            this.btnxuatExcel.Size = new System.Drawing.Size(122, 32);
            this.btnxuatExcel.TabIndex = 6;
            this.btnxuatExcel.Text = "Xuất Excel";
            this.btnxuatExcel.Click += new System.EventHandler(this.btnxuatExcel_Click_1);
            // 
            // gbLeft
            // 
            this.gbLeft.Controls.Add(this.labelMaTK);
            this.gbLeft.Controls.Add(this.tbmaTK);
            this.gbLeft.Controls.Add(this.labelNV);
            this.gbLeft.Controls.Add(this.cbBoxMaNV);
            this.gbLeft.Controls.Add(this.labelSDT);
            this.gbLeft.Controls.Add(this.tbTenDangNhap);
            this.gbLeft.Controls.Add(this.btnThem);
            this.gbLeft.Controls.Add(this.btnXoa);
            this.gbLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbLeft.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbLeft.Location = new System.Drawing.Point(12, 12);
            this.gbLeft.Name = "gbLeft";
            this.gbLeft.Size = new System.Drawing.Size(400, 296);
            this.gbLeft.TabIndex = 2;
            this.gbLeft.TabStop = false;
            this.gbLeft.Text = "Thông tin tài khoản";
            // 
            // labelMaTK
            // 
            this.labelMaTK.Location = new System.Drawing.Point(20, 40);
            this.labelMaTK.Name = "labelMaTK";
            this.labelMaTK.Size = new System.Drawing.Size(100, 23);
            this.labelMaTK.TabIndex = 0;
            this.labelMaTK.Text = "Mã tài khoản";
            // 
            // tbmaTK
            // 
            this.tbmaTK.Location = new System.Drawing.Point(160, 38);
            this.tbmaTK.Name = "tbmaTK";
            this.tbmaTK.Size = new System.Drawing.Size(200, 30);
            this.tbmaTK.TabIndex = 1;
            // 
            // labelNV
            // 
            this.labelNV.Location = new System.Drawing.Point(20, 80);
            this.labelNV.Name = "labelNV";
            this.labelNV.Size = new System.Drawing.Size(100, 23);
            this.labelNV.TabIndex = 2;
            this.labelNV.Text = "Nhân viên";
            // 
            // cbBoxMaNV
            // 
            this.cbBoxMaNV.Location = new System.Drawing.Point(160, 78);
            this.cbBoxMaNV.Name = "cbBoxMaNV";
            this.cbBoxMaNV.Size = new System.Drawing.Size(200, 31);
            this.cbBoxMaNV.TabIndex = 3;
            // 
            // labelSDT
            // 
            this.labelSDT.Location = new System.Drawing.Point(20, 120);
            this.labelSDT.Name = "labelSDT";
            this.labelSDT.Size = new System.Drawing.Size(100, 23);
            this.labelSDT.TabIndex = 4;
            this.labelSDT.Text = "SĐT / Tên đăng nhập";
            // 
            // tbTenDangNhap
            // 
            this.tbTenDangNhap.Location = new System.Drawing.Point(160, 118);
            this.tbTenDangNhap.Name = "tbTenDangNhap";
            this.tbTenDangNhap.Size = new System.Drawing.Size(200, 30);
            this.tbTenDangNhap.TabIndex = 5;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(151, 165);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 32);
            this.btnThem.TabIndex = 6;
            this.btnThem.Text = "Thêm";
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click_1);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(251, 165);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 32);
            this.btnXoa.TabIndex = 7;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click_1);
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.gbGrid);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 400);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Padding = new System.Windows.Forms.Padding(12);
            this.panelGrid.Size = new System.Drawing.Size(1280, 400);
            this.panelGrid.TabIndex = 0;
            // 
            // gbGrid
            // 
            this.gbGrid.Controls.Add(this.dataGridViewTaiKhoan);
            this.gbGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGrid.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbGrid.Location = new System.Drawing.Point(12, 12);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(1256, 376);
            this.gbGrid.TabIndex = 0;
            this.gbGrid.TabStop = false;
            this.gbGrid.Text = "Danh sách tài khoản";
            // 
            // dataGridViewTaiKhoan
            // 
            this.dataGridViewTaiKhoan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTaiKhoan.ColumnHeadersHeight = 29;
            this.dataGridViewTaiKhoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTaiKhoan.Location = new System.Drawing.Point(3, 26);
            this.dataGridViewTaiKhoan.Name = "dataGridViewTaiKhoan";
            this.dataGridViewTaiKhoan.ReadOnly = true;
            this.dataGridViewTaiKhoan.RowHeadersWidth = 51;
            this.dataGridViewTaiKhoan.Size = new System.Drawing.Size(1250, 347);
            this.dataGridViewTaiKhoan.TabIndex = 0;
            this.dataGridViewTaiKhoan.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTaiKhoan_CellClick_1);
            // 
            // F_TaiKhoan
            // 
            this.ClientSize = new System.Drawing.Size(1280, 800);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelHeader);
            this.Name = "F_TaiKhoan";
            this.Text = "Tài khoản";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_TaiKhoan_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.gbMiddle.ResumeLayout(false);
            this.gbMiddle.PerformLayout();
            this.gbRight.ResumeLayout(false);
            this.gbRight.PerformLayout();
            this.gbLeft.ResumeLayout(false);
            this.gbLeft.PerformLayout();
            this.panelGrid.ResumeLayout(false);
            this.gbGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTaiKhoan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // ===== DECLARE =====
        private System.Windows.Forms.Panel panelHeader, panelTop, panelGrid;
        private System.Windows.Forms.Label lblTitle, lblSubTitle;

        private System.Windows.Forms.GroupBox gbLeft, gbMiddle, gbRight, gbGrid;
        private System.Windows.Forms.DataGridView dataGridViewTaiKhoan;

        private System.Windows.Forms.Label labelMaTK, labelNV, labelSDT;
        private System.Windows.Forms.Label labelMK, labelQuyen, labelGhiChu;
        private System.Windows.Forms.Label labelAdminMK;

        private System.Windows.Forms.TextBox tbmaTK, tbTenDangNhap, tbMatKhau, tbGhiChu, tbMKkhoiphuc;
        private System.Windows.Forms.ComboBox cbBoxMaNV, cbBoxQuyen;
        private System.Windows.Forms.CheckBox checkshowpassword;

        private System.Windows.Forms.Button btnThem, btnXoa, btnSua, btnTimKiem;
        private System.Windows.Forms.Button btnHienThiPhongBanCu, btnKhoiPhucPhongBan;
        private System.Windows.Forms.Button btnrestar, btnxuatExcel;
    }
}
