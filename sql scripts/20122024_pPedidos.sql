USE [erp_neogenis_13122024]
GO

/****** Object:  StoredProcedure [dbo].[pPedidos]    Script Date: 20/12/2024 10:34:28 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[pPedidos]
	@cmd int,
	@pedidoid bigint=0,
	@productoid bigint=0,
	@partidaid bigint=0,
	@inventarioid bigint=0,
	@userid	bigint=0,
	@clienteid bigint=0,
	@sucursalid bigint=0,
	@rfc varchar(50)=null,
	@estatusid int=0,
	@guia varchar(50)=null,
	@tasaid int=0,
	@orden_compra varchar(50)=null,
	@cfdid bigint=0,
	@cantidad float=0,
	@proyectoid bigint=0,
	@almacenid bigint=0,
	@marcaid int=0,
	@txtSearch varchar(500)='',
	@pagoid varchar(100) = '',	
	@ordenCompraMex varchar(100) = null,
	@ordenCompraUsa varchar(100) = null,
	@proveedorid bigint =0,
	@ordencompraid bigint = 0,
	@fhaini varchar(10)=null,
	@fhafin varchar(10)=null,
	@idCarga int =0,
	@MensajeError varchar(Max)='',
	@correcto int = 0,
	@error int =0,
	@idPago varchar(100)='',
	@origen int=0,
	@sku varchar(50) = '',
	@precio decimal = 0,
	@fecha datetime = '',
	@marca  varchar(50) = '',
	@ola_id int=0,
	@ola_status int=0,
	@estatusola int= 0

AS	
BEGIN
	SET NOCOUNT ON;
	
	declare @tmpPrecioid int = 0
	declare @tmpDivisaid int
	declare @tmpTipoCambio money
	declare @tSerie varchar(20)
	declare @tFolio bigint
	declare @tTipoid int
	declare @tmpTasaid int
	declare @tmpPerfilid int
	
	/*	Agrega pedido y regresa id	*/
	if @cmd=1
		begin
			insert into tblPedidos ( userid, clienteid, estatusid, sucursalid, tasaid, orden_compra, proyectoid, almacenid,fecha_alta) values (@userid, @clienteid, @estatusid, @sucursalid, @tasaid, @orden_compra, @proyectoid, @almacenid,getdate())
			select @@identity as pedidoid
		end
	
	/*	Listado de pedidos	*/
	if @cmd=2
		begin
			set @tmpPerfilid = 0
			select @tmpPerfilid=perfilid from tblUsuario where id=@userid
			
			if @tmpPerfilid=1		--	Usuario administrador
				begin
				WITH ProductInfo AS ( -- consulta para bodegas de los conceptos de pedidos 
				SELECT DISTINCT
					pc.pedidoid,
					mp.id AS productId
				FROM
					tblPedidosConceptos pc
				INNER JOIN
					tblPedidos p on pc.pedidoid=p.id
				INNER JOIN
					tblMisProductos mp ON pc.codigo = mp.codigo
				WHERE
					mp.matriz > 0 and p.estatusid not in ( 4,7,9,10,11 )
			),
			LocationInfo AS (
				SELECT DISTINCT
					pi.pedidoid,
					sl.floorName
				FROM
					ProductInfo pi
				JOIN
					tblAlmacenado al ON pi.productId = al.productId
				JOIN
					tblStockLocation sl ON al.barcode = sl.barcodelocation
			)
					select 
						a.id, 
						isnull(c.razonsocial,'') as cliente, 
						a.fecha_alta, 
						a.estatusid, 
						isnull(d.nombre,'') as ejecutivo,
						b.nombre as estatus, 
						isnull(a.guia,'') as guia, 
						isnull(e.timbrado,0) as timbrado, 
						isnull(e.serie,'') + convert(varchar(10),isnull(e.folio,'')) as factura, 
						isnull(a.orden_compra,'') as orden_compra,
						isnull(s.sucursal,'') as sucursal,
						isnull(p.nombre,'') as proyecto,
						isnull(c.condicionesid,0) as condicionesid,
						isnull(a.pagoid, '') as pagoid,
						chkcfdid = 0,
						isnull(c.chkFacAutomaticas,0) as chkFacAutomaticas,
						--ISNULL(CASE WHEN a.estatusid IN (1, 2) THEN 1 ELSE 0 END, 0) AS chkcons,
						chkcons = 0,
						isnull(a.olaid, 0) as olaid,
						isnull(so.descripcion, ' ') as ola_estatus,
					    isnull(a.ola_estatus, 0) as olaestatusid,
						ISNULL(STRING_AGG(li.floorName, ', '), '') AS bodega,
						ISNULL(CASE WHEN a.estatusid >= 7 THEN 0 ELSE dbo.fnSumEmpaquetadoPedido(a.id) END, 0) AS totalEmpaquetado
					from 
						tblPedidos a 
						join tblPedidoEstatus b on a.estatusid = b.id
						join tblMisClientes c on a.clienteid=c.id
						left join tblUsuario d on a.userid=d.id
						left join tblCFD e on e.id=a.cfdid
						left join tblProyecto p on p.id=a.proyectoid
						left join tblSucursalCliente s on s.clienteid=a.clienteid and s.id=a.sucursalid and isnull(s.borradobit,0)=0
						left join LocationInfo li ON a.id = li.pedidoid
						left join tblStatusola so on so.ID = a.ola_estatus
						--left join tblPedidosConceptos pc on pc.pedidoid=a.id
					where
						(@clienteid=0 or a.clienteid=@clienteid)
						and (@estatusid=0 or a.estatusid=@estatusid)
						and (@txtSearch='' or (a.id in (select pedidoid from tblPedidosConceptos where (codigo like '%' + @txtSearch + '%' or descripcion like '%' + @txtSearch + '%' or pedidoid like '%' + @txtSearch + '%') and isnull(borradobit,0) = 0)))
						and (@marcaid=0 or a.proyectoid=@marcaid)
						and (@estatusola=0 or a.ola_estatus=@estatusola)
						and a.estatusid <> 11
						--and (@txtSearch='' or ( pc.codigo like '%' + @txtSearch + '%' ) or ( pc.descripcion like '%' + @txtSearch + '%' ))
					group by
						a.id, c.razonsocial, a.fecha_alta, a.estatusid, d.nombre, b.nombre, a.guia, e.timbrado, e.serie, e.folio, a.orden_compra, s.sucursal, p.nombre, c.condicionesid, a.pagoid,c.chkFacAutomaticas, a.olaid, a.ola_estatus, so.descripcion
					order by 
						a.id desc
				end
			else if @tmpPerfilid=3		--	Ejecutivo de ventas
				begin
				WITH ProductInfo AS (
				SELECT DISTINCT
					pc.pedidoid,
					mp.id AS productId
				FROM
					tblPedidosConceptos pc
				INNER JOIN
					tblPedidos p on pc.pedidoid=p.id
				INNER JOIN
					tblMisProductos mp ON pc.codigo = mp.codigo
				WHERE
					mp.matriz > 0 and p.estatusid not in ( 4,7,9,10,11 )
			),
			LocationInfo AS (
				SELECT DISTINCT
					pi.pedidoid,
					sl.floorName
				FROM
					ProductInfo pi
				JOIN
					tblAlmacenado al ON pi.productId = al.productId
				JOIN
					tblStockLocation sl ON al.barcode = sl.barcodelocation
			)
					select 
						a.id, 
						isnull(c.razonsocial,'') as cliente, 
						a.fecha_alta, 
						a.estatusid, 
						isnull(d.nombre,'') as ejecutivo,
						b.nombre as estatus, 
						isnull(a.guia,'') as guia, 
						isnull(e.timbrado,0) as timbrado, 
						isnull(e.serie,'') + convert(varchar(10),isnull(e.folio,'')) as factura, 
						isnull(a.orden_compra,'') as orden_compra,
						isnull(s.sucursal,'') as sucursal,
						isnull(p.nombre,'') as proyecto,
						isnull(c.condicionesid,0) as condicionesid,
						isnull(a.pagoid, '') as pagoid,
						chkcfdid = 0,
						isnull(c.chkFacAutomaticas,0) as chkFacAutomaticas,
						chkcons = 0,
						isnull(a.olaid, 0) as olaid,
						isnull(so.descripcion, ' ') as ola_estatus,
					    isnull(a.ola_estatus, 0) as olaestatusid,
					    ISNULL(STRING_AGG(li.floorName, ', '), '') AS bodega,
						ISNULL(CASE WHEN a.estatusid >= 7 THEN 0 ELSE dbo.fnSumEmpaquetadoPedido(a.id) END, 0) AS totalEmpaquetado
					from 
						tblPedidos a 
						join tblPedidoEstatus b on a.estatusid = b.id 
						join tblMisClientes c on a.clienteid=c.id
						left join tblUsuario d on a.userid=d.id
						left join tblCFD e on e.id=a.cfdid
						left join tblProyecto p on p.id=a.proyectoid
						left join tblSucursalCliente s on s.clienteid=a.clienteid and s.id=a.sucursalid and isnull(s.borradobit,0)=0
						left join LocationInfo li ON a.id = li.pedidoid
						left join tblStatusola so on so.ID = a.ola_estatus
						--left join tblPedidosConceptos pc on pc.pedidoid=a.id
					where 
						a.userid=@userid
						and (@clienteid=0 or a.clienteid=@clienteid)
						and (@estatusid=0 or a.estatusid=@estatusid)
						--and (@txtSearch='' or ( pc.codigo like '%' + @txtSearch + '%' ) or ( pc.descripcion like '%' + @txtSearch + '%' ))
						and (@txtSearch='' or (a.id in (select pedidoid from tblPedidosConceptos where (codigo like '%' + @txtSearch + '%' or descripcion like '%' + @txtSearch + '%' or pedidoid like '%' + @txtSearch + '%') and isnull(borradobit,0) = 0)))
						and (@marcaid=0 or a.proyectoid=@marcaid)
						and (@estatusola=0 or a.ola_estatus=@estatusola)
						and a.estatusid <> 11
					group by
						a.id, c.razonsocial, a.fecha_alta, a.estatusid, d.nombre, b.nombre, a.guia, e.timbrado, e.serie, e.folio, a.orden_compra, s.sucursal, p.nombre, c.condicionesid, a.pagoid,c.chkFacAutomaticas, a.olaid, a.ola_estatus, so.descripcion
					order by 
						a.id desc
				end				
			else if @tmpPerfilid=4		--	Almacen
				begin
				WITH ProductInfo AS (
				SELECT DISTINCT
					pc.pedidoid,
					mp.id AS productId
				FROM
					tblPedidosConceptos pc
				INNER JOIN
					tblPedidos p on pc.pedidoid=p.id
				INNER JOIN
					tblMisProductos mp ON pc.codigo = mp.codigo
				WHERE
					mp.matriz > 0 and p.estatusid not in ( 4,7,9,10,11 )
			),
			LocationInfo AS (
				SELECT DISTINCT
					pi.pedidoid,
					sl.floorName
				FROM
					ProductInfo pi
				JOIN
					tblAlmacenado al ON pi.productId = al.productId
				JOIN
					tblStockLocation sl ON al.barcode = sl.barcodelocation
			)
					select 
						a.id, 
						isnull(c.razonsocial,'') as cliente, 
						a.fecha_alta, 
						a.estatusid, 
						isnull(d.nombre,'') as ejecutivo,
						b.nombre as estatus, 
						isnull(a.guia,'') as guia, 
						isnull(e.timbrado,0) as timbrado, 
						isnull(e.serie,'') + convert(varchar(10),isnull(e.folio,'')) as factura, 
						isnull(a.orden_compra,'') as orden_compra,
						isnull(s.sucursal,'') as sucursal,
						isnull(p.nombre,'') as proyecto,
						isnull(c.condicionesid,0) as condicionesid,
						isnull(a.pagoid, '') as pagoid,
						chkcfdid = 0,
						isnull(c.chkFacAutomaticas,0) as chkFacAutomaticas,
						chkcons = 0,
						isnull(a.olaid,0) as olaid,
						isnull(so.descripcion, ' ') as ola_estatus,
					    isnull(a.ola_estatus, 0) as olaestatusid,
					    ISNULL(STRING_AGG(li.floorName, ', '), '') AS bodega,
						ISNULL(CASE WHEN a.estatusid >= 7 THEN 0 ELSE dbo.fnSumEmpaquetadoPedido(a.id) END, 0) AS totalEmpaquetado
					from 
						tblPedidos a
						join tblPedidoEstatus b on a.estatusid = b.id
						join tblMisClientes c on a.clienteid=c.id
						left join tblUsuario d on a.userid=d.id
						left join tblCFD e on e.id=a.cfdid
						left join tblProyecto p on p.id=a.proyectoid
						left join tblSucursalCliente s on s.clienteid=a.clienteid and s.id=a.sucursalid and isnull(s.borradobit,0)=0
						left join LocationInfo li ON a.id = li.pedidoid
						left join tblStatusola so on so.id = a.ola_estatus
						--left join tblPedidosConceptos pc on pc.pedidoid=a.id
					where 
						--a.estatusid=5 and
						(@clienteid=0 or a.clienteid=@clienteid)
						and (@estatusid=0 or a.estatusid=@estatusid)
						--and (@txtSearch='' or ( pc.codigo like '%' + @txtSearch + '%' ) or ( pc.descripcion like '%' + @txtSearch + '%' ))
						and (@txtSearch='' or (a.id in (select pedidoid from tblPedidosConceptos where (codigo like '%' + @txtSearch + '%' or descripcion like '%' + @txtSearch + '%' or pedidoid like '%' + @txtSearch + '%') and isnull(borradobit,0) = 0)))
						and (@marcaid=0 or a.proyectoid=@marcaid)
						and (@estatusola=0 or a.ola_estatus=@estatusola)
						and a.estatusid <> 11
					group by
						a.id, c.razonsocial, a.fecha_alta, a.estatusid, d.nombre, b.nombre, a.guia, e.timbrado, e.serie, e.folio, a.orden_compra, s.sucursal, p.nombre, c.condicionesid, a.pagoid,c.chkFacAutomaticas, a.olaid, a.ola_estatus, so.descripcion
					order by 
						a.id desc
				end
			else
				begin
				WITH ProductInfo AS (
				SELECT DISTINCT
					pc.pedidoid,
					mp.id AS productId
				FROM
					tblPedidosConceptos pc
				INNER JOIN
					tblPedidos p on pc.pedidoid=p.id
				INNER JOIN
					tblMisProductos mp ON pc.codigo = mp.codigo
				WHERE
					mp.matriz > 0 and p.estatusid not in ( 4,7,9,10,11 )
			),
			LocationInfo AS (
				SELECT DISTINCT
					pi.pedidoid,
					sl.floorName
				FROM
					ProductInfo pi
				JOIN
					tblAlmacenado al ON pi.productId = al.productId
				JOIN
					tblStockLocation sl ON al.barcode = sl.barcodelocation
			)
					select 
						a.id, 
						isnull(c.razonsocial,'') as cliente, 
						a.fecha_alta, 
						a.estatusid, 
						isnull(d.nombre,'') as ejecutivo,
						b.nombre as estatus, 
						isnull(a.guia,'') as guia, 
						isnull(e.timbrado,0) as timbrado, 
						isnull(e.serie,'') + convert(varchar(10),isnull(e.folio,'')) as factura, 
						isnull(a.orden_compra,'') as orden_compra,
						isnull(s.sucursal,'') as sucursal,
						isnull(p.nombre,'') as proyecto,
						isnull(c.condicionesid,0) as condicionesid,
						isnull(a.pagoid, '') as pagoid,
						chkcfdid = 0,
						isnull(c.chkFacAutomaticas,0) as chkFacAutomaticas,
						chkcons = 0,
						isnull(a.olaid,0) as olaid,
						isnull(so.descripcion, ' ') as ola_estatus,
					    isnull(a.ola_estatus, 0) as olaestatusid,
					    ISNULL(STRING_AGG(li.floorName, ', '), '') AS bodega,
						ISNULL(CASE WHEN a.estatusid >= 7 THEN 0 ELSE dbo.fnSumEmpaquetadoPedido(a.id) END, 0) AS totalEmpaquetado
					from 
						tblPedidos a 
						join tblPedidoEstatus b on a.estatusid = b.id 
						join tblMisClientes c on a.clienteid=c.id
						left join tblUsuario d on a.userid=d.id
						left join tblCFD e on e.id=a.cfdid
						left join tblProyecto p on p.id=a.proyectoid
						left join tblSucursalCliente s on s.clienteid=a.clienteid and s.id=a.sucursalid and isnull(s.borradobit,0)=0
						left join LocationInfo li ON a.id = li.pedidoid
						left join tblStatusola so on so.id = a.ola_estatus
						--left join tblPedidosConceptos pc on pc.pedidoid=a.id
					where
						(@clienteid=0 or a.clienteid=@clienteid)
						and (@estatusid=0 or a.estatusid=@estatusid)
						and (@txtSearch='' or (a.id in (select pedidoid from tblPedidosConceptos where (codigo like '%' + @txtSearch + '%' or descripcion like '%' + @txtSearch + '%' or pedidoid like '%' + @txtSearch + '%') and isnull(borradobit,0) = 0)))
						and (@marcaid=0 or a.proyectoid=@marcaid)
						and (@estatusola=0 or a.ola_estatus=@estatusola)
						and a.estatusid <> 11
						--and (@txtSearch='' or ( pc.codigo like '%' + @txtSearch + '%' ) or ( pc.descripcion like '%' + @txtSearch + '%' ))
					group by
						a.id, c.razonsocial, a.fecha_alta, a.estatusid, d.nombre, b.nombre, a.guia, e.timbrado, e.serie, e.folio, a.orden_compra, s.sucursal, p.nombre, c.condicionesid, a.pagoid,c.chkFacAutomaticas, a.olaid, a.ola_estatus, so.descripcion
					order by 
						a.id desc
				end
		end
		-- exec pPedidos @cmd = 2
	/* Cancela pedido */	
	if @cmd=3
	begin
		update tblPedidos set estatusid = 4 where id = @pedidoid
		delete from tblPedidosConceptos where pedidoid=@pedidoid
	end
	
	/* Elimina pedido */	
	if @cmd=4
	begin

		--delete from tblPedidos where id = @pedidoid
		update tblPedidos set estatusid = 11 where id = @pedidoid
		--delete from tblPedidosConceptos where pedidoid=@pedidoid 
	end
		
