-- 1. Tạo Database mới

CREATE DATABASE WebApp_Manage
go
use WebApp_Manage
go 

-- ========================
-- 1. Bảng Phân quyền (Roles)
-- ========================
CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

-- ========================
-- 2. Bảng Nhân viên/Tài khoản (Accounts)
-- ========================
CREATE TABLE Accounts (
    AccountID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FullName NVARCHAR(100),
    Email NVARCHAR(100),
    RoleID INT NOT NULL,
    IsActive BIT DEFAULT 1,
    LastLogin DATETIME2,
    CreatedDate DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Account_Role 
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- ========================
-- 3.Loại thiết bị
-- ========================
CREATE TABLE DeviceTypes (
    TypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(200)
);

-- ========================
-- 4.  Vị trí
-- ========================
CREATE TABLE Locations (
    LocationID INT IDENTITY(1,1) PRIMARY KEY,
    Building NVARCHAR(100) NOT NULL,
    Floor NVARCHAR(50),
    Room NVARCHAR(50),
    Rack NVARCHAR(50)
);

-- ========================
-- 5. Bảng Thiết bị (Devices)
-- ========================
CREATE TABLE Devices (
    DeviceID INT IDENTITY(1,1) PRIMARY KEY,
    DeviceName NVARCHAR(100) NOT NULL,
    DeviceTypeID INT NOT NULL,
    LocationID INT NULL,
    Manufacturer NVARCHAR(100),
    Model NVARCHAR(100),
    SerialNumber NVARCHAR(100) UNIQUE,
    MACAddress NVARCHAR(17) UNIQUE,
    FirmwareVersion NVARCHAR(50),
    Status NVARCHAR(50) DEFAULT 'Offline',
    CreatedDate DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Device_Type 
    FOREIGN KEY (DeviceTypeID) REFERENCES DeviceTypes(TypeID),

    CONSTRAINT FK_Device_Location 
    FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
);

-- ========================
-- 6. Bảng Cấu hình mạng chi tiết (IP, Subnet, Gateway, DNS)
-- ========================
CREATE TABLE NetworkConfigs (
    ConfigID INT IDENTITY(1,1) PRIMARY KEY,
    DeviceID INT NOT NULL UNIQUE,
    IPAddress NVARCHAR(45) NOT NULL,
    SubnetMask NVARCHAR(45),
    DefaultGateway NVARCHAR(45),
    PrimaryDNS NVARCHAR(45),
    SecondaryDNS NVARCHAR(45),
    UpdatedDate DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Config_Device 
    FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID) ON DELETE CASCADE
);

-- ========================
-- 7. Bảng Nhật ký hoạt động nhân viên
-- ========================
CREATE TABLE ActionLogs (
    LogID BIGINT IDENTITY(1,1) PRIMARY KEY,
    AccountID INT NOT NULL,
    ActionType NVARCHAR(50),
    Description NVARCHAR(MAX),
    DeviceID INT NULL,
    LogTime DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Log_Account 
    FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID),

    CONSTRAINT FK_Log_Device 
    FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID)
);

-- ========================
-- 8. Cảnh báo phiên bản
-- ========================
CREATE TABLE FirmwareStandards (
    TypeID INT PRIMARY KEY,
    LatestVersion NVARCHAR(50),
    ReleaseDate DATETIME,

    CONSTRAINT FK_Std_Type 
    FOREIGN KEY (TypeID) REFERENCES DeviceTypes(TypeID)
);

-- 9 VLANs--

CREATE TABLE VLANs (
    VLANID INT PRIMARY KEY,
    VLANName NVARCHAR(50) NOT NULL,
    SubnetMask NVARCHAR(50),
    GatewayIP NVARCHAR(45),
    Status BIT DEFAULT 1
); 

--10 DeviceVLANs--q
CREATE TABLE DeviceVLANs (
    DeviceID INT NOT NULL,
    VLANID INT NOT NULL,
    AssignmentDate DATETIME2 DEFAULT GETDATE(),

    PRIMARY KEY (DeviceID, VLANID),
    CONSTRAINT FK_DV_Device FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID) ON DELETE CASCADE,
    CONSTRAINT FK_DV_VLAN FOREIGN KEY (VLANID) REFERENCES VLANs(VLANID) ON DELETE CASCADE
);

