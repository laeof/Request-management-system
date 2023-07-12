using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RMS.Migrations
{
    /// <inheritdoc />
    public partial class _initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lifecycles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Planning = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Current = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Closed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cancelled = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lifecycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    ImgPath = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserRoleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RequestId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    LifecycleId = table.Column<long>(type: "bigint", nullable: false),
                    CloseId = table.Column<long>(type: "bigint", nullable: true),
                    ClosedId = table.Column<long>(type: "bigint", nullable: true),
                    CancelId = table.Column<long>(type: "bigint", nullable: true),
                    CancelledId = table.Column<long>(type: "bigint", nullable: true),
                    OpenId = table.Column<long>(type: "bigint", nullable: true),
                    OpenedId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedName = table.Column<string>(type: "text", nullable: false),
                    CreatedId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Lifecycles_LifecycleId",
                        column: x => x.LifecycleId,
                        principalTable: "Lifecycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Users_CancelledId",
                        column: x => x.CancelledId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_ClosedId",
                        column: x => x.ClosedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_CreatedId",
                        column: x => x.CreatedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Users_OpenedId",
                        column: x => x.OpenedId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "RequestId" },
                values: new object[] { 1L, "No internet", null });

            migrationBuilder.InsertData(
                table: "Lifecycles",
                columns: new[] { "Id", "Cancelled", "Closed", "Current", "Planning" },
                values: new object[,]
                {
                    { 1L, null, null, null, new DateTime(2023, 7, 12, 18, 53, 32, 383, DateTimeKind.Utc).AddTicks(4426) },
                    { 2L, null, null, null, new DateTime(2023, 7, 12, 18, 53, 32, 383, DateTimeKind.Utc).AddTicks(4446) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "admin" },
                    { 2L, "manager" },
                    { 3L, "mounter" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Comment", "FirstName", "ImgPath", "IsActive", "Login", "Password", "Surname" },
                values: new object[,]
                {
                    { 1L, "Comment", "Max", "../../img/jpg/preview.jpg", true, "ADMIN", "$HASH|V1$10000$urqg7kuhyAD/7uNoiXf3CEx2aJJrXbRKDNRXRD+CUDtTk9wq", "Akchurin" },
                    { 2L, "Comment", "Anton", "../../img/Avatar/user.png", true, "MANAGER", "$HASH|V1$10000$dpPG/ZQja2StkcLwG49kIWzowKv4x7F5N/0bmH7HwAdAzAsM", "Guryshkin" },
                    { 3L, "Comment", "Georgii", "../../img/Avatar/user.png", true, "mounter", "$HASH|V1$10000$VqPQfxDL5/OV5+7RGf1vUEsuf84/U2N2Mxe38FRvK5d6pLQF", "Perepelitsa" }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "Address", "CancelId", "CancelledId", "CategoryId", "CloseId", "ClosedId", "Comment", "CreatedId", "CreatedName", "Description", "LifecycleId", "Name", "OpenId", "OpenedId", "Priority", "Status" },
                values: new object[,]
                {
                    { 1L, "some address", null, null, 1L, null, null, "comment", null, "Max Akchurin", "description", 1L, "request 1", null, null, 1, 1 },
                    { 2L, "some address", null, null, 1L, null, null, "comment", null, "Max Akchurin", "description", 2L, "request 2", null, null, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "UserRoleId", "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 2L, 2L },
                    { 3L, 3L, 3L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_RequestId",
                table: "Categories",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CancelledId",
                table: "Requests",
                column: "CancelledId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CategoryId",
                table: "Requests",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ClosedId",
                table: "Requests",
                column: "ClosedId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CreatedId",
                table: "Requests",
                column: "CreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_LifecycleId",
                table: "Requests",
                column: "LifecycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_OpenedId",
                table: "Requests",
                column: "OpenedId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Requests_RequestId",
                table: "Categories",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Requests_RequestId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Lifecycles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
