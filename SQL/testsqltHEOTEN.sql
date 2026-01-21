
CREATE DATABASE QuanLyNhanVien_Nhom14;
GO
USE QuanLyNhanVien_Nhom14;
GO


CREATE TABLE tblRole_ThuanCD233318 (
    Id_ThuanCD233318      INT IDENTITY(1,1) PRIMARY KEY,
    TenRole_ThuanCD233318 NVARCHAR(50)  NOT NULL,
    MoTa_ThuanCD233318    NVARCHAR(200) NULL
);
GO
CREATE TABLE tblPhongBan_ThuanCD233318 (
    Id_ThuanCD233318          INT IDENTITY(1,1) PRIMARY KEY,
    MaPB_ThuanCD233318        VARCHAR(10)     NOT NULL,
    TenPB_ThuanCD233318       NVARCHAR(100)   NOT NULL,
    DiaChi_ThuanCD233318      NVARCHAR(200)   NULL,
    SoDienThoai_ThuanCD233318 VARCHAR(20)     NULL,
    Ghichu_ThuanCD233318      NVARCHAR(255)   NULL,
    DeletedAt_ThuanCD233318   INT             NOT NULL
);
GO
CREATE TABLE tblChucVu_KhangCD233181 (
    Id_KhangCD233181        INT IDENTITY(1,1) PRIMARY KEY,
    MaCV_KhangCD233181      VARCHAR(10)     NOT NULL,
    TenCV_KhangCD233181     NVARCHAR(100)   NOT NULL,
    Ghichu_KhangCD233181    NVARCHAR(255)   NULL,
    DeletedAt_KhangCD233181 INT             NOT NULL,
    MaPB_ThuanCD233318      VARCHAR(10)     NOT NULL
);
GO

