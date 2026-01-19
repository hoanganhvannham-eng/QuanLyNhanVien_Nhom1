CREATE DATABASE QuanLyNhanSu;
GO
USE QuanLyNhanSu;
GO
CREATE TABLE tblRole (
    Id INT IDENTITY PRIMARY KEY,
    TenRole NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(255)
);
CREATE TABLE tblPhongBan (
    Id INT IDENTITY PRIMARY KEY,
    MaPB NVARCHAR(20) NOT NULL UNIQUE,
    TenPB NVARCHAR(100) NOT NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(15),
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL
);
CREATE TABLE tblChucVu (
    Id INT IDENTITY PRIMARY KEY,
    MaCV NVARCHAR(20) NOT NULL UNIQUE,
    TenCV NVARCHAR(100) NOT NULL,
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL,
    MaPB INT NOT NULL,
    CONSTRAINT FK_ChucVu_PhongBan 
        FOREIGN KEY (MaPB) REFERENCES tblPhongBan(Id)
);
CREATE TABLE tblNhanVien (
    Id INT IDENTITY PRIMARY KEY,
    MaNV NVARCHAR(20) NOT NULL UNIQUE,
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(255),
    SoDienThoai NVARCHAR(15),
    Email NVARCHAR(100),
    MaCV INT NOT NULL,
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL,
    CONSTRAINT FK_NhanVien_ChucVu
        FOREIGN KEY (MaCV) REFERENCES tblChucVu(Id)
);
CREATE TABLE tblTaiKhoan (
    Id INT IDENTITY PRIMARY KEY,
    MaTK NVARCHAR(50) NOT NULL UNIQUE,
    MaNV INT NOT NULL,
    SoDienThoai NVARCHAR(15),
    MatKhau NVARCHAR(255) NOT NULL,
    Quyen NVARCHAR(50),
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL,
    RoleId INT NOT NULL,
    CONSTRAINT FK_TaiKhoan_NhanVien
        FOREIGN KEY (MaNV) REFERENCES tblNhanVien(Id),
    CONSTRAINT FK_TaiKhoan_Role
        FOREIGN KEY (RoleId) REFERENCES tblRole(Id)
);
CREATE TABLE tblDuAn (
    Id INT IDENTITY PRIMARY KEY,
    MaDA NVARCHAR(20) NOT NULL UNIQUE,
    TenDA NVARCHAR(200) NOT NULL,
    MoTa NVARCHAR(255),
    NgayBatDau DATE,
    NgayKetThuc DATE,
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL
);
CREATE TABLE tblChiTietDuAn (
    Id INT IDENTITY PRIMARY KEY,
    MaNV INT NOT NULL,
    MaDA INT NOT NULL,
    VaiTro NVARCHAR(100),
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL,
    CONSTRAINT FK_CTDA_NhanVien
        FOREIGN KEY (MaNV) REFERENCES tblNhanVien(Id),
    CONSTRAINT FK_CTDA_DuAn
        FOREIGN KEY (MaDA) REFERENCES tblDuAn(Id)
);
CREATE TABLE tblHopDong (
    Id INT IDENTITY PRIMARY KEY,
    MaHopDong NVARCHAR(50) NOT NULL UNIQUE,
    MaNV INT NOT NULL,
    NgayBatDau DATE,
    NgayKetThuc DATE,
    LoaiHopDong NVARCHAR(100),
    LuongCoBan DECIMAL(18,2),
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL,
    CONSTRAINT FK_HopDong_NhanVien
        FOREIGN KEY (MaNV) REFERENCES tblNhanVien(Id)
);
CREATE TABLE tblChamCong (
    Id INT IDENTITY PRIMARY KEY,
    MaChamCong NVARCHAR(50) NOT NULL UNIQUE,
    Ngay DATE NOT NULL,
    GioVao TIME,
    GioVe TIME,
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL,
    NhanVienId INT NOT NULL,
    CONSTRAINT FK_ChamCong_NhanVien
        FOREIGN KEY (NhanVienId) REFERENCES tblNhanVien(Id)
);
CREATE TABLE tblLuong (
    Id INT IDENTITY PRIMARY KEY,
    MaLuong NVARCHAR(50) NOT NULL UNIQUE,
    Thang INT,
    Nam INT,
    LuongCoBan DECIMAL(18,2),
    SoNgayCongChuan INT,
    PhuCap DECIMAL(18,2),
    KhauTru DECIMAL(18,2),
    GhiChu NVARCHAR(255),
    DeletedAt DATETIME NULL,
    ChamCongId INT NOT NULL,
    CONSTRAINT FK_Luong_ChamCong
        FOREIGN KEY (ChamCongId) REFERENCES tblChamCong(Id)
);

