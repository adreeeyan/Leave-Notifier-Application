#!/bin/bash

echo "       Leave Notifier Application Builder"
echo "================================================="
echo ""

# Download dependencies
echo "1. Downloading dependencies"
cd src
dotnet restore
echo ""

# Create/Update database
echo ""
echo "2. Create/Update the database"
cd LeaveNotifierApplication.Data
dotnet ef database update
echo ""

# Build the API
echo ""
echo "3. Build the API"
cd ../LeaveNotifierApplication.Api
dotnet build
echo ""

# Run the tests for the API
echo ""
echo "4. Run the tests for the API"
cd ../LeaveNotifierApplication.Api.Tests
dotnet test
echo ""

# Run the API
echo ""
echo "5. Run the API"
cd ../LeaveNotifierApplication.Api
# Put this call on the background
dotnet run