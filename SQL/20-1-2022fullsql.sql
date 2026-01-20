/* =========================
   1) CREATE DATABASE
========================= */
IF DB_ID(N'QuanLyNhanSu') IS NULL
    CREATE DATABASE QuanLyNhanVien_Nhom12;
GO
USE QuanLyNhanVien_Nhom12;
GO

/* =========================
   2) DROP TABLE (n?u ?ã có)
========================= */
IF OBJECT_ID('dbo.tblLuong', 'U') IS NOT NULL DROP TABLE dbo.tblLuong;
IF OBJECT_ID('dbo.tblChamCong', 'U') IS NOT NULL DROP TABLE dbo.tblChamCong;
IF OBJECT_ID('dbo.tblHopDong', 'U') IS NOT NULL DROP TABLE dbo.tblHopDong;
IF OBJECT_ID('dbo.tblChiTietDuAn', 'U') IS NOT NULL DROP TABLE dbo.tblChiTietDuAn;
IF OBJECT_ID('dbo.tblDuAn', 'U') IS NOT NULL DROP TABLE dbo.tblDuAn;
IF OBJECT_ID('dbo.tblTaiKhoan', 'U') IS NOT NULL DROP TABLE dbo.tblTaiKhoan;
IF OBJECT_ID('dbo.tblNhanVien', 'U') IS NOT NULL DROP TABLE dbo.tblNhanVien;
IF OBJECT_ID('dbo.tblChucVu', 'U') IS NOT NULL DROP TABLE dbo.tblChucVu;
IF OBJECT_ID('dbo.tblPhongBan', 'U') IS NOT NULL DROP TABLE dbo.tblPhongBan;
IF OBJECT_ID('dbo.tblRole', 'U') IS NOT NULL DROP TABLE dbo.tblRole;
GO

/* =========================
   3) CREATE TABLES
========================= */

CREATE TABLE tblRole (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    TenRole NVARCHAR(50)  NOT NULL,
    MoTa    NVARCHAR(200) NULL
);
GO

CREATE TABLE tblPhongBan (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaPB        VARCHAR(10)     NOT NULL,
    TenPB       NVARCHAR(100)   NOT NULL,
    DiaChi      NVARCHAR(200)   NULL,
    SoDienThoai VARCHAR(20)     NULL,
    Ghichu      NVARCHAR(255)   NULL,
    DeletedAt   INT             NOT NULL
);
GO

CREATE TABLE  tblChucVu (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    MaCV      VARCHAR(10)     NOT NULL,
    TenCV     NVARCHAR(100)   NOT NULL,
    Ghichu    NVARCHAR(255)   NULL,
    DeletedAt INT             NOT NULL,
    MaPB      VARCHAR(10)     NOT NULL
);
GO

CREATE TABLE  tblNhanVien (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaNV        VARCHAR(10)     NOT NULL,
    HoTen       NVARCHAR(100)   NOT NULL,
    NgaySinh    DATE            NULL,
    GioiTinh    NVARCHAR(10)    NULL,
    DiaChi      NVARCHAR(200)   NULL,
    SoDienThoai VARCHAR(20)     NULL,
    Email       VARCHAR(100)    NULL,
    MaCV        VARCHAR(10)     NOT NULL,
    Ghichu      NVARCHAR(255)   NULL,
    DeletedAt   INT             NOT NULL
);
GO

CREATE TABLE  tblTaiKhoan (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaTK        VARCHAR(10)     NOT NULL,
    MaNV        VARCHAR(10)     NOT NULL,
    SoDienThoai VARCHAR(50)     NOT NULL,
    MatKhau     VARCHAR(255)    NOT NULL,
    Quyen       NVARCHAR(50)    NULL,
    Ghichu      NVARCHAR(255)   NULL,
    DeletedAt   INT             NOT NULL,
    RoleId      INT             NOT NULL
);
GO

