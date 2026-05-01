@echo off
echo ==================================================
echo      KHOI DONG TOAN BO PROJECT GK_CNNET
echo ==================================================

echo.
echo [1] Khoi dong Backend (.NET API)...
start "Backend API (GK_CNNET)" cmd /k "cd GK_CNNET && dotnet run"

echo.
echo [2] Khoi dong Frontend Web (React + Vite)...
start "Frontend Web (GK_CNNET_FE)" cmd /k "cd GK_CNNET_FE && npm run dev"

echo.
echo [3] Khoi dong Game Server (Node.js)...
start "Game Server (Ball_Square_TT)" cmd /k "cd Ball_Square_TT && node server.js"

echo.
echo Tat ca cac ung dung dang duoc khoi dong o cac cua so rieng biet!
echo De dong cac ung dung, chi can dong cac cua so cmd vua hien len.
echo.
pause
