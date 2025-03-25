# Test Store API

Este proyecto es una API desarrollada en .NET 8, conectada a una base de datos en Azure. La API está documentada con Swagger y cuenta con varias funcionalidades clave, como limitación de tasa (rate limiting), versionado de endpoints, logging con Serilog y autenticación utilizando JWT.

## Funcionalidades

- **Controladores**: 
  - **Brand**: CRUD completo para gestionar marcas.
  - **Product**: CRUD completo para gestionar productos.
  - **Login**: Controlador para autenticación de usuarios mediante JWT.

- **Limitación de tasa (Rate Limiting)**: 
  - 5 peticiones cada 5 minutos por usuario sin JWT.
  - 10 peticiones cada 5 minutos con JWT.

- **Autenticación**: 
  - Los endpoints están protegidos mediante JWT (JSON Web Token).
  - Requiere login para obtener un token y acceder a los endpoints.

- **Documentación de la API**:
  - Swagger está integrado para facilitar la consulta de la documentación interactiva de la API.

- **Logging**:
  - Se utiliza **Serilog** para registrar eventos importantes y facilitar el diagnóstico de problemas.

## Requisitos

- **.NET 8**
- **Base de Datos en Azure**
- **JWT para autenticación**

## Endpoints

### Autenticación (Login)

- **POST** `/api/auth/login`
  - Inicia sesión con las credenciales del usuario y devuelve un token JWT.
  - **Credenciales predeterminadas**:
    - **Username**: `admin`
    - **Password**: `admin`
  - **Body**:
    ```json
    {
      "username": "admin",
      "password": "admin"
    }
    ```
  - **Respuesta**:
    ```json
    {
      "username": "Jhon",
      "lastname": "Doe",
      "token": "JWT_TOKEN"
    }
    ```

### Brands (Marcas)

- **GET** `/api/brands`
  - Obtiene todas las marcas.
  
- **GET** `/api/brands/{id}`
  - Obtiene una marca por su ID.
  
- **POST** `/api/brands`
  - Crea una nueva marca.

- **PUT** `/api/brands/{id}`
  - Actualiza una marca existente.

- **DELETE** `/api/brands/{id}`
  - Elimina una marca por su ID.

### Products (Productos)

- **GET** `/api/products`
  - Obtiene todos los productos.

- **GET** `/api/products/{id}`
  - Obtiene un producto por su ID.

- **POST** `/api/products`
  - Crea un nuevo producto.

- **PUT** `/api/products/{id}`
  - Actualiza un producto existente.

- **DELETE** `/api/products/{id}`
  - Elimina un producto por su ID.

## Rate Limiting

La API tiene un mecanismo de limitación de tasa para evitar el abuso de los recursos:

- **Sin JWT**: Máximo 5 peticiones cada 5 minutos.
- **Con JWT**: Máximo 10 peticiones cada 5 minutos.

Si se excede el límite de peticiones, se devolverá un error con un mensaje informativo.

## Swagger

La documentación interactiva de la API está disponible en el siguiente enlace:

[Swagger UI](https://test-store-adonet-api.azurewebsites.net/swagger/index.html)

## Cómo ejecutar el proyecto

1. Clona el repositorio:
   ```bash
   git clone https://github.com/tuusuario/test-store-api.git
