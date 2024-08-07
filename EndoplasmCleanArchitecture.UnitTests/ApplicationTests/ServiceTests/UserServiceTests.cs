using EndoplasmCleanArchitecture.Application.Dtos.User;
using EndoplasmCleanArchitecture.Application.Interfaces;
using EndoplasmCleanArchitecture.Application.Interfaces.User;
using EndoplasmCleanArchitecture.Application.Services.User;
using EndoplasmCleanArchitecture.Domain.Entities;
using Moq;

namespace EndoplasmCleanArchitecture.UnitTests.ApplicationTests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);
            _userService = new UserService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateUser_WhenUserDoesNotExist()
        {
            // Arrange
            var createUserDto = new CreateUserRequestDto
            {
                UserName = "newuser",
                Password = "password"
            };

            _mockUserRepository.Setup(r => r.IsUserExists(It.IsAny<string>())).ReturnsAsync(false);
            _mockUserRepository.Setup(r => r.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.True(result);
            _mockUserRepository.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange
            var createUserDto = new CreateUserRequestDto
            {
                UserName = "existinguser",
                Password = "password"
            };

            _mockUserRepository.Setup(r => r.IsUserExists(It.IsAny<string>())).ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.CreateUserAsync(createUserDto));
            Assert.Equal("existinguser adlý kullanýcý sistemde mevcut", exception.Message);
        }

        [Fact]
        public async Task InsertRefreshToken_ShouldInsertToken_WhenUserExists()
        {
            // Arrange
            var userName = "existinguser";
            var refreshToken = "refreshToken";
            var expiration = DateTime.UtcNow.AddDays(1);

            var user = new User { UserName = userName };
            _mockUserRepository.Setup(r => r.GetByUserName(It.IsAny<string>())).ReturnsAsync(user);
            _mockUserRepository.Setup(r => r.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userService.InsertRefreshToken(userName, refreshToken, expiration);

            // Assert
            Assert.True(result);
            _mockUserRepository.Verify(r => r.Update(It.Is<User>(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime == expiration)), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InsertRefreshToken_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userName = "nonexistinguser";
            _mockUserRepository.Setup(r => r.GetByUserName(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _userService.InsertRefreshToken(userName, "refreshToken", DateTime.UtcNow));
            Assert.Equal("nonexistinguser username bulunamadý.", exception.Message);
        }
    }
}