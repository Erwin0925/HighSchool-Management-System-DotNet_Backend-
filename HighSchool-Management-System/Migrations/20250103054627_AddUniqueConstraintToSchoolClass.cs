using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HighSchool_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToSchoolClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SchoolClasses_ClassName",
                table: "SchoolClasses",
                column: "ClassName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SchoolClasses_ClassName",
                table: "SchoolClasses");
        }
    }
}
