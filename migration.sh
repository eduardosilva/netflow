#!/usr/bin/env bash

#-------------------------------------------------------------
# RECOMMENDATIONS:
# NOT use command arguments abbreviations (Ex.: sed -e / sed -expression)
# OPTIONAL send useless output to (Ex.: command > /dev/null 2>&1)
#-------------------------------------------------------------

# FAULT CONFIGURATION
set -Eeuo pipefail
trap cleanup SIGINT SIGTERM ERR EXIT

# MAGIC VARIABLES
__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
__file="${__dir}/$(basename "${BASH_SOURCE[0]}")"
__file_name="$(basename "$__file")"
__base="$(basename "${__file}" .sh)"

# MIGRATIONS CONFIGURATION
MIGRATIONS_PROJECT="src/core/netflow.core.csproj"
MIGRATIONS_PATH="infrastructure/databases/migrations"
SCRIPTS_PATH="src/core/infrastructure/databases/scripts"
STARTUP_PROJECT="src/api/netflow.api.csproj"

#-------------------------------------------------------------
# MAING FUNCTION
#-------------------------------------------------------------
main() {
  case "$ACTION" in
  "add")
    add_migration
    ;;
  "remove")
    remove_migration
    ;;
  "apply")
    apply_migrations
    ;;
  *)
    die "Invalid action: $ACTION"
    ;;
  esac
}

handle_stdout() {
  msg=${1-}
  if [[ $msg == *"Error"* && $msg != *"Error(s)"* ]] || [[ $msg == *"Exception"* ]]; then
    error "$msg"
  elif [[ $msg == *"Warning"* && $msg != *"Warning(s)"* ]]; then
    warning "$msg"
  else
    log "$msg"
  fi
}

add_migration() {
  dotnet ef migrations add "$MIGRATION_NAME" \
    --project $MIGRATIONS_PROJECT \
    --startup-project $STARTUP_PROJECT \
    --output-dir $MIGRATIONS_PATH \
    --verbose |
    while IFS= read -r line; do handle_stdout "$line"; done &&
  dotnet ef migrations script \
    --project $MIGRATIONS_PROJECT \
    --startup-project $STARTUP_PROJECT \
    --output $SCRIPTS_PATH/${MIGRATION_NAME}.sql \
    --verbose |
    while IFS= read -r line; do handle_stdout "$line"; done
}

remove_migration() {
  dotnet ef migrations remove \
    --project $MIGRATIONS_PROJECT \
    --startup-project $STARTUP_PROJECT \
    --verbose \
    --force | while IFS= read -r line; do handle_stdout "$line"; done &&
    rm --recursive \
      --verbose \
      --force $SCRIPTS_PATH/${MIGRATION_NAME}.sql  |
    while IFS= read -r line; do handle_stdout "$line"; done
}

apply_migrations() {
  dotnet ef database update \
    --project $MIGRATIONS_PROJECT \
    --startup-project $STARTUP_PROJECT \
    --verbose |
    while IFS= read -r line; do handle_stdout "$line"; done
}

#-------------------------------------------------------------
# DISPLAY HOW TO USE THE SCRIPT
#-------------------------------------------------------------
usage() {
  cat <<EOF
Usage: $(basename "$__file_name") [-h] [-v] [-f] -p param_value arg1 [arg2...]

Script description here.

Available options:

-n, --name      Migration Name
-a, --action    Action (add, remove, apply)
-h, --help      Print this help and exit
-v, --verbose   Print script debug info
-f, --flag      Some flag description
-p, --param     Some param description
EOF
  exit
}

#-------------------------------------------------------------
# EXECUTED WHEN THE SCRIPT IS FINISHED
#-------------------------------------------------------------
cleanup() {
  trap - SIGINT SIGTERM ERR EXIT
  # script cleanup here
}

#-------------------------------------------------------------
# SET COLORS CONFIGURATION
#-------------------------------------------------------------
setup_colors() {
  if [[ -t 2 ]] && [[ -z "${NO_COLOR-}" ]] && [[ "${TERM-}" != "dumb" ]]; then
    NOFORMAT='\033[0m' RED='\033[0;31m' GREEN='\033[0;32m' ORANGE='\033[0;33m' BLUE='\033[0;34m' PURPLE='\033[0;35m' CYAN='\033[0;36m' YELLOW='\033[1;33m'
  else
    NOFORMAT='' RED='' GREEN='' ORANGE='' BLUE='' PURPLE='' CYAN='' YELLOW=''
  fi
}

#-------------------------------------------------------------
# PRINT MESSAGES
# EXAMPLES:
# msg "${RED}Read parameters:${NOFORMAT}"
# msg "- flag: ${flag}"
# msg "- param: ${param}"
# msg "- arguments: ${args[*]-}"
#-------------------------------------------------------------
msg() {
  echo >&2 -e "${1-}"
}

#-------------------------------------------------------------
# GET DATE AND TIME
#-------------------------------------------------------------
now() {
  date +%F-%T
}

#-------------------------------------------------------------
# GET DATE
#-------------------------------------------------------------
today() {
  date -I
}

#-------------------------------------------------------------
# INFO MESSAGE
#-------------------------------------------------------------
info() {
  msg "${BLUE}${__file_name} | $(now) - INFO: ${1-} ${NOFORMAT}"
}

#-------------------------------------------------------------
# LOG MESSAGE
#-------------------------------------------------------------
log() {
  msg "${__file_name} | $(now) - LOG: ${1-} ${NOFORMAT}"
}

#-------------------------------------------------------------
# warning MESSAGE
#-------------------------------------------------------------
warning() {
  msg "${YELLOW}${__file_name} | $(now) - WARNING: ${1-} ${NOFORMAT}"
}

#-------------------------------------------------------------
# ERROR MESSAGE
#-------------------------------------------------------------
error() {
  msg "${RED}${__file_name} | $(now) - ERROR: ${1-} ${NOFORMAT}"
}

#-------------------------------------------------------------
# FINISH EXECUTION
#-------------------------------------------------------------
die() {
  local msg=$1
  local code=${2-1} # default exit status 1
  msg "$msg"
  exit "$code"
}

#-------------------------------------------------------------
# PARSE SCRIPT PARAMETERS
#-------------------------------------------------------------
parse_params() {
  # default values of variables set from params
  ACTION=""
  MIGRATION_NAME=""

  while :; do
    case "${1-}" in
    -h | --help) usage ;;
    -v | --verbose) set -x ;;
    --no-color) NO_COLOR=1 ;;
    -a | --action)
      ACTION="${2-}"
      shift
      ;;
    -n | --name)
      MIGRATION_NAME="${2-}"
      shift
      ;;
    -?*) die "Unknown option: $1" ;;
    *) break ;;
    esac
    shift
  done

  args=("$@")

  # check required params and arguments
  # [[ ${#args[@]} -eq 0 ]] && die "Missing script arguments"

  if [[ "$ACTION" == "add" || "$ACTION" == "remove" ]]; then
    [[ -z "${MIGRATION_NAME-}" ]] && die "Missing required parameter: name"
  fi

  # Validate the ACTION variable
  [[ -z "${ACTION-}" ]] && die "Missing required parameter: action"
  if [[ "${ACTION-}" != "add" && "${ACTION-}" != "remove" && "${ACTION-}" != "apply" ]]; then
    die "Invalid ACTION value: ${ACTION-}"
  fi

  return 0
}

parse_params "$@"
setup_colors
main "$@"
