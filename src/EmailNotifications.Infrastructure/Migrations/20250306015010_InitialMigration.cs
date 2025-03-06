using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EmailNotifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NotificationType = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FromAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FromName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ToAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ToName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CcAddresses = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    BccAddresses = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailSpecifications",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NotificationType = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HtmlBody = table.Column<string>(type: "text", nullable: false),
                    TextBody = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    FromAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FromName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReplyToAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSpecifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailRecipientGroups",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EmailSpecificationId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailRecipientGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailRecipientGroups_EmailSpecifications_EmailSpecification~",
                        column: x => x.EmailSpecificationId,
                        principalSchema: "notification",
                        principalTable: "EmailSpecifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailRecipients",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmailAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EmailRecipientGroupId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailRecipients_EmailRecipientGroups_EmailRecipientGroupId",
                        column: x => x.EmailRecipientGroupId,
                        principalSchema: "notification",
                        principalTable: "EmailRecipientGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_CreatedAt",
                schema: "notification",
                table: "EmailLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_NotificationType",
                schema: "notification",
                table: "EmailLogs",
                column: "NotificationType");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_SentAt",
                schema: "notification",
                table: "EmailLogs",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_Status",
                schema: "notification",
                table: "EmailLogs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecipientGroups_EmailSpecificationId_Name",
                schema: "notification",
                table: "EmailRecipientGroups",
                columns: new[] { "EmailSpecificationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecipients_EmailAddress",
                schema: "notification",
                table: "EmailRecipients",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_EmailRecipients_EmailRecipientGroupId_EmailAddress_Type",
                schema: "notification",
                table: "EmailRecipients",
                columns: new[] { "EmailRecipientGroupId", "EmailAddress", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailSpecifications_IsActive",
                schema: "notification",
                table: "EmailSpecifications",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSpecifications_Name",
                schema: "notification",
                table: "EmailSpecifications",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailSpecifications_NotificationType",
                schema: "notification",
                table: "EmailSpecifications",
                column: "NotificationType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLogs",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "EmailRecipients",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "EmailRecipientGroups",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "EmailSpecifications",
                schema: "notification");
        }
    }
}
