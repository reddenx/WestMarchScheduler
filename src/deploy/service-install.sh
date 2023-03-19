siteConfig=site_scheduler.service
siteConfigBackup=backup_site_scheduler.service

# sudo test
if [ ! "`id -u`" = 0 ]; then
    echo "Not running as root"
    exit 2
fi

# check if things are where we think they are
if [ ! -f "./$siteConfig" ];
then
    echo "deployment not found, abandoning publish";
    exit 2;
fi

# copy service config to systemd
echo "step 1/4 deploying new version"
sudo cp ./$siteConfig /lib/systemd/system/$siteConfig

# start new service
echo "step 2/4 enabling new site"
sudo systemctl enable $siteConfig

# reload configuration files
echo "step 3/4 reload service configuration"
sudo systemctl daemon-reload

# start service
echo "step 4/4 restarting service"
systemctl start $siteConfig

echo "-- deployment complete --"