/* Edita Pedido */	
	if @cmd=5
		begin
			select 
				a.fecha_alta, 
				a.estatusid, 
				isnull(a.almacenid,0) as almacenid,
				isnull(a.clienteid,0) as clienteid, 
				isnull(a.guia,'') as guia, 
				b.nombre as estatus, 
				isnull(a.clienteid,0) as clienteid, 
				c.razonsocial as cliente,
				isnull(d.sucursal,'') as sucursal,
				isnull(e.nombre,'') as almacen,
				isnull(f.nombre,'') as proyecto,
				isnull(a.id,'') as idpedido,
				isnull(a.olaid,'') as ola,
				isnull(a.orden_compra,'') as orden_compra

			from 
				tblPedidos a
				join tblPedidoEstatus b on a.estatusid = b.id 
				join tblMisClientes c on a.clienteid = c.id
				left join tblSucursalCliente d on d.clienteid=a.clienteid and d.id=a.sucursalid
				left join tblAlmacen e on e.id= a.almacenid
				left join tblProyecto f on f.id=a.proyectoid
			where 
				a.id = @pedidoid
		end
		
		
	/* Coloca pedido */	
	if @cmd=6
	begin
		update tblPedidos set estatusid = 2 where id = @pedidoid
	end
		
	/*	Listado de TODOS los pedidos */
	if @cmd=7
		begin
			select 
				a.id, 
				a.rfc,
				a.fecha_alta, 
				a.estatusid, 
				c.razonsocial as cliente,
				isnull(a.orden_compra,'') as orden_compra,
				b.nombre as estatus
			from 
				tblPedidos a 
				join tblPedidoEstatus b on a.estatusid = b.id 
				join tblMisClientes c on a.clienteid = c.id
			order by
				a.id desc
		end
			
	/*	Datos del cliente del pedido */
	if @cmd=8
		begin
			select c.razonsocial 
			from tblPedidos a  
			join tblMisClientes c
			on a.clienteid = c.id
			where a.id=@pedidoid
		end
		
	/*	Autoriza pedido	*/	
	if @cmd=9
		begin
			update tblPedidos set estatusid=5 where id=@pedidoid 
		end
		
	/*	Marca un pedido	como empaquetado */
	if @cmd=10
		begin
			update tblPedidos set estatusid=6 where id=@pedidoid
		end
		
	/*	Marca un pedido	como enviado */
	if @cmd=11
		begin
			update tblPedidos set estatusid=8, guia=@guia where id=@pedidoid 
		end
		
	if @cmd=12
		begin
			if (select isnull(perfilid,0) from tblUsuario where id=@userid)=3
				begin
					select distinct a.id, isnull(razonsocial,'') as razonsocial, isnull(contacto,'') as contacto, isnull(telefono_contacto,'') as telefono_contacto, isnull(fac_rfc,'') as rfc
					from tblMisClientes a inner join tblSucursalCliente b on b.clienteId=a.id and b.vendedorId=@userid
					where isnull(a.estatusid,0)=1
					order by razonsocial
				end
			else
				begin
					select id, isnull(razonsocial,'') as razonsocial, isnull(contacto,'') as contacto, isnull(telefono_contacto,'') as telefono_contacto, isnull(fac_rfc,'') as rfc
					from tblMisClientes
					where isnull(estatusid,0)=1
					order by razonsocial
				end			
		end
	
	/*	Detalle de un pedido */	
	if @cmd=13
		begin		
			select a.id, 
				isnull(b.tallaMX,'') as tallamx,
				--isnull(b.upc,'') as upc,
				isnull(cantidad,0) as cantidad,
				case when dbo.fnCodigoCliente(c.clienteid,b.id) is null then 
				b.upc else dbo.fnCodigoCliente(c.clienteid,b.id) end as upc,
				isnull(dbo.fnCodigoCliente(c.clienteid,b.id),'') as codAlterno,
				isnull(a.codigo,'') as codigo, 
				isnull(a.descripcion,'') as descripcion,
				substring(isnull(a.descripcion,''),1,35) as texto,
				isnull(a.unidad,'') as unidad, 
				isnull(a.lote,'') as lote, 
				isnull(a.caducidad,'') as caducidad
			from tblPedidosConceptos a 
			inner join tblMisProductos b on a.productoid = b.id
			left join tblPedidos c on a.pedidoid = c.id
				where pedidoid=@pedidoid
				and isnull(borradobit,0) = 0
				order by a.id
			--order by a.codigo, a.descripcion 
		end
	
	/*	Detalle de un pedido para formato impreso*/
	if @cmd=14
	begin
		select 
			a.id, 
			isnull(e.nombre,'') as almacen,
			isnull(f.nombre,'') as proyecto,
			isnull(d.nombre,'') as vendedor,
			isnull(CONVERT(varchar(10),fecha_alta,103),'') as fecha, 
			isnull(b.razonsocial,'') as cliente,
			isnull(c.sucursal,'') as sucursal,
			isnull(a.orden_compra, '') as orden_compra
		from
			tblPedidos a
			left join tblMisClientes b on b.id=a.clienteid
			left join tblSucursalCliente c on c.id=a.sucursalid
			left join tblUsuario d on d.id=a.userid
			left join tblAlmacen e on e.id=a.almacenid
			left join tblProyecto f on f.id=a.proyectoid
		where 
			a.id=@pedidoid
	end
	
	/*	Total de piezas de un pedido*/
	if @cmd=15
	begin
		select sum(a.cantidad)
		from tblPedidosConceptos a
		inner join tblMisProductos p on p.id=a.productoid
		left join tblUnidad u on u.clave=p.claveunidad
		where a.pedidoid= @pedidoid
		and isnull(borradobit,0) = 0
	end
	
	/*	Datos de un pedido para facturación*/
	if @cmd=16
	begin
		select 
			id, 
			clienteid, 
			sucursalid, 
			ISNULL(almacenid,0) as almacenid,
			ISNULL(cfdid,0) as cfdid, 
			ISNULL(tasaid,0) as tasaid, 
			ISNULL(orden_compra,'') as orden_compra
		from 
			tblPedidos
		where 
			id=@pedidoid
	end
	
	/*	Agrega cfd y partidas de un pedido	*/
	--if @cmd=17
	--begin
	--	insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, tipodocumentoid )
	--	select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, @tasaid, @sucursalid, @almacenid, 1 from tblMisClientes where id=@clienteid
		
	--	select @cfdid=@@identity
	--	update tblPedidos set cfdid=@cfdid
	--	where id=@pedidoid
		
	--	declare c cursor
	--	for
	--	select id from tblPedidosConceptos where pedidoid=@pedidoid and isnull(borradobit,0) = 0

	--	open c
	--	declare @pedidoconceptoIdx int
		
	--	fetch next from c into @pedidoconceptoIdx
	--	while (@@FETCH_STATUS <> -1)
	--		begin
	--			 if (@@FETCH_STATUS <> -2)
	--				 begin
	--					declare @productoIdx int=0
	--					declare @tipoimpuestoId int = 0
	--					declare @ivaIdx float = 0
	--					declare @porcentaje_descuento float=0
	--					declare @importe_descuento float=0
	--					declare @subtotal money=0
	--					declare @tallaProduct int =0
						
	--					select @productoIdx = productoid from tblPedidosConceptos where id=@pedidoconceptoIdx and isnull(borradobit,0) = 0
	--					select @tipoimpuestoId=isnull(tasaid,0) from tblMisProductos where id=@productoIdx

	--					select @porcentaje_descuento=isnull(descuento,0) from tblMisClientes where id=@clienteid

	--					select @importe_descuento = (@porcentaje_descuento / 100) * (cantidad * precio)  from tblPedidosConceptos where id=@pedidoconceptoIdx and isnull(borradobit,0) = 0
						
	--					if @tipoimpuestoid=1
	--						begin
	--							set @ivaidx=0
	--						end
	--					else if @tipoimpuestoid=2
	--						begin
	--							set @ivaidx=0.11
	--						end
	--					else if @tipoimpuestoid=3
	--						begin
	--							set @ivaidx=0.16
	--						end
	--					else
	--						begin
	--							set @ivaidx=0.16
	--						end
						

	--					insert into tblCFD_Partidas ( cfdid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento, claveprodserv, claveunidad,talla, piezasporcaja,cajas,numero_identificacion,fraccion_arancelaria,cantidad_aduana,unidad_aduana,unitario_aduana,valor_dolares)
	--					select 
	--						@cfdid, 
	--						c.productoid, 
	--						case
	--							@clienteid
	--								when 158 then isnull(p.upc,'')
	--								when 182 then isnull(p.upc,'')
	--								when 80296 then isnull(p.upc,'')
	--								else isnull(c.codigo,'') end,
	--						c.descripcion, 
	--						c.cantidad, 
	--						u.nombre, 
	--						convert(money, c.precio ), 
	--						c.importe, 
	--						(c.importe - @importe_descuento) *  @ivaidx, 
	--						convert(decimal (18,6), @importe_descuento),
	--						p.claveprodserv, 
	--						p.claveunidad,
	--						isnull(isnull(TRY_PARSE(p.tallaMX as float),0)*10,0),
	--						10,
	--						1,
	--						case
	--							@clienteid
	--								when 158 then isnull(p.upc,'')
	--								when 182 then isnull(p.upc,'')
	--								when 80296 then isnull(p.upc,'')
	--								else isnull(c.codigo,'') end
	--						,'64041117'
	--						, c.cantidad
	--						,'09'
	--						,convert(money, c.precio )
	--						,c.importe
	--					from 
	--						tblPedidosConceptos c
	--						inner join tblMisProductos p on p.id=c.productoid
	--						inner join tblUnidad u on u.clave=p.claveunidad
	--					where 
	--						c.id=@pedidoconceptoIdx and isnull(borradobit,0) = 0
	--				 end
	--			fetch next from c into @pedidoconceptoIdx
	--		end
	--	close c
	--	deallocate c
		
	--	select @cfdid as cfdid
	--end
