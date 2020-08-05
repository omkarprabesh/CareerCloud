using CareerCloud.Pocos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerCloud.EntityFrameworkDataAccess
{
    public class CareerCloudContext: DbContext

    {
        public DbSet<ApplicantEducationPoco> ApplicantEducations { get; set; }
        public DbSet<ApplicantJobApplicationPoco> ApplicantJobApplications { get; set; }
        public DbSet<ApplicantProfilePoco> ApplicantProfiles { get; set; }
        public DbSet<ApplicantResumePoco> ApplicantResumes { get; set; }
        public DbSet<ApplicantSkillPoco> ApplicantSkills { get; set; }
        public DbSet<ApplicantWorkHistoryPoco> ApplicantWorkHistory{ get; set; }
        public DbSet<CompanyDescriptionPoco> CompanyDescriptions { get; set; }
        public DbSet<CompanyJobDescriptionPoco> CompanyJobDescriptions{ get; set; }
        public DbSet<CompanyJobEducationPoco> CompanyJobEducations{ get; set; }
        public DbSet<CompanyJobPoco> CompanyJobs{ get; set; }
        public DbSet<CompanyJobSkillPoco> CompanyJobSkills{ get; set; }
        public DbSet<CompanyLocationPoco> CompanyLocations{ get; set; }
        public DbSet<CompanyProfilePoco> CompanyProfiles{ get; set; }
        public DbSet<SecurityLoginPoco> SecurityLogins{ get; set; }
        public DbSet<SecurityLoginsLogPoco> SecurityLoginsLogs{ get; set; }
        public DbSet<SecurityLoginsRolePoco> SecurityLoginsRoles{ get; set; }
        public DbSet<SecurityRolePoco> SecurityRoles{ get; set; }
        public DbSet<SystemCountryCodePoco> SystemCountryCodes{ get; set; }
        public DbSet<SystemLanguageCodePoco> SystemLanguageCodes{ get; set; }

        protected override 
            void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
            optionsBuilder.UseSqlServer(@"Data Source=OMKARKANDEL\HUMBERBRIDGING;Initial Catalog=JOB_PORTAL_DB;Integrated Security=True");

            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
