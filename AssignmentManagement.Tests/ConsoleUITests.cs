using Xunit;
using Moq;
using AssignmentManagement.Console;
using System.IO;
using AssignmentManagement.UI;
using AssignmentManagement.Core.Interfaces;
using AssignmentManagement.Core.Models;

namespace AssignmentManagement.Tests
{
    public class ConsoleUITests
    {
        [Fact]
        public void AddAssignment_Should_Call_Service_Add()
        {
            // Arrange
            var mock = new Mock<IAssignmentService>();
            mock.Setup(s => s.AddAssignment(It.IsAny<Assignment>())).Returns(true);
            var ui = new ConsoleUI(mock.Object);
            var input = new StringReader("1\nTitle\nDescription\n\nL\nNotes\n0\n");
            System.Console.SetIn(input);

            // Act
            ui.Run();

            // Assert
            mock.Verify(m => m.AddAssignment(It.IsAny<Assignment>()), Times.Once);
        }

        [Fact]
        public void SearchAssignmentByTitle_Should_Display_Assignment()
        {
            // Arrange
            var mock = new Mock<IAssignmentService>();
            mock.Setup(s => s.FindAssignmentByTitle("Sample"))
                .Returns(new Assignment("Sample", "Details", null, AssignmentPriority.Low, ""));
            var ui = new ConsoleUI(mock.Object);
            var input = new StringReader("5\nSample\n0\n");
            System.Console.SetIn(input);

            // Act
            ui.Run();

            // Assert
            mock.Verify(s => s.FindAssignmentByTitle("Sample"), Times.Once);
        }

        [Fact]
        public void DeleteAssignment_Should_Call_Service_Delete()
        {
            // Arrange
            var mock = new Mock<IAssignmentService>();
            mock.Setup(s => s.DeleteAssignment("ToDelete")).Returns(true);
            var ui = new ConsoleUI(mock.Object);
            var input = new StringReader("7\nToDelete\n0\n");
            System.Console.SetIn(input);

            // Act
            ui.Run();

            // Assert
            mock.Verify(s => s.DeleteAssignment("ToDelete"), Times.Once);
        }
    }
}
