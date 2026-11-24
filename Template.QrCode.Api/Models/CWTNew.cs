using DCC;
using Newtonsoft.Json;
using PeterO.Cbor;
using System;

namespace Template.QrCode.Api.Models
{
    public class CWTNew
    {
        public Sign1CoseMessage CoseMessage { get; private set; }
        //public DgCertificateNew DGCv1 { get; set; }
        public DgCertificateImage DGCv1 { get; set; }
        public string Issuer { get; set; }

        public DateTime IssueAt { get; set; }
        public DateTime ExpirationTime { get; set; }

        const int Header_Iss = 1;
        const int Header_IAT = 6;
        const int Header_EXP = 4;
        const int Header_HCERT = -260;

        internal static CWTNew DecodeFromBytes(Sign1CoseMessage coseMessage)
        {
            var cbor = CBORObject.DecodeFromBytes(coseMessage.Content);
            var cwt = new CWTNew();
            cwt.CoseMessage = coseMessage;

            cwt.Issuer = cbor[Header_Iss].AsString();
            cwt.IssueAt = DateTimeOffset.FromUnixTimeSeconds(cbor[Header_IAT].AsNumber().ToInt64Unchecked()).DateTime;
            cwt.ExpirationTime = DateTimeOffset.FromUnixTimeSeconds(cbor[Header_EXP].AsNumber().ToInt64Unchecked()).DateTime;

            var hcert = cbor[Header_HCERT];
            var dgcJson = hcert[1].ToJSONString();

            //cwt.DGCv1 = JsonConvert.DeserializeObject<DgCertificateNew>(dgcJson);
            cwt.DGCv1 = JsonConvert.DeserializeObject<DgCertificateImage>(dgcJson);

            return cwt;
        }

        internal byte[] EncodeToBytes()
        {
            var cbor = CBORObject.NewMap();

            cbor[Header_Iss] = CBORObject.FromObject(Issuer);
            cbor[Header_EXP] = CBORObject.FromObject(ToUnixTime(ExpirationTime));
            cbor[Header_IAT] = CBORObject.FromObject(ToUnixTime(IssueAt));

            var cborHcer = CBORObject.NewMap();

            var json = JsonConvert.SerializeObject(DGCv1);
            cborHcer[1] = CBORObject.FromJSONString(json);

            cbor[Header_HCERT] = cborHcer;

            return cbor.EncodeToBytes();
        }

        private static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}