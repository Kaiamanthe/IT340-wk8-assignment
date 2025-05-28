
# 🐞 Week 8 Bug Reports

---

## Bug Report: BUG-2025-341

**Title:** Notes not saved in assignment  
**Reported by:** QA Analyst  
**Summary:** When adding an assignment with notes, the notes are never stored or shown in the service.  
**Steps to Reproduce:**  
1. Create a new assignment with a Notes string  
2. Add it to the service  
3. Retrieve the assignment  
**Expected Result:** Notes are saved and retrievable  
**Actual Result:** Notes are null or empty  
**Suggested Fix:** Assign Notes in the constructor  

---

## Bug Report: BUG-2025-342

**Title:** Notes not displayed to user  
**Reported by:** Product Owner  
**Summary:** The Notes property is not visible when printing assignment details  
**Steps to Reproduce:**  
1. Add an assignment with Notes  
2. Use ToString() or UI print method  
**Expected Result:** Notes appear in output  
**Actual Result:** Only title and description appear  
**Suggested Fix:** Include Notes in ToString()  

---

## Bug Report: BUG-2025-343

**Title:** IsOverdue incorrectly flags completed tasks  
**Reported by:** Instructor  
**Summary:** IsOverdue returns true for completed assignments  
**Steps to Reproduce:**  
1. Create an assignment due yesterday  
2. Mark it complete  
3. Call IsOverdue  
**Expected Result:** IsOverdue = false for completed  
**Actual Result:** IsOverdue = true  
**Suggested Fix:** Add check for IsCompleted  

---

## Bug Report: BUG-2025-344

**Title:** Logging missing for key operations  
**Reported by:** Developer  
**Summary:** There are no log entries when assignments are added or overdue is calculated  
**Steps to Reproduce:**  
1. Add an assignment  
2. Check logs  
**Expected Result:** Log statements confirming key actions  
**Actual Result:** No logs generated  
**Suggested Fix:** Inject and use IAppLogger  
