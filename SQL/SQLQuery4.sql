-- ========================================
-- 1. XÓA DỮ LIỆU THEO THỨ TỰ
-- ========================================
DELETE FROM tblChiTietDuAn;
DELETE FROM tblChamCong;
DELETE FROM tblLuong;
DELETE FROM tblHopDong;
DELETE FROM tblTaiKhoan;
DELETE FROM tblNhanVien;
DELETE FROM tblDuAn;
DELETE FROM tblChucVu;
DELETE FROM tblPhongBan;

USE QuanLyNhanVien3;

-- ========================================
-- 2. THÊM DỮ LIỆU MẪU
-- ========================================

-- ===== Bảng Phòng Ban =====
INSERT INTO tblPhongBan (MaPB, TenPB, DiaChi, SoDienThoai, Ghichu)
VALUES
('PB001', N'Phòng Hành Chính', N'Hà Nội', '0241234567', N'Quản lý nhân sự & hành chính'),
('PB002', N'Phòng Kỹ Thuật', N'Hồ Chí Minh', '0287654321', N'Phát triển & bảo trì hệ thống'),
('PB003', N'Phòng Kinh Doanh', N'Đà Nẵng', '0236789123', N'Tìm kiếm khách hàng và bán hàng'),
('PB004', N'Phòng Tài Chính', N'Hà Nội', '0243333444', N'Quản lý tài chính'),
('PB005', N'Phòng Marketing', N'Hồ Chí Minh', '0282222333', N'Chiến lược marketing'),
('PB006', N'Phòng Nghiên Cứu', N'Hà Nội', '0245555666', N'Nghiên cứu sản phẩm mới'),
('PB007', N'Phòng Chăm Sóc KH', N'Hải Phòng', '0222666777', N'Hỗ trợ khách hàng'),
('PB008', N'Phòng QA', N'Hồ Chí Minh', '0281111222', N'Đảm bảo chất lượng'),
('PB009', N'Phòng IT', N'Hà Nội', '0247777888', N'Hỗ trợ kỹ thuật nội bộ'),
('PB010', N'Phòng Logistics', N'Đà Nẵng', '0231234433', N'Quản lý chuỗi cung ứng');

-- ===== Bảng Chức Vụ =====
INSERT INTO tblChucVu (MaCV, TenCV, Ghichu)
VALUES
('CV001', N'Giám đốc', N'Quản lý toàn công ty'),
('CV002', N'Trưởng phòng', N'Quản lý phòng ban'),
('CV003', N'Nhân viên', N'Thực hiện công việc được giao'),
('CV004', N'Phó phòng', N'Hỗ trợ trưởng phòng'),
('CV005', N'Thực tập sinh', N'Học việc'),
('CV006', N'Kỹ sư', N'Chuyên môn kỹ thuật'),
('CV007', N'Nhân viên cao cấp', N'Công việc đặc biệt'),
('CV008', N'Chuyên viên', N'Nghiên cứu và phát triển'),
('CV009', N'Quản lý dự án', N'Điều phối dự án'),
('CV010', N'Kế toán', N'Quản lý tài chính');

-- ===== Bảng Nhân Viên =====
INSERT INTO tblNhanVien (MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaPB, MaCV, Ghichu)
VALUES
('NV001', N'Nguyễn Văn A', '1985-05-20', N'Nam', N'Hà Nội', '0912345678', 'ana@example.com', 'PB001', 'CV001', N'Giám đốc công ty'),
('NV002', N'Trần Thị B', '1990-09-15', N'Nữ', N'Hồ Chí Minh', '0987654321', 'hib@example.com', 'PB002', 'CV002', N'Trưởng phòng kỹ thuật'),
('NV003', N'Lê Văn C', '1995-12-01', N'Nam', N'Đà Nẵng', '0934567890', 'vnc@example.com', 'PB003', 'CV003', N'Nhân viên kinh doanh'),
('NV004', N'Phạm Thị D', '1997-07-07', N'Nữ', N'Hải Phòng', '0978123456', 'thi@example.com', 'PB002', 'CV003', N'Nhân viên kỹ thuật'),
('NV005', N'Hoàng Văn E', '1992-04-18', N'Nam', N'Hà Nội', '0911002200', 'hve@example.com', 'PB004', 'CV010', N'Nhân viên kế toán'),
('NV006', N'Nguyễn Thị F', '1998-10-25', N'Nữ', N'Đà Nẵng', '0933001122', 'ntf@example.com', 'PB005', 'CV005', N'Thực tập sinh marketing'),
('NV007', N'Đỗ Văn G', '1989-11-30', N'Nam', N'Hồ Chí Minh', '0944112233', 'dvg@example.com', 'PB006', 'CV008', N'Chuyên viên nghiên cứu'),
('NV008', N'Bùi Thị H', '1993-03-12', N'Nữ', N'Hải Phòng', '0955223344', 'bth@example.com', 'PB007', 'CV004', N'Phó phòng CSKH'),
('NV009', N'Nguyễn Văn I', '1991-06-22', N'Nam', N'Hà Nội', '0966334455', 'nvi@example.com', 'PB009', 'CV006', N'Kỹ sư IT'),
('NV010', N'Lưu Thị K', '1996-08-14', N'Nữ', N'Đà Nẵng', '0977445566', 'ltk@example.com', 'PB010', 'CV007', N'Nhân viên logistics');
INSERT INTO tblNhanVien (MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaPB, MaCV, Ghichu,DeletedAt)
VALUES
('NV111', N'Hoàng Tuấn Anh', '1985-05-20', N'Nam', N'Hà Nội', '0912345678', 'anh@example.com', 'PB001', 'CV001', N'Giám đốc công ty', '3'),
('NV222', N'Nguyễn Trung Kien', '1985-05-20', N'Nam', N'Hà Nội', '0912345678', 'Kien@example.com', 'PB001', 'CV001', N'Giám đốc công ty', '3'),
('NV333', N'Nguyễn Văn Thành', '1985-05-20', N'Nam', N'Hà Nội', '0912345678', 'Thanh@example.com', 'PB001', 'CV001', N'Giám đốc công ty', '3');

