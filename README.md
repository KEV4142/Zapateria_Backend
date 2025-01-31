# Zapatería Backend

Este repositorio contiene el backend de una aplicación para la gestión de una zapatería. Está desarrollado con .NET y C#, y permite realizar operaciones relacionadas con la administración de productos, usuarios y pedidos.

## Características
- API RESTful para la gestión de la zapatería.
- Conexión a base de datos PostgreSQL integrado desde SupaBase.
- Integración con Cloudinary para la gestión de imágenes.

## Requisitos previos
- .NET SDK 8.0 o superior instalado.
- Una cuenta en [Supabase](https://supabase.com/).
- Una base de datos PostgreSQL.
- Cuenta en [Cloudinary](https://cloudinary.com/).
- Git para clonar el repositorio.

## Instalación y configuración

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/KEV4142/Zapateria_Backend.git
   cd Zapateria_Backend
   ```

2. **Configurar las variables de entorno:**
   Crear un archivo `.env` en el directorio raíz del proyecto y agregar las siguientes variables:
   ```env
   DB_CONNECTION= [enlace de conexion brindado por Supabase]
   TOKEN_KEY= [Campo tipo string aleatorio]
   C_CLOUDNAME= [Campo brindado por Cloudinary]
   C_APIKEY= [Campo brindado por Cloudinary]
   C_APISECRET= [Campo brindado por Cloudinary]
   FRONTEND_ORIGIN=*
   BACKEND_ORIGIN=*
   ```

   > **Nota:** Cambia estas variables de entorno según tus necesidades y evita compartir credenciales sensibles en repositorios públicos. Adicional las ultimas 2 variables es para habilitar la funcion CORS y qué encabezados de host (Host) son permitidos al realizar solicitudes al servidor.

3. **Restaurar las dependencias:**
   Ejecuta el siguiente comando para restaurar los paquetes necesarios:
   ```bash
   dotnet restore
   ```

4. **Ejecutar las migraciones:**
   Asegúrate de que la conexión a la base de datos esté configurada correctamente y ejecuta:
   ```bash
   cd Persistencia
   dotnet ef database update
   ```

5. **Iniciar el servidor:**
   Inicia el backend localmente:
   ```bash
   cd WebApi
   dotnet run
   ```

   El servidor estará disponible en `http://localhost:5000` por defecto (o `https://localhost:5001` para HTTPS).

## Uso
- Usa herramientas como [Postman](https://www.postman.com/) para probar los endpoints de la API.
- Integra el backend con el frontend especificando el origen permitido en `FRONTEND_ORIGIN`.

## Despliegue
Puedes desplegar este proyecto en cualquier servicio compatible, como Azure App Service, AWS, o Heroku. Recuerda configurar las variables de entorno necesarias en tu plataforma de despliegue.

## Tecnologías utilizadas
- **.NET 8**: Framework principal para el backend.
- **Supabase**: Almacenamiento para motor de base de datos.
- **PostgreSQL**: Base de datos relacional.
- **Cloudinary**: Almacenamiento de imágenes.


## Licencia
Este proyecto está bajo la Licencia MIT. Consulta el archivo `LICENSE` para más detalles.