CREATE TABLE tblNhanVien_TuanhCD233018 (
    Id_TuanhCD233018          INT IDENTITY(1,1) PRIMARY KEY,
    MaNV_TuanhCD233018        VARCHAR(10)     NOT NULL,
    HoTen_TuanhCD233018       NVARCHAR(100)   NOT NULL,
    NgaySinh_TuanhCD233018    DATE            NULL,
    GioiTinh_TuanhCD233018    NVARCHAR(10)    NULL,
    DiaChi_TuanhCD233018      NVARCHAR(200)   NULL,
    SoDienThoai_TuanhCD233018 VARCHAR(20)     NULL,
    Email_TuanhCD233018       VARCHAR(100)    NULL,
    MaCV_KhangCD233181        VARCHAR(10)     NOT NULL,
    Ghichu_TuanhCD233018      NVARCHAR(255)   NULL,
    DeletedAt_TuanhCD233018   INT             NOT NULL
);
GO
CREATE TABLE tblTaiKhoan_KhangCD233181 (
    Id_KhangCD233181          INT IDENTITY(1,1) PRIMARY KEY,
    MaTK_KhangCD233181        VARCHAR(10)     NOT NULL,
    MaNV_TuanhCD233018        VARCHAR(10)     NOT NULL,
    SoDienThoai_KhangCD233181 VARCHAR(50)     NOT NULL,
    MatKhau_KhangCD233181     VARCHAR(255)    NOT NULL,
    Quyen_KhangCD233181       NVARCHAR(50)    NULL,
    Ghichu_KhangCD233181      NVARCHAR(255)   NULL,
    DeletedAt_KhangCD233181   INT             NOT NULL,
    RoleId_ThuanCD233318      INT             NOT NULL
);
GO
CREATE TABLE tblDuAn_KienCD233824 (
    Id_KienCD233824        INT IDENTITY(1,1) PRIMARY KEY,
    MaDA_KienCD233824      VARCHAR(10)     NOT NULL,
    TenDA_KienCD233824     NVARCHAR(200)   NOT NULL,
    MoTa_KienCD233824      NVARCHAR(500)   NULL,
    NgayBatDau_KienCD233824 DATE           NULL,
    NgayKetThuc_KienCD233824 DATE          NULL,
    Ghichu_KienCD233824    NVARCHAR(255)   NULL,
    DeletedAt_KienCD233824 INT             NOT NULL
);
GO
CREATE TABLE tblChiTietDuAn_KienCD233824 (
    Id_KienCD233824        INT IDENTITY(1,1) PRIMARY KEY,
    MaNV_TuanhCD233018     VARCHAR(10)     NOT NULL,
    MaDA_KienCD233824      VARCHAR(10)     NOT NULL,
    VaiTro_KienCD233824    NVARCHAR(100)   NULL,
    Ghichu_KienCD233824    NVARCHAR(255)   NULL,
    DeletedAt_KienCD233824 INT             NOT NULL
);
GO
CREATE TABLE tblHopDong_ChienCD232928 (
    Id_ChienCD232928        INT IDENTITY(1,1) PRIMARY KEY,
    MaHopDong_ChienCD232928 VARCHAR(10)     NOT NULL,
    MaNV_TuanhCD233018      VARCHAR(10)     NOT NULL,
    NgayBatDau_ChienCD232928 DATE           NOT NULL,
    NgayKetThuc_ChienCD232928 DATE          NULL,
    LoaiHopDong_ChienCD232928 NVARCHAR(50)  NULL,
    LuongCoBan_ChienCD232928 DECIMAL(18,2)  NOT NULL,
    Ghichu_ChienCD232928    NVARCHAR(255)   NULL,
    DeletedAt_ChienCD232928 INT             NOT NULL
);
GO
CREATE TABLE tblChamCong_TuanhCD233018 (
    Id_TuanhCD233018        INT IDENTITY(1,1) PRIMARY KEY,
    MaChamCong_TuanhCD233018 VARCHAR(20)    NOT NULL,
    Ngay_TuanhCD233018      DATE            NOT NULL,
    GioVao_TuanhCD233018    TIME(7)         NOT NULL,
    GioVe_TuanhCD233018     TIME(7)         NOT NULL,
    Ghichu_TuanhCD233018    NVARCHAR(255)   NULL,
    DeletedAt_TuanhCD233018 INT             NOT NULL,
    NhanVienId_TuanhCD233018 INT            NOT NULL
);
GO
CREATE TABLE tblLuong_ChienCD232928 (
    Id_ChienCD232928              INT IDENTITY(1,1) PRIMARY KEY,
    MaLuong_ChienCD232928         VARCHAR(10)      NOT NULL,
    Thang_ChienCD232928           INT              NULL,
    Nam_ChienCD232928             INT              NOT NULL,
    LuongCoBan_ChienCD232928      DECIMAL(18,2)    NOT NULL,
    SoNgayCongChuan_ChienCD232928 INT              NOT NULL,
    PhuCap_ChienCD232928          DECIMAL(18,2)    NULL,
    KhauTru_ChienCD232928         DECIMAL(18,2)    NULL,
    Ghichu_ChienCD232928          NVARCHAR(255)    NULL,
    DeletedAt_ChienCD232928       INT              NOT NULL,
    ChamCongId_TuanhCD233018      INT              NULL
);
GO

/* =========================
   UNIQUE CONSTRAINTS
========================= */

ALTER TABLE dbo.tblPhongBan_ThuanCD233318
ADD CONSTRAINT UQ_tblPhongBan_ThuanCD233318_MaPB
UNIQUE (MaPB_ThuanCD233318);

ALTER TABLE dbo.tblChucVu_KhangCD233181
ADD CONSTRAINT UQ_tblChucVu_KhangCD233181_MaCV
UNIQUE (MaCV_KhangCD233181);

ALTER TABLE dbo.tblNhanVien_TuanhCD233018
ADD CONSTRAINT UQ_tblNhanVien_TuanhCD233018_MaNV
UNIQUE (MaNV_TuanhCD233018);

