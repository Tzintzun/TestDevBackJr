# EVALUACIÓN TÉCNICA NUXIBA

Prueba: **DESARROLLADOR JR**

Deadline: **1 día**

Nombre: David Tzintzun Gonzalez

---

## Clona y crea tu repositorio para la evaluación

1. Clona este repositorio en tu máquina local.
2. Crea un repositorio público en tu cuenta personal de GitHub, BitBucket o Gitlab.
3. Cambia el origen remoto para que apunte al repositorio público que acabas de crear en tu cuenta.
4. Coloca tu nombre en este archivo README.md y realiza un push al repositorio remoto.

---

## Instrucciones Generales

1. Cada pregunta tiene un valor asignado. Asegúrate de explicar tus respuestas y mostrar las consultas o procedimientos que utilizaste.
2. Se evaluará la claridad de las explicaciones, el pensamiento crítico, y la eficiencia de las consultas.
3. Utiliza **SQL Server** para realizar todas las pruebas y asegúrate de que las consultas funcionen correctamente antes de entregar.
4. Justifica tu enfoque cuando encuentres una pregunta sin una única respuesta correcta.
5. Configura un Contenedor de **SQL Server con Docker** utilizando los siguientes pasos:

### Pasos para ejecutar el contenedor de SQL Server

Asegúrate de tener Docker instalado y corriendo en tu máquina. Luego, ejecuta el siguiente comando para levantar un contenedor con SQL Server:

```bash
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd'    -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
```

6. Conéctate al servidor de SQL con cualquier herramienta como **SQL Server Management Studio** o **Azure Data Studio** utilizando las siguientes credenciales:
   - **Servidor**: localhost, puerto 1433
   - **Usuario**: sa
   - **Contraseña**: YourStrong!Passw0rd

---

# Examen Práctico para Desarrollador Junior en .NET 8 y SQL Server

**Tiempo estimado:** 1 día  
**Total de puntos:** 100

---

## Instrucciones Generales:

El examen está compuesto por tres ejercicios prácticos. Sigue las indicaciones en cada uno y asegúrate de entregar el código limpio y funcional.

Además, se proporciona un archivo **CCenterRIA.xlsx** para que te bases en la estructura de las tablas y datos proporcionados.

[Descargar archivo de ejemplo](CCenterRIA.xlsx)

---

## Ejercicio 1: API RESTful con ASP.NET Core y Entity Framework (40 puntos)

**Instrucciones:**  
Desarrolla una API RESTful con ASP.NET Core y Entity Framework que permita gestionar el acceso de usuarios.

1. **Creación de endpoints**:
   - **GET /logins**: Devuelve todos los registros de logins y logouts de la tabla `ccloglogin`. (5 puntos)
   - **POST /logins**: Permite registrar un nuevo login/logout. (5 puntos)
   - **PUT /logins/{id}**: Permite actualizar un registro de login/logout. (5 puntos)
   - **DELETE /logins/{id}**: Elimina un registro de login/logout. (5 puntos)

2. **Modelo de la entidad**:  
   Crea el modelo `Login` basado en los datos de la tabla `ccloglogin`:
   - `User_id` (int)
   - `Extension` (int)
   - `TipoMov` (int) → 1 es login, 0 es logout
   - `fecha` (datetime)

3. **Base de datos**:  
   Utiliza **Entity Framework Core** para crear la tabla en una base de datos SQL Server basada en este modelo. Aplica migraciónes para crear la tabla en la base de datos. (10 puntos)

4. **Validaciones**:  
   Implementa las validaciones necesarias para asegurar que las fechas sean válidas y que el `User_id` esté presente en la tabla `ccUsers`. Además, maneja errores como intentar registrar un login sin un logout anterior. (10 puntos)

5. **Pruebas Unitarias** (Opcional):  
   Se valorará si incluyes pruebas unitarias para los endpoints de tu API utilizando un framework como **xUnit** o **NUnit**. (Puntos extra)

## Respuesta Ejercicio 1:

