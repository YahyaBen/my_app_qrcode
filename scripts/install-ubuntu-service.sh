#!/usr/bin/env bash
set -euo pipefail

app_dir=$(pwd -P)
dotnet_path=$(command -v dotnet)
service_user=$(id -un)

if [[ $EUID -eq 0 || ! -f Template.QrCode.Api.dll ]]; then
    echo 'Run as your regular VM user from the folder containing Template.QrCode.Api.dll.' >&2
    exit 1
fi

if ! systemctl is-active --quiet qr-api.service &&
    [[ -n $(ss -H -ltn 'sport = :8080') ]]; then
    echo 'Stop the process currently listening on port 8080, then rerun this script.' >&2
    exit 1
fi

read -rsp 'API key (enter test for the existing test setup): ' api_key
printf '\n'
if [[ ! $api_key =~ ^[a-zA-Z0-9._~-]+$ ]]; then
    echo 'Enter a key using letters, numbers, dots, underscores, tildes, or hyphens.' >&2
    exit 1
fi

umask 077
config_dir=$(mktemp -d)
trap 'rm -f -- "$config_dir/qr-api.env" "$config_dir/qr-api.service"; rmdir -- "$config_dir"' EXIT

cat > "$config_dir/qr-api.env" <<EOF
ASPNETCORE_URLS=http://0.0.0.0:8080
EnableSwagger=true
Cors__AllowedOrigins__0=*
AllowedApiKeys=$api_key
EOF

unit_app_dir=${app_dir//%/%%}
unit_dotnet_path=${dotnet_path//%/%%}
cat > "$config_dir/qr-api.service" <<EOF
[Unit]
Description=QR Code API
After=network.target

[Service]
User=$service_user
WorkingDirectory=$unit_app_dir
ExecStart="$unit_dotnet_path" Template.QrCode.Api.dll
EnvironmentFile=/etc/qr-api.env
Restart=on-failure
RestartSec=5

[Install]
WantedBy=multi-user.target
EOF

sudo install -o root -g root -m 600 "$config_dir/qr-api.env" /etc/qr-api.env
sudo install -o root -g root -m 644 "$config_dir/qr-api.service" /etc/systemd/system/qr-api.service
sudo systemctl daemon-reload
sudo systemctl enable qr-api.service
sudo systemctl restart qr-api.service

if ! curl -fsS --retry 5 --retry-connrefused --retry-delay 1 --max-time 5 \
    http://127.0.0.1:8080/swagger/v1/swagger.json -o /dev/null; then
    sudo journalctl -u qr-api.service -n 30 --no-pager
    exit 1
fi

if ! command -v netfilter-persistent >/dev/null; then
    sudo apt-get update
    sudo env DEBIAN_FRONTEND=noninteractive apt-get install -y iptables-persistent
fi

if ! sudo iptables -C INPUT -p tcp --dport 8080 -m conntrack --ctstate NEW -j ACCEPT 2>/dev/null; then
    sudo iptables -I INPUT 1 -p tcp --dport 8080 -m conntrack --ctstate NEW -j ACCEPT
fi
sudo netfilter-persistent save
sudo systemctl enable netfilter-persistent.service

echo 'QR API is running and starts at boot. The TCP 8080 firewall rule is saved.'
