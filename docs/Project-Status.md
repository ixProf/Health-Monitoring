# Astronaut Health Monitoring System - Project Status Report

> **Generated**: 2026-11-09  
> **Target Framework**: .NET 9.0 (`net9.0`) | **Database**: PostgreSQL (Supabase) via EF Core 9.0.2 / Npgsql 9.0.3  
> **Solution Status**: Build Clean (0 Warnings, 0 Errors) | **Automated Tests**: 50 Passed, 0 Failed  

---

## 1. Overall Status per Area

| Area | Planned | Status | Evidence (Files, Tests, Verification) | Notes |
|------|---------|--------|---------------------------------------|-------|
| **Active Domain Entities** | 18 Core Entities for MVP telemetry, reference data, chat, and auth | **Done** | `Models/Entities/*.cs` (18 files: `ApplicationUser`, `Astronaut`, `ChatMessage`, `ConditionSymptom`, `ConditionVitalSign`, `EmergencyProcedure`, `HealthAlert`, `HealthMetricGuide`, `Mission`, `MoodEntry`, `Recommendation`, `ReferenceRange`, `RefreshToken`, `SleepRecord`, `SpaceCondition`, `Symptom`, `VitalSignReading`, `VitalSignType`). `Models/Common/BaseEntity.cs`, `Models/Enums/AppEnums.cs`. | Base properties, navigation collections, and domain enums fully aligned. |
| **Backlog Entities** | 21 Extended Telemetry / Medical Entities deferred to Phase 5+ | **Done (Archived)** | `Models/Backlog/Entities/*.cs` (21 files: `CommunicationStatus`, `CrewMember`, `DailyTask`, `EmergencyIncident`, `EnvironmentStatus`, `ExerciseLog`, `FatigueEntry`, `HealthReport`, `HydrationNutritionLog`, `MedicalCheckup`, `MedicalHistoryEntry`, `MedicalMessage`, `MedicalSupportRequest`, `Medication`, `PersonalBaseline`, `PhysicalActivity`, `PvtTestResult`, `RadiationDoseRecord`, `SimulationAttempt`, `SimulationScenario`, `SymptomLog`). | Excluded from compilation in `AstronautHealthMonitoring.csproj` via `<Compile Remove="Models/Backlog/**" />`. |
| **DbContext & Entity Configurations** | Fluent API mapping for all 18 tables + Identity tables | **Done** | `Data/AppDbContext.cs`, `Data/Configurations/*.cs` (17 configuration files). | `AppDbContext` inherits from `IdentityDbContext<ApplicationUser>`. Cascade/restrict/set-null deletion rules and indexes enforced. |
| **EF Core Migrations & Model Snapshot** | Schema migration pipeline in single root directory | **Done** | `Migrations/` (3 migrations: `20261107022248_InitialCreate`, `20261107033006_FixReferenceRangesAndTexts`, `20261109042856_AddIdentityAndRefreshTokens`). Model snapshot: `Migrations/AppDbContextModelSnapshot.cs`. Command: `dotnet tool run dotnet-ef migrations has-pending-model-changes` verified: *"No changes have been made to the model since the last migration."* | Single migration folder at project root. Namespace: `AstronautHealthMonitoring.Migrations`. Not yet applied to remote DB. |
| **Seed Data** | Seed catalogs, clinical guides, reference ranges, conditions, symptoms, and demo users | **Done** | `Data/Seed/ModelBuilderExtensions.cs` (seeded via `modelBuilder.Seed()` on `HasData`): 6 vital sign types, 6 baseline reference ranges, 6 metric guides, 6 emergency procedures, 14 symptoms, 6 space conditions, join tables. `Services/Implementations/IdentityDataSeeder.cs` (Development/flag only): 1 Mission (`ARES-III`), 3 demo Astronauts (`Morgan Vance`, `Jordan Lee`, `Taylor Quinn`), 1 Monitor (`Casey Rivera`). | Passwords and JWT keys must be supplied via user-secrets/environment variables. Fail-fast validation prevents startup with missing secrets when seeding is enabled. |
| **DTOs (Data Transfer Objects)** | Feature-organized request and response payloads | **Done** | `DTOs/` (44 files organized by feature: `Alerts`, `Astronaut`, `Auth`, `Chat`, `Common`, `Dashboard`, `Emergency`, `Guides`, `Missions`, `Mood`, `Recommendations`, `ReferenceRanges`, `Sleep`, `SymptomGuidance`, `Sync`, `VitalSigns`). | `AstronautId` strictly removed from client write request DTOs (resolved securely from JWT). Response DTOs retain `AstronautId`. |
| **Request Validators** | FluentValidation rules for client payloads, timestamps, and ranges | **Done** | `Validators/` (11 validator files). Registered in DI via `AddValidatorsFromAssembly`. | Enforces required fields, non-empty GUIDs, bounds, and configurable future-timestamp tolerance (default 5 min). |
| **Mapster Mapping Profiles** | Compile-safe object mapping replacing AutoMapper | **Done** | `Mapping/` (12 `IRegister` profile files). Configured in `Program.cs`. Verified by test: `MapsterConfigurationTests.Mapster_Configuration_IsValid_And_Compiles`. | Strictly ignores server-set properties (`Id`, `CreatedAt`, `DurationHours`, `AstronautId` on writes). All 5 mapping tests pass. |
| **Service Interfaces** | Async business logic contracts taking DTOs and `CancellationToken` | **Done** | `Services/Interfaces/` (14 interfaces: `IAstronautService`, `IAuthService`, `IChatService`, `ICurrentUserService`, `IHealthAlertService`, `IIdentityDataSeeder`, `IMissionService`, `IMoodService`, `IRecommendationService`, `ISleepService`, `ISymptomGuidanceService`, `ITokenService`, `IVitalSignService`, `IVitalSignStatusEvaluator`). | All methods use DTOs only and take explicit `astronautId` parameters for write endpoints. |
| **Service Implementations** | Core application business logic engines | **Partial** | Implemented: `VitalSignStatusEvaluator`, `AuthService`, `TokenService`, `CurrentUserService`, `IdentityDataSeeder`. Not yet implemented: `AstronautService`, `MissionService`, `VitalSignService`, `HealthAlertService`, `SleepService`, `MoodService`, `RecommendationService`, `ChatService`, `SymptomGuidanceService`. | Domain services intentionally held until the Service Interface Review step is approved. |
| **Authentication & Authorization** | ASP.NET Core Identity + JWT + Refresh Token Rotation with Grace Window | **Done** | `Services/Implementations/AuthService.cs`, `TokenService.cs`, `CurrentUserService.cs`. `Controllers/AuthController.cs`. Policies: `AstronautOnly`, `MonitorOnly`, `Authenticated`. Tests: `AuthServiceTests` (7 tests), `TokenServiceTests` (4 tests), `CurrentUserServiceTests` (3 tests), `AuthorizationPolicyTests` (6 tests). | Hex SHA-256 hashed refresh tokens. 30s grace window for retry on flaky links. Full session revocation on token reuse outside grace window. Rate limited login endpoint (5 req/min). |
| **Controllers (REST API)** | Web API HTTP endpoints | **Partial** | `Controllers/AuthController.cs` implemented (`POST /api/auth/login`, `POST /api/auth/refresh`, `POST /api/auth/logout`, `GET /api/auth/me`). Feature controllers: **NOT STARTED** (held per roadmap instruction). | Rate limited, generic error messages to eliminate user enumeration. |
| **Swagger / OpenAPI** | API schema discovery and interactive documentation | **Partial** | `builder.Services.AddOpenApi()`, `app.MapOpenApi()`. Accessible at `/openapi/v1.json`. | OpenAPI 3.0 schema generation works. Interactive SwaggerUI or Scalar UI package not yet installed. |
| **SignalR (Real-time)** | Live telemetry broadcast and ground-orbit chat streaming | **Partial** | Query-string access token authentication configured in `Program.cs` (`JwtBearerEvents` for `/hubs/*`). SignalR hubs: **NOT STARTED**. | Foundation for token handoff over WebSockets is in place; Hub classes pending. |
| **Offline Synchronization** | Idempotent bulk sync of vitals, sleep, mood, and chat | **Partial** | `DTOs/Sync/SyncBatchRequest.cs`, `SyncBatchResponse.cs`, `Validators/Sync/SyncValidators.cs`. Dedicated Sync service implementation: **NOT STARTED**. | Request contracts enforce `ClientGeneratedId` and `RecordedAt` on every telemetry item. |
| **Chat Module** | Unified astronaut thread with ground monitors | **Partial** | `Models/Entities/ChatMessage.cs`, `ChatMessageConfiguration.cs`, `DTOs/Chat/`, `ChatValidators.cs`, `ChatMappingProfile.cs`, `IChatService.cs`. Implementation: **NOT STARTED**. | Data contracts, thread key indexing, and read tracking ready. |
| **Symptom Guidance** | Space adaptation topic guidance (non-diagnostic) | **Partial** | `Symptom`, `SpaceCondition`, `ConditionSymptom`, `ConditionVitalSign` entities, configuration, seed data, DTOs, validators, mapping, and `ISymptomGuidanceService.cs`. Service implementation: **NOT STARTED**. | Non-diagnostic design (topic associations without percentages or diagnostic labels) preserved. |
| **Alerts & Evaluator Engine** | Rule-based vital sign status evaluation and alert generation | **Partial** | Evaluator Engine **Done**: `IVitalSignStatusEvaluator.cs`, `VitalSignStatusEvaluator.cs`. 23 unit tests in `VitalSignStatusEvaluatorTests.cs` pass. `IHealthAlertService.cs` interface defined. Implementation: **NOT STARTED**. | Pure, stateless status evaluator (Emergency vs Routine vs MedicalAttention). Dynamic thresholds loaded from DB reference ranges. |
| **Recommendations Engine** | Operational and clinical crew guidance | **Partial** | `Recommendation` entity, configuration, DTOs, validators, mapping profile, `IRecommendationService.cs`. Implementation: **NOT STARTED**. | Status lifecycle (Pending, Acknowledged, Completed, Dismissed) and reason tracking designed. |
| **Logging & Diagnostics** | Diagnostic inspection and operational logging | **Done** | `Program.cs` contains reverse-provider configuration inspector logging database host and provider source without credentials. Console logging enabled. | Centralized RFC 7807 ProblemDetails middleware not yet added. |
| **Automated Tests** | Unit tests for evaluator, mapping, identity, auth, and policies | **Done** | `tests/AstronautHealthMonitoring.Tests/` (50 tests across 7 test classes: `VitalSignStatusEvaluatorTests`, `MapsterConfigurationTests`, `TokenServiceTests`, `CurrentUserServiceTests`, `AuthorizationPolicyTests`, `AuthServiceTests`, `IdentityDataSeederTests`). Command: `dotnet test` (50 passed, 0 failed, duration: 5.78s). | Uses in-memory EF Core database and real seed data for clinical verification. |
| **Documentation** | System architecture, relational model, and status tracking | **Done** | `docs/Relationships.md` (complete relational analysis and ER diagram), `docs/Project-Status.md`, `README.md`. | Neutral monitor terminology verified; no realistic credentials present. |

