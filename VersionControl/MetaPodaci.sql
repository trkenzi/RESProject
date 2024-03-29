/*
   ponedeljak, 03. jun 2019.13:30:50
   User: 
   Server: MAJAPC
   Database: RESProjekat
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Table_1
	(
	AutorRevizije varchar(50) NOT NULL,
	DatumKreiranjaRevizije smalldatetime NOT NULL,
	RedniBrojRevizije int NOT NULL,
	JedinstvenaOznakaRevizije varchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Table_1 SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
