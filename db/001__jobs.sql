CREATE DATABASE Conveyor;
GO
USE Conveyor;
CREATE TABLE dbo.Jobs (
    Id         BIGINT IDENTITY PRIMARY KEY,
    JobType    NVARCHAR(100) NOT NULL,
    Payload    NVARCHAR(MAX) NOT NULL,
    Status     VARCHAR(20)   NOT NULL DEFAULT 'Queued',
    Priority   INT           NOT NULL DEFAULT 0,
    Attempts   INT           NOT NULL DEFAULT 0,
    RunAfter   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    LockedBy   NVARCHAR(100) NULL,
    LockedAt   DATETIME2     NULL,
    CreatedAt  DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
);
-- equality column, then the ORDER BY columns; the range filter goes in INCLUDE
CREATE INDEX IX_Jobs_Dequeue ON dbo.Jobs (Status, Priority DESC, Id) INCLUDE (RunAfter);

INSERT dbo.Jobs (JobType, Payload)
VALUES ('Echo','{"n":1}'),('Echo','{"n":2}'),('Echo','{"n":3}');

BEGIN TRAN;
WITH next AS (
    SELECT TOP (1) *
    FROM dbo.Jobs WITH (UPDLOCK, READPAST, ROWLOCK)
    WHERE Status = 'Queued' AND RunAfter <= SYSUTCDATETIME()
    ORDER BY Priority DESC, Id
)
UPDATE next
SET Status = 'Processing', LockedBy = 'worker-A',
    LockedAt = SYSUTCDATETIME(), Attempts = Attempts + 1
OUTPUT inserted.Id, inserted.JobType, inserted.Payload
COMMIT;