---

## 2. What Remains to Finish the MVP (Execution Roadmap)

Based on the project roadmap, the following sequential implementation batches remain to achieve full MVP completion:

```
[Service Interface Review]
           │
           ▼
[Batch A: Vitals & Alerts] ───► IVitalSignService + IHealthAlertService
           │
           ▼
[Batch B: Core Telemetry]  ───► Dashboard + ISleepService + IMoodService + IRecommendationService
           │
           ▼
[Batch C: Operations & Chat] ─► IAstronautService + IMissionService + IChatService
           │
           ▼
[Batch D: Guidance & Safety] ─► Guide/Emergency retrieval + ISymptomGuidanceService (restore SymptomLog)
           │
           ▼
[Feature Controllers]      ───► REST endpoints for all services
           │
           ▼
[SignalR Hubs]             ───► TelemetryHub, AlertsHub, ChatHub
           │
           ▼
[Offline Sync Service]     ───► SyncService (atomic processing, deduplication count)
           │
           ▼
[Hardening & Quality]      ───► Health check endpoint, ProblemDetails, Scalar/SwaggerUI
```

### Detailed Breakdown

1. **Services Interface Review (Step 1)**:
   - Final review of all 9 pending service interfaces against DTOs and requirements before writing implementations.
2. **Service Implementation Batch A (Vitals & Safety Alerts)**:
   - `VitalSignService`: Implements `RecordReadingAsync`, `SyncReadingsAsync`, `GetRecentReadingsAsync`, `GetReadingsByTimeRangeAsync`. Integrates `IVitalSignStatusEvaluator` to automatically trigger and persist safety alerts when readings breach normal boundaries. Idempotency enforced via `ClientGeneratedId`.
   - `HealthAlertService`: Implements `GetActiveAlertsAsync`, `GetAlertsForAstronautAsync`, `GetAlertByIdAsync`, `AcknowledgeAlertAsync` (stamping `AcknowledgedAt`, `AcknowledgedByUserId`, and `ActionTaken`).
