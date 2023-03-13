USE [FerreteriaDB]
GO
/****** Object:  StoredProcedure [dbo].[GetClientes]    Script Date: 28/2/2023 0:39:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
DROP PROCEDURE IF EXISTS [dbo].[ValidateUser]
GO
CREATE PROCEDURE [dbo].[ValidateUser]
	-- Add the parameters for the stored procedure here
	@iTransaccion as varchar(50),
	@iXML         as XML = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @respuesta as varchar(10)
	DECLARE @leyenda   as varchar(255)
	DECLARE @usuario  as varchar(30)
	DECLARE @clave	   as varchar(30)

	set @usuario = (select @iXML.value('(/Usuarios/Usuario)[1]', 'VARCHAR(30)'))
	set @clave = (select @iXML.value('(/Usuarios/Clave)[1]', 'VARCHAR(30)'))

	BEGIN TRY
		IF (@iTransaccion = 'VALIDA_USUARIO')
		BEGIN
			select *
			from Usuarios
			where Usuario = @usuario
			and   Clave   = @clave

			set @respuesta = 'ok'
			set @leyenda = 'Consulta exitosa'
		END
	END TRY

	BEGIN CATCH
		set @respuesta = 'error'
		set @leyenda = 'Error al ejecutar comando en la BD: ' + ERROR_MESSAGE()
	END CATCH

	select @respuesta, @leyenda
END
