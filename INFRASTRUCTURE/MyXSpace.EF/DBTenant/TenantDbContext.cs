using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyXSpace.Core.Entities;
using System.Threading.Tasks;

namespace MyXSpace.EF
{
    /// <summary>
    /// Tenant DB Context
    /// NOTE: this context must be per each tenant (brand name)
    /// </summary>
    public partial class TenantDbContext/*<Tenant>*/ : IdentityDbContext<AppUser, AppRole, string>
    {
        public string CurrentUserId { get; set; }

        public string CurrentTeanantName { get; set; }

        #region Identification. Entities User, Role already build in context IdentityDbContext

        //NOTE: can be in another DB - CatalogTenants
        //public virtual DbSet<Tenant> Tenants { get; set; }

        /// <summary>
        /// TODO:
        /// </summary>
        public virtual DbSet<SuperAdmin> SuperAdmins { get; set; }

        public virtual DbSet<Agency> Agency { get; set; }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<Candidate> Candidates { get; set; }

        //Profile per user
        public virtual DbSet<UserProfile> UserProfile { get; set; }

        /// <summary>
        /// TODO: maybe used for authentication
        /// </summary>
        public virtual DbSet<UserCode> UserCode { get; set; }

        public virtual DbSet<UserRole_Permission> UserRole_Permission{ get; set; }
        public virtual DbSet<UserRole_PermissionGroup> UserRole_PermissionGroup { get; set; }
        public virtual DbSet<UserSupport> UserSupport { get; set; }  //User with role "Support"

        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<ContractDetail> ContractDetail { get; set; }
        public virtual DbSet<ContractSigned> ContractSigned { get; set; }
        public virtual DbSet<Consent> Consent { get; set; }
        public virtual DbSet<ConsentAgreement> ConsentAgreement { get; set; }
        public virtual DbSet<FaqCategory> FaqCategory { get; set; }
        public virtual DbSet<FaqQuestion> FaqQuestion { get; set; }
        public virtual DbSet<Flow> Flow { get; set; }

        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<LoggingTryout> LoggingTryout { get; set; }
        public virtual DbSet<MockEmail> MockEmail { get; set; }
        public virtual DbSet<MockSms> MockSms { get; set; }
 
        //public virtual DbSet<Schedule> Schedule { get; set; }     
        //public virtual DbSet<Tracking> Tracking { get; set; }

        #endregion

        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {
        }

        public TenantDbContext(string tenantName, DbContextOptions<TenantDbContext> options) : this(options)
        {
            CurrentTeanantName = tenantName;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    string dbConnection = CONSTS.IdentDBConnectionStr; //TODO: to be removed, moved to Config
        //    if (!optionsBuilder.IsConfigured)
        //    {               
        //        optionsBuilder.UseSqlServer(dbConnection);
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //To generate OpenId tables
            builder.UseOpenIddict();//<int>();

            #region User Profile

            builder.Entity<AppUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppUser>().HasMany(u => u.UserRoles).WithOne().HasForeignKey(r => r.UserId)//.IsRequired()
                .OnDelete(DeleteBehavior.SetNull);//.OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppUser>().ToTable("TenantUsers");

            builder.Entity<AppRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId)//IsRequired()
                .OnDelete(DeleteBehavior.SetNull);//.OnDelete(DeleteBehavior.Cascade);
            builder.Entity<AppRole>().ToTable("TenantRoles");

