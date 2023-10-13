using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElmiraFireRecall.Migrations
{
    /// <inheritdoc />
    public partial class RecipientGroupManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Recipients_FireRecipientId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_FireRecipientId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "FireRecipientId",
                table: "Groups");

            migrationBuilder.CreateTable(
                name: "FireGroupFireRecipient",
                columns: table => new
                {
                    GroupsId = table.Column<int>(type: "int", nullable: false),
                    RecipientsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireGroupFireRecipient", x => new { x.GroupsId, x.RecipientsId });
                    table.ForeignKey(
                        name: "FK_FireGroupFireRecipient_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FireGroupFireRecipient_Recipients_RecipientsId",
                        column: x => x.RecipientsId,
                        principalTable: "Recipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FireGroupFireRecipient_RecipientsId",
                table: "FireGroupFireRecipient",
                column: "RecipientsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FireGroupFireRecipient");

            migrationBuilder.AddColumn<int>(
                name: "FireRecipientId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_FireRecipientId",
                table: "Groups",
                column: "FireRecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Recipients_FireRecipientId",
                table: "Groups",
                column: "FireRecipientId",
                principalTable: "Recipients",
                principalColumn: "Id");
        }
    }
}
