CREATE TABLE [dbo].[OrderItems] (
    [Id]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [FoodId]     BIGINT          NOT NULL,
    [FoodName]   NVARCHAR (128)  NOT NULL,
    [Quantity]   INT             NOT NULL,
    [Price]      DECIMAL (18, 2) NOT NULL,
    [OrderId]    BIGINT          NOT NULL,
    [IsDeleted]  BIT             NOT NULL,
    [CreatedBy]  NVARCHAR (MAX)  NULL,
    [ModifiedBy] NVARCHAR (MAX)  NULL,
    [ModifiedOn] DATETIME2 (7)   NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_OrderItems_OrderId]
    ON [dbo].[OrderItems]([OrderId] ASC);

