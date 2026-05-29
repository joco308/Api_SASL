# Documentación de Endpoints — Api SASL

## Autenticación

| Concepto | Detalle |
|----------|---------|
| **Esquema** | Cookie `token_sesion` (HttpOnly, Secure, SameSite=Strict) |
| **Duración** | 8 horas |
| **Login** | `POST /solicitar-2fa` (email+contraseña → código al email) → `POST /verificar-2fa` (código → cookie) |

### Políticas de autorización

| Política | Roles requeridos |
|----------|-----------------|
| `PersonalAutorizado` | `Gerente`, `Administrador` |
| `Cliente` | `Cliente` |
| `Gerente` | `Gerente` |
| Sin policy (`.RequireAuthorization()`) | Cualquier usuario autenticado |
| Público | Sin autenticación |

### Formato general de respuesta

**Éxito:** `200 OK`, `201 Created` o archivo (PDF/CSV).

**Error — estructura consistente:**
```json
{ "error": "mensaje de error" }
// o
{ "mensaje": "mensaje de error" }
```

---

# 1. Usuarios — `/Api/Usuarios`

## Tipos de datos (TypeScript)

```typescript
interface UsuarioLogin {
  correo: string;
  password: string;
}

interface Login2FA {
  email: string;
  codigoIngresado: string;
}

interface NuevoUsuario {
  NombreUsuario: string;
  FechaNacimiento: string;       // date-only "yyyy-MM-dd"
  Correo: string;
  IdRol: number;
  IdEstadoCivil: number;
  IdGradoAcademico: number;
  IdGenero: number;
  Calle: string;
  idZona: number;
  NumeroCasa: number;
  Contrasena: string;
  idPais: number;
  CI: number;
}

interface EditarDireccion {
  CI: number;
  Zona: number;
  Calle: string;
  NumeroCasa: number;
}

interface EditarRol {
  CI: number;
  Rol: number;
}

interface UsuarioDatos {
  IdUsuario: number;
  NombreUsuario: string;
  Ci: number;
  correo: string;
  rol: string;
  salario: number;
  creado: string;                // ISO datetime
}

interface DatosParaSubirDoc {
  idUSer: number;
  idtipoDoc?: number | null;
  tipoDoc?: string | null;
}

interface AnadirCarrera {
  idUsuario: number;
  idCarrera?: number | null;
  Carrera?: string | null;
}

interface PedirDocumento {
  id: number;
  idtipo: number;
}
```

---

### `GET /Api/Usuarios/`
Lista completa de trabajadores.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:** —
- **Respuesta 200:**
```json
[
  {
    "IdUsuario": 1,
    "NombreUsuario": "Juan Pérez",
    "Ci": 12345678,
    "correo": "juan@mail.com",
    "rol": "Gerente",
    "salario": 5000,
    "creado": "2025-01-15T10:30:00"
  }
]
```

---

### `POST /Api/Usuarios/solicitar-2fa`
Inicia sesión enviando código 2FA al correo.

- **Auth:** público
- **Cuerpo:**
```json
{ "correo": "juan@mail.com", "password": "miClave123" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Código enviado al correo" }
```
- **Error 401:**
```json
{ "mensaje": "Credenciales incorrectas." }
```

---

### `POST /Api/Usuarios/verificar-2fa`
Verifica el código 2FA y recibe el token en cookie.

- **Auth:** público
- **Cuerpo:**
```json
{ "email": "juan@mail.com", "codigoIngresado": "482931" }
```
- **Respuesta 200:** Establece cookie `token_sesion` + 
```json
{ "mensaje": "Autenticación exitosa" }
```
- **Error 400:**
```json
{ "error": "Codigo expiro." }
```
- **Error 404:**
```json
{ "error": "Credenciales incorrectas." }
```

---

### `GET /Api/Usuarios/{servicio}`
Usuarios filtrados por asignación de servicio.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `servicio` (boolean, `true`/`false`)
- **Respuesta 200:** Lista filtrada de usuarios
- **Error 404:**
```json
{ "mensaje": "algo salio mal" }
```

---

