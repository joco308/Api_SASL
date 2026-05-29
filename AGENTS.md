# Api_SASL — agent guidance

## Build & run
```bash
dotnet build
dotnet watch          # hot-reload dev at http://localhost:5112
dotnet run            # single-run
```
No tests, lint, typecheck, CI, or pre-commit hooks exist.

## Key dependencies
- **BCrypt.Net-Next** — password hashing
- **QuestPDF** — PDF generation (`LicenseType.Community` at `Program.cs:37`)
- **CsvHelper** — CSV export
- **JwtBearer** — auth via `token_sesion` cookie
- **EF Core + SQL Server** — `DevSaslContext`

## Architecture
- **Minimal APIs** (no Controllers) — each module registers via `Map*Endpoints(this IEndpointRouteBuilder)` extension, wired at `Program.cs:162-169`
- **Result pattern** — logic returns `IResultadoServicio` records (`Success`, `NotFound`, `ValidationError`, `SuccessWithToken`, `Created<T>`, `SuccessM`, `docCreated`); endpoints `switch` on it. See `Servicios/InterfazServicios/IResultadosServicio.cs`
- Cross-cutting services in `Servicios/`: Email (SMTP), WebSocket, `SmtpSettings`, `TokenConfiguracion`
- Logic method names are **Spanish** (e.g. `añadirServicioNuevoAsync`, `listarMaquinariasAsync`)
- **Full endpoint catalog** at `doc/DocumentacionEnpoints.md` (frontend-ready, includes TypeScript types)

## Module structure
Every module under `Modulos/` has `DTO/`, `Endpoints/`, `Interfaz/`, `Logica/`.

**Active** (8): Usuarios, Servicios, Maquinaria, Productos, Provedores, Trabajadores, Reportes, **Clientes**  
**Empty scaffolds** (2): Administradores, Cobros — directories exist, no `.cs` files

## Auth
- JWT delivered via `token_sesion` cookie (HttpOnly, Secure, SameSite=Strict, 8h expiry)
- **Login flow**: `POST /Api/Usuarios/solicitar-2fa` (email+password → 2FA code to email) → `POST /Api/Usuarios/verificar-2fa` (code → cookie)
- **Client login**: same flow at `POST /Api/Clientes/solicitar-2fa` + `POST /Api/Clientes/verificar-2fa` (JWT includes role `Cliente`)
- Policies: `PersonalAutorizado` (Gerente/Admin, `Program.cs:128-129`), `Cliente` (`Program.cs:130-131`)
- `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear()` at `Program.cs:43` (preserves original JWT claim types)
- WebSocket at `/ws` requires auth; connections tracked by user ID + role in `WebSocketGestor`

## Notable quirks
- CORS allows only `http://localhost:3000` (policy `PermitirNEXTJS`, credentials allowed, `Program.cs:50-58`)
- DB connection string at `appsettings.json:9-11` (SQL Server `dev_SASL`)
- Config sections: `SmtpSettings` (Gmail SMTP), `Jwt` (Key, Issuer, Audience)
- `GET /api/diagnostico-final` exists only in `#if DEBUG` (`Program.cs:231-243`)
- `GET /Api/Catalogos/{nombre}` is public (no auth), returns subdomain catalog by domain name — inline in `Program.cs:172-182` (not a separate module)
- PDF logo at `wwwroot/images/logo.png` (used in `ReportesLogica.GenerarMemorandoAsync`)