CREATE TABLE  tblDuAn (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaDA        VARCHAR(10)     NOT NULL,
    TenDA       NVARCHAR(200)   NOT NULL,
    MoTa        NVARCHAR(500)   NULL,
    NgayBatDau  DATE            NULL,
    NgayKetThuc DATE            NULL,
    Ghichu      NVARCHAR(255)   NULL,
    DeletedAt   INT             NOT NULL
);
GO

CREATE TABLE  tblChiTietDuAn (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    MaNV      VARCHAR(10)      NOT NULL,
    MaDA      VARCHAR(10)      NOT NULL,
    VaiTro    NVARCHAR(100)    NULL,
    Ghichu    NVARCHAR(255)    NULL,
    DeletedAt INT              NOT NULL
);
GO

CREATE TABLE  tblHopDong (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaHopDong   VARCHAR(10)     NOT NULL,
    MaNV        VARCHAR(10)     NOT NULL,
    NgayBatDau  DATE            NOT NULL,
    NgayKetThuc DATE            NULL,
    LoaiHopDong NVARCHAR(50)    NULL,
    LuongCoBan  DECIMAL(18,2)   NOT NULL,
    Ghichu      NVARCHAR(255)   NULL,
    DeletedAt   INT             NOT NULL
);
GO

CREATE TABLE  tblChamCong (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    MaChamCong VARCHAR(20)     NOT NULL,
    Ngay       DATE            NOT NULL,
    GioVao     TIME(7)         NOT NULL,
    GioVe      TIME(7)         NOT NULL,
    Ghichu     NVARCHAR(255)   NULL,
    DeletedAt  INT             NOT NULL,
    NhanVienId INT             NOT NULL
);
GO

CREATE TABLE  tblLuong (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    MaLuong         VARCHAR(10)      NOT NULL,
    Thang           INT              NULL,
    Nam             INT              NOT NULL,
    LuongCoBan      DECIMAL(18,2)    NOT NULL,
    SoNgayCongChuan INT              NOT NULL,
    PhuCap          DECIMAL(18,2)    NULL,
    KhauTru         DECIMAL(18,2)    NULL,
    Ghichu          NVARCHAR(255)    NULL,
    DeletedAt       INT              NOT NULL,
    ChamCongId      INT              NULL
);
GO

/* =========================
   4) FOREIGN KEYS (theo s? ??)
========================= */

-- tblChucVu -> tblPhongBan (MaPB)
ALTER TABLE dbo.tblPhongBan
ADD CONSTRAINT UQ_tblPhongBan_MaPB UNIQUE (MaPB);

ALTER TABLE dbo.tblChucVu
ADD CONSTRAINT UQ_tblChucVu_MaCV UNIQUE (MaCV);

ALTER TABLE dbo.tblNhanVien
ADD CONSTRAINT UQ_tblNhanVien_MaNV UNIQUE (MaNV);

ALTER TABLE dbo.tblDuAn
ADD CONSTRAINT UQ_tblDuAn_MaDA UNIQUE (MaDA);

ALTER TABLE dbo.tblChucVu
ADD CONSTRAINT FK_tblChucVu_tblPhongBan
FOREIGN KEY (MaPB) REFERENCES dbo.tblPhongBan(MaPB);

-- tblNhanVien -> tblChucVu (MaCV)
ALTER TABLE dbo.tblNhanVien
ADD CONSTRAINT FK_tblNhanVien_tblChucVu
FOREIGN KEY (MaCV) REFERENCES dbo.tblChucVu(MaCV);

-- tblTaiKhoan -> tblRole (RoleId)
ALTER TABLE dbo.tblTaiKhoan
ADD CONSTRAINT FK_tblTaiKhoan_tblRole
FOREIGN KEY (RoleId) REFERENCES dbo.tblRole(Id);

-- tblTaiKhoan -> tblNhanVien (MaNV)
ALTER TABLE dbo.tblTaiKhoan
ADD CONSTRAINT FK_tblTaiKhoan_tblNhanVien
FOREIGN KEY (MaNV) REFERENCES dbo.tblNhanVien(MaNV);