ALTER TABLE dbo.tblDuAn_KienCD233824
ADD CONSTRAINT UQ_tblDuAn_KienCD233824_MaDA
UNIQUE (MaDA_KienCD233824);



/* =========================
   FOREIGN KEY CONSTRAINTS
========================= */

ALTER TABLE dbo.tblChucVu_KhangCD233181
ADD CONSTRAINT FK_tblChucVu_KhangCD233181_tblPhongBan_ThuanCD233318
FOREIGN KEY (MaPB_ThuanCD233318)
REFERENCES dbo.tblPhongBan_ThuanCD233318 (MaPB_ThuanCD233318);

ALTER TABLE dbo.tblNhanVien_TuanhCD233018
ADD CONSTRAINT FK_tblNhanVien_TuanhCD233018_tblChucVu_KhangCD233181
FOREIGN KEY (MaCV_KhangCD233181)
REFERENCES dbo.tblChucVu_KhangCD233181 (MaCV_KhangCD233181);

ALTER TABLE dbo.tblTaiKhoan_KhangCD233181
ADD CONSTRAINT FK_tblTaiKhoan_KhangCD233181_tblRole_ThuanCD233318
FOREIGN KEY (RoleId_ThuanCD233318)
REFERENCES dbo.tblRole_ThuanCD233318 (Id_ThuanCD233318);

ALTER TABLE dbo.tblTaiKhoan_KhangCD233181
ADD CONSTRAINT FK_tblTaiKhoan_KhangCD233181_tblNhanVien_TuanhCD233018
FOREIGN KEY (MaNV_TuanhCD233018)
REFERENCES dbo.tblNhanVien_TuanhCD233018 (MaNV_TuanhCD233018);

ALTER TABLE dbo.tblChiTietDuAn_KienCD233824
ADD CONSTRAINT FK_tblChiTietDuAn_KienCD233824_tblNhanVien_TuanhCD233018
FOREIGN KEY (MaNV_TuanhCD233018)
REFERENCES dbo.tblNhanVien_TuanhCD233018 (MaNV_TuanhCD233018);

ALTER TABLE dbo.tblChiTietDuAn_KienCD233824
ADD CONSTRAINT FK_tblChiTietDuAn_KienCD233824_tblDuAn_KienCD233824
FOREIGN KEY (MaDA_KienCD233824)
REFERENCES dbo.tblDuAn_KienCD233824 (MaDA_KienCD233824);

ALTER TABLE dbo.tblHopDong_ChienCD232928
ADD CONSTRAINT FK_tblHopDong_ChienCD232928_tblNhanVien_TuanhCD233018
FOREIGN KEY (MaNV_TuanhCD233018)
REFERENCES dbo.tblNhanVien_TuanhCD233018 (MaNV_TuanhCD233018);

ALTER TABLE dbo.tblChamCong_TuanhCD233018
ADD CONSTRAINT FK_tblChamCong_TuanhCD233018_tblNhanVien_TuanhCD233018
FOREIGN KEY (NhanVienId_TuanhCD233018)
REFERENCES dbo.tblNhanVien_TuanhCD233018 (Id_TuanhCD233018);

ALTER TABLE dbo.tblLuong_ChienCD232928
ADD CONSTRAINT FK_tblLuong_ChienCD232928_tblChamCong_TuanhCD233018
FOREIGN KEY (ChamCongId_TuanhCD233018)
REFERENCES dbo.tblChamCong_TuanhCD233018 (Id_TuanhCD233018);
 

 /*them du lieu mau*/

 /* =====================================================
1) ROLE
===================================================== */
INSERT INTO dbo.tblRole_ThuanCD233318
(TenRole_ThuanCD233318, MoTa_ThuanCD233318)
VALUES 
(N'Admin', N'Quản trị hệ thống'),
(N'Nhân sự', N'Quản lý nhân sự'),
(N'Nhân viên', N'Nhân viên thường');
GO


