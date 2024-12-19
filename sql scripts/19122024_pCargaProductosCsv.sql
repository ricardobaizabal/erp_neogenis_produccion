CREATE PROCEDURE [dbo].[pCargaProductosCsv]
	@cmd int,
	@cargaid int = 0,
	@ordenId bigint=0,
	@productoId bigint=0,
	@codigo varchar(50)='',
	@cantidad int =0,
	@userid int = 0,
	@archivo varchar(50)='',
	@registros int = 0,
	@registros_correctos float=null,
	@registros_error float=null,
	@error varchar(max)=null,
	@id int = 0,
	@upc varchar(100)='',
	@claveSat varchar(50)='',
	@unidad varchar(50)='',
	@descripcion varchar(150)='',
	@marcaId int = 0,
	@coleccionId int = 0,

	@descripcion_corta varchar(300)='',
	@marca varchar(50)='',
	@temporada varchar(50)='',
	@unitario1 money = 0,
	@unitario2 money = 0,
	@unitario3 money = 0,
	@unitario4 money = 0,
	@modelo_estilo varchar(100)='',
	@plataforma varchar(100)='',
	@genero varchar(100)='',
	@tallaUSA varchar(100)='',
	@tallaMX varchar(100)='',
	@color varchar(100)='',
	@colorMX varchar(100)='',
	@material varchar(100)='',
	@peso varchar(100)='',
	@empaque_alto varchar(100)='',
	@empaque_largo varchar(100)='',
	@empaque_ancho varchar(100)='',
	@unidadMedida nvarchar(255)='',
	@claveProdServ varchar(50)='',
	@tasa varchar(50)='',
	@moneda varchar(50)= '',
	@marketPlaceLiverpool varchar(2)='',
	@marketPlaceShopify varchar(2)='',
	@marketPlaceAcctivity varchar(2)=''
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @tmpPrecioid int = 0
	
	/*	Inserta en tblCargaConcepto	*/
	if @cmd=1
		begin
			insert into tblCargaProductoCsv(userid,archivo,registros,fecha)
			Values(@userid,@archivo, @registros, GETDATE())
			select(@@IDENTITY)
		end
		--consulta tabla para carga de columnas
	if @cmd=2
		begin
			select * from tblCargaProductoCsvDetalle
		end

	if @cmd=3
		BEGIN
			--Insert into tblCargaProductoCsvDetalle(cargaid,codigo,upc,claveSat,descripcion,marcaId,coleccionId,precioUnit1,precioUnit2,precioUnit3,precioUnit4,precioUnit5,precioUnit6)
			--Values(@cargaid,@codigo,@upc,@claveSat,@unidad,@descripcion,@marcaId,@coleccionId,@precioUnit1,@precioUnit2,@precioUnit3,@precioUnit4,@precioUnit5,@precioUnit6);

			--hago inserciòn en tabla de detalle
			INSERT INTO  tblCargaProductoCsvDetalle (
				cargaid,
				codigo,
				upc,
				descripcion,
				descripcion_corta,
				marca,
				temporada,
				unitario1,
				unitario2,
				unitario3,
				unitario4,
				modelo_estilo,
				plataforma,
				genero,
				tallaUSA,
				tallaMX,
				color,
				colorMX,
				material,
				peso,
				empaque_alto,
				empaque_largo,
				empaque_ancho,
				unidadMedida,
				claveProdServ,
				moneda,
				tasa,
				claveSat,
				
				proyectoId,
				coleccionId,
				monedaId,
				tasaId,
				generoId,
				objImpId,
				marketPlaceLiverpool,
				marketPlaceShopify ,
				marketPlaceAcctivity
			)
			VALUES(
				@cargaid,
				@codigo,
				@upc,
				@descripcion,
				@descripcion_corta,
				@marca,
				@temporada,
				@unitario1,
				@unitario2,
				@unitario3,
				@unitario4,
				@modelo_estilo,
				@plataforma,
				@genero,
				@tallaUSA,
				@tallaMX,
				@color,
				@colorMX,
				@material,
				@peso,
				@empaque_alto,
				@empaque_largo,
				@empaque_ancho,
				@unidadMedida,
				@claveProdServ,
				@moneda,
				@tasa,
				@claveSat,

				(SELECT TOP 1 ISNULL(id, 0) FROM tblProyecto z WHERE LTRIM(RTRIM(UPPER(z.nombre))) LIKE '%'+ @marca +'%')  ,
				(SELECT TOP 1 ISNULL(id, 0) FROM tblColeccion y WHERE LTRIM(RTRIM(UPPER(y.codigo))) LIKE '%'+ @temporada +'%' ) ,
				(SELECT TOP 1 ISNULL(id, 0) FROM tblMoneda x WHERE LTRIM(RTRIM(UPPER(x.nombre))) LIKE '%'+ @moneda +'%' ) ,
				(SELECT TOP 1 ISNULL(id, 0) FROM tblTasa w WHERE LTRIM(RTRIM(UPPER(w.nombre))) LIKE '%'+ @tasa +'%'  ),
				(SELECT case UPPER(@genero) when 'HOMBRE' then 1 when 'MUJER' then 2 when 'UNISEX' then 3 end ),
				2,
				@marketPlaceLiverpool,
				@marketPlaceShopify,
				@marketPlaceAcctivity
			)
		END
		--select * from tblCargaProductoCsvDetalle
    if @cmd=4
		BEGIN
			select * from tblCargaProductoCsvDetalle where codigo = @codigo and cargaid =@cargaid
			
		END

	if @cmd=5
		BEGIN
		--SI
		-- exec pMisProductos @cmd=1, @txtSearch= 'demo'
			--select a.id,b.id productoid, a.codigo, b.descripcion, b.costo_estandar as costo, isnull(m.nombre,'') as moneda
			--from tblCargaProductoCsvDetalle a
			--left join tblMisProductos b on b.codigo = a.codigo
			--left join tblMoneda m on b.monedaid=m.id
			--where a.cargaid = @cargaid

			--SI
			select 
				isnull(a.codigo, '') codigo,
				isnull(a.upc, '') upc,
				isnull(a.descripcion, '') descripcion,
				isnull(a.descripcion_corta, '') descripcion_corta,
				isnull(a.marca, '') marca,
				isnull(a.temporada, '') temporada,
				isnull(a.unitario1, '') unitario1,
				isnull(a.unitario2, '') unitario2,
				isnull(a.unitario3, '') unitario3,
				isnull(a.unitario4, '') unitario4,
				isnull(a.modelo_estilo, '') modelo_estilo,
				isnull(a.plataforma, '') plataforma,
				isnull(a.genero, '') genero,
				isnull(a.tallaUSA, '') tallaUSA,
				isnull(a.tallaMX, '') tallaMX,
				isnull(a.color, '') color,
				isnull(a.colorMX, '') colorMX,
				isnull(a.material, '') material,
				isnull(a.peso, '') peso,
				isnull(a.empaque_alto, '') empaque_alto,
				isnull(a.empaque_largo, '') empaque_largo,
				isnull(a.empaque_ancho, '') empaque_ancho,
				isnull(a.unidadMedida, '') unidadMedida,
				isnull(a.claveProdServ, '') claveProdServ,
				isnull(a.moneda, '') moneda,
				isnull(a.tasa, '') tasa,
				isnull(a.claveSat, '') claveSat,
				--
				isnull(a.proyectoId, '') proyectoId,
				isnull(a.coleccionId, '') coleccionId,
				isnull(a.monedaId, '') monedaId,
				isnull(a.tasaId, '') tasaId,
				isnull(a.generoId, '') generoId,
				isnull(a.objImpId, '') objImpId,
				--isnull(a.marca) as marca,
				--'123' as coleccion
				--isnull(b.nombre,'') marca,
				--isnull(c.codigo,'') as coleccion
				
				isnull(a.marketPlaceLiverpool, '') marketPlaceLiverpool,
				isnull(a.marketPlaceShopify, '') marketPlaceShopify,
				isnull(a.marketPlaceAcctivity, '') marketPlaceAcctivity

			from tblCargaProductoCsvDetalle a
			 --left join tblProyecto b on a.marcaId = b.id
			 --left join tblColeccion c on a.coleccionId = c.id
			where cargaid = @cargaid

		END
		--select * from tblProyecto
		--select * from tblColeccion
		--select * from tblCargaProductoCsvDetalle
	--/*	Agrega item a la orden de compra	*/
	--if @cmd=6
	--	begin
	--		insert into tblOrdenCompraConceptos ( ordenId,  cantidad, productoId, codigo, descripcion, costo, monedaid )  
	--		select @ordenId, @cantidad, @productoId, codigo, descripcion, costo_estandar, monedaid from tblMisProductos where id=@productoId 
	--	end

		/*	Inserta en tblCargaConceptoError	*/
	if @cmd=7
		begin
		--SI
		--hago inserciòn en tabla de detalle
		INSERT INTO  tblCargaProductoCsvDetalleError (
			cargaid,
			codigo,
			upc,
			descripcion,
			descripcion_corta,
			marca,
			temporada,
			unitario1,
			unitario2,
			unitario3,
			unitario4,
			modelo_estilo,
			plataforma,
			genero,
			tallaUSA,
			tallaMX,
			color,
			colorMX,
			material,
			peso,
			empaque_alto,
			empaque_largo,
			empaque_ancho,
			unidadMedida,
			claveProdServ,
			moneda,
			tasa,
			claveSat,
			
			proyectoId,
			coleccionId,
			monedaId,
			tasaId,
			generoId,
			objImpId,

			marketPlaceLiverpool,
			marketPlaceShopify,
			marketPlaceAcctivity,
			error
		)
		VALUES(
			@cargaid,
			@codigo,
			@upc,
			@descripcion,
			@descripcion_corta,
			@marca,
			@temporada,
			@unitario1,
			@unitario2,
			@unitario3,
			@unitario4,
			@modelo_estilo,
			@plataforma,
			@genero,
			@tallaUSA,
			@tallaMX,
			@color,
			@colorMX,
			@material,
			@peso,
			@empaque_alto,
			@empaque_largo,
			@empaque_ancho,
			@unidadMedida,
			@claveProdServ,
			@moneda,
			@tasa,
			@claveSat,
			(SELECT TOP 1 ISNULL(id, 0) FROM tblProyecto z WHERE LTRIM(RTRIM(UPPER(z.nombre))) LIKE '%'+ @marca +'%')  ,
			(SELECT TOP 1 ISNULL(id, 0) FROM tblColeccion y WHERE LTRIM(RTRIM(UPPER(y.codigo))) LIKE '%'+ @temporada +'%' ) ,
			(SELECT TOP 1 ISNULL(id, 0) FROM tblMoneda x WHERE LTRIM(RTRIM(UPPER(x.nombre))) LIKE '%'+ @moneda +'%' ) ,
			(SELECT TOP 1 ISNULL(id, 0) FROM tblTasa w WHERE LTRIM(RTRIM(UPPER(w.nombre))) LIKE '%'+ @tasa +'%'  ),
			(SELECT case UPPER(@genero) when 'HOMBRE' then 1 when 'MUJER' then 2 when 'UNISEX' then 3 end ),
			2,
			@marketPlaceLiverpool,
			@marketPlaceShopify,
			@marketPlaceAcctivity,
			@error
		)
		end

