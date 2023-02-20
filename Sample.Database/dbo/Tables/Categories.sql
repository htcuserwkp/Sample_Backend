CREATE TABLE [dbo].[Categories] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (128) NOT NULL,
    [Description] NVARCHAR (512) NOT NULL,
    [IsDeleted]   BIT            NOT NULL,
    [CreatedBy]   NVARCHAR (MAX) NULL,
    [ModifiedBy]  NVARCHAR (MAX) NULL,
    [ModifiedOn]  DATETIME2 (7)  NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([Id] ASC)
);