@echo off
echo        Leave Notifier Application Builder
echo =================================================
echo.

rem Download dependencies
echo 1. Downloading dependencies
cd src
dotnet restore
echo.

rem Create/Update database
echo.
echo 2. Create/Update the database
cd LeaveNotifierApplication.Data
dotnet ef database update
echo.

rem Build the API
echo.
echo 3. Build the API
cd ../LeaveNotifierApplication.Api
dotnet build
echo.

rem Run the tests for the API
echo.
echo 4. Run the tests for the API
cd ../LeaveNotifierApplication.Api.Tests
dotnet test
echo.

rem Run the API
echo.
echo 5. Run the API
cd ../LeaveNotifierApplication.Api
rem Put this call on the background
dotnet run