/*	Agrega cfd y partidas de un pedido	*/
	if @cmd=17
begin
                declare @productoIdx int = 0
                declare @tipoimpuestoId int = 0
                declare @ivaIdx float = 0
                declare @porcentaje_descuento float = 0
                declare @importe_descuento float = 0
                declare @subtotal money = 0
                declare @tallaProduct int = 0
    -- Verificar si ya existe un cfdid asociado con el pedido
    select @cfdid = ISNULL(cfdid, 0) from tblPedidos where id = @pedidoid
    
    if @cfdid > 0
    begin
        -- Eliminar los conceptos existentes en tblCFD_Partidas relacionados con el cfdid
        delete from tblCFD_Partidas where cfdid = @cfdid
        
        -- Agregar los nuevos conceptos de tblPedidosConceptos a tblCFD_Partidas
        declare @pedidoconceptoIdx int
        
        declare c cursor
        for
        select id from tblPedidosConceptos where pedidoid = @pedidoid and isnull(borradobit, 0) = 0
        
        open c
        
        fetch next from c into @pedidoconceptoIdx
        while (@@FETCH_STATUS <> -1)
        begin
            if (@@FETCH_STATUS <> -2)
            begin
               
                
                select @productoIdx = productoid from tblPedidosConceptos where id = @pedidoconceptoIdx and isnull(borradobit, 0) = 0
                select @tipoimpuestoId = isnull(tasaid, 0) from tblMisProductos where id = @productoIdx
                
                select @porcentaje_descuento = isnull(descuento, 0) from tblMisClientes where id = @clienteid
                
                select @importe_descuento = (@porcentaje_descuento / 100) * (cantidad * precio) from tblPedidosConceptos where id = @pedidoconceptoIdx and isnull(borradobit, 0) = 0
                
                if @tipoimpuestoid = 1
                begin
                    set @ivaidx = 0
                end
                else if @tipoimpuestoid = 2
                begin
                    set @ivaidx = 0.11
                end
                else if @tipoimpuestoid = 3
                begin
                    set @ivaidx = 0.16
                end
                else
                begin
                    set @ivaidx = 0.16
                end
                
                insert into tblCFD_Partidas ( cfdid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento, claveprodserv, claveunidad, talla, piezasporcaja, cajas, numero_identificacion, fraccion_arancelaria, cantidad_aduana, unidad_aduana, unitario_aduana, valor_dolares)
                select 
                    @cfdid, 
                    c.productoid, 
                    case
                        when @clienteid in (158, 182, 80296) then isnull(p.upc, '')
                        else isnull(c.codigo, '') 
                    end,
                    c.descripcion, 
                    c.cantidad, 
                    u.nombre, 
                    convert(money, c.precio), 
                    c.importe, 
                    (c.importe - @importe_descuento) * @ivaidx, 
                    convert(decimal(18, 6), @importe_descuento),
                    p.claveprodserv, 
                    p.claveunidad,
                    isnull(isnull(TRY_PARSE(p.tallaMX as float), 0) * 10, 0),
                    10,
                    1,
                    case
                        when @clienteid in (158, 182, 80296) then isnull(p.upc, '')
                        else isnull(c.codigo, '') 
                    end,
                    '64041117',
                    c.cantidad,
                    '09',
                    convert(money, c.precio),
                    c.importe
                from 
                    tblPedidosConceptos c
                    inner join tblMisProductos p on p.id = c.productoid
                    inner join tblUnidad u on u.clave = p.claveunidad
                where 
                    c.id = @pedidoconceptoIdx and isnull(borradobit, 0) = 0
            end
            fetch next from c into @pedidoconceptoIdx
        end
        close c
        deallocate c
        
        select @cfdid as cfdid
    end
    else
    begin
        -- Insertar un nuevo registro en tblCFD si no existe un cfdid previo
        insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, tipodocumentoid )
        select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, @tasaid, @sucursalid, @almacenid, 1 
        from tblMisClientes 
        where id = @clienteid
        
        select @cfdid = @@identity
        
        update tblPedidos set cfdid = @cfdid where id = @pedidoid
        
        -- Agregar los conceptos de tblPedidosConceptos a tblCFD_Partidas
        declare c cursor
        for
        select id from tblPedidosConceptos where pedidoid = @pedidoid and isnull(borradobit, 0) = 0
        
        open c
        
        fetch next from c into @pedidoconceptoIdx
        while (@@FETCH_STATUS <> -1)
        begin
            if (@@FETCH_STATUS <> -2)
            begin
                
                select @productoIdx = productoid from tblPedidosConceptos where id = @pedidoconceptoIdx and isnull(borradobit, 0) = 0
                select @tipoimpuestoId = isnull(tasaid, 0) from tblMisProductos where id = @productoIdx
                
                select @porcentaje_descuento = isnull(descuento, 0) from tblMisClientes where id = @clienteid
                
                select @importe_descuento = (@porcentaje_descuento / 100) * (cantidad * precio) from tblPedidosConceptos where id = @pedidoconceptoIdx and isnull(borradobit, 0) = 0
                
                if @tipoimpuestoid = 1
                begin
                    set @ivaidx = 0
                end
                else if @tipoimpuestoid = 2
                begin
                    set @ivaidx = 0.11
                end
                else if @tipoimpuestoid = 3
                begin
                    set @ivaidx = 0.16
                end
                else
                begin
                    set @ivaidx = 0.16
                end
                
                insert into tblCFD_Partidas ( cfdid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento, claveprodserv, claveunidad, talla, piezasporcaja, cajas, numero_identificacion, fraccion_arancelaria, cantidad_aduana, unidad_aduana, unitario_aduana, valor_dolares)
                select 
                    @cfdid, 
                    c.productoid, 
                    case
                        when @clienteid in (158, 182, 80296) then isnull(p.upc, '')
                        else isnull(c.codigo, '') 
                    end,
                    c.descripcion, 
                    c.cantidad, 
                    u.nombre, 
                    convert(money, c.precio), 
                    c.importe, 
                    (c.importe - @importe_descuento) * @ivaidx, 
                    convert(decimal(18, 6), @importe_descuento),
                    p.claveprodserv, 
                    p.claveunidad,
                    isnull(isnull(TRY_PARSE(p.tallaMX as float), 0) * 10, 0),
                    10,
                    1,
                    case
                        when @clienteid in (158, 182, 80296) then isnull(p.upc, '')
                        else isnull(c.codigo, '') 
                    end,
                    '64041117',
                    c.cantidad,
                    '09',
                    convert(money, c.precio),
                    c.importe
                from 
                    tblPedidosConceptos c
                    inner join tblMisProductos p on p.id = c.productoid
                    inner join tblUnidad u on u.clave = p.claveunidad
                where 
                    c.id = @pedidoconceptoIdx and isnull(borradobit, 0) = 0
            end
            fetch next from c into @pedidoconceptoIdx
        end
        close c
        deallocate c
        
        select @cfdid as cfdid
    end
