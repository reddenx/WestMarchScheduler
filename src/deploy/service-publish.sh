# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2
fi

# check if things are where we think they are
if [ ! -f "./site_scheduler.service" ]; 
then
    echo "deployment not found, abandoning publish";
    exit 2;
fi

if [ ! -f "/lib/systemd/system/site_scheduler.service" ]; 
then
    echo "destination not found, abandoning publish";
    exit 2;
fi

# copy /lib/systemd/system/site_scheduler.service to .backup.site_scheduler.service
echo "step 1/4 backing up current version"
sudo cp /lib/systemd/system/site_scheduler.service .backup.site_scheduler.service

# copy ./site_scheduler.service to /lib/systemd/system/site_scheduler.service
echo "step 2/4 deploying new version"
sudo cp ./service.txt /lib/systemd/system/site_scheduler.service

# reload configuration file
echo "step 3/4 reload service configuration"
sudo systemctl daemon-reload

# restart service
echo "step 4/4 restarting service"
systemctl restart site_scheduler.service

echo "-- deployment complete --"
exit 0;