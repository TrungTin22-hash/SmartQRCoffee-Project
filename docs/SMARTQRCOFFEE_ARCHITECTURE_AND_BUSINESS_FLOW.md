# SmartQRCoffee - Kiến Trúc Tổng Quan & Flow Nghiệp Vụ

## 1. Mục tiêu tài liệu

Tài liệu này mô tả:

- kiến trúc tổng quan của hệ thống `SmartQRCoffee`
- vai trò của từng layer trong solution
- các thành phần chính của backend
- mô hình dữ liệu cốt lõi
- flow nghiệp vụ chính đang có trong project
- hướng mở rộng cho demo, đồ án và triển khai thực tế

---

## 2. Tổng quan dự án

`SmartQRCoffee` là hệ thống hỗ trợ quy trình gọi món bằng QR code trong quán cà phê.

Mục tiêu chính của hệ thống:
- khách hàng quét QR tại bàn để xem menu
- khách hàng tự tạo order từ thiết bị cá nhân
- hệ thống gửi thông báo realtime cho khu vực bếp / pha chế
- nhân viên cập nhật trạng thái đơn hàng
- khách hàng nhận được cập nhật trạng thái theo thời gian thực
- hệ thống hỗ trợ đăng nhập nhân viên bằng JWT + Refresh Token + HttpOnly Cookies

---

## 3. Kiến trúc tổng quan hệ thống

## 3.1. Mô hình kiến trúc

Dự án hiện được xây dựng theo mô hình:

- Client - Server
- 3-Layer Architecture
- RESTful API + SignalR realtime

### Các tầng chính

#### 1. Presentation Layer
Bao gồm:
- API Controllers trong `SmartQRCoffee.API`
- client web/mobile tương lai
- SignalR Hub để realtime communication

Nhiệm vụ:
- nhận request từ client
- trả response JSON
- xác thực và phân quyền người dùng
- cung cấp endpoint realtime cho kitchen và customer view

#### 2. Business Logic Layer
Bao gồm:
- service interfaces và implementations trong `SmartQRCoffee.Services`
- DTOs phục vụ request/response

Nhiệm vụ:
- xử lý quy tắc nghiệp vụ
- kiểm tra token bàn
- kiểm tra stock sản phẩm
- tính tổng order
- phát hành JWT và refresh token
- gọi notification service khi có sự kiện nghiệp vụ

#### 3. Data Access Layer
Bao gồm:
- `DbContext`
- entities/models
- repository interfaces và implementations trong `SmartQRCoffee.Repositories`
- EF Core migrations

Nhiệm vụ:
- giao tiếp với PostgreSQL / Supabase
- CRUD dữ liệu
- truy vấn entity
- quản lý migration và schema

---

## 3.2. Sơ đồ logic tổng quát

```text
Customer / Staff Client
        |
        v
SmartQRCoffee.API
  - Controllers
  - Auth Middleware
  - SignalR Hub
        |
        v
SmartQRCoffee.Services
  - UserService
  - JwtTokenService
  - TableService
  - OrderService
  - NotificationService
        |
        v
SmartQRCoffee.Repositories
  - Repositories
  - DbContext
  - EF Models
  - Migrations
        |
        v
PostgreSQL / Supabase
```

---

## 4. Cấu trúc solution hiện tại

Solution gồm 3 project chính:

### 4.1. `SmartQRCoffee.API`
Vai trò:
- project khởi chạy chính
- chứa controllers
- chứa `Program.cs`
- cấu hình DI, JWT, CORS, Cookie, SignalR
- chứa hub SignalR và client test realtime

### 4.2. `SmartQRCoffee.Services`
Vai trò:
- xử lý business logic
- định nghĩa DTO
- tách logic nghiệp vụ khỏi controller và repository

### 4.3. `SmartQRCoffee.Repositories`
Vai trò:
- chứa models/entity
- chứa `SmartQRCoffeeContext`
- chứa repository pattern
- quản lý migration EF Core

---

## 5. Thành phần kỹ thuật chính

## 5.1. Entity Framework Core
Hệ thống dùng EF Core với PostgreSQL để:
- mapping entity
- migration database
- truy vấn dữ liệu qua LINQ

## 5.2. PostgreSQL / Supabase
Database hiện tại chạy trên Supabase PostgreSQL.

Vai trò:
- lưu users, roles
- lưu tables và session token
- lưu menu, categories, products, options
- lưu orders, order details, payments
- lưu shifts

## 5.3. JWT + Refresh Token + HttpOnly Cookies
Hệ thống dùng:
- access token sống ngắn
- refresh token sống dài
- cookie `HttpOnly`
- backend tự đọc cookie để xác thực

Mục tiêu:
- bảo mật tốt hơn localStorage
- duy trì session đăng nhập mượt hơn
- hỗ trợ refresh token rotation

## 5.4. SignalR
Hệ thống dùng SignalR để gửi realtime event:
- order mới cho kitchen group
- cập nhật trạng thái order cho table group

Hiện đã có:
- `NotificationHub`
- `SignalRNotificationService`
- `signalr-test.html` để test realtime

