using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientPortal.Api.Migrations
{
    public partial class AddClientsAndClientToOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fixed ID for the demo client.
            // Existing orders will be assigned to this client.
            var demoClientId = new Guid(
                "7A4F2C3D-1B5E-4A9F-8C21-6D7E3F5A9B10"
            );

            // Create Clients table
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: false,
                        defaultValueSql: "(newid())"
                    ),

                    FirstName = table.Column<string>(
                        type: "nvarchar(100)",
                        maxLength: 100,
                        nullable: false
                    ),

                    LastName = table.Column<string>(
                        type: "nvarchar(100)",
                        maxLength: 100,
                        nullable: false
                    ),

                    Email = table.Column<string>(
                        type: "nvarchar(255)",
                        maxLength: 255,
                        nullable: false
                    ),

                    PasswordHash = table.Column<string>(
                        type: "nvarchar(500)",
                        maxLength: 500,
                        nullable: false
                    ),

                    PhoneNumber = table.Column<string>(
                        type: "nvarchar(30)",
                        maxLength: 30,
                        nullable: false
                    ),

                    CompanyName = table.Column<string>(
                        type: "nvarchar(200)",
                        maxLength: 200,
                        nullable: false
                    ),

                    IsActive = table.Column<bool>(
                        type: "bit",
                        nullable: false,
                        defaultValue: true
                    ),

                    CreatedAt = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false,
                        defaultValueSql: "(sysutcdatetime())"
                    )
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                }
            );

            // Unique email
            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true
            );

            // Insert demo client.
            //
            // The password is temporarily set to TEMP.
            // DbSeeder will replace this with a real ASP.NET
            // PasswordHasher hash when the API starts.
            migrationBuilder.Sql($@"
                INSERT INTO [Clients]
                (
                    [Id],
                    [FirstName],
                    [LastName],
                    [Email],
                    [PasswordHash],
                    [PhoneNumber],
                    [CompanyName],
                    [IsActive],
                    [CreatedAt]
                )
                VALUES
                (
                    '{demoClientId}',
                    'John',
                    'Doe',
                    'john.doe@moyo.co.za',
                    'TEMP',
                    '0821234567',
                    'Demo Business',
                    1,
                    SYSUTCDATETIME()
                );
            ");

            // Add ClientId to the existing Orders table.
            //
            // The default value means every existing order
            // is automatically assigned to the demo client.
            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: demoClientId
            );

            // Index for ClientId
            migrationBuilder.CreateIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                column: "ClientId"
            );

            // Connect Orders to Clients
            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients",
                table: "Orders"
            );

            migrationBuilder.DropIndex(
                name: "IX_Orders_ClientId",
                table: "Orders"
            );

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Orders"
            );

            migrationBuilder.DropTable(
                name: "Clients"
            );
        }
    }
}