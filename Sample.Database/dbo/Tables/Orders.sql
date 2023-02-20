CREATE TABLE [dbo].[Orders] (
    [Id]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [OrderNumber]   NVARCHAR (20)   NOT NULL,
    [OrderPlaced]   DATETIME2 (7)   NOT NULL,
    [SubTotal]      DECIMAL (18, 2) NOT NULL,
    [Discount]      DECIMAL (18, 2) NOT NULL,
    [ServiceCharge] DECIMAL (18, 2) NOT NULL,
    [Total]         DECIMAL (18, 2) NOT NULL,
    [CustomerId]    BIGINT          NOT NULL,
    [IsDeleted]     BIT             NOT NULL,
    [CreatedBy]     NVARCHAR (MAX)  NULL,
    [ModifiedBy]    NVARCHAR (MAX)  NULL,
    [ModifiedOn]    DATETIME2 (7)   NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Orders_CustomerId]
    ON [dbo].[Orders]([CustomerId] ASC);

