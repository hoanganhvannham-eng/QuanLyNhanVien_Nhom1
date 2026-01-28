namespace QuanLyNhanVien3
{
    partial class F_BaoCaoNhanVienChiTiet
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
            this.groupCaNhan = new System.Windows.Forms.GroupBox();
            this.lblMaNV = new System.Windows.Forms.Label();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.lblGioiTinh = new System.Windows.Forms.Label();
            this.lblSDT = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtMaNV = new System.Windows.Forms.TextBox();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.txtGioiTinh = new System.Windows.Forms.TextBox();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.groupCongViec = new System.Windows.Forms.GroupBox();
            this.lblPhongBan = new System.Windows.Forms.Label();
            this.lblChucVu = new System.Windows.Forms.Label();
            this.lblNgayVaoLam = new System.Windows.Forms.Label();
            this.lblTrangThaiLV = new System.Windows.Forms.Label();
            this.txtPhongBan = new System.Windows.Forms.TextBox();
            this.txtChucVu = new System.Windows.Forms.TextBox();
            this.dtpNgayVaoLam = new System.Windows.Forms.DateTimePicker();
            this.txtTrangThaiLamViec = new System.Windows.Forms.TextBox();
            this.groupHopDong = new System.Windows.Forms.GroupBox();
            this.lblNgayBD = new System.Windows.Forms.Label();
            this.lblNgayKT = new System.Windows.Forms.Label();
            this.lblTrangThaiHD = new System.Windows.Forms.Label();
            this.dtpNgayBD = new System.Windows.Forms.DateTimePicker();
            this.dtpNgayKT = new System.Windows.Forms.DateTimePicker();
            this.txtTrangThaiHD = new System.Windows.Forms.TextBox();
            this.groupLichSuHD = new System.Windows.Forms.GroupBox();
            this.dgvHopDong = new System.Windows.Forms.DataGridView();
            this.btnDong = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.groupCaNhan.SuspendLayout();
            this.groupCongViec.SuspendLayout();
            this.groupHopDong.SuspendLayout();
            this.groupLichSuHD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHopDong)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(64)))), ((int)(((byte)(175)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1203, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(368, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BÁO CÁO CHI TIẾT NHÂN VIÊN";
            // 
            // groupCaNhan
            // 
            this.groupCaNhan.Controls.Add(this.lblMaNV);
            this.groupCaNhan.Controls.Add(this.lblHoTen);
            this.groupCaNhan.Controls.Add(this.lblNgaySinh);
            this.groupCaNhan.Controls.Add(this.lblGioiTinh);
            this.groupCaNhan.Controls.Add(this.lblSDT);
            this.groupCaNhan.Controls.Add(this.lblEmail);
            this.groupCaNhan.Controls.Add(this.txtMaNV);
            this.groupCaNhan.Controls.Add(this.txtHoTen);
            this.groupCaNhan.Controls.Add(this.dtpNgaySinh);
            this.groupCaNhan.Controls.Add(this.txtGioiTinh);
            this.groupCaNhan.Controls.Add(this.txtSDT);
            this.groupCaNhan.Controls.Add(this.txtEmail);
            this.groupCaNhan.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupCaNhan.Location = new System.Drawing.Point(20, 80);
            this.groupCaNhan.Name = "groupCaNhan";
            this.groupCaNhan.Size = new System.Drawing.Size(626, 190);
            this.groupCaNhan.TabIndex = 1;
            this.groupCaNhan.TabStop = false;
            this.groupCaNhan.Text = "Thông tin cá nhân";
            // 
            // lblMaNV
            // 
            this.lblMaNV.Location = new System.Drawing.Point(20, 30);
            this.lblMaNV.Name = "lblMaNV";
            this.lblMaNV.Size = new System.Drawing.Size(80, 23);
            this.lblMaNV.TabIndex = 0;
            this.lblMaNV.Text = "Mã NV";
            // 
            // lblHoTen
            // 
            this.lblHoTen.Location = new System.Drawing.Point(20, 60);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(80, 23);
            this.lblHoTen.TabIndex = 1;
            this.lblHoTen.Text = "Họ tên";
            // 
            // lblNgaySinh
            // 
            this.lblNgaySinh.Location = new System.Drawing.Point(20, 90);
            this.lblNgaySinh.Name = "lblNgaySinh";
            this.lblNgaySinh.Size = new System.Drawing.Size(80, 23);
            this.lblNgaySinh.TabIndex = 2;
            this.lblNgaySinh.Text = "Ngày sinh";
            // 
            // lblGioiTinh
            // 
            this.lblGioiTinh.Location = new System.Drawing.Point(20, 120);
            this.lblGioiTinh.Name = "lblGioiTinh";
            this.lblGioiTinh.Size = new System.Drawing.Size(80, 23);
            this.lblGioiTinh.TabIndex = 3;
            this.lblGioiTinh.Text = "Giới tính";
            // 
            // lblSDT
            // 
            this.lblSDT.Location = new System.Drawing.Point(314, 27);
            this.lblSDT.Name = "lblSDT";
            this.lblSDT.Size = new System.Drawing.Size(54, 23);
            this.lblSDT.TabIndex = 4;
            this.lblSDT.Text = "SĐT";
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(314, 59);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(54, 23);
            this.lblEmail.TabIndex = 5;
            this.lblEmail.Text = "Email";
            // 
            // txtMaNV
            // 
            this.txtMaNV.Location = new System.Drawing.Point(100, 27);
            this.txtMaNV.Name = "txtMaNV";
            this.txtMaNV.ReadOnly = true;
            this.txtMaNV.Size = new System.Drawing.Size(193, 27);
            this.txtMaNV.TabIndex = 6;
            // 
            // txtHoTen
            // 
            this.txtHoTen.Location = new System.Drawing.Point(100, 57);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.ReadOnly = true;
            this.txtHoTen.Size = new System.Drawing.Size(193, 27);
            this.txtHoTen.TabIndex = 7;
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.Enabled = false;
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgaySinh.Location = new System.Drawing.Point(100, 87);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(193, 27);
            this.dtpNgaySinh.TabIndex = 8;
            // 
            // txtGioiTinh
            // 
            this.txtGioiTinh.Location = new System.Drawing.Point(100, 117);
            this.txtGioiTinh.Name = "txtGioiTinh";
            this.txtGioiTinh.ReadOnly = true;
            this.txtGioiTinh.Size = new System.Drawing.Size(100, 27);
            this.txtGioiTinh.TabIndex = 9;
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(374, 24);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.ReadOnly = true;
            this.txtSDT.Size = new System.Drawing.Size(187, 27);
            this.txtSDT.TabIndex = 10;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(374, 56);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.ReadOnly = true;
            this.txtEmail.Size = new System.Drawing.Size(187, 27);
            this.txtEmail.TabIndex = 11;
            // 
            // groupCongViec
            // 
            this.groupCongViec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCongViec.Controls.Add(this.lblPhongBan);
            this.groupCongViec.Controls.Add(this.lblChucVu);
            this.groupCongViec.Controls.Add(this.lblNgayVaoLam);
            this.groupCongViec.Controls.Add(this.lblTrangThaiLV);
            this.groupCongViec.Controls.Add(this.txtPhongBan);
            this.groupCongViec.Controls.Add(this.txtChucVu);
            this.groupCongViec.Controls.Add(this.dtpNgayVaoLam);
            this.groupCongViec.Controls.Add(this.txtTrangThaiLamViec);
            this.groupCongViec.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupCongViec.Location = new System.Drawing.Point(703, 80);
            this.groupCongViec.Name = "groupCongViec";
            this.groupCongViec.Size = new System.Drawing.Size(460, 190);
            this.groupCongViec.TabIndex = 2;
            this.groupCongViec.TabStop = false;
            this.groupCongViec.Text = "Thông tin công việc";
            // 
            // lblPhongBan
            // 
            this.lblPhongBan.Location = new System.Drawing.Point(20, 40);
            this.lblPhongBan.Name = "lblPhongBan";
            this.lblPhongBan.Size = new System.Drawing.Size(100, 23);
            this.lblPhongBan.TabIndex = 0;
            this.lblPhongBan.Text = "Phòng ban";
            // 
            // lblChucVu
            // 
            this.lblChucVu.Location = new System.Drawing.Point(20, 70);
            this.lblChucVu.Name = "lblChucVu";
            this.lblChucVu.Size = new System.Drawing.Size(100, 23);
            this.lblChucVu.TabIndex = 1;
            this.lblChucVu.Text = "Chức vụ";
            // 
            // lblNgayVaoLam
            // 
            this.lblNgayVaoLam.Location = new System.Drawing.Point(20, 100);
            this.lblNgayVaoLam.Name = "lblNgayVaoLam";
            this.lblNgayVaoLam.Size = new System.Drawing.Size(100, 23);
            this.lblNgayVaoLam.TabIndex = 2;
            this.lblNgayVaoLam.Text = "Ngày vào làm";
            // 
            // lblTrangThaiLV
            // 
            this.lblTrangThaiLV.Location = new System.Drawing.Point(20, 130);
            this.lblTrangThaiLV.Name = "lblTrangThaiLV";
            this.lblTrangThaiLV.Size = new System.Drawing.Size(100, 23);
            this.lblTrangThaiLV.TabIndex = 3;
            this.lblTrangThaiLV.Text = "Trạng thái";
            // 
            // txtPhongBan
            // 
            this.txtPhongBan.Location = new System.Drawing.Point(130, 37);
            this.txtPhongBan.Name = "txtPhongBan";
            this.txtPhongBan.ReadOnly = true;
            this.txtPhongBan.Size = new System.Drawing.Size(200, 27);
            this.txtPhongBan.TabIndex = 4;
            // 
            // txtChucVu
            // 
            this.txtChucVu.Location = new System.Drawing.Point(130, 67);
            this.txtChucVu.Name = "txtChucVu";
            this.txtChucVu.ReadOnly = true;
            this.txtChucVu.Size = new System.Drawing.Size(200, 27);
            this.txtChucVu.TabIndex = 5;
            // 
            // dtpNgayVaoLam
            // 
            this.dtpNgayVaoLam.Enabled = false;
            this.dtpNgayVaoLam.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayVaoLam.Location = new System.Drawing.Point(130, 97);
            this.dtpNgayVaoLam.Name = "dtpNgayVaoLam";
            this.dtpNgayVaoLam.Size = new System.Drawing.Size(200, 27);
            this.dtpNgayVaoLam.TabIndex = 6;
            // 
            // txtTrangThaiLamViec
            // 
            this.txtTrangThaiLamViec.Location = new System.Drawing.Point(130, 127);
            this.txtTrangThaiLamViec.Name = "txtTrangThaiLamViec";
            this.txtTrangThaiLamViec.ReadOnly = true;
            this.txtTrangThaiLamViec.Size = new System.Drawing.Size(111, 27);
            this.txtTrangThaiLamViec.TabIndex = 7;
            // 
            // groupHopDong
            // 
            this.groupHopDong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupHopDong.Controls.Add(this.lblNgayBD);
            this.groupHopDong.Controls.Add(this.lblNgayKT);
            this.groupHopDong.Controls.Add(this.lblTrangThaiHD);
            this.groupHopDong.Controls.Add(this.dtpNgayBD);
            this.groupHopDong.Controls.Add(this.dtpNgayKT);
            this.groupHopDong.Controls.Add(this.txtTrangThaiHD);
            this.groupHopDong.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupHopDong.Location = new System.Drawing.Point(20, 280);
            this.groupHopDong.Name = "groupHopDong";
            this.groupHopDong.Size = new System.Drawing.Size(1143, 90);
            this.groupHopDong.TabIndex = 3;
            this.groupHopDong.TabStop = false;
            this.groupHopDong.Text = "Hợp đồng hiện tại";
            // 
            // lblNgayBD
            // 
            this.lblNgayBD.Location = new System.Drawing.Point(20, 33);
            this.lblNgayBD.Name = "lblNgayBD";
            this.lblNgayBD.Size = new System.Drawing.Size(117, 23);
            this.lblNgayBD.TabIndex = 0;
            this.lblNgayBD.Text = "Ngày bắt đầu";
            // 
            // lblNgayKT
            // 
            this.lblNgayKT.Location = new System.Drawing.Point(435, 33);
            this.lblNgayKT.Name = "lblNgayKT";
            this.lblNgayKT.Size = new System.Drawing.Size(111, 23);
            this.lblNgayKT.TabIndex = 1;
            this.lblNgayKT.Text = "Ngày kết thúc";
            // 
            // lblTrangThaiHD
            // 
            this.lblTrangThaiHD.Location = new System.Drawing.Point(812, 33);
            this.lblTrangThaiHD.Name = "lblTrangThaiHD";
            this.lblTrangThaiHD.Size = new System.Drawing.Size(100, 23);
            this.lblTrangThaiHD.TabIndex = 2;
            this.lblTrangThaiHD.Text = "Trạng thái";
            // 
            // dtpNgayBD
            // 
            this.dtpNgayBD.Enabled = false;
            this.dtpNgayBD.Location = new System.Drawing.Point(143, 30);
            this.dtpNgayBD.Name = "dtpNgayBD";
            this.dtpNgayBD.Size = new System.Drawing.Size(225, 27);
            this.dtpNgayBD.TabIndex = 3;
            // 
            // dtpNgayKT
            // 
            this.dtpNgayKT.Enabled = false;
            this.dtpNgayKT.Location = new System.Drawing.Point(552, 31);
            this.dtpNgayKT.Name = "dtpNgayKT";
            this.dtpNgayKT.Size = new System.Drawing.Size(212, 27);
            this.dtpNgayKT.TabIndex = 4;
            // 
            // txtTrangThaiHD
            // 
            this.txtTrangThaiHD.Location = new System.Drawing.Point(918, 31);
            this.txtTrangThaiHD.Name = "txtTrangThaiHD";
            this.txtTrangThaiHD.ReadOnly = true;
            this.txtTrangThaiHD.Size = new System.Drawing.Size(109, 27);
            this.txtTrangThaiHD.TabIndex = 5;
            // 
            // groupLichSuHD
            // 
            this.groupLichSuHD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLichSuHD.Controls.Add(this.dgvHopDong);
            this.groupLichSuHD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupLichSuHD.Location = new System.Drawing.Point(20, 380);
            this.groupLichSuHD.Name = "groupLichSuHD";
            this.groupLichSuHD.Size = new System.Drawing.Size(1143, 244);
            this.groupLichSuHD.TabIndex = 4;
            this.groupLichSuHD.TabStop = false;
            this.groupLichSuHD.Text = "Lịch sử hợp đồng";
            // 
            // dgvHopDong
            // 
            this.dgvHopDong.AllowUserToAddRows = false;
            this.dgvHopDong.AllowUserToDeleteRows = false;
            this.dgvHopDong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHopDong.ColumnHeadersHeight = 29;
            this.dgvHopDong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHopDong.Location = new System.Drawing.Point(3, 23);
            this.dgvHopDong.Name = "dgvHopDong";
            this.dgvHopDong.ReadOnly = true;
            this.dgvHopDong.RowHeadersWidth = 51;
            this.dgvHopDong.Size = new System.Drawing.Size(1137, 218);
            this.dgvHopDong.TabIndex = 0;
            // 
            // btnDong
            // 
            this.btnDong.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDong.Location = new System.Drawing.Point(561, 634);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(100, 32);
            this.btnDong.TabIndex = 5;
            this.btnDong.Text = "Đóng";
            // 
            // F_BaoCaoNhanVienChiTiet
            // 
            this.ClientSize = new System.Drawing.Size(1203, 684);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.groupCaNhan);
            this.Controls.Add(this.groupCongViec);
            this.Controls.Add(this.groupHopDong);
            this.Controls.Add(this.groupLichSuHD);
            this.Controls.Add(this.btnDong);
            this.MinimumSize = new System.Drawing.Size(1000, 640);
            this.Name = "F_BaoCaoNhanVienChiTiet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "git";
            this.Load += new System.EventHandler(this.F_BaoCaoNhanVienChiTiet_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.groupCaNhan.ResumeLayout(false);
            this.groupCaNhan.PerformLayout();
            this.groupCongViec.ResumeLayout(false);
            this.groupCongViec.PerformLayout();
            this.groupHopDong.ResumeLayout(false);
            this.groupHopDong.PerformLayout();
            this.groupLichSuHD.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHopDong)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupCaNhan, groupCongViec, groupHopDong, groupLichSuHD;
        private System.Windows.Forms.Label lblMaNV, lblHoTen, lblNgaySinh, lblGioiTinh, lblSDT, lblEmail;
        private System.Windows.Forms.Label lblPhongBan, lblChucVu, lblNgayVaoLam, lblTrangThaiLV;
        private System.Windows.Forms.Label lblNgayBD, lblNgayKT, lblTrangThaiHD;
        private System.Windows.Forms.TextBox txtMaNV, txtHoTen, txtGioiTinh, txtSDT, txtEmail;
        private System.Windows.Forms.TextBox txtPhongBan, txtChucVu, txtTrangThaiLamViec, txtTrangThaiHD;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh, dtpNgayVaoLam, dtpNgayBD, dtpNgayKT;
        private System.Windows.Forms.DataGridView dgvHopDong;
        private System.Windows.Forms.Button btnDong;
    }
}
