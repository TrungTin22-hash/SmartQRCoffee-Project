# Final Submission Checklist - SmartQRCoffee

## 1. Mục tiêu

Checklist này dùng để rà lần cuối trước khi:
- nộp source code
- demo cho giảng viên
- xuất báo cáo PDF
- quay video hoặc chụp ảnh minh chứng

---

## 2. Kiểm tra source code

- [ ] Solution mở được bình thường
- [ ] Các project chính còn đầy đủ:
  - [ ] `SmartQRCoffee.API`
  - [ ] `SmartQRCoffee.Services`
  - [ ] `SmartQRCoffee.Repositories`
- [ ] Không xóa nhầm migration
- [ ] Không còn file test tạm rác không cần thiết
- [ ] Không commit nhầm secret hoặc file nhạy cảm ngoài ý muốn

---

## 3. Kiểm tra backend startup

- [ ] Đã tắt các process API cũ nếu bị lock file
- [ ] Chạy được lệnh:

```bash
dotnet run --project SmartQRCoffee.API
```

- [ ] API lên tại: `http://localhost:5121`
- [ ] Swagger mở được tại: `http://localhost:5121/swagger`

---

## 4. Kiểm tra database

- [ ] Supabase / PostgreSQL đang hoạt động
- [ ] Migration đã được apply đầy đủ
- [ ] Dữ liệu test đã có trong DB

### Dữ liệu test cần tồn tại
- [ ] role `Admin`
- [ ] role `Staff`
- [ ] role `Cashier`
- [ ] `table-01-demo-token`
- [ ] products mẫu
- [ ] product options mẫu

---

## 5. Kiểm tra authentication

- [ ] Login bằng `admin_demo / 123456` thành công
- [ ] Login bằng `staff_demo / 123456` thành công
- [ ] Login bằng `cashier_demo / 123456` thành công
- [ ] Cookie `access_token` được set
- [ ] Cookie `refresh_token` được set
- [ ] `exchange-token` hoạt động
- [ ] `logout` hoạt động

---

## 6. Kiểm tra role authorization

- [ ] `Admin` gọi `POST /api/v1/Users/register` thành công
- [ ] `Staff` gọi `POST /api/v1/Users/register` bị chặn `403`
- [ ] `Cashier` gọi `POST /api/v1/Users/register` bị chặn `403` (nếu test)
- [ ] `Admin` update order status được
- [ ] `Staff` update order status được
- [ ] `Cashier` update order status được

---

## 7. Kiểm tra business flow chính

### Menu
- [ ] `GET /api/v1/Tables/table-01-demo-token/menu` trả `200 OK`
- [ ] menu hiển thị đúng category / product / option

### Order
- [ ] submit order cơ bản thành công
- [ ] submit order có options thành công
- [ ] tổng tiền tính đúng
- [ ] order được lưu xuống DB
- [ ] payment record được tạo

### Order Status
- [ ] update `Preparing` thành công
- [ ] update `Completed` thành công
- [ ] test `Order not found` trả lỗi đúng

---

## 8. Kiểm tra SignalR realtime

- [ ] Hub endpoint hoạt động: `/hubs/notifications`
- [ ] Trang test mở được: `http://localhost:5121/signalr-test.html`
- [ ] Kết nối hub thành công
- [ ] `JoinKitchen()` hoạt động
- [ ] `JoinTableGroup(1)` hoạt động
- [ ] Kitchen nhận `ReceiveNewOrder`
- [ ] Table group nhận `ReceiveOrderStatusChanged`

---

## 9. Kiểm tra tài liệu

- [ ] `SMARTQRCOFFEE_HLD_DD.md`
- [ ] `SMARTQRCOFFEE_ARCHITECTURE_AND_BUSINESS_FLOW.md`
- [ ] `TEST_GUIDE_SMARTQRCOFFEE.md`
- [ ] `PROJECT_TEMPLATE_GITFLOW.md`
- [ ] `PROJECT_TEMPLATE_TEST_GUIDE.md`

### Nếu cần nộp PDF
- [ ] Đã copy nội dung sang Word
- [ ] Đã chỉnh format
- [ ] Đã export PDF

---

## 10. Kiểm tra phần trình bày / demo

- [ ] Có thể giải thích kiến trúc 3 lớp
- [ ] Có thể giải thích JWT + Refresh Token + HttpOnly Cookie
- [ ] Có thể giải thích vì sao dùng SignalR
- [ ] Có thể demo flow QR -> menu -> order -> realtime -> update status
- [ ] Có thể giải thích role-based authorization dùng để làm gì

---

## 11. Những gì không bắt buộc nếu không còn thời gian

- [ ] Frontend hoàn chỉnh
- [ ] Dashboard/reporting nâng cao
- [ ] CRUD admin đầy đủ cho mọi module
- [ ] tách bảng RefreshToken riêng
- [ ] fix sạch toàn bộ nullable warnings

---

## 12. Điều kiện để tự tin nộp bài

Bạn có thể tự tin nộp bài nếu:

- [ ] Backend chạy được
- [ ] Swagger mở được
- [ ] Login hoạt động
- [ ] Order flow hoạt động end-to-end
- [ ] SignalR realtime hoạt động
- [ ] Role authorization hoạt động
- [ ] Có HLD + DD và test guide

Nếu các mục trên đều pass, project đã đạt mức khá tốt để nộp và demo.
