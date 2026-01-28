namespace QuanLyNhanVien3
{
    partial class F_BaoCaoLuong
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
            this.lblSubTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnTongLuongPhongBan = new System.Windows.Forms.Button();
            this.btnNVTongLuongCaoNhat = new System.Windows.Forms.Button();
            this.btnLuongHangThang = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.gbContent = new System.Windows.Forms.GroupBox();
            this.dtGridViewBCLuong = new System.Windows.Forms.DataGridView();
            this.panelAction = new System.Windows.Forms.Panel();
            this.btnXuatPDF = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.gbContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCLuong)).BeginInit();
            this.panelAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelHeader.Controls.Add(this.lblSubTitle);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(18, 10, 18, 10);
            this.panelHeader.Size = new System.Drawing.Size(1315, 78);
            this.panelHeader.TabIndex = 2;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubTitle.Location = new System.Drawing.Point(20, 48);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(199, 23);
            this.lblSubTitle.TabIndex = 0;
            this.lblSubTitle.Text = "Theo tháng / phòng ban";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(397, 37);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "BÁO CÁO LƯƠNG NHÂN VIÊN";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.gbFilter);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 78);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12);
            this.panelTop.Size = new System.Drawing.Size(1315, 140);
            this.panelTop.TabIndex = 1;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnTongLuongPhongBan);
            this.gbFilter.Controls.Add(this.btnNVTongLuongCaoNhat);
            this.gbFilter.Controls.Add(this.btnLuongHangThang);
            this.gbFilter.Controls.Add(this.dateTimePicker1);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFilter.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbFilter.Location = new System.Drawing.Point(12, 12);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Padding = new System.Windows.Forms.Padding(12);
            this.gbFilter.Size = new System.Drawing.Size(1291, 116);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Chọn loại báo cáo";
            // 
            // btnTongLuongPhongBan
            // 
            this.btnTongLuongPhongBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTongLuongPhongBan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTongLuongPhongBan.Location = new System.Drawing.Point(1040, 30);
            this.btnTongLuongPhongBan.Name = "btnTongLuongPhongBan";
            this.btnTongLuongPhongBan.Size = new System.Drawing.Size(240, 40);
            this.btnTongLuongPhongBan.TabIndex = 0;
            this.btnTongLuongPhongBan.Text = "Tổng lương theo phòng ban";
            this.btnTongLuongPhongBan.Click += new System.EventHandler(this.btnTongLuongPhongBan_Click);
            // 
            // btnNVTongLuongCaoNhat
            // 
            this.btnNVTongLuongCaoNhat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNVTongLuongCaoNhat.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnNVTongLuongCaoNhat.Location = new System.Drawing.Point(600, 30);
            this.btnNVTongLuongCaoNhat.Name = "btnNVTongLuongCaoNhat";
            this.btnNVTongLuongCaoNhat.Size = new System.Drawing.Size(420, 40);
            this.btnNVTongLuongCaoNhat.TabIndex = 1;
            this.btnNVTongLuongCaoNhat.Text = "Nhân viên có tổng lương cao nhất";
            this.btnNVTongLuongCaoNhat.Click += new System.EventHandler(this.btnNVTongLuongCaoNhat_Click);
            // 
            // btnLuongHangThang
            // 
            this.btnLuongHangThang.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuongHangThang.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnLuongHangThang.Location = new System.Drawing.Point(320, 30);
            this.btnLuongHangThang.Name = "btnLuongHangThang";
            this.btnLuongHangThang.Size = new System.Drawing.Size(260, 40);
            this.btnLuongHangThang.TabIndex = 2;
            this.btnLuongHangThang.Text = "Bảng lương hàng tháng";
            this.btnLuongHangThang.Click += new System.EventHandler(this.btnLuongHangThang_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 32);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(280, 30);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // gbContent
            // 
            this.gbContent.Controls.Add(this.dtGridViewBCLuong);
            this.gbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbContent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbContent.Location = new System.Drawing.Point(0, 218);
            this.gbContent.Name = "gbContent";
            this.gbContent.Padding = new System.Windows.Forms.Padding(10);
            this.gbContent.Size = new System.Drawing.Size(1315, 442);
            this.gbContent.TabIndex = 0;
            this.gbContent.TabStop = false;
            this.gbContent.Text = "Bảng dữ liệu";
            // 
            // dtGridViewBCLuong
            // 
            this.dtGridViewBCLuong.AllowUserToAddRows = false;
            this.dtGridViewBCLuong.AllowUserToDeleteRows = false;
            this.dtGridViewBCLuong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCLuong.ColumnHeadersHeight = 32;
            this.dtGridViewBCLuong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCLuong.Location = new System.Drawing.Point(10, 33);
            this.dtGridViewBCLuong.Name = "dtGridViewBCLuong";
            this.dtGridViewBCLuong.ReadOnly = true;
            this.dtGridViewBCLuong.RowHeadersVisible = false;
            this.dtGridViewBCLuong.RowHeadersWidth = 51;
            this.dtGridViewBCLuong.RowTemplate.Height = 28;
            this.dtGridViewBCLuong.Size = new System.Drawing.Size(1295, 399);
            this.dtGridViewBCLuong.TabIndex = 0;
            // 
            // panelAction
            // 
            this.panelAction.Controls.Add(this.btnXuatPDF);
            this.panelAction.Controls.Add(this.btnXuatExcel);
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAction.Location = new System.Drawing.Point(0, 660);
            this.panelAction.Name = "panelAction";
            this.panelAction.Padding = new System.Windows.Forms.Padding(12);
            this.panelAction.Size = new System.Drawing.Size(1315, 60);
            this.panelAction.TabIndex = 3;
            // 
            // btnXuatPDF
            // 
            this.btnXuatPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXuatPDF.Location = new System.Drawing.Point(168, 12);
            this.btnXuatPDF.Name = "btnXuatPDF";
            this.btnXuatPDF.Size = new System.Drawing.Size(140, 36);
            this.btnXuatPDF.TabIndex = 0;
            this.btnXuatPDF.Text = "Xuất PDF";
            this.btnXuatPDF.Click += new System.EventHandler(this.btnXuatPDF_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXuatExcel.Location = new System.Drawing.Point(16, 12);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(140, 36);
            this.btnXuatExcel.TabIndex = 1;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // F_BaoCaoLuong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1315, 720);
            this.Controls.Add(this.gbContent);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelAction);
            this.Name = "F_BaoCaoLuong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "git";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_BaoCaoLuong_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCLuong)).EndInit();
            this.panelAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnLuongHangThang;
        private System.Windows.Forms.Button btnNVTongLuongCaoNhat;
        private System.Windows.Forms.Button btnTongLuongPhongBan;

        private System.Windows.Forms.GroupBox gbContent;
        private System.Windows.Forms.DataGridView dtGridViewBCLuong;

        private System.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnXuatPDF;
    }
}
