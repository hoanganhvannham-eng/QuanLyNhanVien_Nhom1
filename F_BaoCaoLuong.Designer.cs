namespace QuanLyNhanVien3
{
    partial class F_BaoCaoLuong
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnTongLuongPhongBan = new System.Windows.Forms.Button();
            this.btnNVTongLuongCaoNhat = new System.Windows.Forms.Button();
            this.btnLuongHangThang = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dtGridViewBCLuong = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCLuong)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1396, 122);
            this.panel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(500, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "Báo Cáo Lương";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnXuatExcel);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.btnTongLuongPhongBan);
            this.panel2.Controls.Add(this.btnNVTongLuongCaoNhat);
            this.panel2.Controls.Add(this.btnLuongHangThang);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 122);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1396, 261);
            this.panel2.TabIndex = 4;
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.Location = new System.Drawing.Point(553, 156);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(161, 42);
            this.btnXuatExcel.TabIndex = 4;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Times New Roman", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(518, 29);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(295, 28);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // btnTongLuongPhongBan
            // 
            this.btnTongLuongPhongBan.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTongLuongPhongBan.Location = new System.Drawing.Point(924, 88);
            this.btnTongLuongPhongBan.Name = "btnTongLuongPhongBan";
            this.btnTongLuongPhongBan.Size = new System.Drawing.Size(309, 45);
            this.btnTongLuongPhongBan.TabIndex = 2;
            this.btnTongLuongPhongBan.Text = "Tổng lương theo phòng ban";
            this.btnTongLuongPhongBan.UseVisualStyleBackColor = true;
            this.btnTongLuongPhongBan.Click += new System.EventHandler(this.btnTongLuongPhongBan_Click);
            // 
            // btnNVTongLuongCaoNhat
            // 
            this.btnNVTongLuongCaoNhat.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNVTongLuongCaoNhat.Location = new System.Drawing.Point(417, 88);
            this.btnNVTongLuongCaoNhat.Name = "btnNVTongLuongCaoNhat";
            this.btnNVTongLuongCaoNhat.Size = new System.Drawing.Size(487, 45);
            this.btnNVTongLuongCaoNhat.TabIndex = 1;
            this.btnNVTongLuongCaoNhat.Text = "Nhân viên có tổng lương cao nhất trong tháng";
            this.btnNVTongLuongCaoNhat.UseVisualStyleBackColor = true;
            this.btnNVTongLuongCaoNhat.Click += new System.EventHandler(this.btnNVTongLuongCaoNhat_Click);
            // 
            // btnLuongHangThang
            // 
            this.btnLuongHangThang.Font = new System.Drawing.Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuongHangThang.Location = new System.Drawing.Point(107, 88);
            this.btnLuongHangThang.Name = "btnLuongHangThang";
            this.btnLuongHangThang.Size = new System.Drawing.Size(283, 45);
            this.btnLuongHangThang.TabIndex = 0;
            this.btnLuongHangThang.Text = "Bảng lương hàng tháng";
            this.btnLuongHangThang.UseVisualStyleBackColor = true;
            this.btnLuongHangThang.Click += new System.EventHandler(this.btnLuongHangThang_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dtGridViewBCLuong);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 383);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1396, 361);
            this.panel3.TabIndex = 5;
            // 
            // dtGridViewBCLuong
            // 
            this.dtGridViewBCLuong.AllowUserToAddRows = false;
            this.dtGridViewBCLuong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCLuong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewBCLuong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCLuong.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewBCLuong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtGridViewBCLuong.Name = "dtGridViewBCLuong";
            this.dtGridViewBCLuong.ReadOnly = true;
            this.dtGridViewBCLuong.RowHeadersWidth = 51;
            this.dtGridViewBCLuong.RowTemplate.Height = 24;
            this.dtGridViewBCLuong.Size = new System.Drawing.Size(1396, 361);
            this.dtGridViewBCLuong.TabIndex = 122;
            // 
            // F_BaoCaoLuong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 744);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "F_BaoCaoLuong";
            this.Text = "F_BaoCaoLuong";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCLuong)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnTongLuongPhongBan;
        private System.Windows.Forms.Button btnNVTongLuongCaoNhat;
        private System.Windows.Forms.Button btnLuongHangThang;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dtGridViewBCLuong;
        private System.Windows.Forms.Button btnXuatExcel;
    }
}