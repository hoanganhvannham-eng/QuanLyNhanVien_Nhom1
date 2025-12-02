
-- ========================================
-- T?O C? S? D? LI?U
-- ========================================
CREATE DATABASE QuanLyNhanVien_Nhom1;
GO

USE QuanLyNhanVien_Nhom1;
GO

-- ========================================
-- ============ CÁC B?NG =================
-- ========================================

-- ===== B?ng Phòng Ban =====
CREATE TABLE tblPhongBan (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaPB VARCHAR(10) UNIQUE NOT NULL,
    TenPB NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(200),
    SoDienThoai VARCHAR(20),
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);

-- ===== B?ng Ch?c V? =====
CREATE TABLE tblChucVu (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaCV VARCHAR(10) UNIQUE NOT NULL,
    TenCV NVARCHAR(100) NOT NULL,
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0
);

-- ===== B?ng Nhân Viên =====
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

-- ===== B?ng H?p ??ng =====
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

-- ===== B?ng L??ng =====
CREATE TABLE tblLuong (
    Id INT PRIMARY KEY IDENTITY(1,1),

    MaLuong VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) NOT NULL,

    Thang INT CHECK (Thang BETWEEN 1 AND 12),
    Nam INT NOT NULL,

    LuongCoBan DECIMAL(18,2) NOT NULL,
    SoNgayCongChuan INT NOT NULL DEFAULT 26,   -- ngày công chu?n
    SoNgayCong INT DEFAULT 0,                 -- ngày công th?c t? (T? ??NG TÍNH)

    PhuCap DECIMAL(18,2) DEFAULT 0,
    KhauTru DECIMAL(18,2) DEFAULT 0,

    Ghichu NVARCHAR(255),

    TongLuong AS (
        (LuongCoBan / SoNgayCongChuan) * SoNgayCong
        + PhuCap
        - KhauTru
    ) PERSISTED,

    DeletedAt INT NOT NULL DEFAULT 0,

    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);
GO

-- ===== B?ng D? Án =====
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

-- ===== B?ng Chi Ti?t D? Án =====
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

-- ===== B?ng Ch?m Công =====
CREATE TABLE tblChamCong (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaChamCong VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) NOT NULL,
    Ngay DATE NOT NULL,
    GioVao TIME NOT NULL,
    GioVe TIME NOT NULL,
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);

-- ===== B?ng Tài Kho?n =====
CREATE TABLE tblTaiKhoan (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MaTK VARCHAR(10) UNIQUE NOT NULL,
    MaNV VARCHAR(10) UNIQUE NOT NULL,
    SoDienThoai VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    Quyen NVARCHAR(50) DEFAULT 'User',
    Ghichu NVARCHAR(255),
    DeletedAt INT NOT NULL DEFAULT 0,
    FOREIGN KEY (MaNV) REFERENCES tblNhanVien(MaNV)
);

-- =====================================================

IF OBJECT_ID('tblLuong', 'U') IS NOT NULL
    DROP TABLE tblLuong;
GO





CREATE OR ALTER TRIGGER trg_UpdateSoNgayCong
ON tblChamCong
AFTER INSERT, DELETE
AS
BEGIN
    -- C?p nh?t t?t c? các nhân viên b? ?nh h??ng
    DECLARE @MaNV VARCHAR(10);

    -- L?y các nhân viên v?a ???c ch?m công
    SELECT @MaNV = MaNV FROM inserted;

    -- Tính l?i s? ngày công th?c t? trong tháng
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


-- Xóa d? li?u c? (n?u c?n)
-- DELETE FROM tblLuong;
-- GO

-------------------------------------------------------
--  D? LI?U L??NG CHO 3 NHÂN VIÊN TRONG 12 THÁNG
-------------------------------------------------------


DECLARE @i INT = 1;
DECLARE @nv VARCHAR(10);
DECLARE @Luong INT;
DECLARE @Thang INT;

WHILE @i <= 10
BEGIN
    SET @nv = 'NV0' + CAST(@i AS VARCHAR(2));
    SET @Luong = 6000000 + (@i * 1000000);   -- LCB khác nhau m?i NV (7tr ? 16tr)

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
            22 + (ABS(CHECKSUM(NEWID())) % 5),  -- sinh s? ngày làm 22–26
            500000,
            0,
            N'D? li?u m?u'
        );

        SET @Thang = @Thang + 1;
    END

    SET @i = @i + 1;
END



DECLARE @Thang INT = 1;
DECLARE @Nam INT = 2024;
DECLARE @Ngay DATE;
DECLARE @MaChamCong VARCHAR(20);
DECLARE @i INT;

