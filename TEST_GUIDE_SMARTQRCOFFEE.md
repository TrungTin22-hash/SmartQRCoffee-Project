# SmartQRCoffee Test Guide

## 1. Mục tiêu

Tài liệu này hướng dẫn test nhanh các flow quan trọng của project `SmartQRCoffee`:

- chạy API backend
- kiểm tra Swagger
- test lấy menu theo token bàn
- test tạo order
- test cập nhật trạng thái order
- test SignalR realtime với client test

---

## 2. Thông tin môi trường hiện tại

### API URL
- `http://localhost:5121`

### Swagger URL
- `http://localhost:5121/swagger`

### SignalR Hub URL
- `http://localhost:5121/hubs/notifications`

### SignalR Test Client URL
- `http://localhost:5121/signalr-test.html`

---

## 3. Dữ liệu test đã seed sẵn

### 3.1. Roles
- `1` = `Admin`
- `2` = `Staff`
- `3` = `Cashier`

### 3.2. Tài khoản test
- `admin_demo` / `123456`
- `staff_demo` / `123456`
- `cashier_demo` / `123456`

### 3.3. Tables
- `TableId = 1`
  - `TableName = Bàn 01`
  - `SessionToken = table-01-demo-token`
- `TableId = 2`
  - `TableName = Bàn 02`
  - `SessionToken = table-02-demo-token`

### 3.4. Categories
- `1` = `Cà phê`
- `2` = `Trà sữa`

### 3.5. Products
- `1` = `Cà phê sữa đá` — `29000`
- `2` = `Bạc xỉu` — `32000`
- `3` = `Trà sữa truyền thống` — `35000`

### 3.6. Product Options
- `1` = `Ít đá` — `0`
- `2` = `Thêm sữa` — `5000`
- `3` = `Trân châu đen` — `7000`
- `4` = `Kem cheese` — `10000`

---

## 4. Chạy backend

Mở terminal tại thư mục project:

```bash
dotnet run --project SmartQRCoffee.API
```

Khi chạy thành công, bạn sẽ thấy log kiểu:

```text
Now listening on: http://localhost:5121
Application started. Press Ctrl+C to shut down.
```

---

## 5. Kiểm tra Swagger

Mở trình duyệt tại:

- `http://localhost:5121/swagger`

Nếu Swagger hiển thị các endpoint như:
- `Users`
- `Tables`
- `Orders`

thì backend đã lên ổn.

---

## 6. Test lấy menu theo token bàn

### Endpoint
- `GET /api/v1/Tables/{token}/menu`

### Test case hợp lệ
Dùng token:
- `table-01-demo-token`

URL đầy đủ:
- `http://localhost:5121/api/v1/Tables/table-01-demo-token/menu`

### Kết quả mong đợi
- HTTP `200 OK`
- trả về thông tin bàn và danh sách category / product

### Test case không hợp lệ
Dùng token sai, ví dụ:
- `invalid-token`

### Kết quả mong đợi
- HTTP `400 Bad Request`
- message kiểu `Invalid or expired table token.`

---

## 7. Test login

### Endpoint
- `POST /api/v1/Users/login`

### Body mẫu
```json
{
  "username": "admin_demo",
  "password": "123456"
}
```

### Kết quả mong đợi
- HTTP `200 OK`
- response có message đăng nhập thành công
- server set cookie:
  - `access_token`
  - `refresh_token`

### Gợi ý
Nên test trong Swagger hoặc Postman để quan sát dễ hơn.

---

## 8. Test tạo order cơ bản

### Endpoint
- `POST /api/v1/Orders`

### Body mẫu cơ bản
```json
{
  "tableId": 1,
  "sessionToken": "table-01-demo-token",
  "paymentMethod": "Cash",
  "items": [
    {
      "productId": 1,
      "quantity": 2,
      "options": [],
      "note": "Ít ngọt"
    },
    {
      "productId": 3,
      "quantity": 1,
      "options": [],
      "note": "Không đá"
    }
  ]
}
```