/*	Actualiza total registros correctos	*/
	if @cmd=8
		begin			
			update 
				tblCargaProductoCsv
			set
				registros_correctos=@registros_correctos
			where
				id=@cargaid
		end

		/*Actualiza total registros erroneos	*/
	if @cmd=9
		begin			
			update 
				tblCargaProductoCsv
			set
				registros_error=@registros_error
			where
				id=@cargaid
		end

		if @cmd=10
		BEGIN
		-- exec pMisProductos @cmd=1, @txtSearch= 'demo'
		--SI
			select 
				id,
				isnull(a.codigo, '') codigo,
				isnull(a.upc, '') upc,
				isnull(a.descripcion, '') descripcion,
				isnull(a.descripcion_corta, '') descripcion_corta,
				isnull(a.marca, '') marca,
				isnull(a.temporada, '') temporada,
				isnull(a.unitario1, '') unitario1,
				isnull(a.unitario2, '') unitario2,
				isnull(a.unitario3, '') unitario3,
				isnull(a.unitario4, '') unitario4,
				isnull(a.modelo_estilo, '') modelo_estilo,
				isnull(a.plataforma, '') plataforma,
				isnull(a.genero, '') genero,
				isnull(a.tallaUSA, '') tallaUSA,
				isnull(a.tallaMX, '') tallaMX,
				isnull(a.color, '') color,
				isnull(a.colorMX, '') colorMX,
				isnull(a.material, '') material,
				isnull(a.peso, '') peso,
				isnull(a.empaque_alto, '') empaque_alto,
				isnull(a.empaque_largo, '') empaque_largo,
				isnull(a.empaque_ancho, '') empaque_ancho,
				isnull(a.unidadMedida, '') unidadMedida,
				isnull(a.claveProdServ, '') claveProdServ,
				isnull(a.moneda, '') moneda,
				isnull(a.tasa, '') tasa,
				isnull(a.claveSat, '') claveSat,
				
				--
				isnull(a.proyectoId, '') proyectoId,
				isnull(a.coleccionId, '') coleccionId,
				isnull(a.monedaId, '') monedaId,
				isnull(a.tasaId, '') tasaId,
				isnull(a.generoId, '') generoId,
				isnull(a.objImpId, '') objImpId,
				
				isnull(a.marketPlaceLiverpool, '') marketPlaceLiverpool,
				isnull(a.marketPlaceShopify, '') marketPlaceShopify,
				isnull(a.marketPlaceAcctivity, '') marketPlaceAcctivity,

				isnull(error, 0) as error
			from tblCargaProductoCsvDetalleError a
			where cargaid = @cargaid
		END
	--	select * from tblCargaProductoCsvDetalleError
		if @cmd=11
			BEGIN
				delete from tblCargaProductoCsv where id= @cargaid
			END

		if @cmd=12
			BEGIN
				truncate table tblCargaProductoCsvDetalle
			END

			--id producto relaciòn sku
		if @cmd=13
			BEGIN
				select 
				isnull (id,'') id 
				from tblmisproductos where activoBit = 1 and codigo = @codigo
			END
			-- no productos
		if @cmd=14
			BEGIN
				 select  count(id)
				from tblmisproductos where activoBit = 1 and codigo = @codigo
			END

		if @cmd=15
			BEGIN
				update tblMisProductos set activoBit = 1 where id = @id
			END
		--obtengo id PROYECTO de marca, si es que hay
		if @cmd=16
			BEGIN
				SELECT TOP 1 ISNULL(id, 0) FROM tblProyecto WHERE LTRIM(RTRIM(UPPER(nombre))) LIKE '%'+ UPPER(@marca) +'%'  
			END
		--obtengo id COLECCION de temporada, si es que hay
		if @cmd=17
			BEGIN
				SELECT TOP 1 ISNULL(id, 0) FROM tblColeccion WHERE LTRIM(RTRIM(UPPER(codigo))) LIKE '%'+ UPPER(@temporada) +'%'  
			END
		--obtengo id MONEDA de moneda, si es que hay
		if @cmd=18
			BEGIN
				SELECT TOP 1 ISNULL(id, 0) FROM tblMoneda WHERE LTRIM(RTRIM(UPPER(nombre))) LIKE '%'+ UPPER(@moneda) +'%'  
			END
		--obtengo id GENERO de genero, si es que hay
		if @cmd=19
			BEGIN
				SELECT case @genero when 'Hombre' then 1 when 'Mujer' then 2 when 'Unisex' then 3 end 
			END
		--
		if @cmd=20
			BEGIN
				SELECT TOP 1 ISNULL(id, 0) FROM tblTasa WHERE LTRIM(RTRIM(UPPER(nombre))) LIKE '%'+ @tasa +'%'  
			END
		--
    SET NOCOUNT OFF;
END

GO
