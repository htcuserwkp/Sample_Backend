CREATE TABLE [Foods] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Quantity] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Foods] PRIMARY KEY ([Id])
);


GO
ALTER TABLE [Foods] ADD CONSTRAINT [FK_Foods_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]);
GO
CREATE INDEX [IX_Foods_CategoryId] ON [Foods] ([CategoryId]);