### Tổng tiền mong đợi
- `2 x 29000 + 1 x 35000 = 93000`

### Kết quả mong đợi
- HTTP `201 Created`
- response kiểu:

```json
{
  "orderId": "ORD-x",
  "totalAmount": 93000,
  "status": "Pending",
  "message": "Order placed successfully. The kitchen is preparing your drinks!"
}
```

---

## 9. Test tạo order có options

### Endpoint
- `POST /api/v1/Orders`

### Body mẫu có option
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

### Cách tính tiền mong đợi
- Product `1`: `29000 + 5000 = 34000`
- Product `3`: `35000 + 7000 + 10000 = 52000`
- Tổng: `86000`

### Kết quả mong đợi
- HTTP `201 Created`
- response có `totalAmount = 86000`

---

## 10. Test cập nhật trạng thái order

### Endpoint
- `PATCH /api/v1/Orders/{orderId}/status`

### Body mẫu 1
```json
{
  "newStatus": "Preparing"
}
```

### Body mẫu 2
```json
{
  "newStatus": "Completed"
}
```

### Ví dụ
Nếu order vừa tạo là `ORD-3`, thì id thực là `3`.

Gọi:
- `PATCH /api/v1/Orders/3/status`

### Kết quả mong đợi
- HTTP `200 OK`
- response kiểu:

```json
{
  "orderId": "ORD-3",
  "currentStatus": "Preparing",
  "updatedAt": "..."
}
```

Sau đó gọi lại với `Completed`.

### Test case lỗi
Nếu gọi:
- `PATCH /api/v1/Orders/9999/status`

Kết quả mong đợi:
- HTTP `400 Bad Request`
- message: `Order not found.`

---

## 11. Test role authorization

### 11.1. Rule đang áp dụng

Hiện tại hệ thống có 2 rule phân quyền quan trọng:

- `POST /api/v1/Users/register`
  - chỉ `Admin` được phép gọi
- `PATCH /api/v1/Orders/{orderId}/status`
  - chỉ `Admin`, `Staff`, `Cashier` được phép gọi

---

### 11.2. Test `register` với role `Admin`

#### Bước 1
Login bằng tài khoản admin:

```json
{
  "username": "admin_demo",
  "password": "123456"
}
```

#### Bước 2
Gọi endpoint:
- `POST /api/v1/Users/register`

Body ví dụ:

```json
{
  "username": "staff_test_new",
  "password": "123456",
  "roleId": 2
}
```

#### Kết quả mong đợi
- HTTP `201 Created`
- user mới được tạo thành công

---

### 11.3. Test `register` với role `Staff`

#### Bước 1
Login bằng tài khoản staff:

```json
{
  "username": "staff_demo",
  "password": "123456"
}
```

#### Bước 2
Gọi lại endpoint:
- `POST /api/v1/Users/register`

#### Kết quả mong đợi
- HTTP `403 Forbidden`
- staff không được phép tạo user mới

---

### 11.4. Test `update order status` với role nội bộ

#### Bước 1
Tạo trước một order test bằng endpoint `POST /api/v1/Orders`

Ví dụ body:

```json
{
  "tableId": 1,
  "sessionToken": "table-01-demo-token",
  "paymentMethod": "Cash",
  "items": [
    {
      "productId": 1,
      "quantity": 1,
      "options": [],
      "note": "test role auth"
    }
  ]
}
```

#### Bước 2
Login bằng một trong các tài khoản sau:
- `admin_demo`
- `staff_demo`
- `cashier_demo`

#### Bước 3
Gọi endpoint:
- `PATCH /api/v1/Orders/{orderId}/status`

Body:

```json
{
  "newStatus": "Preparing"
}
```

#### Kết quả mong đợi
- HTTP `200 OK`
- trạng thái order được cập nhật thành công

---

### 11.5. Test `update order status` với role không hợp lệ