end
	
	/*	Datos de un pedido para facturación*/
	if @cmd=18
	begin
		select id, clienteid, sucursalid, ISNULL(cfdid,0) as cfdid, isnull(tasaid,0) as tasaid, isnull(orden_compra,'') as orden_compra 
		from tblPedidos
		where cfdid=@cfdid
	end
	
	/*	Regresa los registros de inventario de un producto para facturación*/
	if @cmd=19
	begin
		select 
			a.id, 
			b.id as productoid, 
			b.descripcion,
			b.unidad,
			c.nombre as almacen,
			case when isnull(b.perecederoBit,0) = 0 then CONVERT(varchar(10), a.id, 103) else isnull(a.lote,'--') end as lote,
			case when isnull(b.perecederoBit,0) = 0 then CONVERT(varchar(10), a.fecha, 103) else CONVERT(varchar(10), a.caducidad, 103) end as caducidad,
			isnull(a.existencia,0) as existencia
		from
			tblInventario a
			inner join tblMisProductos b on b.id=a.productoid
			inner join tblAlmacen c on c.id=a.almacenid
		where 
			a.existencia>0 and a.productoid = @productoid 
		order by 
			b.codigo asc, a.fecha asc
		end
	
	/*	Asigna cantidades de cada partida a inventario de un producto para facturación*/
	if @cmd=20
	begin
		declare @t1 float=0
		declare @t2 float=0
		if (select top 1 id from tblcfdPartidasAlmacen where inventarioid=@inventarioid and cfdid=@cfdid) is null
		begin
			insert into tblcfdPartidasAlmacen (cfdid,partidaid,productoid,cantidad,inventarioid) values (@cfdid,@partidaid,@productoid,@cantidad,@inventarioid)
		end
		else
		begin
			update tblcfdPartidasAlmacen set cantidad = @cantidad where inventarioid=@inventarioid and cfdid=@cfdid
		end
		select @t1 = isnull(SUM(cantidad),0) from tblcfdPartidasAlmacen where partidaid=@partidaid
		select @t2 = isnull(cantidad,0) from tblCFD_Partidas where id=@partidaid
		if isnull(@t1,0)=isnull(@t2,0)
		begin
			update tblCFD_Partidas set almacen_detallado_bit=1, inventarioid=@inventarioid where id=@partidaid
		end
		else
		begin
			update tblCFD_Partidas set almacen_detallado_bit=0, inventarioid=@inventarioid where id=@partidaid
		end
	end
	
	/*	Regresa el número de partidas pendientes de asignar para descontar de inventario de un producto para facturación*/
	if @cmd=21
	begin
		select count(isnull(almacen_detallado_bit,0)) as total from tblCFD_Partidas where isnull(almacen_detallado_bit,0)=0 and cfdid=@cfdid
	end

	/*	Regresa resultado de búsqueda para agregar partidas a pedido	*/
	if @cmd=22
		begin
			select @tmpPrecioid=tipoprecioid from tblMisClientes where id=@clienteid

			if ( select top 1 id from tblMisProductos where codigo=@txtSearch ) is not null
				begin
					select
						a.id as productoid, 
						--a.codigo, 
						case
							@clienteid
								when 158 then isnull(a.upc,'')
								when 182 then isnull(a.upc,'')
								when 80296 then isnull(a.upc,'')
								when 80305 then isnull(a.upc,'')
								else isnull(a.codigo,'') 
						end as codigo,
						--isnull(a.descripcion,'') as descripcion,
						case
							@clienteid
								when 158 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								when 182 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								when 80296 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								when 80305 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								else isnull(a.descripcion,'')
						end as descripcion,
						isnull(a.unidad,'') As unidad,
						1 As cantidad, 
						case 
							@tmpPrecioid 
								when 1 then isnull(unitario,0)
								when 2 then isnull(unitario2,0)
								when 3 then isnull(unitario3,0)
								when 4 then isnull(unitario4,0)
								else isnull(unitario,0) 
						end as unitario,
						case
							@almacenid
								when 1 then isnull(monterrey,0)
								when 2 then isnull(mexico,0)
								when 3 then isnull(guadalajara,0)
								when 4 then isnull(mermas,0)
								when 5 then isnull(matriz,0)
						end as existencia,
						dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a
					where 
						a.codigo=@txtSearch
						and isnull(dbo.fnProductosDisponiblesAlmacen(a.id,@almacenid),0)>0
					order by 
						descripcion asc
				end
			else
				begin
					select
						a.id as productoid, 
						--a.codigo,
						case
							@clienteid
								when 158 then isnull(a.upc,'')
								when 182 then isnull(a.upc,'')
								when 80296 then isnull(a.upc,'')
								when 80305 then isnull(a.upc,'')
								else isnull(a.codigo,'')
						end as codigo,
						--isnull(a.descripcion,'') as descripcion,
						case
							@clienteid
								when 158 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								when 182 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								when 80296 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								when 80305 then isnull(a.codigo,'') + ' - ' + isnull(a.descripcion,'')
								else isnull(a.descripcion,'')
						end as descripcion,
						isnull(a.unidad,'') As unidad,
						1 As cantidad,
						case 
							@tmpPrecioid 
								when 1 then isnull(unitario,0)
								when 2 then isnull(unitario2,0)
								when 3 then isnull(unitario3,0)
								when 4 then isnull(unitario4,0)
								else isnull(unitario,0) 
						end as unitario,
						case
							@almacenid
								when 1 then isnull(monterrey,0)
								when 2 then isnull(mexico,0)
								when 3 then isnull(guadalajara,0)
								when 4 then isnull(mermas,0)
								when 5 then isnull(matriz,0)
						end as existencia,
						dbo.fnProductosDisponiblesAlmacen(a.id,@almacenid) as disponibles
					from
						tblMisProductos a
					where 
						(( a.codigo like '%' + @txtSearch + '%' ) or ( a.descripcion like '%' + @txtSearch + '%' ) or ( a.upc like '%' + @txtSearch + '%' ))
						and @txtSearch <> ''
						and isnull(dbo.fnProductosDisponiblesAlmacen(a.id,@almacenid),0)>0
					order by 
						a.codigo asc
				end
		end
	
	/*	Actualiza estatus de pedido a facturado	*/
	if @cmd=23
	begin
		declare @tmpAlmacenId as Integer=0
		select @pedidoid=id, @tmpAlmacenId=isnull(almacenid,0) from tblPedidos where cfdid=@cfdid
		update tblCFD set pedidoid=@pedidoid where id=@cfdid 
		update tblPedidos set estatusid=7 where cfdid=@cfdid
		if @tmpAlmacenId>0
		begin
			update tblCFD set almacenid=@tmpAlmacenId where id=@cfdid
		end
	end

	/*	Rechazar pedido	*/
	if @cmd=24
	begin
		update tblPedidos set estatusid=10 where id=@pedidoid
	end

	/*	Reactivar pedido	*/
	if @cmd=25
	begin
		update tblPedidos set estatusid=1 where id=@pedidoid
	end

	/*	Reactivar pedido autorizado	*/
	if @cmd=26
	begin
		select @estatusid=isnull(estatusid,0) from tblPedidos where id=@pedidoid
		insert into tblLogCambioEstatusPedido(pedidoid, userid, estatusid, fecha)
		values (@pedidoid, @userid, @estatusid, getdate())
		update tblPedidos set estatusid=1 where id=@pedidoid
	end
	--Actualiza el pago id
	if @cmd=27
	begin
	update tblPedidos set pagoid = @pagoid where id = @pedidoid 
	end
	--actualiza la guia de un pedido
	if @cmd=28
	begin
	update tblPedidos set guia = @guia where id = @pedidoid 
	end
		--devuelve la info de los productos que pertenecen a un pedido
