"# StockEcomAPI" 
สร้างการตั้งค่าฐานข้อมูลและอัพเดตฐานข้อมูลโดยใช้คำสั่งดังนี้:

ติดตั้ง dotnet tool install --global dotnet-ef เพื่อใช้ฟังก์ชั่น ef

dotnet ef migrations add InitialCreate
dotnet ef database update