3. **Service Implementation Batch B (Telemetry & Personal Care)**:
   - `DashboardService`: Aggregates physiological trends, active alerts, latest sleep, and current status for both Astronaut and Flight Monitor consoles.
   - `SleepService`: Computes `DurationHours = (EndTime - StartTime).TotalHours`, calculates sleep efficiency, evaluates thresholds from configuration, and records records idempotently.
   - `MoodService`: Records subjective valence, stress score, and emotion logs idempotently.
   - `RecommendationService`: Handles creation and status transitions (`Pending` -> `Acknowledged` / `Completed`).
4. **Service Implementation Batch C (Operations & Communication)**:
   - `AstronautService` & `MissionService`: Profile management, operational code indexing, and crew mission assignments.
   - `ChatService`: Thread management keyed by `AstronautId`, timestamp ordering (`RecordedAt` vs `SentAt`), unread tracking, and read receipts (`MarkThreadAsReadAsync`).
5. **Service Implementation Batch D (Knowledge & Symptom Guidance)**:
   - Guide & Emergency Procedure services: Fast, cached retrieval of 5-principle metric guides and step-by-step emergency action protocols.
   - `SymptomGuidanceService`: Evaluates reported symptom codes against space condition catalogs and returns ranked topic matches without clinical percentage scores or disease diagnoses.
   - Restore `SymptomLog` from `Models/Backlog` if required for Phase 5.7 roadmap alignment.
