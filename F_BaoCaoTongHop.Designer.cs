namespace QuanLyNhanVien3
{
    partial class F_BaoCaoTongHop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnNhanVienNhieuDuAn = new System.Windows.Forms.Button();
            this.btnTongHopChung = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtGridViewBCTongHop = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCTongHop)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1402, 122);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(502, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(357, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "Báo Cáo Tổng Hợp";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnXuatExcel);
            this.panel2.Controls.Add(this.btnNhanVienNhieuDuAn);
            this.panel2.Controls.Add(this.btnTongHopChung);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 122);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1402, 238);
            this.panel2.TabIndex = 4;
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.Location = new System.Drawing.Point(994, 88);
            this.btnXuatExcel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(92, 65);
            this.btnXuatExcel.TabIndex = 2;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // btnNhanVienNhieuDuAn
            // 
            this.btnNhanVienNhieuDuAn.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNhanVienNhieuDuAn.Location = new System.Drawing.Point(615, 88);
            this.btnNhanVienNhieuDuAn.Name = "btnNhanVienNhieuDuAn";
            this.btnNhanVienNhieuDuAn.Size = new System.Drawing.Size(312, 65);
            this.btnNhanVienNhieuDuAn.TabIndex = 1;
            this.btnNhanVienNhieuDuAn.Text = "Nhân viên có nhiều dự án nhất";
            this.btnNhanVienNhieuDuAn.UseVisualStyleBackColor = true;
            this.btnNhanVienNhieuDuAn.Click += new System.EventHandler(this.btnNhanVienNhieuDuAn_Click);
            // 
            // btnTongHopChung
            // 
            this.btnTongHopChung.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTongHopChung.Location = new System.Drawing.Point(218, 88);
            this.btnTongHopChung.Name = "btnTongHopChung";
            this.btnTongHopChung.Size = new System.Drawing.Size(330, 65);
            this.btnTongHopChung.TabIndex = 0;
            this.btnTongHopChung.Text = "Tổng số nhân viên, tổng số phòng ban, tổng số dự án";
            this.btnTongHopChung.UseVisualStyleBackColor = true;
            this.btnTongHopChung.Click += new System.EventHandler(this.btnTongHopChung_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtGridViewBCTongHop);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 360);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1402, 462);
            this.panel3.TabIndex = 5;
            // 
            // dtGridViewBCTongHop
            // 
            this.dtGridViewBCTongHop.AllowUserToAddRows = false;
            this.dtGridViewBCTongHop.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCTongHop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewBCTongHop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCTongHop.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewBCTongHop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtGridViewBCTongHop.Name = "dtGridViewBCTongHop";
            this.dtGridViewBCTongHop.ReadOnly = true;
            this.dtGridViewBCTongHop.RowHeadersWidth = 51;
            this.dtGridViewBCTongHop.RowTemplate.Height = 24;
            this.dtGridViewBCTongHop.Size = new System.Drawing.Size(1402, 462);
            this.dtGridViewBCTongHop.TabIndex = 122;
            // 
            // F_BaoCaoTongHop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1402, 822);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "F_BaoCaoTongHop";
            this.Text = "git g";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.F_BaoCaoTongHop_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCTongHop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnNhanVienNhieuDuAn;
        private System.Windows.Forms.Button btnTongHopChung;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dtGridViewBCTongHop;
        private System.Windows.Forms.Button btnXuatExcel;
    }
}