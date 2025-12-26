using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Manager = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareLicenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Manufacturer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LicenseKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareLicenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId1",
                        column: x => x.DepartmentId1,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Equipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EquipmentTypeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId1 = table.Column<int>(type: "int", nullable: true),
                    EquipmentTypeId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Equipments_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipments_EquipmentTypes_EquipmentTypeId1",
                        column: x => x.EquipmentTypeId1,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    MovedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldEmployeeId = table.Column<int>(type: "int", nullable: true),
                    NewEmployeeId = table.Column<int>(type: "int", nullable: true),
                    EquipmentId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_Employees_NewEmployeeId",
                        column: x => x.NewEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_Employees_OldEmployeeId",
                        column: x => x.OldEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentMovements_Equipments_EquipmentId1",
                        column: x => x.EquipmentId1,
                        principalTable: "Equipments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InstalledSoftwares",
                columns: table => new
                {
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    SoftwareLicenseId = table.Column<int>(type: "int", nullable: false),
                    InstalledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EquipmentId1 = table.Column<int>(type: "int", nullable: true),
                    SoftwareLicenseId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstalledSoftwares", x => new { x.EquipmentId, x.SoftwareLicenseId });
                    table.ForeignKey(
                        name: "FK_InstalledSoftwares_Equipments_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstalledSoftwares_Equipments_EquipmentId1",
                        column: x => x.EquipmentId1,
                        principalTable: "Equipments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InstalledSoftwares_SoftwareLicenses_SoftwareLicenseId",
                        column: x => x.SoftwareLicenseId,
                        principalTable: "SoftwareLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstalledSoftwares_SoftwareLicenses_SoftwareLicenseId1",
                        column: x => x.SoftwareLicenseId1,
                        principalTable: "SoftwareLicenses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId1",
                table: "Employees",
                column: "DepartmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_EquipmentId",
                table: "EquipmentMovements",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_EquipmentId1",
                table: "EquipmentMovements",
                column: "EquipmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_NewEmployeeId",
                table: "EquipmentMovements",
                column: "NewEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMovements_OldEmployeeId",
                table: "EquipmentMovements",
                column: "OldEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EmployeeId",
                table: "Equipments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EmployeeId1",
                table: "Equipments",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentTypeId",
                table: "Equipments",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_EquipmentTypeId1",
                table: "Equipments",
                column: "EquipmentTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_Equipments_InventoryNumber",
                table: "Equipments",
                column: "InventoryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstalledSoftwares_EquipmentId1",
                table: "InstalledSoftwares",
                column: "EquipmentId1");

            migrationBuilder.CreateIndex(
                name: "IX_InstalledSoftwares_SoftwareLicenseId",
                table: "InstalledSoftwares",
                column: "SoftwareLicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_InstalledSoftwares_SoftwareLicenseId1",
                table: "InstalledSoftwares",
                column: "SoftwareLicenseId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentMovements");

            migrationBuilder.DropTable(
                name: "InstalledSoftwares");

            migrationBuilder.DropTable(
                name: "Equipments");

            migrationBuilder.DropTable(
                name: "SoftwareLicenses");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