--11. Port --

USE WebApp_Manage;
GO

-- ========================
-- 11. Bảng Quản lý Cổng (Ports)
-- ========================
CREATE TABLE Ports (
    PortID INT IDENTITY(1,1) PRIMARY KEY,
    DeviceID INT NOT NULL,             
    PortNumber INT NOT NULL,           
    PortName NVARCHAR(50),              
    Status NVARCHAR(20) DEFAULT 'Down', 
    VLANID INT NULL,                    
    ConnectedToDeviceID INT NULL,      
    Description NVARCHAR(200),        
    UpdatedDate DATETIME2 DEFAULT GETDATE(),

    -- Khóa ngoại liên kết tới Devices
    CONSTRAINT FK_Port_Device FOREIGN KEY (DeviceID) 
        REFERENCES Devices(DeviceID) ON DELETE CASCADE,

    -- Khóa ngoại liên kết tới VLANs
    CONSTRAINT FK_Port_VLAN FOREIGN KEY (VLANID) 
        REFERENCES VLANs(VLANID) ON DELETE SET NULL,

    -- Khóa ngoại tự tham chiếu tới Devices (kết nối giữa 2 thiết bị)
    CONSTRAINT FK_Port_ConnectedDevice FOREIGN KEY (ConnectedToDeviceID) 
        REFERENCES Devices(DeviceID)
);
GO
-- . Thêm dữ liệu mẫu chuẩn cho hạ tầng mạng
INSERT INTO VLANs (VLANID, VLANName, SubnetMask, GatewayIP, Status)
VALUES 
(10, N'VLAN Sales', '255.255.255.0', '192.168.10.1', 1),
(20, N'VLAN HR', '255.255.255.0', '192.168.20.1', 1),
(30, N'VLAN IT', '255.255.255.0', '192.168.30.1', 1),
(40, N'VLAN Guest', '255.255.255.0', '192.168.40.1', 0),
(50, N'VLAN Server', '255.255.255.0', '192.168.50.1', 1),
(60, N'VLAN Camera', '255.255.255.0', '192.168.60.1', 1),
(70, N'VLAN Voice', '255.255.255.0', '192.168.70.1', 1);

INSERT INTO DeviceVLANs (DeviceID, VLANID)
VALUES 
(1, 10), 
(1, 20), 
(2, 10),  
(3, 20); 

USE WebApp_Manage;
GO

-- Bước 1: Xóa liên kết thiết bị trong bảng trung gian (nếu có)
DELETE FROM DeviceVLANs WHERE VLANID IN (10, 20, 30, 40, 50, 60, 70);

-- Bước 2: Cập nhật các bản ghi NetworkConfigs đang trỏ về các VLAN này thành NULL
UPDATE NetworkConfigs SET VLANID = NULL WHERE VLANID IN (10, 20, 30, 40, 50, 60, 70);

-- Bước 3: Xóa chính các VLAN đó
DELETE FROM VLANs WHERE VLANID IN (10, 20, 30, 40, 50, 60, 70);
GO

--role--
INSERT INTO Roles (RoleName)
VALUES 
('Admin'),
('Technician'),
('Viewer');

--account--
INSERT INTO Accounts (Username, PasswordHash, FullName, Email, RoleID)
VALUES 
('admin', '123456', 'Nguyen Van Admin', 'admin@gmail.com', 1),
('tech1', '123456', 'Tran Van Tech', 'tech@gmail.com', 2),
('viewer1', '123456', 'Le Thi Viewer', 'viewer@gmail.com', 3);

-- devicce type--
INSERT INTO DeviceTypes (TypeName, Description)
VALUES
('Router', N'Thiết bị định tuyến'),
('Switch', N'Thiết bị chuyển mạch'),
('Server', N'Máy chủ'),
('Firewall', N'Tường lửa'),
('Access Point', 'Wifi');

-- location--
INSERT INTO Locations (Building, Floor, Room, Rack)
VALUES
(N'Tòa A', '1', '101', 'Rack 1'),
(N'Tòa A', '2', '201', 'Rack 2'),
(N'Tòa B', '1', 'B101', 'Rack 3'),
('Data Center', N'Tầng 1', 'DC1', 'Rack Core');

