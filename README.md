# Template QR Code API

.NET 10 API that accepts personal data and returns a downloadable PNG QR code.
Scanning the image reveals readable JSON. The generic QR endpoint needs no database,
signing certificate, or external image service.

## Run on an Ubuntu VM

For a private VM test, copy `artifacts/qr-api-ubuntu.tar.gz` to your home
directory on the VM. Install the ASP.NET Core 10 runtime and extract the build.

```bash
sudo apt update
sudo apt install -y aspnetcore-runtime-10.0 curl
mkdir -p ~/qr-api
tar -xzf ~/qr-api-ubuntu.tar.gz -C ~/qr-api
cd ~/qr-api
```

If you have the source on the VM instead, install `dotnet-sdk-10.0` and run
these commands from the repository root to build it there.

```bash
dotnet publish Template.QrCode.Api/Template.QrCode.Api.csproj -c Release -p:UseAppHost=false -o ~/qr-api
cd ~/qr-api
```

Start the API in the same terminal as these settings. It listens on port
`8080`, allows all browser origins, and uses `test` as the API key. Use this
configuration only on a private test network.

```bash
export ASPNETCORE_URLS='http://0.0.0.0:8080'
export Cors__AllowedOrigins__0='*'
export AllowedApiKeys='test'
dotnet Template.QrCode.Api.dll
```

Leave the API running. From a second terminal on the VM, generate a QR code.

```bash
curl --fail-with-body http://127.0.0.1:8080/GenerateQrCode \
  -H 'X-API-KEY: test' \
  -H 'Content-Type: application/json' \
  --data '{"id":"00123","name":"Test User","email":"test@example.com"}' \
  --output qrcode.png
```

A successful request saves `qrcode.png`. From another machine, replace
`127.0.0.1` with the VM's IP address and make sure TCP port `8080` is reachable.
To allow one frontend, replace `*` with its exact origin, such as
`http://localhost:3000`, and restart the API.

## Deploy with Dokploy

Create an Application from this repository. Choose the Dockerfile build type,
set the Dockerfile path to `Dockerfile`, and use `.` as the Docker context.
In Domains, set the container port to `8080` and enable HTTPS for your API domain.

Set `AllowedApiKeys` to a long random value in the application's runtime environment.
If a browser frontend calls the API from a different origin, set
`Cors__AllowedOrigins__0` to its exact origin (for example, `https://qr.example.com`).
Set `EnableSwagger=true` if you need the Swagger UI at `/swagger`.

The image uses `docker/appsettings.json` for its defaults and writes logs to
standard output. `POST /GenerateQrCode` works without SQL Server. For certificate
verification, mount the public certificate under `/app/Certificats/` and set
`CryptCertificatePublic` to its filename. Certificate signing uses the Windows
`LocalMachine\My` store and requires a code change to run in this Linux image.

## Run

Install the .NET 10 SDK. Set a private API key for your environment:

```powershell
$env:AllowedApiKeys = 'replace-with-your-own-long-random-key'
dotnet run --project Template.QrCode.Api/Template.QrCode.Api.csproj
```

The project launch profile listens on `http://localhost:5000` and
`https://localhost:5001`. Swagger is at `http://localhost:5000/swagger` when
`EnableSwagger` is true. Environment variables override `appsettings.json`.
Use HTTPS outside local development.

For Azure App Service, select the .NET 10 runtime and use startup command
`dotnet Template.QrCode.Api.dll`. The Azure DevOps pipeline installs the .NET 10 SDK;
configure its agent pool and SonarQube connection/project for your environment.

## Generate a QR code

`POST /GenerateQrCode` requires `Content-Type: application/json` and the
`X-API-KEY` header. Send ID as a string to preserve leading zeroes.

```json
{
  "id": "00123",
  "name": "Amine Example",
  "email": "amine@example.com",
  "photo": "https://example.com/photos/00123.jpg"
}
```

`id`, `name`, and `email` are required. `photo` is optional: omit it or send `null`.
It must be an absolute HTTP or HTTPS URL, rather than an image upload or base64
image. The URL is stored as text; the server does not fetch or embed the photo.

Save the response as an image:

```bash
curl --fail-with-body http://localhost:5000/GenerateQrCode \
  -H "X-API-KEY: YOUR_API_KEY" \
  -H "Content-Type: application/json" \
  --data '{"id":"00123","name":"Amine Example","email":"amine@example.com"}' \
  --output qrcode.png
```

Success returns `200`, `Content-Type: image/png`, and filename `qrcode.png`.
The decoded JSON contains the submitted fields, with absent/null photo omitted.
Unicode names are supported. Responses prevent caching with `Cache-Control: no-store`.

Limits: ID 128 characters, name 200, email 254, photo URL 1024, request body 16 KiB.
The combined JSON must also fit one QR symbol; oversized data returns `400`.
Invalid/missing fields return `400`, a missing/invalid API key returns `401`,
an oversized body returns `413`, and unsupported content types return `415`.

The QR contents are **readable by anyone who can scan it**, and are neither
encrypted nor signed. They do not prove identity. No submitted data is saved.

## Check

With the server running, use PowerShell 7 and the same `AllowedApiKeys` value:

```powershell
./scripts/Test-QrCode.ps1
```

This checks PNG output, optional photo, Unicode requests, API-key enforcement,
invalid fields, body/QR capacity limits, and Swagger metadata.

## Existing certificate API

`POST /GenerateHashedInfo`, `POST /VerifyHashedInfo?Value=...`, and `GET /`
remain available. The certificate endpoints keep their existing certificate and
database requirements. Their signed `FRMF:` payloads are separate from the new
readable JSON QR codes; `VerifyHashedInfo` does not verify generic JSON.
