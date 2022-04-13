using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Service.Implementation;

namespace Tests
{
    public class AuthServiceTest
    {
        private Mock<IUserService> _userService;
        private Mock<IEncryptService> _encryptService;
        private AuthService _authService;
        private IOptions<JwtSetting> _jwtSetting;
        private const int userId = 1;
        [SetUp]
        public void Setup()
        {
            _userService = new Mock<IUserService>();
            _encryptService = new Mock<IEncryptService>();
            _encryptService.Setup(x => x.GenerateMd5String(It.IsAny<string>())).Returns("encrypedPass");
            _userService.Setup(x => x.GetAsync("username", "encrypedPass")).ReturnsAsync(new User()
            {
                Id = userId,
  
                Password = "encrypedPass",
                UserName = "username"
            });
            _userService.Setup(x => x.AddAsync(It.IsAny<User>())).ReturnsAsync(new User()
            {
                Id = userId,
                Password = "encrypedPass",
                UserName = "username"
            });
            _jwtSetting = Options.Create(new JwtSetting()
            {
                SecretKey = "2584546464646456464646466k"
            });
            _authService = new AuthService(_userService.Object, _jwtSetting, _encryptService.Object);
        }

        [Test]
        public void ValidateToken_ShouldReturnTrue()
        {
            var tokenInfo = _authService.SignIn("username", "password").Result;
            var result = _authService.ValidateToken(tokenInfo.Token);
            Assert.IsTrue(result);
        }
        [Test]
        public void ValidateToken_ShouldBeFalseWthWrongToken()
        {
            string wrongToken = "23r32sfs3df3sdsdfaslklgsdf";
            var result = _authService.ValidateToken(wrongToken);
            Assert.IsFalse(result);
        }
        [Test]
        public void SignIn_ShouldReturnNullIfUserNotFound()
        {
            var signInObj = _authService.SignIn("arash", "455d").Result;
            Assert.IsNull(signInObj);
        }
        [Test]
        public void SignIn_ShouldReturnTokenIfUserFound()
        {
            var signInObj = _authService.SignIn("username", "password").Result;
            Assert.IsNotEmpty(signInObj.Token);
        }

        [Test]
        public void GetTokenInfo_ShouldReturnExpectedUserId()
        {
            var signInObj = _authService.SignIn("username", "password").Result;
            var result = _authService.GetTokenInfo(signInObj.Token);
            Assert.AreEqual(result.UserId, userId);
        }

        [Test]
        public void SignUp_ShouldReturnToken()
        {
            var result = _authService.SignUp(new SignupRequest()
            {
                Username = "username",
                Password = "password"
            }).Result;
            Assert.IsNotEmpty(result.Token);
        }
    }
}
