
using AssignmentManagement.Core;
using AssignmentManagement.Core.Models;
using AssignmentManagement.Core.Services;
using Xunit;

namespace AssignmentManagement.Tests
{
    public class AssignmentServiceTests
    {
        [Fact]
        public void AddAssignment_StoresAssignmentCorrectly()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a = new Assignment("Title", "Desc", "", null, Priority.Low);
            service.AddAssignment(a);
            Assert.Contains(a, service.ListAll());
        }

        [Fact]
        public void ListIncomplete_ReturnsOnlyIncompleteAssignments()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a1 = new Assignment("Title 1", "Desc 1", "", null, Priority.Low);
            var a2 = new Assignment("Title 2", "Desc 2","",  null, Priority.Low);
            a2.MarkComplete();
            service.AddAssignment(a1);
            service.AddAssignment(a2);

            var result = service.ListIncomplete();

            Assert.Single(result);
            Assert.Contains(a1, result);
            Assert.DoesNotContain(a2, result);
        }

        [Fact]
        public void ListIncomplete_ReturnsEmptyList_WhenNoAssignments()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var result = service.ListIncomplete();
            Assert.Empty(result);
        }

        [Fact]
        public void ListIncomplete_ReturnsAll_WhenAllAreIncomplete()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a1 = new Assignment("Title 1", "Desc 1", "", null, Priority.Low);
            var a2 = new Assignment("Title 2", "Desc 2", "", null, Priority.Low);

            service.AddAssignment(a1);
            service.AddAssignment(a2);

            var result = service.ListIncomplete();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void UpdateAssignment_UpdatesCorrectly_WhenAssignmentExists()
        {
            // Arrange
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var assignment = new Assignment("Starting Title", "Starting Desc", "Notes", DateTime.Today, Priority.Medium);
            service.AddAssignment(assignment);

            // Act
            bool result = service.UpdateAssignment(
                "Starting Title",
                "Updated Title",
                "Updated Desc",
                "Updated Notes",
                DateTime.Today.AddDays(5),
                Priority.High
            );

            // Assert
            var updated = service.FindAssignmentByTitle("Updated Title");
            Assert.True(result);
            Assert.NotNull(updated);
            Assert.Equal("Updated Title", updated.Title);
            Assert.Equal("Updated Desc", updated.Description);
            Assert.Equal("Updated Notes", updated.Notes);
            Assert.Equal(Priority.High, updated.Priority);
            Assert.Equal(DateTime.Today.AddDays(5).Date, updated.DueDate?.Date);
        }

        [Fact]
        public void UpdateAssignment_ReturnsFalse_WhenAssignmentNotFound()
        {
            // Arrange
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());

            // Act
            var result = service.UpdateAssignment("Test Title 1", "Test Title 2", "Test Desc", "Notes", DateTime.Now, Priority.Low);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void MarkComplete_MarksAssignment_WhenFound()
        {
            // Arrange
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var assignment = new Assignment("Task", "Description", "", null, Priority.Low);
            service.AddAssignment(assignment);

            // Act
            var result = service.MarkComplete("Task");

            // Assert
            var updated = service.FindByTitle("Task");
            Assert.True(result);
            Assert.True(updated.IsCompleted);
        }

        [Fact]
        public void MarkComplete_ReturnsFalse_WhenNotFound()
        {
            // Arrange
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());

            // Act
            var result = service.MarkComplete("NotExisting");

            // Assert
            Assert.False(result);
        }
    }
}