if @cmd = 29
begin
    select
        convert(varchar, isnull(b.fecha_alta,0),103) as fecha,
        upper(FORMAT(isnull(b.fecha_alta,0), 'MMM', 'es-es')) as mes,
        isnull(c.nombre_comercial,'') as cliente,
        isnull(d.nombre,'') as marca,
        isnull(b.orden_compra,0) as nopedido,
        isnull(a.descripcion,'') as modelo,
        isnull(a.codigo, '') as sku,
        isnull(mp.upc, '') as upc,
        isnull(a.cantidad,0) as totalpiezas,
        isnull(dbo.fnPedidosUbicacion(a.productoid),0) as ubicacion,
        isnull(c.razonsocial,'') as comprador,
        isnull(b.cliente_final,'') as clientefinal,
        isnull(b.ciudad_clientefinal,'') as clientefinalciudad,
        isnull(b.guia,'') as guia,
        isnull('','') as comentarios,
        isnull('','') as fullshopify,
        case 
            when c.nombre_comercial like '%paypal%' then 'PayPal'
            when c.nombre_comercial like '%conekta%' then 'Conekta'
            else '' 
        end as metodopago, 
        concat(isnull(e.serie,''), isnull(e.folio,'') ) as factura,
        -- Mostrar el importe desde tblCFD_Partidas si existe cfdid y hay coincidencia en tblCFD_Partidas; de lo contrario, mostrar el importe desde tblPedidosConceptos
        case 
            when b.cfdid > 0 and f.importe is not null then f.importe
            else a.importe
        end as montototal,
        isnull(b.pagoid, '') as idpago
    from tblPedidosConceptos a 
    left join tblPedidos b on b.id = a.pedidoid 
    left join tblMisClientes c on b.clienteid = c.id
    left join tblProyecto d on b.proyectoid = d.id 
    left join tblCFD e on isnull(b.cfdid,0) = e.id 
    left join tblCFD_Partidas f on f.cfdid = b.cfdid and f.codigo = a.codigo and f.cantidad = a.cantidad
	left join tblMisProductos mp on a.codigo = mp.codigo
    where b.id = @pedidoid 
    and isnull(borradobit,0) = 0
end



	/*	Datos de un pedido para facturación*/
	if @cmd=30
	begin
		DECLARE @PractitionerId int = 0, @numCajas int = 0, @pesokg float =0, @productoid_concepto int =0
		DECLARE @nocaja varchar(max),@conceptoid int, @numcantidad float = 0
		DECLARE @tblEtiquetas table (razonsocial varchar(100), 
									 proveedor varchar(100),
									 sucursal varchar(100),
									 notienda int, oc varchar(100),
									 factura varchar(100),
									 carton varchar(100),
									 peso float,
									 piezas int,
									 codigobarras varchar(max))
		-------------------------------------------------------	
		declare @tblConceptos table(nocaja varchar(max),conceptoid int, cantidad float, peso float )
		/* FOREACH START*/	
			DECLARE MY_CURSOR CURSOR
			  LOCAL STATIC READ_ONLY FORWARD_ONLY
			FOR 
			SELECT id FROM tblPedidosConceptos where pedidoid = @pedidoid and isnull(borradobit,0) = 0
			OPEN MY_CURSOR
			FETCH NEXT FROM MY_CURSOR INTO @PractitionerId
			WHILE @@FETCH_STATUS = 0
			BEGIN 
				select @nocaja=isnull(dbo.fnNoCajaConceptoPedido(id),''),@conceptoid=id,@numcantidad=isnull(cantidad,0),@productoid_concepto =productoid from tblPedidosConceptos where id = @PractitionerId and isnull(borradobit,0) = 0
				select @numCajas= ISNULL(count(id),0) from tblPedidoConceptoCajas where conceptoid = @conceptoid and isnull(borradorbit,0) = 0
				select @pesokg = isnull(pesoKg,0) from tblMisProductos where id = @productoid_concepto

				if @numCajas = 0
				begin
					insert into @tblConceptos(nocaja,conceptoid,cantidad,peso)
					values(@nocaja,@conceptoid,@numcantidad, @pesokg)
				end 
				else
				begin
					insert into @tblConceptos(nocaja,conceptoid,cantidad,peso)
					select nocaja, conceptoid, numCantdad, @pesokg from tblPedidoConceptoCajas where conceptoid = @conceptoid and isnull(borradorbit,0) = 0
				end				
				----------------------------------------------	
				FETCH NEXT FROM MY_CURSOR INTO @PractitionerId
			END
			CLOSE MY_CURSOR
			DEALLOCATE MY_CURSOR
		/* FOREACH END*/

		
		-------------------------------------------------------
		declare @tblCajas table(nocaja varchar(max),indexcaja int)
		insert into @tblCajas(nocaja,indexcaja)
		select distinct(nocaja), ROW_NUMBER() over (order by nocaja) from @tblConceptos group by nocaja
		-------------------------------------------------------
		Declare
		@razonsocial varchar(100) = '',  
		@proveedor varchar(100) = '1000098', 
		@sucursal varchar(100) = '',
		@notienda int =0, 
		@oc varchar(100) = '',
		@factura varchar(100) = '',
		@carton varchar(100) = '', 
		@peso float =0,									 
		@piezas int =0,									
	    @codigobarras varchar(max) = '',
		--@clienteid int = 0,
		--@sucursalid int = 0,
		--@cfdid int = 0,
		@crtonindex int =1,
		@totalcartones int = 0

		select @clienteid = clienteid, @sucursalid=sucursalid,@cfdid=cfdid, @notienda= isnull(noTienda,''), @oc=orden_compra from tblPedidos where id =@pedidoid
		select @razonsocial = isnull(razonsocial,'') from tblMisClientes where id = @clienteid
		select @sucursal = isnull(sucursal,'') from tblSucursalCliente where id =@sucursalid
		SELECT @totalcartones= isnull(count(DISTINCT(noCaja)),0) FROM @tblCajas
		select @factura = concat(isnull(serie,''),isnull(folio,'')) from tblCFD where id = @cfdid
		
		/* FOREACH START*/
		--DECLARE @PractitionerId int

			DECLARE MY_CURSOR CURSOR 
			  LOCAL STATIC READ_ONLY FORWARD_ONLY
			FOR 
			SELECT indexcaja
			FROM @tblCajas 
			OPEN MY_CURSOR
			FETCH NEXT FROM MY_CURSOR INTO @PractitionerId
			WHILE @@FETCH_STATUS = 0
			BEGIN 

				select @codigobarras = nocaja from @tblCajas where indexcaja = @PractitionerId

				set @carton = concat(@crtonindex,' de ',@totalcartones)
				select @piezas = isnull(sum(cantidad),0) from @tblConceptos where noCaja = @codigobarras
				select @peso = isnull(sum(peso),0) from @tblConceptos where noCaja = @codigobarras				
				insert into @tblEtiquetas(razonsocial,proveedor,sucursal,notienda,oc,factura,carton,peso,piezas,codigobarras)
				values(@razonsocial,@proveedor,@sucursal,@notienda,@oc,@factura,@carton,@peso,@piezas,@codigobarras)

				set @crtonindex += 1
				----------------------------	
				FETCH NEXT FROM MY_CURSOR INTO @PractitionerId
			END
			CLOSE MY_CURSOR
			DEALLOCATE MY_CURSOR
		/* FOREACH END*/
		select * from @tblEtiquetas
		--select * from @tblConceptos
	end

	-- EXEC pPedidos @cmd=30, @pedidoid=234006
	/* PROVEEDORES*/
	if @cmd=31
	begin
	if @proveedorid <> 0
		select a.id, a.contacto nombre,(a.fac_calle + ' ' + a.fac_num_int + ' ' + a.fac_num_ext + ' ' + a.fac_colonia + ' ' + a.fac_municipio  +' '+ b.nombre) direccion ,
			a.email_contacto correo, a.razonsocial  from tblMisProveedores a
			inner join tblEstado b on a.fac_estadoid = b.id
			where a.id = @proveedorid;
	else
		select a.id, a.contacto nombre,(a.fac_calle + ' ' + a.fac_num_int + ' ' + a.fac_num_ext + ' ' + a.fac_colonia + ' ' + a.fac_municipio  +' '+ b.nombre) direccion ,
			a.email_contacto correo, a.razonsocial  from tblMisProveedores a
			inner join tblEstado b on a.fac_estadoid = b.id
	end
	/* PRODUCTOS*/
	if @cmd=32
	begin
	if @productoid <> 0
		select a.id, a.codigo,descripcion nombre, descripcion, unidad, color, num_disponibles, a.codigo barcode, 
			brand.id as 'brand.brandId', brand.nombre as 'brand.brand',
			coleccion.id as 'coleccion.collectionId', coleccion.nombre as 'coleccion.collectionName',
			stockLocator.CodigoBarrasLocacion as 'stockLocator.barcodeLocation', stockLocator.Cantidad as 'stockLocator.stockQuantity'
			from tblMisProductos a
			left join tblProyecto brand on brand.id = a.proyectoid
			left join tblColeccion coleccion on coleccion.id = a.coleccionid
			left join tblStockLocator stockLocator on stockLocator.productoid = a.id
			where a.id = @productoid and isnull(bajaBit,0)=0;
	Else
	     select a.id, a.codigo,descripcion nombre, descripcion, unidad, color, num_disponibles, a.codigo barcode, 
			brand.id as 'brand.brandId', brand.nombre as 'brand.brand',
			coleccion.id as 'coleccion.collectionId', coleccion.nombre as 'coleccion.collectionName',
			stockLocator.CodigoBarrasLocacion as 'stockLocator.barcodeLocation', stockLocator.Cantidad as 'stockLocator.stockQuantity'
			from tblMisProductos a
			left join tblProyecto brand on brand.id = a.proyectoid
			left join tblColeccion coleccion on coleccion.id = a.coleccionid
			left join tblStockLocator stockLocator on stockLocator.productoid = a.id
			where isnull(bajaBit,0)=0;
	end
	/* ORDEN DE COMPRA*/
	if @cmd=33
	BEGIN
		If @ordencompraid <> 0
			select a.id, fecha orderDate , fecha receivedDate, proveedorid,
			orderDetail.ordenId as [ordenId] ,orderDetail.cantidad as [cantidad], orderDetail.productoId as [productoId]
			from tblOrdenCompra a
			left join tblOrdenCompraConceptos orderDetail on orderDetail.ordenId = a.id
			where a.id = @ordencompraid;
		Else
			select a.id, fecha orderDate , fecha receivedDate, proveedorid,
			orderDetail.ordenId as [ordenId] ,orderDetail.cantidad as [cantidad], orderDetail.productoId as [productoId]
			from tblOrdenCompra a
			left join tblOrdenCompraConceptos orderDetail on orderDetail.ordenId = a.id;
	END
	/* StockLocation*/
	if @cmd=34
	BEGIN
		if @productoid <> 0
			select a.CodigoBarrasLocacion as [barcodeLocation] , a.warehoseId, a.warehoseName, a.rackId, a.rackName, a.hallwayId, a.hallwayName,a.floorId,
			a.floorName,a.levelId, a.levelName  from tblStockLocator a
			where productoid = @productoid and isnull(bajaBit,0)=0;
		Else
			select a.CodigoBarrasLocacion as [barcodeLocation] , a.warehoseId, a.warehoseName, a.rackId, a.rackName, a.hallwayId, a.hallwayName,a.floorId,
			a.floorName,a.levelId, a.levelName  from tblStockLocator a
			where isnull(bajaBit,0)=0;
	END

	    	/*	Listado de pedidos	*/
	if @cmd=35
		begin
				
				begin
					select 
						a.id, 
						isnull(c.razonsocial,'') as cliente, 
						convert(varchar(15),a.fecha_alta,103)as fecha_alta,
						isnull (a.estatusid,0) as estatusid, 
						isnull(d.nombre,'') as ejecutivo,
						isnull(b.nombre,'') as estatus, 
						isnull(a.guia,'') as guia, 
						isnull(e.timbrado,0) as timbrado, 
						isnull(e.serie,'') + convert(varchar(10),isnull(e.folio,'')) as factura, 
						isnull(a.orden_compra,'') as orden_compra,
						isnull(s.sucursal,'') as sucursal,
						isnull(p.nombre,'') as proyecto,
						isnull(c.condicionesid,0) as condicionesid,
						isnull(a.pagoid, '') as pagoid,
						0 as chkcfdid
					from 
						tblPedidos a 
						join tblPedidoEstatus b on a.estatusid = b.id
						join tblMisClientes c on a.clienteid=c.id
						left join tblUsuario d on a.userid=d.id
						left join tblCFD e on e.id=a.cfdid
						left join tblProyecto p on p.id=a.proyectoid
						left join tblSucursalCliente s on s.clienteid=a.clienteid and s.id=a.sucursalid and isnull(s.borradobit,0)=0
						--left join tblPedidosConceptos pc on pc.pedidoid=a.id
					where
						(@clienteid=0 or a.clienteid=@clienteid)
						and (a.estatusid in (1,2))
						--and (@txtSearch='' or ( a.id in (select pedidoid from tblPedidosConceptos where ( codigo like '%' + @txtSearch + '%' or descripcion like '%' + @txtSearch + '%' ) and isnull(borradobit,0) = 0) ) )
						--and (@txtSearch='' or ( pc.codigo like '%' + @txtSearch + '%' ) or ( pc.descripcion like '%' + @txtSearch + '%' ))
						--and fecha_alta between @fhaini and @fhafin
						--and fecha_alta between @fhaini + ' 00:00:00' and @fhafin + ' 23:59:59'
					group by
						a.id, c.razonsocial, a.fecha_alta, a.estatusid, d.nombre, b.nombre, a.guia, e.timbrado, e.serie, e.folio, a.orden_compra, s.sucursal, p.nombre, c.condicionesid, a.pagoid
					order by 
						a.id desc
				End
		end
		-- select * from tblPedidos where id in( 224453,224469,224468)
		-- update tblPedidos set estatusid,  = 1 where id = 224453

		-- select top 300 * from tblPedidos
		-- exec pPedidos @cmd=35, @clienteid=80331, @fhaini='2023-04-22 11:46:18.907', @fhafin ='2023-11-22 11:46:18.907'
     -- EXEC pPedidos @cmd=36, @clienteid='80334', @cfdid='24476', @pedidoid='232601
		  /*Agrega partida de Pedidos Multiples*/
		IF @cmd = 36
