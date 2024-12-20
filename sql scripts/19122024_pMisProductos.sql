USE [erp_neogenis_13122024]
GO

/****** Object:  StoredProcedure [dbo].[pMisProductos]    Script Date: 18/12/2024 05:21:22 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[pMisProductos]
	@cmd int,
	@clienteId int=0,
	@productoid int=0,
	@codigo varchar(50)=null,
	@sku varchar(100)=null,
	@upc varchar(100)=null,
	@claveprodserv varchar(10)=null,
	@claveunidad varchar(10)=null,
	@descripcion varchar(400)=null,
	@unitario varchar(50)=null,
	@unitario2 varchar(50)=null,
	@unitario3 varchar(50)=null,
	@unitario4 varchar(50)=null,
	@tasaid int=0,
	@objeto_impuestoid int = 0,
	@txtSearch varchar(500)=null,
	@maximo float=0,
	@minimo float=0,
	@punto_reorden float=0,
	@costo_estandar varchar(50)=null,
	@costo_promedio varchar(50)=null,
	@compra_min float=0,
	@uso text=null,
	@tiempo_entrega varchar(50)=null,
	@presentacion varchar(50)=null,
	@monedaid int=0,
	@tipo_cambio_std varchar(50)=null,
	@proveedorid int=0,
	@inventariableBit bit=0,
	@foto varchar(100)=null,
	@peso float=0,
	@codigoclienteid int=0,
	@perecederoBit bit=0,
	@sucursalid int=0,
	@almacenid int=0,
	@coleccionid int=0,
	@proyectoid int=0,
	@modeloEstilo varchar(100) = null,

	@plataforma varchar(100) = null,
	@genero varchar(100) = null,
	@generoid int = null,
	@tallaUSA varchar(100) = null,
	@tallaMX varchar(100) = null,
	@color varchar(100) = null,
	@material varchar(100) = null,
	@pesoKg varchar(100) = null,
	@empaqueAlto varchar(100) = null,
	@empaqueLargo varchar(100) = null,
	@empaqueAncho varchar(100) = null,
	@upcSearch varchar(100) = '',
	@marketplaceid int = 0,
	@conExistencia bit = 0,
	@codigoproductoid int = 0,
	@codigo_celda1 varchar(100) = null,
	@inventario_celda1 float = null,
	@codigo_celda2 varchar(100) = null,
	@inventario_celda2 float = null,
	@codigo_celda3 varchar(100) = null,
	@inventario_celda3 float = null,
	@codigo_barras_locacion varchar(100) = null,
	@cantidad int = null,
	@colorMX varchar(100) = null,
	@descripcion_corta varchar(200) = null,
	@asignados bit = 0,
	@objImpId int = null,
	@marketPlaceLiverpool varchar(2) = null, 
	@marketPlaceShopify varchar(2) = null, 
	@marketPlaceAcctivity varchar(2) = null  
	--,@activoBit bit = 1

AS
BEGIN
	SET NOCOUNT ON;
	
	declare @tmpPrecioid int = 0
	
	/*	Lista de Mis productos	*/
	if @cmd=1
		begin
		/* Codigo de clientes start */
		declare @txtCodigoCliente varchar(500)=''		
		select @txtCodigoCliente=  STUFF((SELECT '  ' + cast(a.codigo as varchar)
			from tblMisProductos a left join tblCodigoCliente b on b.productoid = a.id
		where b.codigo like '%' + @txtSearch + '%' FOR XML PATH('')), 1, 2, '') 		
		/* Codigo de clientes end  */
		
		/* actualiza diponibles start */
		update tblMisProductos 
			set num_disponibles = dbo.fnProductosDisponibles(id)
			where
			(@proyectoid=0 or isnull(proyectoid,0) = @proyectoid)
			and (@coleccionid=0 or isnull(coleccionid,0) = @coleccionid)
			and (@txtSearch='' or codigo like '%' + @txtSearch  + '%' 
								or codigo like '%' + @txtCodigoCliente  + '%' 
								or descripcion like '%' + @txtSearch + '%' 
								or modeloEstilo like '%' + @txtSearch + '%' 
								or plataforma like '%' + @txtSearch + '%' 
								or genero like '%' + @txtSearch + '%'
								or tallaUSA like '%' + @txtSearch + '%' 
								or tallaMX like '%' + @txtSearch + '%' 
								or color like '%' + @txtSearch + '%' 
								or material like '%' + @txtSearch + '%' )
			and (@upcSearch = '' or upc = @upcSearch)
			--case when dbo.fnExistenciaProducto(id)=0 then 0 else dbo.fnProductosDisponibles(id) end			
		/* actualiza diponibles end  */

			BEGIN
				select 
					a.id, 
					isnull(a.codigo,'') as codigo, 
					isnull(a.upc,'') as upc, 
					isnull(a.claveprodserv,'') as claveprodserv, 
					isnull(u.clave,'') + '-' + isnull(u.nombre,'') as unidad,
					left(isnull(a.descripcion,''),60) as descripcion, 
					left(isnull(a.descripcion_corta,''),60) as descripcion_corta, 
					isnull(a.unitario,0) as unitario, 
					isnull(a.unitario2,0) as unitario2, 
					isnull(a.unitario3,0) as unitario3, 
					isnull(a.unitario4,0) as unitario4, 
					isnull(a.costo_estandar,0) as costo,
					isnull(a.costo_promedio,0) as promedio,
					isnull(b.nombre,'') as tasa,
					--dbo.fnExistenciaAlmacen(a.id,1) as monterrey, 
					dbo.fnExistenciaAlmacen(a.id,2) as mexico, 
					--dbo.fnExistenciaAlmacen(a.id,3) as guadalajara,
					--dbo.fnExistenciaAlmacen(a.id,4) as mermas,
					dbo.fnExistenciaAlmacen(a.id,5) as matriz, 
					--dbo.fnExistenciaProducto(a.id) - dbo.fnProductosDisponibles(a.id) as proceso,
					--dbo.fnProductosDisponibles(a.id) as disponibles,
					dbo.fnExistenciaProducto(a.id) - num_disponibles as proceso,
					num_disponibles as disponibles,
					dbo.fnExistenciaProductoConsignacion(a.id) as consignacion,
					dbo.fnConsignacionesProducto(a.id) as detalle_consignacion,
					isnull(a.costo_estandar,0) as costo_estandar,
					isnull(m.nombre,'') as moneda,
					isnull(c.codigo,'') as coleccion,
					isnull(p.nombre,'') as proyecto,
					isnull(a.modeloEstilo,'') as modeloEstilo,
					isnull(a.plataforma,'') as plataforma,
					isnull(v.descripcion,'') as genero,
					isnull(a.tallaUSA,'') as tallaUSA,
					isnull(a.tallaMX,'') as tallaMX,
					isnull(a.color,'') as color,
					isnull(a.colorMX,'') as colorMX,
					isnull(a.material,'') as material,
					isnull(a.pesoKg,'') as pesoKg,
					isnull(a.empaqueAlto,'') as empaqueAlto,
					isnull(a.empaqueLargo,'') as empaqueLargo,
					isnull(a.empaqueAncho,'') as empaqueAncho,
					dbo.fnProductosUbicacion(a.id)as ubicacion,
					dbo.fnProductosEnProcesp(a.id) as en_proceso
				from 
					tblMisProductos a 
					left join tblTasa b on a.tasaid=b.id
					left join tblMoneda m on a.monedaid=m.id
					left join tblColeccion c on c.id=a.coleccionid
					left join tblProyecto p on p.id=a.proyectoid
					left join tblUnidad u on u.clave=a.claveunidad
					left join tblGenero v on a.generoid = v.id
				where
					(@proyectoid=0 or isnull(a.proyectoid,0) = @proyectoid)
					and (@coleccionid=0 or isnull(a.coleccionid,0) = @coleccionid)
					and (@txtSearch='' or a.codigo like '%' + @txtSearch  + '%' 
										or a.codigo like '%' + @txtCodigoCliente  + '%' 
										or a.descripcion like '%' + @txtSearch + '%' 
										or a.modeloEstilo like '%' + @txtSearch + '%' 
										or a.plataforma like '%' + @txtSearch + '%' 
										or a.genero like '%' + @txtSearch + '%'
										or a.tallaUSA like '%' + @txtSearch + '%' 
										or a.tallaMX like '%' + @txtSearch + '%' 
										or a.color like '%' + @txtSearch + '%' 
										or a.material like '%' + @txtSearch + '%' )
					and (@upcSearch = '' or a.upc = @upcSearch)
					and (@conExistencia =0 or num_disponibles > 0)
					--and activobit = 1
				order by 
					a.codigo asc				
			end

		END

	/*	Borra producto	*/
	if @cmd=2
		begin
			delete from tblMisProductos where id=@productoid
		end
		
	/*	Agrega	*/
	if @cmd=3
		begin
			if not exists ( select id from tblMisProductos where codigo=@codigo )
			begin
				insert into tblMisProductos ( clienteid, codigo, sku, upc, claveprodserv, claveunidad, descripcion, unitario, unitario2, unitario3, unitario4, tasaid, maximo, minimo, punto_reorden, costo_estandar, compra_min, uso, tiempo_entrega, presentacion, monedaid, tipo_cambio_std, proveedorId, inventariableBit, foto, peso, perecederoBit, coleccionid, proyectoid, costo_promedio, modeloEstilo, plataforma, genero, generoid, tallaUSA, tallaMX, color, material, pesoKg, empaqueAlto, empaqueLargo, empaqueAncho, objeto_impuestoid,activobit,fecha_act,colorMX,descripcion_corta)
				values ( @clienteid, @codigo, @sku, @upc, @claveprodserv, @claveunidad, @descripcion, convert(money, @unitario), convert(money, @unitario2), convert(money, @unitario3), convert(money, @unitario4), @tasaid, @maximo, @minimo, @punto_reorden, convert(money, @costo_estandar), @compra_min, @uso, @tiempo_entrega, @presentacion, @monedaid, convert(money, @tipo_cambio_std), @proveedorId, @inventariableBit, @foto, @peso, @perecederoBit, @coleccionid, @proyectoid, convert(money,@costo_promedio),@modeloEstilo, @plataforma, @genero, @generoid, @tallaUSA, @tallaMX, @color, @material, @pesoKg, @empaqueAlto, @empaqueLargo, @empaqueAncho, @objeto_impuestoid,1,GETDATE(),@colorMX,@descripcion_corta)
				select ''
			end
			else
			begin
				select 'El código de producto: ' + @codigo + ' ya se encuentra registrado.'
			end
		end
		
	/*	Edita	*/
	if @cmd=4
		begin
			select 
				clienteid, 
				isnull(codigo,'') as codigo, 
				isnull(sku,'') as sku, 
				isnull(upc,'') as upc, 
				isnull(claveprodserv,'') as claveprodserv, 
				isnull(claveunidad,'') as claveunidad, 
				isnull(descripcion,'') as descripcion,
				isnull(unitario,0) as unitario, 
				isnull(unitario2,0) as unitario2, 
				isnull(unitario3,0) as unitario3, 
				isnull(unitario4,0) as unitario4, 
				isnull(tasaid,0) as tasaid,
				isnull(objeto_impuestoid,0) as objeto_impuestoid,
				isnull(maximo,0) as maximo, 
				isnull(minimo,0) as minimo, 
				isnull(punto_reorden,0) as punto_reorden, 
				isnull(costo_estandar,0) as costo_estandar, 
				isnull(costo_promedio,0) as costo_promedio,
				isnull(compra_min,0) as compra_min, 
				isnull(uso,'') as uso, 
				isnull(presentacion,'') as presentacion,
				isnull(tiempo_entrega,'') as tiempo_entrega, 
				isnull(monedaid,0) as monedaid, 
				isnull(tipo_cambio_std,0) as tipo_cambio_std, 
				isnull(proveedorid,0) as proveedorid,
				isnull(inventariableBit,0) as inventariableBit, 
				isnull(foto,'') as foto, 
				isnull(peso,0) as peso,
				isnull(perecederoBit,0) as perecederoBit,
				isnull(coleccionid,0) as coleccionid,
				isnull(proyectoid,0) as proyectoid,

				isnull(modeloEstilo, '') as modeloEstilo,
				isnull(plataforma, '') as plataforma,
				isnull(genero, '') as genero,
				isnull(generoid, 0) as generoid,
				isnull(tallaUSA, '') as tallaUSA,
				isnull(tallaMX, '') as tallaMX,
				isnull(color, '') as color,
				isnull(material, '') as material,
				isnull(pesoKg, '') as pesoKg,
				isnull(empaqueAlto, '') as empaqueAlto,
				isnull(empaqueLargo, '') as empaqueLargo,
				isnull(empaqueAncho, '') as empaqueAncho,
				isnull(colorMX,'') as colorMx,
				isnull(descripcion_corta,'')as descripcion_corta

			from 
				tblMisProductos
			where 
				id=@productoid
		end
		
	/*	Actualiza	*/
	if @cmd=5
		begin
			update 
				tblMisProductos
			set 
				codigo=@codigo, 
				sku=@sku, 
				upc=@upc,
				claveprodserv=@claveprodserv,
				claveunidad=@claveunidad, 
				descripcion=@descripcion, 
				tasaid=@tasaid,
				objeto_impuestoid = @objeto_impuestoid,
				unitario=convert(money, @unitario), 
				unitario2=convert(money, @unitario2), 
				unitario3=convert(money, @unitario3), 
				unitario4=convert(money, @unitario4),
				maximo=@maximo, 
				minimo=@minimo, 
				punto_reorden=@punto_reorden, 
				costo_estandar=CONVERT(money, @costo_estandar),
				costo_promedio=CONVERT(money, @costo_promedio),
				tipo_cambio_std=convert(money, @tipo_cambio_std),
				compra_min=@compra_min, 
				uso=@uso, 
				tiempo_entrega=@tiempo_entrega, 
				presentacion=@presentacion, 
				monedaid=@monedaid,
				proveedorId=@proveedorid, peso=@peso,
				inventariableBit=@inventariableBit, 
				foto=@foto, 
				perecederoBit=@perecederoBit,
				coleccionid=@coleccionid,
				proyectoid=@proyectoid,

				modeloEstilo = @modeloEstilo,
				plataforma = @plataforma,
				genero = @genero,
				generoid = @generoid,
				tallaUSA = @tallaUSA,
				tallaMX = @tallaMX,
				color = @color,
				material = @material,
				pesoKg = @pesoKg,
				empaqueAlto = @empaqueAlto,
				empaqueLargo = @empaqueLargo,
				empaqueAncho = @empaqueAncho,
				fecha_act = GETDATE(),
				colorMX = @colorMX,
				descripcion_corta = @descripcion_corta
			where 
				id=@productoid
		end
		
	/*	Agrega código de cliente	*/
	if @cmd=6
		begin
			if (select top 1 id from tblCodigoCliente where clienteid=@clienteId and productoid=@productoid ) is null
			begin
				insert into tblCodigoCliente ( clienteid, productoid, codigo )
				values ( @clienteId, @productoid, @codigo  )
			end
		end
		
	/*	Lista de códigos de un cliente	*/
	if @cmd=7
		begin
			select a.id, b.razonsocial as cliente, a.codigo from tblCodigoCliente a inner join tblMisClientes b on a.clienteid=b.id
			where a.productoid=@productoid order by a.codigo
		end
		
	/*	Elimina código de cliente	*/
	if @cmd=8
		begin
			delete from tblCodigoCliente where id=@codigoclienteid 
		end
	
	/*	Catalogo de productos para transferencias	*/
	if @cmd=9
		begin
			select
				b.id,
				case 
					when isnull(perecederoBit,0) = 0 then  b.descripcion 
					else 'Lote: ' + isnull(a.lote,'') + ' - Cad.: ' + convert(varchar(10), a.caducidad, 103) + ' - ' + b.descripcion 
				end as descripcion
			from 
				tblInventario a 
				inner join tblMisProductos b on a.productoid=b.id 
			where 
				isnull(b.inventariableBit,0)=1 and isnull(a.existencia,0) > 0
				and isnull(dbo.fnExistenciaAlmacen(b.id, @almacenid),0) > 0
			group by
				b.id, 
				case 
					when isnull(perecederoBit,0) = 0 then  b.descripcion 
					else 'Lote: ' + isnull(a.lote,'') + ' - Cad.: ' + convert(varchar(10), a.caducidad, 103) + ' - ' + b.descripcion 
				end
			order by
				descripcion
		end
		
	/*	Catalogo de productos para transferencias	*/
	if @cmd=10
		begin
			
			select @tmpPrecioid=tipoprecioid from tblMisClientes where id=@clienteid
			
			if ( select top 1 id from tblMisProductos where codigo=@txtSearch ) is not null
				begin
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 As cantidad, isnull(a.unidad,'') As unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, isnull(b.lote,'--') as lote, case when isnull(a.perecederoBit,0) = 0 then '--' else CONVERT(varchar(10), b.caducidad, 103) end as caducidad, isnull(b.existencia,0) as existencia, c.nombre as almacen,
						dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						a.codigo=@txtSearch
						and b.existencia>0
						--and b.almacenid=@almacenid
						and isnull(a.perecederoBit,0)=1
					union
					select * from(
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 As cantidad, isnull(a.unidad,'') As unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, '--' as lote, '--' as caducidad, SUM(isnull(b.existencia,0)) as existencia, c.nombre as almacen,
						dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						a.codigo=@txtSearch
						and b.existencia>0
						--and b.almacenid=@almacenid
						and isnull(a.perecederoBit,0)=0
					group by
						a.id,a.codigo,a.descripcion,a.unidad,a.unitario,a.unitario2,a.unitario3, c.nombre) t
					order by 
						descripcion asc					
				end
			else
				begin
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 As cantidad, isnull(a.unidad,'') As unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, isnull(b.lote,'--') as lote, case when isnull(a.perecederoBit,0) = 0 then '--' else CONVERT(varchar(10), b.caducidad, 103) end as caducidad, isnull(b.existencia,0) as existencia, c.nombre as almacen,
						dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						(( a.codigo like '%' + @txtSearch + '%' ) or ( a.descripcion like '%' + @txtSearch + '%' ))
						and b.existencia>0
						--and b.almacenid=@almacenid
						and isnull(a.perecederoBit,0)=1
					union
					select * from(
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 As cantidad, isnull(a.unidad,'') As unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, '--' as lote, '--' as caducidad, SUM(isnull(b.existencia,0)) as existencia, c.nombre as almacen,
						dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						(( a.codigo like '%' + @txtSearch + '%' ) or ( a.descripcion like '%' + @txtSearch + '%' ))
						and b.existencia>0
						--and b.almacenid=@almacenid
						and isnull(a.perecederoBit,0)=0
					group by
						a.id,a.codigo,a.descripcion,a.unidad,a.unitario,a.unitario2,a.unitario3, c.nombre) t
					order by 
						descripcion asc
				end
		end
		
		/*	Catalogo de productos para transferencias	*/
	if @cmd=11
		begin
			
			select @tmpPrecioid=tipoprecioid from tblMisClientes where id=@clienteid
			
			if ( select top 1 id from tblMisProductos where codigo=@txtSearch ) is not null
				begin
					select * from(
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 as cantidad, isnull(a.unidad,'') as unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, '--' as lote, '--' as caducidad, SUM(isnull(b.existencia,0)) as existencia, dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						a.codigo=@txtSearch
						and b.existencia>0
						and isnull(a.perecederoBit,0)=1
					group by
						a.id,a.codigo,a.descripcion,a.unidad,a.unitario,a.unitario2,a.unitario3) t
					union
					select * from(
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 as cantidad, isnull(a.unidad,'') as unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, '--' as lote, '--' as caducidad, SUM(isnull(b.existencia,0)) as existencia, dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						a.codigo=@txtSearch
						and b.existencia>0
						and isnull(a.perecederoBit,0)=0
					group by
						a.id,a.codigo,a.descripcion,a.unidad,a.unitario,a.unitario2,a.unitario3) t
					order by 
						codigo asc
				end
			else
				begin
					select * from(
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 as cantidad, isnull(a.unidad,'') as unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, '--' as lote, '--' as caducidad, SUM(isnull(b.existencia,0)) as existencia, dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						(( a.codigo like '%' + @txtSearch + '%' ) or ( a.descripcion like '%' + @txtSearch + '%' ))
						and b.existencia>0
						and isnull(a.perecederoBit,0)=1
					group by
						a.id,a.codigo,a.descripcion,a.unidad,a.unitario,a.unitario2,a.unitario3) t
					union
					select * from(
					select 
						a.id, a.codigo, isnull(a.descripcion,'') as descripcion, 1 as cantidad, isnull(a.unidad,'') as unidad, case @tmpPrecioid when 1 then isnull(unitario,0) when 2 then isnull(unitario2,0) when 3 then isnull(unitario3,0) else isnull(unitario,0) end as unitario, '--' as lote, '--' as caducidad, SUM(isnull(b.existencia,0)) as existencia, dbo.fnProductosDisponibles(a.id) as disponibles
					from 
						tblMisProductos a 
						inner join tblInventario b on a.id=b.productoid
						inner join tblAlmacen c on c.id=b.almacenid
					where 
						(( a.codigo like '%' + @txtSearch + '%' ) or ( a.descripcion like '%' + @txtSearch + '%' ))
						and b.existencia>0
						and isnull(a.perecederoBit,0)=0
					group by
						a.id,a.codigo,a.descripcion,a.unidad,a.unitario,a.unitario2,a.unitario3) t
					order by 
						codigo asc
				end
		end

		/*	Existencias	*/
	if @cmd=12
		begin
		if @marketplaceid = 0 begin
			/*default Shopify*/
				set @marketplaceid = 6
			/**/
		end



			if (@proyectoid=5)
				begin
					select 
						isnull(b.upc,'') as upc,
						isnull(b.codigo,'') as codigo,
						isnull(b.descripcion,'') as descripcion,
						cast(ROUND((CONVERT(decimal(12,2),d.porcentaje) * CONVERT(decimal(12,2),ISNULL(dbo.fnProductosDisponibles(b.id),0)))/CONVERT(decimal(12,2),100),0)as decimal(12,0)) as disponibles,
						isnull(b.unitario,0) as unitario
					from tblRelacionProductoMarketplaces a left join tblMisProductos b on a.productoid = b.id
						 inner join tblMarketPlace c on a.marketplaceid = c.id inner join tblPrioridad d on isnull(c.prioridadid,0) = d.id 
					where a.marketplaceid = @marketplaceid and b.proyectoid in (5,8,9,12)
				end
			else if (@proyectoid=7) -- Feetures
				begin
					select 
						isnull(b.upc,'') as upc,
						isnull(b.codigo,'') as codigo,
						isnull(b.descripcion,'') as descripcion,
						cast(ROUND((CONVERT(decimal(12,2),d.porcentaje) * CONVERT(decimal(12,2),ISNULL(dbo.fnProductosDisponibles(b.id),0)))/CONVERT(decimal(12,2),100),0)as decimal(12,0)) as disponibles,
						isnull(b.unitario,0) as unitario
					from tblRelacionProductoMarketplaces a left join tblMisProductos b on a.productoid = b.id
						 inner join tblMarketPlace c on a.marketplaceid = c.id inner join tblPrioridad d on isnull(c.prioridadid,0) = d.id 
					where a.marketplaceid = @marketplaceid and b.proyectoid in (7)
				end

			else if (@proyectoid=17) -- Acctivity ( Ciele, fitletic, leki, oofos y lock laces )
				begin
					select 
						isnull(b.upc,'') as upc,
						isnull(b.codigo,'') as codigo,
						isnull(b.descripcion,'') as descripcion,
						cast(ROUND((CONVERT(decimal(12,2),d.porcentaje) * CONVERT(decimal(12,2),ISNULL(dbo.fnProductosDisponibles(b.id),0)))/CONVERT(decimal(12,2),100),0)as decimal(12,0)) as disponibles,
						isnull(b.unitario,0) as unitario
					from tblRelacionProductoMarketplaces a left join tblMisProductos b on a.productoid = b.id
						 inner join tblMarketPlace c on a.marketplaceid = c.id inner join tblPrioridad d on isnull(c.prioridadid,0) = d.id 
					where a.marketplaceid = @marketplaceid and b.proyectoid in ( 8, 9, 16, 21,15)
				end
				else if (@proyectoid=15) -- Oofos
				begin
					select 
						isnull(b.upc,'') as upc,
						isnull(b.codigo,'') as codigo,
						isnull(b.descripcion,'') as descripcion,
						cast(ROUND((CONVERT(decimal(12,2),d.porcentaje) * CONVERT(decimal(12,2),ISNULL(dbo.fnProductosDisponibles(b.id),0)))/CONVERT(decimal(12,2),100),0)as decimal(12,0)) as disponibles,
						isnull(b.unitario,0) as unitario
					from tblRelacionProductoMarketplaces a left join tblMisProductos b on a.productoid = b.id
						 inner join tblMarketPlace c on a.marketplaceid = c.id inner join tblPrioridad d on isnull(c.prioridadid,0) = d.id 
					where a.marketplaceid = @marketplaceid and b.proyectoid in (15)
				end
			else
				begin
					select 
						isnull(b.codigo,'') as codigo,
						isnull(b.descripcion,'') as descripcion,
						cast(ROUND((CONVERT(decimal(12,2),d.porcentaje) * CONVERT(decimal(12,2),ISNULL(dbo.fnProductosDisponibles(b.id),0)))/CONVERT(decimal(12,2),100),0)as decimal(12,0)) as disponibles,
						isnull(b.unitario,0) as unitario
					from tblRelacionProductoMarketplaces a left join tblMisProductos b on a.productoid = b.id
						 inner join tblMarketPlace c on a.marketplaceid = c.id inner join tblPrioridad d on isnull(c.prioridadid,0) = d.id 
					where a.marketplaceid = @marketplaceid and b.proyectoid=@proyectoid
				end
		end


	--Consulta la existencia de un producto
	if @cmd=13
	begin
	select dbo.fnExistenciaAlmacen(a.id,5) as matriz, 
			dbo.fnExistenciaProducto(a.id) - dbo.fnProductosDisponibles(a.id) as proceso,
			dbo.fnExistenciaProductoConsignacion(a.id) as consignacion,
			dbo.fnProductosDisponibles(a.id) as disponibles
			--dbo.fnConsignacionesProducto(a.id) as detalle_consignacion 
			from 
			tblMisProductos a 
	where a.id = @productoid
	end

	/*	Agregar un StockLocator de Producto  */
	if @cmd=14
		begin
				insert into tblStockLocator (productoid, CodigoBarrasLocacion , Cantidad)
				values ( @productoid, @codigo_barras_locacion, @cantidad)
		end

		/*	Lista de códigos de un Producto StockLocator	*/
	if @cmd=15
		begin
			Select
					a.id
					,isnull(a.codigo,'') as codigo
					,isnull(b.barcode,'' )as CodigoBarrasLocacion
					,isnull(b.quantity,0) as Cantidad
			from tblMisProductos a
				inner join tblAlmacenado b on a.id = b.productId
				where a.id = @productoid and b.quantity > 0
				group by b.barcode,a.id,a.codigo, b.quantity
		end

		/*	Elimina código de Producto Stock Locator	*/
	if @cmd=16
		begin
			update tblStockLocator set bajaBit=1 where id=@codigoproductoid
		end

	/*	Lista de Mis productos Reporte StockLocation	*/
	if @cmd=17
		begin
		/* Codigo de clientes start */
		declare @txtCodigoCliente2 varchar(500)=''		
		select @txtCodigoCliente2=  STUFF((SELECT '  ' + cast(a.codigo as varchar)
			from tblMisProductos a left join tblCodigoCliente b on b.productoid = a.id
		where b.codigo like '%' + @txtSearch + '%' FOR XML PATH('')), 1, 2, '') 		
		/* Codigo de clientes end  */
		
		select 
			a.id, 
			isnull(a.codigo,'') as codigo, 
			isnull(a.upc,'') as upc,
			left(isnull(a.descripcion,''),30) as descripcion,
			isnull(c.codigo,'') as coleccion,
			isnull(p.nombre,'') as proyecto,
			isnull(b.barcode,'')as ubicacion,
			isnull(b.quantity,0)as cantidad
		from 
			tblMisProductos a 
			left join tblColeccion c on c.id=a.coleccionid
			left join tblProyecto p on p.id=a.proyectoid
			left join tblAlmacenado b on b.productId = a.id
		where
			(@proyectoid=0 or isnull(a.proyectoid,0) = @proyectoid)
			and (@coleccionid=0 or isnull(a.coleccionid,0) = @coleccionid)
			and (@txtSearch='' or a.codigo like '%' + @txtSearch  + '%' 
								or a.codigo like '%' + @txtCodigoCliente2  + '%' 
								or a.descripcion like '%' + @txtSearch + '%' 
								or a.modeloEstilo like '%' + @txtSearch + '%' 
								or a.plataforma like '%' + @txtSearch + '%' 
								or a.genero like '%' + @txtSearch + '%'
								or a.tallaUSA like '%' + @txtSearch + '%' 
								or a.tallaMX like '%' + @txtSearch + '%' 
								or a.color like '%' + @txtSearch + '%' 
								or a.material like '%' + @txtSearch + '%' )
			and (@upcSearch = '' or a.upc = @upcSearch)
			and (@asignados = 0 or LEN( b.barcode) >1 )
		order by 
			a.codigo asc				
		end
	if @cmd=19
		begin
			insert into tblMisProductos (
			  codigo, sku, upc, unidad, descripcion, 
			  descripcion_corta, genero, generoid, 
			  unitario, unitario2, unitario3, unitario4, 
			  tasaid, monedaid, proveedorid, inventariableBit, 
			  coleccionid, proyectoid, claveprodserv, 
			  claveunidad, objeto_impuestoid, 
			  activoBit, modeloEstilo, plataforma, 
			  tallaUSA, tallaMX, color, colorMX, 
			  material, peso, pesoKg, empaqueAlto, 
			  empaqueLargo, empaqueAncho, fecha_act
			) 
			VALUES( 
			  @codigo, 
			  @codigo, 
			  @upc, 
			  @claveunidad, 
			  @descripcion, 
			  @descripcion_corta, 
			  @genero, 
			  @generoId, 
			  @unitario, 
			  @unitario2, 
			  @unitario3, 
			  @unitario4, 
			  @tasaId, 
			  @monedaId, 
			  1, 
			  1, 
			  @coleccionId, 
			  @proyectoId, 
			  @claveprodserv, 
			  @claveUnidad, 
			  @objImpId, 
			  1, 
			  @modeloEstilo, 
			  @plataforma, 
			  @tallaUSA, 
			  @tallaMX, 
			  @color, 
			  @colorMX, 
			  @material, 
			  @peso, 
			  @peso, 
			  @empaqueAlto, 
			  @empaqueLargo, 
			  @empaqueAncho, 
			  getdate() 
			)

			DECLARE @idproducto bigint;

			SET @idproducto = @@IDENTITY

			--inserto relacion marketplace
			if 	UPPER(@marketPlaceLiverpool) = 'SI'
			begin 
				--Liverpool
				insert into tblRelacionProductoMarketplaces ( productoid, marketplaceid )
				VALUES(@idproducto, 1)
			end
	
			if UPPER(@marketPlaceShopify) = 'SI'
			begin 
				--shopify
				insert into tblRelacionProductoMarketplaces ( productoid, marketplaceid )
				VALUES(@idproducto, 6)
			end

			if 	UPPER(@marketPlaceAcctivity) = 'SI'
			begin 
				--Acctivity
				insert into tblRelacionProductoMarketplaces ( productoid, marketplaceid )
				VALUES(@idproducto, 7)
			end

			--lcng 19 dic 2024: INSERTA EN TABLA almado en ubi Generik con cantidad 1
			insert into tblAlmacenado(productId, barcode, quantity, datetime)
			VALUES(@idproducto, 'MTYPAP04R01N01', 0, getdate())

		end
			/*	Actualiza Precio por carga de CSV	*/
	if @cmd=20
		begin

			Update
				tblMisProductos

				Set
				unitario = @unitario,
				unitario2 = @unitario2,
				unitario3 = @unitario3,
				unitario4 = @unitario4

			Where codigo = @codigo
			select ''
	
		end

		SET NOCOUNT OFF;
	END
GO


