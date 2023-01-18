IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

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

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230117120729_InitialMigration', N'7.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Foods] ADD [CategoryId] bigint NULL;
GO

CREATE TABLE [Categories] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_Foods_CategoryId] ON [Foods] ([CategoryId]);
GO

ALTER TABLE [Foods] ADD CONSTRAINT [FK_Foods_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230118014149_CategoryDbo', N'7.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Foods] DROP CONSTRAINT [FK_Foods_Categories_CategoryId];
GO

DROP INDEX [IX_Foods_CategoryId] ON [Foods];
DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'CategoryId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var0 + '];');
UPDATE [Foods] SET [CategoryId] = CAST(0 AS bigint) WHERE [CategoryId] IS NULL;
ALTER TABLE [Foods] ALTER COLUMN [CategoryId] bigint NOT NULL;
ALTER TABLE [Foods] ADD DEFAULT CAST(0 AS bigint) FOR [CategoryId];
CREATE INDEX [IX_Foods_CategoryId] ON [Foods] ([CategoryId]);
GO

CREATE TABLE [Customers] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Phone] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
GO

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

CREATE TABLE [FoodOrder] (
    [FoodsId] bigint NOT NULL,
    [OrdersId] bigint NOT NULL,
    CONSTRAINT [PK_FoodOrder] PRIMARY KEY ([FoodsId], [OrdersId]),
    CONSTRAINT [FK_FoodOrder_Foods_FoodsId] FOREIGN KEY ([FoodsId]) REFERENCES [Foods] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FoodOrder_Orders_OrdersId] FOREIGN KEY ([OrdersId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_FoodOrder_OrdersId] ON [FoodOrder] ([OrdersId]);
GO

CREATE INDEX [IX_Orders_CustomerId] ON [Orders] ([CustomerId]);
GO

ALTER TABLE [Foods] ADD CONSTRAINT [FK_Foods_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230118074941_CustomerOrder', N'7.0.2');
GO

COMMIT;
GO

