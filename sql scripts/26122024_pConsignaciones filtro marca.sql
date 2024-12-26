USE [erp_neogenis_13122024]
GO

/****** Object:  StoredProcedure [dbo].[pConsignaciones]    Script Date: 26/12/2024 12:07:17 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[pConsignaciones]
	@cmd int,
	@consignacionid bigint=0,
	@clienteid bigint=0,
	@sucursalid bigint=0,
	@consignaciondetalleid bigint=0,
	@inventarioid bigint=0,
	@almacenid bigint=0,
	@origenid int=0,
	@destinoid int=0,
	@userid int=0,
	@productoid bigint=0,
	@proyectoid bigint=0,
	@cantidad float=0,
	@comentario text=null,
	@txtSearch varchar(max)='',
	@codigo varchar(100)='',
	@descripcion varchar(max)='',
	@precio money=0,
	@unidad varchar(50)='',
	@lote varchar(50)='',
	@caducidad varchar(50)='',
	@orden_compra varchar(50)=null,
	@cfdid bigint=0,
	@partidaid bigint=0,
	@estatusid int=0,
	@ordenCompra varchar(max)='',
	@pedidoid bigint=0
AS
BEGIN
	set nocount on;
	
	declare @cantidadGuardada float
	declare @productoIdx bigint
	declare @tmpExistencia float
	declare @tmpPrecioid int = 0
	
	/*	Agrega nuevo lote de consignacion	*/
	if @cmd=1
		begin
			insert into tblConsignacion ( fecha, almacenid, clienteid, sucursalid, userid, estatusid, comentario, orden_compra )
			values ( getdate(), @almacenid, @clienteid, @sucursalid, @userid, 1, @comentario, @orden_compra )
			select @@IDENTITY as consignacionid
		end
		
	/*	Regresa encabezados de consignacion	*/
	if @cmd=2
		begin			
			select
				a.id,
				a.id as lote,
				CONVERT(varchar(10), a.fecha, 103) as fecha,
				isnull(c.nombre,'') as almacen,
				isnull(d.razonsocial,'') as cliente,
				isnull(e.sucursal,'') as sucursal,
				isnull(b.nombre,'') as vendedor,
				convert(varchar,isnull(a.comentario,'')) as comentario,
				isnull(a.estatusid,0) as estatusid,
				--SUM(isnull(g.unitario2,0) * isnull(f.cantidad,0)) as subtotal,
				--SUM(isnull(g.unitario2,0) * isnull(f.cantidad,0)) * (isnull(d.descuento,0)/100) as descuento,
				dbo.fnImporteConsignacion(a.id) as subtotal,
				dbo.fnDescuentoConsignacion(a.id) as descuento,
				dbo.fnIvaConsignacion(a.id) as iva,
				dbo.fnImporteConsignacion(a.id) - dbo.fnDescuentoConsignacion(a.id) + dbo.fnIvaConsignacion(a.id) as total
			from
				tblConsignacion a
				left join tblUsuario b on a.userid=b.id
				inner join tblAlmacen c on c.id=a.almacenid
				inner join tblMisClientes d on d.id=a.clienteid
				left join tblSucursalCliente e on e.id=a.sucursalid
				left join tblConsignacionDetalle f on f.consignacionid=a.id
				left join tblMisProductos g on g.id=f.productoid
			where 
				a.id=@consignacionid
			group by
					a.id,a.fecha,c.nombre,d.razonsocial,e.sucursal,	b.nombre,convert(varchar,isnull(a.comentario,'')),a.estatusid,d.descuento
		end
		
	/*	Agrega item al lote de consignacion	*/
	if @cmd=3
		begin
			if ( select top 1 id from tblConsignacionDetalle where consignacionid=@consignacionid and productoid=@productoid ) is null
			begin
				/*	Agrega tblConsignacionDetalle  */
				insert into tblConsignacionDetalle ( consignacionid, productoid, cantidad, codigo, descripcion, fecha, existencia )
				select @consignacionid, id, @cantidad, isnull(codigo,''), descripcion, GETDATE(), @cantidad from tblMisProductos where id=@productoid
			end
			else
			begin
				/*	Actualiza existencia tblConsignacionDetalle  */
				update tblConsignacionDetalle set cantidad = isnull(cantidad,0) + @cantidad, existencia = isnull(existencia,0) + @cantidad where consignacionid=@consignacionid and productoid=@productoid
			end

			select @almacenid = isnull(almacenid,0) from tblConsignacion where id = @consignacionid

			/*	Descarga inventario	*/
			if @almacenid=1
			begin
				update tblMisProductos set monterrey=(isnull(monterrey,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(monterrey,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  14, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Salida por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			else if @almacenid=2
			begin
				update tblMisProductos set mexico=(isnull(mexico,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(mexico,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  14, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Salida por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end	
			else if @almacenid=3
			begin
				update tblMisProductos set guadalajara=(isnull(guadalajara,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(guadalajara,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  14, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Salida por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			else if @almacenid=4
			begin
				update tblMisProductos set mermas=(isnull(mermas,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(mermas,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  14, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Salida por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			else if @almacenid=5
			begin
				update tblMisProductos set matriz=(isnull(matriz,0) - isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(matriz,0) from tblMisProductos where id=@productoid
				update TOP (1) tblAlmacenado set quantity =isnull(quantity,0)+isnull(@cantidad,0), dateTime = GETDATE() where productId=@productoid AND quantity > 0 
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  14, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Salida por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			/*	Marca el lote como procesado	*/
	
			update tblConsignacion set estatusid=2 where id=@consignacionid
			

			--declare @tmpAlmacenOrigenid int
			--declare @tmpClienteid int
			--declare @tmpUserid int

			--select @tmpAlmacenOrigenid=isnull(almacenid,0), @tmpClienteid=isnull(clienteid,0), @tmpUserid=isnull(userid ,0) from tblConsignacion where id=@consignacionid
		
			--if ( select id from tblConsignacionCliente where clienteid=@tmpClienteid and consignacionid=@consignacionid and productoid=@productoid and inventarioid=@inventarioid ) is null
			--begin
			--	/*	Agrega inventario para consignacion de cliente */
			--	insert into tblConsignacionCliente ( consignacionid, inventarioid, clienteid, productoid, cantidad, caducidad, lote, fecha, userid, existencia )
			--	select @consignacionid, id, @tmpClienteid, productoid, @cantidad, caducidad, lote, GETDATE(), @tmpUserid, @cantidad from tblInventario where id=@inventarioid
			--end
			--else
			--begin
			--	declare c cursor
			--	for
			--	select
			--		isnull(inventarioid,0) as inventarioid, 
			--		isnull(productoid,0) as prodcutoid, 
			--		isnull(cantidad,0) as cantidad
			--	from 
			--		tblConsignacionCliente
			--	where
			--		clienteid=@tmpClienteid
			--		and consignacionid=@consignacionid
			--		and productoid=@productoid
			--		and inventarioid=@inventarioid
				
			--	OPEN c
			--	declare @inventarioIdx bigint
			--	declare @cantidadIdx float
			
			--	FETCH NEXT FROM c INTO @inventarioIdx, @productoIdx, @cantidadIdx
			--	WHILE (@@FETCH_STATUS = 0)
			--		BEGIN
			--				/*	Actualiza cantidad si ya existe registro en tblConsignacionCliente */
			--				update 
			--					tblConsignacionCliente
			--				set
			--					cantidad=isnull(b.cantidad,0)+@cantidad,
			--					existencia=isnull(b.cantidad,0)+@cantidad,
			--					fecha=GETDATE(),
			--					userid=@tmpUserid
			--				from 
			--					tblConsignacionCliente b 
			--					inner join tblInventario a on b.inventarioid = a.id
			--				where
			--					b.clienteid=@tmpClienteid
			--					and b.consignacionid=@consignacionid 
			--					and a.productoid=@productoIdx 
			--					and a.id=@inventarioIdx
			--		FETCH NEXT FROM c INTO @inventarioIdx, @productoIdx, @cantidadIdx
			--		END
			--	CLOSE c
			--	DEALLOCATE c
			--end

		end
		
	/*	Borra item del lote	*/
	if @cmd=4
		begin
			delete from tblConsignacionDetalle where id=@consignaciondetalleid
		end
		
	/*	Lista de lotes de consignacion	*/
	if @cmd=5
		begin
			declare @tmpSucursal table (id int,nombre varchar(200))
			insert into @tmpSucursal 
			select id, nombre from tblAlmacen 
			
			select
				a.id, 
				CONVERT(varchar(10), a.fecha, 103) as fecha,
				isnull(c.nombre,'') as almacen,
				isnull(e.razonsocial,'') as cliente, 
				isnull(d.nombre,'') as estatus, 
				isnull(sum(t.cantidad),0) as piezas,
				dbo.fnExistenciaFinalConsignacion(a.id) as existencia,
				a.estatusid,
				isnull(d.nombre,0) as estatus
			from 
				tblConsignacion a
				left join tblConsignacionDetalle t on a.id=t.consignacionid
				left join tblUsuario b on a.userid=b.id
				left join tblAlmacen c on a.almacenid=c.id
				inner join tblConsignacionEstatus d on a.estatusid=d.id
				inner join tblMisClientes e on e.id=a.clienteid
				inner join tblMisProductos f on f.id=t.productoid
				inner join tblProyecto g on g.id = f.proyectoid
			where
				isnull(inventarioid,0)=0 
				and ((@estatusid=0) or a.estatusid=@estatusid)
				and ((@proyectoid=0) or f.proyectoid=@proyectoid)
			group by
				a.id, a.fecha, c.nombre, e.razonsocial, d.nombre, a.estatusid
			order by 
				a.id desc
		end
		
	/*	Regresa detalle de un lote de consignacion	*/
	if @cmd=6
		begin
			set @tmpPrecioid=0
			select @clienteid=clienteid from tblConsignacion where id=@consignacionid
			select @tmpPrecioid=tipoprecioid from tblMisClientes where id=@clienteid
			
			select
				b.id as productoid, 
				isnull(a.codigo,'') as codigo, 
				isnull(a.descripcion,'') as descripcion,
				isnull(u.nombre,'') as unidad,
				isnull(sum(a.cantidad),0) as cantidad,
				isnull(sum(a.regresado),0) as regresado,
				isnull(sum(a.facturado),0) as facturado,
				isnull(sum(a.existencia),0) as disponible,
				--dbo.fnFacturadoConsignacionCliente(a.consignacionid,a.productoid) as facturado,
				--dbo.fnRegresadoConsignacionCliente(a.consignacionid,a.productoid) as regresado,
				--dbo.fnExistenciaConsignacionCliente(a.consignacionid,a.productoid) as disponible,
				case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as precio,
				dbo.fnImporteConceptoConsignacion(@tmpPrecioid,@consignacionid,a.productoid) as importe
			from 
				tblConsignacionDetalle a
				left join tblMisProductos b on b.id=a.productoid
				left join tblUnidad u on u.clave=b.claveunidad
			where 
				a.consignacionid=@consignacionid
				and isnull(inventarioid,0)=0
			group by
				b.id, 
				a.codigo, 
				a.descripcion, 
				u.nombre,
				a.consignacionid,
				a.productoid,
				b.perecederoBit,
				b.unitario,
				b.unitario2,
				b.unitario3,
				b.codigo
			order by 
				b.codigo
		end
		
	/*	Borra consignacion con todo y su detalle	*/
	if @cmd=7
		begin
			delete from tblConsignacionDetalle where consignacionid=@consignacionid
			delete from tblConsignacion where id=@consignacionid
		end
		
	/*	Procesa un lote de consignación  */
	if @cmd=8
		begin
			select 1
			--declare @tmpAlmacenOrigenid int
			--declare @tmpClienteid int
			--declare @tmpUserid int

			--select @tmpAlmacenOrigenid=isnull(almacenid,0), @tmpClienteid=isnull(clienteid,0), @tmpUserid=isnull(userid ,0) from tblConsignacion where id=@consignacionid
		
			--declare @tmpExistencia float
			--declare c cursor
			--for
			--select isnull(inventarioid,0) as inventarioid, isnull(productoid,0) as prodcutoid, isnull(cantidad,0) as cantidad
			--from tblConsignacionDetalle where consignacionid=@consignacionid
				
			--OPEN c
			--declare @inventarioIdx bigint
			--declare @productoIdx bigint
			--declare @cantidadIdx float
			
			--FETCH NEXT FROM c INTO @inventarioIdx, @productoIdx, @cantidadIdx
			--WHILE (@@FETCH_STATUS = 0)
			--	BEGIN
			--		/*	Descarga inventario	*/
			--		update 
			--			tblInventario 
			--		set 
			--			existencia=isnull(existencia,0)-@cantidadIdx 
			--		where 
			--			id=@inventarioIdx
					
			--		--set @almacenid=0
					
			--		--select @almacenid=isnull(almacenid,0) from tblInventario where id=@inventarioIdx
					
			--		--if @almacenid=1
			--		--	begin
			--		--		update tblMisProductos set monterrey=isnull(monterrey,0)-isnull(@cantidadIdx,0) where id=@productoIdx
			--		--	end
			--		--	if @almacenid=2
			--		--	begin
			--		--		update tblMisProductos set mexico=isnull(mexico,0)-isnull(@cantidadIdx,0) where id=@productoIdx
			--		--	end
			--		--	if @almacenid=3
			--		--	begin
			--		--		update tblMisProductos set guadalajara=isnull(guadalajara,0)-isnull(@cantidadIdx,0) where id=@productoIdx
			--		--	end
			--		--	if @almacenid=4
			--		--	begin
			--		--		update tblMisProductos set mermas=isnull(mermas,0)-isnull(@cantidadIdx,0) where id=@productoIdx
			--		--	end
			--		--	if @almacenid=5
			--		--	begin
			--		--		update tblMisProductos set matriz=isnull(matriz,0)-isnull(@cantidadIdx,0) where id=@productoIdx
			--		--	end
										
			--		if exists (select id from tblConsignacionCliente where consignacionid=@consignacionid and productoid=@productoIdx and inventarioid=@inventarioIdx)
			--		begin
			--			/*	Actualiza cantidad si ya existe registro */
			--			update 
			--				tblConsignacionCliente
			--			set 
			--				consignacionid=@consignacionid, 
			--				inventarioid=@inventarioIdx, 
			--				clienteid=@tmpClienteid,
			--				productoid=a.productoid, 
			--				cantidad=isnull(b.cantidad,0)+@cantidadIdx,
			--				caducidad=a.caducidad, 
			--				lote=a.lote, 
			--				fecha=GETDATE(), 
			--				userid=@tmpUserid,
			--				existencia=@cantidad
			--			from 
			--				tblConsignacionCliente b 
			--				inner join tblInventario a on b.inventarioid = a.id
			--			where 
			--				b.consignacionid=@consignacionid 
			--				and a.productoid=@productoIdx 
			--				and a.id=@inventarioIdx
			--		end
			--		else
			--		begin
			--			/*	Agrega inventario para consignacion de cliente */
			--			insert into tblConsignacionCliente ( consignacionid, inventarioid, clienteid, productoid, cantidad, caducidad, lote, fecha, userid, existencia )
			--			select @consignacionid, id, @tmpClienteid, productoid, @cantidadIdx, caducidad, lote, GETDATE(), @tmpUserid, @cantidadIdx from tblInventario where id=@inventarioIdx
			--		end
			--	-------------------------------------------------------------------------------------------------------------------
			--	FETCH NEXT FROM c INTO @inventarioIdx, @productoIdx, @cantidadIdx
			--	END
			--CLOSE c
			--DEALLOCATE c
			
			/*	Marca el lote como procesado	*/
			--update tblConsignacion set estatusid=2 where id=@consignacionid
			
		end
		
	/*	Regresa la sucursal de origen de una consignación	*/
	if @cmd=9
		begin
			select isnull(almacenid,0) from tblConsignacion where id=@consignacionid
		end
	
	/*	Regresa detalle de consignación para formato de embarque	*/
	if @cmd=10
		begin
			set @tmpPrecioid=0
			select @clienteid=clienteid from tblConsignacion where id=@consignacionid
			select @tmpPrecioid=tipoprecioid from tblMisClientes where id=@clienteid

			select
				a.id,
				isnull(a.codigo,'') as codigo,
				isnull(a.cantidad,0) as cantidad,
				isnull(a.descripcion,'') as descripcion,
				isnull(u.nombre,'') as unidad,
				isnull(sum(a.cantidad),0) as cantidad,
				isnull(sum(a.regresado),0) as regresado,
				isnull(sum(a.facturado),0) as facturado,
				isnull(sum(a.existencia),0) as disponible,
				--'$ ' + CONVERT(varchar, convert(money,isnull(b.unitario2,0))) as precio,
				--'$ ' + CONVERT(varchar, convert(money,isnull(b.unitario2,0) * isnull(a.cantidad,0))) as importe,
				case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as precio,
				case @tmpPrecioid when 1 then isnull(unitario,0) * isnull(a.cantidad,0) when 2 then isnull(unitario2,0) * isnull(a.cantidad,0) when 3 then isnull(unitario3,0) * isnull(a.cantidad,0) else isnull(unitario,0) * isnull(a.cantidad,0) end as importe,
				isnull(a.lote,'') as lote,
				case when isnull(b.perecederoBit,0) = 0 then '' else CONVERT(varchar(10), a.caducidad, 103) end as caducidad
			from 
				tblConsignacionDetalle a
				inner join tblMisProductos b on b.id=a.productoid
				left join tblUnidad u on u.clave=b.claveunidad
			where 
				a.consignacionid=@consignacionid
			group by
				a.id,
				a.codigo,
				a.cantidad,
				a.descripcion,
				a.lote,
				a.caducidad,
				b.unidad,
				b.unitario,
				b.unitario2,
				b.unitario3,
				b.perecederoBit,
				u.nombre
			order by 
				a.codigo
		end
	
	/*	Regresa el total de piezas de una consignación 	*/
	if @cmd=11
		begin
			select SUM(isnull(cantidad,0)) as total from tblConsignacionDetalle where consignacionid=@consignacionid
		end
	
	/*	Regresa resultado de búsqueda para agregar partidas a consignacion	*/
	if @cmd=12
		begin			
			
			select @almacenid = almacenid from tblConsignacion where id=@consignacionid
			
			if ( select top 1 id from tblMisProductos where codigo=@txtSearch ) is not null
				begin
					select
						a.id as productoid, 
						a.codigo, 
						isnull(a.descripcion,'') as descripcion,
						1 As cantidad, 
						isnull(u.nombre,'') As unidad,
						case
							@almacenid
								when 1 then isnull(monterrey,0)
								when 2 then isnull(mexico,0)
								when 3 then isnull(guadalajara,0)
								when 4 then isnull(mermas,0)
								when 5 then isnull(matriz,0)
						end as existencia,
						dbo.fnProductosDisponiblesAlmacen(a.id,@almacenid) as disponibles,
						isnull(a.inventariableBit,0) as inventariableBit
					from 
						tblMisProductos a
						left join tblUnidad u on u.clave=a.claveunidad
					where 
						a.codigo=@txtSearch
					order by 
						descripcion asc
				end
			else
				begin
					select
						a.id as productoid, 
						a.codigo, 
						isnull(a.descripcion,'') as descripcion, 
						1 As cantidad, 
						isnull(u.nombre,'') As unidad,
						case
							@almacenid
								when 1 then isnull(monterrey,0)
								when 2 then isnull(mexico,0)
								when 3 then isnull(guadalajara,0)
								when 4 then isnull(mermas,0)
								when 5 then isnull(matriz,0)
						end as existencia,
						dbo.fnProductosDisponiblesAlmacen(a.id,@almacenid) as disponibles,
						isnull(a.inventariableBit,0) as inventariableBit
					from
						tblMisProductos a
						left join tblUnidad u on u.clave=a.claveunidad
					where 
						(( a.codigo like '%' + @txtSearch + '%' ) or ( a.descripcion like '%' + @txtSearch + '%' ))
						and @txtSearch <> ''
					order by 
						a.codigo asc
				end
		end
	
	/*	Datos de una consignacion para facturación  */
	if @cmd=13
	begin
		select id, clienteid, sucursalid, isnull(orden_compra,'') as orden_compra from tblConsignacion
		where id=@consignacionid
	end
	
	/*	Agrega producto para facturar consignación  */
	if @cmd=14
		begin
			declare @ivaidx float
			declare @tipoimpuestoid int
			declare @estatus_consignacion int
			
			select @tipoimpuestoid=isnull(tasaid,0) from tblMisProductos where id=@productoid
	
			if @tipoimpuestoid=1
				begin
					set @ivaidx=0
				end
			else if @tipoimpuestoid=2
				begin
					set @ivaidx=0.11
				end
			else if @tipoimpuestoid=3
				begin
					set @ivaidx=0.16
				end
			else
				begin
					set @ivaidx=0.16
				end
			
			if exists(select id from tblConsignacionConceptos where consignacionid=@consignacionid and productoid=@productoid)
			begin				
				update tblConsignacionConceptos set codigo=@codigo, descripcion=@descripcion, cantidad=cantidad+@cantidad,precio=convert(money, @precio), importe=(cantidad+@cantidad)*convert(money, @precio), iva=@cantidad*convert(money, @precio)*@ivaidx,productoid=@productoid 
				where consignacionid=@consignacionid and  productoid=@productoid
			end
			else
			begin
				insert into tblConsignacionConceptos ( consignacionid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, lote, caducidad) 
				values (@consignacionid, @productoid, @codigo, @descripcion, @cantidad, @unidad, @precio, @cantidad*convert(money, @precio), @cantidad*convert(money, @precio)*@ivaidx, @lote, @caducidad)
			end
		end
		
	/*	Agrega cfd y partidas de una consignación	*/
	if @cmd=15
		begin
			select @sucursalid = isnull(sucursalid,0), @clienteid = isnull(clienteid,0), @orden_compra=isnull(orden_compra,''), @almacenid=isnull(almacenid,0) from tblConsignacion where id=@consignacionid
			insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, consignacionid )
			select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, 3, @sucursalid, @almacenid, @consignacionid from tblMisClientes where id=@clienteid
			
			select @cfdid=@@identity

			update tblConsignacion set cfdid=@cfdid where id=@consignacionid
		
			declare c cursor
			for
			select id from tblConsignacionConceptos where consignacionid=@consignacionid

			open c
			declare @consignacionconceptoIdx int		
			
			fetch next from c into @consignacionconceptoIdx
			while (@@FETCH_STATUS <> -1)
				begin
					 if (@@FETCH_STATUS <> -2)
						 begin
							declare @porcentaje_descuento float=0
							declare @importe_descuento float=0
							
							select @productoIdx = productoid from tblConsignacionConceptos where id=@consignacionconceptoIdx
							select @tipoimpuestoId=isnull(tasaid,0) from tblMisProductos where id=@productoIdx
							select @porcentaje_descuento=isnull(descuento,0) from tblMisClientes where id=@clienteid
							select @importe_descuento = ((@porcentaje_descuento * (cantidad * precio)) / 100) from tblConsignacionConceptos where id=@consignacionconceptoIdx
							
							if @tipoimpuestoid=1
								begin
									set @ivaidx=0
								end
							else if @tipoimpuestoid=2
								begin
									set @ivaidx=0.11
								end
							else if @tipoimpuestoid=3
								begin
									set @ivaidx=0.16
								end
							else
								begin
									set @ivaidx=0.16
								end
							
							insert into tblCFD_Partidas ( cfdid, inventarioid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento)
							select @cfdid, @inventarioid, productoid, codigo, descripcion, cantidad, unidad, convert(money, precio ), importe, (cantidad * convert(money, precio) * @ivaidx), @importe_descuento from tblConsignacionConceptos where id=@consignacionconceptoIdx
							
						 end
					fetch next from c into @consignacionconceptoIdx
				end
			close c
			deallocate c
		select @cfdid
	end
		
	/*	Datos de una consignacion para facturación  */
	if @cmd=16
	begin
		select id, clienteid, sucursalid, isnull(cfdid,0) as cfdid, isnull(orden_compra,'') as orden_compra from tblConsignacion
		where cfdid=@cfdid
	end
	
	/*	Regresa el número de partidas pendientes de asignar para descontar de inventario de un producto para facturación  */
	if @cmd=17
	begin
		select count(isnull(almacen_detallado_bit,0)) as total from tblCFD_Partidas where isnull(almacen_detallado_bit,0)=0 and cfdid=@cfdid
	end
	
	/*	Regresa los registros de inventario de un producto para facturación	 */
	if @cmd=18
	begin
		select 
			c.id, 
			b.id as productoid, 
			b.descripcion,
			u.nombre as unidad,
			d.nombre as almacen,
			isnull(a.lote,'') as lote,
			case when isnull(b.perecederoBit,0) = 0 then '' else CONVERT(varchar(10), a.caducidad, 103) end as caducidad, 
			isnull(a.existencia,0) as existencia
		from 
			tblConsignacionCliente a
			inner join tblMisProductos b on b.id=a.productoid
			inner join tblInventario c on c.id=a.inventarioid
			inner join tblAlmacen d on d.id=c.almacenid
			left join tblUnidad u on u.clave=b.claveunidad
		where 
			a.existencia>0 and a.productoid=@productoid and a.consignacionid=@consignacionid
		order by
			id desc
	end
	
	/*	Asigna cantidades de cada partida a inventario de un producto para facturación  */
	if @cmd=19
	begin
		declare @t1 float=0
		declare @t2 float=0
		if (select top 1 id from tblCFDPartidasAlmacen where inventarioid=@inventarioid and cfdid=@cfdid) is null
		begin
			insert into tblcfdPartidasAlmacen (cfdid,partidaid,productoid,cantidad,inventarioid) values (@cfdid,@partidaid,@productoid,@cantidad,@inventarioid)
		end
		else
		begin
			update tblcfdPartidasAlmacen set cantidad = @cantidad where inventarioid=@inventarioid
		end
		select @t1 = SUM(cantidad) from tblcfdPartidasAlmacen where partidaid=@partidaid
		select @t2 = isnull(cantidad,0) from tblCFD_Partidas where id=@partidaid
		if @t1=@t2
		begin
			update tblCFD_Partidas set almacen_detallado_bit=1, inventarioid=@inventarioid where id=@partidaid
		end
		else
		begin
			update tblCFD_Partidas set almacen_detallado_bit=0, inventarioid=@inventarioid where id=@partidaid
		end
	end
	
	/*	Elimina conceptos de consignación que no fueron facturados  */
	if @cmd=20
		begin
			select @estatus_consignacion = isnull(estatusid,0) from tblConsignacion where id=@consignacionid
			if @estatus_consignacion=2
			begin
				delete from tblConsignacionConceptos where isnull(facturadoBit,0)=0
			end
		end
	
	/*	Actualiza estatus de consignación  */
	if @cmd=21
		begin
			declare @total_existencia float=0
			select @total_existencia = isnull(sum(isnull(existencia,0)),0) from tblConsignacionDetalle where consignacionid=@consignacionid and isnull(inventarioid,0)=0
			
			if @total_existencia=0
			begin
				update tblConsignacion set estatusid=3 where id=@consignacionid
			end
		end

	/*	Agrega en consignacion desde pedidos  */
	if @cmd=22
		begin
	  -- Obtener datos del pedido
    SELECT 
        @almacenId = almacenId,
        @clienteId = clienteId,
        @sucursalId = sucursalId,
        @comentario = 'Pedido  ' + CAST(@pedidoid AS NVARCHAR),
        @ordenCompra = orden_compra
    FROM tblPedidos
    WHERE id = @pedidoid;

    -- Insertar en tblConsignacion
    INSERT INTO tblConsignacion (fecha, almacenid, clienteid, sucursalid, userid, estatusid, comentario, orden_compra)
    VALUES (GETDATE(), @almacenId, @clienteId, @sucursalId, @userid, 1, @comentario, @ordenCompra);

    -- Retornar el ID insertado
    SELECT SCOPE_IDENTITY() AS consignacionid
		end
	/* Regreso consignacion a pedido desde consigna */
	if @cmd=23
	begin
		UPDATE tblConsignacion SET estatusid = 3, comentario = CONCAT(comentario, CAST(' CERRADO REGRESO ' AS TEXT)) WHERE id = @consignacionid 
	end

	if @cmd=24 
		/*	Regresa item del lote de consignacion	*/
		begin
			/*	Actualiza existencia tblConsignacionDetalle  */
			update tblConsignacionDetalle 
			set 
				cantidad = isnull(cantidad,0) - @cantidad, 
				existencia = isnull(existencia,0) - @cantidad,
				regresado = isnull(regresado,0) + @cantidad
			where consignacionid=@consignacionid and productoid=@productoid

			select @almacenid = isnull(almacenid,0) from tblConsignacion where id = @consignacionid

			/*	Descarga inventario	*/
			if @almacenid=1
			begin
				update tblMisProductos set monterrey=(isnull(monterrey,0) + isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(monterrey,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  6, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Entrada por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			else if @almacenid=2
			begin
				update tblMisProductos set mexico=(isnull(mexico,0) + isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(mexico,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  6, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Entrada por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end	
			else if @almacenid=3
			begin
				update tblMisProductos set guadalajara=(isnull(guadalajara,0) + isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(guadalajara,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  6, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Entrada por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			else if @almacenid=4
			begin
				update tblMisProductos set mermas=(isnull(mermas,0) + isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(mermas,0) from tblMisProductos where id=@productoid
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  6, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Entrada por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			else if @almacenid=5
			begin
				update tblMisProductos set matriz=(isnull(matriz,0) + isnull(@cantidad,0)) where id=@productoid
				select @tmpExistencia = isnull(matriz,0) from tblMisProductos where id=@productoid
				update TOP (1) tblAlmacenado set quantity =isnull(quantity,0)-isnull(@cantidad,0), dateTime = GETDATE() where productId=@productoid AND quantity > 0 
				/*	Agrega registro a la tabla de movimientos	*/
				insert into tblMovimientos ( tipoid, productoid, codigo, descripcion, cantidad, existencia, userid, comentario, almacenid )
				select  6, @productoid, codigo, descripcion, @cantidad, isnull(@tmpExistencia,0), @userid, 'Entrada por consignación ' + convert(varchar(10),@consignacionid), @almacenid  from tblMisProductos where id=@productoid
			end
			/*	Marca el lote como procesado	*/
			--Se cierra desde cmd 23
			--update tblConsignacion set estatusid=2 where id=@consignacionid


		end
	
	set nocount off;
END

GO


