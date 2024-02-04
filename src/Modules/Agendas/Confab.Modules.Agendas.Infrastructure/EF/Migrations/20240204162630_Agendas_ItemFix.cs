using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Confab.Modules.Agendas.Infrastructure.EF.Migrations
{
    /// <inheritdoc />
    public partial class Agendas_ItemFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendaItems_AgendaSlots_AgendaSlotId",
                schema: "agendas",
                table: "AgendaItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgendaSlotId",
                schema: "agendas",
                table: "AgendaItems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AgendaItems_AgendaSlots_AgendaSlotId",
                schema: "agendas",
                table: "AgendaItems",
                column: "AgendaSlotId",
                principalSchema: "agendas",
                principalTable: "AgendaSlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgendaItems_AgendaSlots_AgendaSlotId",
                schema: "agendas",
                table: "AgendaItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "AgendaSlotId",
                schema: "agendas",
                table: "AgendaItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
    }
}
