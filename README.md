 <div align="center">

# 🛵 DÂN TỔ PHÓNG

**HỆ THỐNG QUẢN LÝ NGƯỜI DÙNG & GIAO TIẾP CLIENT-SERVER QUA GIAO THỨC TCP**

[![Language](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Framework](https://img.shields.io/badge/Framework-.NET%20WinForms-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Database](https://img.shields.io/badge/Database-SQL%20Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Architecture](https://img.shields.io/badge/Architecture-TCP%20Socket-0078D4?style=for-the-badge)](https://en.wikipedia.org/wiki/Network_socket)
[![Protocol](https://img.shields.io/badge/Protocol-JSON%20Framing-000000?style=for-the-badge&logo=json&logoColor=white)](https://www.json.org/)

*Đồ án môn học Truyền dữ liệu / Mạng máy tính — Xây dựng hệ thống Client-Server sử dụng TCP Socket, xử lý bất đồng bộ Async/Await và giao thức trao đổi dữ liệu JSON.*

</div>

---

## 📑 Mục lục

- [Giới thiệu dự án](#-giới-thiệu-dự-án)
- [Kiến trúc hệ thống](#-kiến-trúc-hệ-thống)
- [Tính năng nổi bật](#-tính-năng-nổi-bật)
- [Cấu trúc thư mục](#-cấu-trúc-thư-mục)
- [Giao thức truyền thông (Protocol)](#-giao-thức-truyền-thông-protocol)
- [Quy trình hoạt động](#-quy-trình-hoạt-động)
- [Cơ sở dữ liệu](#-cơ-sở-dữ-liệu)
- [Bảo mật & Kiểm thử](#-bảo-mật--kiểm-thử-qa)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Hướng dẫn cài đặt](#-hướng-dẫn-cài-đặt)
- [Hướng phát triển](#-hướng-phát-triển)
- [Tác giả](#-tác-giả)

---

## 📖 Giới thiệu dự án

**Dân Tổ Phóng** là dự án xây dựng hệ thống quản lý người dùng và hạ tầng giao tiếp mạng theo mô hình **Client-Server**, sử dụng giao thức TCP Socket trên nền tảng C# Windows Forms.

Hệ thống được tổ chức thành ba thành phần độc lập:

- **GameShared:** Thư viện chứa các Model, Command và Status Code dùng chung.
- **GameServer:** Máy chủ tiếp nhận, xử lý yêu cầu và quản lý kết nối Client.
- **UserManagement:** Ứng dụng Client cung cấp giao diện đăng nhập, đăng ký và quản lý thông tin người dùng.

Điểm nổi bật của dự án là việc tự xây dựng cơ chế trao đổi dữ liệu trên TCP Stream bằng JSON kết hợp với kỹ thuật Message Framing, nhằm giải quyết vấn đề phân mảnh và dính gói tin vốn có của TCP.

Bên cạnh đó, hệ thống sử dụng cơ chế xử lý bất đồng bộ `async/await`, quản lý nhiều kết nối đồng thời và thiết kế giao diện WinForms theo hướng hiện đại, linh hoạt.

### 🎯 Mục tiêu dự án

- Nắm vững nguyên lý hoạt động của mô hình Client-Server.
- Hiểu cơ chế thiết lập và quản lý kết nối TCP Socket.
- Tự xây dựng giao thức truyền nhận dữ liệu trên TCP Stream.
- Giải quyết vấn đề TCP Packet Fragmentation và TCP Coalescing.
- Áp dụng lập trình bất đồng bộ để xử lý nhiều Client.
- Kết nối và thao tác cơ sở dữ liệu SQL Server.
- Xây dựng giao diện quản lý Server và Client bằng WinForms.
- Tăng tính ổn định, khả năng mở rộng và trải nghiệm người dùng.

---

## 🏗️ Kiến trúc hệ thống

Dự án áp dụng mô hình Client-Server kết hợp thư viện dùng chung (Shared Library).

### 1. Sơ đồ kiến trúc tổng quan

```text
                    USER
                      |
                      v
          +-----------------------+
          |     UserManagement    |
          |        CLIENT         |
          |-----------------------|
          | LoginForm             |
          | RegisterForm          |
          | MainForm              |
          | NetworkService        |
          +-----------+-----------+
                      |
                      |
                 TCP Socket
                JSON / UTF-8
                      |
                      v
          +-----------------------+
          |      GameServer       |
          |        SERVER         |
          |-----------------------|
          | GameTcpServer         |
          | Client Management     |
          | Request Processing    |
          | Session Management    |
          | Logging               |
          +-----------+-----------+
                      |
                      |
                 Data Access
                      |
                      v
          +-----------------------+
          |      SQL Server       |
          |       DATABASE        |
          +-----------------------+

                 GameShared
            Shared Class Library
          (Models / Commands / Codes)
```

### 2. Phân chia trách nhiệm

| Project | Vai trò | Nhiệm vụ |
|---|---|---|
| GameShared | Shared Library | Chuẩn hóa dữ liệu giao tiếp |
| GameServer | Server | Quản lý kết nối và xử lý nghiệp vụ |
| UserManagement | Client | Giao diện và gửi yêu cầu đến Server |

### 3. Nguyên tắc thiết kế

- **Separation of Concerns:** Phân chia rõ ràng trách nhiệm giữa các thành phần.
- **Reusability:** Tái sử dụng Model và định nghĩa giao thức thông qua GameShared.
- **Asynchronous Processing:** Xử lý các thao tác mạng bất đồng bộ.
- **Centralized Management:** Tập trung quản lý kết nối và phiên làm việc tại Server.
- **Maintainability:** Dễ dàng bảo trì và mở rộng chức năng.

---

## ✨ Tính năng nổi bật

### 🖥️ Phía Máy Chủ (`GameServer`)

- [x] **Xử lý đa Client bất đồng bộ:** Sử dụng `TcpListener`, `async/await` để tiếp nhận và xử lý nhiều kết nối đồng thời mà không chặn luồng giao diện chính.
- [x] **Quản lý kết nối tập trung:** Sử dụng `ConcurrentDictionary` để quản lý danh sách Client đang kết nối.
- [x] **Giám sát thời gian thực:** Dashboard hiển thị Uptime thông qua `Stopwatch`, tổng số kết nối và danh sách Client.
- [x] **Theo dõi thông tin Client:** Hiển thị địa chỉ IP, Port và trạng thái kết nối.
- [x] **Ghi nhật ký hoạt động (Logging):** Theo dõi Request, Response, trạng thái kết nối và các lỗi phát sinh.
- [x] **Xử lý lỗi JSON:** Theo dõi và xử lý lỗi phân tích dữ liệu thông qua `JsonErrorInfo`.
- [x] **Quản lý vòng đời Session:** Cấp phát `Guid` định danh phiên làm việc cho từng Client.
- [x] **Tự động dọn dẹp kết nối:** Giải phóng tài nguyên khi Client ngắt kết nối hoặc gửi lệnh `DANGXUAT`.

### 💻 Phía Máy Khách (`UserManagement`)

- [x] **Giao diện Borderless:** Loại bỏ viền cửa sổ Windows mặc định bằng `FormBorderStyle.None`.
- [x] **Kéo thả cửa sổ:** Tích hợp thao tác kéo thả Form bằng chuột.
- [x] **Quản lý trạng thái cửa sổ:** Hỗ trợ điều khiển trạng thái thông qua `WindowState`.
- [x] **Cấu hình mạng động:** Cho phép thay đổi IP và Port kết nối trực tiếp trên giao diện Login.
- [x] **Validation:** Kiểm tra tính hợp lệ của dữ liệu trước khi gửi Request.
- [x] **Visual Error Feedback:** Hiển thị cảnh báo lỗi bằng màu vàng trực quan ngay trên Form.
- [x] **Form Navigation:** Tự động chuyển đổi giữa Login, Register và Main Form.
- [x] **Đồng bộ trạng thái giao diện:** Duy trì trạng thái cửa sổ khi chuyển đổi giữa các Form.

---

## 📂 Cấu trúc thư mục

Dự án được chia thành 3 Projects độc lập để tối ưu hóa khả năng đóng gói, tái sử dụng và bảo trì mã nguồn.

```bash
📦 BaiTap3-TCP-ClientServer
 ┣ 📂 GameShared                  # [Class Library] Thư viện dùng chung
 ┃ ┣ 📜 Commands.cs               # Danh sách hằng số Action
 ┃ ┣ 📜 StatusCodes.cs            # Bảng mã trạng thái phản hồi
 ┃ ┣ 📜 NetworkRequest.cs         # Khuôn mẫu dữ liệu gửi đi
 ┃ ┗ 📜 NetworkResponse.cs        # Khuôn mẫu dữ liệu nhận về
 ┃
 ┣ 📂 GameServer                  # [WinForms] Ứng dụng máy chủ
 ┃ ┣ 📂 DataAccess                # Tầng truy xuất SQL Server (DAO)
 ┃ ┣ 📜 GameTcpServer.cs          # Core xử lý TCP Socket
 ┃ ┗ 📜 ServerForm.cs             # Giao diện Dashboard Server
 ┃
 ┗ 📂 UserManagement              # [WinForms] Ứng dụng máy khách
   ┣ 📜 NetworkService.cs         # Core kết nối và trao đổi dữ liệu
   ┣ 📜 LoginForm.cs              # Giao diện đăng nhập
   ┣ 📜 RegisterForm.cs           # Giao diện đăng ký
   ┗ 📜 MainForm.cs               # Giao diện hồ sơ người dùng
```

### 📌 Chi tiết các thành phần

<details>
<summary><b>📦 GameShared – Shared Library</b></summary>

Thư viện chứa các thành phần được sử dụng chung giữa Client và Server.

| File | Chức năng |
|---|---|
| `Commands.cs` | Định nghĩa các hành động giao tiếp |
| `StatusCodes.cs` | Chuẩn hóa mã trạng thái phản hồi |
| `NetworkRequest.cs` | Định nghĩa cấu trúc Request |
| `NetworkResponse.cs` | Định nghĩa cấu trúc Response |

**Vai trò chính:**

- Đồng bộ cấu trúc dữ liệu giữa hai phía.
- Hạn chế trùng lặp định nghĩa Model.
- Giúp việc mở rộng giao thức dễ dàng hơn.

</details>

<details>
<summary><b>🖥️ GameServer – Server Application</b></summary>

Ứng dụng chịu trách nhiệm tiếp nhận kết nối và xử lý yêu cầu từ các Client.

| File / Folder | Chức năng |
|---|---|
| `DataAccess/` | Truy xuất và thao tác cơ sở dữ liệu |
| `GameTcpServer.cs` | Quản lý TCP Listener và xử lý Client |
| `ServerForm.cs` | Hiển thị trạng thái và thông tin Server |

**Vai trò chính:**

- Lắng nghe kết nối TCP.
- Tiếp nhận và phân tích Request.
- Thực hiện xử lý nghiệp vụ.
- Quản lý Session và vòng đời kết nối.
- Gửi Response về Client.
- Ghi nhật ký và theo dõi lỗi.

</details>

<details>
<summary><b>💻 UserManagement – Client Application</b></summary>

Ứng dụng giao diện dành cho người dùng cuối.

| File | Chức năng |
|---|---|
| `NetworkService.cs` | Thiết lập kết nối và gửi/nhận dữ liệu |
| `LoginForm.cs` | Xác thực tài khoản và cấu hình mạng |
| `RegisterForm.cs` | Đăng ký tài khoản |
| `MainForm.cs` | Hiển thị thông tin người dùng |

**Vai trò chính:**

- Quản lý giao diện người dùng.
- Thiết lập kết nối đến Server.
- Gửi Request và nhận Response.
- Kiểm tra dữ liệu đầu vào.
- Điều hướng giữa các Form.

</details>

---

## 🔌 Giao thức truyền thông (Protocol)

Hệ thống sử dụng **TCP Socket** làm giao thức truyền tải dữ liệu giữa Client và Server.

TCP cung cấp luồng dữ liệu đáng tin cậy, có thứ tự, nhưng không bảo toàn ranh giới giữa các thông điệp được gửi.

Do đó, dự án xây dựng cơ chế **JSON Message Framing** để xác định chính xác điểm kết thúc của từng thông điệp.

### 1. Cấu trúc đóng khung (Message Framing)

<details open>
<summary><b>📦 Chi tiết cơ chế JSON Line Delimited</b></summary>

Dữ liệu được chuyển đổi sang chuỗi JSON, mã hóa bằng UTF-8 và phân tách bằng ký tự xuống dòng `\n`.

**Ví dụ dữ liệu truyền trên TCP Stream:**

```text
{"Action":"DANGNHAP","Username":"admin","Password":"123"}\n
{"Action":"LAYTHONGTIN"}\n
{"Action":"DANGXUAT"}\n
```

Trong đó:

- JSON: Định dạng biểu diễn dữ liệu.
- UTF-8: Bộ mã hóa ký tự.
- `\n`: Ký tự đánh dấu kết thúc một thông điệp.
- TCP Stream: Kênh truyền dữ liệu giữa hai đầu kết nối.

**Cơ chế xử lý:**

1. Client tuần tự hóa đối tượng Request thành JSON.
2. Mã hóa JSON thành mảng byte UTF-8.
3. Gửi dữ liệu qua TCP Socket kèm ký tự kết thúc dòng.
4. Server đọc dữ liệu từ Stream.
5. Sử dụng `ReadLineAsync()` để xác định ranh giới từng thông điệp.
6. Deserialize JSON thành đối tượng tương ứng.

Ví dụ phía Client:

```csharp
string json = JsonSerializer.Serialize(request);

await writer.WriteLineAsync(json);
await writer.FlushAsync();
```

Phía Server:

```csharp
string line = await reader.ReadLineAsync();

if (line != null)
{
    var request = JsonSerializer.Deserialize<NetworkRequest>(line);
}
```

*Lưu ý: Đoạn code trên minh họa cơ chế truyền nhận theo từng dòng; cần điều chỉnh theo đúng kiểu StreamReader/StreamWriter và cấu trúc thực tế của dự án.*

</details>

### 2. Giải quyết vấn đề TCP Fragmentation & Coalescing

<details open>
<summary><b>🔍 Tại sao cần Message Framing?</b></summary>

TCP là giao thức hướng luồng (Stream-Oriented Protocol), không phải giao thức hướng thông điệp (Message-Oriented Protocol).

Điều này dẫn đến hai tình huống:

**TCP Fragmentation – Phân mảnh dữ liệu**

Một thông điệp được gửi đi có thể được chia thành nhiều lần nhận tại phía Server.

```text
Client gửi:
+------------------------------------------+
| {"Action":"DANGNHAP","Username":"admin"} |
+------------------------------------------+

Server có thể nhận:
+------------------+
| {"Action":"DANGN |
+------------------+

+------------------+
| HAP","Username": |
+------------------+

+------------------+
| "admin"}\n       |
+------------------+
```

**TCP Coalescing – Dính nhiều thông điệp**

Nhiều thông điệp được gửi liên tiếp có thể xuất hiện trong cùng một lần đọc dữ liệu.

```text
Client gửi:
+------------------+------------------+
| Request 1        | Request 2        |
+------------------+------------------+

Server nhận:
+-------------------------------------+
| Request 1 + Request 2               |
+-------------------------------------+
```

**Giải pháp của dự án:**

Sử dụng ký tự xuống dòng `\n` làm dấu phân cách giữa các thông điệp.

`ReadLineAsync()` đọc dữ liệu cho đến khi gặp ký tự kết thúc dòng, giúp tách từng thông điệp độc lập ngay cả khi dữ liệu đến theo nhiều đợt.

</details>

### 3. Cấu trúc Request

Request là đối tượng đại diện cho yêu cầu được gửi từ Client đến Server.

Ví dụ minh họa:

```json
{
    "Action": "DANGNHAP",
    "Username": "admin",
    "Password": "123"
}
```

| Trường | Ý nghĩa |
|---|---|
| Action | Xác định hành động cần thực hiện |
| Username | Tên tài khoản |
| Password | Mật khẩu |

Các Action được định nghĩa tập trung trong `Commands.cs`.

### 4. Cấu trúc Response

Response là dữ liệu phản hồi từ Server sau khi xử lý Request.

Ví dụ minh họa:

```json
{
    "StatusCode": 200,
    "Message": "Dang nhap thanh cong",
    "Data": {
        "Username": "admin"
    }
}
```

| Trường | Ý nghĩa |
|---|---|
| StatusCode | Mã trạng thái xử lý |
| Message | Thông báo kết quả |
| Data | Dữ liệu phản hồi |

Các mã trạng thái được quản lý tập trung trong `StatusCodes.cs`.

*Các trường JSON trên là ví dụ mô tả giao thức; tên trường và cấu trúc thực tế phụ thuộc vào Model trong GameShared.*

### 5. Các nhóm lệnh giao tiếp

| Nhóm chức năng | Action minh họa | Mô tả |
|---|---|---|
| Đăng nhập | `DANGNHAP` | Xác thực tài khoản |
| Đăng ký | `DANGKY` | Tạo tài khoản mới |
| Lấy thông tin | `LAYTHONGTIN` | Truy vấn hồ sơ người dùng |
| Đăng xuất | `DANGXUAT` | Kết thúc phiên làm việc |

### 6. Cơ chế quản lý Session

Mỗi Client được cấp một định danh Session thông qua `Guid`.

```text
Client A  ---> Session ID: GUID_A
Client B  ---> Session ID: GUID_B
Client C  ---> Session ID: GUID_C
```

Session hỗ trợ:

- Phân biệt các phiên kết nối.
- Theo dõi trạng thái hoạt động.
- Quản lý vòng đời Client.
- Giải phóng thông tin phiên khi ngắt kết nối.
- Xử lý yêu cầu đăng xuất.

---

## 🔄 Quy trình hoạt động

### 1. Khởi động Server

```text
START SERVER
     |
     v
Initialize TCP Listener
     |
     v
Bind IP Address & Port
     |
     v
Start Listening
     |
     v
Wait for Client Connections
```

Server khởi tạo TCP Listener và bắt đầu lắng nghe kết nối đến từ Client.

### 2. Thiết lập kết nối Client

```text
START CLIENT
     |
     v
Open Login Form
     |
     v
Enter Server IP & Port
     |
     v
Connect to Server
     |
     v
Connection Established
```

Client sử dụng IP và Port được cấu hình trên giao diện Login để thiết lập kết nối.

### 3. Quy trình xử lý Request/Response

```text
       CLIENT                         SERVER
          |                              |
          |------- TCP Connect --------->|
          |                              |
          |------- JSON Request -------->|
          |                              |
          |                       Read Stream
          |                              |
          |                       Parse JSON
          |                              |
          |                       Process Action
          |                              |
          |                       Access Database
          |                              |
          |<------ JSON Response --------|
          |                              |
      Parse Response                     |
          |                              |
     Update UI                           |
          |                              |
```

### 4. Quy trình đăng nhập

1. Người dùng nhập Username và Password.
2. Client thực hiện Validation.
3. Client tạo Request với Action `DANGNHAP`.
4. Request được gửi đến Server.
5. Server kiểm tra thông tin tài khoản.
6. Server truy xuất SQL Server để xác thực.
7. Server trả về Response.
8. Client phân tích Status Code.
9. Nếu thành công, Client chuyển sang MainForm.
10. Nếu thất bại, Client hiển thị thông báo tương ứng.

### 5. Quy trình đăng xuất

1. Người dùng thực hiện thao tác đăng xuất.
2. Client gửi Request `DANGXUAT`.
3. Server tiếp nhận yêu cầu.
4. Server xử lý và kết thúc Session tương ứng.
5. Server cập nhật trạng thái Client.
6. Client quay về giao diện Login.

---

## 🗄️ Cơ sở dữ liệu

Hệ thống sử dụng **Microsoft SQL Server** để lưu trữ và quản lý thông tin người dùng.

Lớp truy xuất dữ liệu được tổ chức trong thư mục `DataAccess` thuộc Project `GameServer`.

### Vai trò của tầng DataAccess

- Thực hiện truy vấn dữ liệu.
- Xử lý thao tác thêm, sửa và lấy thông tin.
- Tách biệt nghiệp vụ xử lý mạng với truy xuất cơ sở dữ liệu.
- Hỗ trợ tái sử dụng các thao tác dữ liệu.

### Luồng truy xuất dữ liệu

```text
Client Request
      |
      v
GameTcpServer
      |
      v
Business Processing
      |
      v
DataAccess (DAO)
      |
      v
SQL Server
      |
      v
Query Result
      |
      v
NetworkResponse
      |
      v
Client
```

---

## 🔐 Bảo mật & Kiểm thử (QA)

### 1. Xử lý lỗi và ngoại lệ

Hệ thống cần kiểm soát các lỗi phát sinh trong quá trình giao tiếp mạng và xử lý dữ liệu.

Các tình huống cần xử lý:

- Client không thể kết nối đến Server.
- Server ngừng hoạt động.
- Client ngắt kết nối đột ngột.
- JSON không hợp lệ.
- Request thiếu thông tin bắt buộc.
- Action không được hỗ trợ.
- Lỗi truy xuất cơ sở dữ liệu.
- Lỗi trong quá trình gửi hoặc nhận dữ liệu.

### 2. Logging – Ghi nhật ký

Hệ thống sử dụng cơ chế Logging để theo dõi hoạt động của Server.

Thông tin cần theo dõi:

- Thời điểm Client kết nối.
- Địa chỉ IP và Port.
- Request được gửi đến Server.
- Response được trả về Client.
- Trạng thái Session.
- Các ngoại lệ và lỗi JSON.

### 3. Kiểm thử hệ thống

| STT | Tình huống kiểm thử | Kết quả mong đợi |
|---|---|---|
| 1 | Client kết nối đúng IP và Port | Kết nối thành công |
| 2 | Client nhập sai IP hoặc Port | Hiển thị lỗi kết nối |
| 3 | Đăng nhập đúng thông tin | Xác thực thành công |
| 4 | Đăng nhập sai thông tin | Trả về thông báo lỗi |
| 5 | Đăng ký tài khoản mới | Xử lý đăng ký |
| 6 | Gửi Request không hợp lệ | Server xử lý lỗi an toàn |
| 7 | Client ngắt kết nối đột ngột | Dọn dẹp Session |
| 8 | Client gửi lệnh đăng xuất | Kết thúc phiên |
| 9 | Nhiều Client kết nối | Server tiếp nhận đồng thời |
| 10 | Gửi nhiều thông điệp liên tiếp | Phân tách đúng từng Request |

### 4. Các nguyên tắc bảo mật

- Kiểm tra dữ liệu đầu vào tại Client và Server.
- Không lưu mật khẩu dưới dạng văn bản thuần (Plaintext).
- Ưu tiên sử dụng Password Hashing có Salt.
- Sử dụng truy vấn SQL có tham số để hạn chế SQL Injection.
- Kiểm soát dữ liệu Request trước khi thực hiện nghiệp vụ.
- Không để lộ thông tin nhạy cảm trong Log.
- Cân nhắc sử dụng TLS khi triển khai qua mạng không tin cậy.

---

## 🛠️ Công nghệ sử dụng

| Công nghệ | Vai trò |
|---|---|
| C# | Ngôn ngữ lập trình |
| .NET | Nền tảng phát triển |
| Windows Forms | Xây dựng giao diện Desktop |
| TCP Socket | Giao tiếp Client-Server |
| TcpListener | Tiếp nhận kết nối |
| Async/Await | Xử lý bất đồng bộ |
| ConcurrentDictionary | Quản lý dữ liệu kết nối đồng thời |
| JSON | Định dạng dữ liệu |
| UTF-8 | Mã hóa dữ liệu truyền tải |
| SQL Server | Hệ quản trị cơ sở dữ liệu |
| Visual Studio | Môi trường phát triển |

---

## 🚀 Hướng dẫn cài đặt

### 1. Yêu cầu hệ thống

- Windows 10 / Windows 11.
- Visual Studio hỗ trợ phiên bản .NET của dự án.
- .NET SDK tương ứng.
- Microsoft SQL Server.
- SQL Server Management Studio (SSMS).

### 2. Clone Repository

```bash
git clone <repository-url>
```

### 3. Mở dự án

Mở file Solution `.sln` bằng Visual Studio.

Kiểm tra các Project:

```text
GameShared
GameServer
UserManagement
```

Đảm bảo các Project Reference được thiết lập chính xác.

### 4. Cấu hình Database

- Khởi động SQL Server.
- Tạo hoặc khôi phục Database theo cấu trúc của dự án.
- Cấu hình Connection String trong phần DataAccess.
- Kiểm tra quyền truy cập cơ sở dữ liệu.

### 5. Khởi chạy Server

Chạy Project `GameServer`.

Kiểm tra:

- Server khởi động thành công.
- TCP Listener hoạt động.
- IP và Port được thiết lập chính xác.
- Dashboard hiển thị trạng thái đang hoạt động.

### 6. Khởi chạy Client

Chạy Project `UserManagement`.

Tại giao diện Login:

- Nhập địa chỉ IP của Server.
- Nhập Port tương ứng.
- Tiến hành kết nối.
- Thử nghiệm chức năng đăng nhập và đăng ký.

### 7. Kiểm tra hệ thống

Thực hiện các thao tác:

- Đăng ký tài khoản.
- Đăng nhập.
- Truy xuất thông tin Profile.
- Đăng xuất.
- Kết nối nhiều Client.
- Kiểm tra Dashboard Server.

---

## 📌 Tổng kết

**Dân Tổ Phóng** không chỉ là một ứng dụng quản lý người dùng đơn thuần mà còn là dự án thực hành các kiến thức quan trọng trong lập trình mạng.

Thông qua dự án, các nội dung được vận dụng bao gồm:

- Mô hình Client-Server.
- TCP Socket Programming.
- TCP Stream và Message Framing.
- Xử lý bất đồng bộ Async/Await.
- Quản lý nhiều kết nối đồng thời.
- JSON Serialization và Deserialization.
- Quản lý Session.
- Truy xuất cơ sở dữ liệu SQL Server.
- Thiết kế giao diện WinForms.

Dự án là nền tảng để tiếp tục mở rộng thành hệ thống có khả năng quản lý nhiều người dùng và tích hợp các chức năng mạng phức tạp hơn.

---

## 🔮 Hướng phát triển

- [ ] Bổ sung mã hóa đường truyền bằng TLS.
- [ ] Hoàn thiện cơ chế xác thực và phân quyền người dùng.
- [ ] Bổ sung chức năng cập nhật thông tin cá nhân.
- [ ] Nâng cấp hệ thống Logging và lưu trữ lịch sử hoạt động.
- [ ] Tối ưu khả năng quản lý số lượng lớn Client.
- [ ] Bổ sung cơ chế tự động kết nối lại khi mất kết nối.
- [ ] Mở rộng hệ thống thành nền tảng trò chơi trực tuyến nhiều người chơi.

---

## 👨‍💻 Tác giả

<div align="center">

**🛵 DÂN TỔ PHÓNG**

*TCP Client-Server Application*

**Course:** Data Communication / Computer Networks

**Language:** C#

**Framework:** .NET WinForms

**Database:** Microsoft SQL Server

*Built with C# and TCP Socket Programming.*

</div>

---

<div align="center">

**⭐ Nếu bạn thấy dự án hữu ích, hãy để lại một Star cho Repository!**

</div>
