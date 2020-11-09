USE [BowlingDB]
GO

/****** Object:  StoredProcedure [dbo].[UpdateStrikeSpareScores]    Script Date: 9/11/2020 9:10:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateStrikeSpareScores] 
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
	WHERE f.Id = @frameScoreId AND f.FrameNum = 10 AND ((i.IsStrike = 1 AND i.ThrowNum = 1) OR (i.IsSpare = 1 AND i.ThrowNum = 2)) AND @throwNum <> i.ThrowNum;


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