-- tblChiTietDuAn -> tblNhanVien, tblDuAn
ALTER TABLE dbo.tblChiTietDuAn
ADD CONSTRAINT FK_tblChiTietDuAn_tblNhanVien
FOREIGN KEY (MaNV) REFERENCES dbo.tblNhanVien(MaNV);

ALTER TABLE dbo.tblChiTietDuAn
ADD CONSTRAINT FK_tblChiTietDuAn_tblDuAn
FOREIGN KEY (MaDA) REFERENCES dbo.tblDuAn(MaDA);

-- tblHopDong -> tblNhanVien (MaNV)
ALTER TABLE dbo.tblHopDong
ADD CONSTRAINT FK_tblHopDong_tblNhanVien
FOREIGN KEY (MaNV) REFERENCES dbo.tblNhanVien(MaNV);

-- tblChamCong -> tblNhanVien (NhanVienId)
ALTER TABLE dbo.tblChamCong
ADD CONSTRAINT FK_tblChamCong_tblNhanVien
FOREIGN KEY (NhanVienId) REFERENCES dbo.tblNhanVien(Id);

-- tblLuong -> tblChamCong (ChamCongId)
ALTER TABLE dbo.tblLuong
ADD CONSTRAINT FK_tblLuong_tblChamCong
FOREIGN KEY (ChamCongId) REFERENCES dbo.tblChamCong(Id);
GO

/* =========================
   5) SAMPLE DATA
   - Có tên ng??i: chèn HoTen theo th? t? b?n yêu c?u
========================= */
-- T?T CHECK FK T?M TH?I
EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

DELETE FROM tblLuong;
DELETE FROM tblChamCong;
DELETE FROM tblHopDong;
DELETE FROM tblChiTietDuAn;
DELETE FROM tblDuAn;
DELETE FROM tblTaiKhoan;
DELETE FROM tblNhanVien;
DELETE FROM tblChucVu;
DELETE FROM tblPhongBan;
DELETE FROM tblRole;
GO

-- RESET IDENTITY
DBCC CHECKIDENT ('tblLuong', RESEED, 0);
DBCC CHECKIDENT ('tblChamCong', RESEED, 0);
DBCC CHECKIDENT ('tblHopDong', RESEED, 0);
DBCC CHECKIDENT ('tblChiTietDuAn', RESEED, 0);
DBCC CHECKIDENT ('tblDuAn', RESEED, 0);
DBCC CHECKIDENT ('tblTaiKhoan', RESEED, 0);
DBCC CHECKIDENT ('tblNhanVien', RESEED, 0);
DBCC CHECKIDENT ('tblChucVu', RESEED, 0);
DBCC CHECKIDENT ('tblPhongBan', RESEED, 0);
DBCC CHECKIDENT ('tblRole', RESEED, 0);
GO

-- B?T L?I FK
EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO


/* =====================================================
0) XÓA TOÀN B? D? LI?U (?úng th? t? FK)
===================================================== */
DELETE FROM dbo.tblLuong;
DELETE FROM dbo.tblChamCong;
DELETE FROM dbo.tblChiTietDuAn;
DELETE FROM dbo.tblHopDong;
DELETE FROM dbo.tblTaiKhoan;
DELETE FROM dbo.tblNhanVien;
DELETE FROM dbo.tblChucVu;
DELETE FROM dbo.tblPhongBan;
DELETE FROM dbo.tblDuAn;
DELETE FROM dbo.tblRole;
GO

/* =====================================================
1) ROLE
===================================================== */
INSERT INTO dbo.tblRole (TenRole, MoTa)
VALUES 
(N'Admin', N'Qu?n tr? h? th?ng'),
(N'Nhân s?', N'Qu?n lý nhân s?'),
(N'Nhân viên', N'Nhân viên th??ng');
GO

