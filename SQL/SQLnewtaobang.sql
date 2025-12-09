-------------------------------------------------------
-- T?O DATABASE
-------------------------------------------------------
CREATE DATABASE QuanLyNhanVien_Nhom1_2;
GO

USE QuanLyNhanVien_Nhom1_2;
GO

-------------------------------------------------------
-- B?NG PHÒNG BAN
-------------------------------------------------------
CREATE TABLE tblPhongBan (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaPB VARCHAR(10) UNIQUE NOT NULL,
    TenPB NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200),
    SoDienThoai VARCHAR(20),
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);

-------------------------------------------------------
-- B?NG CH?C V?
-------------------------------------------------------
CREATE TABLE tblChucVu (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaCV VARCHAR(10) UNIQUE NOT NULL,
    TenCV NVARCHAR(100) NOT NULL,
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);

-------------------------------------------------------
-- B?NG NHÂN VIÊN
-------------------------------------------------------
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
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaPB) REFERENCES tblPhongBan(MaPB),
    FOREIGN KEY (MaCV) REFERENCES tblChucVu(MaCV)
);

-------------------------------------------------------
-- B?NG H?P ??NG
-------------------------------------------------------
CREATE TABLE tblHopDong (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaHopDong VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE,
    LoaiHopDong NVARCHAR(50),
    LuongCoBan DECIMAL(18,2) NOT NULL,
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);

-------------------------------------------------------
-- B?NG L??NG
-------------------------------------------------------
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

    GhiChu NVARCHAR(255),

    TongLuong AS (
        (LuongCoBan / SoNgayCongChuan) * SoNgayCong
        + PhuCap
        - KhauTru
    ) PERSISTED,

    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);
GO

-------------------------------------------------------
-- B?NG D? ÁN
-------------------------------------------------------
CREATE TABLE tblDuAn (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaDA VARCHAR(10) UNIQUE NOT NULL,
    TenDA NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(500),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);

-------------------------------------------------------
-- B?NG CHI TI?T D? ÁN
-------------------------------------------------------
CREATE TABLE tblChiTietDuAn (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaNV VARCHAR(10) NOT NULL,
    MaDA VARCHAR(10) NOT NULL,
    VaiTro NVARCHAR(100),
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    UNIQUE(MaNV, MaDA),
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV),
    FOREIGN KEY (MaDA) REFERENCES tblDuAn(MaDA)
);

-------------------------------------------------------
-- B?NG CH?M CÔNG
-------------------------------------------------------
CREATE TABLE tblChamCong (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaChamCong VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) NOT NULL,
    Ngay DATE NOT NULL,
    GioVao TIME NOT NULL,
    GioVe TIME NOT NULL,
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);

-------------------------------------------------------
-- B?NG TÀI KHO?N
-------------------------------------------------------
CREATE TABLE tblTaiKhoan (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaTK VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) UNIQUE NOT NULL,
    SoDienThoai VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(50) DEFAULT 'User',
    GhiChu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);

-------------------------------------------------------
-- TRIGGER C?P NH?T S? NGÀY CÔNG
-------------------------------------------------------
CREATE OR ALTER TRIGGER trg_UpdateSoNgayCong
ON tblChamCong
AFTER INSERT, DELETE
AS
BEGIN
    DECLARE @MaNV VARCHAR(10);

    SELECT @MaNV = MaNV FROM inserted;

    UPDATE tblLuong
    SET SoNgayCong = (
        SELECT COUNT(*)
        FROM tblChamCong
        WHERE tblChamCong.MaNV = tblLuong.MaNV
          AND MONTH(tblChamCong.Ngay) = tblLuong.Thang
          AND YEAR(tblChamCong.Ngay) = tblLuong.Nam
    )
    WHERE MaNV = @MaNV;
END;
GO

-------------------------------------------------------
-- T?O D? LI?U L??NG M?U
-------------------------------------------------------
DECLARE @i INT = 1;
DECLARE @nv VARCHAR(10);
DECLARE @Luong INT;
DECLARE @Thang INT;

WHILE @i <= 10
BEGIN
    SET @nv = 'NV0' + CAST(@i AS VARCHAR(2));
    SET @Luong = 6000000 + (@i * 1000000);

    SET @Thang = 1;
    WHILE @Thang <= 12
    BEGIN
        INSERT INTO tblLuong (MaLuong, MaNV, Thang, Nam, LuongCoBan, SoNgayCongChuan, SoNgayCong, PhuCap, KhauTru, GhiChu)
        VALUES (
            CONCAT('ML', @nv, '_', @Thang),
            @nv,
            @Thang,
            2024,
            @Luong,
            26,
            22 + (ABS(CHECKSUM(NEWID())) % 5),
            500000,
            0,
            N'D? li?u m?u'
        );

        SET @Thang = @Thang + 1;
    END

    SET @i = @i + 1;
END

-------------------------------------------------------
-- T?O D? LI?U CH?M CÔNG M?U CHO NV02
-------------------------------------------------------
DECLARE @Thang2 INT = 1;
DECLARE @Nam INT = 2024;
DECLARE @Ngay DATE;
DECLARE @MaChamCong VARCHAR(20);
DECLARE @i2 INT;

WHILE @Thang2 <= 12
BEGIN
    DECLARE @SoNgay INT = 22 + (ABS(CHECKSUM(NEWID())) % 5);

    SET @i2 = 1;
    WHILE @i2 <= @SoNgay
    BEGIN
        SET @Ngay = DATEADD(DAY, @i2 - 1, DATEFROMPARTS(@Nam, @Thang2, 1));

        WHILE DATENAME(WEEKDAY, @Ngay) IN ('Saturday', 'Sunday')
        BEGIN
            SET @Ngay = DATEADD(DAY, 1, @Ngay);
        END

        SET @MaChamCong = CONCAT('CCNV02_', @Nam, RIGHT('0' + CAST(@Thang2 AS VARCHAR), 2), '_', RIGHT('0' + CAST(@i2 AS VARCHAR),2));

        INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, GhiChu, DeletedAt)
        VALUES (@MaChamCong, 'NV02', @Ngay, '08:00:00', '17:00:00', N'?i làm bình th??ng', 0);

        SET @i2 = @i2 + 1;
    END

    SET @Thang2 = @Thang2 + 1;
END;

-------------------------------------------------------
-- B?NG ROLE
-------------------------------------------------------
CREATE TABLE tblRole (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenRole NVARCHAR(50) UNIQUE NOT NULL,
    MoTa NVARCHAR(200)
);

INSERT INTO tblRole (TenRole, MoTa) VALUES
('Admin', 'Toàn quy?n h? th?ng'),
('User', 'Ch? xem và ch?m công');

-------------------------------------------------------
-- THÊM ROLE CHO TÀI KHO?N
-------------------------------------------------------
ALTER TABLE tblTaiKhoan ADD RoleId INT NULL;

UPDATE tblTaiKhoan SET RoleId = 2;

UPDATE tblTaiKhoan SET RoleId = 1 WHERE MaTK = 'TK001';

ALTER TABLE tblTaiKhoan ALTER COLUMN RoleId INT NOT NULL;

ALTER TABLE tblTaiKhoan
ADD CONSTRAINT FK_TaiKhoan_Role FOREIGN KEY (RoleId)
REFERENCES tblRole(Id);
