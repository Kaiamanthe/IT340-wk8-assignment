using Microsoft.VisualBasic;

namespace AssignmentManagement.Core.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; private set; } 
        public string Description { get; private set; }
        public DateTime? DueDate { get; private set; }
        public Priority Priority { get; private set; }
        public bool IsCompleted { get; private set; }
        public string Notes { get; private set; }

        public Assignment(string title, string description, string notes, DateTime? dueDate, Priority priority)
        {
            Validate(title, nameof(Title));
            Validate(description, nameof(Description));
            Title = title;
            Description = description;
            Notes = notes;
            DueDate = dueDate;
            Priority = priority;
            IsCompleted = false;
        }

        public void Update(string newTitle, string newDescription, string newNotes, DateTime? newDueDate, Priority? newPriority)
        {
            Validate(newTitle, nameof(Title));
            Validate(newDescription, nameof(Description));

            Title = newTitle;
            Description = newDescription;

            if (newNotes != null)
                Notes = newNotes;

            if (newDueDate.HasValue)
                DueDate = newDueDate;

            if (newPriority.HasValue)
                Priority = newPriority.Value;
        }
        public void MarkComplete()
        {
            IsCompleted = true;
        }
        private void Validate(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException($"{fieldName} cannot be blank or whitespace.");
        }
        public bool IsOverdue()
        {
            return DueDate.HasValue && !IsCompleted && DueDate.Value < DateTime.Now;
        }

        public override string ToString()
        {
            return $"Title: {Title} " +
                   $"Description: {Description} " +
                   $"{(string.IsNullOrWhiteSpace(Notes) ? "" : $"Notes: {Notes} ")}" +
                   $"{(DueDate.HasValue ? $"Due date: {DueDate.Value.ToShortDateString()} " : "")}" +
                   $"Priority: {Priority} " +
                   $"Completed: {IsCompleted}";
        }
    }

}
