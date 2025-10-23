-------------------------------------------------------------------------------
-- Stored procedure: add a region under a given parent
-------------------------------------------------------------------------------
CREATE OR ALTER   PROCEDURE [dbo].[Region_Add]
  @parent_id UNIQUEIDENTIFIER,      -- NOT NULL
  @name      NVARCHAR(50)           -- NOT NULL
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  IF @parent_id IS NULL OR @name IS NULL
    THROW 50000, 'Both parent_id and name are required.', 1;

  -- Normalize name (optional)
  SET @name = LTRIM(RTRIM(@name));

  DECLARE
      @parentNode  hierarchyid,
      @parentLevel INT,
      @lastChild   hierarchyid,
      @newNode     hierarchyid,
      @newId       UNIQUEIDENTIFIER;

  BEGIN TRAN;

  -- 1) Find parent (take update/hold locks to serialize sibling inserts)
  SELECT
      @parentNode  = r.Node,
      @parentLevel = r.Node.GetLevel()
  FROM dbo.Regions AS r WITH (UPDLOCK, HOLDLOCK)
  WHERE r.RegionId = @parent_id;

  IF @parentNode IS NULL
  BEGIN
    ROLLBACK;
    THROW 50001, 'Parent region not found.', 1;
  END

  -- 2) Enforce max depth of 6 levels (0..5 allowed); child would become level parent+1
  IF (@parentLevel + 1) >= 6
  BEGIN
    ROLLBACK;
    THROW 50002, 'Cannot add child: maximum depth of 6 levels would be exceeded.', 1;
  END

  -- 3) Duplicate child-name check under same parent
  --    If you created UX_Regions_ParentNode_Name, this also protects at DDL level.
  IF EXISTS (
      SELECT 1
      FROM dbo.Regions AS c WITH (UPDLOCK, HOLDLOCK)
      WHERE c.ParentNode = @parentNode    -- equivalent to c.Node.GetAncestor(1) = @parentNode
        AND c.Name = @name
  )
  BEGIN
    ROLLBACK;
    THROW 50003, 'Duplicate region: a child with the same name already exists under the parent.', 1;
  END

  -- 4) Allocate next child slot under parent (serialize via same locks)
  SELECT @lastChild = MAX(c.Node)
  FROM dbo.Regions AS c WITH (UPDLOCK, HOLDLOCK)
  WHERE c.ParentNode = @parentNode;

  SET @newNode = @parentNode.GetDescendant(@lastChild, NULL);
  SET @newId   = NEWID();

  INSERT dbo.Regions(RegionId, Name, Node)
  VALUES (@newId, @name, @newNode);

  COMMIT;

  -- Return the newly created row
  SELECT
      RegionId = @newId,
      Name     = @name,
      Path     = @newNode.ToString(),
      [Level]  = @newNode.GetLevel();
END
GO


