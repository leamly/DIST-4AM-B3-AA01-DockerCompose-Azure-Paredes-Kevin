# Proyecto: Arquitectura de Microservicios Distribuidos en Azure
**Asignatura / Tema:** Despliegue de Aplicaciones Distribuidas con Docker Compose, API Gateway y Autenticación JWT en Azure.


## Arquitectura del Proyecto
El sistema está compuesto por una arquitectura orientada a microservicios en .NET, orquestados mediante Docker Compose y desplegados en una máquina virtual Linux (Ubuntu 24.04) en Microsoft Azure. 

La comunicación externa se centraliza a través de un **API Gateway** (puerto 80) que enruta las peticiones HTTP hacia los microservicios correspondientes, los cuales residen en una red interna aislada. La comunicación asíncrona entre servicios se maneja a través de **RabbitMQ**, y la persistencia de datos se realiza en un contenedor de **SQL Server 2022**.

### Estructura de Directorios
```bash
/
├── ApiGetewayA/           # Configuración del API Gateway (YARP/Ocelot)
├── CategoriaA/            # Microservicio de Categorías
├── InventarioA/           # Microservicio de Inventario
├── LibroA/                # Microservicio de Libros
├── OAuthJWT/              # Microservicio de Autenticación
├── VehiculoA/             # Microservicio de Vehículos
├── SqlScripts/            # Scripts de inicialización de Base de Datos
├── docker-compose.yml     # Orquestador de contenedores
├── .gitignore             
└── README.md
```
## Descripción de los Servicios

* OAuthJWT (Autenticación): Encargado de validar las credenciales de los usuarios y emitir los tokens JWT (JSON Web Tokens) necesarios para acceder a los recursos protegidos.

* API Gateway: Punto de entrada único público. Recibe las peticiones en el puerto 80 y hace el proxy inverso hacia el microservicio correcto dentro de la red interna de Docker.

* Microservicios de Negocio (Categoría, Vehículo): APIs RESTful independientes que exponen operaciones CRUD para cada entidad. Se comunican con la base de datos SQL Server y emiten eventos a RabbitMQ.

* SQL Server 2022: Base de datos relacional centralizada. Utiliza un script de inicialización para crear las bases de datos y usuarios específicos de cada microservicio.

* RabbitMQ: Gestor de colas de mensajería (Message Broker) que permite la comunicación asíncrona y el desacoplamiento entre los microservicios.

## Instrucciones para Ejecución (Servidor)
Para levantar la arquitectura completa desde cero, asegúrese de tener Docker y Docker Compose instalados. Ubíquese en la raíz del proyecto y ejecute:
```bash
sudo docker-compose up -d --build
```

Este comando construirá las imágenes y levantará todos los contenedores en segundo plano. 

Puede verificar el estado con 
```bash
sudo docker ps
```

## Procedimiento para Autenticación mediante el Postman (Token JWT)
La mayoría de los endpoints de negocio están protegidos. Para consumirlos, siga estos pasos:

1. Realice una petición POST al endpoint de login a través del Gateway usando:

    POST
    ```bash
    http://57.151.129.118:80/api/Auth/login
    ```
2. El servidor responderá con un código 200 OK y un string que representa el Token JWT.

3. Para acceder a un microservicio, realice la petición GET adjuntando el token en la cabecera HTTP Authorization:

    GET
    ```bash
    http://57.151.129.118:80/api/vehiculo
    ```
    ```bash
    Header: Authorization: Bearer <SU_TOKEN_JWT>
    ```

## Servicios Desplegados en Azure (Accesos Directos)
El proyecto se encuentra alojado en una Máquina Virtual en Azure.

Interfaces Swagger
OAuthJWT (Autenticación): 
```bash
http://57.151.129.118:8085/swagger
```

Vehículos: 
```bash
http://57.151.129.118:8084/swagger
```

Categorías: 
```bash
http://57.151.129.118:8083/swagger
```

Panel de Administración de RabbitMQ

Credenciales por defecto: admin / admin123

URL: 
```bash
http://57.151.129.118:15672
```

## Administración de Recursos en Azure (Post-Revisión)
Para evitar consumos de facturación innecesarios tras finalizar la revisión del proyecto, se recomienda gestionar los recursos desde el Portal de Azure (portal.azure.com):
1. Para detener temporalmente (Pausar cobro de procesamiento):
    * Vaya al recurso de la Máquina Virtual (SRVLENA).
    * En el menú superior de la sección "Información general", haga clic en el botón Detener. Espere a que el estado cambie a "Detenida (desasignada)".
2. Para eliminar permanentemente (Destruir toda la infraestructura):
    * Vaya a "Grupos de recursos" y seleccione SRVLEN_grupo.
    * Haga clic en Eliminar grupo de recursos en el menú superior.
    * Confirme escribiendo el nombre del grupo. Esto borrará la VM, la IP pública, el disco y la red virtual asociada, deteniendo todo cobro definitivamente.

