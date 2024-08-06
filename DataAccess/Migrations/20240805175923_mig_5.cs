using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Scans_OrderDetails_orderDetailId",
                table: "Scans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Scans",
                table: "Scans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "VB_USERS");

            migrationBuilder.RenameTable(
                name: "Scans",
                newName: "VB_SCANS");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "VB_ORDERS");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "VB_ORDERDETAILS");

            migrationBuilder.RenameIndex(
                name: "IX_Scans_orderDetailId",
                table: "VB_SCANS",
                newName: "IX_VB_SCANS_orderDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserId",
                table: "VB_ORDERS",
                newName: "IX_VB_ORDERS_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_OrderId",
                table: "VB_ORDERDETAILS",
                newName: "IX_VB_ORDERDETAILS_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VB_USERS",
                table: "VB_USERS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VB_SCANS",
                table: "VB_SCANS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VB_ORDERS",
                table: "VB_ORDERS",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VB_ORDERDETAILS",
                table: "VB_ORDERDETAILS",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VB_ORDERDETAILS_VB_ORDERS_OrderId",
                table: "VB_ORDERDETAILS",
                column: "OrderId",
                principalTable: "VB_ORDERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VB_ORDERS_VB_USERS_UserId",
                table: "VB_ORDERS",
                column: "UserId",
                principalTable: "VB_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VB_SCANS_VB_ORDERDETAILS_orderDetailId",
                table: "VB_SCANS",
                column: "orderDetailId",
                principalTable: "VB_ORDERDETAILS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VB_ORDERDETAILS_VB_ORDERS_OrderId",
                table: "VB_ORDERDETAILS");

            migrationBuilder.DropForeignKey(
                name: "FK_VB_ORDERS_VB_USERS_UserId",
                table: "VB_ORDERS");

            migrationBuilder.DropForeignKey(
                name: "FK_VB_SCANS_VB_ORDERDETAILS_orderDetailId",
                table: "VB_SCANS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VB_USERS",
                table: "VB_USERS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VB_SCANS",
                table: "VB_SCANS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VB_ORDERS",
                table: "VB_ORDERS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VB_ORDERDETAILS",
                table: "VB_ORDERDETAILS");

            migrationBuilder.RenameTable(
                name: "VB_USERS",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "VB_SCANS",
                newName: "Scans");

            migrationBuilder.RenameTable(
                name: "VB_ORDERS",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "VB_ORDERDETAILS",
                newName: "OrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_VB_SCANS_orderDetailId",
                table: "Scans",
                newName: "IX_Scans_orderDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_VB_ORDERS_UserId",
                table: "Orders",
                newName: "IX_Orders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VB_ORDERDETAILS_OrderId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Scans",
                table: "Scans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderId",
                table: "OrderDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scans_OrderDetails_orderDetailId",
                table: "Scans",
                column: "orderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