-- ===== Bảng Hợp Đồng =====
INSERT INTO tblHopDong (MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu)
VALUES
('HD001', 'NV001', '2020-01-01', NULL, N'Không xác định thời hạn', 30000000, N'Hợp đồng Giám đốc'),
('HD002', 'NV002', '2021-03-01', '2026-03-01', N'Xác định 5 năm', 20000000, N'Hợp đồng Trưởng phòng'),
('HD003', 'NV003', '2022-06-15', '2025-06-15', N'3 năm', 12000000, N'Hợp đồng nhân viên KD'),
('HD004', 'NV004', '2023-02-01', '2026-02-01', N'3 năm', 10000000, N'Hợp đồng nhân viên KT'),
('HD005', 'NV005', '2024-01-01', '2026-01-01', N'2 năm', 15000000, N'Hợp đồng kế toán'),
('HD006', 'NV006', '2024-07-01', '2025-07-01', N'1 năm', 8000000, N'Hợp đồng thực tập sinh'),
('HD007', 'NV007', '2023-04-01', '2026-04-01', N'3 năm', 14000000, N'Hợp đồng chuyên viên'),
('HD008', 'NV008', '2024-05-01', '2026-05-01', N'2 năm', 13000000, N'Hợp đồng phó phòng'),
('HD009', 'NV009', '2023-06-01', '2027-06-01', N'4 năm', 16000000, N'Hợp đồng kỹ sư IT'),
('HD010', 'NV010', '2024-09-01', '2026-09-01', N'2 năm', 11000000, N'Hợp đồng logistics');

-- ===== Bảng Lương =====
INSERT INTO tblLuong (MaLuong, MaNV, Thang, Nam, LuongCoBan, SoNgayCong, PhuCap, KhauTru, Ghichu)
VALUES
('L001', 'NV001', 8, 2025, 30000000, 22, 5000000, 1000000, N'Lương tháng 8 Giám đốc'),
('L002', 'NV002', 8, 2025, 20000000, 22, 3000000, 500000, N'Lương tháng 8 Trưởng phòng'),
('L003', 'NV003', 8, 2025, 12000000, 21, 2000000, 200000, N'Lương tháng 8 NV KD'),
('L004', 'NV004', 8, 2025, 10000000, 20, 1500000, 100000, N'Lương tháng 8 NV KT'),
('L005', 'NV005', 8, 2025, 15000000, 22, 1000000, 300000, N'Lương tháng 8 Kế toán'),
('L006', 'NV006', 8, 2025, 8000000, 18, 500000, 50000, N'Lương tháng 8 TTS'),
('L007', 'NV007', 8, 2025, 14000000, 22, 2000000, 100000, N'Lương tháng 8 Chuyên viên'),
('L008', 'NV008', 8, 2025, 13000000, 21, 1500000, 150000, N'Lương tháng 8 Phó phòng'),
('L009', 'NV009', 8, 2025, 16000000, 22, 2000000, 200000, N'Lương tháng 8 Kỹ sư IT'),
('L010', 'NV010', 8, 2025, 11000000, 20, 1200000, 100000, N'Lương tháng 8 Logistics');

