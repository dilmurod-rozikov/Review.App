using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class OptimizeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_Countries_CountryId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonCategories_Categories_CategoryId",
                table: "PokemonCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonCategories_Pokemons_PokemonId",
                table: "PokemonCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonOwners_Owners_OwnerId",
                table: "PokemonOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonOwners_Pokemons_PokemonId",
                table: "PokemonOwners");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Pokemons_PokemonId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reviewers_ReviewerId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviewers",
                table: "Reviewers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonOwners",
                table: "PokemonOwners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonCategories",
                table: "PokemonCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameTable(
                name: "Reviewers",
                newName: "Reviewer");

            migrationBuilder.RenameTable(
                name: "Pokemons",
                newName: "Pokemon");

            migrationBuilder.RenameTable(
                name: "PokemonOwners",
                newName: "PokemonOwner");

            migrationBuilder.RenameTable(
                name: "PokemonCategories",
                newName: "PokemonCategory");

            migrationBuilder.RenameTable(
                name: "Owners",
                newName: "Owner");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "Country");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Review",
                newName: "IX_Review_ReviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_PokemonId",
                table: "Review",
                newName: "IX_Review_PokemonId");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonOwners_OwnerId",
                table: "PokemonOwner",
                newName: "IX_PokemonOwner_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonCategories_CategoryId",
                table: "PokemonCategory",
                newName: "IX_PokemonCategory_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Owners_CountryId",
                table: "Owner",
                newName: "IX_Owner_CountryId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Review",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewerId",
                table: "Review",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PokemonId",
                table: "Review",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Review",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Reviewer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Reviewer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pokemon",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Owner",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Gym",
                table: "Owner",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Country",
                type: "varchar(50)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Category",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewer",
                table: "Reviewer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemon",
                table: "Pokemon",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonOwner",
                table: "PokemonOwner",
                columns: new[] { "PokemonId", "OwnerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonCategory",
                table: "PokemonCategory",
                columns: new[] { "PokemonId", "CategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owner",
                table: "Owner",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Country",
                table: "Country",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Owner_Country_CountryId",
                table: "Owner",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonCategory_Category_CategoryId",
                table: "PokemonCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonCategory_Pokemon_PokemonId",
                table: "PokemonCategory",
                column: "PokemonId",
                principalTable: "Pokemon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonOwner_Owner_OwnerId",
                table: "PokemonOwner",
                column: "OwnerId",
                principalTable: "Owner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonOwner_Pokemon_PokemonId",
                table: "PokemonOwner",
                column: "PokemonId",
                principalTable: "Pokemon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Pokemon_PokemonId",
                table: "Review",
                column: "PokemonId",
                principalTable: "Pokemon",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Reviewer_ReviewerId",
                table: "Review",
                column: "ReviewerId",
                principalTable: "Reviewer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owner_Country_CountryId",
                table: "Owner");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonCategory_Category_CategoryId",
                table: "PokemonCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonCategory_Pokemon_PokemonId",
                table: "PokemonCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonOwner_Owner_OwnerId",
                table: "PokemonOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_PokemonOwner_Pokemon_PokemonId",
                table: "PokemonOwner");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Pokemon_PokemonId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Reviewer_ReviewerId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviewer",
                table: "Reviewer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonOwner",
                table: "PokemonOwner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PokemonCategory",
                table: "PokemonCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pokemon",
                table: "Pokemon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owner",
                table: "Owner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Country",
                table: "Country");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Reviewer",
                newName: "Reviewers");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "PokemonOwner",
                newName: "PokemonOwners");

            migrationBuilder.RenameTable(
                name: "PokemonCategory",
                newName: "PokemonCategories");

            migrationBuilder.RenameTable(
                name: "Pokemon",
                newName: "Pokemons");

            migrationBuilder.RenameTable(
                name: "Owner",
                newName: "Owners");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Countries");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Review_ReviewerId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Review_PokemonId",
                table: "Reviews",
                newName: "IX_Reviews_PokemonId");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonOwner_OwnerId",
                table: "PokemonOwners",
                newName: "IX_PokemonOwners_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PokemonCategory_CategoryId",
                table: "PokemonCategories",
                newName: "IX_PokemonCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Owner_CountryId",
                table: "Owners",
                newName: "IX_Owners_CountryId");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Reviewers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Reviewers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "ReviewerId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PokemonId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pokemons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Gym",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviewers",
                table: "Reviewers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonOwners",
                table: "PokemonOwners",
                columns: new[] { "PokemonId", "OwnerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PokemonCategories",
                table: "PokemonCategories",
                columns: new[] { "PokemonId", "CategoryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pokemons",
                table: "Pokemons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_Countries_CountryId",
                table: "Owners",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonCategories_Categories_CategoryId",
                table: "PokemonCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonCategories_Pokemons_PokemonId",
                table: "PokemonCategories",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonOwners_Owners_OwnerId",
                table: "PokemonOwners",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokemonOwners_Pokemons_PokemonId",
                table: "PokemonOwners",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Pokemons_PokemonId",
                table: "Reviews",
                column: "PokemonId",
                principalTable: "Pokemons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reviewers_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "Reviewers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
