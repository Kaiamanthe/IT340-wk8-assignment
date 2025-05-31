using AssignmentManagement.Core;
using AssignmentManagement.Core.Interfaces;
using AssignmentManagement.Core.Models;
using AssignmentManagement.UI;
using Moq;
using System;
using System.IO;
using Xunit;

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
            var input = new StringReader("1\nTest Title\nTest Description\nSome notes\n2025-12-31\nH\n0\n");
            System.Console.SetIn(input);

            // Act
            ui.Run();

            // Assert
            mock.Verify(m => m.AddAssignment(It.Is<Assignment>(
                a => a.Title == "Test Title" &&
                     a.Description == "Test Description" &&
                     a.Notes == "Some notes" &&
                     a.DueDate == DateTime.Parse("2025-12-31") &&
                     a.Priority == Priority.High)), Times.Once);
        }

        [Fact]
        public void SearchAssignmentByTitle_Should_Display_Assignment()
        {
            // Arrange
            var mock = new Mock<IAssignmentService>();
            mock.Setup(s => s.FindAssignmentByTitle("Sample"))
                .Returns(new Assignment("Sample", "Details", "Note", new DateTime(2025, 12, 25), Priority.Medium));
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
