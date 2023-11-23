SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[report_status_detail]
@CONFLICT INT = NULL
AS
BEGIN		

SET @CONFLICT        = ISNULL(@CONFLICT, 0) 

SELECT T.STATUS, COUNT(DISTINCT T.ID) AS 'COUNT' 
FROM 
(
	SELECT C.ID, LEFT(P.VALUE,CASE WHEN CHARINDEX('/',P.VALUE) = 0 THEN LEN(P.VALUE) ELSE  CHARINDEX('/',P.VALUE) - 1 END) AS STATUS
	FROM APPCOMPROMISES C
	JOIN APPPARAMETER P ON P.ID = C.STATUSID
	JOIN APPCOMPROMISELOCATIONS CL ON CL.COMPROMISEID  = C.ID
	JOIN APPSOCIALCONFLICTLOCATIONS SCL ON SCL.ID = CL.SOCIALCONFLICTLOCATIONID
	JOIN APPRECORDS R ON R.ID = C.RECORDID 
	WHERE 
		  SCL.SOCIALCONFLICTID  = CASE WHEN @CONFLICT    > 0 THEN @CONFLICT    ELSE SCL.SOCIALCONFLICTID  END 
) T
GROUP BY T.STATUS

END;
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[report_dialog_space]
@SpaceDialogId INT, @CreatorUserId BIGINT, @Command VARCHAR(1)
AS
BEGIN
	--HEADER
	IF @Command = 'C'
	BEGIN
		SET LANGUAGE Spanish
		SELECT 
		CONVERT(VARCHAR(10), [L].[CreationTime], 103) AS [CreationTime],
		CONCAT(CONVERT(VARCHAR, CASE WHEN DAY(ISNULL([L].[LastModificationTime], [L].[CreationTime])) < 10 THEN '0' ELSE '' END),
		CONVERT(VARCHAR, DAY(ISNULL([L].[LastModificationTime], [L].[CreationTime]))),
		' de ', 
		LOWER(DATENAME(MONTH, ISNULL([L].[LastModificationTime], [L].[CreationTime]))), 
		' de ',
		CONVERT(VARCHAR, YEAR(ISNULL([L].[LastModificationTime], [L].[CreationTime])))) AS [LastModificationTime],
		[L].[Code] AS [CaseCode],
		UPPER([L].[CaseName]) AS [CaseName],
        ISNULL([P].[Name], '') AS [ElaboradoPor],
        SC.code as CODIGOSOCIALCONFLICTO, 
        SC.CaseName as NOMBRESOCIALCONFLICTO
		FROM [AppDialogSpaces] [L] 
        LEFT JOIN [dbo].[AppPersons] P on L.PersonId = P.Id
        INNER JOIN [dbo].[AppSocialConflicts] SC on SC.Id = L.SocialConflictId
		WHERE [L].[Id] = @SpaceDialogId AND
			  [L].[IsDeleted] = 0

	END

	-- LOCATIONS
	IF @Command = 'L'
	BEGIN
		SELECT 
        TU.Name as [UnidadTerritorial],
		[DE].[Name] AS [Department],
		[P].[Name] AS [Province],
		[DI].[Name] AS [District],
		ISNULL([R].[Name], '') AS [Region],
		ISNULL([L].[Ubication], '') AS [Ubication]
		FROM [AppDialogSpaceLocations] [L]
        INNER JOIN [dbo].[AppTerritorialUnits] TU
        ON L.TerritorialUnitId = TU.ID
		INNER JOIN [AppDepartments] [DE]
		ON [DE].[Id] = [L].[DepartmentId]
		INNER JOIN [AppProvinces] [P]
		ON [P].[Id]  = [L].[ProvinceId]
		INNER JOIN [AppDistricts] [DI]
		ON [DI].[Id] = [L].[DistrictId]
		LEFT JOIN [AppRegions] [R]
		ON [R].[Id] = [L].[RegionId]
		WHERE [DE].[IsDeleted]            = 0 AND
			  [P].[IsDeleted]             = 0 AND
			  [DI].[IsDeleted]            = 0 AND 
			  ISNULL([R].[IsDeleted], 0)  = 0 AND
			  [L].[DialogSpaceId] = @SpaceDialogId
	END

	--SECTORS 
	IF @Command = 'S'
	BEGIN
	select DG.NAME as SECTOR, DIST.Name as DISTRITO, PROV.Name as PROVINCIA, DEP.Name as DEPARTAMENTO from [dbo].[AppDialogSpaceLeaders] SL
inner join AppDirectoryGovernments DG on SL.DirectoryGovernmentId = DG.Id
inner join AppDistricts dist on DIST.Id = DG.DistrictId
inner join AppProvinces prov on PROV.Id = dist.ProvinceId
inner join AppDepartments dep on DEP.ID = PROV.DepartmentId
 where SL.IsDeleted=0 and SL.DialogSpaceId =@SpaceDialogId
 	END


    --DOCUMENTS 
	IF @Command = 'D'
	BEGIN
select DT.NAME as TIPODOCUMENTO, Document as NUMERODOCUMENTO, D.DocumentTime as FECHAPUBLICACION, D.Observation, D.VigencyTime as FECHAVIGENCIA,
DS.Name as SITUACION
 from [dbo].[AppDialogSpaceDocuments] D
