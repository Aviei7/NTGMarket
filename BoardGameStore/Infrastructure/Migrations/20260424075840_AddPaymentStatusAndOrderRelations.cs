using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatusAndOrderRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "s_PaymentStatus",
                columns: table => new
                {
                    PaymentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_PaymentStatus", x => x.PaymentStatusId);
                });

            migrationBuilder.InsertData(
                table: "s_PaymentStatus",
                columns: new[] { "PaymentStatusId", "Name", "Description", "Status" },
                values: new object[,]
                {
                    { 1, "Очікує оплату", "Платіж створено, але ще не підтверджено.", 1 },
                    { 2, "Оплачено", "Платіж успішно проведено та підтверджено.", 2 },
                    { 3, "Помилка оплати", "Платіж не був проведений через помилку.", 3 },
                    { 4, "Повернено", "Кошти були повернуті покупцю.", 4 },
                    { 5, "Скасовано", "Платіж або замовлення було скасовано.", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryLogID",
                table: "Orders",
                column: "DeliveryLogID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentStatusId",
                table: "Orders",
                column: "PaymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentTypeId",
                table: "Orders",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserOrderLogID",
                table: "Orders",
                column: "UserOrderLogID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_s_PaymentStatus_Status",
                table: "s_PaymentStatus",
                column: "Status",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryLogs_DeliveryLogID",
                table: "Orders",
                column: "DeliveryLogID",
                principalTable: "DeliveryLogs",
                principalColumn: "DeliveryLogID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserLogs_UserOrderLogID",
                table: "Orders",
                column: "UserOrderLogID",
                principalTable: "UserLogs",
                principalColumn: "UserOrderLogID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_s_PaymentStatus_PaymentStatusId",
                table: "Orders",
                column: "PaymentStatusId",
                principalTable: "s_PaymentStatus",
                principalColumn: "PaymentStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_s_PaymentTypes_PaymentTypeId",
                table: "Orders",
                column: "PaymentTypeId",
                principalTable: "s_PaymentTypes",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryLogs_DeliveryLogID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserLogs_UserOrderLogID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_s_PaymentStatus_PaymentStatusId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_s_PaymentTypes_PaymentTypeId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "s_PaymentStatus");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryLogID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentStatusId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentTypeId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserOrderLogID",
                table: "Orders");
        }
    }
}
