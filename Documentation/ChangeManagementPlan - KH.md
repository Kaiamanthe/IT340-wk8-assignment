# 📋 Change Management Plan

**Student Name: Khai Ha**  
**Date Submitted: 06/01/25**  

---

## 🧭 Purpose of Change  
The system was missing a few key things for it to run/build. Key issues included improper injections of attributes for `Assignment.cs` methods, particularly around adding and checking overdue status. These behaviors are critical for user interaction and data accuracy. Ensuring proper behavior impacts both product quality and user trust.

---

## 🔧 Summary of Changes  

**AssignmentService.cs**
- Injected `IAppLogger` and added logging to `AddAssignment`, `UpdateAssignment`, `MarkComplete`, and `CheckOverdue`.
- Ensured exception handling during assignment addition.

**Assignment.cs**
- Added validation in `Update()` and constructor.
- Verified `IsOverdue()` and `ToString()` methods operate as intended.

**ConsoleUI.cs**
- Repaired UI flow by adding missing paths.
- Fixed improper injections.
- Implemented priority converter.
- Integrated overdue logic into relevant methods.

**AssignmentServiceTests.cs**
- Added unit tests for `UpdateAssignment`, `MarkComplete`, and `FindAssignmentByTitle`.

**AssignmentTests.cs**
- Added constructor validation tests.
- Tested `IsOverdue`, `ToString()`, and field mutation.

**ConsoleUITests.cs**
- Validated logic for `AddAssignment`, `DeleteAssignment`, and `SearchAssignmentByTitle`.
- Used mocked services to simulate user input and ensure flow correctness.

**Behaviors Affected**
- Logging now tracks core lifecycle events.
- Update and completion logic fully verified via TDD.
- UI behavior tested through simulated input and service layer interaction.

---

## ✅ TDD Process  

**What test failed first?**  
I didn’t jot down the specific test, but four tests were initially failing. One of them was `AddAssignment`, which failed due to missing logging.

**What did you change?**  
The original project had 14 build errors. I corrected these one by one. Then I addressed the failing tests by reviewing logic, resolving injection issues, and restructuring code. I also added an enum for `Priority`, reorganized files, and injected `Log()` calls into key methods.

**What confirmed the fix?**  
Moq-based unit tests verified expected logger output. All regression and newly added tests passed successfully.

---

## 🧪 Additional Coverage  

**Edge Cases Tested**
- Null/empty inputs for title and description.
- Assignments missing due dates or notes.
- Edge logic for completion and overdue detection.

**Logging Added**
- When an assignment is added.
- When an assignment is updated.
- When overdue status is checked.
- When an assignment is marked complete.

---

## 📌 Challenges  

**Where did you get stuck?**  
Initially I expected to just fix bugs based on feedback, but the project had many compile-time errors and failed tests. I had to rebuild and restore core structure before I could begin addressing actual bug logic.

**How did you resolve it?**  
I systematically fixed each build error, ensured the app could run, and then followed TDD to reintroduce testable logic. Ironically, while stabilizing the codebase, I resolved the first bug without realizing it.

---

## 🔮 Recommendations  

**Tech Debt Discovered**  
`ConvertToPriority()` is a private method in `ConsoleUI` that hinders testing. It should be refactored into a public utility class.

**Suggestions for Future Developers**
- Maintain strict separation of concerns (UI vs. logic).
- Ensure logging is present in all critical operations.
- Use test-driven development early, not after issues arise.