### Levantar la base de datos.
1. Para levantar la base de datos se requiere tener instalado y ejecutando **Docker**
2. Ejecutar el comando `docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourStrong!Passw0rd'    -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2019-latest`
3. Para verificar la conexion podemos conectarnos a tavez de **SQL Server Management Studio**  con las credenciales
   - **Servidor**: localhost, puerto 1433
   - **Usuario**: sa
   - **Contraseña**: YourStrong!Passw0rd
4. Una vez conectados al servidor de base de datos, crear una base de datos con el nombre `CCenterRIA`
   
### Modelado de entidades.
Dentro de la carpeta **TestDevBackJr** podemos encontrar el proyecto de **ASP.NET core Web API** el cual podemos abrir con **Visual Studio**. 

1. Al abrir el proyecto primero debemos configurar el `ConnectionString` a traves de un secreto. Para eso abrimos la terminal del administrador de paquetes (`Herramientas>Administrador de paquete NuGet>Consola del Administrador de paquetes`). Ahí ejecutamos los siguientes comandos:
      ```bash
         dotnet user-secrets init
         dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost; Database=CCenterRIA;User Id=sa; Password=YourStrong!Passw0rd; Encrypt=False;"
      ```
2. Verificamos en el Administrador de paquete NuGet (`Herramientas>Administrador de paquete NuGet>Administrador de paquetes NuGet para la solución`) tener instalados los paquetes `Microsoft.EntityFrameworkCore.SqlServer` y `Microsoft.EntityFrameworkCore.Tools`
3. En la Terminal del administrador de paquetes ejecutamos el siguiente comando:
   ```bash
      update-database
   ```
   Esto utilizará la migración previamente creada. Dicha migración se realizó a partir los modelos que se encuentran en la carpeta `Models` del proyecto **TestDevBackJr**. Los nombres de los atributos se modificaron para seguir la convención de nombres para Entity FrameWork, pero se utilizo el decorador **[Column]** y **[Table]** para utilizar los nombres de columnas y tablas definidos en el archivo **CCenterRIA.xlsx**.

### Carga de información en la Base de datos.
Dentro del repositorio se encuentran los archivos `INSERT_AREAS.sql`, `INSERT_LOGINS.sql` e  `INSERT_USERS.sql` Estos archivos se deben abrir en **SQL Server Management Studio** conectado a la base de datos `CCenterRIA` y ejecutarse en el siguiente orden.
- `INSERT_AREAS.sql`
- `INSERT_USERS.sql`
- `INSERT_LOGINS.sql`


### Ejecución de la API.
Para ejecutar la API, en **Visual Studio** se debe ir a  `Depurar>Iniciar Depuración`. La API estará disponible en `https://localhost:5050` y/o `http://localhost:5051`

### Endpoints
#### GET /logins
Regresa una lista de todos los registros de la tabla ccloglogin

##### Responses
###### 200 OK
```json
[
    {
        "id": 1,
        "userId": 70,
        "extension": -8,
        "tipoMov": 1,
        "fecha": "2023-01-05T18:02:44"
    },...
]
```
###### 404 NotFound
No existen registros.

###### 500 InternalServerError: 
Error interno del servidor.

#### POST /logins
Permite crear un nuevo log en la Base de datos.
##### Request body
```json
{
  "userId": 0, //Required
  "extension": 0, //Required
  "tipoMov": 1, //Required
  "fecha": "2025-05-26T04:54:16.697Z" //Required
}
```
##### Responses
###### 200 Ok
Retorna el log creado en la base de datos.
```json
{
  "id": 0,
  "userId": 0,
  "extension": 0,
  "tipoMov": 1,
  "fecha": "2025-05-26T04:55:02.016"
}
```
###### 400 BadRequest
El servidor retorna este código  por las siguientes razones.
- El cuerpo de la petición esta incompleto 
- `tipoMov` es diferente de 0 y 1
- Se quiere registrar un Logout sin existir un Login previo
- Ya existe un Login/Logout previo, sin concluir.
- La fecha del ultimo login/logout para el usuario es posterior a la fecha del nuevo registro.

###### 500 InternalServerError: 
Error interno del servidor.

#### PUT /logins/{logId}
Permite actualizar un log meiante su ID
##### Request body

