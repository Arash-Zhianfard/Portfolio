using NUnit.Framework;
using Service.Implementation;

namespace Tests
{
    public class EncryptServiceTest
    {

        [Test]
        [TestCase("", "d41d8cd98f00b204e9800998ecf8427e")]
        [TestCase("a", "0cc175b9c0f1b6a831c399e269772661")]
        [TestCase("abc", "900150983cd24fb0d6963f7d28e17f72")]
        public void SignIn_ShouldReturnTokenIfUserFound(string input,string expect)
        {
            var jwtService = new EncryptService();
            var result = jwtService.GenerateMd5String(input);
            Assert.AreEqual(result, expect);
        }
    }
}
