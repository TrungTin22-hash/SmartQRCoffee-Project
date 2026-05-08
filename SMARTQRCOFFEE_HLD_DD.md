<div align="center">

# System Design Specification
## High-Level Design (HLD) & Detailed Design (DD)
### Dự Án: SmartQRCoffee - Hệ Thống Gọi Món Bằng QR Code Kết Hợp Realtime

<br/>
<br/>

**Dương Hồng Quân**  
*Project Manager*

<br/>
<br/>

**Ngày 08 tháng 05 năm 2026**

</div>

---

# Mục lục

1. [Giới thiệu](#1-giới-thiệu)  
2. [High-Level Design (Kiến Trúc Tổng Thể)](#2-high-level-design-kiến-trúc-tổng-thể)  
   2.1. [System Architecture](#21-system-architecture)  
   2.2. [Kiến trúc logic tổng quát](#22-kiến-trúc-logic-tổng-quát)  
   2.3. [Công nghệ chính](#23-công-nghệ-chính)  
   2.4. [Authentication & Security Overview](#24-authentication--security-overview)  
   2.5. [Realtime Architecture](#25-realtime-architecture)  
   2.6. [Database Schema - Core Entities](#26-database-schema---core-entities)  
3. [Detailed Design: User Interface (Thiết Kế Màn Hình)](#3-detailed-design-user-interface-thiết-kế-màn-hình)  
   3.1. [Screen: QR Menu Screen](#31-screen-qr-menu-screen-màn-hình-khách-xem-menu)  
   3.2. [Screen: Order Cart / Checkout](#32-screen-order-cart--checkout)  
   3.3. [Screen: Kitchen Realtime Monitor](#33-screen-kitchen-realtime-monitor)  
   3.4. [Screen: Customer Order Status View](#34-screen-customer-order-status-view)  
4. [Detailed Design: API Specifications (Đặc Tả API)](#4-detailed-design-api-specifications-đặc-tả-api)  
   4.1. [API: Login User](#41-api-login-user)  
   4.2. [API: Exchange Token](#42-api-exchange-token)  
   4.3. [API: Logout](#43-api-logout)  
   4.4. [API: Get Menu By Table Token](#44-api-get-menu-by-table-token)  
   4.5. [API: Submit Order](#45-api-submit-order)  
   4.6. [API: Update Order Status](#46-api-update-order-status)  
   4.7. [SignalR Hub: Notifications](#47-signalr-hub-notifications)  
5. [Flow nghiệp vụ chi tiết](#5-flow-nghiệp-vụ-chi-tiết)  
6. [Các điểm mạnh hiện tại của thiết kế](#6-các-điểm-mạnh-hiện-tại-của-thiết-kế)  
7. [Các hạn chế và hướng cải tiến](#7-các-hạn-chế-và-hướng-cải-tiến)  
8. [Kết luận](#8-kết-luận)  

---

# 1. Giới thiệu

## 1.1. Bối cảnh dự án

Trong mô hình vận hành quán cà phê hiện đại, việc tối ưu quy trình gọi món và truyền thông tin giữa khách hàng với khu vực pha chế là yếu tố quan trọng nhằm nâng cao trải nghiệm người dùng và hiệu suất xử lý đơn hàng. Các mô hình menu giấy hoặc order thủ công thường phát sinh nhiều bất cập như:

- khách hàng phải chờ nhân viên mang menu hoặc ghi nhận order;
- dễ sai lệch khi truyền order từ bàn đến bếp;
- khó cập nhật trạng thái đơn hàng theo thời gian thực;
- khó mở rộng sang mô hình bán hàng số hóa.

Dự án `SmartQRCoffee` được xây dựng nhằm giải quyết các vấn đề trên bằng một hệ thống gọi món sử dụng mã QR tại bàn, kết hợp backend API theo mô hình 3 lớp và realtime communication thông qua SignalR.

---

## 1.2. Mục tiêu hệ thống

Hệ thống hướng tới các mục tiêu sau:

- cho phép khách hàng quét QR tại bàn để xem menu ngay trên thiết bị cá nhân;
- cho phép khách hàng chủ động tạo order mà không cần đợi nhân viên ghi nhận thủ công;
- cho phép khu vực bếp / pha chế nhận được order mới theo thời gian thực;
- cho phép cập nhật trạng thái order realtime đến khách hàng theo từng bàn;
- hỗ trợ xác thực người dùng quản trị bằng JWT, Refresh Token và HttpOnly Cookies;
- xây dựng nền tảng có thể mở rộng thành hệ thống quản trị quán cà phê hoàn chỉnh.

---

## 1.3. Phạm vi tài liệu

Tài liệu này tập trung mô tả:

- kiến trúc tổng quan của hệ thống;
- cấu trúc solution và vai trò các project;
- các thành phần kỹ thuật chính như auth, database, realtime;
- mô hình dữ liệu cốt lõi;
- thiết kế màn hình ở mức nghiệp vụ;
- đặc tả API quan trọng;
- flow nghiệp vụ từ login, xem menu, tạo order đến cập nhật trạng thái đơn hàng.

---

# 2. High-Level Design (Kiến Trúc Tổng Thể)

## 2.1. System Architecture

Hệ thống `SmartQRCoffee` được thiết kế theo mô hình:

- Client - Server Architecture;
- 3-Layer Architecture;
- RESTful API cho xử lý request/response;
- SignalR cho giao tiếp realtime.

### 2.1.1. Presentation Layer

Presentation Layer hiện được hiện thực chủ yếu thông qua project `SmartQRCoffee.API` và đóng vai trò là lớp tiếp nhận tương tác từ client.

Các thành phần chính:
- ASP.NET Core Controllers;
- JWT Authentication Middleware;
- Cookie-based authentication handling;
- SignalR Hub cho realtime notification;
- Swagger phục vụ test và mô tả API.

Vai trò:
- nhận request từ client;
- xác thực request;
- chuyển request đến service layer;
- trả response JSON cho frontend / Swagger;
- cung cấp endpoint realtime cho client kitchen và khách hàng.

---

### 2.1.2. Business Logic Layer

Business Logic Layer được hiện thực trong project `SmartQRCoffee.Services`.

Các thành phần chính:
- `UserService`;
- `JwtTokenService`;
- `TableService`;
- `OrderService`;
- `INotificationService` và các implementation liên quan.

Vai trò:
- xử lý quy tắc nghiệp vụ đăng nhập;
- cấp access token và refresh token;
- kiểm tra token bàn;
- tính toán tổng tiền order;
- kiểm tra stock và trạng thái sản phẩm;
- phát realtime notification khi tạo order hoặc đổi trạng thái order.

---

### 2.1.3. Data Access Layer

Data Access Layer được hiện thực trong project `SmartQRCoffee.Repositories`.

Các thành phần chính:
- `SmartQRCoffeeContext`;
- EF Core Entities;
- Repository interfaces;
- Repository implementations;
- Migration files.

Vai trò:
- truy cập PostgreSQL / Supabase;
- thực hiện các thao tác CRUD;
- quản lý schema thông qua migrations;
- tách logic truy cập dữ liệu khỏi business logic.

---

## 2.2. Kiến trúc logic tổng quát

```text
Customer / Staff Client
        |
        v
SmartQRCoffee.API
  - Controllers
  - Authentication Middleware
  - Cookie Policy
  - SignalR Hub
        |
        v
SmartQRCoffee.Services
  - JwtTokenService
  - UserService
  - TableService
  - OrderService
  - NotificationService
        |
        v
SmartQRCoffee.Repositories
  - Repositories
  - DbContext
  - EF Core Models
  - Migrations
        |
        v
PostgreSQL / Supabase
```

Kiến trúc này bảo đảm nguyên tắc:
- controller không ôm business logic;
- service không truy cập database trực tiếp nếu đã có repository;
- data access được tập trung và dễ bảo trì;
- hệ thống có thể mở rộng thêm frontend hoặc module quản trị sau này.

---

## 2.3. Công nghệ chính

### Backend
- .NET 8 Web API
- ASP.NET Core
- SignalR

### Data Layer
- Entity Framework Core
- Npgsql PostgreSQL Provider
- Supabase PostgreSQL

### Security
- JWT Bearer Authentication
- Refresh Token Rotation
- HttpOnly Cookies

### Documentation & Testing
- Swagger / Swashbuckle
- file hướng dẫn test nội bộ
- SignalR HTML test client

---

## 2.4. Authentication & Security Overview

Hệ thống sử dụng mô hình bảo mật gồm:
- Access Token có thời hạn ngắn;
- Refresh Token có thời hạn dài hơn;
- HttpOnly Cookies để trình duyệt tự đính kèm token;
- backend tự đọc token từ cookie để xác thực.

Mô hình này giúp:
- giảm rủi ro khi so sánh với cách lưu token trong LocalStorage;
- giữ trải nghiệm đăng nhập liền mạch hơn;
- hỗ trợ rotate refresh token khi cấp lại access token.

Hiện tại hệ thống đã chuẩn hóa cấu hình theo nhóm `JwtConfig` và `ConnectionStrings`, phù hợp hơn với yêu cầu giáo án.

---

## 2.5. Realtime Architecture

SignalR được tích hợp để hỗ trợ hai luồng realtime cốt lõi.

### 2.5.1. Kitchen Group
Dùng để phát sự kiện khi có order mới.

- group name: `kitchen`
- event client: `ReceiveNewOrder(payload)`

### 2.5.2. Table Group
Dùng để phát sự kiện khi trạng thái order của bàn thay đổi.

- group name: `table-{tableId}`
- event client: `ReceiveOrderStatusChanged(payload)`

### 2.5.3. Hub Endpoint
- `/hubs/notifications`

### 2.5.4. Hub Methods
- `JoinKitchen()`
- `LeaveKitchen()`
- `JoinTableGroup(int tableId)`
- `LeaveTableGroup(int tableId)`

---

## 2.6. Database Schema - Core Entities

### Users
- `UserId`
- `Username`
- `PasswordHash`
- `RoleId`
- `IsActive`
- `RefreshToken`
- `RefreshTokenExpiryTime`

### Roles
- `RoleId`
- `RoleName`

### Tables
- `TableId`
- `TableName`
- `QRCode`
- `SessionToken`
- `IsOccupied`
- `IsActive`

### Categories
- `CategoryId`
- `Name`
- `IconUrl`

### Products
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

### ProductOptions
- `ProductOptionId`
- `ProductId`
- `Name`
- `PriceAdjustment`

### Orders
- `OrderId`
- `TableId`
- `SessionToken`
- `Status`
- `TotalAmount`
- `CreatedAt`

### OrderDetails
- `OrderDetailId`
- `OrderId`
- `ProductId`
- `Quantity`
- `UnitPrice`

### Payments
- `PaymentId`
- `OrderId`
- `PaymentMethod`
- `Amount`
- `Status`
- `PaymentTime`

### Shifts
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

# 3. Detailed Design: User Interface (Thiết Kế Màn Hình)

## 3.1. Screen: QR Menu Screen (Màn hình khách xem menu)

### Mục đích
Cho phép khách hàng xem menu của bàn sau khi quét QR thành công.

### Thành phần chính
- tiêu đề hiển thị tên bàn;
- danh sách category;
- danh sách sản phẩm theo từng category;
- giá sản phẩm;
- trạng thái khả dụng của món;
- product options;
- giỏ hàng tạm thời;
- nút xác nhận đặt món.

### Quy tắc nghiệp vụ
- chỉ hiển thị menu nếu `SessionToken` hợp lệ;
- các sản phẩm `IsDisabled = true` hoặc `Stock_Quantity <= 0` phải hiển thị trạng thái không khả dụng;
- option chỉ được gắn cho đúng product tương ứng.

---

## 3.2. Screen: Order Cart / Checkout

### Mục đích
Cho phép khách hàng rà soát lại danh sách món và xác nhận tạo order.

### Thành phần chính
- danh sách món đã chọn;
- số lượng từng món;
- option đi kèm;
- ghi chú cho từng món;
- tổng tiền tạm tính;
- lựa chọn phương thức thanh toán;
- nút `Place Order`.

### Quy tắc nghiệp vụ
- tổng tiền phải bao gồm giá sản phẩm và phụ phí option;
- không cho submit nếu token bàn không hợp lệ;
- không cho submit nếu sản phẩm không còn hàng hoặc đã bị disable.

---

## 3.3. Screen: Kitchen Realtime Monitor

### Mục đích
Hiển thị danh sách order mới cho khu vực bếp / pha chế theo thời gian thực.

### Thành phần chính
- danh sách order mới;
- mã order;
- bàn gọi món;
- tổng tiền;
- trạng thái order;
- danh sách item trong order;
- nút cập nhật trạng thái.

### Realtime Requirement
- khi khách tạo order mới, kitchen client đang kết nối phải nhận được event `ReceiveNewOrder`.

---

## 3.4. Screen: Customer Order Status View

### Mục đích
Cho phép khách hàng tại bàn theo dõi tiến độ xử lý order.

### Thành phần chính
- trạng thái hiện tại của order;
- thông báo realtime;
- phần lịch sử trạng thái (nếu mở rộng).

### Realtime Requirement
- khi staff thay đổi trạng thái order, client của bàn tương ứng phải nhận event `ReceiveOrderStatusChanged`.

---

# 4. Detailed Design: API Specifications (Đặc Tả API)

## 4.1. API: Login User

### Endpoint
- `POST /api/v1/Users/login`

### Description
Xác thực user và set access token + refresh token vào HttpOnly cookies.

### Authorization
- Public

### Request Payload
```json
{
  "username": "admin_demo",
  "password": "123456"
}
```

### Response Payload (200 OK)
```json
{
  "message": "Đăng nhập thành công",
  "user": {
    "userId": 1,
    "username": "admin_demo",
    "roleId": 1,
    "roleName": "Admin"
  }
}
```

### Business Rules
- username phải tồn tại;
- password phải đúng theo `BCrypt`;
- user phải đang active;
- refresh token mới phải được lưu vào database.
- role của user sẽ được nhúng vào JWT claims để phục vụ role-based authorization.

---

## 4.1.1. Authorization Notes

Sau khi đăng nhập thành công, hệ thống gắn `ClaimTypes.Role` vào JWT để phục vụ kiểm soát quyền truy cập theo vai trò.

Các role hiện đang sử dụng trong project:
- `Admin`
- `Staff`
- `Cashier`

---

## 4.2. API: Exchange Token

### Endpoint
- `POST /api/v1/Users/exchange-token`

### Description
Đọc refresh token từ cookie, validate và cấp lại access token / refresh token mới.

### Authorization
- Public (nhưng phải có refresh token hợp lệ)

### Response Payload (200 OK)
```json
{
  "message": "Cấp lại Token thành công"
}
```

### Business Rules
- refresh token phải tồn tại;
- refresh token phải còn hạn;
- user phải active;
- refresh token cũ phải được rotate.

---

## 4.2.1. API: Register User

### Endpoint
- `POST /api/v1/Users/register`

### Description
Tạo tài khoản người dùng mới trong hệ thống.

### Authorization
- `Admin` only

### Request Payload
```json
{
  "username": "staff_test_new",
  "password": "123456",
  "roleId": 2
}
```

### Response Payload (201 Created)
```json
{
  "userId": 11,
  "username": "staff_test_new",
  "roleId": 2,
  "roleName": "Staff"
}
```

### Business Rules
- endpoint này chỉ được phép gọi bởi user có role `Admin`;
- mục tiêu là ngăn các role thấp hơn tự tạo thêm tài khoản nội bộ;
- đây là một phần của role-based authorization trong hệ thống.

---

## 4.3. API: Logout

### Endpoint
- `POST /api/v1/Users/logout`

### Description
Đăng xuất người dùng, xóa refresh token khỏi database và xóa cookie auth.

### Authorization
- User đã đăng nhập

### Response Payload (200 OK)
```json
{
  "message": "Đăng xuất thành công. Token đã bị xóa."
}
```

---

## 4.4. API: Get Menu By Table Token

### Endpoint
- `GET /api/v1/Tables/{token}/menu`

### Description
Kiểm tra token bàn và trả về menu cho khách hàng.

### Authorization
- Public

### Response Payload (200 OK)
```json
{
  "table": {
    "id": 1,
    "name": "Bàn 01",
    "sessionToken": "table-01-demo-token"
  },
  "categories": [
    {
      "id": 1,
      "name": "Cà phê",
      "products": [
        {
          "id": 1,
          "name": "Cà phê sữa đá",
          "price": 29000,
          "imageUrl": "ca-phe-sua-da.jpg",
          "isAvailable": true,
          "options": []
        }
      ]
    }
  ]
}
```

### Business Rules
- token phải thuộc về một bàn active;
- chỉ trả dữ liệu menu nếu token hợp lệ;
- trạng thái món phải phản ánh đúng stock và cờ disable.

---

## 4.5. API: Submit Order

### Endpoint
- `POST /api/v1/Orders`

### Description
Tạo order mới từ phía khách hàng.

### Authorization
- Public theo token bàn

### Request Payload
```json
{
  "tableId": 1,
  "sessionToken": "table-01-demo-token",
  "paymentMethod": "Cash",
  "items": [
    {
      "productId": 1,
      "quantity": 1,
      "options": [2],
      "note": "Thêm sữa"
    },
    {
      "productId": 3,
      "quantity": 1,
      "options": [3, 4],
      "note": "Full topping"
    }
  ]
}
```

### Response Payload (201 Created)
```json
{
  "orderId": "ORD-4",
  "totalAmount": 86000,
  "status": "Pending",
  "message": "Order placed successfully. The kitchen is preparing your drinks!"
}
```

### Business Logic / Validation
1. kiểm tra bàn tồn tại;
2. kiểm tra `SessionToken` khớp với bàn;
3. kiểm tra bàn còn active;
4. kiểm tra từng product tồn tại;
5. kiểm tra product không bị disable;
6. kiểm tra stock đủ số lượng;
7. load option của product và tính tổng tiền;
8. tạo `Order`, `OrderDetails`, `Payment`;
9. lưu xuống database;
10. push event realtime cho kitchen.

---

## 4.6. API: Update Order Status

### Endpoint
- `PATCH /api/v1/Orders/{orderId}/status`

### Description
Cập nhật trạng thái order từ staff / kitchen.

### Authorization
- `Admin`, `Staff`, `Cashier`

### Request Payload
```json
{
  "newStatus": "Preparing"
}
```

### Response Payload (200 OK)
```json
{
  "orderId": "ORD-3",
  "currentStatus": "Preparing",
  "updatedAt": "2026-05-08T09:09:34.6173269Z"
}
```

### Business Logic / Validation
- order phải tồn tại;
- chỉ các role nội bộ `Admin`, `Staff`, `Cashier` mới được phép gọi endpoint này;
- cập nhật status trong database;
- push event realtime tới table group tương ứng.

---

## 4.7. SignalR Hub: Notifications

### Endpoint
- `/hubs/notifications`

### Hub Methods
- `JoinKitchen()`
- `LeaveKitchen()`
- `JoinTableGroup(int tableId)`
- `LeaveTableGroup(int tableId)`

### Client Events
- `ReceiveNewOrder(payload)`
- `ReceiveOrderStatusChanged(payload)`

---

# 5. Flow nghiệp vụ chi tiết

## 5.1. Flow: Customer Scan QR -> View Menu

1. khách quét QR tại bàn;  
2. client lấy `SessionToken`;  
3. gọi `GET /api/v1/Tables/{token}/menu`;  
4. backend validate token bàn;  
5. backend trả menu;  
6. khách hàng bắt đầu chọn món.

---

## 5.2. Flow: Customer Create Order

1. khách chọn món và option;  
2. frontend build request `CreateOrderDto`;  
3. gọi `POST /api/v1/Orders`;  
4. backend validate bàn, token và stock;  
5. backend tính tổng tiền;  
6. backend lưu `Order`, `OrderDetails`, `Payment`;  
7. backend push event realtime cho kitchen;  
8. frontend nhận response thành công.

---

## 5.3. Flow: Kitchen Receive New Order Realtime

1. client bếp kết nối hub `/hubs/notifications`;  
2. client invoke `JoinKitchen()`;  
3. khi order mới được tạo, server push `ReceiveNewOrder`;  
4. kitchen UI cập nhật danh sách order ngay lập tức.

---

## 5.4. Flow: Kitchen Update Order Status

1. staff / kitchen chọn order cần xử lý;  
2. gọi `PATCH /api/v1/Orders/{id}/status`;  
3. backend cập nhật database;  
4. backend push `ReceiveOrderStatusChanged` tới group `table-{tableId}`;  
5. client của bàn tương ứng nhận cập nhật realtime.

---

## 5.5. Flow: User Login -> Refresh -> Logout

1. user đăng nhập bằng `username/password`;  
2. server phát access token + refresh token;  
3. token được lưu trong cookie `HttpOnly`;  
4. khi access token hết hạn, client gọi `exchange-token`;  
5. server rotate refresh token;  
6. khi logout, server xóa token trong DB và cookie phía client.

---

# 6. Các điểm mạnh hiện tại của thiết kế

- kiến trúc 3 lớp rõ ràng;
- auth theo JWT + Refresh Token + HttpOnly Cookies;
- dữ liệu được quản lý qua EF Core migrations;
- order flow đã hoạt động end-to-end;
- SignalR đã tích hợp thật ở backend;
- có SignalR test client để demo nhanh;
- có test guide riêng cho dự án.

---

# 7. Các hạn chế và hướng cải tiến

## 7.1. Authorization
- hệ thống đã bổ sung role-based authorization cho các endpoint nhạy cảm;
- `POST /api/v1/Users/register` hiện chỉ dành cho `Admin`;
- `PATCH /api/v1/Orders/{orderId}/status` hiện dành cho `Admin`, `Staff`, `Cashier`;
- trong tương lai có thể mở rộng phân quyền chi tiết hơn cho từng module quản trị.

## 7.2. Refresh Token Design
- có thể tách bảng `RefreshTokens` riêng để quản lý token tốt hơn.

## 7.3. Realtime Security
- có thể siết chặt auth cho SignalR hub ở môi trường production;
- có thể bổ sung chiến lược reconnect/resync cho frontend thật.

## 7.4. Domain mở rộng
- thêm order history;
- thêm payment confirmation flow;
- thêm dashboard doanh thu;
- thêm CRUD menu / bàn / user cho admin;
- thêm giao diện cashier và shift management.

---

# 8. Kết luận

`SmartQRCoffee` là một hệ thống backend phù hợp với bài toán gọi món bằng QR code tại quán cà phê, đồng thời thể hiện khá rõ các kỹ thuật quan trọng trong phát triển .NET API hiện đại, bao gồm:

- kiến trúc 3 lớp;
- quản lý dữ liệu với EF Core;
- xác thực với JWT + Refresh Token + Cookies;
- realtime communication với SignalR.

Tài liệu HLD + DD này giúp hệ thống được mô tả ở mức vừa đủ để:
- phục vụ báo cáo / bảo vệ đồ án;
- hỗ trợ frontend tích hợp;
- hỗ trợ team hiểu flow hệ thống;
- tạo nền tảng cho việc mở rộng hệ thống trong tương lai.
