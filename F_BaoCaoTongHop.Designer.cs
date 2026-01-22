namespace QuanLyNhanVien3
{
    partial class F_BaoCaoTongHop
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
            this.panelSummary = new System.Windows.Forms.Panel();
            this.cardDA = new System.Windows.Forms.Panel();
            this.lblDAText = new System.Windows.Forms.Label();
            this.lblDAValue = new System.Windows.Forms.Label();
            this.cardCV = new System.Windows.Forms.Panel();
            this.lblCVText = new System.Windows.Forms.Label();
            this.lblCVValue = new System.Windows.Forms.Label();
            this.cardPB = new System.Windows.Forms.Panel();
            this.lblPBText = new System.Windows.Forms.Label();
            this.lblPBValue = new System.Windows.Forms.Label();
            this.cardNV = new System.Windows.Forms.Panel();
            this.lblNVText = new System.Windows.Forms.Label();
            this.lblNVValue = new System.Windows.Forms.Label();
            this.gbReportType = new System.Windows.Forms.GroupBox();
            this.btnTheoDuAn = new System.Windows.Forms.Button();
            this.btnTheoChucVu = new System.Windows.Forms.Button();
            this.btnTheoPhongBan = new System.Windows.Forms.Button();
            this.gbContent = new System.Windows.Forms.GroupBox();
            this.dtGridViewBCTongHop = new System.Windows.Forms.DataGridView();
            this.lblGridTitle = new System.Windows.Forms.Label();
            this.panelAction = new System.Windows.Forms.Panel();
            this.btnDong = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnXuatPDF = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.cardDA.SuspendLayout();
            this.cardCV.SuspendLayout();
            this.cardPB.SuspendLayout();
            this.cardNV.SuspendLayout();
            this.gbReportType.SuspendLayout();
            this.gbContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCTongHop)).BeginInit();
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
            this.panelHeader.Size = new System.Drawing.Size(1305, 78);
            this.panelHeader.TabIndex = 0;
            // 
            // lblSubTitle
            // 
            this.lblSubTitle.AutoSize = true;
            this.lblSubTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubTitle.Location = new System.Drawing.Point(20, 48);
            this.lblSubTitle.Name = "lblSubTitle";
            this.lblSubTitle.Size = new System.Drawing.Size(93, 23);
            this.lblSubTitle.TabIndex = 1;
            this.lblSubTitle.Text = "Tổng quan";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(417, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BÁO CÁO TỔNG HỢP NHÂN SỰ";
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.panelSummary);
            this.panelTop.Controls.Add(this.gbReportType);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 78);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(12);
            this.panelTop.Size = new System.Drawing.Size(1305, 210);
            this.panelTop.TabIndex = 1;
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.cardDA);
            this.panelSummary.Controls.Add(this.cardCV);
            this.panelSummary.Controls.Add(this.cardPB);
            this.panelSummary.Controls.Add(this.cardNV);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSummary.Location = new System.Drawing.Point(12, 104);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(6);
            this.panelSummary.Size = new System.Drawing.Size(1281, 94);
            this.panelSummary.TabIndex = 1;
            // 
            // cardDA
            // 
            this.cardDA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.cardDA.Controls.Add(this.lblDAText);
            this.cardDA.Controls.Add(this.lblDAValue);
            this.cardDA.Location = new System.Drawing.Point(972, 12);
            this.cardDA.Name = "cardDA";
            this.cardDA.Size = new System.Drawing.Size(290, 70);
            this.cardDA.TabIndex = 3;
            // 
            // lblDAText
            // 
            this.lblDAText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDAText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDAText.Location = new System.Drawing.Point(0, 40);
            this.lblDAText.Name = "lblDAText";
            this.lblDAText.Size = new System.Drawing.Size(290, 30);
            this.lblDAText.TabIndex = 1;
            this.lblDAText.Text = "Số dự án";
            this.lblDAText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDAValue
            // 
            this.lblDAValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDAValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblDAValue.Location = new System.Drawing.Point(0, 0);
            this.lblDAValue.Name = "lblDAValue";
            this.lblDAValue.Size = new System.Drawing.Size(290, 40);
            this.lblDAValue.TabIndex = 0;
            this.lblDAValue.Text = "0";
            this.lblDAValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardCV
            // 
            this.cardCV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.cardCV.Controls.Add(this.lblCVText);
            this.cardCV.Controls.Add(this.lblCVValue);
            this.cardCV.Location = new System.Drawing.Point(654, 12);
            this.cardCV.Name = "cardCV";
            this.cardCV.Size = new System.Drawing.Size(300, 70);
            this.cardCV.TabIndex = 2;
            // 
            // lblCVText
            // 
            this.lblCVText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCVText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCVText.Location = new System.Drawing.Point(0, 40);
            this.lblCVText.Name = "lblCVText";
            this.lblCVText.Size = new System.Drawing.Size(300, 30);
            this.lblCVText.TabIndex = 1;
            this.lblCVText.Text = "Số chức vụ";
            this.lblCVText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCVValue
            // 
            this.lblCVValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCVValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblCVValue.Location = new System.Drawing.Point(0, 0);
            this.lblCVValue.Name = "lblCVValue";
            this.lblCVValue.Size = new System.Drawing.Size(300, 40);
            this.lblCVValue.TabIndex = 0;
            this.lblCVValue.Text = "0";
            this.lblCVValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardPB
            // 
            this.cardPB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.cardPB.Controls.Add(this.lblPBText);
            this.cardPB.Controls.Add(this.lblPBValue);
            this.cardPB.Location = new System.Drawing.Point(336, 12);
            this.cardPB.Name = "cardPB";
            this.cardPB.Size = new System.Drawing.Size(300, 70);
            this.cardPB.TabIndex = 1;
            // 
            // lblPBText
            // 
            this.lblPBText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPBText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPBText.Location = new System.Drawing.Point(0, 40);
            this.lblPBText.Name = "lblPBText";
            this.lblPBText.Size = new System.Drawing.Size(300, 30);
            this.lblPBText.TabIndex = 1;
            this.lblPBText.Text = "Số phòng ban";
            this.lblPBText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPBValue
            // 
            this.lblPBValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPBValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPBValue.Location = new System.Drawing.Point(0, 0);
            this.lblPBValue.Name = "lblPBValue";
            this.lblPBValue.Size = new System.Drawing.Size(300, 40);
            this.lblPBValue.TabIndex = 0;
            this.lblPBValue.Text = "0";
            this.lblPBValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardNV
            // 
            this.cardNV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.cardNV.Controls.Add(this.lblNVText);
            this.cardNV.Controls.Add(this.lblNVValue);
            this.cardNV.Location = new System.Drawing.Point(18, 12);
            this.cardNV.Name = "cardNV";
            this.cardNV.Size = new System.Drawing.Size(300, 70);
            this.cardNV.TabIndex = 0;
            // 
            // lblNVText
            // 
            this.lblNVText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNVText.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNVText.Location = new System.Drawing.Point(0, 40);
            this.lblNVText.Name = "lblNVText";
            this.lblNVText.Size = new System.Drawing.Size(300, 30);
            this.lblNVText.TabIndex = 1;
            this.lblNVText.Text = "Tổng nhân viên";
            this.lblNVText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNVValue
            // 
            this.lblNVValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNVValue.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblNVValue.Location = new System.Drawing.Point(0, 0);
            this.lblNVValue.Name = "lblNVValue";
            this.lblNVValue.Size = new System.Drawing.Size(300, 40);
            this.lblNVValue.TabIndex = 0;
            this.lblNVValue.Text = "0";
            this.lblNVValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbReportType
            // 
            this.gbReportType.Controls.Add(this.btnTheoDuAn);
            this.gbReportType.Controls.Add(this.btnTheoChucVu);
            this.gbReportType.Controls.Add(this.btnTheoPhongBan);
            this.gbReportType.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbReportType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbReportType.Location = new System.Drawing.Point(12, 12);
            this.gbReportType.Name = "gbReportType";
            this.gbReportType.Padding = new System.Windows.Forms.Padding(12);
            this.gbReportType.Size = new System.Drawing.Size(1281, 92);
            this.gbReportType.TabIndex = 0;
            this.gbReportType.TabStop = false;
            this.gbReportType.Text = "Chọn loại báo cáo";
            // 
            // btnTheoDuAn
            // 
            this.btnTheoDuAn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheoDuAn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTheoDuAn.Location = new System.Drawing.Point(822, 32);
            this.btnTheoDuAn.Name = "btnTheoDuAn";
            this.btnTheoDuAn.Size = new System.Drawing.Size(440, 44);
            this.btnTheoDuAn.TabIndex = 2;
            this.btnTheoDuAn.Text = "Tổng hợp theo Dự án";
            this.btnTheoDuAn.UseVisualStyleBackColor = true;
            // 
            // btnTheoChucVu
            // 
            this.btnTheoChucVu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheoChucVu.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTheoChucVu.Location = new System.Drawing.Point(420, 32);
            this.btnTheoChucVu.Name = "btnTheoChucVu";
            this.btnTheoChucVu.Size = new System.Drawing.Size(380, 44);
            this.btnTheoChucVu.TabIndex = 1;
            this.btnTheoChucVu.Text = "Tổng hợp theo Chức vụ";
            this.btnTheoChucVu.UseVisualStyleBackColor = true;
            // 
            // btnTheoPhongBan
            // 
            this.btnTheoPhongBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheoPhongBan.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnTheoPhongBan.Location = new System.Drawing.Point(18, 32);
            this.btnTheoPhongBan.Name = "btnTheoPhongBan";
            this.btnTheoPhongBan.Size = new System.Drawing.Size(380, 44);
            this.btnTheoPhongBan.TabIndex = 0;
            this.btnTheoPhongBan.Text = "Tổng hợp theo Phòng ban";
            this.btnTheoPhongBan.UseVisualStyleBackColor = true;
            // 
            // gbContent
            // 
            this.gbContent.Controls.Add(this.dtGridViewBCTongHop);
            this.gbContent.Controls.Add(this.lblGridTitle);
            this.gbContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbContent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.gbContent.Location = new System.Drawing.Point(0, 288);
            this.gbContent.Name = "gbContent";
            this.gbContent.Padding = new System.Windows.Forms.Padding(10);
            this.gbContent.Size = new System.Drawing.Size(1305, 372);
            this.gbContent.TabIndex = 2;
            this.gbContent.TabStop = false;
            this.gbContent.Text = "Bảng số liệu";
            // 
            // dtGridViewBCTongHop
            // 
            this.dtGridViewBCTongHop.AllowUserToAddRows = false;
            this.dtGridViewBCTongHop.AllowUserToDeleteRows = false;
            this.dtGridViewBCTongHop.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCTongHop.ColumnHeadersHeight = 32;
            this.dtGridViewBCTongHop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCTongHop.Location = new System.Drawing.Point(10, 63);
            this.dtGridViewBCTongHop.MultiSelect = false;
            this.dtGridViewBCTongHop.Name = "dtGridViewBCTongHop";
            this.dtGridViewBCTongHop.ReadOnly = true;
            this.dtGridViewBCTongHop.RowHeadersVisible = false;
            this.dtGridViewBCTongHop.RowHeadersWidth = 51;
            this.dtGridViewBCTongHop.RowTemplate.Height = 28;
            this.dtGridViewBCTongHop.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dtGridViewBCTongHop.Size = new System.Drawing.Size(1285, 299);
            this.dtGridViewBCTongHop.TabIndex = 1;
            this.dtGridViewBCTongHop.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridViewBCTongHop_CellDoubleClick);
            this.dtGridViewBCTongHop.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtGridViewBCTongHop_CellDoubleClick);
            // 
            // lblGridTitle
            // 
            this.lblGridTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGridTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblGridTitle.Location = new System.Drawing.Point(10, 33);
            this.lblGridTitle.Name = "lblGridTitle";
            this.lblGridTitle.Size = new System.Drawing.Size(1285, 30);
            this.lblGridTitle.TabIndex = 0;
            this.lblGridTitle.Text = "Chi tiết số liệu tổng hợp";
            this.lblGridTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelAction
            // 
            this.panelAction.BackColor = System.Drawing.Color.White;
            this.panelAction.Controls.Add(this.btnDong);
            this.panelAction.Controls.Add(this.btnLamMoi);
            this.panelAction.Controls.Add(this.btnIn);
            this.panelAction.Controls.Add(this.btnXuatPDF);
            this.panelAction.Controls.Add(this.btnXuatExcel);
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAction.Location = new System.Drawing.Point(0, 660);
            this.panelAction.Name = "panelAction";
            this.panelAction.Padding = new System.Windows.Forms.Padding(12);
            this.panelAction.Size = new System.Drawing.Size(1305, 60);
            this.panelAction.TabIndex = 3;
            // 
            // btnDong
            // 
            this.btnDong.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnDong.Location = new System.Drawing.Point(1148, 12);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(140, 36);
            this.btnDong.TabIndex = 4;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLamMoi.Location = new System.Drawing.Point(472, 12);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(140, 36);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            // 
            // btnIn
            // 
            this.btnIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnIn.Location = new System.Drawing.Point(320, 12);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(140, 36);
            this.btnIn.TabIndex = 2;
            this.btnIn.Text = "In báo cáo";
            this.btnIn.UseVisualStyleBackColor = true;
            // 
            // btnXuatPDF
            // 
            this.btnXuatPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatPDF.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXuatPDF.Location = new System.Drawing.Point(168, 12);
            this.btnXuatPDF.Name = "btnXuatPDF";
            this.btnXuatPDF.Size = new System.Drawing.Size(140, 36);
            this.btnXuatPDF.TabIndex = 1;
            this.btnXuatPDF.Text = "Xuất PDF";
            this.btnXuatPDF.UseVisualStyleBackColor = true;
            this.btnXuatPDF.Click += new System.EventHandler(this.btnXuatPDF_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatExcel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnXuatExcel.Location = new System.Drawing.Point(16, 12);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(140, 36);
            this.btnXuatExcel.TabIndex = 0;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // F_BaoCaoTongHop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1305, 720);
            this.Controls.Add(this.gbContent);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelAction);
            this.Name = "F_BaoCaoTongHop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo cáo tổng hợp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_BaoCaoTongHop_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.cardDA.ResumeLayout(false);
            this.cardCV.ResumeLayout(false);
            this.cardPB.ResumeLayout(false);
            this.cardNV.ResumeLayout(false);
            this.gbReportType.ResumeLayout(false);
            this.gbContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCTongHop)).EndInit();
            this.panelAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox gbReportType;
        private System.Windows.Forms.Button btnTheoPhongBan;
        private System.Windows.Forms.Button btnTheoChucVu;
        private System.Windows.Forms.Button btnTheoDuAn;

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Panel cardNV;
        private System.Windows.Forms.Label lblNVValue;
        private System.Windows.Forms.Label lblNVText;

        private System.Windows.Forms.Panel cardPB;
        private System.Windows.Forms.Label lblPBValue;
        private System.Windows.Forms.Label lblPBText;

        private System.Windows.Forms.Panel cardCV;
        private System.Windows.Forms.Label lblCVValue;
        private System.Windows.Forms.Label lblCVText;

        private System.Windows.Forms.Panel cardDA;
        private System.Windows.Forms.Label lblDAValue;
        private System.Windows.Forms.Label lblDAText;

        private System.Windows.Forms.GroupBox gbContent;
        private System.Windows.Forms.Label lblGridTitle;
        private System.Windows.Forms.DataGridView dtGridViewBCTongHop;

        private System.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnXuatPDF;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnDong;
    }
}
