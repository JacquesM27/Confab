using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Confab.Modules.Agendas.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Agendas_SlotFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendaSlots_AgendaItems_AgendaItemId",
                schema: "agendas",
                table: "AgendaSlots");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendaSlots_AgendaItems_AgendaItemId",
                schema: "agendas",
                table: "AgendaSlots",
                column: "AgendaItemId",
                principalSchema: "agendas",
                principalTable: "AgendaItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendaSlots_AgendaItems_AgendaItemId",
                schema: "agendas",
                table: "AgendaSlots");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendaSlots_AgendaItems_AgendaItemId",
                schema: "agendas",
                table: "AgendaSlots",
                column: "AgendaItemId",
                principalSchema: "agendas",
                principalTable: "AgendaItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
