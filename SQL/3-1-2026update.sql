DELETE FROM tblChamCong;
DELETE FROM tblChiTietDuAn;
DELETE FROM tblLuong;
DELETE FROM tblHopDong;
DELETE FROM tblTaiKhoan;
DELETE FROM tblNhanVien;
DELETE FROM tblChucVu;
DELETE FROM tblPhongBan;

ALTER TABLE tblChucVu
ADD MaPB VARCHAR(10) NOT NULL;
GO

ALTER TABLE tblChucVu
ADD CONSTRAINT FK_ChucVu_PhongBan
FOREIGN KEY (MaPB) REFERENCES tblPhongBan(MaPB);
GO

SELECT 
    fk.name AS FK_Name,
    tp.name AS TableName,
    tr.name AS RefTable
FROM sys.foreign_keys fk
JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
WHERE tp.name = 'tblNhanVien';

ALTER TABLE tblNhanVien
DROP CONSTRAINT FK__tblNhanVie__MaPB__5441852A; -- n?u SQL Server t? sinh tên khác thì xem trong SSMS
GO

ALTER TABLE tblNhanVien
DROP COLUMN MaPB;
GO
SELECT 
    dc.name AS DefaultName
FROM sys.default_constraints dc
JOIN sys.columns c 
    ON dc.parent_object_id = c.object_id 
    AND dc.parent_column_id = c.column_id
WHERE OBJECT_NAME(dc.parent_object_id) = 'tblLuong'
  AND c.name = 'SoNgayCong';

  ALTER TABLE tblLuong
DROP CONSTRAINT DF__tblLuong__SoNgay__18EBB532;

ALTER TABLE tblLuong
DROP COLUMN SoNgayCong;
GO

ALTER TABLE tblLuong
DROP COLUMN TongLuong;
GO

ALTER TABLE tblLuong
ADD NhanVienId INT NULL;
GO
ALTER TABLE tblLuong
ADD CONSTRAINT FK_tblLuong_tblNhanVien
FOREIGN KEY (NhanVienId)
REFERENCES tblNhanVien(Id);
GO

SELECT name
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('tblLuong');
ALTER TABLE tblLuong
DROP CONSTRAINT FK__tblLuong__MaNV__1CBC4616;

ALTER TABLE tblLuong
ADD NhanVienId INT NULL;

-- ch? ch?y khi b?n ?ã map xong ID
ALTER TABLE tblLuong
DROP COLUMN MaNV;
GO

UPDATE l
SET NhanVienId = nv.Id
FROM tblLuong l
JOIN tblNhanVien nv ON l.MaNV = nv.MaNV;
ALTER TABLE tblLuong
ALTER COLUMN NhanVienId INT NOT NULL;
ALTER TABLE tblLuong
ADD CONSTRAINT FK_tblLuong_NhanVien
FOREIGN KEY (NhanVienId) REFERENCES tblNhanVien(Id);

ALTER TABLE tblLuong
DROP COLUMN MaNV;

SELECT *
FROM tblLuong l
JOIN tblNhanVien nv ON l.NhanVienId = nv.Id;

SELECT 
    fk.name AS FK_Name,
    c.name AS ColumnName
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
JOIN sys.columns c 
    ON fkc.parent_object_id = c.object_id 
    AND fkc.parent_column_id = c.column_id
WHERE fk.parent_object_id = OBJECT_ID('tblLuong');
ALTER TABLE tblLuong
DROP CONSTRAINT FK_tblLuong_tblNhanVien;



SELECT *
FROM tblLuong
WHERE NhanVienId IS NULL;

SELECT 
    l.Id,
    nv.MaNV,
    nv.HoTen,
    l.Thang,
    l.Nam,
    l.TongLuong
FROM tblLuong l
JOIN tblNhanVien nv ON l.NhanVienId = nv.Id;
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tblLuong';

CREATE OR ALTER VIEW vw_LuongNhanVien
AS
SELECT
    l.Id AS LuongId,
    nv.Id AS NhanVienId,
    nv.MaNV,
    nv.HoTen,
    l.Thang,
    l.Nam,
    l.LuongCoBan,
    COUNT(cc.Id) AS SoNgayCong,
    l.PhuCap,
    l.KhauTru,
    (l.LuongCoBan / l.SoNgayCongChuan) * COUNT(cc.Id)
        + l.PhuCap - l.KhauTru AS TongLuong
FROM tblLuong l
JOIN tblNhanVien nv ON l.NhanVienId = nv.Id
LEFT JOIN tblChamCong cc 
    ON cc.NhanVienId = nv.Id
    AND MONTH(cc.Ngay) = l.Thang
    AND YEAR(cc.Ngay) = l.Nam
GROUP BY
    l.Id, nv.Id, nv.MaNV, nv.HoTen,
    l.Thang, l.Nam,
    l.LuongCoBan, l.SoNgayCongChuan,
    l.PhuCap, l.KhauTru;
	ALTER TABLE tblChamCong
ADD NhanVienId INT NULL;
UPDATE cc
SET NhanVienId = nv.Id
FROM tblChamCong cc
JOIN tblNhanVien nv ON cc.MaNV = nv.MaNV;
SELECT * 
FROM tblChamCong
WHERE NhanVienId IS NULL;
SELECT name 
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('tblChamCong');
ALTER TABLE tblChamCong
DROP CONSTRAINT FK__tblChamCon__MaNV__6FE99F9F;
ALTER TABLE tblChamCong
DROP COLUMN MaNV;

ALTER TABLE tblChamCong
ALTER COLUMN NhanVienId INT NOT NULL;
ALTER TABLE tblChamCong
ADD CONSTRAINT FK_tblChamCong_NhanVien
FOREIGN KEY (NhanVienId)
REFERENCES tblNhanVien(Id);

CREATE OR ALTER VIEW vw_LuongNhanVien
AS
SELECT
    l.Id AS LuongId,
    nv.Id AS NhanVienId,
    nv.MaNV,
    nv.HoTen,
    l.Thang,
    l.Nam,
    l.LuongCoBan,
    COUNT(cc.Id) AS SoNgayCong,
    l.PhuCap,
    l.KhauTru,
    (l.LuongCoBan / l.SoNgayCongChuan) * COUNT(cc.Id)
        + l.PhuCap - l.KhauTru AS TongLuong
FROM tblLuong l
JOIN tblNhanVien nv ON l.NhanVienId = nv.Id
LEFT JOIN tblChamCong cc 
    ON cc.NhanVienId = nv.Id
    AND MONTH(cc.Ngay) = l.Thang
    AND YEAR(cc.Ngay) = l.Nam
GROUP BY
    l.Id, nv.Id, nv.MaNV, nv.HoTen,
    l.Thang, l.Nam,
    l.LuongCoBan, l.SoNgayCongChuan,
    l.PhuCap, l.KhauTru;
	SELECT * FROM vw_LuongNhanVien;
