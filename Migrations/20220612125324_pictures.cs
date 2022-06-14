﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CraigList.Migrations
{
    public partial class pictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                schema: "Identity",
                table: "AuctionModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                schema: "Identity",
                table: "AuctionModel");
        }
    }
}
