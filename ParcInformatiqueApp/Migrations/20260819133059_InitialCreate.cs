using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcInformatiqueApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Localisations",
                columns: table => new
                {
                    IdLocalisation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Batiment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NbBureau = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localisations", x => x.IdLocalisation);
                });

            migrationBuilder.CreateTable(
                name: "Logiciels",
                columns: table => new
                {
                    IdLogiciel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomLogiciel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Licence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logiciels", x => x.IdLogiciel);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    IdService = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomService = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.IdService);
                });

            migrationBuilder.CreateTable(
                name: "TypesEquipements",
                columns: table => new
                {
                    IdType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesEquipements", x => x.IdType);
                });

            migrationBuilder.CreateTable(
                name: "Employes",
                columns: table => new
                {
                    IdEmploye = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Poste = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdService = table.Column<int>(type: "int", nullable: false),
                    ServiceIdService = table.Column<int>(type: "int", nullable: true),
                    IdLocalisation = table.Column<int>(type: "int", nullable: true),
                    LocalisationIdLocalisation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employes", x => x.IdEmploye);
                    table.ForeignKey(
                        name: "FK_Employes_Localisations_LocalisationIdLocalisation",
                        column: x => x.LocalisationIdLocalisation,
                        principalTable: "Localisations",
                        principalColumn: "IdLocalisation");
                    table.ForeignKey(
                        name: "FK_Employes_Services_ServiceIdService",
                        column: x => x.ServiceIdService,
                        principalTable: "Services",
                        principalColumn: "IdService");
                });

            migrationBuilder.CreateTable(
                name: "Equipements",
                columns: table => new
                {
                    IdEquipement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomEquipement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Modele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroSerie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAchat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fournisseur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinGarantie = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Etat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdType = table.Column<int>(type: "int", nullable: false),
                    TypeEquipementIdType = table.Column<int>(type: "int", nullable: true),
                    IdLocalisation = table.Column<int>(type: "int", nullable: false),
                    LocalisationIdLocalisation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipements", x => x.IdEquipement);
                    table.ForeignKey(
                        name: "FK_Equipements_Localisations_LocalisationIdLocalisation",
                        column: x => x.LocalisationIdLocalisation,
                        principalTable: "Localisations",
                        principalColumn: "IdLocalisation");
                    table.ForeignKey(
                        name: "FK_Equipements_TypesEquipements_TypeEquipementIdType",
                        column: x => x.TypeEquipementIdType,
                        principalTable: "TypesEquipements",
                        principalColumn: "IdType");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEmploye = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Employes_IdEmploye",
                        column: x => x.IdEmploye,
                        principalTable: "Employes",
                        principalColumn: "IdEmploye",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Affectations",
                columns: table => new
                {
                    IdAffectation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAffectation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRetour = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEmploye = table.Column<int>(type: "int", nullable: false),
                    EmployeIdEmploye = table.Column<int>(type: "int", nullable: true),
                    IdEquipement = table.Column<int>(type: "int", nullable: false),
                    EquipementIdEquipement = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affectations", x => x.IdAffectation);
                    table.ForeignKey(
                        name: "FK_Affectations_Employes_EmployeIdEmploye",
                        column: x => x.EmployeIdEmploye,
                        principalTable: "Employes",
                        principalColumn: "IdEmploye");
                    table.ForeignKey(
                        name: "FK_Affectations_Equipements_EquipementIdEquipement",
                        column: x => x.EquipementIdEquipement,
                        principalTable: "Equipements",
                        principalColumn: "IdEquipement");
                });

            migrationBuilder.CreateTable(
                name: "InstallationsLogiciels",
                columns: table => new
                {
                    IdInstallation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateInstallation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VersionInstallee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdLogiciel = table.Column<int>(type: "int", nullable: false),
                    LogicielIdLogiciel = table.Column<int>(type: "int", nullable: true),
                    IdEquipement = table.Column<int>(type: "int", nullable: false),
                    EquipementIdEquipement = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstallationsLogiciels", x => x.IdInstallation);
                    table.ForeignKey(
                        name: "FK_InstallationsLogiciels_Equipements_EquipementIdEquipement",
                        column: x => x.EquipementIdEquipement,
                        principalTable: "Equipements",
                        principalColumn: "IdEquipement");
                    table.ForeignKey(
                        name: "FK_InstallationsLogiciels_Logiciels_LogicielIdLogiciel",
                        column: x => x.LogicielIdLogiciel,
                        principalTable: "Logiciels",
                        principalColumn: "IdLogiciel");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    IdTicket = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NomTicket = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diagnostic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeIntervention = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Priorite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionRealisee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCloture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdEquipement = table.Column<int>(type: "int", nullable: false),
                    IdUserCreateur = table.Column<int>(type: "int", nullable: false),
                    IdUserTraiteur = table.Column<int>(type: "int", nullable: true),
                    EquipementIdEquipement = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.IdTicket);
                    table.ForeignKey(
                        name: "FK_Tickets_Equipements_EquipementIdEquipement",
                        column: x => x.EquipementIdEquipement,
                        principalTable: "Equipements",
                        principalColumn: "IdEquipement");
                    table.ForeignKey(
                        name: "FK_Tickets_Equipements_IdEquipement",
                        column: x => x.IdEquipement,
                        principalTable: "Equipements",
                        principalColumn: "IdEquipement",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_IdUserCreateur",
                        column: x => x.IdUserCreateur,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_IdUserTraiteur",
                        column: x => x.IdUserTraiteur,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_EmployeIdEmploye",
                table: "Affectations",
                column: "EmployeIdEmploye");

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_EquipementIdEquipement",
                table: "Affectations",
                column: "EquipementIdEquipement");

            migrationBuilder.CreateIndex(
                name: "IX_Employes_LocalisationIdLocalisation",
                table: "Employes",
                column: "LocalisationIdLocalisation");

            migrationBuilder.CreateIndex(
                name: "IX_Employes_ServiceIdService",
                table: "Employes",
                column: "ServiceIdService");

            migrationBuilder.CreateIndex(
                name: "IX_Equipements_LocalisationIdLocalisation",
                table: "Equipements",
                column: "LocalisationIdLocalisation");

            migrationBuilder.CreateIndex(
                name: "IX_Equipements_TypeEquipementIdType",
                table: "Equipements",
                column: "TypeEquipementIdType");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationsLogiciels_EquipementIdEquipement",
                table: "InstallationsLogiciels",
                column: "EquipementIdEquipement");

            migrationBuilder.CreateIndex(
                name: "IX_InstallationsLogiciels_LogicielIdLogiciel",
                table: "InstallationsLogiciels",
                column: "LogicielIdLogiciel");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EquipementIdEquipement",
                table: "Tickets",
                column: "EquipementIdEquipement");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdEquipement",
                table: "Tickets",
                column: "IdEquipement");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdUserCreateur",
                table: "Tickets",
                column: "IdUserCreateur");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IdUserTraiteur",
                table: "Tickets",
                column: "IdUserTraiteur");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdEmploye",
                table: "Users",
                column: "IdEmploye",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Affectations");

            migrationBuilder.DropTable(
                name: "InstallationsLogiciels");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Logiciels");

            migrationBuilder.DropTable(
                name: "Equipements");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "TypesEquipements");

            migrationBuilder.DropTable(
                name: "Employes");

            migrationBuilder.DropTable(
                name: "Localisations");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
