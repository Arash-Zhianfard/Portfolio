using Abstraction.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace Service.Implementation
{
    public class EncryptService : IEncryptService
    {
        public string GenerateMd5String(string value)
        {
            var sb = new StringBuilder();

            foreach (byte b in GenerateMd5(value))
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
        public byte[] GenerateMd5(string value)
        {
            using (var hash = MD5.Create())
            {
                var enc = Encoding.UTF8;
                return hash.ComputeHash(enc.GetBytes(value));
            }
        }

    }
}