/* =====================================================
2) PHÒNG BAN (>=5)
===================================================== */
INSERT INTO dbo.tblPhongBan (MaPB, TenPB, DiaChi, SoDienThoai, Ghichu, DeletedAt)
VALUES
('PB01', N'Nhân s?',     N'Hà N?i', '090000001', NULL, 0),
('PB02', N'K? toán',     N'Hà N?i', '090000002', NULL, 0),
('PB03', N'CNTT',        N'Hà N?i', '090000003', NULL, 0),
('PB04', N'Kinh doanh',  N'Hà N?i', '090000004', NULL, 0),
('PB05', N'V?n hành',    N'Hà N?i', '090000005', NULL, 0);
GO

/* =====================================================
3) CH?C V? (m?i PB >=5) => t?ng 25 ch?c v?
===================================================== */
INSERT INTO dbo.tblChucVu (MaCV, TenCV, Ghichu, DeletedAt, MaPB)
VALUES
('CV01', N'Tr??ng phòng',         NULL, 0, 'PB01'),
('CV02', N'Phó phòng',            NULL, 0, 'PB01'),
('CV03', N'Chuyên viên nhân s?',  NULL, 0, 'PB01'),
('CV04', N'Nhân viên nhân s?',    NULL, 0, 'PB01'),
('CV05', N'Th?c t?p nhân s?',     NULL, 0, 'PB01'),

('CV06', N'Tr??ng phòng',         NULL, 0, 'PB02'),
('CV07', N'Phó phòng',            NULL, 0, 'PB02'),
('CV08', N'K? toán t?ng h?p',     NULL, 0, 'PB02'),
('CV09', N'K? toán viên',         NULL, 0, 'PB02'),
('CV10', N'Th?c t?p k? toán',     NULL, 0, 'PB02'),

('CV11', N'Tr??ng phòng',         NULL, 0, 'PB03'),
('CV12', N'Team Lead',            NULL, 0, 'PB03'),
('CV13', N'L?p trình viên',       NULL, 0, 'PB03'),
('CV14', N'Ki?m th? (QA)',        NULL, 0, 'PB03'),
('CV15', N'Th?c t?p IT',          NULL, 0, 'PB03'),

('CV16', N'Tr??ng phòng',         NULL, 0, 'PB04'),
('CV17', N'Phó phòng',            NULL, 0, 'PB04'),
('CV18', N'Nhân viên Sales',      NULL, 0, 'PB04'),
('CV19', N'Marketing',            NULL, 0, 'PB04'),
('CV20', N'Th?c t?p KD/MKT',      NULL, 0, 'PB04'),

('CV21', N'Tr??ng phòng',         NULL, 0, 'PB05'),
('CV22', N'Giám sát v?n hành',    NULL, 0, 'PB05'),
('CV23', N'Nhân viên v?n hành',   NULL, 0, 'PB05'),
('CV24', N'Kho v?n',              NULL, 0, 'PB05'),
('CV25', N'Th?c t?p v?n hành',    NULL, 0, 'PB05');
GO

/* =====================================================
4) D? ÁN (thêm nhi?u ?? chia ??u)
===================================================== */
INSERT INTO dbo.tblDuAn (MaDA, TenDA, MoTa, NgayBatDau, NgayKetThuc, Ghichu, DeletedAt)
VALUES
('DA01', N'ERP N?i b?',            NULL, '2025-01-01', '2025-12-31', NULL, 0),
('DA02', N'Website bán hàng',      NULL, '2025-02-01', '2025-11-30', NULL, 0),
('DA03', N'?ng d?ng ch?m công',    NULL, '2025-03-01', '2025-10-31', NULL, 0),
('DA04', N'Data Warehouse',        NULL, '2025-04-01', '2025-12-15', NULL, 0),
('DA05', N'CRM Kinh doanh',        NULL, '2025-05-01', '2025-12-20', NULL, 0);
GO

