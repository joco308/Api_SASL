# Api_SASL — agent guidance

## Environment (read first)
- **OpenCode runs in WSL, but this .NET project lives and executes on Windows** (`/mnt/c/...`). The Windows host has `dotnet`, SQL Server, and all other dependencies installed; WSL does not.
- **Do not try to run `dotnet build`/`dotnet run`/`dotnet test` or any other tool from this shell** — `dotnet` is not on PATH in WSL and there is nothing to fall back to. Don't `apt install`/`winget`/`dotnet-install.sh` either: installing it inside WSL does not make it available to the actual project on Windows, and you'd be polluting the WSL environment for nothing.
- Treat the codebase as **read-mostly from this agent's perspective**: you can read, search, and edit files, but verification (build, run, test) must be performed by the user on Windows. If a task needs verification, **ask the user to run it** and tell them the exact command.

## Stack
- .NET 10 (`net10.0`, nullable + implicit usings enabled) — see `Api_SASL.csproj:4-6`
- ASP.NET Core **Minimal APIs** (no Controllers); see `Program.cs:162-169` for module wiring

## Build & run
```bash
dotnet build
dotnet watch          # hot-reload dev at http://localhost:5112
dotnet run            # single-run (HTTPS profile also at https://localhost:7102 per launchSettings.json)
```
No tests, lint, typecheck, CI, or pre-commit hooks exist.

## Key dependencies
- **BCrypt.Net-Next** — password hashing
- **QuestPDF** — PDF generation (`LicenseType.Community` at `Program.cs:40`)
- **CsvHelper** — CSV export
- **JwtBearer** — auth via `token_sesion` cookie
- **EF Core + SQL Server** — `DevSaslContext` (no migrations folder is committed; schema is managed externally)

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
- Policies: `PersonalAutorizado` (Gerente/Administrador, `Program.cs:129-130`), `Cliente` (`Program.cs:131-132`)
- `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear()` at `Program.cs:43` (preserves original JWT claim types)
- WebSocket at `/ws` requires auth; connections tracked by user ID + role in `WebSocketGestor`

## Database
- Connection string at `appsettings.json:9-11` → SQL Server `dev_SASL` hosted on a **LAN IP** (`192.168.100.12`), not localhost. Only reachable from the dev LAN; expect connection failures if you try to run from elsewhere.
- **No `Migrations/` folder is committed** — schema is managed externally. Don't run `dotnet ef migrations add` or `database update` without coordinating; it will drift from whatever produces the live schema.

## Notable quirks
- CORS allows only `http://localhost:3000` (policy `PermitirNEXTJS`, credentials allowed, `Program.cs:49-62`)
- Config sections: `SmtpSettings` (Gmail SMTP, `appsettings.json:12-17`), `Jwt` (Key, Issuer, Audience, `appsettings.json:18-22`)
- `GET /api/diagnostico-final` exists only in `#if DEBUG` (`Program.cs:233-245`) — dumps the current `ClaimsPrincipal` for debugging auth
- `GET /Api/Catalogos/{nombre}` returns a subdomain catalog by domain name — inline in `Program.cs:174-184` (not a module). It is **auth-required** (`.RequireAuthorization()`), and routes through `IUsuariosLogica.ObtenerCatalogoPorDominioAsync`, so the catalog logic lives in the `Usuarios` module.
- PDF logo at `wwwroot/images/logo.png` (used in `ReportesLogica.GenerarMemorandoAsync`)
- **Dev secrets are committed in `appsettings.json`** (DB password, Gmail app password, JWT signing key). Don't "clean up" by deleting them or moving to env vars without confirming with the owner — they're how `dotnet run` works out of the box.
