using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Primal.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateStarted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CharacterPerformingAction = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FreeCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Defence = table.Column<int>(type: "INTEGER", nullable: false),
                    Might = table.Column<string>(type: "TEXT", nullable: false),
                    Battleflow = table.Column<int>(type: "INTEGER", nullable: false),
                    Effects = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EncounterMightDecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EncounterId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsFreeCompanyDeck = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterMightDecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncounterMightDecks_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Minons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EncounterId = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    Defence = table.Column<int>(type: "INTEGER", nullable: false),
                    XPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    YPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    Might = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Minons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Minons_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FreeCompanyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Class = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    Defence = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAnimus = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimusRegen = table.Column<int>(type: "INTEGER", nullable: false),
                    Might = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_FreeCompanies_FreeCompanyId",
                        column: x => x.FreeCompanyId,
                        principalTable: "FreeCompanies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EncounterPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EncounterId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    XPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    YPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentAnimus = table.Column<int>(type: "INTEGER", nullable: false),
                    Tokens = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncounterPlayers_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EncounterPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerAbilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Battleflow = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerAbilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerAbilities_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    Battleflow = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerItems_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bosses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EncounterId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<string>(type: "TEXT", nullable: false),
                    Defence = table.Column<int>(type: "INTEGER", nullable: false),
                    XPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    YPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    Might = table.Column<string>(type: "TEXT", nullable: false),
                    Direction = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetId = table.Column<int>(type: "INTEGER", nullable: true),
                    ActionComponentIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bosses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bosses_EncounterPlayers_TargetId",
                        column: x => x.TargetId,
                        principalTable: "EncounterPlayers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bosses_Encounters_EncounterId",
                        column: x => x.EncounterId,
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BossId = table.Column<int>(type: "INTEGER", nullable: true),
                    BossPart = table.Column<string>(type: "TEXT", nullable: false),
                    BonusDamage = table.Column<int>(type: "INTEGER", nullable: false),
                    EmpowerTokensUsed = table.Column<int>(type: "INTEGER", nullable: false),
                    RerollTokensUsed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attacks_Bosses_BossId",
                        column: x => x.BossId,
                        principalTable: "Bosses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Attacks_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BossActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BossId = table.Column<int>(type: "INTEGER", nullable: false),
                    Stage = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BossActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BossActions_Bosses_BossId",
                        column: x => x.BossId,
                        principalTable: "Bosses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BossAttacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BossId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BossAttacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BossAttacks_Bosses_BossId",
                        column: x => x.BossId,
                        principalTable: "Bosses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttackMinions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AttackId = table.Column<int>(type: "INTEGER", nullable: false),
                    MinionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttackMinions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttackMinions_Attacks_AttackId",
                        column: x => x.AttackId,
                        principalTable: "Attacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttackMinions_Minons_MinionId",
                        column: x => x.MinionId,
                        principalTable: "Minons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BossAttackPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BossAttackId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BossAttackPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BossAttackPlayers_BossAttacks_BossAttackId",
                        column: x => x.BossAttackId,
                        principalTable: "BossAttacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BossAttackPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MightCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeckId = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackId = table.Column<int>(type: "INTEGER", nullable: true),
                    BossAttackId = table.Column<int>(type: "INTEGER", nullable: true),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IsCritical = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDrawnFromCritical = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MightCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MightCards_Attacks_AttackId",
                        column: x => x.AttackId,
                        principalTable: "Attacks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MightCards_BossAttacks_BossAttackId",
                        column: x => x.BossAttackId,
                        principalTable: "BossAttacks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MightCards_EncounterMightDecks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "EncounterMightDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttackMinions_AttackId",
                table: "AttackMinions",
                column: "AttackId");

            migrationBuilder.CreateIndex(
                name: "IX_AttackMinions_MinionId",
                table: "AttackMinions",
                column: "MinionId");

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_BossId",
                table: "Attacks",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_Attacks_PlayerId",
                table: "Attacks",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BossActions_BossId",
                table: "BossActions",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_BossAttackPlayers_BossAttackId",
                table: "BossAttackPlayers",
                column: "BossAttackId");

            migrationBuilder.CreateIndex(
                name: "IX_BossAttackPlayers_PlayerId",
                table: "BossAttackPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BossAttacks_BossId",
                table: "BossAttacks",
                column: "BossId");

            migrationBuilder.CreateIndex(
                name: "IX_Bosses_EncounterId",
                table: "Bosses",
                column: "EncounterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bosses_TargetId",
                table: "Bosses",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterMightDecks_EncounterId",
                table: "EncounterMightDecks",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterPlayers_EncounterId",
                table: "EncounterPlayers",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterPlayers_PlayerId",
                table: "EncounterPlayers",
                column: "PlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MightCards_AttackId",
                table: "MightCards",
                column: "AttackId");

            migrationBuilder.CreateIndex(
                name: "IX_MightCards_BossAttackId",
                table: "MightCards",
                column: "BossAttackId");

            migrationBuilder.CreateIndex(
                name: "IX_MightCards_DeckId",
                table: "MightCards",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Minons_EncounterId",
                table: "Minons",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerAbilities_PlayerId",
                table: "PlayerAbilities",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_ItemId",
                table: "PlayerItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_PlayerId",
                table: "PlayerItems",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_FreeCompanyId",
                table: "Players",
                column: "FreeCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttackMinions");

            migrationBuilder.DropTable(
                name: "BossActions");

            migrationBuilder.DropTable(
                name: "BossAttackPlayers");

            migrationBuilder.DropTable(
                name: "MightCards");

            migrationBuilder.DropTable(
                name: "PlayerAbilities");

            migrationBuilder.DropTable(
                name: "PlayerItems");

            migrationBuilder.DropTable(
                name: "Minons");

            migrationBuilder.DropTable(
                name: "Attacks");

            migrationBuilder.DropTable(
                name: "BossAttacks");

            migrationBuilder.DropTable(
                name: "EncounterMightDecks");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Bosses");

            migrationBuilder.DropTable(
                name: "EncounterPlayers");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "FreeCompanies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
