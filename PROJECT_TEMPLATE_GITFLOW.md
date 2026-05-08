# Project Template - Git Flow

## 1. Mục tiêu

Template này dùng để tái sử dụng quy trình quản lý source code cho các dự án sau.

Áp dụng tốt cho:
- đồ án nhóm
- backend API
- frontend app
- fullstack project
- sản phẩm có nhiều feature song song

---

## 2. Branch Strategy

### 2.1. Long-lived branches

#### `main`
- dùng cho code ổn định nhất
- chỉ merge từ `release/*` hoặc `hotfix/*`
- không commit trực tiếp
- nên gắn tag version khi release

#### `develop`
- branch tích hợp chính cho dev
- tất cả feature branch sẽ merge vào đây
- không commit trực tiếp nếu làm việc theo team nghiêm ngặt

---

### 2.2. Supporting branches

#### `feature/<ticket_or_feature_name>`
Dùng để phát triển tính năng mới.

Ví dụ:
- `feature/login-api`
- `feature/TGL-101_order-flow`
- `feature/signalr-realtime`

#### `release/vX.Y.Z`
Dùng khi chuẩn bị phát hành.

Ví dụ:
- `release/v1.0.0`
- `release/v1.2.3`

#### `hotfix/<issue_name>`
Dùng để sửa lỗi khẩn cấp từ production.

Ví dụ:
- `hotfix/payment-bug`
- `hotfix/login-cookie-fix`

---

## 3. Quy tắc đặt tên branch

### Feature branch
```text
feature/<short-name>
feature/<ticket>_<short-name>
```

Ví dụ:
```text
feature/login-api
feature/TGL-101_login-api
feature/order-status-realtime
```

### Release branch
```text
release/v1.0.0
release/v1.1.0
```

### Hotfix branch
```text
hotfix/payment-bug
hotfix/jwt-cookie-expire
```

---

## 4. Workflow chuẩn

### 4.1. Feature Development

```bash
git checkout develop
git pull
git checkout -b feature/your-feature-name
```

Sau khi code xong:

```bash
git add .
git commit -m "feat: add your feature"
git push origin feature/your-feature-name
```

Sau đó:
- tạo Pull Request vào `develop`
- chờ review
- CI pass
- merge

---

### 4.2. Release Process

```bash
git checkout develop
git pull
git checkout -b release/v1.0.0
```

Sửa bug cuối cùng, update version nếu cần:

```bash
git add .
git commit -m "chore: prepare release v1.0.0"
```

Merge vào `main`:

```bash
git checkout main
git merge --no-ff release/v1.0.0
git tag v1.0.0
```

Merge ngược lại `develop`:

```bash
git checkout develop
git merge release/v1.0.0
```

---

### 4.3. Hotfix Process

```bash
git checkout main
git pull
git checkout -b hotfix/issue-name
```

Sau khi sửa:

```bash
git add .
git commit -m "fix: resolve production issue"
```

Merge vào `main`:

```bash
git checkout main
git merge hotfix/issue-name
git tag v1.0.1
```

Merge ngược lại `develop`:

```bash
git checkout develop
git merge hotfix/issue-name
```

---

## 5. Commit Message Convention

Khuyến nghị dùng convention đơn giản:

- `feat:` thêm tính năng
- `fix:` sửa lỗi
- `refactor:` refactor code
- `docs:` tài liệu
- `test:` test
- `chore:` việc kỹ thuật/phụ trợ

Ví dụ:

```text
feat: add JWT login API
fix: correct order total calculation
refactor: move db config to appsettings
docs: add test guide for realtime flow
chore: prepare release v1.0.0
```

---

## 6. Pull Request Rules

Trước khi merge PR, nên đảm bảo:

- [ ] branch cập nhật từ branch gốc mới nhất
- [ ] code build thành công
- [ ] test pass
- [ ] không có secret nhạy cảm bị commit nhầm
- [ ] reviewer đã approve
- [ ] mô tả PR rõ ràng

---

## 7. Golden Rules

- không commit trực tiếp vào `main`
- hạn chế commit trực tiếp vào `develop`
- luôn tạo PR nếu làm việc nhóm
- luôn review code trước khi merge
- ưu tiên squash merge nếu lịch sử commit quá vụn
- không push secret thật lên repo
- release nên theo SemVer: `vMAJOR.MINOR.PATCH`

---

## 8. Khi nào nên dùng bản rút gọn

Nếu dự án nhỏ hoặc cá nhân, có thể dùng bản nhẹ:

- `main`
- `develop`
- `feature/*`

Không bắt buộc phải có `release/*` và `hotfix/*` nếu không cần deployment chuẩn.

---

## 9. Checklist áp dụng cho dự án mới

Khi bắt đầu dự án mới, thay các mục sau:

- tên team / ticket prefix
- quy tắc đặt tên branch
- chính sách review
- chiến lược merge
- versioning rule
- CI/CD rule

---

## 10. Mẫu nhanh để copy

### Tạo feature
```bash
git checkout develop
git pull
git checkout -b feature/my-feature
```

### Commit
```bash
git add .
git commit -m "feat: add my feature"
```

### Push
```bash
git push origin feature/my-feature
```

### Release
```bash
git checkout develop
git checkout -b release/v1.0.0
```

### Hotfix
```bash
git checkout main
git checkout -b hotfix/critical-bug
```
