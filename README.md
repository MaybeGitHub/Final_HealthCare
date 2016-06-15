# HealthCare

## Descripcion

La idea de mi proyecto se basa en poder llevar medicamentos y artículos de farmácia a aquellas personas que, por comodidad o por indisposicion, prefieren que se lo lleven a casa por un poco mas de dinero. También tiene un efecto secundario que es, que, como los fármacos no pueden contratar gente sin cualificación para llevar fármacos, esta página abre puestos de trabajo para aquellos farmaceuticos que quieran empezar en una empresa para ir creciendo dentro.

A continuación describo brevemente los distintos componentes de mi programa:

## Modelos

Los modelos se pueden diferenciar en dos tipos: aquellos que el dataContext genera y que hacen referencia a tablas y aquellos cuyo objetivo es facilitar el trabajo con los primeros y las vistas, ya sea añadiendo mas variables interesantes que el primer modelo no tenia o bien con métodos que hacen la vida mas sencilla desde la vista ( por ejemplo, el modelo carro con una funcion para saber el valor total de los productos )

A si pues los modelos serian:

### Usuario

Modelo que representa al cliente y a la empresas, contiene datos y restricciones tipicas para evitar datos absurdos y errores de ejecución, ademas de estar ligado al modelo Direccion, Pedidos y, en el caso de las empresas, tambien al modelo Productos.

### Direccion

Modelo que contiene la informacion de la casa o local del Usuario. Gracias a él, el pedido llega a su destino.

### Producto

Usado por la empresa para recibir Pedidos y con el que el cliente juega a lo largo de su navegacion por la página, mediante el uso del carro, para comprar los que quiera y generar Pedidos.

### Pedido

Generado por el Usuario al comprar Productos de las empresas y terminados en las Empresas al completar el Pedido mediante el envio de los productos al usuario. Es el modelo que se encarga de tramitar el proceso de envio desde que el cliente compra, hasta que le llega los productos.

## Controladores

Estas clases son las encargadas de dirigir y de tratar las peticiones de los clientes a las Views para que se muestren correctamente. Los controladores son:

### Home

Primer controlador encontrado, es el controlador mas simple ya que solo se encarga de dar la bienvenida al cliente.

### Login

Controlador que tramita las peticiones GET y POST de registro y acceso de los usuarios, permitiendo o denegando los formularios para que estos inserten nuevo elementos en la base de datos. Es el primer controlador que se encuentran todos los usuarios.

### Productos

Controlador específico para clientes, se encarga de llevar su flujo de la compra a lo largo de las páginas, manejando el Carro para guardar los productos. Ademas, tambien maneja el perfil del cliente, asi como su historial de pedidos.

### Empresas

Controlador específico de las empresas encargado de manejar los pedidos y editar perfiles.

### Admin

Controlador basado en la Administracion, tambien es especifico de las empresas y aqui pueden generar nuevos productos, dar de baja algunos y editarlos.

### Carro

Controlador encargado de guardar, borrar, vaciar y todo demas que tiene que ver con el carro del cliente.

## Vistas

Todas las vistas estan diseñadas con un entorno agradable, con los colores de empresa verde claro como primario y azul claro como secundario y compuestas con botones y zonas grandes y faciles de usar para que cualquier cliente, sin importar su edad, sea capaz de navegar por la web sin problemas. Todas ellas usan bootstrap y jquery aunque solo algunas manejan jquery de forma mas importante.

## Mejoras

En el apartado de mejoras cabrian destacar que son mejoras de cara al cliente para su facilidad de uso, no porque la pagina no sea usable ahora mismo. Algunas de las mejoras mas imporantes serian:

 - Envio de PDF ademas de email a la hora de terminar la compra ( + un email mas elaborado )
 - Ordenacion de las empresas por cercania usando el codigo postal ( la api de google haria este trabajo mucho mas sencillo )
 - Encriptacion de contraseñas en las tablas
 - Mejor filtrado de rutas, ademas de landing pages personalizadas para los errores
 - Notificacion al cliente del proceso de creacion de cuenta o al cambiar los estados de los pedidos
 - Agrupacion de los pedidos para las empresas, para mejor manejo y efectividad a la hora de tratarlos
 - Cuando se tengan mas datos de productos, generacion de categorias y subcategorias predefinidas para que el cliente tenga mas facil su seleccion a la hora de comprar
 - Posible notificacion por otros medios, como podria ser SMS, para que el cliente estuviera mas en contacto con la empresa
