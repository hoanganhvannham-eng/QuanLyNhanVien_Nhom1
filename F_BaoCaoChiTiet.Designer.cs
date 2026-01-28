namespace QuanLyNhanVien3
{
    partial class F_BaoCaoChiTiet
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
            this.lblContext = new System.Windows.Forms.Label();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.pnlTong = new System.Windows.Forms.Panel();
            this.lblTongNV = new System.Windows.Forms.Label();
            this.pnlConHan = new System.Windows.Forms.Panel();
            this.lblConHan = new System.Windows.Forms.Label();
            this.pnlSapHet = new System.Windows.Forms.Panel();
            this.lblSapHetHan = new System.Windows.Forms.Label();
            this.pnlHetHan = new System.Windows.Forms.Panel();
            this.lblHetHan = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.dgvChiTiet = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnDong = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.pnlTong.SuspendLayout();
            this.pnlConHan.SuspendLayout();
            this.pnlSapHet.SuspendLayout();
            this.pnlHetHan.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblContext);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(20, 12, 20, 12);
            this.panelHeader.Size = new System.Drawing.Size(1196, 90);
            this.panelHeader.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(252, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BÁO CÁO CHI TIẾT";
            // 
            // lblContext
            // 
            this.lblContext.AutoSize = true;
            this.lblContext.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblContext.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblContext.Location = new System.Drawing.Point(22, 52);
            this.lblContext.Name = "lblContext";
            this.lblContext.Size = new System.Drawing.Size(85, 23);
            this.lblContext.TabIndex = 1;
            this.lblContext.Text = "Ngữ cảnh";
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.White;
            this.panelSummary.Controls.Add(this.pnlTong);
            this.panelSummary.Controls.Add(this.pnlConHan);
            this.panelSummary.Controls.Add(this.pnlSapHet);
            this.panelSummary.Controls.Add(this.pnlHetHan);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 90);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(16);
            this.panelSummary.Size = new System.Drawing.Size(1196, 80);
            this.panelSummary.TabIndex = 2;
            // 
            // pnlTong
            // 
            this.pnlTong.BackColor = System.Drawing.Color.LightGray;
            this.pnlTong.Controls.Add(this.lblTongNV);
            this.pnlTong.Location = new System.Drawing.Point(16, 16);
            this.pnlTong.Name = "pnlTong";
            this.pnlTong.Size = new System.Drawing.Size(200, 48);
            this.pnlTong.TabIndex = 0;
            // 
            // lblTongNV
            // 
            this.lblTongNV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTongNV.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTongNV.Location = new System.Drawing.Point(0, 0);
            this.lblTongNV.Name = "lblTongNV";
            this.lblTongNV.Size = new System.Drawing.Size(200, 48);
            this.lblTongNV.TabIndex = 0;
            this.lblTongNV.Text = "Tổng NV: 0";
            this.lblTongNV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlConHan
            // 
            this.pnlConHan.BackColor = System.Drawing.Color.Honeydew;
            this.pnlConHan.Controls.Add(this.lblConHan);
            this.pnlConHan.Location = new System.Drawing.Point(232, 16);
            this.pnlConHan.Name = "pnlConHan";
            this.pnlConHan.Size = new System.Drawing.Size(200, 48);
            this.pnlConHan.TabIndex = 1;
            // 
            // lblConHan
            // 
            this.lblConHan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblConHan.ForeColor = System.Drawing.Color.Green;
            this.lblConHan.Location = new System.Drawing.Point(0, 0);
            this.lblConHan.Name = "lblConHan";
            this.lblConHan.Size = new System.Drawing.Size(200, 48);
            this.lblConHan.TabIndex = 0;
            this.lblConHan.Text = "Còn hạn: 0";
            this.lblConHan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlSapHet
            // 
            this.pnlSapHet.BackColor = System.Drawing.Color.LemonChiffon;
            this.pnlSapHet.Controls.Add(this.lblSapHetHan);
            this.pnlSapHet.Location = new System.Drawing.Point(448, 16);
            this.pnlSapHet.Name = "pnlSapHet";
            this.pnlSapHet.Size = new System.Drawing.Size(200, 48);
            this.pnlSapHet.TabIndex = 2;
            // 
            // lblSapHetHan
            // 
            this.lblSapHetHan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSapHetHan.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblSapHetHan.Location = new System.Drawing.Point(0, 0);
            this.lblSapHetHan.Name = "lblSapHetHan";
            this.lblSapHetHan.Size = new System.Drawing.Size(200, 48);
            this.lblSapHetHan.TabIndex = 0;
            this.lblSapHetHan.Text = "Sắp hết hạn: 0";
            this.lblSapHetHan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlHetHan
            // 
            this.pnlHetHan.BackColor = System.Drawing.Color.MistyRose;
            this.pnlHetHan.Controls.Add(this.lblHetHan);
            this.pnlHetHan.Location = new System.Drawing.Point(664, 16);
            this.pnlHetHan.Name = "pnlHetHan";
            this.pnlHetHan.Size = new System.Drawing.Size(200, 48);
            this.pnlHetHan.TabIndex = 3;
            // 
            // lblHetHan
            // 
            this.lblHetHan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHetHan.ForeColor = System.Drawing.Color.Red;
            this.lblHetHan.Location = new System.Drawing.Point(0, 0);
            this.lblHetHan.Name = "lblHetHan";
            this.lblHetHan.Size = new System.Drawing.Size(200, 48);
            this.lblHetHan.TabIndex = 0;
            this.lblHetHan.Text = "Hết hạn: 0";
            this.lblHetHan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelContent.Controls.Add(this.dgvChiTiet);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 170);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(16);
            this.panelContent.Size = new System.Drawing.Size(1196, 430);
            this.panelContent.TabIndex = 0;
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.BackgroundColor = System.Drawing.Color.White;
            this.dgvChiTiet.ColumnHeadersHeight = 29;
            this.dgvChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChiTiet.Location = new System.Drawing.Point(16, 16);
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.RowHeadersWidth = 51;
            this.dgvChiTiet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChiTiet.Size = new System.Drawing.Size(1164, 398);
            this.dgvChiTiet.TabIndex = 0;
            this.dgvChiTiet.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTiet_CellDoubleClick);
            this.dgvChiTiet.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvChiTiet_RowPrePaint);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.White;
            this.panelBottom.Controls.Add(this.btnDong);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 600);
            this.panelBottom.MinimumSize = new System.Drawing.Size(0, 60);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(16);
            this.panelBottom.Size = new System.Drawing.Size(1196, 60);
            this.panelBottom.TabIndex = 1;
            // 
            // btnDong
            // 
            this.btnDong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDong.Location = new System.Drawing.Point(1048, 12);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(140, 36);
            this.btnDong.TabIndex = 0;
            this.btnDong.Text = "Đóng";
            this.btnDong.UseVisualStyleBackColor = true;
            this.btnDong.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // F_BaoCaoChiTiet
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1200, 660);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelHeader);
            this.Name = "F_BaoCaoChiTiet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Báo cáo chi tiết";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_BaoCaoChiTiet_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.pnlTong.ResumeLayout(false);
            this.pnlConHan.ResumeLayout(false);
            this.pnlSapHet.ResumeLayout(false);
            this.pnlHetHan.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblContext;

        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Panel pnlTong;
        private System.Windows.Forms.Panel pnlConHan;
        private System.Windows.Forms.Panel pnlSapHet;
        private System.Windows.Forms.Panel pnlHetHan;

        private System.Windows.Forms.Label lblTongNV;
        private System.Windows.Forms.Label lblConHan;
        private System.Windows.Forms.Label lblSapHetHan;
        private System.Windows.Forms.Label lblHetHan;

        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.DataGridView dgvChiTiet;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnDong;

    }
}
