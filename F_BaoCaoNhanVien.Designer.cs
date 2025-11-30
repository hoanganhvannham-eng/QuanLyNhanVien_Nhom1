namespace QuanLyNhanVien3
{
    partial class F_BaoCaoNhanVien
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnTimKiemTheoTen = new System.Windows.Forms.Button();
            this.txttimkiemtheoten = new System.Windows.Forms.TextBox();
            this.btnsoluongtheogioitinh = new System.Windows.Forms.Button();
            this.btnThongKeNhanVien = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtGridViewBCNhanVien = new System.Windows.Forms.DataGridView();
            this.btnXuatEXL = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCNhanVien)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1385, 122);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(494, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 49);
            this.label1.TabIndex = 0;
            this.label1.Text = "Báo Cáo Nhân Viên";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnXuatEXL);
            this.panel3.Controls.Add(this.btnTimKiemTheoTen);
            this.panel3.Controls.Add(this.txttimkiemtheoten);
            this.panel3.Controls.Add(this.btnsoluongtheogioitinh);
            this.panel3.Controls.Add(this.btnThongKeNhanVien);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 122);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1385, 248);
            this.panel3.TabIndex = 3;
            // 
            // btnTimKiemTheoTen
            // 
            this.btnTimKiemTheoTen.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnTimKiemTheoTen.Location = new System.Drawing.Point(790, 150);
            this.btnTimKiemTheoTen.Name = "btnTimKiemTheoTen";
            this.btnTimKiemTheoTen.Size = new System.Drawing.Size(246, 25);
            this.btnTimKiemTheoTen.TabIndex = 3;
            this.btnTimKiemTheoTen.Text = "Tìm Kiếm Theo Tên";
            this.btnTimKiemTheoTen.UseVisualStyleBackColor = true;
            this.btnTimKiemTheoTen.Click += new System.EventHandler(this.btnTimKiemTheoTen_Click);
            // 
            // txttimkiemtheoten
            // 
            this.txttimkiemtheoten.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txttimkiemtheoten.Location = new System.Drawing.Point(790, 112);
            this.txttimkiemtheoten.Name = "txttimkiemtheoten";
            this.txttimkiemtheoten.Size = new System.Drawing.Size(246, 22);
            this.txttimkiemtheoten.TabIndex = 2;
            // 
            // btnsoluongtheogioitinh
            // 
            this.btnsoluongtheogioitinh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnsoluongtheogioitinh.Location = new System.Drawing.Point(444, 112);
            this.btnsoluongtheogioitinh.Name = "btnsoluongtheogioitinh";
            this.btnsoluongtheogioitinh.Size = new System.Drawing.Size(224, 50);
            this.btnsoluongtheogioitinh.TabIndex = 1;
            this.btnsoluongtheogioitinh.Text = "Thống Kê Số Lượng Nhân Viên Theo Giới Tính";
            this.btnsoluongtheogioitinh.UseVisualStyleBackColor = true;
            this.btnsoluongtheogioitinh.Click += new System.EventHandler(this.btnsoluongtheogioitinh_Click);
            // 
            // btnThongKeNhanVien
            // 
            this.btnThongKeNhanVien.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnThongKeNhanVien.Location = new System.Drawing.Point(117, 112);
            this.btnThongKeNhanVien.Name = "btnThongKeNhanVien";
            this.btnThongKeNhanVien.Size = new System.Drawing.Size(223, 50);
            this.btnThongKeNhanVien.TabIndex = 0;
            this.btnThongKeNhanVien.Text = "Thống Kê Nhân Viên ";
            this.btnThongKeNhanVien.UseVisualStyleBackColor = true;
            this.btnThongKeNhanVien.Click += new System.EventHandler(this.btnThongKeNhanVien_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtGridViewBCNhanVien);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 370);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1385, 380);
            this.panel2.TabIndex = 4;
            // 
            // dtGridViewBCNhanVien
            // 
            this.dtGridViewBCNhanVien.AllowUserToAddRows = false;
            this.dtGridViewBCNhanVien.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtGridViewBCNhanVien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtGridViewBCNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtGridViewBCNhanVien.Location = new System.Drawing.Point(0, 0);
            this.dtGridViewBCNhanVien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtGridViewBCNhanVien.Name = "dtGridViewBCNhanVien";
            this.dtGridViewBCNhanVien.ReadOnly = true;
            this.dtGridViewBCNhanVien.RowHeadersWidth = 51;
            this.dtGridViewBCNhanVien.RowTemplate.Height = 24;
            this.dtGridViewBCNhanVien.Size = new System.Drawing.Size(1385, 380);
            this.dtGridViewBCNhanVien.TabIndex = 121;
            // 
            // btnXuatEXL
            // 
            this.btnXuatEXL.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnXuatEXL.Location = new System.Drawing.Point(1122, 112);
            this.btnXuatEXL.Name = "btnXuatEXL";
            this.btnXuatEXL.Size = new System.Drawing.Size(223, 50);
            this.btnXuatEXL.TabIndex = 4;
            this.btnXuatEXL.Text = "Xuất Excel";
            this.btnXuatEXL.UseVisualStyleBackColor = true;
            this.btnXuatEXL.Click += new System.EventHandler(this.btnXuatEXL_Click);
            // 
            // F_BaoCaoNhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1385, 750);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "F_BaoCaoNhanVien";
            this.Text = "F_BaoCaoNhanVien";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtGridViewBCNhanVien)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnThongKeNhanVien;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnsoluongtheogioitinh;
        private System.Windows.Forms.Button btnTimKiemTheoTen;
        private System.Windows.Forms.TextBox txttimkiemtheoten;
        private System.Windows.Forms.DataGridView dtGridViewBCNhanVien;
        private System.Windows.Forms.Button btnXuatEXL;
    }
}