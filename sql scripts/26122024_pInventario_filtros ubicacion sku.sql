USE [erp_neogenis_13122024]
GO

/****** Object:  StoredProcedure [dbo].[pInventario]    Script Date: 26/12/2024 01:18:36 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[pInventario]
	@cmd int,
	@cfdid bigint=0,
	@txtSearch varchar(500)=null,
	@productoid bigint=0,
	@codigo varchar(50)=null,
	@descripcion varchar(400)=null, 
	@cantidad float=0, 
	@costo_unitario varchar(50)=null,
	@costo_unitario_var varchar(50)=null,
	@existencia float=0, 
	@documento varchar(10)=null, 
	@userid int=0,
	@comentario varchar(1000)=null,
	@lote varchar(50)=null,
	@caducidad varchar(50)=null,
	@almacenid bigint=0,
	@inventarioid bigint=0,
	@seleccionId int =0,
	@seleccion int = 0,
	@identificador int = 0,
	@barcodeLocation varchar(100) = null,
	@barcodeLocationOrigen varchar(100)  = null,
	@idCiclico int = 0,
	@cantidadFinal int = 0,
	@cantidadDiferencia  int = 0,
	@cadena varchar(300) = null,
	@cantidadERP int = 0,
	@barcodeLocationERP varchar(150)= null,
	@barcodeLocationAPI varchar(150)=null,
	@cantidadDestino int =0,
	@cantidadOrigen int = 0,
	@barcodeLocationDestino varchar(150)=null,
	@id int =0,
	@cantidadOrigenUbi int = 0,
	@proX varchar (100)='',
	@marca int =0,
	@ubicacion varchar(80)='',
	@contPick int = 0,
	@sku varchar(50) = ''

AS
BEGIN
	set nocount on;
	
	declare @tmpExistencia float
	
	/*	Reporte de abastecimiento	*/
	if @cmd=1
		begin
			select 
				a.id, 
				isnull(a.codigo,'') as codigo, 
				isnull(a.unidad,'') as unidad, 
				isnull(a.descripcion,'') as descripcion, 
				isnull(a.punto_reorden,0) as punto_reorden, 
				isnull(dbo.fnExistenciaAlmacen(a.id,1),0)+isnull(dbo.fnExistenciaAlmacen(a.id,2),0)+isnull(dbo.fnExistenciaAlmacen(a.id,3),0) as existencia
			from 
				tblMisproductos a 
			where 
				isnull(a.existencia,0) <= isnull(a.punto_reorden,0)
			order by 
				a.descripcion
		end
		
	/*	Busca producto	*/
	if @cmd=2
		begin
			select 
				a.id, 
				isnull(a.codigo,'') as codigo, 
				isnull(a.unidad,'') as unidad, 
				isnull(a.descripcion,'') as descripcion, 
				isnull(a.unitario,0) as unitario, 
				isnull(a.unitario2,0) as unitario2, 
				isnull(a.unitario3,0) as unitario3,
				isnull(b.nombre,'') as tasa, 
				isnull(existencia,0) as existencia, 
				isnull(a.costo_estandar,0) as costo_estandar, 
				isnull(a.perecederoBit,0) as perecederoBit,
				isnull(a.monterrey,0) as monterrey, 
				isnull(a.mexico,0) as mexico, 
				isnull(a.guadalajara,0) as guadalajara, 
				isnull(a.mermas,0) as mermas,
				isnull(a.matriz,0) as matriz,
				dbo.fnProductosDisponibles(a.id) as disponibles
			from 
				tblMisproductos a 
				left join tblTasa b on a.tasaid=b.id
			where 
				( a.codigo like '%' + @txtSearch + '%' or a.descripcion like '%' + @txtSearch + '%' )
			order by 
				a.descripcion
		end
		
	/*	Agrega entrada alamcén	*/
	if @cmd=3
		begin
			set @tmpExistencia = 0
			
			--insert into tblInventario (ordenid,ordendetalleid,productoid,cantidad,caducidad,lote,costo_variable,almacenid,fecha,userid,existencia)
			--values(0,0,@productoid,@cantidad,@caducidad,@lote,convert(money, @costo_unitario_var),@almacenid,getdate(),@userid,@cantidad)
			
			if @almacenid=1
			begin
				update tblMisProductos set monterrey=(isnull(@cantidad,0) + isnull(monterrey,0)) where id=@productoid
				select @tmpExistencia = isnull(monterrey,0) from tblMisProductos where id=@productoid
				--select @tmpExistencia = isnull(sum(existencia),0) from tblInventario where id=@productoid and almacenid=@almacenid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 2, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
			else if @almacenid=2
			begin
				update tblMisProductos set mexico=(isnull(@cantidad,0) + isnull(mexico,0)) where id=@productoid
				select @tmpExistencia = isnull(mexico,0) from tblMisProductos where id=@productoid
				--select @tmpExistencia = isnull(sum(existencia),0) from tblInventario where id=@productoid and almacenid=@almacenid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 2, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end	
			else if @almacenid=3
			begin
				update tblMisProductos set guadalajara=(isnull(@cantidad,0) + isnull(guadalajara,0)) where id=@productoid
				select @tmpExistencia = isnull(guadalajara,0) from tblMisProductos where id=@productoid
				--select @tmpExistencia = isnull(sum(existencia),0) from tblInventario where id=@productoid and almacenid=@almacenid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 2, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
			else if @almacenid=4
			begin
				update tblMisProductos set mermas=(isnull(@cantidad,0) + isnull(mermas,0)) where id=@productoid
				select @tmpExistencia = isnull(mermas,0) from tblMisProductos where id=@productoid
				--select @tmpExistencia = isnull(sum(existencia),0) from tblInventario where id=@productoid and almacenid=@almacenid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 2, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
			else if @almacenid=5
			begin
				update tblMisProductos set matriz=(isnull(@cantidad,0) + isnull(matriz,0)) where id=@productoid
				select @tmpExistencia = isnull(matriz,0) from tblMisProductos where id=@productoid

				/*	Descuenta productos de almacen  */
			    update TOP (1) tblAlmacenado set quantity =isnull(quantity,0)+isnull(@cantidad,0), dateTime = GETDATE() where productId=@productoid AND quantity > 0 

				--select @tmpExistencia = isnull(sum(existencia),0) from tblInventario where id=@productoid and almacenid=@almacenid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 2, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
		end
		
	/*	Ultimos movimientos de entradas	*/
	if @cmd=4
		begin
			select top 50
				a.id, 
				convert(varchar(10), a.fecha, 103) as fecha,
				isnull(a.codigo,'') as codigo, 
				isnull(a.descripcion,'') as descripcion,
				isnull(a.cantidad,0) as cantidad, 
				isnull(a.costo_unitario,0) as costo_unitario,
				isnull(a.costo_unitario_var,0) as costo_unitario_var, 
				isnull(a.lote,'') as lote,
				convert(varchar(10), a.caducidad, 103) as caducidad,
				isnull(a.existencia,0) as existencia, 
				isnull(a.comentario,'') as comentario,
				isnull(a.documento,'') as documento, 
				isnull(b.nombre,'') as almacen
			from 
				tblMovimientos a
				left join tblAlmacen b on a.almacenid=b.id
			where 
				a.tipoid=2 
			order by 
				id desc
		end
		
	/*	Agrega ajuste	*/
	if @cmd=5
		begin
			set @tmpExistencia = 0
			
			if @almacenid=1
			begin
				update tblMisProductos set monterrey=(isnull(monterrey,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(monterrey,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 3, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
			else if @almacenid=2
			begin
				update tblMisProductos set mexico=(isnull(mexico,0) - isnull(@cantidad,0)) where id=@productoid 
				select @tmpExistencia = isnull(mexico,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 3, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end	
			else if @almacenid=3
			begin
				update tblMisProductos set guadalajara=(isnull(guadalajara,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(guadalajara,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 3, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
			else if @almacenid=4
			begin
				update tblMisProductos set mermas=(isnull(mermas,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(mermas,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 3, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
			else if @almacenid=5
			begin
				update tblMisProductos set matriz=(isnull(matriz,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(matriz,0) from tblMisProductos where id=@productoid

				/*	Descuenta productos de almacen  */
			    update TOP (1) tblAlmacenado set quantity =isnull(quantity,0)-isnull(@cantidad,0), dateTime = GETDATE() where productId=@productoid AND quantity > 0 
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, costo_unitario_var, existencia, documento, userid, comentario, lote, caducidad, almacenid )
				values ( 3, @productoid, @codigo, @descripcion, @cantidad, convert(money, @costo_unitario), convert(money, @costo_unitario_var), isnull(@tmpExistencia,0), @documento, @userid, @comentario, @lote, @caducidad, @almacenid )
			end
		end
		
	/*	Ultimos movimientos de ajustes	*/
	if @cmd=6
		begin
			select top 50 
				a.id, 
				convert(varchar(10), a.fecha, 103) as fecha,
				isnull(a.codigo,'') as codigo, 
				isnull(a.descripcion,'') as descripcion,
				isnull(a.cantidad,0) as cantidad, 
				isnull(a.costo_unitario,0) as costo_unitario,
				isnull(a.costo_unitario_var,0) as costo_unitario_var, 
				isnull(a.lote,'') as lote,
				convert(varchar(10), a.caducidad, 103) as caducidad,
				isnull(a.existencia,0) as existencia, 
				isnull(a.comentario,'') as comentario,
				isnull(a.documento,'') as documento, 
				isnull(b.nombre,'') as almacen
			from 
				tblMovimientos a
				left join tblAlmacen b on a.almacenid=b.id
			where 
				tipoid=3 
			order by 
				id desc
		end
		
	/*	Descarga Inventario de un CFDI	*/
	if @cmd=7	
		begin
			declare c cursor
			for
			select productoid, cantidad from tblCFD_Partidas where cfdid=@cfdid
				
			open c
			declare @tmpSerie varchar(20)
			declare @tmpFolio bigint
			declare @existenciaIdx float
			declare @prodIdx bigint
			declare @cantIdx float
			fetch next from c into @prodIdx, @cantIdx
			while (@@FETCH_STATUS <> -1)
				begin
					 if (@@FETCH_STATUS <> -2)
					 -------------------------------------------------------------------------------------------------------------------
					  begin
						if ( select isnull(inventariableBit,0) from tblMisProductos where id=@prodIdx  ) = 1
							begin
								/*	Obtiene existencia actual, serie y folio	*/
								set @existenciaIdx=0
								select @existenciaIdx=isnull(existencia,0) from tblMisProductos where id=@prodIdx 
								select @tmpSerie=serie, @tmpFolio=folio from tblCFD where id=@cfdid 
								/*	Agrega registro a la tabla de movimientos	*/
								insert into tblMovimientos ( fecha, tipoid, productoid, codigo, descripcion, cantidad, existencia, cfdid, serie, folio, userid ) 
								select GETDATE(), 1, @prodIdx, codigo, descripcion, @cantIdx, ( isnull(@existenciaIdx,0)-isnull(@cantIdx,0) ), @cfdid, @tmpSerie, @tmpFolio, @userid from tblMisProductos where id=@prodIdx 
								/*	Actualiza existencias	*/
								update tblMisProductos set existencia=isnull(existencia,0)-isnull(@cantIdx,0) where id=@prodIdx  
							end
					  end
					 -------------------------------------------------------------------------------------------------------------------
				fetch next from c into @prodIdx, @cantIdx
				end
			close c
			deallocate c
		end

	/*	Kardex de un producto	*/
	if @cmd=8
		begin
			select
				m.id, 
				convert(varchar(10), fecha, 103) as fecha,
				isnull(t.nombre,'') as movimiento,
				isnull(m.codigo,'') as codigo,
				isnull(p.unidad,'') as unidad,
				isnull(m.descripcion,'') as descripcion,
				isnull(cantidad,0) as cantidad,
				isnull(costo_unitario,0) as costo_unitario,
				isnull(m.existencia,0) as existencia,
				isnull(comentario,'') as comentario,
				case tipoid when 1 then isnull(serie,'') + convert(varchar(10), folio) 
							when 2 then isnull(documento,'') 
							when 3 then isnull(documento,'') 
					end as documento,
				isnull(a.nombre,'') as almacen
			from 
				tblMovimientos m
				left join tblTipoMovimientos t on t.id=m.tipoid
				left join tblAlmacen a on a.id=m.almacenid
				left join tblMisProductos p on m.productoid=p.id
			where 
				productoid=@productoid
				--and m.almacenid=3
			order by 
				id 
			desc
		end
	-- exec pInventario @cmd=8, @productoid='32047'
	-- exec pInventario @cmd=8, @productoid='32054'

	/*	Listar productos para ajustes de inventario	*/
	if @cmd=9
	begin
			select 
				b.id, 
				a.id as productoid, 
				c.id as almacenid, 
				a.codigo,
				isnull(a.descripcion,'') as descripcion,
				1 As cantidad,
				isnull(a.unidad,'') As unidad, 
				case when isnull(b.lote,'') = '' then '--' else b.lote end as lote,
				case when isnull(a.perecederoBit,0) = 0 then '--' else CONVERT(varchar(10), b.caducidad, 103) end as caducidad, 
				isnull(b.existencia,0) as existencia, 
				c.nombre as almacen,
				dbo.fnProductosDisponiblesTransferencia(b.productoid,b.id) as disponibles
			from 
				tblMisProductos a
				inner join tblInventario b on a.id=b.productoid
				inner join tblAlmacen c on c.id=b.almacenid
			where
				( a.codigo = @txtSearch or a.descripcion like '%' + @txtSearch + '%' )
				and isnull(b.existencia,0)>0
			order by 
				descripcion asc
	end

	/*	Ultimos movimientos de entradas Sin utilizar	*/ 
	if @cmd=10
		begin
			select top 50
				a.id, 
				convert(varchar(10), a.fecha, 103) as fecha,
				isnull(a.codigo,'') as codigo, 
				isnull(a.descripcion,'') as descripcion,
				isnull(a.cantidad,0) as cantidad, 
				isnull(a.costo_unitario,0) as costo_unitario,
				isnull(a.costo_unitario_var,0) as costo_unitario_var, 
				isnull(a.lote,'') as lote,
				convert(varchar(10), a.caducidad, 103) as caducidad,
				isnull(a.existencia,0) as existencia, 
				isnull(a.comentario,'') as comentario,
				isnull(a.documento,'') as documento, 
				isnull(b.nombre,'') as almacen
			from 
				tblMovimientos a
				left join tblAlmacen b on a.almacenid=b.id
			where 
				a.tipoid in (8,15)
			order by 
				id desc
		end

		
	if @cmd=11
		begin
			select 
			--distinct
			     isnull(a.id,0) as id
				--,ISNULL(b.sku,'') as sku
				,isnull(b.descripcion,'') as descripcion
				,isnull(p.nombre,'') as marca
				,isnull(idCarga,1)as idCarga
				,isnull(fecha,'') as fecha
				,isnull(a.productId,0) as productId
				,isnull(b.codigo,'') as barcode
				,isnull(c.barcode,'') as barcodeLocationERP
				--,isnull(b.num_disponibles,0) as quantityTeorico
				, ISNULL(dbo.fnReacUbicacionAlmac(a.productId,c.barcode),0) as quantityTeorico
				,isnull(a.quantityTeorico,0) as quantityTeoricoApi
				,isnull(a.barcodeLocation,'') as barcodeLocationAPI
				,ISNULL(a.quantityTeorico,0) + isnull(a.quantityDifference,0) as quantityFisico
				--,ISNULL(b.matriz,0) + isnull(a.quantityDifference,0) as quantityFisico
				, (ISNULL(a.quantityTeorico,0) + isnull(a.quantityDifference,0)) - ISNULL(dbo.fnReacUbicacionAlmac(a.productId, a.barcodeLocation), 0) as quantityDifferenceERP
				,isnull(a.quantityDifference,0) as quantityDifference
				--,isnull(a.quantityTeorico,0) + isnull(quantityDifference,0) as quantityFinal   -- API vs API
				--,isnull(b.matriz,0) + isnull(quantityDifference,0) as quantityFinal -- ERP vs API
				,ISNULL(a.quantityTeorico,0) + isnull(a.quantityDifference,0) as quantityFinal -- ERP vs API
				--,isnull(b.num_disponibles,0) + isnull(quantityDifference,0) as quantityFinal -- ERP vs API
				,0 as chkInventC
				,isnull(a.reason,'') as comentario
				,isnull(a.estatus,0) 
				,isnull(c.estatus,0) 
			from
			tblInventariado_Ciclico a
			left join tblMisProductos b on a.productId = b.id
			left join tblProyecto p on b.proyectoid= p.id
			left join tblAlmacenado c on a.productId=c.productId --and a.barcodeLocation=c.barcode
			where 
				ISNULL(a.estatus,0) = 0
			and isnull(c.estatus,0) = 0
			--and ISNULL(c.inventariado,0) = 1
			and (@marca = 0 or isnull(b.proyectoid,0) = @marca)
			and (@ubicacion = '' or RTRIM(LTRIM(ISNULL(c.barcode,'')))=RTRIM(LTRIM(@ubicacion)))
			and (@sku = '' or TRIM(LTRIM(isnull(b.codigo, ''))) = RTRIM(LTRIM(@sku)))
			order by a.productId asc,quantityDifference asc
			--and a.productId = 32049
			--where 
			--fecha = GETDATE()

			-- Validar la ubicacion a donde lo relaciona 
		END

	IF @cmd = 12
BEGIN
    DECLARE @sumTotales INT;
    DECLARE @comenta VARCHAR(MAX);
    DECLARE @idInventariadoX INT;
    DECLARE @idCargaX INT;
    DECLARE @productoidX INT;
    DECLARE @barcodeLocationX VARCHAR(50);
    DECLARE @CantFinalX INT;
    DECLARE @DiferenciaX INT;
    DECLARE @identityX INT;
    DECLARE @comentaX VARCHAR(80);


    -- Obtener el productId basado en la selección
    SET @productoidX = (SELECT productId FROM tblInventariado_Ciclico WHERE id = @seleccion);

    -- Marcar el producto en tblAlmacenado como procesado (estatus = 2) si no ha sido inventariado
    UPDATE tblAlmacenado 
    SET estatus = 2, dateTime = GETDATE() 
    WHERE productId = @productoidX AND ISNULL(estatus, 0) = 0;

    -- Si se va a cambiar de ubicación, registrar la ubicación y cantidad anterior
    IF @barcodeLocationERP <> @barcodeLocationAPI
    BEGIN
        SET @comentaX = 'Se cambia de la Ubicación ' + ISNULL(@barcodeLocationERP, '') + ' a la Ubicación ' + ISNULL(@barcodeLocationAPI, '') 
    END

    -- Insertar un nuevo registro en tblAlmacenado si se trata de una nueva ubicación
    IF NOT EXISTS (SELECT 1 FROM tblAlmacenado WHERE productId = @productoidX AND barcode = @barcodeLocationAPI)
    BEGIN
        INSERT INTO tblAlmacenado (orderId, productId, barcode, quantity, dateTime, estatus, inventariado)
        SELECT 0, productId, @barcodeLocationAPI, @cantidadFinal, GETDATE(), 1, 1 
        FROM tblInventariado_Ciclico WHERE id = @seleccion;
        
        SET @identityX = SCOPE_IDENTITY(); 
    END
    ELSE
    BEGIN
        -- Si ya existe la ubicación en tblAlmacenado, actualizar la cantidad
        UPDATE tblAlmacenado 
        SET quantity = @cantidadFinal, dateTime = GETDATE(), estatus = 1, inventariado = 1 
        WHERE productId = @productoidX AND barcode = @barcodeLocationAPI;
        
        SET @identityX = (SELECT id FROM tblAlmacenado WHERE productId = @productoidX AND barcode = @barcodeLocationAPI);
    END

    -- Actualizar la matriz de tblMisProductos con la suma de las cantidades sin considerar estatus o inventariado
    SELECT @sumTotales = ISNULL(SUM(a.quantity), 0) 
    FROM tblAlmacenado a 
    WHERE productId = @productoidX;

    UPDATE tblMisProductos 
    SET matriz = @sumTotales, fecha_act = GETDATE() 
    WHERE id = @productoidX;

    -- Registro en tblMovimientos
    IF @barcodeLocationERP = @barcodeLocationAPI
    BEGIN
        SET @comentaX = 'Ubicación ' + (SELECT barcode FROM tblAlmacenado WHERE id = @identityX);
        
        INSERT INTO tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
        SELECT 16, id, codigo, descripcion, (@cantidadFinal - @cantidadERP), ISNULL(costo_estandar, 0), @sumTotales, 0, 
               ISNULL(@comentario, ' ') + ' ' + ISNULL(@comentaX, ''), 5 
        FROM tblMisProductos 
        WHERE id = @productoidX;
    END
    ELSE
    BEGIN
        INSERT INTO tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
        SELECT 16, id, codigo, descripcion, @cantidadFinal, ISNULL(costo_estandar, 0), @sumTotales, 0, 
               ISNULL(@comentario, ' ') + ' ' + ISNULL(@comentaX, ''), 5 
        FROM tblMisProductos 
        WHERE id = @productoidX;
    END

    -- Marcar como inventariado el registro en tblInventariado_Ciclico
    UPDATE tblInventariado_Ciclico 
    SET estatus = 1 
    WHERE id = @seleccion;
END

		-- exec pInventario @cmd = 12 , @seleccion = 1
		-- exec pInventario @cmd = 12 , @seleccion = 2

		if @cmd = 13

		BEGIN

		update tblAlmacenado set estatus = 0 , inventariado = 1,dateTime= GETDATE() where isnull(estatus,0) = 1 and isnull(inventariado,0) = 1

		END

		if @cmd = 14
			BEGIN

				select 
				  isnull(a.id,0) id
				, ISNULL(b.descripcion,'') as descripcion
				, ISNULL(p.nombre,'') as marca
				, isnull(a.dateTime,'') as fecha
				, ISNULL(a.identifierId,0) as identifierId
				--, ISNULL(a.companyId,0) as companyId
				, ISNULL(p.nombre,0) as companyId
				--, ISNULL(a.userId,0) as userId
				, ISNULL(u.nombre,0) as userId
				--, ISNULL(a.productOrigeId,0) as productOrigeId
				, ISNULL(b.codigo,0) as productOrigeId
				, ISNULL(b.id,0) as productId
				, ISNULL(a.barcodeLocationOrige,'') as barcodeLocationOrige
				, ISNULL(a.quantityOrige,0) as quantityOrige
				, ISNULL(dbo.fnReacUbicacionAlmac(a.productOrigeId,a.barcodeLocationOrige),0) as quantityOrigenUbicacion
				, ISNULL(a.barcodeLocationDestination,'') as barcodeLocationDestination
				, ISNULL(a.quantityDestination,0) as quantityDestination
				, 0 as chkNuevo
				, 0 as chkReacomodo

				from tblReacomodo a
				left join tblMisProductos b on a.productOrigeId = b.id
				left join tblProyecto p on b.proyectoid = p.id
				left join tblUsuario u on a.userId=u.id
				where isnull(estatus,0) = 0
				and (@marca = 0 or a.companyId = @marca)
				and (@ubicacion = '' or a.barcodeLocationOrige = @ubicacion)

			END
			--select * from tblReacomodo

			-- la ubicacion si existe solo se actualizan los datos 
						-- y se restan las cantidades de la ubicacion pasada.
						-- llega solo la cantidad total esta bloqueado por parte de Handhell
						-- no edita total en TblMisproductos
						-- si no se reacomodo todo ? resto la cantidad del total de la ubicacion 
						-- si se reacomoda todo se elimina la ubicacion 
						-- Restar la cantidad del logar de origen Validar 
		if @cmd = 15
			BEGIN

				declare @idX2 int =0
				declare @cantidadX int =0
				declare @producto int =0
				declare @productoX int =0
				declare @cantidadDestinoX int =0
				declare @barcodeLocationDestinoX varchar(150)=''
			
				select @productoX=id from tblMisProductos where codigo = @proX

				select @idX2 =id ,@cantidadX= quantity from tblAlmacenado where productId = @productoX and barcode = @barcodeLocationOrigen and ISNULL(estatus,0)=0 
				

				if @cantidadX >= @cantidadDestino
				--( select quantity  from tblAlmacenado where id = @id ) >= @cantidad
							BEGIN
									UPDATE tblAlmacenado set quantity = (quantity-@cantidadDestino),dateTime= GETDATE() WHERE id = @idX2
									if (select quantity from tblAlmacenado where id =@idX2 ) >0
										BEGIN
											set @comenta = 'Se ajusta la ubicación ' + @barcodeLocationOrigen + ' dejándola con la cantidad de ' + CONVERT(varchar(10), @cantidadX - @cantidadDestino)
											insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
											select 17, id, codigo, descripcion, ISNULL(@cantidadDestino,0)* -1, ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5 from tblMisProductos where id=@productoX
										END
					
									--Insertar en la misma ubicacion si es el mismo distino o diferenciar si es una ubicacion nueva o sin movimientos previos.
									-- validar si la ubicacion ya existe con cantidad y el estatus de 
									if (select id from tblAlmacenado where productId = @productoX and barcode=@barcodeLocationDestino and estatus = 0) > 0
										BEGIN
											update tblAlmacenado set quantity = (quantity + @cantidadDestino), estatus =1, inventariado =1 where productId = @productoX and barcode=@barcodeLocationDestino and estatus = 1
										END
									else if (select id from tblAlmacenado where productId = @productoX and barcode=@barcodeLocationDestino and estatus = 1) > 0
										BEGIN
											update tblAlmacenado set quantity = (quantity + @cantidadDestino), estatus =1, inventariado =1 where productId = @productoX and barcode=@barcodeLocationDestino and estatus = 1
										END
									else  
										BEGIN
											insert into tblAlmacenado(orderId, productId,barcode,quantity,dateTime,estatus,inventariado)
											Values (0,@productoX,@barcodeLocationDestino,@cantidadDestino,GETDATE(),1,1)
										END

									if (select quantity from tblAlmacenado where id = @idX2) <= 0 
										BEGIN
											update tblAlmacenado set estatus = 2,inventariado=1 where id = @idX2

											set @comenta = 'Se queda en 0 la ubicacion '+ @barcodeLocationOrigen
											insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
											select 17, id, codigo, descripcion, ISNULL(@cantidadDestino,0)* -1, ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5 from tblMisProductos where id=@productoX
										END
										set @comenta = 'Se actualiza la ubicacion '+ @barcodeLocationDestino + ' con la cantidad de '+CONVERT(varchar(10), @cantidadDestino)
									-- poner comentario en cada mov registrado
									-- revisar el cambio de Estatus
									insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
									select 17, id, codigo, descripcion, ISNULL(@cantidadDestino,0), ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5 from tblMisProductos where id=@productoX
								select ''
								update tblReacomodo set estatus = 2 where id = @id
							END
						else if ( select quantity  from tblAlmacenado where id = @idX2 ) < @cantidadX
							BEGIN
								select 'La cantidad a reubicar excede la cantida de Origen'
							END
							SELECT @comenta AS ComentarioFinal;
    			END

		/*	Elimina Inventariado Ciclico	*/
		IF @cmd=16
			BEGIN
				--delete from tblInventariado_Ciclico where id=@idCiclico
				update tblInventariado_Ciclico set estatus = 3 where id=@idCiclico
			END

			/*	Elimina Reacomodo	*/
		IF @cmd=17
			BEGIN
				--delete from tblInventariado_Ciclico where id=@idCiclico
				update tblReacomodo set estatus = 3 where id=@idCiclico
			END

		if @cmd = 18
			BEGIN

				declare @idx int
				declare @productoidx2 int
				declare @CantFinalx2 int

				declare @ids varchar(500)
				set @ids=''
				set @ids=@cadena

				declare c cursor
				FOR
				--select id,idCarga,productId,barcodeLocation,isnull(quantityTeorico,0) + isnull(quantityDifference,0) as cantFinal,ISNULL(quantityDifference,0) as diferencia from tblInventariado_Ciclico where id = @seleccion
				select productId,SUM(isnull(quantity,0)) from tblAlmacenado a where a.id in(select Name from dbo.splitstring(@ids)) and isnull(estatus,0) = 0 and isnull(inventariado,0) = 1 group by a.productId
				open c
					fetch next from c into @productoidx2,@CantFinalx2
						while (@@FETCH_STATUS <> -1)
							begin
									update tblMisProductos set matriz = @CantFinalx2 where id = @productoidx2

					fetch next from c into @productoidx2,@CantFinalx2
							end
					close c
				deallocate c
			
			END

			if @cmd=19
				BEGIN
					update tblAlmacenado set estatus = 0, inventariado = 1,dateTime= GETDATE() where isnull(estatus,0) = 1 
					
				END

			if @cmd=20
			BEGIN
				--declare	@contPick int = 0

				declare @idPickx int
				declare @prodPickx int
				declare @quantutyPickx int
				declare @barcodePickx varchar(150)

					select top 1 @contPick= ISNULL(contador,0) from tblPicking where isnull(estatus,0) = 0 order by contador desc

					declare c cursor
					FOR
					select p.id,p.productId,p.quantity,p.barcodeLocation from tblPicking p  where isnull(p.estatus,0)=0 --contador = @contPick
					open c
						fetch next from c into @idPickx,@prodPickx,@quantutyPickx,@barcodePickx
							while (@@FETCH_STATUS =0)
								begin
										update tblAlmacenado set quantity=(quantity - @quantutyPickx) ,dateTime= GETDATE() where productId = @prodPickx and barcode = @barcodePickx and  isnull(estatus,0) = 0 and isnull(inventariado,0)=1

										if (select id from tblAlmacenado where productId = @prodPickx and barcode='MTYPATRANSITO1' and estatus = 5 and inventariado=1) > 0
											BEGIN
											    update tblAlmacenado set quantity = quantity + @quantutyPickx,dateTime= GETDATE() where productId = @prodPickx and barcode='MTYPATRANSITO1' and estatus = 5 and inventariado=1
												update tblPicking set estatus = 1 where id = @idPickx
											END
										ELSE
											BEGIN
												insert into tblAlmacenado(orderId,productId,barcode,quantity,dateTime,estatus,inventariado)
												Values (0,@prodPickx,'MTYPATRANSITO1',@quantutyPickx,GETDATE(),5,1)
												update tblPicking set estatus = 1 where id = @idPickx

											set @comenta = 'Se actualiza la ubicacion '+ @barcodePickx + ' se resta por Picking  '+CONVERT(varchar(10), @quantutyPickx)
											insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
											select 18, id, codigo, descripcion, ISNULL(@cantidadDestino,0), ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5 from tblMisProductos where id=@productoX
											END
							fetch next from c into @idPickx,@prodPickx,@quantutyPickx,@barcodePickx
							end	
						close c
					deallocate c
			END	
    set nocount off;
END

-- exec pInventario @cmd=20
--select * from tblTipoMovimientos
--Insert Into tblTipoMovimientos(id,nombre)
--Values(18,'Ajuste Sku a Transito')
--use erp_neogenis
--GO
-- select * from  tblInventariado_Ciclico
-- select * from tblReacomodo
-- select * from tblalmacenado

-- update tblReacomodo set identifierId = 981

-- alter table tblReacomodo add estatus int null
-- select * from tblalmacenado
--insert into tblReacomodo(dateTime,identifierId,companyId,userId,productOrigeId,barcodeLocationOrige,quantityOrige,barcodeLocationDestination,quantityDestination)
--Values(getdate(),14,3,0,33654,'MTYSOP04R02N04',10,'MTYSOP04R02N05',10)



---- select @barcodeLocation2x = barcodeLocationOrige from tblReacomodo where id = @identificador

--							-- sacamos cantidad total del producto en ubicaciones para comparar con la cantidad total a reubicar
--						--	update tblAlmacenado set estatus = 3,inventariado = 2 where productId = @productoid  and isnull(estatus,0) = 0 and isnull(inventariado,0) = 1
--						select id from tblAlmacenado where productId = @productoid and barcode = @barcodeLocationOrigen


--							if (select SUM(a.quantity) from tblAlmacenado a where productId=@productoid and estatus=2 and inventariado=1) = @cantidadOrigen
--								BEGIN
--								-- cantidad igual a reubicar es =
--								-- desactivamos las ubicaciones activas de ese producto
--										--update tblAlmacenado 
--										--	set
--										--	--  barcode = @barcodeLocation
--										--	-- ,quantity = quantity - @cantidad
--										--	--, dateTime = GETDATE()
--										--	 estatus = 1,
--										--	 inventariado = 4
--										--where productId = @productoid  and isnull(estatus,0) = 0 and isnull(inventariado,0) = 1

--									-- Insertamos en Alamacenado la resta de las ubicaciones 
--										insert into tblAlmacenado(orderId, productId,barcode,quantity,dateTime,estatus,inventariado)
--										Values (0,@productoid,@barcodeLocationDestino,@cantidadDestino,GETDATE(),1,1)

--										insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
--										select 17, id, codigo, descripcion, ISNULL(@cantidadDestino,0), ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5 from tblMisProductos where id=@productoid
--								END
--							--ELSE if  (select quantity from tblAlmacenado a where productId=@productoid and estatus=0 and inventariado=1 and barcode = @barcodeLocationOrigen) < @cantidadDestino
--							ELSE if  @cantidadOrigenUbi > @cantidadDestino
--								BEGIN
--									update tblAlmacenado 
--											set
--											-- barcode = @barcodeLocation
--											quantity = @cantidadOrigenUbi - @cantidadDestino,
--											--, dateTime = GETDATE()
--											 estatus = 1,
--											 inventariado = 1
--										where productId = @productoid  and isnull(estatus,0) = 0 and isnull(inventariado,0) = 1 and barcode= @barcodeLocationOrigen

--										insert into tblAlmacenado(orderId, productId,barcode,quantity,dateTime,estatus,inventariado)
--										Values (0,@productoid,@barcodeLocationDestino,@cantidadDestino,GETDATE(),1,1)

--										insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
--										select 17, id, codigo, descripcion, ISNULL(@cantidadDestino,0), ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5 from tblMisProductos where id=@productoid
--								END
--						ELSE if  @cantidadOrigenUbi <= @cantidadDestino
--								BEGIN
--									update tblAlmacenado 
--											set
--											-- barcode = @barcodeLocation
--											quantity = 0,
--											--, dateTime = GETDATE()
--											 estatus = 1,
--											 inventariado = 4
--										where productId = @productoid  and isnull(estatus,0) = 0 and isnull(inventariado,0) = 1 and barcode= @barcodeLocationOrigen

--										insert into tblAlmacenado(orderId, productId,barcode,quantity,dateTime,estatus,inventariado)
--										Values (0,@productoid,@barcodeLocationDestino,@cantidadDestino,GETDATE(),1,1)

--										insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid)
--										select 17, id, codigo, descripcion, ISNULL(@cantidadDestino,0), ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5 from tblMisProductos where id=@productoid
--								END


--					--insert into tblMovimientos (tipoid, productoid, codigo, descripcion, cantidad, costo_unitario, existencia, userid, comentario, almacenid, ordencompraid, ordencompraparcialid, ordencompraparcialconceptoid)
--					--select 17, id, codigo, descripcion, ISNULL(@cantidad,0), ISNULL(costo_estandar,0), matriz, 0, ISNULL(@comenta,''), 5, null, null, null from tblMisProductos where id=@productoid

--					update tblReacomodo set estatus = 1 where id = @id
GO