WHILE @Thang <= 12
BEGIN
    -- Xác ??nh s? ngày ?i làm th?c t? trong tháng (22 ? 26)
    DECLARE @SoNgay INT = 22 + (ABS(CHECKSUM(NEWID())) % 5);

    SET @i = 1;
    WHILE @i <= @SoNgay
    BEGIN
        -- Gi? s? nhân viên ?i làm liên t?c t? ngày 1 c?a tháng (b? th? 7, CN)
        SET @Ngay = DATEADD(DAY, @i - 1, DATEFROMPARTS(@Nam, @Thang, 1));

        -- N?u r?i vào th? 7 ho?c ch? nh?t ? t?ng thêm 1 ngày
        WHILE DATENAME(WEEKDAY, @Ngay) IN ('Saturday', 'Sunday')
        BEGIN
            SET @Ngay = DATEADD(DAY, 1, @Ngay);
        END

        SET @MaChamCong = CONCAT('CCNV02_', @Nam, RIGHT('0' + CAST(@Thang AS VARCHAR), 2), '_', RIGHT('0' + CAST(@i AS VARCHAR),2));

        INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu, DeletedAt)
        VALUES (@MaChamCong, 'NV02', @Ngay, '08:00:00', '17:00:00', N'?i làm bình th??ng', 0);

        SET @i = @i + 1;
    END

    SET @Thang = @Thang + 1;
END;






CREATE TABLE tblRole (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TenRole NVARCHAR(50) UNIQUE NOT NULL,
    MoTa NVARCHAR(200)
);

INSERT INTO tblRole (TenRole, MoTa) VALUES
('Admin', 'Toàn quy?n h? th?ng'),
('User', 'Ch? xem & ch?m công');

ALTER TABLE tblTaiKhoan
ADD RoleId INT NULL;

UPDATE tblTaiKhoan SET RoleId = 2;

UPDATE tblTaiKhoan SET RoleId = 1 WHERE MaTK = 'TK001';
UPDATE tblTaiKhoan SET RoleId = 2 WHERE MaTK <> 'TK001';

ALTER TABLE tblTaiKhoan
ALTER COLUMN RoleId INT NOT NULL;


ALTER TABLE tblTaiKhoan
ADD CONSTRAINT FK_TaiKhoan_Role
FOREIGN KEY (RoleId)
REFERENCES tblRole(Id);







-- ========================================
-- ============ Thêm d? li?u Phòng Ban =====
-- ========================================
INSERT INTO tblPhongBan (MaPB, TenPB, DiaChi, SoDienThoai, Ghichu)
VALUES
('PB01', N'Phòng Kinh Doanh', N'123 ???ng A, Hà N?i', '0241234567', N'Ch?u trách nhi?m doanh s?'),
('PB02', N'Phòng Nhân S?', N'456 ???ng B, Hà N?i', '0242345678', N'Tuy?n d?ng & qu?n lý nhân s?'),
('PB03', N'Phòng IT', N'789 ???ng C, Hà N?i', '0243456789', N'Qu?n tr? h? th?ng & ph?n m?m'),
('PB04', N'Phòng Marketing', N'101 ???ng D, Hà N?i', '0244567890', N'Qu?ng bá s?n ph?m'),
('PB05', N'Phòng K? Toán', N'202 ???ng E, Hà N?i', '0245678901', N'Qu?n lý tài chính'),
('PB06', N'Phòng R&D', N'303 ???ng F, Hà N?i', '0246789012', N'Nghiên c?u và phát tri?n'),
('PB07', N'Phòng Hành Chính', N'404 ???ng G, Hà N?i', '0247890123', N'Qu?n lý v?n phòng'),
('PB08', N'Phòng D?ch V? Khách Hàng', N'505 ???ng H, Hà N?i', '0248901234', N'H? tr? khách hàng'),
('PB09', N'Phòng S?n Xu?t', N'606 ???ng I, Hà N?i', '0249012345', N'Qu?n lý s?n xu?t'),
('PB10', N'Phòng Logistics', N'707 ???ng J, Hà N?i', '0240123456', N'V?n chuy?n & kho bãi');

-- ========================================
-- ============ Thêm d? li?u Ch?c V? =======
-- ========================================
INSERT INTO tblChucVu (MaCV, TenCV, Ghichu)
VALUES
('CV01', N'Tr??ng Phòng', N'Qu?n lý phòng ban'),
('CV02', N'Phó Phòng', N'H? tr? tr??ng phòng'),
('CV03', N'Nhân Viên', N'Nhân viên làm vi?c tr?c ti?p'),
('CV04', N'Th?c T?p Sinh', N'H?c vi?c & h? tr?'),
('CV05', N'Giám ??c', N'Lãnh ??o công ty'),
('CV06', N'Chuyên Viên', N'Chuyên môn cao'),
('CV07', N'K? Thu?t Viên', N'Th?c hi?n k? thu?t'),
('CV08', N'T? V?n', N'H? tr? khách hàng'),
('CV09', N'Nhân Viên Hành Chính', N'Qu?n lý v?n phòng'),
('CV10', N'Nhân Viên K? Toán', N'Tính toán tài chính');