### `POST /Api/Usuarios/nuevoUsuario`
Registra un nuevo trabajador.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{
  "NombreUsuario": "Carlos López",
  "FechaNacimiento": "1995-06-15",
  "Correo": "carlos@mail.com",
  "IdRol": 2,
  "IdEstadoCivil": 1,
  "IdGradoAcademico": 3,
  "IdGenero": 1,
  "Calle": "Av. Siempre Viva",
  "idZona": 5,
  "NumeroCasa": 742,
  "Contrasena": "claveSegura",
  "idPais": 1,
  "CI": 87654321
}
```
- **Respuesta 201:**
```json
{ "mensaje": "Usuario creado exitosamente" }
```
- **Error 400:**
```json
{ "error": "El correo ya está registrado" }
```

---

### `PATCH /Api/Usuarios/{ci}/direccion`
Actualiza la dirección de un trabajador.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `ci` (number)
- **Cuerpo:**
```json
{ "CI": 87654321, "Zona": 3, "Calle": "Calle Nueva", "NumeroCasa": 123 }
```
- **Respuesta 200:**
```json
{ "mensaje": "Dirección actualizada correctamente" }
```
- **Error 400:**
```json
{ "error": "El CI de la URL no coincide con el del cuerpo." }
```

---

### `PATCH /Api/Usuarios/{ci}/rol`
Cambia el rol de un trabajador.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `ci` (number)
- **Cuerpo:**
```json
{ "CI": 87654321, "Rol": 1 }
```
- **Respuesta 200:**
```json
{ "mensaje": "Rol actualizado correctamente" }
```
- **Error 400:**
```json
{ "error": "El CI de la URL no coincide con el del cuerpo." }
```

---

### `POST /Api/Usuarios/SubirArchivo`
Sube un archivo/documento para un trabajador.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:** `multipart/form-data`
  - `archivo`: archivo PDF
  - `idUSer`: number
  - `idtipoDoc?`: number | null
  - `tipoDoc?`: string | null
- **Respuesta 201:**
```json
{
  "IdDocumento": 5,
  "NombreArchivo": "certificado.pdf",
  "UbicacionArchivo": "uploads/..."
}
```
- **Error 400:**
```json
{ "error": "Archivo no válido" }
```

---

### `GET /Api/Usuarios/VerArchivo/{id}/{tipo}`
Descarga un archivo de trabajador.

- **Auth:** `PersonalAutorizado`
- **Parámetros ruta:** `id` (number), `tipo` (number)
- **Respuesta 200:** Archivo PDF (`Content-Type: application/pdf`)
- **Error 400:**
```json
{ "error": "No se encontró el archivo" }
```

---

### `POST /Api/Usuarios/AgregarCarrera`
Asigna una carrera universitaria a un trabajador.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "idUsuario": 1, "idCarrera": 3 }
// o
{ "idUsuario": 1, "Carrera": "Ingeniería de Sistemas" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Carrera agregada" }
```
- **Error 400:**
```json
{ "error": "El usuario no existe" }
```

---

# 2. Servicios — `/Api/Servicios`

## Tipos de datos (TypeScript)

```typescript
interface AnadirServicio {
  IdCliente: number;
  calle: string;
  NumeroCasa: number;
  IdZona: number;
  IdTipoServicio: number;
  Fechainicio: string;       // "yyyy-MM-dd"
  FechaFinal: string;        // "yyyy-MM-dd"
  costo: number;
  Descripcion: string;
}

interface AsignarMaquinariaServicios {
  IdServicio: number;
  IdMaquinaria: number;
  CantidadMaquinaria: number;
  DescripcionMaquinaria: string;
}

interface AsignarRecursoServicios {
  idServicio: number;
  IdRecurso: number;
  CantidadRecursos: number;
}

interface ListarServicio {
  IdServicio: number;
  Cliente: string;
  Direccion: string;
  TipoServicio: string;
  FechaInicio: string;       // "yyyy-MM-dd"
  FechaFinal: string | null; // "yyyy-MM-dd"
  costo: number;
}

interface InfoServicio {
  IdServicio: number;
  NombreEmpresa: string;
  NombreCliente: string;
  Contacto: string | null;
  NumeroCasa: number;
  Calle: string;
  Zona: string;
  TipoServicio: string;
  FechaInicio: string;
  FechaFinal: string | null;
  Costo: number;
  Descripcion: string | null;
  Create_at: string;          // ISO datetime
}

interface AsignarUsuariosServicios {
  idUsuario: number;
  IdServicio: number;
  idHorario: number;
  idDiasLaborales: number;
  HoraDeEntrada?: string | null;   // "HH:mm"
  HoraDeSalida?: string | null;    // "HH:mm"
  DiasLaborales?: string | null;
}
```

