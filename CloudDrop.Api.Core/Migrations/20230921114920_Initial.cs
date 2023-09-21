using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudDrop.Api.Core.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "user",
            columns: table => new
            {
                id = table.Column<uint>(type: "int unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                firstname = table.Column<string>(name: "first_name", type: "varchar(100)", maxLength: 100, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                lastname = table.Column<string>(name: "last_name", type: "varchar(100)", maxLength: 100, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                username = table.Column<string>(name: "user_name", type: "varchar(100)", maxLength: 100, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                email = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                isdeleted = table.Column<bool>(name: "is_deleted", type: "tinyint(1)", nullable: false),
                createdat = table.Column<DateTime>(name: "created_at", type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updatedat = table.Column<DateTime>(name: "updated_at", type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
            },
            constraints: table =>
            {
                table.PrimaryKey("PRIMARY", x => x.id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "file",
            columns: table => new
            {
                id = table.Column<uint>(type: "int unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                userid = table.Column<uint>(name: "user_id", type: "int unsigned", nullable: false),
                name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                mimetype = table.Column<string>(name: "mime_type", type: "varchar(255)", maxLength: 255, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                size = table.Column<long>(type: "bigint", nullable: false),
                isdeleted = table.Column<bool>(name: "is_deleted", type: "tinyint(1)", nullable: false),
                createdat = table.Column<DateTime>(name: "created_at", type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updatedat = table.Column<DateTime>(name: "updated_at", type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
            },
            constraints: table =>
            {
                table.PrimaryKey("PRIMARY", x => x.id);
                table.ForeignKey(
                    name: "FK_file_user_id",
                    column: x => x.userid,
                    principalTable: "user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "upload_session",
            columns: table => new
            {
                id = table.Column<uint>(type: "int unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                userid = table.Column<uint>(name: "user_id", type: "int unsigned", nullable: false),
                sessionid = table.Column<string>(name: "session_id", type: "varchar(50)", maxLength: 50, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                filename = table.Column<string>(name: "file_name", type: "varchar(100)", maxLength: 100, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                size = table.Column<long>(type: "bigint", nullable: false),
                receivedbytes = table.Column<long>(name: "received_bytes", type: "bigint", nullable: false),
                expirationdatetime = table.Column<DateTime>(name: "expiration_datetime", type: "datetime(6)", nullable: false),
                isdeleted = table.Column<bool>(name: "is_deleted", type: "tinyint(1)", nullable: false),
                createdat = table.Column<DateTime>(name: "created_at", type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                updatedat = table.Column<DateTime>(name: "updated_at", type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
            },
            constraints: table =>
            {
                table.PrimaryKey("PRIMARY", x => x.id);
                table.ForeignKey(
                    name: "FK_upload_session_user_id",
                    column: x => x.userid,
                    principalTable: "user",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "UK_file_user_id_name",
            table: "file",
            columns: new[] { "user_id", "name" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_upload_session_user_id",
            table: "upload_session",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "UK_upload_session_session_id",
            table: "upload_session",
            column: "session_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UK_user_email_address",
            table: "user",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UK_user_user_name",
            table: "user",
            column: "user_name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "file");

        migrationBuilder.DropTable(
            name: "upload_session");

        migrationBuilder.DropTable(
            name: "user");
    }
}