-- ========================================
-- ============ Thêm d? li?u Nhân Viên =====
-- ========================================
INSERT INTO tblNhanVien (MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaPB, MaCV, Ghichu)
VALUES
('NV01', N'Nguy?n V?n A', '1990-01-15', N'Nam', N'Hà N?i', '0901000001', 'a.nguyen@example.com', 'PB01', 'CV03', N'Nhân viên kinh doanh'),
('NV02', N'Tr?n Th? B', '1992-02-20', N'N?', N'Hà N?i', '0901000002', 'b.tran@example.com', 'PB02', 'CV03', N'Nhân s?'),
('NV03', N'Ph?m V?n C', '1988-03-10', N'Nam', N'Hà N?i', '0901000003', 'c.pham@example.com', 'PB03', 'CV06', N'IT chuyên môn cao'),
('NV04', N'Ngô Th? D', '1995-04-25', N'N?', N'Hà N?i', '0901000004', 'd.ngo@example.com', 'PB04', 'CV03', N'Marketing'),
('NV05', N'Lê V?n E', '1991-05-05', N'Nam', N'Hà N?i', '0901000005', 'e.le@example.com', 'PB05', 'CV10', N'K? toán'),
('NV06', N'Hoàng Th? F', '1993-06-12', N'N?', N'Hà N?i', '0901000006', 'f.hoang@example.com', 'PB06', 'CV06', N'Nghiên c?u & phát tri?n'),
('NV07', N'??ng V?n G', '1989-07-18', N'Nam', N'Hà N?i', '0901000007', 'g.dang@example.com', 'PB07', 'CV09', N'Hành chính'),
('NV08', N'V? Th? H', '1994-08-22', N'N?', N'Hà N?i', '0901000008', 'h.vu@example.com', 'PB08', 'CV08', N'T? v?n khách hàng'),
('NV09', N'Phan V?n I', '1990-09-30', N'Nam', N'Hà N?i', '0901000009', 'i.phan@example.com', 'PB09', 'CV07', N'S?n xu?t'),
('NV10', N'Bùi Th? K', '1992-10-11', N'N?', N'Hà N?i', '0901000010', 'k.bui@example.com', 'PB10', 'CV03', N'Logistics');

-- ========================================
-- ============ Thêm d? li?u H?p ??ng =====
-- ========================================
INSERT INTO tblHopDong (MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu)
VALUES
('HD01', 'NV01', '2023-01-01', '2023-12-31', N'Chính th?c', 15000000, N'H?p ??ng 1 n?m'),
('HD02', 'NV02', '2023-01-01', '2023-06-30', N'Th? vi?c', 12000000, N'H?p ??ng th? vi?c'),
('HD03', 'NV03', '2023-02-01', '2023-12-31', N'Chính th?c', 20000000, N'H?p ??ng IT'),
('HD04', 'NV04', '2023-03-01', '2023-12-31', N'Chính th?c', 14000000, N'Marketing'),
('HD05', 'NV05', '2023-01-01', '2023-12-31', N'Chính th?c', 13000000, N'K? toán'),
('HD06', 'NV06', '2023-04-01', '2023-12-31', N'Chính th?c', 18000000, N'R&D'),
('HD07', 'NV07', '2023-05-01', '2023-12-31', N'Chính th?c', 12500000, N'Hành chính'),
('HD08', 'NV08', '2023-06-01', '2023-12-31', N'Chính th?c', 13500000, N'T? v?n'),
('HD09', 'NV09', '2023-01-01', '2023-12-31', N'Chính th?c', 16000000, N'S?n xu?t'),
('HD10', 'NV10', '2023-07-01', '2023-12-31', N'Chính th?c', 14500000, N'Logistics');

-- ========================================
-- ============ Thêm d? li?u L??ng =====
-- ========================================
DECLARE @i INT = 1
WHILE @i <= 10
BEGIN
    DECLARE @thang INT = 1
    WHILE @thang <= 12
    BEGIN
        INSERT INTO tblLuong (MaLuong, MaNV, Thang, Nam, LuongCoBan, SoNgayCong, PhuCap, KhauTru, Ghichu)
        VALUES (
            CONCAT('L', RIGHT('00' + CAST(@i AS NVARCHAR),2), RIGHT('00' + CAST(@thang AS NVARCHAR),2)),
            CONCAT('NV', RIGHT('00' + CAST(@i AS NVARCHAR),2)),
            @thang,
            2023,
            15000000 + @i*1000000,  -- t?ng l??ng theo nhân viên
            22,
            2000000,  -- ph? c?p
            500000,   -- kh?u tr?
            N'L??ng tháng ' + CAST(@thang AS NVARCHAR)
        );
        SET @thang = @thang + 1
    END
    SET @i = @i + 1