---

### `POST /Api/Servicios/Nuevo`
Crea un nuevo servicio.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{
  "IdCliente": 3,
  "calle": "Av. Principal",
  "NumeroCasa": 150,
  "IdZona": 4,
  "IdTipoServicio": 2,
  "Fechainicio": "2025-06-01",
  "FechaFinal": "2025-12-31",
  "costo": 15000.00,
  "Descripcion": "Servicio de limpieza general"
}
```
- **Respuesta 200:**
```json
{ "mensaje": "Servicio y asignaciones creados correctamente" }
```
- **Error 400:**
```json
{ "error": "Datos inválidos" }
```

---

### `GET /Api/Servicios/`
Lista de todos los servicios.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  {
    "IdServicio": 1,
    "Cliente": "Empresa ABC",
    "Direccion": "Av. Principal #150, Zona Central",
    "TipoServicio": "Limpieza",
    "FechaInicio": "2025-06-01",
    "FechaFinal": "2025-12-31",
    "costo": 15000.00
  }
]
```

---

### `GET /Api/Servicios/{id}`
Información detallada de un servicio.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `id` (number)
- **Respuesta 200:**
```json
{
  "IdServicio": 1,
  "NombreEmpresa": "ABC SRL",
  "NombreCliente": "María García",
  "Contacto": "maria@mail.com",
  "NumeroCasa": 150,
  "Calle": "Av. Principal",
  "Zona": "Central",
  "TipoServicio": "Limpieza",
  "FechaInicio": "2025-06-01",
  "FechaFinal": "2025-12-31",
  "Costo": 15000.00,
  "Descripcion": "Servicio de limpieza",
  "Create_at": "2025-05-20T14:30:00"
}
```
- **Error 404:**
```json
{ "mensaje": "No se encontró el servicio con ID 1" }
```

---

### `POST /Api/Servicios/Asignar-empleado`
Asigna un empleado a un servicio con horario.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{
  "idUsuario": 5,
  "IdServicio": 1,
  "idHorario": 2,
  "idDiasLaborales": 3,
  "HoraDeEntrada": "08:00",
  "HoraDeSalida": "17:00",
  "DiasLaborales": "Lunes a Viernes"
}
```
- **Respuesta 200:**
```json
{ "mensaje": "Empleado asignado al servicio exitosamente" }
```

---

### `POST /Api/Servicios/asignar-maquinaria`
Asigna maquinaria a un servicio.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{
  "IdServicio": 1,
  "IdMaquinaria": 3,
  "CantidadMaquinaria": 2,
  "DescripcionMaquinaria": "Taladros eléctricos"
}
```
- **Respuesta 200:**
```json
{ "mensaje": "Maquinaria asignada correctamente" }
```

---

### `POST /Api/Servicios/asignar-recurso`
Asigna recursos/materiales a un servicio.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{
  "idServicio": 1,
  "IdRecurso": 4,
  "CantidadRecursos": 10
}
```
- **Respuesta 200:**
```json
{ "mensaje": "Recurso asignado correctamente" }
```

---

### `GET /Api/Servicios/exportar-csv`
Exporta servicios a CSV.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:** Archivo CSV (`Content-Type: text/csv`, nombre: `Reporte_Servicios_YYYYMMDD.csv`)
- **Error 404:**
```json
{ "mensaje": "No hay datos disponibles para exportar." }
```

---

# 3. Maquinaria — `/Api/Maquinaria`

## Tipos de datos (TypeScript)

```typescript
interface ListarMaquinaria {
  IdMaquinaria: number;
  NombreMaquinaria: string;
  CodigoInventario: string;
  TipoMaquinaria: string;
}

interface ProvedorInfo {
  Nombre: string;
  Empresa: string;
  NIT: number;
}

interface MaquinariaMarca {
  NombreMarca: string;
  Pais: string;
}

