CREATE TABLE [Logs] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NOT NULL,
    [MessageTemplate] nvarchar(max) NOT NULL,
    [Level] nvarchar(max) NOT NULL,
    [TimeStamp] datetime2 NOT NULL,
    [Exception] nvarchar(max) NOT NULL,
    [Properties] nvarchar(max) NOT NULL,
    [LogEvent] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id])
);