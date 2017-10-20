using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UHWID;

namespace NK_ITMedia_License
{
    public class License
    {
        private static string k { get; set; }
        private static string e { get; set; }
        private static string y;


        public License(string key1, string key2)
        {
            k = key1;
            e = key2;
            y = k + e;
        }

        public string GenerateLicense(long timestamp, bool trial, string hwid, int productid, string firstname, string lastname, string forumname)
        {

            return StringCipher.Encrypt("{  \"timestamp\": \"" + timestamp + "\",  \"trial\": \"" + trial + "\",  \"hwid\": \"" + hwid + "\",  \"productid\": \"" + productid + "\",  \"firstname\": \"" + firstname + "\",  \"lastname\": \"" + lastname + "\",  \"forumname\": \"" + forumname + "\"}", y).Replace("=", "");
        }

        public bool ValidLicense(string license)
        {
            try
            {
                if (JsonConvert.DeserializeObject<LicenseStruct>(StringCipher.Decrypt(license + "=", y)).timestamp < (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds)
                {
                    return false;
                }
                else
                {
                    if (StringCipher.Decrypt(GetHWID() + "=", y) != StringCipher.Decrypt(JsonConvert.DeserializeObject<LicenseStruct>(StringCipher.Decrypt(license + "=", y)).hwid + "=", y))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            } catch(Exception Ex)
            {
                return false;
            }
        }

        public string GetHWID()
        {
            return StringCipher.Encrypt(new HWID().AdvancedUID, y).Replace("=", "");
        }

        public string GetCleanHWID()
        {
            return new HWID().AdvancedUID;
        }

        public bool IsHostManipulated()
        {
            IPAddress Adresse = Dns.GetHostAddresses("license.nkitmedia.de")[0];
            if(Adresse.MapToIPv4().ToString() == "151.80.125.221")
            {
                return false;
            }
            return true;
        }

        public bool IsHWIDBannedForPID(string hwid, int pid)
        {
            using (var Wc = new WebClient())
            {
                try
                {
                    var data = new NameValueCollection();
                    data["hwid"] = StringCipher.Decrypt(hwid + "=", y);
                    data["pid"] = pid.ToString();
                    var response = Wc.UploadValues("https://license.nkitmedia.de/", "POST", data);
                    return bool.Parse(System.Text.Encoding.UTF8.GetString(response));
                } catch(Exception ex)
                {
                    return false;
                }
            }
        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var Client = new WebClient())
                {
                    using (Client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public class GetLicenseInformations
        {
            private string Serial;
            private LicenseStruct License;

            public GetLicenseInformations(string Serial)
            {
                this.Serial = StringCipher.Decrypt(Serial + "=", y);
                License = JsonConvert.DeserializeObject<LicenseStruct>(this.Serial);
            }

            public Int32 Timestamp()
            {
                return License.timestamp;
            }

            public bool IsTrial()
            {
                return License.trial;
            }

            public string HWID()
            {
                return License.hwid;
            }

            public int ProductID()
            {
                return License.productid;
            }

            public string Firstname()
            {
                return License.firstname;
            }

            public string Lastname()
            {
                return License.lastname;
            }

            public string Forumname()
            {
                return License.forumname;
            }

            public bool IsHWIDEqualsLicHWID(string LicHWID, string LocalHWID)
            {
                if(StringCipher.Decrypt(LicHWID, y) == StringCipher.Decrypt(LocalHWID, y))
                {
                    return true;
                } else
                {
                    return false;
                }
            }
                        
        }

        public class LicenseStruct
        {
            public Int32 timestamp { get; set; }
            public bool trial { get; set; }
            public string hwid { get; set; }
            public int productid { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string forumname { get; set; }
        }

        public class LicenseWebResult
        {
            public bool banned { get; set; }
        }
    }
}
