#!/bin/bash

ES_HOST="elasticsearch"
ES_USER="elastic"
ES_PASS="elasticRootPass"

echo "Waiting for Elasticsearch to be available..."
until curl -s -k -u $ES_USER:$ES_PASS https://$ES_HOST:9200 >/dev/null 2>&1; do
  echo "Waiting for Elasticsearch..."
  sleep 5
done

echo "Elasticsearch is up, creating role and user..."

# Роль для логов
curl -X PUT "https://$ES_HOST:9200/_security/role/logs_writer" \
 -u $ES_USER:$ES_PASS -H 'Content-Type: application/json' -d '{
   "cluster": ["monitor"],
   "indices": [{"names":["taskboard-logs-*"],"privileges":["write","create","create_index"]}]
 }'

# Пользователь для логов
curl -X PUT "https://$ES_HOST:9200/_security/user/logs_user" \
 -u $ES_USER:$ES_PASS -H 'Content-Type: application/json' -d '{
   "password": "LogsUserPass123!",
   "roles": ["logs_writer"]
 }'

echo "User and role created."
