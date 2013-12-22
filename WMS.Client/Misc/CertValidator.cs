using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Client.Misc
{
    /// <summary>
    /// Validator certyfikatów
    /// </summary>
    public class CertValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            //if (certificate == null || certificate.SubjectName.Name != "CN=TestCert")
            //    throw new SecurityTokenValidationException("Błędny certyfikat!");
        }
    }
}
