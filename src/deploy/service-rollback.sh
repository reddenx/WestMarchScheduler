# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2;
fi

# check if things are where we think they are
if [ ! -f "./.backup.site_scheduler.service" ]; 
then
    echo "backup not found, abandoning rollback";
    exit 2;
fi

if [ ! -f "/lib/systemd/system/site_scheduler.service" ]; 
then
    echo "destination not found, abandoning rollback";
    exit 2;
fi

# copy ./.backup.site_scheduler.service to /lib/systemd/system/site_scheduler.service
echo "step 1/3 deploying backup"
sudo cp ./.backup.site_scheduler.service /lib/systemd/system/site_scheduler.service

# reload configuration file
echo "step 2/3 reload service configuration"
sudo systemctl daemon-reload

# restart service
echo "step 3/3 starting service"
systemctl restart site_scheduler.service

echo "-- rollback complete --"
exit 0;