interface InfoMaquinaria {
  IdMaquinaria: number;
  NombreMaquinaria: string;
  CodigoInventario: string;
  Provedor: ProvedorInfo;
  TipoMaquinaria: string;
  EstadoCalidad: string;
  Marca: MaquinariaMarca;
  Descripcion: string | null;
}

interface AgregarMaquinaria {
  NombreMaquinaria: string;
  CodigoInv: string;
  IdProvedor: number;
  TipoMaquinaria: number;
  EstadoCalidad: number;
  IdMarcaMaquinaria: number;
  Descripcion: string;
}

interface AgregarMarcaMaquinaria {
  IdPais: number;
  NombreMarca: string;
}

interface MostrarMarcas {
  IdMarca: number;
  Pais: string;
  NombreMarca: string;
}

interface Estado {
  IdEstado: number;
  estado: string;
}

interface InfoResumidaMaquinaria {
  NombreMAquinaria: string;
  Marca: string;
  Descripcion: string | null;
}
```

---

### `GET /Api/Maquinaria/`
Lista de toda la maquinaria.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  {
    "IdMaquinaria": 1,
    "NombreMaquinaria": "Taladro Industrial",
    "CodigoInventario": "TAL-001",
    "TipoMaquinaria": "Eléctrica"
  }
]
```

---

### `GET /Api/Maquinaria/{id}`
Información detallada de una maquinaria.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `id` (number)
- **Respuesta 200:**
```json
{
  "IdMaquinaria": 1,
  "NombreMaquinaria": "Taladro Industrial",
  "CodigoInventario": "TAL-001",
  "Provedor": { "Nombre": "Carlos", "Empresa": "Tool SAC", "NIT": 12345 },
  "TipoMaquinaria": "Eléctrica",
  "EstadoCalidad": "Bueno",
  "Marca": { "NombreMarca": "Bosch", "Pais": "Alemania" },
  "Descripcion": "Taladro percutor profesional"
}
```
- **Error 404:**
```json
{ "mensaje": "No se encontró maquinaria con ID 1" }
```

---

### `POST /Api/Maquinaria/`
Registra una nueva maquinaria.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{
  "NombreMaquinaria": "Compresor de Aire",
  "CodigoInv": "COM-002",
  "IdProvedor": 3,
  "TipoMaquinaria": 2,
  "EstadoCalidad": 1,
  "IdMarcaMaquinaria": 5,
  "Descripcion": "Compresor industrial 50L"
}
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

### `GET /Api/Maquinaria/marcas`
Lista de marcas de maquinaria.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  { "IdMarca": 1, "Pais": "Alemania", "NombreMarca": "Bosch" }
]
```

---

### `POST /Api/Maquinaria/marcas`
Registra una nueva marca.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "IdPais": 2, "NombreMarca": "Makita" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

### `GET /Api/Maquinaria/estados`
Lista de estados de calidad.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  { "IdEstado": 1, "estado": "Bueno" },
  { "IdEstado": 2, "estado": "Regular" }
]
```

---

### `GET /Api/Maquinaria/Short{id}`
Información resumida de una maquinaria (público).

- **Auth:** público
- **Parámetro ruta:** `id` (number) — concatenado sin slash: `/Api/Maquinaria/Short5`
- **Respuesta 200:**
```json
{
  "NombreMAquinaria": "Taladro",
  "Marca": "Bosch",
  "Descripcion": "Taladro percutor"
}
```
- **Error 404:**
```json
{ "mensaje": "No se encontró maquinaria con ID 5" }
```

---

# 4. Productos — `/Api/Productos`

## Tipos de datos (TypeScript)

```typescript
interface AnadirRecurso {
  IdProvedor: number;
  IdTipo: number;
  nombre: string;
  Descripcion: string | null;
}

interface ListarRecurso {
  NombreProvedor: string;
  EmpresaProvedor: string;
  Tipo: string;
  Nombre: string;
  Descripcion: string | null;
}

interface EditarNombre {
  IdRecurso: number;
  nombre: string;
}

interface EditarDescripcion {
  IdRecurso: number;
  Descripcion: string;
}
```

---

### `GET /Api/Productos/`
Lista de productos/recursos (público).

