CREATE DATABASE [BowlingDB]
GO

USE [BowlingDB]
GO

USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PLAYER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GAME](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Player_Id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GAME]  WITH CHECK ADD FOREIGN KEY([Player_Id])
REFERENCES [dbo].[PLAYER] ([Id])
GO


USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FRAMESCORES](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Game_Id] [int] NULL,
	[FrameNum] [int] NULL,
	[TotalScore] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_SCORE] UNIQUE NONCLUSTERED 
(
	[Game_Id] ASC,
	[FrameNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[FRAMESCORES] ADD  DEFAULT ((0)) FOR [FrameNum]
GO

ALTER TABLE [dbo].[FRAMESCORES] ADD  DEFAULT ((0)) FOR [TotalScore]
GO

ALTER TABLE [dbo].[FRAMESCORES]  WITH CHECK ADD FOREIGN KEY([Game_Id])
REFERENCES [dbo].[GAME] ([Id])
GO

ALTER TABLE [dbo].[FRAMESCORES]  WITH CHECK ADD CHECK  (([FrameNum]>(0) AND [FrameNum]<(11)))
GO


USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[INDIVDUALSCORE](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameFrame_Id] [int] NOT NULL,
	[ThrowNum] [int] NOT NULL,
	[Score] [int] NULL,
	[IsStrike] [bit] NULL,
	[IsSpare] [bit] NULL,
	[IsFoul] [bit] NULL,
 CONSTRAINT [PK_SCORE] PRIMARY KEY CLUSTERED 
(
	[GameFrame_Id] ASC,
	[ThrowNum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[INDIVDUALSCORE] ADD  DEFAULT ((0)) FOR [ThrowNum]
GO

ALTER TABLE [dbo].[INDIVDUALSCORE] ADD  DEFAULT ((0)) FOR [IsStrike]
GO

ALTER TABLE [dbo].[INDIVDUALSCORE] ADD  DEFAULT ((0)) FOR [IsSpare]
GO

ALTER TABLE [dbo].[INDIVDUALSCORE] ADD  DEFAULT ((0)) FOR [IsFoul]
GO

ALTER TABLE [dbo].[INDIVDUALSCORE]  WITH CHECK ADD FOREIGN KEY([GameFrame_Id])
REFERENCES [dbo].[FRAMESCORES] ([Id])
GO

ALTER TABLE [dbo].[INDIVDUALSCORE]  WITH CHECK ADD CHECK  (([score]>=(0) AND [score]<(11)))
GO

ALTER TABLE [dbo].[INDIVDUALSCORE]  WITH CHECK ADD CHECK  (([ThrowNum]>(0) AND [ThrowNum]<=(3)))
GO


USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetScoresByGame] @gameId INT
AS
BEGIN
	SELECT p.Name AS 'PlayerName'
		,g.Id AS 'GameId'
		,f.FrameNum 
		,f.TotalScore
		,i.ThrowNum 
		,CASE WHEN i.IsStrike = 1 THEN 'STRIKE' WHEN i.IsSpare = 1 THEN 'SPARE' WHEN i.IsFoul = 1 THEN 'FOUL' ELSE CAST(i.Score AS VARCHAR) END AS Score
	--SELECT *
	FROM GAME g
	INNER JOIN PLAYER p ON g.Player_Id = p.Id
	INNER JOIN FRAMESCORES f ON f.Game_Id = g.Id
	INNER JOIN INDIVDUALSCORE i ON i.GameFrame_Id = f.Id
	WHERE g.Id = @gameId
	ORDER BY f.FrameNum
		,i.ThrowNum
END



GO


USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateStrikeSpareScores] 
	@individualScoreId int,
	@gameId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @frameScoreId int
	Declare @score int
	Declare @frameNum int
	Declare @throwNum int
	Declare @previousframeScoreId int

	Select @frameScoreId = GameFrame_Id, @score = Score, @throwNum = ThrowNum from [INDIVDUALSCORE] where id = @individualScoreId
	
	Update f SET TotalScore = f.TotalScore + @score  FROM [dbo].[FRAMESCORES] f
	JOIN [dbo].[INDIVDUALSCORE] i ON f.Id = i.GameFrame_Id
	WHERE f.Id = @frameScoreId AND f.FrameNum = 10 AND i.IsStrike = 1 AND i.ThrowNum = 1 AND @throwNum <> i.ThrowNum;


	Select @previousframeScoreId = Id, @frameNum = FrameNum from FRAMESCORES f WHERE Game_Id = @gameId AND FrameNum = 
	(Select FrameNum - 1 from FRAMESCORES f where f.Id = @frameScoreId)

	Update f SET TotalScore = f.TotalScore + @score  FROM [dbo].[FRAMESCORES] f
	JOIN [dbo].[INDIVDUALSCORE] i ON f.Id = i.GameFrame_Id
	WHERE f.Id = @previousframeScoreId AND (i.IsStrike = 1 OR (i.IsSpare = 1 AND @throwNum = 1));


	If Exists (SELECT TOP 1 ID from [dbo].[INDIVDUALSCORE] WHERE GameFrame_Id = @previousframeScoreId And ThrowNum = 1 AND IsStrike = 1)
	BEGIN

		Set @previousframeScoreId = 0
		Select @previousframeScoreId = Id from FRAMESCORES f WHERE Game_Id = @gameId AND FrameNum = @frameNum - 1

		Update f SET TotalScore = f.TotalScore + @score  FROM [dbo].[FRAMESCORES] f
		JOIN [dbo].[INDIVDUALSCORE] i ON f.Id = i.GameFrame_Id
		WHERE f.Id = @previousframeScoreId AND i.IsStrike = 1 AND @throwNum = 1;

	END;
	
	SELECT 1 AS RetValue;
END

GO


USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteGameScoresById] 
	@gameId int
AS
BEGIN
	DELETE i FROM INDIVDUALSCORE i INNER JOIN FRAMESCORES f ON i.GameFrame_Id = f.Id WHERE f.Game_Id = @gameId;
	
	DELETE FROM FRAMESCORES WHERE Game_Id = @gameId
	
	DELETE FROM GAME WHERE Id = @gameId

	SELECT 1 AS RetValue;
END

GO


USE [BowlingDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetGameIdByPlayer]
	@playerId int
AS
BEGIN
	SELECT TOP 1 g.* FROM GAME g 
		LEFT JOIN FRAMESCORES f ON g.Id = f.Game_Id 
	WHERE f.Game_Id IS NULL AND g.Player_Id = @playerId
END

GO


