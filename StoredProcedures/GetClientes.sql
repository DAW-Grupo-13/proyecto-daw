USE [ClienteDB]
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
ALTER PROCEDURE [dbo].[GetClientes]
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
	DECLARE @apellido  as varchar(168)
	DECLARE @ciudad	   as varchar(128)

	BEGIN TRY
		IF (@iTransaccion = 'CONSULTA_CLIENTES_EDAD')
		BEGIN
			select a.*, b.Descripcion 
			from Clientes a
			inner join Ciudad b
			on (a.CiudadId = b.Id)
			where Edad between 30 and 50

			set @respuesta = 'ok'
			set @leyenda = 'Consulta exitosa'
		END

		IF (@iTransaccion = 'CONSULTA_CLIENTES_APELLIDO')
		BEGIN
			set @apellido = (select @iXML.value('(/Clientes/Apellidos)[1]', 'VARCHAR(168)'))
			
			select a.*, b.Descripcion 
			from Clientes a
			inner join Ciudad b
			on (a.CiudadId = b.Id)
			where Apellidos like @apellido+'%'

			set @respuesta = 'ok'
			set @leyenda = 'Consulta exitosa'
		END

		IF (@iTransaccion = 'CONSULTA_CLIENTES_CIUDAD')
		BEGIN
			set @ciudad = (select @iXML.value('(/Clientes/Ciudad/Descripcion)[1]', 'VARCHAR(128)'))
			
			select a.*, b.Descripcion 
			from Clientes a
			inner join Ciudad b
			on (a.CiudadId = b.Id)
			where b.Descripcion = @ciudad

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
