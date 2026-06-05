#!/bin/bash
set -e

PUBLISH_DIR="/opt/soteria-api"
APP_NAME="SistemaCadastro.API"

echo "=== Publicando $APP_NAME ==="

dotnet publish -c Release -o "$PUBLISH_DIR" --self-contained false

echo "=== Instalando serviço systemd ==="

cat > /etc/systemd/system/soteria-api.service << 'EOF'
[Unit]
Description=Soteria API - Sistema de Cadastro
After=network.target mysql.service
Wants=mysql.service

[Service]
Type=simple
User=www-data
Group=www-data
WorkingDirectory=/opt/soteria-api
ExecStart=/usr/bin/dotnet /opt/soteria-api/SistemaCadastro.API.dll
Restart=always
RestartSec=10
Environment=ASPNETCORE_URLS=http://0.0.0.0:5000
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_CLI_TELEMETRY_OPTOUT=1

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable soteria-api
systemctl restart soteria-api

echo "=== OK ==="
echo "Status: systemctl status soteria-api"
echo "Logs: journalctl -u soteria-api -f"
echo "Porta: http://0.0.0.0:5000"