-- ===== Bảng Dự Án =====
INSERT INTO tblDuAn (MaDA, TenDA, MoTa, NgayBatDau, NgayKetThuc, Ghichu)
VALUES
('DA001', N'Hệ thống ERP', N'Xây dựng hệ thống quản lý doanh nghiệp', '2023-01-01', '2025-12-31', N'Dự án quan trọng'),
('DA002', N'Website TMĐT', N'Phát triển website thương mại điện tử', '2024-03-01', '2025-03-01', N'Dự án thương mại'),
('DA003', N'Ứng dụng Mobile', N'Ứng dụng di động bán hàng', '2025-01-01', NULL, N'Dự án mới'),
('DA004', N'Hệ thống CRM', N'Quản lý khách hàng', '2024-06-01', '2025-12-31', N'Dự án CSKH'),
('DA005', N'Nâng cấp IT', N'Hạ tầng kỹ thuật', '2023-09-01', '2024-12-31', N'Dự án nội bộ'),
('DA006', N'Hệ thống tài chính', N'Quản lý tài chính', '2024-02-01', NULL, N'Dự án tài chính'),
('DA007', N'Marketing Online', N'Chiến dịch marketing', '2025-04-01', '2025-09-01', N'Quảng bá sản phẩm'),
('DA008', N'Hệ thống QA', N'Nâng cao chất lượng', '2025-05-01', NULL, N'Dự án chất lượng'),
('DA009', N'Ứng dụng AI', N'Ứng dụng trí tuệ nhân tạo', '2024-10-01', NULL, N'Nghiên cứu AI'),
('DA010', N'Logistics thông minh', N'Quản lý kho vận', '2025-06-01', NULL, N'Dự án logistics');

-- ===== Bảng Chi Tiết Dự Án =====
INSERT INTO tblChiTietDuAn (MaNV, MaDA, VaiTro, Ghichu)
VALUES
('NV001', 'DA001', N'Quản lý dự án', N'Giám sát toàn bộ'),
('NV002', 'DA001', N'Chủ trì kỹ thuật', N'Thiết kế hệ thống'),
('NV003', 'DA002', N'Kinh doanh', N'Tìm khách hàng'),
('NV004', 'DA003', N'Lập trình viên', N'Phát triển ứng dụng'),
('NV005', 'DA004', N'Kế toán', N'Quản lý chi phí'),
('NV006', 'DA005', N'Marketing', N'Triển khai chiến dịch'),
('NV007', 'DA006', N'Nghiên cứu', N'Nghiên cứu sản phẩm mới'),
('NV008', 'DA007', N'Hỗ trợ khách hàng', N'Tư vấn'),
('NV009', 'DA008', N'Kỹ sư', N'Xây dựng hệ thống QA'),
('NV010', 'DA009', N'Quản lý kho', N'Tối ưu vận chuyển');

-- ===== Bảng Chấm Công =====
INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu)
VALUES
('CC001', 'NV001', '2025-08-01', '08:00:00', '17:00:00', N'Đi làm đúng giờ'),
('CC002', 'NV002', '2025-08-01', '08:15:00', '17:30:00', N'Đi muộn 15 phút'),
('CC003', 'NV003', '2025-08-01', '08:00:00', '17:00:00', N'Làm việc đầy đủ'),
('CC004', 'NV004', '2025-08-01', '08:30:00', '17:00:00', N'Đi muộn 30 phút'),
('CC005', 'NV005', '2025-08-01', '08:10:00', '17:10:00', N'Đi làm đúng giờ'),
('CC006', 'NV006', '2025-08-01', '08:00:00', '16:30:00', N'Về sớm 30 phút'),
('CC007', 'NV007', '2025-08-01', '08:05:00', '17:05:00', N'Đi muộn 5 phút'),
('CC008', 'NV008', '2025-08-01', '08:00:00', '17:00:00', N'Đi làm đúng giờ'),
('CC009', 'NV009', '2025-08-01', '08:00:00', '17:15:00', N'Làm thêm 15 phút'),
('CC010', 'NV010', '2025-08-01', '08:20:00', '17:20:00', N'Đi muộn 20 phút');

-- ===== Bảng Tài Khoản =====
INSERT INTO tblTaiKhoan (MaTK, MaNV, TenDangNhap, MatKhau, Quyen, Ghichu)
VALUES
('TK001', 'NV001', 'admin', '123456', N'User', N'Tài khoản quản trị'),
('TK002', 'NV002', 'thib', '123456', N'Manager', N'Tài khoản trưởng phòng'),
('TK003', 'NV003', 'vanc', '123456', N'User', N'Tài khoản nhân viên KD'),
('TK004', 'NV004', 'phamthi', '123456', N'User', N'Tài khoản nhân viên KT'),
('TK005', 'NV005', 'hoange', '123456', N'User', N'Tài khoản kế toán'),
('TK006', 'NV006', 'nguyentf', '123456', N'User', N'Tài khoản thực tập sinh'),
('TK007', 'NV007', 'dovan', '123456', N'User', N'Tài khoản chuyên viên'),
('TK008', 'NV008', 'buith', '123456', N'Manager', N'Tài khoản phó phòng'),
('TK009', 'NV009', 'nguyeni', '123456', N'User', N'Tài khoản kỹ sư IT'),
('TK010', 'NV010', 'luuthik', '123456', N'User', N'Tài khoản logistics');

INSERT INTO tblTaiKhoan (MaTK, MaNV, TenDangNhap, MatKhau, Quyen, Ghichu, DeletedAt)
VALUES
('TK111', 'NV111', 'a', '1', N'Admin', N'Tài khoản quản trị', '3'),
('TK222', 'NV222', 'k', '1', N'Admin', N'Tài khoản quản trị', '3'),
('TK333', 'NV333', 't', '1', N'Admin', N'Tài khoản quản trị', '3');