-- divice--
INSERT INTO Devices 
(DeviceName, DeviceTypeID, LocationID, Manufacturer, Model, SerialNumber, MACAddress, FirmwareVersion, Status)
VALUES
('Router Cisco 2901', 1, 1, 'Cisco', '2901', 'SN001', 'AA:BB:CC:DD:EE:01', '1.0.0', 'Online'),

('Switch TP-Link 24 Port', 2, 2, 'TP-Link', 'TL-SG1024', 'SN002', 'AA:BB:CC:DD:EE:02', '2.1.0', 'Offline'),

('Server Dell R740', 3, 4, 'Dell', 'R740', 'SN003', 'AA:BB:CC:DD:EE:03', '3.0.2', 'Online'),

('Firewall Fortigate', 4, 4, 'Fortinet', 'FG-100E', 'SN004', 'AA:BB:CC:DD:EE:04', '6.4.5', 'Maintenance'),

('Access Point Unifi', 5, 3, 'Ubiquiti', 'UAP-AC', 'SN005', 'AA:BB:CC:DD:EE:05', '5.6.7', 'Online')

-- network config--
INSERT INTO NetworkConfigs 
(DeviceID, IPAddress, SubnetMask, DefaultGateway, PrimaryDNS, SecondaryDNS)
VALUES
(1, '192.168.1.1', '255.255.255.0', '192.168.1.254', '8.8.8.8', '8.8.4.4'),
(2, '192.168.1.2', '255.255.255.0', '192.168.1.254', '8.8.8.8', '1.1.1.1'),
(3, '192.168.1.10', '255.255.255.0', '192.168.1.254', '8.8.8.8', NULL);

--logs--
INSERT INTO ActionLogs (AccountID, ActionType, Description, DeviceID)
VALUES
(1, 'LOGIN', N'Admin đăng nhập hệ thống', NULL),
(2, 'CHANGE_IP', N'Đổi IP Router', 1),
(2, 'REBOOT_DEVICE', N'Khởi động lại Switch', 2),
(3, 'VIEW', N'Xem danh sách thiết bị', NULL);

--cảnh báo phiên bản--
INSERT INTO FirmwareStandards (TypeID, LatestVersion, ReleaseDate)
VALUES
(1, '1.2.0', GETDATE()),
(2, '2.5.0', GETDATE()),
(3, '3.1.0', GETDATE());
 -- thông tin thiết bị mạng--
USE WebApp_Manage; -- Quan trọng: Trỏ về đúng DB của bạn
GO

SELECT 
    d.DeviceID, 
    d.DeviceName, 
    dt.TypeName, 
    l.Room, 
    nc.IPAddress, 
    fs.LatestVersion
FROM Devices d
LEFT JOIN DeviceTypes dt ON d.DeviceTypeID = dt.TypeID
LEFT JOIN Locations l ON d.LocationID = l.LocationID
LEFT JOIN NetworkConfigs nc ON d.DeviceID = nc.DeviceID
LEFT JOIN FirmwareStandards fs ON d.DeviceTypeID = fs.TypeID;
-- kiểm tra role--
USE WebApp_Manage; 
GO
SELECT * FROM Roles;

-- cập nhật thông tin cho Port --
USE WebApp_Manage;
GO

-- Xóa dữ liệu cũ nếu có để tránh trùng lặp khi chạy lại
DELETE FROM Ports WHERE DeviceID = 1;

-- Chèn 24 cổng cho Switch ID 1
DECLARE @i INT = 1;
WHILE @i <= 24
BEGIN
    INSERT INTO Ports (DeviceID, PortNumber, PortName, Status, VLANID, Description)
    VALUES (
        1, 
        @i, 
        'GigabitEthernet0/' + CAST(@i AS NVARCHAR), 
        CASE 
            WHEN @i <= 5 THEN 'Up'      -- 5 cổng đầu đang hoạt động
            WHEN @i = 24 THEN 'Up'      -- Cổng 24 làm cổng Uplink
            ELSE 'Down'                 -- Các cổng còn lại trống
        END,
        CASE 
            WHEN @i <= 10 THEN 10       -- Cổng 1-10 thuộc VLAN 10 (Kế toán)
            WHEN @i <= 20 THEN 20       -- Cổng 11-20 thuộc VLAN 20 (Kỹ thuật)
            ELSE 1                      -- Cổng còn lại là VLAN 1 (Mặc định)
        END,
        CASE 
            WHEN @i = 1 THEN N'Kết nối Server Dell R740'
            WHEN @i = 2 THEN N'Máy in phòng kế toán'
            WHEN @i = 24 THEN N'Uplink tới Router Cisco'
            ELSE NULL
        END
    );
    SET @i = @i + 1;
