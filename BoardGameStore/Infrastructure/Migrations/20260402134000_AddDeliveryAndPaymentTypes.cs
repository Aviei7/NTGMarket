using Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(BoardStoreContext))]
    [Migration("20260402134000_AddDeliveryAndPaymentTypes")]
    public partial class AddDeliveryAndPaymentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "s_DeliveryTypes",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_DeliveryTypes", x => x.DeliveryId);
                });

            migrationBuilder.CreateTable(
                name: "s_PaymentTypes",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_PaymentTypes", x => x.PaymentId);
                });

            migrationBuilder.InsertData(
                table: "s_DeliveryTypes",
                columns: new[] { "DeliveryId", "DeliveryName", "Description" },
                values: new object[,]
                {
                    { 1, "\u041d\u043e\u0432\u0430 \u043f\u043e\u0448\u0442\u0430", "\u0414\u043e\u0441\u0442\u0430\u0432\u043a\u0430 \u0443 \u0432\u0456\u0434\u0434\u0456\u043b\u0435\u043d\u043d\u044f \u0430\u0431\u043e \u043f\u043e\u0448\u0442\u043e\u043c\u0430\u0442." },
                    { 2, "\u041a\u0443\u0440'\u0454\u0440", "\u0410\u0434\u0440\u0435\u0441\u043d\u0430 \u0434\u043e\u0441\u0442\u0430\u0432\u043a\u0430 \u043a\u0443\u0440'\u0454\u0440\u043e\u043c \u043f\u043e \u043c\u0456\u0441\u0442\u0443." },
                    { 3, "\u0421\u0430\u043c\u043e\u0432\u0438\u0432\u0456\u0437", "\u0421\u0430\u043c\u043e\u0441\u0442\u0456\u0439\u043d\u0435 \u043e\u0442\u0440\u0438\u043c\u0430\u043d\u043d\u044f \u0437\u0430\u043c\u043e\u0432\u043b\u0435\u043d\u043d\u044f \u0437 \u043c\u0430\u0433\u0430\u0437\u0438\u043d\u0443." }
                });

            migrationBuilder.InsertData(
                table: "s_PaymentTypes",
                columns: new[] { "PaymentId", "PaymentName", "Description" },
                values: new object[,]
                {
                    { 1, "\u041e\u043f\u043b\u0430\u0442\u0430 \u043a\u0430\u0440\u0442\u043a\u043e\u044e", "\u041e\u043f\u043b\u0430\u0442\u0430 \u0431\u0430\u043d\u043a\u0456\u0432\u0441\u044c\u043a\u043e\u044e \u043a\u0430\u0440\u0442\u043a\u043e\u044e \u043f\u0456\u0434 \u0447\u0430\u0441 \u043e\u0444\u043e\u0440\u043c\u043b\u0435\u043d\u043d\u044f." },
                    { 2, "\u0413\u043e\u0442\u0456\u0432\u043a\u043e\u044e \u043f\u0440\u0438 \u043e\u0442\u0440\u0438\u043c\u0430\u043d\u043d\u0456", "\u041e\u043f\u043b\u0430\u0442\u0430 \u0433\u043e\u0442\u0456\u0432\u043a\u043e\u044e \u043f\u0456\u0434 \u0447\u0430\u0441 \u043e\u0442\u0440\u0438\u043c\u0430\u043d\u043d\u044f \u0437\u0430\u043c\u043e\u0432\u043b\u0435\u043d\u043d\u044f." },
                    { 3, "\u041e\u043d\u043b\u0430\u0439\u043d-\u043e\u043f\u043b\u0430\u0442\u0430", "\u041c\u0438\u0442\u0442\u0454\u0432\u0430 \u043e\u043f\u043b\u0430\u0442\u0430 \u0447\u0435\u0440\u0435\u0437 \u043f\u043b\u0430\u0442\u0456\u0436\u043d\u0438\u0439 \u0441\u0435\u0440\u0432\u0456\u0441." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "s_DeliveryTypes");

            migrationBuilder.DropTable(
                name: "s_PaymentTypes");
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Models.StaticModel.s_DeliveryTypesModel", b =>
                {
                    b.Property<int>("DeliveryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeliveryId"));

                    b.Property<string>("DeliveryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("DeliveryId");

                    b.ToTable("s_DeliveryTypes");
                });

            modelBuilder.Entity("Domain.Models.StaticModel.s_PaymentTypesModel", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PaymentName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PaymentId");

                    b.ToTable("s_PaymentTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
