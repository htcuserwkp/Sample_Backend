CREATE TABLE [dbo].[Customers] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [Email]     NVARCHAR (128) NOT NULL,
    [Phone]     NVARCHAR (20)  NOT NULL,
    [IsDeleted] BIT            NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id] ASC)
);