6. **Feature Controllers (REST Web API)**:
   - `VitalSignsController`, `AlertsController`, `SleepController`, `MoodController`, `RecommendationsController`, `AstronautsController`, `MissionsController`, `ChatController`, `SymptomGuidanceController`, `DashboardController`.
   - Enforce authorization policies (`AstronautOnly`, `MonitorOnly`, `Authenticated`) and route conventions (`/api/...`).
7. **SignalR Hubs**:
   - `TelemetryHub`: Live broadcasting of vital readings to monitor consoles.
   - `AlertsHub`: Immediate push notifications for `Critical` and `Warning` safety alerts.
   - `ChatHub`: Real-time bi-directional messaging between astronauts and ground support.
8. **Offline Synchronization Engine**:
   - Dedicated `SyncService` processing `SyncBatchRequest`: Atomic transaction handling across vitals, sleep, mood, and chat; duplicate detection and reporting (`DuplicatesIgnoredCount`); server time stamping (`ServerSyncedAt`).
9. **Infrastructure Hardening & Polish**:
   - Interactive UI: Add Scalar UI (`Scalar.AspNetCore`) or SwaggerUI to visualize the OpenAPI specification.
   - Health Check: Add ASP.NET Core Health Checks (`AddHealthChecks()`, `MapHealthChecks("/health")`) verifying database connectivity.
   - Global Error Handling: Register `UseExceptionHandler()` with RFC 7807 ProblemDetails responses.
   - Rate limiting refinement: Support per-IP partitioning for public endpoints.

