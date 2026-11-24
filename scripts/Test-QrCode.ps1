param(
    [string]$BaseUrl = 'http://localhost:5000',
    [string]$ApiKey = $env:AllowedApiKeys
)

$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($ApiKey)) {
    throw 'Set AllowedApiKeys or pass -ApiKey with the server API key.'
}

$endpoint = "$($BaseUrl.TrimEnd('/'))/GenerateQrCode"
$headers = @{ 'X-API-KEY' = $ApiKey }
$person = @{ id = '00123'; name = 'Amine'; email = 'amine@example.com' }

function Send-Request($Body, $RequestHeaders = $headers) {
    Invoke-WebRequest -Uri $endpoint -Method Post -Headers $RequestHeaders `
        -ContentType 'application/json; charset=utf-8' `
        -Body ([System.Text.Encoding]::UTF8.GetBytes($Body)) -SkipHttpErrorCheck
}

function Assert-Status($Response, $Expected, $Description) {
    if ([int]$Response.StatusCode -ne $Expected) {
        throw "$Description`: expected $Expected, got $($Response.StatusCode)."
    }
}

function Assert-Png($Response) {
    Assert-Status $Response 200 'Valid request'
    if ($Response.Headers['Content-Type'] -notcontains 'image/png') {
        throw 'Expected image/png response.'
    }
    if ([Convert]::ToHexString([byte[]]$Response.Content[0..7]) -ne '89504E470D0A1A0A') {
        throw 'Response is not a PNG file.'
    }
    if (($Response.Headers['Cache-Control'] -join ',') -notmatch 'no-store') {
        throw 'QR responses must prevent caching.'
    }
}

$preflight = Invoke-WebRequest -Uri $endpoint -Method Options -Headers @{
    Origin = 'https://qr-client.example'
    'Access-Control-Request-Method' = 'POST'
    'Access-Control-Request-Headers' = 'content-type,x-api-key'
} -SkipHttpErrorCheck
Assert-Status $preflight 204 'Browser CORS preflight without API key'
$allowedHeaders = ($preflight.Headers['Access-Control-Allow-Headers'] -join ',').Split(',').Trim()
if ($allowedHeaders -notcontains 'X-API-KEY' -or $allowedHeaders -notcontains 'Content-Type') {
    throw 'CORS must allow browser requests with Content-Type and X-API-KEY.'
}
if (-not $preflight.Headers['Access-Control-Allow-Origin']) {
    throw 'CORS must allow the browser origin.'
}

Assert-Png (Send-Request ($person | ConvertTo-Json -Compress))
$person.photo = 'https://example.com/photos/00123.jpg'
Assert-Png (Send-Request ($person | ConvertTo-Json -Compress))
$person.name = 'أمين Élodie 漢字 😀'
Assert-Png (Send-Request ($person | ConvertTo-Json -Compress))
$person.photo = $null
Assert-Png (Send-Request ($person | ConvertTo-Json -Compress))

Assert-Status (Send-Request ($person | ConvertTo-Json) @{}) 401 'Missing API key'
Assert-Status (Send-Request ($person | ConvertTo-Json) @{ 'X-API-KEY' = 'invalid-test-key' }) 401 'Invalid API key'

foreach ($field in @('id', 'name', 'email')) {
    $invalid = $person.Clone()
    $invalid.Remove($field)
    Assert-Status (Send-Request ($invalid | ConvertTo-Json)) 400 "Missing $field"
    $invalid[$field] = ' '
    Assert-Status (Send-Request ($invalid | ConvertTo-Json)) 400 "Blank $field"
}

$invalid = $person.Clone()
$invalid.email = 'not-an-email'
Assert-Status (Send-Request ($invalid | ConvertTo-Json)) 400 'Invalid email'
$invalid = $person.Clone()
$invalid.photo = 'data:image/png;base64,AAAA'
Assert-Status (Send-Request ($invalid | ConvertTo-Json)) 400 'Inline image instead of photo URL'
$invalid.photo = 'ftp://example.com/photo.jpg'
Assert-Status (Send-Request ($invalid | ConvertTo-Json)) 400 'Non-HTTP photo URL'
$invalid.photo = 'not-a-url'
Assert-Status (Send-Request ($invalid | ConvertTo-Json)) 400 'Invalid photo URL'
$invalid = $person.Clone()
$invalid.id = 'x' * 129
Assert-Status (Send-Request ($invalid | ConvertTo-Json)) 400 'ID over length limit'
$invalid = $person.Clone()
$invalid.name = '漢' * 200
$invalid.photo = 'https://example.com/' + ('漢' * 1000)
$tooLong = Send-Request ($invalid | ConvertTo-Json)
Assert-Status $tooLong 400 'Payload exceeds QR capacity'
if (($tooLong.Headers['Content-Type'] -join ',') -notmatch 'json') {
    throw 'Validation errors must have a JSON content type.'
}
$problemJson = if ($tooLong.Content -is [byte[]]) {
    [System.Text.Encoding]::UTF8.GetString($tooLong.Content)
} else {
    [string]$tooLong.Content
}
if (-not ($problemJson | ConvertFrom-Json).errors.payload) {
    throw 'Expected the QR capacity error, rather than a field validation error.'
}
Assert-Status (Send-Request 'null') 400 'Null body'
Assert-Status (Send-Request '{') 400 'Malformed JSON'
Assert-Status (Send-Request ('{"id":"' + ('x' * 17000) + '"}')) 413 'Oversized body'
$unsupported = Invoke-WebRequest -Uri $endpoint -Method Post -Headers $headers `
    -ContentType 'text/plain' -Body 'not-json' -SkipHttpErrorCheck
Assert-Status $unsupported 415 'Unsupported content type'

$swagger = Invoke-RestMethod -Uri "$($BaseUrl.TrimEnd('/'))/swagger/v1/swagger.json"
if ($swagger.info.title -ne 'Template QR Code API') {
    throw 'Swagger must identify the service as Template QR Code API.'
}
if (-not $swagger.paths.'/GenerateQrCode'.post.responses.'200'.content.'image/png') {
    throw 'Swagger does not describe the PNG response.'
}

$apiKeyScheme = $swagger.components.securitySchemes.ApiKey
if ($apiKeyScheme.type -ne 'apiKey' -or $apiKeyScheme.in -ne 'header' -or $apiKeyScheme.name -cne 'X-API-KEY') {
    throw 'Swagger must define X-API-KEY as an API key header.'
}
if (-not ($swagger.security | Where-Object { $_.PSObject.Properties['ApiKey'] })) {
    throw 'Swagger requests must use the API key scheme.'
}

Write-Output 'QR endpoint checks passed: browser CORS, PNG output, optional photo, Unicode, auth, validation, size limits, and Swagger.'
