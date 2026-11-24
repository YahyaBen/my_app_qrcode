using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Models
{
    public enum StatutCertificateEnum : int
    {
        OK = 1 ,
        CertificateExpired = 2,
        CertificateIsRevoked = 3 ,
        CertificateNotFound = 4,
        DateVaccinationExpired = 5,
        Error = -1 ,
        InvalidTokenOrId = -2,
        TokenExpired = -3,
        IdNotFound = -4,
   
    }
    public enum RequestStatusCode
    {
        Initiated = 1,
        InProgressSecurityCheck = 2,
        Validated = 3,
        RejectedSecurityCheck = 4,
        ApprovedSecurityCheck = 5,
        PartiallyValidated = 6,
        PartiallyApprovedSecurityCheck = 7,
        ApprovedSecurityCheckEvisa = 8,
        ApprovedSecurityCheckAevm = 9,
        PartiallyApprovedSecurityCheckEvisa = 10,
        PartiallyApprovedSecurityCheckAevm = 11
    }
}