---

## 3. Open Decisions & Assumptions

| # | Topic | Current Assumption | Decision Required from Stakeholder |
|---|-------|--------------------|-----------------------------------|
| 1 | **Crew Provisioning Model** | Self-registration is disabled. Crew accounts are created out-of-band by administrators via CLI scripts or seeded locally. | Confirm whether an administrative user management endpoint is needed in MVP or if configuration/seeding is sufficient. |
| 2 | **Manual Emergency Alert Triggering** | Alerts are primarily generated by the automated evaluator engine. `EmergencyProcedureDto.cs` defines `TriggerEmergencyAlertRequest`. | Confirm whether flight monitors should be permitted to manually raise emergency alerts from their consoles. |
| 3 | **Supabase Connection Pooling** | Migrations and seed scripts require PostgreSQL direct or session mode (port `5432`). Transaction mode (port `6543`) does not support EF Core prepared statements without special configuration. | Confirm whether runtime connection uses Supabase Session Pooler or Transaction Pooler with `Multiplexing=true` or compatibility mode. |
| 4 | **Interactive API Documentation** | ASP.NET Core 9 minimal OpenAPI is used (`Microsoft.AspNetCore.OpenApi`). | Confirm whether to install Scalar UI or standard Swashbuckle Swagger UI for browser-based testing. |
| 5 | **SymptomLog Roadmap Timing** | `SymptomLog` is preserved in `Models/Backlog` per Phase 2 design notes. | Confirm whether `SymptomLog` should be restored during Batch D or kept in backlog until Phase 5.7. |

---

## 4. Medical Content Requiring Staff Review

All medical rules, emergency steps, and reference ranges must be reviewed and signed off by authorized clinical personnel before operational spaceflight deployment. The following table identifies every placeholder and sample item in the solution:

| Content Item | Code Location | Source / Research Status | Notes & Review Requirements |
|--------------|---------------|--------------------------|-----------------------------|
| **Sleep Thresholds** | `appsettings.json:81-90` | **NOT from research** (Placeholder) | Target: 8.0h, Warning: < 6.0h, Emergency: < 4.5h, Efficiency Warning: < 75%, Efficiency Emergency: < 60%. Needs operational flight surgeon sign-off. |
| **Sync Future Timestamp Tolerance** | `appsettings.json:91-94`, `Validators/*Validators.cs:12` | **Operational assumption** (Placeholder) | Default 5 minutes tolerance for offline clock drift. Requires mission operations review. |
| **Heart Rate Reference Range** | `Data/Seed/ModelBuilderExtensions.cs:91` | Terrestrial Standard (General research) | Normal [60-100] bpm, Emergency (<40 or >140). Marked `IsReviewed = false`, `Notes = "PLACEHOLDER - requires review by authorized staff"`. |
| **Blood Pressure Reference Range** | `Data/Seed/ModelBuilderExtensions.cs:113` | Terrestrial Standard (General research) | Systolic [90-120], Diastolic [60-80]. Emergency (>160 or <70 systolic, >100 or <45 diastolic). Marked `IsReviewed = false`. |
| **SpO2 Reference Range** | `Data/Seed/ModelBuilderExtensions.cs:131` | Terrestrial Standard (General research) | Normal [95-100]%, Emergency (<89%). Marked `IsReviewed = false`. Spacecraft cabin pressure variations may alter baseline. |
| **Core Temperature Reference Range** | `Data/Seed/ModelBuilderExtensions.cs:149` | Terrestrial Standard (General research) | Normal [36.5-37.3] °C, Emergency (<35.0 or >38.5). Marked `IsReviewed = false`. |
| **Blood Glucose Reference Range** | `Data/Seed/ModelBuilderExtensions.cs:167` | Partial research (Fasting only) | Normal [70-99] mg/dL, Emergency (<50 or >200). Lower bound 70 is marked placeholder; post-prandial ranges not specified in research. |
| **Respiratory Rate Reference Range** | `Data/Seed/ModelBuilderExtensions.cs:185` | Terrestrial Standard (General research) | Normal [12-18] br/min, Emergency (<8 or >30). Marked `IsReviewed = false`. |
| **Closed Environment Air Quality Guidance** | `Data/Seed/ModelBuilderExtensions.cs:362` | Sample Content (Non-vital environmental) | Self-care guidance is placeholder. Note that ambient CO2 is environmental sensor telemetry, not human vitals. |
| **Emergency Procedures Instructions** | `Data/Seed/ModelBuilderExtensions.cs:220-310` | Simplified clinical protocols | Protocols for Hypoxia, Tachycardia, Bradycardia, Hyperthermia, Hypoglycemia, Hypotension. Immediate actions need operational verification. |

