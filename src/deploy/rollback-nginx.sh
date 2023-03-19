nginxConfig=schedule.daystone.club
nginxConfigBackup=backup_schedule.daystone.club

# ensure running with appropriate privilages
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# check if things are where we think they are
if [ ! -f "./$nginxConfigBackup" ];
then
    echo "backup not found, abandoning rollback";
    exit 2;
fi

if [ ! -f "/etc/nginx/sites-available/$nginxConfigBackup" ];
then
    echo "destination not found, abandoning rollback";
    exit 2;
fi

# copy backup to deployment
echo "step 1/3 falling back to backup file"
sudo cp ./$nginxConfigBackup /etc/nginx/sites-available/$nginxConfig

# test nginx
echo "step 2/3 testing configuration"
sudo nginx -t

# reset nginx
echo "step 3/3 loading configuration"
sudo nginx -s reload

echo "-- rollback complete --"