BEGIN
    -- Actualizar tblPedidos con el nuevo CFDI
    UPDATE p 
    SET cfdid = @cfdid 
    FROM tblPedidos p
    LEFT JOIN tblEmpaquetado c ON c.custOrderId = p.id
    LEFT JOIN tblPedidosConceptos d ON p.id = d.pedidoid
    WHERE p.id = @pedidoid AND c.custOrderId = @pedidoid AND c.quantity > 0 AND ISNULL(d.borradobit, 0) = 0;

    DECLARE c CURSOR FOR
    -- Seleccionar productos de tblPedidosConceptos empaquetados o productos de gastos de envío
    SELECT DISTINCT c.id
    FROM tblPedidosConceptos c 
    LEFT JOIN tblEmpaquetado e ON c.id = e.conceptId
    WHERE 
        (pedidoid = @pedidoid AND e.custOrderId = @pedidoid AND e.quantity > 0 AND ISNULL(c.borradobit, 0) = 0)
        OR 
        (c.productoid IN (30547, 10002, 10003, 10004, 10005, 10006) AND pedidoid = @pedidoid AND ISNULL(c.borradobit, 0) = 0);

    OPEN c;
    DECLARE @pedidoconceptoIdxM INT;

    FETCH NEXT FROM c INTO @pedidoconceptoIdxM;
    WHILE (@@FETCH_STATUS <> -1)
    BEGIN
        IF (@@FETCH_STATUS <> -2)
        BEGIN
            DECLARE @productoIdxM INT = 0;
            DECLARE @tipoimpuestoIdM INT = 0;
            DECLARE @ivaIdxM FLOAT = 0;
            DECLARE @porcentaje_descuentoM FLOAT = 0;
            DECLARE @importe_descuentoM FLOAT = 0;
            DECLARE @subtotalM MONEY = 0;
            DECLARE @tallaProductM INT = 0;

            -- Obtener el producto del concepto
            SELECT @productoIdxM = productoid 
            FROM tblPedidosConceptos 
            WHERE id = @pedidoconceptoIdxM AND ISNULL(borradobit, 0) = 0;

            -- Obtener el tipo de impuesto
            SELECT @tipoimpuestoIdM = ISNULL(tasaid, 0) 
            FROM tblMisProductos 
            WHERE id = @productoIdxM;

            -- Obtener el porcentaje de descuento del cliente
            SELECT @porcentaje_descuentoM = ISNULL(descuento, 0) 
            FROM tblMisClientes 
            WHERE id = @clienteid;

            -- Calcular el descuento
            SELECT @importe_descuentoM = (@porcentaje_descuentoM / 100) * (cantidad * precio)
            FROM tblPedidosConceptos 
            WHERE id = @pedidoconceptoIdxM AND ISNULL(borradobit, 0) = 0;

            -- Definir el IVA según el tipo de impuesto
            IF @tipoimpuestoIdM = 1
                SET @ivaIdxM = 0;
            ELSE IF @tipoimpuestoIdM = 2
                SET @ivaIdxM = 0.11;
            ELSE IF @tipoimpuestoIdM = 3
                SET @ivaIdxM = 0.16;
            ELSE
                SET @ivaIdxM = 0.16;

            -- Insertar en la tabla temporal
            INSERT INTO tblCFD_Partidas_temporal (cfdid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento, claveprodserv, claveunidad, talla, piezasporcaja, cajas)
            SELECT 
                @cfdid, 
                c.productoid, 
                CASE
                    @clienteid
                    WHEN 158 THEN ISNULL(p.upc, '')
                    WHEN 182 THEN ISNULL(p.upc, '')
                    WHEN 80296 THEN ISNULL(p.upc, '')
                    ELSE ISNULL(c.codigo, '')
                END,
                c.descripcion, 
                CASE 
                    -- Si el producto está en tblEmpaquetado, usar la cantidad de empaquetado; si no, usar la cantidad de tblPedidosConceptos
                    WHEN EXISTS (SELECT 1 FROM tblEmpaquetado e2 WHERE e2.conceptId = c.id) THEN e.quantity
                    ELSE c.cantidad
                END,
                u.nombre, 
                CONVERT(MONEY, c.precio), 
                (CONVERT(MONEY, c.precio) * CASE WHEN EXISTS (SELECT 1 FROM tblEmpaquetado e2 WHERE e2.conceptId = c.id) THEN e.quantity ELSE c.cantidad END) AS importe,
                ((CONVERT(MONEY, c.precio) * CASE WHEN EXISTS (SELECT 1 FROM tblEmpaquetado e2 WHERE e2.conceptId = c.id) THEN e.quantity ELSE c.cantidad END) - @importe_descuentoM) * @ivaIdxM,
                CONVERT(DECIMAL(18, 2), @importe_descuentoM),
                p.claveprodserv, 
                p.claveunidad,
                ISNULL(ISNULL(TRY_PARSE(p.tallaMX AS FLOAT), 0) * 10, 0),
                10,
                1
            FROM 
                tblPedidosConceptos c
                INNER JOIN tblMisProductos p ON p.id = c.productoid
                INNER JOIN tblUnidad u ON u.clave = p.claveunidad
                LEFT JOIN tblEmpaquetado e ON c.id = e.conceptId
            WHERE 
                c.id = @pedidoconceptoIdxM AND ISNULL(c.borradobit, 0) = 0 AND ISNULL(e.borradorbit, 0) = 0;
        END;
        FETCH NEXT FROM c INTO @pedidoconceptoIdxM;
    END;
    
    CLOSE c;
    DEALLOCATE c;

    -- Devolver el CFDI generado
    SELECT @cfdid AS cfdid;

