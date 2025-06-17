#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "chanexadmin" --dbname "postgres" <<-EOSQL
   CREATE DATABASE platform_db;
   GRANT ALL PRIVILEGES ON DATABASE platform_db TO $POSTGRES_USER;    
EOSQL

# Add the required extensions to the platform database
psql -v ON_ERROR_STOP=1 --username "chanexadmin" --dbname "platform_db" <<-EOSQL
   CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
   CREATE EXTENSION IF NOT EXISTS "postgis";
EOSQL

pg_restore -v --no-owner --username "chanexadmin" --dbname "platform_db" /docker-entrypoint-initdb.d/database.backup
