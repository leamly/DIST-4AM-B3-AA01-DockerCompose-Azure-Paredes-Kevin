#!/bin/bash
# Damos unos segundos extra para asegurar que el motor de base de datos esté aceptando conexiones
sleep 15s

echo "Ejecutando scripts de inicialización..."

# Ajusta los nombres de tus archivos .sql según corresponda
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "TuPasswordSeguro123!" -d master -i /scripts/LibroDBA.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "TuPasswordSeguro123!" -d master -i /scripts/InventarioDBA.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "TuPasswordSeguro123!" -d master -i /scripts/CategoriaDBA.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "TuPasswordSeguro123!" -d master -i /scripts/VehiculoDBA.sql

echo "Bases de datos inicializadas correctamente."