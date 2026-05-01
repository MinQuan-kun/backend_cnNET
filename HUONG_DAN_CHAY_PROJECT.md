# Hướng Dẫn Chạy Toàn Bộ Project (Backend, Frontend, Game)

Project này bao gồm 3 phần độc lập:
1. **Backend API**: Ứng dụng .NET (nằm trong thư mục `GK_CNNET`)
2. **Frontend Web**: Ứng dụng React + Vite (nằm trong thư mục `GK_CNNET_FE`)
3. **Game Server**: Ứng dụng Node.js (nằm trong thư mục `Ball_Square_TT`)

Dưới đây là 2 cách để khởi chạy toàn bộ project:

---

## CÁCH 1: CHẠY NHANH BẰNG FILE START.BAT (Khuyên dùng)

Có thể chạy toàn bộ 3 ứng dụng cùng một lúc chỉ với 1 cú click chuột:

1. Đảm bảo đã cài đặt đủ các thư viện (`npm install` ở thư mục FE và Game nếu đây là lần chạy đầu tiên - xem CÁCH 2).
2. Tìm file **`start_all.bat`** ở thư mục gốc (cùng thư mục với file hướng dẫn này).
3. Click đúp chuột (Double click) vào file **`start_all.bat`**.
4. Sẽ có 3 cửa sổ CMD màu đen hiện lên tương ứng với 3 ứng dụng: Backend, Frontend và Game Server.
5. Để tắt các ứng dụng, chỉ cần bấm dấu X để đóng các cửa sổ CMD đó lại.

---

## CÁCH 2: CHẠY THỦ CÔNG TỪNG ỨNG DỤNG

Nếu muốn chạy thủ công hoặc đây là lần đầu tiên clone code về (cần cài đặt thư viện), hãy làm theo các bước sau:

### 1. Khởi động Backend API (.NET)
Mở terminal (hoặc CMD/PowerShell) và trỏ vào thư mục gốc của project, sau đó chạy:
```bash
cd GK_CNNET
dotnet build
dotnet run
```
*Backend sẽ chạy tại: `http://localhost:5207` (hoặc port được config trong properties).*

### 2. Khởi động Frontend Web (React + Vite)
Mở một terminal mới, sau đó chạy:
```bash
cd GK_CNNET_FE
npm install   # (Chỉ cần chạy lệnh này ở lần đầu tiên để tải thư viện)
npm run dev
```
*Frontend sẽ chạy tại: `http://localhost:5173`.*

### 3. Khởi động Game Server (Node.js)
Mở thêm một terminal mới, sau đó chạy:
```bash
cd Ball_Square_TT
npm install   # (Chỉ cần chạy lệnh này ở lần đầu tiên để tải thư viện)
node server.js
```
*Game GraphQL Server sẽ chạy tại: `http://localhost:4000/graphql`.*

---

## LƯU Ý KHI CHẠY LẦN ĐẦU TIÊN
- Hãy đảm bảo máy tính đã cài đặt [Node.js](https://nodejs.org/) để chạy được Frontend và Game Server.
- Cần cài đặt [.NET SDK](https://dotnet.microsoft.com/download) (phiên bản tương ứng với project) để chạy được Backend.
- Nếu gặp lỗi thiếu thư viện khi chạy `start_all.bat`, hãy làm theo **CÁCH 2** một lần trước để chạy lệnh `npm install` tại các thư mục `GK_CNNET_FE` và `Ball_Square_TT`.
