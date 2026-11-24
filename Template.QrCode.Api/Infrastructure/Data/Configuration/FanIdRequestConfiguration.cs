
using Template.QrCode.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Template.QrCode.Api.Infrastructure.Data.Configuration
{
    public class FanIdRequestConfiguration : IEntityTypeConfiguration<FanIdRequest>
    {
        public void Configure(EntityTypeBuilder<FanIdRequest> builder)
        {
            builder.ToTable("DemandeFanId");


            builder.HasKey(e => e.RequestId);
            builder.Property(e => e.RequestId).HasColumnName("DemandeId");

            builder.Property(e => e.FanIdPrefix)
                    .HasMaxLength(50)
                    .HasColumnName("FanIdPrefix");

            builder.Property(e => e.RequestNumber)
                .HasColumnName("NumeroDemande")
                .HasMaxLength(50);

            builder.Property(e => e.IdentityDocumentTypeId)
                .HasColumnName("TypeDocumentIdentiteId");

            builder.Property(e => e.CnieNumber)
                .HasMaxLength(10)
                .HasColumnName("NumeroCNIE");

            builder.Property(e => e.ResidenceCardNumber)
                .HasMaxLength(10)
                .HasColumnName("NumeroCarteSejour");

            builder.Property(e => e.PassportNumber)
                .HasMaxLength(20)
                .HasColumnName("NumeroPasseport");

            builder.Property(e => e.IdentityDocumentExpirationDate)
                .HasColumnName("DateExpirationDocumentIdentite");

            builder.Property(e => e.IdentityDocumentIssueDate)
                .HasColumnName("DateEmissionDocumentIdentite");

            builder.Property(e => e.IdentityAuthority)
                .HasMaxLength(6)
                .HasColumnName("AutoriteIdentite");

            builder.Property(e => e.PassportType)
                .HasMaxLength(5)
                .HasColumnName("TypePasseport");

            builder.Property(e => e.Gender)
                .HasMaxLength(6)
                .HasColumnName("Genre");

            builder.Property(e => e.ApplicantLastNameFr)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("NomDemandeurFr");

            builder.Property(e => e.ApplicantFirstNameFr)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("PrenomDemandeurFr");

            builder.Property(e => e.FanId)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnName("FanId");

            builder.Property(e => e.ApplicantLastNameAr)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("NomDemandeurAr");

            builder.Property(e => e.ApplicantFirstNameAr)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("PrenomDemandeurAr");

            builder.Property(e => e.ApplicantBirthDate)
                .IsRequired()
                .HasColumnName("DateNaissanceDemandeur");

            builder.Property(e => e.ApplicantNationality)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("NationaliteDemandeur");

            builder.Property(e => e.CategoryId)
                .HasColumnName("CategorieId");

            builder.Property(e => e.SubCategoryId)
                .HasColumnName("SousCategorieId");

            builder.Property(e => e.Priority)
                .HasColumnName("Priorite");

            builder.Property(e => e.GuardianIdentityDocumentTypeId)
                .HasColumnName("TypeDocumentIdentiteTuteurId");

            builder.Property(e => e.GuardianIdentityNumber)
                .HasMaxLength(10)
                .HasColumnName("NumeroIdentiteTuteur");

            builder.Property(e => e.GuardianRequestNumber)
                .HasMaxLength(50)
                .HasColumnName("NumeroDemandeTuteur");

            builder.Property(e => e.RequestDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .HasColumnName("DateDemande");

            builder.Property(e => e.ManualEntry)
                .IsRequired()
                .HasColumnName("SaisieManuelle");

            builder.Property(e => e.NeedsEVisa)
                .IsRequired()
                .HasColumnName("ABesoinEVisa");

            builder.Property(e => e.BackOfficeRequest)
                .IsRequired()
                .HasColumnName("DemandeBackOffice");

            builder.Property(e => e.BackOfficeAgentId)
                .HasMaxLength(50)
                .HasColumnName("AgentBackOfficeId");

            builder.Property(e => e.ParentId)
                .HasColumnName("ParentId");

            builder.Property(e => e.UserId)
                .HasColumnName("UserId");

            builder.Property(e => e.StatusId)
                .HasColumnName("StatutDemandeId");

            builder.Property(e => e.StatusDate)
                .HasColumnName("DateStatut");

            builder.Property(e => e.IsAdult)
                .IsRequired()
                .HasColumnName("IsMajeur");

            builder.Property(e => e.VisaNumber)
                .HasMaxLength(50)
                .HasColumnName("NumVisa");

            builder.Property(e => e.VisaEligibility)
                .HasMaxLength(50)
                .HasColumnName("EligibiliteVisa");



            //builder.HasOne(e => e.Parent)
            //    .WithMany(d => d.Minors)
            //    .HasForeignKey(e => e.ParentId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //  builder.Property(u => u.IsArchived)
            //.HasDefaultValue(false)
            //.IsRequired();

            builder.Property(u => u.BirthDateDgsn)
                  .HasMaxLength(10)
                  .IsRequired(false);

            builder.HasQueryFilter(u => !u.IsArchived);

        }
    }
}