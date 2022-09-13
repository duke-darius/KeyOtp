using OtpNet;
using System;

namespace KeyOtp
{
    public class LocalOtpDefinition
    {
        public string Name { get; set; } = null!;
        public string Key { get; set; } = null!;

        public LocalOtpDefinition(string name, string key)
        {
            Name = name;
            Key = key;
        }

        internal string GetCode()
        {
            var otp = new Totp(Base32Encoding.ToBytes(Key));
            return otp.ComputeTotp(DateTime.UtcNow);
        }
    }
}