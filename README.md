# CredentialCheck

CredentialCheck ist eine kleine .NET-8-Web-API zur strukturierten Vorprüfung von Qualifikationsnachweisen.

Das Projekt dient als Lern- und Demonstrationsprojekt für C#/.NET, ASP.NET Core, PostgreSQL, Entity Framework Core, Datei-Uploads, API-Design und einfache fachliche Workflows.

## Zweck

Die Anwendung bildet einen typischen Prüfprozess ab:

- Qualifikationsfall anlegen
- Dokumente und Nachweise erfassen
- Dateien hochladen und wieder abrufen
- Dokumente fachlich prüfen
- Fallstatus ändern
- Audit-Log führen
- einfache Review-Zusammenfassung erzeugen
- regelbasierte Entscheidungsnotiz erstellen

Die finale Entscheidung wird nicht automatisiert. Die Anwendung unterstützt lediglich die strukturierte Vorbereitung einer menschlichen Prüfung.

## Technischer Stack

- .NET 8
- ASP.NET Core Minimal APIs
- C#
- PostgreSQL
- Entity Framework Core
- Docker / Docker Compose
- REST API
- Lokaler Datei-Storage
- Git

## Hauptfunktionen

### Qualification Cases

Ein Qualifikationsfall enthält grundlegende Bewerberdaten und einen Bearbeitungsstatus.

API-Endpunkte:

```text
POST  /api/cases
GET   /api/cases
GET   /api/cases/{id}
PATCH /api/cases/{id}/status
GET   /api/cases/{id}/audit-log
GET   /api/cases/{id}/review-summary
GET   /api/cases/{id}/decision-note
```

### Documents

Zu jedem Fall können Dokumente erfasst und Dateien hochgeladen werden.

API-Endpunkte:

```text
POST  /api/cases/{caseId}/documents
GET   /api/cases/{caseId}/documents
POST  /api/cases/{caseId}/documents/upload
GET   /api/cases/{caseId}/documents/{documentId}/file
PATCH /api/cases/{caseId}/documents/{documentId}/review-status
```

## Datenmodell

### QualificationCase

Ein Fall enthält unter anderem:

- Bewerbername
- E-Mail-Adresse
- Land
- Status
- Erstellungszeitpunkt

Die E-Mail-Adresse ist eindeutig.

### QualificationDocument

Ein Dokument enthält unter anderem:

- Dokumenttyp
- Prüfstatus
- Titel
- ausstellende Institution
- Land
- Ausstellungsdatum
- Datei-Metadaten
- Speicherpfad

### AuditLogEntry

Das Audit-Log protokolliert relevante Aktionen, zum Beispiel:

- Fall angelegt
- Status geändert
- Dokument hinzugefügt
- Dokument-Prüfstatus geändert

## Statuswerte

### Fallstatus

```text
Draft
InReview
WaitingForDocuments
ReadyForDecision
Completed
```

### Dokument-Prüfstatus

```text
PendingReview
Accepted
Rejected
NeedsClarification
```

### Review-Empfehlung

```text
NoDocuments
InReview
NeedsClarification
ManualDecisionRequired
ReadyForDecision
```

## Datei-Upload

Erlaubte Dateitypen:

```text
PDF
JPG
JPEG
PNG
```

Maximale Dateigröße:

```text
10 MB
```

Die Dateien werden lokal im Projekt unter folgendem Pfad gespeichert:

```text
storage/documents
```

Dieser Ordner sollte nicht ins Git-Repository aufgenommen werden.

## Lokale Entwicklung

### Voraussetzungen

- Debian 12 oder vergleichbare Linux-Umgebung
- .NET 8 SDK
- Docker
- PostgreSQL-Client
- Git

### PostgreSQL starten

```bash
docker compose up -d
```

### Datenbank aktualisieren

```bash
~/.dotnet/tools/dotnet-ef database update
```

### Build

```bash
./build.sh
```

### Anwendung starten

```bash
./run.sh
```

Die Anwendung läuft dann auf:

```text
http://dev20:5000
```

Health Check:

```text
GET http://dev20:5000/health
```

## Beispiel: Fall anlegen

```http
POST /api/cases
Content-Type: application/json
```

```json
{
  "applicantName": "Ana García",
  "applicantEmail": "ana.garcia@example.com",
  "country": "Spain"
}
```

## Beispiel: Dokument-Metadaten erfassen

```http
POST /api/cases/{caseId}/documents
Content-Type: application/json
```

```json
{
  "documentType": "Degree",
  "title": "Bachelorzeugnis Informatik",
  "issuingInstitution": "Universidad de Sevilla",
  "country": "Spain",
  "issueDate": "2018-07-15"
}
```

## Beispiel: Dokument mit Datei hochladen

```http
POST /api/cases/{caseId}/documents/upload
Content-Type: multipart/form-data
```

Form-Data-Felder:

```text
documentType        Degree
title               Bachelorzeugnis Informatik
issuingInstitution  Universidad de Sevilla
country             Spain
issueDate           2018-07-15
file                bachelorzeugnis.pdf
```

## Projektstruktur

```text
CredentialCheck/
├── Data/
│   └── AppDbContext.cs
├── Dtos/
├── Endpoints/
├── Mappers/
├── Migrations/
├── Models/
├── Services/
├── Validators/
├── storage/
├── Program.cs
├── docker-compose.yml
├── build.sh
└── run.sh
```

## Architekturidee

Die Anwendung trennt bewusst zwischen:

- Models: interne Fachobjekte
- DTOs: Eingabe- und Ausgabeformate der API
- Services: fachliche Logik
- Endpoints: HTTP-Schnittstellen
- Mappers: Umwandlung von Models in Response-DTOs
- Validators: einfache Eingabeprüfung
- Data: Datenbankzugriff über Entity Framework Core

## Hinweis

CredentialCheck ist ein Lern- und Demonstrationsprojekt. Es ist noch kein produktionsreifes System für echte Bewerberdaten oder sensible Dokumente.

Für produktive Nutzung wären unter anderem erforderlich:

- Authentifizierung und Rollenrechte
- Verschlüsselung sensibler Daten
- DSGVO-konformes Löschkonzept
- sicherer Datei-Storage
- Virenscan für Uploads
- strukturierte Fehlerbehandlung
- Tests
- Logging und Monitoring
- Deployment-Konzept