---

## 6. Mô hình dữ liệu cốt lõi

## 6.1. Users
Thông tin tài khoản nhân viên / admin.

Các cột chính:
- `UserId`
- `Username`
- `PasswordHash`
- `RoleId`
- `IsActive`
- `RefreshToken`
- `RefreshTokenExpiryTime`

## 6.2. Roles
Quản lý vai trò hệ thống.

Ví dụ:
- Admin
- Staff
- Cashier

## 6.3. Tables
Đại diện bàn trong quán.

Các cột chính:
- `TableId`
- `TableName`
- `QRCode`
- `SessionToken`
- `IsOccupied`
- `IsActive`

## 6.4. Categories
Danh mục menu.

Ví dụ:
- Cà phê
- Trà sữa

## 6.5. Products
Sản phẩm/món uống.

Các cột chính:
- `ProductId`
- `CategoryId`
- `Name`
- `Description`
- `Price`
- `ImageUrl`
- `IsFeatured`
- `IsNew`
- `Stock_Quantity`
- `IsDisabled`

## 6.6. ProductOptions
Tùy chọn cộng thêm cho sản phẩm.

Ví dụ:
- thêm sữa
- trân châu
- kem cheese

## 6.7. Orders
Đơn hàng của bàn.

Các cột chính:
- `OrderId`
- `TableId`
- `SessionToken`
- `Status`
- `TotalAmount`
- `CreatedAt`

## 6.8. OrderDetails
Chi tiết từng món trong order.

Các cột chính:
- `OrderDetailId`
- `OrderId`
- `ProductId`
- `Quantity`
- `UnitPrice`

## 6.9. Payments
Thông tin thanh toán của order.

Các cột chính:
- `PaymentId`
- `OrderId`
- `PaymentMethod`
- `Amount`
- `Status`
- `PaymentTime`

## 6.10. Shifts
Quản lý ca làm việc nhân viên.

Các cột chính:
- `ShiftId`
- `UserId`
- `StartTime`
- `EndTime`
- `StartingCash`
- `ExpectedCash`
- `ActualCash`
- `Discrepancy`
- `Status`

---

## 7. Flow nghiệp vụ chính

## 7.1. Flow đăng nhập nhân viên

### Mục tiêu
Cho phép staff/admin đăng nhập vào hệ thống quản trị.

### Bước xử lý
1. Client gọi `POST /api/v1/Users/login`
2. API nhận `username/password`
3. `JwtTokenService` kiểm tra user trong database
4. Password được verify bằng `BCrypt`
5. Nếu hợp lệ:
   - tạo access token
   - tạo refresh token
   - lưu refresh token vào DB
6. API set cookie:
   - `access_token`
   - `refresh_token`
7. Trả response thành công

### Kết quả
- user đăng nhập thành công
- client có thể gọi API cần `[Authorize]`

---

## 7.2. Flow cấp lại token

### Mục tiêu
Duy trì phiên đăng nhập khi access token hết hạn.

### Bước xử lý
1. Client gọi `POST /api/v1/Users/exchange-token`
2. Server đọc `refresh_token` từ cookie
3. `JwtTokenService` tìm user theo refresh token
4. Kiểm tra token còn hạn và user còn active
5. Sinh access token mới
6. Sinh refresh token mới theo cơ chế rotation
7. Cập nhật refresh token mới vào DB
8. Set lại cookie mới

### Kết quả
- user tiếp tục đăng nhập mà không cần login lại ngay

---

## 7.3. Flow đăng xuất

### Mục tiêu
Đăng xuất người dùng khỏi hệ thống.

### Bước xử lý
1. Client gọi `POST /api/v1/Users/logout`
2. Server lấy `userId` từ JWT claims
3. Xóa refresh token trong DB
4. Xóa cookie access/refresh token
5. Trả kết quả thành công

---

## 7.4. Flow quét QR lấy menu

### Mục tiêu
Khách hàng quét QR tại bàn để lấy menu hợp lệ của đúng bàn.

### Bước xử lý
1. Client gọi `GET /api/v1/Tables/{token}/menu`
2. `TableService` kiểm tra token có khớp với bàn active không
3. Nếu hợp lệ:
   - load categories
   - load products
   - load product options
4. Hệ thống map dữ liệu sang DTO menu
5. Trả về thông tin bàn + menu

### Kết quả
- khách nhìn thấy menu theo bàn đang ngồi

---

## 7.5. Flow tạo order

### Mục tiêu
Khách hàng chọn món và gửi order từ thiết bị cá nhân.

### Bước xử lý
1. Client gọi `POST /api/v1/Orders`
2. `OrderService` kiểm tra:
   - `TableId` có tồn tại không
   - `SessionToken` có khớp bàn không
   - bàn có active không
3. Với từng item trong order:
   - kiểm tra product có tồn tại không
   - kiểm tra product có bị disable không
   - kiểm tra stock đủ không
   - load option của product
   - cộng thêm giá option nếu có
