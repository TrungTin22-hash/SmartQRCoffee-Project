# Project Template - Test Guide

## 1. Mục tiêu

Template này dùng để tạo file hướng dẫn test cho bất kỳ dự án nào.

Bạn chỉ cần copy file này rồi thay các phần trong dấu `<>` theo dự án thực tế.

---

## 2. Thông tin dự án

### Tên dự án
- `<PROJECT_NAME>`

### Backend URL
- `<BACKEND_URL>`

### Swagger / API Docs URL
- `<SWAGGER_URL>`

### Frontend URL
- `<FRONTEND_URL>`

### Realtime URL / Hub URL (nếu có)
- `<REALTIME_URL>`

---

## 3. Dữ liệu test

### 3.1. Tài khoản test
- `<USER_1>` / `<PASSWORD_1>` / role `<ROLE_1>`
- `<USER_2>` / `<PASSWORD_2>` / role `<ROLE_2>`

### 3.2. Dữ liệu nghiệp vụ mẫu
- `<ENTITY_SAMPLE_1>`
- `<ENTITY_SAMPLE_2>`
- `<ENTITY_SAMPLE_3>`

Ví dụ:
- product mẫu
- table token mẫu
- category mẫu
- asset mẫu
- quote sample

---

## 4. Chạy dự án

### Backend
```bash
<COMMAND_RUN_BACKEND>
```

### Frontend
```bash
<COMMAND_RUN_FRONTEND>
```

### Kết quả mong đợi
- backend chạy tại `<BACKEND_URL>`
- frontend chạy tại `<FRONTEND_URL>`

---

## 5. Kiểm tra tài liệu API

Mở:
- `<SWAGGER_URL>`

Kiểm tra có các nhóm API chính:
- `<API_GROUP_1>`
- `<API_GROUP_2>`
- `<API_GROUP_3>`

---

## 6. Test authentication

### Endpoint
- `<AUTH_LOGIN_ENDPOINT>`

### Request mẫu
```json
{
  "username": "<USERNAME>",
  "password": "<PASSWORD>"
}
```

### Kết quả mong đợi
- HTTP `200 OK`
- nhận token hoặc cookie auth
- user đăng nhập thành công

### Test lỗi
- sai password
- user không tồn tại
- token hết hạn

---

## 7. Test luồng nghiệp vụ chính

## 7.1. Use case 1: `<MAIN_FLOW_1>`

### Endpoint
- `<ENDPOINT_1>`

### Request mẫu
```json
{
  "sample": "value"
}
```

### Kết quả mong đợi
- HTTP `<EXPECTED_STATUS>`
- response có `<EXPECTED_FIELDS>`

---

## 7.2. Use case 2: `<MAIN_FLOW_2>`

### Endpoint
- `<ENDPOINT_2>`

### Request mẫu
```json
{
  "sample": "value"
}
```

### Kết quả mong đợi
- HTTP `<EXPECTED_STATUS>`
- response có `<EXPECTED_FIELDS>`

---

## 7.3. Use case 3: `<MAIN_FLOW_3>`

### Endpoint
- `<ENDPOINT_3>`

### Request mẫu
```json
{
  "sample": "value"
}
```

### Kết quả mong đợi
- HTTP `<EXPECTED_STATUS>`
- response có `<EXPECTED_FIELDS>`

---

## 8. Test cập nhật trạng thái / xử lý tiếp theo

### Endpoint
- `<STATUS_UPDATE_ENDPOINT>`

### Request mẫu
```json
{
  "status": "<NEW_STATUS>"
}
```

### Kết quả mong đợi
- HTTP `200 OK`
- trạng thái được cập nhật đúng

---

## 9. Test realtime (nếu có)

### Realtime endpoint / hub
- `<REALTIME_URL>`

### Cách kết nối
- mở client realtime
- join group / channel `<GROUP_NAME>`
- giữ kết nối mở

### Event cần kiểm tra
- `<EVENT_1>`
- `<EVENT_2>`

### Kịch bản test
1. kết nối realtime client
2. trigger hành động từ API
3. xác nhận event được nhận ở client

---

## 10. Test UI (nếu có frontend)

### Màn hình 1
- `<SCREEN_1>`
- kiểm tra:
  - `<CHECK_1>`
  - `<CHECK_2>`

### Màn hình 2
- `<SCREEN_2>`
- kiểm tra:
  - `<CHECK_1>`
  - `<CHECK_2>`

---

## 11. Test lỗi / validation

Các trường hợp nên test:

- [ ] thiếu field bắt buộc
- [ ] dữ liệu sai format
- [ ] user không có quyền
- [ ] entity không tồn tại
- [ ] token hết hạn
- [ ] request duplicate
- [ ] realtime không kết nối được

---

## 12. Checklist demo nhanh

### Backend
- [ ] API chạy thành công
- [ ] Swagger mở được
- [ ] Database kết nối được

### Auth
- [ ] Login thành công
- [ ] Logout thành công
- [ ] Token/cookie hoạt động đúng

### Business flow
- [ ] Flow 1 pass
- [ ] Flow 2 pass
- [ ] Flow 3 pass

### Realtime
- [ ] Kết nối hub/channel thành công
- [ ] Nhận event đúng

### UI
- [ ] Form submit được
- [ ] Dữ liệu hiển thị đúng
- [ ] Trạng thái cập nhật đúng

---

## 13. Các lỗi thường gặp

### Backend không chạy
- kiểm tra port
- kiểm tra process cũ đang khóa file
- kiểm tra DB connection string

### API trả lỗi 400/401/403
- kiểm tra body request
- kiểm tra token/cookie
- kiểm tra role/permission

### Realtime không nhận event
- kiểm tra URL hub đúng chưa
- kiểm tra đã join đúng group chưa
- kiểm tra auth cho hub

### Frontend không gọi được API
- kiểm tra CORS
- kiểm tra base URL
- kiểm tra HTTPS / cookie policy

---

## 14. Kết luận

Nếu toàn bộ checklist phía trên pass, có thể kết luận:
- `<PROJECT_NAME>` hoạt động đúng ở mức backend
- các flow chính đã được kiểm thử
- realtime/UI/auth đã được xác nhận theo yêu cầu dự án
