CREATE TABLE [dbo].[Regions]
(
  RegionId UNIQUEIDENTIFIER NOT NULL
    CONSTRAINT PK_Regions PRIMARY KEY NONCLUSTERED,
  Name NVARCHAR(50) NOT NULL,
  Node HIERARCHYID  NOT NULL,
  ParentNode AS Node.GetAncestor(1) PERSISTED
);
GO

CREATE UNIQUE INDEX UX_Regions_ParentNode_Name ON dbo.Regions(ParentNode, Name);
GO

CREATE UNIQUE CLUSTERED INDEX CX_Regions_Node ON dbo.Regions(Node);
GO

CREATE NONCLUSTERED INDEX IX_Regions_Name ON dbo.Regions(Name);
GO