```json
{
  "userId": 0, //Required
  "extension": 0, //Required
  "tipoMov": 1, //Required
  "fecha": "2025-05-26T04:54:16.697Z" //Required
}
```
##### Response
###### 200 Ok
Retorna el Log actualizado en base de datos.
```json
{
  "id": 0,
  "userId": 0,
  "extension": 0,
  "tipoMov": 1,
  "fecha": "2025-05-26T04:55:02.016"
}
```

###### 400 BadRequest
El servidor retorna este código  por las siguientes razones.
- El cuero de la peticion esta incompleto 

###### 404 NotFound
El servidor retorna este código  por las siguientes razones.
- No se encontró el Id del Log.
- No se encontró el Id del usuario a actualizar.

###### 500 InternalServerError: 
Error interno del servidor.

#### DELETE /logins/{logId}
Permite eliminar un log mediante su ID

##### Response
###### 200 Ok
Retorna mensaje `Registro {logId} eliminado correctamente`.

###### 404 NotFound
El servidor retorna este código  por las siguientes razones.
- No se encontró el Id del Log.

###### 500 InternalServerError: 
Error interno del servidor.

---
## Ejercicio 2: Consultas SQL y Optimización (30 puntos)

**Instrucciones:**

Trabaja en SQL Server y realiza las siguientes consultas basadas en la tabla `ccloglogin`:

1. **Consulta del usuario que más tiempo ha estado logueado** (10 puntos):
   - Escribe una consulta que devuelva el usuario que ha pasado más tiempo logueado. Para calcular el tiempo de logueo, empareja cada "login" (TipoMov = 1) con su correspondiente "logout" (TipoMov = 0) y suma el tiempo total por usuario.

   Ejemplo de respuesta:  
   - `User_id`: 92  
   - Tiempo total: 361 días, 12 horas, 51 minutos, 8 segundos

2. **Consulta del usuario que menos tiempo ha estado logueado** (10 puntos):
   - Escribe una consulta similar a la anterior, pero que devuelva el usuario que ha pasado menos tiempo logueado.

   Ejemplo de respuesta:  
   - `User_id`: 90  
   - Tiempo total: 244 días, 43 minutos, 15 segundos

3. **Promedio de logueo por mes** (10 puntos):
   - Escribe una consulta que calcule el tiempo promedio de logueo por usuario en cada mes.

   Ejemplo de respuesta:  
   - Usuario 70 en enero 2023: 3 días, 14 horas, 1 minuto, 16 segundos

## Respuesta Ejercicio 2:
   Dentro del archivo `Ejercicio 2 Consultas SQL y Optimización.sql` se encuentran las consultas para los 3 puntos del ejercicio, el cual se puede ejecutar en **SQL Server Management Studio**

   Para poder sumar las horas entre registros de login y logout. Primero se asigna un número de manera secuencial a cada registro, ordenándolos por fecha y separandolos por Id de usuario.
   ### LogsOrdenados
   ```sql
   SELECT User_id, TipoMov, fecha,
	      ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) AS númeroFila
   FROM ccloglogin;
   ```

   Posteriormente obtenemos los registros que son contiguos dentro de la secuencia y verificamos que el registro anterior tenga `TipoMov=1` y el registro siguiente tenga `TipoMov=0`. 

   ### logsEmparejados
   ```sql
   SELECT login.User_id, 
         login.fecha AS FechaLogin, 
         logout.fecha AS FechaLogout, 
         DATEDIFF(SECOND, login.fecha, logout.fecha) AS tiempoLog
   FROM LogsOrdenados AS login
   JOIN LogsOrdenados AS logout
   ON login.User_id = logout.User_id
   AND login.númeroFila + 1 = logout.númeroFila
   AND login.TipoMov = 1
   AND logout.TipoMov = 0
   ```

   Ahora podemos sumar los segundos de todas las sesiones:
   ```sql
   SELECT 
      User_id,
      SUM(tiempoLog) AS segundosTotales
   FROM logsEmparejados
   GROUP BY User_id
   ```

   También podemos obtener una cadena con los diás, horas, minutos y segundos.
   ```sql
   CONCAT(segundosTotales/86400 ,' dias, ',
      (segundosTotales % 86400)/3600,' horas, ',
      (segundosTotales % 3600)/60 , ' minutos, ', 
      (segundosTotales % 60), ' segundo') AS "Tiempo total"
   ```

   Para obtener el promedio mensual por usuario podemos usar la función AVG() por usuario, agrupado por mes y año.
   ```sql
   AVG(tiempoLog) OVER (PARTITION BY User_ID, MONTH(FechaLogin), YEAR(FechaLogin)) AS promedio
   ```