/* =====================================================
2) PHÒNG BAN
===================================================== */
INSERT INTO dbo.tblPhongBan_ThuanCD233318
(MaPB_ThuanCD233318, TenPB_ThuanCD233318, DiaChi_ThuanCD233318,
 SoDienThoai_ThuanCD233318, Ghichu_ThuanCD233318, DeletedAt_ThuanCD233318)
VALUES
('PB01', N'Nhân sự',    N'Hà Nội', '090000001', NULL, 0),
('PB02', N'Kế toán',    N'Hà Nội', '090000002', NULL, 0),
('PB03', N'CNTT',       N'Hà Nội', '090000003', NULL, 0),
('PB04', N'Kinh doanh', N'Hà Nội', '090000004', NULL, 0),
('PB05', N'Vận hành',   N'Hà Nội', '090000005', NULL, 0);
GO


/* =====================================================
3) CHỨC VỤ
===================================================== */
INSERT INTO dbo.tblChucVu_KhangCD233181
(MaCV_KhangCD233181, TenCV_KhangCD233181, Ghichu_KhangCD233181,
 DeletedAt_KhangCD233181, MaPB_ThuanCD233318)
VALUES
('CV01', N'Trưởng phòng', NULL, 0, 'PB01'),
('CV02', N'Phó phòng', NULL, 0, 'PB01'),
('CV03', N'Chuyên viên nhân sự', NULL, 0, 'PB01'),
('CV04', N'Nhân viên nhân sự', NULL, 0, 'PB01'),
('CV05', N'Thực tập nhân sự', NULL, 0, 'PB01'),

('CV06', N'Trưởng phòng', NULL, 0, 'PB02'),
('CV07', N'Phó phòng', NULL, 0, 'PB02'),
('CV08', N'Kế toán tổng hợp', NULL, 0, 'PB02'),
('CV09', N'Kế toán viên', NULL, 0, 'PB02'),
('CV10', N'Thực tập kế toán', NULL, 0, 'PB02'),

('CV11', N'Trưởng phòng', NULL, 0, 'PB03'),
('CV12', N'Team Lead', NULL, 0, 'PB03'),
('CV13', N'Lập trình viên', NULL, 0, 'PB03'),
('CV14', N'QA', NULL, 0, 'PB03'),
('CV15', N'Thực tập IT', NULL, 0, 'PB03');
GO


/* =====================================================
4) DỰ ÁN
===================================================== */
INSERT INTO dbo.tblDuAn_KienCD233824
(MaDA_KienCD233824, TenDA_KienCD233824, MoTa_KienCD233824,
 NgayBatDau_KienCD233824, NgayKetThuc_KienCD233824,
 Ghichu_KienCD233824, DeletedAt_KienCD233824)
VALUES
('DA01', N'ERP Nội bộ', NULL, '2025-01-01', '2025-12-31', NULL, 0),
('DA02', N'Website bán hàng', NULL, '2025-02-01', '2025-11-30', NULL, 0),
('DA03', N'Ứng dụng chấm công', NULL, '2025-03-01', '2025-10-31', NULL, 0),
('DA04', N'Data Warehouse', NULL, '2025-04-01', '2025-12-15', NULL, 0),
('DA05', N'CRM Kinh doanh', NULL, '2025-05-01', '2025-12-20', NULL, 0);
GO


/* =====================================================
5) NHÂN VIÊN
===================================================== */
DECLARE @Emp TABLE (HoTen NVARCHAR(100), NgaySinh DATE);

