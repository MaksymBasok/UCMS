using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UCMS.Infrastructure.Persistence.Migrations
{
    public partial class AddAssignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_assignments_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_assignments_course_id",
                table: "assignments",
                column: "course_id");

            // опціонально: якщо індекс відсутній
            migrationBuilder.CreateIndex(
                name: "ix_submissions_student_id",
                table: "submissions",
                column: "student_id");

            // 1) очистити «сироти» в submissions перед додаванням FK
            migrationBuilder.Sql(@"
                DELETE FROM submissions s
                WHERE s.assignment_id IS NULL
                   OR NOT EXISTS (SELECT 1 FROM assignments a WHERE a.id = s.assignment_id);
            ");

            // 2) додати зовнішні ключі
            migrationBuilder.AddForeignKey(
                name: "fk_course_schedules_courses_course_id",
                table: "course_schedules",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_submissions_assignments_assignment_id",
                table: "submissions",
                column: "assignment_id",
                principalTable: "assignments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_submissions_students_student_id",
                table: "submissions",
                column: "student_id",
                principalTable: "students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_course_schedules_courses_course_id",
                table: "course_schedules");

            migrationBuilder.DropForeignKey(
                name: "fk_submissions_assignments_assignment_id",
                table: "submissions");

            migrationBuilder.DropForeignKey(
                name: "fk_submissions_students_student_id",
                table: "submissions");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropIndex(
                name: "ix_submissions_student_id",
                table: "submissions");
        }
    }
}