/* =====================================================
5) NHÂN VIÊN: ?? THEO DANH SÁCH B?N ??A (t? chia ??u MaCV)
   - ?u tiên ??a các tên: V? Minh Khang, Hoàng Tu?n Anh, Tr?n ??ng Chi?n, Cao Nhân Thu?n lên ??u n?u có trong list
===================================================== */
DECLARE @Emp TABLE (HoTen NVARCHAR(100) NOT NULL, NgaySinh DATE NOT NULL);

INSERT INTO @Emp (HoTen, NgaySinh) VALUES
(N'Lê Tu?n Anh','2005-09-29'),
(N'Nguy?n Nh?t Anh','2005-07-20'),
(N'Ph?m Hoàng B?c','2005-12-15'),
(N'Lô V?n Công','2005-07-25'),
(N'?oàn V?n C??ng','2004-10-22'),
(N'V? Duy Doanh','2005-10-19'),
(N'Hoàng Ng?c Hùng D?ng','2005-01-20'),
(N'?ào H?i D??ng','2005-03-07'),
(N'Tr?n Vi?t D??ng','2005-02-07'),
(N'Nguy?n ?ình ??ng','2005-11-08'),
(N'Hoàng Anh ??c','2005-10-03'),
(N'Ki?u Trung Giáp','2005-08-18'),
(N'Tr?n Hoàng H?i','2005-07-22'),
(N'V??ng Nh?t Hào','2005-11-30'),
(N'?oàn Trung Hi?u','2005-03-11'),
(N'?? Trung Hi?u','2005-12-30'),
(N'Ph?m Huy Hi?u','2005-02-09'),
(N'Nguy?n Xuân Hoàn','2005-09-20'),
(N'??ng ?ình Hùng','2002-07-30'),
(N'Nguy?n Xuân Hùng','2005-03-09'),
(N'Nguy?n Quang Huy','2005-08-08'),
(N'Nguy?n Quang Huy','2005-06-11'),
(N'Nguy?n Th? H?ng','2005-05-19'),
(N'V? Qu?c Khánh','2005-09-01'),
(N'Nguy?n Hà Linh','2005-03-07'),
(N'Tr?nh Xuân L?c','2005-11-05'),
(N'Nguy?n V?n L?u','2005-06-06'),
(N'Nguy?n Ph??ng Nam','2005-04-29'),
(N'Ph?m V?n Nam','2005-12-02'),
(N'Nguy?n Tr?ng Ngh?a','2004-05-24'),
(N'Ngô V?n Quang','2005-02-12'),
(N'Nguy?n S? Quân','2005-06-29'),
(N'Tr?n Phú Quân','2005-01-16'),
(N'V? Quang Sang','2005-03-09'),
(N'Ph?m Minh S?n','2005-12-22'),
(N'Nguy?n C? Thái','2005-02-10'),
(N'Lê ??c Thanh','2005-02-25'),
(N'Nguy?n V?n Thành','2005-10-03'),
(N'Phùng Ti?n Thành','2005-10-07'),
(N'?? M?nh Th?ng','2005-05-24'),
(N'Nguy?n V?n Th??ng','2005-11-15'),
(N'T?ng Xuân Toàn','2005-11-23'),
(N'?? Anh Tú','2005-09-03'),
(N'Nguy?n Nh? Tùng','2005-07-12'),
(N'Nguy?n Quang Tùng','2005-07-28'),
(N'C?n Xuân Vi?t','2005-11-05'),
(N'Phi H?ng V??ng','2003-06-16'),