INSERT INTO tblRole (TenRole, MoTa)
VALUES (N'User', N'Nhân viên th??ng');
INSERT INTO tblPhongBan (MaPB, TenPB)
VALUES (N'PB01', N'Phòng IT');
INSERT INTO tblChucVu (MaCV, TenCV, MaPB)
VALUES (N'DEV', N'Nhân viên', 1);
INSERT INTO tblNhanVien (MaNV, HoTen, NgaySinh, GioiTinh, MaCV)
VALUES
(N'NV001',N'Lê Tu?n Anh','2005-09-29',N'Nam',1),
(N'NV002',N'Nguy?n Nh?t Anh','2005-07-20',N'Nam',1),
(N'NV003',N'Ph?m Hoàng B?c','2005-12-15',N'Nam',1),
(N'NV004',N'Lô V?n Công','2005-07-25',N'Nam',1),
(N'NV005',N'?oàn V?n C??ng','2004-10-22',N'Nam',1),
(N'NV006',N'V? Duy Doanh','2005-10-19',N'Nam',1),
(N'NV007',N'Hoàng Ng?c Hùng D?ng','2005-01-20',N'Nam',1),
(N'NV008',N'?ào H?i D??ng','2005-03-07',N'Nam',1),
(N'NV009',N'Tr?n Vi?t D??ng','2005-02-07',N'Nam',1),
(N'NV010',N'Nguy?n ?ình ??ng','2005-11-08',N'Nam',1),
(N'NV011',N'Hoàng Anh ??c','2005-10-03',N'Nam',1),
(N'NV012',N'Ki?u Trung Giáp','2005-08-18',N'Nam',1),
(N'NV013',N'Tr?n Hoàng H?i','2005-07-22',N'Nam',1),
(N'NV014',N'V??ng Nh?t Hào','2005-11-30',N'Nam',1),
(N'NV015',N'?oàn Trung Hi?u','2005-03-11',N'Nam',1),
(N'NV016',N'?? Trung Hi?u','2005-12-30',N'Nam',1),
(N'NV017',N'Ph?m Huy Hi?u','2005-02-09',N'Nam',1),
(N'NV018',N'Nguy?n Xuân Hoàn','2005-09-20',N'Nam',1),
(N'NV019',N'??ng ?ình Hùng','2002-07-30',N'Nam',1),
(N'NV020',N'Nguy?n Xuân Hùng','2005-03-09',N'Nam',1),
(N'NV021',N'Nguy?n Quang Huy','2005-08-08',N'Nam',1),
(N'NV022',N'Nguy?n Quang Huy','2005-06-11',N'Nam',1),
(N'NV023',N'Nguy?n Th? H?ng','2005-05-19',N'Nam',1),
(N'NV024',N'V? Qu?c Khánh','2005-09-01',N'Nam',1),
(N'NV025',N'Nguy?n Hà Linh','2005-03-07',N'Nam',1),
(N'NV026',N'Tr?nh Xuân L?c','2005-11-05',N'Nam',1),
(N'NV027',N'Nguy?n V?n L?u','2005-06-06',N'Nam',1),
(N'NV028',N'Nguy?n Ph??ng Nam','2005-04-29',N'Nam',1),
(N'NV029',N'Ph?m V?n Nam','2005-12-02',N'Nam',1),
(N'NV030',N'Nguy?n Tr?ng Ngh?a','2004-05-24',N'Nam',1),
(N'NV031',N'Ngô V?n Quang','2005-02-12',N'Nam',1),
(N'NV032',N'Nguy?n S? Quân','2005-06-29',N'Nam',1),
(N'NV033',N'Tr?n Phú Quân','2005-01-16',N'Nam',1),
(N'NV034',N'V? Quang Sang','2005-03-09',N'Nam',1),
(N'NV035',N'Ph?m Minh S?n','2005-12-22',N'Nam',1),
(N'NV036',N'Nguy?n C? Thái','2005-02-10',N'Nam',1),
(N'NV037',N'Lê ??c Thanh','2005-02-25',N'Nam',1),
(N'NV038',N'Nguy?n V?n Thành','2005-10-03',N'Nam',1),
(N'NV039',N'Phùng Ti?n Thành','2005-10-07',N'Nam',1),
(N'NV040',N'?? M?nh Th?ng','2005-05-24',N'Nam',1),
(N'NV041',N'Nguy?n V?n Th??ng','2005-11-15',N'Nam',1),
(N'NV042',N'T?ng Xuân Toàn','2005-11-23',N'Nam',1),
(N'NV043',N'?? Anh Tú','2005-09-03',N'Nam',1),
(N'NV044',N'Nguy?n Nh? Tùng','2005-07-12',N'Nam',1),
(N'NV045',N'Nguy?n Quang Tùng','2005-07-28',N'Nam',1),
(N'NV046',N'C?n Xuân Vi?t','2005-11-05',N'Nam',1),
(N'NV047',N'Phi H?ng V??ng','2003-06-16',N'Nam',1);
INSERT INTO tblTaiKhoan (MaTK, MaNV, MatKhau, RoleId)
SELECT 
    'TK_' + MaNV,
    Id,
    '123456',
    1
