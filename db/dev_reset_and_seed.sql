-- DEV ONLY: deletes ALL rows in dbo.Jobs, resets the Id counter, and seeds test jobs.
USE Conveyor;
GO

-- clear (also resets the identity so Ids start at 1 again)
TRUNCATE TABLE dbo.Jobs;

-- seed 20 jobs; every 5th one has higher priority so dequeue ordering is visible
WITH n AS (
    SELECT TOP (20) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS i
    FROM sys.all_objects
)
INSERT dbo.Jobs (JobType, Payload, Priority)
SELECT 'Echo',
       CONCAT('{"n":', i, '}'),
       CASE WHEN i % 5 = 0 THEN 10 ELSE 0 END
FROM n;

-- check
SELECT Id, JobType, Payload, Status, Priority, Attempts, LockedBy
FROM dbo.Jobs
ORDER BY Id;