(N'Hoàng Tu?n Anh','2003-01-21'),
(N'Nguy?n H?i Anh','2005-09-24'),
(N'Nguy?n Qu?c Anh','2005-11-06'),
(N'Nguy?n Vi?t Anh','2005-02-08'),
(N'Tr??ng Nguy?t Anh','2004-10-17'),
(N'Nguy?n ?ình Chi?n','2003-11-05'),
(N'Tr?n ??ng Chi?n','2005-08-23'),
(N'?? Thành Công','2005-10-16'),
(N'Lê V?n D?ng','2002-07-02'),
(N'L?u Minh D?ng','2005-12-15'),
(N'Tr?n Qu?c D?ng','2005-11-20'),
(N'Hoàng ??ng D??ng','2005-10-20'),
(N'Nguy?n Ti?n ??t','2005-12-14'),
(N'Tr?n Tu?n ??t','2005-06-23'),
(N'Nguy?n Tr?ng ?i?n','2005-07-26'),
(N'Lê Minh H?i','2005-11-06'),
(N'L?u Ng?c H?i','2005-06-06'),
(N'Nguy?n ??c H?u','2005-08-20'),
(N'Nguy?n Xuân Hi?n','2003-07-22'),
(N'?? ?ình Hi?p','2005-05-25'),
(N'Nguy?n Công Hòa','2005-11-19'),
(N'Nguy?n Xuân Hoàng','2005-08-27'),
(N'Phan H?u Huy Hoàng','2005-09-25'),
(N'Hà ??ng H?p','2005-07-12'),
(N'Mai Chí Hùng','2005-02-24'),
(N'Nguy?n Quang Hùng','2004-11-09'),
(N'Nguy?n Thanh Hùng','2005-02-26'),
(N'?? Xuân Huy','2005-05-02'),
(N'Tr?n Quang Huy','2004-08-19'),
(N'V? Minh Khang','2005-05-23'),
(N'Bùi Quang Khoa','2005-09-16'),
(N'Chu V?n Khoa','2005-09-10'),
(N'Ngô Trung Kiên','2005-08-02'),
(N'Nguy?n Trung Kiên','2005-03-17'),
(N'Nguy?n Trung Kiên','2005-03-13'),
(N'Nguy?n Thành L?c','2005-06-16'),
(N'Nguy?n Tr??ng Lu?n','2005-11-17'),
(N'Lê Quang Minh','2005-03-10'),
(N'Tr?n T?t Minh','2001-04-28'),
(N'M?ch Quý Nhân','2001-01-01'),
(N'Nguy?n H?u Pháp','2005-01-25'),
(N'Nguy?n Thanh Phong','2005-06-02'),
(N'Tr?n ?ình Phúc','2005-02-16'),
(N'Nguy?n V?n Quy?n','2004-08-22'),
(N'Hà M?nh Thái S?n','2004-09-18'),
(N'Nguy?n Minh Tâm','2005-10-10'),
(N'Nguy?n V?n Thành','2005-10-19'),
(N'Nguy?n V?n Th?ng','2005-01-11'),
(N'Nguy?n V?n Thông','2004-04-18'),
(N'Cao Nhân Thu?n','2005-03-17'),
(N'V? Th? Thanh Thuý','2005-04-23'),
(N'Ph?m V?n Toàn','2005-03-06'),
(N'D??ng Anh Tu?n','2005-01-08'),
(N'Nguy?n V?n Tu?n','2003-02-21'),
(N'Nguy?n Duy Tùng','2005-02-25'),
(N'Nguy?n Qu?c Vi?t','2005-12-10'),
(N'Lê Minh V??ng','2005-12-06'),
(N'Nguy?n Th?nh V??ng','2005-03-13');

DECLARE @CV TABLE (rn INT PRIMARY KEY, MaCV VARCHAR(10) NOT NULL);
;WITH x AS (SELECT ROW_NUMBER() OVER (ORDER BY MaCV) rn, MaCV FROM dbo.tblChucVu)
INSERT INTO @CV(rn, MaCV) SELECT rn, MaCV FROM x;

