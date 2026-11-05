### 11-4-2026
Lessons learned
* ROWLOCK - locks rows at row level rather than page level 
* READPAST - allows locked rows to be skipped 
* UPDLOCK - gives rows an update lock so other processes cannot update them 
* Mismatching index priority and ORDER BY priority can result in too many rows being read and given an update lock. 