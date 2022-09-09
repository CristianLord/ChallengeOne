using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeOne.Migrations
{
    public partial class EditDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Journals",
                newName: "UploadDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "Journals",
                newName: "UpdateDate");
        }
    }
}
