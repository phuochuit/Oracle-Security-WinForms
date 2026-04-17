# Đồ án Bảo mật Cơ sở dữ liệu - Oracle & C# WinForms

[cite_start]Đây là ứng dụng quản lý và cấu hình bảo mật trực quan cho hệ quản trị cơ sở dữ liệu Oracle, được xây dựng bằng C# (Windows Forms)[cite: 160]. Ứng dụng giúp đơn giản hóa việc thực thi các kỹ thuật bảo mật phức tạp của Oracle thông qua giao diện người dùng.

## 👥 Thành viên nhóm
1. [cite_start]Trương Tô Đình Phước [cite: 10]

## 🛠 Công nghệ sử dụng
- [cite_start]**Cơ sở dữ liệu:** Oracle Database[cite: 79].
- [cite_start]**Ngôn ngữ lập trình:** C# (Windows Forms)[cite: 160].
- [cite_start]**Thư viện kết nối:** Oracle.ManagedDataAccess[cite: 160].

## 🚀 Các tính năng bảo mật nổi bật
- **Xác thực & Điều khiển truy cập:**
  - [cite_start]Xác thực hai lớp (2FA) thông qua mã OTP gửi qua Email[cite: 526].
  - [cite_start]Phân quyền người dùng theo vai trò (RBAC) và giới hạn tài nguyên hệ thống qua Oracle Profile[cite: 527].
- **Mã hóa dữ liệu:**
  - [cite_start]Áp dụng mã hóa đối xứng (DES) và bất đối xứng (RSA) ở mức ứng dụng[cite: 529].
  - [cite_start]Triển khai mô hình mã hóa lai (Hybrid Encryption) để tối ưu hiệu năng[cite: 530].
  - [cite_start]Sử dụng Stored Functions để mã hóa dữ liệu nhạy cảm (mật khẩu) trực tiếp trên Database[cite: 531].
- **Giám sát & Quản lý truy cập:**
  - [cite_start]Cài đặt VPD (Virtual Private Database) để kiểm soát dữ liệu hiển thị trên từng dòng[cite: 533].
  - [cite_start]Giám sát truy cập mức độ tinh (FGA - Fine-Grained Auditing)[cite: 534].
  - [cite_start]Theo dõi lịch sử đăng nhập/đăng xuất bằng Standard Auditing để phát hiện Brute-force[cite: 535].
- **Phục hồi dữ liệu:**
  - [cite_start]Tích hợp công nghệ Flashback Table, cho phép khôi phục dữ liệu nhanh chóng khi có sai sót mà không cần khôi phục toàn bộ DB[cite: 536].

## 📸 Hình ảnh demo
<img width="1080" height="725" alt="image" src="https://github.com/user-attachments/assets/c61b341b-416b-4674-8f4b-1aa0175fa1f5" />

<img width="1257" height="721" alt="image" src="https://github.com/user-attachments/assets/1859127d-0e79-4533-aba7-0fd8f347f261" />

<img width="1010" height="550" alt="image" src="https://github.com/user-attachments/assets/1fbd2ef2-1e2e-429d-bed4-7b0dc0665d19" />

<img width="1159" height="576" alt="image" src="https://github.com/user-attachments/assets/12552169-9a8a-4da8-ae12-3cfe14d1f07a" />

<img width="977" height="466" alt="image" src="https://github.com/user-attachments/assets/edb8e257-e12d-4b4d-b91f-cae4a7041f83" />