            builder.Entity<UserCode>(entity =>
            {
                entity.HasIndex(e => e.MatriculeCode);
                entity.Property(e => e.MatriculeCode)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("User_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                //entity.HasOne(d => d.User)
                //    .WithOne(p => p.UserCode)
                //    .HasForeignKey<AppUser>(d => d.UserCodeId)
                //    .HasConstraintName("FK_dbo.UserCode_dbo.UserProfile_UserProfile_Id");
                entity.HasKey(e => e.Id)
                    .HasName("PK_dbo.UserCode")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_User_Id");

                entity.HasIndex(e => new { e.UserId, e.AgencyFileCode })
                    .HasName("IX_UserCode_Opt1");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AgencyCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyFileCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            builder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_dbo.UserProfile")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.BirthDate)
                    .HasName("IX_BirthDate");

                entity.HasIndex(e => e.Email)
                    .HasName("IX_Email")
                    .IsUnique()
                    .HasFilter("([Email] IS NOT NULL AND [Email]<>'')");

                entity.HasIndex(e => e.FirstName)
                    .HasName("IX_FirstName");

                entity.HasIndex(e => e.LastName)
                    .HasName("IX_LastName");

                entity.HasIndex(e => e.Ssnumber)
                    .HasName("IX_SSNumber");

                //entity.HasIndex(e => new { e.Id, e.Email })
                //    .HasName("nci_wi_UserProfile_80C1E27B06225B3CA9E37C12C080950A");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.BirthPlace)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);


                entity.Property(e => e.LastName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Nationality)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Ssnumber)
                    .HasColumnName("SSNumber")
                    .HasMaxLength(13)
                    .IsUnicode(false);

            });

            builder.Entity<UserSupport>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                //entity.Property(e => e.Password)
                //    .HasMaxLength(255)
                //    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            builder.Entity<SuperAdmin>(entity =>
            {
                entity.HasIndex(e => e.Id);

                //entity.HasMany(d => d.Tenants)
                //  .WithOne(p => p.SuperAdmin)
                //  .HasForeignKey(d => d.SuperAdminId);
            });

            #endregion

            builder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_dbo.Client")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => new { e.ClientCode, e.AgencyFileCode })
                    .HasName("IX_Client_Opt1");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                //entity.Property(e => e.Address1)
                //    .HasMaxLength(8000)
                //    .IsUnicode(false);

