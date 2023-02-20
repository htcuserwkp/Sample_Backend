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

BEGIN TRANSACTION;
GO

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
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230119083046_Logs', N'7.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DROP TABLE [FoodOrder];
GO

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
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230123123613_OrderItemCreated', N'7.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Foods] ADD [IsFreshlyPrepared] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230124091442_IsFreshlyPreparedFood', N'7.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'OrderNumber');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Orders] ALTER COLUMN [OrderNumber] nvarchar(20) NOT NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItems]') AND [c].[name] = N'FoodName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [OrderItems] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [OrderItems] ALTER COLUMN [FoodName] nvarchar(128) NOT NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'Properties');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Logs] ALTER COLUMN [Properties] nvarchar(512) NOT NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'MessageTemplate');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Logs] ALTER COLUMN [MessageTemplate] nvarchar(512) NOT NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'Message');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Logs] ALTER COLUMN [Message] nvarchar(512) NOT NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'LogEvent');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Logs] ALTER COLUMN [LogEvent] nvarchar(512) NOT NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'Level');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Logs] ALTER COLUMN [Level] nvarchar(128) NOT NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Logs]') AND [c].[name] = N'Exception');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Logs] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Logs] ALTER COLUMN [Exception] nvarchar(512) NOT NULL;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'Name');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Foods] ALTER COLUMN [Name] nvarchar(128) NOT NULL;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'Description');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Foods] ALTER COLUMN [Description] nvarchar(512) NOT NULL;
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'Phone');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Customers] ALTER COLUMN [Phone] nvarchar(20) NOT NULL;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'Name');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Customers] ALTER COLUMN [Name] nvarchar(128) NOT NULL;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'Email');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Customers] ALTER COLUMN [Email] nvarchar(128) NOT NULL;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'Name');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Categories] ALTER COLUMN [Name] nvarchar(128) NOT NULL;
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'Description');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Categories] ALTER COLUMN [Description] nvarchar(512) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230206020142_LimitNvarchar', N'7.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Orders] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Orders] ADD [ModifiedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Orders] ADD [ModifiedOn] datetime2 NULL;
GO

ALTER TABLE [OrderItems] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [OrderItems] ADD [ModifiedBy] nvarchar(max) NULL;
GO

ALTER TABLE [OrderItems] ADD [ModifiedOn] datetime2 NULL;
GO

ALTER TABLE [Foods] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Foods] ADD [ModifiedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Foods] ADD [ModifiedOn] datetime2 NULL;
GO

ALTER TABLE [Customers] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Customers] ADD [ModifiedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Customers] ADD [ModifiedOn] datetime2 NULL;
GO

ALTER TABLE [Categories] ADD [CreatedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Categories] ADD [ModifiedBy] nvarchar(max) NULL;
GO

ALTER TABLE [Categories] ADD [ModifiedOn] datetime2 NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230220105222_AuditInfo', N'7.0.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'ModifiedBy');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Orders] ALTER COLUMN [ModifiedBy] nvarchar(128) NULL;
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'CreatedBy');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [Orders] ALTER COLUMN [CreatedBy] nvarchar(128) NULL;
GO

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItems]') AND [c].[name] = N'ModifiedBy');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [OrderItems] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [OrderItems] ALTER COLUMN [ModifiedBy] nvarchar(128) NULL;
GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItems]') AND [c].[name] = N'CreatedBy');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [OrderItems] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [OrderItems] ALTER COLUMN [CreatedBy] nvarchar(128) NULL;
GO

DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'ModifiedBy');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [Foods] ALTER COLUMN [ModifiedBy] nvarchar(128) NULL;
GO

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Foods]') AND [c].[name] = N'CreatedBy');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Foods] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [Foods] ALTER COLUMN [CreatedBy] nvarchar(128) NULL;
GO

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'ModifiedBy');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [Customers] ALTER COLUMN [ModifiedBy] nvarchar(128) NULL;
GO

DECLARE @var23 sysname;
SELECT @var23 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Customers]') AND [c].[name] = N'CreatedBy');
IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [Customers] DROP CONSTRAINT [' + @var23 + '];');
ALTER TABLE [Customers] ALTER COLUMN [CreatedBy] nvarchar(128) NULL;
GO

DECLARE @var24 sysname;
SELECT @var24 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'ModifiedBy');
IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var24 + '];');
ALTER TABLE [Categories] ALTER COLUMN [ModifiedBy] nvarchar(128) NULL;
GO

DECLARE @var25 sysname;
SELECT @var25 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'CreatedBy');
IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT [' + @var25 + '];');
ALTER TABLE [Categories] ALTER COLUMN [CreatedBy] nvarchar(128) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230220112658_AuditInfoAllocation', N'7.0.2');
GO

COMMIT;
GO

