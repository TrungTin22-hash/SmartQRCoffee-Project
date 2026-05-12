using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQRCoffee.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO ""Users"" (""UserId"", ""Username"", ""PasswordHash"", ""RoleId"", ""IsActive"", ""RefreshToken"", ""RefreshTokenExpiryTime"")
VALUES
    (1, 'admin_demo', '$2a$11$CSQbS.Y7xB90Apxex/QQoelo2RoJWgLI2YloQOYz9ZlKoDSRjEEne', 1, true, NULL, NULL),
    (2, 'staff_demo', '$2a$11$CSQbS.Y7xB90Apxex/QQoelo2RoJWgLI2YloQOYz9ZlKoDSRjEEne', 2, true, NULL, NULL),
    (3, 'cashier_demo', '$2a$11$CSQbS.Y7xB90Apxex/QQoelo2RoJWgLI2YloQOYz9ZlKoDSRjEEne', 3, true, NULL, NULL)
ON CONFLICT (""UserId"") DO NOTHING;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM ""Users"" WHERE ""UserId"" IN (1, 2, 3);
");
        }
    }
}
