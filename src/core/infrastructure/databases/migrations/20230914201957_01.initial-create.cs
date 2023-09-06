using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace netflow.core.infrastructure.databases.migrations
{
    /// <inheritdoc />
    public partial class _01initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false, defaultValue: "netflow"),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(350)", unicode: false, maxLength: 350, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflows",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false, defaultValue: "netflow"),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflows", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workflow_steps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", unicode: false, maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    approved_next_step_id = table.Column<int>(type: "integer", nullable: true),
                    rejected_next_step_id = table.Column<int>(type: "integer", nullable: true),
                    order = table.Column<int>(type: "integer", nullable: true),
                    maximum_time_in_minutes = table.Column<int>(type: "integer", nullable: true),
                    auto_approve_on_threshold = table.Column<bool>(type: "boolean", nullable: true),
                    workflow_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false, defaultValue: "netflow"),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflow_steps", x => x.id);
                    table.ForeignKey(
                        name: "fk_workflow_steps_workflow_steps_approved_next_step_id",
                        column: x => x.approved_next_step_id,
                        principalTable: "workflow_steps",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_workflow_steps_workflow_steps_rejected_next_step_id",
                        column: x => x.rejected_next_step_id,
                        principalTable: "workflow_steps",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_workflow_steps_workflows_workflow_id",
                        column: x => x.workflow_id,
                        principalTable: "workflows",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    workflow_step_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflow_step_roles", x => new { x.role_id, x.workflow_step_id });
                    table.ForeignKey(
                        name: "fk_workflow_step_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_workflow_step_roles_workflow_steps_workflow_step_id",
                        column: x => x.workflow_step_id,
                        principalTable: "workflow_steps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_instances",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    current_step_id = table.Column<int>(type: "integer", nullable: true),
                    base_workflow_id = table.Column<int>(type: "integer", nullable: false),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false, defaultValue: "netflow"),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflow_instances", x => x.id);
                    table.ForeignKey(
                        name: "fk_workflow_instances_workflows_base_workflow_id",
                        column: x => x.base_workflow_id,
                        principalTable: "workflows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_instances",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    base_step_id = table.Column<int>(type: "integer", nullable: false),
                    workflow_instance_id = table.Column<int>(type: "integer", nullable: false),
                    expires_in = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    auto_approve_on_threshold = table.Column<bool>(type: "boolean", nullable: true),
                    is_approved = table.Column<bool>(type: "boolean", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false, defaultValue: "netflow"),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflow_step_instances", x => x.id);
                    table.ForeignKey(
                        name: "fk_workflow_step_instances_workflow_instances_workflow_instanc",
                        column: x => x.workflow_instance_id,
                        principalTable: "workflow_instances",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_workflow_step_instances_workflow_steps_base_step_id",
                        column: x => x.base_step_id,
                        principalTable: "workflow_steps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_step_approvals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_approved = table.Column<bool>(type: "boolean", nullable: true),
                    comments = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: false),
                    workflow_step_instance_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    created_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false, defaultValue: "netflow"),
                    last_modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workflow_step_approvals", x => x.id);
                    table.ForeignKey(
                        name: "fk_workflow_step_approvals_workflow_step_instance_workflow_ste",
                        column: x => x.workflow_step_instance_id,
                        principalTable: "workflow_step_instances",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "created_at", "created_by", "description", "last_modified_at", "last_modified_by", "name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Supervisors review and approve the work hours and activities recorded by their respective team members.", null, null, "Employee Supervisor" },
                    { 2, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Department review and approve the employee time records for their departments or teams. They ensure accuracy and compliance with department-specific policies.", null, null, "Department Manager" },
                    { 3, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Resource managers review and approve the overall payroll data, ensuring that the allocation of resources and budget is accurate and aligns with the organization's goals.", null, null, "Resource Manager" },
                    { 4, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Payroll specialists are responsible for calculating gross pay, deductions, and net pay for each employee. They ensure accuracy and compliance with tax regulations.", null, null, "Payroll Specialist" },
                    { 5, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Tax compliance officers oversee tax withholding and ensure that it is done accurately based on employee information and tax laws.", null, null, "Tax Compliance Officer" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email" },
                values: new object[] { 1, "eduardo.lour.silva@gmail.com" });

            migrationBuilder.InsertData(
                table: "workflows",
                columns: new[] { "id", "created_at", "created_by", "description", "last_modified_at", "last_modified_by", "name" },
                values: new object[] { 1, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Manages employee compensation with precision, covering timekeeping, tax compliance, and benefit administration.", null, null, "Payroll Process" });

            migrationBuilder.InsertData(
                table: "workflow_steps",
                columns: new[] { "id", "approved_next_step_id", "created_at", "created_by", "description", "last_modified_at", "last_modified_by", "name", "order", "rejected_next_step_id", "workflow_id", "auto_approve_on_threshold", "maximum_time_in_minutes" },
                values: new object[] { 5, null, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Ensure accurate tax withholding based on employee information.", null, null, "Tax Withholding", null, null, 1, true, 5 });

            migrationBuilder.InsertData(
                table: "workflow_step_roles",
                columns: new[] { "role_id", "workflow_step_id" },
                values: new object[] { 5, 5 });

            migrationBuilder.InsertData(
                table: "workflow_steps",
                columns: new[] { "id", "approved_next_step_id", "created_at", "created_by", "description", "last_modified_at", "last_modified_by", "name", "order", "rejected_next_step_id", "workflow_id" },
                values: new object[] { 4, 5, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Calculate gross pay, deductions, and net pay for each employee.", null, null, "Payroll Calculation", null, null, 1 });

            migrationBuilder.InsertData(
                table: "workflow_step_roles",
                columns: new[] { "role_id", "workflow_step_id" },
                values: new object[] { 4, 4 });

            migrationBuilder.InsertData(
                table: "workflow_steps",
                columns: new[] { "id", "approved_next_step_id", "created_at", "created_by", "description", "last_modified_at", "last_modified_by", "name", "order", "rejected_next_step_id", "workflow_id" },
                values: new object[] { 3, 4, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Resource management department reviews and approves payroll data.", null, null, "Resource Management Approval", null, null, 1 });

            migrationBuilder.InsertData(
                table: "workflow_step_roles",
                columns: new[] { "role_id", "workflow_step_id" },
                values: new object[] { 3, 3 });

            migrationBuilder.InsertData(
                table: "workflow_steps",
                columns: new[] { "id", "approved_next_step_id", "created_at", "created_by", "description", "last_modified_at", "last_modified_by", "name", "order", "rejected_next_step_id", "workflow_id" },
                values: new object[] { 2, 3, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Managers review and approve employee time records.", null, null, "Manager Approval", null, null, 1 });

            migrationBuilder.InsertData(
                table: "workflow_step_roles",
                columns: new[] { "role_id", "workflow_step_id" },
                values: new object[] { 2, 2 });

            migrationBuilder.InsertData(
                table: "workflow_steps",
                columns: new[] { "id", "approved_next_step_id", "created_at", "created_by", "description", "last_modified_at", "last_modified_by", "name", "order", "rejected_next_step_id", "workflow_id" },
                values: new object[] { 1, 2, new DateTime(2023, 9, 7, 3, 0, 0, 0, DateTimeKind.Utc), "netflow", "Employees record their work hours and activities.", null, null, "Employee Timekeeping", null, null, 1 });

            migrationBuilder.InsertData(
                table: "workflow_step_roles",
                columns: new[] { "role_id", "workflow_step_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "ix_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_workflow_instances_base_workflow_id",
                table: "workflow_instances",
                column: "base_workflow_id");

            migrationBuilder.CreateIndex(
                name: "ix_workflow_instances_current_step_id",
                table: "workflow_instances",
                column: "current_step_id");

            migrationBuilder.CreateIndex(
                name: "ix_workflow_step_approvals_workflow_step_instance_id",
                table: "workflow_step_approvals",
                column: "workflow_step_instance_id");

            migrationBuilder.CreateIndex(
                name: "ix_workflow_step_instances_base_step_id",
                table: "workflow_step_instances",
                column: "base_step_id");

            migrationBuilder.CreateIndex(
                name: "ix_workflow_step_instances_workflow_instance_id",
                table: "workflow_step_instances",
                column: "workflow_instance_id");

            migrationBuilder.CreateIndex(
                name: "ix_workflow_step_roles_workflow_step_id",
                table: "workflow_step_roles",
                column: "workflow_step_id");

            migrationBuilder.CreateIndex(
                name: "ix_workflow_steps_approved_next_step_id",
                table: "workflow_steps",
                column: "approved_next_step_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_workflow_steps_rejected_next_step_id",
                table: "workflow_steps",
                column: "rejected_next_step_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_workflow_steps_workflow_id",
                table: "workflow_steps",
                column: "workflow_id");

            migrationBuilder.CreateIndex(
                name: "ix_workflows_name",
                table: "workflows",
                column: "name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_workflow_instances_workflow_step_instance_current_step_id",
                table: "workflow_instances",
                column: "current_step_id",
                principalTable: "workflow_step_instances",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_workflow_instances_workflow_step_instance_current_step_id",
                table: "workflow_instances");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "workflow_step_approvals");

            migrationBuilder.DropTable(
                name: "workflow_step_roles");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "workflow_step_instances");

            migrationBuilder.DropTable(
                name: "workflow_instances");

            migrationBuilder.DropTable(
                name: "workflow_steps");

            migrationBuilder.DropTable(
                name: "workflows");
        }
    }
}
