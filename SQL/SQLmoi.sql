-- ========================================
-- T?O DATABASE
-- ========================================
CREATE DATABASE QuanLyNhanVien_Nhom1;
GO

USE QuanLyNhanVien_Nhom1;
GO

-- ========================================
-- T?O B?NG
-- ========================================

-- B?ng Phòng Ban
CREATE TABLE tblPhongBan (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaPB VARCHAR(10) UNIQUE NOT NULL,
    TenPB NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200),
    SoDienThoai VARCHAR(20),
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);
GO

-- B?ng Ch?c V?
CREATE TABLE tblChucVu (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaCV VARCHAR(10) UNIQUE NOT NULL,
    TenCV NVARCHAR(100) NOT NULL,
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);
GO

-- B?ng Nhân Viên
CREATE TABLE tblNhanVien (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaNV VARCHAR(10) UNIQUE NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(200),
    SoDienThoai VARCHAR(20),
    Email VARCHAR(100) UNIQUE,
    MaPB VARCHAR(10) NOT NULL,
    MaCV VARCHAR(10) NOT NULL,
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaPB) REFERENCES tblPhongBan(MaPB),
    FOREIGN KEY (MaCV) REFERENCES tblChucVu(MaCV)
);
GO

-- B?ng H?p ??ng
CREATE TABLE tblHopDong (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaHopDong VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE,
    LoaiHopDong NVARCHAR(50),
    LuongCoBan DECIMAL(18,2) NOT NULL,
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);
GO

-- B?ng L??ng
CREATE TABLE tblLuong (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaLuong VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) NOT NULL,
    Thang INT CHECK (Thang BETWEEN 1 AND 12),
    Nam INT NOT NULL,
    LuongCoBan DECIMAL(18,2) NOT NULL,
    SoNgayCongChuan INT NOT NULL DEFAULT 26,
    SoNgayCong INT DEFAULT 0,
    PhuCap DECIMAL(18,2) DEFAULT 0,
    KhauTru DECIMAL(18,2) DEFAULT 0,
    Ghichu NVARCHAR(255),
    TongLuong AS ((LuongCoBan / SoNgayCongChuan) * SoNgayCong + PhuCap - KhauTru) PERSISTED,
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);
GO

-- B?ng D? Án
CREATE TABLE tblDuAn (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaDA VARCHAR(10) UNIQUE NOT NULL,
    TenDA NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(500),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);
GO

-- B?ng Chi Ti?t D? Án
CREATE TABLE tblChiTietDuAn (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaNV VARCHAR(10) NOT NULL,
    MaDA VARCHAR(10) NOT NULL,
    VaiTro NVARCHAR(100),
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    UNIQUE(MaNV, MaDA),
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV),
    FOREIGN KEY (MaDA) REFERENCES tblDuAn(MaDA)
);
GO

-- B?ng Ch?m Công
CREATE TABLE tblChamCong (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaChamCong VARCHAR(20) UNIQUE NOT NULL,
    MaNV VARCHAR(10) NOT NULL,
    Ngay DATE NOT NULL,
    GioVao TIME NOT NULL,
    GioVe TIME NOT NULL,
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);
GO

-- B?ng Tài Kho?n
CREATE TABLE tblTaiKhoan (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaTK VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) UNIQUE NOT NULL,
    SoDienThoai VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(50) DEFAULT 'User',
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);
GO

-- B?ng Role
CREATE TABLE tblRole (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenRole NVARCHAR(50) UNIQUE NOT NULL,
    MoTa NVARCHAR(200)
);
GO

-- ========================================
-- T?O TRIGGER C?P NH?T S? NGÀY CÔNG
-- ========================================
CREATE OR ALTER TRIGGER trg_UpdateSoNgayCong
ON tblChamCong
AFTER INSERT, DELETE
AS
BEGIN
    UPDATE L
    SET SoNgayCong = (
        SELECT COUNT(*)
        FROM tblChamCong C
        WHERE C.MaNV = L.MaNV
          AND MONTH(C.Ngay) = L.Thang
          AND YEAR(C.Ngay) = L.Nam
    )
    FROM tblLuong L
    INNER JOIN inserted i ON L.MaNV = i.MaNV;
END;
GO

-- ========================================
-- THÊM D? LI?U M?U
-- ========================================

