-- ==========================================================
-- Create Stored Procedure Template for SQL Azure Database
-- ==========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		William Raiford
-- Create date: 2/9/2013
-- Description:	Deletes all records associated with a given user.
-- =============================================
CREATE PROCEDURE DeleteUser_SP
	-- Add the parameters for the stored procedure here
	@UserId uniqueidentifier,
	@Username nvarchar(40)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
delete FROM LineDataMultiLineTextData 
      WHERE LineDatas_Id in 
						(
						  select LineDatas_Id from LineDatas WHERE UserDataId = @UserId
						);

delete FROM LineDatas WHERE UserDataId = @UserId;
delete FROM MultiLineTextDatas WHERE UserDataId = @UserId;

delete FROM TranslationDataPhraseData
      WHERE TranslationDatas_Id in 
                            (
                              select TranslationDatas_Id from TranslationDatas WHERE UserDataId = @UserId
                            );

delete FROM TranslationDatas WHERE UserDataId = @UserId;
delete FROM PhraseDatas WHERE UserDataId = @UserId;
delete FROM LanguageDatas WHERE UserDataId = @UserId;
delete FROM PhraseBeliefDatas WHERE UserDataId = @UserId;
delete FROM UserDataRoleData WHERE UserDatas_Id = @UserId;
delete from UserDatas WHERE Id = @UserId;
delete from StudyDataDatas WHERE Username = @Username;

END
GO
