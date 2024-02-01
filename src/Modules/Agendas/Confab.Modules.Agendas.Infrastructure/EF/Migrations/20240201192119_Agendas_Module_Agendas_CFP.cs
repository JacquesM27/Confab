using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Confab.Modules.Agendas.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Agendas_Module_Agendas_CFP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AgendaTracks",
                schema: "agendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaTracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CallForPapers",
                schema: "agendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsOpened = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallForPapers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgendaItemSpeaker",
                schema: "agendas",
                columns: table => new
                {
                    AgendaItemsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpeakersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaItemSpeaker", x => new { x.AgendaItemsId, x.SpeakersId });
                    table.ForeignKey(
                        name: "FK_AgendaItemSpeaker_Speakers_SpeakersId",
                        column: x => x.SpeakersId,
                        principalSchema: "agendas",
                        principalTable: "Speakers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgendaItems",
                schema: "agendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: false),
                    AgendaSlotId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgendaSlots",
                schema: "agendas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TrackId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    Placeholder = table.Column<string>(type: "text", nullable: true),
                    ParticipantsLimit = table.Column<int>(type: "integer", nullable: true),
                    AgendaItemId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendaSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgendaSlots_AgendaItems_AgendaItemId",
                        column: x => x.AgendaItemId,
                        principalSchema: "agendas",
                        principalTable: "AgendaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgendaSlots_AgendaTracks_TrackId",
                        column: x => x.TrackId,
                        principalSchema: "agendas",
                        principalTable: "AgendaTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgendaItemSpeaker_SpeakersId",
                schema: "agendas",
                table: "AgendaItemSpeaker",
                column: "SpeakersId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaItems_AgendaSlotId",
                schema: "agendas",
                table: "AgendaItems",
                column: "AgendaSlotId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaSlots_AgendaItemId",
                schema: "agendas",
                table: "AgendaSlots",
                column: "AgendaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendaSlots_TrackId",
                schema: "agendas",
                table: "AgendaSlots",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendaItemSpeaker_AgendaItems_AgendaItemsId",
                schema: "agendas",
                table: "AgendaItemSpeaker",
                column: "AgendaItemsId",
                principalSchema: "agendas",
                principalTable: "AgendaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AgendaItems_AgendaSlots_AgendaSlotId",
                schema: "agendas",
                table: "AgendaItems",
                column: "AgendaSlotId",
                principalSchema: "agendas",
                principalTable: "AgendaSlots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendaSlots_AgendaItems_AgendaItemId",
                schema: "agendas",
                table: "AgendaSlots");

            migrationBuilder.DropTable(
                name: "AgendaItemSpeaker",
                schema: "agendas");

            migrationBuilder.DropTable(
                name: "CallForPapers",
                schema: "agendas");

            migrationBuilder.DropTable(
                name: "AgendaItems",
                schema: "agendas");

            migrationBuilder.DropTable(
                name: "AgendaSlots",
                schema: "agendas");

            migrationBuilder.DropTable(
                name: "AgendaTracks",
                schema: "agendas");
        }
    }
}