;WITH E AS (
    SELECT 
        HoTen, NgaySinh,
        ROW_NUMBER() OVER (
            ORDER BY 
                CASE 
                    WHEN HoTen = N'V? Minh Khang' THEN 1
                    WHEN HoTen = N'Hoàng Tu?n Anh' THEN 2
                    WHEN HoTen = N'Tr?n ??ng Chi?n' THEN 3
                    WHEN HoTen = N'Cao Nhân Thu?n' THEN 4
                    ELSE 100
                END,
                HoTen, NgaySinh
        ) AS r
    FROM @Emp
),
M AS (
    SELECT 
        r,
        HoTen,
        NgaySinh,
        'NV' + RIGHT('000' + CAST(r AS VARCHAR(10)), 3) AS MaNV,
        ( (r - 1) % 25 ) + 1 AS cv_rn
    FROM E
)
INSERT INTO dbo.tblNhanVien
(MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaCV, Ghichu, DeletedAt)
SELECT
    m.MaNV,
    m.HoTen,
    m.NgaySinh,
    CASE 
        WHEN m.HoTen LIKE N'%Th?%' OR m.HoTen LIKE N'%Nguy?t%' THEN N'N?'
        ELSE N'Nam'
    END AS GioiTinh,
    N'Hà N?i' AS DiaChi,
    '091' + RIGHT('000000' + CAST(m.r AS VARCHAR(10)), 6) AS SoDienThoai,
    'nv' + CAST(m.r AS VARCHAR(10)) + '@mail.com' AS Email,
    cv.MaCV,
    NULL,
    0
FROM M m
JOIN @CV cv ON cv.rn = m.cv_rn;
GO

/* =====================================================
6) TÀI KHO?N (m?i NV 1 TK) + phân quy?n chia ??u (Admin/Nhân s?/Nhân viên)
===================================================== */
;WITH N AS (
    SELECT MaNV, SoDienThoai, ROW_NUMBER() OVER (ORDER BY MaNV) r
    FROM dbo.tblNhanVien
)
INSERT INTO dbo.tblTaiKhoan
(MaTK, MaNV, SoDienThoai, MatKhau, Quyen, Ghichu, DeletedAt, RoleId)
SELECT
    'TK' + MaNV,
    MaNV,
    SoDienThoai,
    CONVERT(VARCHAR(255), HASHBYTES('MD5','123456'),2),
    CASE WHEN (r % 20)=1 THEN N'Admin'
         WHEN (r % 10)=1 THEN N'Nhân s?'
         ELSE N'Nhân viên' END,
    NULL,
    0,
    CASE WHEN (r % 20)=1 THEN 1
         WHEN (r % 10)=1 THEN 2
         ELSE 3 END
FROM N;
GO

/* =====================================================
7) H?P ??NG (chia ??u lo?i H? + l??ng theo nhóm)
===================================================== */
;WITH N AS (
    SELECT MaNV, ROW_NUMBER() OVER (ORDER BY MaNV) r
    FROM dbo.tblNhanVien
)
INSERT INTO dbo.tblHopDong
(MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu, DeletedAt)
SELECT
    'HD' + MaNV,
    MaNV,
    '2025-01-01',
    '2025-12-31',
    CASE WHEN (r % 3)=0 THEN N'H?p ??ng 1 n?m'
         WHEN (r % 3)=1 THEN N'H?p ??ng 2 n?m'
         ELSE N'H?p ??ng th? vi?c' END,
    CASE WHEN (r % 5)=0 THEN 12000000
         WHEN (r % 5)=1 THEN 10000000
         WHEN (r % 5)=2 THEN 9000000
         WHEN (r % 5)=3 THEN 8000000
         ELSE 7000000 END,
    NULL,
    0
FROM N;
GO

/* =====================================================
8) CHI TI?T D? ÁN (chia ??u 5 d? án)
===================================================== */
;WITH N AS (
    SELECT MaNV, ROW_NUMBER() OVER (ORDER BY MaNV) r
    FROM dbo.tblNhanVien
),
D AS (
    SELECT r.MaNV,
           CASE ((r.r - 1) % 5) + 1
                WHEN 1 THEN 'DA01'
                WHEN 2 THEN 'DA02'
                WHEN 3 THEN 'DA03'
                WHEN 4 THEN 'DA04'
                ELSE 'DA05'
           END AS MaDA,
           r.r
    FROM N r
)
INSERT INTO dbo.tblChiTietDuAn (MaNV, MaDA, VaiTro, Ghichu, DeletedAt)
SELECT
    MaNV,
    MaDA,
    CASE WHEN (r % 25)=1 THEN N'Qu?n lý d? án'
         WHEN (r % 7)=0 THEN N'Leader'
         ELSE N'Thành viên' END,
    NULL,
    0