- **Auth:** público
- **Respuesta 200:**
```json
[
  {
    "NombreProvedor": "Proveedor ABC",
    "EmpresaProvedor": "ABC SRL",
    "Tipo": "Insumo",
    "Nombre": "Cloro Líquido",
    "Descripcion": "Galón 5L"
  }
]
```

---

### `POST /Api/Productos/`
Registra un nuevo recurso/producto.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{
  "IdProvedor": 2,
  "IdTipo": 1,
  "nombre": "Escoba Industrial",
  "Descripcion": "Escoba de cerdas duras"
}
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

### `PATCH /Api/Productos/editar/nombre`
Edita el nombre de un recurso.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "IdRecurso": 3, "nombre": "Escoba Pro" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

### `PATCH /Api/Productos/editar/descripcion`
Edita la descripción de un recurso.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "IdRecurso": 3, "Descripcion": "Escoba de cerdas suaves" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

# 5. Provedores — `/Api/Proveedores`

## Tipos de datos (TypeScript)

```typescript
interface ListarProvedores {
  id: number;
  Empresa: string;
  Nombre: string;
  Telefono: number[];
}

interface InformacionProvedor {
  Empresa: string;
  Productos: IdmasNombre[] | null;
  Nit: number;
  nombre: string;
}

interface AnadirProvedor {
  IDEmpresa: number;
  NIT: number;
  nombre: string;
}

interface AgregarTelefonoProvedor {
  telefono: number;
  idDetalle: number;
  Detalle: string | null;
  IdProveedor: number;
}

interface IdmasNombre {
  id: number;
  norbre: string;
}
```

---

### `GET /Api/Proveedores/`
Lista de proveedores.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  {
    "id": 1,
    "Empresa": "Distribuidora XYZ",
    "Nombre": "Carlos López",
    "Telefono": [77123456, 77123457]
  }
]
```

---

### `GET /Api/Proveedores/{id}`
Información detallada de un proveedor.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `id` (number)
- **Respuesta 200:**
```json
{
  "Empresa": "Distribuidora XYZ",
  "Productos": [{ "id": 1, "nobre": "Cloro" }],
  "Nit": 123456,
  "nombre": "Carlos López"
}
```
- **Error 404:**
```json
{ "mensaje": "No se encontró el proveedor con ID 1" }
```

---

### `POST /Api/Proveedores/`
Registra un nuevo proveedor.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "IDEmpresa": 3, "NIT": 654321, "nombre": "María Rojas" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

### `POST /Api/Proveedores/telefono`
Agrega un teléfono a un proveedor.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "telefono": 77123458, "idDetalle": 2, "Detalle": "Móvil", "IdProveedor": 1 }
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

### `PATCH /Api/Proveedores/nombre`
Edita el nombre de un proveedor.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "id": 1, "nobre": "Nuevo Nombre" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito" }
```

---

# 6. Trabajadores — `/Api/Trabajadores`

## Tipos de datos (TypeScript)

```typescript
interface ListarRoles {
  id: number;
  nombre: string;
  salario: number;
}

interface AnadirTelefonoTrabajadores {
  telefono: number;
  idUsuario: number;
  idDetalle: number | null;
  Detalle: string | null;
}

interface VerInfoUsuario {
  id: number;
  estadocivil: string;
  gradoacademico: string;
  genero: string;
  direccion: string;
  Rol: string;
  pais: string;
  correo: string;
  ci: number;
  nombre: string;
  fechanacimiento: string;   // "yyyy-MM-dd"
  ServicioAsignado: boolean;
}

interface VerInfoUsuarioId {
  id: number;
  estadocivil: string;
  gradoacademico: string;
  genero: string;
  direccion: string;
  Rol: string;
  pais: string;
  correo: string;
  ci: number;
  nombre: string;
  fechanacimiento: string;
  ServicioAsignado: boolean;
  Carreras: string[];
  telefonos: number[];
  consultarDocumentos: string;  // URL para documentos
}
```

---

