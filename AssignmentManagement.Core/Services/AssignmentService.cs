using AssignmentManagement.Core.Interfaces;
using AssignmentManagement.Core.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentManagement.Core.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly List<Assignment> _assignments = new();
        private readonly IAssignmentFormatter _formatter;
        private readonly IAppLogger _logger;

        public AssignmentService(IAssignmentFormatter formatter, IAppLogger logger)
        {
            _formatter = formatter;
            _logger = logger;
        }

        public bool AddAssignment(Assignment assignment)
        {
            try
            {
                _assignments.Add(assignment);
                bool isOverdue = assignment.IsOverdue();
                _logger.Log($"Added Assignment: {assignment.Title} | Overdue: {isOverdue}"); return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error adding assignment: {ex.Message}");
                return false;
            }
        }

        public bool DeleteAssignment(string title)
        {
            var titleTodelete = _assignments.FirstOrDefault(a => a.Title == title);
            if (titleTodelete != null)
            {
                _assignments.Remove(titleTodelete);
                _logger.Log($"Deleted Assignment: {titleTodelete.Title} | Overdue: {titleTodelete.IsOverdue()}");
                return true;
            }
            return false;
        }

        public List<Assignment> ListAll() => _assignments;

        public List<Assignment> ListIncomplete() => _assignments.Where(a => !a.IsCompleted).ToList();

        public List<string> ListFormatted() => _assignments.Select(a => _formatter.Format(a)).ToList();

        public Assignment FindByTitle(string title) => _assignments.FirstOrDefault(a => a.Title == title);

        public bool UpdateAssignment(string title, string newTitle, string newDescription, string newNotes, DateTime? newDueDate, Priority? newPriority)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                assignment.Update(newTitle, newDescription, newNotes, newDueDate, newPriority);
                _logger.Log($"Updated Assignment: {assignment.Title}"); return true;
                return true;
            }
            return false;
        }

        public bool MarkComplete(string title)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                assignment.MarkComplete();
                _logger.Log($"Marked Assignment Complete: {assignment.Title} | Overdue: {assignment.IsOverdue()}");
                return true;
            }
            return false;
        }

        public Assignment FindAssignmentByTitle(string title)
        {
            return _assignments.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public bool CheckOverdue(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment == null) return false;

            var result = assignment.IsOverdue();
            _logger.Log($"Checked overdue status: {assignment.Title} - Overdue: {result}");
            return result;
        }
    }
}