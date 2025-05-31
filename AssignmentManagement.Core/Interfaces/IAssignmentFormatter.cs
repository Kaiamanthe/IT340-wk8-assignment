using AssignmentManagement.Core.Models;

namespace AssignmentManagement.Core.Interfaces
{
    public interface IAssignmentFormatter
    {
        string Format(Assignment assignment);
    }
}