END;
GO

USE WebApp_Manage;
GO

-- Xóa ràng buộc Unique trên cột SerialNumber
-- Lưu ý: Nếu SQL báo không tìm thấy tên này, bạn hãy xem thông báo lỗi 
-- để lấy tên chính xác của Constraint (thường bắt đầu bằng UQ__Devices...)
ALTER TABLE Devices DROP CONSTRAINT IF EXISTS UQ__Devices__SerialNumber; 

-- Xóa ràng buộc Unique trên cột MACAddress
ALTER TABLE Devices DROP CONSTRAINT IF EXISTS UQ__Devices__MACAddress;

-- Tạo Index duy nhất cho SerialNumber nhưng bỏ qua các dòng NULL
CREATE UNIQUE NONCLUSTERED INDEX UX_Devices_SerialNumber_NotNull
ON Devices(SerialNumber)
WHERE SerialNumber IS NOT NULL;

-- Tạo Index duy nhất cho MACAddress nhưng bỏ qua các dòng NULL
CREATE UNIQUE NONCLUSTERED INDEX UX_Devices_MACAddress_NotNull
ON Devices(MACAddress)
WHERE MACAddress IS NOT NULL;
GO

USE WebApp_Manage;
GO

-- 1. Xóa các ràng buộc Unique cũ (Tìm tên trong bảng Keys của table Devices)
ALTER TABLE Devices DROP CONSTRAINT IF EXISTS UQ__Devices__SerialNumber; 
ALTER TABLE Devices DROP CONSTRAINT IF EXISTS UQ__Devices__MACAddress;
GO

-- 2. Tạo luật mới: Chỉ bắt trùng nếu CÓ NHẬP, còn để trống (NULL) thì bao nhiêu cũng được
CREATE UNIQUE NONCLUSTERED INDEX UX_Devices_SN_Optional ON Devices(SerialNumber) WHERE SerialNumber IS NOT NULL;
CREATE UNIQUE NONCLUSTERED INDEX UX_Devices_MAC_Optional ON Devices(MACAddress) WHERE MACAddress IS NOT NULL;
GO

USE WebApp_Manage;
GO

-- 1. Thêm các cột kỹ thuật mạng vào bảng VLANs
ALTER TABLE VLANs
ADD 
    NetworkAddress NVARCHAR(45) NULL, -- Địa chỉ mạng (VD: 192.168.1.0)
    CIDR INT NULL,                   -- Số bit mạng (VD: 24)
    Description NVARCHAR(200) NULL;  -- Ghi chú nghiệp vụ
GO

-- 2. Cập nhật dữ liệu mẫu cho các VLAN hiện có (Giả sử mặc định là /24)
-- VLAN Sales
UPDATE VLANs SET 
    NetworkAddress = '192.168.10.0', 
    CIDR = 24, 
    GatewayIP = '192.168.10.1' 
WHERE VLANID = 10;

-- VLAN IT
UPDATE VLANs SET 
    NetworkAddress = '192.168.30.0', 
    CIDR = 24, 
    GatewayIP = '192.168.30.1' 
WHERE VLANID = 30;
GO

-- 1. Thêm cột VLANID vào bảng cấu hình mạng của thiết bị
ALTER TABLE NetworkConfigs
ADD VLANID INT NULL;
GO

-- 2. Tạo khóa ngoại để đảm bảo tính toàn vẹn
ALTER TABLE NetworkConfigs
ADD CONSTRAINT FK_NetworkConfig_VLAN 
FOREIGN KEY (VLANID) REFERENCES VLANs(VLANID);
GO