4. Tính tổng tiền order
5. Tạo `Order`
6. Tạo `OrderDetails`
7. Tạo `Payment`
8. Lưu toàn bộ xuống database
9. Gọi `INotificationService.NotifyKitchenNewOrderAsync(...)`
10. SignalR push event tới kitchen group
11. Trả response `201 Created`

### Kết quả
- order được tạo thành công
- kitchen nhận realtime order mới

---

## 7.6. Flow bếp / staff cập nhật trạng thái order

### Mục tiêu
Nhân viên cập nhật tiến độ order để khách biết trạng thái đơn.

### Bước xử lý
1. Staff gọi `PATCH /api/v1/Orders/{orderId}/status`
2. `OrderService` tìm order theo `orderId`
3. Nếu order tồn tại:
   - cập nhật `Status`
   - lưu xuống DB
4. Gọi `NotifyCustomerOrderStatusChangedAsync(tableId, newStatus)`
5. SignalR push event tới group `table-{tableId}`
6. Trả response thành công

### Kết quả
- khách theo bàn tương ứng nhận được cập nhật realtime

---

## 7.7. Flow realtime kitchen

### Mục tiêu
Khu vực bếp / pha chế nhận được order mới ngay khi khách submit.

### Bước xử lý
1. Client bếp kết nối SignalR hub `/hubs/notifications`
2. Client invoke `JoinKitchen()`
3. Khi có order mới, server push `ReceiveNewOrder(...)`
4. Client bếp nhận payload và cập nhật UI

### Kết quả
- bếp không cần refresh trang để thấy order mới

---

## 7.8. Flow realtime customer theo bàn

### Mục tiêu
Khách ngồi tại bàn biết ngay order đã được tiếp nhận/pha xong/chờ phục vụ.

### Bước xử lý
1. Client khách kết nối SignalR hub
2. Client invoke `JoinTableGroup(tableId)`
3. Khi trạng thái order thay đổi, server push `ReceiveOrderStatusChanged(...)`
4. Client của bàn đó hiển thị trạng thái mới

### Kết quả
- trải nghiệm realtime rõ ràng hơn cho khách

---

## 8. Các endpoint chính hiện tại

## 8.1. Users
- `POST /api/v1/Users/login`
- `POST /api/v1/Users/exchange-token`
- `POST /api/v1/Users/logout`
- `POST /api/v1/Users/register`
- `GET /api/v1/Users/profile`

## 8.2. Tables
- `GET /api/v1/Tables/{token}/menu`

## 8.3. Orders
- `POST /api/v1/Orders`
- `PATCH /api/v1/Orders/{orderId}/status`

## 8.4. SignalR
- `GET/CONNECT /hubs/notifications`

---

## 9. Các nhóm người dùng chính

## 9.1. Customer
- quét QR
- xem menu
- tạo order
- nhận trạng thái order realtime

## 9.2. Staff / Kitchen
- nhận order mới realtime
- cập nhật trạng thái order

## 9.3. Cashier
- có thể dùng để xử lý thanh toán / trạng thái thanh toán ở bước mở rộng

## 9.4. Admin
- quản trị user / role / menu / bàn / cấu hình hệ thống ở bước mở rộng

---

## 10. Các điểm mạnh hiện tại của project

- có 3-layer architecture rõ ràng
- có repository pattern
- có EF Core migration
- có JWT + Refresh Token + HttpOnly Cookie
- có order flow chạy end-to-end
- có SignalR backend thật
- có test client realtime đơn giản
- có dữ liệu test đã seed sẵn

---

## 11. Các điểm còn có thể mở rộng

### 11.1. Auth
- tách bảng `RefreshTokens` riêng thay vì lưu trên `Users`
- nâng cấp cấu hình secret ra environment variables

### 11.2. Order domain
- thêm order history endpoint
- thêm danh sách order theo bàn
- thêm payment confirmation flow
- thêm trạng thái chi tiết hơn: `Received`, `Preparing`, `Ready`, `Served`, `Cancelled`

### 11.3. SignalR
- thêm auth chặt hơn cho hub
- thêm user-specific notification
- thêm reconnect/resync strategy phía frontend

### 11.4. Admin side
- CRUD products
- CRUD categories
- CRUD tables
- user/role management
- shift management UI/API

### 11.5. Reporting
- dashboard doanh thu
- số lượng order theo ngày
- món bán chạy
- hiệu suất ca làm

---

## 12. Kết luận

`SmartQRCoffee` hiện là một backend .NET API theo hướng khá phù hợp với đồ án doanh nghiệp nhỏ hoặc bài tập mô phỏng hệ thống đặt món QR trong quán cà phê.

Hệ thống đã có đủ các thành phần nền tảng quan trọng:
- kiến trúc 3 lớp
- database thực tế
- auth bằng JWT + refresh token + cookies
- flow tạo order
- realtime SignalR cho kitchen và customer table groups

Nếu tiếp tục mở rộng frontend quản trị và frontend khách hàng, hệ thống này có thể phát triển thành một demo end-to-end tương đối hoàn chỉnh để trình bày hoặc bảo vệ đồ án.
