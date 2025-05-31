using AssignmentManagement.Core;
using AssignmentManagement.Core.Interfaces;
using AssignmentManagement.Core.Models;
using AssignmentManagement.Core.Services;
using Moq;
using System;
using Xunit;

namespace AssignmentManagement.Tests
{
    public class AssignmentTests
    {
        [Fact]
        public void AddAssignment_ShouldAddSaveNotes()
        {
            // Arrange
            string title = "Read Chapter 1";
            string description = "Summarize key points";
            string notes = "Initial notes";
            DateTime? dueDate = new DateTime(2025, 12, 31);
            Priority priority = Priority.Medium;

            // Act
            var assignment = new Assignment(title, description, notes, dueDate, priority);

            // Assert
            Assert.Equal("Initial notes", assignment.Notes);
        }

        [Fact]
        public void AddAssignment_ShouldLogMessage()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);

            var assignment = new Assignment("Test Title", "Test Description", "Test Notes", DateTime.Now.AddDays(-1), Priority.Medium);

            // Act
            service.AddAssignment(assignment);

            // Assert
            mockLogger.Verify(logger => logger.Log(It.Is<string>(msg => msg.Contains("Added Assignment"))), Times.Once);
        }

        [Fact]
        public void Constructor_ValidInput_ShouldCreateAssignment()
        {
            // Arrange
            string title = "Read Chapter 2";
            string description = "Summarize key points";
            string notes = "Note";
            DateTime? dueDate = new DateTime(2025, 12, 30);
            Priority priority = Priority.Low;

            // Act
            var assignment = new Assignment(title, description, notes, dueDate, priority);

            // Assert
            Assert.Equal(title, assignment.Title);
            Assert.Equal(description, assignment.Description);
            Assert.Equal(notes, assignment.Notes);
            Assert.Equal(dueDate, assignment.DueDate);
            Assert.Equal(priority, assignment.Priority);
            Assert.False(assignment.IsCompleted);
        }

        [Fact]
        public void Constructor_BlankTitle_ShouldThrowException()
        {
            // Arrange
            string title = "";
            string description = "Valid description";
            string notes = "";
            DateTime? dueDate = null;
            Priority priority = Priority.Low;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new Assignment(title, description, notes, dueDate, priority));
        }

        [Fact]
        public void Update_BlankDescription_ShouldThrowException()
        {
            var assignment = new Assignment("Read Chapter 2", "Initial Desc", "Notes", null, Priority.Low);

            Assert.Throws<ArgumentException>(() =>
                assignment.Update("New Title", "", "New Notes", null, null));
        }

        [Fact]
        public void MarkComplete_SetsIsCompletedToTrue()
        {
            // Arrange
            var assignment = new Assignment("Task", "Complete the lab", "Lab notes", null, Priority.Low);

            // Act
            assignment.MarkComplete();

            // Assert
            Assert.True(assignment.IsCompleted);
        }

        [Fact]
        public void ToString_WithNotes_ShouldShowNotesInOutput()
        {
            // Arrange
            var assignment = new Assignment(
                title: "Write Report",
                description: "Finish quarterly report",
                notes: "Make data chart",
                dueDate: new DateTime(2025, 12, 1),
                priority: Priority.Medium);

            // Act
            var output = assignment.ToString();

            // Assert
            Assert.Contains("Notes: Make data chart", output);
        }

        [Fact]
        public void IsOverdue_ShouldReturnFalse_WhenAssignmentIsCompleted()
        {
            // Arrange
            var dueYesterday = DateTime.Now.AddDays(-1);
            var assignment = new Assignment("Title", "Desc", "Note", dueYesterday, Priority.Medium);
            assignment.MarkComplete();

            // Act
            var isOverdue = assignment.IsOverdue();

            // Assert
            Assert.False(isOverdue);
        }

        [Fact]
        public void CheckOverdue_ShouldLogOverdueStatus()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var mockFormatter = new Mock<IAssignmentFormatter>();
            var service = new AssignmentService(mockFormatter.Object, mockLogger.Object);

            var assignment = new Assignment("Test Title", "Test Description", "Test Notes", DateTime.Now.AddDays(-2), Priority.Medium);
            service.AddAssignment(assignment);

            // Act
            service.CheckOverdue("Test Title");

            // Assert
            mockLogger.Verify(logger => logger.Log(It.Is<string>(msg => msg.Contains("Checked overdue status"))), Times.Once);
        }
    }
}
