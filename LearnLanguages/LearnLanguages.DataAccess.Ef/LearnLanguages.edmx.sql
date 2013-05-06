
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/13/2012 17:25:17
-- Generated from EDMX file: C:\Users\user\Documents\Visual Studio 2010\Projects\LearnLanguages\LearnLanguages.DataAccess.Ef\LearnLanguages.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LearnLanguagesDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AssociateLanguageDataWithPhraseData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseDatas] DROP CONSTRAINT [FK_AssociateLanguageDataWithPhraseData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithPhraseData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseDatas] DROP CONSTRAINT [FK_AssociationUserDataWithPhraseData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithLanguageData_UserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithLanguageData] DROP CONSTRAINT [FK_AssociationUserDataWithLanguageData_UserData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithLanguageData_LanguageData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithLanguageData] DROP CONSTRAINT [FK_AssociationUserDataWithLanguageData_LanguageData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithRoleData_UserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithRoleData] DROP CONSTRAINT [FK_AssociationUserDataWithRoleData_UserData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithRoleData_RoleData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithRoleData] DROP CONSTRAINT [FK_AssociationUserDataWithRoleData_RoleData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDataRoleData_UserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDataRoleData] DROP CONSTRAINT [FK_UserDataRoleData_UserData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDataRoleData_RoleData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDataRoleData] DROP CONSTRAINT [FK_UserDataRoleData_RoleData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDataPhraseData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseDatas] DROP CONSTRAINT [FK_UserDataPhraseData];
GO
IF OBJECT_ID(N'[dbo].[FK_PhraseDataLanguageData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseDatas] DROP CONSTRAINT [FK_PhraseDataLanguageData];
GO
IF OBJECT_ID(N'[dbo].[FK_TranslationDataPhraseData_TranslationData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TranslationDataPhraseData] DROP CONSTRAINT [FK_TranslationDataPhraseData_TranslationData];
GO
IF OBJECT_ID(N'[dbo].[FK_TranslationDataPhraseData_PhraseData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TranslationDataPhraseData] DROP CONSTRAINT [FK_TranslationDataPhraseData_PhraseData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDataTranslationData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TranslationDatas] DROP CONSTRAINT [FK_UserDataTranslationData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDataLanguageData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LanguageDatas] DROP CONSTRAINT [FK_UserDataLanguageData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDataLineData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineDatas] DROP CONSTRAINT [FK_UserDataLineData];
GO
IF OBJECT_ID(N'[dbo].[FK_PhraseDataLineData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineDatas] DROP CONSTRAINT [FK_PhraseDataLineData];
GO
IF OBJECT_ID(N'[dbo].[FK_ContextPhraseDataTranslationData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TranslationDatas] DROP CONSTRAINT [FK_ContextPhraseDataTranslationData];
GO
IF OBJECT_ID(N'[dbo].[FK_PhraseDataDefaultStudyAdvisorKnowledgeBeliefData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DefaultStudyAdvisorKnowledgeBeliefDatas] DROP CONSTRAINT [FK_PhraseDataDefaultStudyAdvisorKnowledgeBeliefData];
GO
IF OBJECT_ID(N'[dbo].[FK_PhraseDataPhraseBeliefData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseBeliefDatas] DROP CONSTRAINT [FK_PhraseDataPhraseBeliefData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserDataPhraseBeliefData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseBeliefDatas] DROP CONSTRAINT [FK_UserDataPhraseBeliefData];
GO
IF OBJECT_ID(N'[dbo].[FK_MultiLineTextDataUserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MultiLineTextDatas] DROP CONSTRAINT [FK_MultiLineTextDataUserData];
GO
IF OBJECT_ID(N'[dbo].[FK_LineDataMultiLineTextData_LineData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineDataMultiLineTextData] DROP CONSTRAINT [FK_LineDataMultiLineTextData_LineData];
GO
IF OBJECT_ID(N'[dbo].[FK_LineDataMultiLineTextData_MultiLineTextData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LineDataMultiLineTextData] DROP CONSTRAINT [FK_LineDataMultiLineTextData_MultiLineTextData];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LanguageDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LanguageDatas];
GO
IF OBJECT_ID(N'[dbo].[PhraseDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhraseDatas];
GO
IF OBJECT_ID(N'[dbo].[UserDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDatas];
GO
IF OBJECT_ID(N'[dbo].[RoleDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleDatas];
GO
IF OBJECT_ID(N'[dbo].[TranslationDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TranslationDatas];
GO
IF OBJECT_ID(N'[dbo].[LineDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineDatas];
GO
IF OBJECT_ID(N'[dbo].[DefaultStudyAdvisorKnowledgeBeliefDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DefaultStudyAdvisorKnowledgeBeliefDatas];
GO
IF OBJECT_ID(N'[dbo].[PhraseBeliefDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhraseBeliefDatas];
GO
IF OBJECT_ID(N'[dbo].[StudyDataDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudyDataDatas];
GO
IF OBJECT_ID(N'[dbo].[MultiLineTextDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MultiLineTextDatas];
GO
IF OBJECT_ID(N'[dbo].[AssociationUserDataWithLanguageData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssociationUserDataWithLanguageData];
GO
IF OBJECT_ID(N'[dbo].[AssociationUserDataWithRoleData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssociationUserDataWithRoleData];
GO
IF OBJECT_ID(N'[dbo].[UserDataRoleData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDataRoleData];
GO
IF OBJECT_ID(N'[dbo].[TranslationDataPhraseData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TranslationDataPhraseData];
GO
IF OBJECT_ID(N'[dbo].[LineDataMultiLineTextData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LineDataMultiLineTextData];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LanguageDatas'
CREATE TABLE [dbo].[LanguageDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [UserDataId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'PhraseDatas'
CREATE TABLE [dbo].[PhraseDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [UserDataId] uniqueidentifier  NOT NULL,
    [LanguageDataId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserDatas'
CREATE TABLE [dbo].[UserDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Salt] int  NOT NULL,
    [SaltedHashedPasswordValue] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RoleDatas'
CREATE TABLE [dbo].[RoleDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Text] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TranslationDatas'
CREATE TABLE [dbo].[TranslationDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [UserDataId] uniqueidentifier  NOT NULL,
    [ContextPhraseDataId] uniqueidentifier  NULL
);
GO

-- Creating table 'LineDatas'
CREATE TABLE [dbo].[LineDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [LineNumber] int  NOT NULL,
    [UserDataId] uniqueidentifier  NOT NULL,
    [PhraseDataId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'DefaultStudyAdvisorKnowledgeBeliefDatas'
CREATE TABLE [dbo].[DefaultStudyAdvisorKnowledgeBeliefDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [ExpirationDate] datetime  NOT NULL,
    [PhraseDataId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'PhraseBeliefDatas'
CREATE TABLE [dbo].[PhraseBeliefDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [TimeStamp] datetime  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Strength] float  NOT NULL,
    [BelieverId] uniqueidentifier  NOT NULL,
    [ReviewMethodId] uniqueidentifier  NOT NULL,
    [PhraseDataId] uniqueidentifier  NOT NULL,
    [UserDataId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'StudyDataDatas'
CREATE TABLE [dbo].[StudyDataDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [NativeLanguageText] nvarchar(max)  NOT NULL,
    [Username] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MultiLineTextDatas'
CREATE TABLE [dbo].[MultiLineTextDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [AdditionalMetadata] nvarchar(max)  NOT NULL,
    [UserDataId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'AssociationUserDataWithLanguageData'
CREATE TABLE [dbo].[AssociationUserDataWithLanguageData] (
    [AssociationUserDataWithLanguageData_LanguageData_Id] uniqueidentifier  NOT NULL,
    [AssociationUserDataWithLanguageData_UserData_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'AssociationUserDataWithRoleData'
CREATE TABLE [dbo].[AssociationUserDataWithRoleData] (
    [AssociationUserDataWithRoleData_RoleData_Id] uniqueidentifier  NOT NULL,
    [AssociationUserDataWithRoleData_UserData_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserDataRoleData'
CREATE TABLE [dbo].[UserDataRoleData] (
    [UserDatas_Id] uniqueidentifier  NOT NULL,
    [RoleDatas_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'TranslationDataPhraseData'
CREATE TABLE [dbo].[TranslationDataPhraseData] (
    [TranslationDatas_Id] uniqueidentifier  NOT NULL,
    [PhraseDatas_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'LineDataMultiLineTextData'
CREATE TABLE [dbo].[LineDataMultiLineTextData] (
    [LineDatas_Id] uniqueidentifier  NOT NULL,
    [MultiLineTextDatas_Id] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'LanguageDatas'
ALTER TABLE [dbo].[LanguageDatas]
ADD CONSTRAINT [PK_LanguageDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [PK_PhraseDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserDatas'
ALTER TABLE [dbo].[UserDatas]
ADD CONSTRAINT [PK_UserDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleDatas'
ALTER TABLE [dbo].[RoleDatas]
ADD CONSTRAINT [PK_RoleDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TranslationDatas'
ALTER TABLE [dbo].[TranslationDatas]
ADD CONSTRAINT [PK_TranslationDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LineDatas'
ALTER TABLE [dbo].[LineDatas]
ADD CONSTRAINT [PK_LineDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DefaultStudyAdvisorKnowledgeBeliefDatas'
ALTER TABLE [dbo].[DefaultStudyAdvisorKnowledgeBeliefDatas]
ADD CONSTRAINT [PK_DefaultStudyAdvisorKnowledgeBeliefDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PhraseBeliefDatas'
ALTER TABLE [dbo].[PhraseBeliefDatas]
ADD CONSTRAINT [PK_PhraseBeliefDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudyDataDatas'
ALTER TABLE [dbo].[StudyDataDatas]
ADD CONSTRAINT [PK_StudyDataDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MultiLineTextDatas'
ALTER TABLE [dbo].[MultiLineTextDatas]
ADD CONSTRAINT [PK_MultiLineTextDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AssociationUserDataWithLanguageData_LanguageData_Id], [AssociationUserDataWithLanguageData_UserData_Id] in table 'AssociationUserDataWithLanguageData'
ALTER TABLE [dbo].[AssociationUserDataWithLanguageData]
ADD CONSTRAINT [PK_AssociationUserDataWithLanguageData]
    PRIMARY KEY NONCLUSTERED ([AssociationUserDataWithLanguageData_LanguageData_Id], [AssociationUserDataWithLanguageData_UserData_Id] ASC);
GO

-- Creating primary key on [AssociationUserDataWithRoleData_RoleData_Id], [AssociationUserDataWithRoleData_UserData_Id] in table 'AssociationUserDataWithRoleData'
ALTER TABLE [dbo].[AssociationUserDataWithRoleData]
ADD CONSTRAINT [PK_AssociationUserDataWithRoleData]
    PRIMARY KEY NONCLUSTERED ([AssociationUserDataWithRoleData_RoleData_Id], [AssociationUserDataWithRoleData_UserData_Id] ASC);
GO

-- Creating primary key on [UserDatas_Id], [RoleDatas_Id] in table 'UserDataRoleData'
ALTER TABLE [dbo].[UserDataRoleData]
ADD CONSTRAINT [PK_UserDataRoleData]
    PRIMARY KEY NONCLUSTERED ([UserDatas_Id], [RoleDatas_Id] ASC);
GO

-- Creating primary key on [TranslationDatas_Id], [PhraseDatas_Id] in table 'TranslationDataPhraseData'
ALTER TABLE [dbo].[TranslationDataPhraseData]
ADD CONSTRAINT [PK_TranslationDataPhraseData]
    PRIMARY KEY NONCLUSTERED ([TranslationDatas_Id], [PhraseDatas_Id] ASC);
GO

-- Creating primary key on [LineDatas_Id], [MultiLineTextDatas_Id] in table 'LineDataMultiLineTextData'
ALTER TABLE [dbo].[LineDataMultiLineTextData]
ADD CONSTRAINT [PK_LineDataMultiLineTextData]
    PRIMARY KEY NONCLUSTERED ([LineDatas_Id], [MultiLineTextDatas_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LanguageDataId] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [FK_AssociateLanguageDataWithPhraseData]
    FOREIGN KEY ([LanguageDataId])
    REFERENCES [dbo].[LanguageDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociateLanguageDataWithPhraseData'
CREATE INDEX [IX_FK_AssociateLanguageDataWithPhraseData]
ON [dbo].[PhraseDatas]
    ([LanguageDataId]);
GO

-- Creating foreign key on [UserDataId] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [FK_AssociationUserDataWithPhraseData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociationUserDataWithPhraseData'
CREATE INDEX [IX_FK_AssociationUserDataWithPhraseData]
ON [dbo].[PhraseDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [AssociationUserDataWithLanguageData_LanguageData_Id] in table 'AssociationUserDataWithLanguageData'
ALTER TABLE [dbo].[AssociationUserDataWithLanguageData]
ADD CONSTRAINT [FK_AssociationUserDataWithLanguageData_UserData]
    FOREIGN KEY ([AssociationUserDataWithLanguageData_LanguageData_Id])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AssociationUserDataWithLanguageData_UserData_Id] in table 'AssociationUserDataWithLanguageData'
ALTER TABLE [dbo].[AssociationUserDataWithLanguageData]
ADD CONSTRAINT [FK_AssociationUserDataWithLanguageData_LanguageData]
    FOREIGN KEY ([AssociationUserDataWithLanguageData_UserData_Id])
    REFERENCES [dbo].[LanguageDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociationUserDataWithLanguageData_LanguageData'
CREATE INDEX [IX_FK_AssociationUserDataWithLanguageData_LanguageData]
ON [dbo].[AssociationUserDataWithLanguageData]
    ([AssociationUserDataWithLanguageData_UserData_Id]);
GO

-- Creating foreign key on [AssociationUserDataWithRoleData_RoleData_Id] in table 'AssociationUserDataWithRoleData'
ALTER TABLE [dbo].[AssociationUserDataWithRoleData]
ADD CONSTRAINT [FK_AssociationUserDataWithRoleData_UserData]
    FOREIGN KEY ([AssociationUserDataWithRoleData_RoleData_Id])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AssociationUserDataWithRoleData_UserData_Id] in table 'AssociationUserDataWithRoleData'
ALTER TABLE [dbo].[AssociationUserDataWithRoleData]
ADD CONSTRAINT [FK_AssociationUserDataWithRoleData_RoleData]
    FOREIGN KEY ([AssociationUserDataWithRoleData_UserData_Id])
    REFERENCES [dbo].[RoleDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociationUserDataWithRoleData_RoleData'
CREATE INDEX [IX_FK_AssociationUserDataWithRoleData_RoleData]
ON [dbo].[AssociationUserDataWithRoleData]
    ([AssociationUserDataWithRoleData_UserData_Id]);
GO

-- Creating foreign key on [UserDatas_Id] in table 'UserDataRoleData'
ALTER TABLE [dbo].[UserDataRoleData]
ADD CONSTRAINT [FK_UserDataRoleData_UserData]
    FOREIGN KEY ([UserDatas_Id])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleDatas_Id] in table 'UserDataRoleData'
ALTER TABLE [dbo].[UserDataRoleData]
ADD CONSTRAINT [FK_UserDataRoleData_RoleData]
    FOREIGN KEY ([RoleDatas_Id])
    REFERENCES [dbo].[RoleDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDataRoleData_RoleData'
CREATE INDEX [IX_FK_UserDataRoleData_RoleData]
ON [dbo].[UserDataRoleData]
    ([RoleDatas_Id]);
GO

-- Creating foreign key on [UserDataId] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [FK_UserDataPhraseData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDataPhraseData'
CREATE INDEX [IX_FK_UserDataPhraseData]
ON [dbo].[PhraseDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [LanguageDataId] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [FK_PhraseDataLanguageData]
    FOREIGN KEY ([LanguageDataId])
    REFERENCES [dbo].[LanguageDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PhraseDataLanguageData'
CREATE INDEX [IX_FK_PhraseDataLanguageData]
ON [dbo].[PhraseDatas]
    ([LanguageDataId]);
GO

-- Creating foreign key on [TranslationDatas_Id] in table 'TranslationDataPhraseData'
ALTER TABLE [dbo].[TranslationDataPhraseData]
ADD CONSTRAINT [FK_TranslationDataPhraseData_TranslationData]
    FOREIGN KEY ([TranslationDatas_Id])
    REFERENCES [dbo].[TranslationDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PhraseDatas_Id] in table 'TranslationDataPhraseData'
ALTER TABLE [dbo].[TranslationDataPhraseData]
ADD CONSTRAINT [FK_TranslationDataPhraseData_PhraseData]
    FOREIGN KEY ([PhraseDatas_Id])
    REFERENCES [dbo].[PhraseDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TranslationDataPhraseData_PhraseData'
CREATE INDEX [IX_FK_TranslationDataPhraseData_PhraseData]
ON [dbo].[TranslationDataPhraseData]
    ([PhraseDatas_Id]);
GO

-- Creating foreign key on [UserDataId] in table 'TranslationDatas'
ALTER TABLE [dbo].[TranslationDatas]
ADD CONSTRAINT [FK_UserDataTranslationData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDataTranslationData'
CREATE INDEX [IX_FK_UserDataTranslationData]
ON [dbo].[TranslationDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [UserDataId] in table 'LanguageDatas'
ALTER TABLE [dbo].[LanguageDatas]
ADD CONSTRAINT [FK_UserDataLanguageData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDataLanguageData'
CREATE INDEX [IX_FK_UserDataLanguageData]
ON [dbo].[LanguageDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [UserDataId] in table 'LineDatas'
ALTER TABLE [dbo].[LineDatas]
ADD CONSTRAINT [FK_UserDataLineData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDataLineData'
CREATE INDEX [IX_FK_UserDataLineData]
ON [dbo].[LineDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [PhraseDataId] in table 'LineDatas'
ALTER TABLE [dbo].[LineDatas]
ADD CONSTRAINT [FK_PhraseDataLineData]
    FOREIGN KEY ([PhraseDataId])
    REFERENCES [dbo].[PhraseDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PhraseDataLineData'
CREATE INDEX [IX_FK_PhraseDataLineData]
ON [dbo].[LineDatas]
    ([PhraseDataId]);
GO

-- Creating foreign key on [ContextPhraseDataId] in table 'TranslationDatas'
ALTER TABLE [dbo].[TranslationDatas]
ADD CONSTRAINT [FK_ContextPhraseDataTranslationData]
    FOREIGN KEY ([ContextPhraseDataId])
    REFERENCES [dbo].[PhraseDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContextPhraseDataTranslationData'
CREATE INDEX [IX_FK_ContextPhraseDataTranslationData]
ON [dbo].[TranslationDatas]
    ([ContextPhraseDataId]);
GO

-- Creating foreign key on [PhraseDataId] in table 'DefaultStudyAdvisorKnowledgeBeliefDatas'
ALTER TABLE [dbo].[DefaultStudyAdvisorKnowledgeBeliefDatas]
ADD CONSTRAINT [FK_PhraseDataDefaultStudyAdvisorKnowledgeBeliefData]
    FOREIGN KEY ([PhraseDataId])
    REFERENCES [dbo].[PhraseDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PhraseDataDefaultStudyAdvisorKnowledgeBeliefData'
CREATE INDEX [IX_FK_PhraseDataDefaultStudyAdvisorKnowledgeBeliefData]
ON [dbo].[DefaultStudyAdvisorKnowledgeBeliefDatas]
    ([PhraseDataId]);
GO

-- Creating foreign key on [PhraseDataId] in table 'PhraseBeliefDatas'
ALTER TABLE [dbo].[PhraseBeliefDatas]
ADD CONSTRAINT [FK_PhraseDataPhraseBeliefData]
    FOREIGN KEY ([PhraseDataId])
    REFERENCES [dbo].[PhraseDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PhraseDataPhraseBeliefData'
CREATE INDEX [IX_FK_PhraseDataPhraseBeliefData]
ON [dbo].[PhraseBeliefDatas]
    ([PhraseDataId]);
GO

-- Creating foreign key on [UserDataId] in table 'PhraseBeliefDatas'
ALTER TABLE [dbo].[PhraseBeliefDatas]
ADD CONSTRAINT [FK_UserDataPhraseBeliefData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDataPhraseBeliefData'
CREATE INDEX [IX_FK_UserDataPhraseBeliefData]
ON [dbo].[PhraseBeliefDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [UserDataId] in table 'MultiLineTextDatas'
ALTER TABLE [dbo].[MultiLineTextDatas]
ADD CONSTRAINT [FK_MultiLineTextDataUserData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MultiLineTextDataUserData'
CREATE INDEX [IX_FK_MultiLineTextDataUserData]
ON [dbo].[MultiLineTextDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [LineDatas_Id] in table 'LineDataMultiLineTextData'
ALTER TABLE [dbo].[LineDataMultiLineTextData]
ADD CONSTRAINT [FK_LineDataMultiLineTextData_LineData]
    FOREIGN KEY ([LineDatas_Id])
    REFERENCES [dbo].[LineDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MultiLineTextDatas_Id] in table 'LineDataMultiLineTextData'
ALTER TABLE [dbo].[LineDataMultiLineTextData]
ADD CONSTRAINT [FK_LineDataMultiLineTextData_MultiLineTextData]
    FOREIGN KEY ([MultiLineTextDatas_Id])
    REFERENCES [dbo].[MultiLineTextDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LineDataMultiLineTextData_MultiLineTextData'
CREATE INDEX [IX_FK_LineDataMultiLineTextData_MultiLineTextData]
ON [dbo].[LineDataMultiLineTextData]
    ([MultiLineTextDatas_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------