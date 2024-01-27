using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AkaShi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileExtensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileExtensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Frameworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VersionName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frameworks", x => x.Id);
                    table.UniqueConstraint("AK_Frameworks_VersionName", x => x.VersionName);
                });

            migrationBuilder.CreateTable(
                name: "LibraryArchiveHashes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryArchiveHashes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Email", x => x.Email);
                    table.UniqueConstraint("AK_Users_Username", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    URL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    FileExtensionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_FileExtensions_FileExtensionId",
                        column: x => x.FileExtensionId,
                        principalTable: "FileExtensions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DownloadsCount = table.Column<int>(type: "int", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ProjectWebsiteURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LogoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                    table.UniqueConstraint("AK_Libraries_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Libraries_Images_LogoId",
                        column: x => x.LogoId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Libraries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DownloadsCount = table.Column<int>(type: "int", nullable: false),
                    LibraryId = table.Column<int>(type: "int", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsageContent = table.Column<string>(type: "nvarchar(max)", maxLength: 6000, nullable: true),
                    SourceRepositoryURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LicenseURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileExtensionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryVersions_FileExtensions_FileExtensionId",
                        column: x => x.FileExtensionId,
                        principalTable: "FileExtensions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryVersions_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryVersionDependencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrameworkId = table.Column<int>(type: "int", nullable: true),
                    LibraryVersionId = table.Column<int>(type: "int", nullable: true),
                    DependencyLibraryId = table.Column<int>(type: "int", nullable: true),
                    SupportedVersions = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryVersionDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryVersionDependencies_Frameworks_FrameworkId",
                        column: x => x.FrameworkId,
                        principalTable: "Frameworks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryVersionDependencies_Libraries_DependencyLibraryId",
                        column: x => x.DependencyLibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryVersionDependencies_LibraryVersions_LibraryVersionId",
                        column: x => x.LibraryVersionId,
                        principalTable: "LibraryVersions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LibraryVersionSupportedFrameworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryVersionId = table.Column<int>(type: "int", nullable: true),
                    FrameworkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryVersionSupportedFrameworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryVersionSupportedFrameworks_Frameworks_FrameworkId",
                        column: x => x.FrameworkId,
                        principalTable: "Frameworks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LibraryVersionSupportedFrameworks_LibraryVersions_LibraryVersionId",
                        column: x => x.LibraryVersionId,
                        principalTable: "LibraryVersions",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "FileExtensions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, ".png" },
                    { 2, ".jpg" },
                    { 3, ".jpeg" },
                    { 4, ".webp" },
                    { 5, ".zip" },
                    { 6, ".rar" },
                    { 7, ".tar" },
                    { 8, ".gzip" },
                    { 9, ".7z" }
                });

            migrationBuilder.InsertData(
                table: "Frameworks",
                columns: new[] { "Id", "ProductName", "VersionName" },
                values: new object[,]
                {
                    { 1, ".NET", "net8.0" },
                    { 2, ".NET", "net7.0" },
                    { 3, ".NET", "net6.0" },
                    { 4, ".NET", "net5.0" },
                    { 5, ".NET Core", "netcoreapp3.1" },
                    { 6, ".NET Core", "netcoreapp3.0" },
                    { 7, ".NET Core", "netcoreapp2.2" },
                    { 8, ".NET Core", "netcoreapp2.1" },
                    { 9, ".NET Core", "netcoreapp2.0" },
                    { 10, ".NET Core", "netcoreapp1.1" },
                    { 11, ".NET Core", "netcoreapp1.0" },
                    { 12, ".NET Standard", "netstandard2.1" },
                    { 13, ".NET Standard", "netstandard2.0" },
                    { 14, ".NET Standard", "netstandard1.6" },
                    { 15, ".NET Standard", "netstandard1.5" },
                    { 16, ".NET Standard", "netstandard1.4" },
                    { 17, ".NET Standard", "netstandard1.3" },
                    { 18, ".NET Standard", "netstandard1.2" },
                    { 19, ".NET Standard", "netstandard1.1" },
                    { 20, ".NET Standard", "netstandard1.0" },
                    { 21, ".NET Framework", "net481" },
                    { 22, ".NET Framework", "net48" },
                    { 23, ".NET Framework", "net472" },
                    { 24, ".NET Framework", "net471" },
                    { 25, ".NET Framework", "net47" },
                    { 26, ".NET Framework", "net462" },
                    { 27, ".NET Framework", "net461" },
                    { 28, ".NET Framework", "net46" },
                    { 29, ".NET Framework", "net452" },
                    { 30, ".NET Framework", "net451" },
                    { 31, ".NET Framework", "net45" },
                    { 32, ".NET Framework", "net40" },
                    { 33, ".NET Framework", "net35" },
                    { 34, ".NET Framework", "net30" },
                    { 35, ".NET Framework", "net20" }
                });

            migrationBuilder.InsertData(
                table: "LibraryArchiveHashes",
                columns: new[] { "Id", "Hash" },
                values: new object[,]
                {
                    { 1, "34aa94f00de4a65d18e4cd8ce7d5b625406e85dbe194cb2005b3749d9ef82618" },
                    { 2, "3253cb184dc494d8a604eb8ff76e88a0a1a5a49e346c91cbcf039d7a1ec196a5" },
                    { 3, "ff96513790b111f4926b4d8246542c6aac41a05d3010894bec0724c3614da998" },
                    { 4, "1570bb5a455626ed4c6ba179f35119f345fd1bb9eb9db2ceb9adac00368c500f" },
                    { 5, "ba2d8c8d376a0c60861d89ee396f62ddc447f0c3119d03d46cd16c7486e01bd7" },
                    { 6, "f56cd9c1273108997f7803bf0075e3099e3af7e24d0a087e419dddd24436cd27" },
                    { 7, "9a0fc28bb4442a60a0e16fcff27af6e2f5e12d62c1567d64d8a8d3d776bf8c6c" },
                    { 8, "85bd54966d28bb38a5cdfcad3c25bc05299e1f08a9492a788e76d48c8d45b9ba" },
                    { 9, "1c0e2925f4634fb73648ace5a62e0ca562fd754d4d10735d9bffe30b91568489" },
                    { 10, "98a07c78c8af32851f28e947fbe869235ccb819c9f14f44439cf54d2d21f21f9" },
                    { 11, "b9505f26ec6388cbdcdd31a229926c0efc817e9578e49cd2d7b9b51a7d79a6de" },
                    { 12, "868422676ad6989500dbdadff4f21db66be08801291b7b1393b52ad184866f12" },
                    { 13, "2d844839badabac5f37fd4314b06fccc211383deb4d2edc0c3e0819886067162" },
                    { 14, "f1254209369dd49b375f5b3885d8f166642a9c8a78fd024cf6fe104ae22ad9ad" },
                    { 15, "ac9ee42dde050db428ea308137357e6727239c020b4173b3fdfbc25b8e1ef95c" },
                    { 16, "bfee44d80ef2f475f90dea67faa01e7ac3c7f5b23ab49be4495e04c276b50171" },
                    { 17, "a3a6be3caae06bb2d987c1d7923c5faf635c7378fa01559d1289911399f60eb7" },
                    { 18, "7f57805196b7010aba64cc2a95c4e29b1a6ca17fc21d4ade908fc9ce98ac3b95" },
                    { 19, "a4d35369d71ea6ff3eb5d30a1d32cc95ba120588e6cb75f15ea3449f113bfd1e" },
                    { 20, "33db92ef95a4576f8fd132e6333f59b9b377ce2be0bf421cc68ee394f7c74bf5" },
                    { 21, "dfc8bd43579391eb4e6f2d27329156075a770fbcb767ae94330003eb87cf2005" },
                    { 22, "acf1af2af0e51f21e49532c374c6790dd076a0d6387d54b4010bccbf012ed0d1" },
                    { 23, "0bc87887a8cd1d42192cfcf67bdce386d96551e61b8c8ec7a4b88d4f783b750e" },
                    { 24, "fb7ec3d3f644ebd71fbefadfebd82faf3e27097b4278f3235220e7795c099d82" },
                    { 25, "eeac901ea7e5399812c178efafa29d39838fbd9c7b51b9450646c5a3c707fabe" },
                    { 26, "6fa329180b5e3bb4be000d9334718f45aecbd416ef906155a0a2fb96d94d4977" },
                    { 27, "9ac09e6717275310136c93b3173dc51ffef9a2700c183a80dec12956b4ce59c8" },
                    { 28, "838cf33badb72795a5ad26de7d51d5e892f456c5812a47babf31cdc491d10da7" },
                    { 29, "8155cb8110b52905e8a0cd11ec4811bbbca127c7453ba82a82553b22c6eb90a8" },
                    { 30, "fa51eb32fbc001048a829fee72bb1830f4955a2acd5c086b9f63b13bd2930a47" },
                    { 31, "13216bd6037c4787d88bf6d46e9f6c4f4b6d2b265e48f51bea10d1a90360a43d" },
                    { 32, "2515074b08a2241e2c17673354de595284aff62147c63cd1ed0d4d8356d51f75" },
                    { 33, "a8fcf75e94084dbe94a031d2a73c768f12187bb7ebbebb9211db6176e75ae9ae" },
                    { 34, "7ca15ca0f5819442e33be7e43b65e8f6582799d0cda64ea2873d53fabf44a20a" },
                    { 35, "ab5c7d91ae090e1d87316f7b188382332237bec99c26ce81e6bbe4fd3861137d" },
                    { 36, "e1c5cc0b2732aae8046ded92ada5c6ab60b586e63c01bdcc671634c653a948d9" },
                    { 37, "eade19bfc4cb8e3f5ca39606c3d7331a1df6c2c9c137cfd8ace7dbe5e3653b3f" },
                    { 38, "35bbc676d2f9e77b2078e7e6ce37dbc18e31f7c735dda122c84134179865d3b2" },
                    { 39, "77094bcb4144102e7d9d019797be686a387ef7dbf60ad645daa4896e43bcef63" },
                    { 40, "adf8e54b1de2874c533a8b90962de935633bc3ee4acf392fb35e4eee8e7bff58" },
                    { 41, "4f4281dc1439b9ce794d42520b31f2498ea80a9d05922b238f00b6cf3b9d3ece" },
                    { 42, "c9675357be54847e8730b0227b94b04ea2bcd8d4206bc7bb2dfd7c6e4e158e49" },
                    { 43, "290e3183c06003db4792b170e89bae1c1cfeb26da27364b8a7dc5716442c55fc" },
                    { 44, "938b8ef2309ca412b926e8e840292f319fcf610695f6bf7b421672c6ccfa9776" },
                    { 45, "3f1da0f0a2d394153bf612f4c9d68b2409909f847e57d5928407b255bdfb1599" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, "test1@gmail.com", "8Utk9BKzTYUtkB1pHtDr/9qNb1wdr+7yzQYSPloXkL4=", "GgxGb1115NitZ2XV9RnbyCt8NDs3+IUA7F5kAFOTsw0=", "testUser1" },
                    { 2, "test2@gmail.com", "pGNmwm1nJqqZuDHEJvNdCECGX6OD6RMVbdvoEUjd4BI=", "GgxGb1115NitZ2XV9RnbyCt8NDs3+IUA7F5kAFOTsw0=", "testUser2" },
                    { 3, "test3@gmail.com", "TkQ70z2eDjuwyWlq47jwvvdbfezkdYzg4WsQw48Ve0E=", "GgxGb1115NitZ2XV9RnbyCt8NDs3+IUA7F5kAFOTsw0=", "testUser3" },
                    { 4, "test4@gmail.com", "DLE7RzwYhIhNx+877XoqYBbJ++LMF5FA1qwo3+bd3cc=", "GgxGb1115NitZ2XV9RnbyCt8NDs3+IUA7F5kAFOTsw0=", "testUser4" },
                    { 5, "test5@gmail.com", "UiQOoV2nh4Hu/VYjncYEjHr/+Xmh1suvdIWwT2O3C1o=", "GgxGb1115NitZ2XV9RnbyCt8NDs3+IUA7F5kAFOTsw0=", "testUser5" }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "FileExtensionId", "Timestamp", "URL" },
                values: new object[,]
                {
                    { 1, 1, 1705650742L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/default-library-version-logo_1705650742.png" },
                    { 2, 1, 1705650743L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser1/Newtonsoft.Json/logo_1705650743.png" },
                    { 3, 1, 1705650744L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser1/AutoMapper/logo_1705650744.png" },
                    { 4, 1, 1705650745L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser2/Microsoft.Data.SqlClient/logo_1705650745.png" },
                    { 5, 1, 1705650746L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser2/Microsoft.EntityFrameworkCore/logo_1705650746.png" },
                    { 6, 1, 1705650747L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser2/Microsoft.Extensions.DependencyInjection/logo_1705650747.png" },
                    { 7, 1, 1705650748L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser2/System.Windows.Extensions/logo_1705650748.png" },
                    { 8, 1, 1705650749L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser3/FluentAssertions/logo_1705650749.png" },
                    { 9, 1, 1705650750L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser3/Serilog/logo_1705650750.png" },
                    { 10, 1, 1705650751L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser4/AWSSDK.Core/logo_1705650751.png" },
                    { 11, 1, 1705650752L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser4/Azure.Core/logo_1705650752.png" },
                    { 12, 1, 1705650753L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser4/FluentValidation/logo_1705650753.png" },
                    { 13, 1, 1705650754L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser4/Moq/logo_1705650754.png" },
                    { 14, 1, 1705650755L, "https://storage.googleapis.com/akashi-a5fff.appspot.com/testUser4/Polly/logo_1705650755.png" }
                });

            migrationBuilder.InsertData(
                table: "Libraries",
                columns: new[] { "Id", "DownloadsCount", "LastUpdateTime", "LogoId", "Name", "ProjectWebsiteURL", "ShortDescription", "Tags", "UserId" },
                values: new object[,]
                {
                    { 1, 1152633305, new DateTime(2023, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Newtonsoft.Json", "https://www.newtonsoft.com/json", "Json.NET is a popular high-performance JSON framework for .NET", "json", 1 },
                    { 2, 480217228, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Microsoft.Extensions.DependencyInjection", "https://dotnet.microsoft.com/en-us/", "Supports the dependency injection (DI) software design pattern which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.", "", 2 },
                    { 3, 50223257, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Microsoft.EntityFrameworkCore", "https://learn.microsoft.com/en-us/ef/core/", "Entity Framework Core is a modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations.", "Entity Framework Core entity-framework-core EF O/RM Data EntityFramework EntityFrameworkCore EFCore", 2 },
                    { 4, 200910090, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "System.IdentityModel.Tokens.Jwt", "https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet", "Includes types that provide support for creating, serializing and validating JSON Web Tokens.", ".NET Windows Authentication Identity Json Web Token", 1 },
                    { 5, 75616379, new DateTime(2023, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Serilog", "https://serilog.net/", "Serilog is a diagnostic logging library for .NET applications. It is easy to set up, has a clean API, and runs on all recent .NET platforms.", "serilog logging semantic structured", 3 },
                    { 6, 215117416, new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Azure.Core", "https://github.com/Azure/azure-sdk-for-net/blob/Azure.Core_1.36.0/sdk/core/Azure.Core/README.md", "Azure.Core provides shared primitives, abstractions, and helpers for modern .NET Azure SDK client libraries.", "Microsoft Azure Client Pipeline", 4 },
                    { 7, 166920774, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "System.Windows.Extensions", "https://dotnet.microsoft.com/en-us/", "Provides miscellaneous Windows-specific types", "", 2 },
                    { 8, 343898732, new DateTime(2022, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Castle.Core", "https://www.castleproject.org/", "Castle Core, including DynamicProxy, Logging Abstractions and DictionaryAdapter", "castle dynamicproxy dynamic proxy dynamicproxy2 dictionaryadapter emailsender", 4 },
                    { 9, 15965074, new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "AWSSDK.Core", "https://github.com/aws/aws-sdk-net/", "The Amazon Web Services SDK for .NET - Core Runtime", "AWS Amazon cloud aws-sdk-v3", 4 },
                    { 10, 176676652, new DateTime(2022, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "Moq", "https://github.com/devlooped/moq", "The most popular and friendly mocking library for .NET", "moq tdd mocking mocks unittesting agile unittest", 4 },
                    { 11, 227957697, new DateTime(2023, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "AutoMapper", "https://automapper.org/", "AutoMapper is a simple little library built to solve a deceptively complex problem - getting rid of code that mapped one object to another.", "", 1 },
                    { 12, 132580062, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "Polly", "https://github.com/App-vNext/Polly", "Polly is a .NET resilience and transient-fault-handling library.", "Polly Exception Handling Resilience Transient Fault Policy Circuit Breaker CircuitBreaker Retry Wait Cache Cache-aside Bulkhead Fallback Timeout Throttle", 4 },
                    { 13, 115367575, new DateTime(2023, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Microsoft.Data.SqlClient", "https://github.com/dotnet/SqlClient", "The current data provider for SQL Server and Azure SQL databases. This has replaced System.Data.SqlClient.", "sqlclient microsoft.data.sqlclient", 2 },
                    { 14, 112238387, new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "FluentAssertions", "https://fluentassertions.com/", "A very extensive set of extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style unit tests.", "MSTest2 xUnit NUnit MSpec TDD BDD Fluent netstandard uwp", 3 },
                    { 15, 36023632, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "FluentValidation", "https://docs.fluentvalidation.net/en/latest/", "FluentValidation is validation library for .NET that uses a fluent interface and lambda expressions for building strongly-typed validation rules.", "", 4 }
                });

            migrationBuilder.InsertData(
                table: "LibraryVersions",
                columns: new[] { "Id", "DownloadsCount", "FileExtensionId", "LastUpdateTime", "LibraryId", "LicenseURL", "Name", "SourceRepositoryURL", "UsageContent" },
                values: new object[,]
                {
                    { 1, 85784960, 6, new DateTime(2023, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "https://licenses.nuget.org/MIT", "13.0.3", "https://github.com/JamesNK/Newtonsoft.Json", "Serialize JSON\nProduct product = new Product();\nproduct.Name = \"Apple\";\nproduct.Expiry = new DateTime(2008, 12, 28);\nproduct.Sizes = new string[] { \"Small\" };\n\nstring json = JsonConvert.SerializeObject(product);\n// {\n//   \"Name\": \"Apple\",\n//   \"Expiry\": \"2008-12-28T00:00:00\",\n//   \"Sizes\": [\n//     \"Small\"\n//   ]\n// }\nDeserialize JSON\nstring json = @\"{\n  'Name': 'Bad Boys',\n  'ReleaseDate': '1995-4-7T00:00:00',\n  'Genres': [\n    'Action',\n    'Comedy'\n  ]\n}\";\n\nMovie m = JsonConvert.DeserializeObject<Movie>(json);\n\nstring name = m.Name;\n// Bad Boys\nLINQ to JSON\nJArray array = new JArray();\narray.Add(\"Manual text\");\narray.Add(new DateTime(2000, 5, 23));\n\nJObject o = new JObject();\no[\"MyArray\"] = array;\n\nstring json = o.ToString();\n// {\n//   \"MyArray\": [\n//     \"Manual text\",\n//     \"2000-05-23T00:00:00\"\n//   ]\n// }" },
                    { 2, 456940068, 6, new DateTime(2018, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "https://raw.githubusercontent.com/JamesNK/Newtonsoft.Json/master/LICENSE.md", "11.0.2", "https://github.com/JamesNK/Newtonsoft.Json", "" },
                    { 3, 609908277, 6, new DateTime(2016, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "https://raw.githubusercontent.com/JamesNK/Newtonsoft.Json/master/LICENSE.md", "9.0.1", "", "" },
                    { 4, 4395942, 6, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "https://licenses.nuget.org/MIT", "7.0.3", "https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet", "" },
                    { 5, 28078564, 6, new DateTime(2022, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "https://licenses.nuget.org/MIT", "6.24.0", "https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet", "" },
                    { 6, 168435584, 6, new DateTime(2020, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "https://licenses.nuget.org/MIT", "6.8.0", "https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet", "" },
                    { 7, 36002692, 6, new DateTime(2023, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "https://licenses.nuget.org/MIT", "12.0.1", "https://github.com/AutoMapper/AutoMapper", "First, configure AutoMapper to know what types you want to map, in the startup of your application:\n\nvar configuration = new MapperConfiguration(cfg => \n{\n    cfg.CreateMap<Foo, FooDto>();\n    cfg.CreateMap<Bar, BarDto>();\n});\n// only during development, validate your mappings; remove it before release\n#if DEBUG\nconfiguration.AssertConfigurationIsValid();\n#endif\n// use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself\nvar mapper = configuration.CreateMapper();\nThen in your application code, execute the mappings:\n\nvar fooDto = mapper.Map<FooDto>(foo);\nvar barDto = mapper.Map<BarDto>(bar);" },
                    { 8, 108550029, 6, new DateTime(2020, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "https://licenses.nuget.org/MIT", "10.1.1", "https://github.com/AutoMapper/AutoMapper", "" },
                    { 9, 83404976, 6, new DateTime(2019, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt", "9.0.0", "https://github.com/AutoMapper/AutoMapper", "" },
                    { 10, 5924731, 6, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "https://licenses.nuget.org/MIT", "8.0.0", "https://github.com/dotnet/runtime", "Provides an implementation of the DI interfaces found in the Microsoft.Extensions.DependencyInjection.Abstractions package.\n\nServiceCollection services = new ();\nservices.AddSingleton<IMessageWriter, MessageWriter>();\nusing ServiceProvider provider = services.BuildServiceProvider();\n\n// The code below, following the IoC pattern, is typically only aware of the IMessageWriter interface, not the implementation.\nIMessageWriter messageWriter = provider.GetService<IMessageWriter>()!;\nmessageWriter.Write(\"Hello\");\n\npublic interface IMessageWriter\n{\n    void Write(string message);\n}\n\ninternal class MessageWriter : IMessageWriter\n{\n    public void Write(string message)\n    {\n        Console.WriteLine($\"MessageWriter.Write(message: \\\"{message}\\\")\");\n    }\n}" },
                    { 11, 135695429, 6, new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "https://licenses.nuget.org/MIT", "7.0.0", "https://github.com/dotnet/runtime", "" },
                    { 12, 338597068, 6, new DateTime(2021, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "https://licenses.nuget.org/MIT", "6.0.0", "https://github.com/dotnet/runtime", "" },
                    { 13, 1834065, 6, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "https://licenses.nuget.org/MIT", "8.0.0", "https://github.com/dotnet/efcore", "" },
                    { 14, 15346288, 6, new DateTime(2023, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "https://licenses.nuget.org/MIT", "7.0.5", "https://github.com/dotnet/efcore", "" },
                    { 15, 33042904, 6, new DateTime(2022, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "https://licenses.nuget.org/MIT", "6.0.7", "https://github.com/dotnet/efcore", "" },
                    { 16, 197995, 6, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "https://licenses.nuget.org/MIT", "8.0.0", "https://github.com/dotnet/runtime", "" },
                    { 17, 26190445, 6, new DateTime(2022, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "https://licenses.nuget.org/MIT", "7.0.0", "https://github.com/dotnet/runtime", "" },
                    { 18, 140532334, 6, new DateTime(2021, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "https://licenses.nuget.org/MIT", "6.0.0", "https://github.com/dotnet/runtime", "" },
                    { 19, 1932616, 6, new DateTime(2023, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "https://licenses.nuget.org/MIT", "5.1.2", "https://github.com/dotnet/sqlclient", "Commonly Used Types:\nMicrosoft.Data.SqlClient.SqlConnection\nMicrosoft.Data.SqlClient.SqlException\nMicrosoft.Data.SqlClient.SqlParameter\nMicrosoft.Data.SqlClient.SqlDataReader\nMicrosoft.Data.SqlClient.SqlCommand\nMicrosoft.Data.SqlClient.SqlTransaction\nMicrosoft.Data.SqlClient.SqlParameterCollection\nMicrosoft.Data.SqlClient.SqlClientFactory" },
                    { 20, 33102569, 6, new DateTime(2022, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "https://licenses.nuget.org/MIT", "5.0.1", "https://github.com/dotnet/sqlclient", "Commonly Used Types:\nMicrosoft.Data.SqlClient.SqlConnection\nMicrosoft.Data.SqlClient.SqlException\nMicrosoft.Data.SqlClient.SqlParameter\nMicrosoft.Data.SqlClient.SqlDataReader\nMicrosoft.Data.SqlClient.SqlCommand\nMicrosoft.Data.SqlClient.SqlTransaction\nMicrosoft.Data.SqlClient.SqlParameterCollection\nMicrosoft.Data.SqlClient.SqlClientFactory" },
                    { 21, 80332390, 6, new DateTime(2021, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "https://licenses.nuget.org/MIT", "2.1.4", "https://github.com/dotnet/sqlclient", "Commonly Used Types:\nMicrosoft.Data.SqlClient.SqlConnection\nMicrosoft.Data.SqlClient.SqlException\nMicrosoft.Data.SqlClient.SqlParameter\nMicrosoft.Data.SqlClient.SqlDataReader\nMicrosoft.Data.SqlClient.SqlCommand\nMicrosoft.Data.SqlClient.SqlTransaction\nMicrosoft.Data.SqlClient.SqlParameterCollection\nMicrosoft.Data.SqlClient.SqlClientFactory" },
                    { 22, 2799817, 6, new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "https://licenses.nuget.org/Apache-2.0", "3.1.1", "https://github.com/serilog/serilog", "Like many other libraries for .NET, Serilog provides diagnostic logging to files, the console, and many other outputs.\n\nusing var log = new LoggerConfiguration()\n    .WriteTo.Console()\n    .WriteTo.File(\"log.txt\")\n    .CreateLogger();\n\nlog.Information(\"Hello, Serilog!\");\nUnlike other logging libraries, Serilog is built from the ground up to record structured event data.\n\nvar position = new { Latitude = 25, Longitude = 134 };\nvar elapsedMs = 34;\n\nlog.Information(\"Processed {@Position} in {Elapsed} ms\", position, elapsedMs);\nSerilog uses message templates, a simple DSL that extends .NET format strings with named as well as positional parameters. Instead of formatting events immediately into text, Serilog captures the values associated with each named parameter.\n\nThe example above records two properties, Position and Elapsed, in the log event. The @ operator in front of Position tells Serilog to serialize the object passed in, rather than convert it using ToString(). Serilog's deep and rich support for structured event data opens up a huge range of diagnostic possibilities not available when using traditional loggers.\n\nRendered into JSON format for example, these properties appear alongside the timestamp, level, and message like:\n\n{\"Position\": {\"Latitude\": 25, \"Longitude\": 134}, \"Elapsed\": 34}\nBack-ends that are capable of recording structured event data make log searches and analysis possible without log parsing or regular expressions.\n\nSupporting structured data doesn't mean giving up text: when Serilog writes events to files or the console, the template and properties are rendered into friendly human-readable text just like a traditional logging library would produce:\n\n09:14:22 [INF] Processed {\"Latitude\": 25, \"Longitude\": 134} in 34 ms." },
                    { 23, 10419507, 6, new DateTime(2023, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "https://licenses.nuget.org/Apache-2.0", "3.0.1", "https://github.com/serilog/serilog", "Like many other libraries for .NET, Serilog provides diagnostic logging to files, the console, and many other outputs.\n\nvar log = new LoggerConfiguration()\n    .WriteTo.Console()\n    .WriteTo.File(\"log.txt\")\n    .CreateLogger();\n\nlog.Information(\"Hello, Serilog!\");\nUnlike other logging libraries, Serilog is built from the ground up to record structured event data.\n\nvar position = new { Latitude = 25, Longitude = 134 };\nvar elapsedMs = 34;\n\nlog.Information(\"Processed {@Position} in {Elapsed} ms\", position, elapsedMs);\nSerilog uses message templates, a simple DSL that extends .NET format strings with named as well as positional parameters. Instead of formatting events immediately into text, Serilog captures the values associated with each named parameter.\n\nThe example above records two properties, Position and Elapsed, in the log event. The @ operator in front of Position tells Serilog to serialize the object passed in, rather than convert it using ToString(). Serilog's deep and rich support for structured event data opens up a huge range of diagnostic possibilities not available when using traditional loggers.\n\nRendered into JSON format for example, these properties appear alongside the timestamp, level, and message like:\n\n{\"Position\": {\"Latitude\": 25, \"Longitude\": 134}, \"Elapsed\": 34}\nBack-ends that are capable of recording structured event data make log searches and analysis possible without log parsing or regular expressions.\n\nSupporting structured data doesn't mean giving up text: when Serilog writes events to files or the console, the template and properties are rendered into friendly human-readable text just like a traditional logging library would produce:\n\n09:14:22 [INF] Processed {\"Latitude\": 25, \"Longitude\": 134} in 34 ms." },
                    { 24, 62397055, 6, new DateTime(2022, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "https://licenses.nuget.org/Apache-2.0", "2.12.0", "https://github.com/serilog/serilog", "" },
                    { 25, 8740195, 6, new DateTime(2023, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "https://licenses.nuget.org/Apache-2.0", "6.12.0", "https://github.com/fluentassertions/fluentassertions", "" },
                    { 26, 26902239, 6, new DateTime(2022, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "https://licenses.nuget.org/Apache-2.0", "6.7.0", "https://github.com/fluentassertions/fluentassertions", "" },
                    { 27, 76595953, 6, new DateTime(2020, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "https://licenses.nuget.org/Apache-2.0", "5.10.3", "https://github.com/fluentassertions/fluentassertions", "" },
                    { 28, 4478813, 6, new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "https://licenses.nuget.org/MIT", "1.36.0", "https://github.com/Azure/azure-sdk-for-net", "Examples\nNOTE: Samples in this file apply only to packages that follow Azure SDK Design Guidelines. Names of such packages usually start with Azure.\n\nConfiguring Service Clients Using ClientOptions\nAzure SDK client libraries typically expose one or more service client types that are the main starting points for calling corresponding Azure services. You can easily find these client types as their names end with the word Client. For example, BlockBlobClient can be used to call blob storage service, and KeyClient can be used to access Key Vault service cryptographic keys.\n\nThese client types can be instantiated by calling a simple constructor, or its overload that takes various configuration options. These options are passed as a parameter that extends ClientOptions class exposed by Azure.Core. Various service specific options are usually added to its subclasses, but a set of SDK-wide options are available directly on ClientOptions.\n\nSecretClientOptions options = new SecretClientOptions()\n{\n    Retry =\n    {\n        Delay = TimeSpan.FromSeconds(2),\n        MaxRetries = 10,\n        Mode = RetryMode.Fixed\n    },\n    Diagnostics =\n    {\n        IsLoggingContentEnabled = true,\n        ApplicationId = \"myApplicationId\"\n    }\n};\n\nSecretClient client = new SecretClient(new Uri(\"http://example.com\"), new DefaultAzureCredential(), options);\nMore on client configuration in client configuration samples.\n\nAccessing HTTP Response Details Using Response<T>\nService clients have methods that can be used to call Azure services. We refer to these client methods service methods. Service methods return a shared Azure.Core type Response<T> (in rare cases its non-generic sibling, a raw Response). This type provides access to both the deserialized result of the service call, and to the details of the HTTP response returned from the server.\n\n// create a client\nvar client = new SecretClient(new Uri(\"http://example.com\"), new DefaultAzureCredential());\n\n// call a service method, which returns Response<T>\nResponse<KeyVaultSecret> response = await client.GetSecretAsync(\"SecretName\");\n\n// Response<T> has two main accessors.\n// Value property for accessing the deserialized result of the call\nKeyVaultSecret secret = response.Value;\n\n// .. and GetRawResponse method for accessing all the details of the HTTP response\nResponse http = response.GetRawResponse();\n\n// for example, you can access HTTP status\nint status = http.Status;\n\n// or the headers\nforeach (HttpHeader header in http.Headers)\n{\n    Console.WriteLine($\"{header.Name} {header.Value}\");\n}\nMore on response types in response samples.\n\nSetting up console logging\nTo create an Azure SDK log listener that outputs messages to console use AzureEventSourceListener.CreateConsoleLogger method.\n\n// Setup a listener to monitor logged events.\nusing AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();\nMore on logging in diagnostics samples.\n\nReporting Errors RequestFailedException\nWhen a service call fails Azure.RequestFailedException would get thrown. The exception type provides a Status property with an HTTP status code and an ErrorCode property with a service-specific error code.\n\ntry\n{\n    KeyVaultSecret secret = client.GetSecret(\"NonexistentSecret\");\n}\n// handle exception with status code 404\ncatch (RequestFailedException e) when (e.Status == 404)\n{\n    // handle not found error\n    Console.WriteLine(\"ErrorCode \" + e.ErrorCode);\n}\nMore on handling responses in response samples.\n\nConsuming Service Methods Returning AsyncPageable<T>\nIf a service call returns multiple values in pages, it would return Pageable<T>/AsyncPageable<T> as a result. You can iterate over AsyncPageable directly or in pages.\n\n// call a service method, which returns AsyncPageable<T>\nAsyncPageable<SecretProperties> allSecretProperties = client.GetPropertiesOfSecretsAsync();\n\nawait foreach (SecretProperties secretProperties in allSecretProperties)\n{\n    Console.WriteLine(secretProperties.Name);\n}\nFor more information on paged responses, see Pagination with the Azure SDK for .NET.\n\nConsuming Long-Running Operations Using Operation<T>\nSome operations take long time to complete and require polling for their status. Methods starting long-running operations return *Operation<T> types.\n\nThe WaitForCompletionAsync method is an easy way to wait for operation completion and get the resulting value.\n\n// create a client\nSecretClient client = new SecretClient(new Uri(\"http://example.com\"), new DefaultAzureCredential());\n\n// Start the operation\nDeleteSecretOperation operation = await client.StartDeleteSecretAsync(\"SecretName\");\n\nResponse<DeletedSecret> response = await operation.WaitForCompletionAsync();\nDeletedSecret value = response.Value;\n\nConsole.WriteLine(value.Name);\nConsole.WriteLine(value.ScheduledPurgeDate);\nMore on long-running operations in long-running operation samples.\n\nCustomizing Requests Using RequestContext\nBesides general configuration of service clients through ClientOptions, it is possible to customize the requests sent by service clients using protocol methods or convenience APIs that expose RequestContext as a parameter.\n\nvar context = new RequestContext();\ncontext.AddClassifier(404, isError: false);\n\nResponse response = await client.GetPetAsync(\"pet1\", context);\nMore on request customization in RequestContext samples.\n\nMocking\nOne of the most important cross-cutting features of our new client libraries using Azure.Core is that they are designed for mocking. Mocking is enabled by:\n\nproviding a protected parameterless constructor on client types.\nmaking service methods virtual.\nproviding APIs for constructing model types returned from virtual service methods. To find these factory methods look for types with the ModelFactory suffix, e.g. SecretModelFactory.\nFor example, the ConfigurationClient.Get method can be mocked (with Moq) as follows:\n\n// Create a mock response\nvar mockResponse = new Mock<Response>();\n\n// Create a mock value\nvar mockValue = SecretModelFactory.KeyVaultSecret(\n    SecretModelFactory.SecretProperties(new Uri(\"http://example.com\"))\n);\n\n// Create a client mock\nvar mock = new Mock<SecretClient>();\n\n// Setup client method\nmock.Setup(c => c.GetSecret(\"Name\", null, default))\n    .Returns(Response.FromValue(mockValue, mockResponse.Object));\n\n// Use the client mock\nSecretClient client = mock.Object;\nKeyVaultSecret secret = client.GetSecret(\"Name\");" },
                    { 29, 101963009, 6, new DateTime(2022, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "https://licenses.nuget.org/MIT", "1.25.0", "https://github.com/Azure/azure-sdk-for-net", "Examples\nNOTE: Samples in this file apply only to packages that follow Azure SDK Design Guidelines. Names of such packages usually start with Azure.\n\nConfiguring Service Clients Using ClientOptions\nAzure SDK client libraries typically expose one or more service client types that are the main starting points for calling corresponding Azure services. You can easily find these client types as their names end with the word Client. For example, BlockBlobClient can be used to call blob storage service, and KeyClient can be used to access Key Vault service cryptographic keys.\n\nThese client types can be instantiated by calling a simple constructor, or its overload that takes various configuration options. These options are passed as a parameter that extends ClientOptions class exposed by Azure.Core. Various service specific options are usually added to its subclasses, but a set of SDK-wide options are available directly on ClientOptions.\n\nSecretClientOptions options = new SecretClientOptions()\n{\n    Retry =\n    {\n        Delay = TimeSpan.FromSeconds(2),\n        MaxRetries = 10,\n        Mode = RetryMode.Fixed\n    },\n    Diagnostics =\n    {\n        IsLoggingContentEnabled = true,\n        ApplicationId = \"myApplicationId\"\n    }\n};\n\nSecretClient client = new SecretClient(new Uri(\"http://example.com\"), new DefaultAzureCredential(), options);\nMore on client configuration in client configuration samples\n\nAccessing HTTP Response Details Using Response<T>\nService clients have methods that can be used to call Azure services. We refer to these client methods service methods. Service methods return a shared Azure.Core type Response<T> (in rare cases its non-generic sibling, a raw Response). This type provides access to both the deserialized result of the service call, and to the details of the HTTP response returned from the server.\n\n// create a client\nvar client = new SecretClient(new Uri(\"http://example.com\"), new DefaultAzureCredential());\n\n// call a service method, which returns Response<T>\nResponse<KeyVaultSecret> response = await client.GetSecretAsync(\"SecretName\");\n\n// Response<T> has two main accessors.\n// Value property for accessing the deserialized result of the call\nKeyVaultSecret secret = response.Value;\n\n// .. and GetRawResponse method for accessing all the details of the HTTP response\nResponse http = response.GetRawResponse();\n\n// for example, you can access HTTP status\nint status = http.Status;\n\n// or the headers\nforeach (HttpHeader header in http.Headers)\n{\n    Console.WriteLine($\"{header.Name} {header.Value}\");\n}\nMore on response types in response samples\n\nSetting up console logging\nTo create an Azure SDK log listener that outputs messages to console use AzureEventSourceListener.CreateConsoleLogger method.\n\n// Setup a listener to monitor logged events.\nusing AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();\nMore on logging in diagnostics samples\n\nReporting Errors RequestFailedException\nWhen a service call fails Azure.RequestFailedException would get thrown. The exception type provides a Status property with an HTTP status code and an ErrorCode property with a service-specific error code.\n\ntry\n{\n    KeyVaultSecret secret = client.GetSecret(\"NonexistentSecret\");\n}\n// handle exception with status code 404\ncatch (RequestFailedException e) when (e.Status == 404)\n{\n    // handle not found error\n    Console.WriteLine(\"ErrorCode \" + e.ErrorCode);\n}\nMore on handling responses in response samples\n\nConsuming Service Methods Returning AsyncPageable<T>\nIf a service call returns multiple values in pages, it would return Pageable<T>/AsyncPageable<T> as a result. You can iterate over AsyncPageable directly or in pages.\n\n// call a service method, which returns AsyncPageable<T>\nAsyncPageable<SecretProperties> allSecretProperties = client.GetPropertiesOfSecretsAsync();\n\nawait foreach (SecretProperties secretProperties in allSecretProperties)\n{\n    Console.WriteLine(secretProperties.Name);\n}\nFor more information on paged responses, see Pagination with the Azure SDK for .NET.\n\nConsuming Long-Running Operations Using Operation<T>\nSome operations take long time to complete and require polling for their status. Methods starting long-running operations return *Operation<T> types.\n\nThe WaitForCompletionAsync method is an easy way to wait for operation completion and get the resulting value.\n\n// create a client\nSecretClient client = new SecretClient(new Uri(\"http://example.com\"), new DefaultAzureCredential());\n\n// Start the operation\nDeleteSecretOperation operation = await client.StartDeleteSecretAsync(\"SecretName\");\n\nResponse<DeletedSecret> response = await operation.WaitForCompletionAsync();\nDeletedSecret value = response.Value;\n\nConsole.WriteLine(value.Name);\nConsole.WriteLine(value.ScheduledPurgeDate);\nMore on long-running operations in long-running operation samples\n\nCustomzing Request Using RequestContext\nBesides general configuration of service clients through ClientOptions, it is possible to customize the requests sent by service clients using protocol methods or convenience APIs that expose RequestContext as a parameter.\n\nvar context = new RequestContext();\ncontext.AddClassifier(404, isError: false);\n\nResponse response = await client.GetPetAsync(\"pet1\", context);\nMore on request customization in RequestContext samples\n\nMocking\nOne of the most important cross-cutting features of our new client libraries using Azure.Core is that they are designed for mocking. Mocking is enabled by:\n\nproviding a protected parameterless constructor on client types.\nmaking service methods virtual.\nproviding APIs for constructing model types returned from virtual service methods. To find these factory methods look for types with the ModelFactory suffix, e.g. SecretModelFactory.\nFor example, the ConfigurationClient.Get method can be mocked (with Moq) as follows:\n\n// Create a mock response\nvar mockResponse = new Mock<Response>();\n\n// Create a mock value\nvar mockValue = SecretModelFactory.KeyVaultSecret(\n    SecretModelFactory.SecretProperties(new Uri(\"http://example.com\"))\n);\n\n// Create a client mock\nvar mock = new Mock<SecretClient>();\n\n// Setup client method\nmock.Setup(c => c.GetSecret(\"Name\", null, default))\n    .Returns(Response.FromValue(mockValue, mockResponse.Object));\n\n// Use the client mock\nSecretClient client = mock.Object;\nKeyVaultSecret secret = client.GetSecret(\"Name\");" },
                    { 30, 108675594, 6, new DateTime(2020, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "https://licenses.nuget.org/MIT", "1.6.0", "https://github.com/Azure/azure-sdk-for-net", "" },
                    { 31, 57854685, 6, new DateTime(2022, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "https://licenses.nuget.org/Apache-2.0", "5.1.1", "https://github.com/castleproject/Core", "" },
                    { 32, 263104443, 6, new DateTime(2019, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "https://www.apache.org/licenses/LICENSE-2.0.html", "4.4.0", "https://github.com/castleproject/Core", "" },
                    { 33, 22939604, 6, new DateTime(2017, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "https://www.apache.org/licenses/LICENSE-2.0.html", "4.0.0", "", "" },
                    { 34, 641241, 6, new DateTime(2023, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "https://licenses.nuget.org/Apache-2.0", "3.7.300", "", "" },
                    { 35, 496049, 6, new DateTime(2023, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "https://licenses.nuget.org/Apache-2.0", "3.7.204", "", "" },
                    { 36, 14827784, 6, new DateTime(2022, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "http://aws.amazon.com/apache2.0/", "3.7.100", "", "" },
                    { 37, 44200281, 6, new DateTime(2022, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "https://raw.githubusercontent.com/moq/moq4/main/License.txt", "4.18.4", "https://github.com/moq/moq4", "" },
                    { 38, 88210016, 6, new DateTime(2021, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "https://raw.githubusercontent.com/moq/moq4/master/License.txt", "4.16.1", "https://github.com/moq/moq4", "" },
                    { 39, 44266355, 6, new DateTime(2019, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "https://raw.githubusercontent.com/moq/moq4/master/License.txt", "4.13.1", "https://github.com/moq/moq4", "" },
                    { 40, 2305117, 6, new DateTime(2023, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "https://licenses.nuget.org/BSD-3-Clause", "8.2.0", "https://github.com/App-vNext/Polly", "" },
                    { 41, 108665588, 6, new DateTime(2022, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "https://licenses.nuget.org/BSD-3-Clause", "7.2.3", "https://github.com/App-vNext/Polly.git", "" },
                    { 42, 21609357, 6, new DateTime(2018, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "https://raw.github.com/App-vNext/Polly/master/LICENSE.txt", "6.0.1", "", "" },
                    { 43, 4617920, 6, new DateTime(2023, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "https://licenses.nuget.org/Apache-2.0", "11.7.1", "https://github.com/JeremySkinner/fluentvalidation", "With FluentValidation, you can define a class that inherits from AbstractValidator which contains the rules for a particular class. The example below shows how you could define rules for a Customer class, and then how to execute the validator.\n\nusing FluentValidation;\n\npublic class CustomerValidator: AbstractValidator<Customer> {\n  public CustomerValidator() {\n    RuleFor(x => x.Surname).NotEmpty();\n    RuleFor(x => x.Forename).NotEmpty().WithMessage(\"Please specify a first name\");\n    RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);\n    RuleFor(x => x.Address).Length(20, 250);\n    RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage(\"Please specify a valid postcode\");\n  }\n\n  private bool BeAValidPostcode(string postcode) {\n    // custom postcode validating logic goes here\n  }\n}\n\nvar customer = new Customer();\nvar validator = new CustomerValidator();\n\n// Execute the validator.\nValidationResult results = validator.Validate(customer);\n\n// Inspect any validation failures.\nbool success = results.IsValid;\nList<ValidationFailure> failures = results.Errors;" },
                    { 44, 19729520, 6, new DateTime(2021, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "https://licenses.nuget.org/Apache-2.0", "10.3.6", "https://github.com/JeremySkinner/fluentvalidation", "Example\nWith FluentValidation, you can define a class that inherits from AbstractValidator which contains the rules for a particular class. The example below shows how you could define rules for a Customer class, and then how to execute the validator.\n\nusing FluentValidation;\n\npublic class CustomerValidator: AbstractValidator<Customer> {\n  public CustomerValidator() {\n    RuleFor(x => x.Surname).NotEmpty();\n    RuleFor(x => x.Forename).NotEmpty().WithMessage(\"Please specify a first name\");\n    RuleFor(x => x.Discount).NotEqual(0).When(x => x.HasDiscount);\n    RuleFor(x => x.Address).Length(20, 250);\n    RuleFor(x => x.Postcode).Must(BeAValidPostcode).WithMessage(\"Please specify a valid postcode\");\n  }\n\n  private bool BeAValidPostcode(string postcode) {\n    // custom postcode validating logic goes here\n  }\n}\n\nvar customer = new Customer();\nvar validator = new CustomerValidator();\n\n// Execute the validator.\nValidationResult results = validator.Validate(customer);\n\n// Inspect any validation failures.\nbool success = results.IsValid;\nList<ValidationFailure> failures = results.Errors;" },
                    { 45, 11676192, 6, new DateTime(2020, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "https://licenses.nuget.org/Apache-2.0", "9.3.0", "https://github.com/JeremySkinner/fluentvalidation", "" }
                });

            migrationBuilder.InsertData(
                table: "LibraryVersionDependencies",
                columns: new[] { "Id", "DependencyLibraryId", "FrameworkId", "LibraryVersionId", "SupportedVersions" },
                values: new object[,]
                {
                    { 1, 8, 26, 37, "(>= 5.1.1)" },
                    { 2, 8, 13, 37, "(>= 5.1.1)" },
                    { 3, 8, 12, 37, "(>= 5.1.1)" },
                    { 4, 8, 3, 37, "(>= 5.1.1)" },
                    { 5, 8, 31, 38, "(>= 4.4.0)" },
                    { 6, 8, 13, 38, "(>= 4.4.0)" },
                    { 7, 8, 12, 38, "(>= 4.4.0)" },
                    { 8, 8, 31, 39, "(>= 4.4.0)" },
                    { 9, 8, 13, 39, "(>= 4.4.0)" }
                });

            migrationBuilder.InsertData(
                table: "LibraryVersionSupportedFrameworks",
                columns: new[] { "Id", "FrameworkId", "LibraryVersionId" },
                values: new object[,]
                {
                    { 1, 3, 1 },
                    { 2, 35, 1 },
                    { 3, 33, 1 },
                    { 4, 32, 1 },
                    { 5, 31, 1 },
                    { 6, 35, 2 },
                    { 7, 33, 2 },
                    { 8, 32, 2 },
                    { 9, 31, 2 },
                    { 10, 35, 3 },
                    { 11, 33, 3 },
                    { 12, 32, 3 },
                    { 13, 3, 4 },
                    { 14, 1, 4 },
                    { 15, 27, 4 },
                    { 16, 26, 4 },
                    { 17, 23, 4 },
                    { 18, 3, 5 },
                    { 19, 31, 5 },
                    { 20, 27, 5 },
                    { 21, 23, 5 },
                    { 22, 31, 6 },
                    { 23, 27, 6 },
                    { 24, 13, 6 },
                    { 25, 12, 7 },
                    { 26, 27, 8 },
                    { 27, 13, 8 },
                    { 28, 27, 9 },
                    { 29, 13, 9 },
                    { 30, 3, 10 },
                    { 31, 2, 10 },
                    { 32, 1, 10 },
                    { 33, 26, 10 },
                    { 34, 13, 10 },
                    { 35, 12, 10 },
                    { 36, 3, 11 },
                    { 37, 2, 11 },
                    { 38, 26, 11 },
                    { 39, 13, 11 },
                    { 40, 12, 11 },
                    { 41, 3, 12 },
                    { 42, 27, 12 },
                    { 43, 13, 12 },
                    { 44, 12, 12 },
                    { 45, 1, 13 },
                    { 46, 3, 14 },
                    { 47, 3, 15 },
                    { 48, 3, 16 },
                    { 49, 2, 16 },
                    { 50, 1, 16 },
                    { 51, 3, 17 },
                    { 52, 2, 17 },
                    { 53, 3, 18 },
                    { 54, 5, 18 },
                    { 55, 3, 19 },
                    { 56, 26, 19 },
                    { 57, 13, 19 },
                    { 58, 12, 19 },
                    { 59, 26, 20 },
                    { 60, 5, 20 },
                    { 61, 13, 20 },
                    { 62, 12, 20 },
                    { 63, 28, 21 },
                    { 64, 8, 21 },
                    { 65, 5, 21 },
                    { 66, 13, 21 },
                    { 67, 12, 21 },
                    { 68, 4, 22 },
                    { 69, 3, 22 },
                    { 70, 2, 22 },
                    { 71, 26, 22 },
                    { 72, 24, 22 },
                    { 73, 13, 22 },
                    { 74, 12, 22 },
                    { 75, 4, 23 },
                    { 76, 3, 23 },
                    { 77, 2, 23 },
                    { 78, 26, 23 },
                    { 79, 24, 23 },
                    { 80, 13, 23 },
                    { 81, 12, 23 },
                    { 82, 4, 24 },
                    { 83, 3, 24 },
                    { 84, 31, 24 },
                    { 85, 28, 24 },
                    { 86, 25, 24 },
                    { 87, 20, 24 },
                    { 88, 17, 24 },
                    { 89, 13, 24 },
                    { 90, 12, 24 },
                    { 91, 3, 25 },
                    { 92, 25, 25 },
                    { 93, 8, 25 },
                    { 94, 6, 25 },
                    { 95, 13, 25 },
                    { 96, 12, 25 },
                    { 97, 3, 26 },
                    { 98, 25, 26 },
                    { 99, 8, 26 },
                    { 100, 6, 26 },
                    { 101, 13, 26 },
                    { 102, 12, 26 },
                    { 103, 31, 27 },
                    { 104, 25, 27 },
                    { 105, 9, 27 },
                    { 106, 8, 27 },
                    { 107, 17, 27 },
                    { 108, 14, 27 },
                    { 109, 13, 27 },
                    { 110, 12, 27 },
                    { 111, 4, 28 },
                    { 112, 3, 28 },
                    { 113, 27, 28 },
                    { 114, 23, 28 },
                    { 115, 8, 28 },
                    { 116, 13, 28 },
                    { 117, 4, 29 },
                    { 118, 27, 29 },
                    { 119, 8, 29 },
                    { 120, 13, 29 },
                    { 121, 27, 30 },
                    { 122, 13, 30 },
                    { 123, 3, 31 },
                    { 124, 26, 31 },
                    { 125, 13, 31 },
                    { 126, 12, 31 },
                    { 127, 33, 32 },
                    { 128, 32, 32 },
                    { 129, 31, 32 },
                    { 130, 17, 32 },
                    { 131, 15, 32 },
                    { 132, 33, 33 },
                    { 133, 31, 33 },
                    { 134, 17, 33 },
                    { 135, 1, 34 },
                    { 136, 33, 34 },
                    { 137, 31, 34 },
                    { 138, 5, 34 },
                    { 139, 13, 34 },
                    { 140, 33, 35 },
                    { 141, 31, 35 },
                    { 142, 5, 35 },
                    { 143, 13, 35 },
                    { 144, 33, 36 },
                    { 145, 31, 36 },
                    { 146, 5, 36 },
                    { 147, 13, 36 },
                    { 148, 3, 37 },
                    { 149, 26, 37 },
                    { 150, 13, 37 },
                    { 151, 12, 37 },
                    { 152, 31, 38 },
                    { 153, 13, 38 },
                    { 154, 12, 38 },
                    { 155, 31, 39 },
                    { 156, 13, 39 },
                    { 157, 3, 40 },
                    { 158, 26, 40 },
                    { 159, 23, 40 },
                    { 160, 13, 40 },
                    { 161, 27, 41 },
                    { 162, 23, 41 },
                    { 163, 19, 41 },
                    { 164, 13, 41 },
                    { 165, 19, 42 },
                    { 166, 13, 42 },
                    { 167, 4, 43 },
                    { 168, 3, 43 },
                    { 169, 2, 43 },
                    { 170, 13, 43 },
                    { 171, 12, 43 },
                    { 172, 4, 44 },
                    { 173, 3, 44 },
                    { 174, 13, 44 },
                    { 175, 12, 44 },
                    { 176, 4, 45 },
                    { 177, 27, 45 },
                    { 178, 13, 45 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileExtensions_Name",
                table: "FileExtensions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_FileExtensionId",
                table: "Images",
                column: "FileExtensionId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_LogoId",
                table: "Libraries",
                column: "LogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_UserId",
                table: "Libraries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersionDependencies_DependencyLibraryId",
                table: "LibraryVersionDependencies",
                column: "DependencyLibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersionDependencies_FrameworkId_LibraryVersionId_DependencyLibraryId",
                table: "LibraryVersionDependencies",
                columns: new[] { "FrameworkId", "LibraryVersionId", "DependencyLibraryId" },
                unique: true,
                filter: "[FrameworkId] IS NOT NULL AND [LibraryVersionId] IS NOT NULL AND [DependencyLibraryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersionDependencies_LibraryVersionId",
                table: "LibraryVersionDependencies",
                column: "LibraryVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersions_FileExtensionId",
                table: "LibraryVersions",
                column: "FileExtensionId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersions_LibraryId_Name",
                table: "LibraryVersions",
                columns: new[] { "LibraryId", "Name" },
                unique: true,
                filter: "[LibraryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersionSupportedFrameworks_FrameworkId_LibraryVersionId",
                table: "LibraryVersionSupportedFrameworks",
                columns: new[] { "FrameworkId", "LibraryVersionId" },
                unique: true,
                filter: "[FrameworkId] IS NOT NULL AND [LibraryVersionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryVersionSupportedFrameworks_LibraryVersionId",
                table: "LibraryVersionSupportedFrameworks",
                column: "LibraryVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryArchiveHashes");

            migrationBuilder.DropTable(
                name: "LibraryVersionDependencies");

            migrationBuilder.DropTable(
                name: "LibraryVersionSupportedFrameworks");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Frameworks");

            migrationBuilder.DropTable(
                name: "LibraryVersions");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FileExtensions");
        }
    }
}