---

## 5. Known Risks & Technical Debt

1. **Project Directory Layout (Nested Tests)**:
   - *Risk*: `tests/AstronautHealthMonitoring.Tests` is nested inside the main Web API root directory `Health-Monitoring/`. In previous builds, recursive MSBuild evaluation caused duplicate `bin/obj` artifact copying.
   - *Mitigation Applied*: Added `<DefaultItemExcludes>$(DefaultItemExcludes);tests/**</DefaultItemExcludes>` and `<Compile Remove="tests/**" />` to `AstronautHealthMonitoring.csproj`.
   - *Recommended Structural Fix*: Restructure solution to standard `src/AstronautHealthMonitoring` and `tests/AstronautHealthMonitoring.Tests` layout (requires stakeholder approval).
2. **In-Memory Test Provider vs PostgreSQL Reality**:
   - *Risk*: Unit tests execute against `Microsoft.EntityFrameworkCore.InMemory`. In-memory EF Core does not enforce relational foreign key constraints, column lengths, transaction isolation levels, or PostgreSQL-specific syntax (such as ILIKE or unique filtered index `WHERE "UserId" IS NOT NULL`).
   - *Mitigation*: Comprehensive manual/integration tests must be executed against a real PostgreSQL/Supabase instance prior to release.
3. **Configuration Precedence / Stale Secrets**:
   - *Risk*: When running `dotnet ef`, configuration sources (like environment variables) may point to an older or alternate Supabase host than local user-secrets.
   - *Mitigation Applied*: Added startup diagnostic logger that outputs the resolved connection host (without credentials) and the exact configuration provider name.
4. **Linux Headless Dev Certs**:
   - *Risk*: On Linux or Docker environments without a local certificate store, Kestrel may fail to establish HTTPS if developers do not trust certificates.
   - *Mitigation*: Provided explicit instructions to run via `http` profile or execute `dotnet dev-certs https --trust`.

---

## 6. Verification Summary (Real Command Proofs)

All conclusions in this report are backed by verified tool execution outputs from this session:
- **Build**: `dotnet build AstronautHealthMonitoring.sln` -> 0 Warnings, 0 Errors.
- **Tests**: `dotnet test AstronautHealthMonitoring.sln --no-build` -> 50 Passed, 0 Failed, Duration: 5.78s.
- **Model Drift Check**: `dotnet tool run dotnet-ef migrations has-pending-model-changes` -> *"No changes have been made to the model since the last migration."*
- **Migrations List**: `dotnet ef migrations list` -> All 3 migrations discovered in chronological order.
- **File System Case Audit**: PowerShell group check verified 0 duplicate files or folders differing only by case.

