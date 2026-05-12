using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQRCoffee.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO ""Roles"" (""RoleId"", ""RoleName"")
VALUES
    (1, 'Admin'),
    (2, 'Staff'),
    (3, 'Cashier')
ON CONFLICT (""RoleId"") DO NOTHING;


INSERT INTO ""Users"" (""UserId"", ""Username"", ""PasswordHash"", ""RoleId"", ""IsActive"", ""RefreshToken"", ""RefreshTokenExpiryTime"")
VALUES
    (1, 'admin_demo', '$2a$11$CSQbS.Y7xB90Apxex/QQoelo2RoJWgLI2YloQOYz9ZlKoDSRjEEne', 1, true, NULL, NULL),
    (2, 'staff_demo', '$2a$11$CSQbS.Y7xB90Apxex/QQoelo2RoJWgLI2YloQOYz9ZlKoDSRjEEne', 2, true, NULL, NULL),
    (3, 'cashier_demo', '$2a$11$CSQbS.Y7xB90Apxex/QQoelo2RoJWgLI2YloQOYz9ZlKoDSRjEEne', 3, true, NULL, NULL)
ON CONFLICT (""UserId"") DO NOTHING;


INSERT INTO ""Tables"" (""TableId"", ""TableName"", ""QRCode"", ""IsActive"", ""IsOccupied"", ""SessionToken"")
VALUES
    (1, 'Bàn 01', 'QR_TABLE_01', true, false, 'table-01-demo-token'),
    (2, 'Bàn 02', 'QR_TABLE_02', true, false, 'table-02-demo-token')
ON CONFLICT (""TableId"") DO NOTHING;


INSERT INTO ""Categories"" (""CategoryId"", ""Name"", ""IconUrl"")
VALUES
    (1, 'Cà phê', 'coffee'),
    (2, 'Trà sữa', 'milk-tea')
ON CONFLICT (""CategoryId"") DO NOTHING;


INSERT INTO ""Products"" (""ProductId"", ""CategoryId"", ""Name"", ""Description"", ""Price"", ""ImageUrl"", ""IsFeatured"", ""IsNew"", ""Stock_Quantity"", ""IsDisabled"")
VALUES
    (1, 1, 'Cà phê sữa đá', 'Cà phê đậm vị pha cùng sữa đặc.', 29000, 'ca-phe-sua-da.jpg', true, true, 100, false),
    (2, 1, 'Bạc xỉu', 'Vị sữa béo nhẹ, phù hợp người mới uống cà phê.', 32000, 'bac-xiu.jpg', true, false, 100, false),
    (3, 2, 'Trà sữa truyền thống', 'Trà sữa thơm béo, topping linh hoạt.', 35000, 'tra-sua-truyen-thong.jpg', false, true, 100, false)
ON CONFLICT (""ProductId"") DO NOTHING;


INSERT INTO ""ProductOptions"" (""ProductOptionId"", ""ProductId"", ""Name"", ""PriceAdjustment"")
VALUES
    (1, 1, 'Ít đá', 0),
    (2, 1, 'Thêm sữa', 5000),
    (3, 3, 'Trân châu đen', 7000),
    (4, 3, 'Kem cheese', 10000)
ON CONFLICT (""ProductOptionId"") DO NOTHING;

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM ""ProductOptions"" WHERE ""ProductOptionId"" IN (1, 2, 3, 4);
DELETE FROM ""Products"" WHERE ""ProductId"" IN (1, 2, 3);
DELETE FROM ""Categories"" WHERE ""CategoryId"" IN (1, 2);
DELETE FROM ""Tables"" WHERE ""TableId"" IN (1, 2);
DELETE FROM ""Users"" WHERE ""UserId"" IN (1, 2, 3);
DELETE FROM ""Roles"" WHERE ""RoleId"" IN (1, 2, 3);
");
        }
    }
}