-- 3. Cập nhật dữ liệu thực tế (Ví dụ: Thiết bị 1 thuộc VLAN 10)
UPDATE NetworkConfigs SET VLANID = 10 WHERE DeviceID = 1;
UPDATE NetworkConfigs SET VLANID = 30 WHERE DeviceID = 3;
GO

USE WebApp_Manage;
GO

-- 1. Kiểm tra và xóa Index cũ nếu nó đã tồn tại (để tránh lỗi 1913)
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UX_Devices_SerialNumber_Optional' AND object_id = OBJECT_ID('Devices'))
BEGIN
    DROP INDEX UX_Devices_SerialNumber_Optional ON Devices;
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UX_Devices_MACAddress_NotNull' AND object_id = OBJECT_ID('Devices'))
BEGIN
    DROP INDEX UX_Devices_MACAddress_NotNull ON Devices;
END
GO

-- 2. Bây giờ mới tạo lại Index thông minh: Cho phép để trống (NULL) thoải mái
CREATE UNIQUE NONCLUSTERED INDEX UX_Devices_SerialNumber_Optional
ON Devices(SerialNumber)
WHERE SerialNumber IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX UX_Devices_MACAddress_NotNull
ON Devices(MACAddress)
WHERE MACAddress IS NOT NULL;
GO

USE WebApp_Manage;
GO

-- 1. Thêm cột đánh dấu: 0 = Vật lý, 1 = Máy ảo
ALTER TABLE Devices ADD IsVirtual BIT DEFAULT 0;

-- 2. Thêm cột ID của máy chủ chứa nó (Host)
ALTER TABLE Devices ADD ParentDeviceId INT NULL;

-- 3. Tạo liên kết nội bộ trong bảng Devices
ALTER TABLE Devices ADD CONSTRAINT FK_Device_Parent 
FOREIGN KEY (ParentDeviceId) REFERENCES Devices(DeviceId);
GO

USE WebApp_Manage;
GO

-- 1. CÁC CỘT CHO QUẢN LÝ TÀI NGUYÊN (CAPACITY)
ALTER TABLE Devices ADD CpuCores INT NULL;      -- Số lõi CPU (VD: 8, 16, 32)
ALTER TABLE Devices ADD RamGB INT NULL;         -- Dung lượng RAM tính bằng GB (VD: 16, 64)
ALTER TABLE Devices ADD StorageGB INT NULL;     -- Ổ cứng tính bằng GB (VD: 500, 1024)
GO

-- 2. CÁC CỘT CHO QUẢN LÝ CẮM CÁP MẠNG (CABLING)
-- Cắm vào thiết bị nào? (Trỏ về ID của Switch/Router)
ALTER TABLE Devices ADD UplinkDeviceId INT NULL; 
-- Cắm vào Port số mấy? (VD: Port 05, Gi0/1)
ALTER TABLE Devices ADD UplinkPort NVARCHAR(50) NULL; 

-- Tạo liên kết an toàn cho cáp mạng
ALTER TABLE Devices ADD CONSTRAINT FK_Device_Uplink 
FOREIGN KEY (UplinkDeviceId) REFERENCES Devices(DeviceId);
GO


-- them chuc nang moi--
USE WebApp_Manage;
GO

-- =======================================================
-- 1. BẢNG LỊCH TRỰC (DutySchedules)
-- Phục vụ: Phân công Admin/Kỹ thuật viên trực ca
-- =======================================================
CREATE TABLE DutySchedules (
    ScheduleID INT IDENTITY(1,1) PRIMARY KEY,
    ShiftDate DATE NOT NULL,               -- Ngày trực
    ShiftType NVARCHAR(50) NOT NULL,       -- Ca Ngày / Ca Đêm
    Site NVARCHAR(100) NOT NULL,           -- Site Mỹ Tho / Site Cao Lãnh
    AccountID INT NOT NULL,                -- User được phân công trực
    CreatedBy INT NULL,                    -- Admin nào đã tạo lịch này
    CreatedDate DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Duty_Account FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID),
    CONSTRAINT FK_Duty_Creator FOREIGN KEY (CreatedBy) REFERENCES Accounts(AccountID)
);
GO

