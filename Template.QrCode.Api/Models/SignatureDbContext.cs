using Template.QrCode.Api.Infrastructure.Data.Configuration;
using Template.QrCode.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Models
{

    public class SignatureDbContext : DbContext
    {
        //public DbSet<TestStructureModel> ListSignature { get; set; }
        public DbSet<FanIdRequest> FanIdRequests { get; set; }

        public SignatureDbContext(DbContextOptions<SignatureDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<TestStructureModel>().ToTable("CountryCertificate");
            modelBuilder.ApplyConfiguration(new FanIdRequestConfiguration());
        }
    }

}