FROM D;
GO

/* =====================================================
9) CH?M CÔNG: ?? 12 THÁNG N?M 2025, M?I NGÀY M?I NV ??U CÓ
   - T?o d? li?u theo ngày: 2025-01-01 -> 2025-12-31
===================================================== */
DECLARE @Ngay DATE = '2025-01-01';
WHILE @Ngay <= '2025-12-31'
BEGIN
    INSERT INTO dbo.tblChamCong
    (MaChamCong, Ngay, GioVao, GioVe, Ghichu, DeletedAt, NhanVienId)
    SELECT
        'CC' + RIGHT('000000' + CAST(nv.Id AS VARCHAR(10)), 6) + '_' + CONVERT(VARCHAR(8), @Ngay, 112),
        @Ngay,
        CASE WHEN (nv.Id % 4)=0 THEN '07:45' WHEN (nv.Id % 4)=1 THEN '08:00' WHEN (nv.Id % 4)=2 THEN '08:10' ELSE '08:05' END,
        CASE WHEN (nv.Id % 4)=0 THEN '17:00' WHEN (nv.Id % 4)=1 THEN '17:15' WHEN (nv.Id % 4)=2 THEN '17:30' ELSE '17:05' END,
        CASE WHEN (nv.Id % 13)=0 THEN N'?i công tác' WHEN (nv.Id % 17)=0 THEN N'T?ng ca' ELSE NULL END,
        0,
        nv.Id
    FROM dbo.tblNhanVien nv;

    SET @Ngay = DATEADD(DAY, 1, @Ngay);
END
GO

/* =====================================================
10) L??NG: ?? N?M 2025 (m?i NV m?i THÁNG 1 b?n ghi)
   - ChamCongId: l?y 1 b?n ghi ch?m công ??i di?n trong tháng (MIN)
   - SoNgayCongChuan: 26
===================================================== */
;WITH CC AS (
    SELECT 
        cc.NhanVienId,
        MONTH(cc.Ngay) AS Thang,
        YEAR(cc.Ngay)  AS Nam,
        MIN(cc.Id) AS ChamCongId,
        COUNT(*)  AS SoNgayCongThucTe
    FROM dbo.tblChamCong cc
    WHERE cc.Ngay >= '2025-01-01' AND cc.Ngay <= '2025-12-31'
    GROUP BY cc.NhanVienId, MONTH(cc.Ngay), YEAR(cc.Ngay)
),
NV AS (
    SELECT nv.Id AS NhanVienId, nv.MaNV, hd.LuongCoBan
    FROM dbo.tblNhanVien nv
    JOIN dbo.tblHopDong hd ON hd.MaNV = nv.MaNV
)
INSERT INTO dbo.tblLuong
(MaLuong, Thang, Nam, LuongCoBan, SoNgayCongChuan, PhuCap, KhauTru, Ghichu, DeletedAt, ChamCongId)
SELECT
    'LG' + nv.MaNV + RIGHT('00' + CAST(cc.Thang AS VARCHAR(2)),2),
    cc.Thang,
    2025,
    nv.LuongCoBan,
    26,
    CASE WHEN (cc.Thang IN (1,12)) THEN 800000
         WHEN (cc.Thang IN (6,7,8)) THEN 500000
         ELSE 300000 END,
    CASE WHEN (cc.SoNgayCongThucTe < 26) THEN (26 - cc.SoNgayCongThucTe) * 150000 ELSE 0 END,
    NULL,
    0,
    cc.ChamCongId
FROM CC cc
JOIN NV nv ON nv.NhanVienId = cc.NhanVienId;
GO
