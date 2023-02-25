# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# check if things are where we think they are
if [ ! -f "./.backup.schedule.daystone.club" ]; 
then
    echo "backup not found, abandoning rollback";
    exit 2;
fi

if [ ! -f "/etc/nginx/sites-available/schedule.daystone.club" ];
then
    echo "destination not found, abandoning rollback";
    exit 2;
fi

# copy .backup.schedule.daystone.club to schedule.daystone.club
echo "step 1/3 falling back to backup file"
sudo cp ./nginx_backup /etc/nginx/sites-available/schedule.daystone.club

# test nginx
echo "step 2/3 testing configuration"
sudo nginx -t

# reset nginx
echo "step 3/3 loading configuration"
sudo nginx -s reload

echo "-- rollback complete --"
exit 0;