END

-- ========================================
-- ============ Thêm d? li?u D? Án =====
-- ========================================
INSERT INTO tblDuAn (MaDA, TenDA, MoTa, NgayBatDau, NgayKetThuc, Ghichu)
VALUES
('DA01', N'D? án Alpha', N'Xây d?ng ph?n m?m Alpha', '2023-01-01', '2023-06-30', N'D? án ?u tiên'),
('DA02', N'D? án Beta', N'Phát tri?n ?ng d?ng Beta', '2023-02-01', '2023-07-31', N''),
('DA03', N'D? án Gamma', N'D? án nghiên c?u Gamma', '2023-03-01', '2023-09-30', N''),
('DA04', N'D? án Delta', N'D? án marketing Delta', '2023-04-01', '2023-12-31', N''),
('DA05', N'D? án Epsilon', N'D? án s?n xu?t Epsilon', '2023-05-01', '2023-11-30', N''),
('DA06', N'D? án Zeta', N'?ng d?ng n?i b? Zeta', '2023-01-15', '2023-12-31', N''),
('DA07', N'D? án Eta', N'T? v?n Eta', '2023-06-01', '2023-12-31', N''),
('DA08', N'D? án Theta', N'Ph?n m?m Theta', '2023-03-01', '2023-10-31', N''),
('DA09', N'D? án Iota', N'Nghiên c?u Iota', '2023-01-01', '2023-12-31', N''),
('DA10', N'D? án Kappa', N'D? án Kappa t?ng h?p', '2023-02-01', '2023-12-31', N'');

-- ========================================
-- ============ Thêm d? li?u Chi Ti?t D? Án =====
-- ========================================
INSERT INTO tblChiTietDuAn (MaNV, MaDA, VaiTro, Ghichu)
VALUES
('NV01', 'DA01', N'Tr??ng nhóm', ''),
('NV02', 'DA01', N'Nhân viên', ''),
('NV03', 'DA02', N'Tr??ng nhóm', ''),
('NV04', 'DA03', N'Nhân viên', ''),
('NV05', 'DA04', N'Nhân viên', ''),
('NV06', 'DA05', N'Tr??ng nhóm', ''),
('NV07', 'DA06', N'Nhân viên', ''),
('NV08', 'DA07', N'Nhân viên', ''),
('NV09', 'DA08', N'Tr??ng nhóm', ''),
('NV10', 'DA09', N'Nhân viên', '');

-- ========================================
-- ============ Thêm d? li?u Ch?m Công =====
-- ========================================
DECLARE @d INT = 1
WHILE @d <= 10
BEGIN
    DECLARE @m INT = 1
    WHILE @m <= 12
    BEGIN
        DECLARE @day INT = 1
        WHILE @day <= 20  -- gi? l?p 20 ngày công/tháng
        BEGIN
            INSERT INTO tblChamCong (MaChamCong, MaNV, Ngay, GioVao, GioVe, Ghichu)
            VALUES (
                CONCAT('CC', RIGHT('00'+CAST(@d AS NVARCHAR),2), RIGHT('00'+CAST(@m AS NVARCHAR),2), RIGHT('00'+CAST(@day AS NVARCHAR),2)),
                CONCAT('NV', RIGHT('00'+CAST(@d AS NVARCHAR),2)),
                DATEFROMPARTS(2023,@m,@day),
                '08:00',
                '17:00',
                N'Ch?m công'
            )
            SET @day = @day + 1
        END
        SET @m = @m + 1
    END
    SET @d = @d + 1
END

-- ========================================
-- ============ Thêm d? li?u Tài Kho?n =====
-- ========================================
INSERT INTO tblTaiKhoan (MaTK, MaNV, SoDienThoai, MatKhau, Quyen, Ghichu)
VALUES
('TK01', 'NV01', '0901000001', '123456', 'Admin', ''),
('TK02', 'NV02', '0901000002', '123456', 'User', ''),
('TK03', 'NV03', '0901000003', '123456', 'User', ''),
('TK04', 'NV04', '0901000004', '123456', 'User', ''),
('TK05', 'NV05', '0901000005', '123456', 'User', ''),
('TK06', 'NV06', '0901000006', '123456', 'User', ''),
('TK07', 'NV07', '0901000007', '123456', 'User', ''),
('TK08', 'NV08', '0901000008', '123456', 'User', ''),
('TK09', 'NV09', '0901000009', '123456', 'User', ''),
('TK10', 'NV10', '0901000010', '123456', 'User', '');