Nếu sau này có thêm role như `Customer` hoặc `Viewer`, hãy test như sau:

#### Bước 1
Login bằng user role không thuộc danh sách cho phép.

#### Bước 2
Gọi:
- `PATCH /api/v1/Orders/{orderId}/status`

#### Kết quả mong đợi
- HTTP `403 Forbidden`

---

## 12. Test SignalR realtime

### 11.1. Mở trang test SignalR
Mở trình duyệt tại:

- `http://localhost:5121/signalr-test.html`

### 11.2. Kết nối hub
Trên trang test:
- `Hub URL`: giữ mặc định `http://localhost:5121/hubs/notifications`
- `JWT Access Token`:
  - có thể để trống khi test đơn giản
  - hoặc dán token nếu frontend/hub cần auth nghiêm ngặt hơn

Bấm:
- `Kết nối`

Kết quả mong đợi:
- trạng thái chuyển thành `Đã kết nối`
- log hiển thị `Kết nối hub thành công`

---

## 13. Test realtime cho bếp

### Bước 1
Trong trang test SignalR, bấm:
- `Join Kitchen`

### Bước 2
Gọi API tạo order bằng một trong hai body ở phần 8 hoặc 9.

### Kết quả mong đợi
Client SignalR nhận event:
- `ReceiveNewOrder`

Log sẽ hiển thị payload order mới.

---

## 14. Test realtime cho khách theo bàn

### Bước 1
Trong trang test SignalR:
- nhập `Table ID = 1`
- bấm `Join Table Group`

### Bước 2
Gọi API cập nhật trạng thái order của bàn 1.

Ví dụ:
- `PATCH /api/v1/Orders/3/status`
- body:

```json
{
  "newStatus": "Preparing"
}
```

### Kết quả mong đợi
Client SignalR nhận event:
- `ReceiveOrderStatusChanged`

Payload mong đợi dạng gần giống:

```json
{
  "tableId": 1,
  "newStatus": "Preparing"
}
```

---

## 15. Checklist demo nhanh

### Backend
- [ ] API chạy tại `http://localhost:5121`
- [ ] Swagger mở được
- [ ] Lấy menu theo token bàn thành công

### Auth
- [ ] Login thành công với `admin_demo / 123456`
- [ ] Cookie auth được set
- [ ] `Admin` gọi `register` thành công
- [ ] `Staff` gọi `register` bị chặn `403`
- [ ] `Admin/Staff/Cashier` được phép cập nhật order status

### Order
- [ ] Submit order cơ bản thành công
- [ ] Submit order có options thành công
- [ ] Update order status thành công

### SignalR
- [ ] SignalR test page mở được
- [ ] Kết nối hub thành công
- [ ] Kitchen nhận `ReceiveNewOrder`
- [ ] Table group nhận `ReceiveOrderStatusChanged`

---

## 16. Các lỗi thường gặp

### Lỗi API không chạy
- kiểm tra có process cũ đang khóa file `.exe/.dll` không
- nếu có, tắt process cũ rồi chạy lại `dotnet run`

### Lỗi không lấy được menu
- kiểm tra token bàn đúng chưa:
  - `table-01-demo-token`
  - `table-02-demo-token`

### Lỗi order fail
- kiểm tra `tableId` và `sessionToken` có khớp không
- kiểm tra product có tồn tại không
- kiểm tra stock còn đủ không

### Lỗi SignalR không nhận event
- chắc chắn đã bấm `Kết nối`
- chắc chắn đã `Join Kitchen` hoặc `Join Table Group`
- kiểm tra Hub URL đúng: `http://localhost:5121/hubs/notifications`

---

## 17. Kết luận

Nếu toàn bộ các bước trên đều pass, bạn có thể kết luận:

- backend SmartQRCoffee chạy ổn
- flow order hoạt động end-to-end
- realtime SignalR hoạt động ở mức backend + test client
- project đã sẵn sàng để demo đồ án
