
namespace AssignmentManagement.Core
{
    public class Assignment
    {
        public int Id { get; set; }
        
        public string Title { get; private set; } 
        public string Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public AssignmentPriority Priority { get; private set; }
        public bool IsCompleted { get; private set; }
        public string Notes { get; private set; }

        public Assignment(string title, string description, DateTime? dueDate, AssignmentPriority priority, string notes = "")
        {
            ValHelper(title, description);
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Notes = notes; // Fixed: assigned Notes to notes
            IsCompleted = false;
        }

        public void Update(string newTitle, string newDescription)
        {
            Validate(newTitle, nameof(Title));

            Title = newTitle;
            Description = newDescription;
        }

        public void MarkComplete()
        {
            IsCompleted = true;
        }

        public bool IsOverdue()
        {
            return DueDate.Value < DateTime.Now; // BUG: no null check, ignores IsCompleted
        }

        public override string ToString()
        {
            return $"- {Title} ({Priority}) due {DueDate?.ToShortDateString() ?? "N/A"}\n{Description}";
            // BUG: Notes not included in output
        }

        private void Validate(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{fieldName} cannot be blank or whitespace.");
        }

        private void ValHelper(string titleField, string descriptionField)
        {
            Validate(titleField, nameof(Title));
            Validate(descriptionField, nameof(Description));
        }
    }

    public enum AssignmentPriority
    {
        Low,
        Medium,
        High
    }
}
