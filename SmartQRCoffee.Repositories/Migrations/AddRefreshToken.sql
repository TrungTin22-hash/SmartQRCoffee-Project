-- Migration: Thêm cột RefreshToken và RefreshTokenExpiryTime vào bảng Users
-- Chạy trên Supabase PostgreSQL (SQL Editor)

ALTER TABLE "Users"
ADD COLUMN "RefreshToken" TEXT NULL;

ALTER TABLE "Users"
ADD COLUMN "RefreshTokenExpiryTime" TIMESTAMP NULL;