-- Phòng Ban
INSERT INTO tblPhongBan (MaPB, TenPB, DiaChi, SoDienThoai, Ghichu)
VALUES
('PB01', N'Phòng Kinh Doanh', N'123 ???ng A, Hà N?i', '0241234567', N'Ch?u trách nhi?m doanh s?'),
('PB02', N'Phòng Nhân S?', N'456 ???ng B, Hà N?i', '0242345678', N'Tuy?n d?ng & qu?n lý nhân s?'),
('PB03', N'Phòng IT', N'789 ???ng C, Hà N?i', '0243456789', N'Qu?n tr? h? th?ng & ph?n m?m');
GO

-- Ch?c V?
INSERT INTO tblChucVu (MaCV, TenCV, Ghichu)
VALUES
('CV01', N'Tr??ng Phòng', N'Qu?n lý phòng ban'),
('CV02', N'Phó Phòng', N'H? tr? tr??ng phòng'),
('CV03', N'Nhân Viên', N'Nhân viên làm vi?c tr?c ti?p');
GO

-- Nhân Viên
INSERT INTO tblNhanVien (MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaPB, MaCV, Ghichu)
VALUES
('NV01', N'Nguy?n V?n A', '1990-01-15', N'Nam', N'Hà N?i', '0901000001', 'a.nguyen@example.com', 'PB01', 'CV03', N'Nhân viên kinh doanh'),
('NV02', N'Tr?n Th? B', '1992-02-20', N'N?', N'Hà N?i', '0901000002', 'b.tran@example.com', 'PB02', 'CV03', N'Nhân s?'),
('NV03', N'Ph?m V?n C', '1988-03-10', N'Nam', N'Hà N?i', '0901000003', 'c.pham@example.com', 'PB03', 'CV03', N'IT chuyên môn cao');
GO

-- H?p ??ng
INSERT INTO tblHopDong (MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu)
VALUES
('HD01', 'NV01', '2023-01-01', '2023-12-31', N'Chính th?c', 15000000, N'H?p ??ng 1 n?m'),
('HD02', 'NV02', '2023-01-01', '2023-06-30', N'Th? vi?c', 12000000, N'H?p ??ng th? vi?c');
GO

-- L??ng
INSERT INTO tblLuong (MaLuong, MaNV, Thang, Nam, LuongCoBan, SoNgayCong, PhuCap, KhauTru, Ghichu)
VALUES
('L0101', 'NV01', 1, 2023, 15000000, 22, 2000000, 500000, N'L??ng tháng 1'),
('L0102', 'NV01', 2, 2023, 15000000, 22, 2000000, 500000, N'L??ng tháng 2'),
('L0201', 'NV02', 1, 2023, 12000000, 22, 2000000, 500000, N'L??ng tháng 1');
GO

-- D? Án
INSERT INTO tblDuAn (MaDA, TenDA, MoTa, NgayBatDau, NgayKetThuc, Ghichu)
VALUES
('DA01', N'D? án Alpha', N'Xây d?ng ph?n m?m Alpha', '2023-01-01', '2023-06-30', N'D? án ??u tiên'),
('DA02', N'D? án Beta', N'Phát tri?n ?ng d?ng Beta', '2023-02-01', '2023-07-31', N'');
GO

-- Chi Ti?t D? Án
INSERT INTO tblChiTietDuAn (MaNV, MaDA, VaiTro, Ghichu)
VALUES
('NV01', 'DA01', N'Tr??ng nhóm', ''),
('NV02', 'DA01', N'Nhân viên', '');
GO

-- Ch?m Công
INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu)
VALUES
('CCNV01_20230101', 'NV01', '2023-01-01', '08:00', '17:00', N'?i làm bình th??ng'),
('CCNV02_20230101', 'NV02', '2023-01-01', '08:00', '17:00', N'?i làm bình th??ng');
GO

-- Role
INSERT INTO tblRole (TenRole, MoTa)
VALUES
('Admin', 'Toàn quy?n h? th?ng'),
('User', 'Ch? xem & ch?m công');
GO

-- Tài Kho?n
ALTER TABLE tblTaiKhoan
ADD RoleId INT NULL;
GO

INSERT INTO tblTaiKhoan (MaTK, MaNV, SoDienThoai, MatKhau, Quyen, Ghichu, RoleId)
VALUES
('TK01', 'NV01', '0901000001', '123456', 'Admin', '', 1),
('TK02', 'NV02', '0901000002', '123456', 'User', '', 2);
GO