END;

			/*	Actualiza estatus de pedido a facturado	*/
	if @cmd=37
	begin
		declare @tmpAlmacenIdM as Integer=0
		select @pedidoid=id, @tmpAlmacenId=isnull(almacenid,0) from tblPedidos where cfdid=@cfdid
		update tblCFD set pedidoid=@pedidoid where id=@cfdid 
		update tblPedidos set estatusid=7 where cfdid=@cfdid
		if @tmpAlmacenIdM>0
		begin
			update tblCFD set almacenid=@tmpAlmacenId where id=@cfdid
		end
	end

	if @cmd=38 
		BEGIN

			insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, tipodocumentoid, proyectoid )
			select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, @tasaid, @sucursalid, @almacenid, 1, @proyectoid from tblMisClientes where id=@clienteid

			--insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, aprobacion, certificado, lugar_exp, cliente, orden_compra, remision, condicionesId, vencimiento, metodopagoid, conducto, descuento, valor_neto, valor_tasa_cero, observaciones, pagare, tasaid, sucursalid, almacenid, proyectoid, tipodocumentoid )
			--select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @aprobacion, @certificado, @lugar_exp, @cliente, @orden_compra, @remision, @condicionesId, convert(datetime, @vencimiento, 103), @metodopagoid, @conducto, convert(money, @descuento), convert(money, @valor_neto), convert(money, @valor_tasa_cero), @observaciones, @pagare, @tasaid, @sucursalid, @almacenid, @proyectoid, @tipodocumentoid from tblMisClientes where id=@clienteid
			
			select @@identity as cfdid
		END

		if @cmd=39
		BEGIN
			select round(isnull(sum(a.importe),0) + isnull(sum(a.iva),0)-isnull(sum(a.importe_descuento),0) ,3) as saldo 
			from tblCFD_Partidas a 
			inner join tblCFD b on b.id=a.cfdid 
			where b.estatus_cobranzaId=1 
				and b.estatus<>3 
				and DATEDIFF(D, b.fecha_promesa, GETDATE())>0
				and b.timbrado=1
				and dbo.fnTipoDocumentoId(b.serie, b.folio)=1 
				and b.clienteid= @clienteid

		END
		-- exec pPedidos @cmd=39,@clienteid=132



		if @cmd=40
	begin
		--insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, tipodocumentoid )
		--select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, @tasaid, @sucursalid, @almacenid, 1 from tblMisClientes where id=@clienteid
		insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, tipodocumentoid,metodopagoid,usocfdi,periodicidad_id,fac_global_mes,fac_global_anio,condicionesId,formapagoId )
		select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, @tasaid, @sucursalid, @almacenid,1,'PPD','S01','04',DATEPART(MONTH,GETDATE()),DATEPART(YEAR,GETDATE()),1,'99' from tblMisClientes where id=@clienteid


		select @cfdid=@@identity
		update tblPedidos set cfdid=@cfdid
		where id=@pedidoid
		
		declare c cursor
		for
		select id from tblPedidosConceptos where pedidoid=@pedidoid and isnull(borradobit,0) = 0

		open c
		--declare @pedidoconceptoIdx int
		
		fetch next from c into @pedidoconceptoIdx
		while (@@FETCH_STATUS <> -1)
			begin
				 if (@@FETCH_STATUS <> -2)
					 begin
						--declare @productoIdx int=0
						--declare @tipoimpuestoId int = 0
						--declare @ivaIdx float = 0
						--declare @porcentaje_descuento float=0
						--declare @importe_descuento float=0
						--declare @subtotal money=0
						--declare @tallaProduct int =0
						
						select @productoIdx = productoid from tblPedidosConceptos where id=@pedidoconceptoIdx and isnull(borradobit,0) = 0
						select @tipoimpuestoId=isnull(tasaid,0) from tblMisProductos where id=@productoIdx

						select @porcentaje_descuento=isnull(descuento,0) from tblMisClientes where id=@clienteid

						select @importe_descuento = (@porcentaje_descuento / 100) * (cantidad * precio)  from tblPedidosConceptos where id=@pedidoconceptoIdx and isnull(borradobit,0) = 0
						
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
						

						insert into tblCFD_Partidas ( cfdid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento, claveprodserv, claveunidad,talla, piezasporcaja,cajas,numero_identificacion,fraccion_arancelaria,cantidad_aduana,unidad_aduana,unitario_aduana,valor_dolares)
						select 
							@cfdid, 
							c.productoid, 
							case
								@clienteid
									when 158 then isnull(p.upc,'')
									when 182 then isnull(p.upc,'')
									when 80296 then isnull(p.upc,'')
									else isnull(c.codigo,'') end,
							c.descripcion, 
							c.cantidad, 
							u.nombre, 
							convert(money, c.precio ), 
							c.importe, 
							(c.importe - @importe_descuento) *  @ivaidx, 
							convert(decimal (18,6), @importe_descuento),
							p.claveprodserv, 
							p.claveunidad,
							isnull(isnull(TRY_PARSE(p.tallaMX as float),0)*10,0),
							10,
							1,
							case
								@clienteid
									when 158 then isnull(p.upc,'')
									when 182 then isnull(p.upc,'')
									when 80296 then isnull(p.upc,'')
									else isnull(c.codigo,'') end
							,'64041117'
							, c.cantidad
							,'09'
							,convert(money, c.precio )
							,c.importe
						from 
							tblPedidosConceptos c
							inner join tblMisProductos p on p.id=c.productoid
							inner join tblUnidad u on u.clave=p.claveunidad
						where 
							c.id=@pedidoconceptoIdx and isnull(borradobit,0) = 0
					 end
				fetch next from c into @pedidoconceptoIdx
			end
		close c
		deallocate c
		
		select @cfdid as cfdid
	end

		/*	Agrega cfd y partidas de un pedido	*/
	--if @cmd=40
	--begin

	--	insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, tipodocumentoid,metodopagoid,usocfdi,periodicidad_id,fac_global_mes,fac_global_anio,condicionesId )
	--	select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, @tasaid, @sucursalid, @almacenid, 1,'PPD','S01','04',DATEPART(MONTH,GETDATE()),DATEPART(YEAR,GETDATE()),1 from tblMisClientes where id=@clienteid
		
	--	select @cfdid=@@identity
	--	update tblPedidos set cfdid=@cfdid
	--	where id=@pedidoid
		
	--	declare c cursor
	--	for
	--	select id from tblPedidosConceptos where pedidoid=@pedidoid and isnull(borradobit,0) = 0

	--	open c
	--	declare @pedidoconceptoIdx2 int
		
	--	fetch next from c into @pedidoconceptoIdx2
	--	while (@@FETCH_STATUS <> -1)
	--		begin
	--			 if (@@FETCH_STATUS <> -2)
	--				 begin
	--					--declare @productoIdx int=0
	--					--declare @tipoimpuestoId int = 0
	--					--declare @ivaIdx float = 0
	--					--declare @porcentaje_descuento float=0
	--					--declare @importe_descuento float=0
	--					--declare @subtotal money=0
	--					--declare @tallaProduct int =0
						
	--					select @productoIdx = productoid from tblPedidosConceptos where id=@pedidoconceptoIdx2 and isnull(borradobit,0) = 0
	--					select @tipoimpuestoId=isnull(tasaid,0) from tblMisProductos where id=@productoIdx

	--					select @porcentaje_descuento=isnull(descuento,0) from tblMisClientes where id=@clienteid

	--					select @importe_descuento = (@porcentaje_descuento / 100) * (cantidad * precio)  from tblPedidosConceptos where id=@pedidoconceptoIdx2 and isnull(borradobit,0) = 0
						
	--					if @tipoimpuestoid=1
	--						begin
	--							set @ivaidx=0
	--						end
	--					else if @tipoimpuestoid=2
	--						begin
	--							set @ivaidx=0.11
	--						end
	--					else if @tipoimpuestoid=3
	--						begin
	--							set @ivaidx=0.16
	--						end
	--					else
	--						begin
	--							set @ivaidx=0.16
	--						end
						

	--					insert into tblCFD_Partidas ( cfdid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento, claveprodserv, claveunidad,talla, piezasporcaja,cajas,numero_identificacion,fraccion_arancelaria,cantidad_aduana,unidad_aduana,unitario_aduana,valor_dolares)
	--					select 
	--						@cfdid, 
	--						c.productoid, 
	--						case
	--							@clienteid
	--								when 158 then isnull(p.upc,'')
	--								when 182 then isnull(p.upc,'')
	--								when 80296 then isnull(p.upc,'')
	--								else isnull(c.codigo,'') end,
	--						c.descripcion, 
	--						c.cantidad, 
	--						u.nombre, 
	--						convert(money, c.precio ), 
	--						c.importe, 
	--						(c.importe - @importe_descuento) *  @ivaidx, 
	--						convert(decimal (18,6), @importe_descuento),
	--						p.claveprodserv, 
	--						p.claveunidad,
	--						isnull(isnull(TRY_PARSE(p.tallaMX as float),0)*10,0),
	--						10,
	--						1,
	--						case
	--							@clienteid
	--								when 158 then isnull(p.upc,'')
	--								when 182 then isnull(p.upc,'')
	--								when 80296 then isnull(p.upc,'')
	--								else isnull(c.codigo,'') end
	--						,'64041117'
	--						, c.cantidad
	--						,'09'
	--						,convert(money, c.precio )
	--						,c.importe
	--					from 
	--						tblPedidosConceptos c
	--						inner join tblMisProductos p on p.id=c.productoid
	--						inner join tblUnidad u on u.clave=p.claveunidad
	--					where 
	--						c.id=@pedidoconceptoIdx2 and isnull(borradobit,0) = 0
	--				 end
	--			fetch next from c into @pedidoconceptoIdx2
	--		end
	--	close c
	--	deallocate c
		
	--	select @cfdid as cfdid
	--end

	/*	Agrega cfd y partidas de un pedido	*/
	/*	Agrega cfd y partidas de un pedido	*/
	--if @cmd=40
	--begin

	--	insert into tblCFD ( clienteid, tipocontribuyenteid, razonsocial, calle, num_int, num_ext, colonia, municipio, estadoid, cp, rfc, fecha_crea, orden_compra, tasaid, sucursalid, almacenid, tipodocumentoid,metodopagoid,usocfdi,periodicidad_id,fac_global_mes,fac_global_anio,condicionesId )
	--	select id, tipocontribuyenteid, razonsocial, fac_calle, fac_num_int, fac_num_ext, fac_colonia, fac_municipio, fac_estadoid, fac_cp, fac_rfc, getdate(), @orden_compra, @tasaid, @sucursalid, @almacenid, 1,'PPD','S01','04',DATEPART(MONTH,GETDATE()),DATEPART(YEAR,GETDATE()),1 from tblMisClientes where id=@clienteid
		
	--	select @cfdid=@@identity
	--	update tblPedidos set cfdid=@cfdid
	--	where id=@pedidoid
		
	--	declare c cursor
	--	for
	--	select id from tblPedidosConceptos where pedidoid=@pedidoid and isnull(borradobit,0) = 0

	--	open c
	--	declare @pedidoconceptoIdx2 int
		
	--	fetch next from c into @pedidoconceptoIdx2
	--	while (@@FETCH_STATUS <> -1)
	--		begin
	--			 if (@@FETCH_STATUS <> -2)
	--				 begin
	--					--declare @productoIdx int=0
	--					--declare @tipoimpuestoId int = 0
	--					--declare @ivaIdx float = 0
	--					--declare @porcentaje_descuento float=0
	--					--declare @importe_descuento float=0
	--					--declare @subtotal money=0
	--					--declare @tallaProduct int =0
						
	--					select @productoIdx = productoid from tblPedidosConceptos where id=@pedidoconceptoIdx2 and isnull(borradobit,0) = 0
	--					select @tipoimpuestoId=isnull(tasaid,0) from tblMisProductos where id=@productoIdx

	--					select @porcentaje_descuento=isnull(descuento,0) from tblMisClientes where id=@clienteid

	--					select @importe_descuento = (@porcentaje_descuento / 100) * (cantidad * precio)  from tblPedidosConceptos where id=@pedidoconceptoIdx2 and isnull(borradobit,0) = 0
						
	--					if @tipoimpuestoid=1
	--						begin
	--							set @ivaidx=0
	--						end
	--					else if @tipoimpuestoid=2
	--						begin
	--							set @ivaidx=0.11
	--						end
	--					else if @tipoimpuestoid=3
	--						begin
	--							set @ivaidx=0.16
	--						end
	--					else
	--						begin
	--							set @ivaidx=0.16
	--						end
						

	--					insert into tblCFD_Partidas ( cfdid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva, importe_descuento, claveprodserv, claveunidad,talla, piezasporcaja,cajas,numero_identificacion,fraccion_arancelaria,cantidad_aduana,unidad_aduana,unitario_aduana,valor_dolares)
	--					select 
	--						@cfdid, 
	--						c.productoid, 
	--						case
	--							@clienteid
	--								when 158 then isnull(p.upc,'')
	--								when 182 then isnull(p.upc,'')
	--								when 80296 then isnull(p.upc,'')
	--								else isnull(c.codigo,'') end,
	--						c.descripcion, 
	--						c.cantidad, 
	--						u.nombre, 
	--						convert(money, c.precio ), 
	--						c.importe, 
	--						(c.importe - @importe_descuento) *  @ivaidx, 
	--						convert(decimal (18,6), @importe_descuento),
	--						p.claveprodserv, 
	--						p.claveunidad,
	--						isnull(isnull(TRY_PARSE(p.tallaMX as float),0)*10,0),
	--						10,
	--						1,
	--						case
	--							@clienteid
	--								when 158 then isnull(p.upc,'')
	--								when 182 then isnull(p.upc,'')
	--								when 80296 then isnull(p.upc,'')
	--								else isnull(c.codigo,'') end
	--						,'64041117'
	--						, c.cantidad
	--						,'09'
	--						,convert(money, c.precio )
	--						,c.importe
	--					from 
	--						tblPedidosConceptos c
	--						inner join tblMisProductos p on p.id=c.productoid
	--						inner join tblUnidad u on u.clave=p.claveunidad
	--					where 
	--						c.id=@pedidoconceptoIdx2 and isnull(borradobit,0) = 0
	--				 end
	--			fetch next from c into @pedidoconceptoIdx2
	--		end
	--	close c
	--	deallocate c
		
	--	select @cfdid as cfdid
	--end


	/*	Agrega cfd y partidas de un pedido	*/
		if @cmd=41
			begin

				Insert into tblFacAutRegistro(cfdid,idCarga,mensajeError,fechaCrea,correcto,error)
				Values (@cfdid,@idCarga,@MensajeError,GETDATE(),@correcto,@error)

			END
			--** Consecutivo de Facturacion Automatica
		if @cmd=42
			begin

				Insert into tblFacAutConsecutivo(fechaCrea)
				Values (GETDATE())
				select @@identity as facAutConsecutivo
			END

		-- 
		/*	Actualiza Guia, id pago de pedido */
		if @cmd=43
			IF @origen = 1
					begin
						update tblPedidos 
							set 
							pagoid=@idPago,
							guia=@guia
						where 
						orden_compra = @orden_compra
					end
				else if @origen = 2
					BEGIN
						update tblPedidos 
							set 
							pagoid=@idPago,
							guia=@guia
						where 
						orden_compra = @orden_compra
					end
					IF @cmd = 44
