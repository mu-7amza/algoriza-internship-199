using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditRelationV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDays_Appointments_DoctorDayId",
                table: "DoctorDays");

            migrationBuilder.DropIndex(
                name: "IX_DoctorDays_DoctorDayId",
                table: "DoctorDays");


            migrationBuilder.DropColumn(
                name: "DoctorDayId",
                table: "DoctorDays");

            migrationBuilder.RenameColumn(
                name: "DoctorDayId",
                table: "Appointments",
                newName: "TimeId");

            migrationBuilder.AddColumn<int>(
                name: "CouponId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayId",
                table: "Appointments",
                type: "int",
                nullable: true);

            

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CouponId",
                table: "Bookings",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DayId",
                table: "Appointments",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TimeId",
                table: "Appointments",
                column: "TimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorDays_DayId",
                table: "Appointments",
                column: "DayId",
                principalTable: "DoctorDays",
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_DoctorTime_TimeId",
                table: "Appointments",
                column: "TimeId",
                principalTable: "DoctorTime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Coupons_CouponId",
                table: "Bookings",
                column: "CouponId",
                principalTable: "Coupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorDays_DayId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_DoctorTime_TimeId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Coupons_CouponId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CouponId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_DayId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_TimeId",
                table: "Appointments");

            
            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "DayId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "TimeId",
                table: "Appointments",
                newName: "DoctorDayId");

            migrationBuilder.AddColumn<int>(
                name: "DoctorDayId",
                table: "DoctorDays",
                type: "int",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_DoctorDays_DoctorDayId",
                table: "DoctorDays",
                column: "DoctorDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDays_Appointments_DoctorDayId",
                table: "DoctorDays",
                column: "DoctorDayId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
