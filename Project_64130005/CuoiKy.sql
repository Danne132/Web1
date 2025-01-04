create database Project_64130005;
use Project_64130005;


CREATE TABLE KhachHang(
	MaKH VARCHAR(10) PRIMARY KEY,
	HoKH NVARCHAR(50),
	TenKH NVARCHAR(50),
	Emai VARCHAR(100),
	MatKhau VARCHAR(255),
	AnhKH NVARCHAR(500),
	DiaChi NVARCHAR(500),
	NgaySinh DATE,
	CCCD VARCHAR(50),
	SDT VARCHAR(20)
)

CREATE TABLE LoaiNV(
	MaLoaiNV VARCHAR(20) PRIMARY KEY,
	TenLoaiNV NVARCHAR(50)
)


CREATE TABLE NhanVien(
	MaNV VARCHAR(20) PRIMARY KEY,
	HoTenNV NVARCHAR(100),
	Email VARCHAR(100),
	DiaChi NVARCHAR(500),
	NgaySinh DATE,
	GioiTinh BIT,
	SDT VARCHAR(20),
	AnhNV NVARCHAR(500),
	MatKhau VARCHAR(255),
	MaLoaiNV VARCHAR(20),
	CreatedAt DATE DEFAULT GETDATE(),
	UpDatedAt DATE
)


CREATE TABLE DonHang(
	MaDH VARCHAR(20) PRIMARY KEY,
	MaKH VARCHAR(20),
	TenNguoiNhan NVARCHAR(100),
	MaTrangThai NVARCHAR(50),
	DiaChi NVARCHAR(500),
	MaThanhToan VARCHAR(20),
	NgayDat DATE DEFAULT GETDATE(),
	NgayGiao DATE DEFAULT NULL,
	TongTien INT,
	TongSoLuong INT, 
	NVDuyetDon VARCHAR(20),
	NVGiaoHang VARCHAR(20),
)

CREATE TABLE ChiTietDH(
	MaDH VARCHAR(20),
	MaSach VARCHAR(20),
	SoLuong INT, 
	ThanhTien INT,
	PRIMARY KEY(MaDH, MaSach)
)

CREATE TABLE Sach(
	MaSach VARCHAR(20) PRIMARY KEY,
	MaTheLoai VARCHAR(20),
	AnhSach NVARCHAR(500),
	TenSach NVARCHAR(200),
	GiamGia INT, 
	DonGia INT,
	SoLuong INT,
	DaBan INT,
	NgayThem DATE DEFAULT GETDATE(),
	MaNgonNgu VARCHAR(20),
	MaNXB VARCHAR(20),
	MaTG INT,
	NgayChinhSua DATE
)


CREATE TABLE DanhGia(
	MaDG VARCHAR(20) PRIMARY KEY,
	Diem INT,
	BinhLuan VARCHAR(500),
	NgayDG DATE DEFAULT GETDATE(),
	MaSach VARCHAR(20),
	MaKH VARCHAR(20)
)

CREATE TABLE TheLoai(
	MaLoai VARCHAR(20) PRIMARY KEY,
	TenLoai NVARCHAR(100)
)

CREATE TABLE NhaXuatBan(
	MaNXB VARCHAR(20) PRIMARY KEY,
	TenNXB NVARCHAR(200) 
)

CREATE TABLE TacGia(
	MaTG INT IDENTITY(1,1) PRIMARY KEY,
	TenTG NVARCHAR(100)
)


CREATE TABLE ThanhToan(
	MaThanhToan VARCHAR(20) PRIMARY KEY,
	TenThanhToan NVARCHAR(100)
)

CREATE TABLE NgonNgu(
	MaNgonNgu VARCHAR(20) PRIMARY KEY,
	TenNgonNgu NVARCHAR(100)
)

CREATE TABLE TrangThaiDH(
	TrangThai NVARCHAR(50) PRIMARY KEY
)

Go
CREATE TRIGGER trg_update_sach
ON Sach
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Sach
    SET NgayChinhSua = GETDATE()
    FROM Sach
    INNER JOIN Inserted ON Sach.MaSach = Inserted.MaSach;
END;

INSERT INTO TheLoai(MaLoai, TenLoai) VALUES
('DD', N'Văn Học Đương Đại'),
('CD', N'Văn Học Cổ Điển'),
('TN', N'Truyện Ngắn'),
('TC', N'Tình Cảm'),
('TT', N'Trinh Thám'),
('PS', N'Phóng Sự')

INSERT INTO NgonNgu(MaNgonNgu, TenNgonNgu) VALUES
('VI', N'Tiếng Việt'),
('EN', N'Tiếng Anh')

INSERT INTO NhaXuatBan(MaNXB, TenNXB) VALUES 
('PN', N'Phương Nam'),
('NT', N'NXB Trẻ'),
('KD', N'NXB Kim Đồng'),
('LD', N'NXB Lao Động'),
('DN', N'NXB Đà Nẵng'),
('VH', N'NXB Văn Học'),
('NV', N'NXB Hội Nhà Văn'),
('PNU',N'NXB Phụ Nữ'),
('TN', N'NXB Thanh Niên'),
('DT', N'NXB Dân Trí')

INSERT INTO TacGia(TenTG) VALUES
(N'Nhiều tác giả'),
(N'Miura Ayako'),
(N'Mikhail Bulgakov'),
(N'Alessandro Baricco'),
(N'Lisa Jewell'),
(N'Jack London'),
(N'Fyodor Dostoevsky'),
(N'Mario Puzo'),
(N'Lucy Maud Montgomery'),
(N'Dazai Osamu'),
(N'Cố Mạn'),
(N'Vu Triết'),
(N'Tào Đình'),
(N'Thời Kính'),
(N'Hồng Thái'),
(N'Jane Harper')

INSERT INTO TrangThaiDH VALUES
(N'Chưa giao hàng'),
(N'Đang giao hàng'),
(N'Đã hủy'),
(N'Hoàn thành')