FROM tblNhanVien;

/*ch?m công*/

DECLARE @nv INT, @date DATE;

DECLARE cur CURSOR FOR
SELECT Id FROM tblNhanVien;

OPEN cur;
FETCH NEXT FROM cur INTO @nv;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @date = '2025-01-01';

    WHILE @date <= '2025-12-31'
    BEGIN
        INSERT INTO tblChamCong (MaChamCong, Ngay, GioVao, GioVe, NhanVienId)
        VALUES (
            CONCAT('CC_', @nv, '_', FORMAT(@date,'yyyyMMdd')),
            @date,
            '08:00',
            '17:00',
            @nv
        );

        SET @date = DATEADD(DAY,1,@date);
    END

    FETCH NEXT FROM cur INTO @nv;
END

CLOSE cur;
DEALLOCATE cur;

/*luong*/
DELETE FROM tblLuong;

INSERT INTO tblLuong
(
    MaLuong,
    Thang,
    Nam,
    LuongCoBan,
    SoNgayCongChuan,
    PhuCap,
    KhauTru,
    ChamCongId
)
SELECT
    CONCAT(
        'L_',
        MONTH(cc.Ngay), '_',
        YEAR(cc.Ngay), '_',
        cc.NhanVienId
    ) AS MaLuong,

    MONTH(cc.Ngay) AS Thang,
    YEAR(cc.Ngay) AS Nam,
    7000000 AS LuongCoBan,
    26 AS SoNgayCongChuan,
    500000 AS PhuCap,
    0 AS KhauTru,
    MIN(cc.Id) AS ChamCongId   -- ?? ??i di?n 1 ngày trong tháng

FROM tblChamCong cc
GROUP BY
    cc.NhanVienId,
    YEAR(cc.Ngay),
    MONTH(cc.Ngay);
