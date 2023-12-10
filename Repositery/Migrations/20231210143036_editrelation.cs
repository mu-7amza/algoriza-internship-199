using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorDays_DoctorDayId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorTime_DoctorDays_DoctorDayId",
                table: "DoctorTime");

            migrationBuilder.DropIndex(
                name: "IX_DoctorTime_DoctorDayId",
                table: "DoctorTime");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DoctorDayId",
                table: "Appointments");


            migrationBuilder.DropColumn(
                name: "DoctorDayId",
                table: "DoctorTime");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "DoctorTime");

            migrationBuilder.DropColumn(
                name: "Day",
                table: "DoctorDays");

            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "DoctorTime",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeValue",
                table: "DoctorTime",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "DoctorDays",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Date",
                table: "DoctorDays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoctorDayId",
                table: "DoctorDays",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorDayId",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");



            migrationBuilder.CreateIndex(
                name: "IX_DoctorTime_DayId",
                table: "DoctorTime",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDays_DoctorDayId",
                table: "DoctorDays",
                column: "DoctorDayId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDays_DoctorId",
                table: "DoctorDays",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDays_Appointments_DoctorDayId",
                table: "DoctorDays",
                column: "DoctorDayId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDays_AspNetUsers_DoctorId",
                table: "DoctorDays",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorTime_DoctorDays_DayId",
                table: "DoctorTime",
                column: "DayId",
                principalTable: "DoctorDays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_AspNetUsers_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDays_Appointments_DoctorDayId",
                table: "DoctorDays");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDays_AspNetUsers_DoctorId",
                table: "DoctorDays");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorTime_DoctorDays_DayId",
                table: "DoctorTime");

            migrationBuilder.DropIndex(
                name: "IX_DoctorTime_DayId",
                table: "DoctorTime");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDays_DoctorDayId",
                table: "DoctorDays");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDays_DoctorId",
                table: "DoctorDays");



            migrationBuilder.DropColumn(
                name: "DayId",
                table: "DoctorTime");

            migrationBuilder.DropColumn(
                name: "TimeValue",
                table: "DoctorTime");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "DoctorDays");

            migrationBuilder.DropColumn(
                name: "DoctorDayId",
                table: "DoctorDays");

            migrationBuilder.AddColumn<int>(
                name: "DoctorDayId",
                table: "DoctorTime",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "DoctorTime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "DoctorDays",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "DoctorDays",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Appointments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorDayId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_DoctorTime_DoctorDayId",
                table: "DoctorTime",
                column: "DoctorDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorDayId",
                table: "Appointments",
                column: "DoctorDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_AspNetUsers_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorDays_DoctorDayId",
                table: "Appointments",
                column: "DoctorDayId",
                principalTable: "DoctorDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorTime_DoctorDays_DoctorDayId",
                table: "DoctorTime",
                column: "DoctorDayId",
                principalTable: "DoctorDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