                entity.Property(e => e.AgencyFileCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                //entity.Property(e => e.ApeCode)
                //    .HasMaxLength(5)
                //    .IsUnicode(false);

                //entity.Property(e => e.ApeCodeCode)
                //    .HasMaxLength(4)
                //    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ClientCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ClientName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Contact1Civility)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Contact1Name)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Contact2Civility)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Contact2Name)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Contact3Civility)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Contact3Name)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Index)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.MedecineCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.MedecineName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.MissionCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentMode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentMode2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentMode3)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SiretNumber)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            builder.Entity<Consent>(entity =>
            {
                entity.HasIndex(e => new { e.ConsentType, e.ConsentVersion })
                    .HasName("IX_Opt1");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConsentContent)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ConsentLangage)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ConsentType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ConsentVersion)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            builder.Entity<ConsentAgreement>(entity =>
            {
                entity.HasIndex(e => e.ConsentId)
                    .HasName("IX_Consent_Id");

                entity.HasIndex(e => e.UserProfileId)
                    .HasName("IX_UserProfile_Id");

                entity.HasIndex(e => new { e.ConsentAgreementDate, e.UserProfileId, e.ConsentId })
                    .HasName("nci_wi_ConsentAgreement_C1E33A85D2F31DF212DB5EFB52DA8BDF");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConsentId)
                    .HasColumnName("Consent_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("UserProfile_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.Consent)
                    .WithMany(p => p.ConsentAgreement)
                    .HasForeignKey(d => d.ConsentId)
                    .HasConstraintName("FK_dbo.ConsentAgreement_dbo.Consent_Consent_Id");

                //entity.HasOne(d => d.UserProfile)
                //    .WithMany(p => p.ConsentAgreement)
                //    .HasForeignKey(d => d.UserProfileId)
                //    .HasConstraintName("FK_dbo.ConsentAgreement_dbo.UserProfile_UserProfile_Id");
            });

            builder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_dbo.Contract")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.ClientId)
                    .HasName("IX_ClientId");

                entity.HasIndex(e => e.ContractDetailId)
                    .HasName("IX_ContractDetail_Id");

                entity.HasIndex(e => e.ContractSignedId)
                    .HasName("IX_ContractSigned_Id");

                entity.HasIndex(e => e.StartDate);

                entity.HasIndex(e => e.UserProfileId)
                    .HasName("IX_UserProfile_Id");

                entity.HasIndex(e => new { e.UserProfileId, e.ClientId })
                    .HasName("IX_VScopeAlertClient");

                entity.HasIndex(e => new { e.ContractCode, e.AgencyFileCode, e.AmendmentCode })
                    .HasName("IX_Contract_Opt2");

                entity.HasIndex(e => new { e.ContractCode, e.AgencyFileCode, e.ClientCode, e.AmendmentCode })
                    .HasName("IX_Contract_Opt3");

                entity.HasIndex(e => new { e.Signed, e.Available, e.AgencyFileCode, e.End, e.SigningDate })
                    .HasName("IX_Contract_Opt4");

                entity.HasIndex(e => new { e.AgencyFileCode, e.AmendmentCode, e.ClientCode, e.ContractCode, e.Index, e.UserCode })
                    .HasName("IX_Contract_Opt1");

                entity.HasIndex(e => new { e.AgencyCompanyCode, e.AmendmentCode, e.ClientCode, e.ContractCode, e.ContractDetailId, e.End, e.EndDate, e.Id, e.StartDate, e.UserCode, e.ValidationDate, e.AgencyFileCode, e.Available, e.Deleted, e.Signed })
                    .HasName("nci_wi_Contract_02E4F62C8629B2C6FF8AF955F046FE72");

                entity.HasIndex(e => new { e.AmendmentCode, e.ClientCode, e.ContractCode, e.ContractDetailId, e.Deleted, e.End, e.EndDate, e.Id, e.StartDate, e.TypeContractCode, e.UserCode, e.ValueReference, e.WeeklyDuration, e.AgencyFileCode, e.Available, e.Signed })
                    .HasName("nci_wi_Contract_040B5AF619A9E386762AE2198F1367D7");

                entity.HasIndex(e => new { e.AgencyCompanyCode, e.AmendmentCode, e.ClientCode, e.ContractCode, e.ContractDetailId, e.ContractSignedId, e.End, e.EndDate, e.Id, e.OriginAgencyCdi, e.Source, e.StartDate, e.TypeContractCode, e.UserCode, e.ValueReference, e.WeeklyDuration, e.Deleted, e.AgencyFileCode, e.Available, e.Signed })
                    .HasName("nci_wi_Contract_699EB96DEAAA2D778443594B59E8E0A6");

                entity.HasIndex(e => new { e.AgencyFileCode, e.AmendmentCode, e.ClientCode, e.Coefficient, e.ContractCode, e.ContractDetailId, e.ContractSignedId, e.Deleted, e.End, e.EndDate, e.Id, e.OriginAgencyCdi, e.PensionFundAddress, e.PensionFundCity, e.PensionFundName, e.PensionFundZipCode, e.SigningDate, e.StartDate, e.TypeContractCode, e.UserPosition, e.UserQualificationCode, e.ValueReference, e.WeeklyDuration, e.UserCode, e.Available, e.Signed })
                    .HasName("IX_Contract_Opt5");

                //entity.HasIndex(e => new { e.AgencyCompanyCode, e.AgencyFusionCode, e.Agreed, e.AmendmentCode, e.Available, e.ClientCode, e.Coefficient, e.CompanyCode, e.ContractCode, e.ContractDetailId, e.ContractOrigin, e.ContractSignedId, e.DataUpdated, e.Deleted, e.End, e.EndDate, e.Id, e.IdAdvContract, e.Index, e.MissionCode, e.PensionFundAddress, e.PensionFundCity, e.PensionFundName, e.PensionFundZipCode, e.PrintDate, e.Processed, e.SalesmanCode, e.Signed, e.SigningDate, e.SignOrigin, e.Source, e.StartDate, e.TypeContractCode, e.UserCategory, e.UserCode, e.UserPosition, e.UserProfileId, e.UserQualificationCode, e.ValidationDate, e.ValueReference, e.Week, e.WeeklyDuration, e.AgencyFileCode, e.OriginAgencyCdi })
                //    .HasName("nci_wi_Contract_B35E34E6046B84025AAD66570ECE97A3");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AgencyCompanyCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyFileCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyFusionCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AmendmentCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ClientCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ClientId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Coefficient).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CompanyCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.ContractDetailId)
                    .HasColumnName("ContractDetail_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ContractSignedId)
                    .HasColumnName("ContractSigned_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.End)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.IdAdvContract)
                    .HasColumnName("Id_Adv_Contract")
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.Index)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.MissionCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.OriginAgencyCdi)
                    .HasColumnName("OriginAgencyCDI")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundCity)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundZipCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SalesmanCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.TypeContractCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserCategory)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.UserPosition)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserProfileId)
                    .HasColumnName("UserProfile_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.UserQualificationCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ValueReference).HasColumnType("decimal(11, 4)");

                entity.Property(e => e.Week)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.WeeklyDuration).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_dbo.Contract_dbo.Client_ClientId");

                entity.HasOne(d => d.ContractDetail)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.ContractDetailId)
                    .HasConstraintName("FK_dbo.Contract_dbo.ContractDetail_ContractDetail_Id");

                entity.HasOne(d => d.ContractSigned)
                    .WithMany(p => p.Contract)
                    .HasForeignKey(d => d.ContractSignedId)
                    .HasConstraintName("FK_dbo.Contract_dbo.ContractSigned_ContractSigned_Id");

                //entity.HasOne(d => d.UserProfile)
                //    .WithMany(p => p.Contract)
                //    .HasForeignKey(d => d.UserProfileId)
                //    .HasConstraintName("FK_dbo.Contract_dbo.UserProfile_UserProfile_Id");
            });

            builder.Entity<ContractDetail>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_dbo.ContractDetail")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("IX_PK")
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => new { e.Id, e.EndDate })
                    .HasName("IX_VUserProfileRecent");

                entity.HasIndex(e => new { e.AgencyFileCode, e.AmendmentCode, e.ClientCode, e.ContractCode, e.Index })
                    .HasName("IX_OptMultiColumn");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Access)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyFileCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AmendmentCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AmendmentReason)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AmendmentType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.BilledAmount).HasColumnType("decimal(11, 4)");

                entity.Property(e => e.Category)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ClientCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CycleCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.End)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EndFlexibility).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Equipment)
                    .HasMaxLength(2005)
                    .IsUnicode(false);

                entity.Property(e => e.FullRubric)
                    .HasMaxLength(250)
                    .IsUnicode(false);
     
                entity.Property(e => e.Index)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Installations)
                    .HasMaxLength(2005)
                    .IsUnicode(false);

                entity.Property(e => e.JobCharacteristics)
                    .HasMaxLength(3005)
                    .IsUnicode(false);

                entity.Property(e => e.LawQuoted)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.MissionCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.NonWorkedPeriod)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrigineAgenceCdi)
                    .HasColumnName("OrigineAgenceCDI")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PayedAmount).HasColumnType("decimal(11, 4)");

                entity.Property(e => e.PersonToAsk)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.ReasonCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ReasonLabel)
                    .HasMaxLength(5010)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceHoursNumber).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RepresentingPerson)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Risks)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.RubricCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.SalaryReference)
                    .HasMaxLength(2005)
                    .IsUnicode(false);

                entity.Property(e => e.SalesmanCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Schedule)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.SiteAddress)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.SiteCity)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SiteName)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.SiteZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Source)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.StartFlexibility).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TrialDuration).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UnitRubric)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UserFreeQualificationLabel)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.UserQualificationCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            builder.Entity<ContractSigned>(entity =>
            {
                entity.HasIndex(e => e.FullCode)
                    .HasName("IX_Contract_Opt1");

                entity.HasIndex(e => new { e.ClientName, e.FullCode, e.UserCivility, e.UserFirstName, e.UserLastName, e.UserSigningDate, e.AgencyFileCode })
                    .HasName("nci_wi_ContractSigned_D68BB9D9555B793C3FA3552F9C710F28");

                //entity.HasIndex(e => new { e.AdequatMedecineAddress, e.AdequatMedecineName, e.AgeCdiiAddress, e.AgeCdiiCity, e.AgeCdiiCode, e.AgeCdiiFaxNumber, e.AgeCdiiId, e.AgeCdiiName, e.AgeCdiiPhoneNumber, e.AgeCdiiSiretNumber, e.AgeCdiiZipCode, e.AgencyAddress, e.AgencyApeCodeLabel, e.AgencyCdiiApeCodeLabel, e.AgencyCity, e.AgencyFaxNumber, e.AgencyFileCode, e.AgencyName, e.AgencyPhoneNumber, e.AgencySigningDate, e.AgencySiretNumber, e.AgencyZipCode, e.AmendmentCode, e.AmendmentReason, e.AmendmentType, e.Characteristics, e.ClientAddress, e.ClientApeCodeLabel, e.ClientCity, e.ClientMedecineName, e.ClientName, e.ClientZipCode, e.ContractSchedule, e.ContributionCenterLabel, e.End, e.EndDate, e.Equipment, e.ExternalOrderRequestId, e.ExternalSignatureRequestId, e.FinancialGuarantorLabel, e.FlexibilityEndDate, e.FlexibilityStartDate, e.FullCode, e.HourReference, e.InitialOrAmendment, e.InitialStartAndEndDates, e.Installation, e.IssueDate, e.IssuePlace, e.Justifications, e.LawQuoted, e.MedicalVisitDate, e.OffPeriod, e.OrderRequestId, e.PensionFundAddress, e.PensionFundCity, e.PensionFundName, e.PensionFundZipCode, e.PersonToAsk, e.QualificationLabel, e.ReferenceHourNumber, e.RepresentingPerson, e.Risks, e.RubricNames, e.SalaryReference1, e.SalaryValues, e.SignatureRequestId, e.SiteAccess, e.SiteAddress, e.SiteCity, e.SiteName, e.SiteZipCode, e.StartDate, e.TrialDuration, e.TypeContractCode, e.UserAddress, e.UserBirthDate, e.UserBirthPlace, e.UserCategory, e.UserCity, e.UserCivility, e.UserCode, e.UserFirstName, e.UserId, e.UserLastName, e.UserNationalityLabel, e.UserNumber, e.UserPaymentModeLabel, e.UserPosition, e.UserSigningDate, e.UserZipCode, e.ValueReference, e.Week, e.WeeklyDuration, e.AgencyId, e.Deleted })
                //    .HasName("nci_wi_ContractSigned_B5FD11AF6E4A348517870748D4B0BB2D");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AdequatMedCdiiAddress)
                    .HasMaxLength(130)
                    .IsUnicode(false);

                entity.Property(e => e.AdequatMedCdiiName)
                    .HasMaxLength(130)
                    .IsUnicode(false);

                entity.Property(e => e.AdequatMedecineAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AdequatMedecineName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiCity)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiFaxNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiPhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiSiretNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AgeCdiiZipCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyApeCodeLabel)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyCdiiApeCodeLabel)
                    .HasMaxLength(130)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyCity)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyFaxNumber)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyFileCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyPhoneNumber)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencySigningDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencySiretNumber)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.AgencyZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.AmendmentCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.AmendmentReason)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.AmendmentType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Characteristics)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ClientAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ClientApeCodeLabel)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ClientCity)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ClientMedecineName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ClientName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ClientZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ContractSchedule)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ContributionCenterCdiiLabel)
                    .HasMaxLength(130)
                    .IsUnicode(false);

                entity.Property(e => e.ContributionCenterLabel)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.End)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Equipment)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalOrderRequestId)
                    .HasColumnName("externalOrderRequestId")
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalSignatureRequestId)
                    .HasColumnName("externalSignatureRequestId")
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.FinancialGuarantorCdiiLabel)
                    .HasMaxLength(130)
                    .IsUnicode(false);

                entity.Property(e => e.FinancialGuarantorLabel)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.FlexibilityEndDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.FlexibilityStartDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.FullCode)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.HourReference)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.InitialOrAmendment)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.InitialStartAndEndDates)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Installation)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.IssueDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.IssuePlace)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Justifications)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.LawQuoted)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.MedicalVisitDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.OffPeriod)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.OrderRequestId)
                    .HasColumnName("orderRequestId")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundCity)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PensionFundZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PersonToAsk)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.QualificationLabel)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceHourNumber).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RepresentingPerson)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Risks)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.RubricNames)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SalaryReference1)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SalaryValues)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SignatureRequestId)
                    .HasColumnName("signatureRequestId")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SiteAccess)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SiteAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SiteCity)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SiteName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.SiteZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.TrialDuration)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.TypeContractCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserBirthDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserBirthPlace)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserCategory)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserCity)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserCivility)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserFirstName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserLastName)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserNationalityLabel)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserNumber)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserPaymentModeLabel)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserPosition)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserSigningDate)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.UserZipCode)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ValueReference)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Week)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.WeeklyDuration)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            builder.Entity<FaqCategory>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });

            builder.Entity<FaqQuestion>(entity =>
            {
                entity.HasIndex(e => e.FaqCategoryId)
                    .HasName("IX_FaqCategory_Id");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Detail)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.FaqCategoryId)
                    .HasColumnName("FaqCategory_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.FaqCategory)
                    .WithMany(p => p.FaqQuestion)
                    .HasForeignKey(d => d.FaqCategoryId)
                    .HasConstraintName("FK_dbo.FaqQuestion_dbo.FaqCategory_FaqCategory_Id");
            });

            builder.Entity<Flow>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            builder.Entity<LoggingTryout>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__LoggingT__3214EC0665957100")
                    .ForSqlServerIsClustered(false);

                entity.HasAnnotation("SqlServer:MemoryOptimized", true);

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.LastTry).HasColumnName("LastTry ");

                entity.Property(e => e.UnlockCode)
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.UserLogin)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            //NOTE: old entities to be removed
            builder.Entity<MockEmail>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.From)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.MailHtml)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Subject)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.To)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            builder.Entity<MockSms>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Body)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.From)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.To)
                    .HasMaxLength(8000)
                    .IsUnicode(false);
            });

            builder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK_dbo.Notification")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.ContractId)
                    .HasName("IX_Contract_Id");

                entity.HasIndex(e => e.ExecutionDate)
                    .HasName("IX_ExecutionDate");

                //entity.HasIndex(e => e.PayId)
                //    .HasName("IX_Pay_Id");

                entity.HasIndex(e => e.To)
                    .HasName("IX_Opt_To");

                entity.HasIndex(e => new { e.Mode, e.State })
                    .HasName("nci_wi_Notification_9EBAC34AC273FFC7782169241F11BD48");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AgencyFileCode)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.Content)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.ContractId)
                    .HasColumnName("Contract_Id")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.From)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                //entity.Property(e => e.PayId)
                //    .HasColumnName("Pay_Id")
                //    .HasMaxLength(32)
                //    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.To)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_dbo.Notification_dbo.Contract_Contract_Id");
            });


        }
    }
}