BEGIN
    DECLARE @pedido BIGINT;
	SELECT TOP 1  @pedido = id
    FROM tblPedidos
    WHERE orden_compra = @orden_compra;

	IF @pedido IS NULL
    
   BEGIN
        -- Insertar en tblPedidos y obtener el ID insertado con SCOPE_IDENTITY()
        INSERT INTO tblPedidos (userid, clienteid, estatusid, sucursalid, tasaid, orden_compra, proyectoid, almacenid, fecha_alta)
        VALUES (
            @userid,                                                        
            80334,                                                         
            1,                                                            
            12413,                                                          
            (SELECT tasaid FROM tblMisProductos WHERE codigo = @sku),      
            @orden_compra,                                                  
            ISNULL((SELECT id FROM tblProyecto WHERE nombre = @marca), 3),  
            5,                                                              
            GETDATE()                                                       
        );

		SET @pedido = SCOPE_IDENTITY();
        -- Devolver el ID del pedido insertado
        SELECT @pedido as pedido

    END
   
    INSERT INTO tblPedidosConceptos (pedidoid, productoid, codigo, descripcion, cantidad, unidad, precio, importe, iva)
    VALUES (
        @pedido,                                                      
        (SELECT id FROM tblMisProductos WHERE codigo = @sku), 
        @sku,                                                           
        (SELECT descripcion FROM tblMisProductos WHERE codigo = @sku),    
        @cantidad,                                                      
        (SELECT unidad FROM tblMisProductos WHERE codigo = @sku),         
        ISNULL(@precio, 0) / 1.16,                                       
        (ISNULL(@cantidad, 0) * ISNULL(@precio, 0)) / 1.16,             
        ((ISNULL(@cantidad, 0) * ISNULL(@precio, 0)) - ((ISNULL(@cantidad, 0) * ISNULL(@precio, 0)) / 1.16)) -- iva
    );
	 SELECT SCOPE_IDENTITY() AS pedidoconcepto;
    
    ---- Devolver el pedidoid generado
    --SELECT @pedidoid AS pedidoid;

		

END


		if @cmd=46
		BEGIN
			update tblPedidos SET olaid=@ola_id, ola_estatus=@ola_status where id=@pedidoid
		END

		if @cmd=47
		BEGIN
			update tblPedidos SET olaid=null, ola_estatus=null where olaid=@ola_id;
		END

		if @cmd = 48 
		begin 
					INSERT INTO tblcfd_partidas (
				cfdid, 
				productoid, 
				codigo, 
				descripcion, 
				cantidad, 
				unidad, 
				precio, 
				importe, 
				iva, 
				importe_descuento, 
				claveprodserv, 
				claveunidad,
				talla, 
				piezasporcaja, 
				cajas
			)
			SELECT 
				cfdid,
				productoid,
				codigo,
				MIN(descripcion) AS descripcion,      -- Tomar el primer valor (puede ser MIN o MAX)
				SUM(cantidad) AS cantidad,            -- Sumar las cantidades
				MIN(unidad) AS unidad,                -- Tomar el primer valor
				precio,                               -- Agrupado por precio
				SUM(importe) AS importe,              -- Sumar los importes
				SUM(iva) AS iva,                      -- Sumar los IVA
				SUM(importe_descuento) AS importe_descuento, -- Sumar los descuentos
				MIN(claveprodserv) AS claveprodserv,  -- Tomar el primer valor
				MIN(claveunidad) AS claveunidad,      -- Tomar el primer valor
				MIN(talla) AS talla,                  -- Tomar el primer valor
				MIN(piezasporcaja) AS piezasporcaja,  -- Tomar el primer valor
				MIN(cajas) AS cajas                   -- Tomar el primer valor
			FROM 
				tblcfd_partidas_temporal
			WHERE 
				cfdid = @cfdid
			GROUP BY 
				cfdid, 
				productoid, 
				codigo, 
				precio;
				 -- Elimina los datos de la tabla temporal para el cfdid especificado
		DELETE FROM tblcfd_partidas_temporal
		WHERE cfdid = @cfdid;

		end


			/* Elimina pedido sin eliminar partidas  */	
	if @cmd = 49
  begin
    -- Actualiza el estatus del pedido
    update tblPedidos
    set estatusid = 11
    where id = @pedidoid;

    -- Guarda el número de filas afectadas
    declare @rowsAffectedPedidos int = @@ROWCOUNT;

    -- Actualiza los conceptos del pedido
    update tblPedidosConceptos
    set borradobit = 1
    where pedidoid = @pedidoid;

    -- Guarda el número de filas afectadas
    declare @rowsAffectedConceptos int = @@ROWCOUNT;

    -- Verifica si ambas operaciones afectaron filas
    if @rowsAffectedPedidos > 0 and @rowsAffectedConceptos > 0
    begin
        select 'Se realizó exitosamente' as Mensaje;
    end
    else
    begin
        select 'Error: No se pudieron actualizar ambos registros' as Mensaje;
     end
  end

	/* Reactiva pedido desde consigna  */	
	if @cmd = 50
  begin
    -- Actualiza el estatus del pedido
    update tblPedidos
    set estatusid = 1
    where id = @pedidoid;

    -- Guarda el número de filas afectadas
    SET @rowsAffectedPedidos = @@ROWCOUNT;

    -- Actualiza los conceptos del pedido
    update tblPedidosConceptos
    set borradobit = 0
    where pedidoid = @pedidoid;

    -- Guarda el número de filas afectadas
    SET @rowsAffectedConceptos = @@ROWCOUNT;

    -- Verifica si ambas operaciones afectaron filas
    if @rowsAffectedPedidos > 0 and @rowsAffectedConceptos > 0
    begin
        select 'Se realizó exitosamente' as Mensaje;
    end
    else
    begin
        select 'Error: No se pudieron actualizar ambos registros' as Mensaje;
     end
  end

    SET NOCOUNT OFF;
END


-- select top 100 * from tblpedidos order by id desc


GO


