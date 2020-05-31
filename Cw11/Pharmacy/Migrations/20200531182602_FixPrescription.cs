using Microsoft.EntityFrameworkCore.Migrations;

namespace Pharmacy.Migrations
{
    public partial class FixPrescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorIdDoctor",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_PatientIdPatient",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_DoctorIdDoctor",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_PatientIdPatient",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "DoctorIdDoctor",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "PatientIdPatient",
                table: "Prescriptions");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_IdDoctor",
                table: "Prescriptions",
                column: "IdDoctor");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_IdPatient",
                table: "Prescriptions",
                column: "IdPatient");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Doctors_IdDoctor",
                table: "Prescriptions",
                column: "IdDoctor",
                principalTable: "Doctors",
                principalColumn: "IdDoctor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_IdPatient",
                table: "Prescriptions",
                column: "IdPatient",
                principalTable: "Patients",
                principalColumn: "IdPatient",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Doctors_IdDoctor",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Patients_IdPatient",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_IdDoctor",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Prescriptions_IdPatient",
                table: "Prescriptions");

            migrationBuilder.AddColumn<int>(
                name: "DoctorIdDoctor",
                table: "Prescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientIdPatient",
                table: "Prescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_DoctorIdDoctor",
                table: "Prescriptions",
                column: "DoctorIdDoctor");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientIdPatient",
                table: "Prescriptions",
                column: "PatientIdPatient");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorIdDoctor",
                table: "Prescriptions",
                column: "DoctorIdDoctor",
                principalTable: "Doctors",
                principalColumn: "IdDoctor",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Patients_PatientIdPatient",
                table: "Prescriptions",
                column: "PatientIdPatient",
                principalTable: "Patients",
                principalColumn: "IdPatient",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
