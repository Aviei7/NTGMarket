using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "UserOrderLogID");

            migrationBuilder.Sql(
                """
                ALTER TABLE [UserLogs] DROP CONSTRAINT [PK_UserLogs];
                EXEC sp_rename 'UserLogs', 'UserLogs_Old';

                CREATE TABLE [UserLogs] (
                    [UserOrderLogID] int NOT NULL IDENTITY,
                    [UserID] int NOT NULL,
                    [FirstName] nvarchar(max) NOT NULL,
                    [Lastname] nvarchar(max) NOT NULL,
                    [Email] nvarchar(max) NOT NULL,
                    [PhoneNumber] nvarchar(max) NOT NULL,
                    CONSTRAINT [PK_UserLogs] PRIMARY KEY ([UserOrderLogID])
                );

                SET IDENTITY_INSERT [UserLogs] ON;

                INSERT INTO [UserLogs] ([UserOrderLogID], [UserID], [FirstName], [Lastname], [Email], [PhoneNumber])
                SELECT [UserID], [UserID], [FirstName], [Lastname], [Email], [PhoneNumber]
                FROM [UserLogs_Old];

                SET IDENTITY_INSERT [UserLogs] OFF;

                DROP TABLE [UserLogs_Old];
                """);

            migrationBuilder.CreateTable(
                name: "DeliveryLogs",
                columns: table => new
                {
                    DeliveryLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryLogs", x => x.DeliveryLogID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryLogs");

            migrationBuilder.RenameColumn(
                name: "UserOrderLogID",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.Sql(
                """
                ALTER TABLE [UserLogs] DROP CONSTRAINT [PK_UserLogs];
                EXEC sp_rename 'UserLogs', 'UserLogs_New';

                CREATE TABLE [UserLogs] (
                    [UserID] int NOT NULL IDENTITY,
                    [FirstName] nvarchar(max) NOT NULL,
                    [Lastname] nvarchar(max) NOT NULL,
                    [Email] nvarchar(max) NOT NULL,
                    [PhoneNumber] nvarchar(max) NOT NULL,
                    [LogDate] datetime2 NOT NULL CONSTRAINT [DF_UserLogs_LogDate] DEFAULT (GETUTCDATE()),
                    CONSTRAINT [PK_UserLogs] PRIMARY KEY ([UserID])
                );

                SET IDENTITY_INSERT [UserLogs] ON;

                INSERT INTO [UserLogs] ([UserID], [FirstName], [Lastname], [Email], [PhoneNumber])
                SELECT [UserOrderLogID], [FirstName], [Lastname], [Email], [PhoneNumber]
                FROM [UserLogs_New];

                SET IDENTITY_INSERT [UserLogs] OFF;

                DROP TABLE [UserLogs_New];
                """);
        }
    }
}
