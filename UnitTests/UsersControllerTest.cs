using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MikroCopUsers.Controllers;
using MikroCopUsers.Data;
using MikroCopUsers.Models;
using MikroCopUsers.Services;
using Xunit;

public class UsersControllerTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetUsers_ReturnsCorrectUser()
    {
        // Arrange
        var context = GetInMemoryDbContext();

        var testUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = "Mihael",
            Culture = "Croatian",
            Email = "mihael.car@hotmail.com",
            FullName = "Mihael Car",
            Language = "Croatian",
            MobileNumber = "12345",
            PasswordHash = "TheCakeIsALie"
        };

        context.Users.Add(testUser);
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<UsersController>();
        var passwordHasher = new PasswordHasher();
        passwordHasher.Hash(testUser.PasswordHash);
        var controller = new UsersController(context, logger, passwordHasher);

        // Act
        var result = await controller.GetUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsType<List<User>>(okResult.Value);

        Assert.Single(users); // Only 1 user expected
        Assert.Equal("Mihael Car", users[0].FullName);
    }

    [Fact]
    public async Task ValidatePassword_ReturnsOk_WhenPasswordIsValid()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var logger = new LoggerFactory().CreateLogger<UsersController>();
        var passwordHasher = new PasswordHasher();

        var plainPassword = "test123";
        var hashedPassword = passwordHasher.Hash(plainPassword);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "mihael",
            PasswordHash = hashedPassword,
            Email = "mihael@example.com",
            FullName = "Mihael Car",
            MobileNumber = "123456789",
            Language = "Croatian",
            Culture = "Croatian"
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var controller = new UsersController(dbContext, logger, passwordHasher);

        var loginRequest = new UsersController.LoginRequest
        {
            UserName = "mihael",
            Password = plainPassword
        };

        // Act
        var result = await controller.ValidatePassword(loginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Password is valid", okResult.Value);
    }

    [Fact]
    public async Task ValidatePassword_ReturnsUnauthorized_WhenPasswordIsInvalid()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var logger = new LoggerFactory().CreateLogger<UsersController>();
        var passwordHasher = new PasswordHasher();

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "mihael",
            PasswordHash = passwordHasher.Hash("correctpassword"),
            Email = "mihael@example.com",
            FullName = "Mihael Car",
            MobileNumber = "123456789",
            Language = "Croatian",
            Culture = "Croatian"
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var controller = new UsersController(dbContext, logger, passwordHasher);

        var loginRequest = new UsersController.LoginRequest
        {
            UserName = "mihael",
            Password = "wrongpassword"
        };

        // Act
        var result = await controller.ValidatePassword(loginRequest);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid password", unauthorizedResult.Value);
    }

    [Fact]
    public async Task ValidatePassword_ReturnsUnauthorized_WhenUserDoesNotExist()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var logger = new LoggerFactory().CreateLogger<UsersController>();
        var passwordHasher = new PasswordHasher();

        var controller = new UsersController(dbContext, logger, passwordHasher);

        var loginRequest = new UsersController.LoginRequest
        {
            UserName = "nonexistent",
            Password = "any"
        };

        // Act
        var result = await controller.ValidatePassword(loginRequest);

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("User not found", unauthorizedResult.Value);
    }
}