inner join  [dbo].[AppDialogSpaceDocumentTypes] DT on D.DialogSpaceDocumentTypeId = DT.Id
inner join  [dbo].[AppDialogSpaceDocumentSituations] DS on D.DialogSpaceDocumentSituationId = DS.Id
where D.DialogSpaceId =@SpaceDialogId
 	END


END
GO


insert into [dbo].[AppReports] ( CreationTime, IsDeleted, name, [Description],Enabled) values (getdate(), 0, 'dialog_space_report','Reporte de espacio de dialogo',1)




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[report_dialog_space_vencimiento] AS BEGIN
SELECT DS.id AS Id,
       DS.Code AS DialogSpaceCode,
       DS.CaseName AS DialogSpaceCaseName,
       SC.Code AS SocialConflictCode,
       SC.CaseName AS SocialConflictCaseName,
       ISNULL(TU.Name, '')  AS [UnidadTerritorialName],
       CONCAT([DI].[Name], '/', [P].[Name], '/', [DE].[Name]) AS UnidadTerritorialUbigeo,
        ISNULL(DG.NAME, '')    AS SectorName,
       CONCAT(DIST.Name, '/', PROV.Name, '/', DEP.Name) AS SectorUbigeo,
       DS.EndDate,
       DS.StartDate,
       DS.PublicationDate,
       DS.CreationTime,
       CONCAT(usC.Name, ' ', usC.Surname) AS RegisteredUser,
       DS.LastModificationTime,
       CONCAT(usM.Name, ' ', usM.Surname) AS ChangeUser,

  (SELECT COUNT(DISTINCT T.ID) AS 'COUNT'
   FROM
     (SELECT C.ID,
             LEFT(P.VALUE, CASE
                               WHEN CHARINDEX('/', P.VALUE) = 0 THEN LEN(P.VALUE)
                               ELSE CHARINDEX('/', P.VALUE) - 1
                           END) AS STATUS
      FROM APPCOMPROMISES C
      JOIN APPPARAMETER P ON P.ID = C.STATUSID
      JOIN APPCOMPROMISELOCATIONS CL ON CL.COMPROMISEID = C.ID
      JOIN APPSOCIALCONFLICTLOCATIONS SCL ON SCL.ID = CL.SOCIALCONFLICTLOCATIONID
      JOIN APPRECORDS R ON R.ID = C.RECORDID
      WHERE SCL.SOCIALCONFLICTID = CASE
                                       WHEN DS.SocialConflictId > 0 THEN DS.SocialConflictId
                                       ELSE SCL.SOCIALCONFLICTID
                                   END ) T
   WHERE T.STATUS='Abierto'
   GROUP BY T.STATUS) AS CountOpenCommitments,

  (SELECT COUNT(DISTINCT T.ID) AS 'COUNT'
   FROM
     (SELECT C.ID,
             LEFT(P.VALUE, CASE
                               WHEN CHARINDEX('/', P.VALUE) = 0 THEN LEN(P.VALUE)
                               ELSE CHARINDEX('/', P.VALUE) - 1
                           END) AS STATUS
      FROM APPCOMPROMISES C
      JOIN APPPARAMETER P ON P.ID = C.STATUSID
      JOIN APPCOMPROMISELOCATIONS CL ON CL.COMPROMISEID = C.ID
      JOIN APPSOCIALCONFLICTLOCATIONS SCL ON SCL.ID = CL.SOCIALCONFLICTLOCATIONID
      JOIN APPRECORDS R ON R.ID = C.RECORDID
      WHERE SCL.SOCIALCONFLICTID = CASE
                                       WHEN DS.SocialConflictId > 0 THEN DS.SocialConflictId
                                       ELSE SCL.SOCIALCONFLICTID
                                   END ) T
   WHERE T.STATUS='Cerrado'
   GROUP BY T.STATUS) AS CountClosedCommitments
FROM AppDialogSpaces DS
LEFT JOIN AppSocialConflicts SC ON DS.SocialConflictId = SC.Id
LEFT JOIN [AppDialogSpaceLocations] [L] ON L.DialogSpaceId = DS.id
LEFT JOIN [dbo].[AppTerritorialUnits] TU ON L.TerritorialUnitId = TU.ID
LEFT JOIN [AppDepartments] [DE] ON [DE].[Id] = [L].[DepartmentId]
LEFT JOIN [AppProvinces] [P] ON [P].[Id] = [L].[ProvinceId]
LEFT JOIN [AppDistricts] [DI]ON [DI].[Id] = [L].[DistrictId]
LEFT JOIN [dbo].[AppDialogSpaceLeaders] SL ON SL.DialogSpaceId = DS.Id
LEFT JOIN AppDirectoryGovernments DG ON SL.DirectoryGovernmentId = DG.Id
LEFT JOIN AppDistricts dist ON DIST.Id = DG.DistrictId
LEFT JOIN AppProvinces prov ON PROV.Id = dist.ProvinceId
LEFT JOIN AppDepartments dep ON DEP.ID = PROV.DepartmentId
LEFT JOIN [dbo].[AbpUsers] usC ON DS.CreatorUserId = usC.id
LEFT JOIN [dbo].[AbpUsers] usM ON DS.LastModifierUserId = usM.id END;

GO