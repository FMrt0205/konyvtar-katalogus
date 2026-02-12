using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konyvtar_katalogus.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    bookid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "TEXT", nullable: false),
                    author = table.Column<string>(type: "TEXT", nullable: false),
                    isbn = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.bookid);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    readerid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.readerid);
                });

            migrationBuilder.CreateTable(
                name: "Copies",
                columns: table => new
                {
                    copyid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InventoryNumber = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    isAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    BookId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copies", x => x.copyid);
                    table.ForeignKey(
                        name: "FK_Copies_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "bookid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    loanid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    loanDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    returnDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    copyid = table.Column<int>(type: "INTEGER", nullable: false),
                    ReaderId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.loanid);
                    table.ForeignKey(
                        name: "FK_Loans_Copies_copyid",
                        column: x => x.copyid,
                        principalTable: "Copies",
                        principalColumn: "copyid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "readerid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fines",
                columns: table => new
                {
                    fineid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    isPaid = table.Column<bool>(type: "INTEGER", nullable: false),
                    LoanId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fines", x => x.fineid);
                    table.ForeignKey(
                        name: "FK_Fines_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "loanid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Copies_BookId",
                table: "Copies",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Fines_LoanId",
                table: "Fines",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_copyid",
                table: "Loans",
                column: "copyid");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_ReaderId",
                table: "Loans",
                column: "ReaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fines");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Copies");

            migrationBuilder.DropTable(
                name: "Readers");

            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
