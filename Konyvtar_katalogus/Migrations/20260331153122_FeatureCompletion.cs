using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konyvtar_katalogus.Migrations
{
    /// <inheritdoc />
    public partial class FeatureCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fines_LoanId",
                table: "Fines");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Readers",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoanPeriodDays",
                table: "Loans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "paidAt",
                table: "Fines",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationQueueItems",
                columns: table => new
                {
                    notificationQueueItemId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoanId = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationQueueItems", x => x.notificationQueueItemId);
                    table.ForeignKey(
                        name: "FK_NotificationQueueItems_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "loanid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fines_LoanId",
                table: "Fines",
                column: "LoanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Copies_InventoryNumber",
                table: "Copies",
                column: "InventoryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_isbn",
                table: "Books",
                column: "isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationQueueItems_LoanId",
                table: "NotificationQueueItems",
                column: "LoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationQueueItems");

            migrationBuilder.DropIndex(
                name: "IX_Fines_LoanId",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_Copies_InventoryNumber",
                table: "Copies");

            migrationBuilder.DropIndex(
                name: "IX_Books_isbn",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Readers");

            migrationBuilder.DropColumn(
                name: "LoanPeriodDays",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "paidAt",
                table: "Fines");

            migrationBuilder.CreateIndex(
                name: "IX_Fines_LoanId",
                table: "Fines",
                column: "LoanId");
        }
    }
}