---

## Ejercicio 3: API RESTful para generación de CSV (30 puntos)

**Instrucciones:**

1. **Generación de CSV**:  
   Crea un endpoint adicional en tu API que permita generar un archivo CSV con los siguientes datos:
   - Nombre de usuario (`Login` de la tabla `ccUsers`)
   - Nombre completo (combinación de `Nombres`, `ApellidoPaterno`, y `ApellidoMaterno` de la tabla `ccUsers`)
   - Área (tomado de la tabla `ccRIACat_Areas`)
   - Total de horas trabajadas (basado en los registros de login y logout de la tabla `ccloglogin`)

   El CSV debe calcular el total de horas trabajadas por usuario sumando el tiempo entre logins y logouts.

2. **Formato y Entrega**:
   - El CSV debe ser descargable a través del endpoint de la API.
   - Asegúrate de probar este endpoint utilizando herramientas como **Postman** o **curl** y documenta los pasos en el archivo README.md.

## Respuesta Ejercicio 3:
Para poder consumir el endpoint se primero se requiere crear un procedimiento almacenado que se encuentra en ` PROCEDIMIENTO_OBTENER_HORAS.sql`. Conectándose a la base de datos `CCenterRIA` mediante **SQL Server Management Studio** conectandose a la base dedatos y ejecutando el archivo.

Este procedimiento permite calcular el número total de horas que un usuario ha estado registrado, basado en los registros de la tabla `ccloglogin`.

### Ejecución de la API.
Para ejecutar la API, en **Visual Studio** se debe ir a  `Depurar > Iniciar Depuración`. La API estará disponible en `https://localhost:5050` y/o `http://localhost:5051`.

### Endpoints

#### GET /users/{userId}/reporte
Obtiene un archivo CSV con el siguiente formato de nombre `Reporte_{userId}_{FechaActual}.csv`, para un usuario con ID: `userId`
##### Response
###### 200 Ok
Retorna un archivo CSV con los siguientes datos.
- Nombre de usuario (Login de la tabla ccUsers)
- Nombre completo
- Área (tomado de la tabla ccRIACat_Areas)
- Total de horas trabajadas (obtenido con el procedimiento `ObtenerHorasTotalesUser` creado anteriormente)

###### 400 BadRequest
El servidor devuelve este código  por las siguientes razones.
- El valor `userId` no es un número entero.

###### 404 NotFound
El servidor devuelve este código por las siguientes razones.
- No se encontró el Id del usuario.

###### 500 InternalServerError
Error interno del servidor.

### Descarga CSV
Si consumimos el endpoint `https://localhost:5050/users/71/reporte` este muestra el archivo CSV
![Prueba Postman](https://imgur.com/sPuyJe5.png)

Al guardar el archivo este se guarda con el nombre `Reporte_71_2025_05_26.csv`
![Vista Excel](https://i.imgur.com/yTmwm38.png)

---

## Entrega

1. Sube tu código a un repositorio en GitHub o Bitbucket y proporciona el enlace para revisión.
2. El repositorio debe contener las instrucciones necesarias en el archivo **README.md** para:
   - Levantar el contenedor de SQL Server.
   - Conectar la base de datos.
   - Ejecutar la API y sus endpoints.
   - Descargar el CSV generado.
3. **Opcional**: Si incluiste pruebas unitarias, indica en el README cómo ejecutarlas.



---

Este examen evalúa tu capacidad para desarrollar APIs RESTful, realizar consultas avanzadas en SQL Server y generar reportes en formato CSV. Se valorará la organización del código, las mejores prácticas y cualquier documentación adicional que proporciones.
