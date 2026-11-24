using System;
using System.Collections.Generic;

namespace Template.QrCode.Api.Models.Entities
{
	public class FanIdRequest
	{
        public int RequestId { get; set; }
        public string RequestNumber { get; set; } = string.Empty;
        public string? FanIdPrefix { get; set; } = string.Empty;
        public string? FanId { get; set; } = string.Empty;

        public int? IdentityDocumentTypeId { get; set; } 
        public string? CnieNumber { get; set; }
        public string? ResidenceCardNumber { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? IdentityDocumentExpirationDate { get; set; }
        public DateTime? IdentityDocumentIssueDate { get; set; }
        public string? IdentityAuthority { get; set; }
        public string? PassportType { get; set; }
        public string? Gender { get; set; }

        public string ApplicantLastNameFr { get; set; } = string.Empty;
        public string ApplicantFirstNameFr { get; set; } = string.Empty;
        public string ApplicantLastNameAr { get; set; } = string.Empty;
        public string ApplicantFirstNameAr { get; set; } = string.Empty;

        public DateTime ApplicantBirthDate { get; set; }
        public string ApplicantNationality { get; set; } = string.Empty;

        public int CategoryId { get; set; } 

        public int? SubCategoryId { get; set; } 

        public short? Priority { get; set; }
        public int? GuardianIdentityDocumentTypeId { get; set; }
        public string? GuardianIdentityNumber { get; set; }
        public string? GuardianRequestNumber { get; set; }

        public DateTime RequestDate { get; set; }
        public bool ManualEntry { get; set; }
        public bool NeedsEVisa { get; set; } = false;
        public bool BackOfficeRequest { get; set; } = false;
        public string? BackOfficeAgentId { get; set; }

        public int? ParentId { get; set; }
        public Guid? UserId { get; set; }
        public FanIdRequest? Parent { get; set; }

        public int StatusId { get; set; } 
        public DateTime? StatusDate { get; set; }
        public bool IsAdult { get; set; }
        public string? VisaNumber { get; set; }
        public string? VisaEligibility { get; set; }
        public bool IsArchived { get; set; }
        public string? Email { get; set; }
        public string? BirthDateDgsn { get; set; }



    }
}