-- =======================================================
-- 2. BẢNG BÁO CÁO SỰ CỐ (IncidentReports)
-- Phục vụ: Số hóa MẪU SỐ 1 trong file Word
-- =======================================================
CREATE TABLE IncidentReports (
    ReportID INT IDENTITY(1,1) PRIMARY KEY,
    DetectedTime DATETIME2 NOT NULL,       -- Thời gian phát hiện sự cố
    ReporterID INT NOT NULL,               -- Người phát hiện (Liên kết bảng Accounts)
    DeviceID INT NULL,                     -- Hệ thống/Thiết bị bị ảnh hưởng (Liên kết bảng Devices)
    Site NVARCHAR(100),                    -- Vị trí (Mỹ Tho / Cao Lãnh)
    Severity NVARCHAR(50),                 -- Mức độ (Nghiêm trọng / Trung bình / Nhẹ)
    
    -- Nội dung sự cố
    Description NVARCHAR(MAX) NOT NULL,    -- Mô tả sự cố
    ImpactScope NVARCHAR(MAX),             -- Phạm vi ảnh hưởng
    
    -- Quá trình xử lý
    RootCause NVARCHAR(MAX),               -- Nguyên nhân
    ActionsTaken NVARCHAR(MAX),            -- Các biện pháp đã xử lý
    FixTime DATETIME2 NULL,                -- Thời gian khắc phục xong
    
    -- Trạng thái và Kiến nghị
    Status NVARCHAR(100) DEFAULT N'Đang theo dõi', -- Đã hoạt động bình thường / Đang tiếp tục theo dõi
    PreventiveMeasures NVARCHAR(MAX),      -- Giải pháp phòng ngừa
    
    CreatedDate DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Incident_Reporter FOREIGN KEY (ReporterID) REFERENCES Accounts(AccountID),
    CONSTRAINT FK_Incident_Device FOREIGN KEY (DeviceID) REFERENCES Devices(DeviceID)
);
GO

-- =======================================================
-- 3. BẢNG BIÊN BẢN BÀN GIAO CA (ShiftHandovers)
-- Phục vụ: Số hóa MẪU SỐ 2 trong file Word
-- =======================================================
CREATE TABLE ShiftHandovers (
    HandoverID INT IDENTITY(1,1) PRIMARY KEY,
    HandoverTime DATETIME2 NOT NULL,       -- Thời điểm bàn giao
    FromAccountID INT NOT NULL,            -- Người bàn giao
    ToAccountID INT NOT NULL,              -- Người nhận bàn giao
    ShiftType NVARCHAR(50),                -- Ca Ngày / Ca Đêm
    Site NVARCHAR(100),                    -- Site Mỹ Tho / Cao Lãnh
    
    -- Tình trạng hệ thống tại thời điểm giao (Bình thường / Có sự cố)
    ServerStatus NVARCHAR(50) DEFAULT N'Bình thường', 
    NetworkStatus NVARCHAR(50) DEFAULT N'Bình thường',
    StorageStatus NVARCHAR(50) DEFAULT N'Bình thường',
    SoftwareStatus NVARCHAR(50) DEFAULT N'Bình thường',
    PowerCoolingStatus NVARCHAR(50) DEFAULT N'Bình thường',
    
    PendingTasks NVARCHAR(MAX),            -- Các công việc cần làm tiếp
    CreatedDate DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Handover_FromAccount FOREIGN KEY (FromAccountID) REFERENCES Accounts(AccountID),
    CONSTRAINT FK_Handover_ToAccount FOREIGN KEY (ToAccountID) REFERENCES Accounts(AccountID)
);
GO

-- =======================================================
-- 4. BẢNG THÔNG BÁO (Notifications)
-- Phục vụ: Đẩy thông báo khi có lịch trực mới hoặc sự cố
-- =======================================================
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    AccountID INT NOT NULL,                -- User nhận thông báo
    Title NVARCHAR(200) NOT NULL,          -- Tiêu đề (VD: Bạn có lịch trực mới)
    Message NVARCHAR(MAX) NOT NULL,        -- Nội dung chi tiết
    ActionLink NVARCHAR(200) NULL,         -- Đường dẫn để click vào (VD: /schedules)
    IsRead BIT DEFAULT 0,                  -- Đã đọc chưa? (0 = Chưa, 1 = Rồi)
    CreatedDate DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT FK_Notification_Account FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID)
);
GO