### `GET /Api/Trabajadores/{ci}`
Información detallada de un trabajador por CI.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `ci` (number)
- **Respuesta 200:**
```json
{
  "id": 1,
  "estadocivil": "Soltero",
  "gradoacademico": "Licenciatura",
  "genero": "Masculino",
  "direccion": "Av. Central #123, Zona 1",
  "Rol": "Gerente",
  "pais": "Bolivia",
  "correo": "juan@mail.com",
  "ci": 12345678,
  "nombre": "Juan Pérez",
  "fechanacimiento": "1990-05-15",
  "ServicioAsignado": true,
  "Carreras": ["Ingeniería Industrial"],
  "telefonos": [77123456, 77123457],
  "consultarDocumentos": "/Api/Usuario/VerArchivo/{id}/{tipo}"
}
```
- **Error 404:**
```json
{ "mensaje": "Algo salio mal" }
```

---

### `GET /Api/Trabajadores/roles`
Lista de roles disponibles con salario.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  { "id": 1, "nombre": "Gerente", "salario": 5000 },
  { "id": 2, "nombre": "Administrador", "salario": 4000 }
]
```
- **Error 404:**
```json
{ "mensaje": "Algo salio mal" }
```

---

### `POST /Api/Trabajadores/telefonos`
Agrega un teléfono a un trabajador.

- **Auth:** `PersonalAutorizado`
- **Cuerpo:**
```json
{ "telefono": 77123459, "idUsuario": 1, "idDetalle": 2, "Detalle": "Móvil" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Operación realizada con éxito." }
```

---

### `GET /Api/Trabajadores/exportar-csv`
Exporta trabajadores a CSV.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:** Archivo CSV (`Content-Type: text/csv`, nombre: `Reporte_Trabajadores_YYYYMMDD.csv`)
- **Error 404:**
```json
{ "mensaje": "No hay datos disponibles para exportar." }
```

---

# 7. Reportes — `/Api/Reportes`

## Tipos de datos (TypeScript)

```typescript
interface AddIncidente {
  descripcion: string;
  fecha: string;          // "yyyy-MM-dd"
}

interface ListaIncidente {
  IdIncidente: number;
  NombreCliente: string;
  fecha: string;          // "yyyy-MM-dd"
}

interface InfoIncidente {
  IdIncidente: number;
  NombreCliente: string;
  Empresa: string;
  DireccionServicio: string;
  ContectoEmergencia: string | null;
  Telefonos: TelefonosCliente[];
  TipoServicio: string;
  descripcion: string;
  fecha: string;
}

interface TelefonosCliente {
  telefono: number;
  descripcion: string;
}

interface AddMemorandum {
  IdTrabajador: number;
  Descripcion: string;
}

interface NotificarIncidente {
  idIncidente: number;
  descripcionResumina: string;
  IdServicio: number;
}
```

---

### `POST /Api/Reportes/incidentes`
Reporta un incidente (solo clientes).

- **Auth:** `Cliente`
- **Cuerpo:**
```json
{ "descripcion": "Fuga de agua en el baño principal", "fecha": "2025-06-15" }
```
- **Respuesta 201:** Objeto `Incidente` con `IdIncidente` asignado
- **Error 400:**
```json
{ "error": "Descripción requerida" }
```

---

### `GET /Api/Reportes/incidentes`
Lista resumida de incidentes.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  { "IdIncidente": 1, "NombreCliente": "Empresa ABC", "fecha": "2025-06-15" }
]
```

---

### `GET /Api/Reportes/incidentes/{id}`
Información detallada de un incidente.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `id` (number)
- **Respuesta 200:**
```json
{
  "IdIncidente": 1,
  "NombreCliente": "Empresa ABC",
  "Empresa": "ABC SRL",
  "DireccionServicio": "Zona Central, Calle Bolívar N° 150",
  "ContectoEmergencia": "contacto@abc.com",
  "Telefonos": [{ "telefono": 77123456, "descripcion": "Móvil" }],
  "TipoServicio": "Limpieza",
  "descripcion": "Fuga de agua en el baño",
  "fecha": "2025-06-15"
}
```
- **Error 404:**
```json
{ "Mensaje": "No se encontró el incidente con ID 1" }
```

---

### `POST /Api/Reportes/memorandums`
Crea un memorando para un trabajador.

- **Auth:** `Gerente`
- **Cuerpo:**
```json
{ "IdTrabajador": 5, "Descripcion": "LLamada de atención por retrasos" }
```
- **Respuesta 201:** Objeto `Memorial` con `IdMemorial` asignado
- **Error 400:**
```json
{ "error": "El trabajador no existe" }
```

---

### `GET /Api/Reportes/memorandums/{id}/pdf`
Descarga el PDF de un memorando.

- **Auth:** cualquier usuario autenticado
- **Parámetro ruta:** `id` (number)
- **Respuesta 200:** Archivo PDF (`Content-Type: application/pdf`, nombre: `Memorando_{id}.pdf`)
- **Error 404:**
```json
{ "error": "No se encontró el registro del memorando" }
```

---

# 8. Clientes — `/Api/Clientes`

## Tipos de datos (TypeScript)

```typescript
interface ClienteLogin {
  correo: string;
  contraseña: string;
}

interface Cliente2FA {
  correo: string;
  Codigo: string;
}

interface InfoCliente {
  IdCliente: number;
  Empresa: string;
  nombreCliente: string;
  Direccion: string;
  correo: string | null;
  contraseña: string;        // hash bcrypt
  nit: number;
}

interface InfoClienteCorto {
  IdCliente: number;
  nombreCliente: string;
  nit: number;
}

interface AnadirCliente {
  nombreCliente: string;
  calle: string;
  ncasa: number;
  correo: string;
  contraseña: string;
  nit: number;
  idEmpresa?: number | null;
  empresa?: string | null;
  idZona?: number | null;
  Zona?: string | null;
}
```

---

### `POST /Api/Clientes/solicitar-2fa`
Inicia sesión como cliente enviando código 2FA al correo de contacto.

- **Auth:** público
- **Cuerpo:**
```json
{ "correo": "cliente@mail.com", "contraseña": "miClave" }
```
- **Respuesta 200:**
```json
{ "mensaje": "Código enviado al correo" }
```
- **Error 401:**
```json
{ "mensaje": "Credenciales incorrectas." }
```

---

### `POST /Api/Clientes/verificar-2fa`
Verifica el código 2FA y recibe el token en cookie.

- **Auth:** público
- **Cuerpo:**
```json
{ "correo": "cliente@mail.com", "Codigo": "482931" }
```
- **Respuesta 200:** Establece cookie `token_sesion` + 
```json
{ "mensaje": "Autenticación exitosa" }
```
- **Error 400:**
```json
{ "error": "Codigo expiro." }
```
- **Error 404:**
```json
{ "error": "El cliente no tiene un 2FA activo" }
```

---

### `GET /Api/Clientes/`
Lista resumida de clientes.

- **Auth:** `PersonalAutorizado`
- **Respuesta 200:**
```json
[
  { "IdCliente": 1, "nombreCliente": "Empresa ABC", "nit": 123456 }
]
```

---

### `GET /Api/Clientes/{id}`
Información detallada de un cliente.

- **Auth:** `PersonalAutorizado`
- **Parámetro ruta:** `id` (number)
- **Respuesta 200:**
```json
{
  "IdCliente": 1,
  "Empresa": "ABC SRL",
  "nombreCliente": "María García",
  "Direccion": "Zona Central, Calle Bolívar N° 150",
  "correo": "cliente@mail.com",
  "contraseña": "$2a$11$...",    // hash bcrypt
  "nit": 123456
}
```
- **Error 404:**
```json
{ "mensaje": "No se encontró el cliente con ID 1" }
```

---

### `POST /Api/Clientes/`
Registra un nuevo cliente.

- **Auth:** público
- **Cuerpo:**
```json
{
  "nombreCliente": "Empresa ABC",
  "calle": "Av. Principal",
  "ncasa": 500,
  "correo": "cliente@mail.com",
  "contraseña": "claveSegura",
  "nit": 654321,
  "idZona": 3,
  "empresa": "ABC SRL"
}
```
- **Respuesta 201:** Objeto `Cliente` creado (location: `/Api/Clientes/{IdCliente}`)
- **Error 400:**
```json
{ "error": "Datos mal ingresados falta empresa" }
```

---

# 9. Endpoints globales (Program.cs)

---

### `GET /Api/Catalogos/{nombre}`
Obtiene subdominios de un catálogo por nombre de dominio. Sirve para llenar dropdowns (roles, zonas, países, etc.).

- **Auth:** público
- **Parámetro ruta:** `nombre` (string) — ej: `"Rol"`, `"Zona"`, `"Genero"`, `"Pais"`
- **Respuesta 200:**
```json
[
  { "Id": 1, "Detalle": "Gerente" },
  { "Id": 2, "Detalle": "Administrador" }
]
```
- **Error 404:**
```json
{ "mensaje": "El catálogo 'Rol' no existe." }
```

**Uso desde frontend:**
```javascript
fetch("/Api/Catalogos/Rol")
  .then(r => r.json())
  .then(data => /* poblar <select> con { id, detalle } */);
```

---

### `GET /ws` — WebSocket
Conexión WebSocket para notificaciones en tiempo real.

- **Auth:** cualquier usuario autenticado (cookie `token_sesion`)
- **Request:** HTTP Upgrade a WebSocket
- **Comportamiento:**
  1. Extrae `userId` y `role` del JWT
  2. Registra la conexión en el servidor por usuario+rol
  3. Mantiene la conexión abierta escuchando mensajes (no procesa mensajes entrantes)
  4. Al cerrarse, elimina la conexión del registro

**Ejemplo frontend:**
```javascript
const ws = new WebSocket("wss://localhost:7102/ws");
ws.onopen = () => console.log("Conectado");
ws.onmessage = (event) => {
  const data = JSON.parse(event.data);
  // manejar notificación
};
```

**Nota:** El servidor emite notificaciones a grupos de rol (ej: `"Gerente"`) cuando ocurren eventos como nuevos incidentes.

---

### `GET /api/diagnostico-final` (solo DEBUG)
Endpoint de diagnóstico para verificar autenticación y claims del JWT.

- **Auth:** cualquier usuario autenticado (disponible solo en compilación Debug)
- **Respuesta 200:**
```json
{
  "Autenticado": true,
  "Esquema": "Bearer",
  "Roles": ["Gerente"],
  "EsGerente": true,
  "ClaimsCompletos": [
    { "Type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "Value": "1" },
    { "Type": "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Value": "Gerente" }
  ]
}
```

---

# Resumen de autenticación por endpoint

| Endpoint | Auth |
|----------|------|
| `POST /solicitar-2fa` (Usuarios y Clientes) | 🔓 público |
| `POST /verificar-2fa` (Usuarios y Clientes) | 🔓 público |
| `POST /Api/Clientes/` | 🔓 público |
| `GET /Api/Catalogos/{nombre}` | 🔓 público |
| `GET /Api/Maquinaria/Short{id}` | 🔓 público |
| `GET /Api/Productos/` | 🔓 público |
| `GET /api/diagnostico-final` | 🔓 público (solo Debug) |
| `POST /Api/Reportes/incidentes` | 🔐 rol `Cliente` |
| `POST /Api/Reportes/memorandums` | 🔐 rol `Gerente` |
| `GET /ws` | 🔐 cualquier autenticado |
| `GET /Api/Reportes/memorandums/{id}/pdf` | 🔐 cualquier autenticado |
| Todos los demás endpoints | 🔐 `PersonalAutorizado` (Gerente o Administrador) |

---

# Notas para frontend

1. **Cookie automática:** El token se entrega como cookie HttpOnly. El navegador la envía automáticamente en cada request si `credentials: "include"` está configurado en fetch/axios.

2. **fetch con credenciales:**
```javascript
fetch("/Api/Servicios/", { credentials: "include" })
  .then(r => r.json())
  .then(console.log);
```

3. **axios con credenciales:**
```javascript
axios.get("/Api/Servicios/", { withCredentials: true });
```

4. **URL base:** `http://localhost:5112` (http) o `https://localhost:7102` (https). CORS solo permite `http://localhost:3000`.

5. **Fechas:** El backend usa `DateOnly` en C#. Enviar como string `"yyyy-MM-dd"` en requests. Las respuestas devuelven el mismo formato.

6. **CSV:** Los endpoints de exportación devuelven `Content-Type: text/csv`. Usar `response.blob()` para descargar.

7. **PDF:** Los endpoints de archivos devuelven `Content-Type: application/pdf`. Usar `response.blob()` para visualizar o descargar.

8. **WebSocket:** Conectar con `wss://localhost:7102/ws` (o `ws://localhost:5112/ws`). La cookie `token_sesion` se envía automáticamente durante el handshake.
