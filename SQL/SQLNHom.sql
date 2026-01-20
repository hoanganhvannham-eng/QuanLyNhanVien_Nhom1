
CREATE DATABASE QuanLyNhanVien_Nhom13;
GO
USE QuanLyNhanVien_Nhom13;
GO
/* =========================
   3) CREATE TABLES
========================= */

CREATE TABLE tblRole_ThuanCD233318 (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    TenRole NVARCHAR(50)  NOT NULL,
    MoTa    NVARCHAR(200) NULL
);
GO

CREATE TABLE tblPhongBan_ThuanCD233318 (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    MaPB        VARCHAR(10)     NOT NULL,
    TenPB       NVARCHAR(100)   NOT NULL,
    DiaChi      NVARCHAR(200)   NULL,
    SoDienThoai VARCHAR(20)     NULL,
    Ghichu      NVARCHAR(255)   NULL,
    DeletedAt   INT             NOT NULL
);
GO

CREATE TABLE  tblChucVu_KhangCD233181 (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    MaCV      VARCHAR(10)     NOT NULL,
    TenCV     NVARCHAR(100)   NOT NULL,
    Ghichu    NVARCHAR(255)   NULL,
    DeletedAt INT             NOT NULL,
    MaPB      VARCHAR(10)     NOT NULL
);
GO

