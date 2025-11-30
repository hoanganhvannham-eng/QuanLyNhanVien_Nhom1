namespace QuanLyNhanVien3
{
    partial class F_BaoCaoDuAn
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.btnSoLuongNhanVien = new System.Windows.Forms.Button();
            this.btnDSNhanVienTheoDuAn = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtGridViewBCDuAn = new System.Windows.Forms.DataGridView();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCDuAn)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1536, 152);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(544, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 60);
            this.label1.TabIndex = 0;
            this.label1.Text = "Báo Cáo Dự Án";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnXuatExcel);
            this.panel2.Controls.Add(this.btnTimKiem);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtTimKiem);
            this.panel2.Controls.Add(this.btnSoLuongNhanVien);
            this.panel2.Controls.Add(this.btnDSNhanVienTheoDuAn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 152);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1536, 245);
            this.panel2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(490, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tên Dự Án";
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(629, 30);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.Size = new System.Drawing.Size(263, 26);
            this.txtTimKiem.TabIndex = 2;
            // 
            // btnSoLuongNhanVien
            // 
            this.btnSoLuongNhanVien.Location = new System.Drawing.Point(681, 109);
            this.btnSoLuongNhanVien.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSoLuongNhanVien.Name = "btnSoLuongNhanVien";
            this.btnSoLuongNhanVien.Size = new System.Drawing.Size(253, 49);
            this.btnSoLuongNhanVien.TabIndex = 1;
            this.btnSoLuongNhanVien.Text = "Số lượng nhân viên tham gia mỗi dự án";
            this.btnSoLuongNhanVien.UseVisualStyleBackColor = true;
            this.btnSoLuongNhanVien.Click += new System.EventHandler(this.btnSoLuongNhanVien_Click);
            // 
            // btnDSNhanVienTheoDuAn
            // 
            this.btnDSNhanVienTheoDuAn.Location = new System.Drawing.Point(316, 109);
            this.btnDSNhanVienTheoDuAn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDSNhanVienTheoDuAn.Name = "btnDSNhanVienTheoDuAn";
            this.btnDSNhanVienTheoDuAn.Size = new System.Drawing.Size(253, 49);
            this.btnDSNhanVienTheoDuAn.TabIndex = 0;
            this.btnDSNhanVienTheoDuAn.Text = "Danh sách nhân viên theo dự án";
            this.btnDSNhanVienTheoDuAn.UseVisualStyleBackColor = true;
            this.btnDSNhanVienTheoDuAn.Click += new System.EventHandler(this.btnDSNhanVienTheoDuAn_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtGridViewBCDuAn);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 397);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1536, 635);
            this.panel3.TabIndex = 5;
            // 
            // dtGridViewBCDuAn
            // 
            this.dtGridViewBCDuAn.AllowUserToAddRows = false;
            this.dtGridViewBCDuAn.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCDuAn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewBCDuAn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCDuAn.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewBCDuAn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtGridViewBCDuAn.Name = "dtGridViewBCDuAn";
            this.dtGridViewBCDuAn.ReadOnly = true;
            this.dtGridViewBCDuAn.RowHeadersWidth = 51;
            this.dtGridViewBCDuAn.RowTemplate.Height = 24;
            this.dtGridViewBCDuAn.Size = new System.Drawing.Size(1536, 635);
            this.dtGridViewBCDuAn.TabIndex = 122;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.Location = new System.Drawing.Point(1054, 26);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(122, 30);
            this.btnTimKiem.TabIndex = 4;
            this.btnTimKiem.Text = "Tìm Kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = true;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Location = new System.Drawing.Point(1035, 122);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(150, 36);
            this.btnXuatExcel.TabIndex = 5;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // F_BaoCaoDuAn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1536, 1032);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "F_BaoCaoDuAn";
            this.Text = "F_BaoCaoDuAn";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCDuAn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Button btnSoLuongNhanVien;
        private System.Windows.Forms.Button btnDSNhanVienTheoDuAn;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dtGridViewBCDuAn;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnTimKiem;
    }
}