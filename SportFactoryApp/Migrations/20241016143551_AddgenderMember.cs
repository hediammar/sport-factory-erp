using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportFactoryApp.Migrations
{
    /// <inheritdoc />
    public partial class AddgenderMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionNumber",
                table: "Sessions",
                newName: "MembershipId");

            migrationBuilder.CreateTable(
                name: "Membership",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => x.MembershipId);
                    table.ForeignKey(
                        name: "FK_Membership_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_MembershipId",
                table: "Sessions",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Membership_MemberId",
                table: "Membership",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Membership_MembershipId",
                table: "Sessions",
                column: "MembershipId",
                principalTable: "Membership",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Membership_MembershipId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "Membership");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_MembershipId",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "MembershipId",
                table: "Sessions",
                newName: "SessionNumber");
        }
    }
}
