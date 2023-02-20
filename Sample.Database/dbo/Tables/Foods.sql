CREATE TABLE [dbo].[Foods] (
    [Id]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (128)  NOT NULL,
    [Description]       NVARCHAR (512)  NOT NULL,
    [Quantity]          INT             NOT NULL,
    [Price]             DECIMAL (18, 2) NOT NULL,
    [IsDeleted]         BIT             NOT NULL,
    [CategoryId]        BIGINT          DEFAULT (CONVERT([bigint],(0))) NOT NULL,
    [IsFreshlyPrepared] BIT             DEFAULT (CONVERT([bit],(0))) NOT NULL,
    [CreatedBy]         NVARCHAR (MAX)  NULL,
    [ModifiedBy]        NVARCHAR (MAX)  NULL,
    [ModifiedOn]        DATETIME2 (7)   NULL,
    CONSTRAINT [PK_Foods] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Foods_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Foods_CategoryId]
    ON [dbo].[Foods]([CategoryId] ASC);

