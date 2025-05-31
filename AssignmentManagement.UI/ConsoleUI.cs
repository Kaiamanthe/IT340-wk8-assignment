using AssignmentManagement.Core;
using AssignmentManagement.Core.Interfaces;
using AssignmentManagement.Core.Models;
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

        private Priority ConvertToPriority(string priorityInput)
        {
            return priorityInput switch
            {
                "L" => Priority.Low,
                "M" => Priority.Medium,
                "H" => Priority.High,
                _ => throw new ArgumentException("Invalid priority input. Use L, M, or H.")
            };
        }

        private void AddAssignment()
        {
            Console.WriteLine("Enter assignment title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter assignment description: ");
            var description = Console.ReadLine();
            Console.WriteLine("Enter assignment notes (optional [enter] to leave blank): ");
            var notes = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Enter due date (optional, format: yyyy-MM-dd): ");
            var dueDateInput = Console.ReadLine();
            DateTime? dueDate = null;
            if (!string.IsNullOrWhiteSpace(dueDateInput))
            {
                if (DateTime.TryParse(dueDateInput, out var parsedDate))
                {
                    dueDate = parsedDate;
                }
                else
                {
                    Console.WriteLine("Invalid date format. Skipping due date.");
                }
            }

            Console.WriteLine("Enter priority (L)ow, (M)edium, (H)igh): ");
            var priorityInput = Console.ReadLine();
            Priority priority = ConvertToPriority(priorityInput);

            try
            {
                var assignment = new Assignment(title, description, notes, dueDate, priority);
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
                Console.WriteLine(assignment.ToString());
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
                Console.WriteLine(assignment.ToString());
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
                Console.WriteLine($"Found Assignment: {assignment.Title} - {assignment.Description} {(string.IsNullOrWhiteSpace(assignment.Notes) ? "" : $" | Notes: {assignment.Notes}")} (Completed: {assignment.IsCompleted})");
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
            var assignment = _assignmentService.FindAssignmentByTitle(oldTitle);

            if (assignment == null)
            {
                Console.WriteLine("Assignment not found.");
                return;
            }

            Console.WriteLine($"Found Assignment: {assignment}");

            Console.WriteLine("Enter new title:");
            var newTitle = Console.ReadLine();

            Console.WriteLine("Enter new description:");
            var newDescription = Console.ReadLine();

            string newNotes = assignment.Notes;
            if (string.IsNullOrWhiteSpace(assignment.Notes))
            {
                Console.Write("Would you like to add a note? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    Console.Write("Enter note: ");
                    newNotes = Console.ReadLine();
                }
            }
            else
            {
                Console.Write("Would you like to update note? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    Console.Write("Enter new note: ");
                    newNotes = Console.ReadLine();
                }
            }

            DateTime? newDueDate = assignment.DueDate;
            Console.Write("Would you like to update due date? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "y")
            {
                Console.Write("Enter new due date (optional, format: yyyy-MM-dd, press enter to leave blank): ");
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (DateTime.TryParse(input, out var parsedDate))
                        newDueDate = parsedDate;
                    else
                        Console.WriteLine("Invalid date format. Keeping existing due date.");
                }
                else
                {
                    newDueDate = null;
                }
            }

            Priority? newPriority = assignment.Priority;
            Console.Write("Would you like to update priority? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "y")
            {
                Console.Write("Enter priority (L)ow, (M)edium, (H)igh (optional, press enter to leave blank): ");
                var priorityInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(priorityInput))
                {
                    try
                    {
                        newPriority = ConvertToPriority(priorityInput.ToUpper());
                    }
                    catch
                    {
                        Console.WriteLine("Invalid priority input. Keeping existing priority.");
                    }
                }
            }

            if (_assignmentService.UpdateAssignment(oldTitle, newTitle, newDescription, newNotes, newDueDate, newPriority))
            {
                Console.WriteLine("Assignment updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update assignment.");
            }
        }


        private void DeleteAssignment()
        {
            Console.WriteLine("Enter the title of the assignment to delete:");
            var title = Console.ReadLine();
            if (_assignmentService.DeleteAssignment(title))
            {
                Console.WriteLine("Assignment deleted successfully.");
            }
            else
            {
                Console.WriteLine("Assignment not found.");
            }
        }
    }
}
