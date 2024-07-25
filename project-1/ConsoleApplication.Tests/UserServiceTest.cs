namespace ConsoleApplication.Tests;

using Program.DAO;
using Program.Model;

public class UserServiceTest
{
    private readonly UserDAO _userDAO;
    public UserServiceTest()
    {
        _userDAO = new UserDAO();
    }

    ~UserServiceTest()
    {
        Console.ResetColor();
        Console.CursorVisible = true;
    }


    [Theory]
    [InlineData("q", "q")]
    [InlineData("qw", "qw")]
    [InlineData("qwe", "qwe")]
    [InlineData("qwert", "qwert")]
    public void AddUser_Pass(String name, String password)
    {
        // When adding a new valid user, AddUser should return that user.

        // Arrange
        User? exists = _userDAO.GetUserByName(name);
        if(exists != null) _userDAO.RemoveUser(exists);

        // Act
        User? newUser = _userDAO.AddUser(new User{Name = name, Password = password});

        // Assert
        Assert.NotNull(newUser);
    }

    [Theory]
    [InlineData("q", "q")]
    [InlineData("qw", "qw")]
    [InlineData("qwe", "qwe")]
    [InlineData("qwert", "qwert")]
    public void AddUser_Null(String name, String password)
    {
        // When adding an existing user, AddUser should return null to indicate that the user already exists.

        // Arrange
        _userDAO.AddUser(new User{Name = name, Password = password});

        // Act
        User? newUser = _userDAO.AddUser(new User{Name = name, Password = password});

        // Assert
        Assert.Null(newUser);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("null", null)]
    [InlineData(null,"null")]
    [InlineData("null", "")]
    [InlineData("","null")]
    public void AddUser_ArgumentException(String name, String password)
    {
        // When Adding an invalid user, AddUser should throw an ArgumentException to indicate that the user is invalid.

        Assert.Throws<ArgumentException>(() => {
            _userDAO.AddUser(new User{Name = name, Password = password});
        });
    }

    [Theory]
    [InlineData("qwerty", "somepass")]
    [InlineData("12154", "somepass")]
    public void RemoveUser_Pass(String name, String password)
    {
        // When removing an existing valid user. RemoveUser should return the removed user.

        // Arrange
        User? newUser = _userDAO.AddUser(new User{Name = name, Password = password});
        Assert.True(newUser != null);

        // Act
        User? deleted = _userDAO.RemoveUser(newUser);

        // Assert
        Assert.True(deleted != null);
    }

    [Theory]
    [InlineData(null, "null")]
    [InlineData("", "null")]
    [InlineData("null", "")]
    [InlineData("null", null)]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void RemoveUser_ArgumentException(String name, String password)
    {
        // When removing an non-existent non-valid user. RemoveUser should throw an ArgumentException.

        Assert.Throws<ArgumentException>(() => {
            _userDAO.RemoveUser(new User{Name = name, Password = password});
        });
    }

    [Theory]
    [InlineData("NonExistant1", "somepass")]
    [InlineData("NonExistant", "somepass")]
    public void RemoveUser_Null(String name, String password)
    {
        // When removing an non-existing valid user. RemoveUser should return null to indicate that no user to remove was found.

        User? user = _userDAO.RemoveUser(new User{Name = name, Password = password});

        Assert.Null(user);
    }
}