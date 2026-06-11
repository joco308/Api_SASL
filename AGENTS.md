# Api_SASL — agent guidance

## Environment (read first)
- **OpenCode runs in WSL; .NET project lives on Windows.** `dotnet` is not on WSL PATH. Do not try to build/run/test from this shell, and do not install .NET inside WSL.
- Treat the codebase as **read-mostly**: you can search and edit, but verification (build, run, test) must be performed by the user on Windows. If a task needs verification, ask the user to run it and provide the exact command.

## Stack
- .NET 10 (`net10.0`, nullable + implicit usings enabled)
- ASP.NET Core **Minimal APIs** (no Controllers). Modules wire at `Program.cs:170-178`.

## Build & run (on Windows)
```
dotnet build
dotnet watch          # hot-reload at http://localhost:5112 / https://localhost:7102
dotnet run
```
Solution: `Api_SASL.sln`. No tests, lint, typecheck, CI, or pre-commit hooks.

## Key dependencies
`BCrypt.Net-Next`, `QuestPDF` (`LicenseType.Community`), `CsvHelper`, `JwtBearer`, `EF Core + SQL Server`

## Architecture
- **Minimal APIs** — every module registers via `Map*Endpoints(this IEndpointRouteBuilder)` at `Program.cs:170-178`.
- **Result pattern** — logic returns `IResultadoServicio` records (`Success`, `NotFound`, `ValidationError`, `SuccessWithToken`, `Created<T>`, `SuccessM`, `docCreated`); endpoints `switch` on them. Defined at `Servicios/InterfazServicios/IResultadosServicio.cs`.
- Cross-cutting services in `Servicios/`: Email (SMTP), `WebSocketGestor`, `SmtpSettings`, `TokenConfiguracion`.
- **Method names are Spanish** (e.g. `añadirServicioNuevoAsync`, `listarMaquinariasAsync`).
- **Full endpoint catalog** at `doc/DocumentacionEnpoints.md` (TypeScript types included).

## Module structure
Every module under `Modulos/` has `DTO/`, `Endpoints/`, `Interfaz/`, `Logica/`.

**Active** (9): Usuarios, Servicios, Maquinaria, Productos, Provedores, Trabajadores, Reportes, Clientes, Cobros.
**Empty scaffold** (1): Administradores — directory skeleton, no `.cs` files.

Note: endpoint file naming is inconsistent (`UsuariosEndpoints.cs`, `EndpointClientes.cs`, `EnpointTrabajadores.cs`).  
Module directory is `Provedores` (with 'd') but endpoint registration is `MapProveedoresEndpoints` (with 'dd') at `Program.cs:174`.

## Auth
- JWT via `token_sesion` cookie (HttpOnly, Secure, SameSite=Strict, 8h expiry).
- **Login**: `POST /Api/Usuarios/solicitar-2fa` (email+password → email code) → `POST /Api/Usuarios/verificar-2fa` (code → cookie).
- **Client login**: same flow at `POST /Api/Clientes/solicitar-2fa` + `/verificar-2fa` (JWT role: `Cliente`).
- Policies: `PersonalAutorizado` (Gerente/Administrador, `Program.cs:134-135`), `Cliente` (`Program.cs:136-137`), `Trabajador` (`Program.cs:138-139`).
- `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear()` at `Program.cs:46`.
- WebSocket at `/ws` (auth required); connections tracked by user ID + role in `WebSocketGestor`.

## Database
- Connection string at `appsettings.json:9-11` → SQL Server `dev_SASL` at **LAN IP** `192.168.100.12`. Not reachable outside dev LAN.
- **No `Migrations/` folder** — schema managed externally. Do not run `dotnet ef migrations`.

## Notable quirks
- CORS: only `http://localhost:3000` (policy `PermitirNEXTJS`, credentials allowed, `Program.cs:58-64`).
- Config sections: `SmtpSettings` (`appsettings.json:12-17`), `Jwt` (Key/Issuer/Audience, `appsettings.json:18-22`).
- `GET /api/diagnostico-final` exists only under `#if DEBUG` (`Program.cs:242-253`) — dumps `ClaimsPrincipal` for auth debugging.
- `GET /Api/Catalogos/{nombre}` is inline at `Program.cs:182-192` (not a module), auth-required, routes through `IUsuariosLogica.ObtenerCatalogoPorDominioAsync`.
- PDF logo at `wwwroot/images/logo.png` (used in `ReportesLogica.GenerarMemorandoAsync`).
- **Dev secrets committed** in `appsettings.json` (DB password, Gmail app password, JWT key). Do not delete or extract to env vars — they're how `dotnet run` works out of the box.
- Branches: `main`, `dev`, `dev_joco`.
