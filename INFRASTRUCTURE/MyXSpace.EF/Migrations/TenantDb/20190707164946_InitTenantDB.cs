using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyXSpace.EF.Migrations.TenantDb
{
    public partial class InitTenantDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    ClientType = table.Column<int>(nullable: false),
                    ClientName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    Index = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    AgencyFileCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    ApeCode = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    City = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Contact1Civility = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Contact1Name = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Contact2Civility = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Contact2Name = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Contact3Civility = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Contact3Name = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    DataUpdated = table.Column<DateTime>(nullable: true),
                    MedecineName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PaymentMode = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Telephone = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    MedecineCode = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ApeCodeCode = table.Column<string>(nullable: true),
                    PaymentMode2 = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    PaymentMode3 = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    MissionCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    SiretNumber = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PaymentMode4 = table.Column<int>(nullable: true),
                    Source = table.Column<string>(unicode: false, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Client", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Consent",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    ConsentType = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    ConsentVersion = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    ConsentDate = table.Column<DateTime>(nullable: true),
                    ConsentContent = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ConsentLangage = table.Column<string>(unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractDetail",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Category = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    DataUpdated = table.Column<DateTime>(nullable: true),
                    ContractCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    ClientCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    Access = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Source = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    Comments = table.Column<string>(unicode: false, maxLength: 500, nullable: true),
                    JobCharacteristics = table.Column<string>(unicode: false, maxLength: 3005, nullable: true),
                    MissionCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    SalaryReference = table.Column<string>(unicode: false, maxLength: 2005, nullable: true),
                    SalesmanCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    CycleCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    EndFlexibility = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Equipment = table.Column<string>(unicode: false, maxLength: 2005, nullable: true),
                    Index = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    Installations = table.Column<string>(unicode: false, maxLength: 2005, nullable: true),
                    BilledAmount = table.Column<decimal>(type: "decimal(11, 4)", nullable: true),
                    LawQuoted = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    NonWorkedPeriod = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    PayedAmount = table.Column<decimal>(type: "decimal(11, 4)", nullable: true),
                    ReasonCode = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    ReasonLabel = table.Column<string>(unicode: false, maxLength: 5010, nullable: true),
                    ReferenceHoursNumber = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Risks = table.Column<string>(unicode: false, maxLength: 1000, nullable: true),
                    RubricCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    Schedule = table.Column<string>(unicode: false, maxLength: 400, nullable: true),
                    SiteAddress = table.Column<string>(unicode: false, maxLength: 300, nullable: true),
                    SiteCity = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    SiteName = table.Column<string>(unicode: false, maxLength: 25, nullable: true),
                    SiteZipCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    StartFlexibility = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TrialDuration = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    AmendmentCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    AgencyFileCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    End = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    PersonToAsk = table.Column<string>(unicode: false, maxLength: 35, nullable: true),
                    UserQualificationCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    AmendmentType = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    UserFreeQualificationLabel = table.Column<string>(unicode: false, maxLength: 35, nullable: true),
                    AmendmentReason = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    RepresentingPerson = table.Column<string>(unicode: false, maxLength: 35, nullable: true),
                    EndDateFlexibility = table.Column<DateTime>(nullable: true),
                    StartDateFlexibility = table.Column<DateTime>(nullable: true),
                    FullRubric = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    UnitRubric = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    OrigineAgenceCDI = table.Column<string>(unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ContractDetail", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "ContractSigned",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    AdequatMedecineName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AdequatMedecineAddress = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyAddress = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyApeCodeLabel = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyCity = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyFaxNumber = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyFileCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    AgencyId = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    AgencyName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyPhoneNumber = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencySigningDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencySiretNumber = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyZipCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    AmendmentCode = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    Characteristics = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientAddress = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientApeCodeLabel = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientCity = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientMedecineName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientZipCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    ContractSchedule = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ContributionCenterLabel = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    End = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    EndDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Equipment = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    FinancialGuarantorLabel = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    FlexibilityEndDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    FlexibilityStartDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    FullCode = table.Column<string>(unicode: false, maxLength: 13, nullable: true),
                    HourReference = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    InitialOrAmendment = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    InitialStartAndEndDates = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Installation = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    IssueDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    IssuePlace = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Justifications = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    LawQuoted = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    MedicalVisitDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    OffPeriod = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PensionFundAddress = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PensionFundCity = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PensionFundName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PensionFundZipCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    PersonToAsk = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    QualificationLabel = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ReferenceHourNumber = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Risks = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    RubricNames = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SalaryReference1 = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SalaryValues = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SiteAccess = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SiteAddress = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SiteCity = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SiteName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SiteZipCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    StartDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    TrialDuration = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserAddress = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserBirthDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserBirthPlace = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserCategory = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserCity = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserCivility = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserCode = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserFirstName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserId = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserLastName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserNationalityLabel = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserNumber = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserPaymentModeLabel = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserPosition = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserSigningDate = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserZipCode = table.Column<string>(unicode: false, maxLength: 5, nullable: true),
                    ValueReference = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Week = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    WeeklyDuration = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AmendmentType = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    AmendmentReason = table.Column<string>(unicode: false, maxLength: 35, nullable: true),
                    RepresentingPerson = table.Column<string>(unicode: false, maxLength: 35, nullable: true),
                    signatureRequestId = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    externalSignatureRequestId = table.Column<string>(unicode: false, maxLength: 36, nullable: true),
                    orderRequestId = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    externalOrderRequestId = table.Column<string>(unicode: false, maxLength: 36, nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    AgeCdiiAddress = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    AgeCdiiCity = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    AgeCdiiCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    AgeCdiiFaxNumber = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    AgeCdiiName = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    AgeCdiiPhoneNumber = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    AgeCdiiSiretNumber = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    AgeCdiiZipCode = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    AgeCdiiId = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    AgencyCdiiApeCodeLabel = table.Column<string>(unicode: false, maxLength: 130, nullable: true),
                    TypeContractCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    AdequatMedCdiiName = table.Column<string>(unicode: false, maxLength: 130, nullable: true),
                    AdequatMedCdiiAddress = table.Column<string>(unicode: false, maxLength: 130, nullable: true),
                    FinancialGuarantorCdiiLabel = table.Column<string>(unicode: false, maxLength: 130, nullable: true),
                    ContributionCenterCdiiLabel = table.Column<string>(unicode: false, maxLength: 130, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractSigned", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaqCategory",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(unicode: false, maxLength: 80, nullable: true),
                    Description = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    UserType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flow",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Source = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    DataUpdated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flow", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoggingTryout",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    UserLogin = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    Tryout = table.Column<int>(nullable: false),
                    LastTry = table.Column<DateTime>(name: "LastTry ", nullable: false),
                    UnlockCode = table.Column<string>(unicode: false, maxLength: 24, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LoggingT__3214EC0665957100", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                })
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.CreateTable(
                name: "MockEmail",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    From = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    To = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Subject = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    MailHtml = table.Column<string>(unicode: false, maxLength: 8000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockEmail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MockSms",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    From = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    To = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Body = table.Column<string>(unicode: false, maxLength: 8000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MockSms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 100, nullable: false),
                    ClientSecret = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    ConsentType = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Permissions = table.Column<string>(nullable: true),
                    PostLogoutRedirectUris = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    RedirectUris = table.Column<string>(nullable: true),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Properties = table.Column<string>(nullable: true),
                    Resources = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCode",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    MatriculeCode = table.Column<string>(unicode: false, maxLength: 9, nullable: true),
                    User_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    AgencyCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    AgencyFileCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.UserCode", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "UserRole_PermissionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole_PermissionGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaqQuestion",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Title = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Detail = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    FaqCategory_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqQuestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.FaqQuestion_dbo.FaqCategory_FaqCategory_Id",
                        column: x => x.FaqCategory_Id,
                        principalTable: "FaqCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    Scopes = table.Column<string>(nullable: true),
                    Status = table.Column<string>(maxLength: 25, nullable: false),
                    Subject = table.Column<string>(maxLength: 450, nullable: false),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole_Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsGranted = table.Column<bool>(nullable: false),
                    RolePermissionGroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Permission_UserRole_PermissionGroup_RolePermissionGroupId",
                        column: x => x.RolePermissionGroupId,
                        principalTable: "UserRole_PermissionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true),
                    AuthorizationId = table.Column<string>(nullable: true),
                    ConcurrencyToken = table.Column<string>(maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(nullable: true),
                    ExpirationDate = table.Column<DateTimeOffset>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    Properties = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<string>(maxLength: 25, nullable: false),
                    Subject = table.Column<string>(maxLength: 450, nullable: false),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    ContractCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    ContractDetail_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    ContractOrigin = table.Column<int>(nullable: true),
                    Agreed = table.Column<bool>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Processed = table.Column<bool>(nullable: false),
                    Available = table.Column<bool>(nullable: true),
                    Signed = table.Column<bool>(nullable: false),
                    SignOrigin = table.Column<int>(nullable: true),
                    SigningDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    ContractSigned_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    End = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    CompanyCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    UserProfile_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    UserCategory = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserCode = table.Column<string>(unicode: false, maxLength: 9, nullable: true),
                    UserPosition = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    UserQualificationCode = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ClientId = table.Column<int>(unicode: false, maxLength: 32, nullable: false),
                    ClientCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    AgencyFileCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    AmendmentCode = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    DataUpdated = table.Column<DateTime>(nullable: true),
                    Index = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    PensionFundAddress = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PensionFundCity = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PensionFundName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    PensionFundZipCode = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    ValueReference = table.Column<decimal>(type: "decimal(11, 4)", nullable: true),
                    Week = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    WeeklyDuration = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    AgencyCompanyCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    ValidationDate = table.Column<DateTime>(nullable: true),
                    Coefficient = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    SalesmanCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    MissionCode = table.Column<string>(unicode: false, maxLength: 6, nullable: true),
                    OriginAgencyCDI = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    Source = table.Column<string>(unicode: false, maxLength: 3, nullable: true),
                    TypeContractCode = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    PrintDate = table.Column<DateTime>(nullable: true),
                    Id_Adv_Contract = table.Column<string>(unicode: false, maxLength: 36, nullable: true),
                    AgencyFusionCode = table.Column<string>(unicode: false, maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Contract", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_dbo.Contract_dbo.Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Contract_dbo.ContractDetail_ContractDetail_Id",
                        column: x => x.ContractDetail_Id,
                        principalTable: "ContractDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.Contract_dbo.ContractSigned_ContractSigned_Id",
                        column: x => x.ContractSigned_Id,
                        principalTable: "ContractSigned",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Contract_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    ExecutionDate = table.Column<DateTime>(nullable: false),
                    Mode = table.Column<int>(nullable: false),
                    Receiver = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    NotifyUnregisteredUsers = table.Column<bool>(nullable: false),
                    From = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    To = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Title = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Content = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    AgencyFileCode = table.Column<string>(unicode: false, maxLength: 8000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Notification", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_dbo.Notification_dbo.Contract_Contract_Id",
                        column: x => x.Contract_Id,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsentAgreement",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Consent_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true),
                    ConsentAgreementDate = table.Column<DateTime>(nullable: false),
                    UserProfile_Id = table.Column<string>(unicode: false, maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentAgreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.ConsentAgreement_dbo.Consent_Consent_Id",
                        column: x => x.Consent_Id,
                        principalTable: "Consent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agency",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Index = table.Column<string>(nullable: true),
                    AgencyCode = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    AgencyName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    BrandName = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    ConnectionString = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    AdminId = table.Column<string>(nullable: true),
                    AdminId1 = table.Column<string>(nullable: true),
                    SuperAdminId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSupport",
                columns: table => new
                {
                    Id = table.Column<int>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ContactName = table.Column<string>(unicode: false, maxLength: 75, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 25, nullable: true),
                    Registered = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSupport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSupport_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "TenantUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    ProviderName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    Configuration = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_TenantUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TenantUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_TenantUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TenantUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_TenantUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TenantUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_TenantUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TenantUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuperAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperAdmins_TenantUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TenantUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RoleType = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantRoles_TenantUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TenantUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    LastName = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: true),
                    BirthPlace = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    Nationality = table.Column<string>(unicode: false, maxLength: 8000, nullable: true),
                    SSNumber = table.Column<string>(unicode: false, maxLength: 13, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.UserProfile", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_UserProfile_TenantUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "TenantUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Invited = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProfileId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_UserProfile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agency_CustomerId",
                table: "Agency",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ProfileId",
                table: "Candidates",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Opt1",
                table: "Clients",
                columns: new[] { "ClientCode", "AgencyFileCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Opt1",
                table: "Consent",
                columns: new[] { "ConsentType", "ConsentVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_Consent_Id",
                table: "ConsentAgreement",
                column: "Consent_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_Id",
                table: "ConsentAgreement",
                column: "UserProfile_Id");

            migrationBuilder.CreateIndex(
                name: "nci_wi_ConsentAgreement_C1E33A85D2F31DF212DB5EFB52DA8BDF",
                table: "ConsentAgreement",
                columns: new[] { "ConsentAgreementDate", "UserProfile_Id", "Consent_Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientId",
                table: "Contract",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetail_Id",
                table: "Contract",
                column: "ContractDetail_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ContractSigned_Id",
                table: "Contract",
                column: "ContractSigned_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_StartDate",
                table: "Contract",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_Id",
                table: "Contract",
                column: "UserProfile_Id");

            migrationBuilder.CreateIndex(
                name: "IX_VScopeAlertClient",
                table: "Contract",
                columns: new[] { "UserProfile_Id", "ClientId" });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Opt2",
                table: "Contract",
                columns: new[] { "ContractCode", "AgencyFileCode", "AmendmentCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Opt3",
                table: "Contract",
                columns: new[] { "ContractCode", "AgencyFileCode", "ClientCode", "AmendmentCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Opt4",
                table: "Contract",
                columns: new[] { "Signed", "Available", "AgencyFileCode", "End", "SigningDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Opt1",
                table: "Contract",
                columns: new[] { "AgencyFileCode", "AmendmentCode", "ClientCode", "ContractCode", "Index", "UserCode" });

            migrationBuilder.CreateIndex(
                name: "nci_wi_Contract_02E4F62C8629B2C6FF8AF955F046FE72",
                table: "Contract",
                columns: new[] { "AgencyCompanyCode", "AmendmentCode", "ClientCode", "ContractCode", "ContractDetail_Id", "End", "EndDate", "Id", "StartDate", "UserCode", "ValidationDate", "AgencyFileCode", "Available", "Deleted", "Signed" });

            migrationBuilder.CreateIndex(
                name: "nci_wi_Contract_040B5AF619A9E386762AE2198F1367D7",
                table: "Contract",
                columns: new[] { "AmendmentCode", "ClientCode", "ContractCode", "ContractDetail_Id", "Deleted", "End", "EndDate", "Id", "StartDate", "TypeContractCode", "UserCode", "ValueReference", "WeeklyDuration", "AgencyFileCode", "Available", "Signed" });

            migrationBuilder.CreateIndex(
                name: "nci_wi_Contract_699EB96DEAAA2D778443594B59E8E0A6",
                table: "Contract",
                columns: new[] { "AgencyCompanyCode", "AmendmentCode", "ClientCode", "ContractCode", "ContractDetail_Id", "ContractSigned_Id", "End", "EndDate", "Id", "OriginAgencyCDI", "Source", "StartDate", "TypeContractCode", "UserCode", "ValueReference", "WeeklyDuration", "Deleted", "AgencyFileCode", "Available", "Signed" });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Opt5",
                table: "Contract",
                columns: new[] { "AgencyFileCode", "AmendmentCode", "ClientCode", "Coefficient", "ContractCode", "ContractDetail_Id", "ContractSigned_Id", "Deleted", "End", "EndDate", "Id", "OriginAgencyCDI", "PensionFundAddress", "PensionFundCity", "PensionFundName", "PensionFundZipCode", "SigningDate", "StartDate", "TypeContractCode", "UserPosition", "UserQualificationCode", "ValueReference", "WeeklyDuration", "UserCode", "Available", "Signed" });

            migrationBuilder.CreateIndex(
                name: "IX_PK",
                table: "ContractDetail",
                column: "Id")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_VUserProfileRecent",
                table: "ContractDetail",
                columns: new[] { "Id", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_OptMultiColumn",
                table: "ContractDetail",
                columns: new[] { "AgencyFileCode", "AmendmentCode", "ClientCode", "ContractCode", "Index" });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Opt1",
                table: "ContractSigned",
                column: "FullCode");

            migrationBuilder.CreateIndex(
                name: "nci_wi_ContractSigned_D68BB9D9555B793C3FA3552F9C710F28",
                table: "ContractSigned",
                columns: new[] { "ClientName", "FullCode", "UserCivility", "UserFirstName", "UserLastName", "UserSigningDate", "AgencyFileCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserId",
                table: "Customer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqCategory_Id",
                table: "FaqQuestion",
                column: "FaqCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_Id",
                table: "Notification",
                column: "Contract_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionDate",
                table: "Notification",
                column: "ExecutionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Opt_To",
                table: "Notification",
                column: "To");

            migrationBuilder.CreateIndex(
                name: "nci_wi_Notification_9EBAC34AC273FFC7782169241F11BD48",
                table: "Notification",
                columns: new[] { "Mode", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_ClientId",
                table: "OpenIddictApplications",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
                table: "OpenIddictAuthorizations",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_Name",
                table: "OpenIddictScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                table: "OpenIddictTokens",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                table: "OpenIddictTokens",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
                table: "OpenIddictTokens",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_SuperAdmins_Id",
                table: "SuperAdmins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SuperAdmins_UserId",
                table: "SuperAdmins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_AdminId1",
                table: "Tenant",
                column: "AdminId1");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_SuperAdminId",
                table: "Tenant",
                column: "SuperAdminId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "TenantRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TenantRoles_UserId",
                table: "TenantRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "TenantUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "TenantUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TenantUsers_RoleId",
                table: "TenantUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCode_MatriculeCode",
                table: "UserCode",
                column: "MatriculeCode");

            migrationBuilder.CreateIndex(
                name: "IX_User_Id",
                table: "UserCode",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCode_Opt1",
                table: "UserCode",
                columns: new[] { "User_Id", "AgencyFileCode" });

            migrationBuilder.CreateIndex(
                name: "IX_BirthDate",
                table: "UserProfile",
                column: "BirthDate");

            migrationBuilder.CreateIndex(
                name: "IX_Email",
                table: "UserProfile",
                column: "Email",
                unique: true,
                filter: "([Email] IS NOT NULL AND [Email]<>'')");

            migrationBuilder.CreateIndex(
                name: "IX_FirstName",
                table: "UserProfile",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_LastName",
                table: "UserProfile",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_SSNumber",
                table: "UserProfile",
                column: "SSNumber");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_UserId",
                table: "UserProfile",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Permission_RolePermissionGroupId",
                table: "UserRole_Permission",
                column: "RolePermissionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSupport_TenantId",
                table: "UserSupport",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_UserProfile_UserProfile_Id",
                table: "Contract",
                column: "UserProfile_Id",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsentAgreement_UserProfile_UserProfile_Id",
                table: "ConsentAgreement",
                column: "UserProfile_Id",
                principalTable: "UserProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Agency_Customer_CustomerId",
                table: "Agency",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_TenantUsers_AdminId1",
                table: "Tenant",
                column: "AdminId1",
                principalTable: "TenantUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenant_SuperAdmins_SuperAdminId",
                table: "Tenant",
                column: "SuperAdminId",
                principalTable: "SuperAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_TenantRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "TenantRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_TenantRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "TenantRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_TenantUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "TenantUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantUsers_TenantRoles_RoleId",
                table: "TenantUsers",
                column: "RoleId",
                principalTable: "TenantRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantUsers_TenantRoles_RoleId",
                table: "TenantUsers");

            migrationBuilder.DropTable(
                name: "Agency");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "ConsentAgreement");

            migrationBuilder.DropTable(
                name: "FaqQuestion");

            migrationBuilder.DropTable(
                name: "Flow");

            migrationBuilder.DropTable(
                name: "LoggingTryout")
                .Annotation("SqlServer:MemoryOptimized", true);

            migrationBuilder.DropTable(
                name: "MockEmail");

            migrationBuilder.DropTable(
                name: "MockSms");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens");

            migrationBuilder.DropTable(
                name: "UserCode");

            migrationBuilder.DropTable(
                name: "UserRole_Permission");

            migrationBuilder.DropTable(
                name: "UserSupport");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Consent");

            migrationBuilder.DropTable(
                name: "FaqCategory");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations");

            migrationBuilder.DropTable(
                name: "UserRole_PermissionGroup");

            migrationBuilder.DropTable(
                name: "Tenant");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ContractDetail");

            migrationBuilder.DropTable(
                name: "ContractSigned");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications");

            migrationBuilder.DropTable(
                name: "SuperAdmins");

            migrationBuilder.DropTable(
                name: "TenantRoles");

            migrationBuilder.DropTable(
                name: "TenantUsers");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("SqlServer:MemoryOptimized", true);
        }
    }
}
