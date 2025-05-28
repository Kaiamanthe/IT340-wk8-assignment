using AssignmentManagement.Core;

using System;

namespace AssignmentManagement.UI
{
    public class ConsoleUI
    {
        private readonly IAssignmentService _assignmentService;

        public ConsoleUI(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\nAssignment Manager Menu:");
                Console.WriteLine("1. Add Assignment");
                Console.WriteLine("2. List All Assignments");
                Console.WriteLine("3. List Incomplete Assignments");
                Console.WriteLine("4. Mark Assignment as Complete");
                Console.WriteLine("5. Search Assignment by Title");
                Console.WriteLine("6. Update Assignment");
                Console.WriteLine("7. Delete Assignment");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddAssignment();
                        break;
                    case "2":
                        ListAllAssignments();
                        break;
                    case "3":
                        ListIncompleteAssignments();
                        break;
                    case "4":
                        MarkAssignmentComplete(); // TODO
                        break;
                    case "5":
                        SearchAssignmentByTitle(); // TODO
                        break;
                    case "6":
                        UpdateAssignment(); // TODO
                        break;
                    case "7":
                        DeleteAssignment(); // TODO
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private AssignmentPriority ConvertToPriority(string priorityInput)
        {
            return priorityInput switch
            {
                "L" => AssignmentPriority.Low,
                "M" => AssignmentPriority.Medium,
                "H" => AssignmentPriority.High,
                _ => throw new ArgumentException("Invalid priority input. Use L, M, or H.")
            };
        }

        private void AddAssignment()
        {
            Console.WriteLine("Enter assignment title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter assignment description: ");
            var description = Console.ReadLine();
            Console.WriteLine("Enter due date (optional, format: yyyy-MM-dd): ");
            var dueDateInput = Console.ReadLine();
            DateTime? dueDate = null;
            Console.WriteLine("Enter priority (L)ow, (M)edium, (H)igh): ");
            var priorityInput = Console.ReadLine();
            AssignmentPriority priority = ConvertToPriority(priorityInput);
            Console.WriteLine("Enter assignment notes (optional [enter] to leave blank): ");
            var notes = Console.ReadLine() ?? string.Empty;

            try
            {
                var assignment = new Assignment(title, description, dueDate, priority, notes); // Bug: missing due date, priority, notes
                if (_assignmentService.AddAssignment(assignment))
                {
                    Console.WriteLine("Assignment added successfully.");
                }
                else
                {
                    Console.WriteLine("An assignment with this title already exists.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ListAllAssignments()
        {
            var assignments = _assignmentService.ListAll();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"- {assignment.Title}: {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
        }

        private void ListIncompleteAssignments()
        {
            var assignments = _assignmentService.ListIncomplete();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No incomplete assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"- {assignment.Title}: {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
        }

        private void MarkAssignmentComplete()
        {
            Console.WriteLine("Enter the title of the assignment to mark as complete:");
            var title = Console.ReadLine();
            if (_assignmentService.MarkAssignmentComplete(title))
            {
                Console.WriteLine("Assignment marked as complete.");
            }
            else
            {
                Console.WriteLine("Assignment not found or already completed.");
            }
        }

        private void SearchAssignmentByTitle()
        {
            Console.WriteLine("Enter the title of the assignment to search:");
            var title = Console.ReadLine();
            var assignment = _assignmentService.FindAssignmentByTitle(title);
            if (assignment != null)
            {
                Console.WriteLine($"Found Assignment: {assignment.Title} - {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
            else
            {
                Console.WriteLine("Assignment not found.");
            }
        }

        private void UpdateAssignment()
        {
            Console.WriteLine("Enter the title of the assignment to update:");
            var oldTitle = Console.ReadLine();
            Console.Write("Enter new title: ");
            var newTitle = Console.ReadLine();
            Console.Write("Enter new description: ");
            var newDescription = Console.ReadLine();
            if (_assignmentService.UpdateAssignment(oldTitle, newTitle, newDescription))
            {
                Console.WriteLine("Assignment updated successfully.");
            }
            else
            {
                Console.WriteLine("Assignment not found or update failed.");
            }
        }

        private void DeleteAssignment()
        {
            // TODO: Implement UI for deleting assignment
        }
    }
}
