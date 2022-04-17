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
        private Mock<IUserService>? _userService;
        private Mock<IEncryptService>? _encryptService;
        private AuthService? _authService;
        private IOptions<JwtSetting>? _jwtSetting;
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
            //arrange
            var tokenInfo = _authService.SignIn("username", "password").Result;
            //act
            var result = _authService.ValidateToken(tokenInfo.Token);
            //assert
            Assert.IsTrue(result);
        }
        [Test]
        public void ValidateToken_ShouldBeFalseWthWrongToken()
        {
            string wrongToken = "23r32sfs3df3sdsdfaslklgsdf";
            var result = _authService.ValidateToken(wrongToken);
            //assert
            Assert.IsFalse(result);
        }
        [Test]
        public void SignIn_ShouldReturnNullIfUserNotFound()
        {
            //arrange
            var signInObj = _authService.SignIn("arash", "455d").Result;
            //assert
            Assert.IsNull(signInObj);
        }
        [Test]
        public void SignIn_ShouldReturnTokenIfUserFound()
        {  
            //arrange
            var signInObj = _authService.SignIn("username", "password").Result;
            //assert
            Assert.IsNotEmpty(signInObj.Token);
        }

        [Test]
        public void GetTokenInfo_ShouldReturnExpectedUserId()
        {
            //arrange
            var signInObj = _authService.SignIn("username", "password").Result;
            //act
            var result = _authService.GetTokenInfo(signInObj.Token);
            //assert
            Assert.AreEqual(result.UserId, userId);
        }

        [Test]
        public void SignUp_ShouldReturnToken()
        {
            //arrange
            var result = _authService.SignUp(new SignupRequest()
            {
                Username = "username",
                Password = "password"
            }).Result;
            //act
            Assert.IsNotEmpty(result.Token);
        }
    }
}
