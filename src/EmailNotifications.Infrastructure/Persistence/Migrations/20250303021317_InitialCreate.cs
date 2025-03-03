using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailNotifications.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.CreateTable(
                name: "EmailSpecifications",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NotificationTypeId = table.Column<int>(type: "integer", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    HtmlBody = table.Column<string>(type: "text", nullable: false),
                    TextBody = table.Column<string>(type: "text", nullable: true),
                    FromAddress = table.Column<string>(type: "text", nullable: false),
                    FromName = table.Column<string>(type: "text", nullable: true),
                    ReplyToAddress = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSpecifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailLogs",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailSpecificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    FromAddress = table.Column<string>(type: "text", nullable: false),
                    FromName = table.Column<string>(type: "text", nullable: true),
                    ToAddresses = table.Column<string>(type: "text", nullable: false),
                    CcAddresses = table.Column<string>(type: "text", nullable: true),
                    BccAddresses = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AttemptCount = table.Column<int>(type: "integer", nullable: false),
                    NextAttemptAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    SerializedModel = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailLogs_EmailSpecifications_EmailSpecificationId",
                        column: x => x.EmailSpecificationId,
                        principalSchema: "notification",
                        principalTable: "EmailSpecifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailRecipientGroups",
                schema: "notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EmailSpecificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    EmailRecipientGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                name: "IX_EmailLogs_EmailSpecificationId",
                schema: "notification",
                table: "EmailLogs",
                column: "EmailSpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailLogs_NextAttemptAt",
                schema: "notification",
                table: "EmailLogs",
                column: "NextAttemptAt");

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
                name: "IX_EmailSpecifications_NotificationTypeId",
                schema: "notification",
                table: "EmailSpecifications",
                column: "NotificationTypeId");
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