CREATE TABLE  tblNhanVien_TuanhCD233018 (
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

CREATE TABLE  tblTaiKhoan_KhangCD233181 (
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

CREATE TABLE  tblDuAn_KienCD233824 (
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

CREATE TABLE  tblChiTietDuAn_KienCD233824 (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    MaNV      VARCHAR(10)      NOT NULL,
    MaDA      VARCHAR(10)      NOT NULL,
    VaiTro    NVARCHAR(100)    NULL,
    Ghichu    NVARCHAR(255)    NULL,
    DeletedAt INT              NOT NULL
);
GO

CREATE TABLE  tblHopDong_ChienCD232928 (
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

CREATE TABLE  tblChamCong_TuanhCD233018 (
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

CREATE TABLE  tblLuong_ChienCD232928 (
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

-- tblChucVu_KhangCD233181 -> tblPhongBan (MaPB)
ALTER TABLE dbo.tblPhongBan_ThuanCD233318
ADD CONSTRAINT UQ_tblPhongBan_ThuanCD233318_MaPB UNIQUE (MaPB);

ALTER TABLE dbo.tblChucVu_KhangCD233181
ADD CONSTRAINT UQ_tblChucVu_KhangCD233181_MaCV UNIQUE (MaCV);

ALTER TABLE dbo.tblNhanVien_TuanhCD233018
ADD CONSTRAINT UQ_tblNhanVien_TuanhCD233018_MaNV UNIQUE (MaNV);

ALTER TABLE dbo.tblDuAn_KienCD233824
ADD CONSTRAINT UQ_tblDuAn_KienCD233824_MaDA UNIQUE (MaDA);

ALTER TABLE dbo.tblChucVu_KhangCD233181
ADD CONSTRAINT FK_tblChucVu_KhangCD233181_tblPhongBan_ThuanCD233318
FOREIGN KEY (MaPB) REFERENCES dbo.tblPhongBan_ThuanCD233318(MaPB);

-- tblNhanVien_TuanhCD233018 -> tblChucVu_KhangCD233181 (MaCV)
ALTER TABLE dbo.tblNhanVien_TuanhCD233018
ADD CONSTRAINT FK_tblNhanVien_TuanhCD233018_tblChucVu_KhangCD233181
FOREIGN KEY (MaCV) REFERENCES dbo.tblChucVu_KhangCD233181(MaCV);

-- tblTaiKhoan_KhangCD233181 -> tblRole_ThuanCD233318 (RoleId)
ALTER TABLE dbo.tblTaiKhoan_KhangCD233181
ADD CONSTRAINT FK_tblTaiKhoan_KhangCD233181_tblRole_ThuanCD233318
FOREIGN KEY (RoleId) REFERENCES dbo.tblRole_ThuanCD233318(Id);

-- tblTaiKhoan_KhangCD233181 -> tblNhanVien_TuanhCD233018 (MaNV)
ALTER TABLE dbo.tblTaiKhoan_KhangCD233181
ADD CONSTRAINT FK_tblTaiKhoan_KhangCD233181_tblNhanVien_TuanhCD233018
FOREIGN KEY (MaNV) REFERENCES dbo.tblNhanVien_TuanhCD233018(MaNV);

-- tblChiTietDuAn_KienCD233824 -> tblNhanVien_TuanhCD233018, tblDuAn_KienCD233824
ALTER TABLE dbo.tblChiTietDuAn_KienCD233824
ADD CONSTRAINT FK_tblChiTietDuAn_KienCD233824_tblNhanVien_TuanhCD233018
FOREIGN KEY (MaNV) REFERENCES dbo.tblNhanVien_TuanhCD233018(MaNV);

ALTER TABLE dbo.tblChiTietDuAn_KienCD233824
ADD CONSTRAINT FK_tblChiTietDuAn_KienCD233824_tblDuAn_KienCD233824
FOREIGN KEY (MaDA) REFERENCES dbo.tblDuAn_KienCD233824(MaDA);

-- tblHopDong_ChienCD232928 -> tblNhanVien_TuanhCD233018 (MaNV)
ALTER TABLE dbo.tblHopDong_ChienCD232928
ADD CONSTRAINT FK_tblHopDong_ChienCD232928_tblNhanVien_TuanhCD233018

FOREIGN KEY (MaNV) REFERENCES dbo.tblNhanVien_TuanhCD233018(MaNV);

-- tblChamCong_TuanhCD233018 -> tblNhanVien_TuanhCD233018 (NhanVienId)
ALTER TABLE dbo.tblChamCong_TuanhCD233018
ADD CONSTRAINT FK_tblChamCong_TuanhCD233018_tblNhanVien_TuanhCD233018
FOREIGN KEY (NhanVienId) REFERENCES dbo.tblNhanVien_TuanhCD233018(Id);

-- tblLuong_ChienCD232928 -> tblChamCong_TuanhCD233018 (ChamCongId)
ALTER TABLE dbo.tblLuong_ChienCD232928
ADD CONSTRAINT FK_tblLuong_ChienCD232928_tblChamCong_TuanhCD233018
FOREIGN KEY (ChamCongId) REFERENCES dbo.tblChamCong_TuanhCD233018(Id);
GO

/* them du lieu mau*/


/* =====================================================
1) ROLE
===================================================== */
INSERT INTO dbo.tblRole_ThuanCD233318(TenRole, MoTa)
VALUES 
(N'Admin', N'Quản trị hệ thống'),
(N'Nhân sự', N'Quản lý nhân sự'),
(N'Nhân viên', N'Nhân viên thường');
GO

/* =====================================================
2) PHÒNG BAN (>=5)
===================================================== */
INSERT INTO dbo.tblPhongBan_ThuanCD233318(MaPB, TenPB, DiaChi, SoDienThoai, Ghichu, DeletedAt)
VALUES
('PB01', N'Nhân sự',     N'Hà Nội', '090000001', NULL, 0),
('PB02', N'Kế toán',     N'Hà Nội', '090000002', NULL, 0),
('PB03', N'CNTT',        N'Hà Nội', '090000003', NULL, 0),
('PB04', N'Kinh doanh',  N'Hà Nội', '090000004', NULL, 0),
('PB05', N'Vận hành',    N'Hà Nội', '090000005', NULL, 0);
GO

/* =====================================================
3) CHỨC VỤ (mỗi PB >=5) => tổng 25 chức vụ
===================================================== */
INSERT INTO dbo.tblChucVu_KhangCD233181(MaCV, TenCV, Ghichu, DeletedAt, MaPB)
VALUES
('CV01', N'Trưởng phòng',         NULL, 0, 'PB01'),
('CV02', N'Phó phòng',            NULL, 0, 'PB01'),
('CV03', N'Chuyên viên nhân sự',  NULL, 0, 'PB01'),
('CV04', N'Nhân viên nhân sự',    NULL, 0, 'PB01'),
('CV05', N'Thực tập nhân sự',     NULL, 0, 'PB01'),

('CV06', N'Trưởng phòng',         NULL, 0, 'PB02'),
('CV07', N'Phó phòng',            NULL, 0, 'PB02'),
('CV08', N'Kế toán tổng hợp',     NULL, 0, 'PB02'),
('CV09', N'Kế toán viên',         NULL, 0, 'PB02'),
('CV10', N'Thực tập kế toán',     NULL, 0, 'PB02'),

('CV11', N'Trưởng phòng',         NULL, 0, 'PB03'),
('CV12', N'Team Lead',            NULL, 0, 'PB03'),
('CV13', N'Lập trình viên',       NULL, 0, 'PB03'),
('CV14', N'Kiểm thử (QA)',        NULL, 0, 'PB03'),
('CV15', N'Thực tập IT',          NULL, 0, 'PB03'),

('CV16', N'Trưởng phòng',         NULL, 0, 'PB04'),
('CV17', N'Phó phòng',            NULL, 0, 'PB04'),
('CV18', N'Nhân viên Sales',      NULL, 0, 'PB04'),
('CV19', N'Marketing',            NULL, 0, 'PB04'),
('CV20', N'Thực tập KD/MKT',      NULL, 0, 'PB04'),

('CV21', N'Trưởng phòng',         NULL, 0, 'PB05'),
('CV22', N'Giám sát vận hành',    NULL, 0, 'PB05'),
('CV23', N'Nhân viên vận hành',   NULL, 0, 'PB05'),
('CV24', N'Kho vận',              NULL, 0, 'PB05'),
('CV25', N'Thực tập vận hành',    NULL, 0, 'PB05');
GO

/* =====================================================
4) DỰ ÁN (thêm nhiều để chia đều)
===================================================== */
INSERT INTO dbo.tblDuAn_KienCD233824(MaDA, TenDA, MoTa, NgayBatDau, NgayKetThuc, Ghichu, DeletedAt)
VALUES
('DA01', N'ERP Nội bộ',            NULL, '2025-01-01', '2025-12-31', NULL, 0),
('DA02', N'Website bán hàng',      NULL, '2025-02-01', '2025-11-30', NULL, 0),
('DA03', N'Ứng dụng chấm công',    NULL, '2025-03-01', '2025-10-31', NULL, 0),
('DA04', N'Data Warehouse',        NULL, '2025-04-01', '2025-12-15', NULL, 0),
('DA05', N'CRM Kinh doanh',        NULL, '2025-05-01', '2025-12-20', NULL, 0);
GO

/* =====================================================
5) NHÂN VIÊN: ĐỦ THEO DANH SÁCH BẠN ĐƯA (tự chia đều MaCV)
   - Ưu tiên đưa các tên: Vũ Minh Khang, Hoàng Tuấn Anh, Trần Đăng Chiến, Cao Nhân Thuận lên đầu nếu có trong list
===================================================== */
DECLARE @Emp TABLE (HoTen NVARCHAR(100) NOT NULL, NgaySinh DATE NOT NULL);

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

DECLARE @CV TABLE (rn INT PRIMARY KEY, MaCV VARCHAR(10) NOT NULL);
;WITH x AS (SELECT ROW_NUMBER() OVER (ORDER BY MaCV) rn, MaCV FROM dbo.tblChucVu_KhangCD233181)
INSERT INTO @CV(rn, MaCV) SELECT rn, MaCV FROM x;

;WITH E AS (
    SELECT 
        HoTen, NgaySinh,
        ROW_NUMBER() OVER (
            ORDER BY 
                CASE 
                    WHEN HoTen = N'Vũ Minh Khang' THEN 1
                    WHEN HoTen = N'Hoàng Tuấn Anh' THEN 2
                    WHEN HoTen = N'Trần Đăng Chiến' THEN 3
                    WHEN HoTen = N'Cao Nhân Thuận' THEN 4
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
INSERT INTO dbo.tblNhanVien_TuanhCD233018
(MaNV, HoTen, NgaySinh, GioiTinh, DiaChi, SoDienThoai, Email, MaCV, Ghichu, DeletedAt)
SELECT
    m.MaNV,
    m.HoTen,
    m.NgaySinh,
    CASE 
        WHEN m.HoTen LIKE N'%Thị%' OR m.HoTen LIKE N'%Nguyệt%' THEN N'Nữ'
        ELSE N'Nam'
    END AS GioiTinh,
    N'Hà Nội' AS DiaChi,
    '091' + RIGHT('000000' + CAST(m.r AS VARCHAR(10)), 6) AS SoDienThoai,
    'nv' + CAST(m.r AS VARCHAR(10)) + '@mail.com' AS Email,
    cv.MaCV,
    NULL,
    0
FROM M m
JOIN @CV cv ON cv.rn = m.cv_rn;
GO

/* =====================================================
6) TÀI KHOẢN (mỗi NV 1 TK) + phân quyền chia đều (Admin/Nhân sự/Nhân viên)
===================================================== */
;WITH N AS (
    SELECT MaNV, SoDienThoai, ROW_NUMBER() OVER (ORDER BY MaNV) r
    FROM dbo.tblNhanVien_TuanhCD233018
)
INSERT INTO dbo.tblTaiKhoan_KhangCD233181
(MaTK, MaNV, SoDienThoai, MatKhau, Quyen, Ghichu, DeletedAt, RoleId)
SELECT
    'TK' + MaNV,
    MaNV,
    SoDienThoai,
    CONVERT(VARCHAR(255), HASHBYTES('MD5','123456'),2),
    CASE WHEN (r % 20)=1 THEN N'Admin'
         WHEN (r % 10)=1 THEN N'Nhân sự'
         ELSE N'Nhân viên' END,
    NULL,
    0,
    CASE WHEN (r % 20)=1 THEN 1
         WHEN (r % 10)=1 THEN 2
         ELSE 3 END
FROM N;
GO

/* =====================================================
7) HỢP ĐỒNG (chia đều loại HĐ + lương theo nhóm)
===================================================== */
;WITH N AS (
    SELECT MaNV, ROW_NUMBER() OVER (ORDER BY MaNV) r
    FROM dbo.tblNhanVien_TuanhCD233018
)
INSERT INTO dbo.tblHopDong_ChienCD232928
(MaHopDong, MaNV, NgayBatDau, NgayKetThuc, LoaiHopDong, LuongCoBan, Ghichu, DeletedAt)
SELECT
    'HD' + MaNV,
    MaNV,
    '2025-01-01',
    '2025-12-31',
    CASE WHEN (r % 3)=0 THEN N'Hợp đồng 1 năm'
         WHEN (r % 3)=1 THEN N'Hợp đồng 2 năm'
         ELSE N'Hợp đồng thử việc' END,
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
8) CHI TIẾT DỰ ÁN (chia đều 5 dự án)
===================================================== */
;WITH N AS (
    SELECT MaNV, ROW_NUMBER() OVER (ORDER BY MaNV) r
    FROM dbo.tblNhanVien_TuanhCD233018
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
INSERT INTO dbo.tblChiTietDuAn_KienCD233824(MaNV, MaDA, VaiTro, Ghichu, DeletedAt)
SELECT
    MaNV,
    MaDA,
    CASE WHEN (r % 25)=1 THEN N'Quản lý dự án'
         WHEN (r % 7)=0 THEN N'Leader'
         ELSE N'Thành viên' END,
    NULL,
    0
FROM D;
GO

/* =====================================================
9) CHẤM CÔNG: ĐỦ 12 THÁNG NĂM 2025, MỖI NGÀY MỖI NV ĐỀU CÓ
   - Tạo dữ liệu theo ngày: 2025-01-01 -> 2025-12-31
===================================================== */
DECLARE @Ngay DATE = '2025-01-01';
WHILE @Ngay <= '2025-12-31'
BEGIN
    INSERT INTO dbo.tblChamCong_TuanhCD233018
    (MaChamCong, Ngay, GioVao, GioVe, Ghichu, DeletedAt, NhanVienId)
    SELECT
        'CC' + RIGHT('000000' + CAST(nv.Id AS VARCHAR(10)), 6) + '_' + CONVERT(VARCHAR(8), @Ngay, 112),
        @Ngay,
        CASE WHEN (nv.Id % 4)=0 THEN '07:45' WHEN (nv.Id % 4)=1 THEN '08:00' WHEN (nv.Id % 4)=2 THEN '08:10' ELSE '08:05' END,
        CASE WHEN (nv.Id % 4)=0 THEN '17:00' WHEN (nv.Id % 4)=1 THEN '17:15' WHEN (nv.Id % 4)=2 THEN '17:30' ELSE '17:05' END,
        CASE WHEN (nv.Id % 13)=0 THEN N'Đi công tác' WHEN (nv.Id % 17)=0 THEN N'Tăng ca' ELSE NULL END,
        0,
        nv.Id
    FROM dbo.tblNhanVien_TuanhCD233018 nv;

    SET @Ngay = DATEADD(DAY, 1, @Ngay);
END
GO

/* =====================================================
10) LƯƠNG: ĐỦ NĂM 2025 (mỗi NV mỗi THÁNG 1 bản ghi)
   - ChamCongId: lấy 1 bản ghi chấm công đại diện trong tháng (MIN)
   - SoNgayCongChuan: 26
===================================================== */
;WITH CC AS (
    SELECT 
        cc.NhanVienId,
        MONTH(cc.Ngay) AS Thang,
        YEAR(cc.Ngay)  AS Nam,
        MIN(cc.Id) AS ChamCongId,
        COUNT(*)  AS SoNgayCongThucTe
    FROM dbo.tblChamCong_TuanhCD233018 cc
    WHERE cc.Ngay >= '2025-01-01' AND cc.Ngay <= '2025-12-31'
    GROUP BY cc.NhanVienId, MONTH(cc.Ngay), YEAR(cc.Ngay)
),
NV AS (
    SELECT nv.Id AS NhanVienId, nv.MaNV, hd.LuongCoBan
    FROM dbo.tblNhanVien_TuanhCD233018 nv
    JOIN dbo.tblHopDong_ChienCD232928 hd ON hd.MaNV = nv.MaNV
)
INSERT INTO dbo.tblLuong_ChienCD232928
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
