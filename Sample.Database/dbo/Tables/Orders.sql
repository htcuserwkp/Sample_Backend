CREATE TABLE [Orders] (
    [Id] bigint NOT NULL IDENTITY,
    [OrderNumber] nvarchar(max) NOT NULL,
    [OrderPlaced] datetime2 NOT NULL,
    [SubTotal] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    [ServiceCharge] decimal(18,2) NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [CustomerId] bigint NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);
GO
CREATE INDEX [IX_Orders_CustomerId] ON [Orders] ([CustomerId]);