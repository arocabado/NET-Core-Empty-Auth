using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace geoportal_back.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    AuthVersion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdUsrCreacion = table.Column<Guid>(type: "uuid", nullable: true),
                    IdUsrModificacion = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecGrupo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    IdProyecto = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdUsrCreacion = table.Column<Guid>(type: "uuid", nullable: true),
                    IdUsrModificacion = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecGrupo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecGrupo_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiMenu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    IdPadre = table.Column<Guid>(type: "uuid", nullable: true),
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Secuencia = table.Column<int>(type: "integer", nullable: false),
                    PathIcono = table.Column<string>(type: "text", nullable: true),
                    PathPadre = table.Column<string>(type: "text", nullable: false),
                    Accion = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdUsrCreacion = table.Column<Guid>(type: "uuid", nullable: true),
                    IdUsrModificacion = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiMenu_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiMenu_RiMenu_IdPadre",
                        column: x => x.IdPadre,
                        principalTable: "RiMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    IdProyecto = table.Column<Guid>(type: "uuid", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Verificado = table.Column<bool>(type: "boolean", nullable: true),
                    CodigoSecreto = table.Column<string>(type: "text", nullable: true),
                    Firma = table.Column<string>(type: "text", nullable: true),
                    Notificacion = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdUsrCreacion = table.Column<Guid>(type: "uuid", nullable: true),
                    IdUsrModificacion = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Proyectos_IdProyecto",
                        column: x => x.IdProyecto,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecGrupoRiMenu",
                columns: table => new
                {
                    GruposId = table.Column<Guid>(type: "uuid", nullable: false),
                    MenusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecGrupoRiMenu", x => new { x.GruposId, x.MenusId });
                    table.ForeignKey(
                        name: "FK_RecGrupoRiMenu_RecGrupo_GruposId",
                        column: x => x.GruposId,
                        principalTable: "RecGrupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecGrupoRiMenu_RiMenu_MenusId",
                        column: x => x.MenusId,
                        principalTable: "RiMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiModelo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    IdMenu = table.Column<Guid>(type: "uuid", nullable: false),
                    IdProyecto = table.Column<Guid>(type: "uuid", nullable: false),
                    NombreModelo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    ProyectoId = table.Column<Guid>(type: "uuid", nullable: true),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdUsrCreacion = table.Column<Guid>(type: "uuid", nullable: true),
                    IdUsrModificacion = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiModelo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiModelo_Proyectos_ProyectoId",
                        column: x => x.ProyectoId,
                        principalTable: "Proyectos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiModelo_RiMenu_IdMenu",
                        column: x => x.IdMenu,
                        principalTable: "RiMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecGrupoUsuario",
                columns: table => new
                {
                    GruposId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuariosId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecGrupoUsuario", x => new { x.GruposId, x.UsuariosId });
                    table.ForeignKey(
                        name: "FK_RecGrupoUsuario_RecGrupo_GruposId",
                        column: x => x.GruposId,
                        principalTable: "RecGrupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecGrupoUsuario_Usuario_UsuariosId",
                        column: x => x.UsuariosId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiAccesoModelo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    IdGrupo = table.Column<Guid>(type: "uuid", nullable: false),
                    IdModelo = table.Column<Guid>(type: "uuid", nullable: false),
                    Ver = table.Column<bool>(type: "boolean", nullable: false),
                    Crear = table.Column<bool>(type: "boolean", nullable: false),
                    Editar = table.Column<bool>(type: "boolean", nullable: false),
                    Eliminar = table.Column<bool>(type: "boolean", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    IdUsrCreacion = table.Column<Guid>(type: "uuid", nullable: true),
                    IdUsrModificacion = table.Column<Guid>(type: "uuid", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiAccesoModelo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiAccesoModelo_RecGrupo_IdGrupo",
                        column: x => x.IdGrupo,
                        principalTable: "RecGrupo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiAccesoModelo_RiModelo_IdModelo",
                        column: x => x.IdModelo,
                        principalTable: "RiModelo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_Nombre",
                table: "Proyectos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecGrupo_IdProyecto",
                table: "RecGrupo",
                column: "IdProyecto");

            migrationBuilder.CreateIndex(
                name: "IX_RecGrupoRiMenu_MenusId",
                table: "RecGrupoRiMenu",
                column: "MenusId");

            migrationBuilder.CreateIndex(
                name: "IX_RecGrupoUsuario_UsuariosId",
                table: "RecGrupoUsuario",
                column: "UsuariosId");

            migrationBuilder.CreateIndex(
                name: "IX_RiAccesoModelo_IdGrupo",
                table: "RiAccesoModelo",
                column: "IdGrupo");

            migrationBuilder.CreateIndex(
                name: "IX_RiAccesoModelo_IdModelo",
                table: "RiAccesoModelo",
                column: "IdModelo");

            migrationBuilder.CreateIndex(
                name: "IX_RiMenu_IdPadre",
                table: "RiMenu",
                column: "IdPadre");

            migrationBuilder.CreateIndex(
                name: "IX_RiMenu_ProyectoId",
                table: "RiMenu",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_RiModelo_IdMenu",
                table: "RiModelo",
                column: "IdMenu");

            migrationBuilder.CreateIndex(
                name: "IX_RiModelo_ProyectoId",
                table: "RiModelo",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdProyecto",
                table: "Usuario",
                column: "IdProyecto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecGrupoRiMenu");

            migrationBuilder.DropTable(
                name: "RecGrupoUsuario");

            migrationBuilder.DropTable(
                name: "RiAccesoModelo");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "RecGrupo");

            migrationBuilder.DropTable(
                name: "RiModelo");

            migrationBuilder.DropTable(
                name: "RiMenu");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
