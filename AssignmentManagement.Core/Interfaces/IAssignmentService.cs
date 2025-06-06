using AssignmentManagement.Core.Models;
using System.Collections.Generic;

namespace AssignmentManagement.Core.Interfaces
{
    public interface IAssignmentService
    {
        bool AddAssignment(Assignment assignment);
        List<Assignment> ListAll();
        List<Assignment> ListIncomplete();
        List<string> ListFormatted();
        Assignment FindByTitle(string title);
        bool UpdateAssignment(string title, string newTitle, string newDescription, string newNotes, DateTime? newDueDate, Priority? newPriority);
        bool DeleteAssignment(string title);
        bool MarkComplete(string title);
        Assignment FindAssignmentByTitle(string title);
        bool CheckOverdue(string title);
    }
}