INSERT INTO @Emp (HoTen, NgaySinh) VALUES
(N'Lê Tuấn Anh','2005-09-29'),
(N'Nguyễn Nhật Anh','2005-07-20'),
(N'Phạm Hoàng Bắc','2005-12-15'),
(N'Lô Văn Công','2005-07-25'),
(N'Đoàn Văn Cương','2004-10-22'),
(N'Vũ Duy Doanh','2005-10-19'),
(N'Hoàng Ngọc Hùng Dũng','2005-01-20'),
(N'Đào Hải Dương','2005-03-07'),
(N'Trần Viết Dương','2005-02-07'),
(N'Nguyễn Đình Đăng','2005-11-08'),
(N'Hoàng Anh Đức','2005-10-03'),
(N'Kiều Trung Giáp','2005-08-18'),
(N'Trần Hoàng Hải','2005-07-22'),
(N'Vương Nhất Hào','2005-11-30'),
(N'Đoàn Trung Hiếu','2005-03-11'),
(N'Đỗ Trung Hiếu','2005-12-30'),
(N'Phạm Huy Hiệu','2005-02-09'),
(N'Nguyễn Xuân Hoàn','2005-09-20'),
(N'Đặng Đình Hùng','2002-07-30'),
(N'Nguyễn Xuân Hùng','2005-03-09'),
(N'Nguyễn Quang Huy','2005-08-08'),
(N'Nguyễn Quang Huy','2005-06-11'),
(N'Nguyễn Thọ Hưng','2005-05-19'),
(N'Vũ Quốc Khánh','2005-09-01'),
(N'Nguyễn Hà Linh','2005-03-07'),
(N'Trịnh Xuân Lộc','2005-11-05'),
(N'Nguyễn Văn Lưu','2005-06-06'),
(N'Nguyễn Phương Nam','2005-04-29'),
(N'Phạm Văn Nam','2005-12-02'),
(N'Nguyễn Trọng Nghĩa','2004-05-24'),
(N'Ngô Văn Quang','2005-02-12'),
(N'Nguyễn Sĩ Quân','2005-06-29'),
(N'Trần Phú Quân','2005-01-16'),
(N'Vũ Quang Sang','2005-03-09'),
(N'Phạm Minh Sơn','2005-12-22'),
(N'Nguyễn Cơ Thái','2005-02-10'),
(N'Lê Đức Thanh','2005-02-25'),
(N'Nguyễn Văn Thành','2005-10-03'),
(N'Phùng Tiến Thành','2005-10-07'),
(N'Đỗ Mạnh Thắng','2005-05-24'),
(N'Nguyễn Văn Thưởng','2005-11-15'),
(N'Tống Xuân Toàn','2005-11-23'),
(N'Đỗ Anh Tú','2005-09-03'),
(N'Nguyễn Như Tùng','2005-07-12'),
(N'Nguyễn Quang Tùng','2005-07-28'),
(N'Cấn Xuân Việt','2005-11-05'),
(N'Phi Hồng Vương','2003-06-16'),

