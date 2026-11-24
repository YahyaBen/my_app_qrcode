using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SchemaModel = Template.QrCode.Api.Models.Schema.Schema;

namespace Template.QrCode.Api.Models
{
    public static class SchemaValues
    {
        public static string Location { get; set; }
        private static string _Location { get; set; }
        private static SchemaModel _prophylaxis;

        public static SchemaModel prophylaxis
        {
            get
            {
                if (_prophylaxis != null)
                    return _prophylaxis;
                else
                {
                    _prophylaxis = ReadSchema(Location + "/Certificats/" + "vaccine-prophylaxis.json");
                    return _prophylaxis;
                }
            }
            set { _prophylaxis = value; }
        }

        public static SchemaModel _VaccineMedicinalProduct;

        public static SchemaModel VaccineMedicinalProduct
        {
            get
            {
                if (_VaccineMedicinalProduct != null)
                    return _VaccineMedicinalProduct;
                else
                {
                    _VaccineMedicinalProduct = ReadSchema(Location + "/Certificats/" + "vaccine-medicinal-product.json");
                    return _VaccineMedicinalProduct;
                }
            }
            set { _VaccineMedicinalProduct = value; }
        }
        public static SchemaModel _Manufacter;
        public static SchemaModel Manufacter
        {
            get
            {
                if (_Manufacter != null)
                    return _Manufacter;
                else
                {
                    _Manufacter = ReadSchema(Location + "/Certificats/" + "vaccine-mah-manf.json");
                    return _Manufacter;
                }
            }
            set { _Manufacter = value; }
        }
        public static SchemaModel _TestType;

        public static SchemaModel TestType
        {
            get
            {
                if (_TestType != null)
                    return _TestType;
                else
                {
                    _TestType = ReadSchema(Location + "/Certificats/" + "test-type.json");
                    return _TestType;
                }
            }
            set { _Manufacter = value; }
        }
        public static SchemaModel _TestManufactor;
        public static SchemaModel TestManufactor
        {
            get
            {
                if (_TestManufactor != null)
                    return _TestManufactor;
                else
                {
                    _TestManufactor = ReadSchema(Location + "/Certificats/" + "test-manf.json");
                    return _TestManufactor;
                }
            }
            set { _TestManufactor = value; }
        }
        public static SchemaModel _TestResult;
        public static SchemaModel TestResult
        {
            get
            {
                if (_TestResult != null)
                    return _TestResult;
                else
                {
                    _TestResult = ReadSchema(Location + "/Certificats/" + "test-result.json");
                    return _TestResult;
                }
            }
            set { _TestResult = value; }
        }
        private static SchemaModel ReadSchema(string Path)
        {

            try
            {
                JsonTextReader reader = new JsonTextReader(new StreamReader(Path));


                using (StreamReader r = new StreamReader(Path))
                {
                    string json = r.ReadToEnd();
                    SchemaModel items = JsonConvert.DeserializeObject<SchemaModel>(json);


                    return items;

                }

            }
            catch (Exception ex)
            {

                return null;
            }


        }
    }
}
