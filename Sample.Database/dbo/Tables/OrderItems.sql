CREATE TABLE [OrderItems] (
    [Id] bigint NOT NULL IDENTITY,
    [FoodId] bigint NOT NULL,
    [FoodName] nvarchar(max) NOT NULL,
    [Quantity] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [OrderId] bigint NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);
GO
CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);