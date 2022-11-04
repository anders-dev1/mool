#!/bin/sh
set -eu

envsubst '${proxy_pass}' < /etc/nginx/conf.d/default.conf.template > /etc/nginx/conf.d/default.conf

exec "$@"