(N'Hoàng Tuấn Anh','2003-01-21'),
(N'Nguyễn Hải Anh','2005-09-24'),
(N'Nguyễn Quốc Anh','2005-11-06'),
(N'Nguyễn Việt Anh','2005-02-08'),
(N'Trương Nguyệt Anh','2004-10-17'),
(N'Nguyễn Đình Chiến','2003-11-05'),
(N'Trần Đăng Chiến','2005-08-23'),
(N'Đỗ Thành Công','2005-10-16'),
(N'Lê Văn Dũng','2002-07-02'),
(N'Lưu Minh Dũng','2005-12-15'),
(N'Trần Quốc Dũng','2005-11-20'),
(N'Hoàng Đăng Dương','2005-10-20'),
(N'Nguyễn Tiến Đạt','2005-12-14'),
(N'Trần Tuấn Đạt','2005-06-23'),
(N'Nguyễn Trọng Điền','2005-07-26'),
(N'Lê Minh Hải','2005-11-06'),
(N'Lưu Ngọc Hải','2005-06-06'),
(N'Nguyễn Đức Hậu','2005-08-20'),
(N'Nguyễn Xuân Hiển','2003-07-22'),
(N'Đỗ Đình Hiệp','2005-05-25'),
(N'Nguyễn Công Hòa','2005-11-19'),
(N'Nguyễn Xuân Hoàng','2005-08-27'),
(N'Phan Hữu Huy Hoàng','2005-09-25'),
(N'Hà Đăng Hợp','2005-07-12'),
(N'Mai Chí Hùng','2005-02-24'),
(N'Nguyễn Quang Hùng','2004-11-09'),
(N'Nguyễn Thanh Hùng','2005-02-26'),
(N'Đỗ Xuân Huy','2005-05-02'),
(N'Trần Quang Huy','2004-08-19'),
(N'Vũ Minh Khang','2005-05-23'),
(N'Bùi Quang Khoa','2005-09-16'),
(N'Chu Văn Khoa','2005-09-10'),
(N'Ngô Trung Kiên','2005-08-02'),
(N'Nguyễn Trung Kiên','2005-03-17'),
(N'Nguyễn Trung Kiên','2005-03-13'),
(N'Nguyễn Thành Lộc','2005-06-16'),
(N'Nguyễn Trường Luận','2005-11-17'),
(N'Lê Quang Minh','2005-03-10'),
(N'Trần Tất Minh','2001-04-28'),
(N'Mạch Quý Nhân','2001-01-01'),
(N'Nguyễn Hữu Pháp','2005-01-25'),
(N'Nguyễn Thanh Phong','2005-06-02'),
(N'Trần Đình Phúc','2005-02-16'),
(N'Nguyễn Văn Quyền','2004-08-22'),
(N'Hà Mạnh Thái Sơn','2004-09-18'),
(N'Nguyễn Minh Tâm','2005-10-10'),
(N'Nguyễn Văn Thành','2005-10-19'),
(N'Nguyễn Văn Thắng','2005-01-11'),
(N'Nguyễn Văn Thông','2004-04-18'),
(N'Cao Nhân Thuận','2005-03-17'),
(N'Vũ Thị Thanh Thuý','2005-04-23'),
(N'Phạm Văn Toàn','2005-03-06'),
(N'Dương Anh Tuấn','2005-01-08'),
(N'Nguyễn Văn Tuấn','2003-02-21'),
(N'Nguyễn Duy Tùng','2005-02-25'),
(N'Nguyễn Quốc Việt','2005-12-10'),
(N'Lê Minh Vương','2005-12-06'),
(N'Nguyễn Thịnh Vượng','2005-03-13');

DECLARE @CV TABLE (rn INT, MaCV VARCHAR(10));
INSERT INTO @CV
SELECT ROW_NUMBER() OVER (ORDER BY MaCV_KhangCD233181), MaCV_KhangCD233181
FROM dbo.tblChucVu_KhangCD233181;

;WITH E AS (
    SELECT HoTen, NgaySinh,
           ROW_NUMBER() OVER (ORDER BY HoTen) r
    FROM @Emp
)
INSERT INTO dbo.tblNhanVien_TuanhCD233018
(MaNV_TuanhCD233018, HoTen_TuanhCD233018, NgaySinh_TuanhCD233018,
 GioiTinh_TuanhCD233018, DiaChi_TuanhCD233018,
 SoDienThoai_TuanhCD233018, Email_TuanhCD233018,
 MaCV_KhangCD233181, Ghichu_TuanhCD233018, DeletedAt_TuanhCD233018)
SELECT
    'NV' + RIGHT('000'+CAST(r AS VARCHAR),3),
    HoTen,
    NgaySinh,
    N'Nam',
    N'Hà Nội',
    '091000000' + CAST(r AS VARCHAR),
    'nv'+CAST(r AS VARCHAR)+'@mail.com',
    cv.MaCV,
    NULL,
    0
FROM E
JOIN @CV cv ON cv.rn = ((E.r-1)%15)+1;
GO


/* =====================================================
6) TÀI KHOẢN
===================================================== */
INSERT INTO dbo.tblTaiKhoan_KhangCD233181
(MaTK_KhangCD233181, MaNV_TuanhCD233018,
 SoDienThoai_KhangCD233181, MatKhau_KhangCD233181,
 Quyen_KhangCD233181, Ghichu_KhangCD233181,
 DeletedAt_KhangCD233181, RoleId_ThuanCD233318)
