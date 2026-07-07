using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiveAID.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminCoreModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutUsSubpages",
                columns: table => new
                {
                    SubpageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutUsSubpages", x => x.SubpageId);
                });

            migrationBuilder.CreateTable(
                name: "CorporatePartners",
                columns: table => new
                {
                    PartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebsiteLink = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorporatePartners", x => x.PartnerId);
                });

            migrationBuilder.CreateTable(
                name: "DonationCauses",
                columns: table => new
                {
                    CauseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationCauses", x => x.CauseId);
                });

            migrationBuilder.CreateTable(
                name: "Ngos",
                columns: table => new
                {
                    NgoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ngos", x => x.NgoId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gateway = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReferenceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "NgoPartners",
                columns: table => new
                {
                    NgoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgoPartners", x => new { x.NgoId, x.PartnerId });
                    table.ForeignKey(
                        name: "FK_NgoPartners_CorporatePartners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "CorporatePartners",
                        principalColumn: "PartnerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NgoPartners_Ngos_NgoId",
                        column: x => x.NgoId,
                        principalTable: "Ngos",
                        principalColumn: "NgoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WelfareProgrammes",
                columns: table => new
                {
                    ProgrammeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CauseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MaxDonation = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WelfareProgrammes", x => x.ProgrammeId);
                    table.ForeignKey(
                        name: "FK_WelfareProgrammes_DonationCauses_CauseId",
                        column: x => x.CauseId,
                        principalTable: "DonationCauses",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WelfareProgrammes_Ngos_NgoId",
                        column: x => x.NgoId,
                        principalTable: "Ngos",
                        principalColumn: "NgoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInterests",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInterests", x => new { x.UserId, x.NgoId });
                    table.ForeignKey(
                        name: "FK_UserInterests_Ngos_NgoId",
                        column: x => x.NgoId,
                        principalTable: "Ngos",
                        principalColumn: "NgoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInterests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserModifications",
                columns: table => new
                {
                    ModificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubpageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HtmlContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModifications", x => x.ModificationId);
                    table.ForeignKey(
                        name: "FK_UserModifications_AboutUsSubpages_SubpageId",
                        column: x => x.SubpageId,
                        principalTable: "AboutUsSubpages",
                        principalColumn: "SubpageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserModifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserQueries",
                columns: table => new
                {
                    QueryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQueries", x => x.QueryId);
                    table.ForeignKey(
                        name: "FK_UserQueries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    DonationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProgrammeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CauseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.DonationId);
                    table.ForeignKey(
                        name: "FK_Donations_DonationCauses_CauseId",
                        column: x => x.CauseId,
                        principalTable: "DonationCauses",
                        principalColumn: "CauseId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Donations_Ngos_NgoId",
                        column: x => x.NgoId,
                        principalTable: "Ngos",
                        principalColumn: "NgoId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Donations_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donations_WelfareProgrammes_ProgrammeId",
                        column: x => x.ProgrammeId,
                        principalTable: "WelfareProgrammes",
                        principalColumn: "ProgrammeId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GalleryImages",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ProgrammeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_GalleryImages_WelfareProgrammes_ProgrammeId",
                        column: x => x.ProgrammeId,
                        principalTable: "WelfareProgrammes",
                        principalColumn: "ProgrammeId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSubpages_Slug",
                table: "AboutUsSubpages",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AboutUsSubpages_Title",
                table: "AboutUsSubpages",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CorporatePartners_Name",
                table: "CorporatePartners",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CauseId",
                table: "Donations",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_NgoId",
                table: "Donations",
                column: "NgoId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_ProgrammeId",
                table: "Donations",
                column: "ProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_TransactionId",
                table: "Donations",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_UserId",
                table: "Donations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryImages_ProgrammeId",
                table: "GalleryImages",
                column: "ProgrammeId");

            migrationBuilder.CreateIndex(
                name: "IX_NgoPartners_PartnerId",
                table: "NgoPartners",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterests_NgoId",
                table: "UserInterests",
                column: "NgoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModifications_SubpageId",
                table: "UserModifications",
                column: "SubpageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserModifications_UserId",
                table: "UserModifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQueries_UserId",
                table: "UserQueries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WelfareProgrammes_CauseId",
                table: "WelfareProgrammes",
                column: "CauseId");

            migrationBuilder.CreateIndex(
                name: "IX_WelfareProgrammes_NgoId",
                table: "WelfareProgrammes",
                column: "NgoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "GalleryImages");

            migrationBuilder.DropTable(
                name: "NgoPartners");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "UserInterests");

            migrationBuilder.DropTable(
                name: "UserModifications");

            migrationBuilder.DropTable(
                name: "UserQueries");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "WelfareProgrammes");

            migrationBuilder.DropTable(
                name: "CorporatePartners");

            migrationBuilder.DropTable(
                name: "AboutUsSubpages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DonationCauses");

            migrationBuilder.DropTable(
                name: "Ngos");
        }
    }
}
