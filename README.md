# ApiRestNetNoxun

**ApiRestNetNoxun** es una API RESTful desarrollada en ASP.NET Core (.NET 6) que gestiona usuarios, roles, procedimientos, campos y datasets, con autenticación JWT y documentación integrada con Swagger.

## Requisitos Previos

- Visual Studio 2022 o superior (opcional)
- MySQL o SQL Server instalado

---

### Configurar Base de Datos

Abre el archivo `appsettings.json` y configura una **de las dos** conexiones según el motor de base de datos que quieras usar:

#### ✅ Usar MySQL

```json
"DatabaseProvider": "MySql",
"MySqlConnection": "server=localhost;database=nombre_bd;user=root;password=tu_contraseña;"
```

#### ✅ Usar SQL Server

```json
"DatabaseProvider": "SqlServer",
"SqlServerConnection": "Server=localhost;Database=nombre_bd;Trusted_Connection=True;MultipleActiveResultSets=true;"
```

### Ejecutar Migraciones

Asegúrate de tener configurado el motor correcto y luego ejecuta en terminal:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Si `dotnet ef` no está disponible, primero ejecuta:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

---

## Ejecutar el Proyecto

En terminal:

```bash
dotnet run
```

Verás algo como:

```bash
listening on: http://localhost:5009
```

---

## Ver la Documentación de la API (Swagger)

Una vez corriendo el proyecto, abre tu navegador y visita:

```
http://localhost:5009
```

Ahí podrás probar todos los endpoints de la API de manera interactiva.

---

## 🧪 Probar con Postman

### Registro de Usuario

- **URL:** `POST http://localhost:5009/api/auth/register`
- **Body (JSON):**
```json
{
  "username": "admin",
  "password": "admin123",
  "email": "admin@example.com",
  "roleName": "Admin"
}
```

### Inicio de Sesión

- **URL:** `POST http://localhost:5009/api/auth/login`
- **Body (JSON):**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

Recibirás un **token JWT** y toda la información del sistema.

---

## Seguridad

- Los endpoints protegidos requieren autorización.
- Usa el token JWT recibido en login con el botón "Authorize" en Swagger o encabezado `Authorization` en Postman:

```
Authorization: Bearer <tu_token>
```

---

## 📂 Estructura del Proyecto

- `Controllers/`: Controladores API
- `Models/`: Entidades de base de datos
- `Dtos/`: Objetos de transferencia de datos
- `Helpers/`: JWT y lógica de utilidad
- `Data/`: DbContext y configuración de EF Core

---