SELECT
    'TK'+MaNV_TuanhCD233018,
    MaNV_TuanhCD233018,
    SoDienThoai_TuanhCD233018,
    CONVERT(VARCHAR(255),HASHBYTES('MD5','123456'),2),
    N'Nhân viên',
    NULL,
    0,
    3
FROM dbo.tblNhanVien_TuanhCD233018;
GO
/*7*/

INSERT INTO dbo.tblChiTietDuAn_KienCD233824
(
    MaNV_TuanhCD233018,
    MaDA_KienCD233824,
    VaiTro_KienCD233824,
    Ghichu_KienCD233824,
    DeletedAt_KienCD233824
)
SELECT
    nv.MaNV_TuanhCD233018,
    da.MaDA_KienCD233824,
    N'Thành viên',
    NULL,
    0
FROM dbo.tblNhanVien_TuanhCD233018 nv
CROSS JOIN dbo.tblDuAn_KienCD233824 da;
GO


/*8*/



INSERT INTO dbo.tblHopDong_ChienCD232928
(
    MaHopDong_ChienCD232928,
    MaNV_TuanhCD233018,
    NgayBatDau_ChienCD232928,
    NgayKetThuc_ChienCD232928,
    LoaiHopDong_ChienCD232928,
    LuongCoBan_ChienCD232928,
    Ghichu_ChienCD232928,
    DeletedAt_ChienCD232928
)
SELECT
    'HD' + RIGHT('000' + CAST(ROW_NUMBER() OVER (ORDER BY Id_TuanhCD233018) AS VARCHAR), 3),
    MaNV_TuanhCD233018,
    '2025-01-01',
    '2026-01-01',
    N'Không xác định',
    8000000,
    NULL,
    0
FROM dbo.tblNhanVien_TuanhCD233018;
GO

/*9*/

INSERT INTO dbo.tblChamCong_TuanhCD233018
(
    MaChamCong_TuanhCD233018,
    Ngay_TuanhCD233018,
    GioVao_TuanhCD233018,
    GioVe_TuanhCD233018,
    Ghichu_TuanhCD233018,
    DeletedAt_TuanhCD233018,
    NhanVienId_TuanhCD233018
)
SELECT
    'CC' + RIGHT('000000' + CAST(nv.Id_TuanhCD233018 AS VARCHAR), 6) + '_20250101',
    '2025-01-01',
    '08:00',
    '17:00',
    NULL,
    0,
    nv.Id_TuanhCD233018
FROM dbo.tblNhanVien_TuanhCD233018 nv
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.tblChamCong_TuanhCD233018 cc
    WHERE cc.NhanVienId_TuanhCD233018 = nv.Id_TuanhCD233018
      AND cc.Ngay_TuanhCD233018 = '2025-01-01'
);
GO


/*10*/


INSERT INTO dbo.tblLuong_ChienCD232928
(
    MaLuong_ChienCD232928,
    Thang_ChienCD232928,
    Nam_ChienCD232928,
    LuongCoBan_ChienCD232928,
    SoNgayCongChuan_ChienCD232928,
    PhuCap_ChienCD232928,
    KhauTru_ChienCD232928,
    Ghichu_ChienCD232928,
    DeletedAt_ChienCD232928,
    ChamCongId_TuanhCD233018
)
SELECT
    'LG' + RIGHT('000000' + CAST(cc.Id_TuanhCD233018 AS VARCHAR), 6),
    MONTH(cc.Ngay_TuanhCD233018),
    YEAR(cc.Ngay_TuanhCD233018),
    8000000,
    26,
    1000000,        -- phụ cấp
    0,              -- khấu trừ
    NULL,
    0,
    cc.Id_TuanhCD233018
FROM dbo.tblChamCong_TuanhCD233018 cc
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.tblLuong_ChienCD232928 l
    WHERE l.ChamCongId_TuanhCD233018 = cc.Id_TuanhCD233018
);
GO
